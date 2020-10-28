using Card_Creator.Classes;
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

namespace Card_Creator
{
    public partial class Add_Type_Window : Window
    {
        List<CardType> types;

        public Add_Type_Window()
        {
            InitializeComponent();
            ReadDatabase();
            color_combobox.ItemsSource = typeof(Colors).GetProperties();
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
            Color selectedColor = (Color)(color_combobox.SelectedItem as PropertyInfo).GetValue(null, null);
            this.Background = new SolidColorBrush(selectedColor);
        }


        void ReadDatabase()
        {
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<CardType>();
                types = connection.Table<CardType>().ToList();

            }
        }

    }
}
