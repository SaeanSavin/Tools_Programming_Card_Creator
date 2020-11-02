using Card_Creator.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Card_Creator.UC
{
    public partial class Card_UC : UserControl
    {

        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        public static readonly DependencyProperty CardProperty =
            DependencyProperty.Register("Card", typeof(Card), typeof(Card_UC), new PropertyMetadata(null, SetData));


        public static void SetData(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Card_UC cardControl = d as Card_UC;

            if(cardControl != null)
            {
                //TODO
                cardControl.name.Content = (e.NewValue as Card).Name;
                cardControl.hp.Content = (e.NewValue as Card).HP;
                
                //Convert from string to Source
                ImageSourceConverter converter = new ImageSourceConverter();
                

            }

        }

        public Card_UC()
        {
            InitializeComponent();
        }
    }
}
