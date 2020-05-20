using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;

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
    }
}
