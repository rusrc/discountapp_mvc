using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Discountapp.Infrastructure.ImageLibrary.Json
{
    public class ImgJson
    {
        /// <summary>
        /// Количество добавленных фотографий, обновленые и удаленные фотографии не считаются. |
        /// The quantity of added imgs during ad existance. Usually keep last img index.
        /// Deleting and updating not calculated
        /// </summary>
        public int CountHistory { get; set; }

        /// <summary>
        /// Коллекция объектов типа ImgJsonOrder
        /// Collection of ImgJsonOrder objects
        /// </summary>
        public List<ImgJsonOrder> ImgJsonOrderList { get; set; }

        /// <summary>
        /// Задает новый порядок начиная с нуля если есть элементы в списке ImgJsonOrderList
        /// Make new order of images if exsits
        /// </summary>
        /// <returns>ImgJson</returns>
        public ImgJson MakeNewOrderInImgJsonOrderList()
        {

            if (this.ImgJsonOrderList.Count > 0)
            {
                for (int i = 0; i < this.ImgJsonOrderList.Count; i++)
                {
                    this.ImgJsonOrderList[i].Order = i;
                }
            }

            return this;
        }

        /// <summary>
        /// Создает имя файла на основе текущего иднекса файла
        /// </summary>
        /// <param name="curImgIndex">Текущая итерация в цикле, которая соответсвует файлу</param>
        /// <returns>Имя файла в виде цифры строкового типа</returns>
        public int CreateIndexName(int curImgIndex)
        {
            var maxIndexName = this.GetMaxIndexNameInList();

            if (maxIndexName == 0 && curImgIndex == 0)
                return 0;
            if (maxIndexName > 0 && curImgIndex == 0)
                return maxIndexName + 1;


            return maxIndexName + 1;
        }

        /// <summary>
        /// Получает последний индекс в названии файла или номер самой последней фотографии
        /// </summary>
        /// <returns>int</returns>
        public int GetMaxIndexNameInList()
        {
            //If list null
            if (this.ImgJsonOrderList == null)
                return 0;

            if (this.ImgJsonOrderList.Count > 0)
            {
                int maxIndexName = this.ImgJsonOrderList.Max(e => e.IndexName);

                if (maxIndexName > this.CountHistory)
                    return maxIndexName;

                if (this.CountHistory > maxIndexName)
                    return this.CountHistory;

                return maxIndexName;
            }

            if (this.ImgJsonOrderList.Count == 0)
            {
                return this.CountHistory;
            }

            return 0;
        }

        /// <summary>
        /// Конвертирует ImgJson как страку json в ImgJson объект   |
        /// Convert ImgJson as json string into ImgJson object      |
        /// </summary>
        /// <param name="imgJsonString">Json string like: "{"CountHistory":1,"ImgJsonOrderList":[{"Order":0,"IndexName":0}]}"</param>
        /// <returns>ImgJson</returns>
        public static ImgJson Parse(string imgJsonString)
        {
            var defaultVal = new ImgJson
            {
                CountHistory = 0,
                ImgJsonOrderList = new List<ImgJsonOrder> { null }
            };

            if (string.IsNullOrEmpty(imgJsonString)) return defaultVal;

            try
            {
                return JsonConvert.DeserializeObject<ImgJson>(imgJsonString);
            }
            catch
            {
                return defaultVal;
            }

        }

        public override string ToString()
        {
            var result = JsonConvert.SerializeObject(this);

            return result;
        }
    }
}
