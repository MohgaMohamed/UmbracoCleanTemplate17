using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MOHPortal.Core.Application.AppService.Base;
using MOHPortal.Core.Contracts.IAppService.Base;

namespace MOHPortal.Core.Application.Resolver
{
    public static class AppServiceResolver
    {
        public static void ResolveAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            Type[] appServices = Assembly.Load(typeof(BaseAppService).Assembly.GetName()).GetTypes().Where(x => x.IsSubclassOf(typeof(BaseAppService))).ToArray();
            Type[] iAppServices = Assembly.Load(typeof(IBaseAppService).Assembly.GetName()).GetTypes().Where(a => a.IsInterface).ToArray(); 

            foreach (Type iAppService in iAppServices)
            {
                Type? classType = appServices.FirstOrDefault(iAppService.IsAssignableFrom);
                if (classType != null)
                {
                    services.AddScoped(iAppService, classType);
                }
            }

        }
    }
}
