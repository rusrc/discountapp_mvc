using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Discountapp.Infrastructure.ImageLibrary.Json;
using Discountapp.Infrastructure.ImageLibrary.ModelViews;

namespace Discountapp.Infrastructure.ImageLibrary
{

    public partial class ImgFolderManager
    {
        private readonly HttpResponseBase _response;
        private readonly HttpRequestBase _request;
        private readonly string _cookieFolderName;
        private readonly string _adImagesTmpFolder;

        /// <summary>
        /// Конструктор, чтобы задать объекты Response и Request
        /// </summary>
        /// <param name="response"></param>
        /// <param name="request"></param>
        /// <param name="cookieFolderName"></param>
        /// <param name="adImagesTmpFolder"></param>
        public ImgFolderManager(HttpResponseBase response,
            HttpRequestBase request,
            string cookieFolderName,
            string adImagesTmpFolder)
        {
            _response = response;
            _request = request;
            _cookieFolderName = cookieFolderName;
            _adImagesTmpFolder = adImagesTmpFolder;
        }

        public string TmpFolderName
        {
            get
            {
                var cookie = _request.Cookies.Get(_cookieFolderName);
                if (cookie != null)
                {
                    return cookie.Value;
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// Получает номер в название файла
        /// </summary>
        /// <param name="str">Название файла</param>
        /// <returns>Число</returns>
        public static string GetImgNumber(string str)
        {
            var reg = new Regex("^\\d+", RegexOptions.IgnoreCase);

            return reg.Match(str).Value;
        }


        /// <summary>
        /// Получает название файлов из списка объектов ImgJsonOrder преобразуя в список объектов типа AdImgModelView 
        /// </summary>
        /// <param name="jsonList">Список сортер полученный из json</param>
        /// <returns>список объектов типа AdImgModelView</returns>
        public static List<AdImgModelView> GetImages(List<ImgJsonOrder> jsonList)
        {
            //1024x720
            var adImgList = new List<AdImgModelView>();

            jsonList.ForEach(e =>
            {
                var indexName = e.IndexName.ToString();
                adImgList.Add(new AdImgModelView
                {
                    FullFileName = string.Concat(indexName, ImgManager.FULL_FILE_NAME_PREFIX, ".jpg"),
                    MiddleFileName = string.Concat(indexName, ImgManager.MIDDLE_FILE_NAME_PREFIX, ".jpg"),
                    SmallFileName = string.Concat(indexName, ImgManager.SMALL_FILE_NAME_PREFIX, ".jpg"),
                    SuperSmallFileName = string.Concat(indexName, ImgManager.SUPER_SMALL_FILE_NAME_PREFIX, ".jpg"),
                    ImageStatus = e.Order == 0 ? AdImageStatus.Main : AdImageStatus.NotMain
                });
            });


            return adImgList;

        }

        /// <summary>
        /// Получает название файлов из json строки JsonList преобразуя в список объектов типа AdImgModelView 
        /// </summary>
        /// <param name="jsonList">json строка</param>
        /// <returns></returns>
        public static List<AdImgModelView> GetImages(string jsonList)
        {
            if (jsonList == null)
            {
                throw new ArgumentNullException($"Hey! {nameof(jsonList)} must be not null");
            }

            ImgJson jsonResult = ImgJson.Parse(jsonList);

            return GetImages(jsonResult.ImgJsonOrderList);
        }

        /// <summary>
        /// Получает название файлов из указанной папки преобразуя в список объектов типа AdImgModelView 
        /// </summary>
        /// <param name="rootFolder">json строка</param>
        /// <param name="folderImgName">json строка</param>
        /// <returns></returns>
        public static List<AdImgModelView> GetImages(string rootFolder, string folderImgName)
        {
            var fileFolderName = Path.Combine(rootFolder, folderImgName);

            if (Directory.Exists(fileFolderName))
            {
                List<string> imgFiles = GetFilesName(Directory.GetFiles(fileFolderName));
                List<string> fullImgs = imgFiles.Where(e => e.Contains(ImgManager.FULL_FILE_NAME_PREFIX)).ToList<string>();
                var adImgList = new List<AdImgModelView>();

                fullImgs.ForEach(img =>
                {
                    var name = new Regex("^[0-9]+", RegexOptions.IgnoreCase).Match(img).Value;

                    adImgList.Add(new AdImgModelView
                    {
                        FullFileName = GetFileName(imgFiles, name, ImgManager.FULL_FILE_NAME_PREFIX), // or if full just "img"
                        MiddleFileName = GetFileName(imgFiles, name, ImgManager.MIDDLE_FILE_NAME_PREFIX),
                        SmallFileName = GetFileName(imgFiles, name, ImgManager.SMALL_FILE_NAME_PREFIX),
                        SuperSmallFileName = GetFileName(imgFiles, name, ImgManager.SUPER_SMALL_FILE_NAME_PREFIX),
                        ImageStatus =
                            new Regex("^0{1}", RegexOptions.IgnoreCase).IsMatch(img)
                                ? AdImageStatus.Main
                                : AdImageStatus.NotMain,
                    });
                });

                return adImgList;
            }

            return new List<AdImgModelView> { null };

        }


        /// <summary>
        /// Получает имена файлов без пути или с путями
        /// </summary>
        /// <param name="fullPaths"></param>
        /// <param name="thatMatchWith">взять файлы соответсвующие регулярному выражению
        /// елси значение Null, то берет все файлы</param>
        /// <returns>List<string/></returns>
        public static List<string> GetFilesName(string[] fullPaths, Regex thatMatchWith = null)
        {
            List<string> result = new List<string>();

            if (thatMatchWith != null)
            {
                fullPaths.ToList().ForEach(e =>
                {
                    if (thatMatchWith.Match(e).Success)
                    {
                        result.Add(Path.GetFileName(e));
                    }
                });
            }
            else
            {
                fullPaths.ToList().ForEach(e => result.Add(Path.GetFileName(e)));
            }


            return result;
        }


        /// <summary>
        /// Set tmp folder name to response
        /// </summary>
        /// <param name="createFolderToo">If creation of folder needed</param>
        /// <returns></returns>
        public string SetResponseToTmpCookieFolder(bool createFolderToo = false)
        {
            var cookie = _request.Cookies.Get(_cookieFolderName);
            var cookieValue = $"{Guid.NewGuid():N}.{ DateTime.Now:yyyyMMdHHmmss}";
            if (cookie == null)
            {
                _response.Cookies.Add(new HttpCookie(_cookieFolderName)
                {
                    Value = cookieValue,
                    Expires = DateTime.Now.AddMinutes(30)
                });

                if (createFolderToo)
                {
                    var path = Path.Combine(_adImagesTmpFolder, cookieValue);
                    if (!Directory.Exists(path))
                    {
                        return Directory.CreateDirectory(path).Name;
                    }
                }
            }
            return cookieValue;
        }


        #region Helpers
        /// <summary>
        /// Получает имя файла из списка ImgFiles, где имя начинается с FileNameBase + AdditionalExtenstion
        /// </summary>
        /// <param name="imgFiles"></param>
        /// <param name="fileNameBase"></param>
        /// <param name="additionalExtenstion"></param>
        /// <returns>Возврощает имя файла вида FileNameBaseAdditionalExtenstion (e.g. 0-full)</returns>
        private static string GetFileName(List<string> imgFiles, string fileNameBase, string additionalExtenstion)
        {
            if (imgFiles.Any(e => e.Contains(fileNameBase + additionalExtenstion)))
            {
                return imgFiles.SingleOrDefault(e => e.StartsWith(fileNameBase + additionalExtenstion));
            }

            return string.Empty;
        }
        #endregion

    }


}
