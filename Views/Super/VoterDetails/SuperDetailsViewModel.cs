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
using VoterX.Manager.Models;

namespace VoterX.Manager.Views.Absentee
{
    public class SuperDetailsViewModel : NotifyPropertyChanged
    {
        public NMVoter Voter { get; set; }

        private bool SERVISSource = false;

        public SuperDetailsViewModel(NMVoter voter) : this(voter, false) { }
        public SuperDetailsViewModel(NMVoter voter, bool source)
        {
            Voter = voter;
        }

        #region VoterHistoryList
        private List<VoterHistoryDisplayModel> _voterHistoryList;
        public List<VoterHistoryDisplayModel> VoterHistoryList
        {
            get
            {
                if (_voterHistoryList == null)
                {
                    _voterHistoryList = Voter.HistoryList()
                        .Select(h => new VoterHistoryDisplayModel()
                        {
                            HistoryTitle = h.LogCode.ToString() + " " + ParseHistoryAction(h.HistoryAction.ToUpper()) + " " + h.HistoryDate.ToString(),
                            LogCode = h.LogCode,
                            HistoryDate = h.HistoryDate,
                            HistoryId = h.VotedRecordHistoryId
                        })
                        .OrderByDescending(h => h.HistoryDate)
                        .ToList();
                }
                return _voterHistoryList;
            }
        }

        // Does not remove extra space at the end
        private string ParseHistoryAction(string action)
        {
            int index = action.IndexOf("(");
            if (index >= 0)
                return action.Substring(0, index);
            else
                return action;
        }

        private VoterHistoryDisplayModel _selectedVoterHistoryItem;
        public VoterHistoryDisplayModel SelectedVoterHistoryItem
        {
            get
            {
                if (VoterHistoryList != null)
                {
                    _selectedVoterHistoryItem = VoterHistoryList.Where(h => h.LogCode == Voter.Data.LogCode).OrderByDescending(h => h.HistoryDate).FirstOrDefault();
                }
                return _selectedVoterHistoryItem;
            }

            set
            {
                _selectedVoterHistoryItem = value;

                Voter.RestoreHistory(_selectedVoterHistoryItem.HistoryId);

                RaisePropertyChanged("Voter");
                RaisePropertyChanged("SelectedPrecinctPartItem");
                RaisePropertyChanged("SelectedBallotStyleItem");
                RaisePropertyChanged("SelectedPartyItem");
                RaisePropertyChanged("SelectedLogCodeItem");
                RaisePropertyChanged("SelectedLocationItem");
            }
        }
        #endregion

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
            NavigationMenuMethods.SuperSearchPage();
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

        #region EditVoterCommands
        private RelayCommand _saveChangesCommand;
        public ICommand SaveChangesCommand
        {
            get
            {
                if (_saveChangesCommand == null)
                {
                    _saveChangesCommand = new RelayCommand(param => this.SaveChangesClick(), param => this.CanSaveChanges);
                }
                return _saveChangesCommand;
            }
        }

        public bool CanSaveChanges
        {
            get
            {
                return true;
            }
        }

        private void SaveChangesClick()
        {
            var test = Voter;
        }

        private RelayCommand _revertHistoryCommand;
        public ICommand RevertHistoryCommand
        {
            get
            {
                if (_revertHistoryCommand == null)
                {
                    _revertHistoryCommand = new RelayCommand(param => this.RevertHistoryClick(), param => this.CanRevertHistory);
                }
                return _revertHistoryCommand;
            }
        }

        public bool CanRevertHistory
        {
            get
            {
                if (Voter.Data.LogCode != 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void RevertHistoryClick()
        {
            YesNoDialog newMessage = new YesNoDialog("Are You Sure", "THIS WILL REVERT THE VOTER'S STATUS BACK TO " + SelectedLogCodeItem.LogDescription.ToUpper() + "\rAND CHANGE THEIR ACTIVITY DATES\r\n\r\nARE YOU SURE YOU WANT CONTINUE?");
            if (newMessage.ShowDialog() == true)
            {
                if(Voter.Data.LogCode < 9)
                {
                    Voter.RemoveFromScanHistory();
                }
                Voter.Data.LocationID = AppSettings.System.SiteID;
                Voter.SaveChanges("Manually Reverted To Previous State");
            }
        }

        private RelayCommand _markBackCommand;
        public ICommand MarkBackCommand
        {
            get
            {
                if (_markBackCommand == null)
                {
                    _markBackCommand = new RelayCommand(param => this.MarkBackClick(), param => this.CanMarkBack);
                }
                return _markBackCommand;
            }
        }

        public bool CanMarkBack
        {
            get
            {
                if (Voter.Data.LogCode > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //return true;
            }
        }

        private void MarkBackClick()
        {
            YesNoDialog newMessage = new YesNoDialog("Are You Sure", "THIS WILL REVERT THE VOTER'S STATUS BACK TO REGISTERED TO VOTE\rAND ERASE THEIR ACTIVITY DATES\r\n\r\nARE YOU SURE YOU WANT CONTINUE?");
            if (newMessage.ShowDialog() == true)
            {
                Voter.Data.LogCode = 1;
                Voter.Data.VotedDate = null;
                Voter.Data.BallotIssued = null;
                Voter.Data.BallotPrinted = null;
                Voter.Data.ActivityCode = null;
                Voter.Data.NotTabulated = false;
                Voter.Data.CreatedOnDate = DateTime.MinValue;

                Voter.Data.ApplicationIssued = null;
                Voter.Data.ApplicationAccepted = null;
                Voter.Data.ApplicationRejected = null;
                Voter.Data.ApplicationRejectedReason = null;

                Voter.Data.AddressType = null;
                Voter.Data.DeliveryAddress1 = null;
                Voter.Data.DeliveryAddress2 = null;
                Voter.Data.DeliveryCity = null;
                Voter.Data.DeliveryState = null;
                Voter.Data.DeliveryZip = null;
                Voter.Data.DeliveryCountry = null;
                Voter.Data.TempAddress = false;

                Voter.Data.BallotRejectedDate = null;
                Voter.Data.BallotRejectedReason = null;

                Voter.Data.OutGoingIMB = null;
                Voter.Data.InComingIMB = null;

                Voter.Data.SpoiledReason = null;

                Voter.Data.ActivityDate = DateTime.Now;

                RaisePropertyChanged("Voter");
                RaisePropertyChanged("SelectedPrecinctPartItem");
                RaisePropertyChanged("SelectedBallotStyleItem");
                RaisePropertyChanged("SelectedPartyItem");
                RaisePropertyChanged("SelectedLogCodeItem");
                RaisePropertyChanged("SelectedLocationItem");

                Voter.Data.LocationID = AppSettings.System.SiteID;

                Voter.SaveChanges("Manually Reverted To Registered To Vote");
            }
        }
        #endregion
    }
}
