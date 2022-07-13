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
using System.Windows.Shapes;
using FontAwesome.WPF;

namespace VoterX.Dialogs
{
    /// <summary>
    /// Interaction logic for AlertDialog.xaml
    /// </summary>
    public partial class AlertDialog : Window
    {
        public AlertDialog(string message)
        {
            InitializeComponent();
            lblmessage.Content = message;

            //this.Icon = FontAwesomeIcon.Exclamation;
            // Get icon images from https://paulferrett.com/fontawesome-favicon/
            Uri iconUri = new Uri("pack://application:,,,/Images/favicon-exclamation.ico");
            this.Icon = BitmapFrame.Create(iconUri);

            //Console.WriteLine("Alert Opened: " + lblmessage.Content.ToString());

            btnCancel.Focus();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("Alert Closed: " + lblmessage.Content.ToString());
            this.DialogResult = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                e.Handled = true;
            }
        }
    }
}
