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
using VoterX.Manager.Views.Absentee;
using VoterX.Manager.Views.Super;

namespace VoterX.Manager.Menus
{
    public class SuperDetailsMenuViewModel : IMenuViewModel
    {
        private SuperDetailsViewModel _superDetailsViewModel;

        public SuperDetailsMenuViewModel(SuperDetailsViewModel model)
        {
            _superDetailsViewModel = model;

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
                    // Create factory
                    MenuButtonFactory buttonFactory = new MenuButtonFactory();

                    _homeCustomControls = new ObservableCollection<Control>();

                    _homeCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.Home,
                            "BACK TO SEARCH",
                            "BACK TO SEARCH",
                            new Thickness(0, 5, 0, 0),
                            param => _superDetailsViewModel.GoBackCommand.Execute(null)
                        ));
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
                        FontAwesome.WPF.FontAwesomeIcon.StepBackward,
                        "REVERT HISTORY",
                        "REVERT HISTORY",
                        new Thickness(0, 65, 0, 0),
                        param => _superDetailsViewModel.RevertHistoryCommand.Execute(null),
                        param => _superDetailsViewModel.RevertHistoryCommand.CanExecute(null)
                    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                        FontAwesome.WPF.FontAwesomeIcon.Recycle,
                        "MARK BACK VOTER",
                        "MARK BACK VOTER",
                        new Thickness(0, 25, 0, 0),
                        param => _superDetailsViewModel.MarkBackCommand.Execute(null),
                        param => _superDetailsViewModel.MarkBackCommand.CanExecute(null)
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

                    //_exitCustomControls.Add(buttonFactory.CreateButton(
                    //        FontAwesome.WPF.FontAwesomeIcon.SignOut,
                    //        "LOG OUT",
                    //        "LOG OUT",
                    //        new Thickness(0, 0, 0, 5),
                    //        param => NavigateToLogin()
                    //    ));
                }
                return _exitCustomControls;
            }
        }

        private void NavigateToLogin()
        {
            GlobalReferences.Header.HamburgerMenuVisibility = false;

            GlobalReferences.MenuSlider.SetMenu(null, MenuCollapseMode.None);

            GlobalReferences.MenuSlider.Close();

            //_superDetailsPage.BackButton_Click(new object(), new RoutedEventArgs());
        }
    }
}
