namespace FayoumGovPortal.Core.Domain.Entities.Base
{
    public class BaseAuditEntity : BaseEntity
    {
        public DateTime LastModificationTime { get; set; }
        public int? LastModifierUserId { get; set; }
    }
}
