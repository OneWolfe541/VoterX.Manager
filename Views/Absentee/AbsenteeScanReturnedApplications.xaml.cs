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
    /// <summary>
    /// Interaction logic for AbsenteeScanReturns.xaml
    /// </summary>
    public partial class AbsenteeScanReturnedApplications : Page
    {
        private ObservableCollection<NMVoter> voterlist = new ObservableCollection<NMVoter>();

        //private int ReturnType = 0;
        //private int BatchType = 0;

        //private avBatch _Batch = new avBatch();

        public AbsenteeScanReturnedApplications()
        {
            InitializeComponent();

            BarCodeSearch.Focus();

            //SetReturnType(returnType);

            // Check for existing batch

            //LoadCurrentBatch(BatchType);

            //LoadLogCodes();

            // Display page header
            GlobalReferences.Header.PageHeader = ("Scan Returned Applications");

            // Hide Menu
            //MainMenuMethods.HideHamburger();
            //GlobalReferences.Header.HamburgerMenuVisibility = false;

            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            //StatusBar.ApplicationCheckSignaturePad();

            CheckServer();
        }

        private async void CheckServer()
        {
            await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(100);
        }

        // Clear the list view object
        private void ClearListView()
        {
            VoterList.ItemsSource = null;
        }

        private async void BarCodeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                // Last keypress remains in the buffer and needs to be cleared
                e.Handled = true;

                ((TextBox)sender).IsEnabled = false;

                // Prevent Spam Scans
                if (LastBarCode.Text != BarCodeSearch.Text)
                {
                    var done = await ProcessSingleVoter(BarCodeSearch.Text);
                }
                else
                {
                    AlertDialog newMessage = new AlertDialog("DUPLICATE ID NUMBER " + BarCodeSearch.Text + " WAS ENTERED");
                    newMessage.ShowDialog();
                }

                if (voterlist.Count() > 0)
                {
                    ProcessBallotsButton.Visibility = Visibility.Visible;

                    TotalEnvelopeCount.Text = voterlist.Count().ToString();

                    EnvelopeCountPanel.Visibility = Visibility.Visible;
                }

                LastBarCode.Text = BarCodeSearch.Text;

                BarCodeSearch.Text = "";

                ((TextBox)sender).IsEnabled = true;

                // Have to set focus after textbox is enabled
                Keyboard.Focus(BarCodeSearch);
            }
        }

        private void BarCodeSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            //Keyboard.Focus(BarCodeSearch);
        }

        private async Task<bool> ProcessSingleVoter(string voterID)
        {
            bool result = false;

            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
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

                    var voter = VoterMethods.Voters.List(searchItems);

                    string errorMessage = voter.CheckForErrors();
                    if (errorMessage != null)
                    {
                        GlobalReferences.StatusBar.TextCenter = ("SQL ERROR: " + errorMessage);
                    }
                    else
                    {
                        // Set local values
                        voter.Localize(
                            AppSettings.Election.ElectionID,
                            AppSettings.Absentee.SiteID,
                            null,
                            null);

                        if (voter != null && voter.Count() > 0)
                        {
                            //StatusBar.ApplicationStatus("Voter Found In Database");

                            int? voterLogCode = voter.FirstOrDefault().Data.LogCode;
                            // Check for issued applications                            
                            if (voterLogCode != 3 && voterLogCode != 4)             // Added rejected applications 6/25/2018 at John's request
                            {
                                if (voterLogCode == 17)
                                {
                                    //StatusBar.ApplicationStatusCenter("THIS VOTER HAS BEEN DELETED OR REMOVED");
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS BEEN DELETED OR REMOVED");
                                    if(newMessage.ShowDialog()==true)
                                    {

                                    }
                                }
                                else if (voterLogCode < 3 || voterLogCode == null)
                                {
                                    //StatusBar.ApplicationStatusCenter("THIS VOTER HAS NOT BEEN ISSUED AN ABSENTEE APPLICATION");
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS NOT BEEN ISSUED AN ABSENTEE APPLICATION");
                                    newMessage.ShowDialog();
                                }
                                // Removed a John's request 06/25/2018
                                //else if (voterLogCode == 4)
                                //{
                                //    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS A REJECTED ABSENTEE APPLICATION");
                                //    newMessage.ShowDialog();
                                //}
                                else if (voterLogCode == 5)
                                {
                                    //StatusBar.ApplicationStatusCenter("THIS VOTER ALREADY HAS AN ACCEPTED ABSENTEE APPLICATION");
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER ALREADY HAS AN ACCEPTED ABSENTEE APPLICATION");
                                    newMessage.ShowDialog();
                                }
                                else if (voterLogCode > 5)
                                {
                                    //
                                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                                    newMessage.ShowDialog();
                                }
                            }
                            else if (voter.FirstOrDefault().IdRequired().Value)
                            {
                                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS AN ID REQUIRED AND MUST BE ACCEPTED MANUALLY");
                                newMessage.ShowDialog();
                            }
                            else
                            {
                                //voterlist = AddVoterToList(voter);
                                voterlist = voterlist.CombineLists(voter);

                                // Mark Voters as "Application Accepted"
                                //var result = await VoterMethods.UpdateApplicationAccepted(voter.FirstOrDefault(), DateTime.Now);                                

                                result = true;
                            }
                        }
                        else
                        {
                            AlertDialog newMessage = new AlertDialog("NO VOTER WAS FOUND WITH ID NUMBER: " + voterID);
                            newMessage.ShowDialog();
                            GlobalReferences.StatusBar.TextLeft = ("Voter Not Found");
                        }
                    }

                }
                else
                {
                    AlertDialog newMessage = new AlertDialog("VOTER NUMBER " + voterID + " HAS ALREADY BEEN ADDED TO THE LIST");
                    newMessage.ShowDialog();
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

            return result;
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

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //var item = sender as ListViewItem;
            //if (item != null)
            //{
            //    RemoveVoterButton.Visibility = Visibility.Visible;
            //    StatusBar.ApplicationStatus(((VoterDataModel)item.DataContext).VoterID.ToString());
            //}
            //else
            //{
            //    RemoveVoterButton.Visibility = Visibility.Collapsed;
            //}
        }

        private void ListViewItem_LostFocus(object sender, RoutedEventArgs e)
        {
            //RemoveVoterButton.Visibility = Visibility.Collapsed;
        }

        private void RemoveVoterButton_Click(object sender, RoutedEventArgs e)
        {
            //if(VoterList.SelectedIndex >= 0)
            //{
            //    VoterDataModel voter = (VoterDataModel)VoterList.SelectedItem;

            //    BatchDataMethods.RemoveBatchedVoter(voter.VoterID.ToInt());

            //    voterlist.Remove(voter);
            //    VoterList.ItemsSource = voterlist;

            //    RemoveVoterButton.Visibility = Visibility.Collapsed;
            //}
            
        }

        private void ScanDoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (voterlist.Count() > 0)
            {
                YesNoDialog newMessage = new YesNoDialog("Process Batch", voterlist.Count().ToString() + " APPLICATIONS HAVE BEEN SCANNED\rDOES THIS MATCH YOUR APPLICATION COUNT?");
                if (newMessage.ShowDialog() == true)
                {
                    // Show menu button
                    //MainMenuMethods.ShowHamburger();
                    GlobalReferences.Header.HamburgerMenuVisibility = true;

                    // Open Menu 
                    //MainMenuMethods.OpenMainMenu();
                    GlobalReferences.MenuSlider.Open();

                    // Return to Absentee Menu page
                    this.NavigateToPage(new AbsenteeAdministrationPage());
                }
            }
            else
            {
                // Show menu button
                //MainMenuMethods.ShowHamburger();
                GlobalReferences.Header.HamburgerMenuVisibility = true;

                // Open Menu 
                //MainMenuMethods.OpenMainMenu();
                GlobalReferences.MenuSlider.Open();

                // Return to Absentee Menu page
                this.NavigateToPage(new AbsenteeAdministrationPage());
            }
        }

        private void CancelBatchButton_Click(object sender, RoutedEventArgs e)
        {
            if (voterlist.Count() > 0)
            {
                OkCancelDialog newMessage = new OkCancelDialog("Cancel Batch", voterlist.Count().ToString() + " APPLICATION(S) HAVE NOT BEEN SAVED\r\nDO YOU STILL WANT TO EXIT?");
                if (newMessage.ShowDialog() == true)
                {
                    // Show menu button
                    //MainMenuMethods.ShowHamburger();
                    GlobalReferences.Header.HamburgerMenuVisibility = true;

                    // Open Menu 
                    //MainMenuMethods.OpenMainMenu();
                    GlobalReferences.MenuSlider.Open();

                    // Return to Absentee Menu page
                    this.NavigateToPage(new AbsenteeAdministrationPage());
                }
            }
            else
            {
                // Show menu button
                //MainMenuMethods.ShowHamburger();
                GlobalReferences.Header.HamburgerMenuVisibility = true;

                // Open Menu 
                //MainMenuMethods.OpenMainMenu();
                GlobalReferences.MenuSlider.Open();

                // Return to Absentee Menu page
                this.NavigateToPage(new AbsenteeAdministrationPage());
            }
        }

        private async void ProcessBallotsButton_Click(object sender, RoutedEventArgs e)
        {
            if (voterlist.Count() > 0)
            {
                string lastCount = voterlist.Count().ToString();
                OkCancelDialog newMessage = new OkCancelDialog("Process Batch", "ACCEPT THE APPLICATION(S) FOR " + lastCount + " VOTER(S)? ");
                if (newMessage.ShowDialog() == true)
                {
                    // Set machine and user to null
                    voterlist.Localize(
                        AppSettings.Election.ElectionID,
                        (int)AppSettings.Absentee.SiteID,
                        AppSettings.System.MachineID,
                        null);

                    // Mark Voters as "Application Accepted"
                    await Task.Run(() => voterlist.AcceptApplications(DateTime.Now) );

                    YesNoDialog exitMessage = new YesNoDialog("Finish Batch", lastCount + " APPLICATION(S) HAS/HAVE BEEN ACCEPTED.\r\nWOULD YOU LIKE TO SCAN MORE APPLICATIONS?");
                    if (exitMessage.ShowDialog() == true)
                    {
                        ProcessBallotsButton.Visibility = Visibility.Collapsed;

                        // Clear the list
                        voterlist = new ObservableCollection<NMVoter>();
                        VoterList.ItemsSource = null;

                        TotalEnvelopeCount.Text = "0";
                        EnvelopeCountPanel.Visibility = Visibility.Collapsed;

                        LastBarCode.Text = "";
                    }
                    else
                    {
                        // Show menu button
                        //MainMenuMethods.ShowHamburger();
                        GlobalReferences.Header.HamburgerMenuVisibility = true;

                        // Open Menu 
                        //MainMenuMethods.OpenMainMenu();
                        GlobalReferences.MenuSlider.Open();

                        // Return to Absentee Menu page
                        this.NavigateToPage(new AbsenteeAdministrationPage());
                    }
                }
            }
        }
    }
}
