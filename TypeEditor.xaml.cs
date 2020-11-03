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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e){}

        private void Type_Save_Button_Click(object sender, RoutedEventArgs e)
        {
           using(CardContext context = new CardContext())
           {
                if(editType)
                {

                    CardType updatedType = context.CardTypes.Find(currentType.ID);

                    updatedType.Name = TypeEditor_Name_Textbox.Text;
                    updatedType.Cardcolor = (TypeEditor_Color_Combobox.SelectedItem as PropertyInfo).Name;

                    context.SaveChanges();
                } else {
                    CardType newType = new CardType()
                    {
                        Name = TypeEditor_Name_Textbox.Text,
                        Cardcolor = (TypeEditor_Color_Combobox.SelectedItem as PropertyInfo).Name
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
            using(CardContext context = new CardContext())
            {
                context.CardTypes.Remove(context.CardTypes.Find(currentType.ID));
                context.SaveChanges();
            }
            Close();
        }
    }
}
