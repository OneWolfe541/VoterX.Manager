using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VoterX.Extensions
{
    public static class ComboBoxExtensions
    {
        public static string GetSelectedItem(this ComboBox sender)
        {
            if (sender.SelectedItem == null) return "";
            else
                return ((ComboBoxItem)sender.SelectedItem).Content.ToString();
        }

        public static int GetSelectedItemNumber(this ComboBox sender)
        {
            if (sender.SelectedItem == null) return 0;
            else
            {
                if (((ComboBoxItem)sender.SelectedItem).DataContext.GetType() == typeof(int))
                    return (int)((ComboBoxItem)sender.SelectedItem).DataContext;
                else if (((ComboBoxItem)sender.SelectedItem).DataContext.GetType() == typeof(string))
                    return Int32.Parse(((ComboBoxItem)sender.SelectedItem).DataContext.ToString());
                else
                    return 0;
            }
        }
    }
}
