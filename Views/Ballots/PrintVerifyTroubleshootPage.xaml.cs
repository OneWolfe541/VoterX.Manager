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
//using VoterX.Core.Models.ViewModels;
using VoterX.Methods;
using System.Windows.Controls.Primitives;
using FontAwesome.WPF;
using VoterX.SystemSettings.Extensions;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for PrintVerifyTroubleshootPage.xaml
    /// </summary>
    public partial class PrintVerifyTroubleshootPage : Page
    {
        //private VoterDataModel _voter = new VoterDataModel();

        private int printingErrors = 0;

        private bool exitLogout = false;

        //public PrintVerifyTroubleshootPage(VoterDataModel voter)
        //{
        //    InitializeComponent();
        //    _voter = voter;
        //    VoterX.Methods.StatusBar.ApplicationPageHeader("Print Verification");
        //}

        //public PrintVerifyTroubleshootPage(VoterDataModel voter, bool returnStatus)
        //{
        //    InitializeComponent();
        //    _voter = voter;
        //    exitLogout = returnStatus;
        //    if (returnStatus == true)
        //    {
        //        BackReturnLabel.Visibility = Visibility.Collapsed;
        //        BackLogoutLabel.Visibility = Visibility.Visible;
        //    }
        //    VoterX.Methods.StatusBar.ApplicationPageHeader("Print Verification");
        //}

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new EditBallotSearchPage());
            }
        }

        private void NextVerifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new EditBallotSearchPage());
            }
        }

        private void ResetQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            printingErrors = 0;

            // Hide return to search button
            NextVerify.Visibility = Visibility.Collapsed;

            // Rest Ballot Questions
            BallotPrinterCheckPanel.Visibility = Visibility.Collapsed;
            BallotOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintBallotCheck.IsChecked = false;
            TransferVoterCheck.IsChecked = false;

            // Reset Application Questions 
            ApplicationOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintApplicationCheck.IsChecked = false;
            ApplicationTransferVoterCheck.IsChecked = false;

            // Reset Permit Questions
            PermitOptionsPanel.Visibility = Visibility.Collapsed;
            PermitReprintCheck.IsChecked = false;
            PermitTransferVoterCheck.IsChecked = false;

            // Reset Stub Questions
            StubOptionsPanel.Visibility = Visibility.Collapsed;
            StubReprintCheck.IsChecked = false;
            StubTransferVoterCheck.IsChecked = false;
        }

        // Find all children of tpye<t> sample
        // https://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type
        //private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        //{
        //    if (depObj != null)
        //    {
        //        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        //        {
        //            DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
        //            if (child != null && child is T)
        //            {
        //                yield return (T)child;
        //            }

        //            foreach (T childOfChild in FindVisualChildren<T>(child))
        //            {
        //                yield return childOfChild;
        //            }
        //        }
        //    }
        //}

        private void BallotPrintQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            // When Answer is True = Yes was clicked
            if (BallotPrintQuestion.GetAnswer() == true)
            {
                if (AppSettings.Election.IsElectionDay())
                {
                    PermitPrintQuestion.Visibility = Visibility.Visible;
                }
                else
                {
                    //ApplicationVerificationPanel.Visibility = Visibility.Visible;
                    ApplicationPrintQuestion.Visibility = Visibility.Visible;
                }
            }
            // Answer False = No was clicked
            else
            {
                if (printingErrors == 0)
                {
                    // Show the troubleshooting message
                    BallotPrinterCheckPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    NextVerify.Visibility = Visibility.Visible;
                }
                printingErrors++;
            }
                
        }

        private void BallotPrinterCheckQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if(BallotPrinterCheckQuestion.GetAnswer() == true)
            {
                // reset printer check
                BallotPrintQuestion.ChangeAnswer(true);

                // hide self
                BallotPrinterCheckPanel.Visibility = Visibility.Collapsed;
                BallotPrinterCheckQuestion.Visibility = Visibility.Collapsed;
                BallotPrinterCheckQuestion.Reset();

                // show the next question
                if (AppSettings.Election.IsElectionDay())
                //if(true)
                {
                    PermitPrintQuestion.Visibility = Visibility.Visible;
                }
                else
                {
                    ApplicationPrintQuestion.Visibility = Visibility.Visible;
                }
            }
            else
            {
                // Display options panel
                BallotOptionsPanel.Visibility = Visibility.Visible;
            }
        }

        private void ReprintBallotCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck the other box
            TransferVoterCheck.IsChecked = false;

            // Print a new ballot
            //await Task.Run(() => BallotPrinting.ReprintBallot(_voter));

            // Spoil previous ballot with "print error" reason
            //VoterMethods.InsertSpoiledBallot(_voter, 1);

            // Get new ballot number
            //_voter.BallotNumber = VoterMethods.Voter.GetNextBallotNumber((int)AppSettings.System.SiteID);
            // Store new ballot number in voted record
            //VoterMethods.UpdateVoterBallotNumber(_voter);

            // Display new printing message
            BallotPrintingStatus.Text = "A NEW BALLOT WAS SENT TO THE PRINTER";

            // Reset and hide questions
            BallotPrintQuestion.Reset();
            BallotPrinterCheckQuestion.Reset();
            BallotPrinterCheckPanel.Visibility = Visibility.Collapsed;
            BallotOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintBallotCheck.IsChecked = false;
            TransferVoterCheck.IsChecked = false;
        }

        private void TransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            ReprintBallotCheck.IsChecked = false;
            // Return to search screen or logout
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new AdministrationPage());
            }
        }

        private void ApplicationPrintQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if(ApplicationPrintQuestion.GetAnswer() == true)
            {
                // Check for Stub
                if (AppSettings.System.BallotStub == 1)
                {
                    // Display Stub Question
                    StubPrintQuestion.Visibility = Visibility.Visible;
                }
                else
                {
                    // Display Return to search button
                    NextVerify.Visibility = Visibility.Visible;
                }
            }
            else
            {
                // Show the troubleshooting message
                ApplicationOptionsPanel.Visibility = Visibility.Visible;
            }
        }

        private void ReprintApplicationCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck the other box
            TransferVoterCheck.IsChecked = false;

            // Print a new application
            //await Task.Run(() => BallotPrinting.ReprintApplication(_voter));

            // Display new printing message
            //BallotPrintingStatus.Text = "A new Ballot was sent to the Printer";

            // Reset and hide questions
            ApplicationPrintQuestion.Reset();
            ApplicationOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintApplicationCheck.IsChecked = false;
            ApplicationTransferVoterCheck.IsChecked = false;
        }

        private void ApplicationTransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            ReprintApplicationCheck.IsChecked = false;
            // Return to search screen
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new AdministrationPage());
            }
        }

        private void PermitPrintQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if(PermitPrintQuestion.GetAnswer() == true)
            {
                // Check for Stub
                if (AppSettings.System.BallotStub == 1)
                {
                    // Display Stub Question
                    StubPrintQuestion.Visibility = Visibility.Visible;
                }
                else
                {
                    // Display Return to search button
                    NextVerify.Visibility = Visibility.Visible;
                }
            }
            else
            {
                // Display Troubleshooting message & question
                PermitOptionsPanel.Visibility = Visibility.Visible;
            }
        }

        private void PermitReprintCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck the other box
            PermitTransferVoterCheck.IsChecked = false;

            // Print a new application
            //await Task.Run(() => BallotPrinting.ReprintPermit(_voter));

            // Display new printing message
            //BallotPrintingStatus.Text = "A new Ballot was sent to the Printer";

            // Reset and hide questions
            PermitPrintQuestion.Reset();
            PermitOptionsPanel.Visibility = Visibility.Collapsed;
            PermitReprintCheck.IsChecked = false;
            PermitTransferVoterCheck.IsChecked = false;
        }

        private void PermitTransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            ReprintApplicationCheck.IsChecked = false;
            // Return to search screen
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new AdministrationPage());
            }
        }

        private void StubPrintQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            if(StubPrintQuestion.GetAnswer() == true)
            {
                // Display Return to search button
                NextVerify.Visibility = Visibility.Visible;
            }
            else
            {
                // Display Troubleshooting message & question
                StubOptionsPanel.Visibility = Visibility.Visible;
            }
        }

        private void StubReprintCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck the other box
            StubTransferVoterCheck.IsChecked = false;

            // Print a new application
            //await Task.Run(() => BallotPrinting.ReprintStub(_voter));

            // Display new printing message
            //BallotPrintingStatus.Text = "A new Ballot was sent to the Printer";

            // Reset and hide questions
            StubPrintQuestion.Reset();
            StubOptionsPanel.Visibility = Visibility.Collapsed;
            StubReprintCheck.IsChecked = false;
            StubTransferVoterCheck.IsChecked = false;
        }

        private void StubTransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            ReprintApplicationCheck.IsChecked = false;
            // Return to search screen
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new AdministrationPage());
            }
        }        
    }
}
