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
    public partial class ManageMenu : Page
    {
        public ManageMenu()
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

            MainFrameMethods.NavigateToPage(new SearchPage());
        }

        private void ReportsMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new ReportsPage());
            NavigationMenuMethods.EarlyVotingReportsPage();
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

            //MainFrameMethods.NavigateToPage(new AddProvisionalPage());
            NavigationMenuMethods.AddProvisional();
        }

        private void AvRegisterMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeRegisterPage());
        }

        private void BatchPrintingMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new BatchBallotPrintingPage());
        }

        private void ReportWizardMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeReportWizardDatePage());
        }

        private void TestDeckMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeTestDeckPage());
        }

        private void EmergencyBallotMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new EmergencyBallotPage());
            NavigationMenuMethods.EmergencyBallots();
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
            //GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AdministrationPage());
        }

        private void EditVoterMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new EditBallotSearchPage());
            NavigationMenuMethods.EditBallotStyle();
        }

        private void AbsenteeSettings_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new ApplicationSettingsPage());
        }

        private void ManualRecordUpdate_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            //MainFrameMethods.NavigateToPage(new ManualRecordUpdate());
            NavigationMenuMethods.ManualRecordUpdate();
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
