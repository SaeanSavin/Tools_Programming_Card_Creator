using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Shapes;

namespace Card_Creator
{

    public partial class CardEditor : Window
    {
      
        public static BitmapImage noImage = new BitmapImage(new Uri(@"pack://application:,,,/Card-Creator;Component/Images/no-image.png"));

        private readonly Card currentCard;
        private readonly bool editCard;

        CardType currentType;
        List<CardType> cardTypes;

        Attack currentAttack1;
        Attack currentAttack2;

        List<Attack> attacks;

        private int typeIndex = -1;

        public CardEditor(bool editCard, Card card)
        {
            InitializeComponent();

            UpdateSettings.UpdateDarkMode(this);

            cardTypes = ReadDatabase.getListOfCardTypes();
            attacks = ReadDatabase.getListOfAttacks();
            
            this.editCard = editCard;
            currentCard = card;

            if (CardEditor_Type_Combobox.SelectedIndex == -1)
            {
                CardEditor_EditType_Button.IsEnabled = false;
            }

            if (editCard)
            {
                CardEditor_Tab_Window.Title = "CardEditor - Edit mode";
                CardEditor_CreateCard_Button.Visibility = Visibility.Collapsed;
                CardEditor_CreateCardAndExit_Button.Content = "Save & Close";

                CardEditor_Type_Combobox.SelectedItem = cardTypes.Find(i => i.ID == card.CardTypeID);
                CardEditor_Attack1_Combobox.SelectedItem = attacks.Find(i => i.ID == card.Attack1ID);
                CardEditor_Attack2_Combobox.SelectedItem = attacks.Find(i => i.ID == card.Attack2ID);
            }

            if(card != null)
            {
                CardEditor_Name_Textbox.Text = card.Name;

                CardEditor_HP_Textbox.Text = card.HP.ToString();

                ImageSourceConverter converter = new ImageSourceConverter();
                CardEditor_Card_Preview.Image.Source = (ImageSource)converter.ConvertFromString(card.ImagePath);

                currentType = cardTypes.Find(t => t.ID == card.CardTypeID);
                currentAttack1 = attacks.Find(i => i.ID == card.Attack1ID);
                currentAttack2 = attacks.Find(i => i.ID == card.Attack2ID);

                RefreshTypes();

                CardEditor_Type_Combobox.SelectedItem = currentType;
                if (currentAttack1 != null)
                    CardEditor_Attack1_Combobox.SelectedItem = attacks.Find(a => a.ID == currentAttack1.ID);
                if (currentAttack2 != null)
                    CardEditor_Attack2_Combobox.SelectedItem = attacks.Find(a => a.ID == currentAttack2.ID);
            }
        }


        private void CardEditor_ImageSelector_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openImage = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };

