using System.Collections.Generic;
using System.Linq;
using BLL.DTO;
using BLL.Extensions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.BusinessLogic
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork db;

        public ProductService(IUnitOfWork db)
        {
            this.db = db;
        }

        public void CreateNewProduct(ProductDTO newProduct, string name)
        {
            Categories category = this.db.Repository.GetAll<Categories>().SingleOrDefault(cat => cat.Name == name);
            Product product = new Product
            {
                Name = newProduct.Name,
                Price = newProduct.Price,
                Description = newProduct.Description,
                Category = category,
                LongDescription = newProduct.LongDescription,
                ImageInByte = newProduct.ImageInByte.ResizeImageFromByte()
            };
            this.db.Repository.Create(product);
            this.db.Save();
        }

        public void UpdateProduct(ProductDTO productDTO, string categoryName)
        {
            Categories category = this.db.Repository.GetAll<Categories>().SingleOrDefault(cat => cat.Name == categoryName);
            Product product = this.db.Repository.Get<Product>(productDTO.Id);
            product.Name = productDTO.Name;
            product.Price = productDTO.Price;
            product.Description = productDTO.Description;
            product.Category = category;
            product.LongDescription = productDTO.LongDescription;
            if (productDTO.ImageInByte != null)
            {
                product.ImageInByte = productDTO.ImageInByte.ResizeImageFromByte();
            }

            this.db.Repository.Update(product);
            this.db.Save();
        }

        public IEnumerable<ProductDTO> GetAllProduct()
        {
            List<ProductDTO> productDTOList = new List<ProductDTO> { };
            foreach (Product product in this.db.Repository.GetAll<Product>())
            {
                ProductDTO productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    ImageInByte = product.ImageInByte,
                    LongDescription = product.LongDescription
                };
                productDTOList.Add(productDTO);
            }

            return productDTOList;
        }

        public ProductDTO GetProductById(int id)
        {
            Product product = this.db.Repository.Get<Product>(id);
            ProductDTO productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageInByte = product.ImageInByte,
                LongDescription = product.LongDescription
            };

            return productDTO;
        }
    }
}