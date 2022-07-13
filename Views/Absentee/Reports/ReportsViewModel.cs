using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoterX.Dialogs;
using VoterX.Factories;
using VoterX.Interfaces;
using VoterX.Methods;
using VoterX.Core.Elections;
using VoterX.SystemSettings.Enums;
using VoterX.SystemSettings.Extensions;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Extensions;
using VoterX.Utilities.Interfaces;
using VoterX.Utilities.Models;
using VoterX.Utilities.Reports;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Views.Absentee
{
    public class DailyReportsViewModel : NotifyPropertyChanged
    {
        public DailyReportsViewModel()
        {
            _dateOptions = "ALL";

            // Display page header
            GlobalReferences.Header.PageHeader = ("Reports");
        }

        #region Commands
        // Bound command for returning to the main menu
        public RelayCommand _goBackCommand;
        public ICommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(param => this.ReturnToMenuClick());
                }
                return _goBackCommand;
            }
        }

        // Force parent frame to navigate back to the main menu
        public void ReturnToMenuClick()
        {
            //MainMenuMethods.OpenMenu(); // THIS COMMAND SHOULD HAPPEN ELSEWHERE!
            GlobalReferences.MenuSlider.Open(); // THIS COMMAND SHOULD HAPPEN ELSEWHERE!

            //_parent.Navigate(new VoterSearchPage(_parent));
            //NavigationMenuMethods.VoterSearchPage();

            //NavigationMenuMethods.ReturnToOrigin();
            NavigationMenuMethods.AbsenteeHome();
        }

        public RelayCommand _printCommand;
        public ICommand PrintCommand
        {
            get
            {
                if (_printCommand == null)
                {
                    _printCommand = new RelayCommand(param => this.PrintClick());
                }
                return _printCommand;
            }
        }

        private void PrintClick()
        {
            if (SelectedReportItem != null)
            {
                int? siteId = null;
                if(_siteOptions == "SPECIFIC" && SelectedLocationItem != null)
                {
                    siteId = SelectedLocationItem.PollId;
                }

                // Check which date option is selected
                switch (_dateOptions)
                {
                    // Activity to date
                    case "ALL":
                        if (SelectedReportItem.Daily == true)
                        {
                            AlertDialog newMessage = new AlertDialog("PLEASE SELECT A SPECIFIC DATE FOR THIS REPORT");
                            newMessage.ShowDialog();
                            return;
                        }

                        GlobalReferences.StatusBar.TextCenter =
                        SelectedReportItem.PrintReport(
                            AppSettings.Global,
                            siteId,
                            true);
                        break;

                    // Todays activity only
                    case "TODAY":
                        GlobalReferences.StatusBar.TextCenter =
                        SelectedReportItem.PrintReport(
                            AppSettings.Global,
                            siteId,
                            DateTime.Now,
                            true);
                        break;

                    // Selected specific date
                    case "SPECIFIC":
                        GlobalReferences.StatusBar.TextCenter =
                        SelectedReportItem.PrintReport(
                            AppSettings.Global,
                            siteId,
                            (DateTime)BeginningDate,
                            true);
                        break;

                    // Selected date range
                    case "RANGE":
                        if (SelectedReportItem.Daily == true)
                        {
                            AlertDialog newMessage = new AlertDialog("PLEASE SELECT A SPECIFIC DATE FOR THIS REPORT");
                            newMessage.ShowDialog();
                            return;
                        }

                        GlobalReferences.StatusBar.TextCenter =
                        SelectedReportItem.PrintReport(
                            AppSettings.Global,
                            siteId,
                            (DateTime)BeginningDate,
                            (DateTime)EndingDate,
                            true);
                        break;
                }
            }
        }

        public RelayCommand _previewCommand;
        public ICommand PreviewCommand
        {
            get
            {
                if (_previewCommand == null)
                {
                    _previewCommand = new RelayCommand(param => this.PreviewClick());
                }
                return _previewCommand;
            }
        }

        private void PreviewClick()
        {
            if (SelectedReportItem != null)
            {
                int? siteId = null;
                if (_siteOptions == "SPECIFIC" && SelectedLocationItem != null)
                {
                    siteId = SelectedLocationItem.PollId;
                }

                // Check which date option is selected
                switch (_dateOptions)
                {
                    // Activity to date
                    case "ALL":
                        if (SelectedReportItem.Daily == true)
                        {
                            AlertDialog newMessage = new AlertDialog("PLEASE SELECT A SPECIFIC DATE FOR THIS REPORT");
                            newMessage.ShowDialog();
                            return;
                        }

                        GlobalReferences.StatusBar.TextCenter =
                        SelectedReportItem.PreviewReport(
                            AppSettings.Global,
                            siteId);
                        break;

                    // Todays activity only
                    case "TODAY":
                        GlobalReferences.StatusBar.TextCenter =
                        SelectedReportItem.PreviewReport(
                            AppSettings.Global,
                            siteId,
                            DateTime.Now);
                        break;

                    // Selected specific date
                    case "SPECIFIC":
                        GlobalReferences.StatusBar.TextCenter =
                        SelectedReportItem.PreviewReport(
                            AppSettings.Global,
                            siteId,
                            (DateTime)BeginningDate);
                        break;

                    // Selected date range
                    case "RANGE":
                        if (SelectedReportItem.Daily == true)
                        {
                            AlertDialog newMessage = new AlertDialog("PLEASE SELECT A SPECIFIC DATE FOR THIS REPORT");
                            newMessage.ShowDialog();
                            return;
                        }

                        GlobalReferences.StatusBar.TextCenter =
                        SelectedReportItem.PreviewReport(
                            AppSettings.Global,
                            siteId,
                            (DateTime)BeginningDate,
                            (DateTime)EndingDate);
                        break;
                }
            }
        }
        #endregion

        #region Reports
        private List<IReport> _reportsList;
        public List<IReport> ReportsList
        {
            get
            {
                if (_reportsList == null)
                {
                    // Get the Election type
                    var type = AppSettings.Election.ElectionType;

                    // Check if all mail mode
                    if (AppSettings.Absentee.AllMailMode == true) type = ElectionType.AllMail;

                    // Get the list of reports
                    _reportsList = AbsenteeReportingFactory.GetReports(type.ToInt()).ToList();
                }
                return _reportsList;
            }
        }

        private IReport _selectedReportItem;
        public IReport SelectedReportItem
        {
            get
            {
                // Preselect the first report
                if (_selectedReportItem == null)
                {
                    _selectedReportItem = ReportsList
                        //.Where(r => r.ReportName == "Daily Summary Report")
                        .FirstOrDefault();
                }
                return _selectedReportItem;
            }
            set
            {
                // When a reason is selected display the print button
                _selectedReportItem = value;
            }
        }
        #endregion

        #region Locations
        private string _siteOptions;
        public bool SiteSelectionVisibility { get; set; }

        public bool AllSitesIsSelected
        {
            get
            {
                if (_siteOptions == "ALL")
                {
                    return true;
                }
                else return false;
            }
            set
            {
                if (value == true)
                {
                    _siteOptions = "ALL";

                    SiteSelectionVisibility = false;
                    RaisePropertyChanged("SiteSelectionVisibility");

                }
            }
        }

        public bool SpecificSiteIsSelected
        {
            get
            {
                if (_siteOptions == "SPECIFIC")
                {
                    return true;
                }
                else return false;
            }
            set
            {
                if (value == true)
                {
                    _siteOptions = "SPECIFIC";

                    SiteSelectionVisibility = true;
                    RaisePropertyChanged("SiteSelectionVisibility");

                }
            }
        }

        private List<LocationModel> _locationsList;
        public List<LocationModel> LocationsList
        {
            get
            {
                if (_locationsList == null)
                {
                    _locationsList = ElectionDataMethods.Locations.OrderBy(o => o.PlaceName).ToList();
                }
                return _locationsList;
            }
        }

        private LocationModel _selectedLocationItem;
        public LocationModel SelectedLocationItem
        {
            get
            {
                // Preselect the first location
                //if (_selectedLocationItem == null)
                //{
                //    _selectedLocationItem = LocationsList
                //        .FirstOrDefault();
                //}
                // Preselect the current location
                if (_selectedLocationItem == null)
                {
                    _selectedLocationItem = LocationsList
                        .Where(l => l.PollId == AppSettings.System.SiteID)
                        .FirstOrDefault();
                }
                return _selectedLocationItem;
            }
            set
            {
                _selectedLocationItem = value;
            }
        }
        #endregion

        #region DateOptions
        private string _dateOptions;
        public bool SpecificDateVisibility { get; set; }
        public bool DateRangeVisibility { get; set; }

        // Set the report date to activity to date
        public bool ActivityIsSelected
        {
            get
            {
                if (_dateOptions == "ALL")
                {
                    return true;
                }
                else return false;
            }
            set
            {
                if (value == true)
                {
                    _dateOptions = "ALL";

                    SpecificDateVisibility = false;
                    DateRangeVisibility = false;
                    RaisePropertyChanged("SpecificDateVisibility");
                    RaisePropertyChanged("DateRangeVisibility");

                }
            }
        }

        // Set the report date to today only
        public bool TodayIsSelected
        {
            get
            {
                if (_dateOptions == "TODAY")
                {
                    return true;
                }
                else return false;
            }
            set
            {
                if (value == true)
                {
                    _dateOptions = "TODAY";

                    SpecificDateVisibility = false;
                    DateRangeVisibility = false;
                    RaisePropertyChanged("SpecificDateVisibility");
                    RaisePropertyChanged("DateRangeVisibility");

                }
            }
        }

        // Set the report date to a specific date
        public bool SpecificIsSelected
        {
            get
            {
                if (_dateOptions == "SPECIFIC")
                {
                    return true;
                }
                else return false;
            }
            set
            {
                if (value == true)
                {
                    _dateOptions = "SPECIFIC";

                    SpecificDateVisibility = true;
                    DateRangeVisibility = false;
                    RaisePropertyChanged("SpecificDateVisibility");
                    RaisePropertyChanged("DateRangeVisibility");
                }
            }
        }

        // Set the report date to a specific date
        public bool RangeIsSelected
        {
            get
            {
                if (_dateOptions == "RANGE")
                {
                    return true;
                }
                else return false;
            }
            set
            {
                if (value == true)
                {
                    _dateOptions = "RANGE";

                    SpecificDateVisibility = false;
                    DateRangeVisibility = true;
                    RaisePropertyChanged("SpecificDateVisibility");
                    RaisePropertyChanged("DateRangeVisibility");
                }
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
                    _beginningDate = DateTime.Now.Date;
                }
                return _beginningDate;
            }

            set
            {
                if (ValidateDate(value))
                {
                    _beginningDate = (value ?? DateTime.Now.Date);
                }
                RaisePropertyChanged("BeginningDate");
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
        #endregion
    }
}
