using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Discountapp.Domain;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public partial class MerchantEntityRepository : IMerchantEntityRepository
    {
        private readonly DiscountappDbContext _context;
        public MerchantEntityRepository(DiscountappDbContext context)
        {
            _context = context;
        }

        public MerchantEntity this[object key]
        {
            get
            {
                return this.Find(key);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                this[key] = value;
            }
        }

        public MerchantEntity Find(object key)
        {
            return GetAll(e => e.Id == (long)key).SingleOrDefault();
        }

        public IEnumerable<MerchantEntity> Find(Expression<Func<MerchantEntity, bool>> predicate)
        {
            return GetAll(predicate);
        }

        public MerchantEntity Get(object key)
        {
            return GetAll(e => e.Id == (long)key).SingleOrDefault();
        }

        public MerchantEntity Get(Expression<Func<MerchantEntity, bool>> predicate)
        {
            return GetAll(predicate).SingleOrDefault();
        }

        public IEnumerable<MerchantEntity> GetAll()
        {
            return GetAll(null).ToList();
        }

        public IEnumerable<MerchantEntity> GetAll(Expression<Func<MerchantEntity, bool>> predicate)
        {
            var query = from rs in _context.RealEstates
                        .Include(r => r.Address)
                        .Include(r => r.Address.City)
                        .Include(r => r.Company)
                        .Include(r => r.MerchantType)
                        .Include(r => r.Promotions)
                            //.Where(predicate)
                        select new MerchantEntity
                        {
                            Id = rs.Id,
                            Name = rs.Company.Name,
                            CompanyId = rs.CompanyId,
                            MerchantTypeId = rs.MerchantTypeId,
                            UserId = rs.UserId,
                            MerchantCategoryId = rs.MerchantCategoryId,
                            HotLineNumber = rs.Company.HotLineNumber,
                            WebSiteLink = rs.Company.WebSiteLink,
                            LogoFolder = rs.Company.LogoFolder,
                            ActiveStatus = rs.ActiveStatus,
                            Company = rs.Company,
                            //User = rs.User,
                            MerchantType = rs.MerchantType,
                            MerchantCategory = rs.MerchantCategory,
                            Promotions = rs.Promotions,
                            ModerationPassed = rs.ModerationPassed,

                            Address = rs.Address,
                            CityId = rs.Address.CityId,
                            MapJsonCoord = rs.Address.MapJsonCoord,
                            Information = rs.Address.Information,
                            Description = rs.Address.Description,
                            WorkTime = rs.Address.WorkTime,
                            WorkTimeSaturday = rs.Address.WorkTimeSaturday,
                            WorkTimeSunday = rs.Address.WorkTimeSunday,
                            City = rs.Address.City
                        };


            return (predicate == null ? query : query.Where(predicate)).ToList();
        }

        public Task<List<MerchantEntity>> GetAllAsync()
        {
            return GetAll().AsQueryable().ToListAsync();
        }

        public Task<List<MerchantEntity>> GetAllAsync(Expression<Func<MerchantEntity, bool>> predicate)
        {
            return GetAll(predicate).AsQueryable().ToListAsync();
        }

        public void Add(MerchantEntity item)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<MerchantEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(MerchantEntity item)
        {
            throw new NotImplementedException();
        }

        public void Remove(MerchantEntity item)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<MerchantEntity> entities)
        {
            throw new NotImplementedException();
        }

        public DiscountappDbContext DiscountAppDbContext => this._context as DiscountappDbContext;
        //public static Expression<Func<TModel, TToProperty>>
        //    Cast<TModel, TFromProperty, TToProperty>
        //    (Expression<Func<TModel, TFromProperty>> expression)
        //{
        //    Expression converted = Expression.Convert(expression.Body, typeof(TToProperty));

        //    return Expression.Lambda<Func<TModel, TToProperty>>(converted, expression.Parameters);
        //}
    }

    public partial class MerchantEntityRepository : IMerchantEntityRepository
    {
        public IEnumerable<MerchantEntity> GetAll(long categoryId, long cityId)
        {
            return this.GetAll(m => m.MerchantCategoryId == categoryId && m.CityId == cityId).ToList();
        }
    }
}
