using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using testMVC.Models;

namespace testMVC.DataBase
{
    public class DBContext : DbContext
    {
        public DbSet<Categories> Categoies { get; set; }
        public DBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
        }
    }
}
