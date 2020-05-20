using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.EF
{
    class DBContext : DbContext
    {
        internal DbSet<Categories> Categories { get; set; }
        internal DbSet<Product> Products { get; set; }
        internal DbSet<Basket> Baskets { get; set; }
        internal DBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
        }
    }
}
