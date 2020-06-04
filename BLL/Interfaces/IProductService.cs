using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        public IEnumerable<ProductDTO> GetAllProduct();

        public ProductDTO GetProductById(int id);

        public void CreateNewProduct(ProductDTO newProduct, string name);

        public void UpdateProduct(ProductDTO productDTO, string categoryName);
    }
}