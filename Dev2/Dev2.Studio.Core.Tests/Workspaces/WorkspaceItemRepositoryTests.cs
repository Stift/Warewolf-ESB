﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using Dev2.Composition;
using Dev2.Studio.Core.AppResources.Enums;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.Core.Workspaces;
using Dev2.Workspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Unlimited.Framework;

namespace Dev2.Core.Tests.Workspaces
{
    // BUG 9492 - 2013.06.08 - TWR : added
    [TestClass]
    public class WorkspaceItemRepositoryTests
    {
        #region Static Class Init

        static string _testDir;

        [ClassInitialize]
        public static void MyClassInit(TestContext context)
        {
            _testDir = context.DeploymentDirectory;
        }

        #endregion

        #region WorkspaceItems

        [TestMethod]
        public void WorkspaceItemRepositoryWorkspaceItemsExpectedInvokesReadFirstTime()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repositoryPath = GetUniqueRepositoryPath();

            // Create repository file with one item in it
            var repository = new WorkspaceItemRepository(repositoryPath);
            repository.AddWorkspaceItem(model.Object);

            // Now create a new repository from the previous file
            repository = new WorkspaceItemRepository(repositoryPath);

            // Access items for the first time
            var items = repository.WorkspaceItems;

            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(workspaceID, items[0].WorkspaceID);
            Assert.AreEqual(serverID, items[0].ServerID);
            Assert.AreEqual(resourceName, items[0].ServiceName);
            Assert.AreEqual(WorkspaceItem.ServiceServiceType, items[0].ServiceType);
        }

        #endregion

        #region AddWorkspaceItem

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WorkspaceItemRepositoryAddWorkspaceItemWithNullModelExpectedThrowsArgumentNullException()
        {
            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.AddWorkspaceItem(null);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryAddWorkspaceItemWithExistingModelExpectedDoesNothing()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.AddWorkspaceItem(model.Object);

            Assert.AreEqual(1, repository.WorkspaceItems.Count);

            repository.AddWorkspaceItem(model.Object);
            Assert.AreEqual(1, repository.WorkspaceItems.Count);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryAddWorkspaceItemWithNewModelExpectedAddsAndAssignsWorkspaceID()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.AddWorkspaceItem(model.Object);

            Assert.AreEqual(1, repository.WorkspaceItems.Count);
            Assert.AreEqual(workspaceID, repository.WorkspaceItems[0].WorkspaceID);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryAddWorkspaceItemWithNewModelExpectedAddsAndAssignsServerID()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.AddWorkspaceItem(model.Object);

            Assert.AreEqual(1, repository.WorkspaceItems.Count);
            Assert.AreEqual(serverID, repository.WorkspaceItems[0].ServerID);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryAddWorkspaceItemWithNewModelExpectedAddsAndAssignsServiceName()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.AddWorkspaceItem(model.Object);

            Assert.AreEqual(1, repository.WorkspaceItems.Count);
            Assert.AreEqual(resourceName, repository.WorkspaceItems[0].ServiceName);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryAddWorkspaceItemWithNewServiceModelExpectedAddsAndAssignsServiceServiceType()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.AddWorkspaceItem(model.Object);

            Assert.AreEqual(1, repository.WorkspaceItems.Count);
            Assert.AreEqual(WorkspaceItem.ServiceServiceType, repository.WorkspaceItems[0].ServiceType);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryAddWorkspaceItemWithNewSourceModelExpectedAddsAndAssignsSourceServiceType()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Source, out resourceName, out workspaceID, out serverID);

            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.AddWorkspaceItem(model.Object);

            Assert.AreEqual(1, repository.WorkspaceItems.Count);
            Assert.AreEqual(WorkspaceItem.SourceServiceType, repository.WorkspaceItems[0].ServiceType);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryAddWorkspaceItemWithNewModelExpectedInvokesWrite()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repositoryPath = GetUniqueRepositoryPath();
            Assert.IsFalse(File.Exists(repositoryPath));

