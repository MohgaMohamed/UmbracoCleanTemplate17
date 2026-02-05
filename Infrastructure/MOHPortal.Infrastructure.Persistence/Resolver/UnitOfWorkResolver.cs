using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MOHPortal.Core.Contracts.IRepository;
using MOHPortal.Infrastructure.Persistence.DbContext;
using MOHPortal.Infrastructure.Persistence.Repository;

namespace MOHPortal.Infrastructure.Persistence.Resolver
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
