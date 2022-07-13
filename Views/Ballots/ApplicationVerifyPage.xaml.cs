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

namespace VoterX
{
    /// <summary>
    /// Interaction logic for ApplicationVerifyPage.xaml
    /// </summary>
    public partial class ApplicationVerifyPage : Page
    {
        //private VoterDataModel _voter = new VoterDataModel();

        //public ApplicationVerifyPage(VoterDataModel voter)
        //{
        //    InitializeComponent();

        //    _voter = voter;

        //    StatusBar.ApplicationStatus("Application Verify Page Loaded");
        //}

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigateToPage(new SearchPage(null));
        }

        // Run code if any of the "YES" boxes are clicked
        private void Validation_Click(object sender, RoutedEventArgs e)
        {
            // Show or hide the Finalize button
            if (AllValidationBoxesChecked()) NextVerify.Visibility = Visibility.Visible;
            else NextVerify.Visibility = Visibility.Collapsed;

            // Toggle the ballot and permit boxes like radio buttons
            if ((CheckBox)sender == ApplicationCorrect)
            {
                ApplicationWrong.IsChecked = false;
                PrintApplication.Visibility = Visibility.Collapsed;
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
            if (ApplicationWrong.IsChecked == true) PrintApplication.Visibility = Visibility.Visible;
            else PrintApplication.Visibility = Visibility.Collapsed;

            // Toggle the Print Permit button
            //if (PermitWrong.IsChecked == true) PrintPermit.Visibility = Visibility.Visible;
            //else PrintPermit.Visibility = Visibility.Collapsed;

            // Toggle the ballot and permit boxes like radio buttons
            if ((CheckBox)sender == ApplicationWrong) ApplicationCorrect.IsChecked = false;
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
            if (ApplicationCorrect.IsChecked == false) result = false;
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

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            // Reprint voter Application
        }

        private void NextVerifyButton_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigateToPage(new BallotFinalizePage(_voter));
        }
    }
}
