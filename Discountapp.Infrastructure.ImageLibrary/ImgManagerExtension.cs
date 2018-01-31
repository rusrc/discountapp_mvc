using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Discountapp.Infrastructure.ImageLibrary.Exceptions;
using Discountapp.Infrastructure.ImageLibrary.Json;
using Discountapp.Infrastructure.ImageLibrary.ModelViews;

namespace Discountapp.Infrastructure.ImageLibrary
{
    using CONSTANT = Discountapp.Infrastructure.Constants.Constant;
    public partial class ImgManager
    {
        public const string FULL_FILE_NAME_PREFIX = CONSTANT.FULL_FILE_NAME_PREFIX;// "-1024x720";
        public const string MIDDLE_FILE_NAME_PREFIX = CONSTANT.MIDDLE_FILE_NAME_PREFIX;//"-400x300";
        public const string SMALL_FILE_NAME_PREFIX = CONSTANT.SMALL_FILE_NAME_PREFIX;//"-200x150";
        public const string SMALL_MIDDLE_FILE_NAME_PREFIX = CONSTANT.SMALL_MIDDLE_FILE_NAME_PREFIX;//"-120x90";
        public const string SUPER_SMALL_FILE_NAME_PREFIX = CONSTANT.SUPER_SMALL_FILE_NAME_PREFIX;//"-60x45";

        public string AdImagesFolder { get; set; }
        public string LogoPath { get; set; }
        public string BackgroundPath { get; set; }


        public ImgJson SaveImages(HttpFileCollectionBase files, string uniqueImgFolderName, long categFolderName, ImgJson addToList = null, List<ImgSizeSetting> imgSizeList = null)
        {
            return SaveImages(files, uniqueImgFolderName, categFolderName.ToString(), addToList, imgSizeList);
        }
        public ImgJson SaveImages(HttpFileCollectionBase files, string uniqueImgFolderName, string categFolderName, ImgJson addToList = null, List<ImgSizeSetting> imgSizeList = null)
        {
            if(!CheckFilesExists(files))
                return new ImgJson
                {
                    CountHistory = 0,
                    ImgJsonOrderList = new List<ImgJsonOrder>()
                };

            #region ImgSizeList
            imgSizeList = imgSizeList ?? new List<ImgSizeSetting> {
                GetImgSizeSetting(FULL_FILE_NAME_PREFIX),           // 1024x720 - full изображение с огрничением 1024x720
                GetImgSizeSetting(MIDDLE_FILE_NAME_PREFIX),         // 400x300 - для карточки объявления
                GetImgSizeSetting(SMALL_FILE_NAME_PREFIX),          // 200x150 - для горячих
                GetImgSizeSetting(SUPER_SMALL_FILE_NAME_PREFIX)     // 60x45 - для превью в карточке
            };
            #endregion

            var imgJson = addToList ?? new ImgJson { CountHistory = 0, ImgJsonOrderList = new List<ImgJsonOrder>() };
            var folderCategPath = CheckFolderAndCreate(Path.Combine(AdImagesFolder, categFolderName), true);
            var userFolderPath = CheckFolderAndCreate(Path.Combine(AdImagesFolder, folderCategPath, uniqueImgFolderName), true);

            var imgLogo = Image.FromFile(LogoPath); // получаем файл логотипа
            var imgBackground = Image.FromFile(BackgroundPath); // получаем файл фонового изображения

            int maxWidth = imgSizeList.Max(x => x.Width);   // Максимальный размер в настройках
            int maxHeight = imgSizeList.Max(x => x.Height); // Минимальный размер в настройках


            ResultImage resultImage = new ResultImage();

            int indexName = 0;

            for(int i = 0; i < files.Count; i++)
            {

                Image resizedImg = null;

                try
                {
                    using(var fullImg = Image.FromStream(files[i].InputStream, useEmbeddedColorManagement: true, validateImageData: false))
                    {
                        resizedImg = ImgManager.ResizeImage(fullImg, ImageResizeMode.OnMinSide, maxWidth, maxHeight);
                    }
                }
                catch
                {
                    resizedImg?.Dispose();
                    files[i].InputStream?.Dispose();
                    continue;
                }


                files[i].InputStream?.Dispose();


                foreach(var e in imgSizeList)
                {
                    // предварительно уменьшаем изображение 
                    resultImage.Bmp = (e.Width == maxWidth) && (e.Height == maxHeight)
                                        ? new Bitmap(ImgManager.ResizeImage(resizedImg, ImageResizeMode.OnMaxSide, e.Width, e.Height))
                                        : new Bitmap(ImgManager.ResizeImage(resizedImg, ImageResizeMode.OnMinSide, e.Width, e.Height));
                    resultImage.ImgRightSideShift = resultImage.Bmp.Width;
                    resultImage.ImgBottomSideShift = resultImage.Bmp.Height;

                    #region proccessing
                    if(e.Width >= DEFAULT_CONTAINER_WIDTH)
                    {
                        int newLogoWidth;

                        if(e.Width == DEFAULT_CONTAINER_WIDTH)
                        {
                            // получаем изображение, вписанное в контейнер
                            resultImage = ImgManager.CreateImageInContainer(resultImage.Bmp, ImageSizeChangeType.Inclose, imgBackground, e.Width, e.Height);
                            newLogoWidth = (int)((double)e.Width * 0.2);
                        }
                        else
                        {
                            newLogoWidth = (resultImage.Bmp.Width < resultImage.Bmp.Height)
                                         ? (int)((double)resultImage.Bmp.Height * 0.2)
                                         : (int)((double)resultImage.Bmp.Width * 0.2);
                        }


                        Image imgLogoWatermark = imgLogo;
                        if(imgLogo.Width > newLogoWidth)
                        {
                            double relation = (double)imgLogo.Width / newLogoWidth;
                            int newLogoHeight = (int)((double)imgLogo.Height / relation);
                            imgLogoWatermark = ImgManager.ResizeLogoImage(imgLogo, new Size(newLogoWidth, newLogoHeight));
                        }

                        resultImage.Bmp = ImgManager.CreateWatermark(resultImage, imgLogoWatermark);
                    }
                    else
                    {
                        resultImage = ImgManager.CreateImageInContainer(resultImage.Bmp, ImageSizeChangeType.Crop, null, e.Width, e.Height);
                    }
                    #endregion


                    //Получаем последний индекс в названии файла
                    indexName = imgJson.CreateIndexName(i);
                    //Путь и имя файла, что там сохранить его
                    var path = Path.Combine(userFolderPath, $"{indexName}-{e.Width}x{e.Height}.jpg");


                    resultImage.Bmp.Save(path, ImageFormat.Jpeg);
                    resultImage.Bmp.Dispose();

                }

                resizedImg.Dispose();

                imgJson.ImgJsonOrderList.Add(new ImgJsonOrder { Order = indexName, IndexName = indexName });

            }

            imgLogo.Dispose();
            imgBackground.Dispose();


            imgJson.CountHistory = imgJson.GetMaxIndexNameInList();

            return imgJson;
        }


