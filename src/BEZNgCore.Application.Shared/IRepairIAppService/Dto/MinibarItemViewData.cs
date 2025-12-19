using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class MinibarItemViewData
    {
        public MinibarItemViewData()
        {

            AddedMinibaritems = new HashSet<MinibarItemAddOutput>();
            MinibarItems = new HashSet<ItemOutput>();
        }
        public Guid ReservationKey { get; set; }
        public string DocNo { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid? GuestKey { get; set; }
        public string GuestName { get; set; }
        public Guid? RoomKey { get; set; }
        public ICollection<MinibarItemAddOutput> AddedMinibaritems { get; set; }
        public ICollection<ItemOutput> MinibarItems { get; set; }
    }
    public class MinibarItemAddOutput
    {
        public Guid ItemKey { get; set; }
        public DateTime? PostDate { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string PostDateDes { get; set; }
    }
    public class ItemOutput
    {
        public Guid ItemKey { get; set; }
        public string Description { get; set; }
        public decimal? SalesPrice { get; set; }
        public Guid PostCodeKey { get; set; }
    }
    public class ReservationOutput
    {
        public Guid ReservationKey { get; set; }
        public string DocNo { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid? GuestKey { get; set; }
        public string GuestName { get; set; }
        public Guid? RoomKey { get; set; }
    }
}
