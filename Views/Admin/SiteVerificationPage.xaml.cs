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
using VoterX.Extensions;
using VoterX.Methods;
using VoterX.Dialogs;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AdministrationPage.xaml
    /// </summary>
    public partial class SiteVerificationPage : Page
    {
        public SiteVerificationPage()
        {
            InitializeComponent();

            GlobalReferences.Header.PageHeader = ("Site Verification");

            //StatusBar.ApplicationStatusClear();
            GlobalReferences.StatusBar.TextClear();

            SiteName.Text = AppSettings.Absentee.SiteName;
            ComputerNumber.Text = AppSettings.System.MachineID.ToString();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new AbsenteeLoginPage());
        }

        private async void ZeroReportButton_Click(object sender, RoutedEventArgs e)
        {
            ZeroReportButton.Visibility = Visibility.Collapsed;
            ZeroReportPrinterCheckPanel.Visibility = Visibility.Visible;
            GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => ReportPrintingMethods.PrintZeroEarlyVotingBSReport(112, 49)));
        }

        private void ZeroReportPrinterCheckQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (ZeroReportPrinterCheckQuestion.GetAnswer() == true)
            {
                BallotTestPanel.Visibility = Visibility.Visible;
                ZeroReportTestPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Display Not Verified Message
                AlertDialog message = new AlertDialog("Make sure the printer is connected and setup correctly.\r\n\r\nFor further assistance call technical support.");
                if (message.ShowDialog() == true)
                {
                    this.NavigateToPage(new AbsenteeLoginPage());
                }
            }
        }

        private async void TestBallotButton_Click(object sender, RoutedEventArgs e)
        {
            TestBallotButton.Visibility = Visibility.Collapsed;
            TestBallotPrinterCheckPanel.Visibility = Visibility.Visible;
            GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => BallotPrinting.PrintTestBallot()));
        }

        private void TestBallotPrinterCheckQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (TestBallotPrinterCheckQuestion.GetAnswer() == true)
            {
                AppSettings.System.SiteVerified = true;

                // Write system settings to the file
                AppSettings.SaveChanges();

                BallotTestPanel.Visibility = Visibility.Collapsed;
                VerifiedSitePanel.Visibility = Visibility.Visible;
                IntroTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                try
                {
                    // Display Not Verified Message
                    AlertDialog message = new AlertDialog("Make sure the printer is connected and setup correctly.\r\n\r\nFor further assistance call technical support.");
                    if (message.ShowDialog() == true)
                    {
                        this.NavigateToPage(new AbsenteeLoginPage());
                    }
                }
                catch (Exception error)
                {
                    GlobalReferences.StatusBar.TextCenter = (error.Message);
                }
            }
        }

        private void SiteNameCheckQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if (SiteNameCheckQuestion.GetAnswer() == true)
            {
                ZeroReportTestPanel.Visibility = Visibility.Visible;
                SiteNameTestPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                AlertDialog message = new AlertDialog("If this site is incorrect please contact technical support.");
                if (message.ShowDialog() == true)
                {
                    this.NavigateToPage(new AbsenteeLoginPage());
                }
            }
        }
    }
}
