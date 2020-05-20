using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.EF
{
    public class DBContext : DbContext
    {
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
        }
    }
}
