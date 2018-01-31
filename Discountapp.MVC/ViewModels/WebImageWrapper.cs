using System;
using System.Web.Helpers;
using Discountapp.Domain.Models.Application;

namespace Discountapp.MVC.ViewModels
{
    public class WebImageWrapper : IWebImage
    {
        private readonly WebImage _instance;

        public WebImageWrapper(byte[] data)
        {
            _instance = new WebImage(data);
        }

        public WebImageWrapper(WebImage image)
        {
            _instance = image;
        }

        public WebImageWrapper(string filePath)
        {
            _instance = new WebImage(filePath);
        }

        public void Write(string requestedFormat = null)
        {
            _instance.Write(requestedFormat);
        }

        public string ImageFormat => _instance.ImageFormat;

        public int Width => _instance.Width;

        public int Height => _instance.Height;

        public byte[] GetBytes(string requestedFormat = null)
        {
            return _instance.GetBytes(requestedFormat);
        }

        public IWebImage Resize(int width, int height, bool preserveAspectRatio = true, bool preventEnlarge = false)
        {
            return new WebImageWrapper(_instance.Resize(width, height, preserveAspectRatio, preventEnlarge));
        }

        public IWebImage Save(string filePath = null, string imageFormat = null, bool forceCorrectExtension = true)
        {
            return new WebImageWrapper(_instance.Save(filePath, imageFormat, forceCorrectExtension));
        }

        public static IWebImage GetImageFromRequest(string postedFileName = null)
        {
            return new WebImageWrapper(WebImage.GetImageFromRequest(postedFileName));
        }
    }
}