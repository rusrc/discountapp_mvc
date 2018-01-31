using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Discountapp.Domain.Models.Application;
using Discountapp.Infrastructure.Repositories;
using Discountapp.MVC.Api.DTO;
using System.Linq;

namespace Discountapp.MVC.Api
{
    /// <summary>
    /// Продавцы
    /// </summary>
    public class MerchantController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public MerchantController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получить товары
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [ResponseType(typeof(IEnumerable<MerchantEntityNoPromotionsDto>))]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.MerchantEntities.GetAll(null).Select(m => new MerchantEntityNoPromotionsDto(m)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Получить товары по id
        /// </summary>
        /// <param name="id">id Товара</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [ResponseType(typeof(MerchantEntity))]
        public IHttpActionResult Get(long id)
        {
            try
            {
                return Ok(_unitOfWork.MerchantEntities.Get(id));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Передаем продавцов по категории и городу
        /// </summary>
        /// <param name="categoryId">id категории</param>
        /// <param name="cityId">id города</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<MerchantEntityNoPromotionsDto>))]
        public IHttpActionResult Get(long categoryId, long cityId)
        {
            try
            {
                return Ok
                    (
                        _unitOfWork
                        .MerchantEntities
                        .GetAll(categoryId, cityId)
                        .Select(m => new MerchantEntityNoPromotionsDto(m))          
                    );
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Получает товары по id города
        /// </summary>
        /// <param name="id">id города. Если не указан возврощает все маганы и юр. лица</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [Route("api/Merchant/GetAllByCityId/{id:long=0}")]
        [ResponseType(typeof(IEnumerable<MerchantEntityNoPromotionsDto>))]
        public IHttpActionResult GetAllByCityId(long? id)
        {
            try
            {
                IEnumerable<MerchantEntity> list = null;
                if (id == null || id == 0)
                    list = _unitOfWork.MerchantEntities.GetAll();
                else
                    list = _unitOfWork.MerchantEntities.GetAll(m => m.CityId == id);

                return Ok(list.Select(e => new MerchantEntityNoPromotionsDto(e)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}