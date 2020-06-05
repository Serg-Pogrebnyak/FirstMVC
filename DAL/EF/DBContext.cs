using System.Collections.Generic;
using DAL.Entities;
using DAL.InitialData;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductInBasket>().HasOne(p => p.Basket).WithMany(t => t.Products).OnDelete(DeleteBehavior.Cascade);
            // set relationship
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(p => p.Products).HasForeignKey(p => p.CategoriesId);
            // get defaultdata from initial class
            var phoneTuple = DBInitialData.GetPhoneProductsWithCategory();
            var tabletTuple = DBInitialData.GetTabletProductsWithCategory();
            var monitorTuple = DBInitialData.GetMonitorProductsWithCategory();
            // set default data to DB
            modelBuilder.Entity<Categories>().HasData(phoneTuple.Item2, tabletTuple.Item2, monitorTuple.Item2);
            List<Product> listOfProduct = new List<Product> { };
            listOfProduct.AddRange(phoneTuple.Item1);
            listOfProduct.AddRange(tabletTuple.Item1);
            listOfProduct.AddRange(monitorTuple.Item1);
            _ = modelBuilder.Entity<Product>().HasData(listOfProduct);
        }
    }
}