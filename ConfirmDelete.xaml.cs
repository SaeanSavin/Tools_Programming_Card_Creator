using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// <summary>
    /// Interaction logic for ConfirmDelete.xaml
    /// </summary>
    public partial class ConfirmDelete : Window
    {

        string typeName;

        public ConfirmDelete(string typeName)
        {
            InitializeComponent();

            this.typeName = typeName;
            ConfirmDelete_TextBox.Text = "Are you sure you want to delete '" + typeName + "' This will also delete all attacks of this type!";
        }

        private void ConfirmDelete_Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void ConfirmDelete_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
