using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace VoterX.Static_Methods
{
    public static class EPPlusMethods
    {
        public static void CreateExcelFile()
        {
            using (var package = new ExcelPackage())
            {
                // Add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Inventory");
                //Add the headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Product";
                worksheet.Cells[1, 3].Value = "Quantity";
                worksheet.Cells[1, 4].Value = "Price";
                worksheet.Cells[1, 5].Value = "Value";

                //Add some items...
                worksheet.Cells["A2"].Value = 12001;
                worksheet.Cells["B2"].Value = "Nails";
                worksheet.Cells["C2"].Value = 37;
                worksheet.Cells["D2"].Value = 3.99;

                worksheet.Cells["A3"].Value = 12002;
                worksheet.Cells["B3"].Value = "Hammer";
                worksheet.Cells["C3"].Value = 5;
                worksheet.Cells["D3"].Value = 12.10;

                worksheet.Cells["A4"].Value = 12003;
                worksheet.Cells["B4"].Value = "Saw";
                worksheet.Cells["C4"].Value = 12;
                worksheet.Cells["D4"].Value = 15.37;

                //var xlFile = GetFileInfo("sample1.xlsx");
                var xlFile = new FileInfo("C:\\VoterX\\Export\\sample1.xlsx");
                // save our new workbook in the output directory and we are done!
                package.SaveAs(xlFile);
            }
        }
    
    }
}
