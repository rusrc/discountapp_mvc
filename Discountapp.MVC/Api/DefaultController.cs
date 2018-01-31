using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Discountapp.Domain;
using Discountapp.Domain.Models.Application;

#pragma warning disable 1591
namespace Discountapp.MVC.Api
{
    public class DefaultController : ApiController
    {
        private readonly DiscountappDbContext _ctx = new DiscountappDbContext();
        //private readonly IUnitOfWork _unitOfWork;
        //public DefaultController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}
        [ResponseType(typeof(Address))]
        public IHttpActionResult Get()
        {
            return Ok(this._ctx.Addresses.ToList());
        }

        // GET: api/Default1/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Default1
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Default1/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default1/5
        public void Delete(int id)
        {
        }
    }
}
