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
    /// Interaction logic for BallotsPage.xaml
    /// </summary>
    public partial class BallotsPage : SettingsBasePage
    {
        public bool pageLoaded = false;

        public BallotsPage()
        {
            InitializeComponent();
            DisplaySettings();

            pageLoaded = true;
        }

        private void DisplaySettings()
        {
            // BALLOT SETTINGS //
            //BallotPDF.Text = AppSettings.Ballots.BallotPDF.ToString();
            BallotsFolder.Text = AppSettings.Ballots.BallotFolder;
            //if (AppSettings.Ballots.ProvisionalBallot) ProvisionalBallot.Foreground = new SolidColorBrush(Colors.Green);
            //else ProvisionalBallot.Foreground = new SolidColorBrush(Colors.Red);
            //ProvisionalBallot.Text = AppSettings.Ballots.ProvisionalBallot.ToString();
            //ProvisionalPDF.Text = AppSettings.Ballots.ProvisionalPDF.ToString();
            ProvisionalFolder.Text = AppSettings.Ballots.ProvisionalFolder;
            ProvisionalPrefix.Text = AppSettings.Ballots.ProvisionalPrefix;
            //if (AppSettings.Ballots.SampleBallot) SampleBallot.Foreground = new SolidColorBrush(Colors.Green);
            //else SampleBallot.Foreground = new SolidColorBrush(Colors.Red);
            //SampleBallot.Text = AppSettings.Ballots.SampleBallot.ToString();
            //SamplePDF.Text = AppSettings.Ballots.SamplePDF.ToString();
            SampleFolder.Text = AppSettings.Ballots.SampleFolder;

            TestFile.Text = AppSettings.Ballots.TestBallot;

            DuplexCheck.IsChecked = AppSettings.Ballots.Duplex;

            NumberOfLabelsToPrint.Text = AppSettings.Ballots.LabelCount.ToString();
            PartyLabelCheck.IsChecked = AppSettings.Ballots.IncludePartyOnLabel;
        }

        public override void SaveSettings()
        {
            //StatusBar.ApplicationStatus("Ballot Settings Page Called");

            // Boolean values will probably not be set on this page

            // BALLOT SETTINGS //
            //AppSettings.Ballots.BallotPDF = BallotPDF.Text;
            AppSettings.Ballots.BallotFolder = BallotsFolder.Text;
            //AppSettings.BallotConfigs().ProvisionalBallot = bool.Parse(ProvisionalBallot.Text);
            //AppSettings.Ballots.ProvisionalPDF = ProvisionalPDF.Text;
            AppSettings.Ballots.ProvisionalFolder = ProvisionalFolder.Text;
            AppSettings.Ballots.ProvisionalPrefix = ProvisionalPrefix.Text;
            //AppSettings.BallotConfigs().SampleBallot = bool.Parse(SampleBallot.Text);
            //AppSettings.Ballots.SamplePDF = SamplePDF.Text;
            AppSettings.Ballots.SampleFolder = SampleFolder.Text;

            AppSettings.Ballots.TestBallot = TestFile.Text;

            AppSettings.Ballots.Duplex = (bool)DuplexCheck.IsChecked;

            if (Int32.TryParse(NumberOfLabelsToPrint.Text, out int labelCount))
            {
                AppSettings.Ballots.LabelCount = labelCount;
            }

            if (PartyLabelCheck.IsChecked == true)
            {
                AppSettings.Ballots.IncludePartyOnLabel = true;
            }
            else
            {
                AppSettings.Ballots.IncludePartyOnLabel = false;
            }
        }

        // Folder Browser Sample
        // https://stackoverflow.com/questions/1922204/open-directory-dialog
        private string OpenFolderBrowser(string currentPath)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = currentPath;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                return dialog.SelectedPath;
            }
        }

        private string OpenFileBrowser(string currentFile)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.FileName = currentFile;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                return dialog.FileName;
            }
        }

        private void FolderBrowser_Click(object sender, RoutedEventArgs e)
        {
            BallotsFolder.Text = OpenFolderBrowser(AppSettings.Ballots.BallotFolder);
            SettingsChange();
        }

        private void ProvisionalFolderBrowser_Click(object sender, RoutedEventArgs e)
        {
            ProvisionalFolder.Text = OpenFolderBrowser(AppSettings.Ballots.ProvisionalFolder);
            SettingsChange();
        }

        private void SampleFolderBrowser_Click(object sender, RoutedEventArgs e)
        {
            SampleFolder.Text = OpenFolderBrowser(AppSettings.Ballots.SampleFolder);
            SettingsChange();
        }

        private void TestFileBrowser_Click(object sender, RoutedEventArgs e)
        {
            TestFile.Text = OpenFileBrowser(AppSettings.Ballots.TestBallot);
            SettingsChange();
        }

        private void DuplexCheck_Click(object sender, RoutedEventArgs e)
        {
            SettingsChange();
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
                //if(NumberOfLabelsToPrint.Text == null || NumberOfLabelsToPrint.Text == "")
                //{
                //    NumberOfLabelsToPrint.Text = "0";
                //}

                SettingsChange();

                //SelectAll(e.OriginalSource as TextBox);
            }
        }

        //https://stackoverflow.com/questions/14813960/how-to-accept-only-integers-in-a-wpf-textbox
        private void MaskNumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !TextIsNumeric(e.Text) & !NumberInRage(e.Text);
        }

        private void MaskNumericPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string input = (string)e.DataObject.GetData(typeof(string));
                if (!TextIsNumeric(input) & !NumberInRage(input)) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool TextIsNumeric(string input)
        {
            return input.All(c => Char.IsDigit(c) || Char.IsControl(c));
        }

        private bool NumberInRage(string input)
        {
            if (Int32.TryParse(input, out int labelCount))
            {
                return (labelCount >= 0 && labelCount <= 4);
            }
            else
            {
                return false;
            }
        }

        private void NumberOfLabelsToPrint_KeyDown(object sender, KeyEventArgs e)
        {
            var test = e.Key;
            if (!e.Key.Equals(Key.Tab) &&
                !e.Key.Equals(Key.Back) &&
                //!e.Key.Equals(Key.D0) && 
                !e.Key.Equals(Key.D1) && 
                !e.Key.Equals(Key.D2) && 
                !e.Key.Equals(Key.D3) &&
                !e.Key.Equals(Key.D4) &&
                //!e.Key.Equals(Key.NumPad0) &&
                !e.Key.Equals(Key.NumPad1) &&
                !e.Key.Equals(Key.NumPad2) &&
                !e.Key.Equals(Key.NumPad3) &&
                !e.Key.Equals(Key.NumPad4))
            {
                e.Handled = true;
                return;
            }
            e.Handled = false;
            return;
        }

        private void NumberOfLabelsToPrint_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectAll(e.OriginalSource as TextBox);
        }

        private void NumberOfLabelsToPrint_GotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SelectAll(e.OriginalSource as TextBox);
        }

        private void NumberOfLabelsToPrint_GotMouseCapture(object sender, MouseEventArgs e)
        {
            SelectAll(e.OriginalSource as TextBox);
        }

        //https://stackoverflow.com/questions/660554/how-to-automatically-select-all-text-on-focus-in-wpf-textbox
        private void SelectAll(TextBox textBox)
        {
            if (textBox != null)
                textBox.SelectAll();
        }
    }
}
