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
    /// Interaction logic for SystemPage.xaml
    /// </summary>
    public partial class ApplicationPage : SettingsBasePage
    {
        public bool pageLoaded = false;

        public ApplicationPage()
        {
            InitializeComponent();

            DisplaySettings();

            //LoadPollSites();

            pageLoaded = true;
        }

        // Need to bind this data instead of manualy stuffing it here
        private void DisplaySettings()
        {
            // APPLICATION SETTINGS //

            ReturnCountyName.Text = AppSettings.Absentee.CountyName;
            ReturnClerksName.Text = AppSettings.Absentee.ClerkName;

            ReturnAddressOne.Text = AppSettings.Absentee.ApplicationReturnAddress1;
            ReturnAddressTwo.Text = AppSettings.Absentee.ApplicationReturnAddress2;
            ReturnCity.Text = AppSettings.Absentee.ApplicationReturnCity;
            ReturnState.Text = AppSettings.Absentee.ApplicationReturnState;
            ReturnZip.Text = AppSettings.Absentee.ApplicationReturnZip;

            BallotReturnAddressOne.Text = AppSettings.Absentee.BallotReturnAddress1;
            BallotReturnAddressTwo.Text = AppSettings.Absentee.BallotReturnAddress2;
            BallotReturnCity.Text = AppSettings.Absentee.BallotReturnCity;
            BallotReturnState.Text = AppSettings.Absentee.BallotReturnState;
            BallotReturnZip.Text = AppSettings.Absentee.BallotReturnZip;
        }

        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/df93d685-844f-47a1-bbbc-454e196369a8/raise-an-event-in-a-child-page-hosted-in-a-frame-to-refresh-a-listbox-on-the-page-hosting-the?forum=wpf
        public override void SaveSettings()
        {
            // Boolean values will probably not be set on this page

            // APPLICATION SETTINGS //
            AppSettings.Absentee.CountyName = ReturnCountyName.Text;
            AppSettings.Absentee.ClerkName = ReturnClerksName.Text;

            AppSettings.Absentee.ApplicationReturnAddress1 = ReturnAddressOne.Text;
            AppSettings.Absentee.ApplicationReturnAddress2 = ReturnAddressTwo.Text;
            AppSettings.Absentee.ApplicationReturnCity = ReturnCity.Text;
            AppSettings.Absentee.ApplicationReturnState = ReturnState.Text;
            AppSettings.Absentee.ApplicationReturnZip = ReturnZip.Text;

            AppSettings.Absentee.BallotReturnAddress1 = BallotReturnAddressOne.Text;
            AppSettings.Absentee.BallotReturnAddress2 = BallotReturnAddressTwo.Text;
            AppSettings.Absentee.BallotReturnCity = BallotReturnCity.Text;
            AppSettings.Absentee.BallotReturnState = BallotReturnState.Text;
            AppSettings.Absentee.BallotReturnZip = BallotReturnZip.Text;
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