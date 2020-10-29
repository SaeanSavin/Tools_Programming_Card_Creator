using SQLite;

namespace Card_Creator.Classes
{
    
    public class Card
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        public CardType CardType { get; set; }

    }
}
