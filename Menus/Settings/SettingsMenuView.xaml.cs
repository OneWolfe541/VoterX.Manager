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
using VoterX;

namespace VoterX.Manager.Menus
{
    /// <summary>
    /// Interaction logic for SettingsMenuView.xaml
    /// </summary>
    public partial class SettingsMenuView : Page
    {
        private SettingsPage _page;

        public SettingsMenuView(SettingsPage page)
        {
            InitializeComponent();

            _page = page;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new SettingsMenuViewModel(_page);
        }
    }
}
