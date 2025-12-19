using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("MPriority")]
    public class MPriority : Entity<int>, IMayHaveTenant
    {
        [Column("PriorityID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public int? TenantId { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Priority { get; set; }
        public virtual int? Sort { get; set; }
    }
}
