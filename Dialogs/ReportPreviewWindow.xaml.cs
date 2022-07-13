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
using System.Windows.Shapes;
using FastReport;
using FastReport.DevComponents.DotNetBar;
using VoterX.Methods;
using VoterX.Logging;
using VoterX.SystemSettings;

// https://www.fast-report.com/en/blog/153/show/
namespace VoterX
{
    /// <summary>
    /// Interaction logic for ReportPreviewWindow.xaml
    /// </summary>
    public partial class ReportPreviewWindow : Window
    {
        FastReport.Preview.PreviewControl prewControl;// = new FastReport.Preview.PreviewControl();
        Report report1 = new Report();

        public ReportPreviewWindow(string reportFileName, string sql, bool useDefaultConnection, string dataSource)
        {
            string reportPath = (AppSettings.ReportConfigs.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Preview File: " + reportPath);
            reportLogger.WriteLog("Report SQL: " + sql);

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                //return "File Not Found: " + reportPath;
                //return;
            }

            RemoveExportItems();

            InitializeComponent();

            prewControl = new FastReport.Preview.PreviewControl();

            if (useDefaultConnection == false)
            {
                try
                {
                    //// Change connection string
                    string OldConnectionString = report1.Dictionary.Connections[0].ConnectionString;
                    string newConnectionString = "data source=AESSQL2;initial catalog=SOSDataExchange;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
                    //newConnectionString = "Data Source=" + AppSettings.Network.SQLServer + ";AttachDbFilename=;Initial Catalog=" + AppSettings.Network.LocalDatabase + ";Integrated Security=False;Persist Security Info=True;User ID=sa;Password=InkImpr35510n5";
                    newConnectionString = "Data Source=" + AppSettings.Network.SQLServer + ";AttachDbFilename=;Initial Catalog=" + AppSettings.Network.LocalDatabase + ";Integrated Security=False;Persist Security Info=True;User ID=sa;Password=@ESf13lddba";
                    report1.Dictionary.Connections[0].ConnectionString = newConnectionString;
                }
                catch (Exception e)
                {
                    e.Message.ToString();
                    reportLogger.WriteLog("Connection Error: " + e.Message.ToString());
                    //return "Connection Error: ";
                }
            }            

            //try
            //{
            //    report1.PrintSettings.ShowDialog = false;
            //    report1.PrintPrepared();
            //}
            //catch (Exception e)
            //{
            //    e.Message.ToString();
            //    return "Cannot Print: ";
            //}

            // https://www.fast-report.com/en/forum/lofiversion/index.php/t6465.html
            //prew.ToolBar.Closing += new EventHandler(customClosing_Click);
            //string itemlist = "";

            //StatusBar.ApplicationStatus(report1);

            RemoveCloseButton(prewControl);
            AddCustomCloseButton(prewControl);
            RemoveEditButton(prewControl);
            //RemoveSaveButton(prewControl);
            RemoveOpenButton(prewControl);
            RemoveOutlineButton(prewControl);


            report1.Preview = prewControl;

            // Get data source from report
            FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource);            

            try
            {
                // Save query to data source
                tds.SelectCommand = sql; 

                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = AppSettings.Printers.ReportPrinter;
                report1.PrintSettings.PaperSource = AppSettings.Printers.ReportBin;

                // Hide the progress boxes
                FastReport.Utils.Config.ReportSettings.ShowProgress = false;

                // https://www.fast-report.com/en/forum/index.php?showtopic=5023
                if (report1.Prepare())
                {
                    // Do Stuff
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
                //var connection = report1.Dictionary.Connections[0];
                reportLogger.WriteLog("Cannot Prepare: " + e.Message.ToString());
                //return "Cannot Prepare: " + connection.GetConnection().ConnectionString;
                //return "Cannot Prepare: " + e.Message;
            }

            report1.ShowPrepared();
            WindowsFormHost1.Child = prewControl;

        }

