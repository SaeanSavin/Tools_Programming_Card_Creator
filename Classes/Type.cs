using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SQLite;

namespace Card_Creator.Classes
{
    public class Type
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        public Color Color { get; set; }

        public Type Weakness { get; set; }

        public Type Strength { get; set; }
    }
}
