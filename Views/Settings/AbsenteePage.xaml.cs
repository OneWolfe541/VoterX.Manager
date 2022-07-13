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
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for SystemPage.xaml
    /// </summary>
    public partial class AbsenteePage : SettingsBasePage
    {
        public bool pageLoaded = false;

        public AbsenteePage()
        {
            InitializeComponent();

            var test = AppSettings.Election;

            DisplaySettings();

            LoadPollSites();

            pageLoaded = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //GlobalReferences.StatusBar.TextCenter = AppSettings.System.BODName + ":" + AppSettings.System.BODVersion;
        }

        private async void LoadPollSites()
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(PollList, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var siteList = await Task.Run(() => ElectionDataMethods.Locations.OrderBy(o => o.PlaceName));
                if (siteList != null)
                {
                    foreach (var pollingLocation in siteList)
                    {
                        ComboBoxMethods.AddComboItemToControl(
                            PollList,
                            pollingLocation.PollId,
                            pollingLocation.PlaceName,
                            AppSettings.Absentee.SiteID
                            );
                    }
                }
            }

            ComboBoxMethods.RemoveListItem(PollList, loadingItem);
        }

        // Need to bind this data instead of manualy stuffing it here
        private void DisplaySettings()
        {
            // SYSTEM SETTINGS //
            
            PollID.Text = AppSettings.Absentee.SiteID.ToString();
            //PollName.Text = AppSettings.System.SiteName.ToString();
            Machine.Text = AppSettings.System.MachineID.ToString();

            //ReturnAddressOne.Text = AppSettings.Absentee.ReturnAddress1;
            //ReturnAddressTwo.Text = AppSettings.Absentee.ReturnAddress2;
            //ReturnCity.Text = AppSettings.Absentee.ReturnCity;
            //ReturnState.Text = AppSettings.Absentee.ReturnState;
            //ReturnZip.Text = AppSettings.Absentee.ReturnZip;

            ElectionTitleEnglish.Text = AppSettings.Absentee.ElectionTitle;
            ElectionDateEnglish.Text = AppSettings.Absentee.ElectionDateLong;
            ElectionTitleSpanish.Text = AppSettings.Absentee.ElectionTitleSpanish;
            ElectionDateSpanish.Text = AppSettings.Absentee.ElectionDateLongSpanish;

            SetBatchMode();

            //if (AppSettings.System.SiteVerified) Verified.Foreground = new SolidColorBrush(Colors.Green);
            //else Verified.Foreground = new SolidColorBrush(Colors.Red);
            //Verified.Text = AppSettings.System.SiteVerified.ToString();
            VerifiedCheck.IsChecked = AppSettings.System.SiteVerified;

            if(AppSettings.Absentee.CanVoteInPersonOnThisMachine == true)
            {
                EarlyVoting.IsSelected = true;
            }
            else
            {
                AbsenteeOnly.IsSelected = true;
            }

            LocationCheck.IsChecked = AppSettings.Absentee.BoardLocationRequired;
        }

        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/df93d685-844f-47a1-bbbc-454e196369a8/raise-an-event-in-a-child-page-hosted-in-a-frame-to-refresh-a-listbox-on-the-page-hosting-the?forum=wpf
        public override void SaveSettings()
        {
            //StatusBar.ApplicationStatus("System Settings Page Called");

            // Boolean values will probably not be set on this page
            AppSettings.System.SiteVerified = (bool)VerifiedCheck.IsChecked;

            // SYSTEM SETTINGS //
            //AppSettings.System.BODName = ApplicationName.Text;
            //AppSettings.System.BODVersion = Version.Text;
            var pollitem = ComboBoxMethods.GetSelectedItemNumber(PollList);
            if (pollitem > 0)
            {
                AppSettings.Absentee.SiteID = ComboBoxMethods.GetSelectedItemNumber(PollList);
                AppSettings.Absentee.SiteName = ComboBoxMethods.GetSelectedItem(PollList);
            }
            AppSettings.System.MachineID = Int32.Parse(Machine.Text);
            //AppSettings.System.VCCType = ComboBoxMethods.GetSelectedItemNumber(VCCTypeList);
            //AppSettings.System.SearchOptions = ComboBoxMethods.GetSelectedItemNumber(SearchOptions);
            //AppSettings.System.LoginType = ComboBoxMethods.GetSelectedItemNumber(LoginTypeList);
            //AppSettings.System.Permit = ComboBoxMethods.GetSelectedItemNumber(PrintPermit);
            //AppSettings.System.BallotStub = ComboBoxMethods.GetSelectedItemNumber(PrintBallotStub);
            //AppSettings.System.SignatureFolder = SignaturesFolder.Text;
            //AppSettings.Absentee.ReturnAddress1 = ReturnAddressOne.Text;
            //AppSettings.Absentee.ReturnAddress2 = ReturnAddressTwo.Text;
            //AppSettings.Absentee.ReturnCity = ReturnCity.Text;
            //AppSettings.Absentee.ReturnState = ReturnState.Text;
            //AppSettings.Absentee.ReturnZip = ReturnZip.Text;

            AppSettings.Absentee.ElectionTitle = ElectionTitleEnglish.Text;
            AppSettings.Absentee.ElectionDateLong = ElectionDateEnglish.Text;
            AppSettings.Absentee.ElectionTitleSpanish = ElectionTitleSpanish.Text;
            AppSettings.Absentee.ElectionDateLongSpanish = ElectionDateSpanish.Text;

            AppSettings.Absentee.BatchPrintingMode = ComboBoxMethods.GetSelectedItem(BatchModeList);

            if (EarlyVoting.IsSelected == true)
            {
                AppSettings.Absentee.CanVoteInPersonOnThisMachine = true;
            }
            else
            {
                AppSettings.Absentee.CanVoteInPersonOnThisMachine = false;
            }

            if (LocationCheck.IsChecked == true)
            {
                AppSettings.Absentee.BoardLocationRequired = true;
            }
            else
            {
                AppSettings.Absentee.BoardLocationRequired = false;
            }
        }

        private void PollList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).IsLoaded)
            {                
                PollID.Text = (ComboBoxMethods.GetSelectedItemData((ComboBox)sender).ToString());
            }
        }

        private void SetBatchMode()
        {
            //VCCType.Text = AppSettings.System.VCCType.ToString();
            switch (AppSettings.Absentee.BatchPrintingMode)
            {
                case "ALL":
                    BatchAll.IsSelected = true;
                    break;
                case "SINGLE":
                    BatchSingle.IsSelected = true;
                    break;
            }
        }

        private void SettingsChange()
        {
            AppSettings.SettingsChanged = true;
        }

        private void SettingsChanged_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).IsLoaded)
            {
                SettingsChange();
            }
        }

        private void SettingsChanged_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((ComboBox)sender).IsLoaded)
            {
                SettingsChange();
            }
        }

        private void SettingsChanged_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pageLoaded == true)
            {
                SettingsChange();
            }
        }

        private void VerifiedCheck_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsLoaded)
            {
                SettingsChange();
            }
        }

        private void LocationCheck_Click(object sender, RoutedEventArgs e)
        {
            SettingsChange();
        }
    }
}