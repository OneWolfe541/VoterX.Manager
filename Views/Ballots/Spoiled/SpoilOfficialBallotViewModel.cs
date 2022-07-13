using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoterX.Dialogs;
using VoterX.Methods;
using VoterX.Core.Elections;
using VoterX.Core.Voters;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Controls;
using VoterX.Utilities.Extensions;
using VoterX.Utilities.Views.VoterDetails;
using VoterX.Utilities.Methods;
using VoterX.Manager.Methods;
using VoterX.Manager.Global;
using VoterX;
using System.Windows;

namespace VoterX.Manager.Views.Ballots
{
    public class SpoilOfficialBallotViewModel : NotifyPropertyChanged
    {
        // Dont use Observable Collections for single voter display
        // Have to use INotifyPropertyChanged when not using an Observable Collection
        private NMVoter VoterItem { get; set; }

        public SpoilOfficialBallotViewModel(NMVoter voter)
        {
            VoterItem = voter;

            SetDisplayMessages();

            // Initialize spoil ballot button
            CanSpoilBallot = true;

            // Display Header
            GlobalReferences.Header.PageHeader = ("Spoil Ballot");

            GlobalReferences.StatusBar.TextClear();
        }

        #region DisplayText
        public string SpoiledBallotMessage { get; set; }
        public string InnerSurrenderedQuestion { get; set; }

        private void SetDisplayMessages()
        {
            SpoiledBallotMessage = "SPOIL A BALLOT FOR CURRENT VOTER";
            InnerSurrenderedQuestion = "Has a physical ballot been surrendered?";
        }
        #endregion

        #region VoterDetails
        private VoterDetailsViewModel _voterDetailsView;
        public VoterDetailsViewModel VoterDetailsView
        {
            get
            {
                if (_voterDetailsView == null)
                {
                    _voterDetailsView = new VoterDetailsViewModel(VoterItem);
                }
                return _voterDetailsView;
            }
            private set
            {
                _voterDetailsView = value;
            }
        }
        #endregion

        #region SpoiledReasons
        private bool WrongVoter;
        private bool FledVoter;

        private bool _surrenderedVisible;
        public bool SurrenderedVisible
        {
            get
            {
                return _surrenderedVisible;
            }
            set
            {
                _surrenderedVisible = value;
            }
        }

        private List<SpoiledReasonModel> _reasonsList;
        public List<SpoiledReasonModel> ReasonsList
        {
            get
            {
                // Create list of invalid reasons
                List<int> doNotUse = new List<int> { 9 };
                if (AppSettings.Absentee.AllMailMode == true)
                {
                    doNotUse = new List<int> { 9, 5 };
                }                

                if (_reasonsList == null)
                {
                    _reasonsList = ElectionDataMethods.Election.Lists.SpoiledReasons
                        .Where(r => !doNotUse.Contains(r.SpoiledReasonId))
                        .ToList();
                }
                return _reasonsList;
            }
        }

        private SpoiledReasonModel _selectedReasonItem;
        public SpoiledReasonModel SelectedReasonItem
        {
            get
            {
                //if (_selectedReasonItem != null)
                //{
                //    GlobalReferences.StatusBar.TextLeft = _selectedReasonItem.ToString();
                //}
                return _selectedReasonItem;
            }
            set
            {
                _selectedReasonItem = value;
                if (_selectedReasonItem != null)
                {
                    SurrenderedVisible = true;
                    RaisePropertyChanged("SurrenderedVisible");

                    // Check if wrong or fled options were selected
                    switch (_selectedReasonItem.SpoiledReasonId)
                    {
                        case 3:
                            FledVoter = true;
                            WrongVoter = false;
                            break;
                        case 4:
                            FledVoter = false;
                            WrongVoter = true;
                            break;
                        default:
                            FledVoter = false;
                            WrongVoter = false;
                            break;
                    }
                }
                else
                {
                    SurrenderedVisible = false;
                    RaisePropertyChanged("SurrenderedVisible");
                }
            }
        }
        #endregion

        #region BallotSurrenderedQuestionControl
        private bool _spoilButtonVisible;
        public bool SpoilButtonVisible
        {
            get
            {
                return _spoilButtonVisible;
            }
            set
            {
                _spoilButtonVisible = value;
            }
        }

        public YesNoQuestionViewModel BallotSurrenderQuestion
        {
            get
            {
                YesNoQuestionViewModel BallotSurrenderQuestion = new YesNoQuestionViewModel(InnerSurrenderedQuestion);
                BallotSurrenderQuestion.PropertyChanged += BallotSurrenderPropertyChanged;
                return BallotSurrenderQuestion;
            }
        }

        private void BallotSurrenderPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                // Show spoil ballot button
                SpoilButtonVisible = true;
                RaisePropertyChanged("SpoilButtonVisible");

