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
    public partial class AllMailManageMenu : Page
    {
        public AllMailManageMenu()
        {
            InitializeComponent();
        }

        private void EmergencyBallotMenu_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new EmergencyBallotPage());
        }

        private void MainMenuHomeButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.AnimateHome();
            GlobalReferences.MenuSlider.Open();

            MainFrameMethods.NavigateToPage(new AdministrationPage());
        }

        private void ManualRecordUpdate_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            MainFrameMethods.NavigateToPage(new ManualRecordUpdate());
        }
    }
}
