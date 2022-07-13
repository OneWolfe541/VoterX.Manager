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
using VoterX.Core.Elections;
using VoterX.Methods;
using VoterX.ABS.Logging;
using VoterX.Manager.Global;
using System.Collections.ObjectModel;
//using VoterX.Core.Models.Database;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for ElectionPage.xaml
    /// </summary>
    public partial class ElectionPage : SettingsBasePage
    {
        private ObservableCollection<PartyModel> _partylist = null;

        public bool pageLoaded = false;
        private bool partyLoaded = false;

        public ElectionPage()
        {
            InitializeComponent();            

            LoadElectionListAsync();

            DisplaySettings();

            ShowPartyList();

            pageLoaded = true;
        }

        private async void LoadElectionListAsync()
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(ElectionList, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var electionItem = ElectionDataMethods.Election.Lists.Election;

                if (electionItem != null)
                {
                    try
                    {
                        // Insert a single election item into the combobox
                        ComboBoxMethods.AddComboItemToControl(
                            ElectionList,                                                       // Destination Combobox
                            electionItem,                                                       // Object added to DataContext
                            electionItem.ToString(),
                            true
                            );
                    }
                    catch (Exception e)
                    {
                        VoterXLogger _errorLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging); ;
                        _errorLogger.WriteLog("Election Settings: " + e.Message);
                    }
                }
                else
                {
                    try
                    {
                        // Try loading entire eleciton table instead
                        var electionList = ElectionDataMethods.Election.LoadAllElections();

                        foreach (var election in electionList)
                        {
                            ComboBoxMethods.AddComboItemToControl(
                            ElectionList,                                                       // Destination Combobox
                            election,                                            // Object added to DataContext
                            election.ToString().TrimEnd(),                                  // String displayed in list
                            false
                            );
                        }
                        if (electionList.Count() == 1)
                        {
                            ElectionList.SelectedIndex = 0;
                            PopulateElectionDetails(ElectionList);
                        }
                    }
                    catch (Exception e)
                    {
                        VoterXLogger _errorLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging); ;
                        _errorLogger.WriteLog("Election Settings: " + e.Message);
                    }
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
            }

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(ElectionList, loadingItem);
        }

        //private void LoadElectionList()
        //{
        //    ElectionList.Items.Clear();

        //    // Create animated loading list item
        //    var loadingItem = ComboBoxMethods.AddLoadingItem(ElectionList, TempLoadingSpinnerItem);

        //    if (ElectionDataMethods.Exists == true)
        //    {
        //        //int count = 0;
        //        //foreach (var electionItem in ElectionMethods.GetElectionList())
        //        var electionItem = ElectionDataMethods.Election.Lists.Election;
        //        {
        //            try
        //            {
        //                // Insert a single election item into the combobox
        //                ComboBoxMethods.AddComboItemToControl(
        //                    ElectionList,                                                       // Destination Combobox
        //                    electionItem.ElectionId,                                                       // Object added to DataContext
        //                    electionItem.ToString(),
        //                    electionItem.ElectionId
        //                    );
        //                // Increment Row Count
        //                //count++;
        //            }
        //            catch (Exception e)
        //            {
        //                VoterXLogger _errorLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging); ;
        //                _errorLogger.WriteLog("Election Settings: " + e.Message);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        StatusBar.ApplicationStatusCenter("Database not found");
        //    }

        //    // Remove animated loading list item
        //    ComboBoxMethods.RemoveListItem(ElectionList, loadingItem);
        //}

        private void ShowPartyList()
        {
            if (AppSettings.Election.ElectionType == VoterX.SystemSettings.Enums.ElectionType.Primary)
            {
                LoadParties();

                PartiesBreak.Visibility = Visibility.Visible;
                PartiesLabel.Visibility = Visibility.Visible;
                PartiesList.Visibility = Visibility.Visible;
                //PartyLoadingPanel.Visibility = Visibility.Visible;
                ClearPartyButton.Visibility = Visibility.Visible;
            }
        }

        private void LoadParties()
        {
            if (AppSettings.Election.ElectionType == VoterX.SystemSettings.Enums.ElectionType.Primary)
            {
                PartiesBreak.Visibility = Visibility.Visible;
                PartiesLabel.Visibility = Visibility.Visible;
                PartiesList.Visibility = Visibility.Visible;
                ClearPartyButton.Visibility = Visibility.Visible;

                if (ElectionDataMethods.Election.Lists.Partys != null)
                {
                    _partylist = new ObservableCollection<PartyModel>(ElectionDataMethods.Election.Lists.Partys);
                    PartiesList.ItemsSource = _partylist.OrderBy(o => o.PartyCode);
                }

                PartyLoadingPanel.Visibility = Visibility.Collapsed;

                // Disable item selection changed
                partyLoaded = false;

                // Load previous selected list items
                if (AppSettings.Election.EligibleParties != null && AppSettings.Election.EligibleParties.Count > 0)
                {
                    for (int i = 0; i < PartiesList.Items.Count; i++)
                    {
                        foreach (var party in AppSettings.Election.EligibleParties)
                        {
                            if (((PartyModel)PartiesList.Items[i]).PartyCode == party)
                            {
                                PartiesList.SelectedItems.Add(PartiesList.Items[i]);
                            }
                        }
                    }
                }

                // Enable selection changed
                partyLoaded = true;
            }
        }

        private void DisplaySettings()
        {
            //// ELECTION SETTINGS //
            //ElectionID.Text = AppSettings.Election.ElectionID.ToString();
            //CountyCode.Text = AppSettings.Election.CountyCode.ToString();
            ////ElectionType.Text = AppSettings.Election.ElectionType.ToString();
            //SetElectionType(AppSettings.Election.ElectionType);
            //ElectionEntity.Text = AppSettings.Election.ElectionEntity.ToString();
            //ElectionTitle.Text = AppSettings.Election.ElectionTitle.ToString();
            //ElectionDate.Text = AppSettings.Election.ElectionDate.ToShortDateString();

            PopulateElectionDetails(ElectionList);
        }

        public override void SaveSettings()
        {
            //StatusBar.ApplicationStatus("Election Settings Page Called");

            // Boolean values will probably not be set on this page

            PopulateElectionDetails(ElectionList);

            // ELECTION SETTINGS //
            try
            {
                AppSettings.Election.ElectionID = Int32.Parse(ElectionID.Text);            
                AppSettings.Election.CountyCode = CountyCode.Text;
                //AppSettings.Election.ElectionType = Int32.Parse(ElectionType.Text);
                AppSettings.Election.ElectionType = (VoterX.SystemSettings.Enums.ElectionType)ComboBoxMethods.GetSelectedItemNumber(ElectionType);
                AppSettings.Election.ElectionEntity = ElectionEntity.Text;
                AppSettings.Election.ElectionTitle = ElectionTitle.Text;
                AppSettings.Election.ElectionDate = DateTime.Parse(ElectionDate.Text);
            }
            catch
            {

            }
        }

        private void ElectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateElectionDetails(sender);
        }

        private void PopulateElectionDetails(object sender)
        {
            if (((ComboBox)sender).IsLoaded)
            {
                var selectedElection = ComboBoxMethods.GetSelectedItemData<ElectionModel>(((ComboBox)sender));

                if (selectedElection != null)
                {
                    ElectionID.Text = selectedElection.ElectionId.ToString();
                    CountyCode.Text = selectedElection.CountyCode;
                    SetElectionType(ConvertElectionType(selectedElection.ElectionType));
                    ElectionEntity.Text = selectedElection.CountyName;
                    ElectionTitle.Text = selectedElection.ElectionName;
                    if (selectedElection.ElectionDate != null)
                    {
                        ElectionDate.Text = ((DateTime)selectedElection.ElectionDate).ToShortDateString();
                    }
                }

                //StatusBar.ApplicationStatusCenter(selectedElection.ElectionName);
            }
        }

        //private tblElection GetSelectedItemData(ComboBox sender)
        //{
        //    if (sender.SelectedItem == null) return null;
        //    else
        //        return (tblElection)((ComboBoxItem)sender.SelectedItem).DataContext;
        //} 

        //private T GetSelectedItemData<T>(ComboBox sender)
        //{
        //        return (T)((ComboBoxItem)sender.SelectedItem).DataContext;
        //}

        private int ConvertElectionType(string value)
        {
            if (value == "Primary") return 1;
            else return 2;
        }

        private void SetElectionType(int type)
        {
            switch (type)
            {
                case 1:
                    PrimaryElectionType.IsSelected = true;
                    break;
                case 2:
                    GeneralElectionType.IsSelected = true;
                    break;
            }
        }

        private void ElectionList_DropDownOpened(object sender, EventArgs e)
        {
            //LoadElectionListAsync();
        }

        private void ElectionList_Loaded(object sender, RoutedEventArgs e)
        {
            DisplaySettings();
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

        private void PartiesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListView)sender).SelectedIndex >= 0 && partyLoaded != false)
            {
                if (AppSettings.Election.EligibleParties == null) AppSettings.Election.EligibleParties = new List<string>();

                AppSettings.Election.EligibleParties.Clear();

                var items = ((ListView)sender).SelectedItems;
                foreach (var item in items)
                {
                    PartyModel partyItem = item as PartyModel;
                    AppSettings.Election.EligibleParties.Add(partyItem.PartyCode);
                }
            }
        }

        private void ClearPartyButton_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.Election.EligibleParties.Clear();
            PartiesList.SelectedItems.Clear();
        }
    }
}
