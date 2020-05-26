using System;
using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private DBContext db;
        private BasketRepository basketRepository;
        private CategoriesRepository categoriesRepository;
        private ProductRepository productRepository;

        public EFUnitOfWork()
        {
            this.db = new DBContext();
        }

        public IRepositoryBasket<Basket> Basket
        {
            get
            {
                if (this.basketRepository == null)
                {
                    this.basketRepository = new BasketRepository(this.db);
                }

                return this.basketRepository;
            }
        }

        public IRepository<Categories> Categories
        {
            get
            {
                if (this.categoriesRepository == null)
                {
                    this.categoriesRepository = new CategoriesRepository(this.db);
                }

                return this.categoriesRepository;
            }
        }

        public IRepository<Product> Product
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new ProductRepository(this.db);
                }

                return this.productRepository;
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