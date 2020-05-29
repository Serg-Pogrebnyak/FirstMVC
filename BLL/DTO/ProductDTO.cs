namespace BLL.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public string LongDescription { get; set; }

        public byte[] ImageInByte { get; set; }
    }
}