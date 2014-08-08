﻿
using System.Collections.ObjectModel;
using System.Windows.Input;
using Dev2.Runtime.Configuration.ViewModels.Base;
using Dev2.Studio.Core.ViewModels.Base;

namespace Dev2.ViewModels.Deploy
{
    public class DeployDialogViewModel : SimpleBaseViewModel
    {
        #region Fields
        private DelegateCommand _executeCommmand;
        private DelegateCommand _cancelComand;
        ObservableCollection<DeployDialogTO> _conflictingItems;

        public ObservableCollection<DeployDialogTO> ConflictingItems
        {
            get
            {
                return _conflictingItems;
            }
            set
            {
                if(Equals(value, _conflictingItems))
                {
                    return;
                }
                _conflictingItems = value;
                NotifyOfPropertyChange("ConflictingItems");
            }
        }

        public DeployDialogViewModel(ObservableCollection<DeployDialogTO> resourcesInConflict)
        {
            ConflictingItems = resourcesInConflict;
        }

        #endregion Fields

        #region Commands
        public ICommand OkCommand
        {
            get
            {
                return _executeCommmand ?? (_executeCommmand = new DelegateCommand(param => Okay()));
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelComand ?? (_cancelComand = new DelegateCommand(param => Cancel()));
            }
        }
        #endregion Cammands

        #region Methods

        /// <summary>
        /// Used for saving the data input by the user to the file system and pushing the data back at the workflow
        /// </summary>
        public void Okay()
        {
            RequestClose(ViewModelDialogResults.Okay);
        }

        /// <summary>
        /// Used for canceling the drop of the design surface
        /// </summary>
        public void Cancel()
        {
            RequestClose(ViewModelDialogResults.Cancel);
        }

        #endregion
    }
}
