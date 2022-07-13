using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using VoterX.Core.Models.Database;
//using VoterX.Core.Repositories;

namespace VoterX.Methods
{
    public static class PartyMethods
    {
        // Get a query for the Precinct Master table
        //public static List<string> GetPartyList()
        //{
        //    return ((App)Application.Current).voterContainer.Resolve<VoterRegistrationRepo>().Query(0).Select(voter => voter.party).Distinct().ToList();
        //}

        //public static Task<List<string>> GetPartyListAsync()
        //{
        //    return Task.Run(() =>
        //        // Get all Distinct Party Codes from tblVoters
        //        ((App)Application.Current).voterContainer.Resolve<VoterRegistrationRepo>().Query(0).OrderBy(o => o.party).Select(voter => voter.party).Distinct().ToList()
        //        );
        //}

        public static List<string> GetPrimaryPartyList()
        {
            List<string> party = new List<string>();
            party.Add("DEM");
            party.Add("REP");
            party.Add("LIB");
            return party;
        }
    }
}
