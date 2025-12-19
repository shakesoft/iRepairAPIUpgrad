using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class BlockRoom
    {
        public BlockRoom() { }

        public Guid? Roomblockkey { get; set; }
        public int Mworkorderno { get; set; }
        public Guid? MWorkOrderKey { get; set; }  // for log and messaging
        public int? Active { get; set; }
        public DateTime? Blockdate { get; set; }
        public DateTime? BlockFromDate { get; set; } // for log and messaging
        public DateTime? BlockToDate { get; set; }  // for log and messaging
        public string Blockstaff { get; set; }
        public DateTime? Blocktime { get; set; }
        private string _Comment = "";
        public string Comment
        {
            get
            {
                return (this._Comment.Length > 500 ? this._Comment.Substring(0, 500) : this._Comment);
            }
            set
            {
                this._Comment = value;
            }
        }
        private string _Reason = "";
        public string Reason
        {
            get
            {
                return (this._Reason.Length > 50 ? this._Reason.Substring(0, 50) : this._Reason);
            }
            set
            {
                this._Reason = value;
            }
        }
        public Guid? Roomkey { get; set; }
        public string RoomNo { get; set; } // for log and messaging
        public string LastUpdatedBy { get; set; } // for log and messaging
        public string Unblockstaff { get; set; }
        public DateTime? Unblocktime { get; set; }
        public string DetailLog { get; set; } // for log and messaging
        public string NewLog { get; set; }
        public string OldLog { get; set; }
        public int? TenantId { get; set; }
    }
}
