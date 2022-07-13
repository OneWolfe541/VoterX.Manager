using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoterX.Manager.Models
{
    public class VoterHistoryDisplayModel
    {
        public string HistoryTitle { get; set; }
        public int? LogCode { get; set; }
        public DateTime HistoryDate { get; set; }
        public Guid HistoryId { get; set; }
    }
}
