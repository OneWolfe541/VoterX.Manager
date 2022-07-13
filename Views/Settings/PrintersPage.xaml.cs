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
using System.Management;
using VoterX.Methods;
using VoterX.Extensions;
using VoterX.Logging;
using VoterX.SystemSettings.Models;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for PrintersPage.xaml
    /// </summary>
    public partial class PrintersPage : SettingsBasePage
    {
        public class PrinterPageReady
        {
            public bool BallotPrinterReady;
            public bool BallotSizeReady;
            public bool BallotBinReady;

            public bool ReportPrinterReady;
            public bool ReportSizeReady;
            public bool ReportBinReady;

            //public bool SamplePrinterReady;
            //public bool SampleSizeReady;
            //public bool SampleBinReady;

            public bool AppPrinterReady;
            public bool AppSizeReady;
            public bool AppBinReady;

            public bool LabelPrinterReady;
            public bool LabelSizeReady;
            public bool LabelBinReady;

            public bool IsReady()
            {
                if (
                    BallotPrinterReady == true &&
                    BallotSizeReady == true &&
                    BallotBinReady == true &&
                    ReportPrinterReady == true &&
                    ReportSizeReady == true &&
                    ReportBinReady == true &&
                    //SamplePrinterReady == true &&
                    //SampleSizeReady == true &&
                    //SampleBinReady == true &&
                    AppPrinterReady == true &&
                    AppSizeReady == true &&
                    AppBinReady == true &&
                    LabelPrinterReady == true &&
                    LabelSizeReady == true &&
                    LabelBinReady == true
                    ) return true;
                else
                    return false;
            }
        }

        public bool pageLoaded = false;

        private IEnumerable<string> printerList = PrinterStatus.PrintersList();

        PrinterPageReady _pageReady = new PrinterPageReady();

        VoterXLogger settingsLog;

        public PrintersPage()
        {
            InitializeComponent();

            settingsLog = new VoterXLogger("VCClogs", true);
            settingsLog.WriteLog("Loading Printer Settings Page");

            LoadPrinters();

            DisplaySettings();            

            pageLoaded = true;
        }

        public PrintersPage(bool reload)
        {
            InitializeComponent();

            settingsLog = new VoterXLogger("VCClogs", true);
            settingsLog.WriteLog("Loading Printer Settings Page");

            ReloadPrinterLists();

            LoadPrinters();

            DisplaySettings();

            pageLoaded = true;
        }

        private void LoadPrinters()
        {
            LoadPrinterListIntoComboBox(BallotPrinter, AppSettings.Printers.BallotPrinter.ToString());
            _pageReady.BallotPrinterReady = true;
            LoadPrinterListIntoComboBox(AppPrinter, AppSettings.Printers.ApplicationPrinter.ToString());
            _pageReady.AppPrinterReady = true;
            LoadPrinterListIntoComboBox(ReportPrinter, AppSettings.Printers.ReportPrinter.ToString());
            _pageReady.ReportPrinterReady = true;
            //LoadPrinterListIntoComboBox(SamplePrinter, AppSettings.Printers.SamplePrinter.ToString());
            //_pageReady.SamplePrinterReady = true;
            LoadPrinterListIntoComboBox(LabelPrinter, AppSettings.Printers.LabelPrinter.ToString());
            _pageReady.LabelPrinterReady = true;
        }

        private void DisplaySettings()
        {
            // PRINTER SETTINGS //
            //if (AppSettings.Printers.NoPad) SignaturePad.Foreground = new SolidColorBrush(Colors.Green);
            //else SignaturePad.Foreground = new SolidColorBrush(Colors.Red);
            //SignaturePad.Text = AppSettings.Printers.NoPad.ToString();
            //BallotSize.Text = AppSettings.Printers.BallotSize.ToString();
            //AppSize.Text = AppSettings.Printers.AppSize.ToString();
            //ReportSize.Text = AppSettings.Printers.ReportSize.ToString();
            //SampleSize.Text = AppSettings.Printers.SampleSize.ToString();
            //LabelSize.Text = AppSettings.Printers.LabelSize.ToString();

            BallotSizeBox.Text = AppSettings.Printers.BallotSize.ToString();
            BallotBinBox.Text = AppSettings.Printers.BallotBin.ToString();
            AppSizeBox.Text = AppSettings.Printers.AppSize.ToString();
            AppBinBox.Text = AppSettings.Printers.AppBin.ToString();
            ReportSizeBox.Text = AppSettings.Printers.ReportSize.ToString();
            ReportBinBox.Text = AppSettings.Printers.ReportBin.ToString();
            LabelSizeBox.Text = AppSettings.Printers.LabelSize.ToString();
            LabelBinBox.Text = AppSettings.Printers.LabelBin.ToString();
        }

        public override void SaveSettings()
        {
            //StatusBar.ApplicationStatus("Printer Settings Page Called");

            // Boolean values will probably not be set on this page

            var ready = _pageReady.IsReady();
            if (_pageReady.IsReady() == true)
            {
                // PRINTER SETTINGS //
                AppSettings.Printers.BallotPrinter = GetSelectedItem(BallotPrinter);
                AppSettings.Printers.BallotSize = GetIntFromString(BallotSizeBox.Text);
                AppSettings.Printers.BallotBin = GetIntFromString(BallotBinBox.Text);

                AppSettings.Printers.ApplicationPrinter = GetSelectedItem(AppPrinter);
                AppSettings.Printers.AppSize = GetIntFromString(AppSizeBox.Text);
                AppSettings.Printers.AppBin = GetIntFromString(AppBinBox.Text);

                AppSettings.Printers.ReportPrinter = GetSelectedItem(ReportPrinter);
                AppSettings.Printers.ReportSize = GetIntFromString(ReportSizeBox.Text);
                AppSettings.Printers.ReportBin = GetIntFromString(ReportBinBox.Text);

                //AppSettings.Printers.SamplePrinter = GetSelectedItem(SamplePrinter);
                //AppSettings.Printers.SampleSize = GetSelectedItemNumber(SampleSize);
                //AppSettings.Printers.SampleBin = GetSelectedItemNumber(SampleBin);

                AppSettings.Printers.LabelPrinter = GetSelectedItem(LabelPrinter);
                AppSettings.Printers.LabelSize = GetIntFromString(LabelSizeBox.Text);
                AppSettings.Printers.LabelBin = GetIntFromString(LabelBinBox.Text);
            }
        }

        private string GetSelectedItem(ComboBox sender)
        {
            try
            {
                if (sender.SelectedItem == null) return "";
                else
                    return ((ComboBoxItem)sender.SelectedItem).Content.ToString();
            }
            catch
            {
                return "";
            }
        }

        private int GetSelectedItemNumber(ComboBox sender)
        {
            try
            {
                if (sender.SelectedItem == null) return 0;
                else
                    return (int)((ComboBoxItem)sender.SelectedItem).DataContext;
            }
            catch
            {
                return 0;
            }
        }

        private void LoadPrinterListIntoComboBox(object sender, string selectedItem)
        {
            ((ComboBox)sender).DataContext = "LOADING";

            // Add blank item to list
            AddComboItemToControl(sender, "", "");

            // Loop through list of printers 
            foreach (string printerName in printerList)
            {
                // Add each printer name to the combo box control
                AddComboItemToControl(sender, printerName, selectedItem);
            }

            ((ComboBox)sender).DataContext = "READY";
        }

        private async Task<bool> LoadBinList(object sender, string printerName, int selectedItem)
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem((ComboBox)sender, TempLoadingSpinnerItem);
            ((ComboBox)sender).DataContext = "LOADING";

            // Add blank item to list
            AddComboItemToControl(sender, "", "");

            List<PrinterTray> trayList = null;

            try
            {
                trayList = await AppSettings.Lists.GetPrinterTraysAsync(printerName);
            }
            catch (Exception e)
            {
                settingsLog.WriteLog("Printer Settings Error: [Loading Trays from File] " + e.Message);
            }

            try
            {
                // Loop through the list of paper trays
                //foreach (PrinterTray tray in await PrinterStatus.TraysList(printerName))
                foreach (var tray in trayList)
                {
                    // Add each item to the combo box control
                    AddComboItemToControl(sender, tray.Index, tray.Name, selectedItem);
                }
            }
            catch (Exception e)
            {
                settingsLog.WriteLog("Printer Settings Error: [Loading Trays into Control] " + e.Message);
            }

            ((ComboBox)sender).DataContext = "READY";
            ComboBoxMethods.RemoveListItem(sender, loadingItem);

            return true;
        }

        private async Task<bool> LoadPaperSizeList(object sender, string printerName, int selectedItem)
        {
            var loadingItem = ComboBoxMethods.AddLoadingItem((ComboBox)sender, TempLoadingSpinnerItem);
            ((ComboBox)sender).DataContext = "LOADING";

            // Add blank item to list
            AddComboItemToControl(sender, "", "");

            List<PaperSize> sizeList = null;

            try
            {
                sizeList = await AppSettings.Lists.GetPaperSizesAsync(printerName);
            }
            catch (Exception e)
            {
                settingsLog.WriteLog("Printer Settings Error: [Loading Paper Sizes from File] " + e.Message);
            }

            try
            {
                // Loop through the list of paper sizes
                //foreach (PaperSize paper in await PrinterStatus.PaperSizesList(printerName))
                foreach (var paper in sizeList)
                {
                    // Add each item to the combo box control
                    AddComboItemToControl(sender, paper.Index, paper.Name, selectedItem);
                }
            }
            catch (Exception e)
            {
                settingsLog.WriteLog("Printer Settings Error: [Loading Paper Sizes into Control] " + e.Message);
            }

            ((ComboBox)sender).DataContext = "READY";
            ComboBoxMethods.RemoveListItem(sender, loadingItem);

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

        // Add a single item to a combo box
        private void AddComboItemToControl(object sender, int itemIndex, string itemName, int selectedIndex)
        {
            // Create blank list item
            ComboBoxItem item = new ComboBoxItem();

            // Set list item name
            item.Content = itemName.ToUpper();

            // Set list item index
            item.DataContext = itemIndex;

            // Set default selected item
            if (itemIndex == selectedIndex) item.IsSelected = true;

            // Add the item to the combo box
            ((ComboBox)sender).Items.Add(item);
        }
        
        //private ComboBoxItem AddLoadingItem(object sender)
        //{
        //    // Create blank list item
        //    ComboBoxItem item = new ComboBoxItem();

        //    item.Content = TempLoadingSpinnerItem.Content;

        //    item.IsSelected = true;

        //    // Add the item to the combo box
        //    ((ComboBox)sender).Items.Add(item);

        //    return item;
        //}

        //// Remove item sample
        //// https://stackoverflow.com/questions/6831825/remove-combobox-item-from-combobox-wpf
        //private void RemoveListItem(object sender, ComboBoxItem item)
        //{
        //    ((ComboBox)sender).Items.Remove(item);
        //}

        // Change the Ballot Bins list when the Ballot Printer changes
        // Also runs when the drop down box loads for the first time
        private async void BallotPrinter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if combo box is loaded 
            // https://stackoverflow.com/questions/5022201/combobox-selectionchanged-event-triggers-without-even-changing-the-selection-in
            //var comboBox = (ComboBox)sender;
            //if (!comboBox.IsLoaded)
            //{
            //    // This is when the combo box is not loaded yet and the event is called.
            //    return;
            //}

            // Clear the combo box
            BallotBin.Items.Clear();

            _pageReady.BallotBinReady = false;
            _pageReady.BallotSizeReady = false;

            await LoadBinList(
                // Where to load the list into
                BallotBin,
                // Which printer to get list from
                GetSelectedItem(BallotPrinter),
                // Default Value from Global object
                AppSettings.Printers.BallotBin
                );

            _pageReady.BallotBinReady = true;

            // Clear the combo box
            BallotSize.Items.Clear();

            await LoadPaperSizeList(
                // Where to load the list into
                BallotSize,
                // Which printer to get list from
                GetSelectedItem(BallotPrinter),
                // Default Value from Global object
                AppSettings.Printers.BallotSize
                );

            _pageReady.BallotSizeReady = true;
        }

        // Load the list into the Ballot Printer combo box
        private void BallotPrinter_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPrinterListIntoComboBox(sender, AppSettings.Printers.BallotPrinter.ToString());
        }

        // Change the Application Bins list when the Application Printer changes
        // Also runs when the drop down box loads for the first time
        private async void AppPrinter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var comboBox = (ComboBox)sender;
            //if (!comboBox.IsLoaded)
            //{
            //    // This is when the combo box is not loaded yet and the event is called.
            //    return;
            //}

            // Clear the combo box
            AppBin.Items.Clear();

            _pageReady.AppBinReady = false;
            _pageReady.AppSizeReady = false;

            await LoadBinList(
                // Where to load the list into
                AppBin,
                // Which printer to get list from                    
                GetSelectedItem(AppPrinter),
                // Default Value from Global object
                AppSettings.Printers.AppBin
                );

            _pageReady.AppBinReady = true;

            // Clear the combo box
            AppSize.Items.Clear();

            await LoadPaperSizeList(
                // Where to load the list into
                AppSize,
                // Which printer to get list from
                GetSelectedItem(AppPrinter),
                // Default Value from Global object
                AppSettings.Printers.AppSize
                );

            _pageReady.AppSizeReady = true;
        }

        // Load the list into the Application Printer combo box
        private void AppPrinter_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPrinterListIntoComboBox(sender, AppSettings.Printers.ApplicationPrinter.ToString());
        }

        // Change the Sample Bins list when the Sample Printer changes
        // Also runs when the drop down box loads for the first time
        //private async void SamplePrinter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //var comboBox = (ComboBox)sender;
        //    //if (!comboBox.IsLoaded)
        //    //{
        //    //    // This is when the combo box is not loaded yet and the event is called.
        //    //    return;
        //    //}

        //    // Clear the combo box
        //    SampleBin.Items.Clear();

        //    _pageReady.SampleBinReady = false;
        //    _pageReady.SampleSizeReady = false;

        //    await LoadBinList(
        //        // Where to load the list into
        //        SampleBin,
        //        // Which printer to get list from                    
        //        GetSelectedItem(SamplePrinter),
        //        // Default Value from Global object
        //        AppSettings.Printers.SampleBin
        //        );

        //    _pageReady.SampleBinReady = true;

        //    // Clear the combo box
        //    SampleSize.Items.Clear();

        //    await LoadPaperSizeList(
        //        // Where to load the list into
        //        SampleSize,
        //        // Which printer to get list from
        //        GetSelectedItem(SamplePrinter),
        //        // Default Value from Global object
        //        AppSettings.Printers.SampleSize
        //        );

        //    _pageReady.SampleSizeReady = true;
        //}

        // Load the list into the Sample Printer combo box
        //private void SamplePrinter_Loaded(object sender, RoutedEventArgs e)
        //{
        //    LoadPrinterListIntoComboBox(sender, AppSettings.Printers.SamplePrinter.ToString());
        //}

        // Change the Report Bins list when the Report Printer changes
        // Also runs when the drop down box loads for the first time
        private async void LabelPrinter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var comboBox = (ComboBox)sender;
            //if (!comboBox.IsLoaded)
            //{
            //    // This is when the combo box is not loaded yet and the event is called.
            //    return;
            //}

            // Clear the combo box
            LabelBin.Items.Clear();

            _pageReady.LabelBinReady = false;
            _pageReady.LabelSizeReady = false;

            await LoadBinList(
                // Where to load the list into
                LabelBin,
                // Which printer to get list from                    
                GetSelectedItem(LabelPrinter),
                // Default Value from Global object
                AppSettings.Printers.LabelBin
                );

            _pageReady.LabelBinReady = true;

            // Clear the combo box
            LabelSize.Items.Clear();

            await LoadPaperSizeList(
                // Where to load the list into
                LabelSize,
                // Which printer to get list from
                GetSelectedItem(LabelPrinter),
                // Default Value from Global object
                AppSettings.Printers.LabelSize
                );

            _pageReady.LabelSizeReady = true;
        }

        // Change the Report Bins list when the Sample Printer changes
        // Also runs when the drop down box loads for the first time
        private async void ReportPrinter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var comboBox = (ComboBox)sender;
            //if (!comboBox.IsLoaded)
            //{
            //    // This is when the combo box is not loaded yet and the event is called.
            //    return;
            //}

            // Clear the combo box
            ReportBin.Items.Clear();

            _pageReady.ReportBinReady = false;
            _pageReady.ReportSizeReady = false;

            await LoadBinList(
                // Where to load the list into
                ReportBin,
                // Which printer to get list from                    
                GetSelectedItem(ReportPrinter),
                // Default Value from Global object
                AppSettings.Printers.ReportBin
                );

            _pageReady.ReportBinReady = true;

            // Clear the combo box
            ReportSize.Items.Clear();

            await LoadPaperSizeList(
                // Where to load the list into
                ReportSize,
                // Which printer to get list from
                GetSelectedItem(ReportPrinter),
                // Default Value from Global object
                AppSettings.Printers.ReportSize
                );

            _pageReady.ReportSizeReady = true;
        }

        // Load the list into the Report Printer combo box
        private void ReportPrinter_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPrinterListIntoComboBox(sender, AppSettings.Printers.ReportPrinter.ToString());
        }        

        // Load the list into the Ballot Bin combo box
        private void BallotBin_Loaded(object sender, RoutedEventArgs e)
        {
            //// Clear the combo box
            //BallotBin.Items.Clear();

            //LoadBinList(
            //    sender,
            //    GetSelectedItem(BallotPrinter),
            //    AppSettings.Printers.BallotBin.ToString()
            //    );
        }        

        // Load the list into the Application Bin combo box
        private void AppBin_Loaded(object sender, RoutedEventArgs e)
        {
            //// Clear the combo box
            //AppBin.Items.Clear();

            //LoadBinList(
            //    sender,
            //    GetSelectedItem(AppPrinter),
            //    AppSettings.Printers.AppBin.ToString()
            //    );
        }

        // Load the list into the Sample Bin combo box
        private void SampleBin_Loaded(object sender, RoutedEventArgs e)
        {
            //// Clear the combo box
            //SampleBin.Items.Clear();

            //LoadBinList(
            //    sender,
            //    GetSelectedItem(SamplePrinter),
            //    AppSettings.Printers.SampleBin.ToString()
            //    );
        }

        private void SampleBin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // Load the list into the Report Bin combo box
        private void ReportBin_Loaded(object sender, RoutedEventArgs e)
        {
            //// Clear the combo box
            //ReportBin.Items.Clear();

            //LoadBinList(
            //    sender,
            //    GetSelectedItem(ReportPrinter),
            //    AppSettings.Printers.ReportBin.ToString()
            //    );
        }        

        private void BallotSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BallotSizeBox.Text = GetSelectedItemNumber(BallotSize).ToString();
        }

        private void BallotBin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BallotBinBox.Text = GetSelectedItemNumber(BallotBin).ToString();
        }

        private void AppSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppSizeBox.Text = GetSelectedItemNumber(AppSize).ToString();
        }

        private void AppBin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppBinBox.Text = GetSelectedItemNumber(AppBin).ToString();
        }

        private void ReportSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReportSizeBox.Text = GetSelectedItemNumber(ReportSize).ToString();
        }

        private void ReportBin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReportBinBox.Text = GetSelectedItemNumber(ReportBin).ToString();
        }

        private void LabelSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LabelSizeBox.Text = GetSelectedItemNumber(LabelSize).ToString();
        }

        private void LabelBin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LabelBinBox.Text = GetSelectedItemNumber(LabelBin).ToString();
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

        private int GetIntFromString(string input)
        {
            int output = 0;
            if (Int32.TryParse(input, out output) == true)
            {
                return output;
            }
            else
            {
                return 0;
            }
        }

        private void LoadPrintersButton_Click(object sender, RoutedEventArgs e)
        {
            //_parent.Navigate(new PrintersPage(_container, _settings, _parent, true));
            this.NavigateToPage(new PrintersPage(true));
        }

        private async void ReloadPrinterLists()
        {
            var done = await ((App)Application.Current).GlobalSettings.ReloadPrinterLists();
            if (done == true)
            {
                //LoadPrinters();
            }
        }
    }
}
