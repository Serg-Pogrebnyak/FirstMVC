using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        public IEnumerable<ProductDTO> GetAllProductsInBasket(String userId = null, String basketInCache = null);
        public int GetOrderTotalAmount(String userId = null, String basketInCache = null);

        public String addProductInBasket(int productId, String userId = null, String basketInCache = null);
    }
}
