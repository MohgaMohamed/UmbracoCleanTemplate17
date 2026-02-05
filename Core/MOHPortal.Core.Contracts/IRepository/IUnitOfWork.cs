namespace MOHPortal.Core.Contracts.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        #region Main Methods
        Task<int> Commit();
        void RollBack();
        #endregion
        #region IRepository
        //IUserRepository UserRepository { get; }
        
        #endregion
    }
}
