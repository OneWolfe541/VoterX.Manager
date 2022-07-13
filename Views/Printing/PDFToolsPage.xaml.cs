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
using Pdftools.Pdf;
using Pdftools.Render;
using Pdftools.PdfPrint;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for PDFToolsPage.xaml
    /// </summary>
    public partial class PDFToolsPage : Page
    {
        public PDFToolsPage()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // http://www.pdf-tools.com/pdf20/en/products/pdf-rendering-desktop-tools/pdf-printer/
        private void PDFPrint_Click(object sender, RoutedEventArgs e)
        {
            using (Printer printer = new Printer())
            {
                if (!printer.OpenPrinter("RICOH AFICIO SP 6330N PCL 6"))
                {
                    GlobalReferences.StatusBar.TextLeft = ("Printer Not Found");
                }
                if (!printer.Open(@"C:\\Temp\\VoterX\\UNFinalPDF.pdf", String.Empty))
                {
                    GlobalReferences.StatusBar.TextLeft = ("File Not Found");
                }
                printer.Orientation = PDFPrintOrientation.ePrintOrientationDefault;
                printer.BeginDocument("My Print Job");
                printer.DefaultSource = 1;
                printer.PrintPage(1);
                printer.EndDocument();         
            }
        }
    }
}
