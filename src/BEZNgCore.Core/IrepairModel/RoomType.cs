using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("RoomType")]
    public class RoomType : Entity<Guid>, IMayHaveTenant
    {
        [Column("RoomTypeKey")]
        public override Guid Id { get; set; }

        public int? TenantId { get; set; }
        [Column("RoomType")]
        [StringLength(10, MinimumLength = 0)]
        public virtual string RoomTypeName { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Description { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] Ts { get; set; }
        public virtual decimal? DefaultRate { get; set; }
        public virtual int? PhysicalRoomType { get; set; }
        public virtual int? Active { get; set; }
        public virtual Guid? RoomTypeGroupingKey { get; set; }
        public virtual short? DefaultPoint { get; set; }
        public virtual int IsMember { get; set; }
        public virtual int LastRoom { get; set; }
        public virtual int LeftRoom { get; set; }
        public virtual int? ArcSent { get; set; }
        public virtual int? Online { get; set; }
        public virtual decimal? MinRate { get; set; }
        public virtual int? Fs { get; set; }
        public virtual int? Lt { get; set; }
        public virtual int? Ds { get; set; }
        [StringLength(1000, MinimumLength = 0)]
        public virtual string LiveImgUrl { get; set; }
    }
}
