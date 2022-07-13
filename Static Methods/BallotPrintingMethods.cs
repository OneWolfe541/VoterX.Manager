using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Core.Voters;
using VoterX.SystemSettings;
using VoterX.SystemSettings.Extensions;
//using VoterX.Core.Models.ViewModels;

namespace VoterX.Methods
{
    public static class BallotPrinting
    {
        //public static string PrintBallotBundle(VoterDataModel voter)
        //{
        //    string message = "";

        //    //// Get ballot printer
        //    //message = AppSettings.Printers.BallotPrinter;
        //    //// Get ballot bin
        //    //message = AppSettings.Printers.BallotBin.ToString();
        //    //// Get ballot paper size
        //    //message = AppSettings.Printers.BallotSize.ToString();
        //    //// Get ballot style from voter data
        //    //message = voter.BallotStyleFile;

        //    // Get ballot file name from tblBallotstlesMaster
        //    // Get ballot folder from settings file
        //    string ballotPath = (AppSettings.Ballots.BallotFolder + "\\" + voter.BallotStyleFile);

        //    //string ballotPath = @"c:\Temp\VoterX\6267_OFF-BOD.pdf";

        //    //StatusBar.ApplicationStatusRight(ballotPath);

        //    // Print Official Ballot
        //    message = PDFToolsMethods.PrintPDF(
        //            AppSettings.Printers.BallotPrinter,     // Printer Name
        //            ballotPath,                             // Ballot PDF File
        //            "Print Official Ballot",                // Job Name
        //            AppSettings.Printers.BallotBin,         // Ballot Paper Tray
        //            (short)AppSettings.Printers.BallotSize, // Ballot Paper Size
        //            1,                                       // PDF Page Number
        //            AppSettings.Ballots.Duplex,
        //            AppSettings.System.PDFTools 
        //            );

        //    // Date based function
        //    //if (AppSettings.Election.IsElectionDay())
        //    // System Mode based function
        //    if(AppSettings.System.VCCType == 2)
        //    {
        //        if (AppSettings.System.Permit == 1)
        //        {
        //            //string permitPath = AppSettings.ReportConfigs.ReportsFolder + "\\UNFinalPDF.pdf";

        //            // If Election day mode
        //            // Print Spoiled Permit
        //            //PDFToolsMethods.PrintPDF(
        //            //        AppSettings.Printers.ApplicationPrinter,    // Printer Name
        //            //        permitPath,                                 // Report PDF File
        //            //        "Print Voting Permit",                      // Job Name
        //            //        AppSettings.Printers.AppBin,                // Report Paper Tray
        //            //        (short)AppSettings.Printers.AppSize,        // Report Paper Size
        //            //        null                                        // PDF Page Number
        //            //        );

        //            message = ReportPrintingMethods.PrintVoterPermit(voter);
        //        }
        //    }
        //    else
        //    {
        //        //string applicationPath = AppSettings.ReportConfigs.ReportsFolder + "\\UNFinalPDF.pdf";

        //        // If Early voting mode
        //        // Print Application
        //        //PDFToolsMethods.PrintPDF(
        //        //        AppSettings.Printers.ApplicationPrinter,    // Printer Name
        //        //        applicationPath,                            // Application PDF File
        //        //        "Print Voting Application",                 // Job Name
        //        //        AppSettings.Printers.AppBin,                // Application Paper Tray
        //        //        (short)AppSettings.Printers.AppSize,        // Application Paper Size
        //        //        null                                        // PDF Page Number
        //        //        );
        //        message = ReportPrintingMethods.PrintVoterApplication(voter);
        //    }

        //    if (AppSettings.System.BallotStub == 1)
        //    {
        //        //string stubPath = AppSettings.ReportConfigs.ReportsFolder + "\\UNFinalPDF.pdf";

