namespace Discountapp.Infrastructure.Constants
{
    public class Constant
    {
        public const string ALL_CITIES = "AllCities";
        public const string URL_REGION_RESET = "Reset";
        /// <summary>
        /// Name of cookie in client  = culture"
        /// </summary>
        public const string COOKIE_NAME_CULTURE = "culture";
        /// <summary>
        /// Name of cookie in client  = currentCityID
        /// </summary>
        public const string COOKIE_NAME_CURRENTCITYID = "currentCityID";
        /// <summary>
        /// Root folder for uploading user's ad images
        /// </summary>
        public const string UPLOADS_IMG_FOLDER_NAME = "Uploads";
        /// <summary>
        /// Subfolder for user's cars ad images
        /// </summary>
        public const string IMG_FOLDER_NAME = "AdvImages";
        /// <summary>
        /// Default no image
        /// </summary>
        public const string DEFAULT_NO_IMG = "no_img1.jpg";
        /// <summary>
        /// Path to default no image
        /// </summary>
        public const string DEFAULT_NO_IMG_FULL_PATH = @"Content/Imgs/" + DEFAULT_NO_IMG;
        public const string DEFAULT_LOGO_PATH = @"Content/Imgs/Logo.png";
        //ImgManager namespace
        public const int DEFAULT_CONTAINER_WIDTH = 400;
        public const int DEFAULT_CONTAINER_HEIGHT = 300;
        /// <summary>
        /// Full file size 
        /// </summary>
        public const string FULL_FILE_NAME_PREFIX = "-1024x720";
        public const string MIDDLE_FILE_NAME_PREFIX = "-400x300";
        public const string SMALL_FILE_NAME_PREFIX = "-200x150";
        public const string SMALL_MIDDLE_FILE_NAME_PREFIX = "-120x90";
        public const string SUPER_SMALL_FILE_NAME_PREFIX = "-60x45";
        /// <summary>
        /// Default logo name
        /// </summary>
        public const string LOGO_NAME = "logo";
        /// <summary>
        /// Company logo width
        /// </summary>
        public const int WIDTH_LOGO = 100;
        /// <summary>
        /// Company logo width
        /// </summary>
        public const int HEIGHT_LOGO = 70;
        /// <summary>
        /// DEFAULT_IMAGE_WIDTH
        /// </summary>
        public const int DEFAULT_IMAGE_WIDTH = 1920;
        /// <summary>
        /// DEFAULT_IMAGE_HEIGHT
        /// </summary>
        public const int DEFAULT_IMAGE_HEIGHT = 1080;
    }
}
