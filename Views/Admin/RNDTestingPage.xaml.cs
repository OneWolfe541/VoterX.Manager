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
using VoterX.Extensions;

using Pdftools.Pdf;
using Pdftools.Render;
using Pdftools.PdfPrint;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for RNDTestingPage.xaml
    /// </summary>
    public partial class RNDTestingPage : Page
    {
        public RNDTestingPage()
        {
            InitializeComponent();

            // Display page header
            GlobalReferences.Header.PageHeader = ("YOU SHOULD NOT BE HERE");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new LoginPage());
        }

        private void BallotPrintQuestion_AnswerClick(object sender, RoutedEventArgs e)
        {
            GlobalReferences.StatusBar.TextLeft = ("User Control Clicked By: " + BallotPrintQuestion.GetAnswer().ToString());
        }

        private void PrintTest_Click(object sender, RoutedEventArgs e)
        {
            string message = PrintPDF(
                    AppSettings.Printers.BallotPrinter,     // Printer Name
                    @"C:\Temp\VoterX\6267_OFF-BOD.pdf",                             // Ballot PDF File
                    "Print Official Ballot",                // Job Name
                    AppSettings.Printers.BallotBin,         // Ballot Paper Tray
                    (short)AppSettings.Printers.BallotSize, // Ballot Paper Size
                    1                                       // PDF Page Number
                    );
        }

        // http://www.pdf-tools.com/pdf20/en/products/pdf-rendering-desktop-tools/pdf-printer/
        public static string PrintPDF(string PrinterName, string FileName, string JobName, int Tray, short? PaperSize, int? PageNumber)
        {
            string message = "";
            using (Printer printer = new Printer())
            {
                if (!printer.OpenPrinter(PrinterName))
                {
                    message += "Printer Not Found ";
                }
                else message += "Printer Found";
                if (!printer.Open(FileName, String.Empty))
                {
                    message += "File Not Found " + FileName;
                }
                else message += "File Found";
                try
                {
                    printer.Orientation = PDFPrintOrientation.ePrintOrientationDefault;
                    printer.BeginDocument(JobName);
                    printer.DefaultSource = Tray;
                    if (PaperSize != null) printer.PaperSize = (short)PaperSize;
                    printer.PrintPage(PageNumber == null ? 1 : (int)PageNumber);
                    printer.EndDocument();
                }
                catch (Exception e)
                {
                    message = e.ToString();
                }

                //printer.GetBin                
            }
            return message;
        }
    }
}
