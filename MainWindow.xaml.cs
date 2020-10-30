using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using Card_Creator.Properties;

namespace Card_Creator
{

    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            
            
            UpdateSettings.UpdateDarkMode(this);
            if (Properties.Settings.Default.darkmode)
            {
                darkMode.IsChecked = true;
            }
        }

        private void ComboBox_01_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Load_Card_Button_Click(object sender, RoutedEventArgs e)
        {
    
        }

        private void Create_Card_Button_Click(object sender, RoutedEventArgs e)
        {
            Create_Card_Window create_Card_Window = new Create_Card_Window();
            create_Card_Window.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

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
