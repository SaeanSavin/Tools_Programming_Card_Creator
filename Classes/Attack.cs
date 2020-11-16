
namespace Card_Creator.Classes
{
    public class Attack
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Damage { get; set; }

        public int CardTypeID { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}