using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace MOHPortal.Infrastructure.Persistence.DbContext
{
    public partial class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SingularizeTableNames(modelBuilder);
            ModelConfiguration(modelBuilder);
            base.OnModelCreating(modelBuilder);

            modelBuilder.SeedLookupData();
            modelBuilder.SeedInitialData();
        }

        private void SingularizeTableNames(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
        }

        #region Apply Model Configuration
        private void ModelConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        #endregion

    }
}
