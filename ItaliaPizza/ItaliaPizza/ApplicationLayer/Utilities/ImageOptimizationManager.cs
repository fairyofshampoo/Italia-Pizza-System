using ImageMagick;
using System.IO;

namespace ItaliaPizza.ApplicationLayer.Utilities
{
    public class ImageOptimizationManager
    {
        public byte[] OptimizeImage(byte[] imageBytes, int quality, int width)
        {
            using (MagickImage image = new MagickImage(imageBytes))
            {
                image.Quality = quality;
                image.Resize(width, 0);
                return image.ToByteArray();
            }
        }

        public byte[] LoadImage(string path)
        {
            return File.ReadAllBytes(path);
        }
    }

}
