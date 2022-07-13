using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Methods.ReportSubQueries;
using VoterX.Utilities.Methods.ReportSubQueries;

namespace VoterX.Methods
{
    public static class ReissuedBallotsReportMethods
    {
        private static bool debugMode = AppSettings.DebugMode;

        public static string GetReissuedBallotsDetailsSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity to Date' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyVotedAudit ";
            sql += "FROM (" + ReportViewsRevised.DailyVotedReissuedView() + ") AS vDailyVotedAudit ";
            sql += "WHERE ";
            if (pollID != null) sql += "[PollId] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND NewLogCode = 6 ";

            return sql;
        }

        public static string GetReissuedBallotsDetailsSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity for " + reportingDate.ToShortDateString() + "' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyVotedAudit ";
            sql += "FROM (" + ReportViewsRevised.DailyVotedReissuedView() + ") AS vDailyVotedAudit ";
            sql += "WHERE ";
            if (pollID != null) sql += "[PollId] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(ChangeDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND NewLogCode = 6 ";

            return sql;
        }

        public static string GetReissuedBallotsDetailsSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyVotedAudit ";
            sql += "FROM (" + ReportViewsRevised.DailyVotedReissuedView() + ") AS vDailyVotedAudit ";
            sql += "WHERE ";
            if (pollID != null) sql += "[PollId] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            sql += "AND ChangeDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND ChangeDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND NewLogCode = 6 ";

            return sql;
        }

        public static string PrintReissuedBallotsDetailsReport(int electionID, int? pollID)
        {
            string auditsql = GetReissuedBallotsDetailsSQL(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeVotedReissuedReport", auditsql, debugMode, "VotedAuditData");
        }

        public static string PrintReissuedBallotsDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string auditsql = GetReissuedBallotsDetailsSQL(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeVotedReissuedReport", auditsql, debugMode, "VotedAuditData");
        }

        public static string PrintReissuedBallotsDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string auditsql = GetReissuedBallotsDetailsSQL(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeVotedReissuedReport", auditsql, debugMode, "VotedAuditData");
        }

        public static void PreviewReissuedBallotsDetailsReport(int electionID, int? pollID)
        {
            string auditsql = GetReissuedBallotsDetailsSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeVotedReissuedReport", auditsql, debugMode, "VotedAuditData");
        }

        public static void PreviewReissuedBallotsDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string auditsql = GetReissuedBallotsDetailsSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeVotedReissuedReport", auditsql, debugMode, "VotedAuditData");
        }

        public static void PreviewReissuedBallotsDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string auditsql = GetReissuedBallotsDetailsSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeVotedReissuedReport", auditsql, debugMode, "VotedAuditData");
        }
    }
}
