using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.ApplicationLayer.Management
{
    public class ImageCacheManager
    {
        private readonly string cacheDirectory;

        public ImageCacheManager()
        {
            cacheDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageCache");
            if (!Directory.Exists(cacheDirectory))
            {
                Directory.CreateDirectory(cacheDirectory);
            }
        }

        public string GetCachePath(string productId)
        {
            return Path.Combine(cacheDirectory, $"{productId}.jpg");
        }

        public bool IsImageCached(string productId)
        {
            string cachePath = GetCachePath(productId);
            return File.Exists(cachePath);
        }

        public byte[] LoadImageFromCache(string productId)
        {
            string cachePath = GetCachePath(productId);
            return File.ReadAllBytes(cachePath);
        }

        public void SaveImageToCache(string productId, byte[] imageBytes)
        {
            string cachePath = GetCachePath(productId);
            File.WriteAllBytes(cachePath, imageBytes);
        }
    }
}