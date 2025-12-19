using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("SqoopeMsgLog")]
    public class SqoopeMsgLog : Entity<Guid>, IMayHaveTenant
    {
        [Column("SqoopeMsgLogKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? SqoopeMessageKey { get; set; }
        public virtual int? FromContactId { get; set; }
        public virtual int? ToContactId { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string Msg { get; set; }
        public virtual int? SqoopeMsgId { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string SqoopeMsgCreatedTS { get; set; }
        public virtual DateTime? SqoopeMsgCreatedOn { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string SqoopeMsgResCode { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual Guid? ModifiedBy { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        public virtual bool Read { get; set; }
        public virtual bool Send { get; set; }
        public virtual Guid? ToStaffKey { get; set; }
        [StringLength(2000, MinimumLength = 0)]
        public virtual string FirebaseToken_Id { get; set; }
    }
}
