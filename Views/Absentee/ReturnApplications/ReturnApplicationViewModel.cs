using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using VoterX.Dialogs;
using VoterX.Methods;
using VoterX.Core.Voters;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Views;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Views.Absentee
{
    public class ReturnApplicationViewModel : NotifyPropertyChanged
    {
        public ReturnApplicationViewModel()
        {
            CanSearch = true;

            CanCancel = true;

            IsBarcodeFocused = true;

            // Display page header
            GlobalReferences.Header.PageHeader = ("Scan Returned Applications");
        }

        public bool IsBarcodeFocused { get; set; }

        #region VoterSearch
        public string BarCode
        {
            get
            {
                return BarcodeScanner.BarCode;
            }
            set
            {
                BarcodeScanner.BarCode = value;
            }
        }
        public string PreviousBarCode
        {
            get
            {
                return BarcodeScanner.LastBarCode;
            }
            set
            {
                BarcodeScanner.LastBarCode = value;
            }
        }

        public int EnvelopeCount { get; set; }

        // Bound command for looking up a voter
        public RelayCommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(param => this.SearchClick(), param => this.CanSearch);
                }
                return _searchCommand;
            }
        }

        // Enable or Disable the Search Voter control
        private bool _canSearch;
        public bool CanSearch
        {
            get { return _canSearch; }
            internal set
            {
                _canSearch = value;
                RaisePropertyChanged("CanSearch");
            }
        }

        private void SearchClick()
        {
            // Check for empty values
            if (BarCode != null && BarCode != "")
            {
                // Prevent double clicking the scanner
                CanSearch = false;

                if (PreviousBarCode != BarCode)
                {
                    if (VoterList != null)
                    {
                        // Check if the voter already exists in the list
                        if (!VoterList.IdExists(BarCode))
                        {
                            // Create the Voter Search Object (Search Parameters)
                            VoterSearchModel searchItems = new VoterSearchModel
                            {
                                VoterID = BarCode
                            };

                            // Get voter from barcode
                            var voters = VoterMethods.Voters.List(searchItems);

                            // Valid the voter
                            if (ValidateVoter(voters))
                            {
                                // Set local values
                                voters.Localize(
                                    AppSettings.Election.ElectionID,
                                    AppSettings.Absentee.SiteID,
                                    null,
                                    null);

                                // Add the voter to the list
                                VoterList.AddVoter(voters);

                                // Turn on Process Ballots button
                                if(VoterList != null && VoterList.VoterList.Count() > 0)
                                {
                                    CanProcessApplications = true;
                                }
                                else
                                {
                                    CanProcessApplications = false;
                                }
                            }
                        }
                        else
                        {
                            // Display the already added message
                            AlertDialog newMessage = new AlertDialog("VOTER NUMBER " + BarCode + " HAS ALREADY BEEN ADDED TO THE LIST");
                            newMessage.ShowDialog();
                        }
                    }
                }
                else
                {
                    // Display duplicate scan message
                    AlertDialog newMessage = new AlertDialog("DUPLICATE ID NUMBER " + BarCode + " WAS ENTERED");
                    newMessage.ShowDialog();
                }

                // Enable the input box
                CanSearch = true;

                // Have to set focus after textbox is enabled
                IsBarcodeFocused = true;
            }
        }

        private bool ValidateVoter(ObservableCollection<NMVoter> voters)
        {
            bool result = false;

            if (voters != null && voters.Count() > 0)
            {
                int? voterLogCode = voters.FirstOrDefault().Data.LogCode;

                // Check for issued applications                            
                if (voterLogCode != 3 && voterLogCode != 4)             // Added rejected applications 6/25/2018 at John's request
                {
                    if (voterLogCode == 17)
                    {
                        AlertDialog newMessage = new AlertDialog("THIS VOTER HAS BEEN DELETED OR REMOVED");
                        if (newMessage.ShowDialog() == true)
                        {

                        }
                    }
                    else if (voterLogCode < 3 || voterLogCode == null)
                    {
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
                        AlertDialog newMessage = new AlertDialog("THIS VOTER ALREADY HAS AN ACCEPTED ABSENTEE APPLICATION");
                        newMessage.ShowDialog();
                    }
                    else if (voterLogCode > 5)
                    {
                        AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                        newMessage.ShowDialog();
                    }
                }
                else if (voters.FirstOrDefault().IdRequired().Value)
                {
                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS AN ID REQUIRED AND MUST BE ACCEPTED MANUALLY");
                    newMessage.ShowDialog();
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("NO VOTER WAS FOUND WITH ID NUMBER: " + BarCode);
                newMessage.ShowDialog();

                // Clear the input box
                BarCode = "";
                RaisePropertyChanged("BarCode");
            }

            return result;
        }
        #endregion

        #region VoterList
        private VoterListViewModel _voterList;
        public VoterListViewModel VoterList
        {
            get
            {
                if(_voterList == null)
                {
                    _voterList = new VoterListViewModel(AppSettings.Global);

                    _voterList.SetLogCodeColors(Brushes.Black);

                    _voterList.PropertyChanged += OnVoterSelectedPropertyChanged;
                }
                return _voterList;
            }
            
            private set
            {
                _voterList = value;
            }
        }

        private void OnVoterSelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedVoter")
            {
                //Code to respond to a change in the ViewModel
                Console.WriteLine(
                    "Selected Voter: " +
                    ((VoterListViewModel)sender).SelectedVoter.Data.VoterID.ToString() + " | STATUS: " +
                    ((VoterListViewModel)sender).SelectedVoter.CheckStatus((int)AppSettings.System.SiteID));

                // Get selected voter
                var selectedVoter = ((VoterSearchViewModel)sender).SelectedVoter;

                // Display Delete Button
            }
            else if (e.PropertyName == "VoterList")
            {
                if (((VoterListViewModel)sender).VoterList != null)
                {
                    EnvelopeCount = ((VoterListViewModel)sender).VoterList.Count();
                    RaisePropertyChanged("EnvelopeCount");
                }
            }
        }
        #endregion

        #region BarcodeScanner
        private VoterSearchScanViewModel _barcodeScanner;
        public VoterSearchScanViewModel BarcodeScanner
        {
            get
            {
                if (_barcodeScanner == null)
                {
                    _barcodeScanner = new VoterSearchScanViewModel(false);

                    _barcodeScanner.PropertyChanged += OnSearchScanPropertyChanged;
                }
                return _barcodeScanner;
            }

            private set
            {
                _barcodeScanner = value;
            }
        }

        // Get Search Parameters from View
        private void OnSearchScanPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VoterSearch")
            {
                // Search for a voter
                SearchClick();
            }
        }
        #endregion

        #region ProcessApplications
        // Bound command for processing the applications
        public RelayCommand _processApplicationsCommand;
        public ICommand ProcessApplicationsCommand
        {
            get
            {
                if (_processApplicationsCommand == null)
                {
                    _processApplicationsCommand = new RelayCommand(
                        param => this.ProcessApplicationsClick(), 
                        param => this.CanProcessApplications);
                }
                return _processApplicationsCommand;
            }
        }

        // Enable or Disable the process applications control
        private bool _canProcessApplications;
        public bool CanProcessApplications
        {
            get { return _canProcessApplications; }
            internal set
            {
                _canProcessApplications = value;
                RaisePropertyChanged("CanProcessApplications");
            }
        }

        private async void ProcessApplicationsClick()
        {
            if (VoterList != null && VoterList.VoterList.Count() > 0)
            {
                string lastCount = VoterList.VoterList.Count().ToString();
                OkCancelDialog newMessage = new OkCancelDialog("Process Batch", "ACCEPT THE APPLICATION(S) FOR " + lastCount + " VOTER(S)? ");
                if (newMessage.ShowDialog() == true)
                {
                    // Set machine and user to null
                    VoterList.VoterList.Localize(
                        AppSettings.Election.ElectionID, 
                        (int)AppSettings.System.SiteID, 
                        null, 
                        null);

                    // Mark Voters as "Application Accepted"
                    await Task.Run(() => VoterList.VoterList.AcceptApplications(DateTime.Now));

                    YesNoDialog exitMessage = new YesNoDialog("Finish Batch", lastCount + " APPLICATION(S) HAS/HAVE BEEN ACCEPTED.\r\nWOULD YOU LIKE TO SCAN MORE APPLICATIONS?");
                    if (exitMessage.ShowDialog() == true)
                    {
                        //ProcessBallotsButton.Visibility = Visibility.Collapsed;

                        // Clear the list
                        VoterList.ClearList();

                        PreviousBarCode = "";
                        RaisePropertyChanged("PreviousBarCode");
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
                        //this.NavigateToPage(new VoterX.AbsenteeAdministrationPage());
                        NavigationMenuMethods.AbsenteeHome();
                    }
                }
            }
        }
        #endregion

        #region Cancel
        public RelayCommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(
                        param => this.CancelClick(),
                        param => this.CanCancel);
                }
                return _cancelCommand;
            }
        }

        private bool _canCancel;
        public bool CanCancel
        {
            get { return _canCancel; }
            internal set
            {
                _canCancel = value;
                RaisePropertyChanged("CanCancel");
            }
        }

        private void CancelClick()
        {            
            if (VoterList != null && VoterList.VoterList.Count() > 0)
            {
                OkCancelDialog newMessage = new OkCancelDialog("Cancel Batch", VoterList.VoterList.Count().ToString() + " APPLICATION(S) HAVE NOT BEEN SAVED\r\nDO YOU STILL WANT TO EXIT?");
                if (newMessage.ShowDialog() == true)
                {
                    // Show menu button
                    //MainMenuMethods.ShowHamburger();
                    GlobalReferences.Header.HamburgerMenuVisibility = true;

                    // Open Menu 
                    //MainMenuMethods.OpenMainMenu();
                    GlobalReferences.MenuSlider.Open();

                    // Return to Absentee Menu page
                    //this.NavigateToPage(new VoterX.AbsenteeAdministrationPage());
                    NavigationMenuMethods.AbsenteeHome();
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
                //this.NavigateToPage(new VoterX.AbsenteeAdministrationPage());
                NavigationMenuMethods.AbsenteeHome();
            }
        }
        #endregion
    }
}