        //        // If stub is turned on
        //        // Print Ballot Stub
        //        //PDFToolsMethods.PrintPDF(
        //        //        AppSettings.Printers.ApplicationPrinter,    // Printer Name
        //        //        stubPath,                                   // Stub PDF File
        //        //        "Print Ballot Stub",                        // Job Name
        //        //        AppSettings.Printers.AppBin,                // Application Paper Tray
        //        //        (short)AppSettings.Printers.AppSize,        // Application Paper Size
        //        //        null                                        // PDF Page Number
        //        //        );
        //        message = ReportPrintingMethods.PrintBallotStub(voter);
        //    }

        //        return message;
        //}

        public static string PrintBallotBatch(string BallotStyleFile, int NumberOfCopies)
        {
            string message = "";

            string ballotPath = AppSettings.Ballots.BallotFolder + "\\" + BallotStyleFile;

            //for (int i = 0; i < NumberOfCopties; i++)
            //{
                // Print Official Ballot
            message = PDFToolsMethods.PrintPDF(
                    AppSettings.Printers.BallotPrinter,     // Printer Name
                    ballotPath,                             // Ballot PDF File
                    "Print Official Ballot",                // Job Name
                    AppSettings.Printers.BallotBin,         // Ballot Paper Tray
                    (short)AppSettings.Printers.BallotSize, // Ballot Paper Size
                    1,                                       // PDF Page Number
                    (short)NumberOfCopies,
                    AppSettings.Ballots.Duplex,
                    AppSettings.System.PDFTools
                    );
            //}

            return message;
        }

        public static string PrintBallotBatch(string BallotStyleFile)
        {
            string message = "";

            string ballotPath = AppSettings.Ballots.BallotFolder + "\\" + BallotStyleFile;

            //for (int i = 0; i < NumberOfCopties; i++)
            //{
            // Print Official Ballot
            message = PDFToolsMethods.PrintPDF(
                    AppSettings.Printers.BallotPrinter,     // Printer Name
                    ballotPath,                             // Ballot PDF File
                    "Print Official Ballot",                // Job Name
                    AppSettings.Printers.BallotBin,         // Ballot Paper Tray
                    (short)AppSettings.Printers.BallotSize, // Ballot Paper Size
                    1,                                       // PDF Page Number
                    AppSettings.Ballots.Duplex,
                    AppSettings.System.PDFTools
                    );
            //}

            return message;
        }

        public static string ReprintBallot(VoterDataModel voter)
        {
            string message = "";

            //// Get ballot printer
            //message = AppSettings.Printers.BallotPrinter;
            //// Get ballot bin
            //message = AppSettings.Printers.BallotBin.ToString();
            //// Get ballot paper size
            //message = AppSettings.Printers.BallotSize.ToString();
            //// Get ballot style from voter data
            //message = voter.BallotStyleFile;

            string ballotPath = AppSettings.Ballots.BallotFolder + "\\" + voter.BallotStyleFile;

            //StatusBar.ApplicationStatusRight(message);

            // Print Official Ballot
            message = PDFToolsMethods.PrintPDF(
                    AppSettings.Printers.BallotPrinter,     // Printer Name
                    ballotPath,                             // Ballot PDF File
                    "Print Official Ballot",                // Job Name
                    AppSettings.Printers.BallotBin,         // Ballot Paper Tray
                    (short)AppSettings.Printers.BallotSize, // Ballot Paper Size
                    1,                                       // PDF Page Number
                    AppSettings.Ballots.Duplex,
                    AppSettings.System.PDFTools
                    );

            //if (AppSettings.Election.IsElectionDay())
            //if (AppSettings.System.VCCType == 2)
            //{
            //    if (AppSettings.System.Permit == 1)
            //    {
            //        string permitPath = AppSettings.ReportConfigs.ReportsFolder + "\\UNFinalPDF.pdf";

            //        // If Election day mode
            //        // Print Spoiled Permit
            //        PDFToolsMethods.PrintPDF(
            //                AppSettings.Printers.ApplicationPrinter,     // Printer Name
            //                permitPath,                             // Report PDF File
            //                "Print Voting Permit",                  // Job Name
            //                AppSettings.Printers.AppBin,         // Report Paper Tray
            //                (short)AppSettings.Printers.AppSize, // Report Paper Size
            //                null,                                    // PDF Page Number
            //                AppSettings.Ballots.Duplex,
            //                AppSettings.System.PDFTools
            //                );
            //    }
            //}

            return message;
        }

