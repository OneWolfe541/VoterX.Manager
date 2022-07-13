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
using VoterX.Manager.Views.Absentee;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for VoterDetailsMenu.xaml
    /// </summary>
    public partial class VoterDetailsMenuMVVM : Page
    {
        VoterDetailsViewModel voterdetails = null;

        public VoterDetailsMenuMVVM(VoterDetailsViewModel page)
        {
            InitializeComponent();

            voterdetails = page;

            if(AppSettings.Absentee.CanVoteInPersonOnThisMachine == true)
            {
                EarlyVotingBallotButton.Visibility = Visibility.Visible;
            }
            else
            {
                EarlyVotingBallotButton.Visibility = Visibility.Collapsed;
            }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            //GlobalReferences.MenuSlider.Close();

            if (voterdetails.GoBackCommand.CanExecute(null))
            {
                voterdetails.GoBackCommand.Execute(null);

                //MainMenuMethods.LoadMenu(new AbsenteeMenu());
                GlobalReferences.MenuSlider.SetMenu(new AbsenteeMenu(), MenuCollapseMode.Full);
            }            
        }

        private void PrintApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintApplicationCommand.CanExecute(null))
                voterdetails.PrintApplicationCommand.Execute(null);
        }

        private void PrintAddressLabelButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintMailingLabelCommand.CanExecute(null))
                voterdetails.PrintMailingLabelCommand.Execute(null);
        }

        private void PrintMailingBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintBallotBundleCommand.CanExecute(null))
                voterdetails.PrintBallotBundleCommand.Execute(null);
        }

        private void PrinProvisionalBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintProvisionalBallotCommand.CanExecute(null))
                voterdetails.PrintProvisionalBallotCommand.Execute(null);
        }

        private void EarlyVotingBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintEarlyVotingBallotCommand.CanExecute(null))
                voterdetails.PrintEarlyVotingBallotCommand.Execute(null);
        }

        private void SpoilBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.SpoilBallotCommand.CanExecute(null))
                voterdetails.SpoilBallotCommand.Execute(null);
        }

        private void BatchVoterButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.BatchVoterCommand.CanExecute(null))
                voterdetails.BatchVoterCommand.Execute(null);
        }

        private void RejectedApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.RejectApplicationCommand.CanExecute(null))
                voterdetails.RejectApplicationCommand.Execute(null);
        }
    }
}
