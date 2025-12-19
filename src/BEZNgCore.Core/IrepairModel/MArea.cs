using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("MArea")]
    public class MArea : Entity<int>, IMayHaveTenant
    {
        [Column("Seqno")]
        public override int Id { get; set; }
        public int? TenantId { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string Description { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Floor { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Block { get; set; }
        public virtual int Active { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Guid? ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
    }
}
