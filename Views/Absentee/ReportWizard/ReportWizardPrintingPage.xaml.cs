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
    /// Interaction logic for ReportWizardPrintingPage.xaml
    /// </summary>
    public partial class ReportWizardPrintingPage : Page
    {
        ReportWizardQueryModel _wizardSearch;

        public ReportWizardPrintingPage() : this(null) { }
        public ReportWizardPrintingPage(ReportWizardQueryModel WizardSearch)
        {
            InitializeComponent();

            _wizardSearch = WizardSearch;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ReportWizardPrintingViewModel(_wizardSearch);
        }
    }
}
