using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class DBContext : DbContext
    {
        internal DbSet<Categories> Categories { get; set; }

        internal DbSet<Product> Products { get; set; }

        internal DbSet<Basket> Baskets { get; set; }

        internal DBContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
        }
    }
}