using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class MWorkTimeSheetInput
    {
        public MWorkTimeSheetInput()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        // Hdr_Seqno, MTechnician, WDate, TimeFrom, TimeTo, Notes, CreatedBy, CreatedOn
        public int Seqno { get; set; }
        public Guid MWorkOrderKey { get; set; }
        public int Hdr_Seqno { get; set; }
        public int? MTechnician { get; set; }
        public string MTechnicianName { get; set; } // For log
        public DateTime? WDate { get; set; }
        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }
        private string _Notes = "";
        public string Notes
        {
            get
            {
                return (this._Notes.Length > 500 ? this._Notes.Substring(0, 500) : this._Notes);
            }
            set
            {
                this._Notes = value;
            }
        }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DetailLog { get; set; } // For log
        public string NewLog { get; set; }
        public string OldLog { get; set; }
        public int? TenantId { get; set; }
    }
}
