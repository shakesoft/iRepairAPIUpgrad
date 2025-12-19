using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("MWorkType")]
    public class MWorkType : Entity<int>, IMayHaveTenant
    {
        [Column("Seqno")]
        public override int Id { get; set; }
        public int? TenantId { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public string Description { get; set; }
        public virtual int Active { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Guid? ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
    }
}
