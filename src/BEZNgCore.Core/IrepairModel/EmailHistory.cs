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
    [Table("EmailHistory")]
    public class EmailHistory : Entity<Guid>, IMayHaveTenant
    {
        [Column("EmailHistoryKey")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? StaffKey { get; set; }
        public virtual DateTime? SentDateTime { get; set; }
        public virtual Guid? ReservationKey { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string From { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string To { get; set; }
        public virtual int? Sent { get; set; }
        [StringLength(2000, MinimumLength = 0)]
        public virtual string Content { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Subject { get; set; }
    }
}
