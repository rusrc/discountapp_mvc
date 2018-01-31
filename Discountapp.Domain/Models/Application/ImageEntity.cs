using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.WebPages;
using Discountapp.Infrastructure.Exceptions;
using Newtonsoft.Json;

namespace Discountapp.Domain.Models.Application
{
    using Config = Discountapp.Infrastructure.Constants.Config;
    using Const = Discountapp.Infrastructure.Constants.Constant;
    public class ImageEntity : BaseEntity, IImageEntity
    {
        const string DEFAULT_IMAGE_FOLDER = "Default";
        const int DEFAULT_IMAGE_WIDTH = Const.DEFAULT_IMAGE_WIDTH;
        const int DEFAULT_IMAGE_HEIGHT = Const.DEFAULT_IMAGE_HEIGHT;
        /// <summary>
        /// Mappable property to store foldername in database
        /// </summary>
        [JsonIgnore]
        public virtual string ImageFolder { get; set; }
        /// <summary>
        /// Store files after client upload or other
        /// </summary>
        [JsonIgnore, NotMapped]
        public virtual string TempFolder { get; set; }

        /// <summary>
        /// Generate ImageFolder and save there image from TempFolder 
        /// </summary>
        public virtual void SaveImage()
        {
            var pathToTempFolder = Path.Combine(Config.UploadTempFolderFullPath, this.TempFolder);
            var fileFullPath = Directory.GetFiles(pathToTempFolder).FirstOrDefault();

            if (string.IsNullOrEmpty(fileFullPath))
            {
                throw new BusinessLogicException("Ups! Can't file file in temp folder");
            }

            var info = this.GenerateFolder();
            this.ImageFolder = info.Name;


            new WebImage(fileFullPath).Resize(DEFAULT_IMAGE_WIDTH, DEFAULT_IMAGE_HEIGHT).Save(Path.Combine(info.FullName, "0"));
        }

        /// <summary>
        /// Simple file path like /Folder/File.name
        /// </summary>
        public virtual string FolderWithImagePathSimple => this.FolderWithImagePath.Replace("\\", "/").Replace("~", string.Empty);
        //TODO Dry method the save in merchant
        /// <summary>
        /// ~/Upload/FolderName/ImageFileName
        /// </summary>
        [NotMapped]
        public virtual string FolderWithImagePath
        {
            get
            {
                var folderImage = this.GetFolderAndImage();
                if (folderImage.Item1.IsEmpty() || folderImage.Item2.IsEmpty())
                    return string.Empty;
                
                return $"{Path.DirectorySeparatorChar}{Path.Combine(Config.UploadFolderName, folderImage.Item1, folderImage.Item2)}";
            }
            set { }
        }

        /// <summary>
        /// Default image
        /// </summary>
        public string DefaultImage => Path.Combine(Config.UploadFolderName, DEFAULT_IMAGE_FOLDER, Config.DefaultPromotionItemImageName);

        /// <summary>
        /// Default image full path
        /// </summary>
        public string DefaultImageFullPath => Path.Combine(Config.UploadFolderFullPath, DEFAULT_IMAGE_FOLDER, Config.DefaultPromotionItemImageName);
        
        //TODO Dry method the save in merchant
        /// <summary>
        /// item1 = FolderName
        /// itme2 = ImageFileName
        /// </summary>
        /// <returns></returns>
        public virtual Tuple<string, string> GetFolderAndImage()
        {
            if (this.ImageFolder != null)
            {
                var fullDiretoryPath = Path.Combine(Config.UploadFolderFullPath, this.ImageFolder);
                if (Directory.Exists(fullDiretoryPath))
                {
                    string[] filePathes = Directory.GetFiles(fullDiretoryPath);
                    if (filePathes.Any() && filePathes?.FirstOrDefault() != null)
                    {
                        return new Tuple<string, string>(this.ImageFolder,
                            new FileInfo(filePathes.FirstOrDefault()).Name);
                    }
                }
            }
            return new Tuple<string, string>(DEFAULT_IMAGE_FOLDER, Config.DefaultPromotionItemImageName);
        }

        /// <summary>
        /// Return fulle path for one image in folder
        /// </summary>
        /// <returns></returns>
        public virtual string GetFullImagePath()
        {
            if (this.ImageFolder != null)
            {
                var fullDiretoryPath = Path.Combine(Config.UploadFolderFullPath, this.ImageFolder);
                if (Directory.Exists(fullDiretoryPath))
                {
                    string[] filePathes = Directory.GetFiles(fullDiretoryPath);
                    if (filePathes.Any() && filePathes?.FirstOrDefault() != null)
                    {
                        return filePathes.FirstOrDefault();
                    }
                }
            }
            return this.DefaultImageFullPath;
        }

        /// <summary>
        /// Image width in pixels
        /// </summary>
        [NotMapped]
        public virtual int ImageWidth => new Lazy<Tuple<int, int>>(ImagePropotion).Value.Item1;
        /// <summary>
        /// Image height in pixels
        /// </summary>
        [NotMapped]
        public virtual int ImageHeight => new Lazy<Tuple<int, int>>(ImagePropotion).Value.Item2;

        /// <summary>
        /// item1 = img.Width
        /// item2 = img.Height
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> ImagePropotion()
        {
            Bitmap img = new Bitmap(this.GetFullImagePath());

            return 
                new Tuple<int, int>(img.Width, img.Height);
        }
    }
}
