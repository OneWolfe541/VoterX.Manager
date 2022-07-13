using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data.Objects;
using System.Data.EntityClient;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
//using VoterX.Core.Models.ViewModels;
using VoterX.Extensions;
//using VoterX.Core.Models.Database;
using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace VoterX.Methods
{
    public static class ExcelMethods
    {
        // https://www.codeproject.com/Tips/676675/Entity-Model-to-Excel-using-LINQ-in-Csharp
        public static void EntityToExcelSheet(string excelFilePath,
       string sheetName, IQueryable result, ObjectContext ctx)
        {
            Excel.Application oXL;
            Excel.Workbook oWB;
            Excel.Worksheet oSheet;
            Excel.Range oRange;
            try
            {
                // Start Excel and get Application object.
                oXL = new Excel.Application();

                // Set some properties
                oXL.Visible = true;
                oXL.DisplayAlerts = false;

                // Get a new workbook. 
                oWB = oXL.Workbooks.Add(Missing.Value);

                // Get the active sheet 
                oSheet = (Excel.Worksheet)oWB.ActiveSheet;
                oSheet.Name = sheetName;

                // Process the DataTable
                // BE SURE TO CHANGE THIS LINE TO USE *YOUR* DATATABLE 
                DataTable dt = EntityToDataTable(result, ctx);

                int rowCount = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    rowCount += 1;
                    for (int i = 1; i < dt.Columns.Count + 1; i++)
                    {
                        // Add the header the first time through 
                        if (rowCount == 2)
                            oSheet.Cells[1, i] = dt.Columns[i - 1].ColumnName;
                        oSheet.Cells[rowCount, i] = dr[i - 1].ToString();
                    }
                }

                // Resize the columns 
                oRange = oSheet.Range[oSheet.Cells[1, 1], oSheet.Cells[rowCount, dt.Columns.Count]];
                oRange.Columns.AutoFit();

                // Save the sheet and close 
                oSheet = null;
                oRange = null;
                oWB.SaveAs(excelFilePath, Excel.XlFileFormat.xlWorkbookNormal, Missing.Value,
                  Missing.Value, Missing.Value, Missing.Value,
                  Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value,
                  Missing.Value, Missing.Value, Missing.Value);
                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oWB = null;
                oXL.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable EntityToDataTable(IQueryable result, ObjectContext ctx)
        {
            try
            {
                EntityConnection conn = ctx.Connection as EntityConnection;
                using (SqlConnection SQLCon = new SqlConnection(conn.StoreConnection.ConnectionString))
                {
                    ObjectQuery query = result as ObjectQuery;
                    using (SqlCommand Cmd = new SqlCommand(query.ToTraceString(), SQLCon))
                    {
                        foreach (var param in query.Parameters)
                        {
                            Cmd.Parameters.AddWithValue(param.Name, param.Value);
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(Cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                                return dt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static void VoterToExcelSheet(string excelFilePath, string sheetName, List<tblVoter> result)
        //{
        //    Excel.Application oXL;
        //    Excel.Workbook oWB;
        //    Excel.Worksheet oSheet;
        //    Excel.Range oRange;
        //    try
        //    {
        //        // Start Excel and get Application object.
        //        oXL = new Excel.Application();

        //        // Set some properties
        //        oXL.Visible = true;
        //        oXL.DisplayAlerts = false;

        //        // Get a new workbook. 
        //        oWB = oXL.Workbooks.Add(Missing.Value);

        //        // Get the active sheet 
        //        oSheet = (Excel.Worksheet)oWB.ActiveSheet;
        //        oSheet.Name = sheetName;

        //        // Process the DataTable
        //        // BE SURE TO CHANGE THIS LINE TO USE *YOUR* DATATABLE 
        //        DataTable dt = result.ToDataTable<tblVoter>();

        //        int rowCount = 1;
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            rowCount += 1;
        //            for (int i = 1; i < dt.Columns.Count + 1; i++)
        //            {
        //                // Add the header the first time through 
        //                if (rowCount == 2)
        //                    oSheet.Cells[1, i] = dt.Columns[i - 1].ColumnName;
        //                oSheet.Cells[rowCount, i] = dr[i - 1].ToString();
        //            }
        //        }

        //        // Resize the columns 
        //        oRange = oSheet.Range[oSheet.Cells[1, 1], oSheet.Cells[rowCount, dt.Columns.Count]];
        //        oRange.Columns.AutoFit();

        //        // Save the sheet and close 
        //        oSheet = null;
        //        oRange = null;
        //        oWB.SaveAs(excelFilePath, Excel.XlFileFormat.xlWorkbookNormal, Missing.Value,
        //          Missing.Value, Missing.Value, Missing.Value,
        //          Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value,
        //          Missing.Value, Missing.Value, Missing.Value);
        //        oWB.Close(Missing.Value, Missing.Value, Missing.Value);
        //        oWB = null;
        //        oXL.Quit();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void ExportToExcel(List<tblVoter> result)
        //{
        //    try
        //    {
        //        System.Data.OleDb.OleDbConnection MyConnection;
        //        System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
        //        //string sql = null;
        //        MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='c:\\VoterX\\voters.xls';Extended Properties=Excel 8.0;");
        //        MyConnection.Open();
        //        myCommand.Connection = MyConnection;

        //        myCommand.CommandText = "Insert into [Sheet1$] (id,name) values('@p1', '@p2')";
        //        myCommand.Parameters.Add("@p1", OleDbType.VarChar, 100);
        //        myCommand.Parameters.Add("@p2", OleDbType.VarChar, 100);

        //        // define query to entity data model
        //        //var model = from i in myEntity.Inquiries select i;

        //        foreach (var m in result)
        //        {
        //            myCommand.Parameters["@p1"].Value = m.voter_id;
        //            myCommand.Parameters["@p2"].Value = m.name_first;
        //            // .. Add other parameters here
        //            myCommand.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }

    // https://social.msdn.microsoft.com/Forums/en-US/69869649-a238-4af9-8059-55499b50dd57/export-linq-query-to-excel-latebind?forum=whatforum
    public class cExportToExcel : IDisposable
    {
        private object _objExApp;
        public cExportToExcel()
        {
            Type Excel = Type.GetTypeFromProgID("Excel.Application");
            try
            {
                //try to get active object, will catch exception if none

                _objExApp = Marshal.GetActiveObject("Excel.Application");

                //try to set Application.ScreenUpdating property, test for Application Busy.  Will throw exception of Application is busy, which will instantiate a new instance of the Excel.Application type
                this.ScreenUpdating = false;
                this.ScreenUpdating = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                _objExApp = null;
                _objExApp = Activator.CreateInstance(Excel);
                this.ScreenUpdating = true;
            }
            this.Visible = true;
        }
        private bool DisplayAlerts
        {
            get
            {
                return (bool)_objExApp.GetType().InvokeMember("DisplayAlerts", BindingFlags.GetProperty, null, _objExApp, null);
            }
            set
            {
                _objExApp.GetType().InvokeMember("DisplayAlerts", BindingFlags.SetProperty, null, _objExApp, new object[] { value });
            }
        }
        private object Workbooks
        {
            get
            {
                return _objExApp.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, _objExApp, null);
            }
        }
        public void OpenXML(string strPath)
        {
            object[] args = new object[3];
            args[0] = strPath;
            args[1] = Type.Missing;
            args[2] = 2;
            this.Workbooks.GetType().InvokeMember("OpenXML", BindingFlags.InvokeMethod, null, this.Workbooks, args);
            Array.Clear(args, 0, args.Length);
            args = null;
        }
        public bool Visible
        {
            get
            {
                return (bool)_objExApp.GetType().InvokeMember("Visible", BindingFlags.GetProperty, null, _objExApp, null);
            }
            set
            {
                object[] args = new object[1];
                args[0] = value;
                _objExApp.GetType().InvokeMember("Visible", BindingFlags.SetProperty, null, _objExApp, args);
                args = null;
            }
        }
        public bool ScreenUpdating
        {
            get
            {
                return (bool)_objExApp.GetType().InvokeMember("ScreenUpdating", BindingFlags.GetProperty, null, _objExApp, null);
            }
            set
            {
                object[] args = new object[1];
                args[0] = value;
                _objExApp.GetType().InvokeMember("ScreenUpdating", BindingFlags.SetProperty, null, _objExApp, args);
            }
        }
        public object StatusBar
        {
            get
            {
                return _objExApp.GetType().InvokeMember("StatusBar", BindingFlags.GetProperty, null, _objExApp, null);
            }
            set
            {
                _objExApp.GetType().InvokeMember("StatusBar", BindingFlags.SetProperty, null, _objExApp, new object[] { value });
            }
        }
        public void ExportDataTable(ref System.Data.DataTable dt)
        {
            this.Visible = true;
            this.StatusBar = "My Display Message while running, will show in Excel statusbar";
            //this.ScreenUpdating = false;
            this.DisplayAlerts = false;
            if (string.IsNullOrEmpty(dt.TableName)) dt.TableName = "Name";
            string strPath = string.Format("{0}Export {1}.XML", System.IO.Path.GetTempPath(), DateTime.Now.ToString("yyyyMMddHHmmss"));
            dt.WriteXml(strPath);
            OpenXML(strPath);
            this.DisplayAlerts = true;
            this.StatusBar = false;
            new System.Threading.Tasks.Task(() => new System.IO.FileInfo(strPath).Delete()).Start();
        }

        public void ExportDataTable(ref System.Data.DataTable dt, string strPath)
        {
            this.Visible = true;
            this.StatusBar = "My Display Message while running, will show in Excel statusbar";
            //this.ScreenUpdating = false;
            this.DisplayAlerts = false;
            if (string.IsNullOrEmpty(dt.TableName)) dt.TableName = "Name";
            //string strPath = string.Format("{0}Export {1}.XML", System.IO.Path.GetTempPath(), DateTime.Now.ToString("yyyyMMddHHmmss"));
            dt.WriteXml(strPath);
            OpenXML(strPath);
            this.DisplayAlerts = true;
            this.StatusBar = false;
            new System.Threading.Tasks.Task(() => new System.IO.FileInfo(strPath).Delete()).Start();
        }

        public void Dispose()
        {
            System.Diagnostics.Debug.WriteLine(Marshal.ReleaseComObject(_objExApp)); //should be 0
            _objExApp = null;
        }
    }
    
}
