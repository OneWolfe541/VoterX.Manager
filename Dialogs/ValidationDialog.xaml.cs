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
using VoterX.Methods;

namespace VoterX.Dialogs
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class ValidationDialog : Window
    {
        string userName;

        public ValidationDialog(string username)
        {
            InitializeComponent();
            //lblQuestion.Content = question;
            //txtAnswer.Text = defaultAnswer;
            userName = username;            
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            //StatusBar.ApplicationStatusLeft(txtAnswer.Password);
            if (userName == "Administrator" && txtAnswer.Password.ToUpper() == AppSettings.User.AdminPassword.ToUpper())
            {
                this.DialogResult = true;
            }

            else if (userName == "Manager" && txtAnswer.Password.ToUpper() == AppSettings.User.ManagePassword.ToUpper())
            {
                this.DialogResult = true;
            }

            else
            {
                this.DialogResult = false;
            }            
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();

            Keyboard.Focus(txtAnswer);
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad();

        //    txtAnswer.Focus();
        //    txtAnswer.SelectAll();
        //}

        //public string Answer
        //{
        //    get { return txtAnswer.Text; }
        //}
    }
}
