using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Discountapp.Domain.Models.Application;
using Discountapp.Infrastructure;
using Discountapp.Infrastructure.Repositories;
using Discountapp.MVC.ViewModels;
using PagedList;

namespace Discountapp.MVC.Controllers
{
    using Config = Discountapp.Infrastructure.Constants.Config;
    [Authorize]
    public class CompanyController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(int? page)
        {
            var list = _unitOfWork.Companies
                .GetAll(c => c.UserId == AppUser.Id)
                .Select(c => (CompanyViewModel)c);

            return View(list.ToPagedList(page ?? 1, Config.RowsCount));
        }

        public async Task<ActionResult> Add()
        {
            var folderName = await AppUser.DeleteThenCreateTempFolderAsync(this.UploadTempFolderFullPath);
            var model = new CompanyViewModel(new DirectoryInfo(folderName).Name);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CompanyViewModel model)
        {

            var resultModel = (Company)model;
            //TODO No message when image is null or empty!!! Need add the message as description of error in resources
            if (string.IsNullOrEmpty(model.TempFolderName))
                ModelState.AddModelError(nameof(model.TempFolderName), "Добавьте логотип");


            //TODO OfferComfirmed находится вне формы при submit
            //if (!model.OfferComfirmed)
            //    ModelState.AddModelError(nameof(model.OfferComfirmed), "Подтвердите правила");

            if (ModelState.IsValid)
            {
                resultModel.UserId = this.AppUser.Id;
                resultModel.LogoFolder = resultModel.GenerateFolder().Name;
                resultModel.TempFolder = model.TempFolderName;
                resultModel.SaveLogoInLogoFolder(new WebImageWrapper(resultModel.GetFullTempLogoFileName()));

                _unitOfWork.Companies.Add(resultModel);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction(nameof(Index), nameof(CompanyController).ControllerName());
            }

            return View(model);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var company = (CompanyViewModel)_unitOfWork.Companies.Get(id);
            if (company == null)
                return HttpNotFound("не удается найти адресс по идентификатору");

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LogoFolder,HotLineNumber,WebSiteLink,ImageFolder")] Company company)
        {
            //TODO
            throw new NotImplementedException();
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index");
            }
            return View(company);
        }

        [HttpGet]
        public ActionResult Delete(long id)
        {
            var company = (CompanyViewModel)_unitOfWork.Companies.Get(id);
            if (company == null)
                return HttpNotFound("не удается найти адресс по идентификатору");

            return View(company);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmedDelete(CompanyViewModel model)
        {
            try
            {
                var company = (CompanyViewModel)_unitOfWork.Companies.Get(model.Id);
                if (company == null)
                    return HttpNotFound("не удается найти компанию по идентификатору");

                _unitOfWork.Companies.Remove(company);
                _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));

            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult UploadLogo()
        {
            try
            {
                var webImage = WebImageWrapper.GetImageFromRequest();
                var tempFolderPath = Path.Combine(this.UploadTempFolderFullPath, Request.Form.Get(nameof(CompanyViewModel.TempFolderName)));

                new Company().SaveTempLogo(webImage, tempFolderPath);

                return Json("Ok");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}
