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
using System.Diagnostics;
using VoterX.Dialogs;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for EndofDayPage.xaml
    /// </summary>
    public partial class EndofDayPage : Page
    {
        public EndofDayPage()
        {
            InitializeComponent();

            GlobalReferences.Header.PageHeader = ("End of Day");

            //StatusBar.ApplicationStatusClear();
            GlobalReferences.StatusBar.TextClear();

            //if (((App)Application.Current).debugMode == false)
            //{
            //PrintDailyReports();
            //}
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            //SaveLogout();
            this.NavigateToPage(new AbsenteeLoginPage());
        }

        private void SaveLogout()
        {
            GlobalReferences.StatusBar.TextLeft = ("Logging In");
            //StatusBar.LoadingSpinner(Visibility.Visible);
            GlobalReferences.StatusBar.SpinnerVisibility = true;
            //if (await Task.Run(() => PollSummaryMethods.SaveLogout()) == false) StatusBar.ApplicationStatusCenter("Logout not saved");
            GlobalReferences.StatusBar.TextLeft = ("");
            //StatusBar.LoadingSpinner(Visibility.Collapsed);
            GlobalReferences.StatusBar.SpinnerVisibility = false;
        }

        private async void PrintDailyReports()
        {
            // Print todays reports 
            GlobalReferences.StatusBar.TextCenter = (await Task.Run(()=>ReportPrintingMethods.PrintDailySummaryReport(DateTime.Now)));
            // Print todays spoiled
            GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => ReportPrintingMethods.PrintDailySpoiledGeneralReport(AppSettings.Election.ElectionID, (int)AppSettings.System.SiteID, DateTime.Now)));
            // Print todays provisional
            GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => ReportPrintingMethods.PrintDailyProvisionalReport(AppSettings.Election.ElectionID, (int)AppSettings.System.SiteID, DateTime.Now)));
            // Print todays voter details
            GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => ReportPrintingMethods.PrintDailyDetailBSReportGeneral((int)AppSettings.System.SiteID, DateTime.Now)));
        }

        private void DailyReportPrintQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (DailyReportPrintQuestion.GetAnswer() == true)
            {
                // Show spoiled report question grid
                SpoiledReportTroubleshootingGrid.Show();
            }
            else
            {
                DailyReportPrinterCheckPanel.Show();
            }
        }

        private void DailyReportPrinterCheckQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (DailyReportPrinterCheckQuestion.GetAnswer() == true)
            {
                DailyReportPrintQuestion.ChangeAnswer(true);

                // Hide Report Check Panel
                DailyReportPrinterCheckPanel.Hide();

                // Show spoiled report question grid
                SpoiledReportTroubleshootingGrid.Show();
            }
            else
            {
                DailyReportOptionsPanel.Show();
            }
        }

        private void TransferClerkCheck_Click(object sender, RoutedEventArgs e)
        {
            LogoutButton.Visibility = Visibility.Visible;
        }

        private void SpoiledReportPrintQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (SpoiledReportPrintQuestion.GetAnswer() == true)
            {
                // Show provisional report question
                ProvisionalReportTroubleshootingGrid.Show();
            }
            else
            {
                DailyReportTroubleshootingGrid.Hide();

                // Show trouble shooting question
                SpoiledReportPrinterCheckPanel.Show();
            }
        }

        private void SpoiledReportPrinterCheckQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (SpoiledReportPrinterCheckQuestion.GetAnswer() == true)
            {
                SpoiledReportPrintQuestion.ChangeAnswer(true);

                // Hide Report Check Panel
                SpoiledReportPrinterCheckPanel.Hide();

                DailyReportTroubleshootingGrid.Hide();

                // Show provisional report question grid
                ProvisionalReportTroubleshootingGrid.Show();
            }
            else
            {
                SpoiledReportOptionsPanel.Show();
            }
        }

        private void ProvisionalReportPrintQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (ProvisionalReportPrintQuestion.GetAnswer() == true)
            {
                // Show daily report question
                DetailReportTroubleshootingGrid.Visibility = Visibility.Visible;
            }
            else
            {
                DailyReportTroubleshootingGrid.Hide();
                SpoiledReportTroubleshootingGrid.Hide();

                // Show trouble shooting question
                ProvisionalReportPrinterCheckPanel.Show();
            }
        }

        private void ProvisionalReportPrinterCheckQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (ProvisionalReportPrinterCheckQuestion.GetAnswer() == true)
            {
                ProvisionalReportPrintQuestion.ChangeAnswer(true);

                // Hide Report Check Panel
                ProvisionalReportPrinterCheckPanel.Hide();

                // Show reconcile button
                //ReconcileButton.Visibility = Visibility.Visible;

                // Hide the other questions
                //DailyReportTroubleshootingGrid.Hide();
                //SpoiledReportTroubleshootingGrid.Hide();
                //ProvisionalReportTroubleshootingGrid.Hide();

                DetailReportTroubleshootingGrid.Show();
            }
            else
            {
                // Hide the other questions
                DailyReportTroubleshootingGrid.Hide();
                SpoiledReportTroubleshootingGrid.Hide();

                ProvisionalReportOptionsPanel.Show();
            }
        }

        private async void ReconcileButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide the other questions
            DailyReportTroubleshootingGrid.Hide();
            SpoiledReportTroubleshootingGrid.Hide();
            ProvisionalReportTroubleshootingGrid.Hide();

            // Activate Reconsile Application
            // https://stackoverflow.com/questions/1585354/get-return-value-from-process

            int result = await Task.Run(() => {
                Process P = Process.Start("C:\\Program Files\\VoterX\\VoterXReconcile.exe");
                P.WaitForExit();
                return P.ExitCode;
            });

            if (result == 0)
            {
                //StatusBar.StatusTextLeft = result.ToString();

                // Print End of Day report twice
                GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintEndOfDayReport(DateTime.Now));
                // Bonnie requested a second copy be printed 7/20/2018
                //StatusBar.ApplicationStatusCenter(ReportPrintingMethods.PrintEndOfDayReport(DateTime.Now));

                // Show end of day print check
                EODReportTroubleshootingGrid.Show();
            }
            else
            {
                //StatusBar.ApplicationStatusCenter("Reconcile was not complete: Exit Code(" + result.ToString() + ")");
                LogoutButton.Visibility = Visibility.Visible;
            }
        }

        private void EODReportPrintQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (EODReportPrintQuestion.GetAnswer() == true)
            {
                // Finish Reconcile and log out
                // popup "Have a nice day"
                FinishEndOfDay();
            }
            else
            {
                // Show trouble shooting question
                EODReportPrinterCheckPanel.Show();
            }
        }

        private void EODReportPrinterCheckQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (EODReportPrinterCheckQuestion.GetAnswer() == true)
            {
                EODReportPrintQuestion.ChangeAnswer(true);

                // Hide Report Check Panel
                EODReportPrinterCheckPanel.Hide();

                // Finish Reconcile and log out
                // popup "Have a nice day"
                FinishEndOfDay();
            }
            else
            {
                EODReportOptionsPanel.Show();
            }
        }

        private void FinishEndOfDay()
        {
            // Display message
            AlertDialog signatureDialog = new AlertDialog("YOU HAVE SUCCESSFULLY COMPLETED THE END OF DAY PROCESS.");
            if (signatureDialog.ShowDialog() == true)
            {
                //this.NavigateToPage(new AbsenteeLoginPage());
                PrintEODReport.Visibility = Visibility.Visible;
                LogoutButton.Visibility = Visibility.Visible;
            }
        }

        private void DetailReportPrintQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (DetailReportPrintQuestion.GetAnswer() == true)
            {
                // Hide the other questions
                DailyReportTroubleshootingGrid.Hide();
                SpoiledReportTroubleshootingGrid.Hide();
                ProvisionalReportTroubleshootingGrid.Hide();
                DetailReportTroubleshootingGrid.Hide();

                // Show daily report question
                ReconcileButton.Visibility = Visibility.Visible;
            }
            else
            {
                // Hide the other questions
                DailyReportTroubleshootingGrid.Hide();
                SpoiledReportTroubleshootingGrid.Hide();
                ProvisionalReportTroubleshootingGrid.Hide();

                // Show trouble shooting question
                DetailReportPrinterCheckPanel.Show();
            }
        }

        private void DetailReportPrinterCheckQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (DetailReportPrinterCheckQuestion.GetAnswer() == true)
            {
                DetailReportPrintQuestion.ChangeAnswer(true);

                // Hide Report Check Panel
                DetailReportTroubleshootingGrid.Hide();

                // Show reconcile button
                ReconcileButton.Visibility = Visibility.Visible;
            }
            else
            {
                DetailReportOptionsPanel.Show();
            }
        }

        private void StartEndOfDayButton_Click(object sender, RoutedEventArgs e)
        {
            YesNoDialog signatureDialog = new YesNoDialog("End of Day","ARE YOU SURE YOU WANT TO START THE\r\nEND OF DAY PROCESS?");
            if (signatureDialog.ShowDialog() == true)
            {
                StartEndOfDayButton.Visibility = Visibility.Collapsed;

                DailyReportPrintingStatus.Visibility = Visibility.Visible;
                DailyReportPrintQuestion.Visibility = Visibility.Visible;

                PrintDailyReports();                
            }
            else
            {
                SaveLogout();
                this.NavigateToPage(new AbsenteeLoginPage());
            }
        }

        private void PrintEODReport_Click(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintEndOfDayReport(DateTime.Now));
        }
    }
}
