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
//using VoterX.Core;
//using VoterX.Core.Interfaces;
//using VoterX.Core.Models.Database;
//using VoterX.Core.Repositories;
//using VoterX.Core.Models.Utilities;
//using VoterX.Core.Extensions;
//using VoterX.Core.Models.ViewModels;
using VoterX.Extensions;
using System.Windows.Media.Effects;
using System.Collections.ObjectModel;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for ProvisionalSearchPage.xaml
    /// </summary>
    public partial class ProvisionalSearchPage : Page
    {
        // Load home page when returning from another page
        //public ProvisionalSearchPage(VoterSearchModel searchItems)
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
        //    StatusBar.ApplicationPageHeader("Provisional Search");

        //    StatusBar.ApplicationStatusClear();

        //    // Check printer status            
        //    StatusBar.ApplicationCheckPrinterAsync();
        //}

        // Load home page for the first time
        public ProvisionalSearchPage() // 
        {
            InitializeComponent();

            //_logCodeRepository = ((App)Application.Current).voterContainer.Resolve<LogCodeRepo>();

            //VoterContainer.Resolve<LogCodeRepo>();

            //string name = _logCodeRepository.GetByID(16).log_description.ToString();

            //if (AppSettings.System.Login)
            //{
            //    LogoutButton.Visibility = Visibility.Visible;
            //}

            GlobalReferences.Header.PageHeader = ("Provisional Search");
            //StatusBar.ApplicationStatus("Begin search");
            //StatusBar.ApplicationStatusCenter("");
            //StatusBar.ApplicationStatusClear();
            GlobalReferences.StatusBar.TextClear();

            // Check printer status            
            //StatusBar.ApplicationCheckPrinterAsync();
            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter );
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

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
        private void SearchButton_Click(object sender, RoutedEventArgs e)
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
            //{
            //    // Get the voter data repository
            //    //var voterLookup = ((App)Application.Current).voterContainer.Resolve<VoterDataRepository>();

            //    // Display search parameters in status bar
            //    // and turn on the spining progress wheel
            //    StatusBar.ApplicationStatus(string.Concat("Searching For: ", LastNameSearch.Text, " ", FirstNameSearch.Text, " ", BirthYearSearch.Text));
            //    StatusBar.LoadingSpinner(Visibility.Visible);
            //    SearchingPanel.Visibility = Visibility.Visible;

            //    // Create the Voter Search Object (Search Parameters)
            //    //VoterSearchModel searchItems = new VoterSearchModel
            //    //{
            //    //    FirstName = FirstNameSearch.Text,
            //    //    LastName = LastNameSearch.Text,
            //    //    BirthYear = BirthYearSearch.Text
            //    //};

            //    // Get list of voters from Search Parameters
            //    // Call voter lookup asynchronously so user can still perform tasks while the search is running
            //    // #### Might want to move the Async to the static method definition ####
            //    //var task = Task.Run(() => VoterDataMethods.VoterList(searchItems).OrderBy(o => o.LastName).Take(50));
            //    //var task = Task.Run(() => voterLookup.VoterList(searchItems).OrderBy(o => o.LastName).Take(50));
            //    //var list = await voterLookup.VoterListAsync(searchItems);

            //    //int count = 0;
            //    //ObservableCollection<VoterDataModel> list = new ObservableCollection<VoterDataModel>();
            //    //try
            //    //{
            //    //    //int count = await VoterMethods.Voter.ListCountAsync(searchItems);
            //    //    //var list = await VoterMethods.Provisional.OrderedListAsync(searchItems);
            //    //    list = await VoterMethods.Voter.ProvisionalOrderedListAsync(searchItems);
            //    //    //list = VoterMethods.Voter.ProvisionalOrderedList(searchItems);
            //    //}
            //    //catch(Exception exception)
            //    //{
            //    //    StatusBar.ApplicationStatusCenter(exception.Message);
            //    //}

            //    //count = list.Count();
            //    //VoterList.ItemsSource = list;

            //    // Turn off the progress spinner
            //    StatusBar.LoadingSpinner(Visibility.Collapsed);
            //    SearchingPanel.Visibility = Visibility.Collapsed;

            //    // Display result count in the status bar
            //    // #### We want to get the record count before reducing the return list ####
            //    //StatusBar.ApplicationStatus(string.Concat("Results: ", count.ToString(), " voters found."));
            //    //if (count < 1)
            //    //{
            //    //    SearchResults.Visibility = Visibility.Visible;
            //    //    SearchResults.Text = string.Concat("Results: ", count.ToString(), " voters found.");
            //    //}
            //    //else if (count < 50)
            //    //{
            //    //    //StatusBar.ApplicationStatus(string.Concat("Results: ", count.ToString(), " voters found."));
            //    //    SearchResultsText.Text = "Search Results:\r\n" + count.ToString() + " voters found.";
            //    //}
            //    //else
            //    //{
            //    //    //StatusBar.ApplicationStatus(string.Concat("Results too large!"));
            //    //    SearchResultsText.Text = "Search Results:\r\nFirst 50 of " + count.ToString() + " voters shown.\r\n\r\n";
            //    //    SearchResultsText.Text += "Try refining the search by\r\n";
            //    //    SearchResultsText.Text += SuggestRefinements(searchItems);
            //    //}

            //    SearchScrollViewer.Focus();
            //}
            //else
            //{
            //    StatusBar.ApplicationStatus("No Search Parameters Entered");
            //    SearchResultsText.Text = "Please enter valid search criteria.";
            //}
        }

        // Returns true if any of the search boxes have a value
        //private bool SearchIsNotNullOrEmpty()
        //{
        //    //bool result = false;

        //    //var testFirst = FirstNameSearch.Text.IsNullOrEmpty();
        //    //var testLast = LastNameSearch.Text.IsNullOrEmpty();
        //    //var testYear = BirthYearSearch.Text.IsNullOrEmpty();

        //    //if (!FirstNameSearch.Text.IsNullOrEmpty()) result = true;
        //    //if (!LastNameSearch.Text.IsNullOrEmpty()) result = true;
        //    //if (!BirthYearSearch.Text.IsNullOrEmpty()) result = true;

        //    //result = !FirstNameSearch.Text.IsNullOrEmpty() || !LastNameSearch.Text.IsNullOrEmpty() || !BirthYearSearch.Text.IsNullOrEmpty();

        //    //return result;
        //    return
        //        !FirstNameSearch.Text.IsNullOrEmpty() ||
        //        !LastNameSearch.Text.IsNullOrEmpty() ||
        //        !BirthYearSearch.Text.IsNullOrEmpty();
        //}

        //private string SuggestRefinements(VoterSearchModel search)
        //{
        //    string message = "";

        //    if (search.FirstName.IsNullOrEmpty())
        //    {
        //        message += "adding a First Name";
        //        if (search.LastName.IsNullOrEmpty() || search.BirthYear.IsNullOrEmpty())
        //            message += ", or";
        //        else
        //            message += ".";
        //        message += "\r\n";
        //    }
        //    if (search.LastName.IsNullOrEmpty())
        //    {
        //        message += "adding a Last Name";
        //        if (search.BirthYear.IsNullOrEmpty())
        //            message += ", or";
        //        else
        //            message += ".";
        //        message += "\r\n";
        //    }
        //    if (search.BirthYear.IsNullOrEmpty())
        //        message += "adding a Birth Year.\r\n";

        //    return message;
        //}

        // Clear both the search boxes and the list view
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear Search Boxes
            //FirstNameSearch.Text = null;
            //LastNameSearch.Text = null;
            //BirthYearSearch.Text = null;
            ClearSearchBoxes();
            // Clear ListView
            //VoterList.ItemsSource = null;
            ClearListView();

            // Clear results message
            SearchResultsText.Text = "";
        }

        // Send selected voter and the search parameters to the next page
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                //StatusBar.ApplicationStatus(((VoterDataModel)item.DataContext).VoterID);

                // Get the search parameters
                //VoterSearchModel searchItems = new VoterSearchModel
                //{
                //    FirstName = FirstNameSearch.Text,
                //    LastName = LastNameSearch.Text,
                //    BirthYear = BirthYearSearch.Text
                //};

                // Merge the voter and the search parameters into a single object
                //VoterNavModel voterNav = new VoterNavModel
                //{
                //    // Get full voter details
                //    Voter = VoterMethods.Voter.ProvisionalSingle(((VoterDataModel)item.DataContext).VoterID).FirstOrDefault(),
                //    Search = searchItems
                //};

                // Hide Hamburger
                ((MainWindow)System.Windows.Application.Current.MainWindow).HamburgerButton.Visibility = Visibility.Collapsed;

                // Check where to send the voter next
                //RedirectVoter(voterNav);

                // Goto the navigation page
                //this.NavigateToPage(new VerifyValidVoterPage(voterNav));
                //this.NavigateToPage(new ProvisionalBallotPage(voterNav.Voter, true));
            }
        }

        //private Page ChooseVoterDestination(VoterNavModel voterNav)
        //{
        //    switch (VoterValidationMethods.CheckVoterStatus(voterNav.Voter, (int)AppSettings.System.SiteID))
        //    {
        //        case VoterLookupStatus.Eligible:
        //            return new VerifyValidVoterPage(voterNav);
        //        //break;
        //        case VoterLookupStatus.Ineligible:
        //            return new VerifyInvalidVoterPage(voterNav);
        //        //break;
        //        case VoterLookupStatus.Spoilable:
        //            return new VerifySpoiledBallotPage(voterNav);
        //        //break;
        //        case VoterLookupStatus.Provisional:
        //            return new VerifyProvisionalBallotPage(voterNav);
        //        //break;
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
            this.NavigateToPage(new LoginPage());
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
            Color myShadowColor = new Color();
            myShadowColor.ScA = 1;
            myShadowColor.ScB = 0;
            myShadowColor.ScG = 0;
            myShadowColor.ScR = 0;
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new AdministrationPage());
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                ((Button)sender).IsEnabled = false;
            }
        }
    }
}
