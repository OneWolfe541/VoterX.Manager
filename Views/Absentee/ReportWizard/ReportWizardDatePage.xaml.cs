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
using VoterX.Models;

namespace VoterX.Manager.Views.Absentee
{
    /// <summary>
    /// Interaction logic for ReportWizardDatePage.xaml
    /// </summary>
    public partial class ReportWizardDatePage : Page
    {
        ReportWizardQueryModel _wizardSearch;

        public ReportWizardDatePage() : this(null) { }
        public ReportWizardDatePage(ReportWizardQueryModel WizardSearch)
        {
            InitializeComponent();

            _wizardSearch = WizardSearch;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ReportWizardDateViewModel(_wizardSearch);
        }
    }
}
