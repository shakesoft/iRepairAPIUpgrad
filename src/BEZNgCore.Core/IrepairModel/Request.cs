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
    [Table("Request")]
    public class Request : Entity<Guid>, IMayHaveTenant
    {
        [Column("RequestKey")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? GuestKey { get; set; }
        public virtual Guid? ReservationKey { get; set; }
        public virtual Guid? LastModifiedStaff { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }

        [Timestamp]
        public virtual byte[] TS { get; set; }

        [StringLength(1, MinimumLength = 0)]
        public virtual string PrintToGuestArrival { get; set; }
        public virtual Guid? CompanyKey { get; set; }

        [StringLength(200, MinimumLength = 0)]
        public virtual string Remark { get; set; }
      
    }
}
