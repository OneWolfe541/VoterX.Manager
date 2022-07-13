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

namespace VoterX.Manager.Menus
{
    public class AllMailVoterDetailsMenuViewModel : NotifyPropertyChanged, IMenuViewModel
    {
        private SystemSettingsController _settings;

        private AllMailVoterDetailsViewModel voterdetails = null;

        public AllMailVoterDetailsMenuViewModel(SliderMenuFrameControl menuSlider, SystemSettingsController settings, AllMailVoterDetailsViewModel page)
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
                            FontAwesome.WPF.FontAwesomeIcon.EnvelopeOutlinepenOutline,
                            "PRINT MAIL-IN BALLOT",
                            "PRINT MAIL-IN BALLOT",
                            new Thickness(0, 15, 0, 0),
                            param => PrintMailingBallot(),
                            param => voterdetails.PrintBallotBundleCommand.CanExecute(null)
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.CheckSquareOutline,
                            "PRINT ID REQUIRED BALLOT",
                            "PRINT ID REQUIRED BALLOT",
                            new Thickness(0, 15, 0, 0),
                            param => PrintIdRequiredBallot(),
                            param => voterdetails.PrintIdRequiredBundleCommand.CanExecute(null)
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.AddressCardOutline,
                            "REPRINT BALLOT LABEL",
                            "REPRINTS ADDRESS LABELS FOR BALLOTS ONLY. THESE LABELS CONTAIN AN INTELLIGENT MAIL BARCODE(IMB) USED FOR BALLOT TRACKING.",
                            new Thickness(0, 10, 0, 0),
                            param => PrintBallotAddressLabel(),
                            param => voterdetails.PrintBallotMailingLabelCommand.CanExecute(null)
                        ));

                    //_centerCustomControls.Add(buttonFactory.CreateButton(
                    //        "\uD83D\uDD03",
                    //        32,
                    //        32,
                    //        "SPOIL A BALLOT",
                    //        "SPOIL A BALLOT",
                    //        new Thickness(0, 30, 0, 0),
                    //        param => SpoilBallot(),
                    //        param => voterdetails.SpoilBallotCommand.CanExecute(null)
                    //    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.Recycle,
                            "SPOIL A BALLOT",
                            "SPOIL A BALLOT",
                            new Thickness(0, 30, 0, 0),
                            param => SpoilBallot(),
                            param => voterdetails.SpoilBallotCommand.CanExecute(null)
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            "\uD83D\uDD03",
                            32,
                            32,
                            "REISSUE BALLOT",
                            "REISSUE BALLOT",
                            new Thickness(0, 15, 0, 0),
                            param => ReissueBallot(),
                            param => voterdetails.PrintReissueBundleCommand.CanExecute(null)
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            new Uri("C:\\Program Files\\VoterX\\Images\\rejectmail_final.ico"),
                            50,
                            32,
                            "REJECT BALLOT",
                            "REJECT BALLOT",
                            new Thickness(0, 10, 0, 0),
                            param => RejectedBallot(),
                            param => voterdetails.RejectBallotCommand.CanExecute(null)
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.AddressCardOutline,
                            "PRINT STANDARD LABEL",
                            "PRINT STANDARD LABEL",
                            new Thickness(0, 15, 0, 0),
                            param => PrintAddressLabel()
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

        private void GoBack()
        {
            if (voterdetails.GoBackCommand.CanExecute(null))
            {
                voterdetails.GoBackCommand.Execute(null);

                //GlobalReferences.MenuSlider.SetMenu(new AbsenteeMenu(), MenuCollapseMode.Full);
            }
        }

        private void PrintApplication()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintApplicationCommand.CanExecute(null))
                voterdetails.PrintApplicationCommand.Execute(null);
        }

        private void PrintMailingBallot()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintBallotBundleCommand.CanExecute(null))
                voterdetails.PrintBallotBundleCommand.Execute(null);
        }

        private void PrintIdRequiredBallot()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintIdRequiredBundleCommand.CanExecute(null))
                voterdetails.PrintIdRequiredBundleCommand.Execute(null);
        }

        private void PrintBallotAddressLabel()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintBallotMailingLabelCommand.CanExecute(null))
                voterdetails.PrintBallotMailingLabelCommand.Execute(null);
        }

        private void PrintAddressLabel()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintMailingLabelCommand.CanExecute(null))
                voterdetails.PrintMailingLabelCommand.Execute(null);
        }

        private void BatchVoter()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.BatchVoterCommand.CanExecute(null))
                voterdetails.BatchVoterCommand.Execute(null);
        }

        private void SpoilBallot()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.SpoilBallotCommand.CanExecute(null))
                voterdetails.SpoilBallotCommand.Execute(null);
        }

        private void ReissueBallot()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintReissueBundleCommand.CanExecute(null))
                voterdetails.PrintReissueBundleCommand.Execute(null);
        }

        private void RejectedApplication()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.RejectApplicationCommand.CanExecute(null))
                voterdetails.RejectApplicationCommand.Execute(null);
        }

        private void PrinProvisionalBallot()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.PrintProvisionalBallotCommand.CanExecute(null))
                voterdetails.PrintProvisionalBallotCommand.Execute(null);
        }

        private void RejectedBallot()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.RejectBallotCommand.CanExecute(null))
                voterdetails.RejectBallotCommand.Execute(null);
        }
    }
}
