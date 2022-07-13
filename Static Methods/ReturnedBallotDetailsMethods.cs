using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Methods.ReportSubQueries;
using VoterX.Dialogs;
using VoterX.Utilities.Methods.ReportSubQueries;

namespace VoterX.Methods
{
    public static class ReturnedBallotDetailsMethods
    {
        private static bool debugMode = AppSettings.DebugMode;

        public static string GetReturnedBallotSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity to Date' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.ReturnedBallotReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[PollId] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND LogCode = 9 ";

            return sql;
        }

        public static string GetReturnedBallotSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity for " + reportingDate.ToShortDateString() + "' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.ReturnedBallotReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[PollId] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Specific date
            sql += "AND CAST(ActivityDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND LogCode = 9 ";

            return sql;
        }

        public static string GetReturnedBallotSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += " FROM (" + ReportViewsRevised.ReturnedBallotReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[PollId] = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            // Given date range
            //sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND ActivityDate >= '" + startDate.ToShortDateString() + "' ";
            sql += "AND ActivityDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";
            sql += "AND LogCode = 9 ";

            return sql;
        }

        public static string PrintReturnedBallotReport(int electionID, int? pollID)
        {
            string sql = GetReturnedBallotSQL(electionID, pollID);

            //int RecordCount = VoterMethods.VoterReport.RawSQLQueryCount(sql);

            //if (RecordCount > 50000)
            //{
            //    YesNoDialog newMessage = new YesNoDialog("RECORD COUNT", "A LARGE NUMBER OF RECORDS HAVE BEEN SELECTED\r\nTHIS REPORT MAY RUN SLOWLY\r\nDO YOU WISH TO CONTINUE?");
            //    if (newMessage.ShowDialog() == true)
            //    {
            //        return FastReportsMethods.PrintSilentReport("AbsenteeReturnedBallots01", sql, debugMode, "DailyDetailData");
            //    }
            //    else
            //    {
            //        return "";
            //    }
            //}
            //else
            //{
            return FastReportsMethods.PrintSilentReport("AbsenteeReturnedBallots", sql, debugMode, "DailyDetailData");
            //}
        }

        public static string PrintReturnedBallotReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = GetReturnedBallotSQL(electionID, pollID, reportingDate);

            //return FastReportsMethods.PrintSilentReport("AbsenteeRosterBySite01", sql, debugMode, "DailyDetailData");

            //int RecordCount = VoterMethods.VoterReport.RawSQLQueryCount(sql);

            //if (RecordCount > 50000)
            //{
            //    YesNoDialog newMessage = new YesNoDialog("RECORD COUNT", "A LARGE NUMBER OF RECORDS HAVE BEEN SELECTED\r\nTHIS REPORT MAY RUN SLOWLY\r\nDO YOU WISH TO CONTINUE?");
            //    if (newMessage.ShowDialog() == true)
            //    {
            //        return FastReportsMethods.PrintSilentReport("AbsenteeReturnedBallots01", sql, debugMode, "DailyDetailData");
            //    }
            //    else
            //    {
            //        return "";
            //    }
            //}
            //else
            //{
            return FastReportsMethods.PrintSilentReport("AbsenteeReturnedBallots", sql, debugMode, "DailyDetailData");
            //}
        }

        public static string PrintReturnedBallotReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = GetReturnedBallotSQL(electionID, pollID, startDate, endDate);

            //return FastReportsMethods.PrintSilentReport("AbsenteeRosterBySite01", sql, debugMode, "DailyDetailData");

            //int RecordCount = VoterMethods.VoterReport.RawSQLQueryCount(sql);

            //if (RecordCount > 50000)
            //{
            //    YesNoDialog newMessage = new YesNoDialog("RECORD COUNT", "A LARGE NUMBER OF RECORDS HAVE BEEN SELECTED\r\nTHIS REPORT MAY RUN SLOWLY\r\nDO YOU WISH TO CONTINUE?");
            //    if (newMessage.ShowDialog() == true)
            //    {
            //        return FastReportsMethods.PrintSilentReport("AbsenteeReturnedBallots01", sql, debugMode, "DailyDetailData");
            //    }
            //    else
            //    {
            //        return "";
            //    }
            //}
            //else
            //{
            return FastReportsMethods.PrintSilentReport("AbsenteeReturnedBallots", sql, debugMode, "DailyDetailData");
            //}
        }

        public static void PreviewReturnedBallotReport(int electionID, int? pollID)
        {
            string sql = GetReturnedBallotSQL(electionID, pollID);

            //FastReportsMethods.PreviewReport("AbsenteeRosterBySite01", sql, debugMode, "DailyDetailData");

            //int RecordCount = VoterMethods.VoterReport.RawSQLQueryCount(sql);

            //if (RecordCount > 50000)
            //{
            //    YesNoDialog newMessage = new YesNoDialog("RECORD COUNT", "A LARGE NUMBER OF RECORDS HAVE BEEN SELECTED\r\nTHIS REPORT MAY RUN SLOWLY\r\nDO YOU WISH TO CONTINUE?");
            //    if (newMessage.ShowDialog() == true)
            //    {
            //        FastReportsMethods.PreviewReport("AbsenteeReturnedBallots01", sql, debugMode, "DailyDetailData");
            //    }
            //    else
            //    {
            //        //return "";
            //    }
            //}
            //else
            //{
            FastReportsMethods.PreviewReport("AbsenteeReturnedBallots", sql, debugMode, "DailyDetailData");
            //}
        }

        public static void PreviewReturnedBallotReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = GetReturnedBallotSQL(electionID, pollID, reportingDate);

            //FastReportsMethods.PreviewReport("AbsenteeRosterBySite01", sql, debugMode, "DailyDetailData");

            //int RecordCount = VoterMethods.VoterReport.RawSQLQueryCount(sql);

            //if (RecordCount > 50000)
            //{
            //    YesNoDialog newMessage = new YesNoDialog("RECORD COUNT", "A LARGE NUMBER OF RECORDS HAVE BEEN SELECTED\r\nTHIS REPORT MAY RUN SLOWLY\r\nDO YOU WISH TO CONTINUE?");
            //    if (newMessage.ShowDialog() == true)
            //    {
            //        FastReportsMethods.PreviewReport("AbsenteeReturnedBallots01", sql, debugMode, "DailyDetailData");
            //    }
            //    else
            //    {
            //        //return "";
            //    }
            //}
            //else
            //{
            FastReportsMethods.PreviewReport("AbsenteeReturnedBallots", sql, debugMode, "DailyDetailData");
            //}
        }

        public static void PreviewReturnedBallotReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = GetReturnedBallotSQL(electionID, pollID, startDate, endDate);

            //FastReportsMethods.PreviewReport("AbsenteeRosterBySite01", sql, debugMode, "DailyDetailData");

            //int RecordCount = VoterMethods.VoterReport.RawSQLQueryCount(sql);

            //if (RecordCount > 50000)
            //{
            //    YesNoDialog newMessage = new YesNoDialog("RECORD COUNT", "A LARGE NUMBER OF RECORDS HAVE BEEN SELECTED\r\nTHIS REPORT MAY RUN SLOWLY\r\nDO YOU WISH TO CONTINUE?");
            //    if (newMessage.ShowDialog() == true)
            //    {
            //        FastReportsMethods.PreviewReport("AbsenteeReturnedBallots01", sql, debugMode, "DailyDetailData");
            //    }
            //    else
            //    {
            //        //return "";
            //    }
            //}
            //else
            //{
            FastReportsMethods.PreviewReport("AbsenteeReturnedBallots", sql, debugMode, "DailyDetailData");
            //}
        }
    }
}
