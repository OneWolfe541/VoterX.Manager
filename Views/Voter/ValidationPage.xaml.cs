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
    /// The Validation Page determines which path to take for a specific voter's needs
    /// It tries to anticipate a voters needs based on their voting history for the current election
    /// The page first checks if they have already been issued a ballot and where that ballot was issued
    /// For ballots issued at the same site on the same day will be sent to the Spoiled ballot path
    /// For ballots issued at a different site or different day are sent to the Provisional ballot path
    /// 
    /// Now Spoiled or Provisional buttons are determined if/when a voter returns their existing ballot
    /// </summary>
    public partial class ValidationPage : Page
    {
        //private VoterSearchModel search = new VoterSearchModel();
        //private VoterDataModel _voter = new VoterDataModel();

        // Get the current system's polling location ID
        int MyLocation = Int32.Parse(AppSettings.System.SiteID.ToString());

        //public ValidationPage(VoterNavModel voterFromNav)
        //{
        //    InitializeComponent();

        //    //LoggingMethods.LogPage("VALIDATION PAGE LOADED");

        //    //StatusBar.ApplicationStatus(voterFromNav.Voter.VoterID + " " + voterFromNav.Voter.IDRequired.ToString());
        //    search = voterFromNav.Search;

        //    _voter = voterFromNav.Voter;

        //    LoadVoterFields(voterFromNav.Voter);

        //    //LoadSpoiledReasons();

        //    CheckVoterStatus(voterFromNav.Voter);

        //    //StatusBar.ApplicationStatus("Validation Page Loaded");

        //    StatusBar.ApplicationPageHeader("Voter Verification");
        //}

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogPage("VALIDATION PAGE UNLOADED");
        }

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

        // This function checks if the voter has already voted and where
        // And sets what options are availible to the user
        //private void CheckVoterStatus(VoterDataModel voter)
        //{
        //    string StatusMessage;
        //    StatusMessage = string.Concat(
        //        "Voter: ", voter.VoterID,
        //        " | PollID: ", voter.PollID,
        //        " | Location: ", voter.PollName,
        //        " | ID Check: ", voter.IDRequired);
        //    StatusBar.ApplicationStatus(StatusMessage);

        //    // Check log code
        //    if (voter.HasVoted())
        //    {
        //        // Display already voted message
        //        AlreadyVoted.Visibility = Visibility.Visible;
        //        PreviousSite.Text = voter.PollName;
        //        PreviousDate.Text = voter.LogDate.ToString();
        //        PreviousComputer.Text = voter.ComputerID.ToString();

        //        //Check voter location and logdate
        //        //if (voter.PollID == MyLocation && voter.VotedToday())
        //        if (voter.VotedHereToday(MyLocation) && !voter.WrongOrFledVoter())
        //        {
        //            // if same location then spoil ballot
        //            SpoilBallot.Visibility = Visibility.Visible;
        //            //SpoiledVarification.Visibility = Visibility.Visible;
        //            StatusBar.ApplicationStatusCenter("Same Day & Same Site");
        //        }
        //        else
        //        {
        //            // if different location or date then provisional ballot

        //            // Hide validation
        //            HideValidationBox();
        //            // Show "Do you wish to provide a provisional ballot?" button or check box
        //            StartProvisionalButton.Visibility = Visibility.Visible;

        //            ProvisionalBallot.Visibility = Visibility.Visible;
        //            StatusBar.ApplicationStatusCenter("Different Day or Different Site");
        //        }
        //    }
        //    else if(!voter.IsEligible())
        //    {
        //        IneligibleVoter.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        // Allow user to procceed to print out a ballot
        //        // Turn on official ballot button
        //        //OfficialBallot.Visibility = Visibility.Visible;
        //        // Capture signature instead
        //        Signature.Visibility = Visibility.Visible;
        //    }
        //}

        private void HideValidationBox()
        {
            //VoterVerificationBorder.Visibility = Visibility.Collapsed;
            //VoterVerificationText.Visibility = Visibility.Collapsed;
            CheckVoterInfoMessage.Visibility = Visibility.Collapsed;
            IDVarification.Visibility = Visibility.Collapsed;
            CheckNameGrid.Visibility = Visibility.Collapsed;
            CheckDateGrid.Visibility = Visibility.Collapsed;
            CheckAddressGrid.Visibility = Visibility.Collapsed;
        }

        private void ShowValidationBox()
        {
            //VoterVerificationBorder.Visibility = Visibility.Visible;
            //VoterVerificationText.Visibility = Visibility.Visible;
            CheckVoterInfoMessage.Visibility = Visibility.Visible;
            //IDVarification.Visibility = Visibility.Collapsed;
            CheckNameGrid.Visibility = Visibility.Visible;
            CheckDateGrid.Visibility = Visibility.Visible;
            CheckAddressGrid.Visibility = Visibility.Visible;
        }

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
            GlobalReferences.StatusBar.TextLeft = (StatusMessage);

            //if (AllValidationBoxesChecked()) BallotFunctions.Visibility = Visibility.Visible;
            //else BallotFunctions.Visibility = Visibility.Collapsed;
        }

        // Returns true if all of the vadilation boxes are checked
        //private bool AllValidationBoxesChecked()
        //{
        //    // Set state to true
        //    bool result = true;

        //    // If any of the following conditions are met state will be set to false

        //    // Check if the voter has already voted
        //    if (!_voter.HasVoted())
        //    {
        //        // Voter has not already voted

        //        // Check if voter ID is required                
        //        if ((bool)IDVarification.DataContext == true)
        //        {
        //            // When ID required also check name date and address
        //            if (IDCorrect.IsChecked == false) result = false;
        //            if (NameCorrect.IsChecked == false) result = false;
        //            if (DateCorrect.IsChecked == false) result = false;
        //            if (AddressCorrect.IsChecked == false) result = false;
        //        }
        //        else
        //        {
        //            // If ID is not required only check name date and address
        //            if (NameCorrect.IsChecked == false) result = false;
        //            if (DateCorrect.IsChecked == false) result = false;
        //            if (AddressCorrect.IsChecked == false) result = false;
        //        }
        //    }
        //    else
        //    {
        //        // Voter has already voted

        //        // When they have already voted dont need to check their ID a second time
        //        //if ((bool)IDVarification.DataContext == true)
        //        //{
        //        //    if (IDCorrect.IsChecked == false) result = false;
        //        //}
        //        //else
        //        //{
        //        if (NameCorrect.IsChecked == false) result = false;
        //        if (DateCorrect.IsChecked == false) result = false;
        //        if (AddressCorrect.IsChecked == false) result = false;
        //        //}
        //        //if (_voter.VotedHereToday(MyLocation))
        //        //    if (SurrenderYes.IsChecked == false && SurrenderNo.IsChecked == false) result = false;
        //    }

        //    // Return the final state
        //    return result;
        //}

        // Goto official ballot page
        private void OfficialButton_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigateToPage(new SignatureCapturePage(_voter));
        }

        // Goto spoil ballot page
        private void SpoilButton_Click(object sender, RoutedEventArgs e)
        {
            ////LoggingMethods.LogUser("NAVIGATE TO SPOIL BALLOT PAGE");
            ////AlertDialog sampleDialog = new AlertDialog("Ballot Spoiled!");

            //// Check if the ballot surrendered option is selected
            ////_voter.BallotSurrendered = BallotSurrendered();

            //// Process:
            //// Spoil Ballot
            ////StatusBar.ApplicationStatus(VoterMethods.InsertSpoiledBallot2(_voter, GetSelectedItem(SpoledReason).spoiled_reason_id));
            ////if (VoterMethods.InsertSpoiledBallot(_voter, GetSelectedItem(SpoledReason).spoiled_reason_id) == true)
            ////{
            //    // Reprint
            //    //BallotPrinting.ReprintBallot(_voter);

            //    // Check if Ballot Printed
            //    this.NavigateToPage(new SpoiledBallotPage(_voter));
            ////}
            ////else
            ////{
            ////    StatusBar.ApplicationStatus("Ballot Not Spoiled");
            ////}
        }

        //private bool BallotSurrendered()
        //{
        //    if (SurrenderYes.IsChecked == true) return true;
        //    else return false;
        //}

        //private void SpoilFledButton_Click(object sender, RoutedEventArgs e)
        //{
        //    AlertDialog sampleDialog = new AlertDialog("Fled a Voter!");
        //    if (sampleDialog.ShowDialog() == true)
        //        StatusBar.ApplicationStatus("Voter has Fled");

        //    // Check if the ballot surrendered option is selected
        //    _voter.BallotSurrendered = BallotSurrendered();

        //    // Spoil Ballot
        //    VoterMethods.InsertSpoiledBallot(_voter, GetSelectedItem(SpoledReason).spoiled_reason_id);

        //    // Return to search
        //    this.NavigateToPage(new SearchPage(null));
        //}

        //private void SpoilWrongButton_Click(object sender, RoutedEventArgs e)
        //{
        //    AlertDialog sampleDialog = new AlertDialog("Wronged a Voter!");
        //    if (sampleDialog.ShowDialog() == true)
        //        StatusBar.ApplicationStatus("Voter was Wronged");

        //    // Check if the ballot surrendered option is selected
        //    _voter.BallotSurrendered = BallotSurrendered();

        //    // Spoil Ballot
        //    VoterMethods.InsertSpoiledBallot(_voter, GetSelectedItem(SpoledReason).spoiled_reason_id);

        //    // Mark Wrong Voter in avVotedRecord

        //    // Return to search
        //    this.NavigateToPage(new SearchPage(null));
        //}

        // Goto provisional ballot page
        private void ProvisionalButton_Click(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogUser("NAVIGATE TO PROVISIONAL BALLOT PAGE");
            //this.NavigateToPage(new ProvisionalBallotPage(_voter));
        }

        private void Signature_Click(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogUser("NAVIGATE TO SIGNATURE PAGE");
            //this.NavigateToPage(new SignatureCapturePage(_voter));
        }

        //private void Surrender_Click(object sender, RoutedEventArgs e)
        //{
        //    Validation_Click(sender, e);

        //    //SpoiledReasonPanel.Visibility = Visibility.Visible;

        //    if ((CheckBox)sender == SurrenderYes)
        //    {
        //        //SpoilBallot.Visibility = Visibility.Visible;
        //        //ProvisionalBallot.Visibility = Visibility.Collapsed;
        //        SurrenderNo.IsChecked = false;
        //    }
        //    if ((CheckBox)sender == SurrenderNo)
        //    {
        //        //SpoilBallot.Visibility = Visibility.Collapsed;
        //        //ProvisionalBallot.Visibility = Visibility.Visible;
        //        SurrenderYes.IsChecked = false;
        //    }
        //}

        //private void LoadSpoiledReasons()
        //{
        //    var spoiledList = VoterMethods.SpoiledReasons.Query().ToList();

        //    SpoledReason.ItemsSource = spoiledList;
        //}

        //private avSpoiledReason GetSelectedItem(ComboBox sender)
        //{
        //    //if (sender.SelectedItem == null) return "";
        //    //else
        //    return ((avSpoiledReason)sender.SelectedItem);
        //}

        //private void SpoledReason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var item = GetSelectedItem((ComboBox)sender);

        //    StatusBar.ApplicationStatus(item.spoiled_reason);

        //    SpoilBallot.Visibility = Visibility.Collapsed;
        //    SpoilFledBallot.Visibility = Visibility.Collapsed;
        //    SpoilWrongBallot.Visibility = Visibility.Collapsed;

        //    switch (item.spoiled_reason_id)
        //    {
        //        case 3:
        //            SpoilFledBallot.Visibility = Visibility.Visible;
        //            break;
        //        case 4:
        //            SpoilWrongBallot.Visibility = Visibility.Visible;
        //            break;
        //        default:
        //            SpoilBallot.Visibility = Visibility.Visible;
        //            break;
        //    }
        //}

        private void StartProvisionalButton_Click(object sender, RoutedEventArgs e)
        {
            ValidationDialog passwordDialog = new ValidationDialog("Administrator");
            //if (passwordDialog.ShowDialog() == true)
            //{
            //    StatusBar.ApplicationStatus("Password Correct");
            //    // show varification box
                ShowValidationBox();
            //    // hide button
                StartProvisionalButton.Visibility = Visibility.Collapsed;
            //}

        }
    }
}
