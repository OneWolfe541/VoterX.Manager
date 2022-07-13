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
//using VoterX.Core.Models.ViewModels;
using VoterX.Extensions;
using VoterX.Methods;
using VoterX.SystemSettings.Extensions;
using VoterX.Dialogs;
using VoterX.Core.Voters;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for BallotVerifyPage.xaml
    /// </summary>
    public partial class SpoiledBallotVerifyPage : Page
    {
        private NMVoter _voter = new NMVoter();

        public SpoiledBallotVerifyPage(NMVoter voter)
        {
            InitializeComponent();

            _voter = voter;

            //StatusBar.ApplicationStatus("Ballot Verify Page Loaded");
            GlobalReferences.Header.PageHeader = ("Print Verification");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new SearchPage());
        }

        // Run code if any of the "YES" boxes are clicked
        //private void Validation_Click(object sender, RoutedEventArgs e)
        //{
        //    // Show or hide the Finalize button
        //    if (AllValidationBoxesChecked()) FinalizeButton.Visibility = Visibility.Visible;
        //    else FinalizeButton.Visibility = Visibility.Collapsed;

        //    // Toggle the ballot and permit boxes like radio buttons
        //    if ((CheckBox)sender == BallotCorrect)
        //    {
        //        BallotWrong.IsChecked = false;
        //        SpoilBallot.Visibility = Visibility.Collapsed;
        //    }
        //    //if ((CheckBox)sender == PermitCorrect)
        //    //{
        //    //    PermitWrong.IsChecked = false;
        //    //    PrintPermit.Visibility = Visibility.Collapsed;
        //    //}
        //}

        // Run code if any of the "NO" boxes are clicked
        //private void ValidationWrong_Click(object sender, RoutedEventArgs e)
        //{
        //    // Toggle Spoil Ballot button
        //    if (BallotWrong.IsChecked == true) SpoilBallot.Visibility = Visibility.Visible;
        //    else SpoilBallot.Visibility = Visibility.Collapsed;

        //    // Toggle the Print Permit button
        //    //if (PermitWrong.IsChecked == true) PrintPermit.Visibility = Visibility.Visible;
        //    //else PrintPermit.Visibility = Visibility.Collapsed;

        //    // Toggle the ballot and permit boxes like radio buttons
        //    if ((CheckBox)sender == BallotWrong) BallotCorrect.IsChecked = false;
        //    //if ((CheckBox)sender == PermitWrong) PermitCorrect.IsChecked = false;

        //    // Turn off the finalize button
        //    NextVerify.Visibility = Visibility.Collapsed;
        //}

        // Returns true if all of the vadilation boxes are checked
        private bool AllValidationBoxesChecked()
        {
            // Set state to true
            bool result = true;

            // If any of the conditions are met state will be set to false
            if (BallotCorrect.IsChecked == false) result = false;
            //if (PermitCorrect.IsChecked == false) result = false;

            // Return the final state
            return result;
        }

        private void FinalizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Mark voted record

            // Write log data to voter record

            // Save voter data to tblVotedRecord
            //if (VoterMethods.InsertVotedAtPolls(_voter) == true)
            //{
            //    StatusBar.ApplicationStatus("Voter Saved Successfull");
            //}
            //else
            //{
            //    StatusBar.ApplicationStatus("Voter Not Saved");
            //}

            // Save spoiled record

            this.NavigateToPage(new SearchPage());
        }

        private void SpoilButton_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigateToPage(new SpoilBallotPage(_voter));
        }

        private void PermitButton_Click(object sender, RoutedEventArgs e)
        {
            // Reprint voter permit
        }

        private void NextVerifyButton_Click(object sender, RoutedEventArgs e)
        {
            // Depending on the election type
            // or the mode of the system
            // will determine where to go next
            //if (AppSettings.Election.IsElectionDay())
            //{
            //    // Permits on election day
            //    this.NavigateToPage(new PermitVerifyPage(_voter));
            //}
            //else
            //{
            //    // Applications for early voting
            //    this.NavigateToPage(new ApplicationVerifyPage(_voter));
            //}
        }

        private void ReturnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        // Check system settings or election settings to see if today is election day
        //private bool ElectionDay()
        //{            
        //    if (AppSettings.Election.ElectionDate.Date == DateTime.Now.Date) return true;
        //    else return false;
        //}

        private void BallotValidation_Click(object sender, RoutedEventArgs e)
        {
            // Turn on the Check Icon
            ballot_fa_check_yes.Visibility = Visibility.Visible;
            // Disable the other button
            BallotWrong.IsEnabled = false;

            // Is the other button checked?
            if (BallotWrong.IsChecked == true)
            {
                // Uncheck the other button
                BallotWrong.IsChecked = false;
                // Turn of Check Icon on the other button
                ballot_fa_check_no.Visibility = Visibility.Hidden;
            }

            // Is today the election day?
            if (AppSettings.Election.IsElectionDay())
            {
                // If election day use the Permit Panel
                PermitVerificationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                // Otherwise use the Application Panel
                ApplicationVerificationPanel.Visibility = Visibility.Visible;
            }
        }

        private void BallotValidationWrong_Click(object sender, RoutedEventArgs e)
        {
            // Turn on the Check Icon
            ballot_fa_check_no.Visibility = Visibility.Visible;
            // Disable the other button
            BallotCorrect.IsEnabled = false;

            // Is the other button checked?
            if (BallotCorrect.IsChecked == true)
            {
                // Uncheck the other button
                BallotCorrect.IsChecked = false;
                // Turn of Check Icon on the other button
                ballot_fa_check_yes.Visibility = Visibility.Hidden;
            }

            // Check printer status
            GlobalReferences.StatusBar.TextCenter = ("Printer Status: "
                + PrinterStatus.GetPrinterStatus(AppSettings.Printers.BallotPrinter)
                + " - State: " + PrinterStatus.GetPrinterState(AppSettings.Printers.BallotPrinter)
                );

            // Create new dialog box object
            YesNoDialog sampleDialog = new YesNoDialog("Reprint", "Do you want to reprint the ballot?");
            // Wait for the use to click "Yes" or "No"
            if (sampleDialog.ShowDialog() == true)
            {
                // If "Yes"
                // Reset check boxes
                BallotCorrect.IsChecked = false;
                BallotWrong.IsChecked = false;
                BallotCorrect.IsEnabled = true;
                BallotWrong.IsEnabled = true;
                ballot_fa_check_yes.Visibility = Visibility.Hidden;
                ballot_fa_check_no.Visibility = Visibility.Hidden;

                GlobalReferences.StatusBar.TextLeft = ("Ballot Reprinted");
            }
            else
            {
                // If "No"
                // Display the Return to Search button
                OptOutButton.Visibility = Visibility.Visible;
            }
        }

        private void ApplicationValidation_Click(object sender, RoutedEventArgs e)
        {
            app_fa_check_yes.Visibility = Visibility.Visible;
            ApplicationWrong.IsEnabled = false;

            if (ApplicationWrong.IsChecked == true)
            {
                ApplicationWrong.IsChecked = false;
                app_fa_check_no.Visibility = Visibility.Hidden;
            }
            // If Ballot Stub is set
            if (AppSettings.System.BallotStub == 1)
            {
                StubVerificationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                NextVerify.Visibility = Visibility.Visible;
            }
        }

        private void ApplicationValidationWrong_Click(object sender, RoutedEventArgs e)
        {
            app_fa_check_no.Visibility = Visibility.Visible;
            ApplicationCorrect.IsEnabled = false;

            if (ApplicationCorrect.IsChecked == true)
            {
                ApplicationCorrect.IsChecked = false;
                app_fa_check_yes.Visibility = Visibility.Hidden;
            }
            YesNoDialog sampleDialog = new YesNoDialog("Reprint", "Do you want to reprint the application?");
            if (sampleDialog.ShowDialog() == true)
            {
                // Reset check boxes
                ApplicationCorrect.IsChecked = false;
                ApplicationWrong.IsChecked = false;
                ApplicationCorrect.IsEnabled = true;
                ApplicationWrong.IsEnabled = true;
                app_fa_check_yes.Visibility = Visibility.Hidden;
                app_fa_check_no.Visibility = Visibility.Hidden;

                GlobalReferences.StatusBar.TextLeft = ("Application Reprinted");
            }
            else
            {
                OptOutButton.Visibility = Visibility.Visible;
            }
        }

        private void PermitValidation_Click(object sender, RoutedEventArgs e)
        {
            permit_fa_check_yes.Visibility = Visibility.Visible;
            PermitWrong.IsEnabled = false;

            if (PermitWrong.IsChecked == true)
            {
                PermitWrong.IsChecked = false;
                permit_fa_check_no.Visibility = Visibility.Hidden;
            }
            // If Ballot Stub is set
            if (AppSettings.System.BallotStub == 1)
            {
                StubVerificationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                NextVerify.Visibility = Visibility.Visible;
            }
        }

        private void PermitValidationWrong_Click(object sender, RoutedEventArgs e)
        {
            permit_fa_check_no.Visibility = Visibility.Visible;
            PermitCorrect.IsEnabled = false;

            if (PermitCorrect.IsChecked == true)
            {
                PermitCorrect.IsChecked = false;
                permit_fa_check_yes.Visibility = Visibility.Hidden;
            }
            YesNoDialog sampleDialog = new YesNoDialog("Reprint", "Do you want to reprint the permit?");
            if (sampleDialog.ShowDialog() == true)
            {
                // Reset check boxes
                PermitCorrect.IsChecked = false;
                PermitWrong.IsChecked = false;
                PermitCorrect.IsEnabled = true;
                PermitWrong.IsEnabled = true;
                permit_fa_check_yes.Visibility = Visibility.Hidden;
                permit_fa_check_no.Visibility = Visibility.Hidden;

                GlobalReferences.StatusBar.TextLeft = ("Permit Reprinted");
            }
            else
            {
                OptOutButton.Visibility = Visibility.Visible;
            }
        }

        private void StubValidation_Click(object sender, RoutedEventArgs e)
        {
            // Turn on the Check Icon
            stub_fa_check_yes.Visibility = Visibility.Visible;
            // Disable the other button
            StubWrong.IsEnabled = false;

            // Is the other button checked?
            if (StubWrong.IsChecked == true)
            {
                // Uncheck the other button
                StubWrong.IsChecked = false;
                // Turn of Check Icon on the other button
                stub_fa_check_no.Visibility = Visibility.Hidden;
            }
            // Display the Next Button
            NextVerify.Visibility = Visibility.Visible;
        }

        private void StubValidationWrong_Click(object sender, RoutedEventArgs e)
        {
            // Turn on the Check Icon
            stub_fa_check_no.Visibility = Visibility.Visible;
            // Disable the other button
            StubCorrect.IsEnabled = false;

            // Is the other button checked?
            if (StubCorrect.IsChecked == true)
            {
                // Uncheck the other button
                StubCorrect.IsChecked = false;
                // Turn of Check Icon on the other button
                stub_fa_check_yes.Visibility = Visibility.Hidden;
            }

            // Create new dialog box object
            YesNoDialog sampleDialog = new YesNoDialog("Reprint", "Do you want to reprint the stub?");
            // Wait for the use to click "Yes" or "No"
            if (sampleDialog.ShowDialog() == true)
            {
                // If "Yes"
                // Reset check boxes
                StubCorrect.IsChecked = false;
                StubWrong.IsChecked = false;
                StubCorrect.IsEnabled = true;
                StubWrong.IsEnabled = true;
                stub_fa_check_yes.Visibility = Visibility.Hidden;
                stub_fa_check_no.Visibility = Visibility.Hidden;

                GlobalReferences.StatusBar.TextLeft = ("Stub Reprinted");
            }
            else
            {
                // If "No"
                // Display the Return to Search button
                OptOutButton.Visibility = Visibility.Visible;
            }
        }
    }
}
