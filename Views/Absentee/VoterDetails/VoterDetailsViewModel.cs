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

namespace VoterX.Manager.Views.Absentee
{
    public class VoterDetailsViewModel : NotifyPropertyChanged
    {
        public NMVoter Voter { get; set; }

        private bool SERVISSource = false;

        public VoterDetailsViewModel(NMVoter voter) : this(voter, false) { }
        public VoterDetailsViewModel(NMVoter voter, bool source)
        {
            Voter = voter;

            SERVISSource = source;

            Voter.UpdateTempAddress();

            InitializeAddressSelection();
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
                    _logCodeList = ((VoterX.App)Application.Current).Election.Lists.LogCodes
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

        #region BallotRejectedReasons
        private List<BallotRejectedReasonModel> _ballotRejectedReasonList;
        public List<BallotRejectedReasonModel> BallotRejectedReasonList
        {
            get
            {
                if (_ballotRejectedReasonList == null)
                {
                    _ballotRejectedReasonList = ((VoterX.App)Application.Current).Election.Lists.BallotRejectedReasons
                        .ToList();
                }
                return _ballotRejectedReasonList;
            }
        }

        private BallotRejectedReasonModel _selectedBallotRejectedReasonItem;
        public BallotRejectedReasonModel SelectedBallotRejectedReasonItem
        {
            get
            {
                // Always set location to voter's current value
                _selectedBallotRejectedReasonItem = BallotRejectedReasonList
                    .Where(l => l.ServiseCode == Voter.Data.BallotRejectedReason).FirstOrDefault();
                return _selectedBallotRejectedReasonItem;
            }

            set
            {
                _selectedBallotRejectedReasonItem = value;

                Voter.Data.BallotRejectedReason = _selectedBallotRejectedReasonItem.ServiseCode;

                RaisePropertyChanged("Voter");
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
            if (SERVISSource == true || Voter.Data.PollID == 0)
            {
                // Navigate to SERVIS lookup screen
                //MainFrameMethods.NavigateToPage(new VoterX.SearchPage(0));
                NavigationMenuMethods.VoterSearchPage(0);
            }
            else
            {
                // Else go to normal search page
                //MainFrameMethods.NavigateToPage(new VoterX.SearchPage());
                NavigationMenuMethods.VoterSearchPage();
            }
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

        #region AddressCommands
        private string _addressUpdatedMessage;
        public string AddressUpdatedMessage
        {
            get
            {
                return _addressUpdatedMessage;
            }
            set
            {
                _addressUpdatedMessage = value;                
                RaisePropertyChanged("AddressUpdatedMessage");

                Voter.Data.AddressChanged = true;
            }
        }

        private bool _isMailingAddressSelected;
        public bool IsMailingAddressSelected
        {
            get
            {
                return _isMailingAddressSelected;
            }
            set
            {
                _isMailingAddressSelected = value;
                RaisePropertyChanged("IsMailingAddressSelected");
            }
        }

        private bool _isRegisteredAddressSelected;
        public bool IsRegisteredAddressSelected
        {
            get
            {
                return _isRegisteredAddressSelected;
            }
            set
            {
                _isRegisteredAddressSelected = value;
                RaisePropertyChanged("IsRegisteredAddressSelected");
            }
        }

        private bool _isTempAddressSelected;
        public bool IsTempAddressSelected
        {
            get
            {
                return _isTempAddressSelected;
            }
            set
            {
                _isTempAddressSelected = value;
                RaisePropertyChanged("IsTempAddressSelected");
            }
        }

        private RelayCommand _tempAddressChanged;
        public ICommand TempAddressChanged
        {
            get
            {
                if (_tempAddressChanged == null)
                {
                    _tempAddressChanged = new RelayCommand(param => this.TempAddressClick());
                }
                return _tempAddressChanged;
            }
        }

        private void TempAddressClick()
        {
            IsMailingAddressSelected = false;
            IsRegisteredAddressSelected = false;
            IsTempAddressSelected = true;
        }

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

        private bool GetSelectionOrDeliveryAddress(int type)
        {
            if (Voter.HasDeliveryAddress())
            {
                return true;
            }
            else
            {
                return GetAddressFromSelection(type);
            }
        }

        private bool GetAddressFromSelection(int type)
        {
            bool result = false;

            if (IsMailingAddressSelected)
            {
                CopyAddressSelection(1);

                result = true;
            }
            else if (IsRegisteredAddressSelected)
            {
                CopyAddressSelection(2);

                result = true;
            }
            else if (IsTempAddressSelected)
            {
                // Check temp address fields
                if (
                // Address 1 not blank
                Voter.Data.TempAddress1.IsNullOrSpace()
                &&
                // Address 2 not blank
                Voter.Data.TempAddress2.IsNullOrSpace()
                &&
                // City not blank
                Voter.Data.TempCity.IsNullOrSpace()
                &&
                // State not blank
                Voter.Data.TempState.IsNullOrSpace()
                &&
                // Zip not blank
                Voter.Data.TempZip.IsNullOrSpace()
                )
                {
                    AlertDialog newMessage = new AlertDialog("INVALID TEMP ADDRESS");
                    newMessage.ShowDialog();

                    result = false;
                }
                else
                {
                    CopyAddressSelection(3);

                    result = true;
                }
            }

            return result;
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
                    Voter.Data.TempAddress = false;
                    break;
                case 2:
                    //StatusBar.ApplicationStatus("Ballot Sent To: " + _voter.Address1 + " " + _voter.Address2 + " " + _voter.City + " " + _voter.State + " " + _voter.Zip);
                    // Copy Registered Address to Delivery Address
                    Voter.Data.DeliveryAddress1 = Voter.Data.Address1;
                    Voter.Data.DeliveryAddress2 = Voter.Data.Address2;
                    Voter.Data.DeliveryCity = Voter.Data.City;
                    Voter.Data.DeliveryState = Voter.Data.State;
                    Voter.Data.DeliveryZip = Voter.Data.Zip;
                    Voter.Data.DeliveryCountry = null;
                    Voter.Data.TempAddress = false;
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
                    Voter.Data.TempAddress = true;
                    break;

                case 4:
                    //StatusBar.ApplicationStatus("Ballot Sent To: " + _voter.MailingAddress1 + " " + _voter.MailingAddress2 + " " + _voter.MailingCity + " " + _voter.MailingState + " " + _voter.MailingZip);
                    // Copy Mailing Address to Delivery Address
                    Voter.Data.DeliveryAddress1 = Voter.Data.MailingAddress1;
                    Voter.Data.DeliveryAddress2 = Voter.Data.MailingAddress2;
                    Voter.Data.DeliveryCity = Voter.Data.MailingCity;
                    Voter.Data.DeliveryState = Voter.Data.MailingState;
                    Voter.Data.DeliveryZip = Voter.Data.MailingZip;
                    Voter.Data.DeliveryCountry = Voter.Data.MailingCountry;
                    Voter.Data.OutofCountry = true;
                    break;
                case 5:
                    //StatusBar.ApplicationStatus("Ballot Sent To: " + _voter.Address1 + " " + _voter.Address2 + " " + _voter.City + " " + _voter.State + " " + _voter.Zip);
                    // Copy Registered Address to Delivery Address
                    Voter.Data.DeliveryAddress1 = Voter.Data.Address1;
                    Voter.Data.DeliveryAddress2 = Voter.Data.Address2;
                    Voter.Data.DeliveryCity = Voter.Data.City;
                    Voter.Data.DeliveryState = Voter.Data.State;
                    Voter.Data.DeliveryZip = Voter.Data.Zip;
                    Voter.Data.DeliveryCountry = null;
                    Voter.Data.OutofCountry = true;
                    break;
                case 6:
                    //StatusBar.ApplicationStatus("Ballot Sent To: " + _voter.TempAddress1 + " " + _voter.TempAddress2 + " " + _voter.TempCity + " " + _voter.TempState + " " + _voter.TempZip);
                    // Copy Temp Address to Delivery Address
                    Voter.Data.DeliveryAddress1 = Voter.Data.TempAddress1;
                    Voter.Data.DeliveryAddress2 = Voter.Data.TempAddress2;
                    Voter.Data.DeliveryCity = Voter.Data.TempCity;
                    Voter.Data.DeliveryState = Voter.Data.TempState;
                    Voter.Data.DeliveryZip = Voter.Data.TempZip;
                    Voter.Data.DeliveryCountry = Voter.Data.TempCountry;
                    Voter.Data.OutofCountry = true;
                    break;
            }

            RaisePropertyChanged("Voter");
        }

        private void InitializeAddressSelection()
        {
            //Voter.Data.AddressChanged = false;

            // Check Temp Address Fields
            if (
            // Address 1 not blank
            !Voter.Data.TempAddress1.IsNullOrSpace()
            ||
            // Address 2 not blank
            !Voter.Data.TempAddress2.IsNullOrSpace()
            ||
            // City not blank
            !Voter.Data.TempCity.IsNullOrSpace()
            ||
            // State not blank
            !Voter.Data.TempState.IsNullOrSpace()
            ||
            // Zip not blank
            !Voter.Data.TempZip.IsNullOrSpace()
            ||
            // Country not blank
            !Voter.Data.TempCountry.IsNullOrSpace()
            )
            {
                // Set temp address selection
                IsTempAddressSelected = true;
            }

            // Check Mailing Address Fields
            else if (
            // Address 1 not blank
            !Voter.Data.MailingAddress1.IsNullOrSpace()
            ||
            // Address 2 not blank
            !Voter.Data.MailingAddress2.IsNullOrSpace()
            ||
            // City not blank
            !Voter.Data.MailingCity.IsNullOrSpace()
            ||
            // State not blank
            !Voter.Data.MailingState.IsNullOrSpace()
            ||
            // Zip not blank
            !Voter.Data.MailingZip.IsNullOrSpace()
            )
            {
                // Set mailing address selection
                IsMailingAddressSelected = true;
            }

            // Check Registered Address fields
            else if (
            // Address 1 not blank
            !Voter.Data.Address1.IsNullOrSpace()
            ||
            // Address 2 not blank
            !Voter.Data.Address2.IsNullOrSpace()
            ||
            // City not blank
            !Voter.Data.City.IsNullOrSpace()
            ||
            // State not blank
            !Voter.Data.State.IsNullOrSpace()
            ||
            // Zip not blank
            !Voter.Data.Zip.IsNullOrSpace()
            )
            {
                // Set registered address selection
                IsRegisteredAddressSelected = true;
            }
            else
            {
                IsTempAddressSelected = true;
            }
        }

        private RelayCommand _editAddressCommand;
        public ICommand EditAddressCommand
        {
            get
            {
                if (_editAddressCommand == null)
                {
                    _editAddressCommand = new RelayCommand(param => this.EditAddressClick(), param => this.CanEditAddress);
                }
                return _editAddressCommand;
            }
        }

        // Enable or Disable the Edit Address Button
        public bool CanEditAddress
        {
            get
            {
                return Voter.Data.LogCode > 1;
            }
        }

        private void EditAddressClick()
        {
            //GetAddressFromDialog(1);
            GetAddressFromSelection(1);

            AddressUpdatedMessage = "ADDRESS UPDATED";

            if(Voter.Data.LogCode <= 5)
            {
                // Set local values
                Voter.Data.ElectionID = AppSettings.Election.ElectionID;

                //var test = Voter.Data.ElectionID;
                Voter.UpdateAddress();
            }
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
                    _printApplicationCommand = new RelayCommand(param => this.PrintApplicationClick(), param => this.CanPrintApplication);
                }
                return _printApplicationCommand;
            }
        }

