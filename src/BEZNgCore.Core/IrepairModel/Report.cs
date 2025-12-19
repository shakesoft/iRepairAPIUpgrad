using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Report")]
    public class Report : Entity<Guid>, IMayHaveTenant
    {
        [Column("ReportKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? ParentReportKey { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string RptName { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Category { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Caption { get; set; }
    }
}
