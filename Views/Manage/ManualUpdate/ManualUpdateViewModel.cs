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
using VoterX.Core.Elections;
using VoterX.Core.Extensions;
using VoterX.Core.ScanHistory;
using VoterX.Core.Voters;
using VoterX.SystemSettings.Extensions;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Views;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Views.Manage
{
    public class ManualUpdateViewModel : NotifyPropertyChanged
    {
        public ManualUpdateViewModel()
        {
            CanSearch = true;

            CanScanSession = true;

            CanCancel = true;

            IsBarcodeFocused = true;

            RemoveVoterVisibility = false;
            CanRemoveVoter = false;

            // Display page header
            GlobalReferences.Header.PageHeader = ("Scan Returned Envelopes");
        }

        private bool _isBarCodeFocused;
        public bool IsBarcodeFocused
        {
            get
            {
                return _isBarCodeFocused;
            }
            set
            {
                _isBarCodeFocused = value;
                RaisePropertyChanged("IsBarcodeFocused");
            }
        }

        #region SessionValues
        private bool _isScanSession;
        public bool IsScanSession
        {
            get
            {
                return _isScanSession;
            }

            set
            {
                _isScanSession = value;

                // Reset the focus to the barcode
                IsBarcodeFocused = true;
                RaisePropertyChanged("IsBarcodeFocused");

                // Show/Hide the session details
                SessionDetailsVisibility = _isScanSession;
                RaisePropertyChanged("SessionDetailsVisibility");
            }
        }

        public bool CanScanSession { get; set; }
        public bool SessionDetailsVisibility { get; set; }

        public string SessionNumber { get; set; }
        public string SessionDate { get; set; }
        #endregion

        #region VoterSearch
        private string _barCode;
        public string BarCode
        {
            get { return _barCode; }
            set
            {
                _barCode = value;
                RaisePropertyChanged("BarCode");
            }
        }

        private string _previousBarCode;
        public string PreviousBarCode
        {
            get { return _previousBarCode; }
            set
            {
                _previousBarCode = value;
                RaisePropertyChanged("PreviousBarCode");
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
                // I also need to disable the text box

                _canSearch = value;
                RaisePropertyChanged("CanSearch");
            }
        }

        //private void SearchOrSessionClick()
        //{
        //    if (IsScanSession == true)
        //    {
        //        SearchSessionClick();
        //    }
        //    else
        //    {
        //        SearchClick();
        //    }
        //}

        // Search the database for the voter and add them to the list control
        private void SearchClick()
        {
            // Check for empty values
            if (BarCode != null && BarCode != "")
            {
                // Prevent double clicking the scanner
                CanSearch = false;

                CanScanSession = false;
                RaisePropertyChanged("CanScanSession");

                if (PreviousBarCode != BarCode)
                {
                    PreviousBarCode = BarCode;

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
                                // Session Scan Selected
                                if (IsScanSession == true)
                                {
                                    // Get session data
                                    var factory = new ScanHistoryFactory();
                                    var session = factory.GetSession(voters.FirstOrDefault().Data);

                                    if (session != null && session.Data.SessionLocked != true)
                                    {
                                        // Display Session Details
                                        SessionNumber = session.Data.SessionId.ToString(); 
                                        RaisePropertyChanged("SessionNumber");
                                        SessionDate = session.Data.SessionDate.ToString();
                                        RaisePropertyChanged("SessionDate");

                                        // Clear the existing list to prevent multiple sessions from being loaded
                                        voters = new ObservableCollection<NMVoter>();
                                        VoterList.ClearList();

                                        // Load Voters from Session
                                        voters = VoterMethods.Voters.List(session);

                                        // Users cannot be allowed to scan another session
                                        CanSearch = false;
                                    }
                                    else if (session.Data.SessionLocked == true)
                                    {
                                        AlertDialog newMessage = new AlertDialog("VOTER RECORDS IN THIS SESSION ARE NOT CONSISTANT!\r\nONE OR MORE RECORDS HAVE BEEN CHANGED,\r\nANY FURTHER UPDATES MUST BE MADE INIDIVUALLY");
                                        if (newMessage.ShowDialog() == true)
                                        {
                                            // SET SCREEN BACK TO DEFAULT STATE
                                            IsScanSession = false;
                                            RaisePropertyChanged("IsScanSession");

                                            BarCode = "";
                                            PreviousBarCode = "";

                                            // Enable the input box
                                            CanSearch = true;

                                            // Have to set focus after textbox is enabled
                                            IsBarcodeFocused = true;

                                            CanScanSession = true;
                                            RaisePropertyChanged("CanScanSession");

                                            return;
                                        }
                                    }
                                    else if(session == null)
                                    {
                                        AlertDialog newMessage = new AlertDialog("NO SCAN SESSION FOUND FOR THIS VOTER");
                                        if (newMessage.ShowDialog() == true)
                                        {
                                            // SET SCREEN BACK TO DEFAULT STATE
                                            IsScanSession = false;
                                            RaisePropertyChanged("IsScanSession");

                                            BarCode = "";

                                            // Enable the input box
                                            CanSearch = true;

                                            // Have to set focus after textbox is enabled
                                            IsBarcodeFocused = true;

                                            CanScanSession = true;
                                            RaisePropertyChanged("CanScanSession");

                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    // Enable the input box
                                    CanSearch = true;

                                    // Once a voter has been scanned they cant change the session option
                                    //CanScanSession = true;
                                    //RaisePropertyChanged("CanScanSession");
                                }

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

                BarCode = "";

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

                // Check if voter's log code matches the from selection
                if (SelectedFromLogCodeItem == null || voterLogCode != SelectedFromLogCodeItem.LogCode)
                {
                    if (voterLogCode == 17)
                    {
                        AlertDialog newMessage = new AlertDialog("THIS VOTER HAS BEEN DELETED OR REMOVED");
                        if (newMessage.ShowDialog() == true)
                        {

                        }
                    }
                    else
                    {
                        string processTypeFrom = null;
                        if (SelectedFromLogCodeItem != null) processTypeFrom = SelectedFromLogCodeItem.LogDescription.ToUpper();

                        AlertDialog newMessage = new AlertDialog("THIS VOTER'S CURRENT STATUS DOES NOT MATCH THE \r\nFROM SELECTION: " + processTypeFrom);
                        if (newMessage.ShowDialog() == true)
                        {

                        }
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

        //private void SearchSessionClick()
        //{
        //    // Check for empty values
        //    if (BarCode != null && BarCode != "")
        //    {
        //        // Prevent double clicking the scanner
        //        CanSearch = false;

        //        CanScanSession = false;
        //        RaisePropertyChanged("CanScanSession");

        //        if (PreviousBarCode != BarCode)
        //        {
        //            PreviousBarCode = BarCode;

        //            if (VoterList != null)
        //            {
        //                // Check if the voter already exists in the list
        //                if (!VoterList.IdExists(BarCode))
        //                {
        //                    // Create the Voter Search Object (Search Parameters)
        //                    VoterSearchModel searchItems = new VoterSearchModel
        //                    {
        //                        VoterID = BarCode
        //                    };

        //                    // Get voter from barcode
        //                    var voters = VoterMethods.Voters.List(searchItems);

        //                    // Valid the voter
        //                    if (ValidateVoter(voters))
        //                    {
        //                        // Set local values
        //                        voters.Localize(
        //                            AppSettings.Election.ElectionID,
        //                            AppSettings.Absentee.SiteID,
        //                            AppSettings.System.MachineID,
        //                            AppSettings.User.UserID);

        //                        // Add the voter to the list
        //                        VoterList.AddVoter(voters);

        //                        // Turn on Process Ballots button
        //                        if (VoterList != null && VoterList.VoterList.Count() > 0)
        //                        {
        //                            CanProcessBallots = true;
        //                        }
        //                        else
        //                        {
        //                            CanProcessBallots = false;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    // Display the already added message
        //                    AlertDialog newMessage = new AlertDialog("VOTER NUMBER " + BarCode + " HAS ALREADY BEEN ADDED TO THE LIST");
        //                    newMessage.ShowDialog();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // Display duplicate scan message
        //            AlertDialog newMessage = new AlertDialog("DUPLICATE ID NUMBER " + BarCode + " WAS ENTERED");
        //            newMessage.ShowDialog();
        //        }

        //        BarCode = "";

        //        // Enable the input box
        //        CanSearch = true;

        //        // Have to set focus after textbox is enabled
        //        IsBarcodeFocused = true;
        //    }
        //}
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
                    //RemoveVoterVisibility = true;
                    //RaisePropertyChanged("RemoveVoterVisibility");
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
        
        private LogCodeModel _selectedFromLogCodeItem;
        public LogCodeModel SelectedFromLogCodeItem
        {
            get
            {
                return _selectedFromLogCodeItem;
            }
            set
            {
                _selectedFromLogCodeItem = value;

                // Set barcode focus after selection is changed
                IsBarcodeFocused = true;
                RaisePropertyChanged("IsBarcodeFocused");
            }
        }

        private LogCodeModel _selectedToLogCodeItem;
        public LogCodeModel SelectedToLogCodeItem
        {
            get
            {
                return _selectedToLogCodeItem;
            }
            set
            {
                _selectedToLogCodeItem = value;

                // Set barcode focus after selection is changed
                IsBarcodeFocused = true;
                RaisePropertyChanged("IsBarcodeFocused");
            }
        }

        public string ChangeReason { get; set; }
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
                if (SelectedToLogCodeItem != null)
                {
                    string fromProcessType = null;
                    if (SelectedFromLogCodeItem != null) fromProcessType = SelectedFromLogCodeItem.LogDescription;

                    string toProcessType = null;
                    if (SelectedToLogCodeItem != null) toProcessType = SelectedToLogCodeItem.LogDescription;

                    if (SelectedToLogCodeItem == SelectedFromLogCodeItem)
                    {
                        AlertDialog newMessage = new AlertDialog("CANNOT CHANGE STATUS FROM " + fromProcessType.ToUpper() + " \r\nTO " + toProcessType.ToUpper());
                        newMessage.ShowDialog();
                    }
                    else
                    {
                        // Check for null or empty reason
                        if (ChangeReason.IsNullOrEmpty())
                        {
                            AlertDialog newMessage = new AlertDialog("PLEASE ENTER A REASON FOR THIS STATUS CHANGE");
                            newMessage.ShowDialog();
                        }
                        else
                        {
                            string lastCount = VoterList.VoterList.Count().ToString();
                            YesNoDialog newMessage = new YesNoDialog("Process Batch", "ARE YOU SURE YOU WANT TO PROCESS \r" + lastCount + " ENVELOPE(S) AS " + toProcessType.ToUpper() + "?");
                            if (newMessage.ShowDialog() == true)
                            {
                                // If session option is checked then Lock all Sessions in the voter list
                                if (IsScanSession != true)
                                {
                                    LockAllSessions();
                                }

                                // Mark Voters as "Returned Ballot"
                                await Task.Run(() => VoterList.VoterList.ReturnBallots(SelectedToLogCodeItem.LogCode));

                                YesNoDialog exitMessage = new YesNoDialog("Finish Batch", lastCount + " ENVELOPES HAVE BEEN PROCESSED AS " + toProcessType.ToUpper() + ".\r\nDO YOU WANT TO SCAN MORE ENVELOPES?");
                                if (exitMessage.ShowDialog() == true)
                                {
                                    // Clear the list
                                    VoterList.ClearList();

                                    PreviousBarCode = "";
                                    RaisePropertyChanged("PreviousBarCode");

                                    IsScanSession = false;
                                    RaisePropertyChanged("IsScanSession"); 

                                    SessionNumber = "";
                                    RaisePropertyChanged("SessionNumber");

                                    SessionDate = "";
                                    RaisePropertyChanged("SessionDate");

                                    IsBarcodeFocused = true;
                                    RaisePropertyChanged("IsBarcodeFocused");

                                    ChangeReason = "";
                                    RaisePropertyChanged("ChangeReason");

                                    SelectedFromLogCodeItem = null;
                                    RaisePropertyChanged("SelectedFromLogCodeItem");

                                    SelectedToLogCodeItem = null;
                                    RaisePropertyChanged("SelectedToLogCodeItem");

                                    // Enable the input box
                                    CanSearch = true;
                                    RaisePropertyChanged("CanSearch");

                                    CanScanSession = true;
                                    RaisePropertyChanged("CanScanSession");
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
                }
                else
                {
                    AlertDialog newMessage = new AlertDialog("NO STATUS HAS BEEN SELECTED");
                    newMessage.ShowDialog();
                }
            }
        }

        private void LockAllSessions()
        {
            var voters = VoterList.VoterList;

            // Get session data
            var factory = new ScanHistoryFactory();
            factory.LockSession(voters);
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
                OkCancelDialog newMessage = new OkCancelDialog("Cancel Batch", VoterList.VoterList.Count().ToString() + " CHANGE(S) HAVE NOT BEEN SAVED\r\nDO YOU STILL WANT TO EXIT?");
                if (newMessage.ShowDialog() == true)
                {
                    // Show menu button
                    //MainMenuMethods.ShowHamburger();
                    //GlobalReferences.Header.HamburgerMenuVisibility = true;

                    // Open Menu 
                    //MainMenuMethods.OpenMainMenu();
                    //GlobalReferences.MenuSlider.Open();

                    // Return to Absentee Menu page
                    //this.NavigateToPage(new VoterX.AbsenteeAdministrationPage());
                    //NavigationMenuMethods.AbsenteeHome();

                    NavigationMenuMethods.ReturnToOrigin();
                }
            }
            else
            {
                // Show menu button
                //MainMenuMethods.ShowHamburger();
                //GlobalReferences.Header.HamburgerMenuVisibility = true;

                // Open Menu 
                //MainMenuMethods.OpenMainMenu();
                //GlobalReferences.MenuSlider.Open();

                // Return to Absentee Menu page
                //this.NavigateToPage(new VoterX.AbsenteeAdministrationPage());
                //NavigationMenuMethods.AbsenteeHome();

                NavigationMenuMethods.ReturnToOrigin();
            }
        }
        #endregion
    }
}
