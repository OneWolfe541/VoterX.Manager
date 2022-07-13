using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoterX.Dialogs;
using VoterX.Models;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Extensions;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Views.Absentee
{
    public class ReportWizardDateViewModel : NotifyPropertyChanged
    {
        public ReportWizardDateViewModel() : this(null) {}
        public ReportWizardDateViewModel(ReportWizardQueryModel wizardSearch)
        {
            // Initialize search parameters
            if(wizardSearch == null)
            {
                wizardSearch = new ReportWizardQueryModel();
            }
            WizardSearch = wizardSearch;

            GlobalReferences.Header.PageHeader = ("Report Wizard");
        }

        #region WizardOptions
        private ReportWizardQueryModel _wizardSearch;
        private ReportWizardQueryModel WizardSearch
        {
            get
            {
                if(_wizardSearch == null)
                {
                    _wizardSearch = new ReportWizardQueryModel();
                }
                return _wizardSearch;
            }

            set
            {
                _wizardSearch = value;
                RaisePropertyChanged("WizardSearch");

            }
        }
        #endregion

        #region Validation
        private bool ValidateTime(string time)
        {
            if (DateTime.TryParse(time, out DateTime s))
            {
                return true;
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("INVALID TIME FORMAT");
                newMessage.ShowDialog();                
                return false;
            }
        }

        private bool ValidateDate(DateTime? date)
        {
            if(date != null)
            {
                return true;
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("INVALID DATE FORMAT");
                newMessage.ShowDialog();
                return false;
            }
        }
        #endregion

        #region BeginningDateTime
        // Manage the start date for the report
        public DateTime? BeginningDate
        {
            get
            {
                if(WizardSearch.BeginningDate == null || WizardSearch.BeginningDate == DateTime.MinValue)
                {
                    WizardSearch.BeginningDate = DateTime.Now.Date;
                }
                return WizardSearch.BeginningDate;
            }

            set
            {
                if (ValidateDate(value))
                {
                    WizardSearch.BeginningDate = (value ?? DateTime.Now.Date);
                }
                RaisePropertyChanged("WizardSearch");
            }
        }

        // Manage the start time for the report
        private string _beginingTime;
        public string BeginningTime
        {
            get
            {
                if(_beginingTime == null)
                {
                    _beginingTime = WizardSearch.BeginningDate.ToShortTimeString();
                }
                return _beginingTime;
            }

            set
            {
                if (ValidateTime(value))
                {
                    _beginingTime = value;

                    // Update wizard time
                    DateTime dateTime = DateTime.Parse(_beginingTime);
                    TimeSpan time = dateTime.TimeOfDay;
                    WizardSearch.BeginningDate = WizardSearch.BeginningDate.Date + time;

                    RaisePropertyChanged("WizardSearch");
                }
            }
        }
        #endregion

        #region EndingDateTime
        // Manage the end date for the report
        public DateTime? EndingDate
        {
            get
            {
                if (WizardSearch.EndingDate == null || WizardSearch.EndingDate == DateTime.MinValue)
                {
                    //DateTime dateTime = DateTime.Parse("11:59 PM");
                    //TimeSpan time = dateTime.TimeOfDay;
                    WizardSearch.EndingDate = DateTime.Now.Date + ("11:59 PM").ToTimeSpan();
                }
                return WizardSearch.EndingDate;
            }

            set
            {
                if (ValidateDate(value))
                {
                    WizardSearch.EndingDate = (value ?? DateTime.Now.Date);
                    WizardSearch.EndingDate = WizardSearch.EndingDate.Date + _endingTime.ToTimeSpan();
                }
                RaisePropertyChanged("WizardSearch");
            }
        }

        // Manage the end time for the report
        private string _endingTime;
        public string EndingTime
        {
            get
            {
                // Check if end date has been set
                if(WizardSearch.EndingDate == null || WizardSearch.EndingDate == DateTime.MinValue)
                {
                    WizardSearch.EndingDate = DateTime.Now.Date + ("11:59 PM").ToTimeSpan();
                    _endingTime = "11:59 PM";
                }

                if (_endingTime == null)
                {
                    _endingTime = WizardSearch.EndingDate.ToShortTimeString();
                }
                return _endingTime;
            }

            set
            {
                if (ValidateTime(value))
                {
                    _endingTime = value;

                    // Update wizard time
                    //DateTime dateTime = DateTime.Parse(_beginingTime);
                    //TimeSpan time = dateTime.TimeOfDay;
                    WizardSearch.EndingDate = WizardSearch.EndingDate.Date + _endingTime.ToTimeSpan();

                    RaisePropertyChanged("WizardSearch");
                }
            }
        }
        #endregion

        #region Commands
        // Bound command for resetting the date ranges
        public RelayCommand _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                {
                    _resetCommand = new RelayCommand(param => this.ResetClick());
                }
                return _resetCommand;
            }
        }

        // Reset all date and time fields
        public void ResetClick()
        {
            WizardSearch.BeginningDate = DateTime.Now.Date;
            WizardSearch.EndingDate = DateTime.Now.Date + ("11:59 PM").ToTimeSpan();

            _beginingTime = null;
            _endingTime = null;

            RaisePropertyChanged("BeginningDate");
            RaisePropertyChanged("BeginningTime");
            RaisePropertyChanged("EndingDate");
            RaisePropertyChanged("EndingTime");
        }

        // Bound command for navigating to the next screen
        public RelayCommand _nextCommand;
        public ICommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                {
                    _nextCommand = new RelayCommand(param => this.NextClick());
                }
                return _nextCommand;
            }
        }

        // Force parent frame to navigate to the next page
        public void NextClick()
        {
            // Ensure all dates have been updated before leaving the page
            RaisePropertyChanged("BeginningTime");
            RaisePropertyChanged("EndingTime");

            NavigationMenuMethods.ReportWizardSelections(WizardSearch);
        }
        #endregion
    }
}
