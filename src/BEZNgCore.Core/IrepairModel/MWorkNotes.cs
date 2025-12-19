using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("MWorkNotes")]
    public class MWorkNotes : Entity<Guid>, IMayHaveTenant
    {
        [Column("MWorkNotesKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        [StringLength(2000,MinimumLength =0)]
        public virtual string Details { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Guid? MWorkOrderKey { get; set; }
    }
}
