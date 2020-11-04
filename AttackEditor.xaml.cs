using Card_Creator.Classes;
using Card_Creator.Classes.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public partial class AttackEditor : Window
    {
        List<CardType> types;

        public AttackEditor()
        {
            InitializeComponent();

            types = ReadDatabase.getListOfCardTypes();
            RefreshComboBox();

        }

        private void AttackEditor_Type_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void RefreshComboBox()
        {
            if(types.Count > 0)
            {
                AttackEditor_Type_Combobox.ItemsSource = types;
                AttackEditor_Type_Combobox.SelectedIndex = 0;
            }
        }

        private void AttackEditor_CreateAttack_Button_Click(object sender, RoutedEventArgs e)
        {
            using(CardContext context = new CardContext())
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
            Close();
        }
    }
}
