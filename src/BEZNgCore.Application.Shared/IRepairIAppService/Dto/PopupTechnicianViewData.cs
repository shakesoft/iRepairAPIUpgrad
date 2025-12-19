using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class PopupTechnicianViewData
    {
        public PopupTechnicianViewData()
        {

            DDLTechnician = new HashSet<DDLTechnicianOutput>();
            listWO = new HashSet<string>();
        }
        public ICollection<DDLTechnicianOutput> DDLTechnician { get; set; }
        public ICollection<string> listWO { get; set; }
    }
    public class PopupBlockRoomStatusViewData
    {
        public PopupBlockRoomStatusViewData()
        {

            DDLBlockRoomStatus = new HashSet<DDLBlockRoomStatusOutput>();
            b = new HashSet<string>();
        }
        public ICollection<DDLBlockRoomStatusOutput> DDLBlockRoomStatus { get; set; }
        public ICollection<string> b { get; set; }
        public string Title { get; set; }
                          
    }
    public class DDLBlockRoomStatusOutput
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public DDLBlockRoomStatusOutput(string value, string text) => (Value, Text) = (value, text);

    }
    public class PopupBlockRoomStatusInput
    {
        public PopupBlockRoomStatusInput()
        {
            ddlValue = "-1";
            ddlText = "--- Please select status ----";
        }
      
        public ICollection<string> b { get; set; }
        public string ddlText { get; set; }
        public string ddlValue { get; set; }

    }
    public class MWorkNote
    {
        public MWorkNote()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Guid MWorkNotesKey { get; set; }
        public string Details { get; set; }
        public Guid MWorkOrderKey { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string DetailLog { get; set; }
        public string NewLog { get; set; }
        public string OldLog { get; set; }
        public int? TenantId { get; set; }
    }

}

