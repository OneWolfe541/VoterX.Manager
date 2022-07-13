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
using VoterX.SystemSettings.Enums;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for SystemPage.xaml
    /// </summary>
    public partial class SystemPage : SettingsBasePage
    {
        public bool pageLoaded = false;

        public SystemPage()
        {
            InitializeComponent();

            DisplaySettings();

            LoadPollSites();

            pageLoaded = true;
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
            ApplicationName.Text = AppSettings.System.BODName.ToString();
            Version.Text = AppSettings.System.BODVersion.ToString();
            PollID.Text = AppSettings.System.SiteID.ToString();
            //PollName.Text = AppSettings.System.SiteName.ToString();
            //Machine.Text = AppSettings.System.MachineID.ToString();

            //if (AppSettings.System.SiteVerified) Verified.Foreground = new SolidColorBrush(Colors.Green);
            //else Verified.Foreground = new SolidColorBrush(Colors.Red);
            //Verified.Text = AppSettings.System.SiteVerified.ToString();

            SignaturesFolder.Text = AppSettings.System.SignatureFolder;

            SetVCCType();

            SetLoginType();

            SetPermits();

            SetBallotStubs();

            SetSearchOptions();
        }

        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/df93d685-844f-47a1-bbbc-454e196369a8/raise-an-event-in-a-child-page-hosted-in-a-frame-to-refresh-a-listbox-on-the-page-hosting-the?forum=wpf
        public override void SaveSettings()
        {
            //StatusBar.ApplicationStatus("System Settings Page Called");

            // Boolean values will probably not be set on this page

            // SYSTEM SETTINGS //
            AppSettings.System.BODName = ApplicationName.Text;
            AppSettings.System.BODVersion = Version.Text;
            var pollitem = ComboBoxMethods.GetSelectedItemNumber(PollList);
            if (pollitem > 0)
            {
                AppSettings.System.SiteID = ComboBoxMethods.GetSelectedItemNumber(PollList);
                AppSettings.System.SiteName = ComboBoxMethods.GetSelectedItem(PollList);
            }
            //AppSettings.System.MachineID = Int32.Parse(Machine.Text);
            AppSettings.System.VCCType = (VotingCenterMode)ComboBoxMethods.GetSelectedItemNumber(VCCTypeList);
            AppSettings.System.SearchOptions = (SearchMode)ComboBoxMethods.GetSelectedItemNumber(SearchOptions);
            AppSettings.System.LoginType = ComboBoxMethods.GetSelectedItemNumber(LoginTypeList);
            AppSettings.System.Permit = ComboBoxMethods.GetSelectedItemNumber(PrintPermit);
            AppSettings.System.BallotStub = ComboBoxMethods.GetSelectedItemNumber(PrintBallotStub);
            AppSettings.System.SignatureFolder = SignaturesFolder.Text;
        }

        private void SetVCCType()
        {
            //VCCType.Text = AppSettings.System.VCCType.ToString();
            switch (AppSettings.System.VCCType)
            {
                case VotingCenterMode.EarlyVoting:
                    EarlyVoting.IsSelected = true;
                    break;
                //case 2:
                //    ElectionDay.IsSelected = true;
                //    break;
            }
        }

        private void SetLoginType()
        {
            //VCCType.Text = AppSettings.System.VCCType.ToString();
            switch (AppSettings.System.LoginType)
            {
                case 1:
                    SingleUser.IsSelected = true;
                    break;
                case 2:
                    MultiUser.IsSelected = true;
                    break;
            }
        }

        private void SetPermits()
        {
            //VCCType.Text = AppSettings.System.VCCType.ToString();
            switch (AppSettings.System.Permit)
            {
                case 0:
                    DontPrint.IsSelected = true;
                    break;
                case 1:
                    DoPrint.IsSelected = true;
                    break;
            }
        }

        private void SetBallotStubs()
        {
            //VCCType.Text = AppSettings.System.VCCType.ToString();
            switch (AppSettings.System.BallotStub)
            {
                case 0:
                    DontPrintStub.IsSelected = true;
                    break;
                case 1:
                    DoPrintStub.IsSelected = true;
                    break;
            }
        }

        private void SetSearchOptions()
        {
            //SearchOptions.Text = AppSettings.System.SearchOptions.ToString();
            switch (AppSettings.System.SearchOptions)
            {
                case SearchMode.Normal:
                    Normal.IsSelected = true;
                    break;
                case SearchMode.Scan:
                    Scan.IsSelected = true;
                    break;
                case SearchMode.Queue:
                    Queue.IsSelected = true;
                    break;
            }
        }

        private void PollList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ignore the error when the page hasnt finished loading
            //try
            //{
            //    PollID.Text = (ComboBoxMethods.GetSelectedItemData((ComboBox)sender).ToString());
            //}
            //catch
            //{ }

            // Skip if the combobox isnt loaded
            //var comboBox = (ComboBox)sender;
            if (((ComboBox)sender).IsLoaded)
            {                
                PollID.Text = (ComboBoxMethods.GetSelectedItemData((ComboBox)sender).ToString());

                //SettingsChange();
            }
        }

        private void FolderBrowser_Click(object sender, RoutedEventArgs e)
        {
            SignaturesFolder.Text = OpenFolderBrowser(AppSettings.System.SignatureFolder);
            SettingsChange();
        }

        // Folder Browser Sample
        // https://stackoverflow.com/questions/1922204/open-directory-dialog
        private string OpenFolderBrowser(string currentPath)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = currentPath;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                return dialog.SelectedPath;
            }
        }

        //private ComboBoxItem AddLoadingItem(object sender)
        //{
        //    // Create blank list item
        //    ComboBoxItem item = new ComboBoxItem();

        //    item.Content = TempLoadingSpinnerItem.Content;

        //    item.IsSelected = true;

        //    // Add the item to the combo box
        //    ((ComboBox)sender).Items.Add(item);

        //    return item;
        //}

        //// Remove item sample
        //// https://stackoverflow.com/questions/6831825/remove-combobox-item-from-combobox-wpf
        //private void RemoveListItem(object sender, ComboBoxItem item)
        //{
        //    ((ComboBox)sender).Items.Remove(item);
        //}

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
    }
}