        public ReportPreviewWindow(string reportFileName, bool useDefaultConnection, string sql1, string dataSource1, string sql2, string dataSource2)
        {
            string reportPath = (AppSettings.ReportConfigs.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Preview File: " + reportPath);
            reportLogger.WriteLog("Report SQL1: " + sql1);
            reportLogger.WriteLog("Report SQL2: " + sql2);

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                //return "File Not Found: " + reportPath;
            }

            RemoveExportItems();

            InitializeComponent();

            prewControl = new FastReport.Preview.PreviewControl();

            if (useDefaultConnection == false)
            {
                try
                {
                    //// Change connection string
                    string OldConnectionString = report1.Dictionary.Connections[0].ConnectionString;
                    string newConnectionString = "data source=AESSQL2;initial catalog=SOSDataExchange;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
                    //newConnectionString = "Data Source=" + AppSettings.Network.SQLServer + ";AttachDbFilename=;Initial Catalog=" + AppSettings.Network.LocalDatabase + ";Integrated Security=False;Persist Security Info=True;User ID=sa;Password=InkImpr35510n5";
                    newConnectionString = "Data Source=" + AppSettings.Network.SQLServer + ";AttachDbFilename=;Initial Catalog=" + AppSettings.Network.LocalDatabase + ";Integrated Security=False;Persist Security Info=True;User ID=sa;Password=@ESf13lddba";
                    report1.Dictionary.Connections[0].ConnectionString = newConnectionString;
                }
                catch (Exception e)
                {
                    e.Message.ToString();
                    reportLogger.WriteLog("Connection Error: " + e.Message.ToString());
                    //return "Connection Error: ";
                }
            }

            //try
            //{
            //    report1.PrintSettings.ShowDialog = false;
            //    report1.PrintPrepared();
            //}
            //catch (Exception e)
            //{
            //    e.Message.ToString();
            //    return "Cannot Print: ";
            //}

            // https://www.fast-report.com/en/forum/lofiversion/index.php/t6465.html
            //prew.ToolBar.Closing += new EventHandler(customClosing_Click);
            //string itemlist = "";

            //StatusBar.ApplicationStatus(report1);

            RemoveCloseButton(prewControl);
            AddCustomCloseButton(prewControl);
            RemoveEditButton(prewControl);
            //RemoveSaveButton(prewControl);
            RemoveOpenButton(prewControl);
            RemoveOutlineButton(prewControl);


            report1.Preview = prewControl;

            // Get data source from report
            FastReport.Data.TableDataSource tds1 = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource1);

            // Save query to data source
            tds1.SelectCommand = sql1;

            // Get data source from report
            FastReport.Data.TableDataSource tds2 = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource2);

            // Save query to data source
            tds2.SelectCommand = sql2;

            try
            {
                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = AppSettings.Printers.ReportPrinter;
                report1.PrintSettings.PaperSource = AppSettings.Printers.ReportBin;

                // Hide the progress boxes
                FastReport.Utils.Config.ReportSettings.ShowProgress = false;

                // https://www.fast-report.com/en/forum/index.php?showtopic=5023
                if (report1.Prepare())
                {
                    // Do Stuff
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
                var connection = report1.Dictionary.Connections[0];
                reportLogger.WriteLog("Cannot Prepare: " + e.Message.ToString());
                //return "Cannot Prepare: " + connection.GetConnection().ConnectionString;
                //return "Cannot Prepare: " + e.Message;
            }

            report1.ShowPrepared();
            WindowsFormHost1.Child = prewControl;

        }

        public ReportPreviewWindow(string reportFileName, SystemSettingsController system, string sqlDetail, string sqlHeader, bool useDefaultConnection, string dataSource)
        {
            string message = "";
            string reportPath = (system.Reports.ReportsFolder + "\\" + reportFileName + ".frx");

            RemoveExportItems();

            InitializeComponent();

            prewControl = new FastReport.Preview.PreviewControl();

            VoterXLogger reportLogger = new VoterXLogger("FastReports", system.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Print File: " + reportPath);
            reportLogger.WriteLog("Report SQL: " + sqlDetail);
            if (sqlHeader != null && sqlHeader != "")
            {
                reportLogger.WriteLog("Header SQL: " + sqlHeader);
            }

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                //return "File Not Found: " + reportPath;
            }

            RemoveCloseButton(prewControl);
            AddCustomCloseButton(prewControl);
            RemoveEditButton(prewControl);
            //RemoveSaveButton(prewControl);
            RemoveOpenButton(prewControl);
            RemoveOutlineButton(prewControl);


            report1.Preview = prewControl;

            if (useDefaultConnection == false)
            {
                try
                {
                    //// Change connection string
                    string OldConnectionString = report1.Dictionary.Connections[0].ConnectionString;
                    string newConnectionString = "data source=AESSQL2;initial catalog=SOSDataExchange;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
                    //newConnectionString = "Data Source=" + AppSettings.Network.SQLServer + ";AttachDbFilename=;Initial Catalog=" + AppSettings.Network.LocalDatabase + ";Integrated Security=False;Persist Security Info=True;User ID=sa;Password=InkImpr35510n5";
                    newConnectionString = "Data Source=" + system.Network.SQLServer + ";AttachDbFilename=;Initial Catalog=" + system.Network.LocalDatabase + ";Integrated Security=False;Persist Security Info=True;User ID=sa;Password=@ESf13lddba";

                    report1.Dictionary.Connections[0].ConnectionString = newConnectionString;
                    reportLogger.WriteLog("Connection String 1: " + report1.Dictionary.Connections[0].ConnectionString);

                    // Check if a second connection exists
                    try
                    {
                        report1.Dictionary.Connections[1].ConnectionString = newConnectionString;
                        reportLogger.WriteLog("Connection String 2: " + report1.Dictionary.Connections[1].ConnectionString);
                    }
                    catch { }
                }
                catch (Exception e)
                {
                    e.Message.ToString();
                    reportLogger.WriteLog("Connection Error: " + e.Message.ToString());
                    //return "Connection Error: ";
                }
            }
            else
            {
                reportLogger.WriteLog("Connection String 1: " + report1.Dictionary.Connections[0].ConnectionString);

                try
                {
                    reportLogger.WriteLog("Connection String 2: " + report1.Dictionary.Connections[1].ConnectionString);
                }
                catch { }
            }

            // Get data source from report
            FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource);

