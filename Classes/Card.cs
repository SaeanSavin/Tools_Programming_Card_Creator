using SQLite;
using System.Runtime.InteropServices;

namespace Card_Creator.Classes
{
    
    public class Card
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string name { get; set; }

    }
}
