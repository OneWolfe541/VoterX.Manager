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

namespace VoterX
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class ApplicationSettingsPage : Page
    {
        public ApplicationSettingsPage()
        {
            InitializeComponent();

            GlobalReferences.StatusBar.TextLeft = ("Absentee Settings");

            LoadPageIntoFrame("Application");

            // Display page header
            GlobalReferences.Header.PageHeader = ("Absentee Settings");

            //StatusBar.ApplicationStatusClear();
            GlobalReferences.StatusBar.TextClear();

            //MainMenuMethods.HideHamburger();
            //GlobalReferences.Header.HamburgerMenuVisibility = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.SettingsChanged == true)
            {
                YesNoDialog sampleDialog = new YesNoDialog("Save Changes", "The system settings have been changed!\r\nDo you want to save your changes before exiting?");
                if (sampleDialog.ShowDialog() == true)
                    SaveSettings_Click(sender, e);

                // Update Save Changes flag
                AppSettings.SettingsChanged = false;

                //MainMenuMethods.ShowHamburger();
                GlobalReferences.Header.HamburgerMenuVisibility = true;
                //MainMenuMethods.OpenAdminMenu();
                GlobalReferences.MenuSlider.Open();
                this.NavigateToPage(new AdministrationPage());
            }
            else
            {
                // Update Save Changes flag
                AppSettings.SettingsChanged = false;

                //MainMenuMethods.ShowHamburger();
                GlobalReferences.Header.HamburgerMenuVisibility = true;
                //MainMenuMethods.OpenAdminMenu();
                GlobalReferences.MenuSlider.Open();
                this.NavigateToPage(new AdministrationPage());
            }
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

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
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

        private void ClearAllTabs()
        {
            ReturnAddressToggle.IsChecked = false;
        }

        private void ReturnAddressToggle_Click(object sender, RoutedEventArgs e)
        {
            ClearAllTabs();
            ReturnAddressToggle.IsChecked = true;
            LoadPageIntoFrame("Application");
        }
    }
}