        public void DeleteImages(string uniqueImgFolderName)
        {
            try
            {
                var userFolderPath = Path.Combine(this.AdImagesFolder, uniqueImgFolderName);
                if(Directory.Exists(userFolderPath))
                {
                    Directory.Delete(userFolderPath, true);
                }
            }
            catch
            {
                throw new ImageLibraryException($@"Произошла ошибка при удалении фалов
                                       в каталоге пользователя каталог пользователя:
                                       {uniqueImgFolderName}");
            }
        }

        /// <summary>
        /// Формирует объект ImgSizeSetting из аргумента
        /// </summary>
        /// <param name="sizePrefix">Строка должна содержать две цифры с разделителем, иначе получим исключение</param>
        /// <returns>ImgSizeSetting</returns>
        /// <exception cref="ImgManager">ImgApplicationException</exception>
        public static ImgSizeSetting GetImgSizeSetting(string sizePrefix)
        {
            var w = new Regex(@"\d{1,4}", RegexOptions.IgnoreCase).Matches(sizePrefix);


            if(w.Count > 2)
                throw new ImageLibraryException(@"Упс! Получено больше 2-х значений, просто проверьте переданную строку. 
                                      Убедитесь, что в строке две цифры с одним разделителем");



            return new ImgSizeSetting
            {
                Width = int.Parse(w[0].Value),
                Height = int.Parse(w[1].Value)
            };

        }


        /// <summary>
        /// Проверяет есть ли папка, если нет создает её
        /// </summary>
        /// <param name="folderPath">Путь где надо искать папку</param>
        /// <returns></returns>
        public static bool CheckFolderAndCreate(string folderPath)
        {
            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return Directory.Exists(folderPath);

        }


        /// <summary>
        /// Проверяет есть ли папка, если нет создает её и возврощает путь к этой папке
        /// </summary>
        /// <param name="folderPath">Путь где надо искать папку</param>
        /// <param name="returnPath">Метка, чтобы вернуть может быть true или false</param>
        /// <returns></returns>
        public static string CheckFolderAndCreate(string folderPath, bool returnPath)
        {
            return CheckFolderAndCreate(folderPath) ? folderPath : string.Empty;
        }

        public static bool CheckFilesExists(HttpFileCollectionBase files)
        {
            return files.AllKeys.Any(f => (files.Get(f)?.ContentLength ?? 0) > 0);
        }

        /// <summary>
        /// Form image path 
        /// </summary>
        /// <param name="categFolderName">Folder name of any ad</param>
        /// <param name="folderImgName">ad image name</param>
        /// <param name="imgJsonObject">ImgJson object with history and count</param>
        /// <param name="imgSize">Frefix name for images, use <see cref="CONSTANT"/> with param as 'MIDDLE_FILE_NAME_PREFIX'</param>
        /// <exception cref="NullReferenceException">
        ///     ImgJsonObject with type of<see cref="ImgJson"/> can't be null
        /// </exception>
        /// <returns>string</returns>
        public static string GetImgPath(string categFolderName, string folderImgName, ImgJson imgJsonObject, string imgSize = CONSTANT.MIDDLE_FILE_NAME_PREFIX)
        {
            if(imgJsonObject == null)
                throw new NullReferenceException($"Check why {nameof(imgJsonObject)} comes with null");

            string imgRootPathResult;
            if(!string.IsNullOrEmpty(folderImgName) && imgJsonObject.ImgJsonOrderList.Count > 0 && imgJsonObject.ImgJsonOrderList != null)
            {
                var defaultImgJson = imgJsonObject
                                    .ImgJsonOrderList
                                    .Where(e => e != null)
                                    .OrderBy(e => e.Order)
                                    .FirstOrDefault();

                int indexMainName = defaultImgJson?.IndexName ?? 0;

                imgRootPathResult = $@"/{CONSTANT.UPLOADS_IMG_FOLDER_NAME}/{CONSTANT.IMG_FOLDER_NAME}/{categFolderName}/{folderImgName}/{indexMainName + imgSize}.jpg";
            }
            else
            {
                imgRootPathResult = $@"/{CONSTANT.DEFAULT_NO_IMG_FULL_PATH}";
            }


            return imgRootPathResult.Trim();
        }
        public static string GetImgPath(string categFolderName, string folderImgName, string imgJsonString, string imgSize = CONSTANT.MIDDLE_FILE_NAME_PREFIX)
        {
            ImgJson imgJsonObject = ImgJson.Parse(imgJsonString) ?? new ImgJson { CountHistory = 0, ImgJsonOrderList = null };
            return GetImgPath(categFolderName, folderImgName, imgJsonObject, imgSize);
        }
        public static string GetImgPath(long categFolderName, string folderImgName, ImgJson imgJsonObject, string imgSize = CONSTANT.MIDDLE_FILE_NAME_PREFIX)
        {
            return GetImgPath(categFolderName.ToString(), folderImgName, imgJsonObject, imgSize);
        }


        public static Bitmap Blur(Bitmap image, Rectangle rectangle, Int32 blurSize)
        {
            Bitmap blurred = new Bitmap(image.Width, image.Height);

            // make an exact copy of the bitmap provided
            using(Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            // look at every pixel in the blur rectangle
            for(Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for(Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    Int32 avgR = 0, avgG = 0, avgB = 0;
                    Int32 blurPixelCount = 0;

                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    for(Int32 x = xx; (x < xx + blurSize && x < image.Width); x++)
                    {
                        for(Int32 y = yy; (y < yy + blurSize && y < image.Height); y++)
                        {
                            Color pixel = blurred.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    // now that we know the average for the blur size, set each pixel to that color
                    for(Int32 x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
                    {
                        for(Int32 y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
                        {
                            blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                        }
                    }


                }
            }

            return blurred;
        }
    }
}
