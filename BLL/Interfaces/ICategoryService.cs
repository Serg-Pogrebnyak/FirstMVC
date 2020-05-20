using System;
using System.Collections.Generic;
using System.Text;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        public IEnumerable<CategoriesDTO> getAllCategory();
        public void createNewCategory(String Name);
    }
}
