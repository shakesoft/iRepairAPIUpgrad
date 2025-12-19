using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
   
    public class PopupTimeSheetViewData
    {
        public PopupTimeSheetViewData()
        {

            DDLTechnician = new HashSet<DDLTechnicianOutput>();
            DDLNoteTemplate=new HashSet<DDLNoteTemplateOutput>();
            btnAddVisible = true;
            btnDeleteVisible = true;
            btnUpdateVisible=true;
            WrokDate = DateTime.Now;

        }
        public ICollection<DDLTechnicianOutput> DDLTechnician { get; set; }
        public ICollection<DDLNoteTemplateOutput> DDLNoteTemplate { get; set; }
        public bool btnAddVisible { get; set; }
        public bool btnDeleteVisible { get; set; }
        public bool btnUpdateVisible { get; set; }
        public DateTime WrokDate { get; set; }
        public string Title { get; set; }
        public string WOID { get; set; }
        public string ddlTechnicianSelectedValue { get; set; }
        public string Seqno { get; set; }
        public TimeSpan? radStartTime { get; set; }
        public TimeSpan? radEndTime { get; set; }
        public string Note { get; set; }
    }
    public class DDLNoteTemplateOutput
    {
        public string Seqno { get; set; }
        public string Description { get; set; }
    }
    public class PopupTimeSheetInput
    {
        //public bool btnAddVisible { get; set; }
        //public bool btnDeleteVisible { get; set; }
        //public bool btnUpdateVisible { get; set; }
        public DateTime WrokDate { get; set; }
        //public string Title { get; set; }
        public string WOID { get; set; }
        public string ddlTechnicianValue { get; set; }
        public string ddlTechnicianText { get; set; }
        public string Seqno { get; set; }
        public TimeSpan? radStartTime { get; set; }
        public TimeSpan? radEndTime { get; set; }
        public string Note { get; set; }
    }
 }