            FastReport.Data.TableDataSource tdsHeader = (FastReport.Data.TableDataSource)report1.GetDataSource("ElectionData");

            // Save query to data source
            tds.SelectCommand = sqlDetail;

            if (tdsHeader != null) tdsHeader.SelectCommand = sqlHeader;

            try
            {
                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = system.Printers.ReportPrinter;
                report1.PrintSettings.PaperSource = system.Printers.ReportBin;

                // Hide the progress boxes
                // https://stackoverflow.com/questions/30937179/how-to-print-fastreport-directly-without-showing-any-dialog-in-my-application
                FastReport.Utils.Config.ReportSettings.ShowProgress = false;

                // https://www.fast-report.com/en/forum/index.php?showtopic=5023
                if (report1.Prepare())
                {
                    // Do Stuff
                }
            }
            catch (Exception e)
            {
                var connection = report1.Dictionary.Connections[0];
                //return "Cannot Prepare: " + connection.GetConnection().ConnectionString;
                reportLogger.WriteLog("Cannot Prepare: " + e.Message.ToString());
                //return "Cannot Prepare: " + e.Message;
            }

            //try
            //{
            //    report1.PrintSettings.ShowDialog = false;
            //    report1.PrintPrepared();
            //}
            //catch (Exception e)
            //{
            //    e.Message.ToString();
            //    reportLogger.WriteLog("Cannot Print: " + e.Message.ToString());
            //    //return "Cannot Print: ";
            //}

            //return message;
            //return report1.PrintSettings.Printer.ToString();

            report1.ShowPrepared();
            WindowsFormHost1.Child = prewControl;
        }

        private void customClosing_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RemoveCloseButton(FastReport.Preview.PreviewControl preview)
        {
            foreach (var item in prewControl.ToolBar.Items)
            {
                if (item.ToString() == "Close")
                {
                    prewControl.ToolBar.Items.Remove((FastReport.DevComponents.DotNetBar.BaseItem)item);
                    return;
                }
            }
        }

        private void RemoveEditButton(FastReport.Preview.PreviewControl preview)
        {
            foreach (var item in prewControl.ToolBar.Items)
            {
                if (item.ToString() == "Edit")
                {
                    prewControl.ToolBar.Items.Remove((FastReport.DevComponents.DotNetBar.BaseItem)item);
                    return;
                }
            }
        }

        private void RemoveSaveButton(FastReport.Preview.PreviewControl preview)
        {
            foreach (var item in prewControl.ToolBar.Items)
            {
                if (item.ToString() == "Save")
                {
                    prewControl.ToolBar.Items.Remove((FastReport.DevComponents.DotNetBar.BaseItem)item);
                    return;
                }
            }                      
        }

        private void RemoveOpenButton(FastReport.Preview.PreviewControl preview)
        {
            foreach (var item in prewControl.ToolBar.Items)
            {
                if (item.ToString() == "Open")
                {
                    prewControl.ToolBar.Items.Remove((FastReport.DevComponents.DotNetBar.BaseItem)item);
                    return;
                }
            }
        }

        private void RemoveOutlineButton(FastReport.Preview.PreviewControl preview)
        {
            foreach (var item in prewControl.ToolBar.Items)
            {
                if (item.ToString() == "Outline")
                {
                    prewControl.ToolBar.Items.Remove((FastReport.DevComponents.DotNetBar.BaseItem)item);
                    return;
                }
            }
        }

        // https://www.fast-report.com/en/forum/index.php?showtopic=13992
        private void RemoveExportItems()
        {
            // to initialize RegisteredObjects, you must do this before initializing PreviewControl
            //report = new FastReport.Report();
            // get list of RegisteredObjects (i do this approach for experiment and learning)
            // you could use RegisteredObjects.FindObject
            List<FastReport.Utils.ObjectInfo> list = new List<FastReport.Utils.ObjectInfo>();
            FastReport.Utils.RegisteredObjects.Objects.EnumItems(list);
            // find object
            foreach (FastReport.Utils.ObjectInfo item in list)
            {
                // Remove all but the PDF export
                if (item.Object != typeof(FastReport.Export.Pdf.PDFExport))
                {
                    // remove it from list
                    item.Enabled = false;
                    //break;
                }
            }
        }

        // https://www.fast-report.com/en/forum/lofiversion/index.php/t6465.html
        private void AddCustomCloseButton(FastReport.Preview.PreviewControl preview)
        {
            ButtonItem customButton = new ButtonItem();
            customButton.Text = "Close";
            customButton.Click += new EventHandler(customClosing_Click);
            preview.ToolBar.Items.Add(customButton);
        }

        //https://www.fast-report.com/en/forum/index.php?showtopic=13042
        private void ChangeDataSource()
        {

        }
    }
}
