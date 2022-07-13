using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VoterX.Extensions
{
    public static class GridPanelExtensions
    {
        public static void Show(this StackPanel panel)
        {
            panel.Visibility = System.Windows.Visibility.Visible;
        }

        public static void Hide(this StackPanel panel)
        {
            panel.Visibility = System.Windows.Visibility.Collapsed;
        }

        public static void Show(this Grid grid)
        {
            grid.Visibility = System.Windows.Visibility.Visible;
        }

        public static void Hide(this Grid grid)
        {
            grid.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
