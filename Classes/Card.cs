﻿using SQLite;

namespace Card_Creator.Classes
{
    
    public class Card
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string name { get; set; }

        public Type type { get; set; }

    }
}
