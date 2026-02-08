using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FayoumGovPortal.Core.Contracts.IRepository;
using FayoumGovPortal.Infrastructure.Persistence.DbContext;
using FayoumGovPortal.Infrastructure.Persistence.Repository;

namespace  FayoumGovPortal.Infrastructure.Persistence.Resolver
{
    public static class UnitOfWorkResolver
    {
        public static void ResolveUnitOfWork(this IServiceCollection services, IConfiguration configuration)
        {
            // Add ConString To Db Context
            services.AddDbContext<AppDbContext>(cnf =>
            {
                cnf.UseSqlServer(configuration.GetConnectionString("DBConString"), w => w.CommandTimeout(100000));
            });

            // Resolve UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}
