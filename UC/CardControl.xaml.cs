using Card_Creator.Classes;
using Card_Creator.Classes.Db;
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
    public partial class CardControl : UserControl
    {

        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        public static readonly DependencyProperty CardProperty =
            DependencyProperty.Register("Card", typeof(Card), typeof(CardControl), new PropertyMetadata(null, SetData));


        public static void SetData(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CardControl cardControl = d as CardControl;

            if(cardControl != null)
            {
                cardControl.name.Content = (e.NewValue as Card).Name;
                cardControl.hp.Content = "HP: " + (e.NewValue as Card).HP;
                
                using(CardContext context = new CardContext())
                {
                    cardControl.type.Content = "Type: " + context.CardTypes.Find((e.NewValue as Card).CardTypeID).Name;
                    string color = context.CardTypes.Find((e.NewValue as Card).CardTypeID).Cardcolor;
                    cardControl.borderColor.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                }

                //Convert from string to Source
                ImageSourceConverter converter = new ImageSourceConverter();
                cardControl.img.Source = (ImageSource)converter.ConvertFromString((e.NewValue as Card).ImagePath);
            }

        }

        public CardControl()
        {
            InitializeComponent();
        }
    }
}
