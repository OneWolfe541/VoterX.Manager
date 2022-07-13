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
using VoterX.Methods;
//using VoterX.Core.Models.Database;
using VoterX.Dialogs;
using VoterX.Core.Elections;
using VoterX.Manager.Global;
using VoterX.ABS.Logging;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class AbsenteeLoginPage : Page
    {
        private bool pwCorrect = false;

        VoterXLogger ABSLogger;

        public AbsenteeLoginPage()
        {
            InitializeComponent();

            ShowExitButton();

            GlobalReferences.Header.PageHeader = ("Log on");

            //StatusBar.ApplicationStatusClear();
            GlobalReferences.StatusBar.TextClear();

            CloseAdminMenu();
            //HideMenu();

            AppSettings.SetElectionMode();

            CheckServer();

            Keyboard.Focus(Password);

            ABSLogger = new VoterXLogger("ABSLogs", true);
            if (AppSettings.Election.ElectionType == VoterX.SystemSettings.Enums.ElectionType.General)
            {
                ABSLogger.WriteLog("Election Type: GENERAL - " + AppSettings.Election.ElectionType.ToString());
            }
            else
            {
                ABSLogger.WriteLog("Election Type: " + AppSettings.Election.ElectionType.ToString());
            }
        }

        private async void CheckServer()
        {
            LoginFields.Visibility = Visibility.Collapsed;
            LoadingMessage.Visibility = Visibility.Visible;

            await PutTaskDelay(100);

            GlobalReferences.StatusBar.TextLeft = ("Checking Server Connection");
            //StatusBar.LoadingSpinner(Visibility.Visible);

            //StatusBar.ApplicationDatabaseChecking();
            //await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(100);

            LoadPrinterDrivers();

            PDFToolsMethods.SetLicenseKey(AppSettings.System.PDFTools);

            await PutTaskDelay(500);

            //string message = await ServerMethods.CheckConnection(new ElectionFactory());

            //StatusBar.ApplicationStatusLeft(message);
            //StatusBar.LoadingSpinner(Visibility.Collapsed);            

            //int result;
            if (await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(0, ((App)Application.Current).Connection) == false)
            {
                GlobalReferences.StatusBar.TextLeft = ("Could not connect to database");

                //StatusBar.ApplicationDatabaseStatus(false);

                Console.WriteLine("Login Error Alert Called");
                AlertDialog messageBox = new AlertDialog("A DATABASE CONNECTION ERROR WAS ENCOUNTERED\r\nPLEASE CONTACT TECHNICAL SUPPORT FOR ASSISTANCE");
                messageBox.ShowDialog();
            }
            else
            {
                await LoadElectionDetails();

                //StatusBar.ApplicationDatabaseStatus(true);
                //StatusBar.ApplicationStatusClear();
                GlobalReferences.StatusBar.TextClear();
            }

            LoadingMessage.Visibility = Visibility.Collapsed;
            LoginFields.Visibility = Visibility.Visible;

            Keyboard.Focus(Password);
        }

        // https://stackoverflow.com/questions/22158278/wait-some-seconds-without-blocking-ui-execution
        private async Task PutTaskDelay(int delay)
        {
            await Task.Delay(delay);
        }

        // There is no where to go back to
        //private void BackButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.NavigateToPage(new SearchPage(null));
        //}

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //StatusBar.ApplicationStatusClear();
            if (pwCorrect == false)
            {
                Login();
            }
        }

        private void Login()
        {
            //StatusBar.ApplicationStatus("");
            //StatusBar.ApplicationStatusClear();
            GlobalReferences.StatusBar.TextClear();
            //StatusBar.ApplicationStatusRight("Copyright © AES 2018");
            string userName = UserList.GetSelectedItem();

            //StatusBar.ApplicationStatus("Login For: " + userName + "," + Password.Password);

            if(AppSettings.Absentee.TestScreen == null)
                AppSettings.Absentee.TestScreen = true;
            if(AppSettings.Absentee.ShowSliderIcons == null)
                AppSettings.Absentee.ShowSliderIcons = true;


            //AppSettings.User.UserID = AppSettings.System.MachineID;

            // Login Admin
            if (userName == "Administrator")
            {
                if (Password.Password.ToUpper() == AppSettings.User.AdminPassword.ToUpper())
                {
                    pwCorrect = true;
                    //StatusBar.ApplicationStatus("Administrator Logged In");

                    HideExitButton();
                    ShowHamburger();
                    //ShowSettingsMenu();

                    //MainMenuMethods.ToggleElectionVisibility();

                    //if(AppSettings.Absentee.ShowSliderIcons == true)
                    //    MainMenuMethods.SetMenuMinWidth(55);

                    //MainMenuMethods.ShowMenuMinimum();

                    // Save Login
                    //SaveLogin(userName);

                    // Before navigating make sure to load site & user settings
                    this.NavigateToPage(new SettingsPage());
                    //OpenAdminMenu();
                    //this.NavigateToPage(new AdministrationPage());
                }
                else
                {
                    AlertDialog message = new AlertDialog("Incorrect Username or Password");
                    if (message.ShowDialog() == true)
                    {
                        Password.Password = "";
                        Keyboard.Focus(Password);
                    }
                }
            }

            // Login Manager
            else if (userName == "Presiding Judge")
            {
                if (Password.Password.ToUpper() == AppSettings.User.ManagePassword.ToUpper())
                {
                    pwCorrect = true;

                    HideExitButton();
                    ShowHamburger();

                    HideMenuHome();

                    // Save Login
                    //SaveLogin(userName);

                    //StatusBar.ApplicationStatus("Manager Logged In");
                    //OpenAdminMenu();
                    //GlobalReferences.MenuSlider.Open();
                    this.NavigateToPage(new AdministrationPage());
                }
                else
                {
                    AlertDialog message = new AlertDialog("Incorrect Username or Password");
                    if (message.ShowDialog() == true)
                    {
                        Password.Password = "";
                        Keyboard.Focus(Password);
                    }
                }
            }

            // Login Absentee Board
            else if (userName == "Absentee Board")
            {
                if (Password.Password.ToUpper() == AppSettings.User.BoardPassword.ToUpper())
                {
                    pwCorrect = true;

                    HideExitButton();
                    ShowHamburger();

                    HideMenuHome();

                    // Save Login
                    //SaveLogin(userName);

                    //StatusBar.ApplicationStatus("Manager Logged In");
                    //OpenAdminMenu();
                    //GlobalReferences.MenuSlider.Open();
                    this.NavigateToPage(new AbsenteeBoardPage());
                }
                else
                {
                    AlertDialog message = new AlertDialog("Incorrect Username or Password");
                    if (message.ShowDialog() == true)
                    {
                        Password.Password = "";
                        Keyboard.Focus(Password);
                    }
                }
            }

            // Login basic user
            else if (userName == "VoterX User")
            {
                if (Password.Password.ToUpper() == AppSettings.User.GlobalPassword.ToUpper())
                {
                    //Password.IsEnabled = false;
                    //pwCorrect = true;
                    ////StatusBar.ApplicationStatus("VoterX User Logged In");
                    //HideExitButton();
                    //LoadGlobalUserData();

                    pwCorrect = true;

                    HideExitButton();
                    ShowHamburger();

                    // Save Login
                    //SaveLogin(userName);

                    //StatusBar.ApplicationStatus("Manager Logged In");
                    //OpenAdminMenu();
                    //GlobalReferences.MenuSlider.Open();
                    this.NavigateToPage(new AbsenteeAdministrationPage());

                }
                else
                {
                    Password.IsEnabled = false;
                    //StatusBar.ApplicationStatusLeft("Incorrect Username or Password");
                    AlertDialog message = new AlertDialog("Incorrect Username or Password");
                    if (message.ShowDialog() == true)
                    {
                        Password.IsEnabled = true;
                        Password.Password = "";
                        Keyboard.Focus(Password);
                    }
                }
            }

            // Login registered user from database
            else if (AppSettings.System.LoginType != 1)
            {
                //var user = PollWorkerMethods.UserLogin(userName, Password.Password);
                //if (user != null)
                //{
                //    if (AppSettings.System.SiteVerified == true)
                //    {
                //        pwCorrect = true;

                //        HideExitButton();
                //        ShowHamburger();

                //        // Before navigating make sure to load site & user settings
                //        LoadUserData(user);

                //        // Save Login
                //        SaveLogin();

                //        //StatusBar.ApplicationStatus("Manager Logged In");
                //        OpenAdminMenu();
                //        this.NavigateToPage(new AbsenteeAdministrationPage());
                //    }
                //    else
                //    {
                //        this.NavigateToPage(new SiteVerificationPage());
                //    }
                //}
                //else
                //{
                //    AlertDialog message = new AlertDialog("Incorrect Username or Password");
                //    if (message.ShowDialog() == true)
                //    {
                //        Password.Password = "";
                //        Keyboard.Focus(Password);
                //    }
                //}
            }

            //AppSettings.SetElectionMode();
        }

        // Load the current user into the global settings
        //private void LoadUserData(tblPollWorker user)
        //{
        //    AppSettings.User.UserID = user.voter_id;
        //    AppSettings.User.UserName = user.username;
        //    AppSettings.User.FullName = user.name_first + " " + user.name_last;
        //    AppSettings.User.Party = user.party;
        //    AppSettings.User.PositionName = user.postion_name;
        //    AppSettings.Network.LastLogin = DateTime.Now;
        //}

        // Load the global user into settings
        private void LoadGlobalUserData()
        {
            AppSettings.User.UserID = 0;
            AppSettings.User.UserName = "Global";
            AppSettings.User.FullName = "Global User";
            AppSettings.User.Party = "NON";
            AppSettings.User.PositionName = "General";
            //AppSettings.Network.LastLogin = DateTime.Now;
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = ComboBoxMethods.GetSelectedItem(sender);
            //StatusBar.ApplicationStatus(item.Content.ToString());

            //Password.Focus = true;
            Keyboard.Focus(Password);
        }

        // Convert "sender" into the selected combo box item
        //private ComboBoxItem GetSelectedItem(object sender)
        //{
        //    return (ComboBoxItem)((ComboBox)sender).SelectedItem;
        //}

        private void UserList_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppSettings.System.LoginType == 1) // Single User
            {
                AdminUser.Visibility = Visibility.Visible;
                //AdminUser.IsSelected = true;
                ManageUser.Visibility = Visibility.Visible;
                //ManageUser.IsSelected = true;
                BoardUser.Visibility = Visibility.Visible;
                GlobalUser.Visibility = Visibility.Visible;
                GlobalUser.IsSelected = true;
            }
            else if (AppSettings.System.LoginType == 2)// Multi User
            {
                // Create animated loading list item
                var loadingItem = ComboBoxMethods.AddLoadingItem(UserList, TempLoadingSpinnerItem);

                //var userList = await Task.Run(() => PollWorkerMethods.UserList((int)AppSettings.System.SiteID));

                //int count = 1;
                //ComboBoxItem selectedItem = new ComboBoxItem();
                //foreach (var user in userList)
                //{
                //    ComboBoxItem item = new ComboBoxItem();
                //    item.Content = user;
                //    ((ComboBox)sender).Items.Add(item);
                //    if (count == 1) selectedItem = item;
                //    count++;
                //}

                //selectedItem.IsSelected = true;
                AdminUser.Visibility = Visibility.Visible;
                //AdminUser.IsSelected = true;
                ManageUser.Visibility = Visibility.Visible;

                // Remove animated loading list item
                ComboBoxMethods.RemoveListItem(UserList, loadingItem);
            }


        }

        // Swap exit button and hamburger bars
        private void ShowExitButton()
        {
            //MainMenuMethods.ShowExitButton();
            GlobalReferences.Header.CloseButtonVisibility = true;
            //((MainWindow)System.Windows.Application.Current.MainWindow).CloseButton.Visibility = Visibility.Visible;
            //((MainWindow)System.Windows.Application.Current.MainWindow).fa_bars.Visibility = Visibility.Collapsed;
            //((MainWindow)System.Windows.Application.Current.MainWindow).HamburgerButton.Visibility = Visibility.Collapsed;
        }

        private void HideExitButton()
        {
            //MainMenuMethods.HideExitButton();
            GlobalReferences.Header.CloseButtonVisibility = false;
            //((MainWindow)System.Windows.Application.Current.MainWindow).CloseButton.Visibility = Visibility.Collapsed;
            //((MainWindow)System.Windows.Application.Current.MainWindow).fa_bars.Visibility = Visibility.Visible;
        }

        private void ShowHamburger()
        {
            //MainMenuMethods.ShowHamburger();
            GlobalReferences.Header.HamburgerMenuVisibility = true;
            //((MainWindow)System.Windows.Application.Current.MainWindow).HamburgerButton.Visibility = Visibility.Visible;
        }

        private void HideHamburger()
        {
            //MainMenuMethods.HideHamburger();
            GlobalReferences.Header.HamburgerMenuVisibility = false;
            //((MainWindow)System.Windows.Application.Current.MainWindow).HamburgerButton.Visibility = Visibility.Collapsed;
        }

        private void ShowSettingsMenu()
        {
            //MainMenuMethods.ShowSettingsMenu();
            //((MainWindow)System.Windows.Application.Current.MainWindow).ShowSettingsMenu();
        }

        private void OpenManageMenu()
        {
            //MainMenuMethods.OpenAdminMenu();
            //((MainWindow)System.Windows.Application.Current.MainWindow).OpenMenuSlider();
        }

        private void CloseManageMenu()
        {
            //MainMenuMethods.CloseAdminMenu();
            //((MainWindow)System.Windows.Application.Current.MainWindow).CloseMenuSlider();
        }

        private void OpenAdminMenu()
        {
            //MainMenuMethods.OpenAdminMenu();
            //((MainWindow)System.Windows.Application.Current.MainWindow).OpenMenuSlider();
        }

        private void CloseAdminMenu()
        {
            //MainMenuMethods.CloseAdminMenu();
            //((MainWindow)System.Windows.Application.Current.MainWindow).CloseMenuSlider();
        }

        private void HideMenu()
        {
            //MainMenuMethods.HideMainMenu();
        }

        private void HideMenuHome()
        {
            //MainMenuMethods.HideHomeMenu();
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (pwCorrect == false)
                {
                    Login();
                }
            }
        }

        private async void LoadPrinterDrivers()
        {
            var done = await ((App)Application.Current).GlobalSettings.LoadPrinterLists();
            if (done == true)
            {
                //StatusBar.StatusTextLeft = "Printers Loaded";
            }
        }

        private async Task<bool> LoadElectionDetails()
        {
            return await Task.Run(() =>
            {
                //((App)Application.Current).Election.Load(AppSettings.Election.ElectionID);
                using (ElectionFactory factory = new ElectionFactory())
                {
                    try
                    {
                        ABSLogger.WriteLog("Load Election from: [" + ((App)Application.Current).Connection + "]");

                        if (((App)Application.Current).Connection != null)
                        {
                            ((App)Application.Current).Election = factory.Create(AppSettings.Election.ElectionID, ((App)Application.Current).Connection);
                        }
                        else
                        {
                            ((App)Application.Current).Election = factory.Create(AppSettings.Election.ElectionID);
                        }
                        //((App)Application.Current).Election = factory.Create(AppSettings.Election.ElectionID);

                        if (((App)Application.Current).Election != null && ((App)Application.Current).Election.Error == null)
                        {
                            LogElectionData();

                            // Election object created with no errors
                            return true;
                        }
                        else
                        {
                            if (((App)Application.Current).Election.Error != null)
                            {
                                ABSLogger.WriteLog("Load Election Failed: " + ((App)Application.Current).Election.Error.Message);
                                if (((App)Application.Current).Election.Error.InnerException != null)
                                {
                                    ABSLogger.WriteLog(((App)Application.Current).Election.Error.Message);
                                }
                            }
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            });
        }

        private void SaveLogin(string user)
        {
            try
            {
                //GlobalReferences.StatusBar.TextLeft = "Logging In";
                //GlobalReferences.StatusBar.SpinnerVisibility = true;
                ////if (await Task.Run(() => PollSummaryMethods.SaveLogin()) == false) StatusBar.ApplicationStatusCenter("Login not saved");
                //if (await Task.Run(() => PollSummaryMethods.SaveLogin(user)) == false) GlobalReferences.StatusBar.TextCenter = "Login not saved";
                //GlobalReferences.StatusBar.TextLeft = "";
                //GlobalReferences.StatusBar.SpinnerVisibility = false;
            }
            catch
            {

            }
        }

        private void LogElectionData()
        {
            ABSLogger.WriteLog("LOADING ELECTION DATA");

            try
            {
                ABSLogger.WriteLog("ApplicationRejectedReasons:" + ((App)Application.Current).Election.Lists.ApplicationRejectedReasons.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("ApplicationRejectedReasons:null");
            }

            try
            {
                ABSLogger.WriteLog("BallotRejectedReasons:" + ((App)Application.Current).Election.Lists.BallotRejectedReasons.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("BallotRejectedReasons:null");
            }

            try
            {
                ABSLogger.WriteLog("BallotStyles:" + ((App)Application.Current).Election.Lists.BallotStyles.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("BallotStyles:null");
            }

            try
            {
                ABSLogger.WriteLog("Election:" + ((App)Application.Current).Election.Lists.Election.ToString());
            }
            catch
            {
                ABSLogger.WriteLog("Election:null");
            }

            try
            {
                ABSLogger.WriteLog("Jurisdictions:" + ((App)Application.Current).Election.Lists.Jurisdictions.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("Jurisdictions:null");
            }

            try
            {
                ABSLogger.WriteLog("JurisdictionTypes:" + ((App)Application.Current).Election.Lists.JurisdictionTypes.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("JurisdictionTypes:null");
            }

            try
            {
                ABSLogger.WriteLog("Locations:" + ((App)Application.Current).Election.Lists.Locations.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("Locations:null");
            }

            try
            {
                ABSLogger.WriteLog("LogCodes:" + ((App)Application.Current).Election.Lists.LogCodes.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("LogCodes:null");
            }

            try
            {
                ABSLogger.WriteLog("Partys:" + ((App)Application.Current).Election.Lists.Partys.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("Partys:null");
            }

            try
            {
                ABSLogger.WriteLog("PollWorkers:" + ((App)Application.Current).Election.Lists.PollWorkers.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("PollWorkers:null");
            }

            try
            {
                ABSLogger.WriteLog("Precincts:" + ((App)Application.Current).Election.Lists.Precincts.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("Precincts:null");
            }

            try
            {
                ABSLogger.WriteLog("ProvisionalReasons:" + ((App)Application.Current).Election.Lists.ProvisionalReasons.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("ProvisionalReasons:null");
            }

            try
            {
                ABSLogger.WriteLog("SpoiledReasons:" + ((App)Application.Current).Election.Lists.SpoiledReasons.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("SpoiledReasons:null");
            }

            try
            {
                ABSLogger.WriteLog("Tabulators:" + ((App)Application.Current).Election.Lists.Tabulators.Count().ToString());
            }
            catch
            {
                ABSLogger.WriteLog("Tabulators:null");
            }

        }
    }
}
