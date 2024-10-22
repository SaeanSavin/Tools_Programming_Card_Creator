﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Card_Creator.Classes;
using Card_Creator.Classes.Db;


namespace Card_Creator
{

    public partial class TypeEditor : Window
    {
        private readonly CardType currentType;
        private readonly bool editType = false;

        List<Attack> attacks;

        public TypeEditor(bool editMode, CardType type)
        {
            InitializeComponent();
            UpdateSettings.UpdateDarkMode(this);

            TypeEditor_Color_Combobox.ItemsSource = typeof(Colors).GetProperties();

            if(editMode)
            {
                editType = true;
                TypeEditor_Window.Title = "TypeEditor - Edit mode";
                TypeEditor_Save_Button.Content = "Save";
                TypeEditor_Delete_Button.Visibility = Visibility.Visible;

                attacks = ReadDatabase.getListOfAttacks();
            }

            if(type != null)
            {
                currentType = type;
                TypeEditor_Name_Textbox.Text = type.Name;
                
                foreach(var c in TypeEditor_Color_Combobox.ItemsSource)
                {
                    if((c as PropertyInfo).Name == type.Cardcolor)
                    {
                        TypeEditor_Color_Combobox.SelectedItem = c;
                        break;
                    }
                }

                TypeEditor_MinHP_Textbox.Text = type.MinHP.ToString();
                TypeEditor_Max_HP_textbox.Text = type.MaxHP.ToString();
                TypeEditor_MinAttackDMG_Textbox.Text = type.MinAttackDMG.ToString();
                TypeEditor_MaxAttackDMG_Textbox.Text = type.MaxAttackDMG.ToString();

            }
        }


        private void TypeEditor_MinHP_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }


        private void TypeEditor_MaxHP_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }


        private void TypeEditor_MinAttackDMG_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }


        private void TypeEditor_MaxAttackDMG_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }
   

        private void TypeEditor_CreateType_Button_Click(object sender, RoutedEventArgs e)
        {

            if (!CheckValidInput())
            {
                MessageBox.Show("Some values are not valid!", "Invalid Input!");
                return;
            }

            using (CardContext context = new CardContext())
            {
                if (editType)
                {

                    CardType updatedType = context.CardTypes.Find(currentType.ID);

                    updatedType.Name = TypeEditor_Name_Textbox.Text;
                    updatedType.Cardcolor = (TypeEditor_Color_Combobox.SelectedItem as PropertyInfo).Name;
                    updatedType.MinHP = int.Parse(TypeEditor_MinHP_Textbox.Text);
                    updatedType.MaxHP = int.Parse(TypeEditor_Max_HP_textbox.Text);
                    updatedType.MinAttackDMG = int.Parse(TypeEditor_MinAttackDMG_Textbox.Text);
                    updatedType.MaxAttackDMG = int.Parse(TypeEditor_MaxAttackDMG_Textbox.Text);

                    context.SaveChanges();
                }
                else
                {
                    CardType newType = new CardType()
                    {
                        Name = TypeEditor_Name_Textbox.Text,
                        Cardcolor = (TypeEditor_Color_Combobox.SelectedItem as PropertyInfo).Name,
                        MinHP = int.Parse(TypeEditor_MinHP_Textbox.Text),
                        MaxHP = int.Parse(TypeEditor_Max_HP_textbox.Text),
                        MinAttackDMG = int.Parse(TypeEditor_MinAttackDMG_Textbox.Text),
                        MaxAttackDMG = int.Parse(TypeEditor_MaxAttackDMG_Textbox.Text)
                    };

                    context.CardTypes.Add(newType);
                    context.SaveChanges();
                }
            }
            Close();
        }


        private void TypeEditor_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void TypeEditor_Delete_Button_Click(object sender, RoutedEventArgs e)
        {

            ConfirmDelete confirmDelete = new ConfirmDelete(currentType.Name);
            confirmDelete.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            bool? confirmedDelete = confirmDelete.ShowDialog();
            if(confirmedDelete == false)
            {
                return;
            }


            using(CardContext context = new CardContext())
            {
                
                foreach(Attack attack in attacks)
                {
                    if(attack.CardTypeID == currentType.ID) {
                        context.Attacks.Remove(attack);
                    }
                }

                context.CardTypes.Remove(context.CardTypes.Find(currentType.ID));
                context.SaveChanges();
            }
            Close();
        }

 
        private bool CheckValidInput()
        {

            bool isValid = true;


            if(TypeEditor_Name_Textbox.Text == "")
            {
                TypeEditor_Error_Name_Label.Content = "Invalid Input!";
                isValid = false;
            }
            else
            {
                TypeEditor_Error_Name_Label.Content = "";
            }


            if(TypeEditor_Color_Combobox.SelectedItem == null)
            {
                TypeEditor_Error_Color_Label.Content = "Invalid Input!";
                isValid = false;
            }
            else
            {
                TypeEditor_Error_Color_Label.Content = "";
            }


            if (TypeEditor_MinHP_Textbox.Text == "")
            {
                TypeEditor_Error_MinHP_Label.Content = "Invalid Input!";
                isValid = false;
            } 
            else
            {
                TypeEditor_Error_MinHP_Label.Content = "";
            }


            if (TypeEditor_Max_HP_textbox.Text == "")
            {
                TypeEditor_Error_MaxHP_Label.Content = "Invalid Input!";
                isValid = false;   
            }
            else
            {
                TypeEditor_Error_MaxHP_Label.Content = "";
            }

            if(TypeEditor_MinHP_Textbox.Text != "" && TypeEditor_Max_HP_textbox.Text != "" )
            {
                if (int.Parse(TypeEditor_MinHP_Textbox.Text) > int.Parse(TypeEditor_Max_HP_textbox.Text))
                {
                    TypeEditor_Error_MinHP_Label.Content = "Min value cant be higher than Max";
                    isValid = false;
                }
            }

            if (TypeEditor_MinAttackDMG_Textbox.Text == "")
            {
                TypeEditor_Error_MinAttackDMG_Label.Content = "Invalid Input!";
                isValid = false;    
            }
            else
            {
                TypeEditor_Error_MinAttackDMG_Label.Content = "";
            }


            if (TypeEditor_MaxAttackDMG_Textbox.Text == "")
            {
                TypeEditor_Error_MaxAttackDMG_Label.Content = "Invalid Input!";
                isValid = false;
            }
            else
            {
                TypeEditor_Error_MaxAttackDMG_Label.Content = "";
            }


            return isValid;
        }
    }
}
