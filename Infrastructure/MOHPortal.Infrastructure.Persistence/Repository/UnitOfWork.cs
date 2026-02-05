using Microsoft.EntityFrameworkCore;
using MOHPortal.Core.Contracts.IRepository;
using MOHPortal.Infrastructure.Persistence.DbContext;

namespace MOHPortal.Infrastructure.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public Lazy<AppDbContext> AppDbContext { get; }

        public UnitOfWork(Lazy<AppDbContext> appContext)
        {
            AppDbContext = appContext;
        }

        #region Main Methods Implementation 
        public Task<int> Commit()
        {
            return AppDbContext.Value.SaveChangesAsync();
        }
        public void RollBack()
        {
            var changedEntries = AppDbContext.Value.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }



        public void Dispose()
        {

        }
        #endregion

        #region Repository Implementation
        //public IUserRepository UserRepository => new UserRepository(AppDbContext);
       

        #endregion

    }
}
