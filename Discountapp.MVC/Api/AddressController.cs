using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Discountapp.Domain.Models.Application;
using Discountapp.Infrastructure.Repositories;
using Discountapp.MVC.Api.DTO;

namespace Discountapp.MVC.Api
{
    public class AddressController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [ResponseType(typeof(IEnumerable<Address>))]
        public IHttpActionResult Get()
        {
            return Ok(_unitOfWork.Addresses.GetAll());
        }

        [ResponseType(typeof(Address))]
        public IHttpActionResult Get(long id)
        {
            return Ok(_unitOfWork.Addresses.Get(id));
        }

        /// <summary>
        /// Получить адреса которые в которых действует акция
        /// </summary>
        /// <param name="cityId">Город магазина</param>
        /// <param name="promotionId">Акция</param>
        /// <param name="merchantId">Магазина, которому принадлежит акция</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<Address>))]
        public IHttpActionResult Get(long cityId, long promotionId, long merchantId)
        {
            var addresses = _unitOfWork
                            .Addresses
                            .GetAllByPromotion(cityId, promotionId, merchantId)
                            .Select(a => new AddressDto(a));

            return Ok(addresses);
        }
    }
}