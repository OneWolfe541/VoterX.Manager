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
    public partial class SystemSettingsMenu : Page
    {
        private SettingsPage _settingsPage;

        public SystemSettingsMenu(SettingsPage page)
        {
            InitializeComponent();

            _settingsPage = page;
        }

        private void SaveMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            _settingsPage.SaveSettings_Click(sender, e);
        }

        private void LogoutMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            _settingsPage.BackButton_Click(sender, e);
        }

        private void AbsenteeMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            _settingsPage.AbsenteeToggle_Click(sender, e);
        }

        private void SystemMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            _settingsPage.SystemToggle_Click(sender, e);
        }

        private void NetworkMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            _settingsPage.NetworkToggle_Click(sender, e);
        }

        private void PrintersMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            _settingsPage.PrinterToggle_Click(sender, e);
        }

        private void ElectionMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            _settingsPage.ElectionToggle_Click(sender, e);
        }

        private void BallotsMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMenuMethods.CloseMainMenu();
            GlobalReferences.MenuSlider.Close();

            _settingsPage.BallotsToggle_Click(sender, e);
        }
    }
}
