using System;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Basket> Basket { get; }
        IRepository<Categories> Categories { get; }
        IRepository<Product> Product { get; }
        void Save();
    }
}
