using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Discountapp.Domain;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;
using Discountapp.Infrastructure.Exceptions;
using Discountapp.Infrastructure.Repositories;
using Discountapp.MVC.Api.DTO;
using Discountapp.MVC.Attributes;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Discountapp.MVC.Api
{
    using Config = Discountapp.Infrastructure.Constants.Config;
    //[ApiExplorerSettings(IgnoreApi = true)]

    /// <summary>
    /// Акции
    /// </summary>
    public class PromotionItemController : ApiController
    {
        protected readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DiscountappDbContext _ctx = new DiscountappDbContext();

        private readonly IUnitOfWork _unitOfWork;
        public PromotionItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private long UserId => User.Identity.GetUserId<long>();

        /// <summary>
        /// get all with include of promotions
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IHttpActionResult Get(int? page, int? count)
        {
            page = page ?? 1;

            var result = (PagedList<PromotionItemNoChildrenDto>)
                _unitOfWork
                    .PromotionItems.GetAllWithPromotions()
                    .Select(p => new PromotionItemNoChildrenDto(p))
                    .ToPagedList((int)page, count ?? Config.PromotionItemPageCount);

            return Ok(new
            {
                result.PageNumber,
                result.IsFirstPage,
                result.IsLastPage,
                result.HasNextPage,
                result.HasPreviousPage,
                result.FirstItemOnPage,
                result.LastItemOnPage,
                result.PageCount,
                result.PageSize,
                result.TotalItemCount,
                result.Count,
                Promotions = result
            });
        }

        [ResponseType(typeof(PromotionItem))]
        public IHttpActionResult Get(long id)
        {

            return Ok(_unitOfWork.PromotionItems.Get(id));
        }

        /// <summary>
        /// Получить товары по городу и категории Продавца (Merchant)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="page"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IPagedList<PromotionItem>))]
        public IHttpActionResult Get(long cityId, int page, long? categoryId = null)
        {
            var result = (PagedList<PromotionItemNoChildrenDto>)
                                _unitOfWork
                                .PromotionItems
                                .GetAllByCategory(cityId, categoryId)
                                .Select(p => new PromotionItemNoChildrenDto(p))
                                .ToPagedList(page, Config.PromotionItemPageCount);

            return Ok(new
            {
                result.PageNumber,
                result.IsFirstPage,
                result.IsLastPage,
                result.HasNextPage,
                result.HasPreviousPage,
                result.FirstItemOnPage,
                result.LastItemOnPage,
                result.PageCount,
                result.PageSize,
                result.TotalItemCount,
                result.Count,
                Promotions = result
            });
        }

        /// <summary>
        /// Получить товары по городу и категории Продавца (Merchant)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="merchantId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ResponseType(typeof(IPagedList<PromotionItem>))]
        public IHttpActionResult Get(long cityId, long merchantId, int page)
        {
            var result = (PagedList<PromotionItemNoChildrenDto>)
                                _unitOfWork
                                .PromotionItems
                                .GetAllByMerchant(cityId, merchantId)
                                .Select(p => new PromotionItemNoChildrenDto(p))
                                .ToPagedList(page, Config.PromotionItemPageCount);

            return Ok(new
            {
                result.PageNumber,
                result.IsFirstPage,
                result.IsLastPage,
                result.HasNextPage,
                result.HasPreviousPage,
                result.FirstItemOnPage,
                result.LastItemOnPage,
                result.PageCount,
                result.PageSize,
                result.TotalItemCount,
                result.Count,
                Promotions = result
            });
        }

        //https://www.codeproject.com/articles/1005485/restful-day-sharp-security-in-web-apis-basic
        /// <summary>
        /// Добавляет лайк, требует авторизации
        /// </summary>
        /// <returns></returns>
        [ApiAuthorization]
        [Route("api/like")]
        //[ResponseType(typeof(PromotionItem))]
        public async Task<IHttpActionResult> GetLike(long id)
        {
            try
            {
                PromotionItem item = _ctx.PromotionItems.Find(id);
                if (item == null)
                    throw new BusinessLogicException("не удается найти акцию");

                if (item.GetUserLike(UserId)?.Value == LikeType.Like)
                    return Ok(new { item.LikeCount, item.DislikeCount }); ;


                if (item.GetUserLike(UserId)?.Value == LikeType.Dislike)
                {
                    Like like = item.GetUserLike(UserId);
                    like.Value = LikeType.Like;

                    item.DislikeCount = item.DislikeCount - 1;
                    item.LikeCount = item.LikeCount + 1;
                }

                if (item.GetUserLike(UserId) == null)
                {
                    _ctx.Likes.Add(new Like(UserId, item.Id, LikeType.Like));
                    item.LikeCount = item.LikeCount + 1;
                }

                /*
                The INSERT statement conflicted with the FOREIGN KEY constraint "FK_dbo.Like_dbo.User_UserId". The conflict occurred in database "tukiflydatabase", table "dbo.User", column 'Id'.
                The statement has been terminated.
                 */
                if (_ctx.ChangeTracker.HasChanges()) await _ctx.SaveChangesAsync();

                return Ok(new { item.LikeCount, item.DislikeCount }); ;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message); throw;
            }
        }

        /// <summary>
        /// Добавляет дислайк, требует авторизации
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("api/dislike")]
        //[ResponseType(typeof(PromotionItem))]
        public async Task<IHttpActionResult> GetDislike(long id)
        {
            try
            {
                PromotionItem item = _ctx.PromotionItems.Find(id);
                if (item == null)
                    throw new BusinessLogicException("не удается найти акцию");

                if (item.GetUserLike(UserId)?.Value == LikeType.Dislike)
                    return Ok(new { item.LikeCount, item.DislikeCount });


                if (item.GetUserLike(UserId)?.Value == LikeType.Like)
                {
                    Like like = item.GetUserLike(UserId);
                    like.Value = LikeType.Dislike;

                    item.LikeCount = item.LikeCount - 1;
                    item.DislikeCount = item.DislikeCount + 1;
                }

                if (item.GetUserLike(UserId) == null)
                {
                    _ctx.Likes.Add(new Like(UserId, item.Id, LikeType.Dislike));
                    item.DislikeCount = item.DislikeCount + 1;
                }

                if (_ctx.ChangeTracker.HasChanges()) await _ctx.SaveChangesAsync();

                return Ok(new { item.LikeCount, item.DislikeCount });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message); throw;
            }
        }
    }
}