using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class RoomPopupViewData
    {
        public RoomPopupViewData()
        {

            Attendantlist = new HashSet<DDLAttendantOutput>();
        }
        public string PreviousAttendantKey { get; set; }
        public string Unit { get; set; }
        public string PreviousAttendantName { get; set; }
        public bool btnUnassignLink { get; set; }
        public ICollection<DDLAttendantOutput> Attendantlist { get; set; }
    }
    public class DDLAttendantOutput
    {
        public Guid MaidKey { get; set; }
        public string Name { get; set; }
    }
    public class ReasonPopupViewData
    {
        public ReasonPopupViewData()
        {

            Reasonlist = new HashSet<DDLReason>();
        }
        public string strQuestion { get; set; }
        public ICollection<DDLReason> Reasonlist { get; set; }
    }
    public class DDLReason
    {
        public string Reason { get; set; }
        public string HousekeepingOptOutReasonCode { get; set; }
    }
    public class ReasonDirtyPopupViewData
    {
        public ReasonDirtyPopupViewData()
        {

            Reasonlist = new HashSet<DDLDirtyReason>();
        }
        public string strQuestion { get; set; }
        public ICollection<DDLDirtyReason> Reasonlist { get; set; }
    }
    public class DDLDirtyReason
    {
        public string Reason { get; set; }
        public string HousekeepingDirtyReasonCode { get; set; }
    }
    public class OptReasonM
    {
        public string strMode { get; set; }
        public string strRoomNo { get; set; }
        public string optReason { get; set; }
        public string ddlOptReasonSelectedValue { get; set; }
        public string ddlOptReasonSelectedText { get; set; }
    }
}
