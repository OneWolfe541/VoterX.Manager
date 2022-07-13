using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoterX.Models
{
    public class ReportWizardQueryModel
    {
        public string ReportName { get; set; }
        public DateTime BeginningDate { get; set; }
        public DateTime EndingDate { get; set; }
        public List<int> LogCodes { get; set; }
        public List<int> BallotStyles { get; set; }
        public List<string> Parties { get; set; }
        public string JurisdictionType { get; set; }
        public List<string> Jurisdictions { get; set; }
        public List<int> PollSites { get; set; }
        public bool IncludeInactive { get; set; }
        public bool IdRequired { get; set; }
        public int SortType1 { get; set; }
        public int SortType2 { get; set; }
        public int GroupType { get; set; }        

        public ReportWizardQueryModel()
        {
            LogCodes = new List<int>();
            BallotStyles = new List<int>();
            Parties = new List<string>();
            Jurisdictions = new List<string>();
            PollSites = new List<int>();
        }
    }
}
