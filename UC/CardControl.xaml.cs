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
                cardControl.Name.Content = (e.NewValue as Card).Name;
                cardControl.HP.Content = (e.NewValue as Card).HP + " HP";
                
                using(CardContext context = new CardContext())
                {

                    if(context.CardTypes.Find((e.NewValue as Card).CardTypeID) != null)
                    {
                        cardControl.Type.Content = context.CardTypes.Find((e.NewValue as Card).CardTypeID).Name;
                        string color = context.CardTypes.Find((e.NewValue as Card).CardTypeID).Cardcolor;
                        cardControl.BorderColor.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                    }
                    else
                    {
                        cardControl.Type.Content = "No Type";
                        cardControl.UC_IsValid_Label.Content = "Not Valid for play";
                    }
                

                    if(context.Attacks.Find((e.NewValue as Card).Attack1ID) != null)
                    {
                        cardControl.Attack1.Content = context.Attacks.Find((e.NewValue as Card).Attack1ID).Name;
                        cardControl.Attack1_Damage.Content = context.Attacks.Find((e.NewValue as Card).Attack1ID).Damage;
                    }
                    else
                    {
                        cardControl.Attack1.Content = "";
                        cardControl.Attack1_Damage.Content = "";
                    }

                    if(context.Attacks.Find((e.NewValue as Card).Attack2ID) != null)
                    {
                        cardControl.Attack2.Content = context.Attacks.Find((e.NewValue as Card).Attack2ID).Name;
                        cardControl.Attack2_Damage.Content = context.Attacks.Find((e.NewValue as Card).Attack2ID).Damage;
                    }
                    else
                    {
                        cardControl.Attack2.Content = "";
                        cardControl.Attack2_Damage.Content = "";
                    }


                    if(context.Attacks.Find((e.NewValue as Card).Attack1ID) == null && context.Attacks.Find((e.NewValue as Card).Attack2ID) == null)
                    {
                        cardControl.UC_IsValid_Label.Content = "Not Valid for play";
                        cardControl.UC_IsValid_Label.Foreground = Brushes.Red;
                    }
                }

                ImageSourceConverter converter = new ImageSourceConverter();
                cardControl.Image.Source = (ImageSource)converter.ConvertFromString((e.NewValue as Card).ImagePath);
            }

        }

        public CardControl()
        {
            InitializeComponent();
        }
    }
}
