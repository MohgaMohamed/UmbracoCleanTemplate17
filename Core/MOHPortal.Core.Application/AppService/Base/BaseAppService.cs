using Microsoft.Extensions.Logging;
using MOHPortal.Core.Contracts.IRepository;

namespace MOHPortal.Core.Application.AppService.Base
{
    internal class BaseAppService
    {
        protected IUnitOfWork? UnitOfWork { get; }
        protected ILogger? Logger { get; }

        public BaseAppService()
        {

        }

        public BaseAppService(IUnitOfWork? unitOfWork , ILogger? logger)
        {
            UnitOfWork = unitOfWork;
            Logger = logger;
        }

    }
}
