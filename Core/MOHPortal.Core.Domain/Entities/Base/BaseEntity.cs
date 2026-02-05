using System.ComponentModel.DataAnnotations;
using MOHPortal.Core.Domain.Enums;

namespace MOHPortal.Core.Domain.Entities.Base
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
