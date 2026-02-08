namespace FayoumGovPortal.Core.Domain.Entities.Base
{
    public class BaseMultiLanguageAuditEntity : BaseAuditEntity
    {
        public string EnName { get; set; }
        public string ArName { get; set; }
    }
}
