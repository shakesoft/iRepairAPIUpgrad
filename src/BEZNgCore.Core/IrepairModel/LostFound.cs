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
    [Table("LostFound")]
    public class LostFound : Entity<Guid>, IMayHaveTenant
    {
        [Column("LostFoundKey")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? LostFoundStatusKey { get; set; }
        public virtual DateTime ReportedDate { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string ItemName { get; set; }
        public virtual int? Area { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string Owner { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string OwnerFolio { get; set; }
        public virtual Guid? OwnerRoomKey { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string OwnerContactNo { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public virtual string Founder { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string FounderFolio { get; set; }
        public virtual Guid? FounderRoomKey { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string FounderContactNo { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string Description { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string Instruction { get; set; }
        [StringLength(500, MinimumLength = 0)]
        public virtual string AdditionalInfo { get; set; }
        public virtual Guid? StaffKey { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] TS { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string AutoReference { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string Reference { get; set; }
    }
}
