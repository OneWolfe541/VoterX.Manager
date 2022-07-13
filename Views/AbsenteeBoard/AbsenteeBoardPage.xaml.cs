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
    public partial class AbsenteeBoardPage : Page
    {
        public AbsenteeBoardPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the page header
            GlobalReferences.Header.PageHeader = "Absentee Board";

            // Clear the status bar
            GlobalReferences.StatusBar.TextClear();

            // Load the menu
            LoadManagerMenu();

            // Set the navigation origin page
            NavigationMenuMethods.SetOrigin(this);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide the hamburger button
            GlobalReferences.Header.HamburgerMenuVisibility = false;

            // Close the slider menu
            GlobalReferences.MenuSlider.Close();

            // Save logout date
            SaveLogout();

            // Navigate to login page
            this.NavigateToPage(new AbsenteeLoginPage());
        }

        private async void SaveLogout()
        {
            GlobalReferences.StatusBar.TextLeft = ("Logging In");
            GlobalReferences.StatusBar.SpinnerVisibility = true;

            // Save the date and time to the database
            if (await Task.Run(() => PollSummaryMethods.SaveLogout()) == false) GlobalReferences.StatusBar.TextCenter = "Logout not saved";

            GlobalReferences.StatusBar.TextLeft = ("");
            GlobalReferences.StatusBar.SpinnerVisibility = false;
        }

        private void LoadManagerMenu()
        {
            // Create View Model container
            IMenuViewModel menuView = null;

            // Load either the all mail or normal view model
            menuView = new AbsenteeBoardMenuViewModel(GlobalReferences.MenuSlider, AppSettings.Global);

            // Create and load the menu from the view model
            GlobalReferences.MenuSlider.SetMenu(new AbsenteeBoardMenuView(menuView), MenuCollapseMode.Full);

            // Open the menu
            GlobalReferences.MenuSlider.Open();
        }
    }
}
