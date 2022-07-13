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
using VoterX.Core.ScanHistory;
using VoterX.Manager.Global;
using VoterX.Core.Elections;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeScanReturns.xaml
    /// </summary>
    public partial class AbsenteeScanReturnedEnvelopes : Page
    {
        private ObservableCollection<NMVoter> voterlist = new ObservableCollection<NMVoter>();

        //private int ReturnType = 0;
        //private int BatchType = 0;

        //private avBatch _Batch = new avBatch();

        private ScanHistory _session;

        public AbsenteeScanReturnedEnvelopes()
        {
            InitializeComponent();

            BarCodeSearch.Focus();

            SetReturnType();

            // Check for existing batch
            
            //LoadCurrentBatch(BatchType);

            LoadLogCodes();

            LoadRejectedReasons();

            // Hide Menu
            //MainMenuMethods.HideHamburger();
            //GlobalReferences.Header.HamburgerMenuVisibility = false;

            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            //CheckServer();
        }

        //private async void CheckServer()
        //{
        //    await StatusBar.CheckServer();
        //}

        private void SetReturnType()
        {
            //switch(type)
            //{
            //    case "Applications":
            //        ReturnType = 1;
            //        BatchType = 5;
            //        StatusBar.ApplicationPageHeader("Scan Accepted Applications");
            //        //PrepareBatchButton.Visibility = Visibility.Visible;
            //        //CancelBatchButton.Visibility = Visibility.Visible;
            //        break;
            //    case "Ballots":
            //        ReturnType = 2;
            //        BatchType = 9;
                    GlobalReferences.Header.PageHeader = ("Scan Returned Envelopes");
                    BallotReturnTypePanel.Visibility = Visibility.Visible;
                    SearchListBorder.Margin = new Thickness(0,110,0,10);
                    //PrepareBatchButton.Visibility = Visibility.Collapsed;
                    //CancelBatchButton.Visibility = Visibility.Collapsed;
                    //ProcessBallotsButton.Visibility = Visibility.Visible;
            //        break;
            //    default:
            //        StatusBar.ApplicationPageHeader("Unknow Type");
            //        break;
            //}
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
                // Last keypress remains in the buffer and needs to be cleared
                e.Handled = true;

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

        private async void LoadLogCodes()
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(LogCodeList, TempLoadingSpinnerItem);

            List<int> codeList = new List<int> { 9, 10, 12, 14 };

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                // Blank record added in order to force the user to select an option (08/08/2018)
                ComboBoxMethods.AddComboItemToControl(
                        LogCodeList,
                        0,
                        "",
                        0
                        );

                foreach (var logcodelist in await Task.Run(() => ElectionDataMethods.LogCodes.Where(l => codeList.Contains(l.LogCode)).ToList()))
                {
                    ComboBoxMethods.AddComboItemToControl(
                        LogCodeList,
                        logcodelist.LogCode,
                        logcodelist.LogDescription,
                        0
                        );
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
            }

            ComboBoxMethods.RemoveListItem(LogCodeList, loadingItem);
        }

        private async void LoadRejectedReasons()
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(RejectedReasonList, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                // Blank record added in order to force the user to select an option (08/08/2018)
                ComboBoxMethods.AddComboItemToControl(
                        RejectedReasonList,
                        "",
                        "",
                        ""
                        );

                foreach (var rejectedlist in await Task.Run(() => ElectionDataMethods.BallotRejectedReasons.ToList()))
                {
                    ComboBoxMethods.AddComboItemToControl(
                        RejectedReasonList,
                        rejectedlist.ServiseCode,
                        rejectedlist.RejectedReasonDescription,
                        ""
                        );
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
            }

            ComboBoxMethods.RemoveListItem(RejectedReasonList, loadingItem);
        }


        private async void ProcessSingleVoter(string voterID)
        {
            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            //VoterNotFoundPanel.Visibility = Visibility.Collapsed;
            GlobalReferences.StatusBar.TextClear();
            SearchResults.Text = "";

            // Clear the list box in order to display searching spinner
            ClearListView();
            
            // Display search spinner
            //StatusBar.LoadingSpinner(Visibility.Visible);
            SearchingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => VoterMethods.Exists) == true)
            {
                // Create the Voter Search Object (Search Parameters)
                VoterSearchModel searchItems = new VoterSearchModel
                {
                    VoterID = voterID
                };

                // Check for previous scans
                if (CheckIfVoterHasBeenScanned(voterID) == true)
                {
                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN SCANNED");
                    newMessage.ShowDialog();
                }
                // Prevent duplicate scans
                else if (CheckIfVoterExists(voterID) != true)
                {
                    //StatusBar.ApplicationStatus("Voter Not In List");

                    //var voter = await VoterMethods.Voter.SingleAsync(searchItems, AppSettings.Election.ElectionType);
                    var voter = VoterMethods.Voters.List(searchItems);                    

                    string errorMessage = voter.CheckForErrors();
                    if (errorMessage != null)
                    {
                        GlobalReferences.StatusBar.TextCenter = ("SQL ERROR: " + errorMessage);
                    }
                    else
                    {
                        if (voter != null && voter.Count() > 0)
                        {
                            // Set local values
                            voter.Localize(
                                AppSettings.Election.ElectionID,
                                AppSettings.Absentee.SiteID,
                                null,
                                null);

                            //StatusBar.ApplicationStatus("Voter Found In Database");

                            voter.FirstOrDefault().Data.ActivityDate = DateTime.Now;

                            int? voterLogCode = voter.FirstOrDefault().Data.LogCode;

                            // Add voter to current list if their log code is 6
                            if (voterLogCode != 6)
                            {
                                if (voterLogCode == 17)
                                {
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS BEEN DELETED OR REMOVED");
                                    newMessage.ShowDialog();
                                }

                                // Enrique suggested we only need 1 alert message

                                //if (voterLogCode < 3 || voterLogCode == null)
                                //{
                                //    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS NOT BEEN ISSUED AN ABSENTEE APPLICATION");
                                //    newMessage.ShowDialog();
                                //}
                                //else 
                                //if (voterLogCode == 3 || voterLogCode == 5)
                                else if (voterLogCode < 6)
                                {
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS NOT BEEN ISSUED A BALLOT");
                                    newMessage.ShowDialog();
                                }
                                //else if (voterLogCode == 4)
                                //{
                                //    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS A REJECTED ABSENTEE APPLICATION");
                                //    newMessage.ShowDialog();
                                //}
                                else if (voterLogCode > 6)
                                {
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER ALREADY HAS AN ACCPETED BALLOT");
                                    newMessage.ShowDialog();
                                }
                                //else if (voterLogCode > 5)
                                //{
                                //    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                                //    newMessage.ShowDialog();
                                //}
                            }
                            else if (voter.FirstOrDefault().Data.VotedIdRequired == true)
                            {
                                YesNoDialog newMessage = new YesNoDialog("Id Required", "THIS VOTER REQUIRES A VALID ID\r\nARE YOU SURE YOU WANT TO CONTINUE?");
                                if(newMessage.ShowDialog() == true)
                                {
                                    voterlist = voterlist.CombineLists(voter);

                                    if (voterlist != null && voterlist.Count() > 0)
                                    {
                                        TotalEnvelopeCount.Text = voterlist.Count().ToString();

                                        ProcessBallotsButton.Visibility = Visibility.Visible;
                                        CancelBatchButton.Visibility = Visibility.Visible;
                                        EnvelopeCountPanel.Visibility = Visibility.Visible;
                                    }
                                }
                            }
                            else
                            {
                                //voterlist = AddVoterToList(voter);
                                voterlist = voterlist.CombineLists(voter);

                                if (voterlist != null && voterlist.Count() > 0)
                                {
                                    TotalEnvelopeCount.Text = voterlist.Count().ToString();

                                    ProcessBallotsButton.Visibility = Visibility.Visible;
                                    CancelBatchButton.Visibility = Visibility.Visible;
                                    EnvelopeCountPanel.Visibility = Visibility.Visible;
                                }
                            }
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextLeft = ("Voter Not Found");
                            //VoterNotFoundPanel.Visibility = Visibility.Visible;
                            AlertDialog newMessage = new AlertDialog("VOTER NOT FOUND");
                            newMessage.ShowDialog();
                        }
                    }
                }
                else
                {
                    GlobalReferences.StatusBar.TextLeft = ("Voter Already In List");

                    AlertDialog newMessage = new AlertDialog("VOTER NUMBER " + voterID + " HAS ALREADY BEEN ADDED TO THE LIST");
                    newMessage.ShowDialog();
                }

                // Display the list
                VoterList.ItemsSource = voterlist;
                //StatusBar.ApplicationStatusCenter("Current Count: " + voterlist.Count().ToString());

            }
            else
            {
                GlobalReferences.StatusBar.TextLeft = ("Database not found");
            }

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
        private bool CheckIfVoterExists(string voterID)
        {
            bool result = false;
            foreach (var v in voterlist)
            {
                // Exit the list when voter is found
                if (v.Data.VoterID == voterID) return true;
            }
            return result;
        }

        private bool CheckIfVoterHasBeenScanned(string voterID)
        {
            bool result = false;
            using (var factory = new ScanHistoryFactory(((App)Application.Current).Connection))
            {
                if (Int32.TryParse(voterID, out int id))
                {
                    result = factory.CheckVoter(id);
                }
            }
            return result;
        }

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

        private void PrepareBatchButton_Click(object sender, RoutedEventArgs e)
        {
            //if (voterlist != null && voterlist.Count() > 0)
            //{
            //    // Popup message
            //    YesNoDialog newMessage = new YesNoDialog("Accepted Applications", "ARE YOU SURE YOU WANT TO MARK THESE APPLICATIONS ACCEPTED?");
            //    if (newMessage.ShowDialog() == true)
            //    {
            //        // Close Batch
            //        //BatchDataMethods.CloseBatch(_Batch.batch_id);

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

            //// Set focus back to scan box
            //Keyboard.Focus(BarCodeSearch);
        }

        private void CancelBatchButton_Click(object sender, RoutedEventArgs e)
        {
            if (VoterList.Items.Count > 0)
            { 
                YesNoDialog newMessage = new YesNoDialog("Cancel Batch", "THE CURRENT LIST WILL BE CLEARED\r\nAND THE VOTERS WILL NOT BE PROCESSED,\r\nARE YOU SURE YOU WANT TO CANCEL?");
                if (newMessage.ShowDialog() == true)
                {
                    //{
                    //    VoterList.ItemsSource = null;
                    //    voterlist = new ObservableCollection<VoterDataModel>();
                    //    LastBarCode.Text = "";

                    //    // Remove all voters from the current batch
                    //    BatchDataMethods.RemoveAllVotersFromBatch(_Batch.batch_id);
                    //}

                    //// Set focus back to scan box
                    //Keyboard.Focus(BarCodeSearch);

                    // Show Menu button
                    //MainMenuMethods.ShowHamburger();
                    GlobalReferences.Header.HamburgerMenuVisibility = true;

                    // Open Menu 
                    //MainMenuMethods.OpenMainMenu();
                    GlobalReferences.MenuSlider.Open();

                    this.NavigateToPage(new AbsenteeAdministrationPage());
                }
            }
            else
            {
                // Show Menu button
                //MainMenuMethods.ShowHamburger();
                GlobalReferences.Header.HamburgerMenuVisibility = true;

                // Open Menu 
                //MainMenuMethods.OpenMainMenu();
                GlobalReferences.MenuSlider.Open();

                this.NavigateToPage(new AbsenteeAdministrationPage());
            }
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
            if (VoterList.SelectedIndex >= 0)
            {
                NMVoter voter = (NMVoter)VoterList.SelectedItem;

                //BatchDataMethods.RemoveBatchedVoter(voter.VoterID.ToInt());

                voterlist.Remove(voter);
                VoterList.ItemsSource = voterlist;
                TotalEnvelopeCount.Text = voterlist.Count().ToString();

                LastBarCode.Text = "";

                RemoveVoterButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void ProcessBallotsButton_Click(object sender, RoutedEventArgs e)
        {
            if (voterlist.Count() > 0)
            {
                string rejecteditem = "";
                var processType = ((ComboBoxItem)LogCodeList.SelectedItem).Content.ToString();
                int logCodeId = ComboBoxMethods.GetSelectedItemNumber(LogCodeList);
                if (logCodeId > 0)
                {
                    if (logCodeId == 10)
                    {
                        // Get rejected reason
                        //rejecteditem = ComboBoxMethods.GetSelectedItemData<string>(RejectedReasonList);
                        var selectedIndex = RejectedReasonList.SelectedIndex;
                        var selectedValue = RejectedReasonList.SelectedValue;
                        rejecteditem = ((ComboBoxItem)RejectedReasonList.SelectedItem).DataContext.ToString();

                        if (rejecteditem == "")
                        {
                            AlertDialog errorMessage = new AlertDialog("PLEASE SELECTED A REJECTED REASON");
                            errorMessage.ShowDialog();

                            return;
                        }
                    }

                    //DateTime processDate = DateTime.Now;

                    //bool results = false;

                    string lastCount = voterlist.Count().ToString();
                    YesNoDialog newMessage = new YesNoDialog("Process Batch", "ARE YOU SURE YOU WANT TO PROCESS \r" + lastCount + " ENVELOPE(S) AS " + processType.ToUpper());
                    if (newMessage.ShowDialog() == true)
                    {
                        // Create scan history session
                        if (_session == null)
                        {
                            try
                            {
                                // Add voters to session
                                voterlist.AddToScanHistory(AppSettings.System.SiteID, AppSettings.System.MachineID, DateTime.Now, ((App)Application.Current).Connection);
                            }
                            catch (Exception error)
                            {
                                AlertDialog errorMessage = new AlertDialog("AN ERROR OCCURED DURING PROCESSING");
                                errorMessage.ShowDialog();

                                if (error.InnerException == null)
                                {
                                    GlobalReferences.StatusBar.TextCenter = ("Could not create session: " + error.Message);
                                }
                                else
                                {
                                    if (error.InnerException.InnerException == null)
                                    {
                                        GlobalReferences.StatusBar.TextCenter = ("Could not create session: " + error.InnerException.Message);
                                    }
                                    else
                                    {
                                        GlobalReferences.StatusBar.TextCenter = ("Could not create session: " + error.InnerException.InnerException.Message);
                                    }
                                }

                                return;
                            }
                        }                        

                        if (logCodeId != 10)
                        {
                            // Mark voted records as Returned Absentee Ballot
                            await Task.Run(() => voterlist.ReturnBallots(logCodeId));
                        }
                        else
                        {                            
                            await Task.Run(() => voterlist.RejectBallots(rejecteditem));
                        }

                        // Clear the counts
                        TotalEnvelopeCount.Text = "0";

                        CancelBatchButton.Visibility = Visibility.Collapsed;
                        FinishBatchButton.Visibility = Visibility.Visible;

                        LastBarCode.Text = "";

                        // Clear the list
                        voterlist = new ObservableCollection<NMVoter>();
                        VoterList.ItemsSource = null;

                        LogCodeList.SelectedIndex = 0;
                        RejectedReasonList.SelectedIndex = 0;

                        newMessage = new YesNoDialog("Process Batch", lastCount + " ENVELOPES HAVE BEEN PROCESSED AS " + processType.ToUpper() + ".\r\nDO YOU WANT TO SCAN MORE ENVELOPES?");
                        if (newMessage.ShowDialog() == false)
                        {
                            // Show Menu button
                            //MainMenuMethods.ShowHamburger();
                            GlobalReferences.Header.HamburgerMenuVisibility = true;

                            // Open Menu 
                            //MainMenuMethods.OpenMainMenu();
                            GlobalReferences.MenuSlider.Open();

                            this.NavigateToPage(new AbsenteeAdministrationPage());
                        }
                    }
                }
                else
                {
                    AlertDialog newMessage = new AlertDialog("NO STATUS HAS BEEN SELECTED");
                    newMessage.ShowDialog();
                }
            }
        }

        //private void ShowButtons()
        //{
        //    if (ReturnType == 1)
        //    {
        //        PrepareBatchButton.Visibility = Visibility.Visible;
        //        CancelBatchButton.Visibility = Visibility.Visible;
        //        ProcessBallotsButton.Visibility = Visibility.Collapsed;
        //    }

        //    if (ReturnType == 2)
        //    {
        //        PrepareBatchButton.Visibility = Visibility.Collapsed;
        //        CancelBatchButton.Visibility = Visibility.Collapsed;
        //        ProcessBallotsButton.Visibility = Visibility.Visible;
        //    }
        //}

        private void LogCodeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BarCodeSearch.Focus();

            int logCodeId = ComboBoxMethods.GetSelectedItemNumber(LogCodeList);
            if(logCodeId == 10)
            {
                RejectedReasonPanel.IsEnabled = true;
            }
            else
            {
                RejectedReasonPanel.IsEnabled = false;
            }
        }

        private void RejectedReasonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
