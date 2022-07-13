using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VoterX.Core.Elections;
using VoterX.Core.Voters;
using VoterX.Utilities.Commands;
using VoterX.SystemSettings.Models;
using System.Windows.Input;
using VoterX.Methods;
using VoterX.Dialogs;
using VoterX.ABS.Logging;
using VoterX.Core.Utilities;
using VoterX.Manager.Global;
using VoterX.Utilities.Models;
using VoterX.Manager.Methods;
using VoterX.Core.Extensions;
using VoterX.Core.AbsenteeBoard;
using VoterX.Utilities.Interfaces;
using VoterX.Manager.Menus;

namespace VoterX
{
    public class AbsenteeBoardVoterDetailsViewModel : NotifyPropertyChanged
    {
        public NMVoter Voter { get; set; }

        private bool SERVISSource = false;

        // Used to check if values have changed since last save
        private bool? BoardSaved = null;

        public AbsenteeBoardVoterDetailsViewModel(NMVoter voter) : this(voter, false) { }
        public AbsenteeBoardVoterDetailsViewModel(NMVoter voter, bool source)
        {
            Voter = voter;

            SERVISSource = source;
        }

        #region PrecinctParts
        private List<PrecinctModel> _precinctPartList;
        public List<PrecinctModel> PrecinctPartList
        {
            get
            {
                if (_precinctPartList == null)
                {
                    _precinctPartList = ((VoterX.App)Application.Current).Election.Lists.Precincts
                        .ToList();
                }
                return _precinctPartList;
            }
        }

        private PrecinctModel _selectedPrecinctPartItem;
        public PrecinctModel SelectedPrecinctPartItem
        {
            get
            {
                if (_selectedPrecinctPartItem == null)
                {
                    _selectedPrecinctPartItem = PrecinctPartList
                    .Where(pp => pp.PrecinctPartID == Voter.Data.PrecinctPartID).FirstOrDefault();
                }
                return _selectedPrecinctPartItem;
            }

            set
            {
                _selectedPrecinctPartItem = value;

                Voter.Data.PrecinctPartID = _selectedPrecinctPartItem.PrecinctPartID;

                RaisePropertyChanged("Voter");
            }
        }
        #endregion

        #region BallotStyles
        private List<BallotStyleModel> _ballotStyleList;
        public List<BallotStyleModel> BallotStyleList
        {
            get
            {
                if (_ballotStyleList == null)
                {
                    _ballotStyleList = ((VoterX.App)Application.Current).Election.Lists.BallotStyles
                        .ToList();
                }
                return _ballotStyleList;
            }
        }

        private BallotStyleModel _selectedBallotStyleItem;
        public BallotStyleModel SelectedBallotStyleItem
        {
            get
            {
                if (_selectedBallotStyleItem == null)
                {
                    _selectedBallotStyleItem = BallotStyleList
                    .Where(bs => bs.BallotStyleId == Voter.Data.BallotStyleID).FirstOrDefault();
                }
                return _selectedBallotStyleItem;
            }

            set
            {
                _selectedBallotStyleItem = value;

                Voter.Data.BallotStyle = _selectedBallotStyleItem.BallotStyleName;
                Voter.Data.BallotStyleID = _selectedBallotStyleItem.BallotStyleId;                
                Voter.Data.BallotStyleFile = _selectedBallotStyleItem.BallotStyleFileName;

                RaisePropertyChanged("Voter");
            }
        }
        #endregion

        #region VoterTypes
        private List<NameCharPairModel> _voterTylesList;
        public List<NameCharPairModel> VoterTypesList
        {
            get
            {
                if (_voterTylesList == null)
                {
                    _voterTylesList = new List<NameCharPairModel>();
                    _voterTylesList.Add(new NameCharPairModel { Char = "R", Description = "REGULAR" });
                    _voterTylesList.Add(new NameCharPairModel { Char = "U", Description = "UNIFORMED SERVICES" });
                    _voterTylesList.Add(new NameCharPairModel { Char = "O", Description = "OVERSEAS VOTER" });
                    _voterTylesList.Add(new NameCharPairModel { Char = "E", Description = "EMERGENCY RESPONDER" });
                }
                return _voterTylesList;
            }
        }

        private NameCharPairModel _selectedVoterTypesItem;
        public NameCharPairModel SelectedVoterTypesItem
        {
            get
            {
                // Always set voter type to voter's current value
                _selectedVoterTypesItem = VoterTypesList
                    .Where(t => t.Char == Voter.Data.AbsenteeType).FirstOrDefault();
                return _selectedVoterTypesItem;
            }

            set
            {
                _selectedVoterTypesItem = value;

                Voter.Data.AbsenteeType = _selectedVoterTypesItem.Char;

                RaisePropertyChanged("Voter");
            }
        }
        #endregion

        #region LogCodes
        private List<LogCodeModel> _logCodeList;
        public List<LogCodeModel> LogCodeList
        {
            get
            {
                if (_logCodeList == null)
                {
                    List<int> codeList = new List<int> { 9, 10, 11, 12, 14 };

                    _logCodeList = ((VoterX.App)Application.Current).Election.Lists.LogCodes
                        .Where(l => codeList.Contains(l.LogCode))
                        .ToList();
                }
                return _logCodeList;
            }
        }

        private LogCodeModel _selectedLogCodeItem;
        public LogCodeModel SelectedLogCodeItem
        {
            get
            {

                // Always set log code to voter's current value
                _selectedLogCodeItem = LogCodeList
                    .Where(lc => lc.LogCode == Voter.Data.LogCode).FirstOrDefault();
                return _selectedLogCodeItem;
            }

            set
            {
                _selectedLogCodeItem = value;

                Voter.Data.LogCode = _selectedLogCodeItem.LogCode;

                RaisePropertyChanged("Voter");
            }
        }
        #endregion

        #region Locations
        private List<LocationModel> _locationList;
        public List<LocationModel> LocationList
        {
            get
            {
                if (_locationList == null)
                {
                    _locationList = ((VoterX.App)Application.Current).Election.Lists.Locations
                        .ToList();
                }
                return _locationList;
            }
        }

