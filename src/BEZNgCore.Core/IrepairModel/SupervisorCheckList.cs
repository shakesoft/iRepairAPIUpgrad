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
    [Table("SupervisorCheckList")]
    public class SupervisorCheckList : Entity<Guid>, IMayHaveTenant
    {
        [Column("InspectionChecklistKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        [Column("Group4")]
        [StringLength(50, MinimumLength = 0)]
        public virtual Guid ItemKey { get; set; }
        public virtual DateTime DocDate { get; set; }
        public virtual int Checked { get; set; }
        public virtual Guid RoomKey { get; set; }
        public virtual Guid? CreatedBy { get; set; }
        public virtual Guid? ModifiedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
    }
}
