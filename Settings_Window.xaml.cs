using Card_Creator.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Card_Creator
{
    public partial class Settings_Window : Window
    {
        public Settings_Window()
        {
            InitializeComponent();
            UpdateSettings.UpdateDarkMode(this);

            if (Properties.Settings.Default.darkmode)
            {
                darkMode.IsChecked = true;
            }
        }

        private void darkMode_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.darkmode = true;
            Properties.Settings.Default.Save();

            foreach (Window window in Application.Current.Windows)
            {
                window.Background = new SolidColorBrush(Colors.Gray);
            }

        }

        public void darkMode_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.darkmode = false;
            Properties.Settings.Default.Save();

            foreach (Window window in Application.Current.Windows)
            {
                window.Background = new SolidColorBrush(Colors.White);
            }
        }
    }
}
