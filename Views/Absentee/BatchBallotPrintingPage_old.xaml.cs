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
    /// Interaction logic for BatchBallotPrintingPage.xaml
    /// </summary>
    public partial class BatchBallotPrintingPage_old : Page
    {
        //private ObservableCollection<VoterDataModel> voterlist = new ObservableCollection<VoterDataModel>();

        //private avBatch _Batch = new avBatch();

        //public BatchBallotPrintingPage_old(avBatch currentBatch)
        //{
        //    InitializeComponent();

        //    _Batch = currentBatch;

        //    LoadBatchedVoters();

        //    StatusBar.ApplicationPageHeader("Print Mail-In Ballots");
        //    StatusBar.ApplicationStatusClear();

        //    MainMenuMethods.ShowHamburger();
        //}

        public BatchBallotPrintingPage_old()
        {
            InitializeComponent();

            //_Batch = await CheckCurrentBatch(5);
            //SetCurrentBatch();            

            GlobalReferences.Header.PageHeader = ("Print Mail-In Ballots");
            //StatusBar.ApplicationStatusClear();
            GlobalReferences.StatusBar.TextClear();

            //MainMenuMethods.ShowHamburger();
            GlobalReferences.Header.HamburgerMenuVisibility = true;
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
            //    List<int> voterIdList = new List<int>();
            //    voterIdList = await GetBatchedVoters(_Batch.batch_id);

            //    // Load voters
            //    voterlist = await Task.Run(() => VoterMethods.Voter.FromListAsync(voterIdList));

            //    // Display the list
            //    VoterList.ItemsSource = voterlist
            //        .OrderBy(o1 => o1.BallotStyle)
            //        .OrderBy(o2 => o2.LastName)
            //        .OrderBy(o3 => o3.FirstName)
            //        .OrderBy(o4 => o4.DeliveryAddress1);
            //    StatusBar.ApplicationStatusCenter("Current Count: " + voterlist.Count().ToString());

            //    DisplayBatchStats();                
            //}
            // Turn off the progress spinner
            //StatusBar.LoadingSpinner(Visibility.Collapsed);
            GlobalReferences.StatusBar.SpinnerVisibility = false;
            SearchingPanel.Visibility = Visibility.Collapsed;

            PrintBatchBallotButton.Visibility = Visibility.Visible;
            PrintBatchLabelsButton.Visibility = Visibility.Visible;
            PrintBatchReportButton.Visibility = Visibility.Visible;
            CancelBatchButton.Visibility = Visibility.Visible;
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

        private void PrintBatchBallotButton_Click(object sender, RoutedEventArgs e)
        {
            //YesNoDialog newMessage = new YesNoDialog("Print Ballots", "ARE YOU SURE YOU WANT TO PRINT THESE BALLOTS?");
            //if (newMessage.ShowDialog() == true)
            //{
            //    StatusBar.ApplicationStatus("Printing Batched Ballots ");

            //    DateTime issuedDate = DateTime.Now;

            //    // Mark Ballot Issued and Printed
            //    var result = await VoterMethods.UpdateBallotIssuedList(voterlist.ToList(), issuedDate);

            //    if (result == true)
            //    {
            //        var voterListGroup = voterlist.GroupBy(g => g.BallotStyleID);
            //        foreach (var group in voterListGroup)
            //        {
            //            var styleId = group.Key;
            //            var file = group.LastOrDefault().BallotStyleFile;
            //            var total = group.Count();
            //            //AlertDialog newPrintMessage = new AlertDialog("NOW PRINTING: " + file.ToString());
            //            //newPrintMessage.ShowDialog();
            //            StatusBar.ApplicationStatus(await Task.Run(() => BallotPrinting.PrintBallotBatch(file, total)));
            //        }
            //    }
            //    else
            //    {
            //        StatusBar.ApplicationStatusCenter("Error Writting to Voter Record");
            //    }
            //}
        }

        private void PrintBatchLabelsButton_Click(object sender, RoutedEventArgs e)
        {
            YesNoDialog newMessage = new YesNoDialog("Print Ballots", "ARE YOU SURE YOU WANT TO PRINT THESE MAILING LABELS?");
            if (newMessage.ShowDialog() == true)
            {
                GlobalReferences.StatusBar.TextLeft = ("Printing Batched Mailing Labels");
            }
        }

        private void PrintBatchReportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        //private async void SetCurrentBatch()
        //{
        //    _Batch = await CheckCurrentBatch(5);
        //    LoadBatchedVoters();
        //}

        //private async Task<avBatch> CheckCurrentBatch(int batchType)
        //{
        //    var currentBatch = await GetBatch(batchType);

        //    if (currentBatch == null)
        //    {
        //        AlertDialog newMessage = new AlertDialog("THERE IS NO EXISTING BATCH");
        //        newMessage.ShowDialog();
        //        StatusBar.ApplicationStatus("No Current Batch");                
        //    }
        //    else
        //    {
        //        StatusBar.ApplicationStatus("Current Open Batch: " + currentBatch.batch_id);                
        //    }

        //    return currentBatch;
        //}

        //private async Task<avBatch> GetBatch(int batchType)
        //{
        //    StatusBar.ApplicationStatus("Checking Current Batch");
        //    StatusBar.LoadingSpinner(Visibility.Visible);

        //    // Reset search result messages
        //    SearchingPanel.Visibility = Visibility.Collapsed;
        //    SearchResults.Visibility = Visibility.Collapsed;
        //    SearchResults.Text = "";

        //    // Display search spinner
        //    StatusBar.LoadingSpinner(Visibility.Visible);
        //    SearchingPanel.Visibility = Visibility.Visible;
        //    avBatch currentBatch = new avBatch();
        //    if (await Task.Run(() => BatchDataMethods.Batches.Exists(0)) == true)
        //    {
        //        currentBatch = await Task.Run(() => BatchDataMethods.Batches.Query(0).Where(b =>
        //            b.election_id == AppSettings.Election.ElectionID &&
        //            b.county_code == AppSettings.Election.CountyCode &&
        //            b.poll_id == AppSettings.System.SiteID &&
        //            b.computer == AppSettings.System.MachineID &&
        //            b.log_code == batchType &&
        //            b.batch_open == false &&
        //            b.printed_date == null).FirstOrDefault());
        //    }

        //    StatusBar.LoadingSpinner(Visibility.Collapsed);

        //    // Turn off the progress spinner
        //    StatusBar.LoadingSpinner(Visibility.Collapsed);
        //    SearchingPanel.Visibility = Visibility.Collapsed;

        //    return currentBatch;
        //}

        private void CancelBatchButton_Click(object sender, RoutedEventArgs e)
        {
            //YesNoDialog newMessage = new YesNoDialog("Cancel Batch", "ARE YOU SURE YOU WANT TO REMOVE ALL THE VOTERS FROM THIS BATCH?");
            //if (newMessage.ShowDialog() == true)
            //{
            //    VoterList.ItemsSource = null;
            //    voterlist = new ObservableCollection<VoterDataModel>();                

            //    // Remove all voters from the current batch
            //    BatchDataMethods.RemoveAllVotersFromBatch(_Batch.batch_id);
            //}

            //// Open Menu 
            //MainMenuMethods.OpenMainMenu();

            //this.NavigateToPage(new AbsenteeAdministrationPage());
        }

        private void DisplayBatchStats()
        {
            //BatchNumber.Text = _Batch.batch_id.ToString();
            //TotalBallotCount.Text = voterlist.Count().ToString();

            //CurrentBallotStyleNumber.Text = "1";
            //var voterListGroup = voterlist.GroupBy(g => g.BallotStyleID);
            //foreach(var group in voterListGroup)
            //{
            //    var styleId = group.Key;
            //    var total = group.Count();
            //}
            //TotalBallotStyles.Text = voterListGroup.Count().ToString();
        }
        
    }
}
