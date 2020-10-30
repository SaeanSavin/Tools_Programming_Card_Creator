using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Card_Creator.Classes
{
    public static class UpdateSettings
    {
        public static void UpdateDarkMode(Window window)
        {
            if (Properties.Settings.Default.darkmode)
            {
                window.Background = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                window.Background = new SolidColorBrush(Colors.White);
            }
        }
    }
}
