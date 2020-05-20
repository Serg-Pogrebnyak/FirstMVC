using System;
using System.Collections.Generic;
using DAL.Entities;
using DAL.Interfaces;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Repositories
{
    class BasketRepository : IRepository<Basket>
    {
        
        private DBContext db;

        public BasketRepository(DBContext context)
        {
            db = context;
        }

        public IEnumerable<Basket> GetAll()
        {
            return db.Baskets;
        }

        public Basket Get(int id)
        {
            return db.Baskets.Find(id);
        }
        
        public void Create(Basket item)
        {
            db.Baskets.Add(item);
        }

        public void Update(Basket item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Basket> Find(Func<Basket, bool> predicate)
        {
            return db.Baskets.Where(predicate).ToList();
        }
        
        public void Delete(int id)
        {
            Basket basket = db.Baskets.Find(id);
            if (basket != null)
                db.Baskets.Remove(basket);
        }
    }
}
