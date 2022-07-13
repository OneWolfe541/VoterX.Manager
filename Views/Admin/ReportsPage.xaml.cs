using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoterX.Methods;
using VoterX.Factories;
using VoterX.Interfaces;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : Page
    {
        private string _DateOptions = "TODAY";

        public ReportsPage()
        {
            InitializeComponent();

            LoadActiveDates();

            LoadReports();

            // Display page header
            GlobalReferences.Header.PageHeader = ("Early Voting Reports");
        }

        private void LoadActiveDates()
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(ActiveDateList, TempLoadingSpinnerItem);

            // Check if the server is alive
            //if (await Task.Run(() => VoterMethods.Voter.Exists()) == true)
            //{
            //    // Get list of active dates
            //    var dateList = await Task.Run(() => VoterMethods.Voter.ActivityDates((int)AppSettings.System.SiteID));
            //    foreach (var dateItem in dateList)
            //    {
            //        // Add date to combo box
            //        // and select todays date
            //        ComboBoxMethods.AddComboItemToControl(
            //            ActiveDateList,
            //            dateItem,
            //            dateItem,
            //            DateTime.Now.ToShortDateString()
            //            );
            //    }
            //}
            //else
            //{
            //    StatusBar.ApplicationStatusCenter("Database not found");
            //}

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(ActiveDateList, loadingItem);
        }

        private void LoadReports()
        {
            var reportList = EndOfDayReportingFactory.GetEndOfDayReports();

            foreach (var report in reportList)
            {
                ComboBoxMethods.AddComboItemToControl(
                    ReportList,
                    report,
                    report.ReportName,
                    false
                    );
            }

            ReportList.SelectedIndex = 0;
        }

        private void ReportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TodaysActivityRadio_Click(object sender, RoutedEventArgs e)
        {
            // Set todays option
            _DateOptions = "TODAY";

            SpecificDatePanel.Visibility = Visibility.Collapsed;
        }

        private void SpecificDateRadio_Click(object sender, RoutedEventArgs e)
        {
            // Set specific option
            _DateOptions = "SPECIFIC";

            SpecificDatePanel.Visibility = Visibility.Visible;
        }

        private void ActiveDateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            // Get selected item
            var item = (System.Windows.Controls.ComboBoxItem)ReportList.SelectedItem;

            // Convert selected item into Reporting Object
            var report = (IAbsenteeReporting)item.DataContext;

            string message = "";

            // Check which date option is selected
            switch (_DateOptions)
            {
                // Todays activity only
                case "TODAY":
                    message = report.PrintReport(
                        AppSettings.Election.ElectionID,
                        AppSettings.System.SiteID);
                    break;

                // Selected a specific date
                case "SPECIFIC":
                    // Initialize the date
                    DateTime theDate = DateTime.Now;

                    // Vaidate the given date
                    if (DateTime.TryParse(ComboBoxMethods.GetSelectedItem(ActiveDateList), out theDate) == true)
                    {
                        message = report.PrintReport(
                            AppSettings.Election.ElectionID,
                            AppSettings.System.SiteID,
                            theDate);
                    }
                    else
                    {
                        message = "Invalid Date";
                    }
                    break;
            }
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            // Get selected item
            var item = (System.Windows.Controls.ComboBoxItem)ReportList.SelectedItem;

            // Convert selected item into Reporting Object
            var report = (IAbsenteeReporting)item.DataContext;

            //string message = "";

            // Check which date option is selected
            switch (_DateOptions)
            {
                // Todays activity only
                case "TODAY":
                    report.PreviewReport(
                        AppSettings.Election.ElectionID,
                        AppSettings.System.SiteID);
                    break;

                // Selected a specific date
                case "SPECIFIC":
                    // Initialize the date
                    DateTime theDate = DateTime.Now;

                    // Vaidate the given date
                    if (DateTime.TryParse(ComboBoxMethods.GetSelectedItem(ActiveDateList), out theDate) == true)
                    {
                        report.PreviewReport(
                            AppSettings.Election.ElectionID,
                            AppSettings.System.SiteID,
                            theDate);
                    }
                    else
                    {
                        //message = "Invalid Date";
                    }
                    break;
            }
        }
    }
}
