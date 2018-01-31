using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Arrba.ImageLibrary.ModelViews
{
    public class ImgJson
    {
        public int CountHistory { get; set; }
        public List<ImgJsonOrder> ImgJsonOrderList { get; set; }


        /// <summary>
        /// Задает новый порядок начиная с нуля если есть элементы в списке ImgJsonOrderList
        /// </summary>
        /// <returns></returns>
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
        /// Получает последний индекс в названии файла или номер самой последней фотографии
        /// </summary>
        /// <returns></returns>
        public int GetMaxIndexInList()
        {
            var ImgJsonOrderList = this.ImgJsonOrderList;
            var _maxIndexInList = (ImgJsonOrderList.Count > 0 && ImgJsonOrderList.Max(e => e.IndexName) > 0)
                    ? ImgJsonOrderList.Max(e => e.IndexName)
                    : (ImgJsonOrderList.Count > 0 && ImgJsonOrderList.Max(e => e.IndexName) == 0)
                    ? 1
                    : 0;



            if (CountHistory == 0 && _maxIndexInList == 0) return 0;

            if (_maxIndexInList == 0 && CountHistory > 0) return CountHistory;

            if (CountHistory > _maxIndexInList) return CountHistory;




            return _maxIndexInList;

        }

    }
}
