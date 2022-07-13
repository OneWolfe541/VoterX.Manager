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
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeMenu.xaml
    /// </summary>
    public partial class AllMailMenu : Page
    {
        public AllMailMenu()
        {
            InitializeComponent();
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

            MainFrameMethods.NavigateToPage(new AbsenteeReportPage());
        }

        private void ScanReturnsMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeScanOptionsPage());
        }

        private void ReportWizardMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeReportWizardDatePage());
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
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new AbsenteeAdministrationPage());
        }
    }
}
