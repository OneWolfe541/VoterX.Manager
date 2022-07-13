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
using VoterX.Dialogs;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for VarifyProvisionalPage.xaml
    /// </summary>
    public partial class VarifyProvisionalPage : Page
    {
        //VoterDataModel _Voter;
        //int _ProvisionalReason;

        //public VarifyProvisionalPage(VoterDataModel voter, int provisionalReasonId)
        //{
        //    InitializeComponent();

        //    // Store voter data localy
        //    _Voter = voter;
        //    _ProvisionalReason = provisionalReasonId;

        //    StatusBar.ApplicationPageHeader("Print Verification");
        //}

        // Return to search screen without any search parameters
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Log user activity
            //LoggingMethods.LogUser("RETURNING TO SEARCH PAGE");

            this.NavigateToPage(new SearchPage());
        }

        private void Validation_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the ballot and permit boxes like radio buttons
            if ((CheckBox)sender == BallotCorrect)
            {
                BallotWrong.IsChecked = false;
                if (BallotCorrect.IsChecked == true)
                {
                    FinalizeBallot.Visibility = Visibility.Visible;
                    ReprintBallot.Visibility = Visibility.Collapsed;
                }
                else
                {
                    FinalizeBallot.Visibility = Visibility.Collapsed;
                }
            }
            else
            // Toggle the ballot and permit boxes like radio buttons
            if ((CheckBox)sender == BallotWrong)
            {
                BallotCorrect.IsChecked = false;
                if (BallotWrong.IsChecked == true)
                {
                    ReprintBallot.Visibility = Visibility.Visible;
                    FinalizeBallot.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ReprintBallot.Visibility = Visibility.Collapsed;
                }
            }

        }

        //private void ValidationWrong_Click(object sender, RoutedEventArgs e)
        //{
        //    // Toggle the ballot and permit boxes like radio buttons
        //    if ((CheckBox)sender == BallotWrong) 
        //    {
        //        BallotCorrect.IsChecked = false;
        //        if (BallotWrong.IsChecked == true)
        //        {
        //            ReprintBallot.Visibility = Visibility.Visible;
        //            FinalizeBallot.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            ReprintBallot.Visibility = Visibility.Collapsed;
        //        }
        //    }            
        //}

        private void FinalizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Save voter details to Provisional Table
            //VoterMethods.InsertProvisionalBallot(_Voter, _ProvisionalReason);
        }

        private void ReprintButton_Click(object sender, RoutedEventArgs e)
        {
            // Print a new ballot
        }

        private void BallotValidation_Click(object sender, RoutedEventArgs e)
        {
            OptOutButton.Visibility = Visibility.Collapsed;

            // Turn on the Check Icon
            ballot_fa_check_yes.Visibility = Visibility.Visible;
            // Disable the other button
            //BallotWrong.IsEnabled = false;
           
            // Uncheck the other button
            BallotWrong.IsChecked = false;
            // Turn of Check Icon on the other button
            ballot_fa_check_no.Visibility = Visibility.Hidden;


            //// Is today the election day?
            //if (AppSettings.Election.IsElectionDay())
            //{
            //    // If election day use the Permit Panel
            //    PermitVerificationPanel.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    // Otherwise use the Application Panel
            //    ApplicationVerificationPanel.Visibility = Visibility.Visible;
            //}

            FinishVerify.Visibility = Visibility.Visible;
        }

        private void BallotValidationWrong_Click(object sender, RoutedEventArgs e)
        {
            FinishVerify.Visibility = Visibility.Collapsed;

            // Turn on the Check Icon
            ballot_fa_check_no.Visibility = Visibility.Visible;
            // Disable the other button
            //BallotWrong.IsEnabled = false;

            // Uncheck the other button
            BallotCorrect.IsChecked = false;
            // Turn of Check Icon on the other button
            ballot_fa_check_yes.Visibility = Visibility.Hidden;

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

        private void FinishVerifyButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new SearchPage());
        }
    }
}
