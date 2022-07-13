using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VoterX
{
    /// <summary>
    /// The base template for application settings pages.
    /// Settings pages that inherit from this template gain SaveSettings(), 
    /// a shared method that can be called from their parent frame.
    /// </summary>
    public abstract class SettingsBasePage : Page
    {
        public abstract void SaveSettings();
    }
}
