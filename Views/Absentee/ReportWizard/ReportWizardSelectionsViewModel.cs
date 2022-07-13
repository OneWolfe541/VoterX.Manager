using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoterX.Methods;
using VoterX.Models;
using VoterX.Core.Elections;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Views;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Views.Absentee
{
    public class ReportWizardSelectionsViewModel : NotifyPropertyChanged
    {
        public ReportWizardSelectionsViewModel() : this(null) { }
        public ReportWizardSelectionsViewModel(ReportWizardQueryModel wizardSearch)
        {
            // Initialize search parameters
            if (wizardSearch == null)
            {
                wizardSearch = new ReportWizardQueryModel();
            }
            WizardSearch = wizardSearch;
        }

        #region WizardOptions
        private ReportWizardQueryModel _wizardSearch;
        private ReportWizardQueryModel WizardSearch
        {
            get
            {
                return _wizardSearch;
            }

            set
            {
                _wizardSearch = value;
                RaisePropertyChanged("WizardSearch");
                RaisePropertyChanged("InactiveVoters");

            }
        }
        #endregion

        #region CheckBoxes
        public bool InactiveVoters
        {
            get
            {
                return WizardSearch.IncludeInactive;
            }
            set
            {
                WizardSearch.IncludeInactive = value;

                LogCodesEnabled = !WizardSearch.IncludeInactive;
                //RaisePropertyChanged("LogCodesList");

                LocationsEnabled = !WizardSearch.IncludeInactive;
                //RaisePropertyChanged("LocationsList");
            }
        }

        public bool IdRequired
        {
            get
            {
                return WizardSearch.IdRequired;
            }

            set
            {
                WizardSearch.IdRequired = value;
            }
        }
        #endregion

        #region LogCodes
        public bool LogCodesEnabled
        {
            get
            {
                return LogCodesList.IsEnabled;
            }
            set
            {
                LogCodesList.IsEnabled = value;
            }
        }

        private WizardSelectorViewModel _logCodesList;
        public WizardSelectorViewModel LogCodesList
        {
            get
            {
                // Check for empty list
                if(_logCodesList == null)
                {
                    // Load list control with log codes
                    _logCodesList = new WizardSelectorViewModel();
                    _logCodesList.LoadList<LogCodeModel>(ElectionDataMethods.LogCodes, WizardSearch.LogCodes);
                    //_logCodesList.SetSelectedItems<int>(WizardSearch.LogCodes);
                    _logCodesList.ListLabel = "STATUS ENTRY";
                    _logCodesList.ClearLabel = "CLEAR STATUS";

                    _logCodesList.IsEnabled = !WizardSearch.IncludeInactive;

                    _logCodesList.PropertyChanged += OnLogCodeSelectedPropertyChanged;
                }
                return _logCodesList;
            }

            private set
            {
                _logCodesList = value;
                RaisePropertyChanged("LogCodesList");
            }
        }

        // Check which properties changed in the log code list
        private void OnLogCodeSelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsList")
            {
                var items = ((WizardSelectorViewModel)sender).ItemsList;
                if(items != null && WizardSearch != null)
                {
                    // Update wizard search object with selected log codes
                    WizardSearch.LogCodes = ((WizardSelectorViewModel)sender).GetList<int>();
                }
            }
        }
        #endregion

        #region BallotStyles
        public bool BallotStylesEnabled { get; set; }

        private WizardSelectorViewModel _ballotStylesList;
        public WizardSelectorViewModel BallotStylesList
        {
            get
            {
                // Check for empty list
                if (_ballotStylesList == null)
                {
                    // Load list control with ballot styles
                    _ballotStylesList = new WizardSelectorViewModel();
                    _ballotStylesList.LoadList<BallotStyleModel>(ElectionDataMethods.BallotStyles.DistinctBallots(), WizardSearch.BallotStyles);
                    //_logCodesList.SetSelectedItems<int>(WizardSearch.BallotStyles);
                    _ballotStylesList.ListLabel = "BALLOT STYLE";
                    _ballotStylesList.ClearLabel = "CLEAR BALLOT STYLE";

                    _ballotStylesList.PropertyChanged += OnBallotStylesSelectedPropertyChanged;
                }
                return _ballotStylesList;
            }

            private set
            {
                _ballotStylesList = value;
                RaisePropertyChanged("BallotStylesList");
            }
        }

        // Check which properties changed in the log code list
        private void OnBallotStylesSelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsList")
            {
                var items = ((WizardSelectorViewModel)sender).ItemsList;
                if (items != null && WizardSearch != null)
                {
                    // Update wizard search object with selected ballot styles
                    WizardSearch.BallotStyles = ((WizardSelectorViewModel)sender).GetList<int>();
                }
            }
        }
        #endregion

        #region PartiesList
        public bool PartiesEnabled
        {
            get
            {
                return PartyList.IsEnabled;
            }
            set
            {
                PartyList.IsEnabled = value;
            }
        }

        private WizardSelectorViewModel _partyList;
        public WizardSelectorViewModel PartyList
        {
            get
            {
                // Check for empty list
                if (_partyList == null)
                {
                    // Load list control with parties
                    _partyList = new WizardSelectorViewModel();
                    _partyList.LoadList<PartyModel>(ElectionDataMethods.Partys.OrderBy(p => p.PartyCode).ToList(), WizardSearch.Parties);
                    //_logCodesList.SetSelectedItems<string>(WizardSearch.Parties);
                    _partyList.ListLabel = "PARTY";
                    _partyList.ClearLabel = "CLEAR PARTY";

                    _partyList.PropertyChanged += OnPartySelectedPropertyChanged;
                }
                return _partyList;
            }

            private set
            {
                _partyList = value;
                RaisePropertyChanged("PartyList");
            }
        }

        // Check which properties changed in the log code list
        private void OnPartySelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsList")
            {
                var items = ((WizardSelectorViewModel)sender).ItemsList;
                if (items != null && WizardSearch != null)
                {
                    // Update wizard search object with selected parties
                    WizardSearch.Parties = ((WizardSelectorViewModel)sender).GetList<string>();
                }
            }
        }
        #endregion

        #region JurisdictionList
        public bool JurisdictionsEnabled
        {
            get
            {
                return JurisdictionList.IsEnabled;
            }
            set
            {
                JurisdictionList.IsEnabled = value;
            }
        }

        private WizardSelectorViewModel _jurisdictionList;
        public WizardSelectorViewModel JurisdictionList
        {
            get
            {
                // Check for empty list
                if (_jurisdictionList == null)
                {
                    // Load list control with jurisdictions
                    _jurisdictionList = new WizardSelectorViewModel();
                    _jurisdictionList.LoadList<JurisdictionModel>(ElectionDataMethods.Jurisdictions, WizardSearch.Jurisdictions);
                    //_logCodesList.SetSelectedItems<string>(WizardSearch.Jurisdictions);
                    _jurisdictionList.ListLabel = "DISTRICT";
                    _jurisdictionList.ClearLabel = "CLEAR DISTRICT";
                    // Set control to use the categories drop down
                    _jurisdictionList.UseCategories = true;
                    _jurisdictionList.CategoryToolTip = "Select a jurisdiction type";

                    _jurisdictionList.PropertyChanged += OnJurisdictionSelectedPropertyChanged;
                }
                return _jurisdictionList;
            }

            private set
            {
                _jurisdictionList = value;
                RaisePropertyChanged("PartyList");
            }
        }

        // Check which properties changed in the log code list
        private void OnJurisdictionSelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsList")
            {
                var items = ((WizardSelectorViewModel)sender).ItemsList;
                if (items != null && WizardSearch != null)
                {
                    // Update wizard search object with the selected jurisdictions
                    WizardSearch.Jurisdictions = ((WizardSelectorViewModel)sender).GetList<string>();
                }
            }
        }
        #endregion

        #region LocationsList
        public bool LocationsEnabled
        {
            get
            {
                return LocationList.IsEnabled;
            }
            set
            {
                LocationList.IsEnabled = value;
            }
        }

        private WizardSelectorViewModel _locationList;
        public WizardSelectorViewModel LocationList
        {
            get
            {
                // Check for empty list
                if (_locationList == null)
                {
                    // Load the list control with locations
                    _locationList = new WizardSelectorViewModel();
                    _locationList.LoadList<LocationModel>(ElectionDataMethods.Locations.OrderBy(l => l.PlaceName).ToList(), WizardSearch.PollSites);
                    _locationList.ListLabel = "SITE/LOCATION";
                    _locationList.ClearLabel = "CLEAR SITE";

                    _locationList.IsEnabled = !WizardSearch.IncludeInactive;

                    _locationList.PropertyChanged += OnLocationSelectedPropertyChanged;
                }
                return _locationList;
            }

            private set
            {
                _locationList = value;
                RaisePropertyChanged("LocationList");
            }
        }

        // Check which properties changed in the log code list
        private void OnLocationSelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsList")
            {
                var items = ((WizardSelectorViewModel)sender).ItemsList;
                if (items != null && WizardSearch != null)
                {
                    // Update the wizard search object with the selected locations
                    WizardSearch.PollSites = ((WizardSelectorViewModel)sender).GetList<int>();
                }
            }
        }
        #endregion

        #region Navigation
        // Bound command for returning to the previous screen
        public RelayCommand _previousCommand;
        public ICommand PreviousCommand
        {
            get
            {
                if (_previousCommand == null)
                {
                    _previousCommand = new RelayCommand(param => this.PreviousClick());
                }
                return _previousCommand;
            }
        }

        // Force parent frame to navigate back to the previous page
        public void PreviousClick()
        {
            NavigationMenuMethods.ReportWizardDates(WizardSearch);
        }

        // Bound command for navigating to the next screen
        public RelayCommand _nextCommand;
        public ICommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                {
                    _nextCommand = new RelayCommand(param => this.NextClick());
                }
                return _nextCommand;
            }
        }

        // Force parent frame to navigate to the next page
        public void NextClick()
        {
            NavigationMenuMethods.ReportWizardPrinting(WizardSearch);
        }
        #endregion
    }
}
