using System;
using DAL.EF;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class EFUnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private DBContext db;
        private Repository<T> repository;

        public EFUnitOfWork()
        {
            this.db = new DBContext();
        }

        public IRepository<T> Repository
        {
            get
            {
                if (this.repository == null)
                {
                    this.repository = new Repository<T>(this.db);
                }

                return this.repository;
            }
        }

        public void Save()
        {
            this.db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.db.Dispose();
                }

                this.disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}