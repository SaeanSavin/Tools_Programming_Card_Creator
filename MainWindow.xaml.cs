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
        List<Card> cards;
        
        public MainWindow()
        {
            InitializeComponent();

            System.Drawing.Color bgColor = Properties.Settings.Default.background;
            Color bgColorMedia = Color.FromArgb(bgColor.A, bgColor.R, bgColor.G, bgColor.B);
            Background = new SolidColorBrush(bgColorMedia);

            using (CardContext context = new CardContext())
            {
                cards = context.Cards.ToList();
                main_window_cards_box.ItemsSource = cards;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings_Window settings_Window = new Settings_Window();
            settings_Window.ShowDialog();
        }
    }
}
