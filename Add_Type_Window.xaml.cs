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
using Card_Creator.Migrations;

namespace Card_Creator
{

    public partial class Add_Type_Window : Window
    {
        CardType currentType;

        public Add_Type_Window(bool editMode, CardType type)
        {
            InitializeComponent();
            UpdateSettings.UpdateDarkMode(this);

            type_color_combobox.ItemsSource = typeof(Colors).GetProperties();

            if(editMode)
            {
                Type_Window.Title = "Edit type";
                Type_Save_Button.Content = "Save";
                delete_type_button.Visibility = Visibility.Visible;
            }

            if(type != null)
            {
                currentType = type;
                type_name_textbox.Text = type.Name;
                type_color_combobox.SelectedItem = type.Cardcolor; 
            }
        }

        private void minStat_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void maxStat_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex reg = new Regex("[^0-9]+");
            e.Handled = reg.IsMatch(e.Text);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e){}

        private void Type_Save_Button_Click(object sender, RoutedEventArgs e)
        {
           using(CardContext context = new CardContext())
           {
                CardType newType = new CardType()
                {
                    Name = type_name_textbox.Text,
                    Cardcolor = (type_color_combobox.SelectedItem as PropertyInfo).Name
                };

                context.CardTypes.Add(newType);
                context.SaveChanges();
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
                //CardType tmpType = context.CardTypes.Where()
                context.CardTypes.Remove(context.CardTypes.Find(currentType.ID));
                context.SaveChanges();
            }
            Close();
        }
    }
}
