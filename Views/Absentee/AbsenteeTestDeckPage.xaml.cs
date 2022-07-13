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
    public partial class AbsenteeTestDeckPage : Page
    {
        public AbsenteeTestDeckPage()
        {
            InitializeComponent();

            GlobalReferences.Header.PageHeader = ("Test Deck");

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

        private void LoadBallotStyles()
        {
            // Create animated loading list item
            var loadingItem = ComboBoxMethods.AddLoadingItem(BallotStyleList, TempLoadingSpinnerItem);

            //if (await Task.Run(() => VoterMethods.BallotStyleMaster.Exists(0)) == true)
            //{
            //    foreach (var ballotstyle in await Task.Run(() => VoterMethods.BallotStyleMaster.List()))
            //    {
            //        ComboBoxMethods.AddComboItemToControl(
            //            BallotStyleList,
            //            ballotstyle.ballot_style_filename,
            //            ballotstyle.ballot_style_name,
            //            ""
            //            );
            //    }
            //}

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
                            GlobalReferences.StatusBar.TextLeft = ("Print All Test Ballots");
                            PrintAllTestBallots(numberOfCopies);
                        }
                        else if(SelectedBallotsPrint.IsChecked == true)
                        {
                            if (BallotStyleList.SelectedIndex > -1)
                            {
                                GlobalReferences.StatusBar.TextLeft = ("Print Selected Test Ballot");
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
                            GlobalReferences.StatusBar.TextLeft = ("Print All Emergency Ballots");
                        }
                        else if (SelectedBallotsPrint.IsChecked == true)
                        {
                            if (BallotStyleList.SelectedIndex > -1)
                            {                                
                                PrintSelectedEmergencyBallot(numberOfCopies);
                                GlobalReferences.StatusBar.TextLeft = ("Print Selected Emergency Ballot");
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

        private void PrintAllTestBallots(int copies)
        {
            //if (await Task.Run(() => VoterMethods.BallotStyleMaster.Exists(0)) == true)
            //{
            //    foreach (var ballotstyle in await Task.Run(() => VoterMethods.BallotStyleMaster.List()))
            //    {
            //        BallotPrinting.PrintSampleCopies(ballotstyle.ballot_style_filename, copies);
            //    }
            //}
        }

        private void PrintSelectedTestBallot(int copies)
        {
            string ballotstylefile = ComboBoxMethods.GetSelectedItemData(BallotStyleList).ToString();
            GlobalReferences.StatusBar.TextLeft = ("Print Selected Test Ballot: " + ballotstylefile);
            BallotPrinting.PrintSampleCopies(ballotstylefile, copies);
        }

        private void PrintAllEmergencyBallots(int copies)
        {
            //if (await Task.Run(() => VoterMethods.BallotStyleMaster.Exists(0)) == true)
            //{
            //    foreach (var ballotstyle in await Task.Run(() => VoterMethods.BallotStyleMaster.List()))
            //    {
            //        BallotPrinting.PrintBallotCopies(ballotstyle.ballot_style_filename, copies);
            //    }
            //}
        }

        private void PrintSelectedEmergencyBallot(int copies)
        {
            string ballotstylefile = ComboBoxMethods.GetSelectedItemData(BallotStyleList).ToString();
            GlobalReferences.StatusBar.TextLeft = ("Print Selected Test Ballot: " + ballotstylefile);
            BallotPrinting.PrintBallotCopies(ballotstylefile, copies);
        }
    }
}
