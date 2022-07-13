using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoterX.ABS.Logging
{
    public class VoterXLogger
    {
        public string FileName { get; set; }

        public bool Logging { get; set; }

        public VoterXLogger()
        {
            Logging = false;
        }

        public VoterXLogger(string File, bool EnableLogging)
        {
            FileName = File;
            Logging = EnableLogging;
        }

        public void WriteLog(string Message)
        {
            if (Logging == true)
            {
                var result = WriteLogToFile(FileName, Message);
            }
        }

        private bool WriteLogToFile(string FileName, string Message)
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
