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
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeRegisterPage.xaml
    /// </summary>
    public partial class AbsenteeRegisterPage : Page
    {
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now;

        public AbsenteeRegisterPage()
        {
            InitializeComponent();

            // Display page header
            GlobalReferences.Header.PageHeader = ("Voter Register");

            GlobalReferences.StatusBar.CheckPrinterStatus(AppSettings.Printers.BallotPrinter);

            CheckServer();

            BeginningDatePicker.SelectedDate = DateTime.Now.AddYears(-1);
        }

        private void CheckServer()
        {
            //PrintOptionsPanel.Visibility = Visibility.Collapsed;

            //if (await StatusBar.CheckServer() == true)
            //{
            //    PrintOptionsPanel.Visibility = Visibility.Visible;
            //}
        }

        private void AbsenteeRegister_Click(object sender, RoutedEventArgs e)
        {
            PrintAbsenteeRegister();
        }

        private void EarlyVotingRegister_Click(object sender, RoutedEventArgs e)
        {
            PrintEarlyRegister();
        }

        private void CombinedRegister_Click(object sender, RoutedEventArgs e)
        {
            PrintCombinedRegister();
        }

        private void PrintAbsenteeRegister()
        {
            CalculateNewDateRange();

            GlobalReferences.StatusBar.TextCenter = (
            ReportPrintingMethods.PrintAbsenteeRegister(startDate, endDate)
            );
        }

        private void PrintEarlyRegister()
        {
            CalculateNewDateRange();

            GlobalReferences.StatusBar.TextCenter = (
            ReportPrintingMethods.PrintEarlyVotingRegister(startDate, endDate)
            );
        }

        private void PrintCombinedRegister()
        {
            CalculateNewDateRange();

            GlobalReferences.StatusBar.TextCenter = (
            ReportPrintingMethods.PrintCombinedRegister(startDate, endDate)
            );
        }

        private void ClearDateRange_Click(object sender, RoutedEventArgs e)
        {
            //BeginingDate.Text = "";
            BeginningDatePicker.SelectedDate = DateTime.Now.AddYears(-1);
            BeginingTime.Text = "";

            EndingDatePicker.SelectedDate = DateTime.Now;
            EndingTime.Text = "";

            CurrentRange.Text = "";
        }

        private void CalculateDateRange_Click(object sender, RoutedEventArgs e)
        {
            CalculateNewDateRange();
        }

        private void CalculateNewDateRange()
        {
            GlobalReferences.StatusBar.TextCenter = ("");

            string startString = "";
            string endString = "";

            // Get Beginning Date and Time
            if (BeginningDatePicker.SelectedDate == null && BeginingTime.Text == "")
            {
                startString = DateTime.Now.AddYears(-1).ToShortDateString();
            }
            else
            {
                if (BeginningDatePicker.SelectedDate == null) BeginningDatePicker.SelectedDate = DateTime.Now.AddYears(-1);
                if (TimeSpan.TryParse(BeginingTime.Text, out TimeSpan dummyOutput) == false) BeginingTime.Text = "";
                startString = ((DateTime)BeginningDatePicker.SelectedDate).ToShortDateString() + " " + BeginingTime.Text.ToUpper();
            }


            // Get Ending Tate and Time
            if (EndingDatePicker.SelectedDate == null && EndingTime.Text == "")
            {
                endString = DateTime.Now.ToShortDateString() + " 11:59 PM";
            }
            else
            {
                if (EndingDatePicker.SelectedDate == null) EndingDatePicker.SelectedDate = DateTime.Now;

                if (EndingTime.Text == "")
                {
                    endString = ((DateTime)EndingDatePicker.SelectedDate).ToShortDateString() + " 11:59 PM";
                }
                else
                {
                    if (TimeSpan.TryParse(EndingTime.Text, out TimeSpan dummyOutput) == false) EndingTime.Text = "";
                    startString = ((DateTime)EndingDatePicker.SelectedDate).ToShortDateString() + " " + EndingTime.Text.ToUpper();
                }
            }

            if (DateTime.TryParse(startString.TrimEnd(), out startDate) == false)
            {
                AlertDialog newMessage = new AlertDialog("INVALID DATE OR TIME");
                newMessage.ShowDialog();
                GlobalReferences.StatusBar.TextCenter = ("Invalid Date or Time");
            }
            else
            {
                if (DateTime.TryParse(endString.TrimEnd(), out endDate) == false)
                {
                    AlertDialog newMessage = new AlertDialog("INVALID DATE OR TIME");
                    newMessage.ShowDialog();
                    GlobalReferences.StatusBar.TextCenter = ("Invalid Date or Time");
                }
                else
                {
                    CurrentRange.Text = startDate.ToString() + " - " + endDate.ToString();
                }
            }
        }

        private void PreviewCombinedRegister_Click(object sender, RoutedEventArgs e)
        {
            PreviewCombinedRegister();
        }

        private void PreviewAbsenteeRegister_Click(object sender, RoutedEventArgs e)
        {
            PreviewAbsenteeRegister();
        }

        private void PreviewEarlyVotingRegister_Click(object sender, RoutedEventArgs e)
        {
            PreviewEarlyRegister();
        }

        private void PreviewAbsenteeRegister()
        {
            CalculateNewDateRange();

            ReportPrintingMethods.PreviewAbsenteeRegister(startDate, endDate);
        }

        private void PreviewEarlyRegister()
        {
            CalculateNewDateRange();
            
            ReportPrintingMethods.PreviewEarlyVotingRegister(startDate, endDate);
        }

        private void PreviewCombinedRegister()
        {
            CalculateNewDateRange();

            ReportPrintingMethods.PreviewCombinedRegister(startDate, endDate);
        }
    }
}


