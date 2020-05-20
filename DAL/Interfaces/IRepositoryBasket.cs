using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IRepositoryBasket<T> : IRepository<T> where T : class
    {
        T GetByUserId(String id);
    }
}
