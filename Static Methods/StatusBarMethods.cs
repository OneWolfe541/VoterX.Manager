using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VoterX.Core.Elections;

namespace VoterX.Methods
{
    //public static class StatusBar
    //{
    //    private static MainWindow MAINWINDOW = ((MainWindow)System.Windows.Application.Current.MainWindow);
    //    private static MainAbsenteeWindow MAINABSENTEEWINDOW = ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault());

    //    private static bool absenteeMode = AppSettings.AbsenteeMode;

    //    //public static void SetAbsentee()
    //    //{
    //    //    MAINWINDOW = ((MainWindow)System.Windows.Application.Current.MainWindow);
    //    //}

    //    //public static void SetVCC()
    //    //{

    //    //}

    //    public static string GetStatusBar()
    //    {
    //        if (absenteeMode)
    //        {
    //            return MAINABSENTEEWINDOW.ToString();
    //        }
    //        else
    //        {
    //            return MAINWINDOW.ToString();
    //        }
    //    }

    //    public static void ApplicationPageHeader(string value)
    //    {
    //        if (absenteeMode)
    //        {
    //            //MAINABSENTEEWINDOW.PageHeaderName.Text = "ABSENTEE " + value.ToUpper();
    //            MAINABSENTEEWINDOW.PageHeaderName.Text = value.ToUpper() +
    //                (((App)Application.Current).debugMode == true ? " - [DEVELOPMENT MODE]" : "");
    //        }
    //        else
    //        {
    //            MAINWINDOW.PageHeaderName.Text = value.ToUpper();
    //        }
    //    }

    //    // Display a text message on the status bar
    //    public static void ApplicationStatus(string value)
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ApplicationStatusLeft.Text = value;
    //        }
    //        else
    //        {
    //            MAINWINDOW.ApplicationStatusLeft.Text = value;
    //        }
    //    }

    //    public static void ApplicationStatusRight(string value)
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ApplicationStatusRight.Text = value;
    //        }
    //        else
    //        {
    //            MAINWINDOW.ApplicationStatusRight.Text = value;
    //        }
    //    }

    //    public static void ApplicationStatusLeft(string value)
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ApplicationStatusLeft.Text = value;
    //        }
    //        else
    //        {
    //            MAINWINDOW.ApplicationStatusLeft.Text = value;
    //        }
    //    }

    //    public static void ApplicationStatusCenter(string value)
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ApplicationStatusCenter.Text = value;
    //        }
    //        else
    //        {
    //            MAINWINDOW.ApplicationStatusCenter.Text = value;
    //        }
    //    }

    //    public static void ApplicationHidePrinterStatus()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ApplicationPrinterStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //            MAINABSENTEEWINDOW.ApplicationPrinterStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //        }
    //        else
    //        {
    //            MAINWINDOW.ApplicationPrinterStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //            MAINWINDOW.ApplicationPrinterStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //        }
    //    }

    //    public static void ApplicationPrinterStatus(bool status)
    //    {
    //        if (absenteeMode)
    //        {
    //            if (status == true)
    //            {
    //                MAINABSENTEEWINDOW.ApplicationPrinterStatusOK.Visibility = System.Windows.Visibility.Visible;
    //                MAINABSENTEEWINDOW.ApplicationPrinterStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //            }
    //            else
    //            {
    //                MAINABSENTEEWINDOW.ApplicationPrinterStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //                MAINABSENTEEWINDOW.ApplicationPrinterStatusBad.Visibility = System.Windows.Visibility.Visible;
    //            }
    //        }
    //        else
    //        {
    //            if (status == true)
    //            {
    //                MAINWINDOW.ApplicationPrinterStatusOK.Visibility = System.Windows.Visibility.Visible;
    //                MAINWINDOW.ApplicationPrinterStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //            }
    //            else
    //            {
    //                MAINWINDOW.ApplicationPrinterStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //                MAINWINDOW.ApplicationPrinterStatusBad.Visibility = System.Windows.Visibility.Visible;
    //            }
    //        }            
    //    }

