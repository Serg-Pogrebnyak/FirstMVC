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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(p => p.Products).HasForeignKey(p => p.CategoriesId);
            //category
            Categories cat1 = new Categories { Id = 1, Name = "Телефон" };
            Categories cat2 = new Categories { Id = 2, Name = "Планшет" };
            Categories cat3 = new Categories { Id = 3, Name = "Монитор" };
            modelBuilder.Entity<Categories>().HasData(cat1, cat2, cat3);
            //product
            Product p1 = new Product
            {
                Id = 1,
                Name = "iPhone SE 2020",
                Price = 14228,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p2 = new Product
            {
                Id = 2,
                Name = "iPhone 11",
                Price = 19899,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p3 = new Product
            {
                Id = 3,
                Name = "iPhone 11 Pro",
                Price = 27699,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p4 = new Product
            {
                Id = 4,
                Name = "iPhone 11 Pro Max",
                Price = 30199,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p5 = new Product
            {
                Id = 5,
                Name = "iPhone 11 Pro Max (Open Box)",
                Price = 38527,
                Description = "512GB",
                CategoriesId = cat1.Id
            };
            Product p6 = new Product
            {
                Id = 6,
                Name = "iPhone XS Max",
                Price = 23399,
                Description = "256GB",
                CategoriesId = cat1.Id
            };
            Product p7 = new Product
            {
                Id = 7,
                Name = "iPhone XS",
                Price = 18232,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p8 = new Product
            {
                Id = 8,
                Name = "iPhone 11 Pro Dual Sim",
                Price = 28973,
                Description = "64",
                CategoriesId = cat1.Id
            };
            Product p9 = new Product
            {
                Id = 9,
                Name = "iPhone XS Max",
                Price = 21138,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p10 = new Product
            {
                Id = 10,
                Name = "iPhone XR",
                Price = 17666,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p11 = new Product
            {
                Id = 11,
                Name = "iPhone 8",
                Price = 12496,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p12 = new Product
            {
                Id = 12,
                Name = "iPhone XR Dual Sim",
                Price = 20390,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p13 = new Product
            {
                Id = 13,
                Name = "iPhone X",
                Price = 17841,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p14 = new Product
            {
                Id = 14,
                Name = "iPhone 8 Plus",
                Price = 15799,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p15 = new Product
            {
                Id = 15,
                Name = "iPhone 7 Plus",
                Price = 12499,
                Description = "32GB",
                CategoriesId = cat1.Id
            };
            Product p16 = new Product
            {
                Id = 16,
                Name = "iPhone 7",
                Price = 9248,
                Description = "32GB",
                CategoriesId = cat1.Id
            };
            Product p17 = new Product
            {
                Id = 17,
                Name = "Samsung A51",
                Price = 7087,
                Description = "64GB",
                CategoriesId = cat1.Id
            };
            Product p18 = new Product
            {
                Id = 18,
                Name = "Samsung S20",
                Price = 23550,
                Description = "128GB",
                CategoriesId = cat1.Id
            };
            Product p19 = new Product
            {
                Id = 19,
                Name = "Samsung S20+",
                Price = 28090,
                Description = "128GB",
                CategoriesId = cat1.Id
            };
            Product p20 = new Product
            {
                Id = 20,
                Name = "Samsung S20 Ultra",
                Price = 34708,
                Description = "128GB",
                CategoriesId = cat1.Id
            };

            modelBuilder.Entity<Product>().HasData(new Product[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20 });
        }
    }
}