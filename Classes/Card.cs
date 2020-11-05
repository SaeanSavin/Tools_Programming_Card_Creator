
namespace Card_Creator.Classes
{
    public class Card
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int HP { get; set; }

        public int CardTypeID { get; set; }

        public int Attack1ID { get; set; }

        public int Attack2ID { get; set; }

        public string ImagePath { get; set; }

        public override string ToString()
        {
            return Name;
        } 

    }
}
