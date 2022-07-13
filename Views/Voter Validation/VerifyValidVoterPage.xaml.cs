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
//using VoterX.Core.Models.Database;
//using VoterX.Core.Models.ViewModels;
//using VoterX.Core.Models.Utilities;
//using VoterX.Core.Extensions;
using VoterX.Methods;
using VoterX.Extensions;
using VoterX.Dialogs;
using VoterX.SystemSettings.Extensions;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for VerifyValidVoterPage.xaml
    /// </summary>
    public partial class VerifyValidVoterPage : Page
    {
        //private VoterSearchModel search = new VoterSearchModel();
        //private VoterDataModel _voter = new VoterDataModel();

        // Get the current system's polling location ID
        //int MyLocation = Int32.Parse(AppSettings.System.SiteID.ToString());

        //public VerifyValidVoterPage(VoterNavModel voterFromNav)
        //{
        //    InitializeComponent();

        //    search = voterFromNav.Search;

        //    _voter = voterFromNav.Voter;

        //    LoadVoterFields(voterFromNav.Voter);

        //    // Display page header
        //    StatusBar.ApplicationPageHeader("Voter Verification");

        //    //StatusBar.ApplicationStatusCenter(_voter.VoterID);
        //    StatusBar.ApplicationStatusClear();
        //}

        // Return to search page with remembered search parameters
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogUser("RETURNING TO SEARCH PAGE");
            //this.NavigateToPage(new SearchPage(search));
        }

        //private void LoadVoterFields(VoterDataModel voter)
        //{
        //    //VoterID.Text = voter.VoterID;
        //    FullName.Text = voter.FullName;
        //    BirthYear.Text = voter.DOBYear;
        //    Address.Text = voter.Address1;
        //    CityStateAndZip.Text = voter.City + ", " + voter.State + " " + voter.Zip;

        //    // Set a flag when voter ID is required 
        //    // Then turn on the ID varification control group
        //    // And turn off the other groups
        //    IDVarification.DataContext = voter.IDRequired;
        //    if (voter.IDRequired == true && !voter.HasVoted())
        //    {
        //        IDVarification.Visibility = Visibility.Visible;
        //        CheckNameGrid.Visibility = Visibility.Visible;
        //        CheckDateGrid.Visibility = Visibility.Visible;
        //        CheckAddressGrid.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        IDVarification.Visibility = Visibility.Collapsed;
        //    }
        //}

        // When any check box is clicked check of all boxes are checked
        // If all the boxes are checked turn on the Print action button group
        // Which button in the group gets displayed is determined in CheckVoterStatus()
        private void Validation_Click(object sender, RoutedEventArgs e)
        {
            string StatusMessage;
            StatusMessage = string.Concat(
                "Name: ", NameCorrect.IsChecked,
                " | Date: ", DateCorrect.IsChecked,
                " | Address: ", AddressCorrect.IsChecked,
                " | ID: ", IDCorrect.IsChecked);
            //StatusBar.ApplicationStatus(StatusMessage);

            if (AllValidationBoxesChecked()) BallotFunctions.Visibility = Visibility.Visible;
            else BallotFunctions.Visibility = Visibility.Collapsed;
        }

        // Returns true if all of the vadilation boxes are checked
        private bool AllValidationBoxesChecked()
        {
            // Set state to true
            bool result = true;

            // If any of the following conditions are met state will be set to false

            // Check if the voter has already voted
            //if (!_voter.HasVoted())
            //{
            //    // Voter has not already voted

            //    // Check if voter ID is required                
            //    if ((bool)IDVarification.DataContext == true)
            //    {
            //        // When ID required also check name date and address
            //        if (IDCorrect.IsChecked == false) result = false;
            //        if (NameCorrect.IsChecked == false) result = false;
            //        if (DateCorrect.IsChecked == false) result = false;
            //        if (AddressCorrect.IsChecked == false) result = false;
            //    }
            //    else
            //    {
            //        // If ID is not required only check name date and address
            //        if (NameCorrect.IsChecked == false) result = false;
            //        if (DateCorrect.IsChecked == false) result = false;
            //        if (AddressCorrect.IsChecked == false) result = false;
            //    }
            //}
            //else
            //{
            //    // Voter has already voted

            //    // When they have already voted dont need to check their ID a second time

            //    if (NameCorrect.IsChecked == false) result = false;
            //    if (DateCorrect.IsChecked == false) result = false;
            //    if (AddressCorrect.IsChecked == false) result = false;
            //}

            // Return the final state
            return result;
        }

        private void Signature_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigateToPage(new SignatureCapturePage(_voter));
        }

        private void PrintApplication_Click(object sender, RoutedEventArgs e)
        {
            //StatusBar.ApplicationStatus("");
            //StatusBar.ApplicationStatusRight("");
            GlobalReferences.StatusBar.TextClear();
            //StatusBar.ApplicationStatusCenter(ReportPrintingMethods.PrintVoterApplication(_voter));
        }

        private void PrintPermit_Click(object sender, RoutedEventArgs e)
        {
            //StatusBar.ApplicationStatus("");
            //StatusBar.ApplicationStatusRight("");
            GlobalReferences.StatusBar.TextClear();
            //StatusBar.ApplicationStatusCenter(ReportPrintingMethods.PrintVoterPermit(_voter));
        }        

        private void PrintStub_Click(object sender, RoutedEventArgs e)
        {
            //StatusBar.ApplicationStatus("");
            //StatusBar.ApplicationStatusRight("");
            GlobalReferences.StatusBar.TextClear();
            //StatusBar.ApplicationStatusCenter(ReportPrintingMethods.PrintBallotStub(_voter));
        }

        private void PrintSignatureForm_Click(object sender, RoutedEventArgs e)
        {
            //StatusBar.ApplicationStatus("");
            //StatusBar.ApplicationStatusRight("");
            GlobalReferences.StatusBar.TextClear();
            //StatusBar.ApplicationStatusCenter(ReportPrintingMethods.PrintSignatureForm(_voter));
        }
    }
}
