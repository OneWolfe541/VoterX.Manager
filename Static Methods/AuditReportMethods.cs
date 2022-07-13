using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Utilities.Methods.ReportSubQueries;

namespace VoterX.Methods
{
    public static class AuditReportMethods
    {
        private static bool debugMode = AppSettings.DebugMode;

        public static string GetAuditDetailsSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyVotedAudit ";
            sql += "FROM (" + ReportViewsRevised.DailyAuditView() + ") AS vDailyVotedAudit ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";

            return sql;
        }

        public static string GetAuditDetailsSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, "; 
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyVotedAudit ";
            sql += "FROM (" + ReportViewsRevised.DailyAuditView() + ") AS vDailyVotedAudit ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(ChangeDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return sql;
        }

        public static string GetAuditDetailsSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyVotedAudit ";
            sql += "FROM (" + ReportViewsRevised.DailyAuditView() + ") AS vDailyVotedAudit ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            sql += "AND ChangeDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND ChangeDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return sql;
        }

        public static string PrintAuditDetailsReport(int electionID, int? pollID)
        {
            string auditsql = GetAuditDetailsSQL(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeAuditReport", auditsql, debugMode, "VotedAuditData");
        }

        public static string PrintAuditDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string auditsql = GetAuditDetailsSQL(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeAuditReport", auditsql, debugMode, "VotedAuditData");
        }

        public static string PrintAuditDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string auditsql = GetAuditDetailsSQL(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeAuditReport", auditsql, debugMode, "VotedAuditData");
        }

        public static void PreviewAuditDetailsReport(int electionID, int? pollID)
        {
            string auditsql = GetAuditDetailsSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeAuditReport", auditsql, debugMode, "VotedAuditData");
        }

        public static void PreviewAuditDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string auditsql = GetAuditDetailsSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeAuditReport", auditsql, debugMode, "VotedAuditData");
        }

        public static void PreviewAuditDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string auditsql = GetAuditDetailsSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeAuditReport", auditsql, debugMode, "VotedAuditData");
        }
    }
}
