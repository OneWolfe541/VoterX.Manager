using VoterX.Core.Voters;
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
using System.Windows.Shapes;

namespace VoterX.Dialogs
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class AddressSelectionDialog : Window
    {
        public int Selection;

        public AddressSelectionDialog(VoterDataModel voter, int type)
        {
            InitializeComponent();
            //lblmessage.Content = message;

            //this.Icon = FontAwesomeIcon.Exclamation;
            // Get icon images from https://paulferrett.com/fontawesome-favicon/
            //Uri iconUri = new Uri("pack://application:,,,/Images/favicon-exclamation.ico");
            //this.Icon = BitmapFrame.Create(iconUri);

            //Console.WriteLine("Alert Opened: " + lblmessage.Content.ToString());

            if (type == 1)
            {
                lblmessage.Content = "SELECT WHICH ADDRESS TO SEND THE BALLOT TO:";
            }
            else if(type == 2)
            {
                lblmessage.Content = "SELECT WHICH ADDRESS TO SEND THE APPLICATION TO:";
            }
            else if (type == 3)
            {
                lblmessage.Content = "SELECT WHICH ADDRESS TO SEND THE ENVELOPE TO:";
            }

            if (voter.MailingAddress1 != null && voter.MailingAddress1 != "")
            {
                // Display Mailing address
                if (voter.MailingAddress2 != null && voter.MailingAddress2 != "")
                {
                    MailAddress.Text = (voter.MailingAddress1 + ", " + voter.MailingAddress2).Trim();
                }
                else
                {
                    MailAddress.Text = (voter.MailingAddress1).Trim();
                }
                MailCityStateAndZip.Text = voter.MailingCity + " " + voter.MailingState + " " + voter.MailingZip + " " + voter.MailingCountry;
            }
            else
            {
                MailingAddressCheck.Visibility = Visibility.Collapsed;
            }

            // Display Registered address
            if (voter.Address2 != null && voter.Address2 != "")
            {
                RegisteredAddress.Text = (voter.Address1 + ", " + voter.Address2).Trim();
            }
            else
            {
                RegisteredAddress.Text = (voter.Address1).Trim();
            }
            RegisteredCityStateAndZip.Text = voter.City + " " + voter.State + " " + voter.Zip;

            if (voter.TempAddress1 != null && voter.TempAddress1 != "")
            {
                // Display Temp address
                if (voter.TempAddress2 != null && voter.TempAddress2 != "")
                {
                    TempAddress.Text = (voter.TempAddress1 + ", " + voter.TempAddress2).Trim();
                }
                else
                {
                    TempAddress.Text = (voter.TempAddress1).Trim();
                }
                TempCityStateAndZip.Text = voter.TempCity + " " + voter.TempState + " " + voter.TempZip + " " + voter.TempCountry;
            }
            else
            {
                TempAddressCheck.Visibility = Visibility.Collapsed;
            }
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (MailAddressSelect.IsChecked == true)
            {
                if (OutOfCountrySelect.IsChecked == false)
                {
                    Selection = 1;
                }
                else
                {
                    Selection = 4;
                }
            }

            if (RegisteredAddressSelect.IsChecked == true)
            {
                if (OutOfCountrySelect.IsChecked == false)
                {
                    Selection = 2;
                }
                else
                {
                    Selection = 5;
                }
            }

            if (TempAddressSelect.IsChecked == true)
            {
                if (OutOfCountrySelect.IsChecked == false)
                {
                    Selection = 3;
                }
                else
                {
                    Selection = 6;
                }
            }

            this.DialogResult = true;
        }

        private void MailAddressSelect_Click(object sender, RoutedEventArgs e)
        {
            if (MailAddressSelect.IsChecked == true)
            {
                RegisteredAddressSelect.IsChecked = false;
                TempAddressSelect.IsChecked = false;

                //btnDialogOk.Visibility = Visibility.Visible;
                btnDialogOk.IsEnabled = true;
            }
            else
            {
                //btnDialogOk.Visibility = Visibility.Collapsed;
                btnDialogOk.IsEnabled = false;
            }
        }

        private void RegisteredAddressSelect_Click(object sender, RoutedEventArgs e)
        {
            if (RegisteredAddressSelect.IsChecked == true)
            {
                MailAddressSelect.IsChecked = false;
                TempAddressSelect.IsChecked = false;

                //btnDialogOk.Visibility = Visibility.Visible;
                btnDialogOk.IsEnabled = true;
            }
            else
            {
                //btnDialogOk.Visibility = Visibility.Collapsed;
                btnDialogOk.IsEnabled = false;
            }
        }

        private void TempAddressSelect_Click(object sender, RoutedEventArgs e)
        {
            if (TempAddressSelect.IsChecked == true)
            {
                RegisteredAddressSelect.IsChecked = false;
                MailAddressSelect.IsChecked = false;

                //btnDialogOk.Visibility = Visibility.Visible;
                btnDialogOk.IsEnabled = true;
            }
            else
            {
                //btnDialogOk.Visibility = Visibility.Collapsed;
                btnDialogOk.IsEnabled = false;
            }
        }
    }
}
