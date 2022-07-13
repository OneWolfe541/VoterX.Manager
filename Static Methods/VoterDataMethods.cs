using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Logging;
using VoterX.Core.Voters;
using VoterX.Models;
using VoterX.Core.Extensions;

namespace VoterX.Methods
{
    /// <summary>
    /// Static Methods for accessing the Voter Repositories
    /// </summary>
    public static class VoterMethods
    {
        public static VoterFactory Voters
        {
            get
            {
                return ((App)Application.Current).Voters;
            }
        }

        public static bool Exists
        {
            get
            {
                if (((App)Application.Current).Voters != null)
                {
                    return ((App)Application.Current).Voters.Exists();
                }
                else
                {
                    return false;
                }
            }
        }

        public static string InsertProvisionalBallot(NMVoter voter, int provisionalReasonId)
        {
            //LoggingMethods.LogObject("INSERTPROVISIONALBALLOT CALLED");

            string message = "";

            // Set local values
            voter.Data.ElectionID = AppSettings.Election.ElectionID;
            voter.Data.PollID = AppSettings.System.SiteID;
            voter.Data.ComputerID = AppSettings.System.MachineID;
            voter.Data.UserId = AppSettings.User.UserID;

            // Save create new Spoiled Record
            try
            {
                voter.ProvisionalBallot(provisionalReasonId);
            }
            catch (Exception e)
            {
                message = e.InnerException.ToString();
            }

            return message;
        }

        public static List<VoterReportModel> ReportList(ReportWizardQueryModel wizard)
        {
            return ((App)Application.Current).Voters.ReportList(
                    wizard.BeginningDate,
                    wizard.EndingDate,
                    wizard.LogCodes,
                    wizard.BallotStyles,
                    wizard.Parties,
                    wizard.Jurisdictions,
                    wizard.PollSites)
                    .Where(v => v.IDRequired == wizard.IdRequired)
                    .ToList()
                    .ConvertToReportModel();
        }

        public static List<VoterReportModel> ReportListRegistered(ReportWizardQueryModel wizard)
        {
            return ((App)Application.Current).Voters.ReportListRegistered(
                    wizard.LogCodes,
                    wizard.BallotStyles,
                    wizard.Parties,
                    wizard.Jurisdictions,
                    wizard.PollSites)
                    .Where(v => v.IDRequired == wizard.IdRequired)
                    .ToList()
                    .ConvertToReportModel();
        }

        public static List<VoterReportModel> ConvertToReportModel(this List<VoterDataModel> voters)
        {
            List<VoterReportModel> reportList = new List<VoterReportModel>();
            foreach (var voter in voters)
            {
                VoterReportModel voterReport = new VoterReportModel
                {
                    VoterID = voter.VoterID,
                    FullName = voter.FullName,
                    LastName = voter.LastName,
                    FirstName = voter.FirstName,
                    MiddleName = voter.MiddleName,
                    Generation = voter.Generation,
                    DOBYear = voter.DOBYear,
                    IDRequired = voter.IDRequired.Value,
                    PrecinctName = voter.PrecinctName,
                    Party = voter.Party,
                    Address1 = voter.Address1,
                    Address2 = voter.Address2,
                    City = voter.City,
                    State = voter.State,
                    Zip = voter.Zip,
                    DeliveryAddress1 = voter.DeliveryAddress1,
                    DeliveryAddress2 = voter.DeliveryAddress2,
                    DeliveryCity = voter.DeliveryCity,
                    DeliveryState = voter.DeliveryState,
                    DeliveryZip = voter.DeliveryZip,
                    OutofCountry = voter.OutofCountry,
                    DeliveryProvince = voter.DeliveryProvince,
                    DeliveryCountry = voter.DeliveryCountry,
                    LogDescription = voter.LogDescription,
                    ActivityDate = voter.ActivityDate,
                    PollName = voter.PollName,
                    BallotStyle = voter.BallotStyle,
                    ApplicationIssued = voter.ApplicationIssued,
                    ApplicationAccepted = voter.ApplicationAccepted,
                    ApplicationRejected = voter.ApplicationRejected,
                    BallotIssued = voter.BallotIssued,
                    BallotPrinted = voter.BallotPrinted
                };
                reportList.Add(voterReport);
            }

            return reportList;
        }
    }
}
