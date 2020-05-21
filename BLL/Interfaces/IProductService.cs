using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        public IEnumerable<ProductDTO> getAllProduct();
        public void createNewProduct(ProductDTO newProduct, String name);
    }
}
