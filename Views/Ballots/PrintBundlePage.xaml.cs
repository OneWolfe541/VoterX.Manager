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
using VoterX.Dialogs;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for PrintBallotPage.xaml
    /// </summary>
    public partial class PrintBundlePage : Page
    {
        //private VoterDataModel _voter = new VoterDataModel();

        private int printingErrors = 0;

        private bool exitLogout = false;

        //public PrintBundlePage(VoterDataModel voter)
        //{
        //    InitializeComponent();

        //    _voter = voter;

        //    //StatusBar.ApplicationStatus(string.Concat("Voter ID: ", voter.VoterID));

        //    //StatusBar.ApplicationStatusRight("Check Printer: offline");

        //    //CheckPrinter();

        //    LoadVoterFields(voter);

        //    CheckPrinter();

        //    StatusBar.ApplicationPageHeader("Print Ballot");
        //    //StatusBar.ApplicationStatus("Print Ballot Page Loaded");
        //    StatusBar.ApplicationStatusClear();
        //}

        //public PrintBundlePage(VoterDataModel voter, bool returnStatus)
        //{
        //    InitializeComponent();

        //    _voter = voter;
        //    exitLogout = returnStatus;

        //    LoadVoterFields(voter);

        //    CheckPrinter();

        //    StatusBar.ApplicationPageHeader("Print Ballot");
        //    //StatusBar.ApplicationStatus("Print Ballot Page Loaded");
        //    StatusBar.ApplicationStatusClear();
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
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new LoginPage());
            }
        }

        private void CheckPrinter()
        {
            if (PrinterStatus.PrinterIsReady(AppSettings.Printers.BallotPrinter))
            {
                PrintBallot.Visibility = Visibility.Visible;
                //StatusBar.ApplicationStatusCenter("Printer Ready");
            }
            else
            {
                printingErrors++;
                PrinterErrorPanel.Visibility = Visibility.Visible;
                //StatusBar.ApplicationStatusCenter("Printer Status: "
                //    + PrinterStatus.GetPrinterStatus(AppSettings.Printers.BallotPrinter)
                //    + " - State: " + PrinterStatus.GetPrinterState(AppSettings.Printers.BallotPrinter)
                //    );
                GlobalReferences.StatusBar.TextLeft = (false).ToString();
            }
        }

        private void PrintBallot_Click(object sender, RoutedEventArgs e)
        {
            // Prevent spam clicking
            PrintBallot.IsEnabled = false;

            //Mark the Voter
            //VoterMethods.InsertVotedAtPolls(_voter);

            // Display voters ballot style filename
            //StatusBar.ApplicationStatusRight(AppSettings.Ballots.BallotFolder + "\\" + _voter.BallotStyleFile);            

            // Check printer status
            if (PrinterStatus.PrinterIsReady(AppSettings.Printers.BallotPrinter))
            {
                // Printer is ready - Go ahead and Print
                //StatusBar.ApplicationStatusCenter("Printer Ready");

                // Print ballot, application, permit, and/or stub
                //StatusBar.ApplicationStatus(await Task.Run(() => BallotPrinting.PrintBallotBundle(_voter)));

                //if (_voter.SignRefused == true)
                //{
                //    if (AppSettings.System.VCCType == 2) // Election Day Mode
                //    {
                //        // Print signature form
                //        await Task.Run(() => BallotPrinting.PrintSignatureForm(_voter));

                //        // Display message
                //        AlertDialog signatureDialog = new AlertDialog("MAKE SURE THE VOTER SIGNS THE SIGNATURE FORM");
                //        signatureDialog.ShowDialog();                        
                //    }
                //    else
                //    {
                //        // Display message
                //        AlertDialog signatureDialog = new AlertDialog("MAKE SURE THE VOTER SIGNS THE PRINTED APPLICATION");
                //        signatureDialog.ShowDialog();
                //    }
                //}

                // Goto the print varification page
            //    if (exitLogout == true)
            //    {
            //        this.NavigateToPage(new PrintVerifyTroubleshootPage(_voter, true));
            //    }
            //    else
            //    {
            //this.NavigateToPage(new PrintVerifyTroubleshootPage(_voter));
            //    }

            }
            else
            {
                // Add 1 to error count
                printingErrors++;
                // Stop the process if they have passed this point before
                if (printingErrors <= 1)
                {
                    // Show error message and rest buttons
                    PrinterErrorPanel.Visibility = Visibility.Visible;
                    // Turn off No button
                    ReadyToPrintNo.IsChecked = false;
                    ready_print_fa_check_no.Visibility = Visibility.Collapsed;
                    // Turn off the Yes button
                    ReadyToPrintYes.IsChecked = false;
                    ready_print_fa_check_yes.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // Show return to search button
                    OptOutButton.Visibility = Visibility.Visible;
                    SeriousPrinterErrorPanel.Visibility = Visibility.Visible;
                }

                // Hide print button
                PrintBallot.Visibility = Visibility.Collapsed;
                // Display printer state and status
                //StatusBar.ApplicationStatusCenter("Printer Status: "
                //    + PrinterStatus.GetPrinterStatus(AppSettings.Printers.BallotPrinter)
                //    + " - State: " + PrinterStatus.GetPrinterState(AppSettings.Printers.BallotPrinter)
                //    );
                GlobalReferences.StatusBar.TextLeft = (false).ToString();
            }

        }

        private void ReadyToPrintYes_Click(object sender, RoutedEventArgs e)
        {
            // Turn on Yes button
            ReadyToPrintYes.IsChecked = true;
            ready_print_fa_check_yes.Visibility = Visibility.Visible;

            // Turn off No button
            ReadyToPrintNo.IsChecked = false;
            ready_print_fa_check_no.Visibility = Visibility.Collapsed;

            // Show Print Button
            PrintBallot.Visibility = Visibility.Visible;
            OptOutButton.Visibility = Visibility.Collapsed;

            // Hide error message
            PrinterErrorPanel.Visibility = Visibility.Collapsed;
        }

        private void ReadyToPrintNo_Click(object sender, RoutedEventArgs e)
        {
            // Turn on the No button
            ReadyToPrintNo.IsChecked = true;
            ready_print_fa_check_no.Visibility = Visibility.Visible;

            // Turn off the Yes button
            ReadyToPrintYes.IsChecked = false;
            ready_print_fa_check_yes.Visibility = Visibility.Collapsed;

            // Show return to search button
            OptOutButton.Visibility = Visibility.Visible;
            PrintBallot.Visibility = Visibility.Collapsed;
        }
    }
}
