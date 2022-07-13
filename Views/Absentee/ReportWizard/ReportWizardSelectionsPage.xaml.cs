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
using VoterX.Utilities.Views;

namespace VoterX.Manager.Views.Absentee
{
    /// <summary>
    /// Interaction logic for ReportWizardSelectionsPage.xaml
    /// </summary>
    public partial class ReportWizardSelectionsPage : Page
    {
        ReportWizardQueryModel _wizardSearch;

        public ReportWizardSelectionsPage() : this(null) { }
        public ReportWizardSelectionsPage(ReportWizardQueryModel WizardSearch)
        {
            InitializeComponent();

            _wizardSearch = WizardSearch;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ReportWizardSelectionsViewModel(_wizardSearch);
        }
    }
}
