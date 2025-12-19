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
    [Table("AllotmentHdr")]
    public class AllotmentHdr : Entity<Guid>, IMayHaveTenant
    {
        [Column("AllotmentHdrKey")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
      
        public virtual int? Active { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Allotment { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual Guid? PartyKey { get; set; }
        public virtual string Notes { get; set; }
        public virtual int? ReleaseDay { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }

        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual Guid? GroupAllotmentHdrKey { get; set; }
    }
}
