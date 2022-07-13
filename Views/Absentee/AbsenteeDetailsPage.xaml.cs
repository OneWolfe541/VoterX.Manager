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
//using VoterX.Core.Models.Utilities;
//using VoterX.Core.Models.ViewModels;
using VoterX.Methods;
//using VoterX.Core.Models.Database;
using VoterX.Core.Extensions;
using VoterX.Extensions;
using VoterX.Dialogs;
using VoterX.Core.Voters;
using VoterX.Core.Elections;
using VoterX.Core.Utilities;
using VoterX.Logging;
using VoterX.Manager.Global;
using VoterX.Utilities.Models;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeDetailsPage.xaml
    /// </summary>
    public partial class AbsenteeDetailsPage : Page
    {
        // Custom Commands
        // http://web.archive.org/web/20121127022259/http://www.switchonthecode.com/tutorials/wpf-tutorial-command-bindings-and-custom-commands
        //public static RoutedCommand MyCommand = new RoutedCommand();

        private VoterSearchModel search = new VoterSearchModel();
        private NMVoter _voter = new NMVoter();

        //private int changecounter = 0;
        //private string changeitem = "";

        private bool SERVISSource = false;

        public AbsenteeDetailsPage(VoterNavModel voterFromNav) : this(voterFromNav, false) { }
        public AbsenteeDetailsPage(VoterNavModel voterFromNav, bool source)
        {
            InitializeComponent();

            search = voterFromNav.Search;

            _voter = voterFromNav.Voter;

            SERVISSource = source;

            LoadVoterFields(voterFromNav.Voter);

            LoadBallotStyles(voterFromNav.Voter.Data);

            LoadPrecinctPartsList(voterFromNav.Voter.Data);

            LoadLogCodes(voterFromNav.Voter.Data);

            LoadPollSites(voterFromNav.Voter.Data);

            //if (AppSettings.Absentee.ShowSliderIcons == true)
            //    MainMenuMethods.SetMenuMinWidth(55);

            //MainMenuMethods.ShowMenuMinimum();

            // Check if voter was deleted
            if (voterFromNav.Voter.Data.LogCode == 17)
            {
                //MainMenuMethods.LoadMenu(new VoterDeletedMenu(this));
                GlobalReferences.MenuSlider.SetMenu(new VoterDeletedMenu(this), MenuCollapseMode.ShowIcons);
            }
            else
            {
                //MainMenuMethods.LoadMenu(new VoterDetailsMenu(this));
                GlobalReferences.MenuSlider.SetMenu(new VoterDetailsMenu(this), MenuCollapseMode.ShowIcons);
            }
            GlobalReferences.MenuSlider.Open();

            // Display page header
            GlobalReferences.Header.PageHeader = "Voter Details";

            CheckServer();

            GlobalReferences.StatusBar.TextLeft = ("Debug: " + ((App)Application.Current).debugMode.ToString());
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.SetMenuMinWidth(0);

            //MainMenuMethods.ShowMenuMinimum();
        }

        //private void MyCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = true;
        //}

        //private void MyCommandExecute(object sender, ExecutedRoutedEventArgs e)
        //{
        //    SaveChanges();
        //}

        private async void CheckServer()
        {
            //StatusBar.ApplicationDatabaseChecking();
            if (await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(100))
            {

                //string message = await ServerMethods.CheckConnection(new ElectionFactory());

                //if (message == "False")
                //{
                //    StatusBar.ApplicationStatusLeft("Could not connect to database");

                //    StatusBar.ApplicationDatabaseStatus(false);

                //    Console.WriteLine("Details Error Alert Called");
                //    AlertDialog messageBox = new AlertDialog("AN ERROR WAS ENCOUNTERED PLEASE CONTACT YOUR ADMINISTRATOR");
                //    messageBox.ShowDialog();
                //}
                //else
                //{
                //    StatusBar.ApplicationDatabaseStatus(true);
                //    StatusBar.ApplicationStatusClear();
                //}
            }
            else
            {
                Console.WriteLine("Details Error Alert Called");
                AlertDialog messageBox = new AlertDialog("AN ERROR WAS ENCOUNTERED PLEASE CONTACT YOUR ADMINISTRATOR");
                messageBox.ShowDialog();
            }
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //StatusBar.ApplicationStatus("CTRL + S");
            SaveChanges();
        }

        private void LoadVoterFields(NMVoter voter)
        {
            VoterId.Text = voter.Data.VoterID;
            LastName.Text = voter.Data.LastName;
            FirstName.Text = voter.Data.FirstName;
            MiddleName.Text = voter.Data.MiddleName;
            BirthYear.Text = voter.Data.DOBYear;
            Gender.Text = voter.Data.Gender;
            Generation.Text = voter.Data.Generation;
            VoterTitle.Text = voter.Data.Title;
            Party.Text = voter.Data.Party;

            MailingAddress1.Text = voter.Data.MailingAddress1;
            MailingAddress2.Text = voter.Data.MailingAddress2;
            MailingCity.Text = voter.Data.MailingCity;
            MailingState.Text = voter.Data.MailingState;
            MailingZip.Text = voter.Data.MailingZip;
            MailingCountry.Text = voter.Data.MailingCountry;

            PhysicalAddress1.Text = voter.Data.Address1;
            PhysicalAddress2.Text = voter.Data.Address2;
            PhysicalCity.Text = voter.Data.City;
            PhysicalState.Text = voter.Data.State;
            PhysicalZip.Text = voter.Data.Zip;

            DeliveryAddress1.Text = voter.Data.DeliveryAddress1;
            DeliveryAddress2.Text = voter.Data.DeliveryAddress2;
            DeliveryCity.Text = voter.Data.DeliveryCity;
            DeliveryState.Text = voter.Data.DeliveryState;
            DeliveryZip.Text = voter.Data.DeliveryZip;
            DeliveryCountry.Text = voter.Data.DeliveryCountry;

            IdRequiredCheck.IsChecked = voter.Data.IDRequired;
            VoterStatus.Text = voter.Data.Status;
            // Voter Type
            SetAbsenteeType(voter.Data.AbsenteeType);

            LogDate.Text = voter.Data.ActivityDate.ToString();
            Computer.Text = voter.Data.ComputerID.ToString();

            ApplicationIssuedDate.Text = voter.Data.ApplicationIssued.ToString();
            ApplicationRejectedDate.Text = voter.Data.ApplicationRejected.ToString();
            ApplicationAcceptedDate.Text = voter.Data.ApplicationAccepted.ToString();

            BallotIssuedDate.Text = voter.Data.BallotIssued.ToString();
            BallotPrintedDate.Text = voter.Data.BallotPrinted.ToString();
        }

        public void SetAbsenteeType(string type)
        {
            switch (type)
            {
                case "R":
                    RegularVoterType.IsSelected = true;
                    break;
                case "U":
                    UniformedVoterType.IsSelected = true;
                    break;
                case "O":
                    OverseasVoterType.IsSelected = true;
                    break;
                case "E":
                    EmergencyVoterType.IsSelected = true;
                    break;
                default:
                    VoterTypeList.SelectedIndex = -1;
                    break;
            }
        }

        public void GoBack()
        {
            if (SERVISSource == true)
            {
                // Navigate to SERVIS lookup screen
                MainFrameMethods.NavigateToPage(new SearchPage(0));
            }
            else
            {
                // Else go to normal search page
                MainFrameMethods.NavigateToPage(new SearchPage());
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Voter_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void LoadBallotStyles(VoterDataModel voter)
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(BallotStylesList, TempLoadingSpinnerItem);

            if (((App)Application.Current).Election != null)
            {
                try
                {
                    foreach (var ballotstyle in await Task.Run(() => ((App)Application.Current).Election.Lists.BallotStyles))
                    {
                        ComboBoxMethods.AddComboItemToControl(
                            BallotStylesList,
                            ballotstyle.BallotStyleId.ToString(),
                            ballotstyle.BallotStyleName,
                            voter.BallotStyle
                            );
                    }
                }
                catch (Exception error)
                {
                    //StatusBar.ApplicationStatusCenter(error.Message);
                    GlobalReferences.StatusBar.TextCenter = error.Message;
                }
            }

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(BallotStylesList, loadingItem);

            BallotStylesList.SelectedIndex = ComboBoxMethods.FindItemIndex(BallotStylesList, voter.BallotStyle);
        }

        private async void LoadPrecinctPartsList(VoterDataModel voter)
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(PrecinctPartsList, TempLoadingSpinnerItem);

            if (((App)Application.Current).Election != null)
            {
                try
                {
                    IEnumerable<PrecinctModel> PrecinctPartList = await Task.Run(() => ((App)Application.Current).Election.Lists.Precincts);
                    foreach (var precinctPart in PrecinctPartList.OrderBy(x => Double.Parse(x.PrecinctPart)))
                    {
                        try
                        {
                            ComboBoxMethods.AddComboItemToControl(
                                PrecinctPartsList,
                                precinctPart.PrecinctPartID,
                                precinctPart.PrecinctPart.ToString(),
                                voter.PrecinctPartID
                                );
                        }
                        catch (Exception error)
                        {
                            //StatusBar.ApplicationStatusCenter(error.Message);
                            GlobalReferences.StatusBar.TextCenter = error.Message;
                        }
                    }
                }
                catch (Exception error)
                {
                    //StatusBar.ApplicationStatusCenter(error.Message);
                    GlobalReferences.StatusBar.TextCenter = error.Message;
                }
            }

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(PrecinctPartsList, loadingItem);
        }

        //private void LoadParties()
        //{
        //    foreach (var party in PartyMethods.GetPrimaryPartyList())
        //    {
        //        ComboBoxMethods.AddComboItemToControl(
        //            PartyList,
        //            party,
        //            party,
        //            "DEM"
        //            );
        //    }

        //    PartyList.SelectedIndex = -1;
        //}

        private void PrecinctPartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //changesSaved = false;
            //GetNewBallotStyle(sender, e);
        }

        private void PartyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //changesSaved = false;
            //GetNewBallotStyle(sender, e);
        }

        //private async void GetNewBallotStyle(object sender, SelectionChangedEventArgs e) 
        //{
        //    // Convert sender to a ComboBox
        //    var comboBox = (ComboBox)sender; 
        //    // Check if the combobox is ready
        //    if (comboBox.IsLoaded)
        //    {
        //        if (await Task.Run(() => VoterMethods.PrecinctMaster.Exists(0)) == true) 
        //        {
        //            //try
        //            {
        //                // Get the precint part Id
        //                string precinctPart = ComboBoxMethods.GetSelectedItemData(PrecinctPartsList).ToString();
        //                // Get the party name
        //                //string party = ComboBoxMethods.GetSelectedItem(PartyList);
        //                string party = Party.Text;
        //                // Check if either precinct part id or party name are blank
        //                if (!precinctPart.IsNullOrEmpty() && !party.IsNullOrEmpty())
        //                {
        //                    // Get the Ballot Style Lookup query based on election type
        //                    var ballotStyleQuery = AppSettings.Election.ElectionType == 1 ?
        //                        // Primary
        //                        BallotStyleMethods.GetBallotStyleLookup(precinctPart, party) :
        //                        // General
        //                        BallotStyleMethods.GetBallotStyleLookup(precinctPart);
        //                    // Check of query returns 0 records                            
        //                    if (ballotStyleQuery!=null && ballotStyleQuery.Count() != 0)
        //                    {
        //                        // Collect all ballot styles asynchronously
        //                        if (await Task.Run(() => VoterMethods.BallotStyleMaster.Exists(0)) == true)
        //                        {
        //                            string ballotStyle = await Task.Run(() => ballotStyleQuery.FirstOrDefault().BallotStyleName);
        //                            // Set Ballot Style list and display results
        //                            try
        //                            {
        //                                SetBallotStyleSelectedItem(BallotStylesList, ballotStyle);
        //                            }
        //                            catch (Exception error)
        //                            {
        //                                StatusBar.ApplicationStatusCenter(error.Message);
        //                            }
        //                            //StatusBar.ApplicationStatus(SetBallotStyleSelectedItem(BallotStylesList, ballotStyle));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Set Ballot Style list to blank
        //                        BallotStylesList.SelectedIndex = -1;
        //                        // Display failure message
        //                        StatusBar.ApplicationStatus("No Ballot Style Found for: " + precinctPart);

        //                        if (ballotStyleQuery == null) StatusBar.ApplicationStatus("NULL Query");
        //                    }
        //                }
        //            }
        //            //catch (Exception error)
        //            //{
        //            //    StatusBar.ApplicationStatusCenter(error.Message);
        //            //}
        //        }
        //    }
        //}

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
                        message = ("No Ballot Style Found: " + ballotStyle);
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

        private void BallotStylesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Track ballot style changes
            //changecounter++;
            //if (BallotStylesList.SelectedIndex >= 0)
            //{
            //    changeitem += ((ComboBoxItem)BallotStylesList.SelectedItem).Content.ToString() + " | ";
            //}
            //else
            //{
            //    changeitem += " -1 ";
            //}
            //StatusBar.ApplicationStatus(changecounter.ToString() + " " + changeitem);
        }

        private async void LoadLogCodes(VoterDataModel voter)
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(LogCodesList, TempLoadingSpinnerItem);

            Int32.TryParse(voter.LogCode.ToString(), out int logcode);

            if (((App)Application.Current).Election != null)
            {
                try
                {
                    foreach (var logcodes in await Task.Run(() => ((App)Application.Current).Election.Lists.LogCodes))
                    {
                        ComboBoxMethods.AddComboItemToControl(
                            LogCodesList,
                            logcodes.LogCode,
                            logcodes.LogDescription,
                            logcode
                            );
                    }
                }
                catch (Exception error)
                {
                    //StatusBar.ApplicationStatusCenter(error.Message);
                    GlobalReferences.StatusBar.TextCenter = error.Message;
                }
            }

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(LogCodesList, loadingItem);

            if (voter.LogCode == null) LogCodesList.SelectedIndex = -1;
        }

        private async void LoadPollSites(VoterDataModel voter)
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(SitesList, TempLoadingSpinnerItem);

            int siteid = -1;
            // TryParse turns null to 0
            // and 0 is a valid value in the select list
            if (voter.PollID != null) Int32.TryParse(voter.PollID.ToString(), out siteid);

            if (((App)Application.Current).Election != null)
            {
                try
                {
                    foreach (var locations in await Task.Run(() => ((App)Application.Current).Election.Lists.Locations))
                    {
                        ComboBoxMethods.AddComboItemToControl(
                            SitesList,
                            locations.PollId,
                            locations.PlaceName,
                            siteid
                            );
                    }
                }
                catch (Exception error)
                {
                    //StatusBar.ApplicationStatusCenter(error.Message);
                    GlobalReferences.StatusBar.TextCenter = error.Message;
                }
            }

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(SitesList, loadingItem);

            if (voter.PollID == null) LogCodesList.SelectedIndex = -1;
        }

        private void ProvisionalReason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ValidateEntryButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogCodesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void VoterTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((ComboBoxItem)((ComboBox)sender).SelectedItem).DataContext;
            
            if(item.ToString() == "R" || item.ToString() == "E")
            {
                Uocava_panel1.Visibility = Visibility.Collapsed;
                Uocava_panel2.Visibility = Visibility.Collapsed;
                Uocava_panel3.Visibility = Visibility.Collapsed;
            }
            else
            {
                Uocava_panel1.Visibility = Visibility.Visible;
                Uocava_panel2.Visibility = Visibility.Visible;
                Uocava_panel3.Visibility = Visibility.Visible;
            }
        }

        //public VoterSearchModel GetSearchParameters()
        //{
        //    return search;
        //}

        //public VoterDataModel GetVoter()
        //{
        //    return _voter;
        //}

        public bool ValidateRecord()
        {
            return false;
        }

        public bool SaveChanges()
        {            
            //StatusBar.ApplicationStatus("Changes Saved"); 
            return false;
        }

        public async void PrintApplication()
        {
            if (_voter.Data.BallotStyleID == null)
            {
                AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Ineligible Voter");
            }
            else if (_voter.HasVoted())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Voter Has Already Voted");
            }
            else
            {
                //SetLocalMailingAddress();
                if (GetAddressFromDialog(2))
                {
                    // Set local values
                    _voter.Data.ElectionID = AppSettings.Election.ElectionID;
                    _voter.Data.PollID = AppSettings.Absentee.SiteID;
                    _voter.Data.ComputerID = AppSettings.System.MachineID;
                    _voter.Data.UserId = AppSettings.User.UserID;

                    // Do not reset application issued date
                    if (_voter.Data.ApplicationIssued == null)
                    {
                        // Mark absentee record as "Application Issued"
                        _voter.Data.ApplicationIssued = DateTime.Now;
                        MarkIssuedApplication();

                        GlobalReferences.StatusBar.TextLeft = 
                            await Task.Run(() =>
                            BallotPrinting.PrintAbsenteeApplicationPre(_voter.Data)
                            );
                    }
                    else
                    {
                        // Ask user "Are You Sure"
                        YesNoDialog newMessage = new YesNoDialog("Resubmit Application", "ARE YOU SURE YOU WANT TO PRINT ANOTHER APPLICATION?");
                        if (newMessage.ShowDialog() == true)
                        {
                            MarkIssuedApplication();
                            GlobalReferences.StatusBar.TextLeft = BallotPrinting.PrintAbsenteeApplicationPre(_voter.Data);
                        }
                    }
                }
            }
        }

        private async void MarkIssuedApplication()
        {
            if (VoterTypeList.SelectedIndex >= 0)
            {
                _voter.Data.AbsenteeType = ((ComboBoxItem)VoterTypeList.SelectedItem).DataContext.ToString();
            }
            else
            {
                _voter.Data.AbsenteeType = "R";
                VoterTypeList.SelectedIndex = 0;
            }
            if (_voter.Data.IDRequired == true)
            {
                AlertDialog newMessage = new AlertDialog("A VALID ID IS REQUIRED FOR THIS VOTER");
                newMessage.ShowDialog();
            }
            _voter.Data.LogCode = 3;
            _voter.Data.LogDescription = "Issued AV Application by Mail";
            _voter.Data.ActivityDate = DateTime.Now;
            _voter.Data.LogDate = DateTime.Now;
            _voter.Data.ComputerID = AppSettings.System.MachineID;
            _voter.Data.PollID = AppSettings.Absentee.SiteID;
            LogCodesList.SelectedIndex = 2; // Selected index = #LogCode - 1
            LogDate.Text = DateTime.Now.ToString();
            // Prevent Poll Location drop down list from changing if Application is Reissued
            if (SitesList.SelectedIndex < 0)
            {
                ComboBoxMethods.SetSelectedItem(SitesList, AppSettings.Absentee.SiteName);
            }
            Computer.Text = AppSettings.System.MachineID.ToString();
            ApplicationIssuedDate.Text = _voter.Data.ApplicationIssued.ToString();
            if (await Task.Run(() => VoterMethods.Voters.Exists()) == true)
            {
                // NEED TO ADD APPLICATION ISSUED METHOD TO NMVOTER OBJECT
                //VoterMethods.InsertOrUpdateApplicationIssued(_voter);

                try
                {
                    _voter.IssueApplication();
                }
                catch (Exception e)
                {
                    // Log Exception
                    GlobalReferences.StatusBar.TextCenter = (e.Message);
                    VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                    ABSLogger.WriteLog("Error Issuing Application: " + e.Message);
                    if (e.InnerException != null)
                    {
                        //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                        ABSLogger.WriteLog(e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                            ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                        }
                    }
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
            }
        }

        public async Task PrintMailingLabelAsync()
        {
            SetLocalMailingAddress();

            // WHY AM I CHECKING THE LOG CODE HERE?
            if (_voter.Data.LogCode == 1 || _voter.Data.LogCode == null)
            {
                if (GetAddressFromDialog(3))
                {
                    GlobalReferences.StatusBar.TextCenter = (
                    await Task.Run(() => { return ReportPrintingMethods.PrintVoterMailingLabels(_voter.Data); }) 
                    );
                }
            }
            else
            {
                if (GetAddressFromDialog(3))
                {
                    GlobalReferences.StatusBar.TextCenter = (
                    await Task.Run(() => { return ReportPrintingMethods.PrintVoterMailingLabels(_voter.Data); })
                    );
                }
            }
        }

        // Print Mail-In Bundle
        public void PrintMailingBundle()
        {
            if (_voter.Data.BallotStyleID == null)
            {
                AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Ineligible Voter");
            }
            else if (_voter.HasVoted() && _voter.Data.LogCode != 5) // Allow 5's to print a ballot from this screen
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Voter Has Already Voted");
            }
            else if (_voter.HasRejectedApplication())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS AN UNRESOLVED REJECTED APPLICATION");
                newMessage.ShowDialog();
            }
            // Copied from All-Mail application (10/28/2019)
            //else if (_voter.IdRequired())
            //{
            //    AlertDialog newMessage = new AlertDialog("A VALID ID IS REQUIRED FOR THIS VOTER\r\nONLY A PROVISIONAL BALLOT CAN BE PRINTED");
            //    newMessage.ShowDialog();
            //}
            else
            {
                // Check for ID required (10/28/2019)
                if (_voter.IdRequired().Value)
                {
                    YesNoDialog newMessage = new YesNoDialog("Id Required", "A VALID ID IS REQUIRED FOR THIS VOTER\r\nWOULD YOU LIKE TO CONTINUE?");
                    if(newMessage.ShowDialog() == false)                    
                    {
                        return;
                    }
                }

                if (GetAddressFromDialog(1))
                {
                    //SetLocalMailingAddress();

                    // Set local values
                    _voter.Data.ElectionID = AppSettings.Election.ElectionID;
                    _voter.Data.PollID = AppSettings.Absentee.SiteID;
                    _voter.Data.ComputerID = AppSettings.System.MachineID;
                    _voter.Data.UserId = AppSettings.User.UserID;

                    if (_voter.Data.ApplicationIssued == null) 
                    {
                        YesNoDialog newMessage = new YesNoDialog("No Application", "THIS VOTER HAS NOT BEEN ISSUED AN APPLICATION\rFROM THE VoterX SYSTEM\r\n\r\nARE YOU SURE YOU WANT TO ISSUE A BALLOT?");
                        if (newMessage.ShowDialog() == true)
                        {
                            YesNoDialog applicationQuestion = new YesNoDialog("Extra Application", "WOULD YOU LIKE TO PRINT APPLICATION\r\nALONG WITH THIS BALLOT?");
                            if (applicationQuestion.ShowDialog() == true)
                            {
                                BallotPrinting.PrintAbsenteeApplicationPre(_voter.Data);
                            }
                            // Add EDDY fix here
                            PrintMailBundleAndIssueApplication();
                        }
                    }
                    else
                    {
                        YesNoDialog newMessage = new YesNoDialog("No Application", "THIS VOTERS APPLICATION WILL BE ACCEPTED\r\nAND A BALLOT WILL BE PRINTED.\r\nARE YOU SURE YOU WANT TO CONTINUE?");
                        if (newMessage.ShowDialog() == true)
                        {
                            // Add EDDY fix here
                            PrintMailBundleAndAcceptApplication();
                        }
                    }
                }
            }
        }

        private async void PrintMailBundleAndIssueApplication()
        {
            if (await Task.Run(() => VoterMethods.Exists == true))
            {

                // Set absentee type
                if (VoterTypeList.SelectedIndex >= 0)
                {
                    _voter.Data.AbsenteeType = ((ComboBoxItem)VoterTypeList.SelectedItem).DataContext.ToString();
                }
                else
                {
                    _voter.Data.AbsenteeType = "R";
                    VoterTypeList.SelectedIndex = 0;
                }

                DateTime issuedDate = DateTime.Now;

                // Mark absentee record as "Ballot Issued" 
                // date_issued and printed_date
                // also LogCode #6 "Issued Absentee Ballot by Mail"
                //if (await Task.Run(() => VoterMethods.Exists) == true)
                {
                    //SetLocalMailingAddress();

                    // Display changes
                    _voter.Data.LogCode = 6;
                    _voter.Data.LogDescription = "Issued Absentee Ballot by Mail";
                    _voter.Data.ActivityDate = DateTime.Now;
                    _voter.Data.LogDate = DateTime.Now;
                    _voter.Data.ComputerID = AppSettings.System.MachineID;
                    _voter.Data.PollID = AppSettings.Absentee.SiteID;
                    // Mark ballot issued & accepted today
                    _voter.Data.ApplicationIssued = DateTime.Now;
                    _voter.Data.ApplicationAccepted = DateTime.Now;
                    LogCodesList.SelectedIndex = 5; // Selected index = #LogCode - 1
                    LogDate.Text = issuedDate.ToString();
                    ApplicationIssuedDate.Text = issuedDate.ToString();
                    BallotIssuedDate.Text = issuedDate.ToString();
                    BallotPrintedDate.Text = issuedDate.ToString();
                    ComboBoxMethods.SetSelectedItem(SitesList, AppSettings.Absentee.SiteName);
                    Computer.Text = AppSettings.System.MachineID.ToString();
                    ApplicationAcceptedDate.Text = issuedDate.ToString();

                    // Get new ballot number
                    //_voter.Data.BallotNumber = VoterMethods.Voter.GetNextBallotNumber((int)AppSettings.System.SiteID);
                    _voter.GetNextBallotNumber((int)AppSettings.System.SiteID);

                    //await VoterMethods.InsertOrUpdateBallotIssued(_voter, issuedDate);
                    try
                    {
                        _voter.IssueBallot();
                    }
                    catch (Exception e)
                    {
                        // Log Exception
                        GlobalReferences.StatusBar.TextCenter = (e.Message);
                        VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                        ABSLogger.WriteLog("Error Issuing Ballot: " + e.Message);
                        if (e.InnerException != null)
                        {
                            //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                            ABSLogger.WriteLog(e.InnerException.Message);
                            if (e.InnerException.InnerException != null)
                            {
                                //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                                ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                            }
                        }
                    }

                    // Print Mailing Labels
                    for (int i = 0; i < AppSettings.Ballots.LabelCount; i++)
                    {
                        //ReportPrintingMethods.PrintVoterMailingLabel(_voter);
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintVoterMailingLabels(_voter.Data)); 
                    }

                    // Print Ballot
                    BallotPrinting.ReprintBallot(_voter.Data);

                    // BALLOT STUBS FOR EDDY ONLY
                    if (AppSettings.System.BallotStub == 1)
                    {
                        BallotPrinting.ReprintStub(_voter.Data);
                        // Second label is for Eddy only
                        //ReportPrintingMethods.PrintVoterMailingLabel(_voter);
                    }
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database Error");
            }
        }

        private async void PrintMailBundleAndAcceptApplication()
        {
            if (await Task.Run(() => VoterMethods.Exists == true))
            {
                // Set absentee type
                if (VoterTypeList.SelectedIndex >= 0)
                {
                    _voter.Data.AbsenteeType = ((ComboBoxItem)VoterTypeList.SelectedItem).DataContext.ToString();
                }
                else
                {
                    _voter.Data.AbsenteeType = "R";
                    VoterTypeList.SelectedIndex = 0;
                }

                DateTime issuedDate = DateTime.Now;

                // Mark absentee record as "Ballot Issued" 
                // date_issued and printed_date
                // also LogCode #6 "Issued Absentee Ballot by Mail"
                //if (await Task.Run(() => VoterMethods.Exists) == true)
                {
                    //SetLocalMailingAddress();

                    // Display changes
                    _voter.Data.LogCode = 6;
                    _voter.Data.LogDescription = "Issued Absentee Ballot by Mail";
                    _voter.Data.ActivityDate = DateTime.Now;
                    _voter.Data.LogDate = DateTime.Now;
                    _voter.Data.ComputerID = AppSettings.System.MachineID;
                    _voter.Data.PollID = AppSettings.Absentee.SiteID;
                    // Mark application accepted today
                    _voter.Data.ApplicationAccepted = DateTime.Now;
                    LogCodesList.SelectedIndex = 5; // Selected index = #LogCode - 1
                    LogDate.Text = issuedDate.ToString();
                    BallotIssuedDate.Text = issuedDate.ToString();
                    BallotPrintedDate.Text = issuedDate.ToString();
                    ComboBoxMethods.SetSelectedItem(SitesList, AppSettings.Absentee.SiteName);
                    Computer.Text = AppSettings.System.MachineID.ToString();
                    ApplicationAcceptedDate.Text = issuedDate.ToString();

                    //await VoterMethods.InsertOrUpdateBallotIssued(_voter, issuedDate);
                    try
                    {
                        _voter.IssueBallot();
                    }
                    catch (Exception e)
                    {
                        // Log Exception
                        GlobalReferences.StatusBar.TextCenter = (e.Message);
                        VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                        ABSLogger.WriteLog("Error Issuing Ballot: " + e.Message);
                        if (e.InnerException != null)
                        {
                            //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                            ABSLogger.WriteLog(e.InnerException.Message);
                            if (e.InnerException.InnerException != null)
                            {
                                //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                                ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                            }
                        }
                    }

                    // Print Mailing Labels
                    for (int i = 0; i < AppSettings.Ballots.LabelCount; i++)
                    {
                        //ReportPrintingMethods.PrintVoterMailingLabel(_voter);
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintVoterMailingLabels(_voter.Data)); 
                    }

                    // Print Ballot
                    BallotPrinting.ReprintBallot(_voter.Data);

                    // BALLOT STUBS FOR EDDY ONLY
                    if (AppSettings.System.BallotStub == 1)
                    {
                        BallotPrinting.ReprintStub(_voter.Data);
                        //ReportPrintingMethods.PrintVoterMailingLabel(_voter);
                    }
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database Error");
            }
        }

        public async void PrintInPersonBallot()
        {
            if (_voter.Data.BallotStyleID == null)
            {
                AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Ineligible Voter");
            }
            else if (_voter.HasVoted())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Voter Has Already Voted");
            }
            else
            {
                SetLocalMailingAddress();

                // Set local values
                _voter.Data.ElectionID = AppSettings.Election.ElectionID;
                _voter.Data.PollID = AppSettings.System.SiteID;
                _voter.Data.ComputerID = AppSettings.System.MachineID;
                _voter.Data.UserId = AppSettings.User.UserID;

                // Set absentee type
                if (VoterTypeList.SelectedIndex >= 0)
                {
                    _voter.Data.AbsenteeType = ((ComboBoxItem)VoterTypeList.SelectedItem).DataContext.ToString();
                }
                else
                {
                    _voter.Data.AbsenteeType = "R";
                    VoterTypeList.SelectedIndex = 0;
                }

                DateTime issuedDate = DateTime.Now;

                // Mark absentee record as "Ballot Issued InPerson"
                // date_issued and printed_date
                // also LogCode #7 "In Person Voting"
                if (await Task.Run(() => VoterMethods.Exists) == true)
                {
                    //await VoterMethods.InsertOrUpdateBallotIssuedInPerson(_voter, issuedDate);

                    // Display changes
                    _voter.Data.LogCode = 7;
                    _voter.Data.LogDescription = "In Person Voting";
                    _voter.Data.ActivityDate = DateTime.Now;
                    _voter.Data.LogDate = DateTime.Now;
                    _voter.Data.ComputerID = AppSettings.System.MachineID;
                    _voter.Data.PollID = AppSettings.Absentee.SiteID;
                    LogCodesList.SelectedIndex = 6; // Selected index = #LogCode - 1
                    LogDate.Text = issuedDate.ToString();
                    ComboBoxMethods.SetSelectedItem(SitesList, AppSettings.System.SiteName);
                    Computer.Text = AppSettings.System.MachineID.ToString();
                    ApplicationAcceptedDate.Text = issuedDate.ToString();

                    try
                    {
                        _voter.VotedInPerson();
                    }
                    catch (Exception e)
                    {
                        // Log Exception
                        GlobalReferences.StatusBar.TextCenter = (e.Message);
                        VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                        ABSLogger.WriteLog("Error Voting In Person: " + e.Message);
                        if (e.InnerException != null)
                        {
                            //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                            ABSLogger.WriteLog(e.InnerException.Message);
                            if (e.InnerException.InnerException != null)
                            {
                                //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                                ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                            }
                        }
                    }

                    BallotPrinting.ReprintBallot(_voter.Data);
                    BallotPrinting.PrintAbsenteeApplicationPre(_voter.Data);
                }
                else
                {
                    GlobalReferences.StatusBar.TextCenter = ("Database not found");
                }
            }
        }

        public async void PrintEarlyVotingBallot()
        {
            if (_voter.Data.BallotStyleID == null)
            {
                AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Ineligible Voter");                
            }
            else if (_voter.HasVoted())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Voter Has Already Voted");
            }
            else
            {
                SetLocalMailingAddress();

                // Set local values
                _voter.Data.ElectionID = AppSettings.Election.ElectionID;
                _voter.Data.PollID = AppSettings.System.SiteID;
                _voter.Data.ComputerID = AppSettings.System.MachineID;
                _voter.Data.UserId = AppSettings.User.UserID;

                YesNoDialog newMessage = new YesNoDialog("Early Voting", "ARE YOU SURE YOU WANT TO PRINT AN\r\nEARLY VOTING BALLOT FOR " + _voter.Data.FullName);
                if (newMessage.ShowDialog() == true)
                {
                    // Set absentee type
                    if (VoterTypeList.SelectedIndex >= 0)
                    {
                        _voter.Data.AbsenteeType = ((ComboBoxItem)VoterTypeList.SelectedItem).DataContext.ToString();
                    }
                    else
                    {
                        _voter.Data.AbsenteeType = "R";
                        VoterTypeList.SelectedIndex = 0;
                    }

                    DateTime issuedDate = DateTime.Now;

                    // Mark absentee record as "Ballot Issued InPerson"
                    // date_issued and printed_date
                    // also LogCode #7 "In Person Voting"
                    if (await Task.Run(() => VoterMethods.Exists) == true)
                    {
                        // Display changes
                        _voter.Data.LogCode = 8;
                        _voter.Data.LogDescription = "Early Voting";
                        _voter.Data.ActivityDate = DateTime.Now;
                        _voter.Data.LogDate = DateTime.Now;
                        _voter.Data.ComputerID = AppSettings.System.MachineID;
                        _voter.Data.PollID = AppSettings.System.SiteID;
                        LogCodesList.SelectedIndex = 7; // Selected index = #LogCode - 1
                        LogDate.Text = issuedDate.ToString();
                        ApplicationIssuedDate.Text = issuedDate.ToString();
                        BallotIssuedDate.Text = issuedDate.ToString();
                        BallotPrintedDate.Text = issuedDate.ToString();
                        ComboBoxMethods.SetSelectedItem(SitesList, AppSettings.System.SiteName);
                        Computer.Text = AppSettings.System.MachineID.ToString();
                        ApplicationAcceptedDate.Text = issuedDate.ToString();

                        // Get new ballot number
                        //_voter.BallotNumber = VoterMethods.Voter.GetNextBallotNumber((int)AppSettings.System.SiteID);
                        _voter.GetNextBallotNumber((int)AppSettings.System.SiteID);

                        //await VoterMethods.InsertOrUpdateEarlyVoting(_voter, issuedDate);
                        try
                        {
                            _voter.VotedAtPolls(LogCodes.EarlyVoting);
                        }
                        catch (Exception e)
                        {
                            // Log Exception
                            GlobalReferences.StatusBar.TextCenter = (e.Message);
                            VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                            ABSLogger.WriteLog("Error Early Voting: " + e.Message);
                            if (e.InnerException != null)
                            {
                                //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                                ABSLogger.WriteLog(e.InnerException.Message);
                                if (e.InnerException.InnerException != null)
                                {
                                    //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                                    ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                                }
                            }
                        }

                        await Task.Run(() =>
                        {
                            BallotPrinting.ReprintBallot(_voter.Data);
                            BallotPrinting.PrintAbsenteeApplicationPre(_voter.Data);
                            // Print Ballot Stub
                            if (AppSettings.System.BallotStub == 1)
                            {
                                ReportPrintingMethods.PrintBallotStub(_voter.Data);
                            }
                        });
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Database not found");
                    }
                }
            }
        }

        //public void PrintOfficialBallot()
        //{
        //    if (_voter.BallotStyleID == null)
        //    {
        //        AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
        //        newMessage.ShowDialog();
        //        //StatusBar.ApplicationStatusCenter("Ineligible Voter");
        //    }
        //    else if (_voter.HasVoted())
        //    {
        //        AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
        //        newMessage.ShowDialog();
        //        //StatusBar.ApplicationStatusCenter("Voter Has Already Voted");
        //    }
        //    else
        //    {
        //        SetLocalMailingAddress();

        //        // Mark absentee record as "Ballot Issued"
        //        // date_issued and printed_date

        //        BallotPrinting.ReprintBallot(_voter);
        //    }
        //}

        public void PrintProvisionalBallot()
        {
            ValidationDialog passwordDialog = new ValidationDialog("Manager");
            if (passwordDialog.ShowDialog() == true)
            {
                if (_voter.Data.BallotStyleID == null)
                {
                    AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
                    newMessage.ShowDialog();
                    //StatusBar.ApplicationStatusCenter("Ineligible Voter");
                }
                else
                {
                    SetLocalMailingAddress();

                    // Set local values
                    _voter.Data.ElectionID = AppSettings.Election.ElectionID;
                    _voter.Data.PollID = AppSettings.Absentee.SiteID;
                    _voter.Data.ComputerID = AppSettings.System.MachineID;
                    _voter.Data.UserId = AppSettings.User.UserID;

                    // Mark provisional record
                    //BallotPrinting.PrintProvisionalBallot(_voter);

                    // Reset Menu
                    //MainMenuMethods.LoadMenu(new AbsenteeMenu());
                    GlobalReferences.MenuSlider.SetMenu(new AbsenteeMenu(), MenuCollapseMode.Full);

                    // Start provisional Process
                    this.NavigateToPage(new ProvisionalBallotPage(_voter));
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Invalid Password");
            }
        }

        public void BatchThisVoter() 
        {
            DateTime acceptedDate = DateTime.Now;

            // Check voter status
            if (_voter.Data.BallotStyleID == null)
            {
                AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
                newMessage.ShowDialog();
            }
            else if (_voter.HasVoted())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
            }
            else if (_voter.IdRequired().Value)
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS AN ID REQUIRED AND CANNOT BE BATCHED");
                newMessage.ShowDialog();
            }
            else if (_voter.Data.LogCode != 5)
            {
                if (_voter.Data.ApplicationIssued == null)
                {
                    YesNoDialog newMessage = new YesNoDialog("No Application", "THIS VOTER HAS NOT BEEN ISSUED AN APPLICATION\rFROM THE VoterX SYSTEM\r\n\r\nARE YOU SURE YOU WANT TO QUEUE A BALLOT?");
                    if (newMessage.ShowDialog() == true)
                    {
                        ApplicationIssuedDate.Text = acceptedDate.ToString();
                        _voter.Data.ApplicationIssued = acceptedDate;

                        QueueBallot(acceptedDate);
                    }
                }
                else
                {
                    QueueBallot(acceptedDate);
                }
            }
            else if (_voter.Data.LogCode == 5)
            {
                GlobalReferences.StatusBar.TextLeft = ("Application Already Accpeted");
            }
        }

        public void QueueBallot(DateTime acceptedDate)
        {
            if (GetAddressFromDialog(1))
            {
                // Set local values
                _voter.Data.ElectionID = AppSettings.Election.ElectionID;
                _voter.Data.PollID = AppSettings.Absentee.SiteID;
                _voter.Data.ComputerID = AppSettings.System.MachineID;
                _voter.Data.UserId = AppSettings.User.UserID;

                // Update Detail Screen
                _voter.Data.LogCode = 5;
                LogCodesList.SelectedIndex = 4; // Selected index = #LogCode - 1
                LogDate.Text = acceptedDate.ToString();
                ComboBoxMethods.SetSelectedItem(SitesList, AppSettings.Absentee.SiteName);
                Computer.Text = AppSettings.System.MachineID.ToString();
                ApplicationAcceptedDate.Text = acceptedDate.ToString();

                // Mark Voter, Application Accepted #5                
                _voter.Data.ApplicationAccepted = acceptedDate;
                try
                {
                    _voter.AcceptApplication();
                }
                catch (Exception e)
                {
                    // Log Exception
                    GlobalReferences.StatusBar.TextCenter = (e.Message);
                    VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                    ABSLogger.WriteLog("Error Queue For Bath: " + e.Message);
                    if (e.InnerException != null)
                    {
                        //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                        ABSLogger.WriteLog(e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                            ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                        }
                    }
                }
            }
        }

        //public async void AcceptApplication()
        //{            
        //    DateTime acceptedDate = DateTime.Now;

        //    // Check if voter has an issued application
        //    if (_voter.BallotStyleID == null)
        //    {
        //        AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
        //        newMessage.ShowDialog();
        //        //StatusBar.ApplicationStatusCenter("Ineligible Voter");
        //    }
        //    else if (_voter.HasVoted()) // Log code greater than 5
        //    {
        //        AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
        //        newMessage.ShowDialog();
        //        //StatusBar.ApplicationStatusCenter("Voter Has Already Voted");
        //    }
        //    else if (_voter.LogCode != 5) // Can only be less than 5
        //    {
        //        SetLocalMailingAddress();
        //        bool? dialogResult = false;
        //        if(_voter.LogCode < 3 || _voter.LogCode == null)
        //        {
        //            YesNoDialog newMessage = new YesNoDialog("Accept Application", _voter.FullName.ToUpper() + "\r\nHAS NOT BEEN ISSUED AN ABSENTEE APPLICATION\r\nBY THIS VoterX SYSTEM.\r\nDO YOU STILL WANT TO ACCEPT AN APPLICATION\r\nAND QUEUE THEM FOR BATCH PROCESSING?");
        //            dialogResult = newMessage.ShowDialog();
        //        }
        //        else
        //        {
        //            OkCancelDialog newMessage = new OkCancelDialog("Accept Application", "YOU ARE ABOUT TO ACCEPT AN APPLICATION FOR \r\n" + _voter.FullName.ToUpper() + "\r\nAND QUEUE THEM FOR BATCH PROCESSING.\r\nDO YOU WANT TO CONTINUE?");
        //            dialogResult = newMessage.ShowDialog();
        //        }
        //        if (dialogResult == true)
        //        {
        //            int temp_voter_id = 0;
        //            if (Int32.TryParse(_voter.VoterID, out temp_voter_id) == true)
        //            {
        //                // Set absentee type
        //                if (VoterTypeList.SelectedIndex >= 0)
        //                {
        //                    _voter.AbsenteeType = ((ComboBoxItem)VoterTypeList.SelectedItem).DataContext.ToString();
        //                }
        //                else
        //                {
        //                    _voter.AbsenteeType = "R";
        //                    VoterTypeList.SelectedIndex = 0;
        //                }

        //                // Mark Voter, Application Accepted #5
        //                if (await Task.Run(() => VoterMethods.Voter.Exists()) == true)
        //                {
        //                    await VoterMethods.InsertOrUpdateApplicationAccepted(_voter, acceptedDate);
        //                    // Update Detail Screen
        //                    _voter.LogCode = 5;
        //                    LogCodesList.SelectedIndex = 4; // Selected index = #LogCode - 1
        //                    LogDate.Text = acceptedDate.ToString();
        //                    ComboBoxMethods.SetSelectedItem(SitesList, AppSettings.Absentee.SiteName);
        //                    Computer.Text = AppSettings.System.MachineID.ToString();
        //                    ApplicationAcceptedDate.Text = acceptedDate.ToString();

        //                    StatusBar.ApplicationStatus("Voter application has been accepted");
        //                }
        //                else
        //                {
        //                    StatusBar.ApplicationStatusCenter("Database not found");
        //                }
        //            }
        //            else
        //            {
        //                StatusBar.ApplicationStatus("Could Not Parse The Voter Id");
        //            }
        //        }
        //    }
        //    else if (_voter.LogCode == 5)
        //    {
        //        AlertDialog newMessage = new AlertDialog("THIS VOTER APPLICATION HAS ALREADY BEEN ACCEPTED");
        //        newMessage.ShowDialog();
        //        //StatusBar.ApplicationStatus("Application Already Accpeted");
        //    }
        //}

        public void ClearVoterId()
        {
            VoterId.Text = "";
        }

        public void SpoilBallot()
        {
            List<int?> valideCodes = new List<int?> { 6, 14 };

            // Check if ballot exists
            if (_voter.HasBeenIssued())
            {
                if (_voter.Data.LogCode == 8 && _voter.Data.PollID != AppSettings.System.SiteID)
                {
                    AlertDialog newMessage = new AlertDialog("CANNOT SPOIL A BALLOT FROM ANOTHER SITE");
                    newMessage.ShowDialog();
                }
                else if (_voter.Data.LogCode == 16)
                {
                    AlertDialog newMessage = new AlertDialog("CANNOT SPOIL AN ELECTION DAY BALLOT");
                    newMessage.ShowDialog();
                }
                else if (!valideCodes.Contains(_voter.Data.LogCode))
                {
                    AlertDialog newMessage = new AlertDialog("CANNOT SPOIL A BALLOT WITH THE CURRENT STATUS");
                    newMessage.ShowDialog();
                }
                else
                {
                    // Reset Menu
                    //MainMenuMethods.LoadMenu(new AbsenteeMenu());
                    GlobalReferences.MenuSlider.SetMenu(new AbsenteeMenu(), MenuCollapseMode.Full);
                    this.NavigateToPage(new SpoiledBallotPage(_voter));
                }
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("THERE IS NO EXISTING BALLOT TO SPOIL");
                newMessage.ShowDialog();
            }
        }

        public void RejectedApplication()
        {
            //SetMailingAddress();

            // Check if voter has already voted
            if (_voter.HasBeenIssued())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Voter Has Already Voted");
            }
            // Check for existing application
            else if (_voter.Data.ApplicationIssued != null)
            {
                // Reset Menu
                //MainMenuMethods.LoadMenu(new AbsenteeMenu());
                GlobalReferences.MenuSlider.SetMenu(new AbsenteeMenu(), MenuCollapseMode.Full);
                this.NavigateToPage(new AbsenteeRejectedApplicationsPage(_voter));
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("THERE IS NO EXISTING APPLICATION");
                newMessage.ShowDialog();
            }
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            GlobalReferences.StatusBar.TextLeft = (e.Key.ToString());
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                MessageBox.Show("CTRL + S");
            }
        }

        private void UpdateMailingAddress()
        {
            // check delivery address
            if (DeliveryAddress1.Text != ""
                || DeliveryAddress2.Text != ""
                || DeliveryCity.Text != ""
                || DeliveryState.Text != "")
            {
                // use delivery address
            }
            else
            {
                // check mailing address
                if (MailingAddress1.Text != ""
                    || MailingAddress2.Text != ""
                    || MailingCity.Text != ""
                    || MailingState.Text != "")
                {
                    // use mailing address
                    DeliveryAddress1.Text = MailingAddress1.Text;
                    DeliveryAddress2.Text = MailingAddress2.Text;
                    DeliveryCity.Text = MailingCity.Text;
                    DeliveryState.Text = MailingState.Text;
                    DeliveryZip.Text = MailingZip.Text;                    
                }
                else
                {
                    if (PhysicalAddress1.Text != ""
                        || PhysicalAddress2.Text != ""
                        || PhysicalCity.Text != ""
                        || PhysicalState.Text != "")
                    {
                        // use physical address
                        DeliveryAddress1.Text = PhysicalAddress1.Text;
                        DeliveryAddress2.Text = PhysicalAddress2.Text;
                        DeliveryCity.Text = PhysicalCity.Text;
                        DeliveryState.Text = PhysicalState.Text;
                        DeliveryZip.Text = PhysicalZip.Text;
                    }
                }
            }

            //_voter.DeliveryAddress1 = DeliveryAddress1.Text;
            //_voter.DeliveryAddress2 = DeliveryAddress2.Text;
            //_voter.DeliveryCity = DeliveryCity.Text;
            //_voter.DeliveryState = DeliveryState.Text;
            //_voter.DeliveryZip = DeliveryZip.Text;
            //_voter.DeliveryCountry = DeliveryCountry.Text;

            // Update Voted Record 
            //if (await Task.Run(() => VoterMethods.Voter.Exists()) == true)
            //{
            //    if (VoterMethods.UpdateVotedAddress(_voter) == true)
            //    {
            //        //StatusBar.ApplicationStatus("Address Updated");
            //    }
            //    else
            //    {
            //        //StatusBar.ApplicationStatus("No Voted Record Found");
            //    }
            //}
        }

        private void SetLocalMailingAddress()
        {
            // check temp address
            if (TempAddress1.Text.Length > 4
                && TempCity.Text != ""
                && TempState.Text != "")
            {
                // use temp address
                DeliveryAddress1.Text = TempAddress1.Text;
                DeliveryAddress2.Text = TempAddress2.Text;
                DeliveryCity.Text = TempCity.Text;
                DeliveryState.Text = TempState.Text;
                DeliveryZip.Text = TempZip.Text;
                DeliveryCountry.Text = TempCountry.Text;
            }
            else
            {
                // check mailing address
                if (MailingAddress1.Text != ""
                    || MailingAddress2.Text != ""
                    || MailingCity.Text != ""
                    || MailingState.Text != "")
                {
                    // use mailing address
                    DeliveryAddress1.Text = MailingAddress1.Text;
                    DeliveryAddress2.Text = MailingAddress2.Text;
                    DeliveryCity.Text = MailingCity.Text;
                    DeliveryState.Text = MailingState.Text;
                    DeliveryZip.Text = MailingZip.Text;
                    DeliveryCountry.Text = MailingCountry.Text;
                }
                else
                {
                    if (PhysicalAddress1.Text != ""
                        || PhysicalAddress2.Text != ""
                        || PhysicalCity.Text != ""
                        || PhysicalState.Text != "")
                    {
                        // use physical address
                        DeliveryAddress1.Text = PhysicalAddress1.Text;
                        DeliveryAddress2.Text = PhysicalAddress2.Text;
                        DeliveryCity.Text = PhysicalCity.Text;
                        DeliveryState.Text = PhysicalState.Text;
                        DeliveryZip.Text = PhysicalZip.Text;
                    }
                }
            }

            _voter.Data.DeliveryAddress1 = DeliveryAddress1.Text;
            _voter.Data.DeliveryAddress2 = DeliveryAddress2.Text;
            _voter.Data.DeliveryCity = DeliveryCity.Text;
            _voter.Data.DeliveryState = DeliveryState.Text;
            _voter.Data.DeliveryZip = DeliveryZip.Text;
            _voter.Data.DeliveryCountry = DeliveryCountry.Text;
        }

        private bool GetAddressFromDialog(int type)
        {
            bool result = false;

            // Copy Temp fields to voter.tempaddress
            _voter.Data.TempAddress1 = TempAddress1.Text;
            _voter.Data.TempAddress2 = TempAddress2.Text;
            _voter.Data.TempCity = TempCity.Text;
            _voter.Data.TempState = TempState.Text;
            _voter.Data.TempZip = TempZip.Text;
            _voter.Data.TempCountry = TempCountry.Text;

            // Check if addresses match
            if (
                // Temp address is not blank 
                TempAddress1.Text != ""
                ||
                // Address 1 does not match
                MailingAddress1.Text != PhysicalAddress1.Text
                ||
                // Address 2 does not match
                MailingAddress2.Text != PhysicalAddress2.Text
                ||
                // City does not match
                MailingCity.Text != PhysicalCity.Text
                ||
                // State does not match
                MailingState.Text != PhysicalState.Text
                ||
                // Zip does not match
                MailingZip.Text != PhysicalZip.Text
                )
            {
                // ADDRESSES DONT MATCH 
                // (SOME VOTERS DO NOT HAVE A MAILING ADDRESS)
                // If no mailing address is found and no temp address supplied then use the registered address (8/30/2019)

                // Check for blanks
                if (TempAddress1.Text != "")
                {
                    result = PromptUserToSelectAddress(type);
                }
                else if(MailingAddress1.Text == "" && PhysicalAddress1.Text != "")
                {
                    // Use Physical
                    CopyAddressSelection(2);
                    result = true;
                }
                else if (MailingAddress1.Text != "" && PhysicalAddress1.Text == "")
                {
                    // Use Mailing
                    CopyAddressSelection(1);
                    result = true;
                }
                else if(PhysicalAddress1.Text != "" && MailingAddress1.Text != "")
                {
                    result = PromptUserToSelectAddress(type);
                }
                else if(TempAddress1.Text == "" && PhysicalAddress1.Text == "" && MailingAddress1.Text == "")
                {
                    AlertDialog newMessage = new AlertDialog("NO ADDRESS COULD BE FOUND FOR THIS VOTER");
                    newMessage.ShowDialog();
                }
                else
                {
                    AlertDialog newMessage = new AlertDialog("NO ADDRESS COULD BE FOUND FOR THIS VOTER");
                    newMessage.ShowDialog();
                }
            }
            else
            {
                _voter.Data.DeliveryAddress1 = PhysicalAddress1.Text;
                _voter.Data.DeliveryAddress2 = PhysicalAddress2.Text;
                _voter.Data.DeliveryCity = PhysicalCity.Text;
                _voter.Data.DeliveryState = PhysicalState.Text;
                _voter.Data.DeliveryZip = PhysicalZip.Text;
                _voter.Data.DeliveryCountry = "";

                // Display new Address on screen
                DeliveryAddress1.Text = _voter.Data.DeliveryAddress1;
                DeliveryAddress2.Text = _voter.Data.DeliveryAddress2;
                DeliveryCity.Text = _voter.Data.DeliveryCity;
                DeliveryState.Text = _voter.Data.DeliveryState;
                DeliveryZip.Text = _voter.Data.DeliveryZip;
                DeliveryCountry.Text = _voter.Data.DeliveryCountry;

                result = true;
            }

            return result;
        }

        private void CopyAddressSelection(int selection)
        {
            // Copy address from user selection
            switch (selection)
            {
                case 1:
                    //StatusBar.ApplicationStatus("Ballot Sent To: " + _voter.MailingAddress1 + " " + _voter.MailingAddress2 + " " + _voter.MailingCity + " " + _voter.MailingState + " " + _voter.MailingZip);
                    // Copy Mailing Address to Delivery Address
                    _voter.Data.DeliveryAddress1 = MailingAddress1.Text;
                    _voter.Data.DeliveryAddress2 = MailingAddress2.Text;
                    _voter.Data.DeliveryCity = MailingCity.Text;
                    _voter.Data.DeliveryState = MailingState.Text;
                    _voter.Data.DeliveryZip = MailingZip.Text;
                    _voter.Data.DeliveryCountry = MailingCountry.Text;
                    break;
                case 2:
                    //StatusBar.ApplicationStatus("Ballot Sent To: " + _voter.Address1 + " " + _voter.Address2 + " " + _voter.City + " " + _voter.State + " " + _voter.Zip);
                    // Copy Registered Address to Delivery Address
                    _voter.Data.DeliveryAddress1 = PhysicalAddress1.Text;
                    _voter.Data.DeliveryAddress2 = PhysicalAddress2.Text;
                    _voter.Data.DeliveryCity = PhysicalCity.Text;
                    _voter.Data.DeliveryState = PhysicalState.Text;
                    _voter.Data.DeliveryZip = PhysicalZip.Text;
                    _voter.Data.DeliveryCountry = "";
                    break;
                case 3:
                    //StatusBar.ApplicationStatus("Ballot Sent To: " + _voter.TempAddress1 + " " + _voter.TempAddress2 + " " + _voter.TempCity + " " + _voter.TempState + " " + _voter.TempZip);
                    // Copy Temp Address to Delivery Address
                    _voter.Data.DeliveryAddress1 = TempAddress1.Text;
                    _voter.Data.DeliveryAddress2 = TempAddress2.Text;
                    _voter.Data.DeliveryCity = TempCity.Text;
                    _voter.Data.DeliveryState = TempState.Text;
                    _voter.Data.DeliveryZip = TempZip.Text;
                    _voter.Data.DeliveryCountry = TempCountry.Text;
                    break;
            }

            // Display new Address on screen
            DeliveryAddress1.Text = _voter.Data.DeliveryAddress1;
            DeliveryAddress2.Text = _voter.Data.DeliveryAddress2;
            DeliveryCity.Text = _voter.Data.DeliveryCity;
            DeliveryState.Text = _voter.Data.DeliveryState;
            DeliveryZip.Text = _voter.Data.DeliveryZip;
            DeliveryCountry.Text = _voter.Data.DeliveryCountry;
        }

        private bool PromptUserToSelectAddress(int type)
        {
            // Prompt user to select a mailing address
            AddressSelectionDialog addresSelection = new AddressSelectionDialog(_voter.Data, type);
            if (addresSelection.ShowDialog() == true)
            {
                //StatusBar.ApplicationStatus(addresSelection.Selection.ToString());

                CopyAddressSelection(addresSelection.Selection);

                // Display new Address on screen
                DeliveryAddress1.Text = _voter.Data.DeliveryAddress1;
                DeliveryAddress2.Text = _voter.Data.DeliveryAddress2;
                DeliveryCity.Text = _voter.Data.DeliveryCity;
                DeliveryState.Text = _voter.Data.DeliveryState;
                DeliveryZip.Text = _voter.Data.DeliveryZip;
                DeliveryCountry.Text = _voter.Data.DeliveryCountry;

                return true;
            }
            else
            {
                return false;
            }
        }

        private void IdRequiredCheck_Click(object sender, RoutedEventArgs e)
        {
            //_voter.IDRequired = (bool)IdRequiredCheck.IsChecked;
        }

        public void UnlockCurrentRecord()
        {
            // Unlock button Removed by request of John 6/26/18
            //PrecinctPartsList.IsEnabled = true;
            //Party.IsReadOnly = false;
            //UpdateBallotStyleButton.Visibility = Visibility.Visible;
            //ProvisionalOnlyCheck.IsEnabled = true;
            //StatusBar.ApplicationStatus("Record Unlocked");
        }

        private void UpdateBallotStyleButton_Click(object sender, RoutedEventArgs e)
        {
            //// Get the precint part Id
            //string precinctPart = ComboBoxMethods.GetSelectedItemData(PrecinctPartsList).ToString();
            //// Get the party name
            //string party = Party.Text;
            //// Check if either precinct part id or party name are blank
            //if (!precinctPart.IsNullOrEmpty() && !party.IsNullOrEmpty())
            //{
            //    if (await Task.Run(() => VoterMethods.BallotStyleMaster.Exists(0)) == true)
            //    {
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
            //            var ballotStyle = await Task.Run(() => ballotStyleQuery.FirstOrDefault());

            //            // Update Voter Record
            //            try
            //            {
            //                _voter.PrecinctPartID = precinctPart;
            //                _voter.Party = party;
            //                _voter.BallotStyle = ballotStyle.BallotStyleName;
            //                _voter.BallotStyleFile = ballotStyle.BallotStyleFileName;
            //                _voter.BallotStyleID = ballotStyle.BallotStyleID;
            //            }
            //            catch (Exception error)
            //            {
            //                StatusBar.ApplicationStatusCenter(error.Message);
            //            }

            //            //Set Ballot Style list and display results
            //            string message = SetBallotStyleSelectedItem(BallotStylesList, ballotStyle.BallotStyleName);
            //            StatusBar.ApplicationStatus(message);
            //            //ComboBoxMethods.SetSelectedItem(BallotStylesList, ballotStyle);

            //        }
            //        else
            //        {
            //            // Set Ballot Style list to blank
            //            BallotStylesList.SelectedIndex = -1;
            //            // Display failure message
            //            StatusBar.ApplicationStatus("No Valid Ballot Style Found");
            //        }

            //    }
            //}
        }

        private void ProvisionalOnlyCheck_Click(object sender, RoutedEventArgs e)
        {
            //if (await Task.Run(() => VoterMethods.Voter.Exists()) == true)
            //{
            //    // Toggle the Provisional Only status
            //    bool provisional = false;
            //    if (ProvisionalOnlyCheck.IsChecked == true) provisional = true;

            //    VoterMethods.UpdateProvisionalOnly(_voter, provisional);
            //}
            //else
            //{
            //    StatusBar.ApplicationStatusCenter("Database not found");
            //}
        }
    }
}
