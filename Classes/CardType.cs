
namespace Card_Creator.Classes
{
    public class CardType
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Cardcolor { get; set; }

        public int MinHP { get; set; }

        public int MaxHP { get; set; }

        public int MinAttackDMG { get; set; }

        public int MaxAttackDMG { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}