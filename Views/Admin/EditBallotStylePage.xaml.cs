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
using VoterX.Methods;
//using VoterX.Core.Models.ViewModels;
//using VoterX.Core.Models.Database;
//using VoterX.Core.Extensions;
//using VoterX.Core.Models.Utilities;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for EditBallotStylePage.xaml
    /// </summary>
    public partial class EditBallotStylePage : Page
    {
        //private VoterSearchModel search = new VoterSearchModel();
        //private VoterDataModel _voter = new VoterDataModel();

        private VoterLookupStatus _voter_status = VoterLookupStatus.None;

        //public EditBallotStylePage(VoterNavModel voterFromNav)
        //{
        //    InitializeComponent();

        //    // Display page header
        //    StatusBar.ApplicationPageHeader("Edit Ballot Style");

        //    search = voterFromNav.Search;

        //    _voter = voterFromNav.Voter;

        //    _voter_status = VoterValidationMethods.CheckVoterStatus(_voter, (int)AppSettings.System.SiteID);

        //    LoadVoterFields();

        //    LoadBallotStyles();

        //    LoadPrecinctPartsStyles();

        //    LoadParties();

        //    // Hide offical ballot question if voter has already voted
        //    if (
        //        _voter_status == VoterLookupStatus.Provisional ||
        //        _voter_status == VoterLookupStatus.Spoilable
        //        ) OfficialBallotCheckPanel.Visibility = Visibility.Collapsed;
        //}

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigateToPage(new EditBallotSearchPage(search));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new LoginPage());
        }

        private void LoadVoterFields()
        {
            // Default voter
            //_voter = await Task.Run(() => VoterMethods.Voter.Single("45769").FirstOrDefault() );

            //VoterID.Text = voter.VoterID;
            //FullName.Text = _voter.FullName;
            //BirthYear.Text = _voter.DOBYear;
            //Address.Text = _voter.Address1;
            //CityStateAndZip.Text = _voter.City + ", " + _voter.State + " " + _voter.Zip;
            //BallotStyle.Text = _voter.BallotStyle;
        }

        private void LoadBallotStyles()
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(BallotStylesList, TempLoadingSpinnerItem);

            //if (await Task.Run(() => VoterMethods.BallotStyleMaster.Exists(0)) == true)
            //{
            //    foreach (var ballotstyle in await Task.Run(() => VoterMethods.BallotStyleMaster.List()))
            //    {
            //        ComboBoxMethods.AddComboItemToControl(
            //            BallotStylesList,
            //            ballotstyle.ballot_style_id,
            //            ballotstyle.ballot_style_name,
            //            1
            //            );
            //    }
            //}

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(BallotStylesList, loadingItem);

            BallotStylesList.SelectedIndex = -1;
        }

        private void LoadPrecinctPartsStyles()
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(PrecinctPartsList, TempLoadingSpinnerItem);

            //IEnumerable<tblPrecinctsMaster> PrecinctPartList = await PrecinctMethods.GetPrecinctsListAsync();
            //// Sort list numericly instead of alphabeticly
            //if (PrecinctPartList != null)
            //{
            //    foreach (var precinctPart in PrecinctPartList.OrderBy(x => Double.Parse(x.precinct_part)))
            //    {
            //        ComboBoxMethods.AddComboItemToControl(
            //            PrecinctPartsList,
            //            precinctPart.precinct_part_id,
            //            precinctPart.precinct_part.ToString(),
            //            "001.001"
            //            );
            //    }
            //}

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(PrecinctPartsList, loadingItem);

            PrecinctPartsList.SelectedIndex = -1;
        }

        private void LoadParties()
        {
            foreach (var party in PartyMethods.GetPrimaryPartyList())
            {
                ComboBoxMethods.AddComboItemToControl(
                    PartyList,
                    party,
                    party,
                    "DEM"
                    );
            }

            PartyList.SelectedIndex = -1;
        }

        //private int GetVoterBallotId(VoterDataModel voter)
        //{
        //    if (voter.BallotStyleID == null) return 0;
        //    else return (int)voter.BallotStyleID;
        //}

        private void SaveBallotStyle_Click(object sender, RoutedEventArgs e)
        {
            // Dont know if we are going to save the ballot changes to the database or the local record
            // Or if the changes only go into the avVotedRecord table            

            //BallotStyle.Text = GetSelectedItemNumber(BallotStylesList).ToString();
            //BallotStyle.Text += ", " + GetSelectedItem(BallotStylesList);

            int ballotstyleid = GetSelectedItemNumber(BallotStylesList);

            //var newballotstyle = VoterMethods.BallotStyleMaster.GetByID(ballotstyleid);

            //_voter.BallotStyleID = newballotstyle.ballot_style_id;
            //_voter.BallotStyle = newballotstyle.ballot_style_name;
            //_voter.BallotStyleFile = newballotstyle.ballot_style_filename;

            //BallotStyle.Text = _voter.BallotStyleID.ToString() + ", " + _voter.BallotStyle + ", " + _voter.BallotStyleFile;
        }

        private int GetSelectedItemNumber(ComboBox sender)
        {
            if (sender.SelectedItem == null) return 0;
            else
                return (int)((ComboBoxItem)sender.SelectedItem).DataContext;
        }

        private string GetSelectedItem(ComboBox sender)
        {
            if (sender.SelectedItem == null) return "";
            else
                return ((ComboBoxItem)sender.SelectedItem).Content.ToString();
        }

        private void PrecinctPartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetNewBallotStyle(sender, e);
            TurnOnCorrectButton();
        }

        private void PartyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetNewBallotStyle(sender, e);
            TurnOnCorrectButton();
        }

        private void GetNewBallotStyle(object sender, SelectionChangedEventArgs e)
        {
            // Convert sender to a ComboBox
            var comboBox = (ComboBox)sender;
            // Check if the combobox is ready
            if (comboBox.IsLoaded)
            {
                // Get the precint part Id
                string precinctPart = ComboBoxMethods.GetSelectedItemData(PrecinctPartsList).ToString();
                // Get the party name
                string party = ComboBoxMethods.GetSelectedItem(PartyList);
                // Check if either precinct part id or party name are blank
                //if (!precinctPart.IsNullOrEmpty() && !party.IsNullOrEmpty())
                //{
                //    // Get the Ballot Style Lookup query
                //    //var ballotStyleQuery = BallotStyleMethods.GetBallotStyleLookup(precinctPart, party);

                //    // Get the Ballot Style Lookup query based on election type
                //    var ballotStyleQuery = AppSettings.Election.ElectionType == 1 ?
                //        // Primary
                //        BallotStyleMethods.GetBallotStyleLookup(precinctPart, party) :
                //        // General
                //        BallotStyleMethods.GetBallotStyleLookup(precinctPart);
                //    // Check of query returns 0 records
                //    if (ballotStyleQuery.Count() != 0)
                //    {
                //        // Collect all ballot styles asynchronously
                //        string ballotStyle = await Task.Run(() => ballotStyleQuery.FirstOrDefault().BallotStyleName);
                //        // Set Ballot Style list and display results
                //        string message = SetBallotStyleSelectedItem(BallotStylesList, ballotStyle);
                //        StatusBar.ApplicationStatus(message);
                //        //ComboBoxMethods.SetSelectedItem(BallotStylesList, ballotStyle);
                //    }
                //    else
                //    {
                //        // Set Ballot Style list to blank
                //        BallotStylesList.SelectedIndex = -1;
                //        // Display failure message
                //        StatusBar.ApplicationStatus("No Ballot Style Found");
                //    }
                //}
            }
        }

        //private string SetBallotStyleSelectedItem(ComboBox sender, string ballotStyle)
        //{
        //    // Create blank message
        //    string message = "";

        //    // Check if ballot style is empty
        //    if (!ballotStyle.IsNullOrEmpty())
        //    {
        //        // Check if the destination list is empty
        //        if (sender.Items.Count > 0)
        //        {
        //            // Loop through list items
        //            foreach (ComboBoxItem item in sender.Items)
        //            {
        //                // Compare list item to the string provided
        //                if (item.Content.ToString() == ballotStyle)
        //                {
        //                    // Set the destination selected item
        //                    sender.SelectedIndex = sender.Items.IndexOf(item);
        //                    // Set message to location of item found
        //                    message = (item.Content.ToString() + " Found At: " + sender.Items.IndexOf(item).ToString());
        //                    // Break out of the loop
        //                    return message;
        //                }
        //            }

        //            // If the item was not found in the list message remains in default state
        //            if (message == "")
        //            {
        //                // Set failure message
        //                message = ("No Ballot Style Found");
        //            }
        //        }
        //        else
        //        {
        //            // Set failure message
        //            message = ("No Items In List");
        //        }
        //    }
        //    else
        //    {
        //        // Set failure message
        //        message = ("No Ballot Style Found");
        //    }

        //    return message;
        //}

        private void TurnOnCorrectButton()
        {
            // Hide the buttons
            HideAllButtons();

            // Only show buttons if a ballot style is selected
            if (BallotStylesList.SelectedIndex >= 0)
            {
                // Choose correct button to display
                switch (_voter_status)
                {
                    case VoterLookupStatus.Eligible:                        
                    case VoterLookupStatus.Ineligible:
                        if(OfficialBallotCheck.IsChecked == true)
                        {
                            // Turn on offical button
                            OfficialBallotButton.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            // Turn on provisional button
                            ProvisionalBallotButton.Visibility = Visibility.Visible;
                        }
                        break;
                    case VoterLookupStatus.Spoilable:
                        // Turn on spoil button
                        SpoilBallotButton.Visibility = Visibility.Visible;
                        break;
                    case VoterLookupStatus.Provisional:
                        // Turn on provisional button
                        ProvisionalBallotButton.Visibility = Visibility.Visible;
                        break;
                    default:
                        
                        break;
                }
            }
        }

        private void HideAllButtons()
        {
            //SaveBallotStyle.Visibility = Visibility.Collapsed;
            ProvisionalBallotButton.Visibility = Visibility.Collapsed;
            OfficialBallotButton.Visibility = Visibility.Collapsed;
            SpoilBallotButton.Visibility = Visibility.Collapsed;
        }

        private void OfficialBallotCheck_Click(object sender, RoutedEventArgs e)
        {
            TurnOnCorrectButton();
        }

        private void BallotStylesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TurnOnCorrectButton();
        }

        private void ProvisionalBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //// Save to quarantine
            //VoterMethods.InsertQuarantine(_voter, "Ballot Style Changed as Provisional");

            //// Hide Hamburger Menu
            ////((MainWindow)System.Windows.Application.Current.MainWindow).HamburgerButton.Visibility = Visibility.Collapsed;

            //// Goto provisional page
            //this.NavigateToPage(new ProvisionalBallotPage(_voter, true));
        }

        private void OfficialBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //// Save to quarantine
            //VoterMethods.InsertQuarantine(_voter, "Ballot Style Changed as Official");

            //// Hide Hamburger Menu
            ////((MainWindow)System.Windows.Application.Current.MainWindow).HamburgerButton.Visibility = Visibility.Collapsed;

            //// Goto official page
            //this.NavigateToPage(new SignatureCapturePage(_voter, true));
        }

        private void SpoilBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //// Save to quarantine
            //VoterMethods.InsertQuarantine(_voter, "Ballot Style Changed as Spoiled");

            //// Hide Hamburger Menu
            ////((MainWindow)System.Windows.Application.Current.MainWindow).HamburgerButton.Visibility = Visibility.Collapsed;

            //// Goto spoiled page
            //this.NavigateToPage(new SpoiledBallotPage(_voter, true));
        }
    }
}
