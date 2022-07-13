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
using VoterX.Extensions;
using System.Windows.Media.Effects;
using System.Collections.ObjectModel;
using VoterX.Dialogs;
using VoterX.Core.Extensions;
using VoterX.Core.Voters;
using VoterX.Manager.Views.Absentee;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        // Load home page when returning from another page
        //public SearchPage(VoterSearchModel searchItems)
        //{
        //    InitializeComponent();

        //    // Reset search boxes to previous values
        //    if (searchItems != null)
        //    {
        //        SetSearchBoxes(searchItems);
        //        // Get list of voters from Search Parameters
        //        //VoterList.ItemsSource = VoterDataMethods.VoterList(searchItems);

        //        // Display status message
        //        //StatusBar.ApplicationStatus("Search for another voter");
        //    }

        //    // If calling page passes NULL then clear all the search boxes
        //    if (searchItems == null)
        //    {
        //        ClearSearchBoxes();

        //        // Display status message
        //        //StatusBar.ApplicationStatus("Begin search");
        //    }

        //    // Display page header
        //    StatusBar.ApplicationPageHeader("Voter Search");

        //    //StatusBar.ApplicationStatus("");
        //    //StatusBar.ApplicationStatusCenter("");
        //    //StatusBar.ApplicationStatusClear();

        //    //StatusBar.ApplicationStatusCenter(SignatureMethods.CheckSignaturePad());
        //    //StatusBar.ApplicationCheckSignaturePad();

        //    CheckPrinter();

        //    CheckServer();

        //    //Console.WriteLine("Search Loaded: Type 2");

        //    StatusBar.ApplicationStatusLeft("Debug: " + ((App)Application.Current).debugMode.ToString());
        //}

        private int? _siteId;

        // Load home page for the first time
        public SearchPage() : this(null) { }
        public SearchPage(int? site)
        {
            InitializeComponent();

            _siteId = site;

            //_logCodeRepository = ((App)Application.Current).voterContainer.Resolve<LogCodeRepo>();

            //VoterContainer.Resolve<LogCodeRepo>();

            //string name = _logCodeRepository.GetByID(16).log_description.ToString();

            //if (AppSettings.System.Login)
            //{
            //    LogoutButton.Visibility = Visibility.Visible;
            //}

            GlobalReferences.Header.PageHeader = ("Voter Search");
            //StatusBar.ApplicationStatus("");
            //StatusBar.ApplicationStatusCenter("");
            //StatusBar.ApplicationStatusClear();

            //StatusBar.ApplicationStatusCenter(SignatureMethods.CheckSignaturePad());
            //StatusBar.ApplicationCheckSignaturePad();

            CheckPrinter();

            CheckServer();

            //Console.WriteLine("Search Loaded: Type 1");

            if(_siteId != null)
            {
                // Preload voter list
                SearchButton_Click(SearchButton, null);
            }
        }

        //public SearchPage(int? debug)
        //{
        //    InitializeComponent();
        //}

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void LastNameSearch_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(LastNameSearch);
        }

        private void CheckPrinter()
        {
            // Check printer status            
            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter );
        }

        private async void SearchCheckServer()
        {
            if(await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(0) == false)
            {
                Console.WriteLine("Search Error Alert Called");
                AlertDialog messageBox = new AlertDialog("AN ERROR WAS ENCOUNTERED PLEASE CONTACT YOUR ADMINISTRATOR");
                messageBox.ShowDialog();
            }

            //StatusBar.ApplicationDatabaseChecking();

            //string message = await ServerMethods.CheckConnection(new VoterX.Core.Elections.ElectionFactory());

            //if (message == "False")
            //{
            //    StatusBar.ApplicationStatusLeft("Could not connect to database");

            //    StatusBar.ApplicationDatabaseStatus(false);

            //    Console.WriteLine("Search Error Alert Called");
            //    AlertDialog messageBox = new AlertDialog("AN ERROR WAS ENCOUNTERED PLEASE CONTACT YOUR ADMINISTRATOR");
            //    messageBox.ShowDialog();
            //}
            //else
            //{
            //    StatusBar.ApplicationDatabaseStatus(true);
            //    StatusBar.ApplicationStatusClear();

            //    SearchButton.IsEnabled = true;
            //    ScanButton.IsEnabled = true;
            //}
        }

        private async void CheckServer()
        {
            SearchGrid.Visibility = Visibility.Collapsed;
            LoadingPanel.Visibility = Visibility.Visible;

            if (await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(0) == true)
            {
                SearchGrid.Visibility = Visibility.Visible;
            }

            LoadingPanel.Visibility = Visibility.Collapsed;
        }

        // Load the search boxes from the search items model
        //private void SetSearchBoxes(VoterSearchModel searchItems)
        //{
        //    FirstNameSearch.Text = searchItems.FirstName;
        //    LastNameSearch.Text = searchItems.LastName;
        //    BirthYearSearch.Text = searchItems.BirthYear;
        //}

        // Erase all values from search boxes
        // This only needs to be done when I have default values set
        private void ClearSearchBoxes()
        {
            FirstNameSearch.Text = null;
            LastNameSearch.Text = null;
            BirthYearSearch.Text = null;

            BarCodeSearch.Text = null;
            LastBarCode.Text = null;
        }

        // Clear the list view object
        private void ClearListView()
        {
            SearchScrollViewer.ScrollToTop();
            VoterList.ItemsSource = null;
        }

        // Run the voter search for the search box values
        // Running the search asynchronously does two things
        //// First it allows the user to perform other tasks while the search is running
        //// Second it allows the Status bar message to display that the application is currently working 
        //// ~ without having to wait for the search to finish
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //if (CheckServer() == false) return;

            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            SearchResults.Text = "";

            // Clear the list box
            ClearListView();

            if (await Task.Run(() => VoterMethods.Exists) == true)
            {

                SearchResultsText.Text = "";
                //// Check if there is anything to search
                if (SearchIsNotNullOrEmpty() || _siteId != null)
                {
                    // Display search parameters in status bar
                    // and turn on the spining progress wheel
                    SearchingPanel.Visibility = Visibility.Visible;

                    // Create the Voter Search Object (Search Parameters)
                    VoterSearchModel searchItems = new VoterSearchModel
                    {
                        FirstName = FirstNameSearch.Text,
                        LastName = LastNameSearch.Text,
                        BirthYear = BirthYearSearch.Text
                    };

                    // Get list of voters from Search Parameters
                    // Call voter lookup asynchronously so user can still perform tasks while the search is running
                    // #### Might want to move the Async to the static method definition ####

                    Console.WriteLine("Searching Voters");

                    int count = 0;

                    var list = await VoterMethods.Voters.PagedListAsync(searchItems, _siteId, 1, 50);
                    string errorMessage = list.CheckForErrors();
                    if (errorMessage != null)
                    {
                        GlobalReferences.StatusBar.TextCenter = ("SQL ERROR: " + errorMessage);
                    }
                    else
                    {
                        count = list.Count();
                        VoterList.ItemsSource = list.OrderByFullName();
                    }

                    // Turn off the progress spinner
                    //StatusBar.LoadingSpinner(Visibility.Collapsed);
                    SearchingPanel.Visibility = Visibility.Collapsed;

                    // Display result count in the status bar
                    // #### We want to get the record count before reducing the return list ####
                    //StatusBar.ApplicationStatus(string.Concat("Results: ", count.ToString(), " voters found."));                
                    if (count < 1)
                    {
                        SearchResults.Visibility = Visibility.Visible;
                        SearchResults.Text = string.Concat("Results: ", count.ToString(), " voters found.");
                    }
                    else if (count < 50)
                    {
                        //StatusBar.ApplicationStatus(string.Concat("Results: ", count.ToString(), " voters found."));
                        SearchResultsText.Text = "Search Results:\r\n" + count.ToString() + " voters found.";
                    }

                    SearchScrollViewer.Focus();
                }
                else
                {
                    GlobalReferences.StatusBar.TextLeft = ("No Search Parameters Entered");
                    SearchResultsText.Text = "Please enter valid search criteria.";
                }
            }
            else
            {
                SearchingPanel.Visibility = Visibility.Collapsed;
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
            }
        }

        // Returns true if any of the search boxes have a value
        private bool SearchIsNotNullOrEmpty()
        {
            return
                !FirstNameSearch.Text.IsNullOrEmpty() ||
                !LastNameSearch.Text.IsNullOrEmpty() ||
                !BirthYearSearch.Text.IsNullOrEmpty();
        }

        // Clear both the search boxes and the list view
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear Search Boxes
            ClearSearchBoxes();

            // Clear ListView
            ClearListView();

            // Clear results message
            SearchResultsText.Text = "";
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchBoxes();
            ClearListView();

            SearchGrid.Visibility = Visibility.Collapsed;

            ScanGrid.Visibility = Visibility.Visible;

            Keyboard.Focus(BarCodeSearch);
        }

        private void SearchAltButton_Click(object sender, RoutedEventArgs e)
        {
            SearchGrid.Visibility = Visibility.Visible;

            ScanGrid.Visibility = Visibility.Collapsed;

            Keyboard.Focus(LastNameSearch);
        }

        // Send selected voter and the search parameters to the next page
        private async void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                this.Cursor = Cursors.Wait;

                var selectedVoter = ((NMVoter)item.DataContext);

                //StatusBar.ApplicationStatus(((VoterDataModel)item.DataContext).VoterID);
                if (await Task.Run(() => VoterMethods.Voters.Exists()) == true)
                {

                    // Get the search parameters
                    VoterSearchModel searchItems = new VoterSearchModel
                    {
                        FirstName = FirstNameSearch.Text,
                        LastName = LastNameSearch.Text,
                        BirthYear = BirthYearSearch.Text
                    };

                    // Set local and user values here

                    // Merge the voter and the search parameters into a single object                
                    VoterNavModel voterNav = new VoterNavModel
                    {
                        // Get full voter details
                        Voter = selectedVoter,
                        Search = searchItems
                    };

                    if (AppSettings.AbsenteeMode)
                    {
                        this.Cursor = Cursors.Arrow;
                        if (AppSettings.Absentee.AllMailMode)
                        {
                            this.NavigateToPage(new AllMailDetailsPage(voterNav));
                        }
                        else if(AppSettings.Absentee.TestScreen == true)
                        {
                            // Check for SERVIS lookup
                            bool SERVISlookup = false;
                            if (_siteId == 0) SERVISlookup = true;

                            this.NavigateToPage(new VoterDetailsPage(voterNav, SERVISlookup));
                        }
                        else
                        {
                            // Check for SERVIS lookup
                            if (_siteId == 0)
                            {
                                this.NavigateToPage(new AbsenteeDetailsPage(voterNav, true));
                            }
                            else
                            {
                                this.NavigateToPage(new AbsenteeDetailsPage(voterNav));
                            }
                        }
                    }
                    //else
                    //{
                    //    this.Cursor = Cursors.Arrow;
                    //    // Check where to send the voter next
                    //    RedirectVoter(voterNav);
                    //}
                }

                this.Cursor = Cursors.Arrow;
            }
        }

        //public async Task<VoterDataModel> GetSingleVoterData(string voterID)
        //{            
        //    var voter = await Task.Run(() => VoterMethods.Voter.Single(voterID, AppSettings.Election.ElectionType).FirstOrDefault());
        //    string errorMessage = voter.CheckForError();
        //    if (errorMessage != null)
        //    {
        //        StatusBar.ApplicationStatusCenter("SQL ERROR: " + errorMessage);
        //    }
        //    return voter;             
        //}

        //private Page ChooseVoterDestination(VoterNavModel voterNav)
        //{
        //    switch (VoterValidationMethods.CheckVoterStatus(voterNav.Voter, (int)AppSettings.System.SiteID))
        //    {
        //        case VoterLookupStatus.Eligible:
        //            return new VerifyValidVoterPage(voterNav);
        //            //break;
        //        case VoterLookupStatus.Ineligible:
        //            return new VerifyInvalidVoterPage(voterNav);
        //            //break;
        //        case VoterLookupStatus.Spoilable:
        //            return new VerifySpoiledBallotPage(voterNav);
        //            //break;
        //        case VoterLookupStatus.Provisional:
        //            return new VerifyProvisionalBallotPage(voterNav);
        //            //break;
        //        default:
        //            return null;
        //            //break;
        //    }
        //}

        //private void RedirectVoter(VoterNavModel voterNav)
        //{
        //    this.NavigateToPage(ChooseVoterDestination(voterNav));
        //}

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.AbsenteeMode)
            {
                this.NavigateToPage(new AbsenteeLoginPage());
            }
            else
            {
                this.NavigateToPage(new LoginPage());
            }
        }

        // Scroll the list items
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Notes on scroll offsets
            // https://stackoverflow.com/questions/1033841/is-it-possible-to-implement-smooth-scroll-in-a-wpf-listview
            // https://social.msdn.microsoft.com/Forums/en-US/3594c80a-7ccf-4cfc-9cc0-9731fd080d72/in-what-unit-is-the-scrollviewerverticaloffset?forum=winappswithcsharp

            //double delta = (e.Delta * .26978); // Roughly half of 1 list item
            double delta = (e.Delta * .36);
            //double delta = (e.Delta / 120)*32; // Reduce to +1 or -1 then multiply to get exact units
            //StatusBar.ApplicationStatusCenter("Scrolling:" + (delta).ToString());

            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - (delta));
            e.Handled = true;
        }

        // Scroll the list items from anywhere on the Page
        private void Page_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Mouse Wheel Status
            //StatusBar.ApplicationStatusCenter("Mouse Wheel Move: " + e.Delta);
            ScrollViewer_PreviewMouseWheel(SearchScrollViewer, e);
        }

        // When the mouse enters the grid of a specific list item
        // then set the child rectangle to Aqua
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Grid)sender).GetChildOfType<Rectangle>().Fill = Brushes.Aqua;

            DropShadowEffect myDropShadowEffect = new DropShadowEffect();
            // Set the color of the shadow to Black.
            Color myShadowColor = new Color
            {
                ScA = 1,
                ScB = 0,
                ScG = 0,
                ScR = 0
            };
            myDropShadowEffect.Color = myShadowColor;

            // Set the direction of where the shadow is cast to 320 degrees.
            myDropShadowEffect.Direction = 320;

            // Set the depth of the shadow being cast.
            myDropShadowEffect.ShadowDepth = 4;

            // Set the shadow softness to the maximum (range of 0-1).
            myDropShadowEffect.BlurRadius = 5;
            // Set the shadow opacity to half opaque or in other words - half transparent.
            // The range is 0-1.
            myDropShadowEffect.Opacity = 0.5;

            // Apply the bitmap effect to the Button.
            ((Grid)sender).GetChildOfType<Border>().Effect = myDropShadowEffect;
        }

        // When the mouse leaves the grid of a specific list item
        // then set the child rectangle to White
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Grid)sender).GetChildOfType<Rectangle>().Fill = Brushes.White;

            ((Grid)sender).GetChildOfType<Border>().Effect = null;
        }

        private void BarCodeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                //StatusBar.ApplicationStatusCenter(BarCodeSearch.Text);
                SearchSingleVoter(BarCodeSearch.Text);

                Keyboard.Focus(BarCodeSearch);

                LastBarCode.Text = BarCodeSearch.Text;

                BarCodeSearch.Text = "";
            }
        }

        private async void SearchSingleVoter(string voterID)
        {
            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            SearchResults.Text = "";

            // Clear the list box
            ClearListView();

            SearchResultsText.Text = "";
            //// Check if there is anything to search
            //if (SearchIsNotNullOrEmpty())
            {
                // Get the voter data repository
                //var voterLookup = ((App)Application.Current).voterContainer.Resolve<VoterDataRepository>();

                // Display search parameters in status bar
                // and turn on the spining progress wheel
                GlobalReferences.StatusBar.TextLeft = (string.Concat("Searching For: ", LastNameSearch.Text, " ", FirstNameSearch.Text, " ", BirthYearSearch.Text));
                //StatusBar.LoadingSpinner(Visibility.Visible);
                GlobalReferences.StatusBar.SpinnerVisibility = true;
                SearchingPanel.Visibility = Visibility.Visible;

                // Create the Voter Search Object (Search Parameters)
                VoterSearchModel searchItems = new VoterSearchModel
                {
                    VoterID = voterID
                };

                // Get list of voters from Search Parameters
                // Call voter lookup asynchronously so user can still perform tasks while the search is running
                // #### Might want to move the Async to the static method definition ####
                //var task = Task.Run(() => VoterDataMethods.VoterList(searchItems).OrderBy(o => o.LastName).Take(50));
                //var task = Task.Run(() => voterLookup.VoterList(searchItems).OrderBy(o => o.LastName).Take(50));
                //var list = await voterLookup.VoterListAsync(searchItems);

                int count = 0;
                //int count = await VoterMethods.Voter.ListCountAsync(searchItems);
                try
                {
                    var list = await VoterMethods.Voters.PagedListAsync(searchItems, 1, 50);

                    count = list.Count();
                    VoterList.ItemsSource = list;
                }
                catch (Exception e)
                {
                    GlobalReferences.StatusBar.TextCenter = ("SQL ERROR: " + e.Message);
                    return;
                }

                // Turn off the progress spinner
                //StatusBar.LoadingSpinner(Visibility.Collapsed);
                GlobalReferences.StatusBar.SpinnerVisibility = false;
                SearchingPanel.Visibility = Visibility.Collapsed;

                // Display result count in the status bar
                // #### We want to get the record count before reducing the return list ####
                GlobalReferences.StatusBar.TextLeft = (string.Concat("Results: ", count.ToString(), " voters found."));
                if (count < 1)
                {
                    SearchResults.Visibility = Visibility.Visible;
                    SearchResults.Text = string.Concat("Results: ", count.ToString(), " voters found.");
                }
                else if (count < 50)
                {
                    //StatusBar.ApplicationStatus(string.Concat("Results: ", count.ToString(), " voters found."));
                    SearchResultsText.Text = "Search Results:\r\n" + count.ToString() + " voters found.";
                }
                else
                {
                    //StatusBar.ApplicationStatus(string.Concat("Results too large!"));
                    //SearchResultsText.Text = "Search Results:\r\nFirst 50 of " + count.ToString() + " voters shown.\r\n\r\n";
                    //SearchResultsText.Text += "Try refining the search by\r\n";
                    //SearchResultsText.Text += SuggestRefinements(searchItems);
                }

                SearchScrollViewer.Focus();
            }
            //else
            //{
            //    StatusBar.ApplicationStatus("No Search Parameters Entered");
            //    SearchResultsText.Text = "Please enter valid search criteria.";
            //}
            Keyboard.Focus(BarCodeSearch);
        }

        private void ReturnToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Show menu button
            //MainMenuMethods.ShowHamburger();
            GlobalReferences.Header.HamburgerMenuVisibility = true;

            // Open Menu 
            //MainMenuMethods.OpenMainMenu();
            GlobalReferences.MenuSlider.Open();

            this.NavigateToPage(new AbsenteeAdministrationPage());
        }

        //private bool CheckServer()
        //{
        //    try
        //    {
        //        var test = ElectionMethods.GetElectionList().Count();
        //        return true;
        //    }
        //    catch (Exception error)
        //    {
        //        StatusBar.ApplicationStatusCenter("SQL ERROR: " + error.Message);
        //        return false;
        //    }
        //}

        //private string CheckVoterError(ObservableCollection<VoterDataModel> voterList)
        //{
        //    if (voterList != null)
        //    {
        //        foreach (VoterDataModel item in voterList)
        //        {
        //            if (item.Error != null)
        //            {
        //                return item.Error.Message;
        //            }
        //        }
        //    }
        //    return null;
        //}

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                ((Button)sender).IsEnabled = false;
            }
        }
    }
}
