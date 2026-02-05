using MOHPortal.Infrastructure.Persistence.DbContext;

namespace MOHPortal.Infrastructure.Persistence.Repository
{
    class UserRepository //: BaseRepository<User>, IUserRepository
    {
        public UserRepository(Lazy<AppDbContext> appDbContext) {} //: base(appDbContext) { }
        
        //public override Expression<Func<User, object>> ColumnsMap(string sortingExpression)
        //{
        //    Expression<Func<User, object>> expression;

        //    Dictionary<string, Expression<Func<User, object>>> expressions = new Dictionary<string, Expression<Func<User, object>>>()
        //    {
        //        ["userName"] = c => c.UserName,
        //        ["email"] = c => c.Email,
        //        ["accountStatus"] = c => c.IsRegistered
        //    };
        //    if (string.IsNullOrEmpty(sortingExpression) || !expressions.ContainsKey(sortingExpression))
        //        expression = c => c.CreationTime;
        //    else
        //        expression = expressions[sortingExpression];

        //    return expression;
        //}

        //public async Task<User> GetUserByEmail(string email, string includeProperties = "") =>
        //    await FirstOrDefaultAsync(src => src.Email == email && src.DeleteStatus == (int)RecordStatusEnum.NotDeleted, includeProperties);
        
        
    }
}
