using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Methods.ReportSubQueries;
using VoterX.Utilities.Methods.ReportSubQueries;

namespace VoterX.Methods
{
    public static class BallotsNotMailedMethods
    {
        private static bool debugMode = AppSettings.DebugMode;

        public static string GetBallotsNotMailedSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity to Date' ";
            sql += "FROM (" + ReportViewsRevised.BallotsNotMailed() + ") AS vVotersNotMailed ";
            sql += "WHERE ";
            sql += "ElectionId = " + electionID + " ";

            return sql;
        }

        public static string GetBallotsNotMailedSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity to Date' ";
            sql += "FROM (" + ReportViewsRevised.BallotsNotMailed() + ") AS vVotersNotMailed ";
            sql += "WHERE ";
            sql += "ElectionId = " + electionID + " ";

            return sql;
        }

        public static string GetBallotsNotMailedSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Activity to Date' ";
            sql += "FROM (" + ReportViewsRevised.BallotsNotMailed() + ") AS vVotersNotMailed ";
            sql += "WHERE ";
            sql += "ElectionId = " + electionID + " ";

            return sql;
        }

        public static string PrintBallotsNotMailedReport(int electionID, int? pollID)
        {
            string ballotsql = GetBallotsNotMailedSQL(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeVotersNotMailed", ballotsql, debugMode, "DailyDetailData");
        }

        public static string PrintBallotsNotMailedReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string ballotsql = GetBallotsNotMailedSQL(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeVotersNotMailed", ballotsql, debugMode, "DailyDetailData");
        }

        public static string PrintBallotsNotMailedReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string ballotsql = GetBallotsNotMailedSQL(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeVotersNotMailed", ballotsql, debugMode, "DailyDetailData");
        }

        public static void PreviewBallotsNotMailedReport(int electionID, int? pollID)
        {
            string ballotsql = GetBallotsNotMailedSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeVotersNotMailed", ballotsql, debugMode, "DailyDetailData");
        }

        public static void PreviewBallotsNotMailedReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string ballotsql = GetBallotsNotMailedSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeVotersNotMailed", ballotsql, debugMode, "DailyDetailData");
        }

        public static void PreviewBallotsNotMailedReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string ballotsql = GetBallotsNotMailedSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeVotersNotMailed", ballotsql, debugMode, "DailyDetailData");
        }
    }
}
