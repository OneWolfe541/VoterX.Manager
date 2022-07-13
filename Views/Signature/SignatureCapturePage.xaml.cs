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
//using VoterX.Core.Models.ViewModels;
using eSign3;
using VoterX.Methods;
using VoterX.Extensions;
using VoterX.Dialogs;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for SignatureCapturePage.xaml
    /// </summary>
    public partial class SignatureCapturePage : Page
    {
        //private VoterDataModel _voter = new VoterDataModel();

        // Get the current system's polling location ID
        int myLocation = Int32.Parse(AppSettings.System.SiteID.ToString());

        private bool exitLogout = false;

        //private eSign3.esCapture epad = new esCapture();

        // Constructor for the current page
        //public SignatureCapturePage(VoterDataModel voter)
        //{
        //    InitializeComponent();

        //    //LoggingMethods.LogPage("SIGNATURE PAGE LOADED");

        //    // Set the local voter object
        //    _voter = voter;

        //    // Display voter details on the page
        //    LoadVoterFields(voter);

        //    // Display any existing signature on the page
        //    LoadSignatureImage(voter);

        //    //StatusBar.ApplicationStatus("Signature Page Loaded");
        //    StatusBar.ApplicationPageHeader("Signature Capture");

        //    StatusBar.ApplicationCheckSignaturePad();
        //}

        //public SignatureCapturePage(VoterDataModel voter, bool returnStatus)
        //{
        //    InitializeComponent();

        //    //LoggingMethods.LogPage("SIGNATURE PAGE LOADED");

        //    // Set the local voter object
        //    _voter = voter;
        //    exitLogout = returnStatus;
        //    if (returnStatus == true)
        //    {
        //        BackReturnLabel.Visibility = Visibility.Collapsed;
        //        BackLogoutLabel.Visibility = Visibility.Visible;
        //    }

        //    // Display voter details on the page
        //    LoadVoterFields(voter);

        //    // Display any existing signature on the page
        //    LoadSignatureImage(voter);

        //    //StatusBar.ApplicationStatus("Signature Page Loaded");
        //    StatusBar.ApplicationPageHeader("Signature Capture");

        //    StatusBar.ApplicationCheckSignaturePad();
        //}

        // Return to the voter look up page
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogUser("RETURNING TO SEARCH PAGE");
            if (exitLogout == true)
            {
                //this.NavigateToPage(new EditBallotSearchPage(null));
            }
            else
            {
                this.NavigateToPage(new SearchPage());
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogPage("SIGNATURE PAGE UNLOADED");
        }

        // Load the voter details into TextBlocks on the page
        // I should consider making the voter details into a separate user control (Same with the signature pad)
        //private void LoadVoterFields(VoterDataModel voter)
        //{
        //    FullName.Text = voter.FullName;
        //    BirthYear.Text = voter.DOBYear;
        //    Address.Text = voter.Address1;
        //    CityStateAndZip.Text = voter.City + ", " + voter.State + " " + voter.Zip;
        //}

        // Load a new file into the image control
        //private void LoadSignatureImage(VoterDataModel voter)
        //{
        //    //LoggingMethods.LogIO("LOAD SIGNATURE IMAGE");
        //    VoterSignature.Source = null;
        //    VoterSignature.Source = SignatureMethods.LoadSignatureFromFile(voter);

        //    if (VoterSignature.Source == null)
        //    {
        //        //StatusBar.ApplicationStatus("No Signature File Found");
        //    }
        //    else
        //    {
        //        //StatusBar.ApplicationStatus("Signature File Found");
        //        PrintBallot.Visibility = Visibility.Visible;
        //    }
        //}

        //// Clear the image and delete the existing file
        //private void DeleteExistingFile(VoterDataModel voter)
        //{
        //    //LoggingMethods.LogIO("DELETE SIGNATURE IMAGE");

        //    // Clear the image control
        //    VoterSignature.Source = null;

        //    // Display which file is being deleted
        //    //StatusBar.ApplicationStatus("Searching For: " + voter.VoterID.ToString());

        //    if (SignatureMethods.DeleteVoterSignature(voter))
        //    {
        //        //StatusBar.ApplicationStatus("File Deleted: " + voter.VoterID.ToString());
        //    }
        //    else
        //    {
        //        //StatusBar.ApplicationStatus("File not found: " + voter.VoterID.ToString());
        //    }
        //}        

        //// Turn on the signature pad and wait for voter to sign or cancel
        private void EnablePadButton_Click(object sender, RoutedEventArgs e)
        {
            ////LoggingMethods.LogUser("SIGNATURE STARTED");
            ////StatusBar.ApplicationStatus("Signature Started");

            //// Delete existing signature file
            //DeleteExistingFile(_voter);

            //// Set affirmation and voter strings
            //string affText = "I, " + _voter.FirstName + " " + _voter.LastName + " confirm that I am a Regsitered Voter and to my knowledge have not cast a ballot in this election.";
            //string userText = _voter.FirstName + " " + _voter.LastName + " Party: " + _voter.Party + " Birth Year: " + _voter.DOBSearch;

            //// Save voter signature from sigPad then display image on the page
            //if (SignatureMethods.SaveSignatureFromPad(_voter, myLocation, AppSettings.System.SiteName, affText, userText))
            //    LoadSignatureImage(_voter);
        }

        // Clear the image and delete the existing file
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ValidationDialog passwordDialog = new ValidationDialog("Administrator");
            if (passwordDialog.ShowDialog() == true)
            {
                PrintBallot.Visibility = Visibility.Collapsed;
                //DeleteExistingFile(_voter);
            }
        }

        private void CheckDevicesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintBallot_Click(object sender, RoutedEventArgs e)
        {
            //LoggingMethods.LogUser("NAVIGATE TO OFFICIAL BALLOT PAGE");

            // Goto the navigation page
            if (exitLogout == true)
            {
                //this.NavigateToPage(new PrintBundlePage(_voter, true));
            }
            else
            {
                //this.NavigateToPage(new PrintBundlePage(_voter));
            }
        }

        // Turns On or Off the print button
        // Sets the Sign Refused field in the voter record
        private void SignRefused_Click(object sender, RoutedEventArgs e)
        {
            //StatusBar.ApplicationStatus("Sign Refused Clicked: " + SignRefused.IsChecked.ToString());

            //if (SignRefused.IsChecked == true)
            //{
            //    AlertDialog signatureDialog = new AlertDialog("MAKE SURE THE VOTER SIGNS THE PRINTED APPLICATION");
            //    if (signatureDialog.ShowDialog() == true)
            //    { }
            //}

            // If no image exists turn on or off the print button
            if (VoterSignature.Source == null)
            {
                if (SignRefused.IsChecked == true)
                {
                    PrintBallot.Visibility = Visibility.Visible;
                }
                else
                {
                    PrintBallot.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                // Display warning message
                RefuseVerificationPanel.Visibility = Visibility.Visible;
            }

            // Regardless if an image exists or not then set the sign refused field
            if (SignRefused.IsChecked == true)
            {
                //_voter.SignRefused = true;
            }
            else
            {
                //_voter.SignRefused = false;
            }
            
        }

        private void RefusedSignatureYes_Click(object sender, RoutedEventArgs e)
        {
            // Turn on Yes button
            RefusedSignatureYes.IsChecked = true;
            refuse_fa_check_yes.Visibility = Visibility.Visible;

            // Turn off the No button
            refuse_fa_check_no.Visibility = Visibility.Collapsed;
            RefusedSignatureNo.IsChecked = false;

            // Set sign refused to true
            SignRefused.IsChecked = true;
            //_voter.SignRefused = true;

            // Delete image file
            ValidationDialog passwordDialog = new ValidationDialog("Administrator");
            if (passwordDialog.ShowDialog() == true)
            {
                //PrintBallot.Visibility = Visibility.Collapsed;
                //DeleteExistingFile(_voter);
            }
        }

        private void RefusedSignatureNo_Click(object sender, RoutedEventArgs e)
        {
            // Turn on No button
            RefusedSignatureNo.IsChecked = true;
            refuse_fa_check_no.Visibility = Visibility.Visible;

            // Turn off the Yes button
            refuse_fa_check_yes.Visibility = Visibility.Collapsed;
            RefusedSignatureYes.IsChecked = false;

            // Set sign refused to false
            SignRefused.IsChecked = false;
            //_voter.SignRefused = false;

            // Hide "Are you sure" question
            RefuseVerificationPanel.Visibility = Visibility.Collapsed;
            // Turn off the No button
            refuse_fa_check_no.Visibility = Visibility.Collapsed;
            RefusedSignatureNo.IsChecked = false;
        }
    }
}
