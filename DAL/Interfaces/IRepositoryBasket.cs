namespace DAL.Interfaces
{
    public interface IRepositoryBasket<T> : IRepository<T> where T : class
    {
        T GetByUserId(string id);
    }
}