using Abp.UI;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.Common
{
    public class CommomData
    {
        public static string HouseKeepingMaidStatusInspectionRequired = "Inspection Required";
        public static string RoomAttendantAssignment = "assign";
        public static string RoomAttendantReAssignment = "reassign";
        public static string RoomAttendantUnAssignment = "unassign";
        public static string HouseKeepingMaidStatusClean = "Clean";
        public static string HouseKeepingMaidStatusDirty = "Dirty";
        public static string HouseKeepingMaidStatusMaidInRoom = "Attendant";
        public static string HouseKeepingMaidStatusMaidInRoom2 = "Maid In Room";
        public static string HouseKeepingMaidStatusPhysicalInspection = "Physical Inspection";
        #region iClean - Message Code
        // Attendant  => END housekeeping in the room
        public static readonly string MsgType_iClean_InspectionRequired = "iClean:InspectionRequired";
        // Supervisor => Check that the room is CLEAN  
        public static readonly string MsgType_iClean_CheckClean = "iClean:Checked_Clean";
        // Supervisor => Check that the room is DIRTY  
        public static readonly string MsgType_iClean_CheckDirty = "iClean:Checked_Dirty";
        // Supervisor => Assign the room to Attendant  
        public static readonly string MsgType_iClean_AssignAttendant = "iClean:Assign_Attendant";
        // Add new Work Order 
        public static readonly string MsgType_iClean_NewWorkOrder = "iClean:NEW_WORKORDER";
        #endregion
        #region iRepair -Message Code
        // Add new Work Order 
        public static readonly string MsgType_iRepair_NewWorkOrder = "iRepair:NEW_WORKORDER";
        // Assign Work Order to Technician
        public static readonly string MsgType_iRepair_AssignWorkOrder = "iRepair:ASSIGN_WORKORDER";
        // Update Work Order Status
        public static readonly string MsgType_iRepair_UpdateWorkOrderStatus = "iRepair:UPDATE_WORKORDER_STATUS";
        // Infrom Block Room Status
        public static readonly string MsgType_iRepair_InformBlockRoomStatus = "iRepair:INFORM_BLOCKROOM_STATUS";
        #endregion

        #region icheckin
        public static int mailPort = 587;
        public static string mailserver = "smtp.office365.com";//"smtp.gmail.com";
        #endregion
        public static string GetNumber(string number)
        {
            string strReturnValue = number;
            try
            {
                int intNumber = Convert.ToInt32(number);
                switch (intNumber)
                {
                    case 1:
                        strReturnValue = number + "st ";
                        break;
                    case 2:
                        strReturnValue = number + "nd ";
                        break;
                    case 3:
                        strReturnValue = number + "rd ";
                        break;
                    default:
                        strReturnValue = number + "th ";
                        break;
                }
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }

        public static CreateOrEditHistoryDto GetRoomStatusChangeHistory(string mode, Room room, Authorization.Users.User user, string AttendantName = "", string PreviousAttendantName = "")
        {
            CreateOrEditHistoryDto input = new CreateOrEditHistoryDto();
            try
            {
                input.StaffKey = user.StaffKey;
                input.Sort = 0;
                input.Sync = 0;
                input.ModuleName = "iClean";
                input.ChangedDate = DateTime.Now;
                input.Operation = "U";
                input.Id = null;
                if (mode.Equals("enable") || mode.Equals("disable"))
                    input.TableName = "RoomDND";
                else if (mode.Equals(CommomData.RoomAttendantAssignment) || mode.Equals(CommomData.RoomAttendantReAssignment) || mode.Equals(CommomData.RoomAttendantUnAssignment))
                    input.TableName = "RoomAssign";
                else
                    input.TableName = "Room";
                input.Detail = "(iClean) " + GetRoomStatusDetail(mode, room, user, AttendantName, PreviousAttendantName);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.ToString());
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return input;
        }
        public static CreateOrEditHistoryDto GetOptReason(string mode, Room room, Authorization.Users.User user, string enabledisable)
        {
            CreateOrEditHistoryDto input = new CreateOrEditHistoryDto();
            try
            {
                input.StaffKey = user.StaffKey;
                input.Sort = 0;
                input.Sync = 0;
                input.ModuleName = "iClean";
                input.ChangedDate = DateTime.Now;
                input.Operation = "U";
                input.Id = null;
                input.TableName = "RoomOPT";
                input.Detail = "(iClean) " + user.UserName + enabledisable;// room.HMMNotes;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.ToString());
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return input;
          
        }
        private static string GetRoomStatusDetail(string mode, Room room, Authorization.Users.User user, string AttendantName, string PreviousAttendantName)
        {
            string strReturnValue = "";
            try
            {

                if (mode.Equals("s") || mode.Equals("e"))
                {

                    strReturnValue = user.UserName + ((mode.Equals("s")) ? " STARTS " : " ENDS ") + " housekeeping in Room# " + room.Unit +
                                      (!string.IsNullOrEmpty(room.HMMNotes) ? ". Notes: " + room.HMMNotes : "");
                }
                else if (mode.Equals("delay"))
                {
                    strReturnValue = user.UserName + " DELAYS " + " housekeeping in Room# " + room.Unit;
                }
                else if (mode.Equals("c") || mode.Equals("d"))
                {
                    strReturnValue = user.UserName + " has updated Room# " + room.Unit + " as " + ((mode.Equals("c")) ? " CLEAN " : " DIRTY ") +
                                      (!string.IsNullOrEmpty(room.HMMNotes) ? ". Notes: " + room.HMMNotes : "");
                }
                else if (mode.Equals("enable")) // enable/disable DND
                {
                    strReturnValue = user.UserName + " has updated Room# " + room.Unit + " as " + "DND";
                }
                else if (mode.Equals("disable")) // enable/disable DND
                {
                    strReturnValue = user.UserName + " has removed DND from Room# " + room.Unit;
                }
                else if (mode.Equals(RoomAttendantAssignment)) // assign Attendant
                {
                    strReturnValue = user.UserName + " has assigned to " + AttendantName + " at Room# " + room.Unit;
                    // strReturnValue = user.Result.UserName + " has assigned to  at Room# " + room.Unit;
                }
                else if (mode.Equals(RoomAttendantReAssignment)) // reassign Attendant
                {
                    strReturnValue = user.UserName + " has reassigned from " + PreviousAttendantName + " to " + AttendantName + " at Room# " + room.Unit;
                    // strReturnValue = user.Result.UserName + " has reassigned from to at Room# " + room.Unit;
                }
                else if (mode.Equals(RoomAttendantUnAssignment)) // unassign Attendant
                {
                    strReturnValue = user.UserName + " has unassigned " + PreviousAttendantName + " from Room# " + room.Unit;
                    //strReturnValue = user.Result.UserName + " has unassigned  from Room# " + room.Unit;
                }
            }
            catch
            {
                throw;
            }
            return strReturnValue;
        }

        public static string GetDateToDisplay(object inputDate)
        {
            try
            {
                string strReturnValue = "";
                if (inputDate != null && inputDate.ToString() != "")
                {
                    strReturnValue = Convert.ToDateTime(inputDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    strReturnValue = " -";
                }
                return strReturnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetWorkOrderIDFromDescription(string description)
        {
            try
            {
                string strReturnValue = "0";
                string[] arrValue = description.Split(':');
                if (arrValue.Length > 1)
                    strReturnValue = arrValue[0].Replace("#", "").Trim();
                return strReturnValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetDateTimeToDisplay(object inputDate)
        {
            try
            {
                string strReturnValue = "";
                if (inputDate != null && inputDate.ToString() != "")
                {
                    strReturnValue = Convert.ToDateTime(inputDate).ToString("dd/MM/yyyy HH:mm:ss");
                }
                return strReturnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetWorkOrderStatusDescriptionByStatus(object woStatus)
        {
            string strReturnValue = "";
            try
            {
                strReturnValue = woStatus.ToString();
                switch (strReturnValue)
                {
                    case "0":
                        strReturnValue = "Initial Entry";
                        break;
                    case "1":
                        strReturnValue = "Inspected";
                        break;
                    case "2":
                        strReturnValue = "In Progress";
                        break;
                    case "3":
                        strReturnValue = "Completed";
                        break;
                    case "4":
                        strReturnValue = "Signed Off";
                        break;
                    case "5":
                        strReturnValue = "Cancelled";
                        break;

                    default:
                        break;
                }
                return strReturnValue;
            }
            catch
            {
                return strReturnValue;
            }
        }

        public static string GetTimeToDisplay(object inputDate)
        {
            try
            {
                string strReturnValue = "";
                if (inputDate != null && inputDate.ToString() != "")
                {
                    strReturnValue = Convert.ToDateTime(inputDate).ToString("HH:mm tt");
                }
                return strReturnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PageListItem> generatePager(int totalRowCount, int pageSize, int currentPage)
        {
            int totalLinkInPage = 5;
            int totalPageCount = (int)Math.Ceiling((decimal)totalRowCount / pageSize);

            int startPageLink = Math.Max(currentPage - (int)Math.Floor((decimal)totalLinkInPage / 2), 1);
            int lastPageLink = Math.Min(startPageLink + totalLinkInPage - 1, totalPageCount);

            if ((startPageLink + totalLinkInPage - 1) > totalPageCount)
            {
                lastPageLink = Math.Min(currentPage + (int)Math.Floor((decimal)totalLinkInPage / 2), totalPageCount);
                startPageLink = Math.Max(lastPageLink - totalLinkInPage + 1, 1);
            }

            List<PageListItem> pageLinkContainer = new List<PageListItem>();


            if (startPageLink != 1)
                pageLinkContainer.Add(new PageListItem("&laquo;", "1", currentPage != 1));
            for (int i = startPageLink; i <= lastPageLink; i++)
            {
                pageLinkContainer.Add(new PageListItem(i.ToString(), i.ToString(), currentPage != i));
            }
            if (lastPageLink != totalPageCount)
                pageLinkContainer.Add(new PageListItem("&raquo;", totalPageCount.ToString(), currentPage != totalPageCount));

            return pageLinkContainer;
        }

        #region icheckincommoncode
        #region Display Date

        public static string GetDateToDisplayIc(object inputDate)
        {
            try
            {
                string strReturnValue = "";
                if (inputDate != null)
                {
                    strReturnValue = Convert.ToDateTime(inputDate).ToString("dd MMM yyyy");
                }
                return strReturnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetFullDateToDisplay(object inputDate)
        {
            try
            {
                string strReturnValue = "";
                if (inputDate != null)
                {
                    strReturnValue = Convert.ToDateTime(inputDate).ToString("dd MMMM yyyy, dddd");
                }
                return strReturnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public static string GetCleanSQLString(string inputSQL)
        {
            try
            {
                string strReturnValue = "";
                strReturnValue = inputSQL.Replace("'", "\'");
                return strReturnValue;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
