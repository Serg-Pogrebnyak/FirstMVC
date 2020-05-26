using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        public IEnumerable<ProductDTO> GetAllProductsInBasket(string userId = null, string basketInCache = null);

        public int GetOrderTotalAmount(string userId = null, string basketInCache = null);

        public string AddProductInBasket(int productId, string userId = null, string basketInCache = null);

        public string DeleteProductFromBasket(int productId, string userId = null, string basketInCache = null);

        public void MigrateBasketFromCookie(string userId, string basketInCache);
    }
}