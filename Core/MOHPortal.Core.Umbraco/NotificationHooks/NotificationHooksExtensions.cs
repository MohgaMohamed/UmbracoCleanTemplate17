using Microsoft.Extensions.DependencyInjection;
using MOHPortal.Core.Umbraco.DocumentValidator.Contracts;
using System.ComponentModel.Design;
using System.Reflection;

namespace MOHPortal.Core.Umbraco.NotificationHooks
{
    public static class NotificationHooksExtensions
    {
        public static IServiceCollection AddNotificationHooksFromAssembly(this IServiceCollection services, Assembly assembly) 
        {
            Type hookInterface = typeof(INotificationHook);
            IEnumerable<Type> hooks = assembly.GetTypes()
                .Where(t => hookInterface.IsAssignableFrom(t)).Except([hookInterface]);

            foreach (Type implementation in hooks)
            {
                Type hookType = implementation
                    .GetInterfaces()
                    .First(x => x.IsGenericType && x.IsAssignableTo(hookInterface));
                
                services.AddScoped(hookType, implementation);
            }

            return services;
        }
    }
}
