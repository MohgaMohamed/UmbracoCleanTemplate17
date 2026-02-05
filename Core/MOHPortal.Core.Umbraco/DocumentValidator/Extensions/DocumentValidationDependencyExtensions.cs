using Microsoft.Extensions.DependencyInjection;
using MOHPortal.Core.Umbraco.DocumentValidator.Contracts;
using System.Reflection;

namespace MOHPortal.Core.Umbraco.DocumentValidator.Extensions
{
    public static class DocumentValidationDependencyExtensions
    {
        public static IServiceCollection AddDocumentValidatorsFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            Type validatorInterface = typeof(IDocumentValidator);
            IEnumerable<Type> validators = assembly.GetTypes().Where(t => validatorInterface.IsAssignableFrom(t)).Except([validatorInterface]);
            foreach (Type implementation in validators)
            {
                services.AddScoped(validatorInterface, implementation);
            }

            return services;
        }
    }
}
