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
using VoterX.Models;
using System.Windows;
using System.IO;
using VoterX.Methods.ReportSubQueries;
using VoterX.Core.Voters;
using VoterX.Core.Extensions;
using VoterX.Utilities.Methods.ReportSubQueries;
using VoterX.SystemSettings.Enums;

namespace VoterX.Methods
{
    public static class ReportPrintingMethods
    {
        private static bool debugMode = AppSettings.DebugMode;

        //public static string PrintVoterApplication(VoterDataModel voter)
        //{
        //    string sql = "SELECT tblElection.*, ";
        //    sql += "voter_id = " + voter.VoterID.ToString() + ", ";
        //    sql += "full_name = '" + voter.FullName.ReplaceApostrophe() + "', ";
        //    sql += "birth_year = " + voter.DOBYear + ", ";
        //    sql += "printed_date = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
        //    sql += "address = '" + voter.Address1.ReplaceApostrophe() + "', ";
        //    sql += "city_state_zip = '" + voter.City.ReplaceApostrophe() + ", " + voter.State + " " + voter.Zip + "', ";
        //    var electionDay = AppSettings.Election.ElectionDate;
        //    sql += "election_date_long = '" + electionDay.DayOfWeek.ToString() + ", " + electionDay.ToLongDateString() + "', ";
        //    sql += "signature_path = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
        //    sql += "ballot_style_name = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
        //    sql += "election_entity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
        //    sql += "jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
        //    sql += "county_name = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
        //    sql += "poll_id = " + AppSettings.System.SiteID + ", ";
        //    sql += "place_name = '" + AppSettings.System.SiteName.ReplaceApostrophe() + "', ";
        //    sql += "computer = " + AppSettings.System.MachineID + ", ";
        //    sql += "vccmode = 'Early Voter', ";
        //    sql += "spoiled_ballot = 'TRUE', ";
        //    sql += "ballot_number = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + " ";
        //    sql += "FROM tblElection WHERE [election_id] = " + AppSettings.Election.ElectionID + " AND [county_code] = '" + AppSettings.Election.CountyCode + "'";

        //    //TextboxDialog passwordDialog = new TextboxDialog(FastReportsMethods.GetDataSource("Application02"));
        //    //if (passwordDialog.ShowDialog() == true)
        //    //{
        //    //}

        //    //return FastReportsMethods.GetDataSource("Application02");

        //    return FastReportsMethods.PrintSilentReport("Application02", sql, debugMode);

        //    //return FastReportsMethods.PrintSilentReport("VoterStandardReport02");
        //}

        //public static string PrintVoterPermit(VoterDataModel voter)
        //{
        //    string sql = "SELECT tblElection.*, ";
        //    sql += "voter_id = " + voter.VoterID.ToString() + ", ";
        //    sql += "full_name = '" + voter.FullName.ReplaceApostrophe() + "', ";
        //    sql += "birth_year = " + voter.DOBYear + ", ";
        //    sql += "printed_date = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
        //    sql += "address = '" + voter.Address1.ReplaceApostrophe() + "', ";
        //    sql += "city_state_zip = '" + voter.City.ReplaceApostrophe() + ", " + voter.State + " " + voter.Zip + "', ";
        //    var electionDay = AppSettings.Election.ElectionDate;
        //    sql += "election_date_long = '" + electionDay.DayOfWeek.ToString() + ", " + electionDay.ToLongDateString() + "', ";
        //    sql += "signature_path = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
        //    sql += "ballot_style_name = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
        //    sql += "election_entity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
        //    sql += "jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
        //    sql += "county_name = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
        //    sql += "poll_id = " + AppSettings.System.SiteID + ", ";
        //    sql += "place_name = '" + AppSettings.System.SiteName.ReplaceApostrophe() + "', ";
        //    sql += "computer = " + AppSettings.System.MachineID + ", ";
        //    sql += "vccmode = 'Early Voter', ";
        //    sql += "spoiled_ballot = 'TRUE', ";
        //    sql += "ballot_number = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + " ";
        //    sql += "FROM tblElection WHERE [election_id] = " + AppSettings.Election.ElectionID + " AND [county_code] = '" + AppSettings.Election.CountyCode + "'";            

        //    return FastReportsMethods.PrintSilentReport("Permit01", sql, debugMode);
        //}

        public static string PrintBallotStub(VoterDataModel voter)
        {
            string sql = "SELECT Elections.*, ";
            sql += "voter_id = " + voter.VoterID.ToString() + ", ";
            sql += "full_name = '" + voter.FullName.ReplaceApostrophe() + "', ";
            sql += "birth_year = " + voter.DOBYear + ", ";
            sql += "printed_date = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
            sql += "address = '" + voter.Address1.ReplaceApostrophe() + "', ";
            sql += "city_state_zip = '" + voter.City.ReplaceApostrophe() + ", " + voter.State + " " + voter.Zip + "', ";
            var electionDay = AppSettings.Election.ElectionDate;
            sql += "election_date_long = '" + electionDay.DayOfWeek.ToString() + ", " + electionDay.ToLongDateString() + "', ";
            sql += "signature_path = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
            sql += "ballot_style_name = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
            sql += "election_entity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
            sql += "county_name = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "poll_id = " + AppSettings.System.SiteID + ", ";
            sql += "place_name = '" + AppSettings.System.SiteName.ReplaceApostrophe() + "', ";
            sql += "computer = " + AppSettings.System.MachineID + ", ";
            sql += "vccmode = 'Early Voter', ";
            sql += "spoiled_ballot = 'TRUE', ";
            sql += "ballot_number = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + " ";
            sql += "FROM Elections WHERE [ElectionId] = " + AppSettings.Election.ElectionID + " AND [CountyCode] = '" + AppSettings.Election.CountyCode + "'";

            return FastReportsMethods.PrintSilentReport("Stub", sql, debugMode);
        }

        //public static string PrintSignatureForm(VoterDataModel voter)
        //{
        //    string sql = "SELECT tblElection.*, ";
        //    sql += "voter_id = " + voter.VoterID.ToString() + ", ";
        //    sql += "full_name = '" + voter.FullName.ReplaceApostrophe() + "', ";
        //    sql += "birth_year = " + voter.DOBYear + ", ";
        //    sql += "printed_date = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
        //    sql += "address = '" + voter.Address1.ReplaceApostrophe() + "', ";
        //    sql += "city_state_zip = '" + voter.City.ReplaceApostrophe() + ", " + voter.State + " " + voter.Zip + "', ";
        //    var electionDay = AppSettings.Election.ElectionDate;
        //    sql += "election_date_long = '" + electionDay.DayOfWeek.ToString() + ", " + electionDay.ToLongDateString() + "', ";
        //    sql += "signature_path = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
        //    sql += "ballot_style_name = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
        //    sql += "election_entity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
        //    sql += "jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
        //    sql += "county_name = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
        //    sql += "poll_id = " + AppSettings.System.SiteID + ", ";
        //    sql += "place_name = '" + AppSettings.System.SiteName.ReplaceApostrophe() + "', ";
        //    sql += "computer = " + AppSettings.System.MachineID + ", ";
        //    sql += "vccmode = 'Early Voter', ";
        //    sql += "spoiled_ballot = 'TRUE', ";
        //    sql += "ballot_number = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + " ";
        //    sql += "FROM tblElection WHERE [election_id] = " + AppSettings.Election.ElectionID + " AND [county_code] = '" + AppSettings.Election.CountyCode + "'";

        //    return FastReportsMethods.PrintSilentReport("SignatureForm01", sql, debugMode);
        //}

