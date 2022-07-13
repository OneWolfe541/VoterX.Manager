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
//using VoterX.Core.Models.ViewModels;
using VoterX.Extensions;
//using VoterX.Core.Models.Database;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for SpoilBallotPage.xaml
    /// </summary>
    public partial class SpoilBallotPage : Page
    {
        //VoterDatabase _VoterDB = ((App)Application.Current).voterContainer.Resolve<VoterDatabase>();

        //public IEnumerable<avSpoiledReason> spoiledList;

        //VoterDataModel _Voter;

        //public SpoilBallotPage(VoterDataModel voter)
        //{
        //    InitializeComponent();

        //    //LoggingMethods.LogPage("VALIDATION PAGE LOADED");

        //    _Voter = voter;

        //    LoadVoterFields(voter);

        //    LoadSpoiledReasons();

        //    StatusBar.ApplicationPageHeader("Spoil Ballot");
        //}

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogPage("VALIDATION PAGE UNLOADED");
        }

        //private void LoadVoterFields(VoterDataModel voter)
        //{
        //    FullName.Text = voter.FullName;
        //    BirthYear.Text = voter.DOBYear;
        //    Address.Text = voter.Address1;
        //    CityStateAndZip.Text = voter.City + ", " + voter.State + " " + voter.Zip;
        //}

        //private void LoadSpoiledReasons()
        //{
        //    if (VoterMethods.SpoiledReasons.Exists(0) == true)
        //    {
        //        var spoiledList = VoterMethods.SpoiledReasons.List();

        //        SpoledReason.ItemsSource = spoiledList;
        //    }
        //}

        //private avSpoiledReason GetSelectedItem(ComboBox sender)
        //{
        //    //if (sender.SelectedItem == null) return "";
        //    //else
        //    return ((avSpoiledReason)sender.SelectedItem);
        //}

        // Return to search screen without search parameters
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogUser("RETURNING TO SEARCH PAGE");

            //var frame = NavigationMethods.FindParent<Frame>(this);
            //var prevPage = new SearchPage(null);
            //frame.Navigate(prevPage);

            this.NavigateToPage(new SearchPage());
        }

        private void SpoilButton_Click(object sender, RoutedEventArgs e)
        {
            //VoterMethods.InsertSpoiledBallot(_Voter, GetSelectedItem(SpoledReason).spoiled_reason_id);
            //this.NavigateToPage(new ReprintBallotPage(_Voter));
        }

        private void SpoledReason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var item = GetSelectedItem((ComboBox)sender);

            //if (item != null)
            //{
            //    StatusBar.ApplicationStatus("Reason Selected: " + item.spoiled_reason + " ID: " + item.spoiled_reason_id);
            //    SpoilBallot.Visibility = Visibility.Visible;
            //}

            ////LoggingMethods.LogUser("SPOILED REASON SELECTED: " + item.spoiled_reason.ToString());
        }

        private void OptionOut_Click(object sender, RoutedEventArgs e)
        {

        }        

        //private void Validation_Click(object sender, RoutedEventArgs e)
        //{
        //    if((CheckBox)sender == SurrenderYes)
        //    {
        //        SurrenderNo.IsChecked = false;
        //    }
        //    if ((CheckBox)sender == SurrenderNo)
        //    {
        //        SurrenderYes.IsChecked = false;
        //    }
        //}
    }
}
