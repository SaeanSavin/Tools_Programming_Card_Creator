using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;

namespace Card_Creator
{

    public partial class Create_Card_Window : Window
    {
        CardType currentType;
        List<CardType> cardTypes;

        Card currentCard;
        bool editCard;

        public Create_Card_Window(bool editCard, Card card)
        {
            InitializeComponent();

            UpdateSettings.UpdateDarkMode(this);

            ReadDatabase();

            this.editCard = editCard;
            currentCard = card;

            if(cardTypes.Count > 0)
            {
                create_card_comboBox_type.SelectedIndex = 0;
            }
            else
            {
                edit_type_button.IsEnabled = false;
            }

            if (editCard)
            {
                Create_Window.Title = "Edit card";
                create_card_button.Content = "Save";
                //delete_card_button.Visibility = Visibility.Visible; //TODO
            }

            if (card != null)
            {

                /*
                name_textBox.Text = card.Name;

                CardType type;

                using (var context = new CardContext())
                {
                    type = context.CardTypes.Single(a => a.ID == card.CardTypeID);
                }

                create_card_comboBox_type.SelectedItem = type;
                */

                name_textBox.Text = card.Name;

                hp_textBox.Text = card.HP.ToString();

            }
        }

        private void Add_type_button_Click(object sender, RoutedEventArgs e)
        {
            Add_Type_Window add_Type_Window = new Add_Type_Window(false, null);
            add_Type_Window.ShowDialog();
            ReadDatabase();
        }

        private void Edit_type_button_Click(object sender, RoutedEventArgs e)
        {
            Add_Type_Window add_Type_Window = new Add_Type_Window(true, currentType);
            add_Type_Window.ShowDialog();
            ReadDatabase();
        }

        private void Generate_card_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Select_Image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openImage = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };

            if (openImage.ShowDialog() == true)
            {
                string fullpath = openImage.FileName;
                ImageSourceConverter converter = new ImageSourceConverter();
                Card_Preview.img.Source = (ImageSource)converter.ConvertFromString(fullpath);
            }

        }

        private void Create_card_comboBox_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentType = (CardType)create_card_comboBox_type.SelectedItem;

            if(create_card_comboBox_type.SelectedIndex >= 0)
            {
                edit_type_button.IsEnabled = true;
            }

            if(currentType != null)
            {
                Card_Preview.type.Content = "Type: " + currentType.Name;
                Card_Preview.borderColor.BorderBrush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString(currentType.Cardcolor);
            }
        }

        void ReadDatabase()
        {
            using (CardContext context = new CardContext())
            {
                cardTypes = context.CardTypes.ToList();

                if(cardTypes.Count > 0)
                {
                    create_card_comboBox_type.ItemsSource = cardTypes;

                    create_card_comboBox_type.SelectedIndex = 0;

                    currentType = (CardType)create_card_comboBox_type.SelectedItem;

                    if (currentType != null)
                    {
                        create_card_comboBox_type.SelectedIndex = currentType.ID;
                    }
                }
                else
                {
                    create_card_comboBox_type.SelectedIndex = -1;
                    create_card_comboBox_type.ItemsSource = null;
                    edit_type_button.IsEnabled = false;
                }
            }
        }

        private void Name_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Card_Preview.name.Content = name_textBox.Text;
        }

        private void Name_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^A-Za-z]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void Hp_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Card_Preview.hp.Content = "HP: " + hp_textBox.Text;
        }

        private void Hp_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void Create_card_button_Click(object sender, RoutedEventArgs e)
        {
            using (CardContext context = new CardContext())
            {
                if (editCard)
                {

                    Card updatedCard = context.Cards.Find(currentCard.ID);

                    updatedCard.Name = name_textBox.Text;
                    updatedCard.CardTypeID = ((CardType)create_card_comboBox_type.SelectedItem).ID;
                    updatedCard.HP = int.Parse(hp_textBox.Text);

                    context.SaveChanges();
                }
                else
                {
                    Card newCard = new Card()
                    {
                        Name = name_textBox.Text,
                        CardTypeID = ((CardType)create_card_comboBox_type.SelectedItem).ID
                        //ImagePath = 
                        
                };

                    context.Cards.Add(newCard);
                    context.SaveChanges();
                }
            }
            Close();
        }
    }
}
