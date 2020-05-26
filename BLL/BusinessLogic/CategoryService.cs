﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using static BLL.Interfaces.ICategoryService;

namespace BLL.BusinessLogic
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork dataLayer;

        public CategoryService(IUnitOfWork dataLayer)
        {
            this.dataLayer = dataLayer;
        }

        public void CreateNewCategory(string name)
        {
            Categories category = new Categories
            {
                Name = name
            };
            this.dataLayer.Categories.Create(category);
            this.dataLayer.Save();
        }

        public IEnumerable<CategoriesDTO> GetAllCategory()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Categories, CategoriesDTO>()).CreateMapper();
            var categoryDTOList = mapper.Map<IEnumerable<Categories>, List<CategoriesDTO>>(this.dataLayer.Categories.GetAll());
            return categoryDTOList;
        }

        public IEnumerable<ProductDTO> GetAllProductInCategory(int id)
        {
            Categories category = this.dataLayer.Categories.Get(id);
            return this.ProductMapper(category);
        }

        public IEnumerable<ProductDTO> SelectProduct(int id, int priceFrom, int priceTo, SortByEnum by)
        {
            List<ProductDTO> productDTOList;
            if (id != 0)
            {
                Categories category = this.dataLayer.Categories.Get(id);
                productDTOList = this.ProductMapper(category).ToList();
            }
            else
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()).CreateMapper();
                productDTOList = mapper.Map<IEnumerable<Product>, List<ProductDTO>>(this.dataLayer.Product.GetAll());
            }

            return this.SelectAndSortByCriteria(productDTOList, priceFrom, priceTo, by);
        }

        public bool ContainCategoryWithName(string name)
        {
            Categories category = this.dataLayer.Categories.GetAll().SingleOrDefault(cat => cat.Name == name);
            return category != null;
        }

        private IEnumerable<ProductDTO> ProductMapper(Categories categories)
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

        private IEnumerable<ProductDTO> SelectAndSortByCriteria(List<ProductDTO> productDTOList, int priceFrom, int priceTo, SortByEnum by)
        {
            if (priceTo > priceFrom)
            {
                productDTOList = productDTOList.Where(productDTO => productDTO.Price >= priceFrom && productDTO.Price <= priceTo).ToList();
            }
            else
            {
                productDTOList = productDTOList.Where(productDTO => productDTO.Price >= priceFrom).ToList();
            }

            switch (by)
            {
                case SortByEnum.PriceToUp:
                    return productDTOList.OrderBy(productDTO => productDTO.Price);
                case SortByEnum.PriceDoDown:
                    return productDTOList.OrderByDescending(productDTO => productDTO.Price);
                case SortByEnum.ByName:
                    return productDTOList.OrderBy(productDTO => productDTO.Name);
                case SortByEnum.ByNameDescending:
                    return productDTOList.OrderByDescending(productDTO => productDTO.Name);
            }

            return productDTOList;
        }
    }
}