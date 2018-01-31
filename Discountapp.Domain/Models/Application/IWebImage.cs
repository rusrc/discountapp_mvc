using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discountapp.Domain.Models.Application
{
    public interface IWebImage
    {
        void Write(string requestedFormat = null);

        string ImageFormat { get; }

        int Width { get; }

        int Height { get; }

        byte[] GetBytes(string requestedFormat = null);

        IWebImage Resize(int width, int height, bool preserveAspectRatio = true, bool preventEnlarge = false);
        IWebImage Save(string filePath = null, string imageFormat = null, bool forceCorrectExtension = true);
    }
}
