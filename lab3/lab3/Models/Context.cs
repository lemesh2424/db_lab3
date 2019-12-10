using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace lab3.Models
{
    class Context : DbContext
    {

        public Context()
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<UserChat> UserChat { get; set; }
        public virtual DbSet<Message> Message { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            using var sr = new StreamReader("../../../DataBaseSettings.txt");
            var connectionString = sr.ReadToEnd();
            optionsBuilder.UseNpgsql(connectionString);

        }
    }
}
