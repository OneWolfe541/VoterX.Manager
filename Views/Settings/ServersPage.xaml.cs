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
using VoterX.SQLConnectionBuilder;
using VoterX.Methods;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using VoterX.Extensions;
using VoterX.Manager.Global;

// https://github.com/JakeGinnivan/SqlConnectionControl
namespace VoterX
{
    /// <summary>
    /// Interaction logic for ServersPage.xaml
    /// </summary>
    public partial class ServersPage : SettingsBasePage
    {
        public ServersPage()
        {
            InitializeComponent();

            DisplaySettings();
        }

        // Copy settings from the controls on the page to the global settings
        public override void SaveSettings()
        {
            AppSettings.Network.SQLServer = ServerList.GetSelectedItem();
            CurrentServer.Text = ServerList.GetSelectedItem();

            AppSettings.Network.LocalDatabase = DatabaseList.GetSelectedItem();
            CurrentDatabase.Text = DatabaseList.GetSelectedItem();
        }

        public void DisplaySettings()
        {
            CurrentServer.Text = AppSettings.Network.SQLServer;
        }

        public async Task<bool> LoadServersList()
        {
            var serverList = await SQLConnectionFinder.GetServerListAsync();

            ServerList.Items.Clear();

            foreach (string serverName in serverList)
            {
                AddComboItemToControl(ServerList, serverName, "AESSQL2");
            }

            return true;
        }

        // Add a single item to a combo box
        private void AddComboItemToControl(object sender, string newItem, string selectedItem)
        {
            // Create blank list item
            ComboBoxItem item = new ComboBoxItem();
            // Set list item name
            item.Content = newItem.ToUpper();
            // Check default for null and empty strings
            if (selectedItem != null && selectedItem != "" && selectedItem.Replace(" ", "") != "")
            {
                // Set selected item default from given string
                if (newItem.ToUpper().Contains(selectedItem.ToUpper()))
                    item.IsSelected = true;
            }
            // Add the item to the combo box
            ((ComboBox)sender).Items.Add(item);
        }

        private void ServerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ServerList_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void ServerList_DropDownOpened(object sender, EventArgs e)
        {
            if (ServerList.Items.Count == 1)
            {
                GlobalReferences.StatusBar.TextLeft = ("Loading Server List");
                //StatusBar.LoadingSpinner(Visibility.Visible);
                GlobalReferences.StatusBar.SpinnerVisibility = true;
                //LoadingItem.Visibility = Visibility.Visible;
                var done = await LoadServersList();
                //StatusBar.LoadingSpinner(Visibility.Collapsed);
                GlobalReferences.StatusBar.SpinnerVisibility = false;
                //LoadingItem.Visibility = Visibility.Collapsed;
            }
        }

        private void DatabaseList_DropDownOpened(object sender, EventArgs e)
        {
            DatabaseList.Items.Clear();

            string server = ServerList.GetSelectedItem();

            GlobalReferences.StatusBar.TextLeft = (server);

            var dbList = SQLConnectionFinder.GetDataBases(server);

            foreach (string dbName in dbList)
            {
                AddComboItemToControl(DatabaseList, dbName, "");
            }
        }

        private void DatabaseList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentServer.Text = ServerList.GetSelectedItem();
        }

        private void DatabaseList_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentDatabase.Text = DatabaseList.GetSelectedItem();
        }
    }
}
