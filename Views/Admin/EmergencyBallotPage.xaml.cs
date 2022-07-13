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
using VoterX.Dialogs;
using VoterX.Extensions;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeTestDeckPage.xaml
    /// </summary>
    public partial class EmergencyBallotPage : Page
    {
        public EmergencyBallotPage()
        {
            InitializeComponent();

            GlobalReferences.Header.PageHeader = ("Emergency Ballot");

            //StatusBar.ApplicationCheckPrinterAsync();
            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            CheckServer();

            LoadBallotStyles();
        }

        private void CheckServer()
        {
            //PrintButton.Visibility = Visibility.Collapsed;

            //if (await StatusBar.CheckServer() == true)
            //{
            //    PrintButton.Visibility = Visibility.Visible;
            //}
        }

        private async void LoadBallotStyles()
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(BallotStyleList, TempLoadingSpinnerItem);

            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                foreach (var ballotstyle in await Task.Run(() => ElectionDataMethods.UniqueBallotStyles))
                {
                    ComboBoxMethods.AddComboItemToControl(
                        BallotStyleList,
                        ballotstyle.BallotStyleFileName,
                        ballotstyle.BallotStyleName,
                        ""
                        );
                }
            }
            else
            {
                GlobalReferences.StatusBar.TextCenter = ("Database not found");
            }

            // Remove animated loading list item
            ComboBoxMethods.RemoveListItem(BallotStyleList, loadingItem);
        }

        private void TestBallotType_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EmergencyBallotType_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AllBallotsPrint_Click(object sender, RoutedEventArgs e)
        {
            BallotStyleList.IsEnabled = false;
            BallotStyleList.SelectedIndex = -1;
        }

        private void SelectedBallotsPrint_Click(object sender, RoutedEventArgs e)
        {
            BallotStylePanel.Visibility = Visibility.Visible;
            //BallotStyleList.IsEnabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Open Menu 
            //MainMenuMethods.OpenMainMenu();
            GlobalReferences.MenuSlider.Open();

            this.NavigateToPage(new AdministrationPage());
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            int numberOfCopies = 0;
            if (Int32.TryParse(NumberOfCopiesBox.Text, out numberOfCopies) && NumberOfCopiesBox.Text != "" && NumberOfCopiesBox.Text != "0")
            {
                if (numberOfCopies > 0)
                {
                    // Check radio button settings and text boxes to decide what to print
                    if(TestBallotType.IsChecked == true)
                    {
                        if(AllBallotsPrint.IsChecked == true)
                        {
                            //StatusBar.ApplicationStatusLeft("Print All Test Ballots");
                            PrintAllTestBallots(numberOfCopies);
                        }
                        else if(SelectedBallotsPrint.IsChecked == true)
                        {
                            if (BallotStyleList.SelectedIndex > -1)
                            {
                                //StatusBar.ApplicationStatusLeft("Print Selected Test Ballot");
                                PrintSelectedTestBallot(numberOfCopies);
                            }
                            else
                            {
                                AlertDialog newMessage = new AlertDialog("PLEASE SELEECT A VALID BALLOT STYLE");
                                newMessage.ShowDialog();
                            }
                        }
                    }
                    else if(EmergencyBallotType.IsChecked == true)
                    {
                        if (AllBallotsPrint.IsChecked == true)
                        {
                            PrintAllEmergencyBallots(numberOfCopies);
                            //StatusBar.ApplicationStatusLeft("Print All Emergency Ballots");
                        }
                        else if (SelectedBallotsPrint.IsChecked == true)
                        {
                            if (BallotStyleList.SelectedIndex > -1)
                            {
                                PrintSelectedEmergencyBallot(numberOfCopies);
                                //StatusBar.ApplicationStatusLeft("Print Selected Emergency Ballot");
                            }
                            else
                            {
                                AlertDialog newMessage = new AlertDialog("PLEASE SELEECT A VALID BALLOT STYLE");
                                newMessage.ShowDialog();
                            }
                        }
                    }
                }
                else
                {
                    AlertDialog newMessage = new AlertDialog("NUMBER OF COPIES MUST NOT BE A WHOLE NUMBER GREATER THAN 0");
                    newMessage.ShowDialog();
                }
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("NUMBER OF COPIES MUST NOT BE A WHOLE NUMBER GREATER THAN 0");
                newMessage.ShowDialog();
            }
        }

        private async void PrintAllTestBallots(int copies)
        {
            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                foreach (var ballotstyle in await Task.Run(() => ElectionDataMethods.BallotStyles))
                {
                    BallotPrinting.PrintSampleCopies(ballotstyle.BallotStyleFileName, copies);
                }
            }
        }

        private void PrintSelectedTestBallot(int copies)
        {
            string ballotstylefile = ComboBoxMethods.GetSelectedItemData(BallotStyleList).ToString();
            //StatusBar.ApplicationStatusLeft("Print Selected Test Ballot: " + ballotstylefile);
            BallotPrinting.PrintSampleCopies(ballotstylefile, copies);
        }

        private async void PrintAllEmergencyBallots(int copies)
        {
            if (await Task.Run(() => ElectionDataMethods.Exists) == true)
            {
                foreach (var ballotstyle in await Task.Run(() => ElectionDataMethods.BallotStyles))
                {
                    BallotPrinting.PrintBallotCopies(ballotstyle.BallotStyleFileName, copies);
                }
            }
        }

        private void PrintSelectedEmergencyBallot(int copies)
        {
            string ballotstylefile = ComboBoxMethods.GetSelectedItemData(BallotStyleList).ToString();
            //StatusBar.ApplicationStatusLeft("Print Selected Test Ballot: " + ballotstylefile);
            BallotPrinting.PrintBallotCopies(ballotstylefile, copies);
        }

        private void PrintAllButton_Click(object sender, RoutedEventArgs e)
        {
            int numberOfCopies = 0;
            if (Int32.TryParse(NumberOfCopiesBox.Text, out numberOfCopies) && NumberOfCopiesBox.Text != "" && NumberOfCopiesBox.Text != "0")
            {
                if (numberOfCopies > 0)
                {
                    if (EmergencyBallotType.IsChecked == true)
                    {
                        PrintAllEmergencyBallots(numberOfCopies);
                        //StatusBar.ApplicationStatusLeft("Print All Emergency Ballots");                        
                    }
                }
                else
                {
                    AlertDialog newMessage = new AlertDialog("NUMBER OF COPIES MUST BE A WHOLE NUMBER GREATER THAN 0");
                    newMessage.ShowDialog();
                }
            }
            else
            {
                AlertDialog newMessage = new AlertDialog("NUMBER OF COPIES MUST BE A WHOLE NUMBER GREATER THAN 0");
                newMessage.ShowDialog();
            }
        }
    }
}
