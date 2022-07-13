using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Methods.ReportSubQueries;
using VoterX.Utilities.Methods.ReportSubQueries;

namespace VoterX.Methods
{
    public static class ReportSQLMethods
    {
        public static string GetDailyDetailBySiteSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReportPrimary ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            sql += "AND log_code <> 1 "; 

            return sql;
        }

        public static string GetDailyDetailBySiteSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReportPrimary ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            // Specific date
            sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        public static string GetDailyDetailBySiteSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReportPrimary ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            // Given date range
            //sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        public static string GetDailyDetailByBallotStyleSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReportPrimary ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            // Given date range
            //sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        /// <summary>
        /// Print Absentee Details Report for all records from a given site
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <returns></returns>
        public static string GetDailyDetailByBallotStyleSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReportPrimary ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            // No Date give, pull all data
            //sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        /// <summary>
        /// Print Absentee Details Report for a specific day
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <param name="reportingDate"></param>
        /// <returns></returns>
        public static string GetDailyDetailByBallotStyleSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReportPrimary ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            // Specific date
            sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        // Site Summary
        public static string GetDailySummaryBySiteSQL(int electionID, int? pollID) 
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetDailySummaryBySiteSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(ActivityDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetDailySummaryBySiteSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            //sql += "AND activity_date BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            sql += "AND ActivityDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND ActivityDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        // Ballot Style Summary
        public static string GetSummaryByBallotStyleSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        public static string GetSummaryByBallotStyleSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        public static string GetSummaryByBallotStyleSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            // Given date range
            //sql += "AND activity_date BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        // Spoiled Details
        public static string GetSpoiledBallotDetailsSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailySpoiledReport ";
            sql += "FROM (" + ReportViewsRevised.DailySpoiledReportView() + ") AS vDailySpoiledReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            

            return sql;
        }

        public static string GetSpoiledBallotDetailsSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailySpoiledReport ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailySpoiledReportView() + ") AS vDailySpoiledReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            sql += "AND CAST(printed_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            

            return sql;
        }

        public static string GetSpoiledBallotDetailsSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailySpoiledReport ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailySpoiledReportView() + ") AS vDailySpoiledReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            // Given date range
            //sql += "AND printed_date BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            sql += "AND printed_date >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND printed_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            

            return sql;
        }

        public static string GetDailyDetailBySiteSQLGeneral(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetDailyDetailBySiteSQLGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Specific date
            sql += "AND CAST(ActivityDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetDailyDetailBySiteSQLGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            //sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND ActivityDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND ActivityDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetDailyDetailByBallotStyleSQLGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            //sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND ActivityDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND ActivityDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        /// <summary>
        /// Print Absentee Details Report for all records from a given site
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <returns></returns>
        public static string GetDailyDetailByBallotStyleSQLGeneral(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // No Date give, pull all data
            //sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        /// <summary>
        /// Print Absentee Details Report for a specific day
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <param name="reportingDate"></param>
        /// <returns></returns>
        public static string GetDailyDetailByBallotStyleSQLGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Specific date
            sql += "AND CAST(ActivityDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        // Site Summary
        public static string GetDailySummaryBySiteSQLGeneral(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        public static string GetDailySummaryBySiteSQLGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        public static string GetDailySummaryBySiteSQLGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportSubQueries.ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            // Given date range
            //sql += "AND activity_date BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        // Ballot Style Summary
        public static string GetSummaryByBallotStyleSQLGeneral(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetSummaryByBallotStyleSQLGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(ActivityDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetSummaryByBallotStyleSQLGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            //sql += "AND activity_date BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            sql += "AND ActivityDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND ActivityDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        // Spoiled Details
        public static string GetSpoiledBallotDetailsSQLGeneral(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailySpoiledReport ";
            sql += "FROM (" + ReportViewsRevised.DailySpoiledReportView() + ") AS vDailySpoiledReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";            

            return sql;
        }

        public static string GetSpoiledBallotDetailsSQLGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailySpoiledReport ";
            sql += "FROM (" + ReportViewsRevised.DailySpoiledReportView() + ") AS vDailySpoiledReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(PrintedDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";            

            return sql;
        }

        public static string GetSpoiledBallotDetailsSQLGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailySpoiledReport ";
            sql += "FROM (" + ReportViewsRevised.DailySpoiledReportView() + ") AS vDailySpoiledReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            //sql += "AND printed_date BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            sql += "AND PrintedDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND PrintedDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";            

            return sql;
        }

        // Provisional Details
        public static string GetProvisionalDetailsSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyProvisionalReport ";
            sql += "FROM (" + ReportViewsRevised.DailyProvisionalReportView() + ") AS vDailyProvisionalReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";            

            return sql;
        }

        public static string GetProvisionalDetailsSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyProvisionalReport ";
            sql += "FROM (" + ReportViewsRevised.DailyProvisionalReportView() + ") AS vDailyProvisionalReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(LastModified AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";            

            return sql;
        }

        public static string GetProvisionalDetailsSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyProvisionalReport ";
            sql += "FROM (" + ReportViewsRevised.DailyProvisionalReportView() + ") AS vDailyProvisionalReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            //sql += "AND last_modified BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            sql += "AND LastModified >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND LastModified <'" + endDate.AddDays(1).ToShortDateString() + "' ";            

            return sql;
        }

        // Unsigned Details
        public static string GetUnsignedDetailsSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vVoterDetails ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vVoterDetails ";
            sql += "WHERE LogCode = 10 AND ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetUnsignedDetailsSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vVoterDetails ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vVoterDetails ";
            sql += "WHERE LogCode = 10 AND ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(ActivityDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetUnsignedDetailsSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vVoterDetails ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vVoterDetails ";
            sql += "WHERE LogCode = 10 AND ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            //sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            sql += "AND ActivityDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND ActivityDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        // Rejected Application Details
        public static string GetRejectedApplicationDetailsSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vVoterDetails ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vVoterDetails ";
            sql += "WHERE LogCode = 4 AND ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetRejectedApplicationDetailsSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vVoterDetails ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vVoterDetails ";
            sql += "WHERE LogCode = 4 AND ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(ActivityDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }

        public static string GetRejectedApplicationDetailsSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vVoterDetails ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vVoterDetails ";
            sql += "WHERE LogCode = 4 AND ";
            if (pollID != null) sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            sql += "AND ActivityDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND ActivityDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND LogCode <> 1 ";

            return sql;
        }
    }
}
