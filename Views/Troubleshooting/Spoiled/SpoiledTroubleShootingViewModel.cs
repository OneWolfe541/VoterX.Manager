using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Core.Voters;
using VoterX.SystemSettings.Models.TroubleShooting;
using VoterX.Utilities.Commands;
using VoterX.Utilities.Controls;
using VoterX.Utilities.Methods;
using VoterX.Methods;
using System.Windows.Input;
using VoterX.Manager.Methods;
using System.ComponentModel;
using VoterX.Manager.Global;

namespace VoterX.Manager.Views.Troubleshooting
{
    public class SpoiledTroubleShootingViewModel : NotifyPropertyChanged
    {
        private NMVoter _voter;

        public SpoiledTroubleShootingViewModel(NMVoter voter)
        {
            _voter = voter;

            // Display Header
            GlobalReferences.Header.PageHeader = ("SPoiled Print Verification");
        }

        #region DisplayText
        public string BallotStyleName
        {
            get
            {
                return _voter.Data.BallotStyle;
            }
        }
        public string BallotNumber
        {
            get
            {
                if (_voter.Data.BallotNumber != null)
                {
                    return _voter.Data.BallotNumber.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region ButtonCommands
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
            //_parent.Navigate(new Voter.VoterSearchPage(_parent));
            //NavigationMenuMethods.VoterSearchPage();

            NavigationMenuMethods.ReturnToOrigin();
        }

        public bool GoBackVisibility { get; set; }
        private void SetGoBackVisibility(bool vis)
        {
            GoBackVisibility = vis;
            RaisePropertyChanged("GoBackVisibility");
        }

        // Bound command for reprinting the ballot
        public RelayCommand _reprintBallotCommand;
        public ICommand ReprintBallotCommand
        {
            get
            {
                if (_reprintBallotCommand == null)
                {
                    _reprintBallotCommand = new RelayCommand(param => this.ReprintBallotClick());
                }
                return _reprintBallotCommand;
            }
        }

        // Reprint the ballot
        public async void ReprintBallotClick()
        {
            Console.WriteLine("Ballot Reprinted");

            // Reprint the Ballot
            var errorMessage = await Task.Run(() => VoterX.Methods.BallotPrinting.ReprintBallot(_voter.Data));

            if (errorMessage != null)
            {
                // Display Error Message
                GlobalReferences.StatusBar.TextCenter = (errorMessage);
            }

            // Reset the Questionnaire
            ResetBallotPrintedQuestionnaire();
        }

        public bool ReprintBallotVisibility { get; set; }
        private void SetReprintBallotVisibility(bool vis)
        {
            ReprintBallotVisibility = vis;
            RaisePropertyChanged("ReprintBallotVisibility");
        }
        #endregion

        #region BallotPrintedQuestionnaire
        private PrintVerificationQuestionnaireViewModel _ballotPrintedQuestionnaire;
        public PrintVerificationQuestionnaireViewModel BallotPrintedQuestionnaire
        {
            get
            {
                if (_ballotPrintedQuestionnaire == null)
                {
                    BallotTroubleshootingQuestionnaireText BallotTroubleshootingQuestionnaire = new BallotTroubleshootingQuestionnaireText
                    {
                        ReportMessage = "THE BALLOT HAS BEEN SENT TO THE PRINTER",
                        ReprintMessage = "A NEW BALLOT HAS BEEN SENT TO THE PRINTER",
                        ReportQuestion = "Did the ballot print properly?",
                        PrinterMessage = "GO THROUGH THE BASIC PRINTER TROUBLESHOOTING STEPS",
                        PrinterQuestion = "After troubleshooting the printer did the ballot print properly?",
                        OptionsMessage = "CHOOSE ONE OF THE FOLLOWING OPTIONS TO PROCEED",
                        ReprintChoiceMessage = "I want to attempt to print the ballot again.",
                        ExitChoiceMessage = "I want to process the voter on another computer and return to the search screen.",
                        FinalMessage = "GOODBYE"
                    };

                    // Create and populate questionnaire
                    _ballotPrintedQuestionnaire = new PrintVerificationQuestionnaireViewModel(BallotTroubleshootingQuestionnaire, false);

                    // Bind onchanged method
                    _ballotPrintedQuestionnaire.PropertyChanged += OnBallotPrintedQuestionnairePropertyChanged;
                }
                return _ballotPrintedQuestionnaire;
            }
        }

        private void OnBallotPrintedQuestionnairePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Get questionnaire response (YES/NO)
            if (e.PropertyName == "Response")
            {
                // When any property is changed reset all the buttons
                // then determine which buttons to activate based on the responses
                SetGoBackVisibility(false);

                // When YES is clicked then display the next questionnaire
                if (((PrintVerificationQuestionnaireViewModel)sender).Response == true)
                {
                    SetGoBackVisibility(true);
                }
                else if (((PrintVerificationQuestionnaireViewModel)sender).Response == false)
                {
                    SetGoBackVisibility(true);
                }
            }

            // Get questionnaire response to reprint the ballot
            if (e.PropertyName == "Reprint")
            {
                SetReprintBallotVisibility(false);

                if (((PrintVerificationQuestionnaireViewModel)sender).Reprint == true)
                {
                    SetReprintBallotVisibility(true);
                }
            }
        }

        private void ResetBallotPrintedQuestionnaire()
        {
            _ballotPrintedQuestionnaire.Reset();
        }
        #endregion
    }
}
