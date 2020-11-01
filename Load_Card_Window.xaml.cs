using Card_Creator.Classes;
using Card_Creator.Classes.Db;
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
    /// Interaction logic for Load_Card_Window.xaml
    /// </summary>
    public partial class Load_Card_Window : Window
    {
        Card currentCard;
        List<Card> cards;

        public Load_Card_Window()
        {
            InitializeComponent();

            ReadDatabase();

            card_select_comboBox.ItemsSource = cards;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Create_Card_Window create_Card_Window = new Create_Card_Window(true, currentCard);
            create_Card_Window.ShowDialog();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentCard = (Card)card_select_comboBox.SelectedItem;

            if (card_select_comboBox.SelectedIndex >= 0)
            {
                load_card_button.IsEnabled = true;
            }
        }

        void ReadDatabase()
        {
            using (CardContext context = new CardContext())
            {
                cards = context.Cards.ToList();

                if (cards.Count > 0)
                {
                    card_select_comboBox.ItemsSource = cards;

                    card_select_comboBox.SelectedIndex = 0;

                    currentCard = (Card)card_select_comboBox.SelectedItem;

                    if (currentCard != null)
                    {
                        card_select_comboBox.SelectedIndex = currentCard.ID;
                    }
                }
                else
                {
                    card_select_comboBox.SelectedIndex = -1;
                    card_select_comboBox.ItemsSource = null;
                    card_select_comboBox.IsEnabled = false;
                }
            }
        }
    }
}
