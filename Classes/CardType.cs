using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SQLite;

namespace Card_Creator.Classes
{
    public class CardType
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

       // public Color Color { get; set; }

        public string Color { get; set; }

        //public CardType Weakness { get; set; }
    }
}
