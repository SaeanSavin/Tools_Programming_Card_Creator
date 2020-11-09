using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Card_Creator
{

    public partial class CardEditor_Tab : Window
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

        private int attack1Index = -1;
        private int attack2Index = -1;

        public CardEditor_Tab(bool editCard, Card card)
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

            if(editCard)
            {
                CardEditor_Tab_Window.Title = "CardEditor - Edit mode";
                CardEditorTab_CreateCard_Button.Content = "Save Card";
            }

            if(card != null)
            {
                CardEditor_Name_Textbox.Text = card.Name;

                CardEditor_HP_Textbox.Text = card.HP.ToString();

                ImageSourceConverter converter = new ImageSourceConverter();
                CardEditor_Card_Preview.img.Source = (ImageSource)converter.ConvertFromString(card.ImagePath);

                foreach(CardType cardType in cardTypes)
                {
                    if(cardType.ID == card.CardTypeID)
                    {
                        currentType = cardType;
                        break;
                    }
                }

                foreach(Attack attack in attacks)
                {
                    if(attack.ID == card.Attack1ID)
                    {
                        currentAttack1 = attack;
                    }
                    if(attack.ID == card.Attack2ID)
                    {
                        currentAttack2 = attack;
                    }
                }

                CardEditor_Type_Combobox.SelectedItem = currentType;
                CardEditor_Attack1_Combobox.SelectedItem = currentAttack1;
                CardEditor_Attack2_Combobox.SelectedItem = currentAttack2;
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
                    CardEditor_Card_Preview.img.Source = (ImageSource)converter.ConvertFromString(fullpath);
                    CardEditor_Error_Image_Label.Content = "";
                }
                catch(NotSupportedException)
                {
                    CardEditor_Card_Preview.img.Source = noImage;
                    CardEditor_Error_Image_Label.Content = "Error! File not supported";
                }
                
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
                CardEditor_Card_Preview.type.Content = "Type: " + currentType.Name;
                CardEditor_Card_Preview.borderColor.BorderBrush = (Brush)new BrushConverter().ConvertFromString(currentType.Cardcolor);
            }

            RefreshAttacks();
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


        private void CardEditor_Attack1_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentAttack1 = (Attack)CardEditor_Attack1_Combobox.SelectedItem;

            if(CardEditor_Attack1_Combobox.SelectedIndex >= 0)
            {
                attack1Index = CardEditor_Attack1_Combobox.SelectedIndex;
                CardEditor_Attack1Edit_Button.IsEnabled = true;
            }

            if(currentAttack1 != null)
            {
                CardEditor_Card_Preview.attack1.Content = currentAttack1.Name;
                CardEditor_Card_Preview.Attack1_Damage.Content = currentAttack1.Damage;
            }

        }


        private void CardEditor_Attack2_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentAttack2 = (Attack)CardEditor_Attack2_Combobox.SelectedItem;

            if (CardEditor_Attack2_Combobox.SelectedIndex >= 0)
            {
                attack2Index = CardEditor_Attack2_Combobox.SelectedIndex;
                CardEditor_Attack2Edit_Button.IsEnabled = true;
            }

            if (currentAttack2 != null)
            {
                CardEditor_Card_Preview.attack2.Content = currentAttack2.Name;
                CardEditor_Card_Preview.Attack2_Damage.Content = currentAttack2.Damage;
            }
        }


        private void CardEditor_Attack1Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            AttackEditor attackEditor = new AttackEditor(true, currentAttack1);
            attackEditor.ShowDialog();
            attacks = ReadDatabase.getListOfAttacks();
            RefreshAttacks();
        }


        private void CardEditor_Attack2Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            AttackEditor attackEditor = new AttackEditor(true, currentAttack2);
            attackEditor.ShowDialog();
            attacks = ReadDatabase.getListOfAttacks();
            RefreshAttacks();
        }


        private void CardEditor_NewAttack_Button_Click(object sender, RoutedEventArgs e)
        {
            AttackEditor attackEditor = new AttackEditor(false, null);
            attackEditor.ShowDialog();
            attacks = ReadDatabase.getListOfAttacks();
            RefreshAttacks();
        }


        private void CardEditorTab_CreateCard_Button_Click(object sender, RoutedEventArgs e)
        {

            if (!CheckValidInput())
            {
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
                    updatedCard.ImagePath = CardEditor_Card_Preview.img.Source.ToString();
                    
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
                    Card newCard = new Card()
                    {
                        Name = CardEditor_Name_Textbox.Text,
                        CardTypeID = ((CardType)CardEditor_Type_Combobox.SelectedItem).ID,
                        HP = int.Parse(CardEditor_HP_Textbox.Text),
                        ImagePath = CardEditor_Card_Preview.img.Source.ToString(),
                        Attack1ID = ((Attack)CardEditor_Attack1_Combobox.SelectedItem).ID,
                        Attack2ID = ((Attack)CardEditor_Attack2_Combobox.SelectedItem).ID
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
                        Console.WriteLine(a.Name);
                    }
                }

                CardEditor_Attack1_Combobox.Items.Refresh();
                CardEditor_Attack2_Combobox.Items.Refresh();

            }
            else if (attacks.Count > 0)
            {
                CardEditor_Attack1_Combobox.ItemsSource = attacks;
                CardEditor_Attack2_Combobox.ItemsSource = attacks;

                CardEditor_Attack1_Combobox.SelectedIndex = attack1Index;
                CardEditor_Attack2_Combobox.SelectedIndex = attack2Index;

            }
            else
            {
                CardEditor_Attack1_Combobox.ItemsSource = attacks;
                CardEditor_Attack2_Combobox.ItemsSource = attacks;

                CardEditor_Attack1_Combobox.SelectedIndex = -1;
                CardEditor_Attack2_Combobox.SelectedIndex = -1;
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

    }
}
