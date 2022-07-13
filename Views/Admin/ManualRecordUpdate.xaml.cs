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
using VoterX.ABS.Logging;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeScanReturns.xaml
    /// </summary>
    public partial class ManualRecordUpdate : Page
    {
        private ObservableCollection<NMVoter> voterlist = new ObservableCollection<NMVoter>();

        //private int ReturnType = 0;
        //private int BatchType = 0;

        //private avBatch _Batch = new avBatch();

        public ManualRecordUpdate()
        {
            InitializeComponent();

            BarCodeSearch.Focus();

            SetReturnType();

            // Check for existing batch

            //LoadCurrentBatch(BatchType);

            LoadLogCodes(LogCodeListChangeFrom);

            LoadLogCodes(LogCodeListChangeTo, true);

            // Hide Menu
            //MainMenuMethods.HideHamburger();
            GlobalReferences.Header.HamburgerMenuVisibility = false;

            //StatusBar.ApplicationCheckPrinterAsync();
            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter );

            CheckServer();
        }

        private async void CheckServer()
        {
            await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(100);
        }

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
                    GlobalReferences.Header.PageHeader = ("Manual Record Update");
                    BallotReturnTypePanel.Visibility = Visibility.Visible;
                    SearchListBorder.Margin = new Thickness(0,138,0,10);
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

        private void LoadLogCodes(ComboBox sender)
        {
            LoadLogCodes(sender, false);
        }

        private async void LoadLogCodes(ComboBox sender, bool alt)
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem(sender, TempLoadingSpinnerItem);

            List<int> codeList = new List<int> { 9, 10, 12, 14 };

            if (alt == true)
            {
                codeList = new List<int> { 9, 12, 14 };
            }
            //List<int> codeList = new List<int> { 2, 3, 4, 6, 8, 9, 10, 12, 14, 16, 17 };

            if (await Task.Run(() => VoterMethods.Exists) == true)
            {
                // Blank record added in order to force the user to select an option (08/08/2018)
                ComboBoxMethods.AddComboItemToControl(
                        sender,
                        0,
                        "",
                        0
                        );

                foreach (var logcodelist in await Task.Run(() => ElectionDataMethods.LogCodes.Where(l => codeList.Contains(l.LogCode)).ToList()))
                {
                    ComboBoxMethods.AddComboItemToControl(
                        sender,
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

            ComboBoxMethods.RemoveListItem(sender, loadingItem);
        }


        private async void ProcessSingleVoter(string voterID)
        {
            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            VoterNotFoundPanel.Visibility = Visibility.Collapsed;
            //StatusBar.ApplicationStatusClear();
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

                // Prevent duplicate scans
                if (CheckIfVoterExists(voterID) != true)
                {
                    //StatusBar.ApplicationStatus("Voter Not In List");

                    //var voter = await VoterMethods.Voter.SingleAsync(searchItems, AppSettings.Election.ElectionType);
                    var voter = VoterMethods.Voters.List(searchItems);

                    // OR
                    // Load voters from session

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

                            int? voterLogCode = voter.FirstOrDefault().Data.LogCode;

                            var processTypeFrom = ((ComboBoxItem)LogCodeListChangeFrom.SelectedItem).Content.ToString();
                            int logCodeIdFrom = ComboBoxMethods.GetSelectedItemNumber(LogCodeListChangeFrom);

                            // Add voter to current list if their log code matches selected log code
                            if (voterLogCode != logCodeIdFrom)
                            {
                                if (voterLogCode == 17)
                                {
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS BEEN DELETED OR REMOVED");
                                    newMessage.ShowDialog();
                                }
                                else
                                {
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER'S CURRENT STATUS DOES NOT MATCH THE \r\nFROM SELECTION: " + processTypeFrom);
                                    newMessage.ShowDialog();
                                }

                                LastBarCode.Text = "";
                            }
                            else
                            {
                                // Check Scan Session is on
                                if (SessionCheck.IsChecked == true)
                                {
                                    // Get session data
                                    var factory = new ScanHistoryFactory();
                                    var session = factory.GetSession(voter.FirstOrDefault().Data);

                                    if (session != null && session.Data != null && session.Data.SessionLocked != true)
                                    {
                                        // Display Session Details
                                        //SessionNumber.Text = session.Data.SessionId.ToString();
                                        SessionDate.Text = session.Data.SessionDate.ToString();

                                        // Load Voters from Session
                                        voterlist = VoterMethods.Voters.List(session);
                                    }
                                    else
                                    {
                                        AlertDialog newMessage = new AlertDialog("VOTER RECORDS IN THIS SESSION ARE NOT CONSISTANT!\r\nONE OR MORE RECORDS HAVE BEEN CHANGED,\r\nANY FURTHER UPDATES MUST BE MADE INIDIVUALLY");
                                        if (newMessage.ShowDialog() == true)
                                        {
                                            // SET SCREEN BACK TO DEFAULT STATE

                                            // Turn of Scan Check
                                            SessionCheck.IsChecked = false;

                                            // Hide session details
                                            SessionMessage.Visibility = Visibility.Collapsed;
                                            SessionDetailLabel.Visibility = Visibility.Collapsed;
                                            //SessionNumberLabel.Visibility = Visibility.Collapsed;
                                            //SessionNumber.Visibility = Visibility.Collapsed;
                                            SessionDateLabel.Visibility = Visibility.Collapsed;
                                            SessionDate.Visibility = Visibility.Collapsed;

                                            // Clear last scanned
                                            LastBarCode.Text = "";

                                            // Turn off the progress spinner
                                            SearchingPanel.Visibility = Visibility.Collapsed;

                                            // Set focus back to scan box
                                            Keyboard.Focus(BarCodeSearch);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    //voterlist = AddVoterToList(voter);
                                    voterlist = voterlist.CombineLists(voter);

                                } // End Session check

                                // Display processing buttons
                                if (voterlist != null && voterlist.Count() > 0)
                                {
                                    TotalEnvelopeCount.Text = voterlist.Count().ToString();

                                    ProcessBallotsButton.Visibility = Visibility.Visible;
                                    CancelBatchButton.Visibility = Visibility.Visible;
                                    EnvelopeCountPanel.Visibility = Visibility.Visible;
                                }

                                // Lock the From log code list until processed or canceled
                                LogCodeListChangeFrom.IsEnabled = false;
                                SessionCheck.IsEnabled = false;

                            } // End Log Code check
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextLeft = ("Voter Not Found");
                            VoterNotFoundPanel.Visibility = Visibility.Visible;
                        }
                    }

                }
                else
                {
                    GlobalReferences.StatusBar.TextLeft = ("Voter Already In List");
                }

                // Display the list
                VoterList.ItemsSource = voterlist;
                //StatusBar.ApplicationStatusCenter("Current Count: " + voterlist.Count().ToString());

            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
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

                    this.NavigateToPage(new AdministrationPage());
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

                this.NavigateToPage(new AdministrationPage());
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && SessionCheck.IsChecked == false)
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

                if (voterlist != null)
                {
                    TotalEnvelopeCount.Text = voterlist.Count().ToString();
                }

                RemoveVoterButton.Visibility = Visibility.Collapsed;
            }

        }

        private async void ProcessBallotsButton_Click(object sender, RoutedEventArgs e)
        {
            if (voterlist.Count() > 0)
            {
                var fromProcessType = ((ComboBoxItem)LogCodeListChangeFrom.SelectedItem).Content.ToString();
                int fromLogCodeId = ComboBoxMethods.GetSelectedItemNumber(LogCodeListChangeFrom);

                var processType = ((ComboBoxItem)LogCodeListChangeTo.SelectedItem).Content.ToString();                
                int logCodeId = ComboBoxMethods.GetSelectedItemNumber(LogCodeListChangeTo);

                int? ballotRejectedReasonId = null;

                // Check if From & To match
                if (fromLogCodeId == logCodeId)
                {
                    AlertDialog newMessage = new AlertDialog("CANNOT CHANGE STATUS FROM " + fromProcessType.ToUpper() + " \r\nTO " + processType.ToUpper()); 
                    newMessage.ShowDialog();
                }
                else if (logCodeId > 0)
                {
                    if (ChangeReasonBox.Text == "")
                    {
                        AlertDialog newMessage = new AlertDialog("PLEASE ENTER A REASON FOR THIS STATUS CHANGE");
                        newMessage.ShowDialog();
                    }
                    else
                    {
                        if(logCodeId == 10)
                        {
                            BallotRejectedDialog rejectedDialog = new BallotRejectedDialog("SELECTED THE REJECTED BALLOT REASON");
                            if(rejectedDialog.ShowDialog() == true)
                            {
                                ballotRejectedReasonId = rejectedDialog.Reason;
                            }
                        }

                        DateTime processDate = DateTime.Now;

                        //bool results = false;

                        string lastCount = voterlist.Count().ToString();
                        YesNoDialog newMessage = new YesNoDialog("Process Batch", "ARE YOU SURE YOU WANT TO PROCESS \r" + lastCount + " ENVELOPES AS " + processType.ToUpper());
                        if (newMessage.ShowDialog() == true)
                        {
                            string auditReason = ChangeReasonBox.Text;
                            //results = await VoterMethods.UpdateManualLogCodeChangeList(voterlist.ToList(), processDate, logCodeId, auditReason);
                            //await Task.Run(()=> voterlist.ChangeStatus(processDate, logCodeId, auditReason));
                            //await Task.Run(() => ChangeStatus(processDate, logCodeId, auditReason, ballotRejectedReasonId));
                            ChangeStatus(processDate, logCodeId, auditReason, ballotRejectedReasonId);

                            // IF IN SINGLE MODE LOCK ALL SESSIONS
                            if (SessionCheck.IsChecked == false)
                            {
                                try
                                {
                                    // Get session data
                                    var factory = new ScanHistoryFactory();
                                    factory.LockSession(voterlist);
                                }
                                catch (Exception error)
                                {
                                    //GlobalReferences.StatusBar.TextCenter = error.Message;

                                    var searchlog = new VoterXLogger("ABSlogs", true);
                                    searchlog.WriteLog("Load Voter List Failed: " + error.Message);
                                    if (error.InnerException != null)
                                    {
                                        searchlog.WriteLog(error.InnerException.Message);
                                    }
                                }
                            }

                            TotalEnvelopeCount.Text = "0";

                            CancelBatchButton.Visibility = Visibility.Collapsed;
                            FinishBatchButton.Visibility = Visibility.Visible;

                            LastBarCode.Text = "";

                            voterlist = new ObservableCollection<NMVoter>();
                            VoterList.ItemsSource = null;

                            AlertDialog message = new AlertDialog(lastCount + " ENVELOPE(S) HAVE BEEN PROCESSED AS " + processType.ToUpper() + ".");                            
                            if (message.ShowDialog() == true)
                            {
                                // Show Menu button
                                //MainMenuMethods.ShowHamburger();
                                GlobalReferences.Header.HamburgerMenuVisibility = true;

                                // Open Menu 
                                //MainMenuMethods.OpenMainMenu();
                                GlobalReferences.MenuSlider.Open();

                                this.NavigateToPage(new AdministrationPage());
                            }

                            LogCodeListChangeFrom.IsEnabled = true;

                            LogCodeListChangeFrom.SelectedIndex = 0;
                            LogCodeListChangeTo.SelectedIndex = 0;

                            ChangeReasonBox.Text = "";
                        }
                    }
                }
                else
                {
                    AlertDialog newMessage = new AlertDialog("NO STATUS WAS SELECTED TO CHANGE THE RECORD(S) TO");
                    newMessage.ShowDialog();
                }
            }
        }

        private void ChangeStatus(DateTime ReceivedDate, int LogCode, string ChangeReason, int? RejectedReasonId)
        {
            try
            {
                if (RejectedReasonId == null)
                {
                    voterlist.ChangeStatus(ReceivedDate, LogCode, ChangeReason);
                }
                else
                {
                    voterlist.ChangeStatus(ReceivedDate, LogCode, ChangeReason, RejectedReasonId);
                }
            }
            catch (Exception e)
            {
                // Log Exception
                GlobalReferences.StatusBar.TextCenter = (e.Message);
                VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                ABSLogger.WriteLog("Error Issuing Ballot: " + e.Message);
                if (e.InnerException != null)
                {
                    //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                    ABSLogger.WriteLog(e.InnerException.Message);
                    if (e.InnerException.InnerException != null)
                    {
                        ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                    }
                }

                return;
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
        }

        private void SessionCheck_Click(object sender, RoutedEventArgs e)
        {
            if (SessionCheck.IsChecked == true)
            {
                // Show session details
                SessionMessage.Visibility = Visibility.Visible;
                SessionDetailLabel.Visibility = Visibility.Visible;
                //SessionNumberLabel.Visibility = Visibility.Visible;
                //SessionNumber.Visibility = Visibility.Visible;
                SessionDateLabel.Visibility = Visibility.Visible;
                SessionDate.Visibility = Visibility.Visible;
            }
            else
            {
                // Hide session details
                SessionMessage.Visibility = Visibility.Collapsed;
                SessionDetailLabel.Visibility = Visibility.Collapsed;
                //SessionNumberLabel.Visibility = Visibility.Collapsed;
                //SessionNumber.Visibility = Visibility.Collapsed;
                SessionDateLabel.Visibility = Visibility.Collapsed;
                SessionDate.Visibility = Visibility.Collapsed;
            }

            BarCodeSearch.Focus();
        }
    }
}
