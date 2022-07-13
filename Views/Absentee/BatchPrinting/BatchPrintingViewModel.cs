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
using VoterX.ABS.Logging;
using VoterX.Methods;
using VoterX.Core.Elections;
using VoterX.Core.Voters;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Dialogs;
using VoterX.Utilities.Models;
using VoterX.Utilities.Views;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;
using VoterX.Manager.Reporting;

namespace VoterX.Manager.Views.Absentee
{
    public class BatchPrintingViewModel : NotifyPropertyChanged
    {
        // Unfiltered list of all voter ballots
        ObservableCollection<NMVoter> _voters;

        private int? _siteId;
        private int? _machineId;
        private int? _userId;

        private int _pageSize = 48;

        private BatchFactory _batchFactory;
        private Guid? _currentBatch;

        public BatchPrintingViewModel() : this(null) { }
        public BatchPrintingViewModel(int? siteId)
        {
            // Disable menu slider
            GlobalReferences.MenuSlider.CollapseMode = MenuCollapseMode.None;

            VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);

            // Switch between using the local site and a specific site
            // Used for loading ballots queued from SERVIS
            if (siteId == null)
            {
                // Local batch mode
                _siteId = AppSettings.Absentee.SiteID;
                _machineId = AppSettings.System.MachineID;
                _userId = AppSettings.System.MachineID;

                InitializeGridVisible = true;
                CanInitialize = true;
                CanPrintReport = false;
            }
            else
            {
                // SERVIS batch mode
                _siteId = siteId;
                _machineId = null;
                _userId = AppSettings.System.MachineID;

                InitializeGridVisible = true;
                CanInitialize = true;
                CanPrintReport = false;
            }

            // Load last active batch
            try
            {
                // Create new batch factory
                _batchFactory = new BatchFactory(((App)Application.Current).Connection);

                // Get active batch
                var batch = _batchFactory.GetCurrentBatch(_siteId, AppSettings.System.MachineID);
                if (batch != null) _currentBatch = batch.BatchId;
            }
            catch (Exception e)
            {
                ABSLogger.WriteLog("Error Loading Previous Batch: " + e.Message);
                if (e.InnerException != null)
                {
                    //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                    ABSLogger.WriteLog(e.InnerException.Message);
                    if (e.InnerException.InnerException != null)
                    {
                        ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                    }
                }

                CanCancel = true;

                return;
            }


            // Preload the list of voters
            try
            {
                SearchVoters();
            }
            catch (Exception e)
            {
                // Log Exception
                GlobalReferences.StatusBar.TextCenter = (e.Message);
                
                ABSLogger.WriteLog("Error Loading Batched Voters: " + e.Message);
                if (e.InnerException != null)
                {
                    //StatusBar.ApplicationStatusCenter(e.InnerException.Message);
                    ABSLogger.WriteLog(e.InnerException.Message);
                    if (e.InnerException.InnerException != null)
                    {
                        ABSLogger.WriteLog(e.InnerException.InnerException.Message);
                    }
                }

                CanCancel = true;

                return;
            }

            // Display Header
            GlobalReferences.Header.PageHeader = ("Print Mail-In Ballots");

            // Load Batch State
            // If the report was already printed today then enable the print buttons
            if (AppSettings.Absentee.BatchReportPrinted == true && AppSettings.Absentee.BatchReportPrintedDate.Date == DateTime.Now.Date)
            {
                CanPrintLabels = true;
                CanPrintBallots = true;
            }

            // Cancel button should always be enabled
            CanCancel = true;
        }

        #region DisplayProperties
        public bool FunctionsGridVisible { get; set; }
        public bool InitializeGridVisible { get; set; }

        // Get count of all queued ballot styles
        public int BallotStyleCount
        {
            get
            {
                // List is not null
                if(_voters != null && VoterList != null && VoterList.VoterList != null)
                {
                    return _voters.GroupBy(v => v.Data.BallotStyleID).Count();
                }
                else
                {
                    return 0;
                }
            }

            // Property is Read Only
            //private set
            //{
            //    BallotStyleCount = value;
            //}
        }

