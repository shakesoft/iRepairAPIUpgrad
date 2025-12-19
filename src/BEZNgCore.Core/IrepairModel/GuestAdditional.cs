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
    [Table("GuestAdditional")]
    public class GuestAdditional : Entity<Guid>, IMayHaveTenant
    {
        [Column("GuestAdditionalKey")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
      
        public virtual Guid? GuestKey { get; set; }
        public virtual Guid? ItemKey { get; set; }
        public virtual decimal? SalesPrice { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }

        [Timestamp]
        public virtual byte[] TS { get; set; }
        public virtual Guid? GLKey { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string Description { get; set; }
    }

}
