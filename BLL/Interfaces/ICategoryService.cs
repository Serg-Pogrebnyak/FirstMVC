using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        public enum SortByEnum : ushort
        {
            None = 0,
            PriceToUp = 1,
            PriceToDown = 2,
            ByName = 3,
            ByNameDescending = 4
        }

        public IEnumerable<CategoriesDTO> GetAllCategory();

        public CategoriesDTO GetCategory(int id);

        public(IEnumerable<CategoriesDTO> elements, int countOfPages) GetCategoriesByPageAndCountOfPages(int byPage, int elementPerPage, SortByEnum sortBy);

        public(IEnumerable<ProductDTO> elements, int countOfPages) GetProductsByPageAndCountOfPages(string categoryTag, int elementPerPage, SelectingSortingProductCriteriaBLL criteria);

        public(string textError, bool isValid) IsContainCategoryWithNameAndTag(CategoriesDTO newCategory);

        public void CreateNewCategory(CategoriesDTO newCategory);

        public void UpdateCategory(CategoriesDTO changedCategory);

        public IEnumerable<ProductDTO> GetAllProductInCategory(string tag);

        public bool ContainCategoryWithName(string name);
    }
}