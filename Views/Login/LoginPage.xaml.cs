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
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private bool pwCorrect = false;

        public LoginPage()
        {
            InitializeComponent();

            ShowExitButton();

            GlobalReferences.Header.PageHeader = ("Log on");

            //StatusBar.ApplicationStatusClear();
            GlobalReferences.StatusBar.TextClear();

            CloseAdminMenu();

            AppSettings.SetElectionMode();
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
            string userName = UserList.GetSelectedItem();

            //StatusBar.ApplicationStatus("Login For: " + userName + "," + Password.Password);

            // Login Admin
            if (userName == "Administrator")
            {
                if (Password.Password.ToUpper() == AppSettings.User.AdminPassword.ToUpper())
                {
                    pwCorrect = true;
                    //StatusBar.ApplicationStatus("Administrator Logged In");

                    HideExitButton();
                    ShowHamburger();
                    ShowSettingsMenu();

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
            else if (userName == "Manager")
            {
                if (Password.Password.ToUpper() == AppSettings.User.ManagePassword.ToUpper())
                {
                    pwCorrect = true;

                    HideExitButton();
                    ShowHamburger();

                    //StatusBar.ApplicationStatus("Manager Logged In");
                    OpenAdminMenu();
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

            // Login basic user
            else if (userName == "VoterX User")
            {
                if (Password.Password.ToUpper() == AppSettings.User.GlobalPassword.ToUpper())
                {
                    Password.IsEnabled = false;
                    pwCorrect = true;
                    //StatusBar.ApplicationStatus("VoterX User Logged In");
                    HideExitButton();
                    LoadGlobalUserData();
                    if (AppSettings.System.SiteVerified == true)
                    {
                        this.NavigateToPage(new SearchPage());
                    }
                    else
                    {
                        this.NavigateToPage(new SiteVerificationPage());
                    }
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
                //    pwCorrect = true;

                //    //StatusBar.ApplicationStatus("User Logged In As: " + user.name_first + " " + user.name_last);
                //    HideExitButton();
                //    // Before navigating make sure to load site & user settings
                //    LoadUserData(user);
                //    this.NavigateToPage(new SearchPage(null));
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
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(UserList, TempLoadingSpinnerItem);

            if (AppSettings.System.LoginType == 1) // Single User
            {
                AdminUser.Visibility = Visibility.Visible;
                //AdminUser.IsSelected = true;
                ManageUser.Visibility = Visibility.Visible;
                GlobalUser.Visibility = Visibility.Visible;
                GlobalUser.IsSelected = true;
            }
            else if (AppSettings.System.LoginType == 2)// Multi User
            {
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
            }

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(UserList, loadingItem);
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
            GlobalReferences.Header.HamburgerMenuVisibility = true;
            //((MainWindow)System.Windows.Application.Current.MainWindow).HamburgerButton.Visibility = Visibility.Collapsed;
        }
        
        private void ShowSettingsMenu()
        {
            //MainMenuMethods.ShowSettingsMenu();
            //((MainWindow)System.Windows.Application.Current.MainWindow).ShowSettingsMenu();
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
    }
}