        //public static string PrintAbsenteeApplication(VoterDataModel voter)
        //{
        //    string message = "";

        //    //// Get ballot printer
        //    //message = AppSettings.Printers.BallotPrinter;
        //    //// Get ballot bin
        //    //message = AppSettings.Printers.BallotBin.ToString();
        //    //// Get ballot paper size
        //    //message = AppSettings.Printers.BallotSize.ToString();
        //    //// Get ballot style from voter data
        //    //message = voter.BallotStyleFile;

        //    //string applicationPath = AppSettings.ReportConfigs.ReportsFolder + "\\UNFinalPDF.pdf";

        //    // If Early voting mode
        //    // Print Application
        //    //PDFToolsMethods.PrintPDF(
        //    //        AppSettings.Printers.ApplicationPrinter,    // Printer Name
        //    //        applicationPath,                            // Application PDF File
        //    //        "Print Voting Application",                 // Job Name
        //    //        AppSettings.Printers.AppBin,                // Application Paper Tray
        //    //        (short)AppSettings.Printers.AppSize,        // Application Paper Size
        //    //        null                                        // PDF Page Number
        //    //        );
        //    message = ReportPrintingMethods.PrintVoterAbsenteeApplication(voter);

        //    return message;
        //}

        public static string PrintAbsenteeApplicationPre(VoterDataModel voter)
        {
            string message = "";
            message = ReportPrintingMethods.PrintVoterAbsenteeApplicationPre(voter);

            return message;
        }

        //public static string PrintEarlyVotingApplicationPre(VoterDataModel voter)
        //{
        //    string message = "";
        //    message = ReportPrintingMethods.PrintVoterEarlyVotingApplicationPre(voter);

        //    return message;
        //}

        //public static string ReprintApplication(VoterDataModel voter)
        //{
        //    string message = "";

        //    //// Get ballot printer
        //    //message = AppSettings.Printers.BallotPrinter;
        //    //// Get ballot bin
        //    //message = AppSettings.Printers.BallotBin.ToString();
        //    //// Get ballot paper size
        //    //message = AppSettings.Printers.BallotSize.ToString();
        //    //// Get ballot style from voter data
        //    //message = voter.BallotStyleFile;

        //    //string applicationPath = AppSettings.ReportConfigs.ReportsFolder + "\\UNFinalPDF.pdf";

        //    // If Early voting mode
        //    // Print Application
        //    //PDFToolsMethods.PrintPDF(
        //    //        AppSettings.Printers.ApplicationPrinter,    // Printer Name
        //    //        applicationPath,                            // Application PDF File
        //    //        "Print Voting Application",                 // Job Name
        //    //        AppSettings.Printers.AppBin,                // Application Paper Tray
        //    //        (short)AppSettings.Printers.AppSize,        // Application Paper Size
        //    //        null                                        // PDF Page Number
        //    //        );
        //    message = ReportPrintingMethods.PrintVoterApplication(voter);

        //    return message;
        //}

        //public static string ReprintPermit(VoterDataModel voter)
        //{
        //    string message = "";

        //    //// Get ballot printer
        //    //message = AppSettings.Printers.BallotPrinter;
        //    //// Get ballot bin
        //    //message = AppSettings.Printers.BallotBin.ToString();
        //    //// Get ballot paper size
        //    //message = AppSettings.Printers.BallotSize.ToString();
        //    //// Get ballot style from voter data
        //    //message = voter.BallotStyleFile;

        //    //string permitPath = AppSettings.ReportConfigs.ReportsFolder + "\\UNFinalPDF.pdf";

