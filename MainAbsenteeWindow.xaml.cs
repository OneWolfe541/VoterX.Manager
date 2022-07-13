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
using VoterX.SystemTrayMenu;
using VoterX.Methods;
using VoterX.Extensions;
using System.Deployment.Application;
using System.Windows.Media.Animation;
using System.Configuration;
using VoterX.Utilities.Views;
using VoterX.Manager.Global;
using System.ComponentModel;
using VoterX.SystemSettings.Enums;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for MainAbsenteeWindow.xaml
    /// </summary>
    public partial class MainAbsenteeWindow : Window
    {
        public int menuMinWidth = 0;

        public MainAbsenteeWindow()
        {
            InitializeComponent();            

            Style = (Style)FindResource(typeof(Window));

            // Setup window drag feature
            MouseDown += DragMyWindow;

            LoadHeader(Window.GetWindow(this));

            LoadStatusBar();

            MainFrame.Navigate(new System.Uri("Views/Login/AbsenteeLoginPage.xaml", UriKind.RelativeOrAbsolute));
            //MainFrame.Navigate(new System.Uri("Views/Voter/SearchPage.xaml", UriKind.RelativeOrAbsolute));
            MainFrame.Navigating += OnNavigating;

            //DynamicMenuSlider.SetMenuSource(new System.Uri("Menus/AbsenteeMenu.xaml", UriKind.RelativeOrAbsolute));

            // Maximize the window to fit with Task Bar
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

            // Maximize the application window if not in Debug Mode (AKA Running on my development computer)
            if (((App)Application.Current).debugMode == false)
            {
                Maximize();
            }

            // Display application version in lower right corner of status bar
            //ApplicationStatusRight.Text = AppSettings.System.BODName + " v" + AppSettings.System.BODVersion.ToString() + "";

            //PageHeaderName.Text = "Absentee";
            GlobalReferences.Header.PageHeader = ("Absentee");

            ((App)Application.Current).mainSliderMenu = DynamicMenuSlider;

            //ApplicationStatusLeft.Text = StatusBar.GetStatusBar();

            //ApplicationStatusLeft.Text = AppSettings.AbsenteeMode.ToString();

            SetElectionTitle();

            //ApplicationStatusRight.Text = "Copyright © AUTOMATED ELECTION SERVICES 2018";
        }

        // Disable the F5 (refresh) hot key
        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            ((Frame)sender).NavigationService.RemoveBackEntry();

            if (e.NavigationMode == NavigationMode.Refresh)
                e.Cancel = true;
        }

        public void SetElectionTitle()
        {
            //ElectionEntity.Text = ((App)Application.Current).GlobalSettings.Election.ElectionEntity;
            //ElectionName.Text = ((App)Application.Current).GlobalSettings.Election.ElectionTitle;
            ////PollLocationName.Text = ((App)Application.Current).GlobalSettings.System.SiteName;
            //PollLocationName.Text = "Absentee System";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Minimize window
            //this.Hide();
            //AppSettings.SaveChanges();
            //MainMenuMethods.CloseMainWindow();
            GlobalReferences.MenuSlider.Close();
            Close();
        }

        private void DragMyWindow(object sender, MouseButtonEventArgs e)
        {
            // Drag the main window when left mouse button is pressed in the title bar area
            if (e.ChangedButton == MouseButton.Left && e.GetPosition(this).Y < 65)
            {
                DragMove();
            }
        }

        // Maximize the window when mouse double clicks in the title bar area
        private void MaximizeMyWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && e.GetPosition(this).Y < 75)
            {
                WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            }
        }

        private void Maximize()
        {
            WindowState = WindowState.Maximized;
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Clear Status bar messages
            ((App)Application.Current).statusBar.TextClear();

            // Remove previous pages from the main window frame's navigation history
            MainFrame.NavigationService.RemoveBackEntry();
            GC.Collect();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            DynamicMenuSlider.Toggle();
        }

        private void LoadStatusBar()
        {
            // Get connection string
            //string connection = "Server=AESSQL2;Database=StateDBRevision;Trusted_Connection=True;";
            string connection = ConfigurationManager.ConnectionStrings["VoterDatabase"].ToString();

            // Initialize Status Bar View Model
            ((App)Application.Current).statusBar = new StatusBarViewModel(connection);

            // Bind View Model to Control
            StatusBarUserControl.DataContext = ((App)Application.Current).statusBar;

            //((App)Application.Current).StatusBar.TextLeft = "This is the status bar.";            
            ((App)Application.Current).statusBar.DisplaySystemMode(VotingCenterMode.Absentee);
        }

        private void LoadHeader(Window parent)
        {
            ((App)Application.Current).mainHeader = new MainHeaderViewModel(parent);
            ((App)Application.Current).mainHeader.PropertyChanged += OnHeaderPropertyChanged;
            MainHeaderControl.DataContext = ((App)Application.Current).mainHeader;
        }

        private void OnHeaderPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MenuClicked")
            {
                DynamicMenuSlider.Toggle();
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            //AppSettings.SaveChanges();
            Close();
        }

        private void SystemSettings_Click(object sender, RoutedEventArgs e)
        {
            DynamicMenuSlider.Toggle();

            MainFrame.Navigate(new System.Uri("Views/Settings/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void VoterLookupMenu_Click(object sender, RoutedEventArgs e)
        {
            DynamicMenuSlider.Toggle();
            MainFrame.Navigate(new System.Uri("Views/Voter/SearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ReportsMenu_Click(object sender, RoutedEventArgs e)
        {
            DynamicMenuSlider.Toggle();

            MainFrame.Navigate(new System.Uri("Views/Admin/ReportsMenuPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void MenuLogOut_Click(object sender, RoutedEventArgs e)
        {
            DynamicMenuSlider.Toggle();

            MainFrame.Navigate(new System.Uri("Views/Login/AbsenteeLoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                AbsenteeDetailsPage voterdetails = MainFrame.GetChildOfTypeFrame<AbsenteeDetailsPage>();
                if (voterdetails != null)
                {
                    voterdetails.SaveChanges();
                }
            }

            if (e.Key == Key.Back)
            {
                e.Handled = true;
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            //AppSettings.SaveChanges();
            Close();

            Application.Current.Shutdown();
        }
    }
}
