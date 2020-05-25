﻿using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;
using System.Linq;
using static BLL.Interfaces.ICategoryService;

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

        public IEnumerable<ProductDTO> selectProduct(int id, int priceFrom, int priceTo, SortByEnum by)
        {
            Categories category = _dataLayer.Categories.Get(id);
            List<ProductDTO> productDTOList = productMapper(category).ToList();
            
            if (priceTo > priceFrom)
            {
                productDTOList = productDTOList.Where(productDTO => productDTO.Price >= priceFrom && productDTO.Price <= priceTo).ToList();
            }
            else
            {
                productDTOList = productDTOList.Where(productDTO => productDTO.Price >= priceFrom).ToList();
            };
            switch (by)
            {
                case SortByEnum.priceToUp:
                    return productDTOList.OrderBy(productDTO => productDTO.Price);
                case SortByEnum.priceDoDown:
                    return productDTOList.OrderByDescending(productDTO => productDTO.Price);
                case SortByEnum.byName:
                    return productDTOList.OrderBy(productDTO => productDTO.Name);
                case SortByEnum.byNameDescending:
                    return productDTOList.OrderByDescending(productDTO => productDTO.Name);
            }
            return productDTOList;
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
