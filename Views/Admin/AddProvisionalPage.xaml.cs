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
//using VoterX.Core.Models.Database;
//using VoterX.Core.Extensions;
//using VoterX.Core.Models.ViewModels;
using VoterX.Dialogs;
using VoterX.Core.Voters;
using VoterX.Core.Elections;
using VoterX.Core.Extensions;
using VoterX.Manager.Global;
using VoterX.SystemSettings.Enums;
using VoterX.Utilities.Methods;
using VoterX.Methods;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AddProvisionalPage.xaml
    /// </summary>
    public partial class AddProvisionalPage : Page
    {
        //public IEnumerable<avProvisionalReason> provisionalList;

        private bool changesSaved = true;

        public AddProvisionalPage()
        {
            InitializeComponent();

            LoadBallotStyles();

            LoadPrecinctPartsList();

            LoadParties();

            // Load the list of provisional reasons into the combo box
            LoadProvisionalReasons();

            // Display page header
            GlobalReferences.Header.PageHeader = ("Add Provisional Voter");

            //StatusBar.ApplicationCheckPrinterAsync();
            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            CheckServer();
        }

        private async void CheckServer()
        {
            ProvisionalFunctions.Visibility = Visibility.Collapsed;

            if (await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(100) == true)
            {
                ProvisionalFunctions.Visibility = Visibility.Visible;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (changesSaved == false)
            {
                // pop up message "Changes not saved, are you sure you want to exit?"
                // Display message
                YesNoDialog signatureDialog = new YesNoDialog("Changes Not Saved","CHANGES NOT SAVED, ARE YOU SURE YOU WANT TO EXIT?");
                if(signatureDialog.ShowDialog() == true)
                {
                    //MainMenuMethods.OpenMainMenu();
                    GlobalReferences.MenuSlider.Open();

                    this.NavigateToPage(new AdministrationPage());
                }
            }
            else
            {
                //MainMenuMethods.OpenMainMenu();
                GlobalReferences.MenuSlider.Open();

                this.NavigateToPage(new AdministrationPage());
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new LoginPage());
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (await Task.Run(() => VoterMethods.Exists) == true)
            {
                if (HighlightInvalidFields() == false)
                {
                    GlobalReferences.StatusBar.TextLeft = (ValidateEntry().ToString());
                }
                else
                {
                    // Create New Voter Data
                    NMVoter voter = new NMVoter();

                    //voter.Data.VoterID = (await GetNextProvisionalVoterID()).ToString();
                    voter.GetNextProvisionalId((int)AppSettings.System.SiteID);
                    voter.Data.FirstName = FirstName.Text;
                    voter.Data.MiddleName = MiddleName.Text;
                    voter.Data.LastName = LastName.Text;
                    voter.Data.Address1 = Address1.Text;
                    voter.Data.Address2 = Address2.Text;
                    voter.Data.City = CityName.Text;
                    voter.Data.State = StateName.Text;
                    voter.Data.Zip = ZipCode.Text;
                    voter.Data.DOBYear = BirthYear.Text;

                    // Get ballot style
                    if (BallotStylesList.SelectedIndex >= 0)
                    {
                        voter.Data.BallotStyle = ((ComboBoxItem)BallotStylesList.SelectedItem).Content.ToString();
                    }

                    // Get ballot style file
                    if (BallotStylesList.SelectedIndex >= 0)
                    {
                        int ballotstyleid;
                        Int32.TryParse(((ComboBoxItem)BallotStylesList.SelectedItem).DataContext.ToString(), out ballotstyleid);

                        // Get Ballot File name
                        voter.Data.BallotStyleFile = 
                            await Task.Run(() => ElectionDataMethods.BallotStyles
                            .Where(bs => bs.BallotStyleId == ballotstyleid)
                            .FirstOrDefault()
                            .BallotStyleFileName);
                    }

                    //StatusBar.ApplicationStatus(voter.BallotStyle);

                    // Write to Provisional Table
                    if (ProvisionalReason.SelectedIndex >= 0)
                    {
                        int reasonid = GetSelectedItem(ProvisionalReason).ProvisionalReasonId;
                        var result = await Task.Run(() => VoterMethods.InsertProvisionalBallot(voter, reasonid));

                        // Print Provisional Ballot
                        VoterX.Utilities.Methods.BallotPrinting.PrintProvisionalBallot(voter.Data, AppSettings.Global); 
                    }

                    changesSaved = true;

                    // Return to login
                    this.NavigateToPage(new ProvisionalVerifyTroubleshootPage(voter, true));
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
            }
        }

        //private async Task<int> GetNextProvisionalVoterID()
        //{
        //    // Get last voter ID
        //    var lowest_voter_id = await Task.Run(() => VoterMethods.Provisional.GetLowestVoterId());

        //    // Set next voter ID
        //    if (lowest_voter_id == 0 || lowest_voter_id == null)
        //    {
        //        return -1;
        //    }
        //    else
        //    {
        //        return (int)(lowest_voter_id - 1);
        //    }
        //}

        //private bool ValidateEntry()
        //{
        //    int ignoreMe;
        //    return
        //    !FirstName.Text.IsNullOrEmpty() &&
        //    !LastName.Text.IsNullOrEmpty() &&
        //    !Address1.Text.IsNullOrEmpty() &&
        //    !CityName.Text.IsNullOrEmpty() &&
        //    !StateName.Text.IsNullOrEmpty() &&
        //    Int32.TryParse(BirthYear.Text, out ignoreMe);
        //}

        private string ValidateEntry()
        {
            string message = "";
            int ignoreMe;

            if (FirstName.Text.IsNullOrEmpty()) return "Invalid First Name";
            if (LastName.Text.IsNullOrEmpty()) return "Invalid Last Name";
            if (Address1.Text.IsNullOrEmpty()) return "Invalid Addres";
            if (CityName.Text.IsNullOrEmpty()) return "Invalid City";
            if (StateName.Text.IsNullOrEmpty()) return "Invalid State";
            if (ZipCode.Text.IsNullOrEmpty() || !Int32.TryParse(ZipCode.Text, out ignoreMe) || ZipCode.Text.Length < 5) return "Invalid Zipcode";
            if (BirthYear.Text.IsNullOrEmpty() || !Int32.TryParse(BirthYear.Text, out ignoreMe) || BirthYear.Text.Length != 4) return "Invalid Birth Year";
            if (ignoreMe < 1900) return "Invalid Birth Year";

            if (BallotStylesList.SelectedIndex < 0) return "Invalid Ballot Style";

            if (ProvisionalReason.SelectedIndex < 0) return "Invalid Provisional Reason";

            return message;
        }

        // Highlights the invalid fields and returns true of all fields are valid
        private bool HighlightInvalidFields()
        {
            bool result = true;
            //int ignoreMe;

            // The && operator checks the left hand expression first, if false it dosnt check the right hand expression
            // Moved result to the right hand side of the && so it always runs the HighlightRequiredField code
            result = HighlightRequiredField(FirstNameLabel, FirstName.Text.IsNullOrEmpty()) && result;

            result = HighlightRequiredField(LastNameLabel, LastName.Text.IsNullOrEmpty()) && result;

            result = HighlightRequiredField(Address1Label, Address1.Text.IsNullOrEmpty()) && result;

            result = HighlightRequiredField(CityLabel, CityName.Text.IsNullOrEmpty()) && result;

            result = HighlightRequiredField(StateLabel, StateName.Text.IsNullOrEmpty()) && result;

            //int zipcode;
            //result = HighlightRequiredField(ZipCodeLabel,
            //    ZipCode.Text.IsNullOrEmpty() ||
            //    !Int32.TryParse(ZipCode.Text, out zipcode) ||
            //    ZipCode.Text.Length < 5
            //    ) && result;
            result = HighlightRequiredField(ZipCodeLabel,
                ZipCode.Text.IsNullOrEmpty() ||
                ZipCode.Text.Length < 5
                ) && result;

            int birthyear;
            bool isInteger = Int32.TryParse(BirthYear.Text, out birthyear);
            result = HighlightRequiredField(BirthYearLabel,
                BirthYear.Text.IsNullOrEmpty() ||
                !isInteger ||
                BirthYear.Text.Length != 4
                ) && result;
            result = HighlightRequiredField(BirthYearLabel, birthyear < 1900 || birthyear > 2050) && result;

            result = HighlightRequiredField(BallotStyleLabel, BallotStylesList.SelectedIndex < 0) && result;

            result = HighlightRequiredField(ReasonLabel, ProvisionalReason.SelectedIndex < 0) && result;            

            return result;
        }

        // Set the field's label to red if expression is true
        private bool HighlightRequiredField(Border sender, bool expression)
        {
            if(expression == true)
            {
                sender.Background = (Brush)FindResource("ApplicationDangerColor");
            }
            else
            {
                sender.Background = (Brush)FindResource("ApplicationPrimaryColor");
            }
            return !expression;
        }

        // Set the field's label to red if expression is true
        private bool HighlightRequiredField(TextBlock sender, bool expression)
        {
            if (expression == true)
            {
                sender.Background = (Brush)FindResource("ApplicationDangerColor");
            }
            else
            {
                sender.Background = (Brush)FindResource("ApplicationPrimaryColor");
            }
            return !expression;
        }

        private async void LoadBallotStyles()
        {
            // Create animated loading list item
            var loadingItem = Methods.ComboBoxMethods.AddLoadingItem(BallotStylesList, TempLoadingSpinnerItem);

            SaveChanges.Visibility = Visibility.Collapsed;

            if (await Task.Run(() => VoterMethods.Exists) == true)
            {
                var ballotStyleList = ElectionDataMethods.BallotStyles;
                //foreach (var ballotstyle in await Task.Run(() => VoterMethods.BallotStyleMaster.List()))
                foreach (var ballotstyle in ballotStyleList)
                {
                    Methods.ComboBoxMethods.AddComboItemToControl(
                        BallotStylesList,
                        ballotstyle.BallotStyleId,
                        ballotstyle.BallotStyleName,
                        1
                        );
                }
            }

            SaveChanges.Visibility = Visibility.Visible;

            // Remove animated loading list item
            Methods.ComboBoxMethods.RemoveListItem(BallotStylesList, loadingItem);

            BallotStylesList.SelectedIndex = -1;
        }

        private async void LoadPrecinctPartsList()
        {
            // Create animated loading list item
            var loadingItem = Methods.ComboBoxMethods.AddLoadingItem(PrecinctPartsList, TempLoadingSpinnerItem);

            if (await Task.Run(() => VoterMethods.Exists) == true)
            {
                //IEnumerable<tblPrecinctsMaster> PrecinctPartList = await PrecinctMethods.GetPrecinctsListAsync();
                var PrecinctPartList = await Task.Run(() => ElectionDataMethods.Precincts);

                // Sort list numericly instead of alphabeticly 
                if (PrecinctPartList != null)
                {
                    foreach (var precinctPart in PrecinctPartList.OrderBy(x => Double.Parse(x.PrecinctPart)))
                    {
                        Methods.ComboBoxMethods.AddComboItemToControl(
                            PrecinctPartsList,
                            precinctPart.PrecinctPartID,
                            precinctPart.PrecinctPart.ToString(),
                            "001.001"
                            );
                    }
                }
            }

            // Remove animated loading list item
            Methods.ComboBoxMethods.RemoveListItem(PrecinctPartsList, loadingItem);

            PrecinctPartsList.SelectedIndex = -1;
        }

        private void LoadParties()
        {
            foreach (var party in ElectionDataMethods.Partys)
            {
                Methods.ComboBoxMethods.AddComboItemToControl(
                    PartyList,
                    party.PartyCode,
                    party.PartyCode,
                    "DEM"
                    );
            }

            PartyList.SelectedIndex = -1;
        }

        private void PrecinctPartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //changesSaved = false;
            GetNewBallotStyleGeneral(sender, e);
        }       

        private void PartyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //changesSaved = false;
            GetNewBallotStylePrimary(sender, e);
        }

        private async void GetNewBallotStyleGeneral(object sender, SelectionChangedEventArgs e)
        {
            // Convert sender to a ComboBox
            var comboBox = (ComboBox)sender;
            // Check if the combobox is ready
            if (comboBox.IsLoaded)
            {
                // Get the precint part Id
                string precinctPart = Methods.ComboBoxMethods.GetSelectedItemData(PrecinctPartsList).ToString();
                // Get the party name
                string party = Methods.ComboBoxMethods.GetSelectedItem(PartyList);
                // Check if either precinct part id or party name are blank
                if (!precinctPart.IsNullOrEmpty())
                {
                    // Get the Ballot Style Lookup query
                    //var ballotStyleQuery = BallotStyleMethods.GetBallotStyleLookup(precinctPart, party);

                    // Get the Ballot Style Lookup query based on election type
                    var ballotStyleQuery = AppSettings.Election.ElectionType == ElectionType.Primary ?
                        // Primary
                        ElectionDataMethods.GetBallotStyles(precinctPart, party) :
                        // General
                        ElectionDataMethods.GetBallotStyles(precinctPart);
                    // Check of query returns 0 records
                    if (ballotStyleQuery.Count() != 0)
                    {
                        // Collect all ballot styles asynchronously
                        string ballotStyle = await Task.Run(() => ballotStyleQuery.FirstOrDefault().BallotStyleName);
                        // Set Ballot Style list and display results
                        GlobalReferences.StatusBar.TextLeft = (SetBallotStyleSelectedItem(BallotStylesList, ballotStyle));
                    }
                    else
                    {
                        // Set Ballot Style list to blank
                        BallotStylesList.SelectedIndex = -1;
                        // Display failure message
                        GlobalReferences.StatusBar.TextLeft = ("No Ballot Style Found");
                    }
                }
            }
        }

        private void GetNewBallotStylePrimary(object sender, SelectionChangedEventArgs e)
        {
            //// Convert sender to a ComboBox
            //var comboBox = (ComboBox)sender;
            //// Check if the combobox is ready
            //if (comboBox.IsLoaded)
            //{
            //    // Get the precint part Id
            //    string precinctPart = ComboBoxMethods.GetSelectedItemData(PrecinctPartsList).ToString();
            //    // Get the party name
            //    string party = ComboBoxMethods.GetSelectedItem(PartyList);
            //    // Check if either precinct part id or party name are blank
            //    if (!precinctPart.IsNullOrEmpty() && !party.IsNullOrEmpty())
            //    {
            //        // Get the Ballot Style Lookup query
            //        //var ballotStyleQuery = BallotStyleMethods.GetBallotStyleLookup(precinctPart, party);

            //        // Get the Ballot Style Lookup query based on election type
            //        var ballotStyleQuery = AppSettings.Election.ElectionType == 1 ?
            //            // Primary
            //            BallotStyleMethods.GetBallotStyleLookup(precinctPart, party) :
            //            // General
            //            BallotStyleMethods.GetBallotStyleLookup(precinctPart);
            //        // Check of query returns 0 records
            //        if (ballotStyleQuery.Count() != 0)
            //        {
            //            // Collect all ballot styles asynchronously
            //            string ballotStyle = await Task.Run(() => ballotStyleQuery.FirstOrDefault().BallotStyleName);
            //            // Set Ballot Style list and display results
            //            StatusBar.ApplicationStatus(SetBallotStyleSelectedItem(BallotStylesList, ballotStyle));
            //        }
            //        else
            //        {
            //            // Set Ballot Style list to blank
            //            BallotStylesList.SelectedIndex = -1;
            //            // Display failure message
            //            StatusBar.ApplicationStatus("No Ballot Style Found");                        
            //        }
            //    }
            //}
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
                            //message = (item.Content.ToString() + " Found At: " + sender.Items.IndexOf(item).ToString());
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

        private async void LoadProvisionalReasons()
        {
            // Create animated loading list item
            //var loadingItem = ComboBoxMethods.AddLoadingItem(ProvisionalReason, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                // Get list of provisional reasons
                var provisionalList = await Task.Run(() => ElectionDataMethods.ProvisionalReasons.Where(r => r.ProvisionalReasonId < 7));

                // Set combo box source to the list of provisional reasons
                ProvisionalReason.ItemsSource = provisionalList;
            }

            // Remove animated loading list item
            //ComboBoxMethods.RemoveListItem(ProvisionalReason, loadingItem);

            ProvisionalReason.SelectedIndex = -1;
        }

        private void ProvisionalReason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //changesSaved = false;
            if (ProvisionalReason.SelectedIndex != -1)
            {
                // Get Selected reason
                var item = GetSelectedItem((ComboBox)sender);

                // Check if reason is null
                if (item != null)
                {
                    // Display selected reason in the status bar
                    //StatusBar.ApplicationStatus("Reason Selected: " + item.provisional_reason + " ID: " + item.provisional_reason_id);
                    // Turn on the Provisional Save Button
                    SaveChanges.Visibility = Visibility.Visible;
                }
            }
        }

        // Convert ComboBoxItem to a Provisional Reason Object
        private ProvisionalReasonModel GetSelectedItem(ComboBox sender)
        {
            try
            {
                return ((ProvisionalReasonModel)sender.SelectedItem);
            }
            catch
            {
                return null;
            }
        }

        private void ValidateEntryButton_Click(object sender, RoutedEventArgs e)
        {
            if (HighlightInvalidFields() == false)
            {
                GlobalReferences.StatusBar.TextLeft = (ValidateEntry().ToString());
            }
        }

        private void Voter_TextChanged(object sender, TextChangedEventArgs e)
        {
            changesSaved = false;
        }

        private void BallotStylesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //changesSaved = false;
        }
    }
}
