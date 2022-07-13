using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VoterX.Methods
{
    public static class NavigationMethods
    {
        public static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }

        // https://stackoverflow.com/questions/10279092/how-to-get-children-of-a-wpf-container-by-type
        public static T GetChildOfTypeFrame<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfTypeFrame<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        // Method to clean up the page navigation code
        // Simply pass in the page your comming from and where you want to go
        public static void NavigateToPage(DependencyObject dependencyObject, Page page)
        {
            // Get parent frame
            var frame = FindParent<Frame>(dependencyObject);
            // Load the new page into the parent frame
            frame.Navigate(page);
        }
    }
}
