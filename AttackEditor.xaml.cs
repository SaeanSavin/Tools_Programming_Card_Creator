using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

    public partial class AttackEditor : Window
    {
        List<CardType> types;
        private Attack currentAttack;
        private CardType currentType;
        private bool editAttack = false;

        public AttackEditor(bool editMode, Attack attack)
        {
            InitializeComponent();

            types = ReadDatabase.getListOfCardTypes();
            RefreshTypes();

            if (editMode)
            {
                editAttack = true;
                AttackEditor_Window.Title = "AttackEditor - Edit mode";
                AttackEditor_CreateAttack_Button.Content = "Save";
                AttackEditor_Delete_Button.Visibility = Visibility.Visible;
            }

            if(attack != null)
            {
                currentAttack = attack;
                AttackEditor_Name_Textbox.Text = attack.Name;

                foreach(var t in types)
                {
                    if(attack.CardTypeID == t.ID)
                    {
                        currentType = t;
                        break;
                    }
                }

                AttackEditor_Type_Combobox.SelectedItem = currentType;
                AttackEditor_Damage_Textbox.Text = attack.Damage.ToString();
            }
        }

        private void AttackEditor_Type_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentType = ((CardType)AttackEditor_Type_Combobox.SelectedItem);

            AttackEditor_Damage_Label.Content = ("Damage: " + "( " + currentType.MinAttackDMG + " - " + currentType.MaxAttackDMG + " )" );

        }


        private void RefreshTypes()
        {
            if(types.Count > 0)
            {
                AttackEditor_Type_Combobox.ItemsSource = types;
                
                if(currentType != null)
                {
                    AttackEditor_Type_Combobox.SelectedItem = currentType.Name;
                }
                //AttackEditor_Type_Combobox.SelectedIndex = 0;
            }
        }

        private void AttackEditor_CreateAttack_Button_Click(object sender, RoutedEventArgs e)
        {

            if (!CheckValidInput())
            {
                MessageBox.Show("Some values are not valid!", "Invalid Input!");
                return;
            }

            using (CardContext context = new CardContext())
            {
                if(editAttack)
                {
                    Attack updatedAttack = context.Attacks.Find(currentAttack.ID);

                    updatedAttack.Name = AttackEditor_Name_Textbox.Text;
                    updatedAttack.CardTypeID = ((CardType)AttackEditor_Type_Combobox.SelectedItem).ID;
                    updatedAttack.Damage = int.Parse(AttackEditor_Damage_Textbox.Text);

                    context.SaveChanges();
                } 
                else
                {
                    Attack newAttack = new Attack()
                    {
                        Name = AttackEditor_Name_Textbox.Text,
                        CardTypeID = ((CardType)AttackEditor_Type_Combobox.SelectedItem).ID,
                        Damage = int.Parse(AttackEditor_Damage_Textbox.Text)
                    };

                    context.Attacks.Add(newAttack);
                    context.SaveChanges();
                }
            }
            Close();
        }

        private void AttackEditor_Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AttackEditor_HP_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }


        private bool CheckValidInput()
        {
            bool isValid = true;

            if(AttackEditor_Name_Textbox.Text == "")
            {
                AttackEditor_Error_Name.Content = "Invalid input!";
                isValid = false;
            }
            else
            {
                AttackEditor_Error_Name.Content = "";
            }

            if(AttackEditor_Type_Combobox.SelectedIndex == -1)
            {
                AttackEditor_Error_Type.Content = "Invalid Input!";
                isValid = false;
            }
            else
            {
                AttackEditor_Error_Type.Content = "";
            }

            if(currentType != null)
            {
                if (AttackEditor_Damage_Textbox.Text == "" || (int.Parse(AttackEditor_Damage_Textbox.Text) < currentType.MinAttackDMG || int.Parse(AttackEditor_Damage_Textbox.Text) > currentType.MaxAttackDMG))
                {
                    AttackEditor_Error_Damage.Content = "Invalid input!";
                    isValid = false;
                }
                else
                {
                    AttackEditor_Error_Name.Content = "";
                }
            }
            return isValid;
        }

        private void AttackEditor_Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            using (CardContext context = new CardContext())
            {
                context.Attacks.Remove(context.Attacks.Find(currentAttack.ID));
                context.SaveChanges();
            }
            Close();
        }
    }
}
