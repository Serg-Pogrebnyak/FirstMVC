using System;
using System.Collections.Generic;
using DAL.Entities;
using DAL.Interfaces;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    class CategoriesRepository : IRepository<Categories>
    {

        private DBContext db;

        public CategoriesRepository(DBContext context)
        {
            db = context;
        }

        public IEnumerable<Categories> GetAll()
        {
            return db.Categories;
        }

        public Categories Get(int id)
        {
            return db.Categories.Find(id);
        }

        public void Create(Categories item)
        {
            db.Categories.Add(item);
        }

        public void Update(Categories item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Categories> Find(Func<Categories, bool> predicate)
        {
            return db.Categories.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Categories category = db.Categories.Find(id);
            if (category != null)
                db.Categories.Remove(category);
        }
    }
}
