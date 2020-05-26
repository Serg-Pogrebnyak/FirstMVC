using System.Collections.Generic;

namespace BLL.DTO
{
    public class BasketDTO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public List<int> ProductsId { get; set; }
    }
}