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
using System.Management;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for BallotVerifyPage.xaml
    /// </summary>
    public partial class PrintVerifyPage : Page
    {
        //private VoterDataModel _voter = new VoterDataModel();

        private int printingErrors = 0;

        //public PrintVerifyPage(VoterDataModel voter)
        //{
        //    InitializeComponent();

        //    _voter = voter;

        //    //StatusBar.ApplicationStatus("Ballot Verify Page Loaded");
        //    StatusBar.ApplicationPageHeader("Print Verification");
        //}

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new SearchPage());
        }

        // Run code if any of the "YES" boxes are clicked
        private void Validation_Click(object sender, RoutedEventArgs e)
        {
            // Show or hide the Finalize button
            if (AllValidationBoxesChecked()) NextVerify.Visibility = Visibility.Visible;
            else NextVerify.Visibility = Visibility.Collapsed;

            // Toggle the ballot and permit boxes like radio buttons
            if ((CheckBox)sender == BallotCorrect)
            {
                BallotWrong.IsChecked = false;
                SpoilBallot.Visibility = Visibility.Collapsed;
            }
            //if ((CheckBox)sender == PermitCorrect)
            //{
            //    PermitWrong.IsChecked = false;
            //    PrintPermit.Visibility = Visibility.Collapsed;
            //}
        }

        // Run code if any of the "NO" boxes are clicked
        private void ValidationWrong_Click(object sender, RoutedEventArgs e)
        {
            // Toggle Spoil Ballot button
            if (BallotWrong.IsChecked == true) SpoilBallot.Visibility = Visibility.Visible;
            else SpoilBallot.Visibility = Visibility.Collapsed;

            // Toggle the Print Permit button
            //if (PermitWrong.IsChecked == true) PrintPermit.Visibility = Visibility.Visible;
            //else PrintPermit.Visibility = Visibility.Collapsed;

            // Toggle the ballot and permit boxes like radio buttons
            if ((CheckBox)sender == BallotWrong) BallotCorrect.IsChecked = false;
            //if ((CheckBox)sender == PermitWrong) PermitCorrect.IsChecked = false;

            // Turn off the finalize button
            NextVerify.Visibility = Visibility.Collapsed;
        }

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

        //private void FinalizeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Mark voted record

        //    // Write log data to voter record

        //    // Save voter data to tblVotedRecord
        //    if (VoterMethods.InsertVotedAtPolls(_voter) == true)
        //    {
        //        StatusBar.ApplicationStatus("Voter Saved Successfull");
        //    }
        //    else
        //    {
        //        StatusBar.ApplicationStatus("Voter Not Saved");
        //    }
        //}

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
            //this.NavigateToPage(new BallotFinalizePage(_voter));

            // Mark Voter and return to search page
            //if (VoterMethods.InsertVotedAtPolls(_voter) == true)
            //{
            //    StatusBar.ApplicationStatus("Voter Saved Successfull");
            //    this.NavigateToPage(new SearchPage(null));
            //}
            //else
            //{
            //    StatusBar.ApplicationStatus("Voter Not Saved");
            //}

            this.NavigateToPage(new SearchPage());
        }

        private void OptOutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new SearchPage());
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
            printingErrors++;
            if (printingErrors <= 1)
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

                // Display reprint question
                //BallotReprintPanel.Visibility = Visibility.Visible;
                BallotOptionsPanel.Visibility = Visibility.Visible;

                // Create new dialog box object (popup dialog)
                //YesNoDialog sampleDialog = new YesNoDialog("Reprint","Do you want to reprint the ballot?");
                // Wait for the use to click "Yes" or "No"
                //if (sampleDialog.ShowDialog() == true)
                //{
                //    // If "Yes"
                //    // Reset check boxes
                //    BallotCorrect.IsChecked = false;
                //    BallotWrong.IsChecked = false;
                //    BallotCorrect.IsEnabled = true;
                //    BallotWrong.IsEnabled = true;
                //    ballot_fa_check_yes.Visibility = Visibility.Hidden;
                //    ballot_fa_check_no.Visibility = Visibility.Hidden;

                //    StatusBar.ApplicationStatus("Ballot Reprinted");
                //}
                //else
                //{
                //    // If "No"

                //    // Display the Return to Search button
                //    OptOutButton.Visibility = Visibility.Visible;
                //}
            }
            else
            {
                SeriousPrinterErrorPanel.Visibility = Visibility.Visible;
                OptOutButton.Visibility = Visibility.Visible;
            }
        }

        private void BallotReprint_Click(object sender, RoutedEventArgs e)
        {
            BallotReprintCorrect.IsEnabled = false;

            // Print a new ballot
            //await Task.Run(() => BallotPrinting.ReprintBallot(_voter));           

            // Spoil previous ballot with "print error" reason
            //VoterMethods.InsertSpoiledBallot(_voter, 1);

            // Turn on the Check Icon
            ballot_reprint_fa_check_yes.Visibility = Visibility.Visible;

            // Disable the other button
            BallotReprintWrong.IsEnabled = false;
            // Uncheck the other button
            BallotReprintWrong.IsChecked = false;
            // Turn of Check Icon on the other button
            ballot_reprint_fa_check_no.Visibility = Visibility.Hidden;

            // Display new printing message
            BallotPrintingStatus.Text = "A new Ballot was sent to the Printer";

            // reset the Reprint panel
            BallotReprintPanel.Visibility = Visibility.Collapsed;
            BallotReprintCorrect.IsChecked = false;
            BallotReprintWrong.IsChecked = false;
            BallotReprintCorrect.IsEnabled = true;
            BallotReprintWrong.IsEnabled = true;
            ballot_reprint_fa_check_yes.Visibility = Visibility.Hidden;
            ballot_reprint_fa_check_no.Visibility = Visibility.Hidden;

            // reset the ballot panel
            BallotCorrect.IsChecked = false;
            BallotWrong.IsChecked = false;
            BallotCorrect.IsEnabled = true;
            BallotWrong.IsEnabled = true;
            ballot_fa_check_yes.Visibility = Visibility.Hidden;
            ballot_fa_check_no.Visibility = Visibility.Hidden;

            // Reset Check Boxes
            TroubleShootCheck.IsChecked = false;
            TransferVoterCheck.IsChecked = false;
            // Hide additional questions
            BallotReprintPanel.Visibility = Visibility.Collapsed;
            BallotTransferPanel.Visibility = Visibility.Collapsed;
            // Hide error panel
            BallotOptionsPanel.Visibility = Visibility.Collapsed;            
        }

        private void BallotReprintWrong_Click(object sender, RoutedEventArgs e)
        {
            // Turn on the Check Icon
            ballot_reprint_fa_check_no.Visibility = Visibility.Visible;

            // Disable the other button
            BallotReprintCorrect.IsEnabled = false;
            // Uncheck the other button
            BallotReprintCorrect.IsChecked = false;
            // Turn of Check Icon on the other button
            ballot_reprint_fa_check_yes.Visibility = Visibility.Collapsed;

            // Display transfer question
            BallotTransferPanel.Visibility = Visibility.Visible;

            // Hide current question
            //BallotReprintPanel.Visibility = Visibility.Collapsed;
        }

        private void BallotTransfer_Click(object sender, RoutedEventArgs e)
        {
            // Turn on the Check Icon
            ballot_transfer_fa_check_yes.Visibility = Visibility.Visible;

            // Disable the other button
            BallotTransferWrong.IsEnabled = false;
            // Uncheck the other button
            BallotTransferWrong.IsChecked = false;
            // Turn of Check Icon on the other button
            ballot_transfer_fa_check_no.Visibility = Visibility.Hidden;

            // Display new printing message
            //BallotPrintingStatus.Text = "A new Ballot was sent to the Printer";

            // reset the Reprint panel
            //BallotReprintPanel.Visibility = Visibility.Collapsed;
            //BallotReprintCorrect.IsChecked = false;
            //BallotReprintWrong.IsChecked = false;
            //BallotReprintCorrect.IsEnabled = true;
            //BallotReprintWrong.IsEnabled = true;
            //ballot_reprint_fa_check_yes.Visibility = Visibility.Hidden;
            //ballot_reprint_fa_check_no.Visibility = Visibility.Hidden;

            // reset the ballot panel
            //BallotCorrect.IsChecked = false;
            //BallotWrong.IsChecked = false;
            //BallotCorrect.IsEnabled = true;
            //BallotWrong.IsEnabled = true;
            //ballot_fa_check_yes.Visibility = Visibility.Hidden;
            //ballot_fa_check_no.Visibility = Visibility.Hidden;

            NextVerify.Visibility = Visibility.Visible;
        }

        private void BallotTransferWrong_Click(object sender, RoutedEventArgs e)
        {
            // Turn on the Check Icon
            ballot_transfer_fa_check_no.Visibility = Visibility.Visible;

            // Disable the other button
            BallotTransferCorrect.IsEnabled = false;
            // Uncheck the other button
            BallotTransferCorrect.IsChecked = false;
            // Turn of Check Icon on the other button
            ballot_transfer_fa_check_yes.Visibility = Visibility.Hidden;

            // Reset Check Boxes
            TroubleShootCheck.IsChecked = false;
            TransferVoterCheck.IsChecked = false;
            // Hide additional questions
            BallotReprintPanel.Visibility = Visibility.Collapsed;
            BallotTransferPanel.Visibility = Visibility.Collapsed;

            //OptOutButton.Visibility = Visibility.Visible;
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
            if(AppSettings.System.BallotStub == 1)
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

            ApplicationErrorPanel.Visibility = Visibility.Visible;
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

            PermitErrorPanel.Visibility = Visibility.Visible;
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

        private void TroubleShootCheck_Click(object sender, RoutedEventArgs e)
        {
            TransferVoterCheck.IsChecked = false;
            BallotReprintPanel.Visibility = Visibility.Visible;

            BallotOptionsPanel.Visibility = Visibility.Collapsed;
        }

        private void TransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            TroubleShootCheck.IsChecked = false;
            BallotTransferPanel.Visibility = Visibility.Visible;

            BallotOptionsPanel.Visibility = Visibility.Collapsed;
        }

        private void TroubleShootApplicationCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck Transfer Option
            TransferVoterApplicationCheck.IsChecked = false;
            // Display Troubleshoot question (are you sure?)
            ApplicationReprintPanel.Visibility = Visibility.Visible;
            // Hide other question if its open
            ApplicationTransferPanel.Visibility = Visibility.Collapsed;            
        }

        private void TransferVoterApplicationCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck Troubleshoot Option
            TroubleShootApplicationCheck.IsChecked = false;            
            // Display Transfer question (are you sure?)            
            ApplicationTransferPanel.Visibility = Visibility.Visible;
            // Hide other question if its open
            ApplicationReprintPanel.Visibility = Visibility.Collapsed;
        }

        private void ApplicationReprint_Click(object sender, RoutedEventArgs e)
        {
            ApplicationReprintCorrect.IsEnabled = false;
            ApplicationReprintCorrect.IsChecked = true;

            // Turn on checked icon
            application_reprint_fa_check_yes.Visibility = Visibility.Visible;
            // Turn off the No Button
            ApplicationReprintWrong.IsChecked = false;
            ApplicationReprintWrong.IsEnabled = false;
            application_reprint_fa_check_no.Visibility = Visibility.Collapsed;
            
            // Print a new Application
            //await Task.Run(() => BallotPrinting.ReprintApplication(_voter));            

            // Reset application questions
            ApplicationCorrect.IsChecked = false;
            ApplicationCorrect.IsEnabled = true;
            app_fa_check_yes.Visibility = Visibility.Collapsed;
            ApplicationWrong.IsChecked = false;
            ApplicationWrong.IsEnabled = true;
            app_fa_check_no.Visibility = Visibility.Collapsed;

            ApplicationErrorPanel.Visibility = Visibility.Collapsed;
            TroubleShootApplicationCheck.IsChecked = false;
            TransferVoterApplicationCheck.IsChecked = false;
            SeriousApplicationErrorPanel.Visibility = Visibility.Collapsed;

            ApplicationReprintPanel.Visibility = Visibility.Collapsed;
            ApplicationReprintCorrect.IsChecked = false;
            ApplicationReprintCorrect.IsEnabled = true;
            application_reprint_fa_check_yes.Visibility = Visibility.Collapsed;
            ApplicationReprintWrong.IsChecked = false;
            ApplicationReprintWrong.IsEnabled = true;
            application_reprint_fa_check_no.Visibility = Visibility.Collapsed;

            ApplicationTransferPanel.Visibility = Visibility.Collapsed;
            ApplicationTransferCorrect.IsChecked = false;
            ApplicationTransferCorrect.IsEnabled = true;
            application_transfer_fa_check_yes.Visibility = Visibility.Collapsed;
            ApplicationTransferWrong.IsChecked = false;
            ApplicationTransferWrong.IsEnabled = true;
            application_transfer_fa_check_no.Visibility = Visibility.Collapsed;

        }

        private void ApplicationReprintWrong_Click(object sender, RoutedEventArgs e)
        {
            // Turn on checked icon
            application_reprint_fa_check_no.Visibility = Visibility.Visible;
            // Turn off the Yes Button
            ApplicationReprintCorrect.IsChecked = false;
            ApplicationReprintCorrect.IsEnabled = false;
            application_reprint_fa_check_yes.Visibility = Visibility.Collapsed;

            // hide current question
            //ApplicationReprintPanel.Visibility = Visibility.Collapsed;

            // Show next question
            ApplicationTransferPanel.Visibility = Visibility.Visible;
        }

        private void ApplicationTransfer_Click(object sender, RoutedEventArgs e)
        {
            ApplicationTransferCorrect.IsChecked = true;
            // Turn on checked icon
            application_transfer_fa_check_yes.Visibility = Visibility.Visible;
            // Turn off the No Button
            ApplicationTransferWrong.IsChecked = false;
            ApplicationTransferWrong.IsEnabled = false;
            application_transfer_fa_check_no.Visibility = Visibility.Collapsed;

            NextVerify.Visibility = Visibility.Visible;
        }

        private void ApplicationTransferWrong_Click(object sender, RoutedEventArgs e)
        {
            ApplicationTransferWrong.IsChecked = true;
            // Turn on checked icon
            application_transfer_fa_check_no.Visibility = Visibility.Visible;
            // Turn off the No Button
            ApplicationTransferCorrect.IsChecked = false;
            ApplicationTransferCorrect.IsEnabled = false;
            application_transfer_fa_check_yes.Visibility = Visibility.Collapsed;

            //// Reset Check Boxes
            //TroubleShootApplicationCheck.IsChecked = false;
            //TransferVoterApplicationCheck.IsChecked = false;
            //// Hide additional questions
            //ApplicationReprintPanel.Visibility = Visibility.Collapsed;
            //ApplicationTransferPanel.Visibility = Visibility.Collapsed;

            // Reset Questions
            TroubleShootApplicationCheck.IsChecked = false;
            TransferVoterApplicationCheck.IsChecked = false;
            SeriousApplicationErrorPanel.Visibility = Visibility.Collapsed;

            ApplicationReprintPanel.Visibility = Visibility.Collapsed;
            ApplicationReprintCorrect.IsChecked = false;
            ApplicationReprintCorrect.IsEnabled = true;
            application_reprint_fa_check_yes.Visibility = Visibility.Collapsed;
            ApplicationReprintWrong.IsChecked = false;
            ApplicationReprintWrong.IsEnabled = true;
            application_reprint_fa_check_no.Visibility = Visibility.Collapsed;

            ApplicationTransferPanel.Visibility = Visibility.Collapsed;
            ApplicationTransferCorrect.IsChecked = false;
            ApplicationTransferCorrect.IsEnabled = true;
            application_transfer_fa_check_yes.Visibility = Visibility.Collapsed;
            ApplicationTransferWrong.IsChecked = false;
            ApplicationTransferWrong.IsEnabled = true;
            application_transfer_fa_check_no.Visibility = Visibility.Collapsed;

        }

        private void PermitTroubleShootCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck other box
            PermitTransferVoterCheck.IsChecked = false;

            // Display Troubleshoot question
            PermitReprintPanel.Visibility = Visibility.Visible;
        }

        private void PermitTransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck other box
            PermitTroubleShootCheck.IsChecked = false;

            // Display Transfer question
            PermitTransferPanel.Visibility = Visibility.Visible;
        }

        private void PermitReprintYes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PermitReprintNo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PermitTransferYes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PermitTransferNo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BallotPrinterCheckYes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BallotPrinterCheckNo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
