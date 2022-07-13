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
using VoterX.Manager.Factories;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Menus
{
    public class AbsenteeBoardMenuViewModel : NotifyPropertyChanged, IMenuViewModel
    {
        private SystemSettingsController _settings;

        public AbsenteeBoardMenuViewModel(SliderMenuFrameControl menuSlider, SystemSettingsController settings)
        {
            _settings = settings;

            if (menuSlider != null)
            {
                menuSlider.AnimationStateChanged += OnAnimationStateChanged;
            }

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
                            "ABSENTEE BOARD HOME",
                            "ABSENTEE BOARD HOME",
                            new Thickness(0, 5, 0, 0),
                            param => NavigationMenuMethods.AbsenteeBoardHome()
                        ));
                }
                return _homeCustomControls;
            }
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
                            FontAwesome.WPF.FontAwesomeIcon.Barcode,
                            "SCAN RETURNS",
                            "SCAN RETURNS",
                            new Thickness(0, 15, 0, 0),
                            param => NavigationMenuMethods.AbsenteeBoardScanEnvelopesPage()
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.User,
                            "VOTER LOOKUP",
                            "VOTER LOOKUP",
                            new Thickness(0, 15, 0, 0),
                            param => NavigationMenuMethods.AbsenteeBoardVoterLookupPage()
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.BarChart,
                            "REPORTS",
                            "REPORTS",
                            new Thickness(0, 15, 0, 0),
                            param => NavigationMenuMethods.AbsenteeBoardReportsPage()
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
                    ElectionInfoPanelFactory electionInfoPanel = new ElectionInfoPanelFactory();

                    _exitCustomControls = electionInfoPanel.Create(_settings);
                }
                return _exitCustomControls;
            }
        }

        private void OnAnimationStateChanged(object sender, System.EventArgs e)
        {
            SliderMenuFrameControl slider = sender as SliderMenuFrameControl;

            if (slider != null && slider.AnimationState != "Open")
            {
                ExitPanelVisibility = false;
                RaisePropertyChanged("ExitPanelVisibility");
            }
            else
            {
                ExitPanelVisibility = true;
                RaisePropertyChanged("ExitPanelVisibility");
            }
        }
    }
}
