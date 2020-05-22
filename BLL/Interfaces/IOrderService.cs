using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<ProductDTO>> GetAllProductsInBasket(String userId = null, String basketInCache = null);
        public Task<int> GetOrderTotalAmount(String userId = null, String basketInCache = null);

        public Task<String> addProductInBasketAsync(int productId, String userId = null, String basketInCache = null);
    }
}
