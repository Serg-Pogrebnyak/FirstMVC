using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        public enum SortByEnum : ushort
        {
            priceToUp = 0,
            priceDoDown = 1,
            byName = 2,
            byNameDescending = 3
        }
        public IEnumerable<CategoriesDTO> getAllCategory();
        public void createNewCategory(String Name);

        public IEnumerable<ProductDTO> getAllProductInCategory(int id);
        public IEnumerable<ProductDTO> selectProduct(int id, int priceFrom, int priceTo, SortByEnum by);

        public bool containCategoryWithName(String name);
    }
}
