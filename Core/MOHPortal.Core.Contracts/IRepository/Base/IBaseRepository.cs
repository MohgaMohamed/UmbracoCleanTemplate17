using System.Linq.Expressions;
using MOHPortal.Core.Domain.Enums;

namespace MOHPortal.Core.Contracts.IRepository.Base
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetPageAsync<TKey>(int skipCount, int takeCount, Expression<Func<T, bool>> filter,
            Expression<Func<T, TKey>> sortingExpression, SortDirectionEnum sortDir = SortDirectionEnum.Ascending,
            string includeProperties = "", Expression<Func<T, TKey>>? sortingExpressionSecond = null);
        Task<List<T>> GetAllAsync();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, string includeProperties = "");
        Task<T> FirstOrDefaultAsync<TKey>(Expression<Func<T, bool>> filter,
            Expression<Func<T, TKey>> sortingExpression, SortDirectionEnum sortDir = SortDirectionEnum.Ascending,
            string includeProperties = "");
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null);
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "");
        Task<List<T>> GetWhereAsync<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> sortingExpression,
            SortDirectionEnum sortDir = SortDirectionEnum.Ascending, string includeProperties = "");

        Task<bool> GetAnyAsync(Expression<Func<T, bool>> filter = null);
        Task<int> GetCountAsync();
        Task<int> GetCountAsync(Expression<Func<T, bool>> filter);
        void CreateAsyn(T entity);
        void CreateListAsyn(List<T> entityList);
        void Update(T entity);
        void UpdateList(List<T> entityList);
        void Delete(T entity);
        void DeleteList(List<T> entityList);
        //Task<int> Delete(Expression<Func<T, bool>> filter, string includeProperties = "");
        Expression<Func<T, object>> ColumnsMap(string sortingExpression);
    }
}
