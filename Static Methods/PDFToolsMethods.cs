using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pdftools.Pdf;
using Pdftools.Render;
using Pdftools.PdfPrint;

namespace VoterX.Methods
{
    public static class PDFToolsMethods
    {
        // http://www.pdf-tools.com/pdf20/en/products/pdf-rendering-desktop-tools/pdf-printer/
        public static string PrintPDF(string PrinterName, string FileName, string JobName, int Tray, short? PaperSize, int? PageNumber, bool duplex, string LicenseKey)
        {
            string message = "";

            // Check for active license and return if license cannot be activated
            if (Pdftools.PdfPrint.Printer.LicenseIsValid == false)
            {
                if (Pdftools.PdfPrint.Printer.SetLicenseKey(LicenseKey) == false)
                {
                    return "PDF Tools License is invalid";
                }
            }

            using (Printer printer = new Printer())
            {               

                if (!printer.OpenPrinter(PrinterName))
                {
                    message += "Printer Not Found ";
                }
                //else message += "Printer Found";
                if (!printer.Open(FileName, String.Empty))
                {
                    message += "File Not Found " + FileName;
                }
                //else message += "File Found";
                try
                {
                    printer.Orientation = PDFPrintOrientation.ePrintOrientationDefault;
                    printer.BeginDocument(JobName);
                    printer.DefaultSource = Tray;
                    if (PaperSize != null) printer.PaperSize = (short)PaperSize;
                    if (duplex == true)
                    {
                        printer.Duplex = PDFDuplexOption.eDuplexVertical;
                        if (PageNumber == null) PageNumber = 1;
                        printer.PrintFile(FileName, PrinterName, null, (int)PageNumber, (int)PageNumber + 1);
                    }
                    else
                    {
                        printer.Duplex = PDFDuplexOption.eDuplexSimplex;
                        printer.PrintPage(PageNumber == null ? 1 : (int)PageNumber);
                    }
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

        // Print copies
        public static string PrintPDF(string PrinterName, string FileName, string JobName, int Tray, short? PaperSize, int? PageNumber, short NumberOfCopies, bool duplex, string LicenseKey)
        {
            string message = "";

            // Check for active license and return if license cannot be activated
            if (Pdftools.PdfPrint.Printer.LicenseIsValid == false)
            {
                if (Pdftools.PdfPrint.Printer.SetLicenseKey(LicenseKey) == false)
                {
                    return "PDF Tools License is invalid";
                }
            }

            using (Printer printer = new Printer())
            {
                if (!printer.OpenPrinter(PrinterName))
                {
                    message += "Printer Not Found ";
                }
                //else message += "Printer Found";
                if (!printer.Open(FileName, String.Empty))
                {
                    message += "File Not Found " + FileName;
                }
                //else message += "File Found";
                try
                {
                    printer.Orientation = PDFPrintOrientation.ePrintOrientationDefault;
                    printer.BeginDocument(JobName);
                    printer.DefaultSource = Tray;
                    if (PaperSize != null) printer.PaperSize = (short)PaperSize;
                    printer.CopyMode = PDFCopyMode.eCopyModePage;
                    printer.Copies = NumberOfCopies;
                    //printer.PrintFile(FileName, PrinterName, null, 1, 1);
                    if (duplex == true)
                    {
                        printer.Duplex = PDFDuplexOption.eDuplexVertical;
                        if (PageNumber == null) PageNumber = 1;
                        printer.PrintFile(FileName, PrinterName, null, (int)PageNumber, (int)PageNumber + 1);
                    }
                    else
                    {
                        printer.Duplex = PDFDuplexOption.eDuplexSimplex;
                        //printer.PrintPage(PageNumber == null ? 1 : (int)PageNumber);
                        printer.PrintFile(FileName, PrinterName, null, 1, 1);
                    }
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

        public static bool SetLicenseKey(string LicenseKey)
        {
            if (Pdftools.PdfPrint.Printer.LicenseIsValid == false)
            {
                return Pdftools.PdfPrint.Printer.SetLicenseKey(LicenseKey);
            }
            else
            {
                return true;
            }

        }
    }
}
