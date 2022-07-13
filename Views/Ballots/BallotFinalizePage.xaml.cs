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
//using VoterX.Core.Models.ViewModels;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for BallotVerifyPage.xaml
    /// </summary>
    public partial class BallotFinalizePage : Page
    {
        //private VoterDataModel _voter = new VoterDataModel();

        //public BallotFinalizePage(VoterDataModel voter)
        //{
        //    InitializeComponent();

        //    _voter = voter;

        //    LoadVoterFields(voter);

        //    StatusBar.ApplicationPageHeader("Finish");
        //}

        //private void LoadVoterFields(VoterDataModel voter)
        //{
        //    FullName.Text = voter.FullName;
        //    BirthYear.Text = voter.DOBYear;
        //    Address.Text = voter.Address1;
        //    CityStateAndZip.Text = voter.City + ", " + voter.State + " " + voter.Zip;
        //}

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigateToPage(new SearchPage(null));
        }

        private void MarkVoter_Click(object sender, RoutedEventArgs e)
        {
            // Write log data to voter record

            //StatusBar.ApplicationStatus(VoterMethods.InsertVotedAtPollsError(_voter));

            //StatusBar.ApplicationStatus(VoterMethods.InsertVotedAtPolls2(_voter));
            // Save voter data to tblVotedRecord
            //if (VoterMethods.InsertVotedAtPolls(_voter) == true)
            //{
            //    StatusBar.ApplicationStatus("Voter Saved Successfull");
            //    this.NavigateToPage(new SearchPage(null));
            //}
            //else
            //{
            //    StatusBar.ApplicationStatus("Voter Not Saved");
            //}
        }
    }
}
