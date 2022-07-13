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

namespace VoterX
{
    /// <summary>
    /// Interaction logic for BallotVerifyPage.xaml
    /// </summary>
    public partial class BallotVerifyPage : Page
    {
        //private VoterDataModel _voter = new VoterDataModel();

        //public BallotVerifyPage(VoterDataModel voter)
        //{
        //    InitializeComponent();

        //    _voter = voter;

        //    //StatusBar.ApplicationStatus("Ballot Verify Page Loaded");
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

        // Check system settings or election settings to see if today is election day
        //private bool ElectionDay()
        //{            
        //    if (AppSettings.Election.ElectionDate.Date == DateTime.Now.Date) return true;
        //    else return false;
        //}
    }
}
