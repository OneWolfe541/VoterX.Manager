using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoterX.Methods
{
    public static class LogFileMethods
    {
        public static bool WriteLogToFile(string FileName, string Message)
        {
            bool result = false;
            try
            {
                string subPath = "C:\\VoterX\\Debug\\";

                System.IO.Directory.CreateDirectory(subPath);

                File.AppendAllText(subPath + FileName + ".log", DateTime.Now.ToString() + " " + Message + "\r\n");

                result = true;
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
            return result;
        }
    }
}
