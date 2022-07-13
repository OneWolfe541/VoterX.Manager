using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoterX.Methods
{
    public static class ConnectionSetup
    {
        // RSA encryption provider
        //http://www.noesisbase.com/Views/Articles/76/Encrypt-Decrypt-Connection-String-in-App-config
        // Data Protection Configurations Provider
        //https://forums.asp.net/t/1803664.aspx?Error+encrypting+sections+in+web+config
        // More Details
        // https://www.codeproject.com/Articles/18209/Encrypting-the-app-config-File-for-Windows-Forms-A

        public static void SetServerConnection(string serverName, string dbName, int sqlMode)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");

            if (sqlMode == 1)
            {
                // Daves connection string for local site servers
                // data source=localhost;initial catalog=ELECTION;persist security info=True;user id=sa;password=@ESf13lddba;MultipleActiveResultSets=True;App=EntityFramework
                connectionStringsSection.ConnectionStrings["VoterDatabase"].ConnectionString = "data source=" + serverName + ";initial catalog=" + dbName + ";persist security info=True;user id=sa;password=@ESf13lddba;MultipleActiveResultSets=True;App=EntityFramework";
            }
            else
            {
                // Direct connection to central server
                connectionStringsSection.ConnectionStrings["VoterDatabase"].ConnectionString = "data source=" + serverName + ";initial catalog=" + dbName + ";integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
            }
            config.Save();

            //ConfigurationManager.RefreshSection("connectionStrings");
        }

        public static void EncryptConnection()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");          

            ConfigurationSection section = config.GetSection("connectionStrings");

            if (!section.SectionInformation.IsProtected)
            {
                // RSA causing error "Object Already Exists"
                //section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                section.SectionInformation.ForceSave = true;

                config.Save(ConfigurationSaveMode.Full);
            }

            //ConfigurationManager.RefreshSection("connectionStrings");

        }

        public static void DecryptConnection()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");          

            ConfigurationSection section = config.GetSection("connectionStrings");

            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();

                section.SectionInformation.ForceDeclaration(true);

                section.SectionInformation.ForceSave = true;

                config.Save(ConfigurationSaveMode.Full);
            }

            //ConfigurationManager.RefreshSection("connectionStrings");

        }

        public static void RefreshConnection()
        {
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
