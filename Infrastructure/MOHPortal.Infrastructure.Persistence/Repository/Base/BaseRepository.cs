using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MOHPortal.Core.Contracts.IRepository.Base;
using MOHPortal.Core.Domain.Enums;
using MOHPortal.Infrastructure.Persistence.DbContext;

namespace MOHPortal.Infrastructure.Persistence.Repository.Base
{
    internal abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        #region Vars
        public Lazy<AppDbContext> AppDbContext { get; }
        #endregion

        #region INI
        internal BaseRepository(Lazy<AppDbContext> appDbContext)
        {
            AppDbContext = appDbContext;
        }
        #endregion

        #region Get Page
        public async Task<List<T>> GetPageAsync<TKey>(int skipCount, int takeCount, Expression<Func<T, bool>> filter,
            Expression<Func<T, TKey>> sortingExpression, SortDirectionEnum sortDir = SortDirectionEnum.Ascending,
            string includeProperties = "", Expression<Func<T, TKey>>? sortingExpressionSecond = null)
        {
            IQueryable<T> query = AppDbContext.Value.Set<T>();

            if (filter != null) 
                query = query.Where(filter);

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            skipCount = skipCount * takeCount;
            switch (sortDir)
            {
                case SortDirectionEnum.Ascending:
                    query = sortingExpressionSecond != null
                        ? query.OrderBy(sortingExpression).ThenByDescending(sortingExpressionSecond).Skip(skipCount).Take(takeCount)
                        : query.OrderBy(sortingExpression).Skip(skipCount).Take(takeCount);
                    break;
                case SortDirectionEnum.Descending:
                    query = sortingExpressionSecond != null
                        ? query.OrderByDescending(sortingExpression).ThenByDescending(sortingExpressionSecond).Skip(skipCount).Take(takeCount)
                        : query.OrderByDescending(sortingExpression).Skip(skipCount).Take(takeCount);
                    break;
            }
            return await query.ToListAsync();

        }
        #endregion

        #region GetAll

        public async Task<List<T>> GetAllAsync() => await AppDbContext.Value.Set<T>().ToListAsync();

        #endregion

        #region First Or Default
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = AppDbContext.Value.Set<T>();

            if (filter != null) query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Value.Set<T>();

            if (filter != null) 
                query = query.Where(filter);

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> FirstOrDefaultAsync<TKey>(Expression<Func<T, bool>> filter,
            Expression<Func<T, TKey>> sortingExpression, SortDirectionEnum sortDir = SortDirectionEnum.Ascending,
            string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Value.Set<T>();

            if (filter != null) 
                query = query.Where(filter);

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            
            switch (sortDir)
            {
                case SortDirectionEnum.Ascending:
                    query = query.OrderBy(sortingExpression);
                    break;
                case SortDirectionEnum.Descending:
                    query = query.OrderByDescending(sortingExpression);
                    break;
            }
            return await query.FirstOrDefaultAsync();

        }
        #endregion

        #region Get Where
        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = AppDbContext.Value.Set<T>();

            if (filter != null)
                query = query.Where(filter);
            
            return await query.ToListAsync();
        }

        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Value.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync();
        }
        public async Task<List<T>> GetWhereAsync<TKey>(Expression<Func<T, bool>> filter,
            Expression<Func<T, TKey>> sortingExpression, SortDirectionEnum sortDir = SortDirectionEnum.Ascending,
            string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Value.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            query = sortDir switch
            {
                SortDirectionEnum.Ascending => query.OrderBy(sortingExpression),
                SortDirectionEnum.Descending => query.OrderByDescending(sortingExpression),
                _ => includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty))
            };

            return await query.ToListAsync();
        }
        #endregion

        #region Get Any
        public async Task<bool> GetAnyAsync(Expression<Func<T, bool>> filter = null)
        {
            return await AppDbContext.Value.Set<T>().AnyAsync(filter);
        }
        #endregion

        #region Get Count
        public async Task<int> GetCountAsync()
        {
            return await AppDbContext.Value.Set<T>().CountAsync();
        }
        public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter)
        {
            return await AppDbContext.Value.Set<T>().Where(filter).CountAsync();
        }
        #endregion

        #region Create 
        public void CreateAsyn(T entity)
        {
            AppDbContext.Value.Set<T>().AddAsync(entity);
        }
        public void CreateListAsyn(List<T> entityList)
        {
            AppDbContext.Value.Set<T>().AddRangeAsync(entityList);
        }
        #endregion

        #region Update
        public void Update(T entity)
        {
            AppDbContext.Value.Set<T>().Update(entity);
        }
        public void UpdateList(List<T> entityList)
        {
            AppDbContext.Value.Set<T>().UpdateRange(entityList);
        }
        #endregion

        #region Delete
        public void Delete(T entity)
        {
            AppDbContext.Value.Set<T>().Remove(entity);
        }
        public void DeleteList(List<T> entityList)
        {
            AppDbContext.Value.Set<T>().RemoveRange(entityList);
        }
        //public async Task<int> Delete(Expression<Func<T, bool>> filter, string includeProperties = "")
        //{
        //    IQueryable<T> query = AppDbContext.Value.Set<T>().Where(filter);
        //    query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //        .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        //    return await query.DeleteFromQueryAsync();
        //}
        public abstract Expression<Func<T, object>> ColumnsMap(string sortingExpression);
        #endregion
    }
}
