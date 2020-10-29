using System;
using System.Collections.Generic;
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
        public Add_Type_Window()
        {
            InitializeComponent();
            add_type_color_combobox.ItemsSource = typeof(Colors).GetProperties();
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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e){
            Color selectedColor = (Color)(add_type_color_combobox.SelectedItem as PropertyInfo).GetValue(null, null);
            this.Background = new SolidColorBrush(selectedColor);
        }

        private void Type_Add_Button_Click(object sender, RoutedEventArgs e)
        {
           using(CardContext context = new CardContext())
           {
                CardType newType = new CardType()
                {
                    Name = add_type_name_textbox.Text,
                    Cardcolor = (add_type_color_combobox.SelectedItem as PropertyInfo).Name
                };

                context.CardTypes.Add(newType);
                context.SaveChanges();
           }

           Close();

        }
    }
}
