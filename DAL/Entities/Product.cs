namespace DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public string LongDescription { get; set; }

        public string ImageName { get; set; }

        public int? CategoriesId { get; set; }

        public virtual Categories Category { get; set; }
    }
}