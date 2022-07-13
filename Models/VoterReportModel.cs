using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoterX.Models
{
    public class VoterReportModel
    {
        public string VoterID { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Generation { get; set; }
        public string DOBYear { get; set; }
        public bool IDRequired { get; set; }
        public string PrecinctName { get; set; }
        public string Party { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string DeliveryAddress1 { get; set; }
        public string DeliveryAddress2 { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryZip { get; set; }
        public bool OutofCountry { get; set; }
        public string DeliveryProvince { get; set; }
        public string DeliveryCountry { get; set; }
        public string LogDescription { get; set; }
        public Nullable<System.DateTime> ActivityDate { get; set; }
        public string PollName { get; set; }
        public string BallotStyle { get; set; }
        public DateTime? ApplicationIssued { get; set; }
        public DateTime? ApplicationAccepted { get; set; }
        public DateTime? ApplicationRejected { get; set; }
        public DateTime? BallotIssued { get; set; }
        public DateTime? BallotPrinted { get; set; }
    }
}