            if (openImage.ShowDialog() == true)
            {
                string fullpath = openImage.FileName;
                ImageSourceConverter converter = new ImageSourceConverter();
                try
                {
                    CardEditor_Card_Preview.Image.Source = (ImageSource)converter.ConvertFromString(fullpath);
                    CardEditor_Error_Image_Label.Content = "";
                }
                catch(NotSupportedException)
                {
                    CardEditor_Card_Preview.Image.Source = noImage;
                    CardEditor_Error_Image_Label.Content = "Error! File not supported";
                }
                
            }
        }


        private void CardEditor_NewType_Button_Click(object sender, RoutedEventArgs e)
        {
            TypeEditor typeEditor = new TypeEditor(false, null);
            typeEditor.Left = this.Left;
            typeEditor.Top = this.Top;
            typeEditor.ShowDialog();
            cardTypes = ReadDatabase.getListOfCardTypes();
            RefreshTypes();
            RefreshAttacks();
        }


        private void CardEditor_EditType_Button_Click(object sender, RoutedEventArgs e)
        {
            TypeEditor typeEditor = new TypeEditor(true, currentType);
            typeEditor.Left = this.Left;
            typeEditor.Top = this.Top;
            typeEditor.ShowDialog();
            cardTypes = ReadDatabase.getListOfCardTypes();

            RefreshTypes();
            RefreshAttacks();
        }

       
        private void CardEditor_Type_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentType = (CardType)CardEditor_Type_Combobox.SelectedItem;

            if (CardEditor_Type_Combobox.SelectedIndex >= 0)
            {
                typeIndex = CardEditor_Type_Combobox.SelectedIndex;
                CardEditor_EditType_Button.IsEnabled = true;
            }

            if (currentType != null)
            {
                CardEditor_HP_Label.Content = "HP: " + ("( " + currentType.MinHP + " - " + currentType.MaxHP +" )");
                CardEditor_Card_Preview.Type.Content = "Type: " + currentType.Name;
                CardEditor_Card_Preview.BorderColor.BorderBrush = (Brush)new BrushConverter().ConvertFromString(currentType.Cardcolor);
            }

            CardEditor_Attack1Edit_Button.IsEnabled = false;
            CardEditor_Attack2Edit_Button.IsEnabled = false;
            RefreshAttacks();
        }


        private void CardEditor_Name_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CardEditor_Card_Preview.Name.Content = CardEditor_Name_Textbox.Text;
        }


        private void CardEditor_Name_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^A-Za-z]+");
            e.Handled = reg.IsMatch(e.Text);
        }


        private void CardEditor_HP_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CardEditor_Card_Preview.HP.Content = "HP: " + CardEditor_HP_Textbox.Text;
        }


        private void CardEditor_HP_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }


        private void CardEditor_Attack1_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentAttack1 = (Attack)CardEditor_Attack1_Combobox.SelectedItem;

            if(CardEditor_Attack1_Combobox.SelectedIndex >= 0)
            {
                CardEditor_Attack1Edit_Button.IsEnabled = true;
            }

            if(currentAttack1 != null)
            {
                CardEditor_Card_Preview.Attack1.Content = currentAttack1.Name;
                CardEditor_Card_Preview.Attack1_Damage.Content = currentAttack1.Damage;
            }

        }


        private void CardEditor_Attack2_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentAttack2 = (Attack)CardEditor_Attack2_Combobox.SelectedItem;

            if (CardEditor_Attack2_Combobox.SelectedIndex >= 0)
            {
                CardEditor_Attack2Edit_Button.IsEnabled = true;
            }

            if (currentAttack2 != null)
            {
                CardEditor_Card_Preview.Attack2.Content = currentAttack2.Name;
                CardEditor_Card_Preview.Attack2_Damage.Content = currentAttack2.Damage;
            }
        }


        private void CardEditor_Attack1Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            AttackEditor attackEditor = new AttackEditor(true, currentAttack1);
            attackEditor.Left = this.Left;
            attackEditor.Top = this.Top;
            attackEditor.ShowDialog();
            attacks = ReadDatabase.getListOfAttacks();
            RefreshTypes();
            RefreshAttacks();
        }


        private void CardEditor_Attack2Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            AttackEditor attackEditor = new AttackEditor(true, currentAttack2);
            attackEditor.Left = this.Left;
            attackEditor.Top = this.Top;
            attackEditor.ShowDialog();
            attacks = ReadDatabase.getListOfAttacks();
            RefreshTypes();
            RefreshAttacks();
        }


        private void CardEditor_NewAttack_Button_Click(object sender, RoutedEventArgs e)
        {
            AttackEditor attackEditor = new AttackEditor(false, null);
            attackEditor.Left = this.Left;
            attackEditor.Top = this.Top;
            attackEditor.ShowDialog();
            attacks = ReadDatabase.getListOfAttacks();
            RefreshAttacks();
        }


        private void CardEditor_SaveCard_Button_Click(object sender, RoutedEventArgs e)
        {

            if (!CheckValidInput())
            {
                MessageBox.Show("Some values are not valid!", "Invalid Input!");
                return;
            }

            using (CardContext context = new CardContext())
            {
                if (editCard)
                {

                    Card updatedCard = context.Cards.Find(currentCard.ID);

                    updatedCard.Name = CardEditor_Name_Textbox.Text;
                    
                    if(CardEditor_Type_Combobox.SelectedIndex == -1)
                    {
                        updatedCard.CardTypeID = 0;
                    }
                    else
                    {
                        updatedCard.CardTypeID = ((CardType)CardEditor_Type_Combobox.SelectedItem).ID;
                    }
                    
                    updatedCard.HP = int.Parse(CardEditor_HP_Textbox.Text);
                    updatedCard.ImagePath = CardEditor_Card_Preview.Image.Source.ToString();
                    
                    if(CardEditor_Attack1_Combobox.SelectedIndex == -1)
                    {
                        updatedCard.Attack1ID = 0;
                    }
                    else
                    {
                        updatedCard.Attack1ID = ((Attack)CardEditor_Attack1_Combobox.SelectedItem).ID;
                    }


                    if (CardEditor_Attack2_Combobox.SelectedIndex == -1)
                    {
                        updatedCard.Attack2ID = 0;
                    }
                    else
                    {
                        updatedCard.Attack2ID = ((Attack)CardEditor_Attack2_Combobox.SelectedItem).ID;
                    }

                    context.SaveChanges();
                }
                else
                {
                    Card newCard = new Card
                    {
                        Name = CardEditor_Name_Textbox.Text,
                        HP = int.Parse(CardEditor_HP_Textbox.Text),
                        ImagePath = CardEditor_Card_Preview.Image.Source.ToString()
                    };

                    if (CardEditor_Type_Combobox.SelectedIndex == -1)
                    {
                        newCard.CardTypeID = 0;
                    }
                    else
                    {
                        newCard.CardTypeID = ((CardType)CardEditor_Type_Combobox.SelectedItem).ID;
                    }

                    if (CardEditor_Attack1_Combobox.SelectedIndex == -1)
                    {
                        newCard.Attack1ID = 0;
                    }
                    else
                    {
                        newCard.Attack1ID = ((Attack)CardEditor_Attack1_Combobox.SelectedItem).ID;
                    }


                    if (CardEditor_Attack2_Combobox.SelectedIndex == -1)
                    {
                        newCard.Attack2ID = 0;
                    }
                    else
                    {
                        newCard.Attack2ID = ((Attack)CardEditor_Attack2_Combobox.SelectedItem).ID;
                    }
    
                    context.Cards.Add(newCard);
                    context.SaveChanges();
                }
            }

            if(((Button)sender).Name == CardEditor_CreateCard_Button.Name)
            {
                ClearAllTextBoxes();
                string popupText = "successfully created new Card";
                if (editCard)
                {
                    popupText = "changes saved";
                }
                Popup popup = new Popup(popupText);
                popup.Left = this.Left + (this.ActualWidth - popup.ActualWidth) / 2;
                popup.Top = this.Top + (this.ActualHeight - popup.ActualHeight) / 2;
                popup.ShowDialog();
            }
            else if (((Button)sender).Name == CardEditor_CreateCardAndExit_Button.Name) {
                Close();
            }
        }


        private void CardEditor_Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void RefreshTypes()
        {
            if (cardTypes.Count > 0)
            {
                CardEditor_Type_Combobox.ItemsSource = cardTypes;
                CardEditor_Type_Combobox.SelectedIndex = typeIndex;
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
            if (currentType != null)
            {
                attacks.Clear();
                foreach(Attack a in ReadDatabase.getListOfAttacks())
                {
                    if(a.CardTypeID == currentType.ID)
                    {
                        attacks.Add(a);
                    }
                }

                CardEditor_Attack1_Combobox.ItemsSource = attacks;
                CardEditor_Attack2_Combobox.ItemsSource = attacks;

                CardEditor_Attack1_Combobox.Items.Refresh();
                CardEditor_Attack2_Combobox.Items.Refresh();

                if(currentAttack1 != null)
                    CardEditor_Attack1_Combobox.SelectedItem = attacks.Find(a => a.ID == currentAttack1.ID);
                if(currentAttack2 != null)
                    CardEditor_Attack2_Combobox.SelectedItem = attacks.Find(a => a.ID == currentAttack2.ID);

                if (attacks.Count == 0)
                {
                    CardEditor_Attack1Edit_Button.IsEnabled = false;
                    CardEditor_Attack2Edit_Button.IsEnabled = false;
                    CardEditor_Card_Preview.Attack1.Content = "Attack1:";
                    CardEditor_Card_Preview.Attack2.Content = "Attack2:";
                    CardEditor_Card_Preview.Attack1_Damage.Content = "0";
                    CardEditor_Card_Preview.Attack2_Damage.Content = "0";

                }

            }
            else if(currentType == null)
            {
                attacks.Clear();
                CardEditor_Card_Preview.Type.Content = "No type";

                CardEditor_Attack1_Combobox.Items.Refresh();
                CardEditor_Attack2_Combobox.Items.Refresh();

                CardEditor_Attack1_Combobox.SelectedIndex = -1;
                CardEditor_Attack2_Combobox.SelectedIndex = -1;
            }
            else if (attacks.Count > 0)
            {
                CardEditor_Attack1_Combobox.ItemsSource = attacks;
                CardEditor_Attack2_Combobox.ItemsSource = attacks;

            }
        }


        private bool CheckValidInput()
        {
            bool isValid = true;

            if (CardEditor_Name_Textbox.Text == "")
            {
                CardEditor_Error_Name_Label.Content = "Invalid Input!";
                isValid = false;
            }
            else
            {
                CardEditor_Error_Name_Label.Content = "";
            }

            if(currentType != null)
            {
                if (CardEditor_HP_Textbox.Text == "")
                {
                    CardEditor_Error_HP_Label.Content = "Invalid Input!";
                    isValid = false;
                }
                else if (int.Parse(CardEditor_HP_Textbox.Text) < currentType.MinHP)
                {
                    CardEditor_Error_HP_Label.Content = "Hp cannot be lower than the minHP of the card type";
                    isValid = false;
                }
                else if (int.Parse(CardEditor_HP_Textbox.Text) > currentType.MaxHP)
                {
                    CardEditor_Error_HP_Label.Content = "Hp cannot be higher than the minHP of the card type";
                    isValid = false;
                }
                else
                {
                    CardEditor_Error_HP_Label.Content = "";
                }
            }
            return isValid;
        }


        private void ClearAllTextBoxes()
        {

            CardEditor_Name_Textbox.Text = "";
            CardEditor_HP_Textbox.Text = "";

            CardEditor_HP_Label.Content = "HP: ";

            CardEditor_Card_Preview.Image.Source = noImage;
            CardEditor_Card_Preview.Name.Content = "Name";
            
            CardEditor_Card_Preview.Attack1.Content = "Attack1:";
            CardEditor_Card_Preview.Attack2.Content = "Attack2:";
            CardEditor_Card_Preview.Attack1_Damage.Content = "0";
            CardEditor_Card_Preview.Attack2_Damage.Content = "0";

            CardEditor_Card_Preview.Type.Content = "No type";

            CardEditor_Card_Preview.BorderColor.BorderBrush = Brushes.Transparent;

            CardEditor_Type_Combobox.SelectedIndex = -1;
            CardEditor_Attack1_Combobox.SelectedIndex = -1;
            CardEditor_Attack2_Combobox.SelectedIndex = -1;

            currentType = null;
            currentAttack1 = null;
            currentAttack2 = null;

            RefreshAttacks();
        }

    }
}
