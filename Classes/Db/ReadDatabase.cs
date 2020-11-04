using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card_Creator.Classes.Db
{
    public static class ReadDatabase
    {
        public static List<Card> getListOfCards()
        {
            using (CardContext context = new CardContext())
            {
                return context.Cards.ToList();
            }
        }

        public static List<CardType> getListOfCardTypes()
        {
            using (CardContext context = new CardContext())
            {
                return context.CardTypes.ToList();
            }
        }
    }
}
