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
using VoterX.Manager.Views.Super;

namespace VoterX.Manager.Menus
{
    /// <summary>
    /// Interaction logic for SettingsMenuView.xaml
    /// </summary>
    public partial class SuperSearchMenuView : Page
    {
        private SuperSearchPage _page;

        public SuperSearchMenuView(SuperSearchPage page)
        {
            InitializeComponent();

            _page = page;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new SuperSearchMenuViewModel(_page);
        }
    }
}
