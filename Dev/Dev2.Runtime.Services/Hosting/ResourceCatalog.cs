﻿
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dev2.Common;
using Dev2.Common.ExtMethods;
using Dev2.Data.Enums;
using Dev2.Data.ServiceModel;
using Dev2.Data.ServiceModel.Messages;
using Dev2.DynamicServices;
using Dev2.DynamicServices.Network;
using Dev2.Network.Messaging.Messages;
using Dev2.Providers.Errors;
using Dev2.Runtime.Compiler;
using Dev2.Runtime.Compiler.CompileRules;
using Dev2.Runtime.ESB.Management;
using Dev2.Runtime.Network;
using Dev2.Runtime.Security;
using Dev2.Runtime.ServiceModel.Data;
using ServiceStack.Common.Extensions;
using ServiceStack.ServiceModel.Extensions;

namespace Dev2.Runtime.Hosting
{
    public class ResourceCatalog : IResourceCatalog
    {
        readonly ConcurrentDictionary<Guid, List<IResource>> _workspaceResources = new ConcurrentDictionary<Guid, List<IResource>>();
        readonly ConcurrentDictionary<Guid, object> _workspaceLocks = new ConcurrentDictionary<Guid, object>();
        readonly ConcurrentDictionary<string, object> _fileLocks = new ConcurrentDictionary<string, object>();
        readonly object _loadLock = new object();

        readonly ConcurrentDictionary<Guid, ManagementServiceResource> _managementServices = new ConcurrentDictionary<Guid, ManagementServiceResource>();
        readonly ConcurrentDictionary<string, List<DynamicServiceObjectBase>> _frequentlyUsedServices = new ConcurrentDictionary<string, List<DynamicServiceObjectBase>>();
        IContextManager<IStudioNetworkSession> _contextManager;

        #region Singleton Instance

        //
        // Multi-threaded implementation - see http://msdn.microsoft.com/en-us/library/ff650316.aspx
        //
        // This approach ensures that only one instance is created and only when the instance is needed. 
        // Also, the variable is declared to be volatile to ensure that assignment to the instance variable
        // completes before the instance variable can be accessed. Lastly, this approach uses a syncRoot 
        // instance to lock on, rather than locking on the type itself, to avoid deadlocks.
        //
        static volatile ResourceCatalog _instance;
        static readonly object SyncRoot = new Object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static ResourceCatalog Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(SyncRoot)
                    {
                        if(_instance == null)
                        {
                            _instance = new ResourceCatalog(EsbManagementServiceLocator.GetServices());

                            // bootstrap the compile message repo ;)
                            CompileMessageRepo.Instance.Ping();

                        }
                    }
                }

