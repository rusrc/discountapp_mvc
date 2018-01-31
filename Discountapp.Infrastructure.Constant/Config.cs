using System;
using System.IO;
using static System.Configuration.ConfigurationManager;
using static System.Convert;

namespace Discountapp.Infrastructure.Constants
{
    public class Config
    {
        public static int RowsCount => ToInt32(AppSettings[nameof(RowsCount)]);
        public static int RowsCountIndex => ToInt32(AppSettings[nameof(RowsCountIndex)]);
        public static string SecretKey => Convert.ToString(AppSettings[nameof(SecretKey)]);
        public static string GrecaptchaUrl => Convert.ToString(AppSettings[nameof(GrecaptchaUrl)]);
        public static RepositoryType ApiRepository => (RepositoryType)ToInt32(AppSettings[nameof(ApiRepository)]);
        public static RepositoryType MvcRepository => (RepositoryType)ToInt32(AppSettings[nameof(MvcRepository)]);
        /// <summary>
        /// Upload
        /// </summary>
        public static string UploadFolderName => Convert.ToString(AppSettings[nameof(UploadFolderName)]);

        public static string DefaultPromotionItemImageName => Convert.ToString(AppSettings[nameof(DefaultPromotionItemImageName)]);
        /// <summary>
        /// Upload/temp
        /// </summary>
        public static string UploadTempFolderFullPath
        {
            get
            {
                string path = Path.Combine(UploadFolderFullPath, "temp");

                return !Directory.Exists(path) ? Directory.CreateDirectory(path).FullName : path;
            }
        }

        public static string UploadFolderFullPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UploadFolderName);

        public static int PromotionPageCount => ToInt32(AppSettings[nameof(PromotionPageCount)]);
        public static int PromotionItemPageCount => ToInt32(AppSettings[nameof(PromotionItemPageCount)]);
    }

    public enum RepositoryType
    {
        Database = 2,
        Memory = 4
    }
}
