namespace DAL.Entities
{
    public class ProductInBasket
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int ProductCount { get; set; }

        public int? BasketId { get; set; }

        public virtual Basket Basket { get; set; }
    }
}