        //    // If Early voting mode
        //    // Print Application
        //    //PDFToolsMethods.PrintPDF(
        //    //        AppSettings.Printers.ApplicationPrinter,    // Printer Name
        //    //        permitPath,                            // Application PDF File
        //    //        "Print Voting Permit",                 // Job Name
        //    //        AppSettings.Printers.AppBin,                // Application Paper Tray
        //    //        (short)AppSettings.Printers.AppSize,        // Application Paper Size
        //    //        null                                        // PDF Page Number
        //    //        );

        //    message = ReportPrintingMethods.PrintVoterPermit(voter);

        //    return message;
        //}

        public static string ReprintStub(VoterDataModel voter)
        {
            string message = "";

            //// Get ballot printer
            //message = AppSettings.Printers.BallotPrinter;
            //// Get ballot bin
            //message = AppSettings.Printers.BallotBin.ToString();
            //// Get ballot paper size
            //message = AppSettings.Printers.BallotSize.ToString();
            //// Get ballot style from voter data
            //message = voter.BallotStyleFile;

            //string stubPath = AppSettings.ReportConfigs.ReportsFolder + "\\UNFinalPDF.pdf";

            // If Early voting mode
            // Print Application
            //PDFToolsMethods.PrintPDF(
            //        AppSettings.Printers.ApplicationPrinter,    // Printer Name
            //        stubPath,                                   // Application PDF File
            //        "Print Ballot Stub",                        // Job Name
            //        AppSettings.Printers.AppBin,                // Application Paper Tray
            //        (short)AppSettings.Printers.AppSize,        // Application Paper Size
            //        null                                        // PDF Page Number
            //        );

            message = ReportPrintingMethods.PrintBallotStub(voter);

            return message;
        }

        public static string PrintProvisionalBallot(VoterDataModel voter)
        {
            string message = "";

            //// Get ballot printer
            //message = AppSettings.Printers.BallotPrinter;
            //// Get ballot bin
            //message = AppSettings.Printers.BallotBin.ToString();
            //// Get ballot paper size
            //message = AppSettings.Printers.BallotSize.ToString();
            //// Get ballot style from voter data
            //message = voter.BallotStyleFile;

            string provisionalPath = AppSettings.Ballots.ProvisionalFolder + "\\" + AppSettings.Ballots.ProvisionalPrefix + voter.BallotStyleFile;

            //StatusBar.ApplicationStatusRight(message);

            // Print Unofficial Ballot
            message = PDFToolsMethods.PrintPDF(
                    AppSettings.Printers.BallotPrinter,     // Printer Name
                    provisionalPath,                        // Ballot PDF File
                    "Print Provisional Ballot",             // Job Name
                    AppSettings.Printers.BallotBin,         // Ballot Paper Tray
                    (short)AppSettings.Printers.BallotSize, // Ballot Paper Size
                    1,                                       // PDF Page Number
                    AppSettings.Ballots.Duplex,
                    AppSettings.System.PDFTools
                    );

            return message;
        }

        public static string PrintTestBallot()
        {
            string message = "";

            //// Get ballot printer
            //message = AppSettings.Printers.BallotPrinter;
            //// Get ballot bin
            //message = AppSettings.Printers.BallotBin.ToString();
            //// Get ballot paper size
            //message = AppSettings.Printers.BallotSize.ToString();
            //// Get ballot style from voter data
            //message = voter.BallotStyleFile;

            string provisionalPath = AppSettings.Ballots.BallotFolder + "\\TEST_BALLOT.pdf" ;

            //StatusBar.ApplicationStatusRight(message);

            // Print Unofficial Ballot
            message = PDFToolsMethods.PrintPDF(
                    AppSettings.Printers.BallotPrinter,     // Printer Name
                    provisionalPath,                        // Ballot PDF File
                    "Print Test Ballot",             // Job Name
                    AppSettings.Printers.BallotBin,         // Ballot Paper Tray
                    (short)AppSettings.Printers.BallotSize, // Ballot Paper Size
                    1,                                       // PDF Page Number
                    AppSettings.Ballots.Duplex,
                    AppSettings.System.PDFTools
                    );

            return message;
        }

