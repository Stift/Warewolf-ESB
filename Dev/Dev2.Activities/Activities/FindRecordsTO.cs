﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dev2.Interfaces;
using Dev2.Providers.Errors;
using Dev2.Providers.Validation;
using Dev2.Providers.Validation.Rules;
using Dev2.Util;

namespace Unlimited.Applications.BusinessDesignStudio.Activities
{
    public class FindRecordsTO : IDev2TOFn, IPerformsValidation
    {
        int _indexNum;
        string _searchType;
        bool _isSearchCriteriaEnabled;
        [NonSerialized]
        readonly IList<string> _requiresSearchCriteria = new List<string> { "Not Contains", "Contains", "Equal", "Not Equal", "Ends With", "Starts With", "Regex", ">", "<", "<=", ">=" };

        string _searchCriteria;
        Dictionary<string, List<IActionableErrorInfo>> _errors = new Dictionary<string, List<IActionableErrorInfo>>();

        public FindRecordsTO()
            : this("Match On", "Equal", 0)
        {
        }

        // TODO: Remove WhereOptionList property - DO NOT USE FOR BINDING, USE VIEWMODEL PROPERTY INSTEAD!
        public IList<string> WhereOptionList { get; set; }
        public FindRecordsTO(string searchCriteria, string searchType, int indexNum, bool include = false, bool inserted = false)
        {
            Inserted = inserted;
            SearchCriteria = searchCriteria;
            SearchType = searchType;
            IndexNumber = indexNum;
            IsSearchCriteriaEnabled = false;
        }

        [FindMissing]
        public string SearchCriteria
        {
            get
            {
                return _searchCriteria;
            }
            set
            {
                _searchCriteria = value;
                OnPropertyChanged("SearchCriteria");
            }
        }

        public string SearchType
        {
            get
            {
                return _searchType;
            }
            set
            {
                _searchType = value;
                OnPropertyChanged("SearchType");
                UpdateIsCriteriaEnabled();
            }
        }

        void UpdateIsCriteriaEnabled()
        {
            if(_requiresSearchCriteria.Contains(SearchType))
            {
                IsSearchCriteriaEnabled = true;
            }
            else
            {
                IsSearchCriteriaEnabled = false;
                SearchCriteria = string.Empty;
            }
        }

        public bool IsSearchCriteriaEnabled
        {
            get
            {
                return _isSearchCriteriaEnabled;
            }
            set
            {
                _isSearchCriteriaEnabled = value;
                OnPropertyChanged("IsSearchCriteriaEnabled");
            }
        }

        #region Implementation of INotifyPropertyChanged

        public int IndexNumber
        {
            get
            {
                return _indexNum;
            }
            set
            {
                _indexNum = value;
                OnPropertyChanged("IndexNumber");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool CanRemove()
        {
            if(string.IsNullOrEmpty(SearchCriteria) && string.IsNullOrEmpty(SearchType))
            {
                return true;
            }
            return false;
        }

        public bool CanAdd()
        {
            var result = !string.IsNullOrEmpty(SearchType);
            return result;
        }

        public void ClearRow()
        {
            SearchCriteria = string.Empty;
            SearchType = "";
        }

        public bool Inserted { get; set; }

        #endregion

        #region Implementation of IDataErrorInfo

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        public string this[string columnName] { get { return null; } }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public string Error { get; private set; }

        #endregion

        #region Implementation of IPerformsValidation

        public Dictionary<string, List<IActionableErrorInfo>> Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
                OnPropertyChanged("Errors");
            }
        }

        public bool Validate(string propertyName, RuleSet ruleSet)
        {
            // TODO: Implement Validate(string propertyName, RuleSet ruleSet) - see ActivityDTO
            return true;
        }

        public bool Validate(string propertyName)
        {
            // TODO: Implement Validate(string propertyName) - see ActivityDTO
            return Validate(propertyName, new RuleSet());
        }

        #endregion
    }
}