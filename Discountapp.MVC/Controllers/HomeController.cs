using System.Web.Mvc;
using Discountapp.Infrastructure.Repositories;
using System.Linq;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;
using Discountapp.MVC.ViewModels;
using PagedList;
using System;

namespace Discountapp.MVC.Controllers
{
    using Config = Discountapp.Infrastructure.Constants.Config;
    public class HomeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(PromotionTimeType? promotionTimeSelect, PromotionFilterType? promotionFilter, int? page)
        {
            var cityName = _unitOfWork.Cities?.Get(e => e.Id == CityId)?.NameMultiLangJsonObject?.Value ?? "Все города";
            //cityName = Nouns.FindSimilar(cityName)[Case.Locative];

            var list = _unitOfWork
                        .PromotionItems
                        .GetAllByCityId(CityId)
                        .OrderBy(e => e.Promotion.Begin)
                        .Select(e => (PromotionItemViewModel)e);

            //Filter by promotionTimeSelect
            switch (promotionTimeSelect)
            {
                case PromotionTimeType.CurrentAndFuture:
                    list = list.Where(e => e.Promotion.Begin == DateTime.Now && e.Promotion.Begin > DateTime.Now);
                    break;
                case PromotionTimeType.Yestoday:
                    list = list.Where(e => e.Promotion.Begin == DateTime.Now.AddDays(-1));
                    break;
                case PromotionTimeType.Today:
                    list = list.Where(e => e.Promotion.Begin == DateTime.Now);
                    break;
                case PromotionTimeType.Tomorrow:
                    list = list.Where(e => e.Promotion.Begin == DateTime.Now.AddDays(1));
                    break;
            }

            //Filter by promotionFilter
            switch (promotionFilter)
            {
                case PromotionFilterType.HighRate:
                    list = list.OrderByDescending(e => e.LikeCount);
                    break;
                case PromotionFilterType.LowRate:
                    list = list.OrderByDescending(e => e.DislikeCount);
                    break;
            }


            ViewBag.Title = CityId > 0 ? $"Акции и скидки супермаркетов в городе {cityName}" : "Акции и скидки во всех городах Казахстана";
            ViewBag.promotionTimeSelect = new SelectList(PromotionTime.GenerateCollection(), nameof(PromotionTime.Type), nameof(PromotionTime.Name));
            ViewBag.promotionFilter = PromotionFilterType.NewOnly;
            return View(list.ToPagedList(page ?? 1, Config.RowsCountIndex));
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}