        private LocationModel _selectedLocationItem;
        public LocationModel SelectedLocationItem
        {
            get
            {
                // Always set location to voter's current value
                _selectedLocationItem = LocationList
                    .Where(l => l.PollId == Voter.Data.PollID).FirstOrDefault();
                return _selectedLocationItem;
            }

            set
            {
                _selectedLocationItem = value;

                Voter.Data.PollID = _selectedLocationItem.PollId;

                RaisePropertyChanged("Voter");
            }
        }
        #endregion

        #region Exceptions
        private List<AbsenteeBoardExceptionsModel> _exceptionsList;
        public List<AbsenteeBoardExceptionsModel> ExceptionsList
        {
            get
            {
                if (_exceptionsList == null)
                {
                    _exceptionsList = GetExceptions();
                }
                return _exceptionsList;
            }
        }

        private List<AbsenteeBoardExceptionsModel> GetExceptions()
        {
            List<AbsenteeBoardExceptionsModel> exceptions = new List<AbsenteeBoardExceptionsModel>();

            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "ADDRESS MISSING" });
            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "VALID ID REQUIRED - MISSING" });
            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "FLED" });
            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "ENVELOPE PREVIOUSLY RECORDED" });
            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "NO 4 DIGIT SSN" });
            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "NO BALLOT" });
            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "NO SIGNATURE" });
            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "VOTER ID MISSING" });
            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "MULTIPLE BALLOTS IN ENVELOPE" });
            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "NO YEAR OF BIRTH" });
            exceptions.Add(new AbsenteeBoardExceptionsModel() { Exception = "NO PRINTED LABEL ON ENVELOPE" });

            return exceptions;
        }

        private AbsenteeBoardExceptionsModel _selectedExceptionItem;
        public AbsenteeBoardExceptionsModel SelectedExceptionItem
        {
            get
            {
                //if(_selectedExceptionItem == null)
                //{
                //    _selectedExceptionItem = new AbsenteeBoardExceptionsModel();
                //}

                if (_selectedExceptionItem == null && VoterBoardStatus != null)
                {
                    // Always set exception to voter's current value
                    _selectedExceptionItem = ExceptionsList
                        .Where(x => x.Exception == VoterBoardStatus.Exception).FirstOrDefault();
                }
                return _selectedExceptionItem;
            }

            set
            {
                _selectedExceptionItem = value;

                VoterBoardStatus.Exception = _selectedExceptionItem.Exception;

                RaisePropertyChanged("Voter");
                RaisePropertyChanged("VoterBoardStatus");
            }
        }
        #endregion

        #region AbsenteeBoard
        public string VoterName
        {
            get
            {
                if (Voter != null)
                {
                    return Voter.Data.FirstName + " " + Voter.Data.MiddleName + " " + Voter.Data.LastName + " " + Voter.Data.Generation;
                }
                else
                    return null;
            }
        }

        public string MailingAddress
        {
            get
            {
                if (Voter != null)
                {
                    string address = Voter.Data.MailingAddress1 + " " + Voter.Data.MailingAddress2 + " " + Voter.Data.MailingCity + " " + Voter.Data.MailingState + " " + Voter.Data.MailingZip;
                    // Remove double spaces
                    return address.Replace("  ", " ");
                }
                else
                    return null;
            }
        }

        public string RegisteredAddress
        {
            get
            {
                if (Voter != null)
                {
                    string address = Voter.Data.Address1 + " " + Voter.Data.Address2 + " " + Voter.Data.City + " " + Voter.Data.State + " " + Voter.Data.Zip;
                    // Remove double spaces
                    return address.Replace("  ", " ");
                }
                else
                    return null;
            }
        }

        public string DeliveryAddress
        {
            get
            {
                if (Voter != null)
                {
                    string address = Voter.Data.DeliveryAddress1 + " " + Voter.Data.DeliveryAddress2 + " " + Voter.Data.DeliveryCity + " " + Voter.Data.DeliveryState + " " + Voter.Data.DeliveryZip;
                    // Remove double spaces
                    return address.Replace("  ", " ");
                }
                else
                    return null;
            }
        }

        private AbsenteeBoardModel _voterBoardStatus;
        public AbsenteeBoardModel VoterBoardStatus
        {
            get
            {
                if (_voterBoardStatus == null && Voter != null)
                {
                    using (var factory = new AbsenteeBoardFactory(((App)Application.Current).Connection))
                    {
                        _voterBoardStatus = factory.VoterSearch(Voter.Data.VoterID.ToInt());

                        if(_voterBoardStatus == null)
                        {
                            _voterBoardStatus = factory.Create();
                        }
                    }
                }
                return _voterBoardStatus;
            }

            private set
            {
                _voterBoardStatus = value;
            }
        }

        private LocationModel _selectedScanLocationItem;
        public LocationModel SelectedScanLocationItem
        {
            get
            {
                if (VoterBoardStatus != null)
                {
                    // Always set location to voter's current value
                    _selectedScanLocationItem = LocationList
                        .Where(l => l.PollId == VoterBoardStatus.PollId).FirstOrDefault();
                }
                return _selectedScanLocationItem;
            }

            set
            {
                _selectedScanLocationItem = value;

                VoterBoardStatus.PollId = _selectedScanLocationItem.PollId;

                RaisePropertyChanged("VoterBoardStatus");

                BoardSaved = false;
            }
        }

        private LogCodeModel _selectedScanStatusItem;
        public LogCodeModel SelectedScanStatusItem
        {
            get
            {
                if (VoterBoardStatus != null)
                {
                    // Always set log code to voter's current value
                    _selectedScanStatusItem = LogCodeList
                        .Where(lc => lc.LogCode == VoterBoardStatus.LogCode).FirstOrDefault();
                }
                return _selectedScanStatusItem;
            }

            set
            {
                _selectedScanStatusItem = value;

                VoterBoardStatus.LogCode = _selectedScanStatusItem.LogCode;

                RaisePropertyChanged("VoterBoardStatus");
            }
        }

        private LogCodeModel _selectedChangeStatusItem;
        public LogCodeModel SelectedChangeStatusItem
        {
            get
            {
                if (VoterBoardStatus != null)
                {
                    // Always set log code to voter's current value
                    _selectedChangeStatusItem = LogCodeList
                    .Where(lc => lc.LogCode == VoterBoardStatus.LogCodeChange).FirstOrDefault();
                }
                return _selectedChangeStatusItem;
            }

            set
            {
                _selectedChangeStatusItem = value;

                VoterBoardStatus.LogCodeChange = _selectedChangeStatusItem.LogCode;

                RaisePropertyChanged("VoterBoardStatus");

                BoardSaved = false;
            }
        }
        #endregion

        #region Commands
        public RelayCommand _goBackCommand;
        public ICommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(param => this.GoBackClick());
                }
                return _goBackCommand;
            }
        }

        private void GoBackClick()
        {
            if (VoterBoardStatus != null)
            {
                if (BoardSaved == false)
                {
                    YesNoDialog aysMessage = new YesNoDialog("GoBack", "ABSENTEE BOARD CHANGES HAVE NOT BEEN SAVED.\r\nARE YOU SURE YOU WANT TO CONTINUE?");
                    if (aysMessage.ShowDialog() == true)
                    {
                        NavigateToSearchPage();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    NavigateToSearchPage();
                }
            }
            else
            {
                NavigateToSearchPage();
            }
        }

        private void NavigateToSearchPage()
        {
            // Create View Model container
            IMenuViewModel menuView = null;

            // Load either the all mail or normal view model
            menuView = new AbsenteeBoardMenuViewModel(GlobalReferences.MenuSlider, AppSettings.Global);

            // Create and load the menu from the view model
            GlobalReferences.MenuSlider.SetMenu(new AbsenteeBoardMenuView(menuView), MenuCollapseMode.Full);

            MainFrameMethods.NavigateToPage(new AbsenteeBoardVoterSearchPage());
        }

        public RelayCommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(param => this.LoadedClick());
                }
                return _loadedCommand;
            }
        }

        private void LoadedClick()
        {
            // Update display
            RaisePropertyChanged("Voter");
        }
        #endregion

        #region AbsenteeBoardCommands
        public RelayCommand _updateVoterBoardCommand;
        public ICommand UpdateVoterBoardCommand
        {
            get
            {
                if (_updateVoterBoardCommand == null)
                {
                    _updateVoterBoardCommand = new RelayCommand(param => this.SaveVoterBoardClick());
                }
                return _updateVoterBoardCommand;
            }
        }

        private void SaveVoterBoardClick()
        {
            using (var factory = new AbsenteeBoardFactory(((App)Application.Current).Connection))
            {
                try
                {
                    if(VoterBoardStatus.ScanDate == null)
                    {
                        // Insert Voter's values to absentee board
                        VoterBoardStatus.VoterId = Voter.Data.VoterID.ToInt();
                        VoterBoardStatus.ScanDate = DateTime.Now;
                        VoterBoardStatus.PollId = AppSettings.Absentee.SiteID;
                        VoterBoardStatus.Computer = AppSettings.System.MachineID;
                        VoterBoardStatus.LogCode = Voter.Data.LogCode.Value;
                        VoterBoardStatus.LastModified = DateTime.Now;
                    }

                    if (AppSettings.Absentee.BoardLocationRequired == true && (VoterBoardStatus.Location1 == null || VoterBoardStatus.Location1 == ""))
                    {
                        AlertDialog newMessage = new AlertDialog("LOCATION CANNOT BE BLANK");
                        newMessage.ShowDialog();
                    }
                    else
                    {
                        VoterBoardStatus.VoterId = Voter.Data.VoterID.ToInt();
                        factory.UpdateBoardVoter(VoterBoardStatus);

                        RaisePropertyChanged("VoterBoardStatus");
                        RaisePropertyChanged("SelectedScanLocationItem");
                        RaisePropertyChanged("SelectedScanStatusItem");

                        BoardSaved = true;

                        AlertDialog newMessage = new AlertDialog("CHANGES HAVE BEEN SAVED");
                        newMessage.ShowDialog();
                    }
                }
                catch (Exception e)
                {
                    // Log Exception
                    GlobalReferences.StatusBar.TextCenter = (e.Message);
                    VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                    ABSLogger.WriteLog("Error Issuing Application: " + e.Message);
                    if (e.InnerException != null)
                    {
                        //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                        ABSLogger.WriteLog(e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                            ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                        }
                    }
                }
            }
        }

        public RelayCommand _removeVoterBoardCommand;
        public ICommand RemoveVoterBoardCommand
        {
            get
            {
                if (_removeVoterBoardCommand == null)
                {
                    _removeVoterBoardCommand = new RelayCommand(param => this.RemoveVoterBoardClick());
                }
                return _removeVoterBoardCommand;
            }
        }

        private void RemoveVoterBoardClick()
        {

        }

        public RelayCommand _textChangedCommand;
        public ICommand TextChangedCommand
        {
            get
            {
                if (_textChangedCommand == null)
                {
                    _textChangedCommand = new RelayCommand(param => this.TextChangedInvoke());
                }
                return _textChangedCommand;
            }
        }

        private void TextChangedInvoke()
        {
            BoardSaved = false;
        }
        #endregion

        #region AddressCommands
        public RelayCommand _saveAddressCommand;
        public ICommand SaveAddressCommand
        {
            get
            {
                if (_saveAddressCommand == null)
                {
                    _saveAddressCommand = new RelayCommand(param => this.SaveAddressClick());
                }
                return _saveAddressCommand;
            }
        }

        private void SaveAddressClick()
        {
            // Check for valid temp address
            if (Voter.Data.TempAddress1 != "" && Voter.Data.TempAddress1 != null
                &&
                Voter.Data.TempCity != "" && Voter.Data.TempCity != null
                && 
                Voter.Data.TempState != "" && Voter.Data.TempState != null
                &&
                Voter.Data.TempZip != "" & Voter.Data.TempZip != null)
            {
                // Check for issued ballots
                if (Voter.Data.LogCode > 1 && (Voter.Data.LogCode <= 5 || Voter.Data.LogCode == 14))
                {
                    // Set delivered address with the new temp address
                    Voter.Data.DeliveryAddress1 = Voter.Data.TempAddress1;
                    Voter.Data.DeliveryAddress2 = Voter.Data.TempAddress2;
                    Voter.Data.DeliveryCity = Voter.Data.TempCity;
                    Voter.Data.DeliveryState = Voter.Data.TempState;
                    Voter.Data.DeliveryZip = Voter.Data.TempZip;
                    Voter.Data.DeliveryCountry = Voter.Data.TempCountry;

                    // Update display
                    RaisePropertyChanged("Voter");

                    // Save address to database
                    Voter.UpdateAddress();
                }
                else if(Voter.Data.LogCode == null || Voter.Data.LogCode <= 1)
                {
                    AlertDialog newMessage = new AlertDialog("NO APPLICATION HAS BEEN ISSUED TO THIS VOTER\r\nFROM THIS SYSTEM");
                    newMessage.ShowDialog();
                }
                else
                {
                    AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                    newMessage.ShowDialog();
                }
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("INVALID TEMP ADDRESS");
                newMessage.ShowDialog();
            }
        }

        private bool GetAddressFromDialog(int type)
        {
            bool result = false;

            // Copy Temp fields to voter.tempaddress
            //_voter.Data.TempAddress1 = TempAddress1.Text;
            //_voter.Data.TempAddress2 = TempAddress2.Text;
            //_voter.Data.TempCity = TempCity.Text;
            //_voter.Data.TempState = TempState.Text;
            //_voter.Data.TempZip = TempZip.Text;
            //_voter.Data.TempCountry = TempCountry.Text;

            // Check if addresses match
            if (
                // Temp address is not blank 
                !Voter.Data.TempAddress1.IsNullOrSpace()
                ||
                // Address 1 does not match
                Voter.Data.MailingAddress1 != Voter.Data.Address1
                ||
                // Address 2 does not match
                Voter.Data.MailingAddress2 != Voter.Data.Address2
                ||
                // City does not match
                Voter.Data.MailingCity != Voter.Data.City
                ||
                // State does not match
                Voter.Data.MailingState != Voter.Data.State
                ||
                // Zip does not match
                Voter.Data.MailingZip != Voter.Data.Zip
                )
            {
                // ADDRESSES DONT MATCH 
                // (SOME VOTERS DO NOT HAVE A MAILING ADDRESS)
                // If no mailing address is found and no temp address supplied then use the registered address (8/30/2019)

                // Check for blanks
                if (!Voter.Data.TempAddress1.IsNullOrSpace())
                {
                    result = PromptUserToSelectAddress(type);
                }
                else if (Voter.Data.MailingAddress1.IsNullOrSpace() && !Voter.Data.Address1.IsNullOrSpace())
                {
                    // Use Physical
                    CopyAddressSelection(2);
                    result = true;
                }
                else if (!Voter.Data.MailingAddress1.IsNullOrSpace() && Voter.Data.Address1.IsNullOrSpace())
                {
                    // Use Mailing
                    CopyAddressSelection(1);
                    result = true;
                }
                else if (!Voter.Data.Address1.IsNullOrSpace() && !Voter.Data.MailingAddress1.IsNullOrSpace())
                {
                    result = PromptUserToSelectAddress(type);
                }
                else if (Voter.Data.TempAddress1.IsNullOrSpace() && Voter.Data.Address1.IsNullOrSpace() && Voter.Data.MailingAddress1.IsNullOrSpace())
                {
                    AlertDialog newMessage = new AlertDialog("NO ADDRESS COULD BE FOUND FOR THIS VOTER");
                    newMessage.ShowDialog();
                }
                else
                {
                    AlertDialog newMessage = new AlertDialog("NO ADDRESS COULD BE FOUND FOR THIS VOTER");
                    newMessage.ShowDialog();
                }
            }
            else
            {
                Voter.Data.DeliveryAddress1 = Voter.Data.Address1;
                Voter.Data.DeliveryAddress2 = Voter.Data.Address2;
                Voter.Data.DeliveryCity = Voter.Data.City;
                Voter.Data.DeliveryState = Voter.Data.State;
                Voter.Data.DeliveryZip = Voter.Data.Zip;
                Voter.Data.DeliveryCountry = "";

                RaisePropertyChanged("Voter");

                result = true;
            }

            return result;
        }

        private bool PromptUserToSelectAddress(int type)
        {
            // Prompt user to select a mailing address
            AddressSelectionDialog addresSelection = new AddressSelectionDialog(Voter.Data, type);
            if (addresSelection.ShowDialog() == true)
            {
                //StatusBar.ApplicationStatus(addresSelection.Selection.ToString());

                CopyAddressSelection(addresSelection.Selection);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void CopyAddressSelection(int selection)
        {
            // Copy address from user selection
            switch (selection)
            {
                case 1:
                    //StatusBar.ApplicationStatus("Ballot Sent To: " + _voter.MailingAddress1 + " " + _voter.MailingAddress2 + " " + _voter.MailingCity + " " + _voter.MailingState + " " + _voter.MailingZip);
                    // Copy Mailing Address to Delivery Address
                    Voter.Data.DeliveryAddress1 = Voter.Data.MailingAddress1;
                    Voter.Data.DeliveryAddress2 = Voter.Data.MailingAddress2;
                    Voter.Data.DeliveryCity = Voter.Data.MailingCity;
                    Voter.Data.DeliveryState = Voter.Data.MailingState;
                    Voter.Data.DeliveryZip = Voter.Data.MailingZip;
                    Voter.Data.DeliveryCountry = Voter.Data.MailingCountry;
                    break;
                case 2:
                    //StatusBar.ApplicationStatus("Ballot Sent To: " + _voter.Address1 + " " + _voter.Address2 + " " + _voter.City + " " + _voter.State + " " + _voter.Zip);
                    // Copy Registered Address to Delivery Address
                    Voter.Data.DeliveryAddress1 = Voter.Data.Address1;
                    Voter.Data.DeliveryAddress2 = Voter.Data.Address2;
                    Voter.Data.DeliveryCity = Voter.Data.City;
                    Voter.Data.DeliveryState = Voter.Data.State;
                    Voter.Data.DeliveryZip = Voter.Data.Zip;
                    Voter.Data.DeliveryCountry = "";
                    break;
                case 3:
                    //StatusBar.ApplicationStatus("Ballot Sent To: " + _voter.TempAddress1 + " " + _voter.TempAddress2 + " " + _voter.TempCity + " " + _voter.TempState + " " + _voter.TempZip);
                    // Copy Temp Address to Delivery Address
                    Voter.Data.DeliveryAddress1 = Voter.Data.TempAddress1;
                    Voter.Data.DeliveryAddress2 = Voter.Data.TempAddress2;
                    Voter.Data.DeliveryCity = Voter.Data.TempCity;
                    Voter.Data.DeliveryState = Voter.Data.TempState;
                    Voter.Data.DeliveryZip = Voter.Data.TempZip;
                    Voter.Data.DeliveryCountry = Voter.Data.TempCountry;
                    break;
            }

            RaisePropertyChanged("Voter");
        }
        #endregion

        #region ApplicationCommands
        public RelayCommand _printApplicationCommand;
        public ICommand PrintApplicationCommand
        {
            get
            {
                if (_printApplicationCommand == null)
                {
                    _printApplicationCommand = new RelayCommand(param => this.PrintApplicationClick());
                }
                return _printApplicationCommand;
            }
        }

        private async void PrintApplicationClick()
        {
            if (Voter.Data.BallotStyleID == null) 
            {
                AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Ineligible Voter");
            }
            else if (Voter.Data.LogCode == 5)
            {
                AlertDialog newMessage = new AlertDialog("AN APPLICATION HAS ALREADY BEENN ACCEPTED FOR THIS VOTER");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Voter Has Already Voted");
            }
            else if (Voter.HasVoted())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Voter Has Already Voted");
            }
            else
            {
                //SetLocalMailingAddress();
                if (GetAddressFromDialog(2))
                {
                    // Set local values
                    Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                    Voter.Data.PollID = AppSettings.Absentee.SiteID;
                    Voter.Data.ComputerID = AppSettings.System.MachineID;
                    Voter.Data.UserId = AppSettings.User.UserID;

                    // Do not reset application issued date
                    if (Voter.Data.ApplicationIssued == null)
                    {
                        // Mark absentee record as "Application Issued"
                        Voter.Data.ApplicationIssued = DateTime.Now;
                        MarkIssuedApplication();

                        GlobalReferences.StatusBar.TextLeft = (
                            await Task.Run(() =>
                            BallotPrinting.PrintAbsenteeApplicationPre(Voter.Data)
                            ));
                    }
                    else
                    {
                        // Ask user "Are You Sure"
                        YesNoDialog newMessage = new YesNoDialog("Resubmit Application", "ARE YOU SURE YOU WANT TO PRINT ANOTHER APPLICATION?");
                        if (newMessage.ShowDialog() == true)
                        {
                            MarkIssuedApplication();
                            GlobalReferences.StatusBar.TextLeft = (BallotPrinting.PrintAbsenteeApplicationPre(Voter.Data));
                        }
                    }
                }
            }
        }

        private async void MarkIssuedApplication()
        {
            // Get selected voter type
            //if (VoterTypeList.SelectedIndex >= 0)
            //{
            //    _voter.Data.AbsenteeType = ((ComboBoxItem)VoterTypeList.SelectedItem).DataContext.ToString();
            //}
            //else
            //{
            //    _voter.Data.AbsenteeType = "R";
            //    VoterTypeList.SelectedIndex = 0;
            //}

            if (Voter.Data.IDRequired == true)
            {
                AlertDialog newMessage = new AlertDialog("A VALID ID IS REQUIRED FOR THIS VOTER");
                newMessage.ShowDialog();
            }

            Voter.Data.LogCode = 3;
            Voter.Data.LogDescription = "Issued AV Application by Mail";
            Voter.Data.ActivityDate = DateTime.Now;
            Voter.Data.LogDate = DateTime.Now;
            Voter.Data.ComputerID = AppSettings.System.MachineID;
            Voter.Data.PollID = AppSettings.Absentee.SiteID;

            // Update screen
            //LogCodesList.SelectedIndex = 2; // Selected index = #LogCode - 1
            //LogDate.Text = DateTime.Now.ToString();
            // Prevent Poll Location drop down list from changing if Application is Reissued
            //if (SitesList.SelectedIndex < 0)
            //{
            //    ComboBoxMethods.SetSelectedItem(SitesList, AppSettings.Absentee.SiteName);
            //}
            //Computer.Text = AppSettings.System.MachineID.ToString();
            //ApplicationIssuedDate.Text = _voter.Data.ApplicationIssued.ToString();

            if (await Task.Run(() => VoterMethods.Voters.Exists()) == true)
            {
                try
                {
                    Voter.IssueApplication();

                    RaisePropertyChanged("Voter"); 
                    RaisePropertyChanged("SelectedLogCodeItem");
                    RaisePropertyChanged("SelectedLocationItem"); 
                }
                catch (Exception e)
                {
                    // Log Exception
                    GlobalReferences.StatusBar.TextCenter = (e.Message);
                    VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                    ABSLogger.WriteLog("Error Issuing Application: " + e.Message);
                    if (e.InnerException != null)
                    {
                        //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                        ABSLogger.WriteLog(e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                            ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                        }
                    }
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
            }
        }
        #endregion

        #region MailingLabelCommands
        public RelayCommand _printMailingLabelCommand;
        public ICommand PrintMailingLabelCommand
        {
            get
            {
                if (_printMailingLabelCommand == null)
                {
                    _printMailingLabelCommand = new RelayCommand(param => this.PrintMailingLabelClick());
                }
                return _printMailingLabelCommand;
            }
        }

        private async void PrintMailingLabelClick()
        {
            if (GetAddressFromDialog(3))
            {
                GlobalReferences.StatusBar.TextCenter = (
                await PrintMailingLabelsAsync()
                );
            }
        }

        private async Task<string> PrintMailingLabelsAsync()
        {
            return await Task.Run(() => { return ReportPrintingMethods.PrintVoterMailingLabels(Voter.Data); });
        }
        #endregion

        #region BallotBundleCommands
        public RelayCommand _printBallotBundleCommand;
        public ICommand PrintBallotBundleCommand
        {
            get
            {
                if (_printBallotBundleCommand == null)
                {
                    _printBallotBundleCommand = new RelayCommand(param => this.PrintBallotBundleClick());
                }
                return _printBallotBundleCommand;
            }
        }

        private void PrintBallotBundleClick()
        {
            if (Voter.Data.BallotStyleID == null)
            {
                AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
                newMessage.ShowDialog();
            }
            else if (Voter.HasVoted() && Voter.Data.LogCode != 5) // Allow 5's to print a ballot from this screen
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
            }
            else if (Voter.HasRejectedApplication())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS AN UNRESOLVED REJECTED APPLICATION");
                newMessage.ShowDialog();
            }
            else
            {
                // Check for ID required (10/28/2019)
                if (Voter.IdRequired().Value)
                {
                    YesNoDialog newMessage = new YesNoDialog("Id Required", "A VALID ID IS REQUIRED FOR THIS VOTER\r\nWOULD YOU LIKE TO CONTINUE?");
                    if (newMessage.ShowDialog() == false)
                    {
                        return;
                    }
                }

                if (GetAddressFromDialog(1))
                {

                    // Set local values
                    Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                    Voter.Data.PollID = AppSettings.Absentee.SiteID;
                    Voter.Data.ComputerID = AppSettings.System.MachineID;
                    Voter.Data.UserId = AppSettings.User.UserID;

                    if (Voter.Data.ApplicationIssued == null)
                    {
                        YesNoDialog newMessage = new YesNoDialog("No Application", "THIS VOTER HAS NOT BEEN ISSUED AN APPLICATION\rFROM THE VoterX SYSTEM\r\n\r\nARE YOU SURE YOU WANT TO ISSUE A BALLOT?");
                        if (newMessage.ShowDialog() == true)
                        {
                            YesNoDialog applicationQuestion = new YesNoDialog("Extra Application", "WOULD YOU LIKE TO PRINT APPLICATION\r\nALONG WITH THIS BALLOT?");
                            if (applicationQuestion.ShowDialog() == true)
                            {
                                BallotPrinting.PrintAbsenteeApplicationPre(Voter.Data);
                            }
                            // Add EDDY fix here
                            PrintMailBundleAndIssueApplication(); 
                        }
                    }
                    else
                    {
                        YesNoDialog newMessage = new YesNoDialog("No Application", "THIS VOTERS APPLICATION WILL BE ACCEPTED\r\nAND A BALLOT WILL BE PRINTED.\r\nARE YOU SURE YOU WANT TO CONTINUE?");
                        if (newMessage.ShowDialog() == true)
                        {
                            // Add EDDY fix here
                            PrintMailBundleAndAcceptApplication();
                        }
                    }
                }
            }
        }

        private async void PrintMailBundleAndIssueApplication()
        {
            if (await Task.Run(() => VoterMethods.Exists == true))
            {
                DateTime issuedDate = DateTime.Now;

                // Mark absentee record as "Ballot Issued" 
                // date_issued and printed_date
                // also LogCode #6 "Issued Absentee Ballot by Mail"
                //if (await Task.Run(() => VoterMethods.Exists) == true)
                {
                    // Display changes
                    Voter.Data.LogCode = 6;
                    Voter.Data.LogDescription = "Issued Absentee Ballot by Mail";
                    Voter.Data.ActivityDate = DateTime.Now;
                    Voter.Data.LogDate = DateTime.Now;
                    Voter.Data.ComputerID = AppSettings.System.MachineID;
                    Voter.Data.PollID = AppSettings.Absentee.SiteID;
                    Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                    Voter.Data.UserId = AppSettings.User.UserID;
                    // Mark ballot issued & accepted today
                    Voter.Data.ApplicationIssued = DateTime.Now;
                    Voter.Data.ApplicationAccepted = DateTime.Now;

                    var electionRecord = ElectionDataMethods.Election.Lists.Election;
                    if (issuedDate < electionRecord.AbsenteeBeginDate)
                    {
                        Voter.Data.BallotPrinted = (DateTime)electionRecord.AbsenteeBeginDate;
                    }
                    else
                    {
                        Voter.Data.BallotPrinted = DateTime.Now;
                    }

                    // Get new ballot number
                    Voter.GetNextBallotNumber((int)AppSettings.System.SiteID);

                    try
                    {
                        Voter.IssueBallot();

                        RaisePropertyChanged("Voter");
                        RaisePropertyChanged("SelectedLogCodeItem");
                        RaisePropertyChanged("SelectedLocationItem");
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

                    // Print Mailing Labels
                    for (int i = 0; i < AppSettings.Ballots.LabelCount; i++)
                    {
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintVoterMailingLabels(Voter.Data));
                    }

                    // Print Ballot
                    BallotPrinting.ReprintBallot(Voter.Data);

                    // BALLOT STUBS FOR EDDY ONLY
                    if (AppSettings.System.BallotStub == 1)
                    {
                        BallotPrinting.ReprintStub(Voter.Data);  
                    }
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database Error");
            }
        }

        private async void PrintMailBundleAndAcceptApplication()
        {
            if (await Task.Run(() => VoterMethods.Exists == true))
            {
                DateTime issuedDate = DateTime.Now;

                // Mark absentee record as "Ballot Issued" 
                // date_issued and printed_date
                // also LogCode #6 "Issued Absentee Ballot by Mail"
                //if (await Task.Run(() => VoterMethods.Exists) == true)
                {
                    // Display changes
                    Voter.Data.LogCode = 6;
                    Voter.Data.LogDescription = "Issued Absentee Ballot by Mail";
                    Voter.Data.ActivityDate = DateTime.Now;
                    Voter.Data.LogDate = DateTime.Now;
                    Voter.Data.ComputerID = AppSettings.System.MachineID;
                    Voter.Data.PollID = AppSettings.Absentee.SiteID;
                    Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                    Voter.Data.UserId = AppSettings.User.UserID;
                    // Mark application accepted today
                    Voter.Data.ApplicationAccepted = DateTime.Now;

                    var electionRecord = ElectionDataMethods.Election.Lists.Election;
                    if (issuedDate < electionRecord.AbsenteeBeginDate)
                    {
                        Voter.Data.BallotPrinted = (DateTime)electionRecord.AbsenteeBeginDate;
                    }
                    else
                    {
                        Voter.Data.BallotPrinted = DateTime.Now;
                    }

                    try
                    {
                        Voter.IssueBallot();

                        RaisePropertyChanged("Voter");
                        RaisePropertyChanged("SelectedLogCodeItem");
                        RaisePropertyChanged("SelectedLocationItem");
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
                                //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                                ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                            }
                        }

                        return;
                    }

                    // Print Mailing Labels
                    for (int i = 0; i < AppSettings.Ballots.LabelCount; i++)
                    {
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintVoterMailingLabels(Voter.Data));
                    }

                    // Print Ballot
                    BallotPrinting.ReprintBallot(Voter.Data);

                    // BALLOT STUBS FOR EDDY ONLY
                    if (AppSettings.System.BallotStub == 1)
                    {
                        BallotPrinting.ReprintStub(Voter.Data);
                    }
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database Error");
            }
        }
        #endregion

        #region BatchVoterCommands
        public RelayCommand _batchVoterCommand;
        public ICommand BatchVoterCommand
        {
            get
            {
                if (_batchVoterCommand == null)
                {
                    _batchVoterCommand = new RelayCommand(param => this.BatchVoterClick());
                }
                return _batchVoterCommand;
            }
        }

        public void BatchVoterClick()
        {
            DateTime acceptedDate = DateTime.Now;

            // Check voter status
            if (Voter.Data.BallotStyleID == null)
            {
                AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
                newMessage.ShowDialog();
            }
            else if(Voter.Data.LogCode == 5)
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN QUEUED FOR BATCH PRINTING");
                newMessage.ShowDialog();
            }
            else if (Voter.HasVoted())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
            }
            else if (Voter.IdRequired().Value)
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS AN ID REQUIRED AND CANNOT BE BATCHED");
                newMessage.ShowDialog();
            }
            else if (Voter.Data.LogCode != 5)
            {
                if (Voter.Data.ApplicationIssued == null)
                {
                    YesNoDialog newMessage = new YesNoDialog("No Application", "THIS VOTER HAS NOT BEEN ISSUED AN APPLICATION\rFROM THE VoterX SYSTEM\r\n\r\nARE YOU SURE YOU WANT TO QUEUE A BALLOT?");
                    if (newMessage.ShowDialog() == true)
                    {
                        Voter.Data.ApplicationIssued = acceptedDate;

                        QueueBallot(acceptedDate);
                    }
                }
                else
                {
                    QueueBallot(acceptedDate);
                }
            }
            else if (Voter.Data.LogCode == 5)
            {
                GlobalReferences.StatusBar.TextLeft = ("Application Already Accpeted");
            }
        }

        public void QueueBallot(DateTime acceptedDate)
        {
            if (GetAddressFromDialog(1))
            {
                // Set local values
                Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                Voter.Data.PollID = AppSettings.Absentee.SiteID;
                Voter.Data.ComputerID = null;
                Voter.Data.UserId = null;

                // Mark Voter, Application Accepted #5
                Voter.Data.LogCode = 5;
                Voter.Data.ApplicationAccepted = acceptedDate;
                Voter.Data.ActivityDate = DateTime.Now;
                Voter.Data.LogDate = DateTime.Now;
                Voter.Data.ComputerID = AppSettings.System.MachineID;
                Voter.Data.PollID = AppSettings.Absentee.SiteID;

                try
                {
                    Voter.AcceptApplication();

                    RaisePropertyChanged("Voter");
                    RaisePropertyChanged("SelectedLogCodeItem");
                    RaisePropertyChanged("SelectedLocationItem");
                }
                catch (Exception e)
                {
                    // Log Exception
                    GlobalReferences.StatusBar.TextCenter = (e.Message);
                    VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                    ABSLogger.WriteLog("Error Queue For Bath: " + e.Message);
                    if (e.InnerException != null)
                    {
                        //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                        ABSLogger.WriteLog(e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                            ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                        }
                    }
                }
            }
        }
        #endregion

        #region SpoilBallotCommands
        public RelayCommand _spoilBallotCommand;
        public ICommand SpoilBallotCommand
        {
            get
            {
                if (_spoilBallotCommand == null)
                {
                    _spoilBallotCommand = new RelayCommand(param => this.SpoilBallotClick());
                }
                return _spoilBallotCommand;
            }
        }

        private void SpoilBallotClick()
        {
            List<int?> valideCodes = new List<int?> { 6, 14 };

            // Check if ballot exists
            if (Voter.HasBeenIssued())
            {
                if (Voter.Data.LogCode == 8 && Voter.Data.PollID != AppSettings.System.SiteID)
                {
                    AlertDialog newMessage = new AlertDialog("CANNOT SPOIL A BALLOT FROM ANOTHER SITE");
                    newMessage.ShowDialog();
                }
                else if (Voter.Data.LogCode == 16)
                {
                    AlertDialog newMessage = new AlertDialog("CANNOT SPOIL AN ELECTION DAY BALLOT");
                    newMessage.ShowDialog();
                }
                else if (!valideCodes.Contains(Voter.Data.LogCode))
                {
                    AlertDialog newMessage = new AlertDialog("CANNOT SPOIL A BALLOT WITH THE CURRENT STATUS");
                    newMessage.ShowDialog();
                }
                else
                {
                    if (GetAddressFromDialog(1))
                    {
                        // Reset Menu
                        //MainMenuMethods.LoadMenu(new VoterX.AbsenteeMenu());
                        GlobalReferences.MenuSlider.SetMenu(new VoterX.AbsenteeMenu(), MenuCollapseMode.Full);

                        //MainMenuMethods.SetMenuMinWidth(0);

                        //MainFrameMethods.NavigateToPage(new VoterX.SpoiledBallotPage(Voter)); 
                        NavigationMenuMethods.SpoiledBallotPage(Voter);
                    }
                }
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("THERE IS NO EXISTING BALLOT TO SPOIL");
                newMessage.ShowDialog();
            }
        }
        #endregion

        #region RejectApplicationCommands
        public RelayCommand _rejectApplicationCommand;
        public ICommand RejectApplicationCommand
        {
            get
            {
                if (_rejectApplicationCommand == null)
                {
                    _rejectApplicationCommand = new RelayCommand(param => this.RejectedApplicationClick());
                }
                return _rejectApplicationCommand;
            }
        }

        private void RejectedApplicationClick()
        {
            // Check if voter has already voted
            if (Voter.HasBeenIssued())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
            }
            // Check for existing application
            else if (Voter.Data.ApplicationIssued != null)
            {
                // Reset Menu
                //MainMenuMethods.LoadMenu(new VoterX.AbsenteeMenu());
                GlobalReferences.MenuSlider.SetMenu(new VoterX.AbsenteeMenu(), MenuCollapseMode.Full);

                //MainMenuMethods.SetMenuMinWidth(0);

                MainFrameMethods.NavigateToPage(new VoterX.AbsenteeRejectedApplicationsPage(Voter));
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("THERE IS NO EXISTING APPLICATION");
                newMessage.ShowDialog();
            }
        }
        #endregion

        #region ProvisionalCommands
        public RelayCommand _printProvisionalBallotCommand;
        public ICommand PrintProvisionalBallotCommand
        {
            get
            {
                if (_printProvisionalBallotCommand == null)
                {
                    _printProvisionalBallotCommand = new RelayCommand(param => this.PrintProvisionalBallotClick());
                }
                return _printProvisionalBallotCommand;
            }
        }

        public void PrintProvisionalBallotClick()
        {
            ValidationDialog passwordDialog = new ValidationDialog("Manager");
            if (passwordDialog.ShowDialog() == true)
            {
                // Set Mailing Address
                CopyAddressSelection(1);
                
                // Set local values
                Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                Voter.Data.PollID = AppSettings.Absentee.SiteID;
                Voter.Data.ComputerID = AppSettings.System.MachineID;
                Voter.Data.UserId = AppSettings.User.UserID;

                // Reset Menu
                GlobalReferences.MenuSlider.SetMenu(new VoterX.AbsenteeMenu(), MenuCollapseMode.Full);

                // Navigate to provisional page
                NavigationMenuMethods.ProvisionalBallotPage(Voter);                
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Invalid Password");
            }
        }
        #endregion

        #region EarlyVotingCommands
        public RelayCommand _printEarlyVotingBallotCommand;
        public ICommand PrintEarlyVotingBallotCommand
        {
            get
            {
                if (_printEarlyVotingBallotCommand == null)
                {
                    _printEarlyVotingBallotCommand = new RelayCommand(param => this.PrintEarlyVotingBallotClick());
                }
                return _printEarlyVotingBallotCommand;
            }
        }

        public async void PrintEarlyVotingBallotClick()
        {
            if (Voter.Data.BallotStyleID == null)
            {
                AlertDialog newMessage = new AlertDialog("INELIGIBLE VOTER");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Ineligible Voter");                
            }
            else if (Voter.HasVoted())
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS ALREADY BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
                //StatusBar.ApplicationStatusCenter("Voter Has Already Voted");
            }
            else
            {
                // Set Physical Address
                CopyAddressSelection(2);

                // Set local values
                Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                Voter.Data.PollID = AppSettings.System.SiteID;
                Voter.Data.ComputerID = AppSettings.System.MachineID;
                Voter.Data.UserId = AppSettings.User.UserID;

                YesNoDialog newMessage = new YesNoDialog("Early Voting", "ARE YOU SURE YOU WANT TO PRINT AN\r\nEARLY VOTING BALLOT FOR " + Voter.Data.FullName);
                if (newMessage.ShowDialog() == true)
                {
                    DateTime issuedDate = DateTime.Now;

                    // Mark absentee record as "Ballot Issued InPerson"
                    // date_issued and printed_date
                    // also LogCode #7 "In Person Voting"
                    if (await Task.Run(() => VoterMethods.Exists) == true)
                    {
                        // Display changes
                        Voter.Data.LogCode = 8;
                        Voter.Data.LogDescription = "Early Voting";
                        Voter.Data.ActivityDate = DateTime.Now;
                        Voter.Data.LogDate = DateTime.Now;
                        Voter.Data.ComputerID = AppSettings.System.MachineID;
                        Voter.Data.PollID = AppSettings.System.SiteID;

                        // Get new ballot number
                        Voter.GetNextBallotNumber((int)AppSettings.System.SiteID);

                        try
                        {
                            Voter.VotedAtPolls(LogCodes.EarlyVoting);

                            RaisePropertyChanged("Voter");
                            RaisePropertyChanged("SelectedLogCodeItem");
                            RaisePropertyChanged("SelectedLocationItem");
                        }
                        catch (Exception e)
                        {
                            // Log Exception
                            GlobalReferences.StatusBar.TextCenter = (e.Message);
                            VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                            ABSLogger.WriteLog("Error Early Voting: " + e.Message);
                            if (e.InnerException != null)
                            {
                                ABSLogger.WriteLog(e.InnerException.Message);
                                if (e.InnerException.InnerException != null)
                                {
                                    ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                                }
                            }

                            return;
                        }

                        await Task.Run(() =>
                        {
                            BallotPrinting.ReprintBallot(Voter.Data);
                            BallotPrinting.PrintAbsenteeApplicationPre(Voter.Data);
                            // Print Ballot Stub
                            if (AppSettings.System.BallotStub == 1)
                            {
                                ReportPrintingMethods.PrintBallotStub(Voter.Data);
                            }
                        });
                    }
                    else
                    {
                        GlobalReferences.StatusBar.TextCenter = ("Database not found");
                    }
                }
            }
        }
        #endregion
    }
}
