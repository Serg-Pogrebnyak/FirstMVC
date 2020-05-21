using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;
using System.Linq;

namespace BLL.BusinessLogic
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _dataLayer;
        public CategoryService(IUnitOfWork dataLayer)
        {
            _dataLayer = dataLayer;
        }

        public void createNewCategory(string name)
        {
            Categories category = new Categories
            {
                Name = name
            };
            _dataLayer.Categories.Create(category);
            _dataLayer.Save();
        }

        public IEnumerable<CategoriesDTO> getAllCategory()
        {
            return categoriesMapper(_dataLayer.Categories.GetAll());
        }

        public IEnumerable<ProductDTO> getAllProductInCategory(int id)
        {
            Categories category = _dataLayer.Categories.Get(id);
            return productMapper(category);
        }

        public bool containCategoryWithName(String name)
        {
            Categories category = _dataLayer.Categories.GetAll().SingleOrDefault(cat => cat.Name == name);
            return category != null;
        }

        private IEnumerable<CategoriesDTO> categoriesMapper(IEnumerable<Categories> categories)
        {
            List<CategoriesDTO> categoriesDTO = new List<CategoriesDTO> { };
            foreach (Categories category in categories)
            {
                CategoriesDTO categoryDTO = new CategoriesDTO
                {
                    Id = category.Id,
                    Name = category.Name
                };
                categoriesDTO.Add(categoryDTO);
            }
            return categoriesDTO;
        }

        private IEnumerable<ProductDTO> productMapper(Categories categories)
        {
            List<ProductDTO> productDTOList = new List<ProductDTO> { };
            foreach (Product product in categories.Products)
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
