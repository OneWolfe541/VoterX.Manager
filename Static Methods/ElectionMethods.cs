using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Core.Elections;

namespace VoterX.Methods
{
    public static class ElectionDataMethods
    {
        public static NMElection Election
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election;
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool Exists
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Exists();
                }
                else
                {
                    return false;
                }
            }
        }

        public static List<ApplicationRejectedReasonModel> ApplicationRejectedReasons
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.ApplicationRejectedReasons;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<BallotRejectedReasonModel> BallotRejectedReasons
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.BallotRejectedReasons;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<BallotStyleModel> BallotStyles
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.BallotStyles;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<BallotStyleModel> DistinctBallots
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.BallotStyles.DistinctBallots();
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<BallotStyleModel> UniqueBallotStyles
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.BallotStyles.DistinctBallots();
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<JurisdictionModel> Jurisdictions
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.Jurisdictions;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<string> JurisdictionTypes
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.JurisdictionTypes;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<LocationModel> Locations
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.Locations;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<LogCodeModel> LogCodes
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.LogCodes;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<PartyModel> Partys
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.Partys;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<PollWorkerModel> PollWorkers
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.PollWorkers;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<PrecinctModel> Precincts
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.Precincts;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<ProvisionalReasonModel> ProvisionalReasons
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.ProvisionalReasons;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<SpoiledReasonModel> SpoiledReasons
        {
            get
            {
                if (((App)Application.Current).Election != null)
                {
                    return ((App)Application.Current).Election.Lists.SpoiledReasons;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<BallotStyleModel> GetBallotStyles(string PrecinctPart)
        {
            if (((App)Application.Current).Election != null)
            {
                return ((App)Application.Current).Election.Lists.BallotStyles
                .Where(bs => bs.PrecinctPartID == PrecinctPart)
                .ToList();
            }
            else
            {
                return null;
            }
        }

        public static List<BallotStyleModel> GetBallotStyles(string PrecinctPart, string Party)
        {
            if (((App)Application.Current).Election != null)
            {
                return ((App)Application.Current).Election.Lists.BallotStyles
                .Where(bs => bs.PrecinctPartID == PrecinctPart && bs.Party == Party)
                .ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
