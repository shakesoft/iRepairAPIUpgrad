using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("MWorkTimeSheetNoteTemplate")]
    public class MWorkTimeSheetNoteTemplate: Entity<int>, IMayHaveTenant
    {
        [Column("Seqno")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public int? TenantId { get; set; }
        
        [StringLength(500, MinimumLength = 0)]
        public virtual string Description { get; set; }
        public virtual int Active { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Guid? ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
    }
}
