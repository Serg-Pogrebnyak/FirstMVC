using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class Repository : IRepository
    {
        private readonly DBContext db;

        public Repository(DBContext context)
        {
            this.db = context;
        }

        public void Create<T>(T item) where T : class
        {
            this.db.Set<T>().Add(item);
        }

        public void Delete<T>(int id) where T : class
        {
            T element = this.db.Set<T>().Find(id);
            if (element != null)
            {
                _ = this.db.Set<T>().Remove(element);
            }
        }

        public IEnumerable<T> Find<T>(Func<T, bool> predicate) where T : class
        {
            return this.db.Set<T>().Where(predicate);
        }

        public T Get<T>(int id) where T : class
        {
            return this.db.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return this.db.Set<T>();
        }

        public void Update<T>(T item) where T : class
        {
            this.db.Entry(item).State = EntityState.Modified;
        }
    }
}