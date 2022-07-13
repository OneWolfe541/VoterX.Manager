using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using VoterX.Core.Models.ViewModels;
using VoterX.Methods;
using VoterX.Dialogs;
using System.Globalization;
using System.Threading;
//using VoterX.Core.Extensions;

namespace VoterX.Methods
{
    public static class ReportPreviewMethods
    {
        private static bool debugMode = AppSettings.DebugMode;

        //#############################################################################
        // Daily Detail By Site
        public static void PreviewDailyDetailBySiteReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeRosterBySite01", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailBySiteReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeRosterBySite01", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailBySiteReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeRosterBySite01", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailBySiteReportGeneral(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQLGeneral(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeRosterBySite", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailBySiteReportGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQLGeneral(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeRosterBySite", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailBySiteReportGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQLGeneral(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeRosterBySite", sql, debugMode, "DailyDetailData");
        }

        //#############################################################################
        // Daily Detail by Ballot Style
        public static void PreviewDailyDetailByBallotStyleReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeRosterByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailByBallotStyleReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeRosterByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailByBallotStyleReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeRosterByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailByBallotStyleReportGeneral(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQLGeneral(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeRosterByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailByBallotStyleReportGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQLGeneral(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeRosterByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailByBallotStyleReportGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQLGeneral(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeRosterByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        //#############################################################################
        // Site Summary
        public static void PreviewDailySummaryBySiteReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailySummaryBySiteSQL(electionID, pollID); 

            FastReportsMethods.PreviewReport("AbsenteeActivityBySite", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailySummaryBySiteReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailySummaryBySiteSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeActivityBySite", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailySummaryBySiteReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailySummaryBySiteSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeActivityBySite", sql, debugMode, "DailyDetailData");
        }

        //#############################################################################
        // Ballot Style Summary
        public static void PreviewSummaryByBallotStyleReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeActivityByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewSummaryByBallotStyleReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeActivityByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewSummaryByBallotStyleReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeActivityByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewSummaryByBallotStyleReportGeneral(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQLGeneral(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeActivityByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewSummaryByBallotStyleReportGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQLGeneral(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeActivityByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewSummaryByBallotStyleReportGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQLGeneral(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeActivityByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        //#############################################################################
        // Spoiled Details
        public static void PreviewSpoiledBallotDetailsReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetSpoiledBallotDetailsSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeSpoiledBallots01", sql, debugMode, "DailySpoiledData");
        }

        public static void PreviewSpoiledBallotDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetSpoiledBallotDetailsSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeSpoiledBallots01", sql, debugMode, "DailySpoiledData");
        }

        public static void PreviewSpoiledBallotDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetSpoiledBallotDetailsSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeSpoiledBallots01", sql, debugMode, "DailySpoiledData");
        }

        public static void PreviewSpoiledBallotDetailsReportGeneral(int electionID, int? pollID)
        {
            string votersql = ReportSQLMethods.GetSpoiledBallotDetailsSQLGeneral(electionID, pollID);

            //string electionsql = "SELECT TOP 1 *, report_type = 'Activity to Date' FROM vElections";

            //FastReportsMethods.PreviewReport("AbsenteeSpoiledBallots02", debugMode, votersql, "DailySpoiledData", electionsql, "ElectionData");
            FastReportsMethods.PreviewReport("AbsenteeSpoiledBallots", votersql, debugMode, "DailySpoiledData");
        }

        public static void PreviewSpoiledBallotDetailsReportGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string votersql = ReportSQLMethods.GetSpoiledBallotDetailsSQLGeneral(electionID, pollID, reportingDate);

            //string electionsql = "SELECT TOP 1 *, ";
            //electionsql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            //electionsql += "FROM vElections";

            //FastReportsMethods.PreviewReport("AbsenteeSpoiledBallots02", debugMode, votersql, "DailySpoiledData", electionsql, "ElectionData");
            FastReportsMethods.PreviewReport("AbsenteeSpoiledBallots", votersql, debugMode, "DailySpoiledData");
        }

        public static void PreviewSpoiledBallotDetailsReportGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string votersql = ReportSQLMethods.GetSpoiledBallotDetailsSQLGeneral(electionID, pollID, startDate, endDate);

            string electionsql = "SELECT TOP 1 *, ";
            electionsql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            electionsql += "FROM vElections";

            //FastReportsMethods.PreviewReport("AbsenteeSpoiledBallots02", debugMode, votersql, "DailySpoiledData", electionsql, "ElectionData");
            FastReportsMethods.PreviewReport("AbsenteeSpoiledBallots", votersql, debugMode, "DailySpoiledData");
        }

        //#############################################################################
        // Provisional Details
        public static void PreviewProvisionalDetailsReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetProvisionalDetailsSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeProvisionalBallots", sql, debugMode, "DailyProvisionalData");
        }

        public static void PreviewProvisionalDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetProvisionalDetailsSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeProvisionalBallots", sql, debugMode, "DailyProvisionalData");
        }

        public static void PreviewProvisionalDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetProvisionalDetailsSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeProvisionalBallots", sql, debugMode, "DailyProvisionalData");
        }

        //#############################################################################
        // Unsigned Details
        public static void PreviewUnsignedDetailsReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetUnsignedDetailsSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeUnsignedEnvelopes", sql, debugMode, "DailyUnsignedData");
        }

        public static void PreviewUnsignedDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetUnsignedDetailsSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeUnsignedEnvelopes", sql, debugMode, "DailyUnsignedData");
        }

        public static void PreviewUnsignedDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetUnsignedDetailsSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeUnsignedEnvelopes", sql, debugMode, "DailyUnsignedData");
        }

        //#############################################################################
        // Rejected Application Details
        public static void PreviewRejectedApplicationDetailsReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetRejectedApplicationDetailsSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeRejectedApplications", sql, debugMode, "DailyRejectedData");
        }

        public static void PreviewRejectedApplicationDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetRejectedApplicationDetailsSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeRejectedApplications", sql, debugMode, "DailyRejectedData");
        }

        public static void PreviewRejectedApplicationDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetRejectedApplicationDetailsSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeRejectedApplications", sql, debugMode, "DailyRejectedData");
        }
    }
}
