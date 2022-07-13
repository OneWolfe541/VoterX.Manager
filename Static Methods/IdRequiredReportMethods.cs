using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Methods.ReportSubQueries;
using VoterX.Utilities.Methods.ReportSubQueries;

namespace VoterX.Methods
{
    public static class IdRequiredReportMethods
    {
        private static bool debugMode = AppSettings.DebugMode;

        public static string GetIdRequiredSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyProvisionalReport ";
            sql += "FROM (" + ReportViewsRevised.DailyProvisionalReportView() + ") AS vDailyProvisionalReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";


            return sql;
        }

        public static string GetIdRequiredSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyProvisionalReport ";
            sql += "FROM (" + ReportViewsRevised.DailyProvisionalReportView() + ") AS vDailyProvisionalReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            sql += "AND CAST(last_modified AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";


            return sql;
        }

        public static string GetIdRequiredSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyProvisionalReport ";
            sql += "FROM (" + ReportViewsRevised.DailyProvisionalReportView() + ") AS vDailyProvisionalReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            // Given date range
            //sql += "AND last_modified BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            sql += "AND last_modified >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND last_modified <'" + endDate.AddDays(1).ToShortDateString() + "' ";


            return sql;
        }

        public static string PrintIdRequiredReport(int electionID, int? pollID)
        {
            string sql = GetIdRequiredSQL(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeIdRequiredBallots01", sql, debugMode, "DailyProvisionalData");
        }

        public static string PrintIdRequiredReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = GetIdRequiredSQL(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeIdRequiredBallots01", sql, debugMode, "DailyProvisionalData");
        }

        public static string PrintIdRequiredReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = GetIdRequiredSQL(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeIdRequiredBallots01", sql, debugMode, "DailyProvisionalData");
        }

        public static void PreviewIdRequiredReport(int electionID, int? pollID)
        {
            string sql = GetIdRequiredSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeIdRequiredBallots01", sql, debugMode, "DailyProvisionalData");
        }

        public static void PreviewIdRequiredReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = GetIdRequiredSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeIdRequiredBallots01", sql, debugMode, "DailyProvisionalData");
        }

        public static void PreviewIdRequiredReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = GetIdRequiredSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeIdRequiredBallots01", sql, debugMode, "DailyProvisionalData");
        }

        //############################################################################################//

        public static string GetDailyIdRequiredBySiteSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity to Date' ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";

            // Only select voters with an existing Provisional reason Id Required
            sql += "VoterId IN (SELECT VoterId FROM [dbo].[AvProvisionals] WHERE ProvisionalReason = 2) AND ";

            // Accept null site id
            if (pollID != null) sql += "PollId = " + pollID + " AND ";

            sql += "ElectionId = " + electionID + " ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetDailyIdRequiredBySiteSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity for " + reportingDate.ToShortDateString() + "' ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";

            // Only select voters with an existing Provisional reason Id Required
            sql += "VoterId IN (SELECT VoterId FROM [dbo].[AvProvisionals] WHERE ProvisionalReason = 2) AND ";

            // Accept null site id
            if (pollID != null) sql += "PollId = " + pollID + " AND ";

            sql += "ElectionId = " + electionID + " ";

            // Specific date
            sql += "AND CAST(ActivityDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetDailyIdRequiredBySiteSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "' ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";

            // Only select voters with an existing Provisional reason Id Required
            sql += "VoterId IN (SELECT VoterId FROM [dbo].[AvProvisionals] WHERE ProvisionalReason = 2) AND ";

            // Accept null site id
            if (pollID != null) sql += "PollId = " + pollID + " AND ";

            sql += "ElectionId = " + electionID + " ";

            // Given date range
            sql += "AND ActivityDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND ActivityDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string PrintDailyIdRequiredBySiteReport(int electionID, int? pollID)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeIdRequiredBallots", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailyIdRequiredBySiteReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeIdRequiredBallots", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailyIdRequiredBySiteReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeIdRequiredBallots", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyIdRequiredBySiteReport(int electionID, int? pollID)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeIdRequiredBallots", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyIdRequiredBySiteReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeIdRequiredBallots", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyIdRequiredBySiteReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeIdRequiredBallots", sql, debugMode, "DailyDetailData");
        }

        //############################################################################################//

        public static string PrintIdRequiredSummaryReport(int electionID, int? pollID)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeIdRequiredSummary", sql, debugMode, "DailyDetailData");
        }

        public static string PrintIdRequiredSummaryReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeIdRequiredSummary", sql, debugMode, "DailyDetailData");
        }

        public static string PrintIdRequiredSummaryReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeIdRequiredSummary", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewIdRequiredSummaryReport(int electionID, int? pollID)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeIdRequiredSummary", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewIdRequiredSummaryReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeIdRequiredSummary", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewIdRequiredSummaryReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = GetDailyIdRequiredBySiteSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeIdRequiredSummary", sql, debugMode, "DailyDetailData");
        }


    }
}
