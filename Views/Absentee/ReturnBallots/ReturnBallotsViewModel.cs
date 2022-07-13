using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using VoterX;
using VoterX.Dialogs;
using VoterX.Methods;
using VoterX.Core.Elections;
using VoterX.Core.Voters;
using VoterX.SystemSettings.Extensions;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Views;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Views.Absentee
{
    public class ReturnBallotsViewModel : NotifyPropertyChanged
    {
        public ReturnBallotsViewModel()
        {
            CanSearch = true;

            CanCancel = true;

            IsBarcodeFocused = true;

            RemoveVoterVisibility = false;
            CanRemoveVoter = false;

            // Display page header
            GlobalReferences.Header.PageHeader = ("Scan Returned Envelopes");
        }

        public bool IsBarcodeFocused
        {
            get
            {
                return BarcodeScanner.IsBarcodeFocused;
            }
            set
            {
                BarcodeScanner.IsBarcodeFocused = value;
            }
        }

        #region VoterSearch
        // Link the barcode from the scanner control
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

        // Link the previous barcode from the scanner control
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

        // Search the database for the voter and add them to the list control
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
                                    AppSettings.System.MachineID,
                                    0);

                                // Set date and time the voter was scanned
                                voters.FirstOrDefault().Data.ActivityDate = DateTime.Now;

                                // Add the voter to the list
                                VoterList.AddVoter(voters);

                                // Turn on Process Ballots button
                                if (VoterList != null && VoterList.VoterList.Count() > 0)
                                {
                                    CanProcessBallots = true;
                                }
                                else
                                {
                                    CanProcessBallots = false;
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

        // Check if the scanned voter has a valid issed ballot
        private bool ValidateVoter(ObservableCollection<NMVoter> voters)
        {
            bool result = false;

            if (voters != null && voters.Count() > 0)
            {
                int? voterLogCode = voters.FirstOrDefault().Data.LogCode;

                // Check for issued ballots                            
                if (voterLogCode != 6)
                {
                    if (voterLogCode == 17)
                    {
                        AlertDialog newMessage = new AlertDialog("THIS VOTER HAS BEEN DELETED OR REMOVED");
                        if (newMessage.ShowDialog() == true)
                        {

                        }
                    }
                    else if (voterLogCode < 6 || voterLogCode == null)
                    {
                        AlertDialog newMessage = new AlertDialog("THIS VOTER HAS NOT BEEN ISSUED A BALLOT");
                        newMessage.ShowDialog();
                    }
                    else if (voterLogCode > 6)
                    {
                        AlertDialog newMessage = new AlertDialog("THIS VOTER ALREADY HAS AN ACCPETED BALLOT");
                        newMessage.ShowDialog();
                    }
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
        // Load the voter list control
        private VoterListViewModel _voterList;
        public VoterListViewModel VoterList
        {
            get
            {
                if (_voterList == null)
                {
                    _voterList = new VoterListViewModel(AppSettings.Election.ElectionType.ToInt());

                    _voterList.SetLogCodeColors(Brushes.Black);

                    //_voterList.VoterListHeight = 500;

                    _voterList.PropertyChanged += OnVoterSelectedPropertyChanged;
                }
                return _voterList;
            }

            private set
            {
                _voterList = value;
            }
        }

        // Check which properties changed in the Voter List View
        private void OnVoterSelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedVoter")
            {
                // REMOVING A VOTER UPDATES THE PROPERTY AND RETURNS A NULL

                // Check for empty election
                if (((VoterListViewModel)sender).SelectedVoter != null)
                {
                    //Code to respond to a change in the ViewModel
                    Console.WriteLine(
                        "Selected Voter: " +
                        ((VoterListViewModel)sender).SelectedVoter.Data.VoterID.ToString() + " | STATUS: " +
                        ((VoterListViewModel)sender).SelectedVoter.CheckStatus((int)AppSettings.System.SiteID));

                    // Get selected voter
                    //var selectedVoter = ((VoterListViewModel)sender).SelectedVoter;

                    // Display Delete Button
                    RemoveVoterVisibility = true;
                    RaisePropertyChanged("RemoveVoterVisibility");
                }
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
        // Load the barcode scanner control
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

        #region LogCodes
        // Load the list of valid log codes
        private List<LogCodeModel> _logCodeList;
        public List<LogCodeModel> LogCodeList
        {
            get
            {
                // Create list of valid log codes
                List<int> validCodes = new List<int> { 9, 10, 12, 14 };

                if (_logCodeList == null)
                {
                    _logCodeList = ElectionDataMethods.Election.Lists.LogCodes
                        .Where(lc => validCodes.Contains(lc.LogCode))
                        .ToList();
                }
                return _logCodeList;
            }
        }

        // Contains the currently selected log code
        private LogCodeModel _selectedLogCodeItem;
        public LogCodeModel SelectedLogCodeItem
        {
            get
            {
                return _selectedLogCodeItem;
            }
            set
            {
                _selectedLogCodeItem = value;

                // Set barcode focus after selection is changed
                IsBarcodeFocused = true;
                RaisePropertyChanged("IsBarcodeFocused");
            }
        }
        #endregion

        #region ProcessBallots
        // Bound command for processing the applications
        public RelayCommand _processBallotsCommand;
        public ICommand ProcessBallotsCommand
        {
            get
            {
                if (_processBallotsCommand == null)
                {
                    _processBallotsCommand = new RelayCommand(
                        param => this.ProcessBallotsClick(),
                        param => this.CanProcessBallots);
                }
                return _processBallotsCommand;
            }
        }

        // Enable or Disable the process applications control
        private bool _canProcessBallots;
        public bool CanProcessBallots
        {
            get { return _canProcessBallots; }
            internal set
            {
                _canProcessBallots = value;
                RaisePropertyChanged("CanProcessBallots");
            }
        }

        // Process the ballots and mark the voters with the selected log code
        private async void ProcessBallotsClick()
        {
            if (VoterList != null && VoterList.VoterList.Count() > 0)
            {
                if (SelectedLogCodeItem != null)
                {
                    string lastCount = VoterList.VoterList.Count().ToString();
                    YesNoDialog newMessage = new YesNoDialog("Process Batch", "ARE YOU SURE YOU WANT TO PROCESS \r" + lastCount + " ENVELOPE(S) AS " + SelectedLogCodeItem.LogDescription.ToUpper() + "?");
                    if (newMessage.ShowDialog() == true)
                    {
                        // Add voters to session
                        await Task.Run(() => VoterList.VoterList.AddToScanHistory(AppSettings.System.SiteID, AppSettings.System.MachineID, DateTime.Now, ((App)Application.Current).Connection));

                        // Mark Voters as "Returned Ballot"
                        await Task.Run(() => VoterList.VoterList.ReturnBallots(SelectedLogCodeItem.LogCode));

                        YesNoDialog exitMessage = new YesNoDialog("Finish Batch", lastCount + " ENVELOPES HAVE BEEN PROCESSED AS " + SelectedLogCodeItem.LogDescription.ToUpper() + ".\r\nDO YOU WANT TO SCAN MORE ENVELOPES?");
                        if (exitMessage.ShowDialog() == true)
                        {
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
                else
                {
                    AlertDialog newMessage = new AlertDialog("NO STATUS HAS BEEN SELECTED");
                    newMessage.ShowDialog();
                }
            }
        }
        #endregion

        #region RemoveVoter
        // Property controls the Remove Voter button visibility
        private bool _removeVoterVisibility;
        public bool RemoveVoterVisibility
        {
            get
            {
                return _removeVoterVisibility;
            }
            set
            {
                _removeVoterVisibility = value;
                RaisePropertyChanged("RemoveVoterVisibility");

                // Enable the Remove Voter button
                CanRemoveVoter = value;
            }
        }

        // Bound command for removing a voter from the list
        public RelayCommand _removeVoterCommand;
        public ICommand RemoveVoterCommand
        {
            get
            {
                if (_removeVoterCommand == null)
                {
                    _removeVoterCommand = new RelayCommand(
                        param => this.RemoveVoterClick(),
                        param => this.CanRemoveVoter);
                }
                return _removeVoterCommand;
            }
        }

        // Enable or Disable the Remove Voter button
        private bool _canRemoveVoter;
        public bool CanRemoveVoter
        {
            get { return _canRemoveVoter; }
            internal set
            {
                _canRemoveVoter = value;
                RaisePropertyChanged("CanRemoveVoter");
            }
        }

        // Remove the currently selected voter
        private void RemoveVoterClick()
        {
            // Remove selected voter from list
            VoterList.RemoveSelectedVoter();

            // Have to set focus after voter is remove
            IsBarcodeFocused = true;
            RaisePropertyChanged("IsBarcodeFocused");

            // Hide the Remove Voter button
            RemoveVoterVisibility = false;
        }
        #endregion

        #region Cancel
        // Bound command for exiting the screen
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

        // Enable or Disable the Cancel button
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

        // Force the parent frame to navigate to the home screen
        private void CancelClick()
        {
            // Check if the list is empty
            if (VoterList != null && VoterList.VoterList.Count() > 0)
            {
                // Ask the user "ARE YOU SURE?"
                OkCancelDialog newMessage = new OkCancelDialog("Cancel Batch", VoterList.VoterList.Count().ToString() + " BALLOT(S) HAVE NOT BEEN SAVED\r\nDO YOU STILL WANT TO EXIT?");
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
