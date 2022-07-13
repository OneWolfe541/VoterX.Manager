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
using VoterX.Dialogs;
using VoterX.Methods;
using VoterX.Core.Elections;
using VoterX.Core.Voters;
using VoterX.Utilities.Models;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Views.Absentee
{ 
    /// <summary>
    /// Interaction logic for VoterDetailsPage.xaml
    /// </summary>
    public partial class VoterDetailsPage : Page
    {
        //private VoterSearchModel search = new VoterSearchModel();
        private NMVoter _voter = new NMVoter();

        private bool SERVISSource = false;

        public VoterDetailsPage(VoterNavModel voterFromNav) : this(voterFromNav, false) { }
        public VoterDetailsPage(VoterNavModel voterFromNav, bool source) : this(voterFromNav.Voter, source) { }
        public VoterDetailsPage(NMVoter voter) : this(voter, false) { }
        public VoterDetailsPage(NMVoter voter, bool source)
        {
            InitializeComponent();

            _voter = voter;

            SERVISSource = source;

            //CheckServer();

            // Display page header
            GlobalReferences.Header.PageHeader = ("Voter Details");

            // Set Origin
            NavigationMenuMethods.SetOrigin(this);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // The menu view model needs to have a reference to the page's view model so it can called commands from it

            GlobalReferences.Header.PageHeader = ("Voter Details");

            if (AppSettings.Absentee.AllMailMode == true)
            {
                // Get view model for the page
                var viewModel = new AllMailVoterDetailsViewModel(_voter);

                // Load the view model
                this.DataContext = viewModel;

                // Get the view model for the menu
                var menu = new Menus.AllMailVoterDetailsMenuViewModel(GlobalReferences.MenuSlider, AppSettings.Global, viewModel);

                // Load the menu
                GlobalReferences.MenuSlider.SetMenu(new Menus.VoterDetailMenuView(menu), MenuCollapseMode.ShowIcons);
            }
            else
            {
                // Get view model for the page
                var viewModel = new VoterDetailsViewModel(_voter, SERVISSource);

                // Load the view model
                this.DataContext = viewModel;

                // Get the view model for the menu
                var menu = new Menus.VoterDetailsMenuViewModel(GlobalReferences.MenuSlider, AppSettings.Global, viewModel);

                // Load the menu
                GlobalReferences.MenuSlider.SetMenu(new Menus.VoterDetailMenuView(menu), MenuCollapseMode.ShowIcons);
            }            
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // Return menu to default settings
            //MainMenuMethods.SetMenuMinWidth(0);

            //MainMenuMethods.ShowMenuMinimum();
        }

        // Run the check server connection process
        private async void CheckServer()
        {
            if(await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(100) == false)
            {
                Console.WriteLine("Details Error Alert Called");
                AlertDialog messageBox = new AlertDialog("AN ERROR WAS ENCOUNTERED PLEASE CONTACT YOUR ADMINISTRATOR");
                messageBox.ShowDialog();
            }
        }

        private void Contacts_LostFocus(object sender, RoutedEventArgs e)
        {

            ((VoterDetailsViewModel)this.DataContext).UpdateContactInfoCommand.Execute(null);
        }
    }
}
