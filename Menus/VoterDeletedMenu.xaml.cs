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
    public partial class VoterDeletedMenu : Page
    {
        AbsenteeDetailsPage voterdetails = null;

        public VoterDeletedMenu(AbsenteeDetailsPage page)
        {
            InitializeComponent();

            voterdetails = page;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new SearchPage());
            //MainFrameMethods.NavigateToPage(new SearchPage(voterdetails.GetSearchParameters())); 

            //MainMenuMethods.LoadMenu(new AbsenteeMenu());
            GlobalReferences.MenuSlider.SetMenu(new AbsenteeMenu(), MenuCollapseMode.Full);
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            voterdetails.ClearVoterId();
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeAdministrationPage());

            //MainMenuMethods.OpenMainMenu();
            GlobalReferences.MenuSlider.Open();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            voterdetails.SaveChanges();
        }

        private void PrintApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            //voterdetails.PrintApplication();
        }

        private void PrintAddressLabelButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            //voterdetails.PrintMailingLabel();
        }

        private void PrintMailingBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            //voterdetails.PrintMailingBundle();
        }

        private void PrinInPersonBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            //voterdetails.PrintInPersonBallot();
        }

        private void PrinProvisionalBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            //voterdetails.PrintProvisionalBallot();
        }

        private void EarlyVotingBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            //voterdetails.PrintEarlyVotingBallot();
        }

        private void SpoilBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            //voterdetails.SpoilBallot();
        }

        private void BatchVoterButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            //StatusBar.ApplicationStatus("Voter Added to Batch");
            //voterdetails.BatchThisVoter();
            //voterdetails.AcceptApplication();
        }

        private void RejectedApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            // Goto Rejected Application Page
            //voterdetails.RejectedApplication();
        }

        private void UnlockCurrentRecordButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            //voterdetails.UnlockCurrentRecord();
        }

        private void AcceptApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();

            //voterdetails.AcceptApplication();
        }        
    }
}
