using Discountapp.Domain.Models.Application;
using Discountapp.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Discountapp.MVC.Api
{
    /// <summary>
    /// Работа с товарами
    /// </summary>
    public class PromotionItem2Controller : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public PromotionItem2Controller(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получить товары
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PromotionItem))]
        public IHttpActionResult Get()
        {
            return Ok(_unitOfWork.PromotionItems.GetAll());
        }

        /// <summary>
        /// Получить товары по id
        /// </summary>
        /// <param name="id">id Товара</param>
        /// <returns></returns>
        [ResponseType(typeof(PromotionItem))]
        public IHttpActionResult Get(long id)
        {
            return Ok(_unitOfWork.PromotionItems.Get(id));
        }

        [ResponseType(typeof(IEnumerable<PromotionItem>))]
        public IHttpActionResult Get(long categoryId, long cityId)
        {
            return Ok(_unitOfWork.PromotionItems.GetAll(categoryId, cityId));
        }

        /// <summary>
        /// Получает товары по id города
        /// </summary>
        /// <param name="id">id города</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/PromotionItem2/GetAllByCityId")]
        [ResponseType(typeof(IEnumerable<PromotionItem>))]
        public IHttpActionResult GetAllByCityId(long? id)
        {
            return Ok(_unitOfWork.PromotionItems.GetAllByCityId(id ?? 0));
        }

        /// <summary>
        /// Получает товары по id пользователя
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<PromotionItem>))]
        public IHttpActionResult GetAllByUser(long userId)
        {
            return Ok(_unitOfWork.PromotionItems.GetAllByUser(userId));
        }

        /// <summary>
        /// Получает товары по id пользователя и id города
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <param name="cityId">id города</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<PromotionItem>))]
        public IHttpActionResult GetAllByUser(long userId, long cityId)
        {
            return Ok(_unitOfWork.PromotionItems.GetAllByUser(userId, cityId));
        }
    }
}