            var repository = new WorkspaceItemRepository(repositoryPath);
            repository.AddWorkspaceItem(model.Object);
            Assert.IsTrue(File.Exists(repositoryPath));
        }

        //Added by Massimo.Guerrera this will ensure that when saving a remote workflow that it will save to the right workspace
        [TestMethod]
        public void WorkspaceItemRepositoryAddWorkspaceItemWithNewModelWithSameNameExpectedInvokesWrite()
        {
            string resourceName = "test"+Guid.NewGuid();
            Guid workspaceID = Guid.NewGuid();
            Guid serverID = Guid.NewGuid();
            Guid envID = Guid.NewGuid();
            var model1 = CreateModel(ResourceType.Service, resourceName, workspaceID, serverID, envID);
            workspaceID = Guid.NewGuid();
            serverID = Guid.NewGuid();
            envID = Guid.NewGuid();
            var model2 = CreateModel(ResourceType.Service, resourceName, workspaceID, serverID, envID);

            var repositoryPath = GetUniqueRepositoryPath();
            Assert.IsFalse(File.Exists(repositoryPath));

            var repository = new WorkspaceItemRepository(repositoryPath);
            repository.AddWorkspaceItem(model1.Object);
            repository.AddWorkspaceItem(model2.Object);
            Assert.IsTrue(repository.WorkspaceItems.Count == 2);
            Assert.IsTrue(File.Exists(repositoryPath));
        }

        #endregion

        #region UpdateWorkspaceItem

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WorkspaceItemRepositoryUpdateWorkspaceItemWithNullModelExpectedThrowsArgumentNullException()
        {
            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.UpdateWorkspaceItem(null, false);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryUpdateWorkspaceItemWithNonExistingModelExpectedDoesNothing()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());

            var result = repository.UpdateWorkspaceItem(model.Object, true);
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void WorkspaceItemRepositoryUpdateWorkspaceItemWithExistingModelExpectedInvokesExecuteCommand()
        {
            const string ExpectedResult = "Workspace item updated";
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var context = new Mock<IStudioClientContext>();
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID, context);

            #region Setup ImportService - GRRR!

            var importServiceContext = new ImportServiceContext();
            ImportService.CurrentContext = importServiceContext;
            ImportService.Initialize(new List<ComposablePartCatalog>
            {
                new FullTestAggregateCatalog()
            });
            var securityContext = new Mock<IFrameworkSecurityContext>();
            ImportService.AddExportedValueToContainer(securityContext.Object);

            #endregion

            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.AddWorkspaceItem(model.Object);

            #region Get request xml

            var workspaceItem = repository.WorkspaceItems[0];
            workspaceItem.Action = WorkspaceItemAction.Commit;
            dynamic requestObj = new UnlimitedObject();
            requestObj.Service = "UpdateWorkspaceItemService";
            requestObj.Roles = "";
            requestObj.ItemXml = workspaceItem.ToXml();
            requestObj.IsLocalSave = true;
            string requestXml = requestObj.XmlString;

            #endregion

            context.Setup(c => c.ExecuteCommand(It.Is<string>(s => s.ToLower() == requestXml.ToLower()), It.Is<Guid>(id => id == workspaceID), It.IsAny<Guid>())).Returns(ExpectedResult).Verifiable();

            var result = repository.UpdateWorkspaceItem(model.Object, true);
            context.Verify(c => c.ExecuteCommand(It.Is<string>(s => s.ToLower() == requestXml.ToLower()), It.Is<Guid>(id => id == workspaceID), It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(ExpectedResult, result);
        }

        #endregion

        #region Remove

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WorkspaceItemRepositoryRemoveWithNullModelExpectedThrowsArgumentNullException()
        {
            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.Remove(null);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryRemoveWithNonExistingModelExpectedDoesNothing()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.AddWorkspaceItem(model.Object);
            Assert.AreEqual(1, repository.WorkspaceItems.Count);

            model.Setup(m => m.ResourceName).Returns("Test_" + Guid.NewGuid());

            repository.Remove(model.Object);
            Assert.AreEqual(1, repository.WorkspaceItems.Count);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryRemoveWithExistingModelExpectedRemovesItem()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repository = new WorkspaceItemRepository(GetUniqueRepositoryPath());
            repository.AddWorkspaceItem(model.Object);
            Assert.AreEqual(1, repository.WorkspaceItems.Count);

            repository.Remove(model.Object);
            Assert.AreEqual(0, repository.WorkspaceItems.Count);
        }

        [TestMethod]
        public void WorkspaceItemRepositoryRemoveWithExistingModelExpectedInvokesWrite()
        {
            string resourceName;
            Guid workspaceID;
            Guid serverID;
            var model = CreateModel(ResourceType.Service, out resourceName, out workspaceID, out serverID);

            var repositoryPath = GetUniqueRepositoryPath();
            Assert.IsFalse(File.Exists(repositoryPath));

            var repository = new WorkspaceItemRepository(repositoryPath);
            repository.AddWorkspaceItem(model.Object);
            if(File.Exists(repositoryPath))
            {
                File.Delete(repositoryPath);
            }
            repository.Remove(model.Object);
            Assert.IsTrue(File.Exists(repositoryPath));
        }


        #endregion

        #region CreateModel

        static Mock<IContextualResourceModel> CreateModel(ResourceType resourceType, out string resourceName, out Guid workspaceID, out Guid serverID, Mock<IStudioClientContext> context = null)
        {
            resourceName = "Test_" + Guid.NewGuid();
            workspaceID = Guid.NewGuid();
            serverID = Guid.NewGuid();

            if(context == null)
            {
                context = new Mock<IStudioClientContext>();
            }
            context.Setup(c => c.WorkspaceID).Returns(workspaceID);
            context.Setup(c => c.ServerID).Returns(serverID);

            var env = new Mock<IEnvironmentModel>();
            env.Setup(e => e.DsfChannel).Returns(context.Object);

            var model = new Mock<IContextualResourceModel>();
            model.Setup(m => m.Environment).Returns(env.Object);
            model.Setup(m => m.ResourceName).Returns(resourceName);
            model.Setup(m => m.ResourceType).Returns(resourceType);

            return model;
        }

        static Mock<IContextualResourceModel> CreateModel(ResourceType resourceType,string resourceName,Guid workspaceID, Guid serverID,Guid envId, Mock<IStudioClientContext> context = null)
        {                      
            if (context == null)
            {
                context = new Mock<IStudioClientContext>();
            }
            context.Setup(c => c.WorkspaceID).Returns(workspaceID);
            context.Setup(c => c.ServerID).Returns(serverID);

            var env = new Mock<IEnvironmentModel>();
            env.Setup(e => e.DsfChannel).Returns(context.Object);
            env.Setup(e => e.ID).Returns(envId);

            var model = new Mock<IContextualResourceModel>();
            model.Setup(m => m.Environment).Returns(env.Object);
            model.Setup(m => m.ResourceName).Returns(resourceName);
            model.Setup(m => m.ResourceType).Returns(resourceType);

            return model;
        }

        #endregion

        #region GetUniqueRepositoryPath

        static string GetUniqueRepositoryPath()
        {
            return Path.Combine(_testDir, string.Format("WorkspaceItems{0}.xml", Guid.NewGuid()));
        }

        #endregion


    }
}
