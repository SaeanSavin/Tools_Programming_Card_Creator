using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
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

        List<CardType> cardTypes;

        List<Card> cardsToView;
        List<CardType> selectedTypes;

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
            cardTypes = ReadDatabase.getListOfCardTypes();
            cardsToView = cards.ToList();
            selectedTypes = cardTypes.ToList();
            MainWindow_Cards_ListView.ItemsSource = cardsToView;

            MainWindow_SortBy_ComboBox.ItemsSource = Enum.GetNames(typeof(CardSortBy));
            MainWindow_SortBy_ComboBox.SelectedIndex = 0;

            MainWindow_FilterBy_Type_ListBox.ItemsSource = cardTypes;

            UpdateSettings.UpdateDarkMode(this);
            if (Settings.Default.darkmode)
            {
                darkMode.IsChecked = true;
            }

            RefreshListView();
        }


        private void MainWindow_LoadCard_Button_Click(object sender, RoutedEventArgs e)
        {
            Card selectedCard = (Card)MainWindow_Cards_ListView.SelectedItem;

            if (selectedCard != null)
            {
                CardEditor editCard = new CardEditor(true, selectedCard);
                editCard.ShowDialog();
                cards = ReadDatabase.getListOfCards();
                cardsToView = cards.ToList();
                cardTypes = ReadDatabase.getListOfCardTypes();
                MainWindow_FilterBy_Type_ListBox.ItemsSource = cardTypes;
                MainWindow_Cards_ListView.ItemsSource = cards;
                RefreshListView();
            }
        }


        private void MainWindow_CreateCard_Button_Click(object sender, RoutedEventArgs e)
        {
            CardEditor cardEditor = new CardEditor(false, null);
            cardEditor.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cardEditor.ShowDialog();
            cards = ReadDatabase.getListOfCards();
            cardsToView = cards.ToList();
            cardTypes = ReadDatabase.getListOfCardTypes();
            MainWindow_FilterBy_Type_ListBox.ItemsSource = cardTypes;
            MainWindow_Cards_ListView.ItemsSource = cards;
            RefreshListView();
        }


        private void MainWindow_MenuItem_Click(object sender, RoutedEventArgs e) { }


        private void MainWindow_ImportFromJSON(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openJSON = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json"
            };
            openJSON.CheckFileExists = true;

            if (openJSON.ShowDialog() == true)
            {
                if (openJSON.FileName.Trim() != string.Empty)
                {
                    using (StreamReader sr = new StreamReader(openJSON.FileName))
                    {
                        string json = sr.ReadToEnd();

                        Card importedCard = JsonConvert.DeserializeObject<Card>(json);

                        CardEditor cardEditor = new CardEditor(true, importedCard);
                        cardEditor.ShowDialog();
                    }
                }
            }
        }


        private void MainWindow_ExportFromJSON(object sender, RoutedEventArgs e)
        {
            if(currentCard == null)
            {
                MessageBox.Show("No card selected for exporting", "No card Selected");
                return;
            }

            SaveFileDialog saveJSON = new SaveFileDialog();
            saveJSON.Filter = "JSON file (*.JSON)|*.JSON";


            if (saveJSON.ShowDialog() == true)
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


        private void MainWindow_Cards_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentCard = (Card)MainWindow_Cards_ListView.SelectedItem;
        }


        private void MainWindow_Cards_ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Card selectedCard = (Card)MainWindow_Cards_ListView.SelectedItem;

            if (selectedCard != null)
            {
                CardEditor editCard = new CardEditor(true, selectedCard);
                editCard.Left = this.Left;
                editCard.Top = this.Top;
                editCard.ShowDialog();
                cards = ReadDatabase.getListOfCards();
                cardTypes = ReadDatabase.getListOfCardTypes();
                MainWindow_FilterBy_Type_ListBox.ItemsSource = cardTypes;
                MainWindow_Cards_ListView.ItemsSource = cards;
                RefreshListView();
            }
        }


        private void MainWindow_Searchbox_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshListView();
        }


        private void MainWindow_SortBy_ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            RefreshListView();
        }


        private void MainWindow_SearchBox_GotFocus(object sender, EventArgs e)
        {
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
            }
        }


        private void MainWindow_SearchBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
            }
        }

        
        private void MainWindow_FilterBy_Type_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTypes = MainWindow_FilterBy_Type_ListBox.SelectedItems.Cast<CardType>().ToList();
            RefreshListView();
        }


        private void RefreshListView()
        {

            //1: add matching searches if search has input
            if (cards != null)
            {
                if (string.IsNullOrWhiteSpace(SearchBox.Text) || SearchBox.Text == "" || SearchBox.Text == "Search...")
                {
                    cardsToView = cards.ToList();
                }
                else
                {
                    cardsToView.Clear();

                    foreach (Card c in cards)
                    {
                        if (c.Name.ToLower().Contains(SearchBox.Text.ToLower()))
                        {
                            cardsToView.Add(c);
                        }
                    }
                }
            }

            //2: remove cards if any filters have been checked
            if (selectedTypes != null)
            {
                if (selectedTypes.Count < cardTypes.Count && selectedTypes.Count != 0)
                {
                    foreach (Card c in cardsToView.ToList())
                    {
                        bool removeCard = true;
                        foreach (CardType t in selectedTypes.ToList())
                        {
                            if (c.CardTypeID == t.ID)
                            {
                                removeCard = false;
                            }
                        }
                        if (removeCard)
                        {
                            cardsToView.Remove(c);
                        }
                    }
                }
            }

            //3: sort by current sorting criteria
            if (MainWindow_SortBy_ComboBox != null)
            {
                switch (Enum.Parse(typeof(CardSortBy), MainWindow_SortBy_ComboBox.SelectedItem.ToString()))
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
            }

            MainWindow_Cards_ListView.ItemsSource = cardsToView;
            MainWindow_Cards_ListView.Items.Refresh();
        }
    }
}
