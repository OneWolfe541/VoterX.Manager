using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Methods;
using System.Data;
using System.Reflection;
//using VoterX.Core.Models.Database;
//using VoterX.Core.Models.ViewModels;
using System.Collections;

namespace VoterX.Extensions
{
    static class ExcelExtensions
    {
        //My Extension Class:

        public static void ExportToExcel<IEnumType>(this IEnumerable<IEnumType> ie)
        {
            if (ie.Count() > 0)
            {
                using (cExportToExcel cexp = new cExportToExcel())
                {
                    DataTable dt = new DataTable();
                    ie.First(ie2 => true).GetType().GetProperties().ToList().ForEach(pr => dt.Columns.Add(pr.Name, typeof(string)));
                    ie.ToList().ForEach(ie2 =>
                    {
                        List<object> objAdd = new List<object>();
                        ie2.GetType().GetProperties().ToList().ForEach(pr => objAdd.Add(ie2.GetType().InvokeMember(pr.Name, BindingFlags.GetProperty, null, ie2, null)));
                        dt.Rows.Add(objAdd.ToArray());
                        objAdd.Clear();
                        objAdd = null;
                    });
                    cexp.ExportDataTable(ref dt);
                    dt.Dispose();
                }
            }
        }

        // https://social.msdn.microsoft.com/Forums/en-US/69869649-a238-4af9-8059-55499b50dd57/export-linq-query-to-excel-latebind?forum=whatforum
        public static void ExportToExcel<IEnumType>(this IEnumerable<IEnumType> ie, string path)
        {
            if (ie.Count() > 0)
            {
                using (cExportToExcel cexp = new cExportToExcel())
                {
                    DataTable dt = new DataTable();
                    ie.First(ie2 => true).GetType().GetProperties().ToList().ForEach(pr => dt.Columns.Add(pr.Name, typeof(string)));
                    ie.ToList().ForEach(ie2 =>
                    {
                        List<object> objAdd = new List<object>();
                        ie2.GetType().GetProperties().ToList().ForEach(pr => objAdd.Add(ie2.GetType().InvokeMember(pr.Name, BindingFlags.GetProperty, null, ie2, null)));
                        dt.Rows.Add(objAdd.ToArray());
                        objAdd.Clear();
                        objAdd = null;
                    });
                    cexp.ExportDataTable(ref dt, path);
                    dt.Dispose();
                }
            }
        }

        //public static void ExportToExcel(this IEnumerable<VoterReportModel> ie)
        //{
        //    if (ie.Count() > 0)
        //    {
        //        using (cExportToExcel cexp = new cExportToExcel())
        //        {
        //            DataTable dt = new DataTable();
        //            ie.First(ie2 => true).GetType().GetProperties().ToList().ForEach(pr => dt.Columns.Add(pr.Name, typeof(string)));
        //            ie.ToList().ForEach(ie2 =>
        //            {
        //                List<object> objAdd = new List<object>();
        //                ie2.GetType().GetProperties().ToList().ForEach(pr => objAdd.Add(ie2.GetType().InvokeMember(pr.Name, BindingFlags.GetProperty, null, ie2, null)));
        //                dt.Rows.Add(objAdd.ToArray());
        //                objAdd.Clear();
        //                objAdd = null;
        //            });
        //            cexp.ExportDataTable(ref dt);
        //            dt.Dispose();
        //        }
        //    }
        //}

        // https://stackoverflow.com/questions/3199604/is-there-a-quick-way-to-convert-an-entity-to-csv-file
        // http://wiki.lessthandot.com/index.php/Objects_to_CSV
        /// <summary>
        /// Return a CSV file in a string.
        /// </summary>
        /// <param name="collection">IList.</param>
        /// <returns>string.</returns>
        public static string CollectionToCsv(IList collection)
        {

            if (collection == null) throw new ArgumentNullException("collection", "Value can not be null or nothing!");

            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < collection.Count; index++)
            {
                object item = collection[index];

                if (index == 0)
                {
                    sb.Append(ObjectToCsvHeader(item));
                }
                sb.Append(ObjectToCsvData(item));
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates a comma delimeted string of all the objects property values names.
        /// </summary>
        /// <param name="obj">object.</param>
        /// <returns>string.</returns>
        public static string ObjectToCsvData(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj", "Value can not be null or Nothing!");
            }

            StringBuilder sb = new StringBuilder();
            Type t = obj.GetType();
            PropertyInfo[] pi = t.GetProperties();

            for (int index = 0; index < pi.Length; index++)
            {
                //var test = pi[index].GetValue(obj, null);
                if (NotDateOrNumber(pi[index].GetValue(obj, null))) sb.Append("\"");

                sb.Append(RemoveCommas(pi[index].GetValue(obj, null)));

                if (NotDateOrNumber(pi[index].GetValue(obj, null))) sb.Append("\"");

                if (index < pi.Length - 1)
                {
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates a comma delimeted string of all the objects property names.
        /// </summary>
        /// <param name="obj">object.</param>
        /// <returns>string.</returns>
        public static string ObjectToCsvHeader(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj", "Value can not be null or Nothing!");
            }

            StringBuilder sb = new StringBuilder();
            Type t = obj.GetType();
            PropertyInfo[] pi = t.GetProperties();

            for (int index = 0; index < pi.Length; index++)
            {
                sb.Append(pi[index].Name);

                if (index < pi.Length - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        public static void ExportToCSV<IEnumType>(this IEnumerable<IEnumType> ie)
        {
            //var header = ObjectToCsvHeader(ie.ToList());
            var output = CollectionToCsv(ie.ToList());

            System.IO.File.WriteAllText(@"C:\VoterX\Export\export.csv", output);
        }

        public static void ExportToCSV<IEnumType>(this IEnumerable<IEnumType> ie, string path)
        {
            //var header = ObjectToCsvHeader(ie.ToList());
            var output = CollectionToCsv(ie.ToList());            

            System.IO.File.WriteAllText(path, output);
        }

        public static bool NotDateOrNumber(object obj)
        {
            bool result = true;

            if (obj != null)
            {
                DateTime dateValue;

                int intValue;

                if (DateTime.TryParse(obj.ToString(), out dateValue)) return false;

                if (Int32.TryParse(obj.ToString(), out intValue)) return false;
            }

            return result;
        }

        public static string RemoveCommas(object obj)
        {
            if (obj != null)
            {
                return obj.ToString().Replace(",", "");
            }
            else
                return null;
        }
    }
}
