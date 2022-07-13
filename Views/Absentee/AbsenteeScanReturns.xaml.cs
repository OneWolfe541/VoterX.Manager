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
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeScanReturns.xaml
    /// </summary>
    public partial class AbsenteeScanReturns : Page
    {
        //private ObservableCollection<VoterDataModel> voterlist = new ObservableCollection<VoterDataModel>();

        private int ReturnType = 0;
        private int BatchType = 0;

        //private avBatch _Batch = new avBatch();

        public AbsenteeScanReturns(string returnType)
        {
            InitializeComponent();

            BarCodeSearch.Focus();

            SetReturnType(returnType);

            // Check for existing batch
            
            LoadCurrentBatch(BatchType);

            LoadLogCodes();

            // Hide Menu
            //MainMenuMethods.HideHamburger();
            GlobalReferences.Header.HamburgerMenuVisibility = false;

            CheckServer();
        }

        private void CheckServer()
        {
            //ScanGrid.Visibility = Visibility.Collapsed;

            //if (await StatusBar.CheckServer() == true)
            //{
            //    ScanGrid.Visibility = Visibility.Visible;
            //}
        }

        private void SetReturnType(string type)
        {
            switch(type)
            {
                case "Applications":
                    ReturnType = 1;
                    BatchType = 5;
                    GlobalReferences.Header.PageHeader = ("Scan Accepted Applications");
                    //PrepareBatchButton.Visibility = Visibility.Visible;
                    //CancelBatchButton.Visibility = Visibility.Visible;
                    break;
                case "Ballots":
                    ReturnType = 2;
                    BatchType = 9;
                    GlobalReferences.Header.PageHeader = ("Scan Returned Envelopes");
                    BallotReturnTypePanel.Visibility = Visibility.Visible;
                    SearchListBorder.Margin = new Thickness(0,63,0,10);
                    //PrepareBatchButton.Visibility = Visibility.Collapsed;
                    //CancelBatchButton.Visibility = Visibility.Collapsed;
                    //ProcessBallotsButton.Visibility = Visibility.Visible;
                    break;
                default:
                    GlobalReferences.Header.PageHeader = ("Unknow Type");
                    break;
            }
        }

        // Clear the list view object
        private void ClearListView()
        {
            VoterList.ItemsSource = null;
        }

        private void BarCodeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ((TextBox)sender).IsEnabled = false;

                // Prevent Spam Scans
                if (LastBarCode.Text != BarCodeSearch.Text)
                {
                    ProcessSingleVoter(BarCodeSearch.Text);
                }

                Keyboard.Focus(BarCodeSearch);

                LastBarCode.Text = BarCodeSearch.Text;

                BarCodeSearch.Text = "";

                ((TextBox)sender).IsEnabled = true;
            }
        }

        private void LoadLogCodes()
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(LogCodeList, TempLoadingSpinnerItem);

            List<int> codeList = new List<int> { 9, 10, 12, 14 };

            //if (await Task.Run(() => VoterMethods.LogCodes.Exists(0)) == true)
            //{
            //    foreach (var logcodelist in await Task.Run(() => VoterMethods.LogCodes.Query(0).Where(l => codeList.Contains(l.log_code)).ToList()))
            //    {
            //        ComboBoxMethods.AddComboItemToControl(
            //            LogCodeList,
            //            logcodelist.log_code,
            //            logcodelist.log_description,
            //            9
            //            );
            //    }
            //}

            ComboBoxMethods.RemoveListItem(LogCodeList, loadingItem);
        }


        private void ProcessSingleVoter(string voterID)
        {
            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            SearchResults.Text = "";

            // Clear the list box in order to display searching spinner
            ClearListView();
            
            // Display search spinner
            //StatusBar.LoadingSpinner(Visibility.Visible);
            SearchingPanel.Visibility = Visibility.Visible;

            //if (await Task.Run(() => VoterMethods.Voter.Exists()) == true)
            //{

            //    // Create the Voter Search Object (Search Parameters)
            //    VoterSearchModel searchItems = new VoterSearchModel
            //    {
            //        VoterID = voterID
            //    };

            //    // Prevent duplicate scans
            //    if (CheckIfVoterExists(voterID) != true)
            //    {
            //        StatusBar.ApplicationStatus("Voter Not In List");

            //        var voter = await VoterMethods.Voter.SingleAsync(searchItems);

            //        if (voter != null && voter.Count() > 0)
            //        {
            //            StatusBar.ApplicationStatus("Voter Found In Database");

            //            int? voterLogCode = voter.FirstOrDefault().LogCode;
            //            // Check for issued applications
            //            if (ReturnType == 1 && voterLogCode != 3)
            //            {
            //                if (voterLogCode < 3 || voterLogCode == null)
            //                {
            //                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS NOT BEEN ISSUED AN ABSENTEE APPLICATION");
            //                    newMessage.ShowDialog();
            //                }
            //                else if (voterLogCode == 4)
            //                {
            //                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS A REJECTED ABSENTEE APPLICATION");
            //                    newMessage.ShowDialog();
            //                }
            //                else if (voterLogCode == 5)
            //                {
            //                    AlertDialog newMessage = new AlertDialog("THIS VOTER ALREADY HAS AN ACCEPTED ABSENTEE APPLICATION");
            //                    newMessage.ShowDialog();
            //                }
            //                else if (voterLogCode > 5)
            //                {
            //                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
            //                    newMessage.ShowDialog();
            //                }
            //            }

            //            // Process returned ballots immediately
            //            else if (ReturnType == 2 && voter.FirstOrDefault().LogCode != 5)
            //            {
            //                // Process record as voted
            //            }
            //            else
            //            {
            //                voterlist = AddVoterToList(voter);

            //                AddVoterToBatch(voter.FirstOrDefault());
            //            }
            //        }
            //        else
            //        {
            //            StatusBar.ApplicationStatus("Voter Not Found");
            //        }

            //    }
            //    else
            //    {
            //        StatusBar.ApplicationStatusCenter("Database not found");
            //    }

            //    // Display the list
            //    VoterList.ItemsSource = voterlist;
            //    StatusBar.ApplicationStatusCenter("Current Count: " + voterlist.Count().ToString());
            //}
            //else
            //{
            //    StatusBar.ApplicationStatusCenter("Database not found");
            //}

            // Turn off the progress spinner
            //StatusBar.LoadingSpinner(Visibility.Collapsed);
            SearchingPanel.Visibility = Visibility.Collapsed;

            // Set focus back to scan box
            Keyboard.Focus(BarCodeSearch);
        }

        // https://stackoverflow.com/questions/1565289/union-two-observablecollection-lists
        //private ObservableCollection<VoterDataModel> AddVoterToList(ObservableCollection<VoterDataModel> voter)
        //{
        //    ObservableCollection<VoterDataModel> list = new ObservableCollection<VoterDataModel>();
        //    foreach (var v in voter.Union(voterlist)) list.Add(v);
        //    return list;
        //}

        /// <summary>
        /// Returns true if voter exists in the current list
        /// </summary>
        /// <param name="voterID"></param>
        /// <returns></returns>
        //private bool CheckIfVoterExists(string voterID)
        //{
        //    bool result = false;
        //    foreach(var v in voterlist)
        //    {
        //        if (v.VoterID == voterID) return true;
        //    }
        //    return result;
        //}

        //private async void CheckBatch(int batchType)
        //{
        //    var currentBatch = await GetBatch(batchType);

        //    if (currentBatch == null)
        //    {
        //        StatusBar.ApplicationStatus("No Current Batch");
        //        CreateBatchButton.Visibility = Visibility.Visible;
        //        PrepareBatchButton.Visibility = Visibility.Collapsed;
        //        CancelBatchButton.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        StatusBar.ApplicationStatus("Current Open Batch: " + currentBatch.batch_id);
        //        CreateBatchButton.Visibility = Visibility.Collapsed;

        //        ShowButtons();
        //    }            

        //    //_Batch = currentBatch;
        //}

        //private async Task<avBatch> CheckCurrentBatch(int batchType)
        //{
        //    var currentBatch = await GetBatch(batchType);

        //    if (currentBatch == null)
        //    {
        //        StatusBar.ApplicationStatus("No Current Batch");
        //        //CreateBatchButton.Visibility = Visibility.Visible;
        //        PrepareBatchButton.Visibility = Visibility.Collapsed;
        //        CancelBatchButton.Visibility = Visibility.Collapsed;
        //        // Create batch
        //        CreateBatch(BatchType);

        //        CheckBatch(BatchType);

        //        BarCodeSearch.Focus();
        //    }
        //    else
        //    {
        //        StatusBar.ApplicationStatus("Current Open Batch: " + currentBatch.batch_id);
        //        CreateBatchButton.Visibility = Visibility.Collapsed;

        //        ShowButtons();
        //    }

        //    return currentBatch;
        //}

        private void LoadCurrentBatch(int batchType)
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

            // Check batch
            //_Batch = await CheckCurrentBatch(batchType); 

            //if (_Batch != null)
            //{
            //    List<int> voterIdList = new List<int>();
            //    voterIdList = await GetBatchedVoters(_Batch.batch_id);

            //    // Load voters
            //    voterlist = await Task.Run(() => VoterMethods.Voter.FromListAsync(voterIdList));

            //    // Display the list
            //    VoterList.ItemsSource = voterlist;
            //    StatusBar.ApplicationStatusCenter("Current Count: " + voterlist.Count().ToString());
            //}
            // Turn off the progress spinner
            //StatusBar.LoadingSpinner(Visibility.Collapsed);
            GlobalReferences.StatusBar.SpinnerVisibility = false;
            SearchingPanel.Visibility = Visibility.Collapsed;

            // Set focus back to scan box
            Keyboard.Focus(BarCodeSearch);
        }

        //private async Task<avBatch> GetBatch(int batchType)
        //{
        //    StatusBar.ApplicationStatus("Checking Current Batch");
        //    StatusBar.LoadingSpinner(Visibility.Visible);

        //    var currentBatch = await Task.Run(() => BatchDataMethods.Batches.Query(0).Where(b =>
        //        b.election_id == AppSettings.Election.ElectionID &&
        //        b.county_code == AppSettings.Election.CountyCode &&
        //        b.poll_id == AppSettings.System.SiteID &&
        //        b.computer == AppSettings.System.MachineID &&
        //        b.log_code == batchType &&
        //        b.batch_open == true).FirstOrDefault());

        //    StatusBar.LoadingSpinner(Visibility.Collapsed);

        //    return currentBatch;
        //}

        //private async Task<List<int>> GetBatchedVoters(int batchId)
        //{
        //    List<int> idList = new List<int>();
        //    var voterList = await Task.Run(() => BatchDataMethods.BatchedVoters.Query(0).Where(v => v.batch_id == batchId).ToList());
        //    if (voterList != null)
        //    {
        //        foreach (var voter in voterList)
        //        {
        //            idList.Add(voter.voter_id);
        //        }
        //        return idList;
        //    }
        //    else
        //    {
        //        return null;
        //    }
            
        //}

        //private void AddVoterToBatch(VoterDataModel voter)
        //{
        //    if (voter != null)
        //    {
        //        BatchDataMethods.InsertBatchedVoter(_Batch.batch_id, Int32.Parse(voter.VoterID));
        //    }
        //}

        private void Page_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer_PreviewMouseWheel(SearchScrollViewer, e);
        }

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

        private void CreateBatchButton_Click(object sender, RoutedEventArgs e)
        {
            CreateBatch(BatchType); 

            //CheckBatch(BatchType);

            BarCodeSearch.Focus();
        }

        private void CreateBatch(int batchType)
        {
            //var newBatch = new avBatch
            //{
            //    election_id = AppSettings.Election.ElectionID,
            //    county_code = AppSettings.Election.CountyCode,
            //    poll_id = AppSettings.System.SiteID,
            //    computer = AppSettings.System.MachineID,
            //    log_code = batchType,
            //    batch_open = true,
            //    batch_created_date = DateTime.Now
            //};
                        
            //BatchDataMethods.InsertBatch(newBatch);
        }

        private void PrepareBatchButton_Click(object sender, RoutedEventArgs e)
        {
            //if (voterlist != null && voterlist.Count() > 0)
            //{
            //    // Popup message
            //    YesNoDialog newMessage = new YesNoDialog("Accepted Applications", "ARE YOU SURE YOU WANT TO MARK THESE APPLICATIONS ACCEPTED?");
            //    if (newMessage.ShowDialog() == true)
            //    {
            //        // Close Batch
            //        BatchDataMethods.CloseBatch(_Batch.batch_id);

            //        DateTime acceptedDate = DateTime.Now;

            //        // Get list of voters
            //        //foreach (var voter in voterlist)
            //        //{
            //        //    // Mark Voters as "Application Accepted"
            //        //    var result = await Task.Run(() => VoterMethods.UpdateApplicationAccepted(voter, acceptedDate));
            //        //}

            //        // Mark Voters as "Application Accepted"
            //        var result = await VoterMethods.UpdateApplicationAcceptedList(voterlist.ToList(), acceptedDate);

            //        // Navigate to ballot priting page
            //        //this.NavigateToPage(new BatchBallotPrintingPage(_Batch));
            //    }
            //}
            //else
            //{
            //    AlertDialog newMessage = new AlertDialog("NO VOTERS SELECTED");
            //    newMessage.ShowDialog();
            //}

            // Set focus back to scan box
            Keyboard.Focus(BarCodeSearch);
        }

        private void CancelBatchButton_Click(object sender, RoutedEventArgs e)
        {
            //YesNoDialog newMessage = new YesNoDialog("Cancel Batch", "ARE YOU SURE YOU WANT TO REMOVE ALL THE VOTERS FROM THIS BATCH?");
            //if (newMessage.ShowDialog() == true)
            //{
            //    VoterList.ItemsSource = null;
            //    voterlist = new ObservableCollection<VoterDataModel>();
            //    LastBarCode.Text = "";

            //    // Remove all voters from the current batch
            //    BatchDataMethods.RemoveAllVotersFromBatch(_Batch.batch_id);
            //}

            //// Set focus back to scan box
            //Keyboard.Focus(BarCodeSearch);

            //// Show Menu button
            //MainMenuMethods.ShowHamburger();

            //// Open Menu 
            //MainMenuMethods.OpenMainMenu();

            //this.NavigateToPage(new AbsenteeAdministrationPage());
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                RemoveVoterButton.Visibility = Visibility.Visible;
                //StatusBar.ApplicationStatus(((VoterDataModel)item.DataContext).VoterID.ToString());
            }
            else
            {
                RemoveVoterButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ListViewItem_LostFocus(object sender, RoutedEventArgs e)
        {
            //RemoveVoterButton.Visibility = Visibility.Collapsed;
        }

        private void RemoveVoterButton_Click(object sender, RoutedEventArgs e)
        {
            if(VoterList.SelectedIndex >= 0)
            {
                //VoterDataModel voter = (VoterDataModel)VoterList.SelectedItem;

                //BatchDataMethods.RemoveBatchedVoter(voter.VoterID.ToInt());

                //voterlist.Remove(voter);
                //VoterList.ItemsSource = voterlist;

                RemoveVoterButton.Visibility = Visibility.Collapsed;
            }
            
        }

        private void ProcessBallotsButton_Click(object sender, RoutedEventArgs e)
        {
            var processType = ((ComboBoxItem)LogCodeList.SelectedItem).Content.ToString();
            int logCodeId = ComboBoxMethods.GetSelectedItemNumber(LogCodeList);

            YesNoDialog newMessage = new YesNoDialog("Process Batch", "ARE YOU SURE YOU WANT TO PROCESS THESE " + processType.ToUpper());
            if (newMessage.ShowDialog() == true)
            {

                GlobalReferences.StatusBar.TextLeft = ("Ballots have been processed: " + logCodeId.ToString());
                //MainMenuMethods.ShowHamburger();
                //MainFrameMethods.NavigateToPage(new BatchBallotPrintingPage(_Batch));
            }
        }

        private void ShowButtons()
        {
            if (ReturnType == 1)
            {
                PrepareBatchButton.Visibility = Visibility.Visible;
                CancelBatchButton.Visibility = Visibility.Visible;
                ProcessBallotsButton.Visibility = Visibility.Collapsed;
            }

            if (ReturnType == 2)
            {
                PrepareBatchButton.Visibility = Visibility.Collapsed;
                CancelBatchButton.Visibility = Visibility.Collapsed;
                ProcessBallotsButton.Visibility = Visibility.Visible;
            }
        }

        private void LogCodeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BarCodeSearch.Focus();
        }
    }
}
