using Microsoft.Extensions.Logging;
using FayoumGovPortal.Core.Contracts.IRepository;

namespace FayoumGovPortal.Core.Application.AppService.Base
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
