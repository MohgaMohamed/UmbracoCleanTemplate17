using System.ComponentModel.DataAnnotations;
using FayoumGovPortal.Core.Domain.Enums;

namespace FayoumGovPortal.Core.Domain.Entities.Base
{
    public class BaseEntity
    {
        internal BaseEntity()
        {
            RecordStatus = (int)RecordStatusEnum.NotDeleted;
        }
        [Required]
        public int RecordStatus { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public int? CreatorUserId { get; set; }
    }
}
