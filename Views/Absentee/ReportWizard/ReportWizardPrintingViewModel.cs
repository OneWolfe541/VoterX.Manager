using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoterX.Dialogs;
using VoterX.Extensions;
using VoterX.Methods;
using VoterX.Models;
using VoterX.SystemSettings.Models;
using VoterX.Utilities.Commands;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Views.Absentee
{
    public class ReportWizardPrintingViewModel : NotifyPropertyChanged
    {
        public ReportWizardPrintingViewModel() : this(null) { }
        public ReportWizardPrintingViewModel(ReportWizardQueryModel wizardSearch)
        {
            // Initialize search parameters
            if (wizardSearch == null)
            {
                wizardSearch = new ReportWizardQueryModel();
            }
            WizardSearch = wizardSearch;

            // Initialize print buttons
            CanPrintSummaryReport = true;
            CanPrintDetailReport = true;
            CanPrintLabelReport = true;
            CanPreviewSummaryReport = true;
            CanPreviewDetailReport = true;
            CanExportCSV = true;
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

            }
        }

        public string ReportName
        {
            get
            {
                return WizardSearch.ReportName;
            }

            set
            {
                WizardSearch.ReportName = value;
                RaisePropertyChanged("WizardSearch");
            }
        }
        #endregion

        #region PrintCommands
        private RelayCommand _printSummaryReportCommand;
        public ICommand PrintSummaryReportCommand
        {
            get
            {
                if (_printSummaryReportCommand == null)
                {
                    _printSummaryReportCommand = new RelayCommand(param => this.PrintSummaryReportClick(), param => CanPrintSummaryReport);
                }
                return _printSummaryReportCommand;
            }
        }

        private bool _canPrintSummaryReport;
        public bool CanPrintSummaryReport
        {
            get { return _canPrintSummaryReport; }
            internal set
            {
                _canPrintSummaryReport = value;
                RaisePropertyChanged("CanPrintSummaryReport");
            }
        }

        private void PrintSummaryReportClick()
        {
            CanPrintSummaryReport = false;

            try
            {
                GlobalReferences.StatusBar.TextLeft = (ReportPrintingMethods.PrintCustomSummaryReportGeneral(WizardSearch));
            }
            finally
            {
                CanPrintSummaryReport = true;
            }
        }

        private RelayCommand _printDetailReportCommand;
        public ICommand PrintDetailReportCommand
        {
            get
            {
                if (_printDetailReportCommand == null)
                {
                    _printDetailReportCommand = new RelayCommand(param => this.PrintDetailReportClick(), param => CanPrintDetailReport);
                }
                return _printDetailReportCommand;
            }
        }

        private bool _canPrintDetailReport;
        public bool CanPrintDetailReport
        {
            get { return _canPrintDetailReport; }
            internal set
            {
                _canPrintDetailReport = value;
                RaisePropertyChanged("CanPrintDetailReport");
            }
        }

        private void PrintDetailReportClick()
        {
            CanPrintDetailReport = false;

            try
            {
                GlobalReferences.StatusBar.TextLeft = (ReportPrintingMethods.PrintCustomSpecialReportGeneral(WizardSearch));
            }
            finally
            {
                CanPrintDetailReport = true;
            }
        }

        private RelayCommand _printLabelReportCommand;
        public ICommand PrintLabelReportCommand
        {
            get
            {
                if (_printLabelReportCommand == null)
                {
                    _printLabelReportCommand = new RelayCommand(param => this.PrintLabelReportClick(), param => CanPrintLabelReport);
                }
                return _printLabelReportCommand;
            }
        }

        private bool _canPrintLabelReport;
        public bool CanPrintLabelReport
        {
            get { return _canPrintLabelReport; }
            internal set
            {
                _canPrintLabelReport = value;
                RaisePropertyChanged("CanPrintSummaryReport");
            }
        }

        private void PrintLabelReportClick()
        {
            CanPrintLabelReport = false;

            try
            {
                GlobalReferences.StatusBar.TextLeft = (ReportPrintingMethods.PrintCustomLabelReportGeneral(WizardSearch));
            }
            finally
            {
                CanPrintLabelReport = true;
            }
        }
        #endregion

        #region PreviewCommands
        private RelayCommand _previewSummaryReportCommand;
        public ICommand PreviewSummaryReportCommand
        {
            get
            {
                if (_previewSummaryReportCommand == null)
                {
                    _previewSummaryReportCommand = new RelayCommand(param => this.PreviewSummaryReportClick(), param => CanPreviewSummaryReport);
                }
                return _previewSummaryReportCommand;
            }
        }

        private bool _canPreviewSummaryReport;
        public bool CanPreviewSummaryReport
        {
            get { return _canPreviewSummaryReport; }
            internal set
            {
                _canPreviewSummaryReport = value;
                RaisePropertyChanged("CanPreviewSummaryReport");
            }
        }

        private void PreviewSummaryReportClick()
        {
            CanPreviewSummaryReport = false;

            try
            {
                ReportPrintingMethods.PreviewCustomSummaryReportGeneral(WizardSearch);
            }
            finally
            {
                CanPreviewSummaryReport = true;
            }
        }

        private RelayCommand _previewDetailReportCommand;
        public ICommand PreviewDetailReportCommand
        {
            get
            {
                if (_previewDetailReportCommand == null)
                {
                    _previewDetailReportCommand = new RelayCommand(param => this.PreviewDetailReportClick(), param => CanPreviewDetailReport);
                }
                return _previewDetailReportCommand;
            }
        }

        private bool _canPreviewDetailReport;
        public bool CanPreviewDetailReport
        {
            get { return _canPreviewDetailReport; }
            internal set
            {
                _canPreviewDetailReport = value;
                RaisePropertyChanged("CanPreviewDetailReport");
            }
        }

        private void PreviewDetailReportClick()
        {
            CanPreviewDetailReport = false;

            try
            {
                ReportPrintingMethods.PreviewCustomSpecialReportGeneral(WizardSearch);
            }
            finally
            {
                CanPreviewDetailReport = true;
            }
        }
        #endregion

        #region ExportCommands
        private RelayCommand _exportCSVCommand;
        public ICommand ExportCSVCommand
        {
            get
            {
                if (_exportCSVCommand == null)
                {
                    _exportCSVCommand = new RelayCommand(param => this.ExportCSVClick(), param => CanExportCSV);
                }
                return _exportCSVCommand;
            }
        }

        private bool _canExportCSV;
        public bool CanExportCSV
        {
            get { return _canExportCSV; }
            internal set
            {
                _canExportCSV = value;
                RaisePropertyChanged("CanExportCSV");
            }
        }

        private void ExportCSVClick()
        {
            CanExportCSV = false;

            try
            {
                string path = @"C:\VoterX\Export\";
                System.IO.Directory.CreateDirectory(path);
                string file = "Report Export " + DateTime.Now.ToString();
                if (ReportName != "" && ReportName != null)
                {
                    file = ReportName;
                }
                else
                {
                    AlertDialog messageBox = new AlertDialog("PLEASE ENTER A VALID REPORT NAME");
                    messageBox.ShowDialog();
                }

                AlertDialog fileIoMessage = null;
                if (File.Exists(path + "\\" + file + ".csv"))
                {
                    YesNoDialog overwriteMessage = new YesNoDialog("Overwrite Message", "Do you want to overwrite the existing file?\r\n" + path + file + ".csv");
                    if (overwriteMessage.ShowDialog() == false)
                    {
                        return;
                    }
                    fileIoMessage = new AlertDialog("FILE CREATED!\r\n" + path + file + ".csv");
                }
                else
                {
                    fileIoMessage = new AlertDialog("FILE CREATED!\r\n" + path + file + ".csv");
                }

                if (_wizardSearch.IncludeInactive == true)
                {
                    try
                    {
                        VoterMethods.ReportListRegistered(WizardSearch)
                            .ExportToCSV(path + "\\" + file + ".csv");
                        fileIoMessage.ShowDialog();
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        VoterMethods.ReportList(WizardSearch)
                            .ExportToCSV(path + "\\" + file + ".csv");
                        fileIoMessage.ShowDialog();
                    }
                    catch { }
                }

                AppSettings.ReportConfigs.ExcelExportFolder = path;
                AppSettings.ReportConfigs.ExcelExportFile = file;

                AppSettings.SaveChanges();
            }
            catch (Exception error)
            {
                GlobalReferences.StatusBar.TextCenter = (error.Message);

                AlertDialog messageBox = new AlertDialog("THE REPORT COULD NOT BE EXPORTED");
                messageBox.ShowDialog();
            }
            finally
            {
                CanExportCSV = true;
            }
        }
        #endregion

        #region SortingGroupingOptions
        private List<NameValuePairModel> _firstSortList;
        public List<NameValuePairModel> FirstSortList
        {
            get
            {
                if(_firstSortList == null)
                {
                    _firstSortList = new List<NameValuePairModel>();
                    _firstSortList.Add(new NameValuePairModel(0, "None"));
                    _firstSortList.Add(new NameValuePairModel(1, "Last then First"));
                    _firstSortList.Add(new NameValuePairModel(2, "Status Code"));
                    _firstSortList.Add(new NameValuePairModel(3, "Party Affiliation"));
                    _firstSortList.Add(new NameValuePairModel(4, "Ballot Style"));
                }
                return _firstSortList;
            }

            private set
            {
                _firstSortList = value;
            }
        }

        private NameValuePairModel _selectedFirstSortItem;
        public NameValuePairModel SelectedFirstSortItem
        {
            get
            {
                if(_selectedFirstSortItem == null && WizardSearch != null && FirstSortList != null)
                {
                    _selectedFirstSortItem = FirstSortList.Where(f => f.Id == WizardSearch.SortType1).FirstOrDefault();
                }
                return _selectedFirstSortItem;
            }

            set
            {
                _selectedFirstSortItem = value;
                WizardSearch.SortType1 = value.Id;
                RaisePropertyChanged("WizardSearch");
            }
        }

        private List<NameValuePairModel> _secondSortList;
        public List<NameValuePairModel> SecondSortList
        {
            get
            {
                if (_secondSortList == null)
                {
                    _secondSortList = new List<NameValuePairModel>();
                    _secondSortList.Add(new NameValuePairModel(0, "None"));
                    _secondSortList.Add(new NameValuePairModel(1, "Last then First"));
                    _secondSortList.Add(new NameValuePairModel(2, "Status Code"));
                    _secondSortList.Add(new NameValuePairModel(3, "Party Affiliation"));
                    _secondSortList.Add(new NameValuePairModel(4, "Ballot Style"));
                }
                return _secondSortList;
            }

            private set
            {
                _secondSortList = value;
            }
        }

        private NameValuePairModel _selectedSecondSortItem;
        public NameValuePairModel SelectedSecondSortItem
        {
            get
            {
                if (_selectedSecondSortItem == null && WizardSearch != null && SecondSortList != null)
                {
                    _selectedSecondSortItem = SecondSortList.Where(f => f.Id == WizardSearch.SortType2).FirstOrDefault();
                }
                return _selectedSecondSortItem;
            }

            set
            {
                _selectedSecondSortItem = value;
                WizardSearch.SortType2 = value.Id;
                RaisePropertyChanged("WizardSearch");
            }
        }

        private List<NameValuePairModel> _groupList;
        public List<NameValuePairModel> GroupList
        {
            get
            {
                if (_groupList == null)
                {
                    _groupList = new List<NameValuePairModel>();
                    _groupList.Add(new NameValuePairModel(0, "None"));
                    _groupList.Add(new NameValuePairModel(1, "Last Initial"));
                    _groupList.Add(new NameValuePairModel(2, "Status Code"));
                    _groupList.Add(new NameValuePairModel(3, "Party Affiliation"));
                    _groupList.Add(new NameValuePairModel(4, "Ballot Style"));
                }
                return _groupList;
            }

            private set
            {
                _groupList = value;
            }
        }

        private NameValuePairModel _selectedGroupItem;
        public NameValuePairModel SelectedGroupItem
        {
            get
            {
                if (_selectedGroupItem == null && WizardSearch != null && GroupList != null)
                {
                    _selectedGroupItem = GroupList.Where(f => f.Id == WizardSearch.GroupType).FirstOrDefault();
                }
                return _selectedGroupItem;
            }

            set
            {
                _selectedGroupItem = value;
                WizardSearch.GroupType = value.Id;
                RaisePropertyChanged("WizardSearch");
            }
        }
        #endregion

        #region NavigationCommands
        // Bound command for returning to the previous screen
        private RelayCommand _previousCommand;
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
            NavigationMenuMethods.ReportWizardSelections(WizardSearch);
        }
        #endregion
    }
}
