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

namespace VoterX.Manager.Views.Absentee
{
    /// <summary>
    /// Interaction logic for BatchPrintingPage.xaml
    /// </summary>
    public partial class BatchPrintingPage : Page
    {
        private int? _siteId;

        public BatchPrintingPage() : this(null) { }
        public BatchPrintingPage(int? siteId)
        {
            InitializeComponent();

            _siteId = siteId;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new BatchPrintingViewModel(_siteId);
        }
    }
}