                var question = sender as YesNoQuestionViewModel;

                // Update the voters ballot surrendered status
                VoterItem.Data.BallotSurrendered = question.IsChecked ?? false;
                RaisePropertyChanged("VoterItem");
            }
        }
        #endregion

        #region Commands
        // Bound command for returning to the search screen
        public RelayCommand _goBackCommand;
        public ICommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(param => this.ReturnToSearchClick());
                }
                return _goBackCommand;
            }
        }

        // Force parent frame to navigate back to the search page
        public void ReturnToSearchClick()
        {
            //_parent.Navigate(new VoterSearchPage(_parent));
            //NavigationMenuMethods.VoterSearchPage();

            NavigationMenuMethods.ReturnToOrigin();
        }

        // Bound command for printing an official ballot
        public RelayCommand _spoilBallotCommand;
        public ICommand SpoilBallotCommand
        {
            get
            {
                if (_spoilBallotCommand == null)
                {
                    _spoilBallotCommand = new RelayCommand(param => this.SpoilBallotClick(), param => this.CanSpoilBallot);
                }
                return _spoilBallotCommand;
            }
        }

        // Enable or Disable the Print Ballot Button
        private bool _canSpoilBallot;
        public bool CanSpoilBallot
        {
            get { return _canSpoilBallot; }
            internal set
            {
                _canSpoilBallot = value;
                RaisePropertyChanged("CanPrintBallot");
            }
        }

        private async void SpoilBallotClick()
        {
            // Disable spoil ballot button
            CanSpoilBallot = false;

            if (FledVoter == true)
            {
                // Mark Fled Voter
                VoterItem.UpdateFledVoter();

                // Display message
                AlertDialog fledDialog = new AlertDialog("THIS VOTER'S STATUS BEEN CHANGED TO FLED VOTER");
                if (fledDialog.ShowDialog() == true)
                {
                    ReturnToSearchClick();
                }
            }
            else if (WrongVoter == true)
            {
                // Mark Wrong Voter
                VoterItem.UpdateWrongVoter();

                // Display message
                AlertDialog wrongDialog = new AlertDialog("THIS VOTER'S STATUS BEEN CHANGED TO WRONG VOTER");
                if (wrongDialog.ShowDialog() == true)
                {
                    ReturnToSearchClick();
                }
            }
            else
            {
                // Set local system values
                VoterItem.Localize(AppSettings.Global);

                // Mark Spoiled Ballot
                VoterItem.SpoilBallot(SelectedReasonItem.SpoiledReasonId);                
                
                IntelligentBarcodeModel imbParameters = new IntelligentBarcodeModel()
                {
                    Barcode = AppSettings.Absentee.IMBBarCodeId,
                    OutServiceType = AppSettings.Absentee.IMBOutServiceTypeId,
                    InServiceType = AppSettings.Absentee.IMBInServiceTypeId,
                    //MailerId = AppSettings.Absentee.IMBMailerId,
                    OutMailerId = AppSettings.Absentee.IMBOutGoing,
                    InMailerId = AppSettings.Absentee.IMBIncoming,
                    Prefix = AppSettings.Absentee.IMBPrefix,
                    Computer = Math.Abs(AppSettings.System.MachineID),
                    ReturnZip = AppSettings.Absentee.BallotReturnZip
                };

                if (VoterItem.GenerateIntelligentBarcodes(imbParameters) == true)
                {
                    // Update record --
                    VoterItem.UpdateReissued();
                    VoterItem.Data.LogCode = 6;

                    // Lock Scan History table
                    VoterItem.RemoveFromScanHistory(((App)Application.Current).Connection);

                    // Print new Ballot
                    //var errorMessage = await BallotPrinting.PrintOfficialBallotBundleAsync(VoterItem, AppSettings.Global);
                    var errorMessage = await Task.Run(() => VoterX.Methods.BallotPrinting.ReprintBallot(VoterItem.Data));

                    // Print Address Label
                    await Task.Run(() => { return VoterX.Methods.ReportPrintingMethods.PrintVoterMailingLabelsIMB(VoterItem.Data); });

                    if (errorMessage != null && errorMessage != "")
                    {
                        // Display Error Message
                        GlobalReferences.StatusBar.TextCenter = (errorMessage);
                    }
                    else
                    {
                        // Navigate to Spoiled Ballot Troubleshooting page
                        NavigationMenuMethods.SpoiledTroubleShootingPage(VoterItem);
                    }
                    NavigationMenuMethods.SpoiledTroubleShootingPage(VoterItem);
                }
                else
                {
                    GlobalReferences.StatusBar.TextCenter = "IMB could not be generated.";
                }
            }
        }
        #endregion
    }
}
