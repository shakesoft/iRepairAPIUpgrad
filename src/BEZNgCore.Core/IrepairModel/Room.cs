using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEZNgCore.IrepairModel
{
    [Table("Room")]
    public class Room : Entity<Guid>, IMayHaveTenant
    {
        [Column("RoomKey")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public int? TenantId { get; set; }
        public virtual Guid? RoomTypeKey { get; set; }
        public virtual int? Active { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string Unit { get; set; }
        [StringLength(5, MinimumLength = 0)]
        public virtual string Status { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public virtual string Remark { get; set; }
        public virtual int? MaxPax { get; set; }
        public virtual Guid? InterconnectRoomKey { get; set; }
        public virtual int? Tv { get; set; }
        public virtual int? Shower { get; set; }
        public virtual int? Sort { get; set; }
        public virtual int? Sync { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Seq { get; set; }
        [Timestamp]
        public virtual byte[] Ts { get; set; }
        public virtual int? Floor { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public virtual string CleaningTime { get; set; }
        public virtual int? LinenDays { get; set; }
        public virtual int? Bed { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string PhoneExt { get; set; }
        public virtual Guid? RoomStatusKey { get; set; }
        public virtual Guid? MaidStatusKey { get; set; }
        public virtual Guid? MaidKey { get; set; }
        public virtual string LinenChange { get; set; }
        public virtual DateTime? CheckInDate { get; set; }
        public virtual DateTime? CheckInTime { get; set; }
        public virtual DateTime? CheckOutDate { get; set; }
        public virtual DateTime? CheckOutTime { get; set; }
        public virtual int? Extra1 { get; set; }
        public virtual int? Extra2 { get; set; }
        public virtual int? Extra3 { get; set; }
        public virtual int? Extra4 { get; set; }
        public virtual int? Extra5 { get; set; }
        public virtual int? Extra6 { get; set; }
        public virtual int? Extra7 { get; set; }
        public virtual int? Extra8 { get; set; }
        public virtual int? Extra9 { get; set; }
        public virtual int? Extra10 { get; set; }
        public virtual int? Extra11 { get; set; }
        public virtual int? Extra12 { get; set; }
        public virtual int? Extra13 { get; set; }
        public virtual int? Extra14 { get; set; }
        public virtual int? Extra15 { get; set; }
        public virtual int? Extra16 { get; set; }
        public virtual int? Extra17 { get; set; }
        public virtual int? Extra18 { get; set; }
        public virtual int? Extra19 { get; set; }
        public virtual int? Extra20 { get; set; }
        public virtual int? Extra21 { get; set; }
        public virtual int? Extra22 { get; set; }
        public virtual int? Extra23 { get; set; }
        public virtual int? Extra24 { get; set; }
        [StringLength(512, MinimumLength = 0)]
        public virtual string HMMNotes { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public virtual string Tower { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string PhoneExt2 { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string PhoneExt3 { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public virtual string PhoneExt4 { get; set; }
        public virtual int? DND { get; set; }
        public virtual int? ARC_Sent { get; set; }
    }
}