    //    public static void ApplicationHideSignaturePadStatus()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ApplicationSignatureStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //            MAINABSENTEEWINDOW.ApplicationSignatureStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //        }
    //        else
    //        {
    //            MAINWINDOW.ApplicationSignatureStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //            MAINWINDOW.ApplicationSignatureStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //        }
    //    }

    //    public static void ApplicationSignaturePadStatus(bool status)
    //    {
    //        if (absenteeMode)
    //        {
    //            if (status == true)
    //            {
    //                MAINABSENTEEWINDOW.ApplicationSignatureStatusOK.Visibility = System.Windows.Visibility.Visible;
    //                MAINABSENTEEWINDOW.ApplicationSignatureStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //            }
    //            else
    //            {
    //                MAINABSENTEEWINDOW.ApplicationSignatureStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //                MAINABSENTEEWINDOW.ApplicationSignatureStatusBad.Visibility = System.Windows.Visibility.Visible;
    //            }
    //        }
    //        else
    //        {
    //            if (status == true)
    //            {
    //                MAINWINDOW.ApplicationSignatureStatusOK.Visibility = System.Windows.Visibility.Visible;
    //                MAINWINDOW.ApplicationSignatureStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //            }
    //            else
    //            {
    //                MAINWINDOW.ApplicationSignatureStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //                MAINWINDOW.ApplicationSignatureStatusBad.Visibility = System.Windows.Visibility.Visible;
    //            }
    //        }            
    //    }

    //    public static void ApplicationDatabaseStatus(bool status)
    //    {
    //        if (absenteeMode)
    //        {
    //            if (status == true)
    //            {
    //                MAINABSENTEEWINDOW.ApplicationDatabaseStatusOK.Visibility = System.Windows.Visibility.Visible;
    //                MAINABSENTEEWINDOW.ApplicationDatabaseStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //                MAINABSENTEEWINDOW.ApplicationDatabaseStatusSpinner.Visibility = System.Windows.Visibility.Collapsed;
    //            }
    //            else
    //            {
    //                MAINABSENTEEWINDOW.ApplicationDatabaseStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //                MAINABSENTEEWINDOW.ApplicationDatabaseStatusBad.Visibility = System.Windows.Visibility.Visible;
    //                MAINABSENTEEWINDOW.ApplicationDatabaseStatusSpinner.Visibility = System.Windows.Visibility.Collapsed;
    //            }
    //        }
    //        else
    //        {
    //            if (status == true)
    //            {
    //                MAINWINDOW.ApplicationDatabaseStatusOK.Visibility = System.Windows.Visibility.Visible;
    //                MAINWINDOW.ApplicationDatabaseStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //                MAINWINDOW.ApplicationDatabaseStatusSpinner.Visibility = System.Windows.Visibility.Collapsed;
    //            }
    //            else
    //            {
    //                MAINWINDOW.ApplicationDatabaseStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //                MAINWINDOW.ApplicationDatabaseStatusBad.Visibility = System.Windows.Visibility.Visible;
    //                MAINWINDOW.ApplicationDatabaseStatusSpinner.Visibility = System.Windows.Visibility.Collapsed;
    //            }
    //        }
    //    }

    //    public static void ApplicationDatabaseChecking()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ApplicationDatabaseStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //            MAINABSENTEEWINDOW.ApplicationDatabaseStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //            MAINABSENTEEWINDOW.ApplicationDatabaseStatusSpinner.Visibility = System.Windows.Visibility.Visible;
    //        }
    //        else
    //        {
    //            MAINWINDOW.ApplicationDatabaseStatusOK.Visibility = System.Windows.Visibility.Collapsed;
    //            MAINWINDOW.ApplicationDatabaseStatusBad.Visibility = System.Windows.Visibility.Collapsed;
    //            MAINWINDOW.ApplicationDatabaseStatusSpinner.Visibility = System.Windows.Visibility.Visible;
    //        }
    //    }

