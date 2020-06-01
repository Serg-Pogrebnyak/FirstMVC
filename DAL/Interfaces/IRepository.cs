using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IRepository
    {
        IEnumerable<T> GetAll<T>() where T : class;

        IEnumerable<T> GetForPage<T>(int page, int countPerPage) where T : class;

        int GetCount<T>() where T : class;

        T Get<T>(int id) where T : class;

        IEnumerable<T> Find<T>(Func<T, bool> predicate) where T : class;

        void Create<T>(T item) where T : class;

        void Update<T>(T item) where T : class;

        public void Delete<T>(int id) where T : class;
    }
}