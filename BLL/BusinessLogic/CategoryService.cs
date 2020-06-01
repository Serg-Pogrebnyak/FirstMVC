using System.Collections.Generic;
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
        private readonly IUnitOfWork db;

        public CategoryService(IUnitOfWork db)
        {
            this.db = db;
        }

        public(string textError, bool isValid) IsContainCategoryWithNameAndTag(CategoriesDTO newCategory)
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
                ParentCategory = parentCategory ?? null
            };
            this.db.Repository.Create(category);
            this.db.Save();
        }

        public IEnumerable<CategoriesDTO> GetAllCategory()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Categories, CategoriesDTO>()).CreateMapper();
            var categoryDTOList = mapper.Map<IEnumerable<Categories>, List<CategoriesDTO>>(this.db.Repository.GetAll<Categories>());
            return categoryDTOList;
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