    //    public static void ApplicationStatusClear()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ApplicationStatusLeft.Text = "";
    //            MAINABSENTEEWINDOW.ApplicationStatusCenter.Text = "";
    //            //MAINABSENTEEWINDOW.ApplicationStatusRight.Text = AppSettings.System.BODName + " v" + AppSettings.System.BODVersion.ToString() + "";
    //            MAINABSENTEEWINDOW.ApplicationStatusRight.Text = "";
    //        }
    //        else
    //        {
    //            MAINWINDOW.ApplicationStatusLeft.Text = "";
    //            MAINWINDOW.ApplicationStatusCenter.Text = "";
    //            //MAINWINDOW.ApplicationStatusRight.Text = AppSettings.System.BODName + " v" + AppSettings.System.BODVersion.ToString() + "";
    //            MAINABSENTEEWINDOW.ApplicationStatusRight.Text = "";
    //        }            
    //        //System.Threading.Thread.Sleep(100);
    //    }

    //    // Toggle the loading spinner on and off
    //    public static void LoadingSpinnerToggle()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.LoadingSpinner.Visibility = MAINWINDOW.LoadingSpinner.Visibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
    //        }
    //        else
    //        {
    //            MAINWINDOW.LoadingSpinner.Visibility = MAINWINDOW.LoadingSpinner.Visibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
    //        }
    //    }

    //    // Directly set loading spinner visibility
    //    public static void LoadingSpinner(System.Windows.Visibility state)
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.LoadingSpinner.Visibility = state;
    //        }
    //        else
    //        {
    //            MAINWINDOW.LoadingSpinner.Visibility = state;
    //        }
    //    }

    //    // Directly set loading spinner visibility
    //    public static void LoadingPrinterSpinner(System.Windows.Visibility state)
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ApplicationPrinterStatusSpinner.Visibility = state;
    //        }
    //        else
    //        {
    //            MAINWINDOW.ApplicationPrinterStatusSpinner.Visibility = state;
    //        }
    //    }

    //    // Directly set loading spinner visibility
    //    public static void LoadingSignatureSpinner(System.Windows.Visibility state)
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ApplicationSignatureStatusSpinner.Visibility = state;
    //        }
    //        else
    //        {
    //            MAINWINDOW.ApplicationSignatureStatusSpinner.Visibility = state;
    //        }
    //    }

    //    public static async void ApplicationCheckPrinterAsync()
    //    {
    //        ApplicationHidePrinterStatus();
    //        LoadingPrinterSpinner(Visibility.Visible);

    //        // Check printer status 
    //        bool status = await Task.Run(()=>PrinterStatus.PrinterIsReadyAsync(AppSettings.Printers.BallotPrinter));

    //        LoadingPrinterSpinner(Visibility.Collapsed);
    //        StatusBar.ApplicationPrinterStatus(status);            
    //    }

    //    public static async void ApplicationCheckSignaturePad()
    //    {
    //        ApplicationHideSignaturePadStatus();
    //        LoadingSignatureSpinner(Visibility.Visible);

    //        if (await Task.Run(() => SignatureMethods.CheckSignaturePadAsync()) != null)
    //        {
    //            LoadingSignatureSpinner(Visibility.Collapsed);
    //            StatusBar.ApplicationSignaturePadStatus(true);
    //        }
    //        else
    //        {
    //            LoadingSignatureSpinner(Visibility.Collapsed);
    //            StatusBar.ApplicationSignaturePadStatus(false);
    //        }
    //    }

    //    public static async Task<bool> CheckServer()
    //    {
    //        ApplicationDatabaseChecking();

    //        if (await Task.Run(() => ServerMethods.CheckConnection(new ElectionFactory())) == "False")
    //        {
    //            ApplicationDatabaseStatus(false);

    //            ApplicationStatusCenter("Database Connection Error");
    //            Console.WriteLine("Database Not Found");

    //            return false;
    //        }
    //        else
    //        {
    //            ApplicationDatabaseStatus(true);
    //            return true;
    //        }
    //        //return true;
    //    }
    //}
}
