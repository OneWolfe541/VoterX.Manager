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
using VoterX.Dialogs;
using VoterX.Core.Voters;
using VoterX.Core.Elections;
using VoterX.Core.ScanHistory;
using VoterX.Manager.Views.Absentee;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for SpoilBallotPage.xaml
    /// </summary>
    public partial class SpoiledBallotPage : Page
    {
        //VoterDatabase _VoterDB = ((App)Application.Current).voterContainer.Resolve<VoterDatabase>();

        //public IEnumerable<avSpoiledReason> spoiledList;

        private NMVoter _Voter;

        private bool exitLogout = false;

        public SpoiledBallotPage(NMVoter voter)
        {
            InitializeComponent();

            //LoggingMethods.LogPage("VALIDATION PAGE LOADED");

            _Voter = voter;

            LoadVoterFields(voter.Data);

            LoadSpoiledReasons();

            GlobalReferences.Header.PageHeader = ("Spoil Ballot");
        }

        public SpoiledBallotPage(NMVoter voter, bool returnStatus)
        {
            InitializeComponent();

            _Voter = voter;
            exitLogout = returnStatus;

            LoadVoterFields(voter.Data);

            LoadSpoiledReasons();

            GlobalReferences.Header.PageHeader = ("Spoil Ballot");
        }

        // Return to search screen without search parameters
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (exitLogout == true)
            {
                //this.NavigateToPage(new EditBallotSearchPage(null));
            }
            else
            {
                if (AppSettings.Absentee.TestScreen == true)
                {
                    // Check for SERVIS lookup
                    bool SERVISlookup = false;
                    if (_Voter.Data.PollID == 0) SERVISlookup = true;

                    this.NavigateToPage(new VoterDetailsPage(_Voter, SERVISlookup));
                }
                else
                {
                    this.NavigateToPage(new SearchPage());
                }
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogPage("VALIDATION PAGE UNLOADED");
        }

        private void LoadVoterFields(VoterDataModel voter)
        {
            FullName.Text = voter.FullName;
            BirthYear.Text = voter.DOBYear;
            Address.Text = voter.Address1;
            CityStateAndZip.Text = voter.City + ", " + voter.State + " " + voter.Zip;
        }

        private async void LoadSpoiledReasons()
        {
            //SpoiledReason.Items.Clear(); // No need to clear the list when directly binding to data

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var spoiledList = ElectionDataMethods.SpoiledReasons;

                SpoiledReason.ItemsSource = spoiledList;
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
            }
        }

        //private void LoadFilteredSpoiledReasons()
        //{
        //    //SpoiledReason.Items.Clear(); // No need to clear the list when directly binding to data

        //    var spoiledList = VoterMethods.SpoiledReasons
        //        .Query(0)
        //        .Where(reason => reason.spoiled_reason_id == 3 || reason.spoiled_reason_id == 4)
        //        .ToList();

        //    SpoiledReason.ItemsSource = spoiledList;
        //}

        private SpoiledReasonModel GetSelectedItem(ComboBox sender)
        {
            //if (sender.SelectedItem == null) return "";
            //else
            return ((SpoiledReasonModel)sender.SelectedItem);
        }

        private void SpoilButton_Click(object sender, RoutedEventArgs e)
        {
            int reasonID = GetSelectedItem(SpoiledReason).SpoiledReasonId;
            if (reasonID == 5 && _Voter.Data.LogCode != 14)
            {
                AlertDialog newMessage = new AlertDialog("ONLY UNDELIVERABLE BALLOTS CAN BE REISSUED");
                newMessage.ShowDialog();
            }
            else
            {
                // Set local values
                _Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                _Voter.Data.PollID = AppSettings.System.SiteID;
                _Voter.Data.ComputerID = AppSettings.System.MachineID;
                _Voter.Data.UserId = AppSettings.User.UserID;

                // Allow Undeliverable ballots to be reissued
                if (reasonID == 5 && _Voter.Data.LogCode == 14)
                {
                    // AS if user really wants to reissue the voters ballot
                    YesNoDialog newMessage = new YesNoDialog("MarkBackVoter", "THIS VOTER'S STATUS WILL BE MARKED BACK TO\r\nISSUED ABSENTEE BALLOT BY MAIL,\r\nAND A NEW BALLOT WILL BE PRINTED.\r\n\r\nDO YOU WANT TO CONTINUE?");
                    if (newMessage.ShowDialog() == true)
                    {
                        // Reset voted record to Ballot Issued
                        //VoterMethods.UpdateVoterReissuedBallot(_Voter);
                        _Voter.UpdateReissued();

                        // Remove voter from scan session
                        using (var scanHistory = new ScanHistoryFactory())
                        {
                            scanHistory.RemoveVoter(_Voter);
                        }
                    }
                    else
                    {
                        // Stop the process and allow the user to select another option
                        return;
                    }
                }

                // Update Ballot Number
                // Get new ballot number
                if (_Voter.Data.LogCode == 8)
                {
                    _Voter.GetNextBallotNumber((int)AppSettings.System.SiteID);
                }
                else
                {
                    _Voter.GetNextBallotNumber((int)AppSettings.Absentee.SiteID);
                }

                // Create Spoiled Record
                //VoterMethods.InsertSpoiledAbsenteeBallot(_Voter, reasonID);
                _Voter.SpoilBallot(reasonID);

                // Store new ballot number in voted record
                //VoterMethods.UpdateVoterBallotNumber(_Voter);
                _Voter.UpdateBallotNumber();

                // Reprint ballot and/or permit
                BallotPrinting.ReprintBallot(_Voter.Data);

                // Print Ballot Stub
                if (AppSettings.System.BallotStub == 1 && _Voter.Data.LogCode == 8)
                {
                    ReportPrintingMethods.PrintBallotStub(_Voter.Data);
                }

                // Goto the print varification page
                if (exitLogout == true)
                {
                    this.NavigateToPage(new SpoiledVerifyTroubleshootPage(_Voter, true));
                }
                else
                {
                    this.NavigateToPage(new SpoiledVerifyTroubleshootPage(_Voter));
                }
            }
        }

        private void SpoiledReason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = GetSelectedItem((ComboBox)sender);

            if (item != null)
            {
                if (item.SpoiledReasonId >= 1)
                {
                    // Show surrender question
                    SurrenderBallotVerificationPanel.Visibility = Visibility.Visible;

                    if (item.SpoiledReasonId == 3 || item.SpoiledReasonId == 4)
                    {
                        if (SurrenderYes.IsChecked == true || SurrenderNo.IsChecked == true)
                        {
                            SpoilWrongOrFledBallot.Visibility = Visibility.Visible;
                            SpoilBallot.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            SpoilWrongOrFledBallot.Visibility = Visibility.Collapsed;
                            SpoilBallot.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        if (SurrenderYes.IsChecked == true || SurrenderNo.IsChecked == true)
                        {
                            SpoilWrongOrFledBallot.Visibility = Visibility.Collapsed;
                            SpoilBallot.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            SpoilWrongOrFledBallot.Visibility = Visibility.Collapsed;
                            SpoilBallot.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }

            //LoggingMethods.LogUser("SPOILED REASON SELECTED: " + item.spoiled_reason.ToString());
        }

        //public bool CheckSpoiledRequirements()
        //{
        //    bool result = false;
        //    if (SpoiledReason.SelectedItem != null) StatusBar.ApplicationStatus(((avSpoiledReason)SpoiledReason.SelectedItem).spoiled_reason);
        //    if ((SurrenderNo.IsChecked == true || SurrenderYes.IsChecked == true) && SpoiledReason.SelectedItem != null) result = true;
        //    return result;

        //}

        private void OptionOut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Surrender_Click(object sender, RoutedEventArgs e)
        {
            if ((CheckBox)sender == SurrenderYes)
            {
                //SpoilBallot.Visibility = Visibility.Visible;
                //ProvisionalBallot.Visibility = Visibility.Collapsed;
                SurrenderNo.IsChecked = false;
            }
            if ((CheckBox)sender == SurrenderNo)
            {
                //SpoilBallot.Visibility = Visibility.Collapsed;
                //ProvisionalBallot.Visibility = Visibility.Visible;
                SurrenderYes.IsChecked = false;
            }
            if (SurrenderNo.IsChecked == false && SurrenderYes.IsChecked == false)
            {
                SpoilBallot.Visibility = Visibility.Visible;
            }
            //if (CheckSpoiledRequirements() == true)
            //{
            //    SpoilBallot.Visibility = Visibility.Visible;
            //}
        }

        private void SurrenderYes_Click(object sender, RoutedEventArgs e)
        {
            var item = GetSelectedItem(SpoiledReason);

            // Set Surrendered Ballot Status
            _Voter.Data.BallotSurrendered = true;

            // Cannot uncheck this button
            SurrenderYes.IsChecked = true;
            SurrenderYes.IsEnabled = false;

            // Turn on the Check Icon
            ballot_fa_check_yes.Visibility = Visibility.Visible;

            SurrenderNo.IsEnabled = true;

            // Uncheck the other button
            SurrenderNo.IsChecked = false;
            // Turn of Check Icon on the other button
            ballot_fa_check_no.Visibility = Visibility.Hidden;

            // Unfilter Spoiled Reason List
            //LoadSpoiledReasons();

            //if (SurrenderNo.IsChecked == false && SurrenderYes.IsChecked == false)
            //{
            //    SpoilBallot.Visibility = Visibility.Collapsed;
            //}
            //if (CheckSpoiledRequirements() == true)
            //{
            //    SpoilBallot.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    SpoilBallot.Visibility = Visibility.Collapsed;
            //}    

            if (item.SpoiledReasonId == 3 || item.SpoiledReasonId == 4)
            {
                SpoilWrongOrFledBallot.Visibility = Visibility.Visible;
                SpoilBallot.Visibility = Visibility.Collapsed;
            }
            else
            {
                SpoilWrongOrFledBallot.Visibility = Visibility.Collapsed;
                SpoilBallot.Visibility = Visibility.Visible;
            }
        }

        private void SurrenderNo_Click(object sender, RoutedEventArgs e)
        {
            var item = GetSelectedItem(SpoiledReason);

            // Set Surrendered Ballot Status
            _Voter.Data.BallotSurrendered = false;

            // Cannot uncheck this button
            SurrenderNo.IsChecked = true;
            SurrenderNo.IsEnabled = false;

            // Turn on the Check Icon
            ballot_fa_check_no.Visibility = Visibility.Visible;

            SurrenderYes.IsEnabled = true;

            // Uncheck the other button
            SurrenderYes.IsChecked = false;
            // Turn of Check Icon on the other button
            ballot_fa_check_yes.Visibility = Visibility.Hidden;

            // Filter Spoiled Reason List
            //LoadFilteredSpoiledReasons();

            //SpoiledReason.SelectedIndex = -1;

            if (item.SpoiledReasonId == 3 || item.SpoiledReasonId == 4)
            {
                SpoilWrongOrFledBallot.Visibility = Visibility.Visible;
                SpoilBallot.Visibility = Visibility.Collapsed;
            }
            else
            {
                SpoilWrongOrFledBallot.Visibility = Visibility.Collapsed;
                SpoilBallot.Visibility = Visibility.Visible;
            }

            //if (SurrenderNo.IsChecked == false && SurrenderYes.IsChecked == false)
            //{
            //    SpoilBallot.Visibility = Visibility.Collapsed;
            //}
            //if (CheckSpoiledRequirements() == true)
            //{
            //    SpoilBallot.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    SpoilBallot.Visibility = Visibility.Collapsed;
            //}
        }

        private void SpoilWrongOrFledBallot_Click(object sender, RoutedEventArgs e)
        {
            int reason = GetSelectedItem(SpoiledReason).SpoiledReasonId;
            // Create Spoiled Record
            //VoterMethods.InsertSpoiledBallot(_Voter, reason);
            _Voter.SpoilBallot(reason);

            if (reason == 3)
            {
                //VoterMethods.MarkAsFledVoter(_Voter);
                _Voter.UpdateFledVoter();
            }
            if (reason == 4)
            {
                //VoterMethods.MarkAsWrongVoter(_Voter);
                _Voter.UpdateWrongVoter();
            }

            this.NavigateToPage(new SearchPage());
        }

        //private void Validation_Click(object sender, RoutedEventArgs e)
        //{
        //    if((CheckBox)sender == SurrenderYes)
        //    {
        //        SurrenderNo.IsChecked = false;
        //    }
        //    if ((CheckBox)sender == SurrenderNo)
        //    {
        //        SurrenderYes.IsChecked = false;
        //    }
        //}
    }
}
