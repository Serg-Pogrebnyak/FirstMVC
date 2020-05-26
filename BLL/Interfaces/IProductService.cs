using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        public IEnumerable<ProductDTO> GetAllProduct();

        public void CreateNewProduct(ProductDTO newProduct, string name);
    }
}