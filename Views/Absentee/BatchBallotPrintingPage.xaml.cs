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
//using VoterX.Core.Models.Utilities;
using System.Collections.ObjectModel;
//using VoterX.Core.Models.ViewModels;
//using VoterX.Core.Extensions;
//using VoterX.Core.Models.Database;
using VoterX.Dialogs;
using VoterX.Extensions;
using VoterX.Core.Voters;
using VoterX.Manager.Global;

namespace VoterX
{
    class BallotItem
    {
        public int ID { get; set; }
        public string File { get; set; }
        public string Name { get; set; }
        public int Total { get; set; }
    }

    /// <summary>
    /// Interaction logic for BatchBallotPrintingPage.xaml
    /// </summary>
    public partial class BatchBallotPrintingPage : Page
    {
        private ObservableCollection<NMVoter> voterlist = new ObservableCollection<NMVoter>();

        //private avBatch _Batch = new avBatch();

        private bool BallotStyleLoaded = false;

        private bool BatchPrinted = false;

        private List<BallotItem> BallotItemList = new List<BallotItem>();

        private int? _siteId;
        private int? _machineId;

        public BatchBallotPrintingPage(): this(null) {}
        public BatchBallotPrintingPage(int? site)
        {
            InitializeComponent();

            if (site == null)
            {
                _siteId = AppSettings.Absentee.SiteID;
                _machineId = AppSettings.System.MachineID;
            }
            else
            {
                _siteId = site;
                _machineId = null;
            }

            GlobalReferences.Header.PageHeader = ("Print Mail-In Ballots");
            //StatusBar.ApplicationStatusClear();
            GlobalReferences.StatusBar.TextClear();

            //MainMenuMethods.ShowHamburger();
            //MainMenuMethods.HideHamburger();
            //GlobalReferences.Header.HamburgerMenuVisibility = false;

            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            CheckServer();

            // Load Batch State
            if (AppSettings.Absentee.BatchReportPrinted == true && AppSettings.Absentee.BatchReportPrintedDate.Date == DateTime.Now.Date)
            {
                PrintFunctionsPanel.Visibility = Visibility.Visible;
            }

            // Check Batch Mode
            //StatusBar.ApplicationStatusLeft(AppSettings.Absentee.BatchPrintingMode.ToString());
            if(AppSettings.Absentee.BatchPrintingMode == "ALL")
            {
                // Change informantion text
                BallotStyleDropDownInfo.Text = "SELECT A BALLOT STYLE TO VIEW";
                PrintLabelsAndBallotsInfo.Text = "PRINT ALL LABELS AND BALLOTS";
                FinishBallotsInfo.Text = "FINISH ALL BATCHED STYLES";

                // Hide single style buttons
                PrintBatchLabelsButton.Visibility = Visibility.Collapsed;
                PrintBatchBallotButton.Visibility = Visibility.Collapsed;
                FinishBatchButton.Visibility = Visibility.Collapsed;
                // Show all style buttons
                PrintAllBatchLabelsButton.Visibility = Visibility.Visible;
                PrintAllBatchBallotButton.Visibility = Visibility.Visible;
                FinishAllBatchButton.Visibility = Visibility.Visible;

                // Load ballot list
                LoadBallotStylesAll();
            }
            else
            {
                LoadBallotStylesSingle();
            }

            var stuff = BatchPrinted;
        }

        private void CheckServer()
        {
            //await StatusBar.CheckServer();
        }

        private async void LoadBallotStylesSingle()
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(BallotStylesList, TempLoadingSpinnerItem);

            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            SearchResults.Text = "";

            // Clear the list box in order to display searching spinner
            ClearListView();

            // Display search spinner
            SearchingPanel.Visibility = Visibility.Visible;