        // Enable or Disable the Print Application Button
        public bool CanPrintApplication
        {
            get
            {
                return Voter.Data.LogCode < 5 && Voter.Data.BallotStyleID != null && Voter.Data.PrecinctPartID != null && Voter.Data.PrecinctPartID != "";
            }
        }

        // Enable or Disable the Reject Ballot control
        //private bool _canPrintApplication;
        //public bool CanPrintApplication
        //{
        //    get
        //    {
        //        if (Voter.Data.LogCode == 6 || Voter.Data.LogCode == 9)
        //        {
        //            _canPrintApplication = true;
        //        }
        //        else
        //        {
        //            _canPrintApplication = false;
        //        }
        //        return _canPrintApplication;
        //    }
        //    internal set
        //    {
        //        _canPrintApplication = value;
        //        RaisePropertyChanged("CanPrintApplication");
        //    }
        //}

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
                AlertDialog newMessage = new AlertDialog("AN APPLICATION HAS ALREADY BEEN ACCEPTED FOR THIS VOTER");
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
                // Do not reset application issued date
                if (Voter.Data.ApplicationIssued == null)
                {
                    //SetLocalMailingAddress();
                    //if (GetAddressFromDialog(2))
                    if (GetSelectionOrDeliveryAddress(2))
                    {
                        // Set local values
                        Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                        Voter.Data.PollID = AppSettings.Absentee.SiteID;
                        Voter.Data.ComputerID = AppSettings.System.MachineID;
                        Voter.Data.UserId = AppSettings.User.UserID;

                        // Mark absentee record as "Application Issued"
                        Voter.Data.ApplicationIssued = DateTime.Now;
                        MarkIssuedApplication();

                        GlobalReferences.StatusBar.TextLeft = (
                            await Task.Run(() =>
                            BallotPrinting.PrintAbsenteeApplicationPre(Voter.Data)
                            ));
                    }
                }
                else
                {
                    //SetLocalMailingAddress();
                    //if (GetAddressFromDialog(2))
                    if (GetSelectionOrDeliveryAddress(2))
                    {
                        // Set local values
                        Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                        Voter.Data.PollID = AppSettings.Absentee.SiteID;
                        Voter.Data.ComputerID = AppSettings.System.MachineID;
                        Voter.Data.UserId = AppSettings.User.UserID;

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
            Voter.Data.LogDescription = ("Issued AV Application by Mail").ToUpper();
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
        public RelayCommand _printBallotMailingLabelCommand;
        public ICommand PrintBallotMailingLabelCommand
        {
            get
            {
                if (_printBallotMailingLabelCommand == null)
                {
                    _printBallotMailingLabelCommand = new RelayCommand(param => this.PrintBallotMailingLabelClick(), param => this.CanPrintBallotMailingLabel);
                }
                return _printBallotMailingLabelCommand;
            }
        }

        // Enable or Disable the Print Mailing Labels Button
        public bool CanPrintBallotMailingLabel
        {
            get
            {
                return Voter.Data.LogCode == 6 && Voter.Data.BallotStyleID != null && Voter.Data.PrecinctPartID != null && Voter.Data.PrecinctPartID != "";
            }
        }

        private async void PrintBallotMailingLabelClick()
        {
            if (Voter.Data.LogCode == 6)
            {
                // IF ADDRESS UPDATED THEN GET NEW IMB
                if(Voter.Data.AddressChanged == true)
                {
                    try
                    {
                        IntelligentBarcodeModel imbParameters = new IntelligentBarcodeModel()
                        {
                            Barcode = AppSettings.Absentee.IMBBarCodeId,
                            OutServiceType = AppSettings.Absentee.IMBOutServiceTypeId,
                            InServiceType = AppSettings.Absentee.IMBInServiceTypeId,
                            //MailerId = AppSettings.Absentee.IMBMailerId,
                            OutMailerId = AppSettings.Absentee.IMBOutGoing,
                            InMailerId = AppSettings.Absentee.IMBIncoming,
                            Prefix = AppSettings.Absentee.IMBPrefix,
                            Computer = Math.Abs(AppSettings.System.MachineID),
                            ReturnZip = AppSettings.Absentee.BallotReturnZip
                        };

                        // Get next IMB number
                        if (Voter.GenerateIntelligentBarcodes(imbParameters) == true)
                        {
                            Voter.UpdateAddress();
                        }
                        else
                        {
                            GlobalReferences.StatusBar.TextCenter = ("Error Generating IMB: Length exceeds limit or invalid characters used. ");
                            VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                            ABSLogger.WriteLog("Error Printing IMB Label:");

                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        // Log Exception
                        GlobalReferences.StatusBar.TextCenter = (e.Message);
                        VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                        ABSLogger.WriteLog("Error Printing IMB Label: " + e.Message);
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

                // Print IMB labels
                GlobalReferences.StatusBar.TextCenter = (
                    await PrintMailingLabelsIMBAsync() 
                    );
            }
        }

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
            if (GetSelectionOrDeliveryAddress(3))
            {
                // Print normal labels
                GlobalReferences.StatusBar.TextCenter = (
                await PrintMailingLabelsAsync()
                );
            }
        }

        // ASk what type of labe to pring
        //private async void PrintMailingLabelClick()
        //{
        //    // If a ballot was issued
        //    if (Voter.Data.LogCode == 6)
        //    {
        //        // Ask user if they want to reprint the IMB labels
        //        YesNoDialog newMessage = new YesNoDialog("PrintIMBLabels", "IS THIS LABEL FOR MAILING AN ABSENTEE BALLOT?");
        //        if(newMessage.ShowDialog() == true)
        //        {
        //            // Print the existing delivery address and IMBs
        //            GlobalReferences.StatusBar.TextCenter = (
        //            await PrintMailingLabelsIMBAsync()
        //            );                    
        //        }
        //        else
        //        {
        //            // Ask which address to use
        //            if (GetAddressFromDialog(3))
        //            {
        //                // Print normal labels
        //                GlobalReferences.StatusBar.TextCenter = (
        //                await PrintMailingLabelsAsync()
        //                );
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // Ask which address to use
        //        if (GetAddressFromDialog(3))
        //        {
        //            // Print normal labels
        //            GlobalReferences.StatusBar.TextCenter = (
        //            await PrintMailingLabelsAsync()
        //            );
        //        }
        //    }
        //}

        private async Task<string> PrintMailingLabelsAsync()
        {
            return await Task.Run(() => { return ReportPrintingMethods.PrintVoterMailingLabels(Voter.Data); });
        }

        private async Task<string> PrintMailingLabelsIMBAsync()
        {
            return await Task.Run(() => { return ReportPrintingMethods.PrintVoterMailingLabelsIMB(Voter.Data); });
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
                    _printBallotBundleCommand = new RelayCommand(param => this.PrintBallotBundleClick(), param => this.CanPrintBallotBundle);
                }
                return _printBallotBundleCommand;
            }
        }

        // Enable or Disable the Print Ballot Button 
        public bool CanPrintBallotBundle
        {
            get
            {
                return Voter.Data.LogCode <= 5 && Voter.Data.BallotStyleID != null && Voter.Data.PrecinctPartID != null && Voter.Data.PrecinctPartID != "" && ((VoterX.App)Application.Current).Election.BallotProcessingWindow();
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

                //if (GetAddressFromDialog(1))
                if (GetSelectionOrDeliveryAddress(1))
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
                            if (AppSettings.User.UserName != "Eddy")
                            {
                                YesNoDialog applicationQuestion = new YesNoDialog("Extra Application", "WOULD YOU LIKE TO PRINT AN APPLICATION\r\nALONG WITH THIS BALLOT?");
                                if (applicationQuestion.ShowDialog() == true)
                                {
                                    BallotPrinting.PrintAbsenteeApplicationPre(Voter.Data);
                                }
                            }
                            // Add EDDY fix here
                            PrintMailBundleAndIssueApplication();  
                        }
                    }
                    else
                    {
                        YesNoDialog newMessage = new YesNoDialog("No Application", "THIS VOTER'S APPLICATION WILL BE ACCEPTED\r\nAND A BALLOT WILL BE PRINTED.\r\nARE YOU SURE YOU WANT TO CONTINUE?");
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

                    try
                    {
                        IntelligentBarcodeModel imbParameters = new IntelligentBarcodeModel()
                        {
                            Barcode = AppSettings.Absentee.IMBBarCodeId,
                            OutServiceType = AppSettings.Absentee.IMBOutServiceTypeId,
                            InServiceType = AppSettings.Absentee.IMBInServiceTypeId,
                            //MailerId = AppSettings.Absentee.IMBMailerId,
                            OutMailerId = AppSettings.Absentee.IMBOutGoing,
                            InMailerId = AppSettings.Absentee.IMBIncoming,
                            Prefix = AppSettings.Absentee.IMBPrefix,
                            Computer = Math.Abs(AppSettings.System.MachineID),
                            ReturnZip = AppSettings.Absentee.BallotReturnZip
                        };

                        // Get next IMB number
                        if (Voter.GenerateIntelligentBarcodes(imbParameters) == true)
                        {
                            // Display changes
                            Voter.Data.LogCode = 6;
                            Voter.Data.LogDescription = ("Issued Absentee Ballot by Mail").ToUpper();
                            Voter.Data.ActivityDate = DateTime.Now;
                            Voter.Data.ActivityCode = "A";
                            Voter.Data.LogDate = DateTime.Now;
                            Voter.Data.ComputerID = AppSettings.System.MachineID;
                            Voter.Data.PollID = AppSettings.Absentee.SiteID;
                            Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                            Voter.Data.UserId = AppSettings.User.UserID;
                            Voter.Data.AbsenteeType = "R";
                            Voter.Data.BallotNumber = null;
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

                            Voter.IssueBallot();

                            RaisePropertyChanged("Voter");
                            RaisePropertyChanged("SelectedLogCodeItem");
                            RaisePropertyChanged("SelectedLocationItem");
                            RaisePropertyChanged("SelectedVoterTypesItem");
                        }
                        //else
                        //{
                        //    GlobalReferences.StatusBar.TextCenter = ("Error Generating IMB: Length exceeds ");
                        //    VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                        //    ABSLogger.WriteLog("Error Issuing Ballot:");

                        //    return;
                        //}
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
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintVoterMailingLabelsIMB(Voter.Data));
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
                    
                    try
                    {
                        IntelligentBarcodeModel imbParameters = new IntelligentBarcodeModel()
                        {
                            Barcode = AppSettings.Absentee.IMBBarCodeId,
                            OutServiceType = AppSettings.Absentee.IMBOutServiceTypeId,
                            InServiceType = AppSettings.Absentee.IMBInServiceTypeId,
                            //MailerId = AppSettings.Absentee.IMBMailerId,
                            OutMailerId = AppSettings.Absentee.IMBOutGoing,
                            InMailerId = AppSettings.Absentee.IMBIncoming,
                            Prefix = AppSettings.Absentee.IMBPrefix,
                            Computer = Math.Abs(AppSettings.System.MachineID),
                            ReturnZip = AppSettings.Absentee.BallotReturnZip
                        };

                        // Get next IMB number
                        if (Voter.GenerateIntelligentBarcodes(imbParameters) == true)
                        {
                            // If application has already been accepted dont update the date
                            if(Voter.Data.LogCode != 5)
                            {
                                // Mark application accepted today
                                Voter.Data.ApplicationAccepted = DateTime.Now;
                            }

                            // Display changes
                            Voter.Data.LogCode = 6;
                            Voter.Data.LogDescription = "Issued Absentee Ballot by Mail";
                            Voter.Data.ActivityDate = DateTime.Now;
                            Voter.Data.ActivityCode = "A";
                            Voter.Data.LogDate = DateTime.Now;
                            Voter.Data.ComputerID = AppSettings.System.MachineID;
                            Voter.Data.PollID = AppSettings.Absentee.SiteID;
                            Voter.Data.ElectionID = AppSettings.Election.ElectionID;
                            Voter.Data.UserId = AppSettings.User.UserID;
                            Voter.Data.AbsenteeType = "R";
                            Voter.Data.BallotNumber = null;

                            var electionRecord = ElectionDataMethods.Election.Lists.Election;
                            if (issuedDate < electionRecord.AbsenteeBeginDate)
                            {
                                Voter.Data.BallotPrinted = (DateTime)electionRecord.AbsenteeBeginDate;
                            }
                            else
                            {
                                Voter.Data.BallotPrinted = DateTime.Now;
                            }

                            Voter.IssueBallot();

                            RaisePropertyChanged("Voter");
                            RaisePropertyChanged("SelectedLogCodeItem");
                            RaisePropertyChanged("SelectedLocationItem");
                            RaisePropertyChanged("SelectedVoterTypesItem");
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
                                //StatusBar.ApplicationStatusCenter(e.InnerException.InnerException.Message);
                                ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                            }
                        }

                        return;
                    }

                    // Print Mailing Labels
                    for (int i = 0; i < AppSettings.Ballots.LabelCount; i++)
                    {
                        GlobalReferences.StatusBar.TextCenter = (ReportPrintingMethods.PrintVoterMailingLabelsIMB(Voter.Data));
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
                    _batchVoterCommand = new RelayCommand(param => this.BatchVoterClick(), param => this.CanBatchVoter);
                }
                return _batchVoterCommand;
            }
        }

