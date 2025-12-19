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
    [Table("AllotmentLine")]
    public class AllotmentLine : Entity<Guid>, IMayHaveTenant
    {
        [Column("AllotmentLineKey")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
       
        public virtual Guid? AllotmentHdrKey { get; set; }
        public virtual Guid? RoomTypeKey { get; set; }
        public virtual int? MondayQty { get; set; }
        public virtual int? TuesdayQty { get; set; }
        public virtual int? WednesdayQty { get; set; }
        public virtual int? ThursdayQty { get; set; }
        public virtual int? FridayQty { get; set; }
        public virtual int? SaturdayQty { get; set; }
        public virtual int? SundayQty { get; set; }
        public virtual int? HolidayQty { get; set; }
        public virtual int? HolidayEveQty { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }

        [Timestamp]
        public virtual byte[] TS { get; set; }

      
    }
}
