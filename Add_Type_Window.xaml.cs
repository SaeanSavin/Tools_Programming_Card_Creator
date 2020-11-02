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

    public partial class Add_Type_Window : Window
    {
        CardType currentType;
        bool editType = false;

        public Add_Type_Window(bool editMode, CardType type)
        {
            InitializeComponent();
            UpdateSettings.UpdateDarkMode(this);

            type_color_combobox.ItemsSource = typeof(Colors).GetProperties();

            if(editMode)
            {
                editType = true;
                Type_Window.Title = "Edit type";
                Type_Save_Button.Content = "Save";
                delete_type_button.Visibility = Visibility.Visible;
            }

            if(type != null)
            {
                currentType = type;
                type_name_textbox.Text = type.Name;
                
                foreach(var c in type_color_combobox.ItemsSource)
                {
                    if((c as PropertyInfo).Name == type.Cardcolor)
                    {
                        type_color_combobox.SelectedItem = c;
                        break;
                    }
                }
            }
        }

        private void Type_min_stat_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void Type_max_stat_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

                    updatedType.Name = type_name_textbox.Text;
                    updatedType.Cardcolor = (type_color_combobox.SelectedItem as PropertyInfo).Name;

                    context.SaveChanges();
                } else {
                    CardType newType = new CardType()
                    {
                        Name = type_name_textbox.Text,
                        Cardcolor = (type_color_combobox.SelectedItem as PropertyInfo).Name
                    };

                    context.CardTypes.Add(newType);
                    context.SaveChanges();
                }
           }
           Close();
        }

        private void Cancel_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Delete_type_button_Click(object sender, RoutedEventArgs e)
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
