using System;
using DAL.EF;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private DBContext db;
        private Repository repository;

        public EFUnitOfWork()
        {
            this.db = new DBContext();
        }

        public IRepository Repository
        {
            get
            {
                if (this.repository == null)
                {
                    this.repository = new Repository(this.db);
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