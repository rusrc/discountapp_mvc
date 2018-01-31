using Discountapp.Domain.Models.Location;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Discountapp.Infrastructure.Repositories;

namespace Discountapp.MVC.Api
{
    public class CitiesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public CitiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [ResponseType(typeof(City))]
        public IHttpActionResult Get()
        {
            return Ok(_unitOfWork.Cities.GetAll().OrderBy(c => c.NameMultiLangJsonObject.Value).ToList());
        }

        [ResponseType(typeof(City))]
        public IHttpActionResult Get(long id)
        {
            return Ok(_unitOfWork.Cities.Find(id));
        }
    }
}