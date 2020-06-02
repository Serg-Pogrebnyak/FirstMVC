using System.Drawing;
using System.IO;

namespace BLL.Extensions
{
    public static class ByteExtension
    {
        public static byte[] ResizeImageFromByte(this byte[] bytesArr)
        {
            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                Image img = Image.FromStream(memstr);
                Image resizedImage = img.ResizeImage();
                using (var ms = new MemoryStream())
                {
                    resizedImage.Save(ms, img.RawFormat);
                    return ms.ToArray();
                }
            }
        }
    }
}