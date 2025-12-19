using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
namespace BEZNgCore.IRepairIAppService.Dto
{
    public class RoomDto : EntityDto<Guid?>
    {
        public Guid RoomKey { get; set; }
        public int? TenantId { get; set; }
        public Guid? RoomTypeKey { get; set; }
        public int? Active { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public string Unit { get; set; }
        [StringLength(5, MinimumLength = 0)]
        public string Status { get; set; }
        [StringLength(200, MinimumLength = 0)]
        public string Remark { get; set; }
        public int? MaxPax { get; set; }
        public Guid? InterconnectRoomKey { get; set; }
        public int? Tv { get; set; }
        public int? Shower { get; set; }
        public int? Sort { get; set; }
        public int? Sync { get; set; }
        public int Seq { get; set; } = 0;
        [Timestamp]
        public byte[] Ts { get; set; }
        public int? Floor { get; set; }
        [StringLength(20, MinimumLength = 0)]
        public string CleaningTime { get; set; }
        public int? LinenDays { get; set; }
        public int? Bed { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public string PhoneExt { get; set; }
        public Guid? RoomStatusKey { get; set; }
        public Guid? MaidStatusKey { get; set; }
        public Guid? MaidKey { get; set; }
        public string LinenChange { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public int? Extra1 { get; set; }
        public int? Extra2 { get; set; }
        public int? Extra3 { get; set; }
        public int? Extra4 { get; set; }
        public int? Extra5 { get; set; }
        public int? Extra6 { get; set; }
        public int? Extra7 { get; set; }
        public int? Extra8 { get; set; }
        public int? Extra9 { get; set; }
        public int? Extra10 { get; set; }
        public int? Extra11 { get; set; }
        public int? Extra12 { get; set; }
        public int? Extra13 { get; set; }
        public int? Extra14 { get; set; }
        public int? Extra15 { get; set; }
        public int? Extra16 { get; set; }
        public int? Extra17 { get; set; }
        public int? Extra18 { get; set; }
        public int? Extra19 { get; set; }
        public int? Extra20 { get; set; }
        public int? Extra21 { get; set; }
        public int? Extra22 { get; set; }
        public int? Extra23 { get; set; }
        public int? Extra24 { get; set; }
        [StringLength(512, MinimumLength = 0)]
        public string HMMNotes { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public string Tower { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public string PhoneExt2 { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public string PhoneExt3 { get; set; }
        [StringLength(10, MinimumLength = 0)]
        public string PhoneExt4 { get; set; }
        public int? DND { get; set; }
        public int? ARC_Sent { get; set; }
    }
}
