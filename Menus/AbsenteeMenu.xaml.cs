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
using System.Diagnostics;
using System.IO;
using VoterX.Extensions;
using VoterX.Manager.Methods;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeMenu.xaml
    /// </summary>
    public partial class AbsenteeMenu : Page
    {
        public AbsenteeMenu()
        {
            InitializeComponent();
        }

        private void SystemSettings_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new SettingsPage());
        }

        private void VoterLookupMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new SearchPage());
            NavigationMenuMethods.VoterSearchPage();
        }

        private void SERVISLookupMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new SearchPage(0));
            NavigationMenuMethods.VoterSearchPage(0);
        }

        private void ReportsMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new AbsenteeReportPage());
            NavigationMenuMethods.AbsenteeReportsPage();
        }

        private void BatchMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeBatchManagerPage());
        }

        private void ScanReturnsMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeScanOptionsPage());
        }

        private void AddVoterMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AddProvisionalPage());
        }

        private void AvRegisterMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new AbsenteeRegisterPage());
            NavigationMenuMethods.Register();
        }

        private void BatchPrintingMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new BatchBallotPrintingPage());
            NavigationMenuMethods.BatchPrinting();
        }

        private void SERVISBatchMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new BatchBallotPrintingPage(0));
            NavigationMenuMethods.BatchPrinting(0);
        }

        private void ReportWizardMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new AbsenteeReportWizardDatePage());
            NavigationMenuMethods.ReportWizardDates();
        }

        private void TestDeckMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeTestDeckPage());
        }

        private void ClosePolls_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new EndofDayPage());
        }

        private void MainMenuHomeButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.AnimateHome();
            GlobalReferences.MenuSlider.Open();

            MainFrameMethods.NavigateToPage(new AbsenteeAdministrationPage());
        }

        //private void TestMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    string filePath = "C:\\VoterXSync\\AVReconcile.hta";
        //    if (File.Exists(filePath))
        //    {
        //        Process.Start(filePath);
        //    }
        //    else
        //    {
        //        StatusBar.ApplicationStatusCenter("File Not Found");
        //    }
        //}
    }
}
