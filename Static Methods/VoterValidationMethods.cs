using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using VoterX.Core.Models.ViewModels;
//using VoterX.Core.Extensions;

namespace VoterX.Methods
{
    public enum VoterLookupStatus
    {
        Eligible = 1,
        Ineligible = 2,
        Spoilable = 3,
        Provisional = 4,
        Deleted = 5,
        None = 0
    };

    public static class VoterValidationMethods
    {
        // This function checks if the voter has already voted and where
        // And sets what options are availible to the user
        //public static VoterLookupStatus CheckVoterStatus(VoterDataModel voter, int pollId)
        //{
        //    VoterLookupStatus voterStatus = VoterLookupStatus.None;

        //    // Check deleted status
        //    if (voter.WasRemoved())
        //    {
        //        voterStatus = VoterLookupStatus.Deleted;
        //    }
        //    // Check log code
        //    else if (voter.HasVoted())
        //    {

        //        //Check voter location and logdate
        //        if (voter.VotedHereToday(pollId) && !voter.WrongOrFledVoter())
        //        {
        //            // if same location then spoil ballot
        //            voterStatus = VoterLookupStatus.Spoilable;
        //        }
        //        else
        //        {
        //            // if different location or date then provisional ballot
        //            voterStatus = VoterLookupStatus.Provisional;
        //        }
        //    }
        //    else if (!voter.IsEligible())
        //    {
        //        //IneligibleVoter
        //        voterStatus = VoterLookupStatus.Ineligible;
        //    }
        //    else
        //    {
        //        // Allow user to procceed to print out a ballot
        //        voterStatus = VoterLookupStatus.Eligible;
        //    }

        //    return voterStatus;
        //}


        //public static VoterLookupStatus CheckVoterStatus(VoterDataModel voter)
        //{
        //    VoterLookupStatus voterStatus = VoterLookupStatus.None;

        //    return voterStatus;
        //}

    }
}
