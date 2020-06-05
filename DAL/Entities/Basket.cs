using System.Collections.Generic;

namespace DAL.Entities
{
    public class Basket
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<ProductInBasket> Products { get; set; }
    }
}