        //public static string PrintSignatureForm(VoterDataModel voter)
        //{
        //    string message = "";

        //    //string sigFormPath = AppSettings.ReportConfigs.ReportsFolder + "\\UNFinalPDF.pdf";

        //    // If Early voting mode
        //    // Print Application
        //    //PDFToolsMethods.PrintPDF(
        //    //        AppSettings.Printers.ApplicationPrinter,    // Printer Name
        //    //        sigFormPath,                                // Application PDF File
        //    //        "Print Signature Form",                     // Job Name
        //    //        AppSettings.Printers.AppBin,                // Application Paper Tray
        //    //        (short)AppSettings.Printers.AppSize,        // Application Paper Size
        //    //        null                                        // PDF Page Number
        //    //        );

        //    message = ReportPrintingMethods.PrintSignatureForm(voter);

        //    return message;
        //}

        public static string PrintBallotCopies(string BallotStyleFile, int copies)
        {
            string message = "";

            string ballotPath = (AppSettings.Ballots.BallotFolder + "\\" + BallotStyleFile);

            // Print Official Ballot
            message = PDFToolsMethods.PrintPDF(
                    AppSettings.Printers.BallotPrinter,         // Printer Name
                    ballotPath,                                 // Ballot PDF File
                    "Print Emergency Ballot ",                  // Job Name
                    AppSettings.Printers.BallotBin,             // Ballot Paper Tray
                    (short)AppSettings.Printers.BallotSize,     // Ballot Paper Size
                    1,                                          // PDF Page Number
                    (short)copies,
                    AppSettings.Ballots.Duplex,
                    AppSettings.System.PDFTools
                    );

            return message;
        }

        public static string PrintEmergencyBallot(SystemSettingsController settings, string ballotstylefile)
        {
            string message = "";

            string ballotPath = settings.Ballots.BallotFolder + "\\" + ballotstylefile;

            // Print Unofficial Ballot
            message = PDFToolsMethods.PrintPDF(
                    settings.Printers.BallotPrinter,            // Printer Name
                    ballotPath,                                 // Ballot PDF File
                    "Print Test Ballot",                        // Job Name
                    settings.Printers.BallotBin,                // Ballot Paper Tray
                    (short)settings.Printers.BallotSize,        // Ballot Paper Size
                    1,                                          // PDF Page Number
                    settings.Ballots.Duplex,
                    settings.System.PDFTools
                    );

            return message;
        }

        public static string PrintSampleCopies(string BallotStyleFile, int copies)
        {
            string message = "";

            string ballotPath = (AppSettings.Ballots.SampleFolder + "\\" + BallotStyleFile);

            // Print Official Ballot
            message = PDFToolsMethods.PrintPDF(
                    AppSettings.Printers.BallotPrinter,         // Printer Name
                    ballotPath,                                 // Ballot PDF File
                    "Print Emergency Ballot ",                  // Job Name
                    AppSettings.Printers.BallotBin,             // Ballot Paper Tray
                    (short)AppSettings.Printers.BallotSize,     // Ballot Paper Size
                    1,                                          // PDF Page Number
                    (short)copies,
                    AppSettings.Ballots.Duplex,
                    AppSettings.System.PDFTools
                    );

            return message;
        }

        public static string PrintSampleBallot(string BallotStyleFile)
        {
            string message = "";

            string ballotPath = AppSettings.Ballots.BallotFolder + "\\" + BallotStyleFile;

            // Print Official Ballot
            message = PDFToolsMethods.PrintPDF(
                    AppSettings.Printers.BallotPrinter,     // Printer Name
                    ballotPath,                             // Ballot PDF File
                    "Print Official Ballot",                // Job Name
                    AppSettings.Printers.BallotBin,         // Ballot Paper Tray
                    (short)AppSettings.Printers.BallotSize, // Ballot Paper Size
                    1,                                       // PDF Page Number
                    AppSettings.Ballots.Duplex,
                    AppSettings.System.PDFTools
                    );

            return message;
        }
    }
}
