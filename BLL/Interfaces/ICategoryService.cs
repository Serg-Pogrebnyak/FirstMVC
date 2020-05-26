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

        public void CreateNewCategory(string name);

        public IEnumerable<ProductDTO> GetAllProductInCategory(int id);

        public IEnumerable<ProductDTO> SelectProduct(int id, int priceFrom, int priceTo, SortByEnum by);

        public bool ContainCategoryWithName(string name);
    }
}