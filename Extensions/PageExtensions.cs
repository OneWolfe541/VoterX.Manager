using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VoterX.Methods;

namespace VoterX.Extensions
{
    public static class PageExtensions
    {
        // Extension method to simplify navigation on child pages
        // Simply call this.NavigateToPage and pass in the page to want to go to
        public static void NavigateToPage(this Page sender, Page nextPage)
        {
            // Get parent frame
            var frame = NavigationMethods.FindParent<Frame>(sender);
            if (frame != null)
            {
                // Load the new page into the parent frame
                frame.Navigate(nextPage);
            }
        }
    }
}
