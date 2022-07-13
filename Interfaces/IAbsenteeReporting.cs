using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoterX.Interfaces
{
    public interface IAbsenteeReporting
    {
        string ReportName { get; set; }

        string PrintReport(int electionID, int? pollID);

        string PrintReport(int electionID, int? pollID, DateTime reportingDate);

        string PrintReport(int electionID, int? pollID, DateTime startDate, DateTime endDate);

        void PreviewReport(int electionID, int? pollID);

        void PreviewReport(int electionID, int? pollID, DateTime reportingDate);

        void PreviewReport(int electionID, int? pollID, DateTime startDate, DateTime endDate);
    }
}
