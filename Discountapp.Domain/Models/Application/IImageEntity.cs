using System;

namespace Discountapp.Domain.Models.Application
{
    public interface IImageEntity
    {
        string FolderWithImagePath { get; set; }
        string FolderWithImagePathSimple { get; }
        string ImageFolder { get; set; }
        string TempFolder { get; set; }

        Tuple<string, string> GetFolderAndImage();
        void SaveImage();
    }
}