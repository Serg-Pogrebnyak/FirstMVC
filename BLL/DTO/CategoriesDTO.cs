using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    class CategoriesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductDTO> Products { get; set; }
    }
}
