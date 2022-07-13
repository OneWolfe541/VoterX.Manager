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
using System.Collections.ObjectModel;
//using VoterX.Core.Models.ViewModels;
//using VoterX.Core.Models.Database;
using VoterX.Models;
using VoterX.Core.Elections;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeReportWizardPage.xaml
    /// </summary>
    public partial class AbsenteeReportWizardPage : Page
    {
        private ReportWizardQueryModel _wizardSearch;

        private bool logCodeLoaded = false;
        private bool ballotStyleLoaded = false;
        private bool partyLoaded = false;
        private bool jurisdictionLoaded = false;
        private bool siteLoaded = false;

        public AbsenteeReportWizardPage()
        {
            InitializeComponent();

            _wizardSearch = new ReportWizardQueryModel();

            GlobalReferences.Header.PageHeader = ("Report Wizard");

            LoadLogCodes();

            LoadBallotStyles();

            LoadParties();

            LoadJurisdictionTypes();

            //LoadJurisdictions();

            LoadSites();

            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            CheckServer();
        }

        public AbsenteeReportWizardPage(ReportWizardQueryModel searchParameters)
        {
            InitializeComponent();

            _wizardSearch = searchParameters;

            //StatusBar.ApplicationStatus(_wizardSearch.BeginningDate.ToString() + " " + _wizardSearch.EndingDate.ToString());

            GlobalReferences.Header.PageHeader = ("Report Wizard");

            // Set Include Inactive check box and hide Locations
            if(searchParameters.IncludeInactive == true)
            {
                InactiveVotersCheck.IsChecked = true;

                SitesLabel.Visibility = Visibility.Hidden;
                SitesList.Visibility = Visibility.Hidden;
                //SitesLoadingPanel.Visibility = Visibility.Collapsed;
                ClearSitesButton.Visibility = Visibility.Hidden;

                StatusLabel.IsEnabled = false;
                LogCodesList.IsEnabled = false;
                ClearStatusButton.IsEnabled = false;
            }

            LoadLogCodes(searchParameters);

            LoadBallotStyles(searchParameters);

            LoadParties(searchParameters);

            LoadJurisdictionTypes(searchParameters);

            //LoadJurisdictions();

            LoadSites(searchParameters);

            if (_wizardSearch.IdRequired == true)
            {
                IdRequiredCheck.IsChecked = true;
            }
            else
            {
                IdRequiredCheck.IsChecked = false;
            }

            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            CheckServer();
        }

        private async void CheckServer()
        {
            NextWizardButton.Visibility = Visibility.Collapsed;

            if (await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(100) == true)
            {
                NextWizardButton.Visibility = Visibility.Visible;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

        }        

        private async void LoadLogCodes()
        {
            LogCodeLoadingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var loglist = await Task.Run(() => ElectionDataMethods.LogCodes.Where(l => l.LogCode > 2).ToList());
                LogCodesList.ItemsSource = loglist;
            }

            LogCodeLoadingPanel.Visibility = Visibility.Collapsed;
        }

        private async void LoadLogCodes(ReportWizardQueryModel searchParameters)
        {
            LogCodeLoadingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var loglist = await Task.Run(() => ElectionDataMethods.LogCodes.Where(l => l.LogCode > 2).ToList());
                LogCodesList.ItemsSource = loglist;
            }

            LogCodeLoadingPanel.Visibility = Visibility.Collapsed;

            // Disable item selection changed
            logCodeLoaded = false;

            // Load previous selected list items
            for(int i = 0; i < LogCodesList.Items.Count; i++)
            {                
                foreach (var logcode in searchParameters.LogCodes)
                {
                    if (((LogCodeModel)LogCodesList.Items[i]).LogCode == logcode)
                    {
                        LogCodesList.SelectedItems.Add(LogCodesList.Items[i]);
                    }
                }
            }

            // Enable selection changed
            logCodeLoaded = true;
        }

        private void LogCodesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListView)sender).SelectedIndex >= 0 && logCodeLoaded != false)
            {
                //string messagelist = "";

                _wizardSearch.LogCodes.Clear();

                if (InactiveVotersCheck.IsChecked == true)
                {
                    _wizardSearch.LogCodes.Add(1);
                }

                var items = ((ListView)sender).SelectedItems;
                foreach (var item in items)
                {
                    LogCodeModel logCodeItem = item as LogCodeModel;
                    _wizardSearch.LogCodes.Add(logCodeItem.LogCode);

                    //messagelist += logCodeItem.log_description + " ";
                }

                //StatusBar.ApplicationStatus(messagelist);
            }
        }

        private void ClearStatusButton_Click(object sender, RoutedEventArgs e)
        {
            _wizardSearch.LogCodes.Clear();
            LogCodesList.SelectedItems.Clear();
            //StatusBar.ApplicationStatus("");
        }

        private async void LoadBallotStyles()
        {
            BallotStyleLoadingPanel.Visibility = Visibility.Visible;

            var stylelist = await Task.Run(() => ElectionDataMethods.BallotStyles);
            if (stylelist != null)
            {
                BallotStyleList.ItemsSource = stylelist;
            }

            BallotStyleLoadingPanel.Visibility = Visibility.Collapsed;
        }

        private async void LoadBallotStyles(ReportWizardQueryModel searchParameters)
        {
            BallotStyleLoadingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var stylelist = await Task.Run(() => ElectionDataMethods.UniqueBallotStyles);
                BallotStyleList.ItemsSource = stylelist;
            }

            BallotStyleLoadingPanel.Visibility = Visibility.Collapsed;

            // Disable item selection changed
            ballotStyleLoaded = false;

            // Load previous selected list items
            for (int i = 0; i < BallotStyleList.Items.Count; i++)
            {
                foreach (var ballotstyle in searchParameters.BallotStyles)
                {
                    if (((BallotStyleModel)BallotStyleList.Items[i]).BallotStyleId == ballotstyle)
                    {
                        BallotStyleList.SelectedItems.Add(BallotStyleList.Items[i]);
                    }
                }
            }

            // Enable selection changed
            ballotStyleLoaded = true;
        }

        private void BallotStyleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListView)sender).SelectedIndex >= 0 && ballotStyleLoaded != false)
            {
                //string messagelist = "";

                _wizardSearch.BallotStyles.Clear();

                var items = ((ListView)sender).SelectedItems;
                foreach (var item in items)
                {
                    BallotStyleModel ballotStyleItem = item as BallotStyleModel;
                    _wizardSearch.BallotStyles.Add(ballotStyleItem.BallotStyleId);

                    //messagelist += ballotStyleItem.ballot_style_name + " ";
                }

                //StatusBar.ApplicationStatus(messagelist);
            }
        }

        private void ClearBallotStyleButton_Click(object sender, RoutedEventArgs e)
        {
            _wizardSearch.BallotStyles.Clear();
            BallotStyleList.SelectedItems.Clear();
            //StatusBar.ApplicationStatus("");
        }

        private async void LoadParties()
        {
            PartyLoadingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                ObservableCollection<PartyModel> partylist = await Task.Run(() => new ObservableCollection<PartyModel>(ElectionDataMethods.Partys));
                PartiesList.ItemsSource = partylist.OrderBy(o => o.PartyCode);
            }

            PartyLoadingPanel.Visibility = Visibility.Collapsed;
        }

        private async void LoadParties(ReportWizardQueryModel searchParameters)
        {
            PartyLoadingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                ObservableCollection<PartyModel> partylist = await Task.Run(() => new ObservableCollection<PartyModel>(ElectionDataMethods.Partys));
                PartiesList.ItemsSource = partylist.OrderBy(o => o.PartyCode);
            }

            PartyLoadingPanel.Visibility = Visibility.Collapsed;

            // Disable item selection changed
            partyLoaded = false;

            // Load previous selected list items
            for (int i = 0; i < PartiesList.Items.Count; i++)
            {
                foreach (var party in searchParameters.Parties)
                {
                    if (((PartyModel)PartiesList.Items[i]).PartyCode == party)
                    {
                        PartiesList.SelectedItems.Add(PartiesList.Items[i]);
                    }
                }
            }

            // Enable selection changed
            partyLoaded = true;
        }

        private void PartiesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListView)sender).SelectedIndex >= 0 && partyLoaded != false)
            {
                //string messagelist = "";

                _wizardSearch.Parties.Clear();

                var items = ((ListView)sender).SelectedItems;
                foreach (var item in items)
                {
                    PartyModel partyItem = item as PartyModel;
                    _wizardSearch.Parties.Add(partyItem.PartyCode);

                    //messagelist += partyItem.party + " ";
                }

                //StatusBar.ApplicationStatus(messagelist);
            }
        }

        private void ClearPartyButton_Click(object sender, RoutedEventArgs e)
        {
            _wizardSearch.Parties.Clear();
            PartiesList.SelectedItems.Clear();
            //StatusBar.ApplicationStatus("");
        }

        private async void LoadJurisdictionTypes()
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(JurisdictionTypeList, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                foreach (var jurisdictiontype in await Task.Run(() => ElectionDataMethods.JurisdictionTypes))
                {
                    ComboBoxMethods.AddComboItemToControl(
                        JurisdictionTypeList,
                        jurisdictiontype,
                        jurisdictiontype,
                        ""
                        );
                }
            }

            ComboBoxMethods.RemoveListItem(JurisdictionTypeList, loadingItem);

            jurisdictionLoaded = true;
        }

        private async void LoadJurisdictionTypes(ReportWizardQueryModel searchParameters)
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(JurisdictionTypeList, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                foreach (var jurisdictiontype in await Task.Run(() => ElectionDataMethods.JurisdictionTypes))
                {
                    ComboBoxMethods.AddComboItemToControl(
                        JurisdictionTypeList,
                        jurisdictiontype,
                        jurisdictiontype,
                        searchParameters.JurisdictionType
                        );
                }
            }

            ComboBoxMethods.RemoveListItem(JurisdictionTypeList, loadingItem);

            LoadJurisdictions(searchParameters);

            jurisdictionLoaded = true;
        }

        private void JurisdictionTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(jurisdictionLoaded == true && ((ComboBox)sender).SelectedIndex >= 0)
            {
                string type = ComboBoxMethods.GetSelectedItem(sender).Content as string;
                _wizardSearch.JurisdictionType = type;
                LoadJurisdictions(type);
                //StatusBar.ApplicationStatus(ComboBoxMethods.GetSelectedItem(sender).Content as string);
            }
        }

        private async void LoadJurisdictions()
        {
            JurisdictionLoadingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var jurisdictionlist = await Task.Run(() => ElectionDataMethods.Jurisdictions);
                JurisdictionList.ItemsSource = jurisdictionlist.OrderBy(o => o.JurisdictionName);
            }

            JurisdictionLoadingPanel.Visibility = Visibility.Collapsed;
        }

        private async void LoadJurisdictions(string jurisdictionType)
        {
            JurisdictionLoadingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var jurisdictionlist = await Task.Run(() => ElectionDataMethods.Jurisdictions.Where(jt => jt.JurisdictionType == jurisdictionType).ToList());
                JurisdictionList.ItemsSource = jurisdictionlist.OrderBy(o => o.JurisdictionName);
            }

            JurisdictionLoadingPanel.Visibility = Visibility.Collapsed;
        }

        private async void LoadJurisdictions(ReportWizardQueryModel searchParameters)
        {
            JurisdictionLoadingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var jurisdictionlist = await Task.Run(() => ElectionDataMethods.Jurisdictions.Where(jt => jt.JurisdictionType == searchParameters.JurisdictionType).ToList());
                JurisdictionList.ItemsSource = jurisdictionlist.OrderBy(o => o.JurisdictionName);
            }

            JurisdictionLoadingPanel.Visibility = Visibility.Collapsed;

            // Disable item selection changed
            jurisdictionLoaded = false;

            // Load previous selected list items
            for (int i = 0; i < JurisdictionList.Items.Count; i++)
            {
                foreach (var jurisdiction in searchParameters.Jurisdictions)
                {
                    if (((JurisdictionModel)JurisdictionList.Items[i]).JurisdictionId == jurisdiction)
                    {
                        JurisdictionList.SelectedItems.Add(JurisdictionList.Items[i]);
                    }
                }
            }

            // Enable selection changed
            jurisdictionLoaded = true;
        }

        private void JurisdictionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListView)sender).SelectedIndex >= 0 && jurisdictionLoaded != false)
            {
                //string messagelist = "";

                _wizardSearch.Jurisdictions.Clear();

                var items = ((ListView)sender).SelectedItems;
                foreach (var item in items)
                {
                    JurisdictionModel jurisdictionItem = item as JurisdictionModel;
                    _wizardSearch.Jurisdictions.Add(jurisdictionItem.JurisdictionId);

                    //messagelist += jurisdictionItem.jurisdiction_type + ":" + jurisdictionItem.jurisdiction_name + " ";
                }

                //StatusBar.ApplicationStatus(messagelist);
            }
        }

        private void ClearJurisdictionButton_Click(object sender, RoutedEventArgs e)
        {
            _wizardSearch.Jurisdictions.Clear();
            JurisdictionList.SelectedItems.Clear();
            //StatusBar.ApplicationStatus("");
        }

        private async void LoadSites()
        {
            SitesLoadingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var siteslist = await Task.Run(() => ElectionDataMethods.Locations);
                SitesList.ItemsSource = siteslist.OrderBy(o => o.PlaceName);
            }

            SitesLoadingPanel.Visibility = Visibility.Collapsed;
        }

        private async void LoadSites(ReportWizardQueryModel searchParameters)
        {
            SitesLoadingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                var siteslist = await Task.Run(() => ElectionDataMethods.Locations);
                SitesList.ItemsSource = siteslist.OrderBy(o => o.PlaceName);
            }

            SitesLoadingPanel.Visibility = Visibility.Collapsed;

            // Disable item selection changed
            siteLoaded = false;

            // Load previous selected list items
            for (int i = 0; i < SitesList.Items.Count; i++)
            {
                foreach (var site in searchParameters.PollSites)
                {
                    if (((LocationModel)SitesList.Items[i]).PollId == site)
                    {
                        SitesList.SelectedItems.Add(SitesList.Items[i]);
                    }
                }
            }

            // Enable selection changed
            siteLoaded = true;
        }

        private void SitesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListView)sender).SelectedIndex >= 0 && siteLoaded != false)
            {
                //string messagelist = "";

                _wizardSearch.PollSites.Clear();

                var items = ((ListView)sender).SelectedItems;
                foreach (var item in items)
                {
                    LocationModel locationItem = item as LocationModel;
                    _wizardSearch.PollSites.Add(locationItem.PollId);

                    //messagelist += locationItem.place_name + " ";
                }

                //StatusBar.ApplicationStatus(messagelist);
            }
        }

        private void ClearSitesButton_Click(object sender, RoutedEventArgs e)
        {
            _wizardSearch.PollSites.Clear();
            SitesList.SelectedItems.Clear();
            //StatusBar.ApplicationStatus("");
        }

        private void PrintTestButton_Click(object sender, RoutedEventArgs e)
        {
            _wizardSearch.BeginningDate = DateTime.Parse("03/01/2018");
            _wizardSearch.EndingDate = DateTime.Parse("04/10/2018");

            GlobalReferences.StatusBar.TextLeft = (ReportPrintingMethods.PrintCustomSpecialReport(_wizardSearch));
        }

        private void PreviousWizardButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrameMethods.NavigateToPage(new AbsenteeReportWizardDatePage(_wizardSearch));
        }

        private void NextWizardButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrameMethods.NavigateToPage(new AbsenteeReportWizardPrintPage(_wizardSearch));
        }

        private void InactiveVotersCheck_Click(object sender, RoutedEventArgs e)
        {
            if (InactiveVotersCheck.IsChecked == true)
            {
                // Add log code 1 to list
                _wizardSearch.LogCodes.Add(1);

                _wizardSearch.IncludeInactive = true;

                SitesLabel.Visibility = Visibility.Hidden;
                SitesList.Visibility = Visibility.Hidden;
                //SitesLoadingPanel.Visibility = Visibility.Collapsed;
                ClearSitesButton.Visibility = Visibility.Hidden;

                StatusLabel.IsEnabled = false;
                LogCodesList.IsEnabled = false;
                ClearStatusButton.IsEnabled = false;
            }
            else
            {
                SitesLabel.Visibility = Visibility.Visible;
                SitesList.Visibility = Visibility.Visible;
                //SitesLoadingPanel.Visibility = Visibility.Visible;
                ClearSitesButton.Visibility = Visibility.Visible;

                // Take log code 1 out of the list
                _wizardSearch.LogCodes.Remove(1);

                _wizardSearch.IncludeInactive = false;

                StatusLabel.IsEnabled = true;
                LogCodesList.IsEnabled = true;
                ClearStatusButton.IsEnabled = true;
            }
        }

        private void IdRequiredCheck_Click(object sender, RoutedEventArgs e)
        {
            if(IdRequiredCheck.IsChecked == true)
            {
                _wizardSearch.IdRequired = true;
            }
            else
            {
                _wizardSearch.IdRequired = false;
            }
        }
    }
}
