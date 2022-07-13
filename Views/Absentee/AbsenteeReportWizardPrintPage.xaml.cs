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
using System.Collections.ObjectModel;
//using VoterX.Core.Models.ViewModels;
//using VoterX.Core.Models.Database;
using VoterX.Models;
using VoterX.Extensions;
using System.Data;
using VoterX.Dialogs;
using System.Diagnostics;
using SHDocVw;
using System.Threading;
using System.IO;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeReportWizardPrintPage.xaml
    /// </summary>
    public partial class AbsenteeReportWizardPrintPage : Page
    {
        private ReportWizardQueryModel _wizardSearch;

        private bool _reportProcessing = false;

        public AbsenteeReportWizardPrintPage()
        {
            InitializeComponent();

            _wizardSearch = new ReportWizardQueryModel();

            GlobalReferences.Header.PageHeader = ("Report Wizard");

            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            //CheckServer();
        }

        public AbsenteeReportWizardPrintPage(ReportWizardQueryModel searchParameters)
        {
            InitializeComponent();

            _wizardSearch = searchParameters;

            GlobalReferences.Header.PageHeader = ("Report Wizard");

            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            CheckServer();

            ReportName.Text = searchParameters.ReportName;
            //if(searchParameters.ReportName != "" && searchParameters.ReportName != null)
            //{
            //    ReportName.Text = searchParameters.ReportName;
            //}
        }

        private async void CheckServer()
        {
            await GlobalReferences.StatusBar.CheckDatabaseStatusAsync(100);
        }

        private void ReportName_KeyDown(object sender, KeyEventArgs e)
        {
            //_wizardSearch.ReportName = ReportName.Text;
        }

        private void ReportName_KeyUp(object sender, KeyEventArgs e)
        {
            _wizardSearch.ReportName = ReportName.Text;

            GlobalReferences.StatusBar.TextLeft = (_wizardSearch.ReportName);
        }

        private void PreviousWizardButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrameMethods.NavigateToPage(new AbsenteeReportWizardPage(_wizardSearch));
        }

        private async void ExcelExportButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if another report is being processed
            if (_reportProcessing == false)
            {
                ExcelExportButton.IsEnabled = false;
                ExcelExportBrowseButton.IsEnabled = false;
                _reportProcessing = true;
                this.Cursor = Cursors.Wait;
                Mouse.OverrideCursor = Cursors.Wait;
                await PutTaskDelay(1000);
                //try
                //{
                //    //var path = AppSettings.ReportConfigs.ExcelExportFolder;
                //    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //    //if (path == null || path == "")
                //    //{
                //    //    path = @"C:\VoterX\Export\";
                //    //    System.IO.Directory.CreateDirectory(path);
                //    //}
                //    //path = OpenFolderBrowser(path);
                //    string file = "Report Export " + DateTime.Now.ToString();
                //    if(ReportName.Text != "" && ReportName.Text != null)
                //    {
                //        file = ReportName.Text;
                //    }
                //    else
                //    {
                //        AlertDialog messageBox = new AlertDialog("PLEASE ENTER A VALID REPORT NAME");
                //        messageBox.ShowDialog();
                //    }

                //    if (_wizardSearch.IncludeInactive == true)
                //    {
                //        VoterMethods.VoterReport.ReportListRegistered(
                //            _wizardSearch.LogCodes,
                //            _wizardSearch.BallotStyles,
                //            _wizardSearch.Parties,
                //            _wizardSearch.Jurisdictions,
                //            _wizardSearch.PollSites
                //            ).ExportToExcel();
                //    }
                //    else
                //    {
                //        VoterMethods.VoterReport.ReportList(
                //            _wizardSearch.BeginningDate,
                //            _wizardSearch.EndingDate,
                //            _wizardSearch.LogCodes,
                //            _wizardSearch.BallotStyles,
                //            _wizardSearch.Parties,
                //            _wizardSearch.Jurisdictions,
                //            _wizardSearch.PollSites
                //            ).ExportToExcel();
                //    }

                //    AppSettings.ReportConfigs.ExcelExportFolder = path;
                //    AppSettings.ReportConfigs.ExcelExportFile = file;

                //    AppSettings.SaveChanges();

                // Check if excel is installed
                //Type officeType = Type.GetTypeFromProgID("Excel.Application");
                //if (officeType == null)
                //{
                //    //no Excel installed
                //    AlertDialog newMessage = new AlertDialog("NO INSTANCE OF EXCEL WAS FOUND");
                //    newMessage.ShowDialog();
                //}
                //else
                //{
                //Excel installed
                //VoterMethods.VoterReport.ReportList(
                //_wizardSearch.BeginningDate,
                //_wizardSearch.EndingDate,
                //_wizardSearch.LogCodes,
                //_wizardSearch.BallotStyles,
                //_wizardSearch.Parties,
                //_wizardSearch.Jurisdictions,
                //_wizardSearch.PollSites
                //).ExportToExcel();
                //}

                //    ////ExportToExcel(string.Format("{0}Export {1}.XML", "C:\\VoterX\\", DateTime.Now.ToString("yyyyMMddHHmmss")));
                //}
                //catch (Exception error)
                //{
                //    StatusBar.ApplicationStatusCenter(error.Message);

                //    AlertDialog messageBox = new AlertDialog("THE REPORT COULD NOT BE EXPORTED");
                //    messageBox.ShowDialog();
                //}
                //finally
                //{
                //    this.Cursor = Cursors.Arrow;
                //    Mouse.OverrideCursor = null;
                //    ExcelExportButton.IsEnabled = true;
                //    ExcelExportBrowseButton.IsEnabled = true;
                //    _reportProcessing = false;
                //}
            }
        }

        private async void CSVExportButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if another report is being processed
            if (_reportProcessing == false)
            {
                ExcelExportButton.IsEnabled = false;
                ExcelExportBrowseButton.IsEnabled = false;
                _reportProcessing = true;
                this.Cursor = Cursors.Wait;
                Mouse.OverrideCursor = Cursors.Wait;
                await PutTaskDelay(1000);
                try
                {
                    //var path = AppSettings.ReportConfigs.ExcelExportFolder;
                    //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    //if (path == null || path == "")
                    //{
                    //    path = @"C:\VoterX\Export\";
                    //    System.IO.Directory.CreateDirectory(path);
                    //}
                    //path = OpenFolderBrowser(path);
                    string path = @"C:\VoterX\Export\";
                    System.IO.Directory.CreateDirectory(path);
                    string file = "Report Export " + DateTime.Now.ToString();
                    if (ReportName.Text != "" && ReportName.Text != null)
                    {
                        file = ReportName.Text;
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
                        if(overwriteMessage.ShowDialog() == false)
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
                        //VoterMethods.VoterReport.ReportListRegistered(
                        //    _wizardSearch.LogCodes,
                        //    _wizardSearch.BallotStyles,
                        //    _wizardSearch.Parties,
                        //    _wizardSearch.Jurisdictions,
                        //    _wizardSearch.PollSites
                        //    ).ExportToCSV(path + "\\" + file + ".csv");

                        try
                        {
                            VoterMethods.ReportListRegistered(_wizardSearch)
                                .ExportToCSV(path + "\\" + file + ".csv");
                            fileIoMessage.ShowDialog();
                        }
                        catch { }
                    }
                    else
                    {
                        //VoterMethods.VoterReport.ReportList(
                        //    _wizardSearch.BeginningDate,
                        //    _wizardSearch.EndingDate,
                        //    _wizardSearch.LogCodes,
                        //    _wizardSearch.BallotStyles,
                        //    _wizardSearch.Parties,
                        //    _wizardSearch.Jurisdictions,
                        //    _wizardSearch.PollSites
                        //    ).ExportToCSV(path + "\\" + file + ".csv");

                        try
                        {
                            VoterMethods.ReportList(_wizardSearch)
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
                    this.Cursor = Cursors.Arrow;
                    Mouse.OverrideCursor = null;
                    ExcelExportButton.IsEnabled = true;
                    ExcelExportBrowseButton.IsEnabled = true;
                    _reportProcessing = false;
                }
            }
        }

        private void ExcelExportBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            //ExcelExportBrowseButton.IsEnabled = false;
            //var path = AppSettings.ReportConfigs.ExcelExportFolder;
            //if (path == null || path == "")
            //{
            //    path = @"C:\VoterX\Export\";
            //    System.IO.Directory.CreateDirectory(path);
            //}
            //string file = AppSettings.ReportConfigs.ExcelExportFile + ".csv";
            ////OpenFileBrowser(path, file);
            ////var result = Process.Start(path);
            //if (_reportProcessing == false)
            //{
            //    _reportProcessing = true;
            //    Process.Start("explorer.exe", path).WaitForExit();
            //}
            ////int result = await Task.Run(() => {
            ////    Process P = Process.Start("explorer.exe",path);
            ////    P.WaitForExit();
            ////    return P.ExitCode;
            ////});
            ////if (result >= 0 )
            ////{
            //ExcelExportBrowseButton.IsEnabled = true;
            //_reportProcessing = false;
            ////}

            var path = AppSettings.ReportConfigs.ExcelExportFolder;
            if (path == null || path == "")
            {
                path = @"C:\VoterX\Export\";
                System.IO.Directory.CreateDirectory(path);
            }
            string file = AppSettings.ReportConfigs.ExcelExportFile + ".csv";

            var currentlyOpenedWindows = GetAllOpenedExplorerWindow();
            if (ActiveOpenedWindowHwnd == 0 || !ExistOpenedWindow())
            {
                Process.Start("explorer.exe", path);
                ShellWindows windows;
                while ((windows = new SHDocVw.ShellWindows()).Count <= currentlyOpenedWindows.Count)
                {
                    Thread.Sleep(50);
                }
                var currentlyOpenedWindowsNew = GetAllOpenedExplorerWindow();
                var openedWindow = currentlyOpenedWindowsNew.Except(currentlyOpenedWindows).Single();
                ActiveOpenedWindowHwnd = openedWindow.HWND;
            }
        }

        private async void SummaryReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (_reportProcessing == false)
            {
                SummaryReportButton.IsEnabled = false;
                _reportProcessing = true;
                this.Cursor = Cursors.Wait;
                Mouse.OverrideCursor = Cursors.Wait;
                await PutTaskDelay(1000);
                try
                {
                    GlobalReferences.StatusBar.TextLeft = (ReportPrintingMethods.PrintCustomSummaryReportGeneral(_wizardSearch));
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                    Mouse.OverrideCursor = null;
                    SummaryReportButton.IsEnabled = true;
                    _reportProcessing = false;
                }
            }
        }

        private async void DetailReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (_reportProcessing == false)
            {
                DetailReportButton.IsEnabled = false;
                _reportProcessing = true;
                this.Cursor = Cursors.Wait;
                Mouse.OverrideCursor = Cursors.Wait;
                await PutTaskDelay(1000);
                try
                {
                    GlobalReferences.StatusBar.TextLeft = (ReportPrintingMethods.PrintCustomSpecialReportGeneral(_wizardSearch));
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                    Mouse.OverrideCursor = null;
                    DetailReportButton.IsEnabled = true;
                    _reportProcessing = false;
                }
            }
        }

        private async void LabelReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (_reportProcessing == false)
            {
                LabelReportButton.IsEnabled = false;
                _reportProcessing = true;
                this.Cursor = Cursors.Wait;
                Mouse.OverrideCursor = Cursors.Wait;
                await PutTaskDelay(1000);
                try
                {
                    GlobalReferences.StatusBar.TextLeft = (ReportPrintingMethods.PrintCustomLabelReportGeneral(_wizardSearch));
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                    Mouse.OverrideCursor = null;
                    LabelReportButton.IsEnabled = true;
                    _reportProcessing = false;
                }
            }
        }

        private async void SummaryPreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (_reportProcessing == false)
            {
                SummaryPreviewButton.IsEnabled = false;
                _reportProcessing = true;
                this.Cursor = Cursors.Wait;
                Mouse.OverrideCursor = Cursors.Wait;
                await PutTaskDelay(1000);
                try
                {
                    ReportPrintingMethods.PreviewCustomSummaryReportGeneral(_wizardSearch);
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                    Mouse.OverrideCursor = null;
                    SummaryPreviewButton.IsEnabled = true;
                    _reportProcessing = false;
                }
            }
        }

        private async void DetailPreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (_reportProcessing == false)
            {
                DetailPreviewButton.IsEnabled = false;
                _reportProcessing = true;
                this.Cursor = Cursors.Wait;
                Mouse.OverrideCursor = Cursors.Wait;
                await PutTaskDelay(1000);
                try
                {
                    ReportPrintingMethods.PreviewCustomSpecialReportGeneral(_wizardSearch);
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                    Mouse.OverrideCursor = null;
                    DetailPreviewButton.IsEnabled = true;
                    _reportProcessing = false;
                }
            }
        }

        private void SortTypeFirst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).IsLoaded)
            {
                //StatusBar.ApplicationStatusLeft(ComboBoxMethods.GetSelectedItemNumber(SortTypeFirst).ToString());
                _wizardSearch.SortType1 = ComboBoxMethods.GetSelectedItemNumber(SortTypeFirst);
            }
        }

        private void SortTypeSecond_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).IsLoaded)
            {
                //StatusBar.ApplicationStatusLeft(SortTypeSecond.SelectedIndex.ToString());
                _wizardSearch.SortType2 = ComboBoxMethods.GetSelectedItemNumber(SortTypeSecond);
            }
        }

        private void GroupType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).IsLoaded)
            {
                //StatusBar.ApplicationStatusLeft(GroupType.SelectedIndex.ToString());
                _wizardSearch.GroupType = ComboBoxMethods.GetSelectedItemNumber(GroupType);
            }
        }

        // https://stackoverflow.com/questions/22158278/wait-some-seconds-without-blocking-ui-execution
        private async Task PutTaskDelay(int delay)
        {
            await Task.Delay(delay);
        }

        // Folder Browser Sample
        // https://stackoverflow.com/questions/1922204/open-directory-dialog
        private string OpenFolderBrowser(string currentPath)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = currentPath;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                return dialog.SelectedPath;
            }
        }

        private string OpenFileBrowser(string currentPath, string currentFile)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.InitialDirectory = currentPath;
                dialog.FileName = currentFile;                
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                return dialog.FileName;
            }
        }

        // https://stackoverflow.com/questions/45442681/condition-process-start-to-the-number-of-instances-of-a-given-process
        public bool ExistOpenedWindow()
        {
            var currentlyOpenedWindows = GetAllOpenedExplorerWindow();
            return currentlyOpenedWindows.Any(t => t.HWND == ActiveOpenedWindowHwnd);
        }

        // https://stackoverflow.com/questions/45442681/condition-process-start-to-the-number-of-instances-of-a-given-process
        public List<InternetExplorer> GetAllOpenedExplorerWindow()
        {
            List<InternetExplorer> windows = new List<InternetExplorer>();

            ShellWindows _shellWindows = new SHDocVw.ShellWindows();
            string processType;

            foreach (InternetExplorer ie in _shellWindows)
            {
                //this parses the name of the process
                processType = System.IO.Path.GetFileNameWithoutExtension(ie.FullName).ToLower();

                //this could also be used for IE windows with processType of "iexplore"
                if (processType.Equals("explorer"))
                {
                    windows.Add(ie);
                }
            }
            return windows;
        }

        public static int ActiveOpenedWindowHwnd;

        // https://stackoverflow.com/questions/45442681/condition-process-start-to-the-number-of-instances-of-a-given-process
        private void button1_Click(object sender, EventArgs e)
        {
            var currentlyOpenedWindows = GetAllOpenedExplorerWindow();
            if (ActiveOpenedWindowHwnd == 0 || !ExistOpenedWindow())
            {
                Process.Start("explorer.exe");
                ShellWindows windows;
                while ((windows = new SHDocVw.ShellWindows()).Count <= currentlyOpenedWindows.Count)
                {
                    Thread.Sleep(50);
                }
                var currentlyOpenedWindowsNew = GetAllOpenedExplorerWindow();
                var openedWindow = currentlyOpenedWindowsNew.Except(currentlyOpenedWindows).Single();
                ActiveOpenedWindowHwnd = openedWindow.HWND;
            }
        }

        
    }
}
