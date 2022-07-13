using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VoterX.SystemSettings;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Controls;
using VoterX.Utilities.Interfaces;
using VoterX.Manager.Factories;
using VoterX.Manager.Methods;

namespace VoterX.Manager.Menus
{
    public class ManageMenuViewModel : NotifyPropertyChanged, IMenuViewModel
    {
        private SystemSettingsController _settings;

        public ManageMenuViewModel(SliderMenuFrameControl menuSlider, SystemSettingsController settings)
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
                            "MANAGE HOME",
                            "MANAGE HOME",
                            new Thickness(0, 0, 0, 0),
                            param => NavigationMenuMethods.ManageHome()
                        ));

                    _homeCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.Gear,
                            "RETURN ADDRESS",
                            "RETURN ADDRESS",
                            new Thickness(0, 15, 0, 0),
                            param => NavigationMenuMethods.ReturnAddress()
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
                        FontAwesome.WPF.FontAwesomeIcon.UserPlus,
                        "ADD PROVISIONAL VOTER",
                        "ADD PROVISIONAL VOTER",
                        new Thickness(0, 35, 0, 0),
                        param => NavigationMenuMethods.AddProvisional()
                    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                        FontAwesome.WPF.FontAwesomeIcon.PencilSquareOutline,
                        "EDIT BALLOT STYLE",
                        "EDIT BALLOT STYLE",
                        new Thickness(0, 15, 0, 0),
                        param => NavigationMenuMethods.EditBallotStyle()
                    ));

                    //_centerCustomControls.Add(buttonFactory.CreateButton(
                    //    FontAwesome.WPF.FontAwesomeIcon.BarChart,
                    //    "EARLY VOTING REPORTS",
                    //    "EARLY VOTING REPORTS",
                    //    new Thickness(0, 15, 0, 0),
                    //    param => NavigationMenuMethods.EarlyVotingReportsPage()
                    //));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                        FontAwesome.WPF.FontAwesomeIcon.Clipboard,
                        "EMERGENCY BALLOTS",
                        "EMERGENCY BALLOTS",
                        new Thickness(0, 15, 0, 0),
                        param => NavigationMenuMethods.EmergencyBallots()
                    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                        FontAwesome.WPF.FontAwesomeIcon.Wrench,
                        "MANUAL UPDATE",
                        "MANUAL UPDATE",
                        new Thickness(0, 55, 0, 0),
                        param => NavigationMenuMethods.ManualRecordUpdate()
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
                    //_exitCustomControls = new ObservableCollection<Control>();

                    ElectionInfoPanelFactory electionInfoPanel = new ElectionInfoPanelFactory();

                    _exitCustomControls = electionInfoPanel.Create(_settings);
                }
                return _exitCustomControls;
            }
        }        

        private void OnAnimationStateChanged(object sender, System.EventArgs e)
        {
            SliderMenuFrameControl slider = sender as SliderMenuFrameControl;

            if(slider != null && slider.AnimationState != "Open")
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
