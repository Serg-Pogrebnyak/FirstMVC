using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        public enum SortByEnum : ushort
        {
            PriceToUp = 0,
            PriceDoDown = 1,
            ByName = 2,
            ByNameDescending = 3
        }

        public IEnumerable<CategoriesDTO> GetAllCategory();

        public(IEnumerable<CategoriesDTO> elements, int countOfPages) GetElementsByPageAndCountOfPages(int byPage, int elementPerPage);

        public(string textError, bool isValid) IsContainCategoryWithNameAndTag(CategoriesDTO newCategory);

        public void CreateNewCategory(CategoriesDTO newCategory);

        public IEnumerable<ProductDTO> GetAllProductInCategory(string tag);

        public IEnumerable<ProductDTO> SelectProduct(int id, int priceFrom, int priceTo, SortByEnum by);

        public bool ContainCategoryWithName(string name);
    }
}