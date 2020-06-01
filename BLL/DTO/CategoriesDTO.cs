using BLL.Interfaces;

namespace BLL.DTO
{
    public class CategoriesDTO : IName
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public byte[] ImageInByte { get; set; }

        public string ParentCategory { get; set; }
    }
}