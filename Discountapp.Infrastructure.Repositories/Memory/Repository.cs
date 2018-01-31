using Discountapp.Domain;
using Discountapp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Discountapp.Infrastructure.Repositories.Memory
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IIdentifiable
    {
        protected readonly DiscountappMemoryContext _context;
        public Repository(DiscountappMemoryContext context)
        {
            this._context = context;
        }
        public TEntity this[object key]
        {
            get
            {
                return Find(key);
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException();
                this[key] = value;
            }
        }

        public void Add(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public TEntity Find(object key)
        {
            return _context.GetAll<TEntity>().SingleOrDefault(e => e.Id == (long)key);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.GetAll<TEntity>().Where(predicate.Compile());
        }

        public TEntity Get(object key)
        {
            return _context.GetAll<TEntity>().SingleOrDefault(e => e.Id == (long)key);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.GetAll<TEntity>().SingleOrDefault(predicate.Compile());
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.GetAll<TEntity>();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.GetAll<TEntity>().Where(predicate.Compile());
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return Task.Factory.StartNew(() => _context.GetAll<TEntity>().ToList());
        }

        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.Factory.StartNew(() => _context.GetAll<TEntity>().Where(predicate.Compile()).ToList());
        }

        public void Remove(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity item)
        {
            throw new NotImplementedException();
        }
    }
}
