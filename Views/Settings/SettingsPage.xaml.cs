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
using System.Windows.Media.Effects;
using VoterX.Methods;
using VoterX.Extensions;
using VoterX.SystemSettings;
using VoterX.Dialogs;
using VoterX.Manager.Global;
using VoterX.Utilities.Models;
using VoterX.Manager.Menus;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            //GlobalReferences.StatusBar.TextLeft = ("System Settings");

            LoadPageIntoFrame("Absentee");

            // Display page header
            GlobalReferences.Header.PageHeader = ("System Settings");

            //StatusBar.ApplicationStatusClear();
            GlobalReferences.StatusBar.TextClear();

            //MainMenuMethods.LoadMenu(new SystemSettingsMenu(this));
            //GlobalReferences.MenuSlider.SetMenu(new SystemSettingsMenu(this), MenuCollapseMode.ShowIcons);
            GlobalReferences.MenuSlider.SetMenu(new SettingsMenuView(this), MenuCollapseMode.ShowIcons);

            //GlobalReferences.MenuSlider.Open();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextCenter = AppSettings.System.BODName + ":" + AppSettings.System.BODVersion;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.SetMenuMinWidth(0);

            //MainMenuMethods.ShowMenuMinimum();

            //MainMenuMethods.ToggleElectionVisibility();
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.SettingsChanged == true)
            {
                YesNoDialog sampleDialog = new YesNoDialog("Save Changes", "The system settings have been changed!\r\nDo you want to save your changes before exiting?");
                if (sampleDialog.ShowDialog() == true)
                    SaveSettings_Click(sender, e);

                // Update Save Changes flag
                AppSettings.SettingsChanged = false;
                //SaveLogout();
                this.NavigateToPage(new AbsenteeLoginPage());
            }
            else
            {
                // Update Save Changes flag
                AppSettings.SettingsChanged = false;
                //SaveLogout();
                this.NavigateToPage(new AbsenteeLoginPage());
            }
        }

        private void SaveLogout()
        {
            GlobalReferences.StatusBar.TextLeft = ("Logging In");
            //StatusBar.LoadingSpinner(Visibility.Visible);
            GlobalReferences.StatusBar.SpinnerVisibility = true;
            //if (await Task.Run(() => PollSummaryMethods.SaveLogout()) == false) StatusBar.ApplicationStatusCenter("Logout not saved");
            GlobalReferences.StatusBar.TextLeft = ("");
            //StatusBar.LoadingSpinner(Visibility.Collapsed);
            GlobalReferences.StatusBar.SpinnerVisibility = false;
        }

        private void CreateSettings_Click(object sender, RoutedEventArgs e)
        {
            SystemSettingsController settings = new SystemSettingsController();

            settings.System.SiteID = 459;

            settings.CreateJsonFile();
        }

        private void LoadSettings_Click(object sender, RoutedEventArgs e)
        {
            SystemSettingsController settings = new SystemSettingsController();

            settings.LoadJsonFile();

            GlobalReferences.StatusBar.TextLeft = ("Settings Loaded for Site: " + settings.System.SiteID.ToString());
        }

        private void ChangeSettings_Click(object sender, RoutedEventArgs e)
        {
            //((App)Application.Current).GlobalSettings.Systems.SiteName = "Testing the stuff";
            //((App)Application.Current).GlobalSettings.SaveSettings();
        }

        public void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            // Load textboxes into global object
            //UpdateSettings();

            var setG = AppSettings.Printers;

            AlertDialog sampleDialog = new AlertDialog("Settings Saved!");
            if (sampleDialog.ShowDialog() == true)
                GlobalReferences.StatusBar.TextLeft = ("Settings Saved");

            var settingsPage = (SettingsBasePage)SettingsFrame.Content;         // Get active page
            settingsPage.SaveSettings();                                        // Copy settings from page to global object

            // Write system settings to the file
            AppSettings.SaveChanges();
            AppSettings.SaveAbsentee();

            // Update Save Changes flag
            AppSettings.SettingsChanged = false;
        }

        // Resets all tabs to their default and lowered state
        //public void LowerAllTabs()
        //{
        //    SystemSettingsTab.FontSize = 20;
        //    SystemSettingsTab.FontWeight = FontWeights.Normal;

        //    NetworkSettingsTab.FontSize = 20;
        //    NetworkSettingsTab.FontWeight = FontWeights.Normal;

        //    PrintersSettingsTab.FontSize = 20;
        //    PrintersSettingsTab.FontWeight = FontWeights.Normal;

        //    ElectionSettingsTab.FontSize = 20;
        //    ElectionSettingsTab.FontWeight = FontWeights.Normal;

        //    BallotSettingsTab.FontSize = 20;
        //    BallotSettingsTab.FontWeight = FontWeights.Normal;

        //    UserSettingsTab.FontSize = 20;
        //    UserSettingsTab.FontWeight = FontWeights.Normal;

        //    ServerSettingsTab.FontSize = 20;
        //    ServerSettingsTab.FontWeight = FontWeights.Normal;
        //}

        // Remove all tab borders
        //public void ClearBorderTabs()
        //{
        //    Thickness tabBorder = new Thickness(1, 1, 1, 1);

        //    SystemBorder.BorderThickness = tabBorder;
        //    SystemBorder.Background = new SolidColorBrush(Colors.Transparent);
        //    SystemBorder.Effect = null;

        //    NetworkBorder.BorderThickness = tabBorder;
        //    NetworkBorder.Background = new SolidColorBrush(Colors.Transparent);
        //    NetworkBorder.Effect = null;

        //    PrinterBorder.BorderThickness = tabBorder;
        //    PrinterBorder.Background = new SolidColorBrush(Colors.Transparent);
        //    PrinterBorder.Effect = null;

        //    ElectionBorder.BorderThickness = tabBorder;
        //    ElectionBorder.Background = new SolidColorBrush(Colors.Transparent);
        //    ElectionBorder.Effect = null;

        //    BallotBorder.BorderThickness = tabBorder;
        //    BallotBorder.Background = new SolidColorBrush(Colors.Transparent);
        //    BallotBorder.Effect = null;

        //    UserBorder.BorderThickness = tabBorder;
        //    UserBorder.Background = new SolidColorBrush(Colors.Transparent);
        //    UserBorder.Effect = null;

        //    ServerBorder.BorderThickness = tabBorder;
        //    ServerBorder.Background = new SolidColorBrush(Colors.Transparent);
        //    ServerBorder.Effect = null;
        //}

        // Set all setting grids to collapsed
        public void HideAllGrids()
        {
            //SystemGrid.Visibility = Visibility.Collapsed;
            //NetworkGrid.Visibility = Visibility.Collapsed;
            //PrinterGrid.Visibility = Visibility.Collapsed;
            //ElectionGrid.Visibility = Visibility.Collapsed;
            //BallotGrid.Visibility = Visibility.Collapsed;
        }

        private void HighlightSelectedTab(TextBlock selectedTab, Border selectedBorder)
        {
            selectedTab.FontSize = 24;
            selectedTab.FontWeight = FontWeights.Bold;

            selectedBorder.BorderThickness = new Thickness(1, 1, 1, 1);
            selectedBorder.Background = new SolidColorBrush(Colors.White);
            selectedBorder.Effect =
                new DropShadowEffect
                {
                    Color = new Color { A = 0, R = 0, G = 0, B = 0 },
                    Direction = 320,
                    ShadowDepth = 2,
                    Opacity = 5
                };
        }

        private void LoadPageIntoFrame(string pageName)
        {
            SettingsFrame.Navigate(new System.Uri("Views/Settings/" + pageName + "Page.xaml", UriKind.RelativeOrAbsolute));
            SettingsFrame.DataContext = pageName;
        }

        // Raise and highlight the System tab
        //private void System_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    StatusBar.ApplicationStatus("System Setup");

        //    LowerAllTabs();
        //    //HideAllGrids();
        //    ClearBorderTabs();

        //    HighlightSelectedTab(SystemSettingsTab, SystemBorder);

        //    LoadPageIntoFrame("System");
        //}

        // Raise and highlight the Network tab
        //private void Network_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    StatusBar.ApplicationStatus("Network Setup");

        //    LowerAllTabs();
        //    //HideAllGrids();
        //    ClearBorderTabs();

        //    HighlightSelectedTab(NetworkSettingsTab, NetworkBorder);

        //    LoadPageIntoFrame("Network");
        //}

        // Raise and highlight the Printer tab
        //private void Printer_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    StatusBar.ApplicationStatus("Printer Setup");

        //    LowerAllTabs();
        //    //HideAllGrids();
        //    ClearBorderTabs();

        //    HighlightSelectedTab(PrintersSettingsTab, PrinterBorder);

        //    LoadPageIntoFrame("Printers");
        //}

        // Raise and highlight the Election tab
        //private void Election_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    StatusBar.ApplicationStatus("Election Setup");

        //    LowerAllTabs();
        //    //HideAllGrids();
        //    ClearBorderTabs();

        //    HighlightSelectedTab(ElectionSettingsTab, ElectionBorder);

        //    LoadPageIntoFrame("Election");
        //}

        // Raise and highlight the Ballot tab
        //private void Ballot_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    StatusBar.ApplicationStatus("Ballot Setup");

        //    LowerAllTabs();
        //    //HideAllGrids();
        //    ClearBorderTabs();

        //    HighlightSelectedTab(BallotSettingsTab, BallotBorder);

        //    LoadPageIntoFrame("Ballots");
        //}

        //private void User_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    LowerAllTabs();
        //    //HideAllGrids();
        //    ClearBorderTabs();

        //    HighlightSelectedTab(UserSettingsTab, UserBorder);

        //    LoadPageIntoFrame("User");
        //}

        //private void Server_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    LowerAllTabs();
        //    //HideAllGrids();
        //    ClearBorderTabs();

        //    HighlightSelectedTab(ServerSettingsTab, ServerBorder);

        //    LoadPageIntoFrame("Servers");
        //}

        private void ClearAllTabs()
        {
            AbsenteeToggle.IsChecked = false;
            SystemToggle.IsChecked = false;
            NetworkToggle.IsChecked = false;
            PrinterToggle.IsChecked = false;
            ElectionToggle.IsChecked = false;
            BallotsToggle.IsChecked = false;
            UserToggle.IsChecked = false;
            ServerToggle.IsChecked = false;
        }

        public void AbsenteeToggle_Click(object sender, RoutedEventArgs e)
        {
            AbsenteePage();
        }

        public void AbsenteePage()
        {
            ClearAllTabs();
            AbsenteeToggle.IsChecked = true;
            LoadPageIntoFrame("Absentee");

            SettingsHeader.Text = "ABSENTEE";
        }

        public void SystemToggle_Click(object sender, RoutedEventArgs e)
        {
            SystemsPage();
        }

        public void SystemsPage()
        {
            var settingsPage = (SettingsBasePage)SettingsFrame.Content;         // Get active page
            settingsPage.SaveSettings();                                        // Copy settings from page to global object

            ClearAllTabs();
            SystemToggle.IsChecked = true;
            LoadPageIntoFrame("System");

            SettingsHeader.Text = "EARLY VOTING";
        }

        public void NetworkToggle_Click(object sender, RoutedEventArgs e)
        {
            NetworkPage();
        }

        public void NetworkPage()
        {
            var settingsPage = (SettingsBasePage)SettingsFrame.Content;         // Get active page
            settingsPage.SaveSettings();                                        // Copy settings from page to global object

            ClearAllTabs();
            NetworkToggle.IsChecked = true;
            LoadPageIntoFrame("Network");

            SettingsHeader.Text = "NETWORK";
        }

        public void PrinterToggle_Click(object sender, RoutedEventArgs e)
        {
            PrintersPage();
        }

        public void PrintersPage()
        {
            var settingsPage = (SettingsBasePage)SettingsFrame.Content;         // Get active page
            settingsPage.SaveSettings();                                        // Copy settings from page to global object

            ClearAllTabs();
            PrinterToggle.IsChecked = true;
            LoadPageIntoFrame("Printers");

            SettingsHeader.Text = "PRINTERS";
        }

        public void ElectionToggle_Click(object sender, RoutedEventArgs e)
        {
            ElectionPage();
        }

        public void ElectionPage()
        {
            var settingsPage = (SettingsBasePage)SettingsFrame.Content;         // Get active page
            settingsPage.SaveSettings();                                        // Copy settings from page to global object

            ClearAllTabs();
            ElectionToggle.IsChecked = true;
            LoadPageIntoFrame("Election");

            SettingsHeader.Text = "ELECTION";
        }

        public void BallotsToggle_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void BallotsPage()
        {
            var settingsPage = (SettingsBasePage)SettingsFrame.Content;         // Get active page
            settingsPage.SaveSettings();                                        // Copy settings from page to global object

            ClearAllTabs();
            BallotsToggle.IsChecked = true;
            LoadPageIntoFrame("Ballots");

            SettingsHeader.Text = "BALLOTS";
        }

        private void UserToggle_Click(object sender, RoutedEventArgs e)
        {
            var settingsPage = (SettingsBasePage)SettingsFrame.Content;         // Get active page
            settingsPage.SaveSettings();                                        // Copy settings from page to global object

            ClearAllTabs();
            UserToggle.IsChecked = true;
            LoadPageIntoFrame("User");
        }

        private void ServerToggle_Click(object sender, RoutedEventArgs e)
        {
            var settingsPage = (SettingsBasePage)SettingsFrame.Content;         // Get active page
            settingsPage.SaveSettings();                                        // Copy settings from page to global object

            ClearAllTabs();
            ServerToggle.IsChecked = true;
            LoadPageIntoFrame("Servers");
        }
    }
}
