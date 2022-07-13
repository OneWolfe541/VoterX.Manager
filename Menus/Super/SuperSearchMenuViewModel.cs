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
using VoterX.Manager.Views.Super;

namespace VoterX.Manager.Menus
{
    public class SuperSearchMenuViewModel : IMenuViewModel
    {
        private SuperSearchPage _superSearchPage;

        public SuperSearchMenuViewModel(SuperSearchPage page)
        {
            _superSearchPage = page;

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
                        FontAwesome.WPF.FontAwesomeIcon.Gear,
                        "SYSTEM",
                        "SYSTEM",
                        new Thickness(0, 25, 0, 0),
                        param => NavigationMenuMethods.NavigateToPage(new SettingsPage())
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

        private void NavigateToLogin()
        {
            GlobalReferences.Header.HamburgerMenuVisibility = false;

            GlobalReferences.MenuSlider.SetMenu(null, MenuCollapseMode.None);

            GlobalReferences.MenuSlider.Close();

            _superSearchPage.BackButton_Click(new object(), new RoutedEventArgs());
        }
    }
}
