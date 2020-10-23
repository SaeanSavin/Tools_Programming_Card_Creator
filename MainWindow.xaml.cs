using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Card_Creator
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            comboBox_01.ItemsSource = typeof(Colors).GetProperties();
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
    }
}
