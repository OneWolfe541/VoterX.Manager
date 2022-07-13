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
using VoterX.Utilities.Interfaces;
using VoterX.Utilities.Models;
using VoterX.Manager.Global;
using VoterX.Manager.Menus;
using VoterX.Manager.Methods;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AdministrationPage.xaml
    /// </summary>
    public partial class AbsenteeAdministrationPage : Page
    {
        public AbsenteeAdministrationPage()
        {
            InitializeComponent();

            // Set page header
            GlobalReferences.Header.PageHeader = "VoterX";

            // Clear status bar text
            GlobalReferences.StatusBar.TextClear();

            // Show hamburger button
            GlobalReferences.Header.HamburgerMenuVisibility = true;

            // Load the menu
            LoadAbsenteeMenu();

            // Set Origin
            NavigationMenuMethods.SetOrigin(this);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the menu slider
            GlobalReferences.MenuSlider.Close();

            // Hide the hamburger button
            GlobalReferences.Header.HamburgerMenuVisibility = false;

            // Save the log out date and time
            //SaveLogout();

            // Navigate to the login page
            this.NavigateToPage(new AbsenteeLoginPage());
        }

        private async void SaveLogout()
        {
            //GlobalReferences.StatusBar.TextLeft = "Logging In";
            //GlobalReferences.StatusBar.SpinnerVisibility = true;

            //// Save the date and time to the database
            //if (await Task.Run(() => PollSummaryMethods.SaveLogout()) == false) GlobalReferences.StatusBar.TextCenter = "Logout not saved";

            //GlobalReferences.StatusBar.TextLeft  = "";
            //GlobalReferences.StatusBar.SpinnerVisibility = false;
        }

        private void LoadAbsenteeMenu()
        {
            // Create View Model container
            IMenuViewModel menuView = null;

            // Load either the all mail or normal view model
            if (AppSettings.Absentee.AllMailMode == true)
            {
                menuView = new AllMailAbsenteeMenuViewModel(GlobalReferences.MenuSlider, AppSettings.Global);
            }
            else
            {
                menuView = new AbsenteeMenuViewModel(GlobalReferences.MenuSlider, AppSettings.Global);
            }

            // Create and load the menu from the view model
            GlobalReferences.MenuSlider.SetMenu(new ManageMenuView(menuView), MenuCollapseMode.Full);

            // Open the menu
            GlobalReferences.MenuSlider.Open();
        }
    }
}
