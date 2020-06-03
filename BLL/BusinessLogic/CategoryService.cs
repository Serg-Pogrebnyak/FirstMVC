﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BLL.DTO;
using BLL.Extensions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using static BLL.Interfaces.ICategoryService;

namespace BLL.BusinessLogic
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork db;

        public CategoryService(IUnitOfWork db)
        {
            this.db = db;
        }

        public(string textError, bool isValid) IsContainCategoryWithNameAndTag(CategoriesDTO newCategory)
        {
            // validate new element because id == 0
            if (newCategory.Id == 0)
            {
                Categories[] categories = this.db.Repository.GetAll<Categories>().ToArray();
                if (categories.SingleOrDefault(c => c.Name == newCategory.Name) == null && categories.SingleOrDefault(c => c.Tag == newCategory.Tag) == null)
                {
                    return (null, true);
                }
                else if (!(categories.SingleOrDefault(c => c.Name == newCategory.Name) == null))
                {
                    return ("Contain category with this name", false);
                }
                else if (!(categories.SingleOrDefault(c => c.Tag == newCategory.Tag) == null))
                {
                    return ("Contain category with this tag", false);
                }
                else
                {
                    return ("Internal error", false);
                }
            }// validate new element because id != 0
            else
            {
                Categories[] categories = this.db.Repository.GetAll<Categories>().ToArray();
                bool validateResultByName = categories.SingleOrDefault(c => c.Name == newCategory.Name && c.Id != newCategory.Id) == null;
                bool validateResultByTag = categories.SingleOrDefault(c => c.Tag == newCategory.Tag && c.Id != newCategory.Id) == null;
                if (validateResultByName && validateResultByTag)
                {
                    return (null, true);
                }
                else if (!validateResultByName)
                {
                    return ("Contain category with this name", false);
                }
                else if (!validateResultByTag)
                {
                    return ("Contain category with this tag", false);
                }
                else
                {
                    return ("Internal error", false);
                }
            }
        }

        public void CreateNewCategory(CategoriesDTO newCategory)
        {
            Categories parentCategory = null;
            if (newCategory.ParentCategory != null)
            {
                parentCategory = this.db.Repository.GetAll<Categories>().SingleOrDefault(category => category.Name == newCategory.ParentCategory);
            }

            Categories category = new Categories
            {
                Name = newCategory.Name,
                Tag = newCategory.Tag,
                ImageInByte = newCategory.ImageInByte.ResizeImageFromByte(),
                ParentCategory = parentCategory ?? null
            };
            this.db.Repository.Create(category);
            this.db.Save();
        }

        public void UpdateCategory(CategoriesDTO changedCategory)
        {
            Categories category = this.db.Repository.Get<Categories>(changedCategory.Id);
            category.Name = changedCategory.Name;
            category.Tag = changedCategory.Tag;

            Categories parentCategory = null;
            if (changedCategory.ParentCategory != null)
            {
                parentCategory = this.db.Repository.GetAll<Categories>().SingleOrDefault(category => category.Name == changedCategory.ParentCategory);
            }

            if (changedCategory.ImageInByte != null)
            {
                category.ImageInByte = changedCategory.ImageInByte.ResizeImageFromByte();
            }

            category.ParentCategory = parentCategory;
            this.db.Repository.Update(category);
            this.db.Save();
        }

        public IEnumerable<CategoriesDTO> GetAllCategory()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Categories, CategoriesDTO>()).CreateMapper();
            var categoryDTOList = mapper.Map<IEnumerable<Categories>, List<CategoriesDTO>>(this.db.Repository.GetAll<Categories>());
            return categoryDTOList;
        }

        public CategoriesDTO GetCategory(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Categories, CategoriesDTO>()).CreateMapper();
            var categoryDTO = mapper.Map<Categories, CategoriesDTO>(this.db.Repository.Get<Categories>(id));
            return categoryDTO;
        }

        public(IEnumerable<CategoriesDTO> elements, int countOfPages) GetCategoriesByPageAndCountOfPages(int byPage, int elementPerPage, SortByEnum sortBy)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Categories, CategoriesDTO>()).CreateMapper();
            var categoryDTOArray = mapper.Map<IEnumerable<Categories>, CategoriesDTO[]>(this.db.Repository.GetForPage<Categories>(byPage, elementPerPage));
            double notRoundedPages = this.db.Repository.GetCount<Categories>() / elementPerPage;
            int pages = Convert.ToInt32(Math.Ceiling(notRoundedPages));
            if (sortBy != SortByEnum.None)
            {
                categoryDTOArray = SelectingSortingService.SortByCriteria(categoryDTOArray, sortBy).ToArray();
            }

            return (categoryDTOArray, pages);
        }

        public(IEnumerable<ProductDTO> elements, int countOfPages) GetProductsByPageAndCountOfPages(string categoryTag, int elementPerPage, SelectingSortingProductCriteriaBLL criteria)
        {
            Categories category = this.db.Repository.GetAll<Categories>().SingleOrDefault(category => category.Tag == categoryTag);
            var allProductsList = this.GetAllProducts(category).Distinct();
            allProductsList = SelectingSortingService.SelectByPrice(allProductsList.Cast<IPrice>().ToArray(), criteria.PriceFrom, criteria.PriceTo).Cast<ProductDTO>().ToList();
            if (criteria.SortBy != SortByEnum.None)
            {
                allProductsList = SelectingSortingService.SortByCriteria(allProductsList.ToArray(), criteria.SortBy).Cast<ProductDTO>().ToList();
            }

            double notRoundedPages = this.db.Repository.GetCount<Categories>() / elementPerPage;
            int pages = Convert.ToInt32(Math.Ceiling(notRoundedPages));
            var selectedProducts = allProductsList.Skip(criteria.CurrentPage * elementPerPage).Take(elementPerPage);
            return (selectedProducts, pages);
        }

        public IEnumerable<ProductDTO> GetAllProductInCategory(string tag)
        {
            Categories category = this.db.Repository.GetAll<Categories>().SingleOrDefault(category => category.Tag == tag);
            return this.GetAllProducts(category).Distinct();
        }

        public IEnumerable<ProductDTO> SelectProduct(int id, int priceFrom, int priceTo, SortByEnum by)
        {
            List<ProductDTO> productDTOList;
            if (id != 0)
            {
                Categories category = this.db.Repository.Get<Categories>(id);
                productDTOList = this.GetAllProducts(category).Distinct().ToList();
            }
            else
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDTO>()).CreateMapper();
                productDTOList = mapper.Map<IEnumerable<Product>, List<ProductDTO>>(this.db.Repository.GetAll<Product>());
            }

            return this.SelectAndSortByCriteria(productDTOList, priceFrom, priceTo, by);
        }

        public bool ContainCategoryWithName(string name)
        {
            Categories category = this.db.Repository.GetAll<Categories>().SingleOrDefault(cat => cat.Name == name);
            return category != null;
        }

        // private funcitons
        private List<ProductDTO> GetAllProducts(Categories category, List<ProductDTO> products = null)
        {
            if (products == null)
            {
                products = new List<ProductDTO>();
            }

            products.AddRange(this.ProductMapper(category));

            foreach (var child in category.ChildCategory)
            {
                this.GetAllProducts(child, products);
            }

            return products;
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
                    Description = product.Description,
                    LongDescription = product.LongDescription,
                    ImageInByte = product.ImageInByte
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
                case SortByEnum.PriceToDown:
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