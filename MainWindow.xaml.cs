using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using Card_Creator.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Card_Creator
{

    public partial class MainWindow : Window
    {

        List<Card> cards;
        Card currentCard;
        
        public MainWindow()
        {
            InitializeComponent();

            cards = ReadDatabase.getListOfCards();

            refreshListView();

            UpdateSettings.UpdateDarkMode(this);
            if (Settings.Default.darkmode)
            {
                darkMode.IsChecked = true;
            }
        }

        //Remove later?
        private void Load_Card_Button_Click(object sender, RoutedEventArgs e)
        {
            Load_Card_Window load_Card_Window = new Load_Card_Window();
            load_Card_Window.ShowDialog();
            cards = ReadDatabase.getListOfCards();
            refreshListView();
        }

        private void MainWindow_CreateCard_Button_Click(object sender, RoutedEventArgs e)
        {
            CardEditor cardEditor = new CardEditor(false, null);
            cardEditor.ShowDialog();
            cards = ReadDatabase.getListOfCards();
            refreshListView();
        }

        private void MainWindow_MenuItem_Click(object sender, RoutedEventArgs e){}

        private void MainWindow_ImportFromJSON(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openJSON = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json"
            };
            openJSON.CheckFileExists = true;

            if (openJSON.ShowDialog() == true)
            {
                if(openJSON.FileName.Trim() != string.Empty)
                {
                    using (StreamReader sr = new StreamReader(openJSON.FileName))
                    {
                        string json = sr.ReadToEnd();

                        Card importedCard = JsonConvert.DeserializeObject<Card>(json);

                        CardEditor create_Card_Window = new CardEditor(true, importedCard);
                        create_Card_Window.ShowDialog();
                    }
                }
            }
        }

        private void MainWindow_ExportFromJSON(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveJSON = new SaveFileDialog();
            saveJSON.Filter = "JSON file (*.JSON)|*.JSON";

            
            if(saveJSON.ShowDialog() == true)
            {
                string output = JsonConvert.SerializeObject(currentCard, Formatting.Indented);

                File.WriteAllText(saveJSON.FileName, output);
            }

        }

        private void DarkMode_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.darkmode = true;
            Settings.Default.Save();

            foreach (Window window in Application.Current.Windows)
            {
                window.Background = new SolidColorBrush(Colors.Gray);
            }
        }

        public void DarkMode_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.darkmode = false;
            Settings.Default.Save();

            foreach (Window window in Application.Current.Windows)
            {
                window.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void Cards_ListView_Main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentCard = (Card)Cards_ListView_Main.SelectedItem;
        }

        private void Cards_ListView_Main_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Card selectedCard = (Card)Cards_ListView_Main.SelectedItem;

            if(selectedCard != null)
            {
                CardEditor editCard = new CardEditor(true, selectedCard);
                editCard.ShowDialog();
                cards = ReadDatabase.getListOfCards();
            }
        }

        private void refreshListView()
        {
            if (Cards_ListView_Main != null)
            {
                foreach (Card c in cards)
                {
                    Cards_ListView_Main.ItemsSource = cards;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(cards != null)
            {
                cards = cards.OrderBy(o=>o.Name).ToList();
                //cards.Sort((x, y) => x.Name.CompareTo(y.Name));
                refreshListView();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBox_GotFocus(object sender, EventArgs e)
        {
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
            }
        }

        private void SearchBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
            }
        }
    }
}
