using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card_Creator.Classes.Db
{
    public class CardContext : DbContext
    {

        private object _lock = new object();

        public DbSet<Card> Cards { get; set; }

        public DbSet<CardType> CardTypes { get; set; }

        public DbSet<Attack> Attacks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server = (localdb)\MSSQLLocalDB; " +
                @"Database = CardCreator; " +
                @"Trusted_Connection = True; "
                );
        }
    }
}