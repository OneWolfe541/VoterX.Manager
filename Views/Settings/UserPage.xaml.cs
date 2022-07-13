using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoterX.Methods;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : SettingsBasePage
    {
        public UserPage()
        {
            InitializeComponent();
            DisplaySettings();
        }

        // Need to bind this data instead of manualy stuffing it here
        private void DisplaySettings()
        {
            UserName.Text = AppSettings.User.UserName;
            UserID.Text = AppSettings.User.UserID.ToString();
        }

        public override void SaveSettings()
        {
            GlobalReferences.StatusBar.TextLeft = ("User Settings Page Called");
        }
    }
}
