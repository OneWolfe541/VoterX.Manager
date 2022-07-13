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
using VoterX.SystemSettings.Extensions;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Interfaces;
using VoterX.Utilities.Models;
using VoterX.Utilities.Reports;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Views.Manage
{
    public class DailyReportsViewModel : NotifyPropertyChanged
    {
        public DailyReportsViewModel()
        {
            _dateOptions = "TODAY";

            // Display page header
            GlobalReferences.Header.PageHeader = ("Early Voting Reports");
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
            NavigationMenuMethods.ReturnToOrigin();
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
                // Initialize the date
                DateTime theDate = DateTime.Now;

                // Check which date option is selected
                switch (_dateOptions)
                {
                    // Todays activity only
                    case "TODAY":
                        SelectedReportItem.PrintReport(
                            AppSettings.Global,
                            (int)AppSettings.System.SiteID,
                            theDate,
                            true);
                        break;

                    // Selected a specific date
                    case "SPECIFIC":
                        // Vaidate the given date
                        if (
                            SelectedActivityDateItem != null &&
                            DateTime.TryParse(SelectedActivityDateItem.Date, out theDate) == true
                            )
                        {
                            SelectedReportItem.PrintReport(
                                AppSettings.Global,
                                (int)AppSettings.System.SiteID,
                                theDate,
                                true);
                        }
                        else
                        {
                            // No Date Selected
                            AlertDialog newMessage = new AlertDialog("No Date Selected");
                            newMessage.ShowDialog();
                        }
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
                // Initialize the date
                DateTime theDate = DateTime.Now;

                // Check which date option is selected
                switch (_dateOptions)
                {
                    // Todays activity only
                    case "TODAY":
                        SelectedReportItem.PreviewReport(
                            AppSettings.Global,
                            (int)AppSettings.System.SiteID,
                            theDate
                            );
                        break;

                    // Selected a specific date
                    case "SPECIFIC":
                        // Vaidate the given date
                        if (
                            SelectedActivityDateItem != null &&
                            DateTime.TryParse(SelectedActivityDateItem.Date, out theDate) == true
                            )
                        {
                            SelectedReportItem.PreviewReport(
                                AppSettings.Global,
                                (int)AppSettings.System.SiteID,
                                theDate);
                        }
                        else
                        {
                            // No Date Selected
                            AlertDialog newMessage = new AlertDialog("No Date Selected");
                            newMessage.ShowDialog();
                        }
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
                    int type = AppSettings.Election.ElectionType.ToInt();

                    //_reportsList = VCCReportingFactory.GetVCCReports().ToList();
                    //_reportsList = EndOfDayReportingFactory.GetEndOfDayReports().ToList();
                    _reportsList = EndOfDayReportingFactory.GetReports(type, (int)AppSettings.System.VCCType).ToList();
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

        #region ActivityDates
        private List<ActivityDateModel> _activityDateList;
        public List<ActivityDateModel> ActivityDateList
        {
            get
            {
                if (_activityDateList == null)
                {
                    _activityDateList = GetActivityDates();
                }
                return _activityDateList;
            }
        }

        private List<ActivityDateModel> GetActivityDates()
        {
            // Check if the server is alive
            if (VoterMethods.Exists == true)
            {
                // Get all active dates for this site from voted records
                var dateList = VoterMethods.Voters.ActivityDates((int)AppSettings.System.SiteID);

                // Create blank list
                List<ActivityDateModel> activityDates = new List<ActivityDateModel>();

                if (dateList != null)
                {
                    // Loop through list
                    foreach (var date in dateList)
                    {
                        // Convert date collection to Activity Date list
                        ActivityDateModel item = new ActivityDateModel { Date = date };
                        activityDates.Add(item);
                    }
                }
                return activityDates;
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
                return null;
            }
        }

        private ActivityDateModel _selectedActivityDateItem;
        public ActivityDateModel SelectedActivityDateItem
        {
            get
            {
                // Preselect the first report
                if (_selectedActivityDateItem == null)
                {
                    _selectedActivityDateItem = ActivityDateList
                        .FirstOrDefault();
                }
                return _selectedActivityDateItem;
            }
            set
            {
                // When a reason is selected display the print button
                _selectedActivityDateItem = value;
            }
        }
        #endregion

        #region DateOptions
        private string _dateOptions;
        public bool SpecificDateVisibility { get; set; }

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
                    RaisePropertyChanged("SpecificDateVisibility");

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
                    RaisePropertyChanged("SpecificDateVisibility");
                }
            }
        }
        #endregion
    }
}
