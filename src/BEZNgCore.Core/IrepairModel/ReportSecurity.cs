using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairModel
{
    [Table("ReportSecurity")]
    public class ReportSecurity : Entity<Guid>, IMayHaveTenant
    {
        [Column("ReportSecurityKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid ReportKey { get; set; }
        public virtual int? Sec_Report { get; set; }
        public virtual Guid? StaffKey { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
    }
}
