using VoterX.Logging;
using VoterX.SystemSettings;
using VoterX.SystemSettings.Models;
using FastReport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoterX.Methods
{
    public static class FastReportsMethods
    {
        // Change connection strings
        // https://www.fast-report.com/en/forum/index.php?showtopic=5332
        // https://www.fast-report.com/en/forum/index.php?showtopic=4778

        public static void PreviewReport(string reportFileName, string sql, bool useDefaultConnection, string dataSource)
        {
            ReportPreviewWindow reportPreview = new ReportPreviewWindow(reportFileName, sql, useDefaultConnection, dataSource);
            reportPreview.ShowDialog();
        }

        public static void PreviewReport(string reportFileName, bool useDefaultConnection, string sql1, string dataSource1, string sql2, string dataSource2)
        {
            ReportPreviewWindow reportPreview = new ReportPreviewWindow(reportFileName, useDefaultConnection, sql1, dataSource1, sql2, dataSource2);
            reportPreview.ShowDialog();
        }

        public static void PreviewReport(string reportFileName, SystemSettingsController system, string sqlDetail, string sqlHeader, bool useDefaultConnection, string dataSource)
        {
            ReportPreviewWindow reportPreview = new ReportPreviewWindow(reportFileName, system, sqlDetail, sqlHeader, useDefaultConnection, dataSource);
            reportPreview.ShowDialog();
        }

        public static string PrintSilentReport(string reportFileName)
        {
            string message = "";
            string reportPath = (AppSettings.ReportConfigs.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Print File: " + reportPath);

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                message = "File Not Found";
            }

            try
            {
                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = AppSettings.Printers.ReportPrinter;
                report1.PrintSettings.PaperSource = AppSettings.Printers.ReportBin;

                // Hide the progress boxes
                // https://stackoverflow.com/questions/30937179/how-to-print-fastreport-directly-without-showing-any-dialog-in-my-application
                FastReport.Utils.Config.ReportSettings.ShowProgress = false;

                // https://www.fast-report.com/en/forum/index.php?showtopic=5023
                if (report1.Prepare())
                {
                    // Do Stuff
                }

                report1.PrintSettings.ShowDialog = false;
                report1.PrintPrepared();

                reportLogger.WriteLog("Printed To: " + report1.PrintSettings.Printer + " Tray: " + report1.PrintSettings.PaperSource);
            }
            catch (Exception e)
            {
                reportLogger.WriteLog("Connection Error: " + e.Message.ToString());
                return e.Message.ToString();
            }

            return message;
        }

        // https://www.fast-report.com/en/forum/index.php?showtopic=14434
        public static string PrintSilentReport(string reportFileName, string sql, bool useDefaultConnection)
        {
            string message = "";
            string reportPath = (AppSettings.ReportConfigs.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Print File: " + reportPath);
            reportLogger.WriteLog("Report SQL: " + sql);

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                return "File Not Found: " + reportPath;
            }

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
                    return "Connection Error: ";
                }
            }

            try
            {
                // Get data source from report
                FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)report1.GetDataSource("ApplicationData");

                // Save query to data source
                tds.SelectCommand = sql;
            }
            catch (Exception e)
            {
                reportLogger.WriteLog("Data Source Error: " + e.Message.ToString());
                return e.Message.ToString();
            }

            try
            {
                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = AppSettings.Printers.ReportPrinter;
                report1.PrintSettings.PaperSource = AppSettings.Printers.ReportBin;

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
                reportLogger.WriteLog("Cannot Prepare: " + e.Message.ToString());
                //return "Cannot Prepare: " + connection.GetConnection().ConnectionString;
                return "Cannot Prepare: " + e.Message;
            }

            try
            {
                report1.PrintSettings.ShowDialog = false;
                report1.PrintPrepared();

                reportLogger.WriteLog("Printed To: " + report1.PrintSettings.Printer + " Tray: " + report1.PrintSettings.PaperSource);
            }
            catch (Exception e)
            {
                e.Message.ToString();
                reportLogger.WriteLog("Cannot Print: " + e.Message.ToString());
                return "Cannot Print: ";
            }

            return message;
        }

        public static string PrintSilentReport(string reportFileName, string sql, bool useDefaultConnection, PrinterSettingsModel printer, string printerName, int bin)
        {
            string message = ""; 
            string reportPath = (AppSettings.ReportConfigs.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);

            reportLogger.WriteLog("Loading Report Print File: " + reportPath);
            reportLogger.WriteLog("Report SQL: " + sql);
            //if (AppSettings.System.ReportErrorLogging == true)
            //{
            //    LogFileMethods.WriteLogToFile("FastReports", "Loading Report File: " + reportPath);
            //    LogFileMethods.WriteLogToFile("FastReports", "Report SQL: " + sql);
            //}

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                //if (AppSettings.System.ReportErrorLogging == true)
                //{
                //    LogFileMethods.WriteLogToFile("FastReports", "File Not Found: " + reportPath);
                //}
                return "File Not Found: " + reportPath;
            }

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
                    //if (AppSettings.System.ReportErrorLogging == true)
                    //{
                    //    LogFileMethods.WriteLogToFile("FastReports", "Connection Error: " + e.Message.ToString());
                    //}
                    return "Connection Error: ";
                }
            }

            try
            {
                // Get data source from report
                FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)report1.GetDataSource("ApplicationData");

                // Save query to data source
                tds.SelectCommand = sql;
            }
            catch (Exception e)
            {
                reportLogger.WriteLog("Data Source Error: " + e.Message.ToString());
                //if (AppSettings.System.ReportErrorLogging == true)
                //{
                //    LogFileMethods.WriteLogToFile("FastReports", "Data Source Error: " + e.Message.ToString());
                //}
                return e.Message.ToString();
            }

            try
            {
                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = printerName;
                report1.PrintSettings.PaperSource = bin;

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
                //if (AppSettings.System.ReportErrorLogging == true)
                //{
                //    LogFileMethods.WriteLogToFile("FastReports", "Cannot Prepare: " + e.Message.ToString());
                //}
                return "Cannot Prepare: " + e.Message;
            }

            try
            {
                report1.PrintSettings.ShowDialog = false;
                report1.PrintPrepared();

                reportLogger.WriteLog("Printed To: " + report1.PrintSettings.Printer + " Tray: " + report1.PrintSettings.PaperSource);
            }
            catch (Exception e)
            {
                e.Message.ToString();
                reportLogger.WriteLog("Cannot Print: " + e.Message.ToString());
                //if (AppSettings.System.ReportErrorLogging == true)
                //{
                //    LogFileMethods.WriteLogToFile("FastReports", "Cannot Print: " + e.Message.ToString());
                //}
                return "Cannot Print: ";
            }

            return message;
        }

        public static string PrintSilentReport(string reportFileName, string sql, bool useDefaultConnection, string dataSource)
        {
            string message = "";
            string reportPath = (AppSettings.ReportConfigs.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Print File: " + reportPath);
            reportLogger.WriteLog("Report SQL: " + sql);

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                return "File Not Found: " + reportPath;
            }

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
                    return "Connection Error: ";
                }
            }

            try
            {
                // Get data source from report
                FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource);

                // Save query to data source
                tds.SelectCommand = sql;
            }
            catch (Exception e)
            {
                reportLogger.WriteLog("Data Source Error: " + e.Message.ToString());
                return e.Message.ToString();
            }

            try
            {
                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = AppSettings.Printers.ReportPrinter;
                report1.PrintSettings.PaperSource = AppSettings.Printers.ReportBin;

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
                return "Cannot Prepare: " + e.Message;
            }

            try
            {
                report1.PrintSettings.ShowDialog = false;
                report1.PrintPrepared();

                reportLogger.WriteLog("Printed To: " + report1.PrintSettings.Printer + " Tray: " + report1.PrintSettings.PaperSource);
            }
            catch (Exception e)
            {
                e.Message.ToString();
                reportLogger.WriteLog("Cannot Print: " + e.Message.ToString());
                return "Cannot Print: ";
            }

            return message;
        }

        public static string PrintSortedSilentReport(string reportFileName, string sql, bool useDefaultConnection, string dataSource)
        {
            string message = "";
            string reportPath = (AppSettings.ReportConfigs.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Print File: " + reportPath);
            reportLogger.WriteLog("Report SQL: " + sql);

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                return "File Not Found: " + reportPath;
            }

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
                    return "Connection Error: ";
                }
            }

            try
            {
                // Get data source from report
                FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource);

                // Save query to data source
                tds.SelectCommand = sql;
            }
            catch (Exception e)
            {
                reportLogger.WriteLog("Data Source Error: " + e.Message.ToString());
                return e.Message.ToString();
            }

            try
            {
                // Sorting Example
                // https://stackoverflow.com/questions/31922898/c-sharp-how-to-change-sort-order-in-fast-report
                (report1.FindObject("Data1") as DataBand).Sort.Add(new Sort("[name_last]", true));
            }
            catch (Exception e)
            {
                reportLogger.WriteLog("Cannot Sort Error: " + e.Message.ToString());
            }

            try
            {
                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = AppSettings.Printers.ReportPrinter;
                report1.PrintSettings.PaperSource = AppSettings.Printers.ReportBin;

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
                return "Cannot Prepare: " + e.Message;
            }

            try
            {
                report1.PrintSettings.ShowDialog = false;
                report1.PrintPrepared();

                reportLogger.WriteLog("Printed To: " + report1.PrintSettings.Printer + " Tray: " + report1.PrintSettings.PaperSource);
            }
            catch (Exception e)
            {
                e.Message.ToString();
                reportLogger.WriteLog("Cannot Print: " + e.Message.ToString());
                return "Cannot Print: ";
            }

            return message;
        }

        public static string PrintSilentLabels(string reportFileName, string sql, bool useDefaultConnection, string dataSource)
        {
            string message = "";
            string reportPath = (AppSettings.ReportConfigs.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Print File: " + reportPath);
            reportLogger.WriteLog("Report SQL: " + sql);

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                return "File Not Found: " + reportPath;
            }

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
                    return "Connection Error: ";
                }
            }

            try
            {
                // Get data source from report
                FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource);

                // Save query to data source
                tds.SelectCommand = sql;
            }
            catch (Exception e)
            {
                reportLogger.WriteLog("Data Source Error: " + e.Message.ToString());
                return e.Message.ToString();
            }

            try
            {
                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = AppSettings.Printers.LabelPrinter;
                report1.PrintSettings.PaperSource = AppSettings.Printers.LabelBin;

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
                return "Cannot Prepare: " + e.Message;
            }

            try
            {
                report1.PrintSettings.ShowDialog = false;
                report1.PrintPrepared();

                reportLogger.WriteLog("Printed To: " + report1.PrintSettings.Printer + " Tray: " + report1.PrintSettings.PaperSource);
            }
            catch (Exception e)
            {
                e.Message.ToString();
                reportLogger.WriteLog("Cannot Print: " + e.Message.ToString());
                return "Cannot Print: ";
            }

            return message;
        }

        public static string PrintSilentReport(string reportFileName, string reportFolder, PrinterSettingsModel printer, string sql, bool useDefaultConnection, NetworkSettingsModel network, string dataSource)
        {
            string message = "";
            string reportPath = (reportFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Print File: " + reportPath);
            reportLogger.WriteLog("Report SQL: " + sql);

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                return "File Not Found: " + reportPath;
            }

            if (useDefaultConnection == false)
            {
                try
                {
                    //// Change connection string
                    string OldConnectionString = report1.Dictionary.Connections[0].ConnectionString;
                    string newConnectionString = "data source=AESSQL2;initial catalog=SOSDataExchange;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
                    //newConnectionString = "Data Source=" + AppSettings.Network.SQLServer + ";AttachDbFilename=;Initial Catalog=" + AppSettings.Network.LocalDatabase + ";Integrated Security=False;Persist Security Info=True;User ID=sa;Password=InkImpr35510n5";
                    newConnectionString = "Data Source=" + network.SQLServer + ";AttachDbFilename=;Initial Catalog=" + network.LocalDatabase + ";Integrated Security=False;Persist Security Info=True;User ID=sa;Password=@ESf13lddba";
                    report1.Dictionary.Connections[0].ConnectionString = newConnectionString;
                }
                catch (Exception e)
                {
                    e.Message.ToString();
                    reportLogger.WriteLog("Connection Error: " + e.Message.ToString());
                    return "Connection Error: ";
                }
            }

            try
            {
                // Get data source from report
                FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource);

                // Save query to data source
                tds.SelectCommand = sql;
            }
            catch (Exception e)
            {
                reportLogger.WriteLog("Data Source Error: " + e.Message.ToString());
                return e.Message.ToString();
            }

            try
            {
                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = printer.ApplicationPrinter;
                report1.PrintSettings.PaperSource = printer.AppBin;

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
                return "Cannot Prepare: " + e.Message;
            }

            try
            {
                report1.PrintSettings.ShowDialog = false;
                report1.PrintPrepared();

                reportLogger.WriteLog("Printed To: " + report1.PrintSettings.Printer + " Tray: " + report1.PrintSettings.PaperSource);
            }
            catch (Exception e)
            {
                e.Message.ToString();
                reportLogger.WriteLog("Cannot Print: " + e.Message.ToString());
                return "Cannot Print: ";
            }

            return message;
            //return report1.PrintSettings.Printer.ToString();
        }

        public static string PrintSilentReport(string reportFileName, SystemSettingsController system, string sqlDetail, string sqlHeader, bool useDefaultConnection, string dataSource)
        {
            string message = "";
            string reportPath = (system.Reports.ReportsFolder + "\\" + reportFileName + ".frx");

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
                return "File Not Found: " + reportPath;
            }

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
                    return "Connection Error: ";
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
                return "Cannot Prepare: " + e.Message;
            }

            try
            {
                report1.PrintSettings.ShowDialog = false;
                report1.PrintPrepared();
            }
            catch (Exception e)
            {
                e.Message.ToString();
                reportLogger.WriteLog("Cannot Print: " + e.Message.ToString());
                return "Cannot Print: ";
            }

            return message;
            //return report1.PrintSettings.Printer.ToString();
        }

        public static string PrintSilentReport(string reportFileName, SystemSettingsController system, string sql, bool useDefaultConnection, string dataSource)
        {
            string message = "";
            string reportPath = (system.Reports.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", system.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Print File: " + reportPath);
            reportLogger.WriteLog("Report SQL: " + sql);

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                return "File Not Found: " + reportPath;
            }

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
                }
                catch (Exception e)
                {
                    e.Message.ToString();
                    reportLogger.WriteLog("Connection Error: " + e.Message.ToString());
                    return "Connection Error: ";
                }
            }

            // Get data source from report
            FastReport.Data.TableDataSource tds = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource);

            // Save query to data source
            tds.SelectCommand = sql;

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
                return "Cannot Prepare: " + e.Message;
            }

            try
            {
                report1.PrintSettings.ShowDialog = false;
                report1.PrintPrepared();
            }
            catch (Exception e)
            {
                e.Message.ToString();
                reportLogger.WriteLog("Cannot Print: " + e.Message.ToString());
                return "Cannot Print: ";
            }

            return message;
            //return report1.PrintSettings.Printer.ToString();
        }

        public static string PrintSilentReportDual(string reportFileName, bool useDefaultConnection, string sql1, string dataSource1, string sql2, string dataSource2)
        {
            string message = "";
            string reportPath = (AppSettings.ReportConfigs.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Print File: " + reportPath);
            reportLogger.WriteLog("Report SQL1: " + sql1);
            reportLogger.WriteLog("Report SQL2: " + sql2);

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                return "File Not Found: " + reportPath;
            }

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
                    return "Connection Error: ";
                }
            }

            try
            {
                // Get data source from report
                FastReport.Data.TableDataSource tds1 = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource1);

                // Save query to data source
                tds1.SelectCommand = sql1;

                // Get data source from report
                FastReport.Data.TableDataSource tds2 = (FastReport.Data.TableDataSource)report1.GetDataSource(dataSource2);

                // Save query to data source
                tds2.SelectCommand = sql2;
            }
            catch (Exception e)
            {
                reportLogger.WriteLog("Data Source Error: " + e.Message.ToString());
                return e.Message.ToString();
            }

            try
            {
                // https://www.fast-report.com/en/forum/index.php?showtopic=14341
                report1.PrintSettings.Printer = AppSettings.Printers.ReportPrinter;
                report1.PrintSettings.PaperSource = AppSettings.Printers.ReportBin;

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
                return "Cannot Prepare: " + e.Message;
            }

            try
            {
                report1.PrintSettings.ShowDialog = false;
                report1.PrintPrepared();

                reportLogger.WriteLog("Printed To: " + report1.PrintSettings.Printer + " Tray: " + report1.PrintSettings.PaperSource);
            }
            catch (Exception e)
            {
                e.Message.ToString();
                reportLogger.WriteLog("Cannot Print: " + e.Message.ToString());
                return "Cannot Print: ";
            }

            return message;
        }

        public static string GetDataSource(string reportFileName)
        {
            string message = "";
            string reportPath = (AppSettings.ReportConfigs.ReportsFolder + "\\" + reportFileName + ".frx");

            VoterXLogger reportLogger = new VoterXLogger("FastReports", AppSettings.System.ReportErrorLogging);
            reportLogger.WriteLog("Loading Report Print File: " + reportPath);

            FastReport.Report report1 = new FastReport.Report();

            try
            {
                report1.Load(reportPath);
            }
            catch
            {
                reportLogger.WriteLog("File Not Found: " + reportPath);
                message = "File Not Found";
            }

            //var datasource = report1.GetDataSource("VoterDataConnection");
            message = report1.Dictionary.Connections[0].ConnectionString;

            //var connection = report1.Dictionary.Connections[0];

            //connection.GetConnection().ConnectionString = ConfigurationManager.ConnectionStrings["VoterDatabase"].ConnectionString;

            //foreach (FastReport.Data.MsSqlDataConnection connection in report1.Dictionary.Connections)
            //{
            //    message = connection.ConnectionString.ToString();
            //}

            return message;
        }

    }
}
