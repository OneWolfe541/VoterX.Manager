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
using System.Deployment.Application;
using System.Windows.Media.Animation;
using System.Security.Cryptography;

// https://www.codeproject.com/Articles/768427/The-big-MVVM-Template

namespace VoterX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private SystemTray sysTray;

        public MainWindow()
        {
            InitializeComponent();

            // Create the system tray menu
            //sysTray = new SystemTray(this);

            Style = (Style)FindResource(typeof(Window));

            // Adrian's design
            // https://xd.adobe.com/spec/8dc630ec-fe99-4a62-86e5-84598dea1640/#screen/7d16c9d9-847c-49ae-8c2c-06c43d86f7d4/Log%20On%20-%20Home
            //TitleBarRectangle.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#446cb3"));
            //StatusBarRectangle.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#606078"));

            // Setup window drag feature
            MouseDown += DragMyWindow;

            // Load first page into the main window frame
            // Check if login required -- For development purposes only
            //if (AppSettings.System.Login)
            //{
            //    //LogoutButton.Visibility = Visibility.Visible;
            //    MainFrame.Navigate(new System.Uri("Views/Login/LoginPage.xaml", UriKind.RelativeOrAbsolute));

            //    // File the screen
            //    Maximize();
            //}
            //else
            //{
            //    //CloseButton.Visibility = Visibility.Visible;
            //    MainFrame.Navigate(new System.Uri("Views/Voter/SearchPage.xaml", UriKind.RelativeOrAbsolute));
            //    //MainFrame.Navigate(new System.Uri("Views/Settings/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
            //}

            //MainFrame.Navigate(new System.Uri("Views/Voter/SearchPage.xaml", UriKind.RelativeOrAbsolute));

            // Maximize the window
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            //WindowState = WindowState.Maximized;

            // Get application version 
            // https://stackoverflow.com/questions/23459806/how-can-i-read-wpf-publish-version-number-in-code-behind
            // https://stackoverflow.com/questions/3546691/reading-assembly-version-information-of-wpf-application?rq=1
            try
            {
                //// get deployment version
                AppSettings.System.BODVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();                
            }
            catch (InvalidDeploymentException)
            {
                //// you cannot read publish version when app isn't installed 
                //// (e.g. during debug)
                //ApplicationStatusRight.Text = "version not installed";
            }
            // Display application version in lower right corner of status bar
            ApplicationStatusRight.Text = AppSettings.System.BODName + " v" + AppSettings.System.BODVersion.ToString() + "";

            PageHeaderName.Text = "App Start";

            if (AppSettings.AbsenteeMode)
            {
                // Then absentee type
                MainAbsenteeWindow AbsenteeWindow = new MainAbsenteeWindow();
                AbsenteeWindow.Show();

                this.Hide();
            }
        }

        // https://stackoverflow.com/questions/16999361/obtain-sha-256-string-of-a-string
        //public static String sha256_hash(String value)
        //{
        //    StringBuilder Sb = new StringBuilder();

        //    using (SHA256 hash = SHA256Managed.Create())
        //    {
        //        Encoding enc = Encoding.UTF8;
        //        Byte[] result = hash.ComputeHash(enc.GetBytes(value));

        //        foreach (Byte b in result)
        //            Sb.Append(b.ToString("x2"));
        //    }

        //    return Sb.ToString();
        //}

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Minimize window
            //this.Hide();
            //AppSettings.SaveChanges();
            Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new System.Uri("Views/Login/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void DragMyWindow(object sender, MouseButtonEventArgs e)
        {
            // Drag the main window when left mouse button is pressed in the title bar area
            if (e.ChangedButton == MouseButton.Left && e.GetPosition(this).Y < 75)
            {
                DragMove();
            }
        }

        // Maximize the window when mouse double clicks in the title bar area
        private void MaximizeMyWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            }
        }

        private void Maximize()
        {
            WindowState = WindowState.Maximized;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            // Load the settings page into the main window frame
            MainFrame.Navigate(new System.Uri("Views/Settings/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Load the login page into the main window frame
            MainFrame.Navigate(new System.Uri("Views/Login/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void GemBox_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new System.Uri("GemBoxPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Printing_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new System.Uri("Views/Printing/PDFiumPrintingPage.xaml", UriKind.RelativeOrAbsolute));
            //MainFrame.Navigate(new System.Uri("Views/Printing/LPRPrintingPage.xaml", UriKind.RelativeOrAbsolute));
            MainFrame.Navigate(new System.Uri("Views/Printing/PDFToolsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Remove previous pages from the main window frame's navigation history
            MainFrame.NavigationService.RemoveBackEntry();
            GC.Collect();
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new System.Uri("Views/Reports/FastReports.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Unicode_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new System.Uri("Views/Testing/UnicodeCharacterTestingPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void CheckBoxTest_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new System.Uri("Views/Testing/CheckBoxTestPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuSlider();
        }

        public void OpenMenuSlider()
        {
            if (MenuSlider.DataContext.ToString() == "Close")
            {
                MenuSlider.DataContext = "Open";
                AnimateMenuSliderOpen(MenuSlider);
            }
        }

        public void CloseMenuSlider()
        {
            if (MenuSlider.DataContext.ToString() == "Open")
            {
                MenuSlider.DataContext = "Close";
                AnimateMenuSliderClose(MenuSlider);
            }
        }

        private void ToggleMenuSlider()
        {
            if (MenuSlider.DataContext.ToString() == "Close")
            {
                AnimateMenuSliderOpen(MenuSlider);
            }
            else
            {
                AnimateMenuSliderClose(MenuSlider);
            }
        }

        private void AnimateMenuSliderOpen(Border menu)
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 0,
                To = 300,
                Duration = TimeSpan.FromSeconds(.2)
            };

            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Border.WidthProperty));
            Storyboard.SetTarget(widthAnimation, menu);

            Storyboard s = new Storyboard();
            s.Children.Add(widthAnimation);
            s.Begin();

            menu.DataContext = "Open";
        }

        private void AnimateMenuSliderClose(Border menu)
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 300,
                To = 0,
                Duration = TimeSpan.FromSeconds(.2)
            };

            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Border.WidthProperty));
            Storyboard.SetTarget(widthAnimation, menu);

            Storyboard s = new Storyboard();
            s.Children.Add(widthAnimation);
            s.Begin();

            menu.DataContext = "Close";
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            //AppSettings.SaveChanges();
            Close();
        }

        private void SystemSettings_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuSlider();

            MainFrame.Navigate(new System.Uri("Views/Settings/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ProvisionalSearch_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuSlider();

            MainFrame.Navigate(new System.Uri("Views/Admin/ProvisionalSearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void MenuLogOut_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuSlider();

            MainFrame.Navigate(new System.Uri("Views/Login/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ChangeBallotStyle_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuSlider();

            MainFrame.Navigate(new System.Uri("Views/Admin/EditBallotSearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void AddProvisional_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuSlider();

            MainFrame.Navigate(new System.Uri("Views/Admin/AddProvisionalPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ReportsMenu_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuSlider();

            MainFrame.Navigate(new System.Uri("Views/Admin/ReportsMenuPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void TestMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuSlider();

            MainFrame.Navigate(new System.Uri("Views/Admin/RNDTestingPage.xaml", UriKind.RelativeOrAbsolute));
        }

        public void HideSettingsMenu()
        {
            SystemSettings.Visibility = Visibility.Collapsed;
        }

        public void ShowSettingsMenu()
        {
            SystemSettings.Visibility = Visibility.Visible;
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ModalBackground.Visibility = Visibility.Collapsed;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            //AppSettings.SaveChanges();
            Close();

            Application.Current.Shutdown();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
