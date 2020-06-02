using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using DAL.Entities;

namespace DAL.InitialData
{
    internal static class DBInitialData
    {
        internal static(Product[], Categories) GetPhoneProductsWithCategory()
        {
            Categories category = new Categories { Id = 1, Name = "Phones", Tag = "phones", ImageInByte = ImageInByte("phones.jpg") };
            Product p1 = new Product { Id = 1, Name = "iPhone SE 2020", Price = 14228, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p2 = new Product { Id = 2, Name = "iPhone 11", Price = 19899, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p3 = new Product { Id = 3, Name = "iPhone 11 Pro", Price = 27699, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p4 = new Product { Id = 4, Name = "iPhone 11 Pro Max", Price = 30199, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p5 = new Product { Id = 5, Name = "iPhone 11 Pro Max (Open Box)", Price = 38527, Description = "512GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p6 = new Product { Id = 6, Name = "iPhone XS Max", Price = 23399, Description = "256GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p7 = new Product { Id = 7, Name = "iPhone XS", Price = 18232, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p8 = new Product { Id = 8, Name = "iPhone 11 Pro Dual Sim", Price = 28973, Description = "64", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p9 = new Product { Id = 9, Name = "iPhone XS Max", Price = 21138, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p10 = new Product { Id = 10, Name = "iPhone XR", Price = 17666, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p11 = new Product { Id = 11, Name = "iPhone 8", Price = 12496, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p12 = new Product { Id = 12, Name = "iPhone XR Dual Sim", Price = 20390, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p13 = new Product { Id = 13, Name = "iPhone X", Price = 17841, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p14 = new Product { Id = 14, Name = "iPhone 8 Plus", Price = 15799, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p15 = new Product { Id = 15, Name = "iPhone 7 Plus", Price = 12499, Description = "32GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p16 = new Product { Id = 16, Name = "iPhone 7", Price = 9248, Description = "32GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p17 = new Product { Id = 17, Name = "Samsung A51", Price = 7087, Description = "64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p18 = new Product { Id = 18, Name = "Samsung S20", Price = 23550, Description = "128GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p19 = new Product { Id = 19, Name = "Samsung S20+", Price = 28090, Description = "128GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p20 = new Product { Id = 20, Name = "Samsung S20 Ultra", Price = 34708, Description = "128GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };

            return (new Product[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20 }, category);
        }

        internal static(Product[], Categories) GetTabletProductsWithCategory()
        {
            Categories category = new Categories { Id = 2, Name = "Tablets", Tag = "tablets", ImageInByte = ImageInByte("tablets.jpg") };

            Product p1 = new Product { Id = 21, Name = "iPad Pro 11 (2019)", Price = 28895, Description = "Wi-Fi+Cellular 256GB", ImageInByte = Image, CategoriesId = category.Id };
            Product p2 = new Product { Id = 22, Name = "iPad Pro 11 (2020)", Price = 26517, Description = "Wi-Fi 128GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p3 = new Product { Id = 23, Name = "iPad Pro 11 (2020)", Price = 30425, Description = "Wi-Fi+Cellular 128GB", ImageInByte = Image, CategoriesId = category.Id };
            Product p4 = new Product { Id = 24, Name = "iPad Pro 12.9 (2020)", Price = 31821, Description = "Wi-Fi 128GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p5 = new Product { Id = 25, Name = "iPad Pro 12.9", Price = 20399, Description = "Wi-Fi+Cellular 64GB", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };

            return (new Product[] { p1, p2, p3, p4, p5 }, category);
        }

        internal static(Product[], Categories) GetMonitorProductsWithCategory()
        {
            Categories category = new Categories { Id = 3, Name = "Monitors", Tag = "monitors", ImageInByte = ImageInByte("monitors.png") };
            Product p1 = new Product { Id = 26, Name = "AOC 23.6", Price = 5299, Description = "144GH", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p2 = new Product { Id = 27, Name = "Samsung Curved 27", Price = 4444, Description = "60GH", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p3 = new Product { Id = 28, Name = "LG UltraGear 31.5", Price = 9649, Description = "144GH", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p4 = new Product { Id = 29, Name = "Samsung Curved 27", Price = 7999, Description = "60GH", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };
            Product p5 = new Product { Id = 30, Name = "Samsung Curved 23.5", Price = 3299, Description = "60GH", LongDescription = "l d", ImageInByte = Image, CategoriesId = category.Id };

            return (new Product[] { p1, p2, p3, p4, p5 }, category);
        }

        private static byte[] Image
        {
            get
            {
                var path = "wwwroot/Images/noImage.jpg";
                using (Bitmap image = new Bitmap(System.Drawing.Image.FromFile(path)))
                {
                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
        }

        private static byte[] ImageInByte(string name)
        {
            var path = "wwwroot/Images/" + name;
            using (Bitmap image = ResizeImage(new Bitmap(System.Drawing.Image.FromFile(path))))
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static Bitmap ResizeImage(Image image)
        {
            int width = 286;
            int height = 180;
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}