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
    [Table("LostFoundStatus")]
    public class LostFoundStatus : Entity<Guid>, IMayHaveTenant
    {
        [Column("LostFoundStatusKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        [Column("LostFoundStatus")]
        [StringLength(50, MinimumLength = 0)]
        public virtual string LostFoundStatusName { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
    }
}
