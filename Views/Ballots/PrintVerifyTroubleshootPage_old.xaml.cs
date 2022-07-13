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
using VoterX.Extensions;
//using VoterX.Core.Models.ViewModels;
using VoterX.Methods;
using System.Windows.Controls.Primitives;
using FontAwesome.WPF;
using VoterX.SystemSettings.Extensions;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for PrintVerifyTroubleshootPage.xaml
    /// </summary>
    public partial class PrintVerifyTroubleshootPage_old : Page
    {
        //private VoterDataModel _voter = new VoterDataModel();

        private int printingErrors = 0;

        private bool exitLogout = false;

        //public PrintVerifyTroubleshootPage_old(VoterDataModel voter)
        //{
        //    InitializeComponent();
        //    _voter = voter;
        //    VoterX.Methods.StatusBar.ApplicationPageHeader("Print Verification");
        //}

        //public PrintVerifyTroubleshootPage_old(VoterDataModel voter, bool returnStatus)
        //{
        //    InitializeComponent();
        //    _voter = voter;
        //    exitLogout = returnStatus;
        //    if (returnStatus == true)
        //    {
        //        BackReturnLabel.Visibility = Visibility.Collapsed;
        //        BackLogoutLabel.Visibility = Visibility.Visible;
        //    }
        //    VoterX.Methods.StatusBar.ApplicationPageHeader("Print Verification");
        //}

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new LoginPage());
            }
        }

        private void NextVerifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new LoginPage());
            }
        }

        private void ResetQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            printingErrors = 0;

            // Hide return to search button
            NextVerify.Visibility = Visibility.Collapsed;

            // Rest Ballot Questions
            ClearYesNoPair(BallotValidationYes, BallotValidationNo);            
            ClearYesNoPair(BallotPrinterCheckYes, BallotPrinterCheckNo);            
            BallotPrinterCheckPanel.Visibility = Visibility.Collapsed;
            BallotOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintBallotCheck.IsChecked = false;
            TransferVoterCheck.IsChecked = false;

            // Reset Application Questions            
            ClearYesNoPair(ApplicationValidationYes, ApplicationValidationNo);            
            //ClearYesNoPair(ApplicationPrinterCheckYes, ApplicationPrinterCheckNo);            
            ApplicationVerificationPanel.Visibility = Visibility.Collapsed;
            //ApplicationPrinterCheckPanel.Visibility = Visibility.Collapsed;
            ApplicationOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintApplicationCheck.IsChecked = false;
            ApplicationTransferVoterCheck.IsChecked = false;

            // Reset Permit Questions
            ClearYesNoPair(PermitValidationYes, PermitValidationNo);
            //ClearYesNoPair(PermitPrinterCheckYes, PermitPrinterCheckNo);
            PermitVerificationPanel.Visibility = Visibility.Collapsed;
            //PermitPrinterCheckPanel.Visibility = Visibility.Collapsed;
            PermitOptionsPanel.Visibility = Visibility.Collapsed;
            PermitReprintCheck.IsChecked = false;
            PermitTransferVoterCheck.IsChecked = false;

            // Reset Stub Questions
            ClearYesNoPair(StubValidationYes, StubValidationNo);
            //ClearYesNoPair(StubPrinterCheckYes, StubPrinterCheckNo);
            StubVerificationPanel.Visibility = Visibility.Collapsed;
            //StubPrinterCheckPanel.Visibility = Visibility.Collapsed;
            StubOptionsPanel.Visibility = Visibility.Collapsed;
            StubReprintCheck.IsChecked = false;
            StubTransferVoterCheck.IsChecked = false;
        }

        private void CheckYesNoPair(ToggleButton buttonToCheck, ToggleButton buttonToUncheck)
        {
            // Set checked button
            buttonToCheck.IsChecked = true;
            buttonToCheck.IsEnabled = false;
            
            var childListChecked = FindVisualChildren<ImageAwesome>(buttonToCheck);
            var iconChecked = childListChecked.FirstOrDefault();
            if (iconChecked != null)
            {
                iconChecked.Visibility = Visibility.Visible;
            }            

            // Set unchecked button
            buttonToUncheck.IsChecked = false;
            buttonToUncheck.IsEnabled = false;
            
            var childListUnchecked = FindVisualChildren<ImageAwesome>(buttonToUncheck);
            var iconUnchecked = childListUnchecked.FirstOrDefault();
            if (iconUnchecked != null)
            {
                iconUnchecked.Visibility = Visibility.Hidden;
            }
            
        }

        private void ClearYesNoPair(ToggleButton buttonYes, ToggleButton buttonNo)
        {
            // Set checked button
            buttonYes.IsChecked = false;
            buttonYes.IsEnabled = true;
            
            var childListYes = FindVisualChildren<ImageAwesome>(buttonYes);
            var iconYes = childListYes.FirstOrDefault();
            if (iconYes != null)
            {
                iconYes.Visibility = Visibility.Hidden;
            }            

            // Set unchecked button
            buttonNo.IsChecked = false;
            buttonNo.IsEnabled = true;
            
            var childListNo = FindVisualChildren<ImageAwesome>(buttonNo);
            var iconNo = childListNo.FirstOrDefault();
            if (iconNo != null)
            {
                iconNo.Visibility = Visibility.Hidden;
            }            
        }

        // Find all children of tpye<t> sample
        // https://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void BallotValidationYes_Click(object sender, RoutedEventArgs e)
        {
            // Set Yes to checked and No to Disabled
            CheckYesNoPair(BallotValidationYes, BallotValidationNo);

            // Show the Next Question
            if (AppSettings.Election.IsElectionDay())
            //if (true)
            {
                PermitVerificationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ApplicationVerificationPanel.Visibility = Visibility.Visible;
            }
        }

        private void BallotValidationNo_Click(object sender, RoutedEventArgs e)
        {
            // Set No to checked and Yes to Disabled
            CheckYesNoPair(BallotValidationNo, BallotValidationYes);

            if (printingErrors == 0)
            {
                // Show the troubleshooting message
                BallotPrinterCheckPanel.Visibility = Visibility.Visible;
            }
            else
            {
                NextVerify.Visibility = Visibility.Visible;
            }
            printingErrors++;
        }

        private void BallotPrinterCheckYes_Click(object sender, RoutedEventArgs e)
        {
            // Set Yes to checked and No to Disabled
            CheckYesNoPair(BallotPrinterCheckYes, BallotPrinterCheckNo);

            // Reset Printer Check
            ClearYesNoPair(BallotValidationYes, BallotValidationNo);
            ClearYesNoPair(BallotPrinterCheckYes, BallotPrinterCheckNo);
            BallotPrinterCheckPanel.Visibility = Visibility.Collapsed;
            BallotOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintBallotCheck.IsChecked = false;
            TransferVoterCheck.IsChecked = false;
            CheckYesNoPair(BallotValidationYes, BallotValidationNo);

            // Show the Next Question
            if (AppSettings.Election.IsElectionDay())
            //if(true)
            {
                PermitVerificationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ApplicationVerificationPanel.Visibility = Visibility.Visible;
            }
        }

        private void BallotPrinterCheckNo_Click(object sender, RoutedEventArgs e)
        {
            // Set No to checked and Yes to Disabled
            CheckYesNoPair(BallotPrinterCheckNo, BallotPrinterCheckYes);
            // Display options panel
            BallotOptionsPanel.Visibility = Visibility.Visible;
        }

        private void ReprintBallotCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck the other box
            TransferVoterCheck.IsChecked = false;

            // Print a new ballot
            //await Task.Run(() => BallotPrinting.ReprintBallot(_voter));

            // Spoil previous ballot with "print error" reason
            //VoterMethods.InsertSpoiledBallot(_voter, 1);

            // Display new printing message
            BallotPrintingStatus.Text = "A new Ballot was sent to the Printer";

            // Reset and hide questions
            ClearYesNoPair(BallotValidationYes, BallotValidationNo);
            ClearYesNoPair(BallotPrinterCheckYes, BallotPrinterCheckNo);
            BallotPrinterCheckPanel.Visibility = Visibility.Collapsed;
            BallotOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintBallotCheck.IsChecked = false;
            TransferVoterCheck.IsChecked = false;
        }

        private void TransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            ReprintBallotCheck.IsChecked = false;
            // Return to search screen
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new LoginPage());
            }
        }

        private void ApplicationValidationYes_Click(object sender, RoutedEventArgs e)
        {
            // Set Yes to checked and No to Disabled
            CheckYesNoPair(ApplicationValidationYes, ApplicationValidationNo);

            // Check for Stub
            if (AppSettings.System.BallotStub == 1)
            {
                // Display Stub Question
                StubVerificationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                // Display Return to search button
                NextVerify.Visibility = Visibility.Visible;
            }
        }

        private void ApplicationValidationNo_Click(object sender, RoutedEventArgs e)
        {
            // Set No to checked and Yes to Disabled
            CheckYesNoPair(ApplicationValidationNo, ApplicationValidationYes);

            // Show the troubleshooting message
            ApplicationOptionsPanel.Visibility = Visibility.Visible;
        }

        //private void ApplicationPrinterCheckYes_Click(object sender, RoutedEventArgs e)
        //{
        //    // Set Yes to checked and No to Disabled
        //    CheckYesNoPair(ApplicationPrinterCheckYes, ApplicationPrinterCheckNo);

        //    // Reset application question
        //    ClearYesNoPair(ApplicationValidationYes, ApplicationValidationNo);
        //    ClearYesNoPair(ApplicationPrinterCheckYes, ApplicationPrinterCheckNo);
        //    ApplicationPrinterCheckPanel.Visibility = Visibility.Collapsed;
        //    ApplicationOptionsPanel.Visibility = Visibility.Collapsed;
        //    ReprintApplicationCheck.IsChecked = false;
        //    ApplicationTransferVoterCheck.IsChecked = false;
        //    CheckYesNoPair(ApplicationValidationYes, ApplicationValidationNo);

        //    // Check for Stub
        //    if (AppSettings.System.BallotStub == 1)
        //    {
        //        // Display Stub Question
        //        StubVerificationPanel.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        // Display Return to search button
        //        NextVerify.Visibility = Visibility.Visible;
        //    }
        //}

        //private void ApplicationPrinterCheckNo_Click(object sender, RoutedEventArgs e)
        //{
        //    // Set No to checked and Yes to Disabled
        //    CheckYesNoPair(ApplicationPrinterCheckNo, ApplicationPrinterCheckYes);

        //    // Display Applicaton Options Panel
        //    ApplicationOptionsPanel.Visibility = Visibility.Visible;
        //}

        private void ReprintApplicationCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck the other box
            TransferVoterCheck.IsChecked = false;

            // Print a new application
            //await Task.Run(() => BallotPrinting.ReprintApplication(_voter));

            // Display new printing message
            //BallotPrintingStatus.Text = "A new Ballot was sent to the Printer";

            // Reset and hide questions
            ClearYesNoPair(ApplicationValidationYes, ApplicationValidationNo);
            //ClearYesNoPair(ApplicationPrinterCheckYes, ApplicationPrinterCheckNo);
            //ApplicationPrinterCheckPanel.Visibility = Visibility.Collapsed;
            ApplicationOptionsPanel.Visibility = Visibility.Collapsed;
            ReprintApplicationCheck.IsChecked = false;
            ApplicationTransferVoterCheck.IsChecked = false;
        }

        private void ApplicationTransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            ReprintApplicationCheck.IsChecked = false;
            // Return to search screen
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new LoginPage());
            }
        }

        private void PermitValidationYes_Click(object sender, RoutedEventArgs e)
        {
            // Set Yes to checked and No to Disabled
            CheckYesNoPair(PermitValidationYes, PermitValidationNo);

            // Check for Stub
            if (AppSettings.System.BallotStub == 1)
            {
                // Display Stub Question
                StubVerificationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                // Display Return to search button
                NextVerify.Visibility = Visibility.Visible;
            }
        }

        private void PermitValidationNo_Click(object sender, RoutedEventArgs e)
        {
            // Set No to checked and Yes to Disabled
            CheckYesNoPair(PermitValidationNo, PermitValidationYes);

            // Display Troubleshooting message & question
            PermitOptionsPanel.Visibility = Visibility.Visible;
        }

        //private void PermitPrinterCheckYes_Click(object sender, RoutedEventArgs e)
        //{
        //    // Set Yes to checked and No to Disabled
        //    CheckYesNoPair(PermitPrinterCheckYes, PermitPrinterCheckNo);

        //    // Reset application question
        //    ClearYesNoPair(PermitValidationYes, PermitValidationNo);
        //    ClearYesNoPair(PermitPrinterCheckYes, PermitPrinterCheckNo);
        //    PermitPrinterCheckPanel.Visibility = Visibility.Collapsed;
        //    PermitOptionsPanel.Visibility = Visibility.Collapsed;
        //    PermitReprintCheck.IsChecked = false;
        //    PermitTransferVoterCheck.IsChecked = false;
        //    CheckYesNoPair(PermitValidationYes, PermitValidationNo);

        //    // Check for Stub
        //    if (AppSettings.System.BallotStub == 1)
        //    {
        //        // Display Stub Question
        //        StubVerificationPanel.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        // Display Return to search button
        //        NextVerify.Visibility = Visibility.Visible;
        //    }
        //}

        //private void PermitPrinterCheckNo_Click(object sender, RoutedEventArgs e)
        //{
        //    // Set No to checked and Yes to Disabled
        //    CheckYesNoPair(PermitPrinterCheckNo, PermitPrinterCheckYes);

        //    // Display Permit Options Panel
        //    PermitOptionsPanel.Visibility = Visibility.Visible;
        //}

        private void PermitReprintCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck the other box
            PermitTransferVoterCheck.IsChecked = false;

            // Print a new application
            //await Task.Run(() => BallotPrinting.ReprintPermit(_voter));

            // Display new printing message
            //BallotPrintingStatus.Text = "A new Ballot was sent to the Printer";

            // Reset and hide questions
            ClearYesNoPair(PermitValidationYes, PermitValidationNo);
            //ClearYesNoPair(PermitPrinterCheckYes, PermitPrinterCheckNo);
            //PermitPrinterCheckPanel.Visibility = Visibility.Collapsed;
            PermitOptionsPanel.Visibility = Visibility.Collapsed;
            PermitReprintCheck.IsChecked = false;
            PermitTransferVoterCheck.IsChecked = false;
        }

        private void PermitTransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            ReprintApplicationCheck.IsChecked = false;
            // Return to search screen
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new LoginPage());
            }
        }

        private void StubValidationYes_Click(object sender, RoutedEventArgs e)
        {
            // Set Yes to checked and No to Disabled
            CheckYesNoPair(StubValidationYes, StubValidationNo);
            
            // Display Return to search button
            NextVerify.Visibility = Visibility.Visible;
        }

        private void StubValidationNo_Click(object sender, RoutedEventArgs e)
        {
            // Set No to checked and Yes to Disabled
            CheckYesNoPair(StubValidationNo, StubValidationYes);

            // Display Troubleshooting message & question
            StubOptionsPanel.Visibility = Visibility.Visible;
        }

        //private void StubPrinterCheckYes_Click(object sender, RoutedEventArgs e)
        //{
        //    // Set Yes to checked and No to Disabled
        //    CheckYesNoPair(StubPrinterCheckYes, StubPrinterCheckNo);

        //    // Reset application question
        //    //ClearYesNoPair(PermitValidationYes, PermitValidationNo);
        //    //ClearYesNoPair(PermitPrinterCheckYes, PermitPrinterCheckNo);
        //    //PermitPrinterCheckPanel.Visibility = Visibility.Collapsed;
        //    //PermitOptionsPanel.Visibility = Visibility.Collapsed;
        //    //PermitReprintCheck.IsChecked = false;
        //    //PermitTransferVoterCheck.IsChecked = false;
        //    //CheckYesNoPair(PermitValidationYes, PermitValidationNo);
            
        //    // Display Return to search button
        //    NextVerify.Visibility = Visibility.Visible;
            
        //}

        //private void StubPrinterCheckNo_Click(object sender, RoutedEventArgs e)
        //{
        //    // Set No to checked and Yes to Disabled
        //    CheckYesNoPair(StubPrinterCheckNo, StubPrinterCheckYes);

        //    // Display Permit Options Panel
        //    StubOptionsPanel.Visibility = Visibility.Visible;
        //}

        private void StubReprintCheck_Click(object sender, RoutedEventArgs e)
        {
            // Uncheck the other box
            StubTransferVoterCheck.IsChecked = false;

            // Print a new application
            //await Task.Run(() => BallotPrinting.ReprintStub(_voter));

            // Display new printing message
            //BallotPrintingStatus.Text = "A new Ballot was sent to the Printer";

            // Reset and hide questions
            ClearYesNoPair(StubValidationYes, StubValidationNo);
            //ClearYesNoPair(StubPrinterCheckYes, StubPrinterCheckNo);
            //StubPrinterCheckPanel.Visibility = Visibility.Collapsed;
            StubOptionsPanel.Visibility = Visibility.Collapsed;
            StubReprintCheck.IsChecked = false;
            StubTransferVoterCheck.IsChecked = false;
        }

        private void StubTransferVoterCheck_Click(object sender, RoutedEventArgs e)
        {
            ReprintApplicationCheck.IsChecked = false;
            // Return to search screen
            if (exitLogout == false)
            {
                this.NavigateToPage(new SearchPage());
            }
            else
            {
                this.NavigateToPage(new LoginPage());
            }
        }
    }
}
