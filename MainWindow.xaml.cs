using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using Card_Creator.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Card_Creator
{

    public partial class MainWindow : Window
    {

        List<Card> cards;
        Card currentCard;

        List<Card> cardsToView;

        enum CardSortBy
        {
            ID,
            Name,
            Type,
            Health
        }
        

        public MainWindow()
        {
            InitializeComponent();

            cards = ReadDatabase.getListOfCards();
            cardsToView = cards.ToList();
            Cards_ListView_Main.ItemsSource = cardsToView;

            SortBy_ComboBox.ItemsSource = Enum.GetNames(typeof(CardSortBy));
            SortBy_ComboBox.SelectedIndex = 0;

            UpdateSettings.UpdateDarkMode(this);
            if (Settings.Default.darkmode)
            {
                darkMode.IsChecked = true;
            }
        }


        private void Load_Card_Button_Click(object sender, RoutedEventArgs e)
        {
            Card selectedCard = (Card)Cards_ListView_Main.SelectedItem;

            if (selectedCard != null)
            {
                CardEditor_Tab editCard = new CardEditor_Tab(true, selectedCard);
                editCard.ShowDialog();
                cards = ReadDatabase.getListOfCards();
                cardsToView = cards.ToList();
                RefreshListView();
            }
        }


        private void MainWindow_CreateCard_Button_Click(object sender, RoutedEventArgs e)
        {
            CardEditor_Tab cardEditor = new CardEditor_Tab(false, null);
            cardEditor.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cardEditor.ShowDialog();
            cards = ReadDatabase.getListOfCards();
            cardsToView = cards.ToList();
            RefreshListView();
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

                        CardEditor_Tab cardEditor = new CardEditor_Tab(true, importedCard);
                        cardEditor.ShowDialog();
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
                CardEditor_Tab editCard = new CardEditor_Tab(true, selectedCard);
                editCard.ShowDialog();
                cardsToView = ReadDatabase.getListOfCards();
                RefreshListView();
            }
        }

       
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(cards != null)
            {
                if (string.IsNullOrWhiteSpace(SearchBox.Text) || SearchBox.Text == "" || SearchBox.Text == "Search...")
                {
                    cardsToView = cards.ToList();
                    RefreshListView();
                    return;
                }

                cardsToView.Clear();

                foreach (Card c in cards)
                {
                    if (c.Name.ToLower().Contains(SearchBox.Text.ToLower()))
                    {
                        cardsToView.Add(c);
                    }
                }
                Console.WriteLine("adding" + cardsToView.Count);
                RefreshListView();

                //cards = cards.OrderBy(o=>o.Name).ToList();
                //cards.Sort((x, y) => x.Name.CompareTo(y.Name));
            }
        }


        private void SortBy_ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            RefreshListView();
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


        private void RefreshListView()
        {
            Console.WriteLine(SortBy_ComboBox.SelectedItem);

            switch (Enum.Parse(typeof(CardSortBy), SortBy_ComboBox.SelectedItem.ToString()))
            {
                case CardSortBy.ID:
                    cardsToView = cardsToView.OrderBy(c => c.ID).ToList();
                    break;
                case CardSortBy.Name:
                    cardsToView = cardsToView.OrderBy(c => c.Name).ToList();
                    break;
                case CardSortBy.Type:
                    cardsToView = cardsToView.OrderBy(c => c.CardTypeID).ToList();
                    break;
                case CardSortBy.Health:
                    cardsToView = cardsToView.OrderByDescending(c => c.HP).ToList();
                    break;
            }

            Cards_ListView_Main.ItemsSource = cardsToView;
            Cards_ListView_Main.Items.Refresh();
        }
    }
}
