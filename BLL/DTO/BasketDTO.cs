using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    class BasketDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<int> ProductsId { get; set; }
    }
}