        // Get count of all queued ballots
        public int TotalBallotCount
        {
            get
            {
                // List is not null
                if (VoterList != null && VoterList.VoterList != null)
                {
                    return VoterList.VoterList.Count();
                }
                else
                {
                    return 0;
                }
            }

            // Property is Read Only
            //private set
            //{
            //    TotalBallotCount = value;
            //}
        }

        // Total count of voters with the slected ballot style
        public int SelectedBallotCount
        {
            get
            {
                // List is not null
                if(_voters != null && SelectedBallotStyleItem != null)
                {
                    return _voters.Where(v => v.Data.BallotStyleID == SelectedBallotStyleItem.BallotStyleId).Count();
                }
                else
                {
                    return 0;
                }
            }
        }

        // Get all queued ballot style objects
        private List<BallotStyleModel> BallotStyles 
        {
            get
            {
                // List is not null
                if (_voters != null)
                {
                    // Get the list of queued ballot style ids from the voter list
                    List<int?> styleIds = _voters.Select(v => v.Data.BallotStyleID).Distinct().ToList();

                    // Return the list of distinct ballot style objects with matching ids
                    return ElectionDataMethods.Election.Lists.BallotStyles.DistinctBallots()
                        .Where(bs => styleIds.Contains(bs.BallotStyleId))
                        .ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        private int? _totalQueued;
        public int? TotalQueued
        {
            get
            {
                if (_voters != null && _voters.Count() > 0)
                {
                    _totalQueued = _voters.Count();
                }
                else
                {
                    _totalQueued = 0;
                }
                return _totalQueued;
            }
            set
            {
                _totalQueued = value;
                RaisePropertyChanged("TotalQueued");
            }
        }
        #endregion

        #region VoterList
        // Create voter list control view model and initialize its settings
        private VoterListViewModel _voterList;
        public VoterListViewModel VoterList 
        {
            get
            {
                // List is not null
                if (_voterList == null)
                {
                    // Create the view model
                    _voterList = new VoterListViewModel(AppSettings.Global);

                    // Turn off the party field
                    _voterList.PartyVisibility = false;

                    // Turn on the ballot style field
                    _voterList.BallotStyleVisibility = true;

                    // Set the display colors to balck
                    _voterList.SetLogCodeColors(Brushes.Black);

                    // Add the property changed method
                    _voterList.PropertyChanged += OnVoterSelectedPropertyChanged;
                }
                return _voterList;
            }

            private set
            {
                _voterList = value;
            }
        }

        // Check for property changes in the voter list
        private void OnVoterSelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == "SelectedVoter")
            //{
            //    //Code to respond to a change in the ViewModel
            //    Console.WriteLine(
            //        "Selected Voter: " +
            //        ((VoterListViewModel)sender).SelectedVoter.Data.VoterID.ToString() + " | STATUS: " +
            //        ((VoterListViewModel)sender).SelectedVoter.CheckStatus((int)AppSettings.System.SiteID));

            //    // Get selected voter
            //    var selectedVoter = ((VoterSearchViewModel)sender).SelectedVoter;

            //    // Display Delete Button
            //}
            //else if (e.PropertyName == "VoterList")
            //{
            //    if (((VoterListViewModel)sender).VoterList != null)
            //    {
            //        EnvelopeCount = ((VoterListViewModel)sender).VoterList.Count();
            //        RaisePropertyChanged("EnvelopeCount");
            //    }
            //}
        }
        #endregion

        #region BallotStyles
        public bool IsBallotStyleEnabled { get; set; }

        // Load the list of currently queued ballot styles
        private List<BallotStyleModel> _ballotStyleList;
        public List<BallotStyleModel> BallotStyleList 
        {
            get
            {
                // Check if list is empty
                if (_ballotStyleList == null)
                {
                    // Only load list if batch printing mode is SINGLE
                    if (AppSettings.Absentee.BatchPrintingMode != "ALL")
                    {
                        // Get the list of ballots 
                        if(BallotStyles != null)
                        {
                            _ballotStyleList = BallotStyles;
                        }

                        // Enable the drop down list
                        IsBallotStyleEnabled = true;
                        RaisePropertyChanged("IsBallotStyleEnabled");
                    }
                    else
                    {
                        // Create a default item if batch printing mode is ALL
                        _ballotStyleList = new List<BallotStyleModel>();
                        BallotStyleModel allBallots = new BallotStyleModel();
                        allBallots.BallotStyleName = "ALL";
                        allBallots.BallotStyleId = 0;

                        _ballotStyleList.Add(allBallots);

                        // Disable the drop down list
                        IsBallotStyleEnabled = false;
                        RaisePropertyChanged("IsBallotStyleEnabled");
                    }
                }
                return _ballotStyleList;
            }
            set
            {
                _ballotStyleList = value;
                RaisePropertyChanged("BallotStyleList");
            }
        }

        // Manage the selection of ballot styles
        private BallotStyleModel _selectedBallotStyleItem; 
        public BallotStyleModel SelectedBallotStyleItem
        {
            get
            {
                // Default the selected item to the first in the list
                if(BallotStyleList != null && _selectedBallotStyleItem == null)
                {
                    _selectedBallotStyleItem = BallotStyleList.FirstOrDefault();
                }
                if (_selectedBallotStyleItem == null)
                {
                    // Log Exception
                    GlobalReferences.StatusBar.TextCenter = ("Selected ballot style not found");
                    VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                    ABSLogger.WriteLog("Selected ballot style not found");

                    _selectedBallotStyleItem = new BallotStyleModel();
                }
                return _selectedBallotStyleItem;
            }
            set
            {
                _selectedBallotStyleItem = value;

                SearchVoters();
            }
        }
        #endregion

        #region SearchVoters
        // Search voters has to load all the queued voters then populate the ballot styles
        // and finally display the list of voters in the selected ballot style
        public async void SearchVoters()
        {
            // Change cursor
            Mouse.OverrideCursor = Cursors.Wait;

            // Reset the list control
            VoterList.ClearList();

            // Get all queued voters only once
            if (_voters == null)
            {
                try 
                {
                    //_voters = VoterMethods.Voters.BatchList(
                    //        (int)_siteId,
                    //        _machineId,
                    //        5,
                    //        _userId
                    //        );

                    _voters = await Task.Run(()=> VoterMethods.Voters.BatchList(_currentBatch, (int)_siteId, 5) );
                }
                catch (Exception e)
                {
                    // Log Exception
                    GlobalReferences.StatusBar.TextCenter = (e.Message);
                    VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                    ABSLogger.WriteLog("Error Loading Batched Voters: " + e.Message);
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

                try
                {
                    _voters.Localize(
                        AppSettings.Election.ElectionID,
                        (int)AppSettings.Absentee.SiteID,
                        AppSettings.System.MachineID,
                        AppSettings.User.UserID
                        );
                }
                catch (Exception e)
                {
                    // Log Exception
                    GlobalReferences.StatusBar.TextCenter = (e.Message);
                    VoterXLogger ABSLogger = new VoterXLogger("ABSLogs", AppSettings.System.ReportErrorLogging);
                    ABSLogger.WriteLog("Error Localizing Voters: " + e.Message);
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

            // Choose how to display the voter list
            if (AppSettings.Absentee.BatchPrintingMode == "ALL")
            {
                // Get all queued voters
                //var voters = VoterMethods.Voters.List(
                //    (int)AppSettings.System.SiteID,
                //    AppSettings.System.MachineID,
                //    5,
                //    null
                //    );

                // Check if the list is empty
                if (_voters != null && _voters.Count() > 0)
                {
                    // Add the voters to the list
                    var sortedVoters = new ObservableCollection<NMVoter>(
                        _voters
                        .OrderBy(o => o.Data.BallotStyle)
                        .ThenBy(o => o.Data.LastName)
                        .ThenBy(o => o.Data.FirstName)
                        .Take(_pageSize)
                        .ToList()
                        );
                    VoterList.AddVoter(sortedVoters);

                    // Turn on the Print Functions Grid
                    FunctionsGridVisible = true;
                    RaisePropertyChanged("FunctionsGridVisible");

                    // Update Counts
                    RaisePropertyChanged("BallotStyleCount");
                    RaisePropertyChanged("TotalBallotCount");
                    RaisePropertyChanged("TotalQueued");
                }
            }
            else
            {
                // Get all queued voters by ballot style
                //var voters = VoterMethods.Voters.List(
                //    (int)AppSettings.System.SiteID,
                //    AppSettings.System.MachineID,
                //    5
                //    // styleId
                //    );

                // Check if the list is empty
                if (_voters != null && _voters.Count() > 0)
                {
                    // Get only the voters with the selected ballot style
                    var voters = new ObservableCollection<NMVoter>(_voters.Where(v => v.Data.BallotStyleID == SelectedBallotStyleItem.BallotStyleId).ToList());

                    // Check if the filtered list is empty
                    if (voters != null && voters.Count() > 0)
                    {
                        // Add the voters to the list
                        VoterList.AddVoter(voters);

                        // Turn on the Print Functions Grid
                        FunctionsGridVisible = true;
                        RaisePropertyChanged("FunctionsGridVisible");

                        // Update Counts
                        RaisePropertyChanged("BallotStyleCount");
                        RaisePropertyChanged("TotalBallotCount");
                    }
                }
            }

            // Change cursor back
            Mouse.OverrideCursor = null;
        }
        #endregion

        #region BatchReport
        // Bound command for printing the full batch report
        private RelayCommand _printBatchReportCommand;
        public ICommand PrintBatchReportCommand
        {
            get
            {
                if (_printBatchReportCommand == null)
                {
                    _printBatchReportCommand = new RelayCommand(param => this.PrintBatchReportClick(), param => this.CanPrintReport);
                }
                return _printBatchReportCommand;
            }
        }

        // Enable or Disable report printing
        private bool _canPrintReport;
        public bool CanPrintReport
        {
            get { return _canPrintReport; }
            internal set
            {
                _canPrintReport = value;
                RaisePropertyChanged("CanPrintReport");
            }
        }

        // Print the report
        private async void PrintBatchReportClick()
        {
            // Save batch state
            AppSettings.Absentee.BatchReportPrinted = true;
            AppSettings.Absentee.BatchReportPrintedDate = DateTime.Now;
            AppSettings.SaveAbsentee();

            // Print the report and display any errors
            //GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => BatchPrintingMethods.PrintBatchDetails(
            //    AppSettings.Global,
            //    _siteId,
            //    _machineId
            //    )));

            var voterIds = VoterList.VoterList.Select(v => v.Data.VoterID).ToList(); 
            GlobalReferences.StatusBar.TextCenter = await Task.Run(() => BatchPrintingMethods.PrintBatchDetails(
                AppSettings.Global,
                voterIds,
                true
                ));
            //await Task.Run(() => BatchPrintingMethods.PreviewBatchDetails(
            //    AppSettings.Global,
            //    voterIds
            //    ));

            // Check if voter list is not empty
            if (voterIds != null && voterIds.Count() > 0) 
            {
                // Enable the ballot and lable printing buttons
                CanPrintBallots = true;
                CanPrintLabels = true;

                CanCancel = false;
                RaisePropertyChanged("CanCancel");
            }
            
        }
        #endregion

        #region InitializeBatch
        // Bound command for printing the full batch report
        private RelayCommand _initializeBatchCommand;
        public ICommand InitializeBatchCommand
        {
            get
            {
                if (_initializeBatchCommand == null)
                {
                    _initializeBatchCommand = new RelayCommand(param => this.InitializeBatchClick(), param => this.CanInitialize);
                }
                return _initializeBatchCommand;
            }
        }

        // Enable or Disable report printing
        private bool _canInitialize;
        public bool CanInitialize
        {
            get { return _canInitialize; }
            internal set
            {
                _canInitialize = value;
                RaisePropertyChanged("CanInitialize");
            }
        }

        // Lock the voter record for this batch
        // Generate IMBs for each voter in the list
        private async void InitializeBatchClick()
        {
            if (VoterList.VoterList != null && VoterList.VoterList.Count() > 0)
            {
                // Ask "ARE YOU SURE"
                YesNoDialog aysMessage = new YesNoDialog("AreYouSure", "Are you sure you want to proceed with processing this batch?");
                if (aysMessage.ShowDialog() == false)
                {
                    return;
                }

                // Check if list is valid
                if (await ValidateBatchList()) 
                {
                    // Update voters poll id, machine number and user id (in case of system failure, these records can be retrieved in the normal batch screen)
                    //VoterList.VoterList.UpdateLocation();

                    // Get IMB Parameters
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
                    if (VoterList.VoterList.GenerateIntelligentBarcodes(imbParameters) == true)
                    {
                        if (_currentBatch == null)
                        {
                            // Create new batch 
                            var batch = _batchFactory.Create((int)_siteId, AppSettings.System.MachineID);
                            if (batch != null) _currentBatch = batch.BatchId;
                        }

                        //VoterList.VoterList.UpdateForBatch(AppSettings.System.MachineID);
                        VoterList.VoterList.UpdateForBatch(_currentBatch);

                        CanPrintReport = true;
                        RaisePropertyChanged("CanPrintReport");

                        CanCancel = false;
                        RaisePropertyChanged("CanCancel");

                        CanInitialize = false;
                        RaisePropertyChanged("CanInitialize");

                        if(CheckLockedStatus() == true)
                        {
                            AlertDialog newMessage = new AlertDialog("A PREVIOUS BATCH WAS INTERUPTED! \r\nMAKE SURE TO REPRINT ALL LABELS.");
                            newMessage.ShowDialog();
                        }
                    }
                }
                else
                {
                    // Display already processed message
                    AlertDialog newMessage = new AlertDialog("ONE OR MORE VOTERS IN THIS BATCH CANNOT BE PROCESSED.\r\nTHE BATCH WILL NOW BE REFRESHED.");
                    newMessage.ShowDialog();

                    // Reload page
                    if (_siteId == 0)
                    {
                        // Reload SERVIS batch
                        NavigationMenuMethods.BatchPrinting(_siteId);
                    }
                    else
                    {
                        // Reload normal batch
                        NavigationMenuMethods.BatchPrinting();
                    }
                }
            }
            else
            {
                // Display no voters message
                AlertDialog newMessage = new AlertDialog("THERE ARE NO VOTERS TO BE PROCESSED"); 
                newMessage.ShowDialog();
            }
        }

        private async Task<bool> ValidateBatchList()
        {
            bool result = false;

            var stringIdList = VoterList.VoterList.Select(v => v.Data.VoterID).ToList();
            var intIdList = stringIdList.Select(int.Parse).ToList();

            // Load current state of queued records
            //var votersToValidate = await Task.Run(()=> VoterMethods.Voters.BatchCompare(
            //    intIdList,
            //    (int)_siteId,
            //    AppSettings.System.MachineID,
            //    5,
            //    AppSettings.System.MachineID)
            //    .Take(_pageSize)
            //    );

            var votersToValidate = await Task.Run(() => VoterMethods.Voters.BatchCompare(
                intIdList,
                _currentBatch,
                (int)_siteId,
                5)
                .Take(_pageSize)
                );

            // Get a list of voter ids
            var voterIdList = votersToValidate.Select(v => v.Data.VoterID).ToList();

            // Check for empty list
            if (VoterList.VoterList != null && votersToValidate != null)
            {
                // Check counts
                if(VoterList.VoterList.Count() == votersToValidate.Count())
                {
                    result = true;
                }
                else
                {
                    result = false;
                    return result;
                }

                // Compare records                
                {
                    // Pull list of valid voters from the curren list
                    var validVoters = VoterList.VoterList.Where(v => voterIdList.Contains(v.Data.VoterID)).ToList();

                    // Compare current list to valid voters
                    if(VoterList.VoterList != null && validVoters != null && VoterList.VoterList.Count() == validVoters.Count())
                    {
                        result = true;
                        return result;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        private bool CheckLockedStatus()
        {
            bool result = false;

            if(VoterList.VoterList != null && VoterList.VoterList.Count() > 0)
            {
                foreach(var voterItem in VoterList.VoterList)
                {
                    if(voterItem.Data.UserId != null && voterItem.Data.UserId > 0)
                    {
                        return true;
                    }
                }
            }

            return result;
        }
        #endregion

        #region BatchLabels
        // Bound command for priting the address labels
        private RelayCommand _printBatchLabelsCommand;
        public ICommand PrintBatchLabelsCommand
        {
            get
            {
                if (_printBatchLabelsCommand == null)
                {
                    _printBatchLabelsCommand = new RelayCommand(param => this.PrintBatchLabelsClick(), param => this.CanPrintLabels);
                }
                return _printBatchLabelsCommand;
            }
        }

        // Enable or Disable label printing
        private bool _canPrintLabels;
        public bool CanPrintLabels
        {
            get { return _canPrintLabels; }
            internal set
            {
                _canPrintLabels = value;
                RaisePropertyChanged("CanPrintLabels");
            }
        }

        // Choose which printing method to use
        private void PrintBatchLabelsClick()
        {
            if (AppSettings.Absentee.BatchPrintingMode == "ALL")
            {
                PrintAllLabels();
            }
            else
            {
                PrintLabels();
            }
        }

        // Print labels from a single ballot style
        private async void PrintLabels()
        {
            if (SelectedBallotStyleItem != null)
            {
                OkCancelDialog newMessage = new OkCancelDialog("Print Labels", "YOU ARE ABOUT TO PRINT " + SelectedBallotCount.ToString() + " LABEL(S)\r\nFOR BALLOT STYLE " + SelectedBallotStyleItem.BallotStyleName + "\r\nMAKE SURE YOUR LABELS ARE IN THE REPORT TRAY\r\nAND THE PRINTER IS READY");
                if (newMessage.ShowDialog() == true)
                {
                    //StatusBar.ApplicationStatus("Printing Batched Mailing Labels"); 
                    GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => BatchPrintingMethods.PrintBatchLabelsWithIMB(
                        AppSettings.Global,
                        SelectedBallotStyleItem.BallotStyleId,
                        (int)_siteId,
                        _machineId,
                        _pageSize,
                        true
                        )));
                }
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("NO BALLOT STYLE IS SELECTED");
                newMessage.ShowDialog();
            }
        }

        // Print labels from all queued ballot styles
        private async void PrintAllLabels()
        {
            // Check if list is empty
            if (VoterList != null && VoterList.VoterList != null)
            {
                int labelSheets = (int)Math.Ceiling((double)TotalBallotCount / 6);

                // Display the ARE YOU SURE message
                OkCancelDialog newMessage = new OkCancelDialog("Print Labels", "YOU ARE ABOUT TO PRINT " + labelSheets.ToString() + " SHEET(S) OF LABELS\r\nMAKE SURE YOUR LABELS ARE IN THE REPORT TRAY\r\nAND THE PRINTER IS READY");
                if (newMessage.ShowDialog() == true)
                {
                    // Get ids from voter list
                    List<string> styleIDlist = VoterList.VoterList.Select(v => v.Data.VoterID).ToList();

                    // Print the labels and display any error messages
                    GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => BatchPrintingMethods.PrintBatchLabelsWithIMB(
                        AppSettings.Global,
                        styleIDlist,
                        _pageSize,
                        true
                        )));
                }
            }
        }
        #endregion

        #region BatchBallots
        // Bound command for printing ballots
        private RelayCommand _printBatchBallotsCommand;
        public ICommand PrintBatchBallotsCommand
        {
            get
            {
                if (_printBatchBallotsCommand == null)
                {
                    _printBatchBallotsCommand = new RelayCommand(param => this.PrintBatchBallotsClick(), param => this.CanPrintBallots);
                }
                return _printBatchBallotsCommand;
            }
        }

        // Enable or Disable ballot printing
        private bool _canPrintBallots;
        public bool CanPrintBallots
        {
            get { return _canPrintBallots; }
            internal set
            {
                _canPrintBallots = value;
                RaisePropertyChanged("CanPrintBallots");
            }
        }

        // Choose which printing method to use
        private void PrintBatchBallotsClick()
        {
            if (AppSettings.Absentee.BatchPrintingMode == "ALL")
            {
                PrintAllBallots();
            }
            else
            {
                PrintBallots();
            }
        }

        // Print all ballots from a single ballot style
        private async void PrintBallots()
        {
            if (VoterList != null && VoterList.VoterList != null)
            {
                // Display ARE YOU SURE message
                OkCancelDialog newMessage = new OkCancelDialog("Print Ballots", "YOU ARE ABOUT TO PRINT " + SelectedBallotCount.ToString() + " BALLOT(S)\r\nFOR BALLOT STYLE " + SelectedBallotStyleItem.BallotStyleName + "\r\nMAKE SURE YOUR BALLOT STOCK IS IN THE BALLOT TRAY\r\nAND THE PRINTER IS READY");
                if (newMessage.ShowDialog() == true)
                {
                    // Get selected ballot style
                    if(SelectedBallotStyleItem != null)
                    {
                        int totalBallots = VoterList.VoterList.Where(v => v.Data.BallotStyleID == SelectedBallotStyleItem.BallotStyleId).Count();
                        GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => BallotPrinting.PrintBallotBatch(SelectedBallotStyleItem.BallotStyleFileName, totalBallots)));

                        // Unlock the finish ballot button
                        CanFinishBatch = true;
                    }
                }
            }
        }

        // Print ballots from all queued ballot styles
        private async void PrintAllBallots()
        {
            if (VoterList != null && VoterList.VoterList != null)
            {
                // Display ARE YOU SURE message
                OkCancelDialog newMessage = new OkCancelDialog("Print Ballots", "YOU ARE ABOUT TO PRINT " + TotalBallotCount.ToString() + " BALLOT(S)\r\nMAKE SURE YOUR BALLOT STOCK IS IN THE BALLOT TRAY\r\nAND THE PRINTER IS READY");
                if (newMessage.ShowDialog() == true)
                {
                    // Loop through each ballot style in voter list
                    //foreach (var style in BallotStyles)
                    //{
                    //    int totalBallots = VoterList.VoterList.Where(v => v.BallotStyle == style).Count();
                    //    GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => BallotPrinting.PrintBallotBatch(style.BallotStyleFileName, totalBallots)));
                    //}

                    // Loop through voters in list
                    foreach(var voter in VoterList.VoterList)
                    {
                        GlobalReferences.StatusBar.TextCenter = (await Task.Run(() => BallotPrinting.ReprintBallot(voter.Data)));
                    }

                    // Unlock the finish ballot button
                    CanFinishBatch = true;
                }
            }
        }
        #endregion

        #region Finish
        // Bound command for finishing the current batch
        private RelayCommand _finishBatchCommand;
        public ICommand FinishBatchCommand
        {
            get
            {
                if (_finishBatchCommand == null)
                {
                    _finishBatchCommand = new RelayCommand(param => this.FinishBatchClick(), param => this.CanFinishBatch);
                }
                return _finishBatchCommand;
            }
        }

        // Enable or Disable Finish Batch button
        private bool _canFinishBatch;
        public bool CanFinishBatch
        {
            get { return _canFinishBatch; }
            internal set
            {
                _canFinishBatch = value;
                RaisePropertyChanged("CanFinishBatch");
            }
        }

        private void FinishBatchClick()
        {
            if (AppSettings.Absentee.BatchPrintingMode == "ALL")
            {
                FinishAllBatch();
            }
            else
            {
                FinishSingleBatch();
            }

            // Lock the current batch
            if (_currentBatch != null)
            {
                _batchFactory.CloseBatch(_currentBatch.Value);
            }
        }

        private async void FinishSingleBatch()
        {
            CanFinishBatch = false;

            string style = SelectedBallotStyleItem.BallotStyleName;

            OkCancelDialog newMessage = new OkCancelDialog("Clear Batch", "MAKE SURE YOU HAVE ALL LABELS, BALLOTS AND THE REPORT\r\nBEFORE CLEARING THE BATCH FOR " + style.ToString());
            if (newMessage.ShowDialog() == true)
            {
                bool result = false;
                if (_voters != null)
                {
                    result = await Task.Run(() =>
                    {
                        try
                        {
                            // Get voters from ballot style
                            var voters = _voters.Where(v => v.Data.BallotStyleID == SelectedBallotStyleItem.BallotStyleId).ToList();
                            voters.IssueBallots();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    });
                }

                if (result == false)
                {
                    GlobalReferences.StatusBar.TextCenter = ("Error Writting to Voter Record");
                }
                else
                {
                    // RELOAD THE PAGE

                    // Clear the voter list
                    _voters = null;
                    RaisePropertyChanged("VoterList");

                    // Reload the list control
                    //SearchVoters();

                    // Update counts
                    RaisePropertyChanged("BallotStyleCount");
                    RaisePropertyChanged("TotalBallotCount");
                    RaisePropertyChanged("TotalQueued");

                    // Update ballot style list                    
                    BallotStyleList = null;
                    RaisePropertyChanged("BallotStyleList");

                    // Update Selected ballot style
                    SelectedBallotStyleItem = BallotStyleList.FirstOrDefault();
                    RaisePropertyChanged("SelectedBallotStyleItem");

                    // Disable other buttons 
                    CanPrintReport = false;
                    RaisePropertyChanged("CanPrintReport");

                    CanPrintLabels = false;
                    RaisePropertyChanged("CanPrintLabels");

                    // Enable the cancel button
                    CanCancel = true;
                    RaisePropertyChanged("CanCancel");
                }
            }
            else
            {
                CanFinishBatch = true;
            }
        }

        private async void FinishAllBatch() 
        {
            CanFinishBatch = false;

            OkCancelDialog newMessage = new OkCancelDialog("Clear Batch", "MAKE SURE YOU HAVE ALL LABELS, BALLOTS AND THE REPORT\r\nBEFORE CLEARING THE BATCH(ES) ");
            if (newMessage.ShowDialog() == true)
            {
                bool result = false;
                if (VoterList.VoterList != null)
                {
                    result = await Task.Run(() =>
                    {
                        try
                        {
                            VoterList.VoterList.IssueBallots();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    });
                }

                if (result == false)
                {
                    GlobalReferences.StatusBar.TextCenter = ("Error Writting to Voter Record");
                }
                else
                {
                    // RELOAD THE PAGE

                    // Clear the voter list
                    _voters = null;
                    RaisePropertyChanged("VoterList");

                    // Reload the list control
                    //SearchVoters();

                    // Update counts
                    RaisePropertyChanged("BallotStyleCount");
                    RaisePropertyChanged("TotalBallotCount");
                    RaisePropertyChanged("TotalQueued");

                    // Disable the ballot button
                    CanPrintBallots = false;
                    RaisePropertyChanged("CanPrintBallots");

                    // Disable other buttons 
                    CanPrintReport = false;
                    RaisePropertyChanged("CanPrintReport");

                    CanPrintLabels = false;
                    RaisePropertyChanged("CanPrintLabels");

                    // Enable the cancel button
                    CanCancel = true;
                    RaisePropertyChanged("CanCancel");
                }
            }
            else
            {
                CanFinishBatch = true;
            }
        }
        #endregion

        #region Cancel
        // Bound command to navigate back to the absentee home screen
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

        // Navigate back to the absentee home screen
        private void CancelClick()
        {
            // Save batch state
            AppSettings.Absentee.BatchReportPrinted = false;
            //AppSettings.SaveAbsentee();

            // Show menu button
            //MainMenuMethods.ShowHamburger();
            GlobalReferences.Header.HamburgerMenuVisibility = true;

            // Open Menu 
            //MainMenuMethods.OpenMainMenu();
            GlobalReferences.MenuSlider.CollapseMode = MenuCollapseMode.Full;
            GlobalReferences.MenuSlider.Open();

            // Return to Absentee Menu page
            NavigationMenuMethods.AbsenteeHome();
        }
        #endregion
    }
}
