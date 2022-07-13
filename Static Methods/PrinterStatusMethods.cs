using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using VoterX.Models;

namespace VoterX.Methods
{

    // Alternate job status check https://stackoverflow.com/questions/4261984/system-printing-printqueue-queuestatus-not-updating

    public static class PrinterStatus
    {
        public static IEnumerable<string> PrintersList()
        {
            List<string> printers = new List<string>();

            // Set management scope
            //ManagementScope scope = new ManagementScope("\\root\\cimv2");
            //scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            // Add blank item to list
            //AddComboItemToControl(sender, "", "");

            foreach (ManagementObject printer in searcher.Get())
            {
                printers.Add(printer["Name"].ToString());
            }

            return printers;
        }

        public static async Task<IEnumerable<PrinterTray>> TraysList(string printerName)
        {
            return await Task.Run(() => { 
                List<PrinterTray> trays = new List<PrinterTray>();

                using (System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument())
                {
                    // Attach printer to print document
                    pd.PrinterSettings.PrinterName = printerName;

                    // Loop through printer trays
                    for (int i = 0; i < pd.PrinterSettings.PaperSources.Count; i++)
                    {
                        // PaperSources are the trays
                        trays.Add(new PrinterTray
                            {
                                Index = pd.PrinterSettings.PaperSources[i].RawKind,
                                Name = pd.PrinterSettings.PaperSources[i].SourceName
                            });
                    }
                }

                return trays;
            });
        }

        public static async Task<IEnumerable<PaperSize>> PaperSizesList(string printerName)
        {
            return await Task.Run(() => {
                List<PaperSize> sizes = new List<PaperSize>();

                using (System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument())
                {
                    // Attach printer to print document
                    pd.PrinterSettings.PrinterName = printerName;

                    // Loop through printer trays
                    for (int i = 0; i < pd.PrinterSettings.PaperSizes.Count; i++)
                    {
                        // PaperSources are the trays
                        sizes.Add(new PaperSize
                        {
                            Index = pd.PrinterSettings.PaperSizes[i].RawKind,
                            Name = pd.PrinterSettings.PaperSizes[i].PaperName
                        });
                    }
                }

                return sizes;
            });
        }

        public static string GetPrinterState(string printerName)
        {
            // Set management scope
            ManagementScope scope = new ManagementScope("\\root\\cimv2");
            scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            string output = "";
            // Loop through each printer
            foreach (ManagementObject printer in searcher.Get())
            {
                // Get printer name
                // List of properties https://msdn.microsoft.com/en-us/library/Aa394363
                if (printer["Name"].ToString().ToUpper().Equals(printerName))
                {
                    switch(Int32.Parse(printer["PrinterState"].ToString()))
                    {
                        case 0:
                            output = "Idle";
                            break;
                        case 1:
                            output = "Paused";
                            break;
                        case 2:
                            output = "Error";
                            break;
                        case 3:
                            output = "Pending Deletion";
                            break;
                        case 4:
                            output = "Paper Jam";
                            break;
                        case 5:
                            output = "Paper Out";
                            break;
                        case 6:
                            output = "Manual Feed";
                            break;
                        case 7:
                            output = "Paper Problem";
                            break;
                        case 8:
                            output = "Offline";
                            break;
                        case 9:
                            output = "I/O Active";
                            break;
                        case 10:
                            output = "Busy";
                            break;
                        case 11:
                            output = "Printing";
                            break;
                        case 12:
                            output = "Output Bin Full";
                            break;
                        case 13:
                            output = "Not Available";
                            break;
                        case 14:
                            output = "Waiting";
                            break;
                        case 15:
                            output = "Processing";
                            break;
                        case 16:
                            output = "Initialization";
                            break;
                        case 17:
                            output = "Warming Up";
                            break;
                        case 18:
                            output = "Toner Low";
                            break;
                        case 19:
                            output = "No Toner";
                            break;
                        case 20:
                            output = "Page Punt";
                            break;
                        case 21:
                            output = "User Intervention Required";
                            break;
                        case 22:
                            output = "Out of Memory";
                            break;
                        case 23:
                            output = "Door Open";
                            break;
                        case 24:
                            output = "Server_Unknown";
                            break;
                        case 25:
                            output = "Power Save";
                            break;
                    }
                }
            }

            scope = null;

            return output;
        }

