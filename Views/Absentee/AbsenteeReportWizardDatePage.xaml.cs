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
using VoterX.Dialogs;
using VoterX.Methods;
using VoterX.Models;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeReportWizardDatePage.xaml
    /// </summary>
    public partial class AbsenteeReportWizardDatePage : Page
    {
        private ReportWizardQueryModel _wizardSearch;

        public AbsenteeReportWizardDatePage()
        {
            InitializeComponent();

            _wizardSearch = new ReportWizardQueryModel();

            GlobalReferences.Header.PageHeader = ("Report Wizard");

            GlobalReferences.StatusBar.TextClear();

            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            CheckServer();
        }

        public AbsenteeReportWizardDatePage(ReportWizardQueryModel searchParameters)
        {
            InitializeComponent();

            _wizardSearch = searchParameters;
            //StatusBar.ApplicationStatus(_wizardSearch.BeginningDate.ToString() + " " + _wizardSearch.EndingDate.ToString());

            GlobalReferences.Header.PageHeader = ("Report Wizard");

            //StatusBar.ApplicationStatus("");

            // Set default dates and times
            BeginningDatePicker.SelectedDate = _wizardSearch.BeginningDate;
            BeginingTime.Text = _wizardSearch.BeginningDate.ToShortTimeString();
            EndingDatePicker.SelectedDate = _wizardSearch.EndingDate;
            EndingTime.Text = _wizardSearch.EndingDate.ToShortTimeString();

            GlobalReferences.StatusBar.CheckPrinterStatusAsync(AppSettings.Printers.BallotPrinter);

            CheckServer();
        }

        private void CheckServer()
        {
            //NextWizardButton.Visibility = Visibility.Collapsed;

            //if (await StatusBar.CheckServer() == true)
            //{
            //    NextWizardButton.Visibility = Visibility.Visible;
            //}
        }

        private void ClearDateRange_Click(object sender, RoutedEventArgs e)
        {
            //BeginingDate.Text = "";
            BeginningDatePicker.SelectedDate = DateTime.Now;
            BeginingTime.Text = "";

            //EndingDate.Text = "";
            EndingDatePicker.SelectedDate = DateTime.Now;
            EndingTime.Text = "";

            //CurrentRange.Text = "";
        }

        private void CalculateDateRange_Click(object sender, RoutedEventArgs e)
        {
            CalculateNewDateRange();
        }

        private void CalculateNewDateRange()
        {
            string startString = "";
            string endString = "";

            DateTime startDate;
            DateTime endDate;

            try
            {
                // Get Beginning Date and Time
                if (BeginningDatePicker.SelectedDate == null && BeginingTime.Text == "")
                {
                    startString = DateTime.Now.ToShortDateString();
                }
                else
                {
                    startString = ((DateTime)BeginningDatePicker.SelectedDate).ToShortDateString() + " " + BeginingTime.Text.ToUpper();
                }
            }
            catch
            {
                startString = DateTime.Now.ToShortDateString();
            }

            try
            {
                // Get Ending Tate and Time
                if (EndingDatePicker.SelectedDate == null && EndingTime.Text == "")
                {
                    endString = DateTime.Now.ToShortDateString() + " 11:59 PM";
                }
                else
                {
                    if (EndingTime.Text == "")
                    {
                        endString = ((DateTime)EndingDatePicker.SelectedDate).ToShortDateString() + " 11:59 PM";
                    }
                    else
                    {
                        endString = ((DateTime)EndingDatePicker.SelectedDate).ToShortDateString() + " " + EndingTime.Text.ToUpper();
                    }
                }
            }
            catch
            {
                endString = DateTime.Now.ToShortDateString() + " 11:59 PM";
            }

            if (DateTime.TryParse(startString.TrimEnd(), out startDate) == false)
            {
                AlertDialog newMessage = new AlertDialog("INVALID BEGINNING DATE OR TIME");
                newMessage.ShowDialog();
                GlobalReferences.StatusBar.TextCenter = ("Invalid Beginning Date or Time");
            }
            else
            {
                if (DateTime.TryParse(endString.TrimEnd(), out endDate) == false)
                {
                    AlertDialog newMessage = new AlertDialog("INVALID ENDING DATE OR TIME");
                    newMessage.ShowDialog();
                    GlobalReferences.StatusBar.TextCenter = ("Invalid Ending Date or Time");
                }
                else
                {
                    _wizardSearch.BeginningDate = startDate;
                    _wizardSearch.EndingDate = endDate;
                    CurrentRange.Text = startDate.ToString() + " - " + endDate.ToString();
                }
            }
        }

        private void NextWizardButton_Click(object sender, RoutedEventArgs e)
        {
            CalculateNewDateRange();

            GlobalReferences.StatusBar.TextLeft = (_wizardSearch.BeginningDate.ToString() + " " + _wizardSearch.EndingDate.ToString());

            MainFrameMethods.NavigateToPage(new AbsenteeReportWizardPage(_wizardSearch));
        }
    }
}
