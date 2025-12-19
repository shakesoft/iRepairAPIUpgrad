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
   
    [Table("ReservationOptOut")]
    public class ReservationOptOut : Entity<Guid>, IMayHaveTenant
    {
        [Column("ReservationOptOutKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public string ReservationOptOutCode { get; set; }
        public string ReservationOptOutReason { get; set; }
        public virtual Guid? AttendantID { get; set; }
        public virtual Guid? ReservationKey { get; set; }
        public virtual DateTime? OptOut { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string Unit { get; set; }
       
    }
}
