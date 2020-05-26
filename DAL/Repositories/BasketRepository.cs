using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BasketRepository : IRepositoryBasket<Basket>
    {
        private DBContext db;

        public BasketRepository(DBContext context)
        {
            this.db = context;
        }

        public IEnumerable<Basket> GetAll()
        {
            return this.db.Baskets;
        }

        public Basket Get(int id)
        {
            return this.db.Baskets.Find(id);
        }

        public Basket GetByUserId(string id)
        {
            return this.db.Baskets.SingleOrDefault(basket => basket.UserId == id);
        }

        public void Create(Basket item)
        {
            this.db.Baskets.Add(item);
        }

        public void Update(Basket item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Basket> Find(Func<Basket, bool> predicate)
        {
            return this.db.Baskets.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Basket basket = this.db.Baskets.Find(id);
            if (basket != null)
            {
                _ = this.db.Baskets.Remove(basket);
            }
        }
    }
}