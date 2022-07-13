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
    public partial class YesNoDialog : Window
    {
        public YesNoDialog(string title, string message)
        {
            InitializeComponent();
            this.Title = title;
            lblmessage.Content = message;   

            //this.Icon = FontAwesomeIcon.Exclamation;
            // Get icon images from https://paulferrett.com/fontawesome-favicon/
            //Uri iconUri = new Uri("pack://application:,,,/Images/favicon-exclamation.ico");
            //this.Icon = BitmapFrame.Create(iconUri);
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
