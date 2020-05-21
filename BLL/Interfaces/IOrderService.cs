using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        public IEnumerable<ProductDTO> GetAllProductsInBasket(String userId);
        public int GetOrderTotalAmount(String userId);

        public void addProductInBasket(String userId, int productId);
    }
}
