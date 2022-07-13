using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoterX.Methods;
using System.Windows.Controls;

namespace VoterX.Methods
{
    public static class MainFrameMethods
    {
        private static MainWindow MAINWINDOW = ((MainWindow)System.Windows.Application.Current.MainWindow);
        private static MainAbsenteeWindow MAINABSENTEEWINDOW = ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault());

        private static bool absenteeMode = AppSettings.AbsenteeMode;

        public static void NavigateToPage(Page page)
        {
            if (absenteeMode)
            {
                MAINABSENTEEWINDOW.MainFrame.Navigate(page);
            }
            else
            {
                MAINWINDOW.MainFrame.Navigate(page);
            }
        }
    }
}
