using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private DBContext db;

        public ProductRepository(DBContext context)
        {
            this.db = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return this.db.Products;
        }

        public Product Get(int id)
        {
            return this.db.Products.Find(id);
        }

        public void Create(Product item)
        {
            this.db.Products.Add(item);
        }

        public void Update(Product item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Product> Find(Func<Product, bool> predicate)
        {
            return this.db.Products.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Product product = this.db.Products.Find(id);
            if (product != null)
            {
                this.db.Products.Remove(product);
            }
        }
    }
}