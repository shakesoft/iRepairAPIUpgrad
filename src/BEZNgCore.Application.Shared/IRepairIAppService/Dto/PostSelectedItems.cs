using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class PostSelectedItems
    {
        public PostSelectedItems()
        {
            ItemSelected = new HashSet<ItemSelectedOutput>();
        }
        public ICollection<ItemSelectedOutput> ItemSelected { get; set; }
        public string roomNo { get; set; }
        public string voucherNo { get; set; }
        public string ReservationKey { get; set; }
        public string roomKey { get; set; }
    }
    public class ItemSelectedOutput
    {
        public int No { get; set; }
        public Guid ItemKey { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public decimal? SalesPrice { get; set; }
        //public  ICollection<SalesPrice> SalesPrice { get; set; }
        public Guid PostCodeKey { get; set; }
    }
}
