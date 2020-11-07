using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using Card_Creator.Classes;
using Card_Creator.Classes.Db;


namespace Card_Creator
{

    public partial class TypeEditor : Window
    {
        private readonly CardType currentType;
        private readonly bool editType = false;

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

                TypeEditor_Min_HP_textbox.Text = type.MinHP.ToString();
                TypeEditor_Max_HP_textbox.Text = type.MaxHP.ToString();
                TypeEditor_MinAttackDMG_Textbox.Text = type.MinAttackDMG.ToString();
                TypeEditor_MaxAttackDMG_Textbox.Text = type.MaxAttackDMG.ToString();

            }
        }

        private void TypeEditor_min_stat_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void TypeEditor_max_stat_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e){}

        private void Type_Save_Button_Click(object sender, RoutedEventArgs e)
        {

            bool InputValidation = CheckValidInput();

            if(InputValidation)
            {
                using (CardContext context = new CardContext())
                {
                    if (editType)
                    {

                        CardType updatedType = context.CardTypes.Find(currentType.ID);

                        updatedType.Name = TypeEditor_Name_Textbox.Text;
                        updatedType.Cardcolor = (TypeEditor_Color_Combobox.SelectedItem as PropertyInfo).Name;
                        updatedType.MinHP = int.Parse(TypeEditor_Min_HP_textbox.Text);
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
                            MinHP = int.Parse(TypeEditor_Min_HP_textbox.Text),
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
            return;
            
        }

        private void TypeEditor_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TypeEditor_Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            using(CardContext context = new CardContext())
            {
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
            if (TypeEditor_Min_HP_textbox.Text == "")
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
