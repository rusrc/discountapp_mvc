using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Discountapp.Domain.Models.Application;
using Discountapp.Infrastructure.Repositories;
using Discountapp.MVC.ViewModels;
using WebGrease.Css.Extensions;
using Discountapp.Domain.Models.Location;
using System.Linq;
using PagedList;
using System.Net;

namespace Discountapp.MVC.Controllers
{
    using Config = Discountapp.Infrastructure.Constants.Config;
    [Authorize]
    public class PromotionController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public PromotionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> Index(long? cityId, DateTime? promotionDate, int? page)
        {
            IEnumerable<PromotionItemViewModel> listItem = null;

            var cityList = await _unitOfWork.Cities.GetAllAsync(); cityList.Insert(0, null);

            if (cityId != null)
            {
                listItem = _unitOfWork.PromotionItems.GetAllByUser(AppUser.Id, cityId.Value).Select(e => (PromotionItemViewModel)e);
            }
            //else if (cityId.HasValue && promotionDate.HasValue)
            //{

            //}
            else
            {
                listItem = _unitOfWork.PromotionItems.GetAllByUser(AppUser.Id).Select(e => (PromotionItemViewModel)e);
            }

            ViewBag.promotionDate = promotionDate;
            ViewBag.selectedCityId = cityId;
            ViewBag.cityId = new SelectList(cityList, nameof(City.Id), nameof(City.Name), cityId ?? 0);
            return View(listItem.ToPagedList(page ?? 1, Config.RowsCount));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int? page, long cityId, DateTime promotionDate)
        {
            //TODO filter
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Add()
        {
            var model = new PromotionViewModel();

            ViewBag.addresses = _unitOfWork.Addresses.GetAllByUser(AppUser.Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(PromotionViewModel model)
        {
            try
            {
                if (!model.Agreement)
                    ModelState.AddModelError(nameof(model.Agreement), "Подтвердите согласие");
                if (model.PromotionItems == null)
                    ModelState.AddModelError(nameof(model.PromotionItems), "Укажите товары");

                if (ModelState.IsValid)
                {
                    var seletetedRealStates = _unitOfWork.RealEstates.GetSelected(AppUser.Id, model.RealEstateSelected.ToArray());
                    var promotion = (Promotion)model;
                    var now = DateTime.Now;

                    promotion.PromotionItems.ForEach(e => e.SaveImage());
                    promotion.PromotionItems.ForEach(e => e.PublishDate = now);
                    promotion.PromotionItems.ForEach(e => e.UpdateDate = now);
                    promotion.RealEstates = seletetedRealStates.ToList();
                    promotion.UserId = AppUser.Id;

                    _unitOfWork.Promotions.Add(promotion);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.addresses = _unitOfWork.Addresses.GetAll();
                ViewBag.categoryId = await _unitOfWork.Categories.GetAllAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                Log.For(typeof(PromotionController)).Error(ex.Message); throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long id)
        {
            //TODO
            throw new NotImplementedException();

            if (id == 0) return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");

            var model = (PromotionViewModel)_unitOfWork.Promotions.Get(id);

            try
            {             
                if (model == null)
                    return HttpNotFound("не удается найти акцию по идентификатору");

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message); return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PromotionViewModel model)
        {

            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Delete(long id)
        {
            var promotion = _unitOfWork.Promotions.Get(id);

            if (promotion == null)
                return HttpNotFound("Не найдено акции");

            return View();
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmedDelete(PromotionViewModel model)
        {
            try
            {
                var promotion = _unitOfWork.Promotions.Get(model.Id);

                if (promotion == null)
                    return HttpNotFound("не удается найти акцию по идентификатору");

                _unitOfWork.Promotions.Remove(promotion);
                _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                //TODO implementation
                throw;
            }
        }

        [HttpPost]
        public ActionResult UploadImage()
        {
            try
            {
                var file = WebImageWrapper.GetImageFromRequest();

                var folderName = $"_{DateTime.Now:yyyyMMdd}_{Guid.NewGuid():N}";
                var info = Directory.CreateDirectory(Path.Combine(this.UploadFolderFullPath, "temp", folderName));

                file.Resize(1920, 1080)
                    .Save(Path.Combine(info.FullName, "0"));

                return Json(new
                {
                    IsError = false,
                    Name = folderName
                });
            }
            catch (Exception ex)
            {
                Log.For(typeof(PromotionController)).Error(ex.Message);
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 500;
                return Json(new { IsError = true, ex.Message });
            }
        }
    }
}
