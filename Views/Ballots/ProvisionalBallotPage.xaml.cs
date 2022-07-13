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
using VoterX.Methods;
//using VoterX.Core.Models.Database;
using VoterX.Extensions;
using VoterX.Core.Voters;
using VoterX.Logging;
using VoterX.Core.Elections;
using VoterX.Core.Extensions;
using VoterX.Manager.Views.Absentee;
using VoterX.Manager.Global;
//using VoterX.Core.Extensions;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for ProvisionalBallotPage.xaml
    /// </summary>
    public partial class ProvisionalBallotPage : Page
    {
        //VoterDatabase _VoterDB = ((App)Application.Current).voterContainer.Resolve<VoterDatabase>();

        //public IEnumerable<avProvisionalReason> provisionalList;

        NMVoter _Voter;

        private bool exitLogout = false;

        VoterXLogger provisionalLog;

        public ProvisionalBallotPage(NMVoter voter)
        {
            InitializeComponent();

            // Log page activity
            //LoggingMethods.LogPage("PROVISIONAL BALLOT PAGE LOADED");
            provisionalLog = new VoterXLogger("VCClogs", true);

            // Store voter data localy
            _Voter = voter;

            // Load voter's data to appropriate fields
            LoadVoterFields(voter);

            LoadBallotStyles();

            LoadPrecinctPartsStyles();

            // Load the list of provisional reasons into the combo box
            LoadProvisionalReasons();

            // Display page header
            GlobalReferences.Header.PageHeader = ("Provisional Reason");

            ProvisionalTitle.Text = "PRINT ABSENTEE PROVISIONAL BALLOT FOR";
        }

        public ProvisionalBallotPage(NMVoter voter, bool returnStatus)
        {
            InitializeComponent();

            // Log page activity
            //LoggingMethods.LogPage("PROVISIONAL BALLOT PAGE LOADED");

            // Store voter data localy
            _Voter = voter;
            exitLogout = returnStatus;
            if (returnStatus == true)
            {
                BackReturnLabel.Visibility = Visibility.Collapsed;
                BackLogoutLabel.Visibility = Visibility.Visible;
            }

            // Load voter's data to appropriate fields
            LoadVoterFields(voter);

            LoadBallotStyles();

            LoadPrecinctPartsStyles();

            // Load the list of provisional reasons into the combo box
            LoadProvisionalReasons();

            // Display page header
            GlobalReferences.Header.PageHeader = ("Provisional Reason");
        }

        private void LoadProvisionalReasons()
        {
            if (VoterMethods.Exists == true)
            {
                // Get list of provisional reasons
                //provisionalList = VoterMethods.ProvisionalReasons.List();

                // Set combo box source to the list of provisional reasons
                ProvisionalReason.ItemsSource = ElectionDataMethods.ProvisionalReasons
                    .Where(pr => pr.ProvisionalReasonId != 7)
                    .ToList();
            }
        }

        // Set textbox values to voter details
        private void LoadVoterFields(NMVoter voter)
        {
            FullName.Text = voter.Data.FullName;
            BirthYear.Text = voter.Data.DOBYear;
            Address.Text = voter.Data.Address1;
            CityStateAndZip.Text = voter.Data.City + ", " + voter.Data.State + " " + voter.Data.Zip;
        }

        // Convert ComboBoxItem to a Provisional Reason Object
        private ProvisionalReasonModel GetSelectedItem(ComboBox sender)
        {
            return ((ProvisionalReasonModel)sender.SelectedItem);
        }

        // Return to search screen without any search parameters
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (exitLogout == false)
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
            else
            {
                //MainMenuMethods.OpenAdminMenu();
                GlobalReferences.MenuSlider.Open();
                this.NavigateToPage(new AdministrationPage());
            }
        }

        private void ProvisionalReason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get Selected reason
            var item = GetSelectedItem((ComboBox)sender);

            // Check if reason is null
            if (item != null)
            {
                // Display selected reason in the status bar
                //StatusBar.ApplicationStatus("Reason Selected: " + item.provisional_reason + " ID: " + item.provisional_reason_id);
                // Turn on the Provisional Save Button
                ProvisionalBallot.Visibility = Visibility.Visible;
            }

            //LoggingMethods.LogUser("PROVISIONAL REASON SELECTED: " + item.provisional_reason.ToString());
        }

        // Print a Provisional and Save the details to the Provisional Table
        private void ProvisionalBallot_Click(object sender, RoutedEventArgs e)
        {
            _Voter.Data.BallotStyle = ComboBoxMethods.GetSelectedItem(BallotStylesList).ToString();
            _Voter.Data.BallotStyleFile = ComboBoxMethods.GetSelectedItem(BallotStylesList).ToString() + ".pdf";
            _Voter.Data.BallotStyleID = Int32.Parse(ComboBoxMethods.GetSelectedItemData(BallotStylesList).ToString());

            // Print the Provisional Ballot
            Task.Run(() =>
            {
                BallotPrinting.PrintProvisionalBallot(_Voter.Data);
            });

            VoterMethods.InsertProvisionalBallot(_Voter, GetSelectedItem(ProvisionalReason).ProvisionalReasonId);

            // Verify the ballot printed
            // If exitLogout is true then user is coming from an admin page
            if (exitLogout == true)
            {
                this.NavigateToPage(new ProvisionalVerifyTroubleshootPage(_Voter, true));
            }
            else
            {
                this.NavigateToPage(new ProvisionalVerifyTroubleshootPage(_Voter));
            }
        }

        private async void LoadBallotStyles()
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(BallotStylesList, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                //int voterBallotID = _Voter.BallotStyleID == null ? -1 : (int)_Voter.BallotStyleID;

                //StatusBar.StatusTextLeft = voterBallotID.ToString();

                var ballotStyleList = await Task.Run(() =>
                {
                    try
                    {
                        return ElectionDataMethods.BallotStyles;
                    }
                    catch (Exception e)
                    {
                        provisionalLog.WriteLog("Downloading Ballot Styles: " + e.Message);
                        return null;
                    }
                }
                );

                foreach (var ballotstyle in await Task.Run(() => ballotStyleList))
                {
                    ComboBoxMethods.AddComboItemToControl(
                            BallotStylesList,
                            ballotstyle.BallotStyleId.ToString(),
                            ballotstyle.BallotStyleName,
                            _Voter.Data.BallotStyle
                            );
                }
            }
            else
            {
                //StatusBar.StatusTextCenter = "Database not found";
            }

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(BallotStylesList, loadingItem);

            //BallotStylesList.SelectedIndex = -1;

            BallotStylesList.SelectedIndex = ComboBoxMethods.FindItemIndex(BallotStylesList, _Voter.Data.BallotStyle);
        }

        private async void LoadPrecinctPartsStyles()
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(PrecinctPartsList, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var PrecinctPartList = await Task.Run(() => ElectionDataMethods.Precincts);
                // Sort list numericly instead of alphabeticly            
                foreach (var precinctPart in PrecinctPartList.OrderBy(x => Double.Parse(x.PrecinctPart)))
                {
                    try
                    {
                        ComboBoxMethods.AddComboItemToControl(
                            PrecinctPartsList,
                            precinctPart.PrecinctPartID,
                            precinctPart.PrecinctPart.ToString(),
                            _Voter.Data.PrecinctPartID
                            );
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                //StatusBar.StatusTextCenter = "Database not found";
            }

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(PrecinctPartsList, loadingItem);

            //PrecinctPartsList.SelectedIndex = -1;
        }

        private void PrecinctPartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PrecinctPartsList.IsLoaded && BallotStylesList.IsLoaded)
            {
                //GetNewBallotStyle(sender, e);

                // Get the precint part Id
                string precinctPart = ComboBoxMethods.GetSelectedItemData(PrecinctPartsList).ToString();
                // Get the ballot style
                var ballotStyle = ElectionDataMethods.BallotStyles.Where(bs => bs.PrecinctPartID == precinctPart).FirstOrDefault();
                // Set the selected item to new ballot style
                SetBallotStyleSelectedItem(BallotStylesList, ballotStyle.BallotStyleName);
            }
        }

        private void BallotStylesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //StatusBar.StatusTextLeft = _Voter.BallotStyleID.ToString();

            if (BallotStylesList.IsLoaded)
            {
                //if (await Task.Run(() => VoterMethods.Voter.Exists()) == true)
                //{
                //    int ballotstyleid = ComboBoxMethods.GetSelectedItemNumber(BallotStylesList);

                //    var newballotstyle = VoterMethods.BallotStyleMaster.GetByID(ballotstyleid);

                //    _Voter.BallotStyleID = newballotstyle.ballot_style_id;
                //    _Voter.BallotStyle = newballotstyle.ballot_style_name;
                //    _Voter.BallotStyleFile = newballotstyle.ballot_style_filename;
                //}
                //else
                //{
                //    StatusBar.ApplicationStatusCenter("Database not found");
                //}
            }
        }

        private void GetNewBallotStyle(object sender, SelectionChangedEventArgs e)
        {
            // Convert sender to a ComboBox
            var comboBox = (ComboBox)sender;
            // Check if the combobox is ready
            if (comboBox.IsLoaded)
            {
                //if (await Task.Run(() => VoterMethods.Voter.Exists()) == true)
                //{
                //    // Get the precint part Id
                //    string precinctPart = ComboBoxMethods.GetSelectedItemData(PrecinctPartsList).ToString();

                //    // Fix after general election
                //    // Get the party name
                //    //string party = ComboBoxMethods.GetSelectedItem(PartyList);
                //    string party = null;

                //    // Check if either precinct part id or party name are blank
                //    if (!precinctPart.IsNullOrEmpty() && (!party.IsNullOrEmpty() || AppSettings.Election.ElectionType == 2))
                //    {
                //        // Get the Ballot Style Lookup query
                //        var ballotStyleQuery = AppSettings.Election.ElectionType == 2 ?
                //            // General Election Type is not Party dependant
                //            BallotStyleMethods.GetBallotStyleLookup(precinctPart)
                //            :
                //            // Primary Election Type is party dependant
                //            BallotStyleMethods.GetBallotStyleLookup(precinctPart, party);
                //        // Check of query returns 0 records
                //        if (ballotStyleQuery.Count() != 0)
                //        {
                //            // Collect all ballot styles asynchronously
                //            string ballotStyle = await Task.Run(() => ballotStyleQuery.FirstOrDefault().BallotStyleName);
                //            // Set Ballot Style list and display results
                //            string message = SetBallotStyleSelectedItem(BallotStylesList, ballotStyle);
                //            //StatusBar.StatusTextLeft = message;
                //            //ComboBoxMethods.SetSelectedItem(BallotStylesList, ballotStyle);
                //        }
                //        else
                //        {
                //            // Set Ballot Style list to blank
                //            BallotStylesList.SelectedIndex = -1;
                //            // Display failure message
                //            StatusBar.ApplicationStatusCenter("No Ballot Style Found");
                //        }
                //    }
                //}
                //else
                //{
                //    StatusBar.ApplicationStatusCenter("Database not found");
                //}
            }
        }

        private string SetBallotStyleSelectedItem(ComboBox sender, string ballotStyle)
        {
            // Create blank message
            string message = "";

            // Check if ballot style is empty
            if (!ballotStyle.IsNullOrEmpty())
            {
                // Check if the destination list is empty
                if (sender.Items.Count > 0)
                {
                    // Loop through list items
                    foreach (ComboBoxItem item in sender.Items)
                    {
                        // Compare list item to the string provided
                        if (item.Content.ToString() == ballotStyle)
                        {
                            // Set the destination selected item
                            sender.SelectedIndex = sender.Items.IndexOf(item);
                            // Set message to location of item found
                            message = (item.Content.ToString() + " Found At: " + sender.Items.IndexOf(item).ToString());
                            // Break out of the loop
                            return message;
                        }
                    }

                    // If the item was not found in the list message remains in default state
                    if (message == "")
                    {
                        // Set failure message
                        message = ("No Ballot Style Found");
                    }
                }
                else
                {
                    // Set failure message
                    message = ("No Items In List");
                }
            }
            else
            {
                // Set failure message
                message = ("No Ballot Style Found");
            }

            return message;
        }
    }
}
