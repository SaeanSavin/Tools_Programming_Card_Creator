using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Card_Creator
{

    public partial class CardEditor : Window
    {
        CardType currentType;
        List<CardType> cardTypes;
        List<Attack> attacks;

        private readonly Card currentCard;
        private readonly bool editCard;

        public CardEditor(bool editCard, Card card)
        {
            InitializeComponent();

            UpdateSettings.UpdateDarkMode(this);

            cardTypes = ReadDatabase.getListOfCardTypes();
            attacks = ReadDatabase.getListOfAttacks();
            RefreshTypes();
            RefreshAttacks();

            this.editCard = editCard;
            currentCard = card;

            if(cardTypes.Count > 0)
            {
                CardEditor_Type_Combobox.SelectedIndex = 0;
            }
            else
            {
                CardEditor_EditType_Button.IsEnabled = false;
            }

            if (editCard)
            {
                CardEditor_Window.Title = "CardEditor - Edit mode";
                CardEditor_SaveCard_Button.Content = "Save";
                //delete_card_button.Visibility = Visibility.Visible; //TODO
            }

            if (card != null)
            {

                CardEditor_Name_Textbox.Text = card.Name;

                CardEditor_HP_Textbox.Text = card.HP.ToString();

                ImageSourceConverter converter = new ImageSourceConverter();
                CardEditor_Card_Preview.img.Source = (ImageSource)converter.ConvertFromString(currentCard.ImagePath);

                foreach (CardType cType in cardTypes)
                {
                    if(cType.ID == card.CardTypeID)
                    {
                        currentType = cType;
                        break;
                    }
                }
                
                CardEditor_Type_Combobox.SelectedItem = currentType;
            }
        }

        private void CardEditor_NewType_Button_Click(object sender, RoutedEventArgs e)
        {
            TypeEditor typeEditor = new TypeEditor(false, null);
            typeEditor.ShowDialog();
            cardTypes = ReadDatabase.getListOfCardTypes();
            RefreshTypes();
        }

        private void CardEditor_EditType_Button_Click(object sender, RoutedEventArgs e)
        {
            TypeEditor typeEditor = new TypeEditor(true, currentType);
            typeEditor.ShowDialog();
            cardTypes = ReadDatabase.getListOfCardTypes();
            RefreshTypes();
        }

        private void CardEditor_NewAttack_Button_Click(object sender, RoutedEventArgs e)
        {
            AttackEditor attackEditor = new AttackEditor(false, null);
            attackEditor.ShowDialog();
            attacks = ReadDatabase.getListOfAttacks();
            RefreshAttacks();
        }

        private void CardEtior_EditAttack1_Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("sploink");
            AttackEditor attackEditor = new AttackEditor(true, (Attack)CardEditor_Attack1_Combobox.SelectedItem);
            attackEditor.ShowDialog();
            attacks = ReadDatabase.getListOfAttacks();
            RefreshAttacks();
        }

        private void CardEtior_EditAttack2_Button_Click(object sender, RoutedEventArgs e)
        {
            AttackEditor attackEditor = new AttackEditor(true, (Attack)CardEditor_Attack2_Combobox.SelectedItem);
            attackEditor.ShowDialog();
            attacks = ReadDatabase.getListOfAttacks();
            RefreshAttacks();
        }

        private void CardEditor_GenerateCard_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CardEditor_SelectImage_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openImage = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };

            if (openImage.ShowDialog() == true)
            {
                string fullpath = openImage.FileName;
                ImageSourceConverter converter = new ImageSourceConverter();
                CardEditor_Card_Preview.img.Source = (ImageSource)converter.ConvertFromString(fullpath);
            }

        }

        private void CardEditor_Type_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentType = (CardType)CardEditor_Type_Combobox.SelectedItem;

            if(CardEditor_Type_Combobox.SelectedIndex >= 0)
            {
                CardEditor_EditType_Button.IsEnabled = true;
            }

            if(currentType != null)
            {
                CardEditor_Card_Preview.type.Content = "Type: " + currentType.Name;
                CardEditor_Card_Preview.borderColor.BorderBrush = (Brush)new BrushConverter().ConvertFromString(currentType.Cardcolor);
            }
        }

        private void CardEditor_Name_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CardEditor_Card_Preview.name.Content = CardEditor_Name_Textbox.Text;
        }

        private void CardEditor_Name_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^A-Za-z]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void CardEditor_HP_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CardEditor_Card_Preview.hp.Content = "HP: " + CardEditor_HP_Textbox.Text;
        }

        private void CardEditor_HP_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void CardEditor_CreateCard_Button_Click(object sender, RoutedEventArgs e)
        {
            using (CardContext context = new CardContext())
            {
                if (editCard)
                {

                    Card updatedCard = context.Cards.Find(currentCard.ID);

                    updatedCard.Name = CardEditor_Name_Textbox.Text;
                    updatedCard.CardTypeID = ((CardType)CardEditor_Type_Combobox.SelectedItem).ID;
                    updatedCard.HP = int.Parse(CardEditor_HP_Textbox.Text);
                    updatedCard.ImagePath = CardEditor_Card_Preview.img.Source.ToString();

                    context.SaveChanges();
                }
                else
                {
                    Card newCard = new Card()
                    {
                        Name = CardEditor_Name_Textbox.Text,
                        CardTypeID = ((CardType)CardEditor_Type_Combobox.SelectedItem).ID,
                        HP = int.Parse(CardEditor_HP_Textbox.Text),
                        ImagePath = CardEditor_Card_Preview.img.Source.ToString()
                    };

                    context.Cards.Add(newCard);
                    context.SaveChanges();
                }
            }
            Close();
        }

        private void RefreshTypes()
        {
            if (cardTypes.Count > 0)
            {
                CardEditor_Type_Combobox.ItemsSource = cardTypes;
                CardEditor_Type_Combobox.SelectedIndex = 0;
            }

            else
            {
                CardEditor_Type_Combobox.SelectedIndex = -1;
                CardEditor_Type_Combobox.ItemsSource = null;
                CardEditor_EditType_Button.IsEnabled = false;
            }
        }

        private void RefreshAttacks()
        {
            if(attacks.Count > 0)
            {
                CardEditor_Attack1_Combobox.ItemsSource = attacks;
                CardEditor_Attack1_Combobox.SelectedIndex = 0;
                CardEditor_Attack1_Combobox.Items.Refresh();

                CardEditor_Attack2_Combobox.ItemsSource = attacks;
                CardEditor_Attack2_Combobox.SelectedIndex = 0;
            }
            else
            {
                CardEditor_Attack1_Combobox.SelectedIndex = -1;
                CardEditor_Attack1_Combobox.ItemsSource = null;
            }
        }
    }
}
