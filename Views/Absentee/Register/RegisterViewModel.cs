using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoterX.Dialogs;
using VoterX.Methods;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Extensions;
using VoterX.Manager.Global;

namespace VoterX.Manager.Views.Absentee
{
    public class RegisterViewModel : NotifyPropertyChanged
    {
        public RegisterViewModel()
        {
            // Display page header
            GlobalReferences.Header.PageHeader = ("Voter Register");
        }

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
            if (date != null)
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
        private DateTime? _beginningDate;
        public DateTime? BeginningDate
        {
            get
            {
                if (_beginningDate == null || _beginningDate == DateTime.MinValue)
                {
                    //_beginningDate = DateTime.Now.AddYears(-1).Date;
                    _beginningDate = new DateTime(DateTime.Now.Year, 1, 1);
                }
                return _beginningDate;
            }

            set
            {
                if (ValidateDate(value))
                {
                    //_beginningDate = (value ?? DateTime.Now.AddYears(-1).Date);
                    _beginningDate = (value ?? new DateTime(DateTime.Now.Year, 1, 1));
                }
                RaisePropertyChanged("BeginningDate");
            }
        }

        // Manage the start time for the report
        private string _beginingTime;
        public string BeginningTime
        {
            get
            {
                if (_beginingTime == null)
                {
                    _beginingTime = BeginningDate.Value.ToShortTimeString();
                }
                return _beginingTime;
            }

            set
            {
                if (ValidateTime(value))
                {
                    _beginingTime = value;
                    
                    DateTime dateTime = DateTime.Parse(_beginingTime);
                    TimeSpan time = dateTime.TimeOfDay;
                    BeginningDate = BeginningDate.Value.Date + time;

                    RaisePropertyChanged("BeginningDate");
                }
            }
        }
        #endregion

        #region EndingDateTime
        // Manage the end date for the report
        private DateTime? _endingDate;
        public DateTime? EndingDate
        {
            get
            {
                if (_endingDate == null || _endingDate == DateTime.MinValue)
                {
                    _endingDate = DateTime.Now.Date + ("11:59 PM").ToTimeSpan();
                }
                return _endingDate;
            }

            set
            {
                if (ValidateDate(value))
                {
                    _endingDate = (value ?? DateTime.Now.Date);
                }
                RaisePropertyChanged("EndingDate");
            }
        }

        // Manage the end time for the report
        private string _endingTime;
        public string EndingTime
        {
            get
            {
                if (_endingTime == null)
                {
                    _endingTime = EndingDate.Value.ToShortTimeString();
                }
                return _endingTime;
            }

            set
            {
                if (ValidateTime(value))
                {
                    _endingTime = value;

                    EndingDate = EndingDate.Value.Date + _beginingTime.ToTimeSpan();

                    RaisePropertyChanged("EndingDate");
                }
            }
        }
        #endregion

        #region CalculateDateRange
        private string _dateRange;
        public string DateRange
        {
            get
            {
                return _dateRange;
            }

            private set
            {
                _dateRange = value;
                RaisePropertyChanged("DateRange");
            }
        }

        private RelayCommand _calculateRangeCommand;
        public ICommand CalculateRangeCommand
        {
            get
            {
                if (_calculateRangeCommand == null)
                {
                    _calculateRangeCommand = new RelayCommand(param => this.CalculateRangeClick());
                }
                return _calculateRangeCommand;
            }
        }

        private void CalculateRangeClick()
        {
            DateRange = BeginningDate.ToString() + " - " + EndingDate.ToString(); 
        }

        private RelayCommand _clearRangeCommand;
        public ICommand ClearRangeCommand
        {
            get
            {
                if (_clearRangeCommand == null)
                {
                    _clearRangeCommand = new RelayCommand(param => this.ClearRangeClick());
                }
                return _clearRangeCommand;
            }
        }

        private void ClearRangeClick()
        {
            DateRange = "";

            //_beginningDate = DateTime.Now.AddYears(-1).Date;
            _beginningDate = new DateTime(DateTime.Now.Year, 1, 1);
            _endingDate = DateTime.Now.Date + ("11:59 PM").ToTimeSpan();

            _beginingTime = null;
            _endingTime = null;

            RaisePropertyChanged("BeginningDate");
            RaisePropertyChanged("BeginningTime");
            RaisePropertyChanged("EndingDate");
            RaisePropertyChanged("EndingTime");
        }
        #endregion

        #region PrintCommands
        private RelayCommand _printAbsenteeCommand;
        public ICommand PrintAbsenteeCommand
        {
            get
            {
                if (_printAbsenteeCommand == null)
                {
                    _printAbsenteeCommand = new RelayCommand(param => this.PrintAbsenteeClick());
                }
                return _printAbsenteeCommand;
            }
        }

        private void PrintAbsenteeClick()
        {
            CalculateRangeClick();

            GlobalReferences.StatusBar.TextCenter = (
            ReportPrintingMethods.PrintAbsenteeRegister((DateTime)BeginningDate, (DateTime)EndingDate)
            );
        }

        private RelayCommand _printEarlyCommand;
        public ICommand PrintEarlyCommand
        {
            get
            {
                if (_printEarlyCommand == null)
                {
                    _printEarlyCommand = new RelayCommand(param => this.PrintEarlyClick());
                }
                return _printEarlyCommand;
            }
        }

        private void PrintEarlyClick()
        {
            CalculateRangeClick();

            GlobalReferences.StatusBar.TextCenter = (
            ReportPrintingMethods.PrintEarlyVotingRegister((DateTime)BeginningDate, (DateTime)EndingDate)
            );
        }
        #endregion

        #region PreviewCommands
        private RelayCommand _previewAbsenteeCommand;
        public ICommand PreviewAbsenteeCommand
        {
            get
            {
                if (_previewAbsenteeCommand == null)
                {
                    _previewAbsenteeCommand = new RelayCommand(param => this.PreviewAbsenteeClick());
                }
                return _previewAbsenteeCommand;
            }
        }

        private void PreviewAbsenteeClick()
        {
            CalculateRangeClick();

            ReportPrintingMethods.PreviewAbsenteeRegister((DateTime)BeginningDate, (DateTime)EndingDate);
        }

        private RelayCommand _previewEarlyCommand;
        public ICommand PreviewEarlyCommand
        {
            get
            {
                if (_previewEarlyCommand == null)
                {
                    _previewEarlyCommand = new RelayCommand(param => this.PreviewEarlyClick());
                }
                return _previewEarlyCommand;
            }
        }

        private void PreviewEarlyClick()
        {
            CalculateRangeClick();

            ReportPrintingMethods.PreviewEarlyVotingRegister((DateTime)BeginningDate, (DateTime)EndingDate);
        }
        #endregion
    }
}
