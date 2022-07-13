using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VoterX;
using VoterX.Methods;
using VoterX.SystemSettings;
using VoterX.Utilities.Controls;
using VoterX.Utilities.Views;

namespace VoterX.Manager.Global
{
    public static class GlobalReferences
    {
        public static MainAbsenteeWindow MainWindow
        {
            get
            {
                return ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault());
            }
            private set
            {

            }
        }

        public static Frame MainFrame
        {
            get { return MainWindow.MainFrame; }            
            set { MainWindow.MainFrame = value; }
        }

        public static MainHeaderViewModel Header
        {
            get { return ((App)Application.Current).mainHeader; }
            set { ((App)Application.Current).mainHeader = value; }
        }

        public static StatusBarViewModel StatusBar
        {
            get { return ((App)Application.Current).statusBar; }
            set { ((App)Application.Current).statusBar = value; }
        }

        public static SliderMenuFrameControl MenuSlider
        {
            get { return ((App)Application.Current).mainSliderMenu; }
            set { ((App)Application.Current).mainSliderMenu = value; }
        }

        public static SystemSettingsController Settings
        {
            get { return ((App)Application.Current).GlobalSettings; }
            set { ((App)Application.Current).GlobalSettings = value; }
        }

        public static Page Origin
        {
            get { return ((App)Application.Current).originpage; }
            set { ((App)Application.Current).originpage = value; }
        }

        public static bool DebugMode
        {
            get { return ((App)Application.Current).debugMode; }
            set { ((App)Application.Current).debugMode = value; }
        }
    }
}
