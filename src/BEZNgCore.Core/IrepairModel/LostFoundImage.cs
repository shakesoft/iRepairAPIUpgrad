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
    [Table("LostFoundImage")]
    public class LostFoundImage : Entity<Guid>, IMayHaveTenant
    {
        [Column("LostFoundImageKey")]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid LostFoundKey { get; set; }
        [Column("LostFoundImage")]
        [StringLength(256, MinimumLength = 0)]
        public virtual string LostFoundImageName { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual Guid? CreatedUser { get; set; }
        public virtual byte[] LostFoundImages { get; set; }
        public virtual byte[] LostFoundImages2 { get; set; }
        public virtual byte[] LostFoundImages3 { get; set; }
        public virtual byte[] LostFoundImages4 { get; set; }
        public virtual byte[] LostFoundImages5 { get; set; }
    }
}
