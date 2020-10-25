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

    public partial class Create_Card_Window : Window
    {
        public Create_Card_Window()
        {
            InitializeComponent();
        }

        private void Add_type_button_Click(object sender, RoutedEventArgs e)
        {
            Add_Type_Window add_Type_Window = new Add_Type_Window();
            add_Type_Window.ShowDialog();
        }
    }
}
