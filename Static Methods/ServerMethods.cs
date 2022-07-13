using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VoterX.Core.Elections;

namespace VoterX.Methods
{
    public static class ServerMethods
    {
        public static async Task<string> CheckConnection(ElectionFactory election)
        {
            var message = await Task.Run(() =>
                {
                    try
                    {
                        var test = election.Exists();
                        return test.ToString();
                        //return null;
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }
                );

            return message;
        }
    }
}
