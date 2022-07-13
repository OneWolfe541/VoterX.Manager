using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
//using VoterX.Core.Models.ViewModels;
//using VoterX.Core.Models.Database;
using VoterX.Methods;
using VoterX.Extensions;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeBatchManagerPage.xaml
    /// </summary>
    public partial class AbsenteeBatchManagerPage : Page
    {
        //private ObservableCollection<BatchDataModel> batchList = new ObservableCollection<BatchDataModel>();

        public AbsenteeBatchManagerPage()
        {
            InitializeComponent();

            LoadBatches();

            // Display page header
            GlobalReferences.Header.PageHeader = "Batch Manager" ;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.OpenMainMenu();
            GlobalReferences.MenuSlider.Open();
            this.NavigateToPage(new AbsenteeAdministrationPage());
        }

        private void LoadBatches()
        {
            // Reset search result messages
            SearchingPanel.Visibility = Visibility.Collapsed;
            SearchResults.Visibility = Visibility.Collapsed;
            SearchResults.Text = "";

            // Clear the list box in order to display searching spinner
            ClearListView();

            // Display search spinner
            GlobalReferences.StatusBar.SpinnerVisibility = true;
            SearchingPanel.Visibility = Visibility.Visible;

            // Load batches
            //batchList = await Task.Run(() => VoterMethods.Batches.GetLocalBatchedListAsync(AppSettings.Election.ElectionID, AppSettings.Election.CountyCode));

            //BatchListView.ItemsSource = batchList;

            // Turn off the progress spinner
            GlobalReferences.StatusBar.SpinnerVisibility = false;
            SearchingPanel.Visibility = Visibility.Collapsed;
        }

        // Clear the list view object
        private void ClearListView()
        {
            BatchListView.ItemsSource = null;
        }

        private void Page_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {

        }

        private void BatchListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Panel.SetZIndex(SearchListBorder, 999);
            //BatchDataModel selectedDataItem = (BatchDataModel)(((ListBox)sender).SelectedItem);
            object selectedItem = ((ListBox)sender).SelectedItem;
            ListBoxItem selectedListBoxItem = ((ListBox)sender).ItemContainerGenerator.ContainerFromItem(selectedItem) as ListBoxItem;
            //Panel.SetZIndex(selectedListBoxItem, 1);
            //SiteName.fore

            //StatusBar.ApplicationStatus(selectedDataItem.BatchId.ToString() + " " + selectedDataItem.BatchStatus);

            PrintBallotBatch.Visibility = Visibility.Visible;
        }

        private void PrintBallotBatch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
