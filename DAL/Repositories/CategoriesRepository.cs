using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CategoriesRepository : IRepository<Categories>
    {
        private DBContext db;

        public CategoriesRepository(DBContext context)
        {
            this.db = context;
        }

        public IEnumerable<Categories> GetAll()
        {
            return this.db.Categories;
        }

        public Categories Get(int id)
        {
            Categories category = this.db.Categories.Find(id);
            this.db.Entry(category).Collection(c => c.Products).Load();
            return category;
        }

        public void Create(Categories item)
        {
            this.db.Categories.Add(item);
        }

        public void Update(Categories item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Categories> Find(Func<Categories, bool> predicate)
        {
            return this.db.Categories.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Categories category = this.db.Categories.Find(id);
            if (category != null)
            {
                _ = this.db.Categories.Remove(category);
            }
        }
    }
}