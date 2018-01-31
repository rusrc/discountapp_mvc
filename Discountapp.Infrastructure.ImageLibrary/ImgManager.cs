using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Discountapp.Infrastructure.ImageLibrary
{
    using CONSTANT = Discountapp.Infrastructure.Constants.Constant;
    /// <summary>
    /// определяет действие, которое необходимо сделать с графическим файлом
    /// </summary>
    public enum ImageSizeChangeType
    {
        /// <summary>
        /// заключить в контейнер
        /// </summary>
        Inclose,
        /// <summary>
        /// образать
        /// </summary>
        Crop
    }

    public enum ImageResizeMode
    {
        /// <summary>
        ///  по максимальной составляющей
        /// </summary>
        OnMaxSide,
        /// <summary>
        /// по минимальной составляющей
        /// </summary>
        OnMinSide
    }

    /// <summary>
    /// структура для хранения изображения и координат смещения внутри контейнера
    /// </summary>
    public struct ResultImage
    {
        public Bitmap Bmp;             // содержит изображение
                                       // следующие 2 переменные используются для расчёта координат при нанесении водяного знака на изображение
        public int ImgRightSideShift;  // содержит смещение правого края изображения внутри контейнера
        public int ImgBottomSideShift; // содержит смещение нижнего края изображения внутри контейнера
    }


    public partial class ImgManager
    {
        /// <summary>
        /// ширина контейнера для изображения по умолчанию
        /// </summary>
        public const int DEFAULT_CONTAINER_WIDTH = CONSTANT.DEFAULT_CONTAINER_WIDTH;  //400;
        /// <summary>
        /// высота контейнера для изображения по умолчанию
        /// </summary>
        public const int DEFAULT_CONTAINER_HEIGHT = CONSTANT.DEFAULT_CONTAINER_HEIGHT;


        /// <summary>
        /// функция уменьшения логотипа
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image ResizeLogoImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }


        public static Image ResizeImage(Stream imgToResize, int containerWidth = DEFAULT_CONTAINER_WIDTH, int containerHeight = DEFAULT_CONTAINER_HEIGHT)
        {
            return ResizeImage(Image.FromStream(imgToResize), ImageResizeMode.OnMinSide, containerWidth, containerHeight);
        }


        public static Image ResizeImage(Image imgToResize, ImageResizeMode resizeMode = ImageResizeMode.OnMinSide, int containerWidth = DEFAULT_CONTAINER_WIDTH, int containerHeight = DEFAULT_CONTAINER_HEIGHT)
        {
            Size imgSize = GetNewImageSize(imgToResize.Width, imgToResize.Height, resizeMode, containerWidth, containerHeight);

            return (Image)(new Bitmap(imgToResize, imgSize));
        }


        public static Size GetNewImageSize(int imgWidth, int imgHeight, ImageResizeMode resizeMode = ImageResizeMode.OnMinSide, int containerWidth = DEFAULT_CONTAINER_WIDTH, int containerHeight = DEFAULT_CONTAINER_HEIGHT)
        {
            double relX = (double)imgWidth / (double)containerWidth;
            double relY = (double)imgHeight / (double)containerHeight;

            switch (resizeMode)
            {
                case ImageResizeMode.OnMinSide:
                    if ((relX >= relY) && (relY > 1.0))
                    {
                        imgHeight = containerHeight;
                        imgWidth = (relX > relY) ? (int)((double)imgWidth / relY) : containerWidth;
                    }
                    else if ((relY > relX) && (relX > 1.0))
                    {
                        imgWidth = containerWidth;
                        imgHeight = (int)((double)imgHeight / relX);
                    }
                    break;
                case ImageResizeMode.OnMaxSide:
                    if ((relX >= relY) && (relY > 1.0))
                    {
                        imgWidth = containerWidth;
                        imgHeight = (relX > relY) ? (int)((double)imgHeight / relX) : containerHeight;
                    }
                    else if ((relY > relX) && (relX > 1.0))
                    {
                        imgHeight = containerHeight;
                        imgWidth = (int)((double)imgWidth / relY);
                    }
                    break;
            }

            return new Size(imgWidth, imgHeight);
        }

        /// <summary>
        /// функция наложения водяного знака
        /// </summary>
        /// <param name="resultImage">Объект типа ResultImage</param>
        /// <param name="imgLogo"></param>
        /// <returns></returns>
        public static Bitmap CreateWatermark(ResultImage resultImage, Image imgLogo)
        {
            int right = (int)((double)resultImage.Bmp.Width * 0.03);
            int bottom = (int)((double)resultImage.Bmp.Height * 0.03);

            int dX = resultImage.ImgRightSideShift - (imgLogo.Width + right);
            int dY = resultImage.ImgBottomSideShift - (imgLogo.Height + bottom);

            using (Graphics g = Graphics.FromImage(resultImage.Bmp))
            {
                g.DrawImage(imgLogo, dX, dY, imgLogo.Width, imgLogo.Height);
            }
            
            
            return resultImage.Bmp;
        }

        /// <summary>
        /// функция размещения изображения в рамках контейнера
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <param name="action"></param>
        /// <param name="imgBackground"></param>
        /// <param name="containerWidth"></param>
        /// <param name="containerHeight"></param>
        /// <returns></returns>
        public static ResultImage CreateImageInContainer(Image sourceImage, ImageSizeChangeType action, Image imgBackground = null, int containerWidth = DEFAULT_CONTAINER_WIDTH, int containerHeight = DEFAULT_CONTAINER_HEIGHT)
        {
            int newWidth = sourceImage.Width;
            int newHeight = sourceImage.Height;
            
            double relX = (double)newWidth / (double)containerWidth;
            double relY = (double)newHeight / (double)containerHeight;

            int dX = 0;
            int dY = 0;

            // предварительно уменьшаем изображение по наименьшей составляющей (ширина или высота) под контейнер,
            // чтобы было легче с ним работать
            //sourceImage = ResizeImage(sourceImage, containerWidth, containerHeight);

            switch (action)
            {
                case ImageSizeChangeType.Inclose:
                    if ((relX >= relY) && (relX > 1.0))
                    {
                        newWidth = containerWidth;
                        newHeight = (relX > relY) ? (int)((double)newHeight / relX) : containerHeight;
                    }
                    else if ((relY > relX) && (relY > 1.0))
                    {
                        newHeight = containerHeight;
                        newWidth = (int)((double)newWidth / relY);
                    }

                    dX = (int)((double)(containerWidth - newWidth) / 2.0);
                    dY = (int)((double)(containerHeight - newHeight) / 2.0);
                    break;

                case ImageSizeChangeType.Crop:
                    if ((relX >= relY) && (relY > 1.0))
                    {
                        newHeight = containerHeight;
                        newWidth = (relX > relY) ? (int)((double)newWidth / relY) : containerWidth;
                    }
                    else if ((relY > relX) && (relX > 1.0))
                    {
                        newWidth = containerWidth;
                        newHeight = (int)((double)newHeight / relX);
                    }

                    dX = (int)((double)(containerWidth - newWidth) / 2.0);
                    dY = (int)((double)(containerHeight - newHeight) / 2.0);

                    if ((dX < 0) || (dY < 0))
                    {
                        int dX1 = 0;
                        int dY1 = 0;

                        if (dX < 0)
                        {
                            dX1 = dX * (-1);
                            dX = 0;
                        }

                        if (dY < 0)
                        {
                            dY1 = dY * (-1);
                            dY = 0;
                        }

                        newWidth = newWidth - (dX1 * 2);
                        newHeight = newHeight - (dY1 * 2);

                        Bitmap bmpSourceImage = new Bitmap(sourceImage);

                        Rectangle section = new Rectangle(dX1, dY1, newWidth, newHeight);

                        sourceImage = bmpSourceImage.Clone(section, bmpSourceImage.PixelFormat);

                        bmpSourceImage.Dispose();
                    }

                    break;
            }

            ResultImage resultImage = new ResultImage
            {
                Bmp = new Bitmap(containerWidth, containerHeight)
            };


            using (Graphics g = Graphics.FromImage(resultImage.Bmp))
            {
                // здесь при определённых условиях делаем фон контейнера
                // если изображение занимает всю область контейнера, то фон соответственно не нужен
                if ((newWidth < containerWidth) || (newHeight < containerHeight))
                {
                    g.Clear(GetDominantColor(new Bitmap(ResizeImage(sourceImage, ImageResizeMode.OnMinSide, (int)((double)newWidth / 3.0), (int)((double)newHeight / 3.0))))); // заливка фона контейнера
                    // здесь накладываем на фон контейнера фоновый рисунок при условии, 
                    // что размеры контейнера совпадают с размерами фонового изображения
                    if ((imgBackground != null) && (containerWidth == imgBackground.Width) && (containerHeight == imgBackground.Height))
                    {
                        g.DrawImage(imgBackground, 0, 0, imgBackground.Width, imgBackground.Height);
                    }
                }

                g.DrawImage(sourceImage, dX, dY, newWidth, newHeight);
            }

            sourceImage.Dispose();

            resultImage.ImgRightSideShift = (newWidth == containerWidth) ? containerWidth : newWidth + dX;
            resultImage.ImgBottomSideShift = (newHeight == containerHeight) ? containerHeight : newHeight + dY;

            return resultImage;
        }


        /// <summary>
        /// функция нахождения среднего цвета картинки для заливки фона
        /// </summary>
        /// <param name="processedBitmap"></param>
        /// <returns></returns>
        public static Color GetDominantColor(Bitmap processedBitmap)
        {
            int red = 0, green = 0, blue = 0;
            int total = 0;

            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadOnly, processedBitmap.PixelFormat);

                int bytesPerPixel = Image.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* ptrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = ptrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        blue += (int)currentLine[x];
                        green += (int)currentLine[x + 1];
                        red += (int)currentLine[x + 2];

                        total++;
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }

            processedBitmap.Dispose();

            red /= total;
            green /= total;
            blue /= total;

            red = (red > 255) ? 255 : (red < 0) ? 0 : red;
            green = (green > 255) ? 255 : (green < 0) ? 0 : green;
            blue = (blue > 255) ? 255 : (blue < 0) ? 0 : blue;

            return Color.FromArgb(red, green, blue);
        }


        // функция нахождения среднего цвета картинки для заливки фона
        // эта функция была заменена на более быструю
        public static Color GetDominantColorSafeMode(Bitmap processedBitmap)
        {
            //Used for tally
            int red = 0;
            int green = 0;
            int blue = 0;

            int total = 0;

            for (int x = 0; x < processedBitmap.Width; x++)
            {
                for (int y = 0; y < processedBitmap.Height; y++)
                {
                    Color clr = processedBitmap.GetPixel(x, y);

                    red += clr.R;
                    green += clr.G;
                    blue += clr.B;

                    total++;
                }
            }

            processedBitmap.Dispose();

            //Calculate average
            red /= total;
            green /= total;
            blue /= total;

            return Color.FromArgb(red, green, blue);
        }


    }
}
