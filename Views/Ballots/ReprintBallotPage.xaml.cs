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
using System.Management;
using System.Drawing.Printing;
using VoterX.Extensions;
using VoterX.Methods;
//using VoterX.Core.Models.ViewModels;
using VoterX.SystemSettings.Extensions;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for PrintBallotPage.xaml
    /// </summary>
    public partial class ReprintBallotPage : Page
    {
        //private VoterDataModel _voter = new VoterDataModel();

        //public ReprintBallotPage(VoterDataModel voter)
        //{
        //    InitializeComponent();

        //    _voter = voter;

        //    //StatusBar.ApplicationStatus(string.Concat("Voter ID: ", voter.VoterID));

        //    //StatusBar.ApplicationStatusRight("Check Printer: offline");

        //    //CheckPrinter();

        //    LoadVoterFields(voter);

        //    StatusBar.ApplicationPageHeader("Reprint Ballot");
        //    //StatusBar.ApplicationStatus("Print Ballot Page Loaded");
        //}

        //private void LoadVoterFields(VoterDataModel voter)
        //{
        //    FullName.Text = voter.FullName;
        //    BirthYear.Text = voter.DOBYear;
        //    Address.Text = voter.Address1;
        //    CityStateAndZip.Text = voter.City + ", " + voter.State + " " + voter.Zip;
        //}

        // Return to search screen without search parameters
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogUser("RETURNING TO SEARCH PAGE");
            //var frame = NavigationMethods.FindParent<Frame>(this);
            //var prevPage = new HomePage(null);
            //frame.Navigate(prevPage);
            this.NavigateToPage(new SearchPage());
        }

        private void CheckPrinter()
        {
            // Set management scope
            ManagementScope scope = new ManagementScope("\\root\\cimv2");
            scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            string printerName = "";
            string output = "";
            // Loop through each printer
            foreach (ManagementObject printer in searcher.Get())
            {
                // Get printer name
                printerName = printer["Name"].ToString().ToUpper();
                if (printerName.Contains("RICOH"))
                {
                    // Check if printer is offline
                    if (printer["WorkOffline"].Equals("True")) output = "Offline";
                    else output = "Online";

                    // Create print document in order to check printer trays
                    System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();

                    pd.PrinterSettings.PrinterName = printerName;

                    output += " Trays: ";

                    // Loop through printer trays
                    for (int i = 0; i < pd.PrinterSettings.PaperSources.Count; i++)
                    {
                        // PaperSources are the trays
                        output += pd.PrinterSettings.PaperSources[i].SourceName + " | ";
                    }
                }
            }

            scope = null;

            // Set status bar on main window to display the printer status and list of trays
            GlobalReferences.StatusBar.TextCenter = ("Check Printer: " + output);
            //LoggingMethods.LogIO("CHECK PRINTER " + output);
        }

        private void PrintBallot_Click(object sender, RoutedEventArgs e)
        {
            // Reprint ballot and/or permit
            //BallotPrinting.ReprintBallot(_voter);

            // Goto the print varification page
            //this.NavigateToPage(new SpoiledBallotVerifyPage(_voter));
        }
    }
}
