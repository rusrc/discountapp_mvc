using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.IO;
using System.Linq;
using Discountapp.Domain.Models.Identity;
using Discountapp.Infrastructure.Constants;
using Discountapp.Infrastructure.Exceptions;
using Newtonsoft.Json;

namespace Discountapp.Domain.Models.Application
{
    using Const = Discountapp.Infrastructure.Constants.Constant;
    public class Company : ImageEntity, INameable, ICompany
    {
        public const string LOGO_NAME = Const.LOGO_NAME;
        public const int WIDTH_LOGO = Const.WIDTH_LOGO;
        public const int HEIGHT_LOGO = Const.HEIGHT_LOGO;

        public long UserId { get; set; }
        public string Name { get; set; }
        public string LogoFolder { get; set; }
        public string HotLineNumber { get; set; }
        public string WebSiteLink { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual AppUser User { get; set; }
        [JsonIgnore]
        public ICollection<RealEstate> RealEstates { get; set; }

        /// <summary>
        /// Get logo file name from drive
        /// </summary>
        [NotMapped]
        public string LogoName
        {
            get
            {
                var foldersOk = !string.IsNullOrEmpty(LogoFullName) && 
                                !string.IsNullOrEmpty(LogoFolder) && 
                                !string.IsNullOrEmpty(Config.UploadFolderName);

                if (foldersOk)
                    return $"{Path.DirectorySeparatorChar}{Path.Combine(Config.UploadFolderName, LogoFolder, new DirectoryInfo(LogoFullName).Name)}";
                else
                    return this.DefaultImageFullPath;
            }
            set { }
        }

        /// <summary>
        /// Get full logo path with file name form drive
        /// </summary>
        [NotMapped]
        public string LogoFullName
        {
            get
            {
                if (string.IsNullOrEmpty(this.LogoFolder))
                {
                    return string.Empty;
                }

                string path = Path.Combine(Config.UploadFolderFullPath, this.LogoFolder);
                string fullPathName = string.Empty;

                if (Directory.Exists(path))
                {
                    fullPathName = Directory.GetFiles(path).FirstOrDefault(f => new DirectoryInfo(f).Name.StartsWith(LOGO_NAME));
                }

                return fullPathName;
            }
            set
            {
                //if (value == null) throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Get first logo full pathh name or empty
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessLogicException">BusinessLogicException</exception>
        public string GetFullTempLogoFileName()
        {
            var rootPath = Path.Combine(Config.UploadTempFolderFullPath, this.TempFolder);
            string[] rootFiles = Directory.GetFiles(rootPath);

            if (rootFiles.Any())
            {
                return rootFiles.FirstOrDefault();
            }

            throw new BusinessLogicException("Не удается найти логотип. Возможно он не был загружен");
        }

        /// <summary>
        /// Check if image file is in temp folder
        /// </summary>
        public bool IsLogoExistInTempFolder
        {
            get
            {
                try
                {
                    var rootPath = Path.Combine(Config.UploadTempFolderFullPath, this.TempFolder);
                    return Directory.GetFiles(rootPath).Any();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <exception cref="BusinessLogicException"></exception>
        public virtual void SaveLogoInLogoFolder(IWebImage webImage, int width = WIDTH_LOGO, int height = HEIGHT_LOGO)
        {
            if (string.IsNullOrEmpty(this.LogoFolder))
            {
                throw new ApplicationCriticalException(
                    $"Generate first unique folder name by method {nameof(GenerateFolder)} and set it to {nameof(LogoFolder)}");
            }

            if (!Directory.Exists(Path.Combine(Config.UploadFolderFullPath, LogoFolder)))
            {
                DirectoryInfo info = Directory.CreateDirectory(Path.Combine(Config.UploadFolderFullPath, LogoFolder));
                this.LogoFolder = info.Name;
            }

            webImage
                ?.Resize(width, height)
                ?.Save(Path.Combine(Config.UploadFolderFullPath, this.LogoFolder, LOGO_NAME));
        }

        /// <summary>
        /// Delete all items from tempFolderPath and create there temp image
        /// </summary>
        /// <param name="webImage">IWebImage</param>
        /// <param name="tempFolderPath">path of current temp folder genereted for logo store</param>
        /// <param name="width">width</param>
        /// <param name="height">heightparam>
        public virtual void SaveTempLogo(IWebImage webImage, string tempFolderPath, int width = WIDTH_LOGO, int height = HEIGHT_LOGO)
        {
            var dirInfo = new DirectoryInfo(tempFolderPath);

            dirInfo.GetFiles()
                .ToList().ForEach(f => f.Delete());

            dirInfo.GetDirectories()
                .ToList().ForEach(d => d.Delete(true));

            if (webImage != null)
            {
                string fileName = $"{Guid.NewGuid():N}";

                webImage
                    .Resize(width, height)
                    .Save(Path.Combine(tempFolderPath, fileName));
            }
        }
    }

    public class CompanyEntityConfiguration : EntityTypeConfiguration<Company>
    {
        public CompanyEntityConfiguration()
        {
            this.Property(p => p.HotLineNumber).HasMaxLength(20);
        }
    }
}
