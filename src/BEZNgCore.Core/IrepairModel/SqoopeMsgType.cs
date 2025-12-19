using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BEZNgCore.IrepairModel
{
    [Table("SqoopeMsgType")]
    public class SqoopeMsgType : Entity<Guid>, IMayHaveTenant
    {
        [Column("MessageKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Code { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Description { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual int? Active { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string MessageTemplate { get; set; }
    }
}
