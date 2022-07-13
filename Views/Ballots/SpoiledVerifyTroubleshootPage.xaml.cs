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
//using VoterX.Core.Models.ViewModels;
using VoterX.Extensions;
//using VoterX.Core.Models.Database;
using System.Windows.Controls.Primitives;
using VoterX.SystemSettings.Extensions;
using FontAwesome.WPF;
using VoterX.Core.Voters;
using VoterX.Manager.Views.Absentee;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for SpoiledVerifyTroubleshootPage.xaml
    /// </summary>
    public partial class SpoiledVerifyTroubleshootPage : Page
    {
        private NMVoter _voter = new NMVoter();

        private int printingErrors = 0;

        private bool exitLogout = false;

        public SpoiledVerifyTroubleshootPage(NMVoter voter)
        {
            InitializeComponent();
            _voter = voter;
            GlobalReferences.Header.PageHeader = ("Spoiled Print Verification");

            if (AppSettings.Absentee.TestScreen == true)
            {
                ReturnLabel.Text = "RETURN TO VOTER";
            }
        }

        public SpoiledVerifyTroubleshootPage(NMVoter voter, bool returnStatus)
        {
            InitializeComponent();
            _voter = voter;
            exitLogout = returnStatus;
            if (returnStatus == true)
            {
                //VoterX.Methods.StatusBar.ApplicationStatusCenter("Admin");
                BackReturnLabel.Visibility = Visibility.Collapsed;
                BackLogoutLabel.Visibility = Visibility.Visible;
            }
            GlobalReferences.Header.PageHeader = ("Spoiled Print Verification");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (exitLogout == false)
            {
                if (AppSettings.Absentee.TestScreen == true)
                {
                    // Check for SERVIS lookup
                    bool SERVISlookup = false;
                    if (_voter.Data.PollID == 0) SERVISlookup = true;

                    this.NavigateToPage(new VoterDetailsPage(_voter, SERVISlookup));
                }
                else
                {
                    this.NavigateToPage(new SearchPage());
                }
            }
            else
            {
                this.NavigateToPage(new SettingsPage());
            }
        }

        private void NextVerifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (exitLogout == false)
            {
                if (AppSettings.Absentee.TestScreen == true)
                {
                    // Check for SERVIS lookup
                    bool SERVISlookup = false;
                    if (_voter.Data.PollID == 0) SERVISlookup = true;

                    this.NavigateToPage(new VoterDetailsPage(_voter, SERVISlookup));
                }
                else
                {
                    this.NavigateToPage(new SearchPage());
                }
            }
            else
            {
                this.NavigateToPage(new SettingsPage());
            }
        }

        private void ResetQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            printingErrors = 0;

            // Hide return to search button
            NextVerify.Visibility = Visibility.Collapsed;

            // Rest Ballot Questions
            ClearYesNoPair(BallotValidationYes, BallotValidationNo);
            ClearYesNoPair(BallotPrinterCheckYes, BallotPrinterCheckNo);
            BallotPrinterCheckPanel.Visibility = Visibility.Collapsed;
            BallotOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintBallotCheck.IsChecked = false;
            TransferVoterCheck.IsChecked = false;

            // Reset Permit Questions
            ClearYesNoPair(PermitValidationYes, PermitValidationNo);
            //ClearYesNoPair(PermitPrinterCheckYes, PermitPrinterCheckNo);
            PermitVerificationPanel.Visibility = Visibility.Collapsed;
            //PermitPrinterCheckPanel.Visibility = Visibility.Collapsed;
            PermitOptionsPanel.Visibility = Visibility.Collapsed;
            PermitReprintCheck.IsChecked = false;
            PermitTransferVoterCheck.IsChecked = false;

            // Reset Stub Questions
            ClearYesNoPair(StubValidationYes, StubValidationNo);
            //ClearYesNoPair(StubPrinterCheckYes, StubPrinterCheckNo);
            StubVerificationPanel.Visibility = Visibility.Collapsed;
            //StubPrinterCheckPanel.Visibility = Visibility.Collapsed;
            StubOptionsPanel.Visibility = Visibility.Collapsed;
            StubReprintCheck.IsChecked = false;
            StubTransferVoterCheck.IsChecked = false;
        }

        private void CheckYesNoPair(ToggleButton buttonToCheck, ToggleButton buttonToUncheck)
        {
            // Set checked button
            buttonToCheck.IsChecked = true;
            buttonToCheck.IsEnabled = false;

            var childListChecked = FindVisualChildren<ImageAwesome>(buttonToCheck);
            var iconChecked = childListChecked.FirstOrDefault();
            if (iconChecked != null)
            {
                iconChecked.Visibility = Visibility.Visible;
            }

            // Set unchecked button
            buttonToUncheck.IsChecked = false;
            buttonToUncheck.IsEnabled = false;

            var childListUnchecked = FindVisualChildren<ImageAwesome>(buttonToUncheck);
            var iconUnchecked = childListUnchecked.FirstOrDefault();
            if (iconUnchecked != null)
            {
                iconUnchecked.Visibility = Visibility.Hidden;
            }
        }

        private void ClearYesNoPair(ToggleButton buttonYes, ToggleButton buttonNo)
        {
            // Set checked button
            buttonYes.IsChecked = false;
            buttonYes.IsEnabled = true;

            var childListYes = FindVisualChildren<ImageAwesome>(buttonYes);
            var iconYes = childListYes.FirstOrDefault();
            if (iconYes != null)
            {
                iconYes.Visibility = Visibility.Hidden;
            }

            // Set unchecked button
            buttonNo.IsChecked = false;
            buttonNo.IsEnabled = true;

            var childListNo = FindVisualChildren<ImageAwesome>(buttonNo);
            var iconNo = childListNo.FirstOrDefault();
            if (iconNo != null)
            {
                iconNo.Visibility = Visibility.Hidden;
            }
        }

        // Find all children of tpye<t> sample
        // https://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void BallotValidationYes_Click(object sender, RoutedEventArgs e)
        {
            // Set Yes to checked and No to Disabled
            CheckYesNoPair(BallotValidationYes, BallotValidationNo);

            // Show the Next Question
            if (AppSettings.Election.IsElectionDay())
            //if (true)
            {
                PermitVerificationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                NextVerify.Visibility = Visibility.Visible;
            }
        }

        private void BallotValidationNo_Click(object sender, RoutedEventArgs e)
        {
            // Set No to checked and Yes to Disabled
            CheckYesNoPair(BallotValidationNo, BallotValidationYes);

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

        private void BallotPrinterCheckYes_Click(object sender, RoutedEventArgs e)
        {
            // Set Yes to checked and No to Disabled
            CheckYesNoPair(BallotPrinterCheckYes, BallotPrinterCheckNo);

            // Reset Printer Check
            ClearYesNoPair(BallotValidationYes, BallotValidationNo);
            ClearYesNoPair(BallotPrinterCheckYes, BallotPrinterCheckNo);
            BallotPrinterCheckPanel.Visibility = Visibility.Collapsed;
            BallotOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintBallotCheck.IsChecked = false;
            TransferVoterCheck.IsChecked = false;
            CheckYesNoPair(BallotValidationYes, BallotValidationNo);

            // Show the Next Question
            if (AppSettings.Election.IsElectionDay())
            //if(true)
            {
                PermitVerificationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                NextVerify.Visibility = Visibility.Visible;
            }
        }

        private void BallotPrinterCheckNo_Click(object sender, RoutedEventArgs e)
        {
            // Set No to checked and Yes to Disabled
            CheckYesNoPair(BallotPrinterCheckNo, BallotPrinterCheckYes);
            // Display options panel
            BallotOptionsPanel.Visibility = Visibility.Visible;
        }

        private async void ReprintBallotCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck the other box
            TransferVoterCheck.IsChecked = false;

            // Print a new ballot
            await Task.Run(() => BallotPrinting.ReprintBallot(_voter.Data));

            // Spoil previous ballot with "print error" reason
            //VoterMethods.InsertSpoiledBallot(_voter, 1);
            _voter.SpoilBallot(1); // Reason Code 1 is Printer Error

            // Get new ballot number
            //_voter.BallotNumber = VoterMethods.Voter.GetNextBallotNumber((int)AppSettings.System.SiteID);
            _voter.GetNextBallotNumber((int)AppSettings.System.SiteID);
            // Store new ballot number in voted record
            //VoterMethods.UpdateVoterBallotNumber(_voter);
            _voter.UpdateBallotNumber();

            // Display new printing message
            BallotPrintingStatus.Text = "A new Ballot was sent to the Printer";

            // Reset and hide questions
            ClearYesNoPair(BallotValidationYes, BallotValidationNo);
            ClearYesNoPair(BallotPrinterCheckYes, BallotPrinterCheckNo);
            BallotPrinterCheckPanel.Visibility = Visibility.Collapsed;
            BallotOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintBallotCheck.IsChecked = false;
            TransferVoterCheck.IsChecked = false;
        }

        private void TransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            ReprintBallotCheck.IsChecked = false;
            // Return to search screen
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new SettingsPage());
            }
        }

        private void PermitValidationYes_Click(object sender, RoutedEventArgs e)
        {
            // Set Yes to checked and No to Disabled
            CheckYesNoPair(PermitValidationYes, PermitValidationNo);

            // Check for Stub
            if (AppSettings.System.BallotStub == 1)
            {
                // Display Stub Question
                StubVerificationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                // Display Return to search button
                NextVerify.Visibility = Visibility.Visible;
            }
        }

        private void PermitValidationNo_Click(object sender, RoutedEventArgs e)
        {
            // Set No to checked and Yes to Disabled
            CheckYesNoPair(PermitValidationNo, PermitValidationYes);

            // Display Troubleshooting message & question
            PermitOptionsPanel.Visibility = Visibility.Visible;
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
            ClearYesNoPair(PermitValidationYes, PermitValidationNo);
            //ClearYesNoPair(PermitPrinterCheckYes, PermitPrinterCheckNo);
            //PermitPrinterCheckPanel.Visibility = Visibility.Collapsed;
            PermitOptionsPanel.Visibility = Visibility.Collapsed;
            PermitReprintCheck.IsChecked = false;
            PermitTransferVoterCheck.IsChecked = false;
        }

        private void PermitTransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            PermitReprintCheck.IsChecked = false;
            // Return to search screen
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new SettingsPage());
            }
        }

        private void StubValidationYes_Click(object sender, RoutedEventArgs e)
        {
            // Set Yes to checked and No to Disabled
            CheckYesNoPair(StubValidationYes, StubValidationNo);

            // Display Return to search button
            NextVerify.Visibility = Visibility.Visible;
        }

        private void StubValidationNo_Click(object sender, RoutedEventArgs e)
        {
            // Set No to checked and Yes to Disabled
            CheckYesNoPair(StubValidationNo, StubValidationYes);

            // Display Troubleshooting message & question
            StubOptionsPanel.Visibility = Visibility.Visible;
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
            ClearYesNoPair(StubValidationYes, StubValidationNo);
            //ClearYesNoPair(StubPrinterCheckYes, StubPrinterCheckNo);
            //StubPrinterCheckPanel.Visibility = Visibility.Collapsed;
            StubOptionsPanel.Visibility = Visibility.Collapsed;
            StubReprintCheck.IsChecked = false;
            StubTransferVoterCheck.IsChecked = false;
        }

        private void StubTransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            StubReprintCheck.IsChecked = false;
            // Return to search screen
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new SettingsPage());
            }
        }
    }
}
