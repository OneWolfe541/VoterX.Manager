using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using VoterX.ABS.Logging;
using VoterX.Methods;
using VoterX.Core.Voters;
using VoterX.Utilities.Models;
using VoterX.Utilities.Views;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Views.Super
{
    /// <summary>
    /// Interaction logic for VoterSearchPage.xaml
    /// </summary>
    public partial class SuperSearchPage : Page
    {
        private int? _siteId;

        private VoterSearchModel _searchItems;

        public SuperSearchPage() : this(null) { }
        public SuperSearchPage(int? siteId)
        {
            InitializeComponent();

            _siteId = siteId;

            GlobalReferences.Header.PageHeader = ("Edit Voter Search");

            GlobalReferences.StatusBar.TextClear();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalReferences.Header.PageHeader = ("Edit Voter Search");

            GlobalReferences.StatusBar.TextClear();

            GlobalReferences.MenuSlider.SetMenu(new Menus.SuperSearchMenuView(this), MenuCollapseMode.ShowIcons);
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationMenuMethods.LoginPage();
        }

        private void ReturnToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Show menu button
            //MainMenuMethods.ShowHamburger();
            GlobalReferences.Header.HamburgerMenuVisibility = true;

            // Open Menu 
            //MainMenuMethods.OpenMainMenu();
            GlobalReferences.MenuSlider.Open();

            NavigationMenuMethods.NavigateToPage(new Views.Super.SuperSearchPage());
        }

        #region VoterListView
        // Load Default Search View
        private void VoterSearchView_Loaded(object sender, RoutedEventArgs e)
        {
            VoterSearchViewModel voterViewModelObject = new VoterSearchViewModel(AppSettings.Global);

            voterViewModelObject.PropertyChanged += OnVoterSelectedPropertyChanged;

            voterViewModelObject.SearchAnimation = false;

            VoterSearchView.DataContext = voterViewModelObject;

            if (_siteId == 0)
            {
                VoterSearchModel searchItems = new VoterSearchModel();
                SearchForVoters(searchItems);
            }
        }

        // Get the Selected Voter
        private void OnVoterSelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedVoter")
            {
                //Code to respond to a change in the ViewModel
                Console.WriteLine(
                    "Selected Voter: " +
                    ((VoterSearchViewModel)sender).SelectedVoter.Data.VoterID.ToString() + " | STATUS: " +
                    ((VoterSearchViewModel)sender).SelectedVoter.CheckStatus((int)AppSettings.System.SiteID));

                // Get selected voter
                var selectedVoter = ((VoterSearchViewModel)sender).SelectedVoter;

                VoterNavModel voterNav = new VoterNavModel
                {
                    Voter = selectedVoter,
                    Search = _searchItems
                };

                // Navigate to next page based on the selected voters status
                //NavigationMenuMethods.NavigateToPage(ChooseVoterDestination(selectedVoter, (int)AppSettings.System.SiteID));

                // Check for SERVIS lookup
                //bool SERVISlookup = false;
                //if (_siteId == 0) SERVISlookup = true;

                NavigationMenuMethods.NavigateToPage(new SuperDetailsPage(selectedVoter));
            }
        }

        // Search for a Voter with the given parameters
        private async void SearchForVoters(VoterSearchModel SearchItems)
        {
            _searchItems = SearchItems;

            VoterSearchViewModel voterViewModelObject = new VoterSearchViewModel(AppSettings.Global);

            // Wire up Voter Selection
            voterViewModelObject.PropertyChanged += OnVoterSelectedPropertyChanged;

            // Start Search Animation
            voterViewModelObject.SearchAnimation = true;
            VoterSearchView.DataContext = voterViewModelObject;

            // Load voters from database by passing a Query method to the view model
            try
            {
                var result = await voterViewModelObject.LoadVotersAsync(
                    () => VoterMethods.Voters.PagedListAsync(SearchItems, _siteId, 0, 50)
                    );
            }
            catch (Exception e)
            {
                GlobalReferences.StatusBar.TextCenter = e.Message;

                var searchlog = new VoterXLogger("VCClogs", true);
                searchlog.WriteLog("Load Voter List Failed: " + e.Message);
                if (e.InnerException != null)
                {
                    searchlog.WriteLog(e.InnerException.Message);
                }
            }

            // Stop Search Animation
            voterViewModelObject.SearchAnimation = false;
            VoterSearchView.DataContext = null;

            // Display List of Voters
            VoterSearchView.DataContext = voterViewModelObject;
        }

        // Clear the Search View
        private void ClearVotersList()
        {
            VoterSearchViewModel voterViewModelObject = new VoterSearchViewModel(AppSettings.Global)
            {
                SearchAnimation = false
            };

            VoterSearchView.DataContext = voterViewModelObject;
        }
        #endregion VoterListView


        #region SearchByName
        // Load initial Search By Name View
        private void VoterSearchName_Loaded(object sender, RoutedEventArgs e)
        {
            LoadVoterSearchName();
        }

        private void LoadVoterSearchName()
        {
            // Initialize the Name Search panel
            VoterSearchNameViewModel voterSearchNameViewModel;
            if (_searchItems == null)
            {
                // Create Blank search panel
                voterSearchNameViewModel = new VoterSearchNameViewModel();
            }
            else
            {
                // Create a panel with values from the previous search
                voterSearchNameViewModel = new VoterSearchNameViewModel(_searchItems);
            }

            // Wire up property changed methods
            voterSearchNameViewModel.PropertyChanged += OnSearchNamePropertyChanged;
            voterSearchNameViewModel.PropertyChanged += OnSearchNameClearPropertyChanged;
            voterSearchNameViewModel.PropertyChanged += OnSearchNameScanPropertyChanged;

            VoterSearchNameView.DataContext = voterSearchNameViewModel;
        }

        // Get Search Parameters from View
        private void OnSearchNamePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VoterSearch")
            {
                // Search for voters
                Console.WriteLine("Search Options: " + ((VoterSearchNameViewModel)sender).VoterSearch.LastName);
                SearchForVoters(((VoterSearchNameViewModel)sender).VoterSearch);
            }
        }

        // Get the Clear command from View
        private void OnSearchNameClearPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VoterClear")
            {
                // Clear the list
                Console.WriteLine("Search Cleared: ");
                ClearVotersList();
            }
        }

        // Get the Switch command from View
        private void OnSearchNameScanPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VoterScan")
            {
                // Switch to Scan View
                Console.WriteLine("Search Set To Scan: ");
                VoterSearchNameView.Visibility = Visibility.Collapsed;
                VoterSearchScanView.Visibility = Visibility.Visible;
            }
        }
        #endregion SearchByName


        #region SearchByNumber
        // Load initial Search By Number View
        private void VoterSearchScan_Loaded(object sender, RoutedEventArgs e)
        {
            LoadVoterSearchScan();
        }

        private void LoadVoterSearchScan()
        {
            VoterSearchScanViewModel voterSearchScanViewModel = new VoterSearchScanViewModel();

            // Wire up property changed methods
            voterSearchScanViewModel.PropertyChanged += OnSearchScanPropertyChanged;
            voterSearchScanViewModel.PropertyChanged += OnSearchScanClearPropertyChanged;
            voterSearchScanViewModel.PropertyChanged += OnSearchScanSwtichPropertyChanged;

            VoterSearchScanView.DataContext = voterSearchScanViewModel;
        }

        // Get Search Parameters from View
        private void OnSearchScanPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VoterSearch")
            {
                // Search for a voter
                Console.WriteLine("Search Options: " + ((VoterSearchScanViewModel)sender).VoterSearch.VoterID);
                SearchForVoters(((VoterSearchScanViewModel)sender).VoterSearch);
            }
        }

        // Get the Clear command from View
        private void OnSearchScanClearPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VoterClear")
            {
                // Clear the list
                Console.WriteLine("Search Cleared: ");
                ClearVotersList();
            }
        }

        // Get the Switch command from View
        private void OnSearchScanSwtichPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VoterSwitch")
            {
                // Switch to Name View
                Console.WriteLine("Search Set To Name: ");
                VoterSearchNameView.Visibility = Visibility.Visible;
                VoterSearchScanView.Visibility = Visibility.Collapsed;
            }
        }
        #endregion SearchByNumber  
    }
}
