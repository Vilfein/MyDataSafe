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
        public Context() : base()
        { 
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source = SafeDatas.db");
        }
    }
}
