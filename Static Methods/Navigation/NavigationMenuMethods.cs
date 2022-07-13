using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VoterX;
using VoterX.Models;
using VoterX.Core.Voters;
using VoterX.Utilities.Models;
using VoterX.Manager.Global;

namespace VoterX.Manager.Methods
{
    public static class NavigationMenuMethods
    {
        public static void NavigateToPage(Page Destination)
        {
            //((App)Application.Current).MainFrame.Navigate(Destination);
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(Destination);
        }

        public static void LoginPage()
        {
            //((App)Application.Current).MainFrame.Navigate(new AbsenteeLoginPage());
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new AbsenteeLoginPage());
        }

        #region VotingPages
        public static void VoterSearchPage()
        {
            VoterSearchPage(null);
        }
        public static void VoterSearchPage(int? SiteId)
        {
            GlobalReferences.MenuSlider.Close();

            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Voter.Search.VoterSearchPage(SiteId));
        }
        #endregion

        #region ReturnEnvelopePages
        public static void ReturnApplications()
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Absentee.ReturnApplicationsPage());
        }

        public static void ReturnBallots()
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Absentee.ReturnBallotsPage());
        }
        #endregion

        #region MainMenu
        public static void AbsenteeHome() 
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new AbsenteeAdministrationPage());
        }
        #endregion

        #region BatchPrintingPages
        public static void BatchPrinting()
        {
            BatchPrinting(null);
        }

        public static void BatchPrinting(int? SiteId)
        {
            GlobalReferences.MenuSlider.Close();

            // NEW
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Absentee.BatchPrintingPage(SiteId));

            // OLD
            //((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
            //    .MainFrame.Navigate(new BatchBallotPrintingPage(SiteId));
        }
        #endregion

        #region ReportWizardPages
        public static void ReportWizardDates()
        {
            ReportWizardDates(null);
        }
        public static void ReportWizardDates(ReportWizardQueryModel WizardSearch)
        {
            GlobalReferences.MenuSlider.Close();

            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Absentee.ReportWizardDatePage(WizardSearch));
        }

        public static void ReportWizardSelections()
        {
            ReportWizardSelections(null);
        }
        public static void ReportWizardSelections(ReportWizardQueryModel WizardSearch)
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Absentee.ReportWizardSelectionsPage(WizardSearch));
        }

        public static void ReportWizardPrinting()
        {
            ReportWizardPrinting(null);
        }
        public static void ReportWizardPrinting(ReportWizardQueryModel WizardSearch)
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Absentee.ReportWizardPrintingPage(WizardSearch));
        }
        #endregion

        #region Register
        public static void Register()
        {
            GlobalReferences.MenuSlider.Close();

            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Absentee.RegisterPage());
        }
        #endregion

        #region TroubleShootingPages
        public static void ProvisionalTroubleShootingPage(NMVoter voter)
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Troubleshooting.ProvisionalTroubleShootingPage(voter));
        }

        public static void SpoiledTroubleShootingPage(NMVoter voter)
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Troubleshooting.SpoiledTroubleShootingPage(voter));
        }
        #endregion

        #region Spoiled
        public static void SpoiledBallotPage(NMVoter voter)
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Ballots.SpoilOfficialBallotPage(voter));
        }
        #endregion

        #region Provisional
        public static void ProvisionalBallotPage(NMVoter voter)
        {
            ProvisionalBallotPage(voter, false);
        }
        public static void ProvisionalBallotPage(NMVoter voter, bool editStyle)
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Ballots.ProvisionalBallotPage(voter, editStyle));
        }

        public static void AddProvisional()
        {
            GlobalReferences.MenuSlider.Close();

            // NEW
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Manage.AddProvisionalPage());

            // OLD
            //((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
            //    .MainFrame.Navigate(new AddProvisionalPage());
        }
        #endregion

        #region Manage
        public static void ManageHome()
        {
            // Does not appear as a smooth transition (possible due to the creation of a new menu panel when the page is loaded)
            GlobalReferences.MenuSlider.Close();

            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new VoterX.AdministrationPage());
        }

        public static void ReturnAddress()
        {
            GlobalReferences.MenuSlider.Close();

            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new VoterX.ApplicationSettingsPage());
        }
        #endregion

        #region EmergencyBallots
        public static void EmergencyBallots()
        {
            GlobalReferences.MenuSlider.Close();

            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Manage.EmergencyBallotsPage());
        }
        #endregion

        #region EditBallotStyle
        public static void EditBallotStyle()
        {
            GlobalReferences.MenuSlider.Close();

            // NEW
            //((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
            //    .MainFrame.Navigate(new Views.Manage.EditBallotStylesPage());

            // OLD
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new EditBallotSearchPage());
        }
        #endregion

        #region ManualRecordUpdate
        public static void ManualRecordUpdate()
        {
            GlobalReferences.MenuSlider.SetMenu(null, MenuCollapseMode.None);

            GlobalReferences.MenuSlider.Close();

            // NEW
            //((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
            //    .MainFrame.Navigate(new Views.Manage.ManualUpdatePage());

            // OLD
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new ManualRecordUpdate());
        }
        #endregion

        #region Reports
        public static void EarlyVotingReportsPage()
        {
            GlobalReferences.MenuSlider.Close();

            // NEW
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Manage.DailyReportsPage());

            // OLD
            //((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
            //    .MainFrame.Navigate(new ReportsPage());
        }

        public static void AbsenteeReportsPage()
        {
            GlobalReferences.MenuSlider.Close();

            // NEW
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Absentee.ReportsPage());

            // OLD
            //((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
            //    .MainFrame.Navigate(new AbsenteeReportPage());
        }
        #endregion

        #region SuperUser
        public static void SuperSearchPage()
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Super.SuperSearchPage());
        }

        public static void SuperVoterDetailsPage()
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new Views.Super.SuperSearchPage());
        }
        #endregion

        #region AbsenteeBoard
        public static void AbsenteeBoardHome()
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new AbsenteeBoardPage());
        }

        public static void AbsenteeBoardVoterLookupPage()
        {
            GlobalReferences.MenuSlider.Close();

            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new AbsenteeBoardVoterSearchPage());
        }

        public static void AbsenteeBoardScanEnvelopesPage()
        {
            GlobalReferences.MenuSlider.Close();

            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new AbsenteeBoardScanReturnedEnvelopes());
        }

        public static void AbsenteeBoardReportsPage()
        {
            GlobalReferences.MenuSlider.Close();

            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(new AbsenteeBoardReportsPage());
        }
        #endregion

        #region Origin
        public static void SetOrigin(Page page)
        {
            GlobalReferences.Origin = page;
        }

        public static void ReturnToOrigin()
        {
            ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault())
                .MainFrame.Navigate(GlobalReferences.Origin);
        }
        #endregion
    }
}
