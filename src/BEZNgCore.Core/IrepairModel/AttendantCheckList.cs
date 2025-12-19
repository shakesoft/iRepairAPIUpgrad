using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairModel
{
    [Table("AttendantCheckList")]
    public class AttendantCheckList : Entity<Guid>, IMayHaveTenant
    {
        [Column("LinenChecklistKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid ItemKey { get; set; }
        public virtual DateTime DocDate { get; set; }
        public virtual int Quantity { get; set; }
        public virtual Guid RoomKey { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual Guid? ModifiedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        public virtual int? Stop { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
