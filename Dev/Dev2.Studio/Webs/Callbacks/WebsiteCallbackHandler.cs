﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Dev2.Composition;
using Dev2.Studio.Core;
using Dev2.Studio.Core.AppResources.DependencyInjection.EqualityComparers;
using Dev2.Studio.Core.AppResources.Enums;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.Core.Messages;
using Dev2.Studio.InterfaceImplementors;
using Dev2.Studio.Utils;
using Newtonsoft.Json.Linq;

namespace Dev2.Studio.Webs.Callbacks
{
    public abstract class WebsiteCallbackHandler : IPropertyEditorWizard
    {
        protected WebsiteCallbackHandler(IEnvironmentRepository currentEnvironmentRepository)
        {
            if(currentEnvironmentRepository == null)
            {
                throw new ArgumentNullException("currentEnvironmentRepository");
            }
            CurrentEnvironmentRepository = currentEnvironmentRepository;
            ImportService.SatisfyImports(this);
        }

        #region Properties

        public Window Owner { get; set; }

        public IEnvironmentRepository CurrentEnvironmentRepository { get; private set; }

        [Import]
        public IEventAggregator EventAggregator { get; set; }

        #endregion

        protected abstract void Save(IEnvironmentModel environmentModel, dynamic jsonArgs);

        #region Navigate

        protected virtual void Navigate(IEnvironmentModel environmentModel, string uri, dynamic jsonArgs, string returnUri)
        {
        }

        #endregion

        #region ReloadResource

        protected void ReloadResource(IEnvironmentModel environmentModel, string resourceName, ResourceType resourceType)
        {
            if(EventAggregator == null || environmentModel == null || environmentModel.ResourceRepository == null)
            {
                return;
            }
            var effectedResources = environmentModel.ResourceRepository.ReloadResource(resourceName, resourceType, ResourceModelEqualityComparer.Current);
            foreach(var resource in effectedResources)
            {
                EventAggregator.Publish(new UpdateResourceMessage(resource));
            }
        }

        #endregion

        #region Implementation of IPropertyEditorWizard

        public ILayoutObjectViewModel SelectedLayoutObject
        {
            get
            {
                return null;
            }
        }

        public virtual void Save(string value, bool closeBrowserWindow = true)
        {
            Save(value, EnvironmentRepository.Instance.Source, closeBrowserWindow);
        }

        public virtual void Save(string value, IEnvironmentModel environmentModel, bool closeBrowserWindow = true)
        {
            if(closeBrowserWindow)
            {
                Close();
            }

            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            value = JSONUtils.ScrubJSON(value);

            dynamic jsonObj = JObject.Parse(value);
            Save(environmentModel, jsonObj);
        }

        public virtual void NavigateTo(string uri, string args, string returnUri)
        {
            dynamic jsonArgs = JObject.Parse(args);
            Navigate(EnvironmentRepository.Instance.Source, uri, jsonArgs, returnUri);
        }

        public virtual void OpenPropertyEditor()
        {
        }

        public virtual void Dev2Set(string data, string uri)
        {
        }

        public virtual void Dev2SetValue(string value)
        {
        }

        public virtual void Dev2Done()
        {
        }

        public virtual void Dev2ReloadResource(string resourceName, string resourceType)
        {
        }

        public virtual void Close()
        {
            if(Owner != null)
            {
                Owner.Close();
            }
        }

        public virtual void Cancel()
        {
            Close();
        }

        public string FetchData(string args)
        {
            return null;
        }

        public string GetIntellisenseResults(string searchTerm, int caretPosition)
        {
            return GetJsonIntellisenseResults(searchTerm, caretPosition);
        }

        public event NavigateRequestedEventHandler NavigateRequested;

        protected void Navigate(string uri)
        {
            if(NavigateRequested != null)
            {
                NavigateRequested(uri);
            }
        }

        #endregion

        #region GetJsonIntellisenseResults

        public static string GetJsonIntellisenseResults(string searchTerm, int caretPosition)
        {
            var provider = new DefaultIntellisenseProvider();
            var context = new IntellisenseProviderContext { InputText = searchTerm, CaretPosition = caretPosition };

            return "[" + string.Join(",", provider.GetIntellisenseResults(context).Select(r => string.Format("\"{0}\"", r.ToString()))) + "]";
        }

        #endregion



    }
}
