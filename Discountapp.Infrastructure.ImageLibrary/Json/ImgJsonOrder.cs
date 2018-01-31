namespace Discountapp.Infrastructure.ImageLibrary.Json
{
    public class ImgJsonOrder
    {
        /// <summary>
        /// Порядок изображений, которые хочет видеть пользователь |
        /// The order of img files to display on website.
        /// </summary>
        public int Order { get; set; }
        
        
        /// <summary>
        /// Реальное название файла в качестве индекса |
        /// The real file name as index 
        /// </summary>
        public int IndexName { get; set; }

    }
}
