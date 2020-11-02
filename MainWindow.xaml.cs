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

            ReadDatabase();
            
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
            //Create_Card_Window create_Card_Window = new Create_Card_Window(true, );
            Load_Card_Window load_Card_Window = new Load_Card_Window();
            load_Card_Window.ShowDialog();
            ReadDatabase();
        }

        private void Create_Card_Button_Click(object sender, RoutedEventArgs e)
        {
            Create_Card_Window create_Card_Window = new Create_Card_Window(false, null);
            create_Card_Window.ShowDialog();
            ReadDatabase();
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


        void ReadDatabase()
        {
            using(CardContext context = new CardContext())
            {
                cards = context.Cards.ToList();

                if(card_list_view != null)
                {
                    foreach(Card c in cards)
                    {
                        card_list_view.ItemsSource = cards;
                    }
                }
            }
        }

        private void Card_list_view_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Card_list_view_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Card selectedCard = (Card)card_list_view.SelectedItem;

            if(selectedCard != null)
            {
                Create_Card_Window editCard = new Create_Card_Window(true, selectedCard);
                editCard.ShowDialog();
                ReadDatabase();
            }
        }
    }
}
