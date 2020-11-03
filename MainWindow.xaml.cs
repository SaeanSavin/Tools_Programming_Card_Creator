using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using Card_Creator.Properties;
using Newtonsoft.Json;

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
            if (Settings.Default.darkmode)
            {
                darkMode.IsChecked = true;
            }
        }

        private void ComboBox_01_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Load_Card_Button_Click(object sender, RoutedEventArgs e)
        {
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

        private void MenuItem_Click(object sender, RoutedEventArgs e){}

        private void Import_From_JSON(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openJSON = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json"
            };
            openJSON.CheckFileExists = true;

            if (openJSON.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(openJSON.FileName.Trim() != string.Empty)
                {
                    using (StreamReader sr = new StreamReader(openJSON.FileName))
                    {
                        string json = sr.ReadToEnd();

                        Card importedCard = JsonConvert.DeserializeObject<Card>(json);

                        Create_Card_Window create_Card_Window = new Create_Card_Window(true, importedCard);
                        create_Card_Window.ShowDialog();
                    }
                }
            }
        }

        private void DarkMode_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.darkmode = true;
            Settings.Default.Save();

            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                window.Background = new SolidColorBrush(Colors.Gray);
            }
        }

        public void DarkMode_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.darkmode = false;
            Settings.Default.Save();

            foreach (Window window in System.Windows.Application.Current.Windows)
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
