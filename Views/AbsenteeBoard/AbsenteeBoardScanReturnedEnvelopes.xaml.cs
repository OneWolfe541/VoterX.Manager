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
using VoterX.Core.AbsenteeBoard;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeScanReturns.xaml
    /// </summary>
    public partial class AbsenteeBoardScanReturnedEnvelopes : Page
    {
        private ObservableCollection<NMVoter> voterlist = new ObservableCollection<NMVoter>();

        //private int ReturnType = 0;
        //private int BatchType = 0;

        //private avBatch _Batch = new avBatch();

        private ScanHistory _session;

        public AbsenteeBoardScanReturnedEnvelopes()
        {
            InitializeComponent();

            BarCodeSearch.Focus();

            SetReturnType();

            LoadLogCodes();

            SetLocationsDisplay();

            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);
        }

        private void SetReturnType()
        {
            GlobalReferences.Header.PageHeader = ("Absentee Board Scan");
            BallotReturnTypePanel.Visibility = Visibility.Visible;
            SearchListBorder.Margin = new Thickness(0,63,0,10);
        }

        // Clear the list view object
        private void ClearListView()
        {
            VoterList.ItemsSource = null;
        }

        private void SetLocationsDisplay()
        {
            // John wanted to disable locations if they are not required (7/15/2020)
            //if(AppSettings.Absentee.BoardLocationRequired == true)
            //{
            //    LocationsLabel.Visibility = Visibility.Visible;
            //    Location1Label.Visibility = Visibility.Visible;
            //    Location1.Visibility = Visibility.Visible;
            //    Location2Label.Visibility = Visibility.Visible;
            //    Location2.Visibility = Visibility.Visible;
            //}

            LocationsLabel.Visibility = Visibility.Visible;
            Location1Label.Visibility = Visibility.Visible;
            Location1.Visibility = Visibility.Visible;
            Location2Label.Visibility = Visibility.Visible;
            Location2.Visibility = Visibility.Visible;
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

            List<int> codeList = new List<int> { 9, 10, 11, 12, 14 };

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
            SearchingPanel.Visibility = Visibility.Visible;

            if (await Task.Run(() => VoterMethods.Exists) == true)
            {
                // Create the Voter Search Object (Search Parameters)
                VoterSearchModel searchItems = new VoterSearchModel
                {
                    VoterID = voterID
                };

                if (AppSettings.Absentee.BoardLocationRequired == true && (Location1.Text == null || Location1.Text == ""))
                {
                    AlertDialog newMessage = new AlertDialog("LOCATION CANNOT BE BLANK");
                    newMessage.ShowDialog();

                    LastBarCode.Text = "";
                }
                // Check for previous scans
                else if (CheckIfVoterHasBeenScanned(voterID) == true)
                {
                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN SCANNED");
                    newMessage.ShowDialog();
                }
                // Prevent duplicate scans
                else if (CheckIfVoterExists(voterID) != true)
                {
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
                            int? voterLogCode = voter.FirstOrDefault().Data.LogCode;

                            // Get Selected LogCode
                            int selectedLogCodeId = ComboBoxMethods.GetSelectedItemNumber(LogCodeList);

                            // Add voter to current list if their log code is 6
                            if (voterLogCode != selectedLogCodeId)
                            {
                                if (voterLogCode == 17)
                                {
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS BEEN DELETED OR REMOVED");
                                    newMessage.ShowDialog();
                                }
                                else if (voterLogCode < 6)
                                {
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS NOT BEEN ISSUED A BALLOT");
                                    newMessage.ShowDialog();
                                }                                
                                else if (voterLogCode != selectedLogCodeId)
                                {
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER DOES NOT MATCH THE SELECTED STATUS");
                                    newMessage.ShowDialog();

                                    LastBarCode.Text = "";
                                }                                
                            }
                            //else if (voterLogCode == 9 && voter.FirstOrDefault().Data.VotedIdRequired == true)
                            //{
                            //    AlertDialog newMessage = new AlertDialog("THIS VOTER REQUIRES A VALID ID");
                            //    newMessage.ShowDialog();
                            //}
                            else
                            {
                                YesNoDialog newMessage = new YesNoDialog("Id Required", "THIS VOTER REQUIRES A VALID ID\r\nARE YOU SURE YOU WANT TO CONTINUE?");
                                if (newMessage.ShowDialog() == true)
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

                                // Disable Log Code Selection
                                LogCodeList.IsEnabled = false;
                                Location1.IsEnabled = false;
                                Location2.IsEnabled = false;
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

            }
            else
            {
                GlobalReferences.StatusBar.TextLeft = ("Database not found");
            }

            // Turn off the progress spinner
            SearchingPanel.Visibility = Visibility.Collapsed;

            // Set focus back to scan box
            Keyboard.Focus(BarCodeSearch);
        }

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
            using (AbsenteeBoardFactory factory = new AbsenteeBoardFactory(((App)Application.Current).Connection))
            {
                // Convert Voter Id to int
                if (Int32.TryParse(voterID, out int id))
                {
                    // Get voter from Absentee Board
                    var voterBoard = factory.VoterSearch(id);

                    // Return True if voter exists
                    result = voterBoard != null;
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

        private void CancelBatchButton_Click(object sender, RoutedEventArgs e)
        {
            if (VoterList.Items.Count > 0)
            { 
                YesNoDialog newMessage = new YesNoDialog("Cancel Batch", "THE CURRENT LIST WILL BE CLEARED\r\nAND THE VOTERS WILL NOT BE PROCESSED,\r\nARE YOU SURE YOU WANT TO CANCEL?");
                if (newMessage.ShowDialog() == true)
                {
                    // Show Menu button
                    GlobalReferences.Header.HamburgerMenuVisibility = true;

                    // Open Menu 
                    GlobalReferences.MenuSlider.Open();

                    this.NavigateToPage(new AbsenteeBoardPage());
                }
            }
            else
            {
                // Show Menu button
                GlobalReferences.Header.HamburgerMenuVisibility = true;

                // Open Menu 
                GlobalReferences.MenuSlider.Open();

                this.NavigateToPage(new AbsenteeBoardPage());
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                RemoveVoterButton.Visibility = Visibility.Visible;
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
                var processType = ((ComboBoxItem)LogCodeList.SelectedItem).Content.ToString();
                int logCodeId = ComboBoxMethods.GetSelectedItemNumber(LogCodeList);
                if (logCodeId > 0)
                {
                    DateTime processDate = DateTime.Now;

                    string lastCount = voterlist.Count().ToString();
                    YesNoDialog newMessage = new YesNoDialog("Process Batch", "ARE YOU SURE YOU WANT TO PROCESS \r" + lastCount + " ENVELOPE(S) AS " + processType.ToUpper());
                    if (newMessage.ShowDialog() == true)
                    {                        
                        try
                        {                                
                            // Add to Absentee Board
                            voterlist.AddToAbsenteeBoard(AppSettings.System.SiteID, AppSettings.System.MachineID, processDate, Location1.Text, Location2.Text, ((App)Application.Current).Connection);
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

                        // Clear the counts
                        TotalEnvelopeCount.Text = "0";

                        CancelBatchButton.Visibility = Visibility.Collapsed;
                        FinishBatchButton.Visibility = Visibility.Visible;

                        LastBarCode.Text = "";

                        // Clear the list
                        voterlist = new ObservableCollection<NMVoter>();
                        VoterList.ItemsSource = null;

                        LogCodeList.SelectedIndex = 0;

                        LogCodeList.IsEnabled = true;
                        Location1.IsEnabled = true;
                        Location2.IsEnabled = true;

                        newMessage = new YesNoDialog("Process Batch", lastCount + " ENVELOPES HAVE BEEN PROCESSED AS " + processType.ToUpper() + ".\r\nDO YOU WANT TO SCAN MORE ENVELOPES?");
                        if (newMessage.ShowDialog() == false)
                        {
                            // Show Menu button
                            GlobalReferences.Header.HamburgerMenuVisibility = true;

                            // Open Menu 
                            GlobalReferences.MenuSlider.Open();

                            this.NavigateToPage(new AbsenteeBoardPage());
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

        private void LogCodeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LastBarCode.Text = "";

            BarCodeSearch.Focus();
        }
    }
}
