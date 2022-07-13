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
using VoterX.Extensions;
using VoterX.Dialogs;
using VoterX.Manager.Methods;
using VoterX.Manager.Global;

namespace VoterX
{
    /// <summary>
    /// Interaction logic for AbsenteeScanOptionsPage.xaml
    /// </summary>
    public partial class AbsenteeBoardScanOptionsPage : Page
    {
        public AbsenteeBoardScanOptionsPage()
        {
            InitializeComponent();

            // Display page header
            GlobalReferences.Header.PageHeader = ("Scan Returns");

            if (AppSettings.Absentee.AllMailMode)
            {
                //ReturnedApplicationsPanel.Visibility = Visibility.Collapsed;
            }

            GlobalReferences.MenuSlider.Close();
        }

        private void ScanReturnedApplications_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new AbsenteeScanReturnedApplications());
            //NavigationMenuMethods.ReturnApplications();
        }

        private void ScanReturnedBallots_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateToPage(new AbsenteeBoardScanReturnedEnvelopes());
            //NavigationMenuMethods.ReturnBallots();
        }
    }
}
