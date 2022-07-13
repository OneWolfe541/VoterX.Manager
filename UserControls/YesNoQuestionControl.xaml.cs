using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FontAwesome.WPF;

namespace VoterX.UserControls
{
    /// <summary>
    /// Interaction logic for YesNoQuestionControl.xaml
    /// </summary>
    public partial class YesNoQuestionControl : UserControl
    {
        public event RoutedEventHandler AnswerClick;

        public string Question
        {
            get { return TextQuestion.Text; }
            set { TextQuestion.Text = value; }
        }

        public double QuestionHeight
        {
            get { return QuestionBorder.Height; }
            set
            {
                //double temp = value;
                QuestionBorder.Height = value;
                ToggleCheckYes.Height = value;
                ToggleCheckNo.Height = value;
            }
        }

        public double QuestionWidth
        {
            get { return QuestionBorder.Width; }
            set { QuestionBorder.Width = value; }
        }

        public double QuestionFontSize
        {
            get { return TextQuestion.FontSize; }
            set
            {
                double temp = value == 0 ? this.FontSize : value;
                TextQuestion.FontSize = temp;
                ToggleCheckYes.FontSize = temp;
                toggle_check_fa_check_yes.Height = temp;
                toggle_check_fa_check_yes.Width = temp;
                ToggleCheckNo.FontSize = temp;
                toggle_check_fa_check_no.Height = temp;
                toggle_check_fa_check_no.Width = temp;
            }
        }
        
        private bool? Answer { get; set; }
            

        public YesNoQuestionControl()
        {
            InitializeComponent();
        }

        public bool? GetAnswer()
        {
            return Answer;
        }

        public void ChangeAnswer(bool value)
        {
            if (value == true)
            {
                Answer = value;
                SetButtonChecked(ToggleCheckYes);
                SetButtonDisabled(ToggleCheckNo);
            }
            if (value == false)
            {
                Answer = value;
                SetButtonChecked(ToggleCheckNo);
                SetButtonDisabled(ToggleCheckYes);
            }
            //if (value == null)
            //{
            //    Reset();
            //}
        }

        public void Reset()
        {
            Answer = null;
            ResetButton(ToggleCheckYes);
            ResetButton(ToggleCheckNo);
        }

        private void ToggleCheckYes_Click(object sender, RoutedEventArgs e)
        {
            Answer = true;

            SetButtonChecked(ToggleCheckYes);
            SetButtonDisabled(ToggleCheckNo);

            if (AnswerClick != null)
            {
                AnswerClick(sender, e);
            }
        }

        private void ToggleCheckNo_Click(object sender, RoutedEventArgs e)
        {
            Answer = false;

            SetButtonChecked(ToggleCheckNo);
            SetButtonDisabled(ToggleCheckYes);

            if (AnswerClick != null)
            {
                AnswerClick(sender, e);
            }
        }

        private void SetButtonChecked(ToggleButton sender)
        {
            // Set checked button
            sender.IsChecked = true;
            sender.IsEnabled = false;

            var childList = FindVisualChildren<ImageAwesome>(sender);
            var iconChecked = childList.FirstOrDefault();
            if (iconChecked != null)
            {
                iconChecked.Visibility = Visibility.Visible;
            }
        }

        private void SetButtonDisabled(ToggleButton sender)
        {
            // Set unchecked button
            sender.IsChecked = false;
            sender.IsEnabled = false;

            var childList = FindVisualChildren<ImageAwesome>(sender);
            var iconUnchecked = childList.FirstOrDefault();
            if (iconUnchecked != null)
            {
                iconUnchecked.Visibility = Visibility.Hidden;
            }
        }

        private void ResetButton(ToggleButton sender)
        {
            // Set checked button
            sender.IsChecked = false;
            sender.IsEnabled = true;

            var childList = FindVisualChildren<ImageAwesome>(sender);
            var iconYes = childList.FirstOrDefault();
            if (iconYes != null)
            {
                iconYes.Visibility = Visibility.Hidden;
            }
        }

        // Find all children of tpye<t> sample
        // https://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
