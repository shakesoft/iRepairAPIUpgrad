using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class LaundryItemViewData
    {
        public LaundryItemViewData()
        {

            AddedLaundryitems = new HashSet<MinibarItemAddOutput>();
            LaundryItems = new HashSet<ItemOutput>();
        }
        public Guid ReservationKey { get; set; }
        public string DocNo { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid? GuestKey { get; set; }
        public string GuestName { get; set; }
        public Guid? RoomKey { get; set; }
        public ICollection<MinibarItemAddOutput> AddedLaundryitems { get; set; }
        public ICollection<ItemOutput> LaundryItems { get; set; }

    }
}
