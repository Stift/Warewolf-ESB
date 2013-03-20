﻿using Dev2;
using Dev2.Common;
using Dev2.DataList.Contract;
using Dev2.DataList.Contract.Binary_Objects;
using Dev2.Diagnostics;
using Dev2.Enums;
using Dev2.Network.Execution;
using System;
using System.Activities;
using System.Collections.Generic;
using Dev2.Util;

namespace Unlimited.Applications.BusinessDesignStudio.Activities
{

    public class DsfActivity : DsfActivityAbstract<bool>
    {
        #region Fields
        //private string uri = "http://localhost:786/dsf/";
        private string _iconPath = string.Empty;
        string _previousParentID;
        #endregion

        #region Constructors
        public DsfActivity()
            : base()
        {
        }

        public DsfActivity(string toolboxFriendlyName, string iconPath, string serviceName, string dataTags, string resultValidationRequiredTags, string resultValidationExpression)
            : base(serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("serviceName");
            }
            ToolboxFriendlyName = toolboxFriendlyName;
            IconPath = iconPath;
            ServiceName = serviceName;
            DataTags = dataTags;
            ResultValidationRequiredTags = resultValidationRequiredTags;
            ResultValidationExpression = resultValidationExpression;
        }
        #endregion

        #region Properties



        /// <summary>
        /// Gets or sets the help link.
        /// </summary>
        /// <value>
        /// The help link.
        /// </value>
        public InArgument<string> HelpLink { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the source.
        /// </summary>
        /// <value>
        /// The friendly name of the source.
        /// </value>
        public InArgument<string> FriendlySourceName { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public InArgument<string> Type { get; set; }

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        /// <value>
        /// The action name.
        /// </value>
        public InArgument<string> ActionName { get; set; }

        /// <summary>
        /// The Name of Dynamic Service Framework Service that will be invoked
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// The Tags that are required to invoke the Dynamic Service Framework Service
        /// </summary>
        public string DataTags { get; set; }
        /// <summary>
        /// The Tags are are required to be in the result of the service invocation 
        /// in order for the result to be interpreted as valid.
        /// </summary>
        public string ResultValidationRequiredTags { get; set; }
        /// <summary>
        /// The JScript expression that must evaluate to true (boolean) in order for the
        /// result to be interpreted as valid. 
        /// </summary>
        public string ResultValidationExpression { get; set; }
        public string Category { get; set; }
        public string Tags { get; set; }
        public bool DeferExecution { get; set; }
        //2012.10.01 : massimo.guerrera - Change for the unlimited migration
        public string IconPath
        {
            get
            {
                return _iconPath;
            }

            set
            {
                _iconPath = value;
                OnPropertyChanged("IconPath");
            }
        }
        public string ToolboxFriendlyName { get; set; }
        public string AuthorRoles { get; set; }
        public string ActivityStateData { get; set; }
        public bool RemoveInputFromOutput { get; set; }


        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Overridden NativeActivity Methods

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
        }

        private bool _IsDebug = false;

        protected override void OnExecute(NativeActivityContext context)
        {
            
            context.Properties.ToObservableCollection(); /// ???? Why is this here....

            bool createResumptionPoint = false;
            IEsbChannel esbChannel = context.GetExtension<IEsbChannel>();
            IDSFDataObject dataObject = context.GetExtension<IDSFDataObject>();
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

            ErrorResultTO errors = new ErrorResultTO();
            ErrorResultTO allErrors = new ErrorResultTO();

            Guid executionID = DataListExecutionID.Get(context);
            ParentServiceName = dataObject.ServiceName;
            ParentWorkflowInstanceId = context.WorkflowInstanceId.ToString();
            try
            {
                compiler.ClearErrors(dataObject.DataListID);

                string executionServiceName = ServiceName;

                // Set Debug Mode Value
                string debugMode = compiler.EvaluateSystemEntry(executionID, enSystemTag.BDSDebugMode, out errors);
                allErrors.MergeErrors(errors);
               
                bool.TryParse(debugMode, out _IsDebug);

                if (_IsDebug)
                {
                    DispatchDebugState(context, StateType.Before);
                }

                // scrub it clean ;)
                ScrubDataList(compiler, executionID, context.WorkflowInstanceId.ToString(), out errors);
                allErrors.MergeErrors(errors);

                // set the parent service
                _previousParentID = dataObject.ParentInstanceID;
                dataObject.ParentServiceName = executionServiceName;
                dataObject.ParentInstanceID = InstanceID;
                dataObject.ParentWorkflowInstanceId = ParentWorkflowInstanceId;

                if (!DeferExecution)
                {

                    // In all cases the ShapeOutput will have merged the execution data up into the current
                    ErrorResultTO tmpErrors = new ErrorResultTO();

                    if (esbChannel == null)
                    {
                        throw new Exception("FATAL ERROR : Null ESB channel!!");
                    }
                    else
                    {
                        // PBI 7913
                        if (executionID != GlobalConstants.NullDataListID)
                        {
                            // 1) I need to build iterators to loop
                            Dev2ActivityIOIteration inputItr = new Dev2ActivityIOIteration();

                            int iterateTotal = 2;
                            // only iterate if we are not invoking from a for each ;)
                            if (!dataObject.IsDataListScoped)
                            {
                                iterateTotal = FetchMaxIterations(compiler, executionID, out tmpErrors);
                            }
                            allErrors.MergeErrors(tmpErrors);

                            // save input mapping to restore later
                            string newInputs = InputMapping;
                            int iterateIdx = 1;

                            // 2) Then I need to manip input mapping to replace (*) with ([[idx]]) and invoke ;)
                            while (iterateIdx < iterateTotal)
                            {
                                // Set proper index ;)
                                string myInputMapping = inputItr.IterateMapping(newInputs, iterateIdx);

                                // Inputs adjustment as the request
                                Guid subExeID = compiler.Shape(executionID, enDev2ArgumentType.Input, myInputMapping, out errors);
                                dataObject.DataListID = subExeID;
                                dataObject.ServiceName = ServiceName; // set up for sub-exection ;)

                                // Execute Request
                                esbChannel.ExecuteTransactionallyScopedRequest(dataObject, dataObject.WorkspaceID, out tmpErrors);
                                allErrors.MergeErrors(tmpErrors);

                                compiler.SetParentID(subExeID, executionID);

                                //  Do Output shaping
                                compiler.Shape(subExeID, enDev2ArgumentType.Output, OutputMapping, out errors);
                                allErrors.MergeErrors(errors);

                                compiler.DeleteDataListByID(subExeID); // remove sub service DL
                                iterateIdx++;
                            }

                            dataObject.DataListID = executionID; // re-set DL ID
                            dataObject.ServiceName = ServiceName;
                        }

                    }

                    bool whereErrors = compiler.HasErrors(executionID);

                    if (!whereErrors)
                    {
                        string entry = compiler.EvaluateSystemEntry(executionID, enSystemTag.FormView, out errors);
                        allErrors.MergeErrors(errors);

                        if (entry != string.Empty)
                        {
                            createResumptionPoint = true;
                            //compiler.UpsertSystemTag(executionID, enSystemTag.FormView, string.Empty, out errors);
                            allErrors.MergeErrors(errors);
                        }
                    }

                    Result.Set(context, whereErrors);
                    HasError.Set(context, whereErrors);
                    IsValid.Set(context, whereErrors);

                    if ((IsWorkflow || IsUIStep) && createResumptionPoint && !_IsDebug)
                    {
                        dataObject.ServiceName = ServiceName;
                        dataObject.ParentServiceName = ParentServiceName;
                        dataObject.ParentInstanceID = ParentInstanceID.ToString();
                        dataObject.ParentWorkflowInstanceId = ParentWorkflowInstanceId;
                        dataObject.WorkflowInstanceId = context.WorkflowInstanceId.ToString();
                        dataObject.WorkflowResumeable = true;
                        context.CreateBookmark("dsfResumption", Resumed);


                        compiler.ConditionalMerge(DataListMergeFrequency.Always | DataListMergeFrequency.OnBookmark,
                            dataObject.DatalistOutMergeID, dataObject.DataListID, dataObject.DatalistOutMergeFrequency, dataObject.DatalistOutMergeType, dataObject.DatalistOutMergeDepth);
                        ExecutionStatusCallbackDispatcher.Instance.Post(dataObject.BookmarkExecutionCallbackID, ExecutionStatusCallbackMessageType.BookmarkedCallback);

                        // Signal DataList server to persist the data ;)
                        compiler.PersistResumableDataListChain(dataObject.DataListID);

                        // INFO : In these cases resumption handles the delete and shape ;)
                    }
                }
                else
                {
                    // TODO : Build instruction list....????
                }
            }
            finally
            {
                if (!dataObject.WorkflowResumeable || !dataObject.IsDataListScoped)
                {
                    // Handle Errors
                    if (allErrors.HasErrors())
                    {
                        DisplayAndWriteError("DsfBaseActivity", allErrors);
                        compiler.UpsertSystemTag(dataObject.DataListID, enSystemTag.Error, allErrors.MakeDataListReady(), out errors);
                    }
                }
                if (_IsDebug)
                {
                    DispatchDebugState(context, StateType.After);
                }
                dataObject.ParentInstanceID = _previousParentID;
                compiler.ClearErrors(dataObject.DataListID);
            }
        }
        #endregion

        #region Private Methods

        private void ScrubDataList(IDataListCompiler compiler, Guid executionID, string workflowID, out ErrorResultTO invokeErrors)
        {
            ErrorResultTO errors = new ErrorResultTO();
            invokeErrors = new ErrorResultTO();
            // Strip System Tags
            compiler.UpsertSystemTag(executionID, enSystemTag.FormView, string.Empty, out errors);
            invokeErrors.MergeErrors(errors);

            compiler.UpsertSystemTag(executionID, enSystemTag.InstanceId, string.Empty, out errors);
            invokeErrors.MergeErrors(errors);

            compiler.UpsertSystemTag(executionID, enSystemTag.Bookmark, string.Empty, out errors);
            invokeErrors.MergeErrors(errors);

            compiler.UpsertSystemTag(executionID, enSystemTag.ParentWorkflowInstanceId, string.Empty, out errors);
            invokeErrors.MergeErrors(errors);

            compiler.UpsertSystemTag(executionID, enSystemTag.ParentServiceName, string.Empty, out errors);
            invokeErrors.MergeErrors(errors);

            compiler.UpsertSystemTag(executionID, enSystemTag.BDSDebugMode, string.Empty, out errors);
            invokeErrors.MergeErrors(errors);

            compiler.UpsertSystemTag(executionID, enSystemTag.ParentWorkflowInstanceId, workflowID, out errors);
            invokeErrors.MergeErrors(errors);
        }

        /// <summary>
        /// Fetches the max iterations.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        /// <param name="executionID">The execution ID.</param>
        /// <param name="allErrors">All errors.</param>
        /// <returns></returns>
        private int FetchMaxIterations(IDataListCompiler compiler, Guid executionID, out ErrorResultTO allErrors)
        {
            // Break the inputs apart into individual segments for use
            IDev2LanguageParser ilp = DataListFactory.CreateInputParser();
            int itTotal = 1;

            allErrors = new ErrorResultTO();
            ErrorResultTO tmpErrors;

            IList<IDev2Definition> defs = ilp.Parse(InputMapping);

            IBinaryDataList bdl = compiler.FetchBinaryDataList(executionID, out tmpErrors);
            allErrors.MergeErrors(tmpErrors);
            bool foundRS = false;

            foreach (IDev2Definition d in defs)
            {
                if (d.RawValue != null)
                {
                    string rs = DataListUtil.ExtractRecordsetNameFromValue(d.RawValue);
                    if (!string.IsNullOrEmpty(rs))
                    {
                        // find the total number of entries ;)
                        IBinaryDataListEntry entry;
                        string error;
                        if (bdl.TryGetEntry(rs, out entry, out error))
                        {
                            if (entry != null)
                            {
                                foundRS = true;
                                int tmpItrCnt = entry.FetchAppendRecordsetIndex();
                                // set max iterations ;)
                                if (tmpItrCnt > itTotal)
                                {
                                    itTotal = tmpItrCnt;
                                }
                            }
                            else
                            {
                                allErrors.AddError("Fatal Error : Null entry returned for [ " + rs + " ]");
                            }
                        }

                        allErrors.AddError(error);
                    }
                }
            }

            // force all scalars mappings to execute once ;)
            if (!foundRS && defs.Count > 0)
            {
                itTotal = 2;
            }

            return itTotal;
        }

        //private void NotifyApplicationHost(IApplicationMessage messageNotification, string instruction, string result, string transformedResult)
        //{
        //    //Notifications out from this activity for tracking purposes
        //    Notify(messageNotification, string.Format("<{0}>", this.DisplayName.Replace(" ", string.Empty)));

        //    Notify(messageNotification, string.Format("\r\n\t<{0}DSFINSTRUCTION>\r\n ", this.DisplayName.Replace(" ", string.Empty)));
        //    Notify(messageNotification, instruction);
        //    Notify(messageNotification, string.Format("\r\n\t</{0}DSFINSTRUCTION>\r\n", this.DisplayName.Replace(" ", string.Empty)));

        //    Notify(messageNotification, string.Format("\r\n\t<{0}DSFRESULT>\r\n ", this.DisplayName.Replace(" ", string.Empty)));
        //    Notify(messageNotification, string.Format("\r\n{0}\r\n", result));
        //    Notify(messageNotification, string.Format("\r\n\t</{0}DSFRESULT>\r\n", this.DisplayName.Replace(" ", string.Empty)));

        //    Notify(messageNotification, string.Format("\r\n\t<{0}DSFRESULT_TRANSFORMED>\r\n ", this.DisplayName.Replace(" ", string.Empty)));
        //    Notify(messageNotification, string.Format("\r\n{0}\r\n", transformedResult));
        //    Notify(messageNotification, string.Format("\r\n\t</{0}DSFRESULT_TRANSFORMED>\r\n", this.DisplayName.Replace(" ", string.Empty)));

        //    Notify(messageNotification, string.Format("</{0}>\r\n", this.DisplayName.Replace(" ", string.Empty)));
        //}

        #endregion

        #region Overridden ActivityAbstact Methods

        public override IBinaryDataList GetInputs()
        {
            IBinaryDataList result = null;
            ErrorResultTO errors;
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

            string inputDlString = compiler.GenerateWizardDataListFromDefs(InputMapping, enDev2ArgumentType.Input, false, out errors, true);
            string inputDlShape = compiler.GenerateWizardDataListFromDefs(InputMapping, enDev2ArgumentType.Input, false, out errors);
            if (!errors.HasErrors())
            {
                Guid dlID = compiler.ConvertTo(DataListFormat.CreateFormat(GlobalConstants._XML_Without_SystemTags), inputDlString, inputDlShape, out errors);
                if (!errors.HasErrors())
                {
                    result = compiler.FetchBinaryDataList(dlID, out errors);
                }
                else
                {
                    string errorString = string.Join(",", errors.FetchErrors());
                    throw new Exception(errorString);
                }
            }
            else
            {
                string errorString = string.Join(",", errors.FetchErrors());
                throw new Exception(errorString);
            }

            return result;
        }

        public override IBinaryDataList GetOutputs()
        {
            IBinaryDataList result = null;
            ErrorResultTO errors;
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

            string outputDlString = compiler.GenerateWizardDataListFromDefs(OutputMapping, enDev2ArgumentType.Output, false, out errors, true);
            string outputDlShape = compiler.GenerateWizardDataListFromDefs(OutputMapping, enDev2ArgumentType.Output, false, out errors);
            if (!errors.HasErrors())
            {
                Guid dlID = compiler.ConvertTo(DataListFormat.CreateFormat(GlobalConstants._XML_Without_SystemTags), outputDlString, outputDlShape, out errors);
                if (!errors.HasErrors())
                {
                    result = compiler.FetchBinaryDataList(dlID, out errors);
                }
                else
                {
                    string errorString = string.Join(",", errors.FetchErrors());
                    throw new Exception(errorString);
                }
            }
            else
            {
                string errorString = string.Join(",", errors.FetchErrors());
                throw new Exception(errorString);
            }

            return result;
        }

        public override IBinaryDataList GetWizardData()
        {
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

            ErrorResultTO errors;
            IBinaryDataList indl = GetInputs();
            IBinaryDataList outdl = GetOutputs();
            IBinaryDataList result = compiler.Merge(indl, outdl, enDataListMergeTypes.Union, enTranslationDepth.Data, true, out errors);

            return result;
        }

        #endregion Overridden ActivityAbstact Methods

        #region Debug IO
        public override IList<IDebugItem> GetDebugInputs(IBinaryDataList dataList)
        {
            IDev2LanguageParser parser = DataListFactory.CreateInputParser();
            IList<IDev2Definition> inputs = parser.Parse(InputMapping);
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

            IList<IDebugItem> results = new List<IDebugItem>();
            foreach (IDev2Definition dev2Definition in inputs)
            {
                string displayName = dev2Definition.Name;
                if (!string.IsNullOrEmpty(dev2Definition.RecordSetName))
                {
                    displayName = dev2Definition.RecordSetName + "(*)." + dev2Definition.Name;
                }
                ErrorResultTO errors = new ErrorResultTO();
                IBinaryDataListEntry tmpEntry = compiler.Evaluate(dataList.UID, enActionType.User, dev2Definition.RawValue, false, out errors);                                                                

                DebugItem itemToAdd = new DebugItem();

                itemToAdd.Add(new DebugItemResult { Type = DebugItemResultType.Label, Value = displayName });

                itemToAdd.AddRange(CreateDebugItemsFromEntry(dev2Definition.RawValue,tmpEntry,dataList.UID,enDev2ArgumentType.Input));
                results.Add(itemToAdd);
            }

            foreach (IDebugItem debugInput in results)
            {
                debugInput.FlushStringBuilder();
            }

            return results;
        }

        public override IList<IDebugItem> GetDebugOutputs(IBinaryDataList dataList)
        {
            IDev2LanguageParser parser = DataListFactory.CreateOutputParser();
            IList<IDev2Definition> inputs = parser.Parse(OutputMapping);
             IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

            IList<IDebugItem> results = new List<IDebugItem>();
            foreach (IDev2Definition dev2Definition in inputs)
            {
                ErrorResultTO errors = new ErrorResultTO();
                IBinaryDataListEntry tmpEntry = compiler.Evaluate(dataList.UID, enActionType.User, dev2Definition.RawValue, false, out errors);

                DebugItem itemToAdd = new DebugItem();

                itemToAdd.Add(new DebugItemResult { Type = DebugItemResultType.Label, Value = dev2Definition.Name });

                itemToAdd.AddRange(CreateDebugItemsFromEntry(dev2Definition.RawValue, tmpEntry, dataList.UID, enDev2ArgumentType.Input));
                results.Add(itemToAdd);
            }

            foreach (IDebugItem debugOutput in results)
            {
                debugOutput.FlushStringBuilder();
            }

            return results;
        }

        #endregion

        #region Get ForEach Input/Output Updates

        public override void UpdateForEachInputs(IList<Tuple<string, string>> updates, NativeActivityContext context)
        {
            throw new NotImplementedException();
        }

        public override void UpdateForEachOutputs(IList<Tuple<string, string>> updates, NativeActivityContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
