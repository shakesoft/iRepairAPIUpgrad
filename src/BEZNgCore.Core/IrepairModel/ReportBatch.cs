using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("ReportBatch")]
    public class ReportBatch : Entity<Guid>, IMayHaveTenant
    {
        [Column("ReportBatchKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Report1 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Report2 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Report3 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Report4 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Report5 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Report6 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Report7 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Report8 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Report9 { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Report10 { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string BatchName { get; set; }
    }
}
