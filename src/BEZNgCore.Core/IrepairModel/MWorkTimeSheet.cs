using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("MWorkTimeSheet")]
    public class MWorkTimeSheet : Entity<int>, IMayHaveTenant
    {
        [Column("Seqno")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public int? TenantId { get; set; }
        public virtual int? Hdr_Seqno { get; set; }
        public virtual int? MTechnician { get; set; }
        public virtual DateTime? WDate { get; set; }
        public virtual DateTime? TimeFrom { get; set; }
        public virtual DateTime? TimeTo { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string Notes { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Guid? ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
    }
}
