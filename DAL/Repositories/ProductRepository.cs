using System;
using System.Collections.Generic;
using DAL.Entities;
using DAL.Interfaces;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    class ProductRepository : IRepository<Product>
    {

        private DBContext db;

        public ProductRepository(DBContext context)
        {
            db = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Products;
        }

        public Product Get(int id)
        {
            return db.Products.Find(id);
        }

        public void Create(Product item)
        {
            db.Products.Add(item);
        }

        public void Update(Product item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Product> Find(Func<Product, bool> predicate)
        {
            return db.Products.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product != null)
                db.Products.Remove(product);
        }
    }
}
