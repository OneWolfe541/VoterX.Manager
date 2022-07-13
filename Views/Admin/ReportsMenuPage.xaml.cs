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
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for ReportsMenuPage.xaml
    /// </summary>
    public partial class ReportsMenuPage : Page
    {
        public ReportsMenuPage()
        {
            InitializeComponent();

            // Display page header
            GlobalReferences.Header.PageHeader = ("Reports");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new AdministrationPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new LoginPage());
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
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailyDetailBCReport(49, DateTime.Now));
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
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintDailySpoiledPrimaryReport(112, 49, DateTime.Now));
        }
    }
}
