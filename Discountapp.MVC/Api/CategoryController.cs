using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Discountapp.Domain.Models.Application;
using Discountapp.Infrastructure.Repositories;

namespace Discountapp.MVC.Api
{
    public class CategoryController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получает категории
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Category))]
        public IHttpActionResult Get()
        {
            return Ok(this._unitOfWork
                .Categories
                .GetAll()
                .OrderBy(e => e.NameMultiLangJsonObject.Value)
                .Select(e => new
                {
                    e.Id,
                    e.NameMultiLangJsonObject.Value
                }))
               ;
        }
    }
}
