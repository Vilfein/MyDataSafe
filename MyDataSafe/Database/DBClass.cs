using Microsoft.EntityFrameworkCore;
using MyDataSafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataSafe.Database
{
    public class Context : DbContext
    {
        public DbSet<DataClass> Datas { get; set; }
        public DbSet<DataColor> Colors { get; set; }
        public Context() : base()
        { 
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source = SafeDatas.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DataColor>().HasData(

                new DataColor { Id = 1, Name = "Red", Value = "#FF0000" },
                new DataColor { Id = 2, Name = "Green", Value = "#00FF00" },
                new DataColor { Id = 3, Name = "Blue", Value = "#0000FF" },
                new DataColor { Id = 4, Name = "Purple", Value = "#8A1AC2" },
                new DataColor { Id = 5, Name = "Pink", Value = "#FF009B" },
                new DataColor { Id = 6, Name = "Violet", Value = "#7f00ff" },
                new DataColor { Id = 7, Name = "Black", Value = "#000000" },
                new DataColor { Id = 8, Name = "Yellow", Value = "#FCFC02" },
                new DataColor { Id = 9, Name = "Azure", Value = "#02FCFC" }
                );
        }
    }
}