        public static string PrintDailyDetailBCReport(int pollID, DateTime date)
        {
            string sql = "SELECT * FROM  vDailyDetailReportPrimary ";
            sql += "WHERE ";
            sql += "[poll_id] = " + pollID + " AND ";
            sql += "CAST([date_voted] AS DATE) = CAST(CONVERT(DATETIME,'" + date + "') AS DATE) ";
            sql += "AND log_code <> 1 ";

            return FastReportsMethods.PrintSilentReport("DailyDetailReportBC_Primary01", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailyDetailBCReportGeneral(int pollID, DateTime date)
        {
            string sql = "SELECT * FROM  vDailyDetailReport ";
            sql += "WHERE ";
            sql += "[poll_id] = " + pollID + " AND ";
            sql += "CAST([date_voted] AS DATE) = CAST(CONVERT(DATETIME,'" + date + "') AS DATE) ";
            sql += "AND log_code <> 1 ";

            return FastReportsMethods.PrintSilentReport("DailyDetailReportBC_General01", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailyDetailBSReport(int pollID, DateTime date)
        {
            string sql = "SELECT * FROM  vDailyDetailReportPrimary ";
            sql += "WHERE ";
            sql += "[poll_id] = " + pollID + " AND ";
            sql += "CAST([date_voted] AS DATE) = CAST(CONVERT(DATETIME,'" + date + "') AS DATE) ";
            sql += "AND log_code <> 1 ";

            return FastReportsMethods.PrintSilentReport("DailyDetailReportBS_Primary01", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailyDetailBSReportGeneral(int pollID, DateTime date)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Early Voting' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            sql += "PollId = " + pollID + " AND ";
            sql += "CAST(DateVoted AS DATE) = CAST(CONVERT(DATETIME,'" + date + "') AS DATE) ";
            sql += "AND LogCode <> 1 ";

            return FastReportsMethods.PrintSilentReport("DailyDetailReportBS_General", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDetailBSReportGeneral(int pollID, DateTime date)
        {
            string sql = "SELECT *, ";
            sql += "ReportType = 'Early Voting' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            sql += "PollId = " + pollID + " AND ";
            sql += "CAST(DateVoted AS DATE) = CAST(CONVERT(DATETIME,'" + date + "') AS DATE) ";
            sql += "AND LogCode <> 1 ";

            //return FastReportsMethods.PrintSilentReport("DailyDetailReportBS_General01", sql, debugMode, "DailyDetailData");
            FastReportsMethods.PreviewReport("DailyDetailReportBS_General", sql, debugMode, "DailyDetailData");
        }

        public static string PrintZeroEarlyVotingBSReport(int electionID, int pollID)
        {
            string sql = "SELECT * ";
            sql += "FROM ( " + VoterX.Utilities.Methods.ReportSubQueries.ReportViews.SiteSummaryView() + " ) AS vSiteSummary "; 
            sql += "WHERE ";
            sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            sql += "AND log_code <> 1 ";

            return FastReportsMethods.PrintSilentReport("ZeroReportBS_EarlyVoting02", sql, debugMode, "ZeroReportData");
        }

        public static string PrintZeroElectionDayReport(int electionID, int pollID, DateTime electionDay)
        {
            string sql = "SELECT * FROM vSiteSummaryByDate ";
            sql += "WHERE ";
            sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            //sql += "AND date_voted = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + electionDay.ToString() + "'), 101) ";
            sql += "AND log_code <> 1 ";

            return FastReportsMethods.PrintSilentReport("ZeroReportBD_ElectionDay02", sql, debugMode, "ZeroReportData");
        }
        
        public static string PrintDailyProvisionalReport(int electionID, int pollID, DateTime reportingDate)
        {
            string sql = "SELECT * ";
            //string sql = "SELECT * FROM vDailyProvisionalReport ";
            // Replaced stored Views with inline SQL 8/23/2018
            sql += "FROM (" + ReportViewsRevised.DailyProvisionalReportView() + ") AS vDailyProvisionalReport ";
            sql += "WHERE ";
            sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(PrintedDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("DailyProvisionalReport", sql, debugMode, "DailyProvisionalData");
        }

        public static void PreviewDailyProvisionalReport(int electionID, int pollID, DateTime reportingDate)
        {
            string sql = "SELECT * ";
            //string sql = "SELECT * FROM vDailyProvisionalReport ";
            // Replaced stored Views with inline SQL 8/23/2018
            sql += "FROM (" + ReportViewsRevised.DailyProvisionalReportView() + ") AS vDailyProvisionalReport ";
            sql += "WHERE ";
            sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(PrintedDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            //return FastReportsMethods.PrintSilentReport("DailyProvisionalReportBS01", sql, debugMode, "DailyProvisionalData");
            FastReportsMethods.PreviewReport("DailyProvisionalReport", sql, debugMode, "DailyProvisionalData");
        }

        public static string PrintDailySpoiledPrimaryReport(int electionID, int pollID, DateTime reportingDate)
        {
            string sql = "SELECT * FROM vDailySpoiledReportPrimary ";
            sql += "WHERE ";
            sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            sql += "AND CAST(printed_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            

            return FastReportsMethods.PrintSilentReport("DailySpoiledReportBS_Primary01", sql, debugMode, "DailySpoiledData");
        }

        public static string PrintDailySpoiledGeneralReport(int electionID, int pollID, DateTime reportingDate)
        {
            string sql = "SELECT * ";
            //string sql = "SELECT * FROM vDailySpoiledReport ";
            // Replaced stored Views with inline SQL 8/23/2018
            sql += "FROM (" + ReportViewsRevised.DailySpoiledReportView() + ") AS vDailySpoiledReport ";
            sql += "WHERE ";
            sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(PrintedDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("DailySpoiledReport", sql, debugMode, "DailySpoiledData");
        }

        public static void PreviewDailySpoiledGeneralReport(int electionID, int pollID, DateTime reportingDate)
        {
            string sql = "SELECT * ";
            //string sql = "SELECT * FROM vDailySpoiledReport ";
            // Replaced stored Views with inline SQL 8/23/2018 sql += "FROM (" + ReportViewsRevised.DailySpoiledReportView() + ") AS vDailySpoiledReport ";
            sql += "FROM (" + ReportViewsRevised.DailySpoiledReportView() + ") AS vDailySpoiledReport ";
            sql += "WHERE ";
            sql += "PollId = " + pollID + " AND ";
            sql += "ElectionId = " + electionID + " ";
            sql += "AND CAST(PrintedDate AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            //return FastReportsMethods.PrintSilentReport("DailySpoiledReportBS_General01", sql, debugMode, "DailySpoiledData");
            FastReportsMethods.PreviewReport("DailySpoiledReport", sql, debugMode, "DailySpoiledData");
        }

        public static string PrintDailySummaryReport(DateTime date)
        {
            string sql = "SELECT *, ";
            if (AppSettings.System.VCCType == VotingCenterMode.EarlyVoting)
            {
                sql += "VotingType = 'Early Voting' ";
            }
            else
            {
                sql += "VotingType = 'Election Day' ";
            }
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vSiteSummaryByDate ";
            sql += "FROM (" + ReportViewsRevised.SiteSummaryByDateView() + ") AS vSiteSummaryByDate ";
            sql += "WHERE ";
            sql += "PollId = " + AppSettings.System.SiteID + " AND ";
            sql += "ElectionId = " + AppSettings.Election.ElectionID + " ";
            sql += "AND CAST(DateVoted AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), 101) ";
            sql += "AND LogCode <> 1 ";

            //return FastReportsMethods.PrintSilentReport("DailySummaryReportBS02", AppSettings.ReportConfigs.ReportsFolder, AppSettings.Printers, sql, ((App)Application.Current).debugMode, AppSettings.Network, "SummaryReportData");
            return FastReportsMethods.PrintSilentReport("DailySummaryReport", sql, debugMode, "SummaryReportData");
        }

        public static void PreviewDailySummaryReport(DateTime date)
        {
            string sql = "SELECT *, ";
            if (AppSettings.System.VCCType == VotingCenterMode.EarlyVoting)
            {
                sql += "VotingType = 'Early Voting' ";
            }
            else
            {
                sql += "VotingType = 'Election Day' ";
            }
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vSiteSummaryByDate ";
            sql += "FROM (" + ReportViewsRevised.SiteSummaryByDateView() + ") AS vSiteSummaryByDate ";
            sql += "WHERE ";
            sql += "PollId = " + AppSettings.System.SiteID + " AND ";
            sql += "ElectionId = " + AppSettings.Election.ElectionID + " ";
            sql += "AND CAST(DateVoted AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), 101) ";
            sql += "AND LogCode <> 1 ";

            //return FastReportsMethods.PrintSilentReport("DailySummaryReportBS02", AppSettings.ReportConfigs.ReportsFolder, AppSettings.Printers, sql, ((App)Application.Current).debugMode, AppSettings.Network, "SummaryReportData");
            FastReportsMethods.PreviewReport("DailySummaryReport", sql, debugMode, "SummaryReportData");
        }

        public static string PrintEndOfDayReport(DateTime reportingDate) 
        {
            string sql = "SELECT * ";
            //string sql = "SELECT * FROM vEndOfDayReport ";
            // Replaced stored Views with inline SQL 8/23/2018
            sql += "FROM (" + VoterX.Utilities.Methods.ReportSubQueries.ReportViews.EndOfDayReportView() + ") AS vEndOfDayReport ";
            sql += "WHERE ";
            sql += "poll_id = " + AppSettings.System.SiteID + " AND ";
            sql += "county_code = " + AppSettings.Election.CountyCode + " AND ";
            sql += "election_id = " + AppSettings.Election.ElectionID + " ";
            sql += "AND CAST(poll_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            

            string reportfile;
            if (AppSettings.System.VCCType == VotingCenterMode.EarlyVoting)
            {
                reportfile = "EndOfDayReportEV01";
            }
            else
            {
                reportfile = "EndOfDayReportED01";
            }

            //return FastReportsMethods.PrintSilentReport(reportfile, AppSettings.ReportConfigs.ReportsFolder, AppSettings.Printers, sql, ((App)Application.Current).debugMode, AppSettings.Network, "vEndOfDayReport");
            return FastReportsMethods.PrintSilentReport(reportfile, sql, debugMode, "vEndOfDayReport");
        }

        public static void PreviewEndOfDayReport(DateTime reportingDate)
        {
            string sql = "SELECT * ";
            //string sql = "SELECT * FROM vEndOfDayReport ";
            // Replaced stored Views with inline SQL 8/23/2018
            sql += "FROM (" + VoterX.Utilities.Methods.ReportSubQueries.ReportViews.EndOfDayReportView() + ") AS vEndOfDayReport ";
            sql += "WHERE ";
            sql += "poll_id = " + AppSettings.System.SiteID + " AND ";
            sql += "county_code = " + AppSettings.Election.CountyCode + " AND ";
            sql += "election_id = " + AppSettings.Election.ElectionID + " ";
            sql += "AND CAST(poll_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            

            string reportfile;
            if (AppSettings.System.VCCType == VotingCenterMode.EarlyVoting)
            {
                reportfile = "EndOfDayReportEV01";
            }
            else
            {
                reportfile = "EndOfDayReportED01";
            }

            //return FastReportsMethods.PreviewReport(reportfile, AppSettings.ReportConfigs.ReportsFolder, AppSettings.Printers, sql, ((App)Application.Current).debugMode, AppSettings.Network, "vEndOfDayReport");
            FastReportsMethods.PreviewReport(reportfile, sql, debugMode, "vEndOfDayReport");
        }

        //public static string PrintVoterAbsenteeApplication(VoterDataModel voter)
        //{
        //    string sql = "SELECT tblElection.*, ";
        //    sql += "voter_id = " + voter.VoterID.ToString() + ", ";
        //    sql += "full_name = '" + voter.FullName.ReplaceApostrophe() + "', ";
        //    sql += "birth_year = " + voter.DOBYear + ", ";
        //    sql += "printed_date = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
        //    sql += "address = '" + voter.DeliveryAddress1.ReplaceApostrophe() + "', ";
        //    sql += "address2 = '" + voter.Address2.ReplaceApostrophe() + "', ";
        //    sql += "city_state_zip = '" + voter.DeliveryCity.ReplaceApostrophe() + ", " + voter.DeliveryState + " " + voter.DeliveryZip + "', ";
        //    var electionDay = AppSettings.Election.ElectionDate;
        //    sql += "election_date_long = '" + electionDay.DayOfWeek.ToString() + ", " + electionDay.ToLongDateString() + "', ";
        //    sql += "signature_path = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
        //    sql += "ballot_style_name = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
        //    sql += "election_entity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
        //    sql += "jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
        //    sql += "county_name = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
        //    sql += "poll_id = " + AppSettings.System.SiteID + ", ";
        //    sql += "place_name = '" + AppSettings.System.SiteName.ReplaceApostrophe() + "', ";
        //    sql += "computer = " + AppSettings.System.MachineID + ", ";
        //    sql += "vccmode = 'Early Voter', ";
        //    sql += "spoiled_ballot = 'TRUE', ";
        //    sql += "ballot_number = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + " ";
        //    sql += "FROM tblElection WHERE [election_id] = " + AppSettings.Election.ElectionID + " AND [county_code] = '" + AppSettings.Election.CountyCode + "'";

        //    return FastReportsMethods.PrintSilentReport("Application02", sql, debugMode);
        //}

        public static string PrintVoterAbsenteeApplicationPre(VoterDataModel voter)
        {
            string sql = "SELECT Elections.*, ";
            sql += "VoterId = " + voter.VoterID.ToString() + ", ";
            sql += "FullName = '" + voter.FullName.ReplaceApostrophe() + "', ";
            sql += "BirthYear = " + voter.DOBYear + ", ";
            sql += "PrintedDate = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
            sql += "Address = '" + voter.Address1.ReplaceApostrophe() + "', ";
            sql += "Address2 = '" + voter.Address2.ReplaceApostrophe() + "', ";
            sql += "CityStateZip = '" + voter.City.ReplaceApostrophe() + ", " + voter.State + " " + voter.Zip + "', ";
            sql += "MailAddress = '" + voter.DeliveryAddress1.ReplaceApostrophe() + "', ";
            sql += "MailAddress2 = '" + voter.DeliveryAddress2.ReplaceApostrophe() + "', ";
            sql += "MailCityStateZip = '" + voter.DeliveryCity.ReplaceApostrophe() + ", " + voter.DeliveryState + " " + voter.DeliveryZip + "', ";

            //var electionDay = AppSettings.Election.ElectionDate;
            //String electionDay = AppSettings.Election.ElectionDate.Date.ToLongDateString().Replace(AppSettings.Election.ElectionDate.DayOfWeek.ToString() + ", ", "");

            sql += "ElectionTitle = '" + AppSettings.Absentee.ElectionTitle.ReplaceApostrophe() + "', ";
            sql += "ElectionDateLong = '" + AppSettings.Absentee.ElectionDateLong.ReplaceApostrophe() + "', ";
            sql += "SignaturePath = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
            sql += "BallotStyleName = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
            sql += "ElectionEntity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "Jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
            sql += "AlternateCountyName = '" + AppSettings.Absentee.CountyName.ReplaceApostrophe() + "', ";
            sql += "ClerkName = '" + AppSettings.Absentee.ClerkName.ReplaceApostrophe() + "', ";
            sql += "PollId = " + AppSettings.Absentee.SiteID + ", ";
            sql += "PlaceName = '" + AppSettings.Absentee.SiteName.RemoveSpecialCharacters() + "', ";
            sql += "Computer = " + AppSettings.System.MachineID + ", ";
            sql += "VCCMode = 'Absentee', ";
            sql += "SpoiledBallot = 'TRUE', ";
            sql += "BallotNumber = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + ", ";
            //if (VoterMethods.Locations.Exists(0) == true)
            //{
            //var site = VoterMethods.Locations.Query(0).Where(l => l.poll_id == AppSettings.System.SiteID).FirstOrDefault();
            sql += "SiteAddress = '" + AppSettings.Absentee.ApplicationReturnAddress1.ReplaceApostrophe() + "', ";
            if (AppSettings.Absentee.ApplicationReturnAddress2.ReplaceApostrophe() == null || AppSettings.Absentee.ApplicationReturnAddress2.ReplaceApostrophe() == "")
            {
                sql += "SiteAddress2 = '', ";
            }
            else
            {
                sql += "SiteAddress2 = char(10) + char(13) + '" + AppSettings.Absentee.ApplicationReturnAddress2.ReplaceApostrophe() + "', ";
            }
            sql += "SiteCSZ = '" + AppSettings.Absentee.ApplicationReturnCity.ReplaceApostrophe() + " " + AppSettings.Absentee.ApplicationReturnState.ReplaceApostrophe() + " " + AppSettings.Absentee.ApplicationReturnZip + "', ";
            //}            

            sql += "ElectionDateLongSP = '" + AppSettings.Absentee.ElectionDateLongSpanish.ReplaceApostrophe() + "', ";
            sql += "ElectionNameSP = '" + AppSettings.Absentee.ElectionTitleSpanish.ReplaceApostrophe() + "' ";
            sql += "FROM Elections WHERE [ElectionId] = " + AppSettings.Election.ElectionID + " AND [CountyCode] = '" + AppSettings.Election.CountyCode + "'";

            return FastReportsMethods.PrintSilentReport("ApplicationPreprint", sql, debugMode, AppSettings.Printers, AppSettings.Printers.ApplicationPrinter, AppSettings.Printers.AppBin);
        }

        public static string PrintRejectedBallotForm(VoterDataModel voter, string rejectionDescription)
        {
            string sql = "SELECT Elections.*, ";
            sql += "VoterId = " + voter.VoterID.ToString() + ", ";
            sql += "FullName = '" + voter.FullName.ReplaceApostrophe() + "', ";
            sql += "BirthYear = " + voter.DOBYear + ", ";
            sql += "PrintedDate = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
            sql += "Address = '" + voter.Address1.ReplaceApostrophe() + "', ";
            sql += "Address2 = '" + voter.Address2.ReplaceApostrophe() + "', ";
            sql += "CityStateZip = '" + voter.City.ReplaceApostrophe() + ", " + voter.State + " " + voter.Zip + "', ";
            sql += "MailAddress = '" + voter.DeliveryAddress1.ReplaceApostrophe() + "', ";
            sql += "MailAddress2 = '" + voter.DeliveryAddress2.ReplaceApostrophe() + "', ";
            sql += "MailCityStateZip = '" + voter.DeliveryCity.ReplaceApostrophe() + ", " + voter.DeliveryState + " " + voter.DeliveryZip + "', ";

            //var electionDay = AppSettings.Election.ElectionDate;
            //String electionDay = AppSettings.Election.ElectionDate.Date.ToLongDateString().Replace(AppSettings.Election.ElectionDate.DayOfWeek.ToString() + ", ", "");

            sql += "ElectionTitle = '" + AppSettings.Absentee.ElectionTitle.ReplaceApostrophe() + "', ";
            sql += "ElectionDateLong = '" + AppSettings.Absentee.ElectionDateLong.ReplaceApostrophe() + "', ";
            sql += "SignaturePath = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
            sql += "BallotStyleName = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
            sql += "ElectionEntity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "Jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
            sql += "AlternateCountyName = '" + AppSettings.Absentee.CountyName.ReplaceApostrophe() + "', ";
            sql += "ClerkName = '" + AppSettings.Absentee.ClerkName.ReplaceApostrophe() + "', ";
            sql += "PollId = " + AppSettings.Absentee.SiteID + ", ";
            sql += "PlaceName = '" + AppSettings.Absentee.SiteName.RemoveSpecialCharacters() + "', ";
            sql += "Computer = " + AppSettings.System.MachineID + ", ";
            sql += "VCCMode = 'Absentee', ";
            sql += "SpoiledBallot = 'TRUE', ";
            sql += "BallotNumber = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + ", ";
            sql += "RejectedReason = '" + rejectionDescription + "', ";
            //if (VoterMethods.Locations.Exists(0) == true)
            //{
            //var site = VoterMethods.Locations.Query(0).Where(l => l.poll_id == AppSettings.System.SiteID).FirstOrDefault();
            sql += "SiteAddress = '" + AppSettings.Absentee.ApplicationReturnAddress1.ReplaceApostrophe() + "', ";
            if (AppSettings.Absentee.ApplicationReturnAddress2.ReplaceApostrophe() == null || AppSettings.Absentee.ApplicationReturnAddress2.ReplaceApostrophe() == "")
            {
                sql += "SiteAddress2 = '', ";
            }
            else
            {
                sql += "SiteAddress2 = char(10) + char(13) + '" + AppSettings.Absentee.ApplicationReturnAddress2.ReplaceApostrophe() + "', ";
            }
            sql += "SiteCSZ = '" + AppSettings.Absentee.ApplicationReturnCity.ReplaceApostrophe() + " " + AppSettings.Absentee.ApplicationReturnState.ReplaceApostrophe() + " " + AppSettings.Absentee.ApplicationReturnZip + "', ";
            //}            

            sql += "ElectionDateLongSP = '" + AppSettings.Absentee.ElectionDateLongSpanish.ReplaceApostrophe() + "', ";
            sql += "ElectionNameSP = '" + AppSettings.Absentee.ElectionTitleSpanish.ReplaceApostrophe() + "' ";
            sql += "FROM Elections WHERE [ElectionId] = " + AppSettings.Election.ElectionID + " AND [CountyCode] = '" + AppSettings.Election.CountyCode + "'";

            return FastReportsMethods.PrintSilentReport("BallotRejectionForm", sql, debugMode, AppSettings.Printers, AppSettings.Printers.ApplicationPrinter, AppSettings.Printers.AppBin);
        }

        public static string PrintRejectedBallot(VoterDataModel voter, string rejectionDescription, int? style)
        {
            string sql = "SELECT Elections.*, ";
            sql += "voter_id = " + voter.VoterID.ToString() + ", ";
            sql += "full_name = '" + voter.FullName.ReplaceApostrophe() + "', ";
            sql += "birth_year = " + voter.DOBYear + ", ";
            sql += "printed_date = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
            sql += "address = '" + voter.DeliveryAddress1.ReplaceApostrophe() + "', ";
            sql += "address2 = '" + voter.Address2.ReplaceApostrophe() + "', ";
            sql += "city_state_zip = '" + voter.DeliveryCity.ReplaceApostrophe() + ", " + voter.DeliveryState + " " + voter.DeliveryZip + "', ";
            var electionDay = AppSettings.Election.ElectionDate;
            sql += "election_date_long = '" + electionDay.DayOfWeek.ToString() + ", " + electionDay.ToLongDateString() + "', ";
            sql += "signature_path = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
            sql += "ballot_style_name = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
            sql += "election_entity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
            sql += "county_name = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "poll_id = " + AppSettings.System.SiteID + ", ";
            sql += "place_name = '" + AppSettings.System.SiteName.ReplaceApostrophe() + "', ";
            sql += "computer = " + AppSettings.System.MachineID + ", ";
            sql += "vccmode = 'Early Voter', ";
            sql += "spoiled_ballot = 'TRUE', ";
            sql += "ballot_number = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + ", ";
            sql += "rejected_reason = '" + rejectionDescription + "' ";
            sql += "FROM Elections WHERE [ElectionId] = " + AppSettings.Election.ElectionID;

            string reportFile = "BallotRejectionForm";

            switch(style)
            {
                case 1:
                    reportFile = "BallotRejectionForm";
                    break;
                case 2:
                    reportFile = "BallotRejectionForm";
                    break;
                case 3:
                    reportFile = "BallotRejectionForm";
                    break;
                default:
                    reportFile = "BallotRejectionForm";
                    break;
            }

            return FastReportsMethods.PrintSilentReport(reportFile, sql, debugMode);
        }

        //public static string PrintVoterEarlyVotingApplicationPre(VoterDataModel voter)
        //{
        //    string sql = "SELECT tblElection.*, ";
        //    sql += "voter_id = " + voter.VoterID.ToString() + ", ";
        //    sql += "full_name = '" + voter.FullName.ReplaceApostrophe() + "', ";
        //    sql += "birth_year = " + voter.DOBYear + ", ";
        //    sql += "printed_date = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
        //    sql += "address = '" + voter.Address1.ReplaceApostrophe() + "', ";
        //    sql += "address2 = '" + voter.Address2.ReplaceApostrophe() + "', ";
        //    sql += "city_state_zip = '" + voter.City.ReplaceApostrophe() + ", " + voter.State + " " + voter.Zip + "', ";
        //    sql += "mail_address = '" + voter.DeliveryAddress1.ReplaceApostrophe() + "', ";
        //    sql += "mail_address2 = '" + voter.DeliveryAddress2.ReplaceApostrophe() + "', ";
        //    sql += "mail_city_state_zip = '" + voter.DeliveryCity.ReplaceApostrophe() + ", " + voter.DeliveryState + " " + voter.DeliveryZip + "', ";

        //    //var electionDay = AppSettings.Election.ElectionDate;
        //    //String electionDay = AppSettings.Election.ElectionDate.Date.ToLongDateString().Replace(AppSettings.Election.ElectionDate.DayOfWeek.ToString() + ", ", "");

        //    sql += "election_title = '" + AppSettings.Absentee.ElectionTitle.ReplaceApostrophe() + "', ";
        //    sql += "election_date_long = '" + AppSettings.Absentee.ElectionDateLong.ReplaceApostrophe() + "', ";
        //    sql += "signature_path = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
        //    sql += "ballot_style_name = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
        //    sql += "election_entity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
        //    sql += "jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
        //    sql += "alternate_county_name = '" + AppSettings.Absentee.CountyName.ReplaceApostrophe() + "', ";
        //    sql += "clerk_name = '" + AppSettings.Absentee.ClerkName.ReplaceApostrophe() + "', ";
        //    sql += "poll_id = " + AppSettings.Absentee.SiteID + ", ";
        //    sql += "place_name = '" + AppSettings.Absentee.SiteName.RemoveSpecialCharacters() + "', ";
        //    sql += "computer = " + AppSettings.System.MachineID + ", ";
        //    sql += "vccmode = 'Early Voting', ";
        //    sql += "spoiled_ballot = 'TRUE', ";
        //    sql += "ballot_number = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + ", ";
        //    //if (VoterMethods.Locations.Exists(0) == true)
        //    //{
        //    //var site = VoterMethods.Locations.Query(0).Where(l => l.poll_id == AppSettings.System.SiteID).FirstOrDefault();
        //    sql += "site_address = '" + AppSettings.Absentee.ReturnAddress1.ReplaceApostrophe() + "', ";
        //    if (AppSettings.Absentee.ReturnAddress2.ReplaceApostrophe() == null || AppSettings.Absentee.ReturnAddress2.ReplaceApostrophe() == "")
        //    {
        //        sql += "site_address2 = '', ";
        //    }
        //    else
        //    {
        //        sql += "site_address2 = char(10) + char(13) + '" + AppSettings.Absentee.ReturnAddress2.ReplaceApostrophe() + "', ";
        //    }
        //    sql += "site_csz = '" + AppSettings.Absentee.ReturnCity.ReplaceApostrophe() + " NM " + AppSettings.Absentee.ReturnZip + "', ";
        //    //}            

        //    sql += "election_date_long_sp = '" + AppSettings.Absentee.ElectionDateLongSpanish.ReplaceApostrophe() + "', ";
        //    sql += "election_name_sp = '" + AppSettings.Absentee.ElectionTitleSpanish.ReplaceApostrophe() + "' ";
        //    sql += "FROM tblElection WHERE [election_id] = " + AppSettings.Election.ElectionID + " AND [county_code] = '" + AppSettings.Election.CountyCode + "'";


        //    return FastReportsMethods.PrintSilentReport("ApplicationPreprint02", sql, debugMode, AppSettings.Printers, AppSettings.Printers.ApplicationPrinter, AppSettings.Printers.AppBin);
        //}

        public static string PrintVoterMailingLabel(VoterDataModel voter)
        {
            string sql = "SELECT Elections.*, ";
            sql += "VoterId = " + voter.VoterID.ToString() + ", ";
            sql += "FullName = '" + voter.FullName.ReplaceApostrophe() + "', ";
            sql += "BirthYear = " + voter.DOBYear + ", ";
            sql += "PrintedDate = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
            sql += "Address = '" + voter.DeliveryAddress1.ReplaceApostrophe() + "', ";
            sql += "Address2 = '" + voter.DeliveryAddress2.ReplaceApostrophe() + "', ";
            sql += "CityStateZip = '" + voter.DeliveryCity.ReplaceApostrophe() + ", " + voter.DeliveryState + " " + voter.DeliveryZip + "', ";
            var electionDay = AppSettings.Election.ElectionDate;
            sql += "ElectionDateLong = '" + electionDay.DayOfWeek.ToString() + ", " + electionDay.ToLongDateString() + "', ";
            sql += "SignaturePath = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
            sql += "BallotStyleName = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
            if (AppSettings.Ballots.IncludePartyOnLabel == true)
            {
                sql += "Party = '" + voter.Party + "', ";
            }
            else
            {
                sql += "Party = '', ";
            }
            sql += "ElectionEntity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "Jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
            sql += "CountyName = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "PollId = " + AppSettings.System.SiteID + ", ";
            //sql += "place_name = '" + AppSettings.System.SiteName + "', ";
            sql += "Computer = " + AppSettings.System.MachineID + ", ";
            sql += "VCCMode = 'Early Voter', ";
            sql += "SpoiledBallot = 'TRUE', ";
            sql += "BallotNumber = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + " ";
            sql += "FROM Elections WHERE [ElectionId] = " + AppSettings.Election.ElectionID + " AND [CountyCode] = '" + AppSettings.Election.CountyCode + "'";

            return FastReportsMethods.PrintSilentLabels("AddressLabelSingle", sql, debugMode, "ApplicationData");
        }

        public static string PrintVoterMailingLabels(VoterDataModel voter)
        {
            string sql = "SELECT Elections.*, ";
            sql += "VoterId = " + voter.VoterID.ToString() + ", ";
            sql += "FullName = '" + voter.FullName.ReplaceApostrophe() + "', ";
            sql += "BirthYear = " + voter.DOBYear + ", ";
            sql += "PrintedDate = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
            sql += "Address = '" + voter.DeliveryAddress1.ReplaceApostrophe() + "', ";
            sql += "Address2 = '" + voter.DeliveryAddress2.ReplaceApostrophe() + "', ";
            sql += "CityStateZip = '" + voter.DeliveryCity.ReplaceApostrophe() + ", " + voter.DeliveryState + " " + voter.DeliveryZip + "', ";
            sql += "Country = '" + voter.DeliveryCountry.ReplaceApostrophe() + "', ";
            var electionDay = AppSettings.Election.ElectionDate;
            sql += "ElectionDateLong = '" + electionDay.DayOfWeek.ToString() + ", " + electionDay.ToLongDateString() + "', ";
            sql += "SignaturePath = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
            sql += "BallotStyleName = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
            if (AppSettings.Ballots.IncludePartyOnLabel == true)
            {
                sql += "Party = '" + voter.Party + "', ";
            }
            else
            {
                sql += "Party = '', ";
            }
            sql += "ElectionEntity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "Jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
            sql += "CountyName = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "PollId = " + AppSettings.System.SiteID + ", ";
            //sql += "place_name = '" + AppSettings.System.SiteName + "', ";
            sql += "Computer = " + AppSettings.System.MachineID + ", ";
            sql += "VCCMode = 'Early Voter', ";
            sql += "SpoiledBallot = 'TRUE', ";
            sql += "BallotNumber = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + " ";
            sql += "FROM Elections WHERE [ElectionId] = " + AppSettings.Election.ElectionID + " AND [CountyCode] = '" + AppSettings.Election.CountyCode + "'";

            FastReportsMethods.PrintSilentLabels("AddressLabelSingleBarcode", sql, debugMode, "ApplicationData");
            return FastReportsMethods.PrintSilentLabels("AddressLabelSingleAddress", sql, debugMode, "ApplicationData");
        }

        public static string PrintVoterMailingLabelsIMB(VoterDataModel voter)
        {
            string sql = "SELECT Elections.*, ";
            sql += "VoterId = " + voter.VoterID.ToString() + ", ";
            sql += "FullName = '" + voter.FullName.ReplaceApostrophe() + "', ";
            sql += "BirthYear = " + voter.DOBYear + ", ";
            sql += "PrintedDate = CONVERT(DATETIME,'" + DateTime.Now.ToString() + "'), ";
            sql += "Address = '" + voter.DeliveryAddress1.ReplaceApostrophe() + "', ";
            sql += "Address2 = '" + voter.DeliveryAddress2.ReplaceApostrophe() + "', ";
            sql += "CityStateZip = '" + voter.DeliveryCity.ReplaceApostrophe() + ", " + voter.DeliveryState + " " + voter.DeliveryZip + " " + voter.DeliveryCountry + "', ";
            sql += "Country = '" + voter.DeliveryCountry.ReplaceApostrophe() + "', ";
            var electionDay = AppSettings.Election.ElectionDate;
            sql += "ElectionDateLong = '" + electionDay.DayOfWeek.ToString() + ", " + electionDay.ToLongDateString() + "', ";
            sql += "SignaturePath = '" + AppSettings.System.SignatureFolder + "\\" + voter.VoterID.ToString() + ".jpg', ";
            sql += "BallotStyleName = '" + voter.BallotStyle.ReplaceApostrophe() + "', ";
            if (AppSettings.Ballots.IncludePartyOnLabel == true)
            {
                sql += "Party = '" + voter.Party + "', ";
            }
            else
            {
                sql += "Party = '', ";
            }
            sql += "ElectionEntity = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "Jurisdiction = '" + voter.PrecinctName.ReplaceApostrophe() + "', ";
            sql += "CountyName = '" + AppSettings.Election.ElectionEntity.ReplaceApostrophe() + "', ";
            sql += "PollId = " + AppSettings.System.SiteID + ", ";
            //sql += "place_name = '" + AppSettings.System.SiteName + "', ";
            sql += "Computer = " + AppSettings.System.MachineID + ", ";
            sql += "VCCMode = 'Early Voter', ";
            sql += "SpoiledBallot = 'TRUE', ";
            sql += "BallotNumber = " + (voter.BallotNumber == null ? "NULL" : voter.BallotNumber.ToString().ReplaceApostrophe()) + ", ";
            sql += "AlternateCountyName = '" + AppSettings.Absentee.CountyName.ReplaceApostrophe() + "', ";
            sql += "ClerkName = '" + AppSettings.Absentee.ClerkName.ReplaceApostrophe() + "', ";
            sql += "SiteAddress = '" + AppSettings.Absentee.BallotReturnAddress1.ReplaceApostrophe() + "',  ";
            sql += "SiteAddress2 = '" + AppSettings.Absentee.BallotReturnAddress2.ReplaceApostrophe() + "',  ";
            sql += "SiteCSZ = '" + AppSettings.Absentee.BallotReturnCity.ReplaceApostrophe() + ", " + AppSettings.Absentee.BallotReturnState + " " + AppSettings.Absentee.BallotReturnZip + "', ";
            sql += "OutGoingIMB = '" + voter.OutGoingIMB + "',  ";
            sql += "InComingIMB = '" + voter.InComingIMB + "' ";
            sql += "FROM Elections WHERE [ElectionId] = " + AppSettings.Election.ElectionID + " AND [CountyCode] = '" + AppSettings.Election.CountyCode + "'";

            FastReportsMethods.PrintSilentLabels("AddressLabelSingleAddressIMB", sql, debugMode, "ApplicationData");
            FastReportsMethods.PrintSilentLabels("AddressLabelSingleBarcode", sql, debugMode, "ApplicationData");
            return FastReportsMethods.PrintSilentLabels("AddressLabelSingleReturnIMB", sql, debugMode, "ApplicationData");
        }

        public static string PrintAbsenteeRegister(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT ";
            sql += "[vRegister].*, ";
            sql += "[StartDate] = '" + startDate.ToString() + "', ";
            sql += "[EndDate] = '" + endDate.ToString() + "', ";
            sql += "[ReportHeader] = 'ABSENT VOTER REGISTER', ";
            sql += "[ReportSubheader] = 'Absentee' ";
            //sql += "FROM [dbo].[vRegister] ";
            sql += "FROM (SELECT ";
            //sql += "ROW_NUMBER() OVER(ORDER BY UNLISTED_REGISTER.[OrderByDate], UNLISTED_REGISTER.[LastName], UNLISTED_REGISTER.[VoterId]) AS [RegisterNumber], ";
            sql += "ROW_NUMBER() OVER(ORDER BY UNLISTED_REGISTER.[OrderByDate], UNLISTED_REGISTER.[LastName], UNLISTED_REGISTER.[VoterId]) AS [RegisterNumber], ";
            sql += "UNLISTED_REGISTER.* ";
            sql += "FROM (" + ReportViewsRevised.RegisterView() + ") AS UNLISTED_REGISTER ";            
            sql += "WHERE ";
            sql += "(LogCode IN (6,9,10,14)) OR [AbsenteeIssued] = 1 ";
            sql += ") AS vRegister ";
            sql += "WHERE ";
            sql += "OrderByDate >= '" + startDate.ToString() + "' ";
            sql += "AND OrderByDate <'" + endDate.AddDays(1).ToShortDateString() + "' "; 

            return FastReportsMethods.PrintSilentReport("VoterXRegister", sql, debugMode, "vRegister");
        }

        public static string PrintEarlyVotingRegister(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT ";
            sql += "[vRegister].*, ";
            sql += "[StartDate] = '" + startDate.ToString() + "', ";
            sql += "[EndDate] = '" + endDate.ToString() + "', ";
            sql += "[ReportHeader] = 'EARLY VOTING REGISTER', ";
            sql += "[ReportSubheader] = 'Absentee' ";
            //sql += "FROM [dbo].[vRegister] ";
            sql += "FROM (SELECT ";
            sql += "ROW_NUMBER() OVER(ORDER BY UNLISTED_REGISTER.[OrderByDate], UNLISTED_REGISTER.[LastName], UNLISTED_REGISTER.[VoterId]) AS [RegisterNumber], ";
            sql += "UNLISTED_REGISTER.* ";
            sql += "FROM (" + ReportViewsRevised.RegisterView() + ") AS UNLISTED_REGISTER ";
            sql += "WHERE ";
            sql += "(LogCode = 8) ";
            sql += ") AS vRegister ";
            sql += "WHERE ";
            sql += "OrderByDate >= '" + startDate.ToString() + "' ";
            sql += "AND OrderByDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return FastReportsMethods.PrintSilentReport("VoterXRegister", sql, debugMode, "vRegister");
        }

        public static string PrintCombinedRegister(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT [vRegister].*, ";
            sql += "[start_date] = '" + startDate.ToString() + "', ";
            sql += "[end_date] = '" + endDate.ToString() + "', ";
            sql += "[report_header] = 'ABSENTEE & EARLY VOTING REGISTER', ";
            sql += "[report_subheader] = 'Absentee & Early Voting' ";
            //sql += "FROM [dbo].[vRegister] ";
            sql += "FROM (" + VoterX.Utilities.Methods.ReportSubQueries.ReportViews.RegisterView() + ") AS vRegister ";
            sql += "WHERE ";
            //sql += "([log_code] = 8) ";
            //sql += "AND ";
            //sql += "([order_by_date] BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "')";
            sql += "order_by_date >= '" + startDate.ToString() + "' ";
            sql += "AND order_by_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return FastReportsMethods.PrintSilentReport("AvRegister01", sql, debugMode, "vRegister");
        }

        public static void PreviewAbsenteeRegister(DateTime startDate, DateTime endDate) 
        {
            string sql = "SELECT ";
            sql += "[vRegister].*, ";
            sql += "[StartDate] = '" + startDate.ToString() + "', ";
            sql += "[EndDate] = '" + endDate.ToString() + "', ";
            sql += "[ReportHeader] = 'ABSENT VOTER REGISTER', ";
            sql += "[ReportSubheader] = 'Absentee' ";
            //sql += "FROM [dbo].[vRegister] ";
            sql += "FROM (SELECT ";
            //sql += "ROW_NUMBER() OVER(ORDER BY UNLISTED_REGISTER.[OrderByDate], UNLISTED_REGISTER.[VoterId], UNLISTED_REGISTER.[LastName] ) AS [RegisterNumber], ";
            // Sort changed for Soccoro 12/11/2020
            sql += "ROW_NUMBER() OVER(ORDER BY UNLISTED_REGISTER.[OrderByDate], UNLISTED_REGISTER.[LastName], UNLISTED_REGISTER.[VoterId]) AS [RegisterNumber], ";
            sql += "UNLISTED_REGISTER.* ";
            sql += "FROM (" + ReportViewsRevised.RegisterView() + ") AS UNLISTED_REGISTER ";
            sql += "WHERE ";
            sql += "(LogCode IN (6,9,10,14)) OR [AbsenteeIssued] = 1 ";
            sql += ") AS vRegister ";
            sql += "WHERE ";
            sql += "OrderByDate >= '" + startDate.ToString() + "' ";
            sql += "AND OrderByDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            FastReportsMethods.PreviewReport("VoterXRegister", sql, debugMode, "vRegister");
        }

        public static void PreviewEarlyVotingRegister(DateTime startDate, DateTime endDate) 
        {
            string sql = "SELECT ";
            sql += "[vRegister].*, ";
            sql += "[StartDate] = '" + startDate.ToString() + "', ";
            sql += "[EndDate] = '" + endDate.ToString() + "', ";
            sql += "[ReportHeader] = 'EARLY VOTING REGISTER', ";
            sql += "[ReportSubheader] = 'Absentee' ";
            //sql += "FROM [dbo].[vRegister] ";
            sql += "FROM (SELECT ";
            sql += "ROW_NUMBER() OVER(ORDER BY UNLISTED_REGISTER.[OrderByDate], UNLISTED_REGISTER.[LastName], UNLISTED_REGISTER.[VoterId]) AS [RegisterNumber], ";
            sql += "UNLISTED_REGISTER.* ";
            sql += "FROM (" + ReportViewsRevised.RegisterView() + ") AS UNLISTED_REGISTER ";
            sql += "WHERE ";
            sql += "(LogCode = 8) ";
            sql += ") AS vRegister ";
            sql += "WHERE ";
            sql += "OrderByDate >= '" + startDate.ToString() + "' ";
            sql += "AND OrderByDate <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            FastReportsMethods.PreviewReport("VoterXRegister", sql, debugMode, "vRegister");
        }

        public static void PreviewCombinedRegister(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT [vRegister].*, ";
            sql += "[start_date] = '" + startDate.ToString() + "', ";
            sql += "[end_date] = '" + endDate.ToString() + "', ";
            sql += "[report_header] = 'ABSENTEE & EARLY VOTING REGISTER', ";
            sql += "[report_subheader] = 'Absentee & Early Voting' ";
            //sql += "FROM [dbo].[vRegister] ";
            sql += "FROM (" + VoterX.Utilities.Methods.ReportSubQueries.ReportViews.RegisterView() + ") AS vRegister ";
            sql += "WHERE ";
            //sql += "([log_code] = 8) ";
            //sql += "AND ";
            //sql += "([order_by_date] BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "')";
            sql += "order_by_date >= '" + startDate.ToString() + "' ";
            sql += "AND order_by_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            FastReportsMethods.PreviewReport("AvRegister01", sql, debugMode, "vRegister");
        }

        public static string PrintBatchDetailReport(int? SiteId, int? Computer)
        {
            string sql = "SELECT * ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            sql += "(LogCode = 5) ";
            if (SiteId != null)
            {
                sql += "AND (PollId = " + SiteId.ToString() + ") ";
            }
            if (Computer != null)
            {
                sql += "AND (Computer = " + AppSettings.System.MachineID.ToString() + ") ";
            }
            return FastReportsMethods.PrintSilentReport("BatchPrintingReport", sql, debugMode, "DailyDetailData");
        }

        public static string PrintBatchLabels(int ballotStyleID)
        {
            return PrintBatchLabels(ballotStyleID, null);
        }

        public static string PrintBatchLabels(int ballotStyleID, int? siteId) 
        {
            string sql = "SELECT *, ";
            sql += "ShowParty = '" + AppSettings.Ballots.IncludePartyOnLabel.ToString() + "', ";
            sql += "report_type = 'Activity to Date' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            sql += "(LogCode = 5) AND ";
            if (siteId == null)
            {
                sql += "(PollId = " + AppSettings.Absentee.SiteID.ToString() + ") AND ";
            }
            else
            {
                sql += "(PollId = " + siteId.ToString() + ") AND ";
            }
            sql += "(BallotStyleId = " + ballotStyleID.ToString() + ") ";

            //return FastReportsMethods.PrintSilentLabels("AddressLabel01", sql, debugMode, "ApplicationData");
            return FastReportsMethods.PrintSilentReport("AbsenteeSpecialLabelReport", sql, debugMode, "DailyDetailData");
        }

        public static string PrintBatchLabels(List<int> ballotStyles)
        {
            return PrintBatchLabels(ballotStyles, null);
        }

        public static string PrintBatchLabels(List<int> ballotStyles, int? siteId)
        {
            string sql = "SELECT *, ";
            sql += "ShowParty = '" + AppSettings.Ballots.IncludePartyOnLabel.ToString() + "', ";
            sql += "report_type = 'Activity to Date' ";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + ReportViewsRevised.DailyDetailReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            sql += "(LogCode = 5) AND ";
            if (siteId == null)
            {
                sql += "(PollId = " + AppSettings.Absentee.SiteID.ToString() + ") AND ";
            }
            else
            {
                sql += "(PollId = " + siteId.ToString() + ") AND ";
            }

            string styleList = "";
            foreach(var style in ballotStyles)
            {
                styleList += style.ToString() + ",";
            }
            styleList += "0";
            sql += "(BallotStyleId IN (" + styleList + ")) ";

            //return FastReportsMethods.PrintSilentLabels("AddressLabel01", sql, debugMode, "ApplicationData");
            return FastReportsMethods.PrintSilentReport("AbsenteeSpecialLabelReport", sql, debugMode, "DailyDetailData");
        }

        //public static string PrintMailingLabel(VoterDataModel voter)
        //{
        //    string sql = "SELECT *, ";
        //    sql += "mail_address = '" + voter.DeliveryAddress1.ReplaceApostrophe() + "', ";
        //    sql += "mail_city_state_zip = '" + voter.DeliveryCity.ReplaceApostrophe() + ", " + voter.DeliveryState + " " + voter.DeliveryZip + "' ";
        //    // Replaced stored Views with inline SQL 8/23/2018
        //    //sql += "FROM vDailyDetailReport ";
        //    sql += "FROM (" + ReportViews.DailyDetailReportView() + ") AS vDailyDetailReport ";
        //    sql += "WHERE ";
        //    sql += "WHERE ";
        //    sql += "([voter_id] = " + voter.VoterID.ToString() + ") ";

        //    return FastReportsMethods.PrintSilentLabels("AddressLabel02", sql, debugMode, "ApplicationData"); 
        //}

        /// <summary>
        /// Print Absentee Details Report for a given date range
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static string PrintDailyDetailBySiteReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQL(electionID, pollID, startDate, endDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            //if(pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// Given date range
            ////sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            ////sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            //sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            //sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterBySite01", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for a given date range
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static string PrintDailyDetailBySiteReportGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQLGeneral(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterBySite", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for all records from a given site
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <returns></returns>
        public static string PrintDailyDetailBySiteReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQL(electionID, pollID);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity to Date'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// No Date give, pull all data
            ////sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            ////sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterBySite01", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for all records from a given site
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <returns></returns>
        public static string PrintDailyDetailBySiteReportGeneral(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQLGeneral(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterBySite", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for a specific day
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <param name="reportingDate"></param>
        /// <returns></returns>
        public static string PrintDailyDetailBySiteReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQL(electionID, pollID, reportingDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// Specific date
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterBySite", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for a specific day
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <param name="reportingDate"></param>
        /// <returns></returns>
        public static string PrintDailyDetailBySiteReportGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailBySiteSQLGeneral(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterBySite", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for a given date range
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static string PrintDailyDetailByBallotStyleReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQL(electionID, pollID, startDate, endDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            ////if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// Given date range
            ////sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            ////sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            //sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            //sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for a given date range
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static string PrintDailyDetailByBallotStyleReportGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQLGeneral(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for all records from a given site
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <returns></returns>
        public static string PrintDailyDetailByBallotStyleReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQL(electionID, pollID);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity to Date'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            ////if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// No Date give, pull all data
            ////sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            ////sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for all records from a given site
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <returns></returns>
        public static string PrintDailyDetailByBallotStyleReportGeneral(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQLGeneral(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for a specific day
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <param name="reportingDate"></param>
        /// <returns></returns>
        public static string PrintDailyDetailByBallotStyleReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQL(electionID, pollID, reportingDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            ////if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// Specific date
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        /// <summary>
        /// Print Absentee Details Report for a specific day
        /// </summary>
        /// <param name="electionID"></param>
        /// <param name="pollID"></param>
        /// <param name="reportingDate"></param>
        /// <returns></returns>
        public static string PrintDailyDetailByBallotStyleReportGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailyDetailByBallotStyleSQLGeneral(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeRosterByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        // Site Summary
        public static string PrintDailySummaryBySiteReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailySummaryBySiteSQL(electionID, pollID);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity to Date'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityBySite", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailySummaryBySiteReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailySummaryBySiteSQL(electionID, pollID, reportingDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityBySite", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailySummaryBySiteReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailySummaryBySiteSQL(electionID, pollID, startDate, endDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// Given date range
            ////sql += "AND activity_date BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            //sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            //sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityBySite", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailySummaryBySiteReportGeneral(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetDailySummaryBySiteSQLGeneral(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityBySite01", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailySummaryBySiteReportGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetDailySummaryBySiteSQLGeneral(electionID, pollID, reportingDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityBySite01", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailySummaryBySiteReportGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetDailySummaryBySiteSQLGeneral(electionID, pollID, startDate, endDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// Given date range
            ////sql += "AND activity_date BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            //sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            //sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityBySite01", sql, debugMode, "DailyDetailData");
        }

        // Ballot Style Summary
        public static string PrintSummaryByBallotStyleReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQL(electionID, pollID);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity to Date'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            ////if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        public static string PrintSummaryByBallotStyleReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQL(electionID, pollID, reportingDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            ////if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        public static string PrintSummaryByBallotStyleReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQL(electionID, pollID, startDate, endDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            //sql += "FROM vDailyDetailReportPrimary ";
            //sql += "WHERE ";
            ////if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// Given date range
            ////sql += "AND activity_date BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            //sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            //sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityByBallotStyle01", sql, debugMode, "DailyDetailData");
        }

        // Ballot Style Summary
        public static string PrintSummaryByBallotStyleReportGeneral(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQLGeneral(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        public static string PrintSummaryByBallotStyleReportGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQLGeneral(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        public static string PrintSummaryByBallotStyleReportGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetSummaryByBallotStyleSQLGeneral(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeActivityByBallotStyle", sql, debugMode, "DailyDetailData");
        }

        // Spoiled Details
        public static string PrintSpoiledBallotDetailsReport(int electionID, int? pollID)
        {
            string votersql = ReportSQLMethods.GetSpoiledBallotDetailsSQL(electionID, pollID);

            //string electionsql = "SELECT TOP 1 *, report_type = 'Activity to Date' FROM vElections";

            //return FastReportsMethods.PrintSilentReportDual("AbsenteeSpoiledBallots02", debugMode, votersql, "DailySpoiledData", electionsql, "ElectionData");
            return FastReportsMethods.PrintSilentReport("AbsenteeSpoiledBallots", votersql, debugMode, "DailySpoiledData");
        }

        public static string PrintSpoiledBallotDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string votersql = ReportSQLMethods.GetSpoiledBallotDetailsSQLGeneral(electionID, pollID, reportingDate);

            string electionsql = "SELECT TOP 1 *, ";
            electionsql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            electionsql += "FROM vElections";

            //return FastReportsMethods.PrintSilentReportDual("AbsenteeSpoiledBallots02", debugMode, votersql, "DailySpoiledData", electionsql, "ElectionData");
            return FastReportsMethods.PrintSilentReport("AbsenteeSpoiledBallots", votersql, debugMode, "DailySpoiledData");
        }

        public static string PrintSpoiledBallotDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string votersql = ReportSQLMethods.GetSpoiledBallotDetailsSQLGeneral(electionID, pollID, startDate, endDate);

            string electionsql = "SELECT TOP 1 *, ";
            electionsql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            electionsql += "FROM vElections";

            //return FastReportsMethods.PrintSilentReportDual("AbsenteeSpoiledBallots02", debugMode, votersql, "DailySpoiledData", electionsql, "ElectionData");
            return FastReportsMethods.PrintSilentReport("AbsenteeSpoiledBallots", votersql, debugMode, "DailySpoiledData");
        }

        public static string PrintSpoiledBallotDetailsReportGeneral(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetSpoiledBallotDetailsSQLGeneral(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeSpoiledBallots01", sql, debugMode, "DailySpoiledData");
        }

        public static string PrintSpoiledBallotDetailsReportGeneral(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetSpoiledBallotDetailsSQLGeneral(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeSpoiledBallots01", sql, debugMode, "DailySpoiledData");
        }

        public static string PrintSpoiledBallotDetailsReportGeneral(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetSpoiledBallotDetailsSQLGeneral(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeSpoiledBallots01", sql, debugMode, "DailySpoiledData");
        }

        // Provisional Details
        public static string PrintProvisionalDetailsReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetProvisionalDetailsSQL(electionID, pollID);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity to Date'";
            //sql += "FROM vDailyProvisionalReport ";
            //sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";

            return FastReportsMethods.PrintSilentReport("AbsenteeProvisionalBallots", sql, debugMode, "DailyProvisionalData");
        }

        public static string PrintProvisionalDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetProvisionalDetailsSQL(electionID, pollID, reportingDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            //sql += "FROM vDailyProvisionalReport ";
            //sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //sql += "AND CAST(last_modified AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("AbsenteeProvisionalBallots", sql, debugMode, "DailyProvisionalData");
        }

        public static string PrintProvisionalDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetProvisionalDetailsSQL(electionID, pollID, startDate, endDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            //sql += "FROM vDailyProvisionalReport ";
            //sql += "WHERE ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// Given date range
            ////sql += "AND last_modified BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "' ";
            //sql += "AND last_modified >= '" + startDate.ToShortDateString() + "' ";
            //sql += "AND last_modified <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return FastReportsMethods.PrintSilentReport("AbsenteeProvisionalBallots", sql, debugMode, "DailyProvisionalData");
        }

        // Unsigned Details
        public static string PrintUnsignedDetailsReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetUnsignedDetailsSQL(electionID, pollID);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity to Date'";
            //sql += "FROM vVoterDetails ";
            //sql += "WHERE [log_code] = 10 AND ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";

            return FastReportsMethods.PrintSilentReport("AbsenteeUnsignedEnvelopes", sql, debugMode, "DailyUnsignedData");
        }

        public static string PrintUnsignedDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetUnsignedDetailsSQL(electionID, pollID, reportingDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            //sql += "FROM vVoterDetails ";
            //sql += "WHERE [log_code] = 10 AND ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("AbsenteeUnsignedEnvelopes", sql, debugMode, "DailyUnsignedData");
        }

        public static string PrintUnsignedDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetUnsignedDetailsSQL(electionID, pollID, startDate, endDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            //sql += "FROM vVoterDetails ";
            //sql += "WHERE [log_code] = 10 AND ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// Given date range
            ////sql += "AND activity_date BETWEEN '" + startDate.ToString() + "' AND '" + endDate.ToString() + "' ";
            //sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            //sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return FastReportsMethods.PrintSilentReport("AbsenteeUnsignedEnvelopes", sql, debugMode, "DailyUnsignedData");
        }

        // Rejected Application Details
        public static string PrintRejectedApplicationDetailsReport(int electionID, int? pollID)
        {
            string sql = ReportSQLMethods.GetRejectedApplicationDetailsSQL(electionID, pollID); 
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity to Date'";
            //sql += "FROM vVoterDetails ";
            //sql += "WHERE [log_code] = 4 AND ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";

            return FastReportsMethods.PrintSilentReport("AbsenteeRejectedApplications", sql, debugMode, "DailyRejectedData");
        }

        public static string PrintRejectedApplicationDetailsReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = ReportSQLMethods.GetRejectedApplicationDetailsSQL(electionID, pollID, reportingDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            //sql += "FROM vVoterDetails ";
            //sql += "WHERE [log_code] = 4 AND ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";

            return FastReportsMethods.PrintSilentReport("AbsenteeRejectedApplications", sql, debugMode, "DailyRejectedData");
        }

        public static string PrintRejectedApplicationDetailsReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = ReportSQLMethods.GetRejectedApplicationDetailsSQL(electionID, pollID, startDate, endDate);
            //string sql = "SELECT *, ";
            //sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            //sql += "FROM vVoterDetails ";
            //sql += "WHERE [log_code] = 4 AND ";
            //if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            //sql += "election_id = " + electionID + " ";
            //// Given date range
            //sql += "AND activity_date >= '" + startDate.ToShortDateString() + "' ";
            //sql += "AND activity_date <'" + endDate.AddDays(1).ToShortDateString() + "' ";

            return FastReportsMethods.PrintSilentReport("AbsenteeRejectedApplications", sql, debugMode, "DailyRejectedData");
        }

        //######################################################################################
        // REPORT WIZARD
        public static string PrintCustomSpecialReport(ReportWizardQueryModel reportOptions)
        {
            // Create SQL Select statement from Wizard selections
            string sql = "SELECT *, ";
            //sql += "report_type = '" + reportOptions.ReportName.ReplaceApostrophe() + "' ";
            sql += "FROM vDailyDetailReportPrimary ";
            sql += "WHERE ";            
            sql += "activity_date >= '" + reportOptions.BeginningDate.ToString() + "' ";
            sql += "AND activity_date < '" + reportOptions.EndingDate.AddDays(1).ToShortDateString() + "' ";
            // Log Codes
            if (reportOptions.LogCodes.Count() > 0)
            {
                string logCodeList = "";
                foreach (int logCode in reportOptions.LogCodes)
                {
                    logCodeList += logCode.ToString() + ",";
                }
                logCodeList += "NULL";
                sql += "AND [log_code] IN (" + logCodeList + ") ";
            }
            // Ballot Styles
            if (reportOptions.BallotStyles.Count() > 0)
            {
                string styleList = "";
                foreach (int styleID in reportOptions.BallotStyles)
                {
                    styleList += styleID.ToString() + ",";
                }
                styleList += "NULL";
                sql += "AND [ballot_style_id] IN (" + styleList + ") ";
            }
            // Parties
            if (reportOptions.Parties.Count() > 0)
            {
                string partyList = "";
                foreach (string party in reportOptions.Parties)
                {
                    partyList += "'" + party + "',";
                }
                partyList += "NULL";
                sql += "AND [party] IN (" + partyList + ") ";
            }
            // Jurisdictions
            if (reportOptions.Jurisdictions.Count() > 0)
            {
                string jurisdictionList = "";
                foreach (string jurisdictionID in reportOptions.Jurisdictions)
                {
                    jurisdictionList += "'" + jurisdictionID + "',";
                }
                jurisdictionList += "NULL";
                sql += "AND [precinct_part_id] IN (SELECT [precinct_part_id] FROM [dbo].[tblJurisdictionPrecinct] WHERE [jurisdiction_id] IN (" + jurisdictionList + ")) ";
            }
            // Polling Sites
            if (reportOptions.PollSites.Count() > 0)
            {
                string siteList = "";
                foreach(int siteID in reportOptions.PollSites)
                {
                    siteList += siteID.ToString() + ",";
                }
                siteList += "NULL";
                sql += "AND [poll_id] IN (" + siteList + ") ";
            }
            // Id Required
            if (reportOptions.IdRequired == true)
            {
                sql += "AND IdRequired = 1 ";
            }

            // Pass query to to Special report
            return FastReportsMethods.PrintSilentReport("AbsenteeSpecialReport01", sql, debugMode, "DailyDetailData");
        }
         
        public static string PrintCustomSummaryReport(ReportWizardQueryModel reportOptions)
        {
            // Create SQL Select statement from Wizard selections
            string sql = "SELECT *, ";
            //sql += "report_type = '" + reportOptions.ReportName.ReplaceApostrophe() + "' ";
            sql += "FROM vDailyDetailReportPrimary ";
            sql += "WHERE ";
            sql += "activity_date >= '" + reportOptions.BeginningDate.ToString() + "' ";
            sql += "AND activity_date < '" + reportOptions.EndingDate.AddDays(1).ToShortDateString() + "' ";
            // Log Codes
            if (reportOptions.LogCodes.Count() > 0)
            {
                string logCodeList = "";
                foreach (int logCode in reportOptions.LogCodes)
                {
                    logCodeList += logCode.ToString() + ",";
                }
                logCodeList += "NULL";
                sql += "AND [log_code] IN (" + logCodeList + ") ";
            }
            // Ballot Styles
            if (reportOptions.BallotStyles.Count() > 0)
            {
                string styleList = "";
                foreach (int styleID in reportOptions.BallotStyles)
                {
                    styleList += styleID.ToString() + ",";
                }
                styleList += "NULL";
                sql += "AND [ballot_style_id] IN (" + styleList + ") ";
            }
            // Parties
            if (reportOptions.Parties.Count() > 0)
            {
                string partyList = "";
                foreach (string party in reportOptions.Parties)
                {
                    partyList += "'" + party + "',";
                }
                partyList += "NULL";
                sql += "AND [party] IN (" + partyList + ") ";
            }
            // Jurisdictions
            if (reportOptions.Jurisdictions.Count() > 0)
            {
                string jurisdictionList = "";
                foreach (string jurisdictionID in reportOptions.Jurisdictions)
                {
                    jurisdictionList += "'" + jurisdictionID + "',";
                }
                jurisdictionList += "NULL";
                sql += "AND [precinct_part_id] IN (SELECT [precinct_part_id] FROM [dbo].[tblJurisdictionPrecinct] WHERE [jurisdiction_id] IN (" + jurisdictionList + ")) ";
            }
            // Polling Sites
            if (reportOptions.PollSites.Count() > 0)
            {
                string siteList = "";
                foreach (int siteID in reportOptions.PollSites)
                {
                    siteList += siteID.ToString() + ",";
                }
                siteList += "NULL";
                sql += "AND [poll_id] IN (" + siteList + ") ";
            }
            // Id Required
            if (reportOptions.IdRequired == true)
            {
                sql += "AND IdRequired = 1 ";
            }

            // Pass query to to Special report
            return FastReportsMethods.PrintSilentReport("AbsenteeSpecialSummaryReport02", sql, debugMode, "DailyDetailData");
        }

        public static string PrintCustomLabelReport(ReportWizardQueryModel reportOptions)
        {
            // Create SQL Select statement from Wizard selections
            string sql = "SELECT *, ";
            //sql += "report_type = '" + reportOptions.ReportName.ReplaceApostrophe() + "' ";
            sql += "FROM vDailyDetailReportPrimary ";
            sql += "WHERE ";
            sql += "activity_date >= '" + reportOptions.BeginningDate.ToString() + "' ";
            sql += "AND activity_date < '" + reportOptions.EndingDate.AddDays(1).ToShortDateString() + "' ";
            // Log Codes
            if (reportOptions.LogCodes.Count() > 0)
            {
                string logCodeList = "";
                foreach (int logCode in reportOptions.LogCodes)
                {
                    logCodeList += logCode.ToString() + ",";
                }
                logCodeList += "NULL";
                sql += "AND [log_code] IN (" + logCodeList + ") ";
            }
            // Ballot Styles
            if (reportOptions.BallotStyles.Count() > 0)
            {
                string styleList = "";
                foreach (int styleID in reportOptions.BallotStyles)
                {
                    styleList += styleID.ToString() + ",";
                }
                styleList += "NULL";
                sql += "AND [ballot_style_id] IN (" + styleList + ") ";
            }
            // Parties
            if (reportOptions.Parties.Count() > 0)
            {
                string partyList = "";
                foreach (string party in reportOptions.Parties)
                {
                    partyList += "'" + party + "',";
                }
                partyList += "NULL";
                sql += "AND [party] IN (" + partyList + ") ";
            }
            // Jurisdictions
            if (reportOptions.Jurisdictions.Count() > 0)
            {
                string jurisdictionList = "";
                foreach (string jurisdictionID in reportOptions.Jurisdictions)
                {
                    jurisdictionList += "'" + jurisdictionID + "',";
                }
                jurisdictionList += "NULL";
                sql += "AND [precinct_part_id] IN (SELECT [precinct_part_id] FROM [dbo].[tblJurisdictionPrecinct] WHERE [jurisdiction_id] IN (" + jurisdictionList + ")) ";
            }
            // Polling Sites
            if (reportOptions.PollSites.Count() > 0)
            {
                string siteList = "";
                foreach (int siteID in reportOptions.PollSites)
                {
                    siteList += siteID.ToString() + ",";
                }
                siteList += "NULL";
                sql += "AND [poll_id] IN (" + siteList + ") ";
            }
            // Id Required
            if (reportOptions.IdRequired == true)
            {
                sql += "AND IdRequired = 1 ";
            }

            // Pass query to to Special report
            return FastReportsMethods.PrintSilentReport("AbsenteeSpecialLabelReport01", sql, debugMode, "DailyDetailData");
        }

        public static string PrintCustomSpecialReportGeneral(ReportWizardQueryModel reportOptions)
        {
            // Create SQL Select statement from Wizard selections
            string sql = "SELECT *, ";
            sql += "report_type = '" + reportOptions.ReportName.ReplaceApostrophe() + "', ";
            sql += SortSpecialReportBy(reportOptions) + ", ";
            sql += GroupSpecialReportBy(reportOptions);
            if (reportOptions.IncludeInactive == false)
            {
                sql += "FROM (" + ReportViewsRevised.DailyDetailReportView((int)AppSettings.Election.ElectionType) + ") AS vDailyDetailReport ";                
            }
            else
            {
                sql += "FROM (" + ReportViewsRevised.DailyInactiveDetailView((int)AppSettings.Election.ElectionType) + ") AS vDailyDetailReport ";
            }
            //sql += "FROM vDailyDetailReport ";
            sql += "WHERE ";
            sql += "ElectionId = " + AppSettings.Election.ElectionID.ToString() + " ";
            if (reportOptions.IncludeInactive == false)
            {
                sql += "AND ActivityDate >= '" + reportOptions.BeginningDate.ToString() + "' ";
                sql += "AND ActivityDate < '" + reportOptions.EndingDate.AddDays(1).ToShortDateString() + "' ";
            }
            // Log Codes
            if (reportOptions.IncludeInactive == false) sql += "AND LogCode <> 1 ";
            if (reportOptions.LogCodes.Count() > 0)
            {
                string logCodeList = "";
                foreach (int logCode in reportOptions.LogCodes)
                {
                    logCodeList += logCode.ToString() + ",";
                }
                logCodeList += "NULL";
                sql += "AND LogCode IN (" + logCodeList + ") ";
            }
            // Ballot Styles
            if (reportOptions.BallotStyles.Count() > 0)
            {
                string styleList = "";
                foreach (int styleID in reportOptions.BallotStyles)
                {
                    styleList += styleID.ToString() + ",";
                }
                styleList += "NULL";
                if (reportOptions.IncludeInactive == false)
                {
                    sql += "AND BallotStyleId IN (" + styleList + ") ";
                }
                else
                {
                    sql += "AND AssignedBallotStyleId IN (" + styleList + ") ";
                }
            }
            // Parties
            if (reportOptions.Parties.Count() > 0)
            {
                string partyList = "";
                foreach (string party in reportOptions.Parties)
                {
                    partyList += "'" + party + "',";
                }
                partyList += "NULL";
                sql += "AND Party IN (" + partyList + ") ";
            }
            // Jurisdictions
            if (reportOptions.Jurisdictions.Count() > 0)
            {
                string jurisdictionList = "";
                foreach (string jurisdictionID in reportOptions.Jurisdictions)
                {
                    jurisdictionList += "'" + jurisdictionID + "',";
                }
                jurisdictionList += "NULL";
                sql += "AND PrecinctPartId IN (SELECT [PrecinctPartId] FROM [dbo].[JurisdictionPrecincts] WHERE JurisdictionId IN (" + jurisdictionList + ")) ";
            }
            // Polling Sites
            if (reportOptions.PollSites.Count() > 0)
            {
                string siteList = "";
                foreach (int siteID in reportOptions.PollSites)
                {
                    siteList += siteID.ToString() + ",";
                }
                siteList += "NULL";
                sql += "AND PollId IN (" + siteList + ") ";
            }
            // Id Required
            if (reportOptions.IdRequired == true)
            {
                sql += "AND IdRequired = 1 ";
            }

            // Pass query to to Special report
            return FastReportsMethods.PrintSilentReport("AbsenteeSpecialReport", sql, debugMode, "DailyDetailData");
        }

        public static string PrintCustomSummaryReportGeneral(ReportWizardQueryModel reportOptions) 
        {
            // Create SQL Select statement from Wizard selections
            string sql = "SELECT *, ";
            sql += "report_type = '" + reportOptions.ReportName.ReplaceApostrophe() + "', ";
            sql += SortSpecialReportBy(reportOptions) + ", ";
            sql += GroupSpecialSummaryReportBy(reportOptions);
            if (reportOptions.IncludeInactive == false)
            {
                sql += "FROM (" + ReportViewsRevised.DailyDetailReportView((int)AppSettings.Election.ElectionType) + ") AS vDailyDetailReport ";
            }
            else
            {
                sql += "FROM (" + ReportViewsRevised.DailyInactiveDetailView((int)AppSettings.Election.ElectionType) + ") AS vDailyDetailReport ";
            }
            //sql += "FROM vDailyDetailReport ";
            sql += "WHERE ";
            sql += "ElectionId = " + AppSettings.Election.ElectionID.ToString() + " ";
            if (reportOptions.IncludeInactive == false)
            {
                sql += "AND ActivityDate >= '" + reportOptions.BeginningDate.ToString() + "' ";
                sql += "AND ActivityDate < '" + reportOptions.EndingDate.AddDays(1).ToShortDateString() + "' ";
            }
            // Log Codes
            if (reportOptions.IncludeInactive == false) sql += "AND LogCode <> 1 ";
            if (reportOptions.LogCodes.Count() > 0)
            {
                string logCodeList = "";
                foreach (int logCode in reportOptions.LogCodes)
                {
                    logCodeList += logCode.ToString() + ",";
                }
                logCodeList += "NULL";
                sql += "AND LogCode IN (" + logCodeList + ") ";
            }
            // Ballot Styles
            if (reportOptions.BallotStyles.Count() > 0)
            {
                string styleList = "";
                foreach (int styleID in reportOptions.BallotStyles)
                {
                    styleList += styleID.ToString() + ",";
                }
                styleList += "NULL";
                if (reportOptions.IncludeInactive == false)
                {
                    sql += "AND BallotStyleId IN (" + styleList + ") ";
                }
                else
                {
                    sql += "AND AssignedBallotStyleId IN (" + styleList + ") ";
                }
            }
            // Parties
            if (reportOptions.Parties.Count() > 0)
            {
                string partyList = "";
                foreach (string party in reportOptions.Parties)
                {
                    partyList += "'" + party + "',";
                }
                partyList += "NULL";
                sql += "AND Party IN (" + partyList + ") ";
            }
            // Jurisdictions
            if (reportOptions.Jurisdictions.Count() > 0)
            {
                string jurisdictionList = "";
                foreach (string jurisdictionID in reportOptions.Jurisdictions)
                {
                    jurisdictionList += "'" + jurisdictionID + "',";
                }
                jurisdictionList += "NULL";
                sql += "AND PrecinctPartId IN (SELECT [PrecinctPartId] FROM [dbo].[JurisdictionPrecincts] WHERE JurisdictionId IN (" + jurisdictionList + ")) ";
            }
            // Polling Sites
            if (reportOptions.PollSites.Count() > 0)
            {
                string siteList = "";
                foreach (int siteID in reportOptions.PollSites)
                {
                    siteList += siteID.ToString() + ",";
                }
                siteList += "NULL";
                sql += "AND PollId IN (" + siteList + ") ";
            }
            // Id Required
            if (reportOptions.IdRequired == true)
            {
                sql += "AND IdRequired = 1 ";
            }

            string summary = "SELECT ";
            summary += "report_type, ";
            summary += "GroupingNumber, ";
            summary += "ElectionId, ";
            summary += "ElectionName, ";
            summary += "ElectionDate, ";
            summary += "ElectionDateLong, ";
            summary += "CountyCode, ";
            summary += "CountyName, ";
            summary += "COUNT(*) AS[VoterCount] ";
            summary += "FROM ( " + sql + " ) AS SubSumaryReport ";
            summary += "GROUP BY ";
            summary += "report_type, ";
            summary += "GroupingNumber, ";
            summary += "ElectionId, ";
            summary += "ElectionName, ";
            summary += "ElectionDate, ";
            summary += "ElectionDateLong, ";
            summary += "CountyCode, ";
            summary += "CountyName ";

            // Pass query to to Special report
            return FastReportsMethods.PrintSilentReport("AbsenteeSpecialSummaryReport", summary, debugMode, "DailyDetailData");
        }

        public static string PrintCustomLabelReportGeneral(ReportWizardQueryModel reportOptions) 
        {
            // Create SQL Select statement from Wizard selections
            string sql = "SELECT *, ";
            sql += "ShowParty = '" + AppSettings.Ballots.IncludePartyOnLabel.ToString() + "', ";
            sql += "report_type = '" + reportOptions.ReportName.ReplaceApostrophe() + "' ";
            if (reportOptions.IncludeInactive == false)
            {
                sql += "FROM (" + ReportViewsRevised.DailyDetailReportView((int)AppSettings.Election.ElectionType) + ") AS vDailyDetailReport ";
            }
            else
            {
                sql += "FROM (" + ReportViewsRevised.DailyInactiveDetailView((int)AppSettings.Election.ElectionType) + ") AS vDailyDetailReport ";
            }
            //sql += "FROM vDailyDetailReport ";
            sql += "WHERE ";
            sql += "ElectionId = " + AppSettings.Election.ElectionID.ToString() + " ";
            if (reportOptions.IncludeInactive == false)
            {
                sql += "AND ActivityDate >= '" + reportOptions.BeginningDate.ToString() + "' ";
                sql += "AND ActivityDate < '" + reportOptions.EndingDate.AddDays(1).ToShortDateString() + "' ";
            }
            // Log Codes
            if (reportOptions.IncludeInactive == false) sql += "AND LogCode <> 1 ";
            if (reportOptions.LogCodes.Count() > 0)
            {
                string logCodeList = "";
                foreach (int logCode in reportOptions.LogCodes)
                {
                    logCodeList += logCode.ToString() + ",";
                }
                logCodeList += "NULL";
                sql += "AND LogCode IN (" + logCodeList + ") ";
            }
            // Ballot Styles
            if (reportOptions.BallotStyles.Count() > 0)
            {
                string styleList = "";
                foreach (int styleID in reportOptions.BallotStyles)
                {
                    styleList += styleID.ToString() + ",";
                }
                styleList += "NULL";
                if (reportOptions.IncludeInactive == false)
                {
                    sql += "AND BallotStyleId IN (" + styleList + ") ";
                }
                else
                {
                    sql += "AND AssignedBallotStyleId IN (" + styleList + ") ";
                }
            }
            // Parties
            if (reportOptions.Parties.Count() > 0)
            {
                string partyList = "";
                foreach (string party in reportOptions.Parties)
                {
                    partyList += "'" + party + "',";
                }
                partyList += "NULL";
                sql += "AND Party IN (" + partyList + ") ";
            }
            // Jurisdictions
            if (reportOptions.Jurisdictions.Count() > 0)
            {
                string jurisdictionList = "";
                foreach (string jurisdictionID in reportOptions.Jurisdictions)
                {
                    jurisdictionList += "'" + jurisdictionID + "',";
                }
                jurisdictionList += "NULL";
                sql += "AND PrecinctPartId IN (SELECT [PrecinctPartId] FROM [dbo].[JurisdictionPrecincts] WHERE JurisdictionId IN (" + jurisdictionList + ")) ";
            }
            // Polling Sites
            if (reportOptions.PollSites.Count() > 0)
            {
                string siteList = "";
                foreach (int siteID in reportOptions.PollSites)
                {
                    siteList += siteID.ToString() + ",";
                }
                siteList += "NULL";
                sql += "AND PollId IN (" + siteList + ") ";
            }
            // Id Required
            if (reportOptions.IdRequired == true)
            {
                sql += "AND IdRequired = 1 ";
            }

            // Pass query to to Special report
            return FastReportsMethods.PrintSilentReport("AbsenteeSpecialLabelReport", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewCustomSpecialReportGeneral(ReportWizardQueryModel reportOptions)
        {
            // Create SQL Select statement from Wizard selections
            string sql = "SELECT *, ";
            sql += "report_type = '" + reportOptions.ReportName.ReplaceApostrophe() + "', ";
            sql += SortSpecialReportBy(reportOptions) + ", ";
            sql += GroupSpecialReportBy(reportOptions);
            if (reportOptions.IncludeInactive == false)
            {
                sql += "FROM (" + ReportViewsRevised.DailyDetailReportView((int)AppSettings.Election.ElectionType) + ") AS vDailyDetailReport ";
            }
            else
            {
                sql += "FROM (" + ReportViewsRevised.DailyInactiveDetailView((int)AppSettings.Election.ElectionType) + ") AS vDailyDetailReport ";
            }
            //sql += "FROM vDailyDetailReport ";
            sql += "WHERE ";
            sql += "ElectionId = " + AppSettings.Election.ElectionID.ToString() + " ";
            if (reportOptions.IncludeInactive == false)
            {
                sql += "AND ActivityDate >= '" + reportOptions.BeginningDate.ToString() + "' ";
                sql += "AND ActivityDate < '" + reportOptions.EndingDate.AddDays(1).ToShortDateString() + "' ";
            }
            // Log Codes
            if (reportOptions.IncludeInactive == false) sql += "AND LogCode <> 1 ";
            if (reportOptions.LogCodes.Count() > 0)
            {
                string logCodeList = "";
                foreach (int logCode in reportOptions.LogCodes)
                {
                    logCodeList += logCode.ToString() + ",";
                }
                logCodeList += "NULL";
                sql += "AND LogCode IN (" + logCodeList + ") ";
            }
            // Ballot Styles
            if (reportOptions.BallotStyles.Count() > 0)
            {
                string styleList = "";
                foreach (int styleID in reportOptions.BallotStyles)
                {
                    styleList += styleID.ToString() + ",";
                }
                styleList += "NULL";
                if (reportOptions.IncludeInactive == false)
                {
                    sql += "AND BallotStyleId IN (" + styleList + ") ";
                }
                else
                {
                    sql += "AND AssignedBallotStyleId IN (" + styleList + ") ";
                }
            }
            // Parties
            if (reportOptions.Parties.Count() > 0)
            {
                string partyList = "";
                foreach (string party in reportOptions.Parties)
                {
                    partyList += "'" + party + "',";
                }
                partyList += "NULL";
                sql += "AND Party IN (" + partyList + ") ";
            }
            // Jurisdictions
            if (reportOptions.Jurisdictions.Count() > 0)
            {
                string jurisdictionList = "";
                foreach (string jurisdictionID in reportOptions.Jurisdictions)
                {
                    jurisdictionList += "'" + jurisdictionID + "',";
                }
                jurisdictionList += "NULL";
                sql += "AND PrecinctPartId IN (SELECT [PrecinctPartId] FROM [dbo].[JurisdictionPrecincts] WHERE JurisdictionId IN (" + jurisdictionList + ")) ";
            }
            // Polling Sites
            if (reportOptions.PollSites.Count() > 0)
            {
                string siteList = "";
                foreach (int siteID in reportOptions.PollSites)
                {
                    siteList += siteID.ToString() + ",";
                }
                siteList += "NULL";
                sql += "AND PollId IN (" + siteList + ") ";
            }
            // Id Required
            if (reportOptions.IdRequired == true)
            {
                sql += "AND IdRequired = 1 ";
            }

            // Pass query to to Special report
            FastReportsMethods.PreviewReport("AbsenteeSpecialReport", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewCustomSummaryReportGeneral(ReportWizardQueryModel reportOptions)
        {
            // Create SQL Select statement from Wizard selections
            string sql = "SELECT *, ";
            sql += "report_type = '" + reportOptions.ReportName.ReplaceApostrophe() + "', ";
            sql += SortSpecialReportBy(reportOptions) + ", ";
            sql += GroupSpecialSummaryReportBy(reportOptions);
            if (reportOptions.IncludeInactive == false)
            {
                sql += "FROM (" + ReportViewsRevised.DailyDetailReportView((int)AppSettings.Election.ElectionType) + ") AS vDailyDetailReport ";
            }
            else
            {
                sql += "FROM (" + ReportViewsRevised.DailyInactiveDetailView((int)AppSettings.Election.ElectionType) + ") AS vDailyDetailReport ";
            }
            //sql += "FROM vDailyDetailReport ";
            sql += "WHERE ";
            sql += "ElectionId = " + AppSettings.Election.ElectionID.ToString() + " ";
            if (reportOptions.IncludeInactive == false)
            {
                sql += "AND ActivityDate >= '" + reportOptions.BeginningDate.ToString() + "' ";
                sql += "AND ActivityDate < '" + reportOptions.EndingDate.AddDays(1).ToShortDateString() + "' ";
            }
            // Log Codes
            if (reportOptions.IncludeInactive == false) sql += "AND LogCode <> 1 ";
            if (reportOptions.LogCodes.Count() > 0)
            {
                string logCodeList = "";
                foreach (int logCode in reportOptions.LogCodes)
                {
                    logCodeList += logCode.ToString() + ",";
                }
                logCodeList += "NULL";
                sql += "AND LogCode IN (" + logCodeList + ") ";
            }
            // Ballot Styles
            if (reportOptions.BallotStyles.Count() > 0)
            {
                string styleList = "";
                foreach (int styleID in reportOptions.BallotStyles)
                {
                    styleList += styleID.ToString() + ",";
                }
                styleList += "NULL";
                if (reportOptions.IncludeInactive == false)
                {
                    sql += "AND BallotStyleId IN (" + styleList + ") ";
                }
                else
                {
                    sql += "AND AssignedBallotStyleId IN (" + styleList + ") ";
                }
            }
            // Parties
            if (reportOptions.Parties.Count() > 0)
            {
                string partyList = "";
                foreach (string party in reportOptions.Parties)
                {
                    partyList += "'" + party + "',";
                }
                partyList += "NULL";
                sql += "AND Party IN (" + partyList + ") ";
            }
            // Jurisdictions
            if (reportOptions.Jurisdictions.Count() > 0)
            {
                string jurisdictionList = "";
                foreach (string jurisdictionID in reportOptions.Jurisdictions)
                {
                    jurisdictionList += "'" + jurisdictionID + "',";
                }
                jurisdictionList += "NULL";
                sql += "AND PrecinctPartId IN (SELECT [PrecinctPartId] FROM [dbo].[JurisdictionPrecincts] WHERE JurisdictionId IN (" + jurisdictionList + ")) ";
            }
            // Polling Sites
            if (reportOptions.PollSites.Count() > 0)
            {
                string siteList = "";
                foreach (int siteID in reportOptions.PollSites)
                {
                    siteList += siteID.ToString() + ",";
                }
                siteList += "NULL";
                sql += "AND PollId IN (" + siteList + ") ";
            }
            // Id Required
            if (reportOptions.IdRequired == true)
            {
                sql += "AND IdRequired = 1 ";
            }

            string summary = "SELECT ";
            summary += "report_type, ";
            summary += "GroupingNumber, ";
            summary += "ElectionId, ";
            summary += "ElectionName, ";
            summary += "ElectionDate, ";
            summary += "ElectionDateLong, ";
            summary += "CountyCode, ";
            summary += "CountyName, ";
            summary += "COUNT(*) AS[VoterCount] ";
            summary += "FROM ( " + sql + " ) AS SubSumaryReport ";
            summary += "GROUP BY ";
            summary += "report_type, ";
            summary += "GroupingNumber, ";
            summary += "ElectionId, ";
            summary += "ElectionName, ";
            summary += "ElectionDate, ";
            summary += "ElectionDateLong, ";
            summary += "CountyCode, ";
            summary += "CountyName ";

            // Pass query to to Special report
            FastReportsMethods.PreviewReport("AbsenteeSpecialSummaryReport", summary, debugMode, "DailyDetailData");
        }

        private static string SortOptions(int sort)
        {
            string sql = "";
            switch(sort)
            {
                case 1:
                    sql = "[LastName], [FirstName]";
                    break;
                case 2:
                    sql = "[LogCode]";
                    break;
                case 3:
                    sql = "[party]";
                    break;
                case 4:
                    sql = "[BallotStyleName]";
                    break;
                default:
                    sql = "[LastName], [FirstName]";
                    break;
            }
            return sql;
        }

        private static string GroupOptions(int sort)
        {
            string sql = "";
            switch (sort)
            {
                case 1:
                    sql = "LEFT([LastName],1) + '''S'";
                    break;
                case 2:
                    sql = "[LogDescription]";
                    break;
                case 3:
                    sql = "[Party]";
                    break;
                case 4:
                    sql = "[BallotStyleName]";
                    break;
                default:
                    sql = "' '";
                    break;
            }
            return sql;
        }

        private static string GroupSummaryOptions(int sort)
        {
            string sql = "";
            switch (sort)
            {
                case 1:
                    sql = "LEFT([LastName],1) + '''S'";
                    break;
                case 2:
                    sql = "[LogDescription]";
                    break;
                case 3:
                    sql = "[Party]";
                    break;
                case 4:
                    sql = "[BallotStyleName]";
                    break;
                default:
                    sql = "'TOTAL VOTERS'";
                    break;
            }
            return sql;
        }

        // Need to provide defaults for both sort options
        private static string SortSpecialReportBy(ReportWizardQueryModel sortOptions)
        {
            string sql = "";
            sql += "ROW_NUMBER() OVER ( ORDER BY " + SortOptions(sortOptions.SortType1) + ", " + SortOptions(sortOptions.SortType2) + " ) AS SortingNumber ";
            //sql += "ROW_NUMBER() OVER ( ORDER BY " + SortOptions(4) + ", " + SortOptions(3) + " ) AS SortingNumber ";

            return sql;
        }

        private static string GroupSpecialReportBy(ReportWizardQueryModel groupOptions)
        {
            string sql = "";
            sql += GroupOptions(groupOptions.GroupType) + " AS GroupingNumber ";
            //sql += GroupOptions(4) + " AS GroupingNumber ";
            return sql;
        }

        private static string GroupSpecialSummaryReportBy(ReportWizardQueryModel groupOptions)
        {
            string sql = "";
            sql += GroupSummaryOptions(groupOptions.GroupType) + " AS GroupingNumber ";
            //sql += GroupOptions(4) + " AS GroupingNumber ";
            return sql;
        }

        //#############################################################################

        public static string GetDailyDeletedActiveVotersSQL(int electionID, int? pollID)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity to Date'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + VoterX.Methods.ReportSubQueries.ReportViews.DailyDeletedReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        public static string GetDailyDeletedActiveVotersSQL(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity for " + reportingDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + VoterX.Methods.ReportSubQueries.ReportViews.DailyDeletedReportView() + ") AS vDailyDetailReport ";
            sql += "WHERE ";
            if (pollID != null) sql += "[poll_id] = " + pollID + " AND ";
            sql += "election_id = " + electionID + " ";
            // Specific date
            sql += "AND CAST(activity_date AS DATE) = CONVERT(VARCHAR(10), CONVERT(DATETIME,'" + reportingDate.ToString() + "'), 101) ";
            sql += "AND log_code <> 1 ";

            return sql;
        }

        public static string GetDailyDeletedActiveVotersSQL(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT *, ";
            sql += "report_type = 'Activity from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "'";
            // Replaced stored Views with inline SQL 8/23/2018
            //sql += "FROM vDailyDetailReport ";
            sql += "FROM (" + VoterX.Methods.ReportSubQueries.ReportViews.DailyDeletedReportView() + ") AS vDailyDetailReport ";
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

        public static string PrintDailyDeletedActiveVotersReport(int electionID, int? pollID)
        {
            string sql = GetDailyDeletedActiveVotersSQL(electionID, pollID);

            return FastReportsMethods.PrintSilentReport("AbsenteeDeletedActiveVoters02", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailyDeletedActiveVotersReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = GetDailyDeletedActiveVotersSQL(electionID, pollID, reportingDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeDeletedActiveVoters02", sql, debugMode, "DailyDetailData");
        }

        public static string PrintDailyDeletedActiveVotersReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = GetDailyDeletedActiveVotersSQL(electionID, pollID, startDate, endDate);

            return FastReportsMethods.PrintSilentReport("AbsenteeDeletedActiveVoters02", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDeletedActiveVotersReport(int electionID, int? pollID)
        {
            string sql = GetDailyDeletedActiveVotersSQL(electionID, pollID);

            FastReportsMethods.PreviewReport("AbsenteeDeletedActiveVoters02", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDeletedActiveVotersReport(int electionID, int? pollID, DateTime reportingDate)
        {
            string sql = GetDailyDeletedActiveVotersSQL(electionID, pollID, reportingDate);

            FastReportsMethods.PreviewReport("AbsenteeDeletedActiveVoters02", sql, debugMode, "DailyDetailData");
        }

        public static void PreviewDailyDeletedActiveVotersReport(int electionID, int? pollID, DateTime startDate, DateTime endDate)
        {
            string sql = GetDailyDeletedActiveVotersSQL(electionID, pollID, startDate, endDate);

            FastReportsMethods.PreviewReport("AbsenteeDeletedActiveVoters02", sql, debugMode, "DailyDetailData");
        }
    }
}
