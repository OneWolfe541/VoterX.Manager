using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using VoterX.Dialogs;
using VoterX.Extensions;
using VoterX.Methods;
using VoterX.Core.Voters;
using VoterX.Utilities.Views;

namespace VoterX.Manager.Views.Absentee
{
    /// <summary>
    /// Interaction logic for ReturnApplicationsPage.xaml
    /// </summary>
    public partial class ReturnApplicationsPage : Page
    {
        public ReturnApplicationsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ReturnApplicationViewModel();
        }
    }
}
