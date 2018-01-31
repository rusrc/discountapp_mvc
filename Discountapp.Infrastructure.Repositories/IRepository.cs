using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Discountapp.Infrastructure.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        TEntity Find(object key);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(object key);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity item);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity item);
        TEntity this[object key] { get; set; }
        void Remove(TEntity item);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}

