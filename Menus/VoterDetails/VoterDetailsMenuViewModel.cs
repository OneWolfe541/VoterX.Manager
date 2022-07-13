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
    public class VoterDetailsMenuViewModel : NotifyPropertyChanged, IMenuViewModel
    {
        private SystemSettingsController _settings;

        private VoterDetailsViewModel voterdetails = null;

        public VoterDetailsMenuViewModel(SliderMenuFrameControl menuSlider, SystemSettingsController settings, VoterDetailsViewModel page)
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
                            FontAwesome.WPF.FontAwesomeIcon.Print,
                            "PRINT APPLICATION",
                            "PRINT APPLICATION",
                            new Thickness(0, 20, 0, 0),
                            param => PrintApplication(),
                            param => voterdetails.PrintApplicationCommand.CanExecute(null)
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.EnvelopeOutlinepenOutline,
                            "PRINT MAIL-IN BALLOT",
                            "PRINT MAIL-IN BALLOT",
                            new Thickness(0, 10, 0, 0),
                            param => PrintMailingBallot(),
                            param => voterdetails.PrintBallotBundleCommand.CanExecute(null)
                        ));

                    //_centerCustomControls.Add(buttonFactory.CreateButton(
                    //        FontAwesome.WPF.FontAwesomeIcon.AddressCardOutline,
                    //        "PRINT ADDRESS LABEL",
                    //        "PRINT ADDRESS LABEL",
                    //        new Thickness(0, 10, 0, 0),
                    //        param => PrintAddressLabel(),
                    //        param => voterdetails.PrintMailingLabelCommand.CanExecute(null)
                    //    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.AddressCardOutline,
                            "REPRINT BALLOT LABEL",
                            "REPRINTS ADDRESS LABELS FOR BALLOTS ONLY. THESE LABELS CONTAIN AN INTELLIGENT MAIL BARCODE(IMB) USED FOR BALLOT TRACKING.",
                            new Thickness(0, 10, 0, 0),
                            param => PrintBallotAddressLabel(),
                            param => voterdetails.PrintBallotMailingLabelCommand.CanExecute(null)
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.ListOl,
                            "QUEUE FOR BATCH",
                            "QUEUE FOR BATCH",
                            new Thickness(0, 10, 0, 0),
                            param => BatchVoter(),
                            param => voterdetails.BatchVoterCommand.CanExecute(null)
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            "\uD83D\uDD03",
                            32,
                            32,
                            "SPOIL A BALLOT",
                            "SPOIL A BALLOT",
                            new Thickness(0, 30, 0, 0),
                            param => SpoilBallot(),
                            param => voterdetails.SpoilBallotCommand.CanExecute(null)
                        ));

                    //_centerCustomControls.Add(buttonFactory.CreateButton(
                    //        FontAwesome.WPF.FontAwesomeIcon.Ban,
                    //        "REJECT APPLICATION",
                    //        "REJECT APPLICATION",
                    //        new Thickness(0, 10, 0, 0),
                    //        param => RejectedApplication(),
                    //        param => voterdetails.RejectApplicationCommand.CanExecute(null)
                    //    ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            new Uri("C:\\Program Files\\VoterX\\Images\\rejectletter_final.ico"),
                            28,
                            32,
                            "REJECT APPLICATION",
                            "REJECT APPLICATION",
                            new Thickness(0, 10, 0, 0),
                            param => RejectedApplication(),
                            param => voterdetails.RejectApplicationCommand.CanExecute(null)
                        ));

                    _centerCustomControls.Add(buttonFactory.CreateButton(
                            FontAwesome.WPF.FontAwesomeIcon.CheckSquareOutline,
                            "PRINT PROVISIONAL BALLOT",
                            "PRINT PROVISIONAL BALLOT",
                            new Thickness(0, 10, 0, 0),
                            param => PrintProvisionalBallot(),
                            param => voterdetails.PrintProvisionalBallotCommand.CanExecute(null)
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
                            "PRINTS STANDARD ADDRESS LABELS",
                            new Thickness(0, 20, 0, 0),
                            param => PrintAddressLabel(),
                            param => voterdetails.PrintMailingLabelCommand.CanExecute(null)
                        ));

                    //_centerCustomControls.Add(buttonFactory.CreateButton(
                    //        FontAwesome.WPF.FontAwesomeIcon.AddressBook,
                    //        "SAVE TEMP ADDRESS",
                    //        "SAVE TEMP ADDRESS",
                    //        new Thickness(0, 20, 0, 0),
                    //        param => SaveTempAddress()
                    //    ));

                    //_centerCustomControls.Add(buttonFactory.CreateButton(
                    //        FontAwesome.WPF.FontAwesomeIcon.Barcode,
                    //        "GENERATE IMB",
                    //        "GENERATE IMB",
                    //        new Thickness(0, 20, 0, 0),
                    //        param => GenerateBarCode()
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

                //GlobalReferences.MenuSlider.SetMenu(new AbsenteeMenu(), MenuCollapseMode.Full);

                // Create View Model container
                IMenuViewModel menuView = null;

                // Load either the all mail or normal view model
                if (AppSettings.Absentee.AllMailMode == true)
                {
                    menuView = new AllMailAbsenteeMenuViewModel(GlobalReferences.MenuSlider, AppSettings.Global);
                }
                else
                {
                    menuView = new AbsenteeMenuViewModel(GlobalReferences.MenuSlider, AppSettings.Global);
                }

                // Create and load the menu from the view model
                GlobalReferences.MenuSlider.SetMenu(new ManageMenuView(menuView), MenuCollapseMode.Full);
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

        private void SaveTempAddress()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.SaveAddressCommand.CanExecute(null))
                voterdetails.SaveAddressCommand.Execute(null);
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

        private void RejectedApplication()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.RejectApplicationCommand.CanExecute(null))
                voterdetails.RejectApplicationCommand.Execute(null);
        }

        private void PrintProvisionalBallot()
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

        private void GenerateBarCode()
        {
            GlobalReferences.MenuSlider.Close();

            if (voterdetails.GenerateBarcodeCommand.CanExecute(null))
                voterdetails.GenerateBarcodeCommand.Execute(null);
        }
    }
}
