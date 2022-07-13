using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VoterX;
using VoterX.SystemSettings;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Controls;
using VoterX.Utilities.Interfaces;
using VoterX.Utilities.Models;
using VoterX.Manager.Factories;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Menus
{
    public class SettingsMenuViewModel : IMenuViewModel
    {
        private SettingsPage _settingsPage;

        public SettingsMenuViewModel(SettingsPage page)
        {
            _settingsPage = page;

            HomePanelVisibility = true;
            CenterPanelVisibility = true;
            ExitPanelVisibility = true;
        }

        public bool HomePanelVisibility { get; set; }
        public bool CenterPanelVisibility { get; set; }
        public bool ExitPanelVisibility { get; set; }

        private ObservableCollection<Control> _homeCustomControls;
        public ObservableCollection<Control> HomeCustomControls
        {
            get
            {
                if (_homeCustomControls == null)
                {
                    
                }
                return _homeCustomControls;
            }

            private set { _homeCustomControls = value; }
        }

        private ObservableCollection<Control> _centerCustomControls;
        public ObservableCollection<Control> CenterCustomControls
        {
            get
            {
                if (_centerCustomControls == null)
                {
                    // Create factory
                    MenuButtonFactory buttonFactory = new MenuButtonFactory();

                    _centerCustomControls = new ObservableCollection<Control>();

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.AddressBookOutline,
                            "ABSENTEE",
                            "ABSENTEE",
                            new Thickness(0, 5, 0, 0),
                            param => NavigateToAbsentee()
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                        FontAwesome.WPF.FontAwesomeIcon.Gear,
                        "SYSTEM",
                        "SYSTEM",
                        new Thickness(0, 25, 0, 0),
                        param => NavigateToSystem()
                    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                        FontAwesome.WPF.FontAwesomeIcon.Clipboard,
                        "NETWORK",
                        "NETWORK",
                        new Thickness(0, 25, 0, 0),
                        param => NavigateToNetwork()
                    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                        FontAwesome.WPF.FontAwesomeIcon.Print,
                        "PRINTERS",
                        "PRINTERS",
                        new Thickness(0, 25, 0, 0),
                        param => NavigateToPrinters()
                    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                        FontAwesome.WPF.FontAwesomeIcon.CheckSquareOutline,
                        "ELECTION",
                        "ELECTION",
                        new Thickness(0, 25, 0, 0),
                        param => NavigateToElection()
                    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                        FontAwesome.WPF.FontAwesomeIcon.FilePdfOutline,
                        "BALLOTS",
                        "BALLOTS",
                        new Thickness(0, 25, 0, 0),
                        param => NavigateToBallots()
                    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                        FontAwesome.WPF.FontAwesomeIcon.PencilSquareOutline,
                        "EDIT VOTER",
                        "EDIT VOTER",
                        new Thickness(0, 65, 0, 0),
                        param => NavigationMenuMethods.SuperSearchPage()
                    ));

                }
                return _centerCustomControls;
            }
        }

        private ObservableCollection<Control> _exitCustomControls;
        public ObservableCollection<Control> ExitCustomControls
        {
            get
            {
                if (_exitCustomControls == null)
                {
                    // Create factory
                    MenuButtonFactory buttonFactory = new MenuButtonFactory();

                    _exitCustomControls = new ObservableCollection<Control>();

                    _exitCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.Save,
                            "SAVE",
                            "SAVE",
                            new Thickness(0, 0, 0, 25),
                            param => _settingsPage.SaveSettings_Click(new object(), new RoutedEventArgs())
                        ));

                    _exitCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.SignOut,
                            "LOG OUT",
                            "LOG OUT",
                            new Thickness(0, 0, 0, 5),
                            param => NavigateToLogin()
                        ));
                }
                return _exitCustomControls;
            }
        }

        private void NavigateToAbsentee()
        {
            GlobalReferences.MenuSlider.Close();

            _settingsPage.AbsenteePage();
        }

        private void NavigateToSystem()
        {
            GlobalReferences.MenuSlider.Close();

            _settingsPage.SystemsPage();
        }

        private void NavigateToNetwork()
        {
            GlobalReferences.MenuSlider.Close();

            _settingsPage.NetworkPage();
        }

        private void NavigateToPrinters()
        {
            GlobalReferences.MenuSlider.Close();

            _settingsPage.PrintersPage();
        }

        private void NavigateToElection()
        {
            GlobalReferences.MenuSlider.Close();

            _settingsPage.ElectionPage();
        }

        private void NavigateToBallots()
        {
            GlobalReferences.MenuSlider.Close();

            _settingsPage.BallotsPage();
        }

        private void NavigateToLogin()
        {
            GlobalReferences.Header.HamburgerMenuVisibility = false;

            GlobalReferences.MenuSlider.SetMenu(null, MenuCollapseMode.None);

            GlobalReferences.MenuSlider.Close();

            _settingsPage.BackButton_Click(new object(), new RoutedEventArgs());
        }
    }
}
