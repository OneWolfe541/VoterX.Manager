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
using VoterX.Core.Voters;
using VoterX.Dialogs;
using VoterX.Manager.Views.Absentee;
using VoterX.Manager.Global;
using VoterX.Utilities.Models;
//using VoterX.Core.Models.Database;
//using VoterX.Core.Models.Utilities;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeRejectedApplicationsPage.xaml
    /// </summary>
    public partial class AbsenteeRejectedApplicationsPage : Page
    {
        private NMVoter _Voter;

        public AbsenteeRejectedApplicationsPage(NMVoter voter)
        {
            InitializeComponent();

            _Voter = voter;

            LoadVoterFields(voter.Data);

            LoadRejectedReasons();

            //MainMenuMethods.LoadMenu(new AbsenteeMenu());
            GlobalReferences.MenuSlider.SetMenu(new AbsenteeMenu(), MenuCollapseMode.Full);

            GlobalReferences.Header.PageHeader = ("Rejected Application");

            GlobalReferences.StatusBar.TextClear();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            VoterNavModel previousVoter = new VoterNavModel
            {
                Voter = _Voter
            };

            //if (AppSettings.Absentee.TestScreen == true)
            //{
            // Check for SERVIS lookup
            bool SERVISlookup = false;
            if (_Voter.Data.PollID == 0) SERVISlookup = true;

            this.NavigateToPage(new VoterDetailsPage(_Voter, SERVISlookup));
            //}
            //else
            //{
            //    this.NavigateToPage(new AbsenteeDetailsPage(previousVoter));
            //}

        }

        private void LoadVoterFields(VoterDataModel voter)
        {
            FullName.Text = voter.FullName;
            BirthYear.Text = voter.DOBYear;
            Address.Text = voter.Address1;
            CityStateAndZip.Text = voter.City + ", " + voter.State + " " + voter.Zip;
        }

        private async void LoadRejectedReasons()
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(RejectedReasonList, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                foreach (var rejectedReasons in await Task.Run(() => ElectionDataMethods.ApplicationRejectedReasons ))
                {
                    ComboBoxMethods.AddComboItemToControl(
                        RejectedReasonList,
                        rejectedReasons.ServiseCode,
                        rejectedReasons.RejectedReasonDescription,
                        ""
                        );
                }
            }

            ComboBoxMethods.RemoveListItem(RejectedReasonList, loadingItem);

            RejectedReasonList.SelectedIndex = -1;
        }

        private void SurrenderYes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SurrenderNo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RejectedReason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RejectedReasonList.SelectedIndex >= 0)
            {
                //var rejecteditem = ComboBoxMethods.GetSelectedItemData<string>(RejectedReasonList);
                //StatusBar.ApplicationStatus(rejecteditem);
                RejectApplicationButton.Visibility = Visibility.Visible;
            }
        }

        private void RejectApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (RejectedReasonList.SelectedIndex >= 0)
            {
                string rejecteditem = ComboBoxMethods.GetSelectedItemData<string>(RejectedReasonList);

                DateTime rejectedDate = DateTime.Now;

                _Voter.Data.ApplicationRejectedReason = rejecteditem;

                // Update local record in RAM
                _Voter.Data.ApplicationRejected = rejectedDate;
                _Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                _Voter.Data.UserId = AppSettings.User.UserID;
                _Voter.Data.PollID = AppSettings.Absentee.SiteID;
                _Voter.Data.PollName = AppSettings.System.SiteName;
                _Voter.Data.ComputerID = AppSettings.System.MachineID;
                _Voter.Data.LogCode = 4;
                _Voter.Data.ActivityDate = rejectedDate;

                //var result = await VoterMethods.UpdateApplicationRejected(_Voter, rejectedDate);
                _Voter.RejectApplication(rejecteditem);

                // Return to details
                VoterNavModel previousVoter = new VoterNavModel
                {
                    Voter = _Voter
                };

                //if (AppSettings.Absentee.TestScreen == true)
                //{
                // Check for SERVIS lookup
                bool SERVISlookup = false;
                if (_Voter.Data.PollID == 0) SERVISlookup = true;

                this.NavigateToPage(new VoterDetailsPage(previousVoter, SERVISlookup));
                //}
                //else
                //{
                //    this.NavigateToPage(new AbsenteeDetailsPage(previousVoter));
                //}
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("SELECT A VALID REASON");
                newMessage.ShowDialog();
            }
        }
    }
}
