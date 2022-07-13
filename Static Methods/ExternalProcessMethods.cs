using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Methods;
using VoterX.Manager.Global;

namespace VoterX.Methods
{
    public static class ExternalProcessMethods
    {
        private static void OpenExternalApp(string filePath)
        {
            if (File.Exists(filePath))
            {
                Process.Start(filePath);
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = "File Not Found";
            }
        }

        public static void OpenReconcileApp()
        {
            string filePath = "C:\\VoterXSync\\AVReconcile.hta";
            OpenExternalApp(filePath);
        }

        public static void OpenHelpDeskApp()
        {
            string filePath = "C:\\VoterXSync\\AVHelpdesk.hta";
            OpenExternalApp(filePath);
        }
    }
}
