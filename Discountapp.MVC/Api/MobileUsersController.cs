using Discountapp.Domain.Models.Application;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Discountapp.Infrastructure.Exceptions;
using Discountapp.Infrastructure.Repositories;

namespace Discountapp.MVC.Api
{
    /// <summary>
    /// Пользователь для мобильных приложений
    /// </summary>
    public class MobileUsersController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public MobileUsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Пользователь
        /// </summary>
        /// <param name="mobileUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]MobileUser mobileUser)
        {
            //AA-BBBBBB-CCCCCC-D или АА-BBBBBB-CCCCCC-EE
            if (!Regex.IsMatch(mobileUser.DeviceImei, "^[0-9]{15}(,[0-9]{15})*$"))
                throw new BusinessLogicException("Неверный формат международного идентификатора мобильного оборудования");

            var mobileUserFromDb = _unitOfWork.MobileUsers.GetByDeviceImei(mobileUser.DeviceImei);
            if (mobileUserFromDb == null)
            {
                try
                {
                    _unitOfWork.MobileUsers.Add(mobileUser);
                    await _unitOfWork.CompleteAsync();
                    return Ok(mobileUser);
                }
                catch (Exception ex)
                {
                    return Ok(ex);
                }
            }
            //TODO проверить телефон
            if (!mobileUserFromDb.PhoneNumber.Equals(mobileUser.PhoneNumber))
            {
                throw new NotImplementedException();
            }

            return Ok(mobileUserFromDb);
        }

        public IHttpActionResult Get()
        {
            return Ok(new MobileUser { UserId = 0, DeviceImei = "35-209900-176148-1", PhoneNumber = "90385844455" });
        }
    }
}