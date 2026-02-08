using Microsoft.EntityFrameworkCore;

namespace  FayoumGovPortal.Infrastructure.Persistence.DbContext
{
    public static class InitialDataSeeding
    {
        public static void SeedInitialData(this ModelBuilder modelBuilder)
        {

            #region User
            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        UserId = 1,
            //        Email = "superadmin@admin.com",
            //        UserName = "SuperAdmin",
            //        RecordStatus = (int)RecordStatusEnum.NotDeleted,
            //    });

            #endregion
        }
    }
}