            // Check database connection
            if (await Task.Run(() => VoterMethods.Exists) == true) 
            {
                //StatusBar.ApplicationStatusCenter("Loading Ballot Styles");
                ObservableCollection<NMVoter> voters = new ObservableCollection<NMVoter>();

                // Get list of voters with Log Code 5 (Application Accepted)
                voters = await Task.Run(() => VoterMethods.Voters.List(
                    (int)_siteId,
                    _machineId,
                    5
                    ));

                // Check if there was an error 
                string errorMessage = voters.CheckForErrors();
                if (errorMessage != null)
                {
                    GlobalReferences.StatusBar.TextCenter = ("SQL ERROR: " + errorMessage);
                }
                else
                {
                    // Put total voter count in the box
                    TotalBallotCount.Text = voters.Count().ToString();

                    try
                    {
                        // Get a list of ballot styles from the current voter list
                        var voterListGroup = voters.GroupBy(g => g.Data.BallotStyleID);
                        BallotStyleCount.Text = voterListGroup.Count().ToString();
                        BallotStylesList.Items.Clear();
                        foreach (var group in voterListGroup.OrderBy(o => o.Key))
                        {
                            BallotItem BallotStyleItem = new BallotItem();
                            BallotStyleItem.ID = (int)group.Key;
                            BallotStyleItem.File = group.LastOrDefault().Data.BallotStyleFile;
                            BallotStyleItem.Name = group.LastOrDefault().Data.BallotStyle;
                            BallotStyleItem.Total = group.Count();

                            BallotItemList.Add(BallotStyleItem);

                            ComboBoxMethods.AddComboItemToControl(
                                BallotStylesList,
                                BallotStyleItem.ID,
                                BallotStyleItem.Name,
                                0
                                );
                            BallotStylesList.SelectedIndex = 0;
                        }
                    }
                    catch
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Could Not Group Ballot Styles:" + voters.Count().ToString());
                        return;
                    }

                    BallotStylesList.SelectedIndex = 0;

                    if (BallotStylesList.SelectedIndex >= 0)
                    {
                        LoadAcceptedVotersByBallotStyle();
                    }
                    else
                    {
                        CancelBatchButton.Visibility = Visibility.Visible;
                    }

                    BallotStyleLoaded = true;
                }
            }

            ComboBoxMethods.RemoveListItem(BallotStylesList, loadingItem);

