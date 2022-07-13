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
using VoterX.Methods;
using VoterX.SystemSettings;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Controls;
using VoterX.Utilities.Interfaces;
using VoterX.Utilities.Models;
using VoterX.Manager.Factories;
using VoterX.Manager.Global;
using VoterX.Manager.Methods;
using VoterX.Manager.Views.Absentee;

namespace VoterX.Manager.Menus
{
    public class AbsenteeBoardVoterDetailsMenuViewModel : NotifyPropertyChanged, IMenuViewModel
    {
        private SystemSettingsController _settings;

        private AbsenteeBoardVoterDetailsViewModel voterdetails = null;

        public AbsenteeBoardVoterDetailsMenuViewModel(SliderMenuFrameControl menuSlider, SystemSettingsController settings, AbsenteeBoardVoterDetailsViewModel page)
        {
            _settings = settings;

            voterdetails = page;

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
                            FontAwesome.WPF.FontAwesomeIcon.ArrowLeft,
                            "BACK TO SEARCH",
                            "BACK TO SEARCH",
                            new Thickness(0, 5, 0, 0),
                            param => GoBack()
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
                            FontAwesome.WPF.FontAwesomeIcon.Save,
                            "SAVE CHANGES",
                            "SAVE CHANGES",
                            new Thickness(0, 25, 0, 0),
                            param => UpdateVoterStatus()
                        ));

                    //_centerCustomControls.Add(buttonFactory.CreateButton(
                    //        FontAwesome.WPF.FontAwesomeIcon.Ban,
                    //        "REMOVE VOTER STATUS",
                    //        "REMOVE VOTER STATUS",
                    //        new Thickness(0, 15, 0, 0),
                    //        param => RemoveVoterStatus()
                    //    ));
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

        private void GoBack()
        {
            if (voterdetails.GoBackCommand.CanExecute(null))
            {
                voterdetails.GoBackCommand.Execute(null);
            }
        }

        private void UpdateVoterStatus()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.UpdateVoterBoardCommand.CanExecute(null))
                voterdetails.UpdateVoterBoardCommand.Execute(null);
        }

        private void RemoveVoterStatus()
        {
            GlobalReferences.MenuSlider.Close();

            //if (voterdetails.PrintBallotBundleCommand.CanExecute(null))
            //    voterdetails.PrintBallotBundleCommand.Execute(null);
        }
    }
}
