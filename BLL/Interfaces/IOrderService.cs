using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        public IEnumerable<ProductDTO> GetAllProductsInBasket(String userId);
        public int GetOrderTotalAmount(String userId);

        public Task<String> addProductInBasketAsync(int productId, String userId = null, String basketInCache = null);
    }
}