            // Turn off the progress spinner
            //StatusBar.LoadingSpinner(Visibility.Collapsed);
            SearchingPanel.Visibility = Visibility.Collapsed;
        }

        private void LoadAcceptedVotersByBallotStyle()
        {
            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            SearchResults.Text = "";

            // Clear the list box in order to display searching spinner
            ClearListView();

            // Display search spinner
            //StatusBar.LoadingSpinner(Visibility.Visible);
            GlobalReferences.StatusBar.SpinnerVisibility = true;
            SearchingPanel.Visibility = Visibility.Visible;

            PrintFunctionsGrid.Visibility = Visibility.Collapsed;
            BatchStatsGrid.Visibility = Visibility.Collapsed;

            int styleID = ComboBoxMethods.GetSelectedItemNumber(BallotStylesList);
            //StatusBar.ApplicationStatus("Ballot ID" + styleID.ToString());
            var selectedballotitem = BallotItemList.Where(bs => bs.ID == styleID).FirstOrDefault();
            if (selectedballotitem != null)
            {
                SelectedBallotCount.Text = selectedballotitem.Total.ToString();
            }

            // Load voters
            voterlist = VoterMethods.Voters.List(
            (int)_siteId,
            _machineId,
            5,
            styleID
            );

            //Display the list
            VoterList.ItemsSource = voterlist
                .OrderBy(o1 => o1.Data.BallotStyle)
                .OrderBy(o2 => o2.Data.LastName)
                .OrderBy(o3 => o3.Data.FirstName)
                .OrderBy(o4 => o4.Data.DeliveryAddress1);
            //StatusBar.ApplicationStatusCenter("Current Count: " + voterlist.Count().ToString());

            // Turn off the progress spinner
            //StatusBar.LoadingSpinner(Visibility.Collapsed);
            GlobalReferences.StatusBar.SpinnerVisibility = false;
            SearchingPanel.Visibility = Visibility.Collapsed;

            PrintFunctionsGrid.Visibility = Visibility.Visible;
            BatchStatsGrid.Visibility = Visibility.Visible;
        }

        private void LoadBatchedVoters()
        {
            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            SearchResults.Text = "";

            // Clear the list box in order to display searching spinner
            ClearListView();

            // Display search spinner
            //StatusBar.LoadingSpinner(Visibility.Visible);
            GlobalReferences.StatusBar.SpinnerVisibility = true;
            SearchingPanel.Visibility = Visibility.Visible;

            //if (_Batch != null)
            //{
            List<int> voterIdList = new List<int>();
            //voterIdList = await GetBatchedVoters(_Batch.batch_id);

            // Load voters
            //voterlist = await Task.Run(() => VoterMethods.Voter.ListAsync(
            //    (int)AppSettings.System.SiteID,
            //    AppSettings.System.MachineID,
            //    5
            //    ));

            // Display the list
            //VoterList.ItemsSource = voterlist
            //    .OrderBy(o1 => o1.BallotStyle)
            //    .OrderBy(o2 => o2.LastName)
            //    .OrderBy(o3 => o3.FirstName)
            //    .OrderBy(o4 => o4.DeliveryAddress1);
            //StatusBar.ApplicationStatusCenter("Current Count: " + voterlist.Count().ToString());

            //DisplayBatchStats();                
            //}
            // Turn off the progress spinner
            //StatusBar.LoadingSpinner(Visibility.Collapsed);
            GlobalReferences.StatusBar.SpinnerVisibility = false;
            SearchingPanel.Visibility = Visibility.Collapsed;

            //PrintBatchBallotButton.Visibility = Visibility.Visible;
            //PrintBatchLabelsButton.Visibility = Visibility.Visible;
            ////PrintBatchReportButton.Visibility = Visibility.Visible;
            //FinishBatchButton.Visibility = Visibility.Visible;
            BatchStatsGrid.Visibility = Visibility.Visible;
            PrintFunctionsGrid.Visibility = Visibility.Visible;
        }

        private async void LoadBallotStylesAll()
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(BallotStylesList, TempLoadingSpinnerItem);            

            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            SearchResults.Text = "";

            // Clear the list box in order to display searching spinner
            ClearListView();

            // Display search spinner
            SearchingPanel.Visibility = Visibility.Visible;

            // Check database connection
            if (await Task.Run(() => VoterMethods.Exists) == true)
            {
                //StatusBar.ApplicationStatusCenter("Loading Ballot Styles");
                ObservableCollection<NMVoter> voters = new ObservableCollection<NMVoter>();

                // Get list of voters with Log Code 5 (Application Accepted)
                voters = await Task.Run(() => VoterMethods.Voters.List(
                    (int)_siteId,
                    _machineId,
                    5
                    ));

                // Check if there was an error 
                string errorMessage = voters.CheckForErrors();
                if (errorMessage != null)
                {
                    GlobalReferences.StatusBar.TextCenter = ("SQL ERROR: " + errorMessage);
                }
                else
                {
                    // Put total voter count in the box
                    TotalBallotCount.Text = voters.Count().ToString();

                    try
                    {
                        // Get a list of ballot styles from the current voter list
                        var voterListGroup = voters.GroupBy(g => g.Data.BallotStyleID).ToList();
                        BallotStyleCount.Text = voterListGroup.Count().ToString();
                        BallotStylesList.Items.Clear();
                        ComboBoxMethods.AddLoadingItem(BallotStylesList, ShowAllItem);
                        foreach (var group in voterListGroup.OrderBy(o => o.Key))
                        {
                            BallotItem BallotStyleItem = new BallotItem();
                            BallotStyleItem.ID = (int)group.Key;
                            BallotStyleItem.File = group.LastOrDefault().Data.BallotStyleFile;
                            BallotStyleItem.Name = group.LastOrDefault().Data.BallotStyle;
                            BallotStyleItem.Total = group.Count();

                            BallotItemList.Add(BallotStyleItem);

                            ComboBoxMethods.AddComboItemToControl(
                                BallotStylesList,
                                BallotStyleItem.ID,
                                BallotStyleItem.Name,
                                0
                                );
                            BallotStylesList.SelectedIndex = 0;
                        }
                    }
                    catch
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Could Not Group Ballot Styles:" + voters.Count().ToString());
                        return;
                    }

                    BallotStylesList.SelectedIndex = 0;

                    if (BallotStylesList.SelectedIndex >= 0)
                    {
                        LoadAcceptedVotersByBallotStyleAll();
                    }
                    else
                    {
                        CancelBatchButton.Visibility = Visibility.Visible;
                    }

                    BallotStyleLoaded = true;

                    BallotStylesList.IsEnabled = false;
                }
            }

            ComboBoxMethods.RemoveListItem(BallotStylesList, loadingItem);

            // Turn off the progress spinner
            //StatusBar.LoadingSpinner(Visibility.Collapsed);
            SearchingPanel.Visibility = Visibility.Collapsed;
        }

        private void LoadAcceptedVotersByBallotStyleAll()
        {
            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            SearchResults.Text = "";

            // Clear the list box in order to display searching spinner
            ClearListView();

            // Display search spinner
            //StatusBar.LoadingSpinner(Visibility.Visible);
            GlobalReferences.StatusBar.SpinnerVisibility = true;
            SearchingPanel.Visibility = Visibility.Visible;

            PrintFunctionsGrid.Visibility = Visibility.Collapsed;
            BatchStatsGrid.Visibility = Visibility.Collapsed;

            //PrintBatchBallotButton.Visibility = Visibility.Collapsed;
            //PrintBatchLabelsButton.Visibility = Visibility.Collapsed;
            ////PrintBatchReportButton.Visibility = Visibility.Collapsed;
            //FinishBatchButton.Visibility = Visibility.Collapsed;

            int styleID = ComboBoxMethods.GetSelectedItemNumber(BallotStylesList);
            //StatusBar.ApplicationStatus("Ballot ID" + styleID.ToString());
            var selectedballotitem = BallotItemList.Where(bs => bs.ID == styleID).FirstOrDefault();
            if (selectedballotitem != null)
            {
                SelectedBallotCount.Text = selectedballotitem.Total.ToString();
            }

            // Load voters
            voterlist = VoterMethods.Voters.List(
            (int)_siteId,
            _machineId,
            5
            //, styleID
            );

            string errorMessage = voterlist.CheckForErrors();
            if (errorMessage != null)
            {
                GlobalReferences.StatusBar.TextCenter = ("SQL ERROR: " + errorMessage);
            }
            else
            {
                // Display the list
                VoterList.ItemsSource = voterlist
                    .OrderBy(o1 => o1.Data.BallotStyle)
                    .OrderBy(o2 => o2.Data.LastName)
                    .OrderBy(o3 => o3.Data.FirstName)
                    .OrderBy(o4 => o4.Data.DeliveryAddress1);
                //StatusBar.ApplicationStatusCenter("Current Count: " + voterlist.Count().ToString());
            }

            // Turn off the progress spinner
            //StatusBar.LoadingSpinner(Visibility.Collapsed);
            GlobalReferences.StatusBar.SpinnerVisibility = false;
            SearchingPanel.Visibility = Visibility.Collapsed;

            //PrintBatchBallotButton.Visibility = Visibility.Visible;
            //PrintBatchLabelsButton.Visibility = Visibility.Visible;
            ////PrintBatchReportButton.Visibility = Visibility.Visible;
            //FinishBatchButton.Visibility = Visibility.Visible;

            PrintFunctionsGrid.Visibility = Visibility.Visible;
            BatchStatsGrid.Visibility = Visibility.Visible;
        }

        // Clear the list view object
        private void ClearListView()
        {
            VoterList.ItemsSource = null;
        }

        //private async Task<List<int>> GetBatchedVoters(int batchId)
        //{
        //    List<int> idList = new List<int>();
        //    var voterList = await Task.Run(() => BatchDataMethods.BatchedVoters.Query(0).Where(v => v.batch_id == batchId).ToList());            
        //    foreach (var voter in voterList)
        //    {
        //        idList.Add(voter.voter_id);
        //    }
        //    return idList;
        //}

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Notes on scroll offsets
            // https://stackoverflow.com/questions/1033841/is-it-possible-to-implement-smooth-scroll-in-a-wpf-listview
            // https://social.msdn.microsoft.com/Forums/en-US/3594c80a-7ccf-4cfc-9cc0-9731fd080d72/in-what-unit-is-the-scrollviewerverticaloffset?forum=winappswithcsharp

            //double delta = (e.Delta * .26978); // Roughly half of 1 list item
            double delta = (e.Delta * .36);
            //double delta = (e.Delta / 120)*32; // Reduce to +1 or -1 then multiply to get exact units
            //StatusBar.ApplicationStatusCenter("Scrolling:" + (delta).ToString());

            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - (delta));
            e.Handled = true;
        }

        private void Page_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer_PreviewMouseWheel(SearchScrollViewer, e);
        }

        private async void PrintBatchBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //string style = ComboBoxMethods.GetSelectedItem(BallotStylesList);
            int styleID = ComboBoxMethods.GetSelectedItemNumber(BallotStylesList);
            var selectedballotitem = BallotItemList.Where(bs => bs.ID == styleID).FirstOrDefault();
            string style = selectedballotitem.Name;

            OkCancelDialog newMessage = new OkCancelDialog("Print Ballots", "YOU ARE ABOUT TO PRINT " + selectedballotitem.Total.ToString() + " BALLOT(S)\r\nFOR BALLOT STYLE " + selectedballotitem.Name + "\r\nMAKE SURE YOUR BALLOT STOCK IS IN THE BALLOT TRAY\r\nAND THE PRINTER IS READY");
            if (newMessage.ShowDialog() == true)
            {
                GlobalReferences.StatusBar.TextLeft = ("Printing Batched Ballots ");

                //if (result == true)
                {
                    var voterListGroup = voterlist.GroupBy(g => g.Data.BallotStyleID);
                    foreach (var group in voterListGroup)
                    {
                        var styleId = group.Key;
                        var file = group.LastOrDefault().Data.BallotStyleFile;
                        var total = group.Count();
                        //AlertDialog newPrintMessage = new AlertDialog("NOW PRINTING: " + file.ToString());
                        //newPrintMessage.ShowDialog();
                        GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => BallotPrinting.PrintBallotBatch(file, total)));

                        BatchPrinted = true;

                        BallotStylesList.IsEnabled = false;
                        CancelBatchButton.IsEnabled = false;
                        FinishBatchButton.IsEnabled = true;
                    }
                }
                //else
                //{
                //    StatusBar.ApplicationStatusCenter("Error Writting to Voter Record");
                //}
                GlobalReferences.StatusBar.TextLeft = ("");
            }
        }

        private async void PrintAllBatchBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //string style = ComboBoxMethods.GetSelectedItem(BallotStylesList);
            //int styleID = ComboBoxMethods.GetSelectedItemNumber(BallotStylesList);
            //var selectedballotitem = BallotItemList.Where(bs => bs.ID == styleID).FirstOrDefault();
            //string style = selectedballotitem.Name;

            OkCancelDialog newMessage = new OkCancelDialog("Print Ballots", "YOU ARE ABOUT TO PRINT " + TotalBallotCount.Text.ToString() + " BALLOT(S)\r\nMAKE SURE YOUR BALLOT STOCK IS IN THE BALLOT TRAY\r\nAND THE PRINTER IS READY");
            if (newMessage.ShowDialog() == true)
            {
                //StatusBar.ApplicationStatus("Printing Batched Ballots ");

                //if (result == true)
                {
                    BatchPrinted = true;

                    BallotStylesList.IsEnabled = false;
                    CancelBatchButton.IsEnabled = false;
                    FinishAllBatchButton.IsEnabled = true;

                    var voterListGroup = voterlist.GroupBy(g => g.Data.BallotStyleID).ToList();
                    foreach (var styleItem in BallotItemList)
                    {
                        //var styleId = group.Key;
                        //var file = group.LastOrDefault().BallotStyleFile;
                        //var total = group.Count();
                        //AlertDialog newPrintMessage = new AlertDialog("NOW PRINTING: " + file.ToString());
                        //newPrintMessage.ShowDialog();
                        GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => BallotPrinting.PrintBallotBatch(styleItem.File, styleItem.Total)));

                    }
                }
                //else
                //{
                //    StatusBar.ApplicationStatusCenter("Error Writting to Voter Record");
                //}
            }
        }

        private async void PrintBatchLabelsButton_Click(object sender, RoutedEventArgs e)
        {
            int styleID = ComboBoxMethods.GetSelectedItemNumber(BallotStylesList);
            var selectedballotitem = BallotItemList.Where(bs => bs.ID == styleID).FirstOrDefault();

            OkCancelDialog newMessage = new OkCancelDialog("Print Labels","YOU ARE ABOUT TO PRINT " + selectedballotitem.Total.ToString() + " LABEL(S)\r\nFOR BALLOT STYLE " + selectedballotitem.Name + "\r\nMAKE SURE YOUR LABELS ARE IN THE REPORT TRAY\r\nAND THE PRINTER IS READY");
            if (newMessage.ShowDialog() == true)
            {
                //StatusBar.ApplicationStatus("Printing Batched Mailing Labels");
                GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => ReportPrintingMethods.PrintBatchLabels(styleID, _siteId)));
            }
        }

        private async void PrintAllBatchLabelsButton_Click(object sender, RoutedEventArgs e)
        {
            OkCancelDialog newMessage = new OkCancelDialog("Print Labels", "YOU ARE ABOUT TO PRINT " + VoterList.Items.Count.ToString() + " LABEL(S)\r\nMAKE SURE YOUR LABELS ARE IN THE REPORT TRAY\r\nAND THE PRINTER IS READY");
            if (newMessage.ShowDialog() == true)
            {
                List<int> styleIDlist = new List<int>();
                foreach (var style in BallotItemList)
                {
                    styleIDlist.Add(style.ID);
                }

                GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => ReportPrintingMethods.PrintBatchLabels(styleIDlist, _siteId)));
            }
        }

        private async void PrintBatchReportButton_Click(object sender, RoutedEventArgs e)
        {
            // Save batch state
            AppSettings.Absentee.BatchReportPrinted = true;
            AppSettings.Absentee.BatchReportPrintedDate = DateTime.Now;
            AppSettings.SaveAbsentee();

            PrintFunctionsPanel.Visibility = Visibility.Visible;
            GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => ReportPrintingMethods.PrintBatchDetailReport(_siteId, _machineId))); 
        }

        private void CancelBatchButton_Click(object sender, RoutedEventArgs e)
        {
            //YesNoDialog newMessage = new YesNoDialog("Cancel Batch", "");
            //if (newMessage.ShowDialog() == true)
            //{
                // Open Menu 
                //MainMenuMethods.OpenMainMenu();
                GlobalReferences.MenuSlider.Open(); 

                this.NavigateToPage(new AbsenteeAdministrationPage());
            //}
        }

        private void BallotStylesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(BallotStyleLoaded == true)
            {
                if (BallotStylesList.SelectedIndex >= 0)
                {
                    if (AppSettings.Absentee.BatchPrintingMode == "ALL")
                    {
                        LoadAcceptedVotersByBallotStyleAll();
                    }
                    else
                    {
                        LoadAcceptedVotersByBallotStyle();
                    }
                }
                else
                {
                    CancelBatchButton.Visibility = Visibility.Visible;
                }
            }
        }

        private async void FinishBatchButton_Click(object sender, RoutedEventArgs e)
        {
            string style = ComboBoxMethods.GetSelectedItem(BallotStylesList);

            OkCancelDialog newMessage = new OkCancelDialog("Clear Batch", "MAKE SURE YOU HAVE ALL LABELS, BALLOTS AND THE REPORT\r\nBEFORE CLEARING THE BATCH FOR " + style.ToString());
            if (newMessage.ShowDialog() == true)
            {
                DateTime issuedDate = DateTime.Now;

                bool result = false;
                // Mark Ballot Issued and Printed
                if (voterlist != null)
                {
                    //result = await VoterMethods.UpdateBallotIssuedList(voterlist.ToList(), issuedDate);
                    result = await Task.Run(() =>
                    {
                        try
                        {
                            voterlist.IssueBallots();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    });
                }

                if (result == false)
                {
                    GlobalReferences.StatusBar.TextCenter = ("Error Writting to Voter Record");
                }
                else
                {
                    // Reload Page
                    LoadBallotStylesSingle(); 

                    BatchPrinted = false;

                    BallotStylesList.IsEnabled = true;
                    CancelBatchButton.IsEnabled = true;

                    if (VoterList.Items.Count == 0)
                    {
                        AppSettings.Absentee.BatchReportPrinted = false;
                        AppSettings.SaveAbsentee();
                    }
                }
            }
        }

        private async void FinishAllBatchButton_Click(object sender, RoutedEventArgs e)
        {
            string style = ComboBoxMethods.GetSelectedItem(BallotStylesList);

            OkCancelDialog newMessage = new OkCancelDialog("Clear Batch", "MAKE SURE YOU HAVE ALL LABELS, BALLOTS AND THE REPORT\r\nBEFORE CLEARING THE BATCH(ES) ");
            if (newMessage.ShowDialog() == true)
            {
                DateTime issuedDate = DateTime.Now;

                // Load voters
                voterlist = await Task.Run(() => VoterMethods.Voters.List(
                    (int)_siteId,
                    _machineId,
                    5
                    ));

                bool result = false;
                // Mark Ballot Issued and Printed
                if (voterlist != null)
                {
                    //result = await VoterMethods.UpdateBallotIssuedList(voterlist.ToList(), issuedDate);
                    result = await Task.Run(() => 
                    {
                        try
                        {
                            voterlist.IssueBallots();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    });
                }

                if (result == false)
                {
                    GlobalReferences.StatusBar.TextCenter = ("Error Writting to Voter Record");
                }
                else
                {
                    // Reload Page
                    //LoadBallotStylesSingle();

                    //BatchPrinted = false;

                    //BallotStylesList.IsEnabled = true;
                    //CancelBatchButton.IsEnabled = true;

                    AppSettings.Absentee.BatchReportPrinted = false;
                    AppSettings.SaveAbsentee();

                    // Open Menu 
                    //MainMenuMethods.OpenMainMenu();
                    GlobalReferences.MenuSlider.Open();

                    this.NavigateToPage(new AbsenteeAdministrationPage());
                }
            }
        }

        private void TestBatchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadBallotStylesSingle();
        }
    }
}
