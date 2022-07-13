using VoterX.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VoterX.Methods
{
    public static class ComboBoxMethods
    {
        // Get the string name of a selected item
        public static string GetSelectedItem(ComboBox sender)
        {
            if (sender.SelectedItem == null) return "";
            else
                return ((ComboBoxItem)sender.SelectedItem).Content.ToString();
        }

        // Get the selected item 
        public static ComboBoxItem GetSelectedItem(object sender)
        {
            if (((ComboBox)sender).SelectedItem == null) return null;
            else
                return (ComboBoxItem)((ComboBox)sender).SelectedItem;
        }

        // Get the data element of a selected item
        public static object GetSelectedItemData(ComboBox sender)
        {
            if (sender.SelectedItem == null) return "";
            else
                return ((ComboBoxItem)sender.SelectedItem).DataContext;
        }

        // Get the data element of a selected item
        public static object GetSelectedItemData(object sender)
        {
            if (((ComboBox)sender).SelectedItem == null) return null;
            else
                return (ComboBoxItem)((ComboBox)sender).DataContext;
        }

        // Cast the data element of a selected item to typeof<T>
        public static T GetSelectedItemData<T>(ComboBox sender)
        {
            if (sender.SelectedItem == null) return default(T);
            else
                return (T)((ComboBoxItem)sender.SelectedItem).DataContext;
        }        

        // Get the integer value of the selected item's data element
        public static int GetSelectedItemNumber(ComboBox sender)
        {
            if (sender.SelectedItem == null) return 0;
            else
            {
                try
                {
                    if (((ComboBoxItem)sender.SelectedItem).DataContext.GetType() == typeof(int))
                        return (int)((ComboBoxItem)sender.SelectedItem).DataContext;
                    else if (((ComboBoxItem)sender.SelectedItem).DataContext.GetType() == typeof(string))
                        return Int32.Parse(((ComboBoxItem)sender.SelectedItem).DataContext.ToString());
                    else
                        return 0;
                }
                catch
                {
                    return 0;
                }
            }
        }

        // Add a single item to a combo box
        public static void AddComboItemToControl(ComboBox sender, int newIndex, string newItem, int selectedIndex)
        {
            // Create blank list item
            ComboBoxItem item = new ComboBoxItem();

            // Set list item name
            item.Content = newItem.ToUpper();
            item.DataContext = newIndex;

            // Check default for null and empty strings
            //if (selectedItem != null && selectedItem != "" && selectedItem.Replace(" ", "") != "")
            //{
            // Set selected item default from given string
            if (newIndex == selectedIndex)
                item.IsSelected = true;
            //}

            // Add the item to the combo box
            sender.Items.Add(item);
        }

        // Add a single item to a combo box
        // With a string for the data element
        public static void AddComboItemToControl(ComboBox sender, string newIndex, string newItem, string selectedIndex)
        {
            // Create blank list item
            ComboBoxItem item = new ComboBoxItem();

            // Set list item name
            if(newItem != null) item.Content = newItem.ToUpper();
            item.DataContext = newIndex;
            
            if(selectedIndex!=null)
            if (newIndex == selectedIndex)
                item.IsSelected = true;            

            // Add the item to the combo box
            sender.Items.Add(item);
        }

        // Add a single item to a combo box
        // With a string for the data element
        // And allows for enabling and disabling of specific items
        public static void AddComboItemToControl(ComboBox sender, string newIndex, string newItem, string selectedIndex, bool isEnabled)
        {
            // Create blank list item
            ComboBoxItem item = new ComboBoxItem();

            // Set list item name
            if (newItem != null) item.Content = newItem.ToUpper();
            item.DataContext = newIndex;
            item.IsEnabled = isEnabled;

            if (newIndex == selectedIndex)
                item.IsSelected = true;

            // Add the item to the combo box
            sender.Items.Add(item);
        }

        // Add a single item to a combo box
        // With generic object as data element
        public static void AddComboItemToControl(ComboBox sender, object newData, string newItem, bool isSelectedIndex)
        {
            // Create blank list item
            ComboBoxItem item = new ComboBoxItem();

            // Set list item name
            item.Content = newItem.ToUpper();
            item.DataContext = newData;
            
            // Set default selected item
            if (isSelectedIndex)
                item.IsSelected = true;            

            // Add the item to the combo box
            sender.Items.Add(item);
        }

        // Copy a predefined animated loading message
        public static ComboBoxItem AddLoadingItem(ComboBox DestinationCombobox, ComboBoxItem SpinnerItem)
        {
            // Create blank list item
            ComboBoxItem item = new ComboBoxItem();

            item.Content = SpinnerItem.Content;

            item.IsSelected = true;

            // Add the item to the combo box
            (DestinationCombobox).Items.Add(item);

            return item;
        }

        // Create a copy of an existing combobox list item
        public static ComboBoxItem CopyExistingItem(ComboBox DestinationCombobox, ComboBoxItem ItemToCopy, bool isSelectedIndex)
        {
            // Create blank list item
            ComboBoxItem item = new ComboBoxItem();

            item.Content = ItemToCopy.Content;

            item.IsSelected = isSelectedIndex;

            // Add the item to the combo box
            (DestinationCombobox).Items.Add(item);

            return item;
        }

        // Remove item sample
        // https://stackoverflow.com/questions/6831825/remove-combobox-item-from-combobox-wpf
        public static void RemoveListItem(object sender, ComboBoxItem item)
        {
            ((ComboBox)sender).Items.Remove(item);
        }

        public static void SetSelectedItem(ComboBox sender, string itemContent)
        {
            // Check if ballot style is empty
            if (!itemContent.IsNullOrEmpty())
            {
                // Check if the destination list is empty
                if (sender.Items.Count > 0)
                {
                    // Loop through list items
                    foreach (ComboBoxItem item in sender.Items)
                    {
                        // Compare list item to the string provided
                        if (item.Content.ToString() == itemContent)
                        {
                            // Set the destination selected item
                            sender.SelectedIndex = sender.Items.IndexOf(item);
                        }
                    }
                }                
            }            
        }

        public static int FindItemIndex(ComboBox sender, string itemContent)
        {
            // Check if ballot style is empty
            if (!itemContent.IsNullOrEmpty())
            {
                // Check if the destination list is empty
                if (sender.Items.Count > 0)
                {
                    // Loop through list items
                    foreach (ComboBoxItem item in sender.Items)
                    {
                        // Compare list item to the string provided
                        if (item.Content.ToString() == itemContent)
                        {
                            // return selected item index
                            return sender.Items.IndexOf(item);
                        }
                    }
                }
            }
            return -1;
        }
    }
}