        // Enable or Disable the Queue Voter For Batch Button  
        public bool CanBatchVoter
        {
            get
            {
                return Voter.Data.LogCode < 5 && Voter.Data.BallotStyleID != null && Voter.Data.PrecinctPartID != null && Voter.Data.PrecinctPartID != "" && ((VoterX.App)Application.Current).Election.BallotProcessingWindow();
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
            else if (Voter.HasRejectedApplication())
            {
                YesNoDialog newMessage = new YesNoDialog("Rejected Application", "THIS VOTER HAS A REJECTED APPLICATION\r\nDO YOU WANT TO ACCEPT THE APPLICATION AT THIS TIME?");
                if (newMessage.ShowDialog() == true)
                {
                    Voter.Data.ApplicationIssued = acceptedDate;

                    QueueBallot(acceptedDate);
                }
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
            //if (GetAddressFromDialog(1)) 
            if (GetSelectionOrDeliveryAddress(1))
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
                    ABSLogger.WriteLog("Error Queue For Batch: " + e.Message);
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
                    _spoilBallotCommand = new RelayCommand(param => this.SpoilBallotClick(), param => this.CanSpoilBallot);
                }
                return _spoilBallotCommand;
            }
        }

        // Enable or Disable the Spoil Ballot Button  
        public bool CanSpoilBallot
        {
            get
            {
                return Voter.Data.LogCode == 6 || Voter.Data.LogCode == 14;
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
                    //if (GetAddressFromDialog(1))
                    if (GetSelectionOrDeliveryAddress(1))
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
                    _rejectApplicationCommand = new RelayCommand(param => this.RejectedApplicationClick(), param => this.CanRejectApplication);
                }
                return _rejectApplicationCommand;
            }
        }

