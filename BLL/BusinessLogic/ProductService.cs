using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System.Linq;

namespace BLL.BusinessLogic
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _db;
        public ProductService(IUnitOfWork db)
        {
            _db = db;
        }
        public void createNewProduct(ProductDTO newProduct, String name)
        {
            Categories category = _db.Categories.GetAll().SingleOrDefault(cat => cat.Name == name);
            Product product = new Product
            {
                Name = newProduct.Name,
                Price = newProduct.Price,
                Description = newProduct.Description,
                Category = category
            };
            _db.Product.Create(product);
            _db.Save();
        }

        public IEnumerable<ProductDTO> getAllProduct()
        {
            List<ProductDTO> productDTOList = new List<ProductDTO> { };
            foreach (Product product in _db.Product.GetAll())
            {
                ProductDTO productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description
                };
                productDTOList.Add(productDTO);
            }
            return productDTOList;
        }
    }
}
