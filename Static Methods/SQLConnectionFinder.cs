using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

// http://jake.ginnivan.net/wpf-sql-connection-user-control/
// https://github.com/JakeGinnivan/SqlConnectionControl
namespace VoterX.Methods
{
    public static class SQLConnectionFinder
    {
        // https://github.com/JakeGinnivan/SqlConnectionControl/blob/master/WpfApplication1/SmoTasks.cs
        private static IEnumerable<string> SqlServers()
        {
            return
                SmoApplication
                .EnumAvailableSqlServers()
                .AsEnumerable()
                .Select(r => r["Name"].ToString());
        }

        // Async because of the long load times
        public static Task<ObservableCollection<string>> GetServerListAsync()
        {
            return Task.Run(() =>
            {
                ObservableCollection<string> servers = new ObservableCollection<string>();

                foreach (string serverName in SqlServers().OrderBy(r => r))
                {
                    servers.Add(serverName);
                }

                return servers;
            }
            );
        }

        // Not async because of the relatively fast load times
        public static ObservableCollection<string> GetDataBases(string serverName)
        {
            string connectionString = "data source=" + serverName + ";initial catalog=master;integrated security=True;MultipleActiveResultSets=True";

            var databases = new List<string>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var serverConnection = new ServerConnection(conn);
                var server = new Server(serverConnection);
                databases.AddRange(from Database database in server.Databases select database.Name);
            }

            return new ObservableCollection<string>(databases);
        }
    }
}
