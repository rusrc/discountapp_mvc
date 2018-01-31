using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Discountapp.Domain;
using Discountapp.Domain.Models;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DiscountappDbContext _context;

        public Repository(DiscountappDbContext context)
        {
            this._context = context;
        }
        public virtual TEntity this[object key]
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

        public virtual void Add(TEntity item)
        {
            this._context.Set<TEntity>().Add(item);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            this._context.Set<TEntity>().AddRange(entities);
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this._context.Set<TEntity>().Where(predicate);
        }

        public virtual TEntity Find(object key)
        {
            return this._context.Set<TEntity>().Find(key);
        }

        public virtual TEntity Get(object key)
        {
            return this._context.Set<TEntity>().Find(key);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate) 
            => _context.Set<TEntity>().FirstOrDefault(predicate);

        public virtual IEnumerable<TEntity> GetAll()
        {
            return this._context.Set<TEntity>().ToList();
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return this._context.Set<TEntity>().Where(predicate).ToList();
        }

        public virtual Task<List<TEntity>> GetAllAsync()
        {
            return  _context.Set<TEntity>().ToListAsync();
        }

        public virtual Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual void Update(TEntity item)
        {
            this._context.Entry<TEntity>(item).State = EntityState.Modified;
        }

        public virtual void Remove(TEntity item)
        {
            this._context.Set<TEntity>().Remove(item);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            this._context.Set<TEntity>().RemoveRange(entities);
        }
    }
}
