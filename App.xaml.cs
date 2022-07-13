using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
//using VoterX.Core.Containers;
//using VoterX.Core.Methods;
using VoterX.SystemSettings;
using VoterX.Dialogs;
using VoterX.Core.Voters;
using VoterX.Core.Elections;
using VoterX.Methods;
using VoterX.Utilities.BasePageDefinitions;
using System.Windows.Controls;
using VoterX.Utilities.Views;
using VoterX.Utilities.Controls;
using VoterX.SystemSettings.Extensions;
using System.Diagnostics;
using VoterX.SystemSettings.Models;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Create empty global system settings object
        public SystemSettingsController GlobalSettings = new SystemSettingsController();

        // Create empty Unity container
        //public VoterContainer voterContainer;

        public string Connection;

        // Create empty Voter Factory
        public VoterFactory Voters;

        // Create empty Election Factory
        public NMElection Election;

        public bool debugMode = false;
        public bool absenteeMode = false;

        public bool settingsChanged = false;

        public StatusBarViewModel statusBar;
        public MainHeaderViewModel mainHeader;
        public SliderMenuFrameControl mainSliderMenu = null;

        public Page originpage = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load System Settings File
            try
            {
                //GlobalSettings = new SystemSettingsController(StorageType.Json, ConfigurationManager.AppSettings["AppFolder"], null);

                string server = System.Environment.MachineName;

                var DebugSettings = new DebugSettingModel();
                try
                {
                    DebugSettings = GlobalSettings.LoadDebugFile("C:\\VoterX\\Settings\\");
                }
                catch
                {
                    DebugSettings = new DebugSettingModel();
                }

                //DebugSettings.SettingsType = 0;

                if (DebugSettings.SettingsType == null)
                {
                    string connection = "XXXX";
                    GlobalSettings = new SystemSettingsController(StorageType.Database, ConfigurationManager.AppSettings["AppFolder"], connection, null);
                    Connection = "XXXX";
                    if(GlobalSettings.Absentee.SiteName != null) absenteeMode = true;
                }
                if (DebugSettings.SettingsType == 0)
                {
                    GlobalSettings = new SystemSettingsController(StorageType.Json, ConfigurationManager.AppSettings["AppFolder"], null);
                    absenteeMode = GlobalSettings.CheckAbsenteeFile();
                }
                else if (DebugSettings.SettingsType == 1)
                {
                    string connection = @"XXXX";
                    GlobalSettings = new SystemSettingsController(StorageType.Database, ConfigurationManager.AppSettings["AppFolder"], connection, DebugSettings.SystemId);
                    Connection = "XXXX";
                    if (GlobalSettings.Absentee.SiteName != null) absenteeMode = true;
                }
                else if (DebugSettings.SettingsType == 2)
                {
                    string connection = @"XXXX";
                    GlobalSettings = new SystemSettingsController(StorageType.Database, ConfigurationManager.AppSettings["AppFolder"], connection, null);
                    Connection = "XXXX";
                    if (GlobalSettings.Absentee.SiteName != null) absenteeMode = true;
                }
            }
            catch(Exception exception)
            {
                AlertDialog settingsFailed = new AlertDialog("SETTINGS FILE FAILED TO LOAD\r\n" + exception.Message);
                settingsFailed.ShowDialog();
                // Sample Shut down process
                // https://stackoverflow.com/questions/606043/shutting-down-a-wpf-application-from-app-xaml-cs
                Shutdown(1);
                return;
            }

            // Set VCC Mode based on the current election date
            //GlobalSettings.SetElectionMode();
            GlobalSettings.System.BODVersion = "12.04.06.16";
            GlobalSettings.System.BODName = "VoterX Manager";
            

            // Clear Eligible Parties
            //GlobalSettings.Election.EligibleParties = null;

            if (absenteeMode == false)
            {
                AlertDialog settingsFailed = new AlertDialog("THIS SYSTEM IS NOT CONFIGURED TO RUN THE ABSENTEE APPLICATION\r\nPLEASE CONTACT TECHNICAL SUPPORT FOR ASSISTANCE");
                settingsFailed.ShowDialog();
                Shutdown(1);
                return;
            }

            debugMode = GlobalSettings.CheckDebugFile();

            // Shutdown system if already running
            if (debugMode != true)
            {
                Process proc = Process.GetCurrentProcess();
                int count = Process.GetProcesses().Where(p =>
                    p.ProcessName == proc.ProcessName).Count();

                if (count > 1)
                {
                    MessageBox.Show("An instance of VoterX is already running");
                    App.Current.Shutdown();
                }
            }

            GlobalSettings.User.UserName = GlobalSettings.LoadUserFile().UserName;

            // Set Default PDFTools License
            if (GlobalSettings.System.PDFTools == null || GlobalSettings.System.PDFTools == "")
            {
                GlobalSettings.System.PDFTools = "XXXX";
            }

            //debugMode = GlobalSettings.Network.Table2;
            //if(GlobalSettings.System.BODType == "82f99512335c701d889ba52faf31bcc224b05e43e4f6ebf892e5d1806fbef1eb")
            //{
            //    // Make the setting in a separate file
            //    // Then absentee type
            //    absenteeMode = true;
            //}
            //if (GlobalSettings.System.BODType == "f42d8b2d4e1364ed176ca8fac3c6f6a9762831b471b4421d5de6bc3c6989f941")
            //{
            //    // Then vcc type
            //    absenteeMode = false;
            //}

            //// Set connection string from system settings file
            ConnectionSetup.SetServerConnection(GlobalSettings.Network.SQLServer, GlobalSettings.Network.LocalDatabase, GlobalSettings.Network.SQLMode);
            ////ConnectionSetup.EncryptConnection();
            ConnectionSetup.RefreshConnection();

            //// Create and Load new Unity container
            //voterContainer = new VoterContainer();

            //// Connection check
            //AlertDialog connectionMessage = new AlertDialog(Connection);
            //connectionMessage.ShowDialog();

            // Create new Voter Factory
            Voters = new VoterFactory(GlobalSettings.Election.ElectionType.ToInt(), Connection);

            GlobalSettings.System.ReportErrorLogging = true;

            // Get machine name
            var localSqlServer = Environment.MachineName.Substring(0, Environment.MachineName.Length - 1) + "1";            
            
        }
    }
}
