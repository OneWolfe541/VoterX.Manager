using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace VoterX.Extensions
{
    public static class GridExtensions
    {
        // https://stackoverflow.com/questions/10279092/how-to-get-children-of-a-wpf-container-by-type
        // Loop through the Visual Tree Helper and return the first child of type <T>
        public static T GetChildOfType<T>(this DependencyObject depObj)
    where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }
    }
}
