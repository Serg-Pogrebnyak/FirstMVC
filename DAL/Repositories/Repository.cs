using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DBContext db;

        public Repository(DBContext context)
        {
            this.db = context;
        }

        public void Create(T item)
        {
            this.db.Set<T>().Add(item);
        }

        public void Delete(int id)
        {
            T element = this.db.Set<T>().Find(id);
            if (element != null)
            {
                _ = this.db.Set<T>().Remove(element);
            }
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return this.db.Set<T>().Where(predicate);
        }

        public T Get(int id)
        {
            return this.db.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return this.db.Set<T>();
        }

        public void Update(T item)
        {
            this.db.Entry(item).State = EntityState.Modified;
        }
    }
}