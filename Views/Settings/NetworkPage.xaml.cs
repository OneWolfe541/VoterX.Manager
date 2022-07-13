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

namespace VoterX
{
    /// <summary>
    /// Interaction logic for NetworkPage.xaml
    /// </summary>
    public partial class NetworkPage : SettingsBasePage
    {
        public bool pageLoaded = false;

        public NetworkPage()
        {
            InitializeComponent();
            DisplaySettings();

            pageLoaded = true;
        }

        private void DisplaySettings()
        {
            SQLServerName.Text = AppSettings.Network.SQLServer.ToString();
            DatabaseName.Text = AppSettings.Network.LocalDatabase.ToString();

            SetSQLMode();

            // NETWORK SETTINGS //
            //NetworkType.Text = AppSettings.Network.NetworkTypeID.ToString();
            //TableLink.Text = AppSettings.Network.TableLink.ToString();
            //StartDate.Text = AppSettings.Network.StartDate.ToString();
            //LastLogin.Text = AppSettings.Network.LastLogin.ToString();
            //LastEODUpdate.Text = AppSettings.Network.LastEndOfDayUpdate.ToLocalTime().ToString();
            //LastSQLUpdate.Text = AppSettings.Network.LastSQLUpdate.ToLocalTime().ToString();
            //LocalTable.Text = AppSettings.Network.LocalTable.ToString();
            //RemoteTable.Text = AppSettings.Network.RemoteTable.ToString();
            //if (AppSettings.Network.Table2) Table2.Foreground = new SolidColorBrush(Colors.Green);
            //else Table2.Foreground = new SolidColorBrush(Colors.Red);
            //Table2.Text = AppSettings.Network.Table2.ToString();
            //LocalTable2.Text = AppSettings.Network.LocalTable2.ToString();
            //RemoteTable2.Text = AppSettings.Network.RemoteTable2.ToString();
        }

        public override void SaveSettings()
        {
            //StatusBar.ApplicationStatus("Network Settings Page Called");

            // Boolean values will probably not/never be set on this page

            AppSettings.Network.SQLServer = SQLServerName.Text;
            AppSettings.Network.LocalDatabase = DatabaseName.Text;

            AppSettings.Network.SQLMode = ComboBoxMethods.GetSelectedItemNumber(SQLModeList);

            // NETWORK SETTINGS //
            //AppSettings.Network.NetworkTypeID = Int32.Parse(NetworkType.Text);
            //AppSettings.Network.TableLink = TableLink.Text;
            //AppSettings.Network.StartDate = DateTime.Parse(StartDate.Text);
            //AppSettings.Network.LastLogin = DateTime.Parse(LastLogin.Text);
            //AppSettings.Network.LastEndOfDayUpdate = DateTime.Parse(LastEODUpdate.Text);
            //AppSettings.Network.LastSQLUpdate = DateTime.Parse(LastSQLUpdate.Text);
            //AppSettings.Network.LocalTable = LocalTable.Text;
            //AppSettings.Network.RemoteTable = RemoteTable.Text;
            //AppSettings.NetworkConfigs.Table2 = bool.Parse(Table2.Text);
            //AppSettings.Network.LocalTable2 = LocalTable2.Text;
            //AppSettings.Network.RemoteTable2 = RemoteTable2.Text;
        }

        private void SetSQLMode()
        {
            switch (AppSettings.Network.SQLMode)
            {
                case 0:
                    WindowsLogin.IsSelected = true;
                    break;
                case 1:
                    SQLLogin.IsSelected = true;
                    break;
            }
        }

        private void SettingsChange()
        {
            AppSettings.SettingsChanged = true;
        }

        private void SettingsChanged_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).IsLoaded)
            {
                SettingsChange();
            }
        }

        private void SettingsChanged_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((ComboBox)sender).IsLoaded)
            {
                SettingsChange();
            }
        }

        private void SettingsChanged_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pageLoaded == true)
            {
                SettingsChange();
            }
        }
    }
}
