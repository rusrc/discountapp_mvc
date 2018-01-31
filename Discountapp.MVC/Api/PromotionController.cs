using Discountapp.Domain.Models.Application;
using Discountapp.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using PagedList;

namespace Discountapp.MVC.Api
{
    using Config = Discountapp.Infrastructure.Constants.Config;
    /// <summary>
    /// Работа с акциями
    /// </summary>
    public class PromotionController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public PromotionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Передает список всех акций
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<Promotion>))]
        public IHttpActionResult Get()
        {
            var promotioinList = _unitOfWork.Promotions.GetAll();

            return Ok(promotioinList);
        }

        /// <summary>
        /// Получить акцию
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [ResponseType(typeof(Promotion))]
        public IHttpActionResult Get(long id)
        {

            return Ok(_unitOfWork.Promotions.Get(id));
        }

        /// <summary>
        /// Получить акции
        /// </summary>
        /// <param name="cityId">id города</param>
        /// <param name="categoryId">id катигории магазина</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<Promotion>))]
        public IHttpActionResult Get(long cityId, long? categoryId = null)
        {

            return Ok(_unitOfWork.Promotions.GetAllByCategory(cityId, categoryId));
        }

        /// <summary>
        /// Получить акции
        /// </summary>
        /// <param name="cityId">id города</param>
        /// <param name="merchantId">id магазина</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<Promotion>))]
        public IHttpActionResult Get(long cityId, long merchantId)
        {
            return Ok(_unitOfWork.Promotions.GetAllByMerchant(cityId, merchantId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="page"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        //[ResponseType(typeof(IPagedList<Promotion>))]
        public IHttpActionResult Get(long cityId, int page, long? categoryId = null)
        {
            var result = (PagedList<Promotion>)_unitOfWork
                                            .Promotions
                                            .GetAllByCategory(cityId, categoryId)
                                            .ToPagedList(page, Config.PromotionPageCount);

            return Ok(new
            {
                result.PageNumber,
                result.IsFirstPage,
                result.IsLastPage,
                result.HasNextPage,
                result.HasPreviousPage,
                result.FirstItemOnPage,
                result.LastItemOnPage,
                result.PageCount,
                result.PageSize,
                result.TotalItemCount,
                result.Count,
                Promotions = result
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="merchantId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ResponseType(typeof(IPagedList<Promotion>))]
        public IHttpActionResult Get(long cityId, long merchantId, int page)
        {
            var result = (PagedList<Promotion>)_unitOfWork
                                            .Promotions
                                            .GetAllByMerchant(cityId, merchantId)
                                            .ToPagedList(page, Config.PromotionPageCount);

            return Ok(new
            {
                result.PageNumber,
                result.IsFirstPage,
                result.IsLastPage,
                result.HasNextPage,
                result.HasPreviousPage,
                result.FirstItemOnPage,
                result.LastItemOnPage,
                result.PageCount,
                result.PageSize,
                result.TotalItemCount,
                result.Count,
                Promotions = result
            });
        }

        /// <summary>
        /// Получить акцию через идентификатор пользователя
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<Promotion>))]
        public IHttpActionResult GetByUserId(long userId)
        {
            return Ok(_unitOfWork.Promotions.GetAllByUser(userId));
        }
    }
}