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
using VoterX.Extensions;
using FastReport;
using FastReport.DevComponents.DotNetBar;
using System.Configuration;
using VoterX.Reporting;
using VoterX.Interfaces;
using VoterX.Factories;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for ReportsMenuPage.xaml
    /// </summary>
    public partial class AbsenteeReportPage : Page
    {
        string _DateOptions = "ALL";

        int? _PollID = null;

        public AbsenteeReportPage()
        {
            InitializeComponent();

            LoadPollSites();

            LoadReportsList();

            // Display page header
            GlobalReferences.Header.PageHeader = ("Reports");

            GlobalReferences.StatusBar.CheckPrinterStatus(AppSettings.Printers.BallotPrinter);

            CheckServer();
        }

        private void CheckServer()
        {
            //PrintButtonsPanel.Visibility = Visibility.Collapsed;

            //if (await StatusBar.CheckServer() == true)
            //{
            //    PrintButtonsPanel.Visibility = Visibility.Visible;
            //}
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.OpenMainMenu();
            GlobalReferences.MenuSlider.Open();


            this.NavigateToPage(new AbsenteeAdministrationPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new LoginPage());
        }

        private async void LoadPollSites()
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(PollList, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var siteList = await Task.Run(() => ElectionDataMethods.Locations.OrderBy(o => o.PlaceName));
                if (siteList != null)
                {
                    foreach (var pollingLocation in siteList)
                    {
                        ComboBoxMethods.AddComboItemToControl(
                            PollList,
                            pollingLocation.PollId,
                            pollingLocation.PlaceName,
                            (int)AppSettings.System.SiteID
                            );
                    }
                }
            }

            ComboBoxMethods.RemoveListItem(PollList, loadingItem);
        }

        private void LoadReports()
        {
            //RosterBySiteItem.DataContext = new DailyDetailBySiteReport("Roster Details by Site");
            //ActivityBySiteItem.DataContext = new DailySummaryBySiteReport("Activity by Site");
            //SummaryByBallotStyleItem.DataContext = new SummaryByBallotStyleReport("Summary Report");
            //SpoiledBallotsItem.DataContext = new SpoiledBallotDetailsReport("Spoiled Ballots Report");
            //ProvisionalBallotsItem.DataContext = new ProvisionalDetailsReport("Provisional Ballots Report");
            //UnsignedEnvelopesItem.DataContext = new UnsignedDetailsReport("Unsigned Envelopes");
            //RejectedApplicationsItem.DataContext = new RejectedApplicationsReport("Rejected Applications");
        }

        private void LoadReportsList()
        {
            var reportList = GetAbsenteeReports();

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

        // Get the list of reports depending on what mode the system is in
        private IEnumerable<IAbsenteeReporting> GetAbsenteeReports()
        {
            if (AppSettings.Absentee.AllMailMode)
            {
                return AbsenteeReportingFactory.GetAllMailReports();
            }
            else
            {
                return AbsenteeReportingFactory.GetAbsenteeReports();
            }
        }

        private void FastReportButton_Click(object sender, RoutedEventArgs e)
        {
            PrintFastReport();
        }

        private void OpenFastReportDialog()
        {
            //ReportPreviewWindow reportPreview = new ReportPreviewWindow("VoterStandardReport02");
            //reportPreview.ShowDialog();
        }

        private void PrintFastReport()
        {
            //string ReportName = "VoterStandardReport02";

            //FastReport.Report report1 = new FastReport.Report();
            //report1.Load(@"C:/VoterX/Reports/" + ReportName + ".frx");
            //report1.Prepare();
            //report1.PrintSettings.ShowDialog = false;
            //report1.PrintPrepared();

            //var message = FastReportsMethods.PrintSilentReport("VoterStandardReport02");
            //var message = FastReportsMethods.PrintSilentReport("Application01");

            //string sql = "SELECT tblElection.*, voter_id = 2107792, full_name = 'VoterX User', birth_year = 1980, printed_date = '" + DateTime.Now.ToString() + "', address = '7000 Zenith Dr', city_state_zip = 'Albuquerque', election_date_long = 'Dont Bother Me', signature_path = 'C:\\VoterX\\Signatures\\2107792.jpg', election_entity = 'Town of Santa Fe', jurisdiction = 'Not My Jurisdiction', county_name = 'Santa Fe', poll_id = 459, place_name = 'Debuging North Dakota', computer = -1, vccmode = 'Early Voter' FROM tblElection WHERE[election_id] = 112";
            //string message = FastReportsMethods.PrintSilentReport("Application01", sql);
            //StatusBar.ApplicationStatusCenter(message);

            GlobalReferences.StatusBar.TextLeft = (ConfigurationManager.ConnectionStrings["VoterDatabase"].ConnectionString);
        }

        private void DailyDetailReportButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyDetailBCReportGeneral(49, DateTime.Now));
        }

        private void DailyDetailReportBSButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyDetailBSReport(49, DateTime.Now));
        }

        private void ZeroReportButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintZeroEarlyVotingBSReport(112, 49));
            //StatusBar.ApplicationStatusCenter(ReportPrintingMethods.PrintZeroElectionDayReport(112, 49, DateTime.Now ));
        }

        private void DailyProvisionalButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyProvisionalReport(112, 49, DateTime.Now));
        }

        private void DailySpoiledButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailySpoiledGeneralReport(112, 49, DateTime.Now));
        }

        private void DetailsReportButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyDetailBySiteReportGeneral(
                AppSettings.Election.ElectionID,
                null));
        }

        private void TodayDetailReportButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyDetailBySiteReport(
                AppSettings.Election.ElectionID,
                AppSettings.System.SiteID,
                DateTime.Now));
        }

        private void DailyDetailRangedButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyDetailBySiteReport(
                AppSettings.Election.ElectionID,
                AppSettings.System.SiteID,
                DateTime.Parse("04/01/2018"),
                DateTime.Now));
        }

        private void SiteSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailySummaryBySiteReport(
                AppSettings.Election.ElectionID,
                null));
        }

        private void TodaysSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailySummaryBySiteReport(
                AppSettings.Election.ElectionID,
                AppSettings.System.SiteID,
                DateTime.Now));
        }

        private void RangedSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailySummaryBySiteReport(
                AppSettings.Election.ElectionID,
                AppSettings.System.SiteID,
                DateTime.Parse("04/01/2018"),
                DateTime.Now));
        }

        private void SiteSummaryBSButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSummaryByBallotStyleReport(
                AppSettings.Election.ElectionID,
                null));
        }

        private void TodaysSummaryBSButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSummaryByBallotStyleReport(
                AppSettings.Election.ElectionID,
                AppSettings.System.SiteID,
                DateTime.Now));
        }

        private void RangedSummaryBSButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSummaryByBallotStyleReport(
                AppSettings.Election.ElectionID,
                AppSettings.System.SiteID,
                DateTime.Parse("04/01/2018"),
                DateTime.Now));
        }

        private void SpoiledSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSpoiledBallotDetailsReport(
                AppSettings.Election.ElectionID,
                null));
        }

        private void TodaysSpoiledSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSpoiledBallotDetailsReport(
                AppSettings.Election.ElectionID,
                AppSettings.System.SiteID,
                DateTime.Now));
        }

        private void RangedSpoiledSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSpoiledBallotDetailsReport(
                AppSettings.Election.ElectionID,
                AppSettings.System.SiteID,
                DateTime.Parse("03/28/2018"),
                DateTime.Now));
        }

        private void ProvisionalSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintProvisionalDetailsReport(
                AppSettings.Election.ElectionID,
                null));
        }

        private void TodaysProvisionalSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintProvisionalDetailsReport(
                AppSettings.Election.ElectionID,
                //AppSettings.System.SiteID,
                null,
                DateTime.Now));
        }

        private void RangedProvisionalSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintProvisionalDetailsReport(
                AppSettings.Election.ElectionID,
                //AppSettings.System.SiteID,
                null,
                DateTime.Parse("03/28/2018"),
                DateTime.Now));
        }

        private void UnsignedSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintUnsignedDetailsReport(
                AppSettings.Election.ElectionID,
                null));
        }

        private void TodaysUnsignedSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintUnsignedDetailsReport(
                AppSettings.Election.ElectionID,
                //AppSettings.System.SiteID,
                null,
                DateTime.Now));
        }

        private void RangedUnsignedSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintUnsignedDetailsReport(
                AppSettings.Election.ElectionID,
                //AppSettings.System.SiteID,
                null,
                DateTime.Parse("03/28/2018"),
                DateTime.Now));
        }

        private void RejectedDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintRejectedApplicationDetailsReport(
                AppSettings.Election.ElectionID,
                null));
        }

        private void TodaysRejectedDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintRejectedApplicationDetailsReport(
                AppSettings.Election.ElectionID,
                //AppSettings.System.SiteID,
                null,
                DateTime.Now));
        }

        private void RangedRejectedDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintRejectedApplicationDetailsReport(
                AppSettings.Election.ElectionID,
                //AppSettings.System.SiteID,
                null,
                DateTime.Parse("03/28/2018"),
                DateTime.Now));
        }        

        private void DateRadioOptions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ActivityToDateRadio_Click(object sender, RoutedEventArgs e)
        {
            // Set to date option
            _DateOptions = "ALL";

            // Hide date boxes
            SpecificDatePanel.Visibility = Visibility.Collapsed;
            DateRangePanel.Visibility = Visibility.Collapsed;
        }

        private void TodaysActivityRadio_Click(object sender, RoutedEventArgs e)
        {
            // Set todays option
            _DateOptions = "TODAY";

            // Hide date boxes
            SpecificDatePanel.Visibility = Visibility.Collapsed;
            DateRangePanel.Visibility = Visibility.Collapsed;
        }

        private void SpecificDateRadio_Click(object sender, RoutedEventArgs e)
        {
            // Set specific option
            _DateOptions = "SPECIFIC";

            // Display single date box
            SpecificDatePanel.Visibility = Visibility.Visible;
            DateRangePanel.Visibility = Visibility.Collapsed;
        }

        private void DateRangeRadio_Click(object sender, RoutedEventArgs e)
        {
            // Set date range option'
            _DateOptions = "RANGE";

            // Display date range boxes
            SpecificDatePanel.Visibility = Visibility.Collapsed;
            DateRangePanel.Visibility = Visibility.Visible;
        }

        private void AllSitesRadio_Click(object sender, RoutedEventArgs e)
        {
            _PollID = null;
            SitesListPanel.Visibility = Visibility.Hidden;
        }

        private void SelectedSitesRadio_Click(object sender, RoutedEventArgs e)
        {
            _PollID = ComboBoxMethods.GetSelectedItemNumber(PollList);
            SitesListPanel.Visibility = Visibility.Visible;
        }

        private void ReportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PollList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedSitesRadio.IsChecked == true)
            {
                _PollID = ComboBoxMethods.GetSelectedItemNumber(PollList);                
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            var reportItem = ComboBoxMethods.GetSelectedItemNumber(ReportList);
            //string reportName = ComboBoxMethods.GetSelectedItem(ReportList);

            //StatusBar.ApplicationStatus(
            //    reportItem.ToString() + " " +
            //    reportName.ToString() + " " +
            //    _DateOptions + " " +
            //    SpecificDatePicker.SelectedDate.ToString() + " " +
            //    BeginningDatePicker.SelectedDate.ToString() + " " +
            //    EndingDatePicker.SelectedDate.ToString() + " " +
            //    _PollID.ToString());

            //StatusBar.ApplicationStatus(((DateTime)PickedDate.SelectedDate).ToShortDateString());

            switch (reportItem)
            {
                case 1:
                    PrintDailyDetailReport();
                    break;
                case 2:
                    PrintActivityBySiteReport();
                    break;
                case 3:
                    PrintSummaryByBallotStyleReport();
                    break;
                case 4:
                    PrintSpoiledBallotsReport();
                    break;
                case 5:
                    PrintProvisionalBallotsReport();
                    break;
                case 6:
                    PrintUnsignedEnvelopesReport();
                    break;
                case 7:
                    PrintRejectedApplicationsReport();
                    break;
            }
        }

        // Make these reports into generic objects 
        // Then just simply call .PrintReport from the selected item
        private void PrintDailyDetailReport()
        {
            switch(_DateOptions)
            {
                case "ALL":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyDetailBySiteReport(
                        AppSettings.Election.ElectionID,
                        _PollID));
                    break;
                case "TODAY":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyDetailBySiteReport(
                        AppSettings.Election.ElectionID,
                        _PollID,
                        DateTime.Now));
                    break;
                case "SPECIFIC":
                    DateTime theDate = DateTime.Now;
                    if (DateTime.TryParse(SpecificDatePicker.SelectedDate.ToString(), out theDate) == true)
                    {
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyDetailBySiteReport(
                            AppSettings.Election.ElectionID,
                            _PollID,
                            theDate));
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Date");
                    }                   
                    break;
                case "RANGE":
                    DateTime startdate = DateTime.Now;
                    DateTime enddate = DateTime.Now.AddDays(1);
                    if (DateTime.TryParse(BeginningDatePicker.SelectedDate.ToString(), out startdate) == true)
                    {
                        if (DateTime.TryParse(EndingDatePicker.SelectedDate.ToString(), out enddate) == true)
                        {
                            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyDetailBySiteReport(
                                AppSettings.Election.ElectionID,
                                _PollID,
                                startdate,
                                enddate));
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextCenter = ("Invalid Ending Date");
                        }
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Beginning Date");
                    }
                    break;
            }            
        }

        private void PrintActivityBySiteReport()
        {
            switch (_DateOptions)
            {
                case "ALL":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailySummaryBySiteReportGeneral(
                        AppSettings.Election.ElectionID,
                        _PollID));
                    break;
                case "TODAY":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailySummaryBySiteReportGeneral(
                        AppSettings.Election.ElectionID,
                        _PollID,
                        DateTime.Now));
                    break;
                case "SPECIFIC":
                    DateTime theDate = DateTime.Now;
                    if (DateTime.TryParse(SpecificDatePicker.SelectedDate.ToString(), out theDate) == true)
                    {
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailySummaryBySiteReportGeneral(
                            AppSettings.Election.ElectionID,
                            _PollID,
                            theDate));
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Date");
                    }
                    break;
                case "RANGE":
                    DateTime startdate = DateTime.Now;
                    DateTime enddate = DateTime.Now.AddDays(1);
                    if (DateTime.TryParse(BeginningDatePicker.SelectedDate.ToString(), out startdate) == true)
                    {
                        if (DateTime.TryParse(EndingDatePicker.SelectedDate.ToString(), out enddate) == true)
                        {
                            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailySummaryBySiteReportGeneral(
                                AppSettings.Election.ElectionID,
                                _PollID,
                                startdate,
                                enddate));
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextCenter = ("Invalid Ending Date");
                        }
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Beginning Date");
                    }
                    break;
            }
        }

        private void PrintSummaryByBallotStyleReport()
        {
            switch (_DateOptions)
            {
                case "ALL":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSummaryByBallotStyleReport(
                        AppSettings.Election.ElectionID,
                        _PollID));
                    break;
                case "TODAY":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSummaryByBallotStyleReport(
                        AppSettings.Election.ElectionID,
                        _PollID,
                        DateTime.Now));
                    break;
                case "SPECIFIC":
                    DateTime theDate = DateTime.Now;
                    if (DateTime.TryParse(SpecificDatePicker.SelectedDate.ToString(), out theDate) == true)
                    {
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSummaryByBallotStyleReport(
                            AppSettings.Election.ElectionID,
                            _PollID,
                            theDate));
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Date");
                    }
                    break;
                case "RANGE":
                    DateTime startdate = DateTime.Now;
                    DateTime enddate = DateTime.Now.AddDays(1);
                    if (DateTime.TryParse(BeginningDatePicker.SelectedDate.ToString(), out startdate) == true)
                    {
                        if (DateTime.TryParse(EndingDatePicker.SelectedDate.ToString(), out enddate) == true)
                        {
                            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSummaryByBallotStyleReport(
                                AppSettings.Election.ElectionID,
                                _PollID,
                                startdate,
                                enddate));
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextCenter = ("Invalid Ending Date");
                        }
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Beginning Date");
                    }
                    break;
            }
        }

        private void PrintSpoiledBallotsReport()
        {
            switch (_DateOptions)
            {
                case "ALL":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSpoiledBallotDetailsReport(
                        AppSettings.Election.ElectionID,
                        _PollID));
                    break;
                case "TODAY":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSpoiledBallotDetailsReport(
                        AppSettings.Election.ElectionID,
                        _PollID,
                        DateTime.Now));
                    break;
                case "SPECIFIC":
                    DateTime theDate = DateTime.Now;
                    if (DateTime.TryParse(SpecificDatePicker.SelectedDate.ToString(), out theDate) == true)
                    {
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSpoiledBallotDetailsReport(
                            AppSettings.Election.ElectionID,
                            _PollID,
                            theDate));
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Date");
                    }
                    break;
                case "RANGE":
                    DateTime startdate = DateTime.Now;
                    DateTime enddate = DateTime.Now.AddDays(1);
                    if (DateTime.TryParse(BeginningDatePicker.SelectedDate.ToString(), out startdate) == true)
                    {
                        if (DateTime.TryParse(EndingDatePicker.SelectedDate.ToString(), out enddate) == true)
                        {
                            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintSpoiledBallotDetailsReport(
                                AppSettings.Election.ElectionID,
                                _PollID,
                                startdate,
                                enddate));
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextCenter = ("Invalid Ending Date");
                        }
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Beginning Date");
                    }
                    break;
            }
        }

        private void PrintProvisionalBallotsReport()
        {
            switch (_DateOptions)
            {
                case "ALL":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintProvisionalDetailsReport(
                        AppSettings.Election.ElectionID,
                        _PollID));
                    break;
                case "TODAY":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintProvisionalDetailsReport(
                        AppSettings.Election.ElectionID,
                        _PollID,
                        DateTime.Now));
                    break;
                case "SPECIFIC":
                    DateTime theDate = DateTime.Now;
                    if (DateTime.TryParse(SpecificDatePicker.SelectedDate.ToString(), out theDate) == true)
                    {
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintProvisionalDetailsReport(
                            AppSettings.Election.ElectionID,
                            _PollID,
                            theDate));
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Date");
                    }
                    break;
                case "RANGE":
                    DateTime startdate = DateTime.Now;
                    DateTime enddate = DateTime.Now.AddDays(1);
                    if (DateTime.TryParse(BeginningDatePicker.SelectedDate.ToString(), out startdate) == true)
                    {
                        if (DateTime.TryParse(EndingDatePicker.SelectedDate.ToString(), out enddate) == true)
                        {
                            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintProvisionalDetailsReport(
                                AppSettings.Election.ElectionID,
                                _PollID,
                                startdate,
                                enddate));
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextCenter = ("Invalid Ending Date");
                        }
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Beginning Date");
                    }
                    break;
            }
        }

        private void PrintUnsignedEnvelopesReport()
        {
            switch (_DateOptions)
            {
                case "ALL":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintUnsignedDetailsReport(
                        AppSettings.Election.ElectionID,
                        _PollID));
                    break;
                case "TODAY":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintUnsignedDetailsReport(
                        AppSettings.Election.ElectionID,
                        _PollID,
                        DateTime.Now));
                    break;
                case "SPECIFIC":
                    DateTime theDate = DateTime.Now;
                    if (DateTime.TryParse(SpecificDatePicker.SelectedDate.ToString(), out theDate) == true)
                    {
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintUnsignedDetailsReport(
                            AppSettings.Election.ElectionID,
                            _PollID,
                            theDate));
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Date");
                    }
                    break;
                case "RANGE":
                    DateTime startdate = DateTime.Now;
                    DateTime enddate = DateTime.Now.AddDays(1);
                    if (DateTime.TryParse(BeginningDatePicker.SelectedDate.ToString(), out startdate) == true)
                    {
                        if (DateTime.TryParse(EndingDatePicker.SelectedDate.ToString(), out enddate) == true)
                        {
                            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintUnsignedDetailsReport(
                                AppSettings.Election.ElectionID,
                                _PollID,
                                startdate,
                                enddate));
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextCenter = ("Invalid Ending Date");
                        }
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Beginning Date");
                    }
                    break;
            }
        }

        private void PrintRejectedApplicationsReport()
        {
            switch (_DateOptions)
            {
                case "ALL":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintRejectedApplicationDetailsReport(
                        AppSettings.Election.ElectionID,
                        _PollID));
                    break;
                case "TODAY":
                    GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintRejectedApplicationDetailsReport(
                        AppSettings.Election.ElectionID,
                        _PollID,
                        DateTime.Now));
                    break;
                case "SPECIFIC":
                    DateTime theDate = DateTime.Now;
                    if (DateTime.TryParse(SpecificDatePicker.SelectedDate.ToString(), out theDate) == true)
                    {
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintRejectedApplicationDetailsReport(
                            AppSettings.Election.ElectionID,
                            _PollID,
                            theDate));
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Date");
                    }
                    break;
                case "RANGE":
                    DateTime startdate = DateTime.Now;
                    DateTime enddate = DateTime.Now.AddDays(1);
                    if (DateTime.TryParse(BeginningDatePicker.SelectedDate.ToString(), out startdate) == true)
                    {
                        if (DateTime.TryParse(EndingDatePicker.SelectedDate.ToString(), out enddate) == true)
                        {
                            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintRejectedApplicationDetailsReport(
                                AppSettings.Election.ElectionID,
                                _PollID,
                                startdate,
                                enddate));
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextCenter = ("Invalid Ending Date");
                        }
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Beginning Date");
                    }
                    break;
            }
        }

        private void PickedDate_Loaded(object sender, RoutedEventArgs e)
        {
            //PickedDate.SelectedDate = DateTime.Now;
        }

        private void PrintReportObjectButton_Click(object sender, RoutedEventArgs e)
        {
            // Get selected item
            var item = (System.Windows.Controls.ComboBoxItem)ReportList.SelectedItem;

            // Convert selected item into Reporting Object
            var report = (IAbsenteeReporting)item.DataContext;
            //StatusBar.ApplicationStatus(report.PrintReport(AppSettings.Election.ElectionID, _PollID));

            // Check which date option is selected
            switch (_DateOptions)
            {
                // Activity to Date
                case "ALL":
                    GlobalReferences.StatusBar.TextCenter = (report.PrintReport(
                        AppSettings.Election.ElectionID,
                        _PollID));
                    break;

                // Todays activity only
                case "TODAY":
                    GlobalReferences.StatusBar.TextCenter = (report.PrintReport(
                        AppSettings.Election.ElectionID,
                        _PollID,
                        DateTime.Now));
                    break;

                // Selected a specific date
                case "SPECIFIC":
                    // Initialize the date
                    DateTime theDate = DateTime.Now;

                    // Vaidate the given date
                    if (DateTime.TryParse(SpecificDatePicker.SelectedDate.ToString(), out theDate) == true)
                    {
                        GlobalReferences.StatusBar.TextCenter = (report.PrintReport(
                            AppSettings.Election.ElectionID,
                            _PollID,
                            theDate));
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Date");
                    }
                    break;

                // Full date range
                case "RANGE":
                    // Initialize the start and end dates
                    DateTime startdate = DateTime.Now;
                    DateTime enddate = DateTime.Now.AddDays(1);

                    // Validate starting date
                    if (DateTime.TryParse(BeginningDatePicker.SelectedDate.ToString(), out startdate) == true)
                    {
                        // Validate ending date
                        if (DateTime.TryParse(EndingDatePicker.SelectedDate.ToString(), out enddate) == true)
                        {
                            GlobalReferences.StatusBar.TextCenter = (report.PrintReport(
                                AppSettings.Election.ElectionID,
                                _PollID,
                                startdate,
                                enddate));
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextCenter = ("Invalid Ending Date");
                        }
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Beginning Date");
                    }
                    break;
            }
        }

        private void PreviewReportObjectButton_Click(object sender, RoutedEventArgs e)
        {
            // Get selected item
            var item = (System.Windows.Controls.ComboBoxItem)ReportList.SelectedItem;

            // Convert selected item into Reporting Object
            var report = (IAbsenteeReporting)item.DataContext;
            //StatusBar.ApplicationStatus(report.PrintReport(AppSettings.Election.ElectionID, _PollID));

            // Check which date option is selected
            switch (_DateOptions)
            {
                // Activity to Date
                case "ALL":
                    report.PreviewReport(
                        AppSettings.Election.ElectionID,
                        _PollID);
                    break;

                // Todays activity only
                case "TODAY":
                    report.PreviewReport(
                        AppSettings.Election.ElectionID,
                        _PollID,
                        DateTime.Now);
                    break;

                // Selected a specific date
                case "SPECIFIC":
                    // Initialize the date
                    DateTime theDate = DateTime.Now;

                    // Vaidate the given date
                    if (DateTime.TryParse(SpecificDatePicker.SelectedDate.ToString(), out theDate) == true)
                    {
                        report.PreviewReport(
                            AppSettings.Election.ElectionID,
                            _PollID,
                            theDate);
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Date");
                    }
                    break;

                // Full date range
                case "RANGE":
                    // Initialize the start and end dates
                    DateTime startdate = DateTime.Now;
                    DateTime enddate = DateTime.Now.AddDays(1);

                    // Validate starting date
                    if (DateTime.TryParse(BeginningDatePicker.SelectedDate.ToString(), out startdate) == true)
                    {
                        // Validate ending date
                        if (DateTime.TryParse(EndingDatePicker.SelectedDate.ToString(), out enddate) == true)
                        {
                            report.PreviewReport(
                                AppSettings.Election.ElectionID,
                                _PollID,
                                startdate,
                                enddate);
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextCenter = ("Invalid Ending Date");
                        }
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Invalid Beginning Date");
                    }
                    break;
            }
        }

        private void PrintDailyDetailBySiteReport(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            sql += "FROM vDailyDetailReportPrimary ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";            

            ReportPreviewWindow previewReport = new ReportPreviewWindow("AbsenteeRosterBySite01", sql, true, "DailyDetailData");
            previewReport.ShowDialog();
        }
    }
}
