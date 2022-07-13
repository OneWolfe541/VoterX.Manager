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
using VoterX.Methods;
using VoterX.Utilities.Models;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for VoterDetailsMenu.xaml
    /// </summary>
    public partial class AllMailDetailsMenu : Page
    {
        AllMailDetailsPage voterdetails = null;

        public AllMailDetailsMenu(AllMailDetailsPage page)
        {
            InitializeComponent();

            voterdetails = page;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new SearchPage());

            //MainMenuMethods.LoadMenu(new AbsenteeMenu());
            GlobalReferences.MenuSlider.SetMenu(new AbsenteeMenu(), MenuCollapseMode.ShowIcons);
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeAdministrationPage());

            //MainMenuMethods.OpenMainMenu();
            GlobalReferences.MenuSlider.Open();
        }

        private async void PrintAddressLabelButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            await voterdetails.PrintMailingLabelAsync();
        }

        private void PrintMailingBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            voterdetails.PrintMailingBundle();
        }

        private void PrintIdRequiredBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            voterdetails.PrintIdRequiredBundle();
        }

        private void ReissueBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            voterdetails.PrintReissuedBundle();
        }
    }
}
