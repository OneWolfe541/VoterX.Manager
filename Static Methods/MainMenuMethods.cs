using VoterX.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VoterX.Methods
{
    //public static class MainMenuMethods
    //{
    //    private static MainWindow MAINWINDOW = ((MainWindow)System.Windows.Application.Current.MainWindow);
    //    private static MainAbsenteeWindow MAINABSENTEEWINDOW = ((MainAbsenteeWindow)System.Windows.Application.Current.Windows.OfType<MainAbsenteeWindow>().SingleOrDefault());

    //    private static bool absenteeMode = AppSettings.AbsenteeMode;

    //    public static void LoadMenu(Page page, MenuCollapseMode mode)
    //    {
    //        if (absenteeMode)
    //        {
    //            //MAINABSENTEEWINDOW.MenuFrame.Navigate(page);

    //            ((App)Application.Current).mainSliderMenu.SetMenu(page, mode);
    //        }
    //        else
    //        {
    //            //MAINWINDOW.MenuFrame.Navigate(page);
    //        }
    //    }

    //    public static void ShowExitButton()
    //    {
    //        if (absenteeMode)
    //        {
    //            //MAINABSENTEEWINDOW.CloseButton.Visibility = Visibility.Visible;
    //            //MAINABSENTEEWINDOW.fa_bars.Visibility = Visibility.Collapsed;
    //            //MAINABSENTEEWINDOW.HamburgerButton.Visibility = Visibility.Collapsed;

    //            ((App)Application.Current).mainHeader.HamburgerMenuVisibility = false;
    //            ((App)Application.Current).mainHeader.CloseButtonVisibility = true;

    //        }
    //        else
    //        {
    //            //MAINWINDOW.CloseButton.Visibility = Visibility.Visible;
    //            //MAINWINDOW.fa_bars.Visibility = Visibility.Collapsed;
    //            //MAINWINDOW.HamburgerButton.Visibility = Visibility.Collapsed;
    //        }
    //    }

    //    public static void HideExitButton()
    //    {
    //        if (absenteeMode)
    //        {
    //            //MAINABSENTEEWINDOW.CloseButton.Visibility = Visibility.Collapsed;
    //            //MAINABSENTEEWINDOW.fa_bars.Visibility = Visibility.Visible;
    //            ((App)Application.Current).mainHeader.CloseButtonVisibility = false;
    //        }
    //        else
    //        {
    //            //MAINWINDOW.CloseButton.Visibility = Visibility.Collapsed;
    //            //MAINWINDOW.fa_bars.Visibility = Visibility.Visible;
    //        }
    //    }

    //    public static void ShowHamburger()
    //    {
    //        if (absenteeMode)
    //        {
    //            //MAINABSENTEEWINDOW.HamburgerButton.Visibility = Visibility.Visible;
    //            ((App)Application.Current).mainHeader.HamburgerMenuVisibility = true;
    //        }
    //        else
    //        {
    //            //MAINWINDOW.HamburgerButton.Visibility = Visibility.Visible;
    //        }
    //    }

    //    public static void HideHamburger()
    //    {
    //        if (absenteeMode)
    //        {
    //            //MAINABSENTEEWINDOW.HamburgerButton.Visibility = Visibility.Collapsed;
    //            ((App)Application.Current).mainHeader.HamburgerMenuVisibility = true;
    //        }
    //        else
    //        {
    //            //MAINWINDOW.HamburgerButton.Visibility = Visibility.Collapsed;
    //        }
    //    }

    //    public static void ShowSettingsMenu()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ShowSettingsMenu();
    //        }
    //        else
    //        {
    //            MAINWINDOW.ShowSettingsMenu();
    //        }
    //    }

    //    public static void OpenAdminMenu()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.OpenMenuSlider();
    //        }
    //        else
    //        {
    //            MAINWINDOW.OpenMenuSlider();
    //        }
    //    }

    //    public static void CloseAdminMenu()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.CloseMenuSlider();
    //        }
    //        else
    //        {
    //            MAINWINDOW.CloseMenuSlider();
    //        }
    //    }

    //    public static void OpenMainMenu()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.OpenMenuSlider();
    //        }
    //        else
    //        {
    //            MAINWINDOW.OpenMenuSlider();
    //        }
    //    }

    //    public static void CloseMainMenu()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.CloseMenuSlider();
    //        }
    //        else
    //        {
    //            MAINWINDOW.CloseMenuSlider();
    //        }
    //    }

    //    public static void HideMainMenu()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.HideMenuSlider();
    //        }
    //        else
    //        {
    //            MAINWINDOW.CloseMenuSlider();
    //        }
    //    }

    //    public static void HideHomeMenu()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.HideHomeMenu();
    //        }
    //        else
    //        {
    //            //MAINWINDOW.CloseMenuSlider();
    //        }
    //    }

    //    public static void CloseMainWindow()
    //    {
    //        ((MainWindow)System.Windows.Application.Current.MainWindow).Close();
    //    }

    //    public static void AnimateHome()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.AnimateHome();
    //        }
    //    }

    //    public static void SetMenuMinWidth(int width)
    //    {
    //        MAINABSENTEEWINDOW.menuMinWidth = width;
    //    }

    //    public static void ShowMenuMinimum()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ShowMenuMinimum();
    //        }
    //    }

    //    public static void ToggleElectionVisibility()
    //    {
    //        if (absenteeMode)
    //        {
    //            MAINABSENTEEWINDOW.ToggleElectionInfo();
    //        }
    //    }
    //}
}
