using System;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository Repository { get; }

        void Save();
    }
}