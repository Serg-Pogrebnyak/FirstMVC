namespace BLL.DTO
{
    public class CategoriesDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public string ParentCategory { get; set; }
    }
}