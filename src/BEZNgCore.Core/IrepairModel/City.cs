using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairModel
{
    [Table("City")]
    public class City : Entity<Guid>, IMayHaveTenant
    {

        [Column("CityKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string ShortCode { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }

        [Column("City")]
        [StringLength(50, MinimumLength = 0)]
        public virtual string CityCode { get; set; }
    }
}