                return _instance;
            }
        }


        #endregion

        
        public static ResourceCatalog Start(IContextManager<IStudioNetworkSession> contextManager)
        {
            if(contextManager == null)
            {
                throw new ArgumentNullException("contextManager");
            }

            _instance = new ResourceCatalog(EsbManagementServiceLocator.GetServices(), contextManager);

            return _instance;
        }

        #region CTOR

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceCatalog" /> class.
        /// <remarks>
        /// DO NOT instantiate directly - use static Instance property instead; this is for testing only!
        /// </remarks>
        /// </summary>
        /// <param name="managementServices">The management services to be loaded.</param>
        /// <param name="networkOperator">The network operator.</param>
        public ResourceCatalog(IEnumerable<DynamicService> managementServices = null, IContextManager<IStudioNetworkSession> contextManager = null)
        {
            _contextManager = contextManager;

            // MUST load management services BEFORE server workspace!!
            if(managementServices != null)
            {
                foreach(var service in managementServices)
                {
                    var resource = new ManagementServiceResource(service);
                    _managementServices.TryAdd(resource.ResourceID, resource);
                }
            }

            LoadFrequentlyUsedServices().Wait();
        }

        #endregion

        #region Properties

        public int WorkspaceCount
        {
            get
            {
                return _workspaceResources.Count;
            }
        }

        #endregion

        #region GetResourceCount

        public int GetResourceCount(Guid workspaceID)
        {
            return GetResources(workspaceID).Count;
        }

        #endregion

        #region RemoveWorkspace

        public void RemoveWorkspace(Guid workspaceID)
        {
            object workspaceLock;
            lock(_loadLock)
            {
                if(!_workspaceLocks.TryRemove(workspaceID, out workspaceLock))
                {
                    workspaceLock = new object();
                }
            }
            lock(workspaceLock)
            {
                List<IResource> resources;
                _workspaceResources.TryRemove(workspaceID, out resources);
            }
        }

        #endregion

        #region GetResource

        public IResource GetResource(Guid workspaceID, string resourceName, ResourceType resourceType = ResourceType.Unknown, string version = null)
        {
            if(string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }
            var resources = GetResources(workspaceID);
            return resources.FirstOrDefault(r => string.Equals(r.ResourceName, resourceName, StringComparison.InvariantCultureIgnoreCase)
                && (resourceType == ResourceType.Unknown || r.ResourceType == resourceType));
        }

        public IResource GetResource(Guid workspaceID, Guid resourceID, Version version = null)
        {
            var resources = GetResources(workspaceID);
            var resource = resources.FirstOrDefault(r => r.ResourceID == resourceID && (version == null || r.Version == version));
            return resource;
        }

        #endregion

        #region GetResourceContents

        /// <summary>
        /// Gets the contents of the resource with the given name.
        /// </summary>
        /// <param name="workspaceID">The workspace ID to be queried.</param>
        /// <param name="resourceID">The resource ID to be queried.</param>
        /// <param name="version">The version to be queried.</param>
        /// <returns>The resource's contents or <code>string.Empty</code> if not found.</returns>
        public string GetResourceContents(Guid workspaceID, Guid resourceID, Version version = null)
        {
            var resource = GetResource(workspaceID, resourceID, version);
            return GetResourceContents(resource);
        }

        /// <summary>
        /// Gets the contents of the resource with the given name.
        /// </summary>
        /// <param name="workspaceID">The workspace ID to be queried.</param>
        /// <param name="resourceID">The resource ID to be queried.</param>
        /// <param name="version">The version to be queried.</param>
        /// <returns>The resource's contents or <code>string.Empty</code> if not found.</returns>
        public string GetResourceContents(Guid workspaceID, string resourceName)
        {
            var resource = GetResource(workspaceID, resourceName);
            return GetResourceContents(resource);
        }

        /// <summary>
        /// Gets the resource's contents.
        /// </summary>
        /// <param name="resource">The resource to be queried.</param>
        /// <returns>The resource's contents or <code>string.Empty</code> if not found.</returns>
        public string GetResourceContents(IResource resource)
        {
            string contents = string.Empty;

            if(resource == null || string.IsNullOrEmpty(resource.FilePath) || !File.Exists(resource.FilePath))
            {
                return contents;
            }

            // Open the file with the file share option of read. This will ensure that if the file is opened for write while this read operation
            // is happening the wite will fail.
            using(FileStream fs = new FileStream(resource.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using(StreamReader sr = new StreamReader(fs))
                {
                    contents = sr.ReadToEnd();
                }
            }

            return contents;
        }

        #endregion

        #region GetPayload

        /// <summary>
        /// Gets the contents of the resource with the given guids.
        /// </summary>
        /// <param name="workspaceID">The workspace ID to be queried.</param>
        /// <param name="guidCsv">The guids to be queried.</param>
        /// <param name="type">The type string: WorkflowService, Service, Source, ReservedService or *, to be queried.</param>
        /// <returns>The resource's contents or <code>string.Empty</code> if not found.</returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public string GetPayload(Guid workspaceID, string guidCsv, string type)
        {
            if(type == null)
            {
                throw new ArgumentNullException("type");
            }

            if(guidCsv == null)
            {
                guidCsv = string.Empty;
            }

            var guids = new List<Guid>();
            foreach(var guidStr in guidCsv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Guid guid;
                if(Guid.TryParse(guidStr, out guid))
                {
                    guids.Add(guid);
                }
            }
            var resourceTypes = ResourceTypeConverter.ToResourceTypes(type);

            var workspaceResources = GetResources(workspaceID);
            var resources = workspaceResources.FindAll(r => guids.Contains(r.ResourceID)
                                                            && resourceTypes.Contains(r.ResourceType));

            var result = ToPayload(resources);
            return result;
        }

        /// <summary>
        /// Gets the contents of the resources with the given source type.
        /// </summary>
        /// <param name="workspaceID">The workspace ID to be queried.</param>
        /// <param name="sourceType">The type of the source to be queried.</param>
        /// <returns>The resource's contents or <code>string.Empty</code> if not found.</returns>
        public string GetPayload(Guid workspaceID, enSourceType sourceType)
        {
            var resourceType = ResourceTypeConverter.ToResourceType(sourceType);

            var workspaceResources = GetResources(workspaceID);
            var resources = workspaceResources.FindAll(r => r.ResourceType == resourceType);
            var result = ToPayload(resources);
            return result;
        }

        /// <summary>
        /// Gets the contents of the resource with the given name and type (WorkflowService, Service, Source, ReservedService or *).
        /// </summary>
        /// <param name="workspaceID">The workspace ID to be queried.</param>
        /// <param name="resourceName">The name of the resource to be queried.</param>
        /// <param name="type">The type string: WorkflowService, Service, Source, ReservedService or *, to be queried.</param>
        /// <param name="userRoles">The user roles to be queried.</param>
        /// <param name="useContains"><code>true</code> if matching resource name's should contain the given <paramref name="resourceName"/>;
        /// <code>false</code> if resource name's must exactly match the given <paramref name="resourceName"/>.</param>
        /// <returns>The resource's contents or <code>string.Empty</code> if not found.</returns>
        /// <exception cref="System.Runtime.Serialization.InvalidDataContractException">ResourceName or Type is missing from the request</exception>
        public string GetPayload(Guid workspaceID, string resourceName, string type, string userRoles, bool useContains = true)
        {
            if(resourceName == "*")
            {
                resourceName = string.Empty;
            }
            else if(string.IsNullOrEmpty(resourceName) || string.IsNullOrEmpty(type))
            {
                ThrowExceptionIfInvalid(resourceName, type);
            }

            var resourceTypes = ResourceTypeConverter.ToResourceTypes(type);

            var workspaceResources = GetResources(workspaceID);
            var resources = useContains
                ? workspaceResources.FindAll(r => r.ResourceName.Contains(resourceName) && resourceTypes.Contains(r.ResourceType))
                : workspaceResources.FindAll(r => r.ResourceName.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase)
                                                && resourceTypes.Contains(r.ResourceType));

            var result = ToPayload(resources);
            return result;
        }

        #endregion

        #region LoadWorkspace

        public void LoadWorkspace(Guid workspaceID)
        {
            var workspaceLock = GetWorkspaceLock(workspaceID);
            lock(workspaceLock)
            {
                _workspaceResources.AddOrUpdate(workspaceID,
                    id => LoadWorkspaceImpl(workspaceID),
                    (id, resources) => LoadWorkspaceImpl(workspaceID));
            }
        }

        #endregion

        #region LoadWorkspaceImpl

        List<IResource> LoadWorkspaceImpl(Guid workspaceID)
        {
            var folders = ServiceModel.Resources.RootFolders.Values.Distinct();
            var workspacePath = EnvironmentVariables.GetWorkspacePath(workspaceID);
            var userServices = LoadWorkspaceViaBuilder(workspacePath, folders.ToArray());

            var result = userServices.Union(_managementServices.Values);

            return result.ToList();
        }

        #endregion

        #region LoadWorkspaceAsync

        // Travis.Frisinger - 02.05.2013 
        // 
        // Removed the Async operation with file stream as it would fail to use the correct stream from time to time
        // causing the integration test suite to fail. By moving the operation into a Parallel.ForEach approach this 
        // appears to have nearly the same impact with better stability.
        // ResourceCatalogBuilder now contains the refactored async logic ;)

        /// <summary>
        /// Loads the workspace via builder.
        /// </summary>
        /// <param name="workspacePath">The workspace path.</param>
        /// <param name="folders">The folders.</param>
        /// <returns></returns>
        public IList<IResource> LoadWorkspaceViaBuilder(string workspacePath, params string[] folders)
        {
            ResourceCatalogBuilder builder = new ResourceCatalogBuilder();

            builder.BuildCatalogFromWorkspace(workspacePath, folders);

            return builder.ResourceList;
        } 

        #endregion

        #region Read/WriteTextAsync

        // Use the FileStream class, which has an option that causes asynchronous I/O to occur at the operating system level.  
        // In many cases, this will avoid blocking a ThreadPool thread.  

        //public static async Task WriteTextAsync(string filePath, string text)
        //{
        //    var encodedText = Encoding.Unicode.GetBytes(text);

        //    using(var sourceStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
        //    {
        //        await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
        //    }
        //}

        //public static async Task<string> ReadTextAsync(string filePath)
        //{
        //    using(var sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
        //    {
        //        var sb = new StringBuilder();

        //        var buffer = new byte[0x1000];
        //        int numRead;
        //        while((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
        //        {
        //            var text = Encoding.Unicode.GetString(buffer, 0, numRead);
        //            sb.Append(text);
        //        }

        //        return sb.ToString();
        //    }
        //}

        #endregion

        #region CopyResource

        public bool CopyResource(string resourceName, ResourceType resourceType, Guid sourceWorkspaceID, Guid targetWorkspaceID, string userRoles = null)
        {
            var resource = GetResource(sourceWorkspaceID, resourceName);
            return CopyResource(resource, targetWorkspaceID, userRoles);
        }

        public bool CopyResource(Guid resourceID, Guid sourceWorkspaceID, Guid targetWorkspaceID, string userRoles = null)
        {
            var resource = GetResource(sourceWorkspaceID, resourceID);
            return CopyResource(resource, targetWorkspaceID, userRoles);
        }

        public bool CopyResource(IResource resource, Guid targetWorkspaceID, string userRoles = null)
        {
            if(resource != null)
            {
                var copy = new Resource(resource);
                var contents = GetResourceContents(resource);
                var saveResult = SaveImpl(targetWorkspaceID, copy, contents, userRoles);
                return saveResult.Status == ExecStatus.Success;
            }
            return false;
        }

        #endregion

        #region SaveResource

        public ResourceCatalogResult SaveResource(Guid workspaceID, string resourceXml, string userRoles = null)
        {
            if(string.IsNullOrEmpty(resourceXml))
            {
                throw new ArgumentNullException("resourceXml");
            }

            var workspaceLock = GetWorkspaceLock(workspaceID);
            lock(workspaceLock)
            {
                var xml = XElement.Parse(resourceXml);
                var resource = new Resource(xml);
                resource.UpgradeXml(xml);
                return CompileAndSave(workspaceID, resource, xml.ToString(SaveOptions.DisableFormatting), userRoles);
            }
        }

        public ResourceCatalogResult SaveResource(Guid workspaceID, IResource resource, string userRoles = null)
        {
            if(resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            var workspaceLock = GetWorkspaceLock(workspaceID);
            lock(workspaceLock)
            {
                if(resource.ResourceID == Guid.Empty)
                {
                    resource.ResourceID = Guid.NewGuid();
                }
                return CompileAndSave(workspaceID, resource, resource.ToXml().ToString(SaveOptions.DisableFormatting), userRoles);
            }
        }

        #endregion

        #region DeleteResource

        public ResourceCatalogResult DeleteResource(Guid workspaceID, string resourceName, string type, string userRoles = null)
        {
            var workspaceLock = GetWorkspaceLock(workspaceID);
            lock(workspaceLock)
            {
                if(resourceName == "*")
                {
                    return new ResourceCatalogResult
                    {
                        Status = ExecStatus.NoWildcardsAllowed,
                        Message = "<Result>Delete resources does not accept wildcards.</Result>."
                    };
                }

                ThrowExceptionIfInvalid(resourceName, type);

                var resourceTypes = ResourceTypeConverter.ToResourceTypes(type, false);

                var workspaceResources = GetResources(workspaceID);
                var resources = workspaceResources.FindAll(r =>
                    string.Equals(r.ResourceName, resourceName, StringComparison.InvariantCultureIgnoreCase)
                    && resourceTypes.Contains(r.ResourceType));

                switch(resources.Count)
                {
                    case 0:
                        return new ResourceCatalogResult
                        {
                            Status = ExecStatus.NoMatch,
                            Message = string.Format("<Result>{0} '{1}' was not found.</Result>", type, resourceName)
                        };

                    case 1:
                        var resource = resources[0];
                        if(!resource.IsUserInAuthorRoles(userRoles))
                        {
                            return new ResourceCatalogResult
                            {
                                Status = ExecStatus.AccessViolation,
                                Message = string.Format("<Error>{0} '{1}' deletion failed: Access Violation: you are attempting to delete a resource that you do not have rights to.</Error>", type, resourceName)
                            };
                        }

                        VersionControl(Path.GetDirectoryName(resource.FilePath), resource);

                        workspaceResources.Remove(resource);
                        if(File.Exists(resource.FilePath))
                        {
                            File.Delete(resource.FilePath);
                        }
                        var messages = new List<CompileMessageTO>
                        {
                            new CompileMessageTO
                            {
                                ErrorType = ErrorType.Critical,
                                MessageID = Guid.NewGuid(),
                                MessagePayload = "The resource has been deleted",
                                MessageType = CompileMessageType.ResourceDeleted,
                                ServiceID = resource.ResourceID

                            }
                        };
                        UpdateDependantResourceWithCompileMessages(workspaceID, resource, messages);
                        return new ResourceCatalogResult
                        {
                            Status = ExecStatus.Success,
                            Message = "Success"
                        };

                    default:
                        return new ResourceCatalogResult
                        {
                            Status = ExecStatus.DuplicateMatch,
                            Message = string.Format("<Result>Multiple matches found for {0} '{1}'.</Result>", type, resourceName)
                        };
                }
            }
        }

        #endregion

        #region RollbackResource

        public bool RollbackResource(Guid workspaceID, Guid resourceID, Version fromVersion, Version toVersion)
        {
            var resource = GetResource(workspaceID, resourceID, fromVersion);
            if(resource != null)
            {
                var folder = Path.GetDirectoryName(resource.FilePath);
                if(folder != null)
                {
                    var fileName = Path.Combine(folder, "VersionControl", string.Format("{0}.V{1}.xml", resource.ResourceName, toVersion.Major));
                    if(File.Exists(fileName))
                    {
                        string fileContent;
                        using(FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using(StreamReader sr = new StreamReader(fs))
                            {
                                fileContent = sr.ReadToEnd();
                            }
                        }

                        var isValid = HostSecurityProvider.Instance.VerifyXml(fileContent);
                        if(isValid)
                        {
                            lock(GetFileLock(resource.FilePath))
                            {
                                File.WriteAllText(resource.FilePath, fileContent);
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #endregion

        #region SyncTo

        public void SyncTo(string sourceWorkspacePath, string targetWorkspacePath, bool overwrite = true, bool delete = true, IList<string> filesToIgnore = null)
        {
            if(filesToIgnore == null)
            {
                filesToIgnore = new List<string>();
            }

            var directories = new List<string> { "Sources", "Services" };

            foreach(var directory in directories)
            {
                var source = new DirectoryInfo(Path.Combine(sourceWorkspacePath, directory));
                var destination = new DirectoryInfo(Path.Combine(targetWorkspacePath, directory));

                if(!source.Exists)
                {
                    continue;
                }

                if(!destination.Exists)
                {
                    destination.Create();
                }

                //
                // Get the files from the source and desitnations folders, excluding the files which are to be ignored
                //
                var sourceFiles = source.GetFiles().Where(f => !filesToIgnore.Contains(f.Name)).ToList();
                var destinationFiles = destination.GetFiles().Where(f => !filesToIgnore.Contains(f.Name)).ToList();

                //
                // Calculate the files which are to be copied from source to destination, this respects the override parameter
                //

                var filesToCopyFromSource = new List<FileInfo>();

                if(overwrite)
                {
                    filesToCopyFromSource.AddRange(sourceFiles);
                }
                else
                {
                    filesToCopyFromSource.AddRange(sourceFiles
                        // ReSharper disable SimplifyLinqExpression
                        .Where(sf => !destinationFiles.Any(df => String.Compare(df.Name, sf.Name, StringComparison.OrdinalIgnoreCase) == 0)));
                    // ReSharper restore SimplifyLinqExpression
                }

                //
                // Calculate the files which are to be deleted from the destination, this respects the delete parameter
                //
                var filesToDeleteFromDestination = new List<FileInfo>();
                if(delete)
                {
                    filesToDeleteFromDestination.AddRange(destinationFiles
                        // ReSharper disable SimplifyLinqExpression
                        .Where(sf => !sourceFiles.Any(df => String.Compare(df.Name, sf.Name, StringComparison.OrdinalIgnoreCase) == 0)));
                    // ReSharper restore SimplifyLinqExpression
                }

                //
                // Copy files from source to desination
                //
                foreach(var file in filesToCopyFromSource)
                {
                    file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
                }
            }
        }

        #endregion

        #region GetDynamicObjects

        public List<TServiceType> GetDynamicObjects<TServiceType>(Guid workspaceID, string resourceName, bool useContains = false)
            where TServiceType : DynamicServiceObjectBase
        {
            if(string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }

            List<DynamicServiceObjectBase> results;

            if(useContains)
            {
                var resources = GetResources(workspaceID);
                results = GetDynamicObjects(resources.Where(r => r.ResourceName.Contains(resourceName)));
            }
            else
            {
                var resource = GetResource(workspaceID, resourceName);
                results = resource == null ? new List<DynamicServiceObjectBase>() : GetDynamicObjects(resource);
            }
            return results.OfType<TServiceType>().ToList();
        }

        public List<DynamicServiceObjectBase> GetDynamicObjects(IResource resource)
        {
            if(resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            var result = new List<DynamicServiceObjectBase>();
            AddResourceAsDynamicServiceObject(result, resource);
            return result;
        }

        public List<DynamicServiceObjectBase> GetDynamicObjects(Guid workspaceID)
        {
            var resources = GetResources(workspaceID);
            return GetDynamicObjects(resources);
        }

        public List<DynamicServiceObjectBase> GetDynamicObjects(IEnumerable<IResource> resources)
        {
            if(resources == null)
            {
                throw new ArgumentNullException("resources");
            }

            var result = new List<DynamicServiceObjectBase>();
            foreach(var resource in resources)
            {
                AddResourceAsDynamicServiceObject(result, resource);
            }
            return result;
        }

        #endregion

        //
        // Private Methods
        //

        #region GetResources

        List<IResource> GetResources(Guid workspaceID)
        {
            var workspaceLock = GetWorkspaceLock(workspaceID);
            lock(workspaceLock)
            {
                return _workspaceResources.GetOrAdd(workspaceID, LoadWorkspaceImpl);
            }
        }

        public virtual IResource GetResource(Guid workspaceID, Guid serviceID)
        {
            var workspaceLock = GetWorkspaceLock(workspaceID);
            lock(workspaceLock)
            {
                return _workspaceResources.GetOrAdd(workspaceID, LoadWorkspaceImpl).FirstOrDefault(c => c.ResourceID == serviceID);
            }
        }

        public virtual T GetResource<T>(Guid workspaceID, Guid serviceID) where T : Resource, new()
        {
            string resourceContents = GetResourceContents(workspaceID, serviceID);
            return GetResource<T>(resourceContents);
        }

        static T GetResource<T>(string resourceContents) where T : Resource, new()
        {
            XElement resourceElement = XElement.Load(new StringReader(resourceContents), LoadOptions.None);
            object[] args = { resourceElement };
            return (T)Activator.CreateInstance(typeof(T), args);
        }

        public T GetResource<T>(Guid workspaceID, string resourceName) where T : Resource, new()
        {
            string resourceContents = GetResourceContents(workspaceID, resourceName);
            return GetResource<T>(resourceContents);
        }

        #endregion

        #region GetWorkspaceLock

        object GetWorkspaceLock(Guid workspaceID)
        {
            lock(_loadLock)
            {
                return _workspaceLocks.GetOrAdd(workspaceID, guid => new object());
            }
        }

        #endregion

        #region GetFileLock

        object GetFileLock(string file)
        {
            return _fileLocks.GetOrAdd(file, o => new object());
        }

        #endregion GetFileLock

        #region VersionControl

        void VersionControl(string directoryName, IResource resource)
        {
            var versionDirectory = String.Format("{0}\\{1}", directoryName, "VersionControl");
            if(!Directory.Exists(versionDirectory))
            {
                Directory.CreateDirectory(versionDirectory);
            }

            if(File.Exists(resource.FilePath))
            {
                var count = Directory.GetFiles(versionDirectory, String.Format("{0}*.xml", resource.ResourceName)).Count();

                File.Copy(resource.FilePath, String.Format("{0}\\{1}.V{2}.xml", versionDirectory, resource.ResourceName, (count + 1).ToString(CultureInfo.InvariantCulture)), true);
            }
        }

        #endregion

        #region CompileAndSave

        ResourceCatalogResult CompileAndSave(Guid workspaceID, IResource resource, string contents, string userRoles = null)
        {
            // Find the service before edits ;)
            var beforeService = Instance.GetDynamicObjects<DynamicService>(workspaceID, resource.ResourceName).FirstOrDefault();
            ServiceAction beforeAction = null;
            if(beforeService != null)
            {
                beforeAction = beforeService.Actions.FirstOrDefault();
            }

            var result = SaveImpl(workspaceID, resource, contents, userRoles);

            CompileTheResourceAfterSave(workspaceID, resource, contents, beforeAction);
            SavedResourceCompileMessage(workspaceID, resource, string.Format("<CompilerMessage>{0}'</CompilerMessage>", result.Message));

            return result;
        }

        #endregion

        #region SaveImpl

        ResourceCatalogResult SaveImpl(Guid workspaceID, IResource resource, string contents, string userRoles = null)
        {
            var resources = GetResources(workspaceID);
            var conflicting = resources.FirstOrDefault(r => r.ResourceType != resource.ResourceType && r.ResourceName.Equals(resource.ResourceName, StringComparison.InvariantCultureIgnoreCase));
            if(conflicting != null)
            {
                return new ResourceCatalogResult
                {
                    Status = ExecStatus.DuplicateMatch,
                    Message = string.Format("<Error>Compilation Error: There is a {0} with the same name.</Error>", conflicting.ResourceType)
                };
            }

            //TODO Reconsider when security piece comes in
            //var existing = resources.FirstOrDefault(r => r.ResourceID == resource.ResourceID && r.Version == resource.Version);
            //if (existing != null && !existing.IsUserInAuthorRoles(userRoles))
            //{
            //    return new ResourceCatalogResult
            //    {
            //        Status = ExecStatus.AccessViolation,
            //        Message = string.Format("<Error>{0} '{1}' compilation failed: Access Violation: you are attempting to overwrite a resource that you do not have rights to.</Error>", resource.ResourceType, resource.ResourceName)
            //    };
            //}

            var workspacePath = EnvironmentVariables.GetWorkspacePath(workspaceID);
            var directoryName = Path.Combine(workspacePath, ServiceModel.Resources.RootFolders[resource.ResourceType]);
            resource.FilePath = String.Format("{0}\\{1}.xml", directoryName, resource.ResourceName);

            #region Save to disk

            if(!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            VersionControl(directoryName, resource);

            if(File.Exists(resource.FilePath))
            {
                // Remove readonly attribute if it is set
                var attributes = File.GetAttributes(resource.FilePath);
                if((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(resource.FilePath, attributes ^ FileAttributes.ReadOnly);
                }
            }

            var xml = XElement.Parse(contents);
            xml = resource.UpgradeXml(xml);

            var signedXml = HostSecurityProvider.Instance.SignXml(xml.ToString(SaveOptions.DisableFormatting));

            lock(GetFileLock(resource.FilePath))
            {
                File.WriteAllText(resource.FilePath, signedXml, Encoding.UTF8);
            }

            #endregion

            #region Add to catalog

            var index = resources.IndexOf(resource);
            var updated = false;
            if(index != -1)
            {
                resources.RemoveAt(index);
                updated = true;
            }
            resources.Add(resource);

            #endregion

            return new ResourceCatalogResult
            {
                Status = ExecStatus.Success,
                Message = string.Format("{0} {1} '{2}'", (updated ? "Updated" : "Added"), resource.ResourceType, resource.ResourceName)
            };
        }

        void SavedResourceCompileMessage(Guid workspaceID, IResource resource, string saveMessage)
        {
            var savedResourceCompileMessage = new List<CompileMessageTO>
            {
                new CompileMessageTO
                {
                    ErrorType = ErrorType.None,
                    MessageID = Guid.NewGuid(),
                    MessagePayload = saveMessage,                    
                    ServiceID = resource.ResourceID,
                    ServiceName = resource.ResourceName,
                    MessageType = CompileMessageType.ResourceSaved,
                    WorkspaceID = workspaceID,
                }
            };
            CompileMessageRepo.Instance.AddMessage(workspaceID, savedResourceCompileMessage);
        }

        public void CompileTheResourceAfterSave(Guid workspaceID, IResource resource, string contents, ServiceAction beforeAction)
            {
            if(beforeAction != null)
            {
                // Compile the service 
                ServiceModelCompiler smc = new ServiceModelCompiler();

                var messages = GetCompileMessages(resource, contents, beforeAction, smc);
                if(messages != null)
                {
                    UpdateDependantResourceWithCompileMessages(workspaceID, resource, messages);
                }
            }
        }

        static IList<CompileMessageTO> GetCompileMessages(IResource resource, string contents, ServiceAction beforeAction, ServiceModelCompiler smc)
        {
            List<CompileMessageTO> messages = new List<CompileMessageTO>();
            switch(beforeAction.ActionType)
            {
                case enActionType.InvokeStoredProc:
                    messages.AddRange(smc.Compile(resource.ResourceID, ServerCompileMessageType.DbMappingChangeRule, beforeAction.Parent.XmlString, contents));
                    messages.AddRange(smc.Compile(resource.ResourceID, ServerCompileMessageType.DbIsRequireChangeRule, beforeAction.Parent.XmlString, contents));
                    break;
                case enActionType.InvokeWebService:
                    messages.AddRange(smc.Compile(resource.ResourceID, ServerCompileMessageType.WebServiceMappingChangeRule, beforeAction.Parent.XmlString, contents));
                    messages.AddRange(smc.Compile(resource.ResourceID, ServerCompileMessageType.WebServiceIsRequiredChangeRule, beforeAction.Parent.XmlString, contents));
                    break;
                case enActionType.Plugin:
                    messages.AddRange(smc.Compile(resource.ResourceID, ServerCompileMessageType.PluginMappingChangeRule, beforeAction.Parent.XmlString, contents));
                    messages.AddRange(smc.Compile(resource.ResourceID, ServerCompileMessageType.PluginIsRequiredChangeRule, beforeAction.Parent.XmlString, contents));
                    break;
                case enActionType.Workflow:
                    messages.AddRange(smc.Compile(resource.ResourceID, ServerCompileMessageType.WorkflowMappingChangeRule, beforeAction.Parent.XmlString, contents));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return messages;
        }

        void UpdateDependantResourceWithCompileMessages(Guid workspaceID, IResource resource, IList<CompileMessageTO> messages)
        {
            var dependants = Instance.GetDependantsAsResourceForTrees(workspaceID, resource.ResourceName);
            foreach(var dependant in dependants)
            {
                var affectedResource = GetResource(workspaceID, dependant.ResourceName);
                foreach(var compileMessageTO in messages)
                {
                    compileMessageTO.WorkspaceID = workspaceID;
                    compileMessageTO.ServiceName = affectedResource.ResourceName;
                    compileMessageTO.ServiceID = affectedResource.ResourceID;
                    compileMessageTO.UniqueID = dependant.UniqueID;
                }
                UpdateResourceXML(workspaceID, affectedResource, messages);
                    CompileMessageRepo.Instance.AddMessage(workspaceID, messages);
                }
            }

        void UpdateResourceXML(Guid workspaceID, IResource effectedResource, IList<CompileMessageTO> compileMessagesTO)
        {
            var resourceContents = GetResourceContents(workspaceID, effectedResource.ResourceID);
            UpdateXMLToDisk(effectedResource, compileMessagesTO, resourceContents);
            var serverResource = GetResource(Guid.Empty, effectedResource.ResourceName);
            if(serverResource != null)
            {
                resourceContents = GetResourceContents(Guid.Empty, serverResource.ResourceID);
                UpdateXMLToDisk(serverResource, compileMessagesTO, resourceContents);
            }
        }

        void UpdateXMLToDisk(IResource resource, IList<CompileMessageTO> compileMessagesTO, string resourceContents)
        {
            var resourceElement = XElement.Load(new StringReader(resourceContents));
            if(compileMessagesTO.Count > 0)
            {
                SetErrors(resourceElement, compileMessagesTO);
                UpdateIsValid(resourceElement);
            }
            else
            {
                UpdateIsValid(resourceElement);
            }
            var contents = resourceElement.ToString(SaveOptions.DisableFormatting);
            File.WriteAllText(resource.FilePath, contents, Encoding.UTF8);
        }

        void SetErrors(XElement resourceElement, IList<CompileMessageTO> compileMessagesTO)
        {
            if(compileMessagesTO == null || compileMessagesTO.Count == 0)
            {
                return;
            }
            var errorMessagesElement = GetErrorMessagesElement(resourceElement);
            if(errorMessagesElement == null)
            {
                errorMessagesElement = new XElement("ErrorMessages");
                resourceElement.Add(errorMessagesElement);
            }
            else
            {
                //TODO: How do we determine what really changed e.g. remove 1 of 2 err msgs
                compileMessagesTO.ForEach(to =>
                {
                    IEnumerable<XElement> xElements = errorMessagesElement.Elements("ErrorMessage");
                    XElement firstOrDefault = xElements.FirstOrDefault(element =>
                    {
                        XAttribute xAttribute = element.Attribute("InstanceID");
                        if(xAttribute != null)
                        {
                            return xAttribute.Value == to.UniqueID.ToString();
            }
                        return false;
                    });
                    if(firstOrDefault != null)
                    {
                        firstOrDefault.Remove();
                    }
                });

            }

            foreach(var compileMessageTO in compileMessagesTO)
            {
                //var errorMessageElement = errorMessagesElement.Descendants().FirstOrDefault(
                //    x => x.AttributeSafe("InstanceID") == compileMessageTO.UniqueID.ToString());

                //if(errorMessageElement != null)
                //{
                //    errorMessageElement.Remove();
                //}
                var errorMessageElement = new XElement("ErrorMessage");
                errorMessagesElement.Add(errorMessageElement);
                errorMessageElement.Add(new XAttribute("InstanceID", compileMessageTO.UniqueID));
                errorMessageElement.Add(new XAttribute("Message", compileMessageTO.MessageType.GetDescription()));
                errorMessageElement.Add(new XAttribute("ErrorType", compileMessageTO.ErrorType));
                errorMessageElement.Add(new XAttribute("MessageType", compileMessageTO.MessageType));
                errorMessageElement.Add(new XAttribute("FixType", compileMessageTO.ToFixType()));
                errorMessageElement.Add(new XAttribute("StackTrace", ""));
                errorMessageElement.Add(new XCData(compileMessageTO.MessagePayload));
            }
        }

        static XElement GetErrorMessagesElement(XElement resourceElement)
        {
            var errorMessagesElement = resourceElement.Element("ErrorMessages");
            return errorMessagesElement;
        }

        static void UpdateIsValid(XElement resourceElement)
        {
            bool isValid = false;
            var isValidAttrib = resourceElement.Attribute("IsValid");
            var errorMessagesElement = resourceElement.Element("ErrorMessages");
            if(errorMessagesElement == null || !errorMessagesElement.HasElements)
            {
                isValid = true;
            }
            if(isValidAttrib == null)
            {
                resourceElement.Add(new XAttribute("IsValid", isValid));
            }
            else
            {
                isValidAttrib.SetValue(isValid);
            }
        }

        public void FireUpdateMessage(Guid id)
        {
            var updateWorkflowFromServerMessage = new UpdateWorkflowFromServerMessage();
            updateWorkflowFromServerMessage.ResourceID = id;
            foreach(var attachedContext in _contextManager.CurrentContexts)
            {
                StudioMessaging.MessageBroker.Send(updateWorkflowFromServerMessage, attachedContext);
            }
        }

        #endregion

        #region ToPayload

        string ToPayload(IEnumerable<IResource> resources)
        {
            var result = new StringBuilder();
            foreach(var resource in resources)
            {
                if(resource.ResourceType == ResourceType.ReservedService)
                {
                    result.AppendFormat("<ReservedName>{0}</ReservedName>", resource.ResourceName);
                }
                else
                {
                    var contents = GetResourceContents(resource);
                    if(!string.IsNullOrEmpty(contents))
                    {
                        contents = contents.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Trim();
                        result.Append(contents);
                    }
                }
            }
            return string.Format("<Payload>{0}</Payload>", result);
        }

        #endregion

        #region ThrowExceptionIfInvalid

        static void ThrowExceptionIfInvalid(string resourceName, string type)
        {
            if(string.IsNullOrEmpty(resourceName) || string.IsNullOrEmpty(type))
            {
                throw new InvalidDataContractException("ResourceName or Type is missing from the request");
            }
        }

        #endregion

        #region AddResourceAsDynamicServiceObject

        void AddResourceAsDynamicServiceObject(List<DynamicServiceObjectBase> result, IResource resource)
        {
            if(resource.ResourceType == ResourceType.ReservedService)
            {
                var managementResource = resource as ManagementServiceResource;
                if(managementResource != null)
                {
                    result.Add(managementResource.Service);
                }
            }
            else
            {
                List<DynamicServiceObjectBase> objects;
                if(!_frequentlyUsedServices.TryGetValue(resource.ResourceName, out objects))
                {
                    ServerLogger.LogError(string.Format("{0} -> Cache MISS", resource.ResourceName));
                    objects = GenerateObjectGraph(resource);
                }
                else
                {
                    ServerLogger.LogError(string.Format("{0} -> Cache HIT", resource.ResourceName));
                }
                if(objects != null)
                {
                    result.AddRange(objects);
                }
            }
        }

        List<DynamicServiceObjectBase> GenerateObjectGraph(IResource resource)
        {
            var xml = GetResourceContents(resource);
            return !string.IsNullOrEmpty(xml) ? DynamicObjectHelper.GenerateObjectGraphFromString(xml) : null;
        }

        #endregion

        public async Task LoadFrequentlyUsedServices()
        {
            #region ServiceNames

            var serviceNames = new[]
            {
                "Button",
                "CssClassInject",
                "Dev2ServiceDetails",
                "Drop Down List",
                "FindResourceService",
                "FindResourcesByID",
                "FindResourcesService",
                "HelpRegion",
                "HtmlWidget",
                "InjectLabel_New",
                "InjectRequiredTracking",
                "Label",
                "SetReadOnly",
                "StyleInject",
                "System",
                "TabIndexInject",
                "TabIndexInjected",
                "Textbox",
                "TooltipInject"
            };

            #endregion

            foreach(var serviceName in serviceNames)
            {
                var resourceName = serviceName;
                var theTask = new Task(() =>
                {
                    var resource = GetResource(GlobalConstants.ServerWorkspaceID, resourceName);
                    var objects = GenerateObjectGraph(resource);
                    _frequentlyUsedServices.TryAdd(resourceName, objects);
                });
                theTask.Start();
                await theTask;
            }
        }

        public List<string> GetDependants(Guid workspaceID, string resourceName)
        {
            // ReSharper disable LocalizableElement
            if(string.IsNullOrEmpty(resourceName)) throw new ArgumentNullException("resourceName", "No resource name given.");
            // ReSharper restore LocalizableElement

            var resources = GetResources(workspaceID);
            var dependants = new List<string>();
            resources.ForEach(resource =>
            {
                if(resource.Dependencies == null) return;
                resource.Dependencies.ForEach(tree =>
                {
                    if(tree.ResourceName == resourceName)
                    {
                        dependants.Add(resource.ResourceName);
                    }
                });
            });
            return dependants.ToList();
        }

        public ResourceCatalogResult RenameResource(Guid workspaceID, string resourceID, string newName)
        {
            if (resourceID == null)
            {
                throw new ArgumentNullException("resourceID", "No value provided for resourceID");
            }
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentNullException("newName", "No value provided for newName");
            }
            var resourcesToUpdate = Instance.GetResources(workspaceID, resource => resource.ResourceID == Guid.Parse(resourceID)).ToArray();
            try
            {
                if (resourcesToUpdate.Any())
                {
                    if(UpdateResourceName(workspaceID, resourcesToUpdate[0], newName).Status == ExecStatus.Success)
                    {
                        return new ResourceCatalogResult
                        {
                            Status = ExecStatus.Success,
                            Message = string.Format("<CompilerMessage>{0} '{1}' to '{2}'</CompilerMessage>", "Renamed Resource", resourceID, newName)
                        };
                    };
                }
            }
            catch (Exception)
            {
                return new ResourceCatalogResult
                {
                    Status = ExecStatus.Fail,
                    Message = string.Format("<CompilerMessage>{0} '{1}' to '{2}'</CompilerMessage>", "Failed to Rename Resource", resourceID, newName)
                };
            }
            return new ResourceCatalogResult
            {
                Status = ExecStatus.Fail,
                Message = string.Format("<CompilerMessage>{0} '{1}' to '{2}'</CompilerMessage>", "Failed to Rename Resource", resourceID, newName)
            };
        }

        ResourceCatalogResult UpdateResourceName(Guid workspaceID, IResource resource, string newName)
        {
            //get old resource
            var resourceContents = GetResourceContents(workspaceID, resource.ResourceID);
            var resourceElement = XElement.Load(new StringReader(resourceContents), LoadOptions.None);
            var nameAttrib = resourceElement.Attribute("Name");
            string oldName = null;
            if (nameAttrib == null)
            {
                resourceElement.Add(new XAttribute("Name", newName));
            }
            else
            {
                oldName = nameAttrib.Value;
                nameAttrib.SetValue(newName);
            }
            var actionElement = resourceElement.Element("Action");
            if(actionElement != null)
            {
                var xaml = actionElement.Element("XamlDefinition");
                if(xaml != null)
                {
                    xaml.SetValue(xaml.Value
                        .Replace("x:Class=\"" + oldName, "x:Class=\"" + newName)
                        .Replace("ToolboxFriendlyName=\"" + oldName, "ToolboxFriendlyName=\"" + newName)
                        .Replace("DisplayName=\"" + oldName, "DisplayName=\"" + newName));
                }
            }
            //delete old resource
            if(File.Exists(resource.FilePath))
            {
                lock (GetFileLock(resource.FilePath))
                {
                    File.Delete(resource.FilePath);
                }
            }
            //rename resource
            resource.ResourceName = newName;
            resource.FilePath = resource.FilePath.Replace(oldName, newName);
            //re-create and resign
            return SaveImpl(workspaceID, resource, resourceElement.ToString(SaveOptions.DisableFormatting));
        }

        public ResourceCatalogResult RenameCategory(Guid workspaceID, string oldCategory, string newCategory, string resourceTypeStr)
        {
            if (oldCategory == null)
            {
                throw new ArgumentNullException("oldCategory", "No value provided for oldCategory");
            }
            if (string.IsNullOrEmpty(newCategory))
            {
                throw new ArgumentNullException("newCategory", "No value provided for oldCategory");
            }
            ResourceType resourceType;
            Enum.TryParse(resourceTypeStr, out resourceType);
            var resourcesToUpdate = Instance.GetResources(workspaceID, resource => resource.ResourcePath.Equals(oldCategory,StringComparison.OrdinalIgnoreCase) && resource.ResourceType == resourceType);
            try
            {
                foreach(var resource in resourcesToUpdate)
                {
                    UpdateResourceCategory(workspaceID, resource, newCategory);
                }
                return new ResourceCatalogResult
                {
                    Status = ExecStatus.Success,
                    Message = string.Format("<CompilerMessage>{0} from '{1}' to '{2}'</CompilerMessage>", "Updated Category", oldCategory, newCategory)
                };
            }
            catch (Exception)
            {
                return new ResourceCatalogResult
                {
                    Status = ExecStatus.Fail,
                    Message = string.Format("<CompilerMessage>{0} from '{1}' to '{2}'</CompilerMessage>", "Failed to Category", oldCategory, newCategory)
                };
            }
        }

        void UpdateResourceCategory(Guid workspaceID, IResource resource, string newCategory)
        {
            
            string resourceContents = GetResourceContents(workspaceID, resource.ResourceID);
            XElement resourceElement = XElement.Load(new StringReader(resourceContents), LoadOptions.None);
            XElement categoryElement = resourceElement.Element("Category");
            if (categoryElement == null)
            {
                resourceElement.Add(new XElement("Category", newCategory));
            }
            else
            {
                categoryElement.SetValue(newCategory);
            }
            lock(GetFileLock(resource.FilePath))
            {
                File.WriteAllText(resource.FilePath, resourceElement.ToString(SaveOptions.DisableFormatting), Encoding.UTF8);
            }
            resource.ResourcePath = newCategory;
        }

        IEnumerable<IResource> GetResources(Guid workspaceID, Func<IResource, bool> filterResources)
        {
            return GetResources(workspaceID).Where(filterResources);
        }

        public List<ResourceForTree> GetDependantsAsResourceForTrees(Guid workspaceID, string resourceName)
        {
            // ReSharper disable LocalizableElement
            if(string.IsNullOrEmpty(resourceName)) throw new ArgumentNullException("resourceName", "No resource name given.");
            // ReSharper restore LocalizableElement

            var resources = GetResources(workspaceID);
            var dependants = new List<ResourceForTree>();
            resources.ForEach(resource =>
            {
                if(resource.Dependencies == null) return;
                resource.Dependencies.ForEach(tree =>
                {
                    if(tree.ResourceName == resourceName)
                    {
                        dependants.Add(CreateResourceForTree(resource, tree));
                    }
                });
            });
            return dependants.ToList();
        }

        static ResourceForTree CreateResourceForTree(IResource resource, ResourceForTree tree)
        {
            return new ResourceForTree
            {
                UniqueID = tree.UniqueID,
                ResourceID = resource.ResourceID,
                ResourceName = resource.ResourceName,
                ResourceType = resource.ResourceType
            };
        }
    }
}