        // Enable or Disable the Reject Application Button
        public bool CanRejectApplication
        {
            get
            {
                var test = Voter.Data.ApplicationIssued;
                return Voter.Data.ApplicationIssued != null && !Voter.HasBeenIssued();
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
                    _printProvisionalBallotCommand = new RelayCommand(param => this.PrintProvisionalBallotClick(), param => this.CanPrintProvisional);
                }
                return _printProvisionalBallotCommand;
            }
        }

        public bool CanPrintProvisional
        {
            get
            {
                return ((VoterX.App)Application.Current).Election.BallotProcessingWindow();
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

                // Flag inelligble voters 
                if (Voter.Data.BallotStyleID == null)
                {
                    Voter.Data.ComboNo = 1;
                }
                else
                {
                    Voter.Data.DistrictName = Voter.Data.BallotStyleFile;
                    Voter.Data.ComboNo = Voter.Data.BallotStyleID;
                }

                // Navigate to provisional page
                NavigationMenuMethods.ProvisionalBallotPage(Voter);                
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Invalid Password");
            }
        }
        #endregion

        #region RejectBallotCommands
        public RelayCommand _rejectBallotCommand;
        public ICommand RejectBallotCommand
        {
            get
            {
                if (_rejectBallotCommand == null)
                {
                    _rejectBallotCommand = new RelayCommand(param => this.RejectedBallotClick(), param => this.CanRejectBallot);
                }
                return _rejectBallotCommand;
            }
        }

        // Enable or Disable the Reject Ballot Button
        public bool CanRejectBallot
        {
            get
            {
                return Voter.Data.LogCode == 6 || Voter.Data.LogCode == 9 || Voter.Data.LogCode == 10;
            }
        }

        // Enable or Disable the Reject Ballot command
        //private bool _canRejectBallot;
        //public bool CanRejectBallot
        //{
        //    get
        //    {
        //        if(Voter.Data.LogCode == 6 || Voter.Data.LogCode == 9)
        //        {
        //            _canRejectBallot = true;
        //        }
        //        else
        //        {
        //            _canRejectBallot = false;
        //        }
        //        return _canRejectBallot;
        //    }
        //    internal set
        //    {
        //        _canRejectBallot = value;
        //        RaisePropertyChanged("CanRejectBallot");
        //    }
        //}

        private void RejectedBallotClick()
        {
            // Check if voter has already voted
            if (Voter.Data.LogCode < 6)
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER HAS NOT BEEN ISSUED A BALLOT");
                newMessage.ShowDialog();
            }
            // Check for existing ballot
            else if (Voter.Data.LogCode > 6 && Voter.Data.LogCode != 10 && Voter.Data.LogCode != 9 && Voter.Data.LogCode != 12 && Voter.Data.LogCode != 14)
            {
                AlertDialog newMessage = new AlertDialog("THIS VOTER ALREADY HAS AN ACCEPTED BALLOT");
                newMessage.ShowDialog();
            }
            // Only allow 6 or 10 or 9
            else
            {
                // Reset Menu
                //MainMenuMethods.LoadMenu(new VoterX.AbsenteeMenu());
                GlobalReferences.MenuSlider.SetMenu(new VoterX.AbsenteeMenu(), MenuCollapseMode.Full);

                //MainMenuMethods.SetMenuMinWidth(0);

                MainFrameMethods.NavigateToPage(new VoterX.AbsenteeRejectedBallotsPage(Voter, SERVISSource));
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
                        Voter.Data.LogDescription = ("Early Voting").ToUpper();
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

        #region IntelligentBarcode
        public RelayCommand _generateBarcodeCommand;
        public ICommand GenerateBarcodeCommand
        {
            get
            {
                if (_generateBarcodeCommand == null)
                {
                    _generateBarcodeCommand = new RelayCommand(param => this.GenerateBarcodeClick());
                }
                return _generateBarcodeCommand;
            }
        }

        private void GenerateBarcodeClick()
        {
            IntelligentBarcodeModel imbParameters = new IntelligentBarcodeModel()
            {
                Barcode = AppSettings.Absentee.IMBBarCodeId,
                OutServiceType = AppSettings.Absentee.IMBOutServiceTypeId,
                InServiceType = AppSettings.Absentee.IMBInServiceTypeId,
                //MailerId = AppSettings.Absentee.IMBMailerId,
                OutMailerId = AppSettings.Absentee.IMBOutGoing,
                InMailerId = AppSettings.Absentee.IMBIncoming,
                Prefix = AppSettings.Absentee.IMBPrefix,
                Computer = Math.Abs(AppSettings.System.MachineID),
                ReturnZip = AppSettings.Absentee.BallotReturnZip
            };

            Voter.GenerateIntelligentBarcodes(imbParameters);

            var outGoing = Voter.Data.OutGoingIMB;
            var inComing = Voter.Data.InComingIMB;
        }
        #endregion

        #region ContactInfoCommands
        public RelayCommand _updateContactInfoCommand;
        public ICommand UpdateContactInfoCommand
        {
            get
            {
                if (_updateContactInfoCommand == null)
                {
                    _updateContactInfoCommand = new RelayCommand(param => this.UpdateContactInfoClick());
                }
                return _updateContactInfoCommand;
            }
        }

        private void UpdateContactInfoClick()
        {
            //Voter.UpdateContact();
            //var test = Voter.Data.Phone;
            //var em = Voter.Data.Email;
        }
        #endregion
    }
}