        public static string GetPrinterStatus(string printerName)
        {
            // Set management scope
            ManagementScope scope = new ManagementScope("\\root\\cimv2");
            scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            //string printerName = "";
            string output = "";
            // Loop through each printer
            foreach (ManagementObject printer in searcher.Get())
            {
                // Get printer name
                // List of properties https://msdn.microsoft.com/en-us/library/Aa394363
                if (printer["Name"].ToString().ToUpper().Equals(printerName))
                {
                    switch (Int32.Parse(printer["PrinterStatus"].ToString()))
                    {
                        case 0:
                            output = "";
                            break;
                        case 1:
                            output = "Other";
                            break;
                        case 2:
                            output = "Unknown";
                            break;
                        case 3:
                            output = "Idle";
                            break;
                        case 4:
                            output = "Printing";
                            break;
                        case 5:
                            output = "Warming up";
                            break;
                        case 6:
                            output = "Stopped Printing";
                            break;
                        case 7:
                            output = "Offline";
                            break;
                        
                    }
                }
            }

            scope = null;

            return output;
        }

        public static bool PrinterIsReady(string printerName)
        {
            // Set management scope
            ManagementScope scope = new ManagementScope("\\root\\cimv2");
            scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            //string printerName = "";
            bool result = false;
            // Loop through each printer
            foreach (ManagementObject printer in searcher.Get())
            {
                // Get printer name
                // List of properties https://msdn.microsoft.com/en-us/library/Aa394363
                if (printer["Name"].ToString().ToUpper().Equals(printerName))
                {
                    if (
                        Int32.Parse(printer["PrinterStatus"].ToString()) == 3 &&
                        Int32.Parse(printer["PrinterState"].ToString()) == 0
                        )
                    {
                        result = true;
                    }

                }
            }

            scope = null;

            return result;
        }

        public static async Task<bool> PrinterIsReadyAsync(string printerName)
        {
            // Set management scope
            ManagementScope scope = new ManagementScope("\\root\\cimv2");
            scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            //string printerName = "";
            bool result = false;

            await Task.Run(() =>
            {
                // Loop through each printer
                foreach (ManagementObject printer in searcher.Get())
                {
                    // Get printer name
                    // List of properties https://msdn.microsoft.com/en-us/library/Aa394363
                    if (printer["Name"].ToString().ToUpper().Equals(printerName))
                    {
                        if (
                            Int32.Parse(printer["PrinterStatus"].ToString()) == 3 &&
                            Int32.Parse(printer["PrinterState"].ToString()) == 0
                            )
                        {
                            result = true;
                        }

                    }
                }
            });

            scope = null;

            return result;
        }

        // Check printer status example
        //private void CheckPrinter()
        //{
        //    // Set management scope
        //    ManagementScope scope = new ManagementScope("\\root\\cimv2");
        //    scope.Connect();

        //    // Select Printers from WMI Object Collections
        //    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
        //    string printerName = "";
        //    string output = "";
        //    // Loop through each printer
        //    foreach (ManagementObject printer in searcher.Get())
        //    {
        //        // Get printer name
        //        // List of properties https://msdn.microsoft.com/en-us/library/Aa394363
        //        printerName = printer["Name"].ToString().ToUpper();
        //        if (printerName.Contains("RICOH AFICIO SP 6330N PCL 6"))
        //        {
        //            // Check if printer is offline
        //            if (printer["WorkOffline"].Equals("True")) output = "Offline";
        //            else output = "Online";

        //            // Create print document in order to check printer trays
        //            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();

        //            pd.PrinterSettings.PrinterName = printerName;

        //            output += " Trays: ";

        //            // Loop through printer trays
        //            for (int i = 0; i < pd.PrinterSettings.PaperSources.Count; i++)
        //            {
        //                // PaperSources are the trays
        //                output += pd.PrinterSettings.PaperSources[i].SourceName + " | ";
        //            }

        //            output = printer["PrinterStatus"].ToString();
        //        }
        //    }

        //    scope = null;

        //    // Set status bar on main window to display the printer status and list of trays
        //    StatusBar.ApplicationStatusRight("Check Printer: " + output);
        //}
    }
}
