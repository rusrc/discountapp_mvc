using System.Web.Mvc;
using Discountapp.Domain.Models.Application;
using Discountapp.Infrastructure.Repositories;
using Discountapp.MVC.ViewModels;
using System.Linq;
using Discountapp.Domain.Models.Location;
using PagedList;

namespace Discountapp.MVC.Controllers
{
    using Config = Discountapp.Infrastructure.Constants.Config;
    [Authorize]
    public class MerchantController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public MerchantController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(int? page)
        {
            var list = _unitOfWork.MerchantEntities
                .GetAll(m => m.UserId == AppUser.Id)
                .Select(m => (MerchantEntityViewModel)m)
                .ToList();
                //.Select(e => (MerchantEntityViewModel)e);

            return View(list.ToPagedList(page ?? 1, Config.RowsCount));
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.merchantCategoryId = new SelectList(_unitOfWork.MerchantCategory.GetAll(), nameof(MerchantCategory.Id), nameof(MerchantCategory.Name));
            ViewBag.companyId = new SelectList(_unitOfWork.Companies.GetAll(c => c.UserId == AppUser.Id), nameof(Company.Id), nameof(Company.Name));
            ViewBag.cityId = new SelectList(_unitOfWork.Cities.GetOrderedCities(), nameof(City.Id), nameof(City.Name));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MerchantEntityViewModel model)
        {
            //TODO "The parameter conversion from type 'System.String' to type 'Discountapp.Domain.Models.Location.City' failed because no type converter can convert between these types."
            if (ModelState.ContainsKey("City")) ModelState["City"].Errors.Clear();

            if (ModelState.IsValid)
            {
                var merchantEntity = (MerchantEntity)model;
                var realEstate = (RealEstate)merchantEntity.ToRealEstate();
                var address = (Address)merchantEntity.ToAddress();

                realEstate.UserId = AppUser.Id;
                _unitOfWork.RealEstates.Add(realEstate);

                address.AddressId = realEstate.Id;
                _unitOfWork.Addresses.Add(address);

                _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }


            ViewBag.merchantCategoryId = new SelectList(_unitOfWork.MerchantCategory.GetAll(), nameof(MerchantCategory.Id), nameof(MerchantCategory.Name));
            ViewBag.companyId = new SelectList(_unitOfWork.Companies.GetAll(c => c.UserId == AppUser.Id), nameof(Company.Id), nameof(Company.Name));
            ViewBag.cityId = new SelectList(_unitOfWork.Cities.GetOrderedCities(), nameof(City.Id), nameof(City.Name));

            return View(model);
        }
    }
}