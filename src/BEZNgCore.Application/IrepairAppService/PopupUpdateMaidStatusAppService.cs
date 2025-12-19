using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.Common;
using BEZNgCore.IRepairIAppService;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class PopupUpdateMaidStatusAppService : BEZNgCoreAppServiceBase, IPopupUpdateMaidStatusAppService
    {
        private readonly RoomRepository _roomdalRepository;
        private readonly MaidStatusRepository _maidStatusdalRepository;
        private readonly ISqoopeMessgingAppService _sqoopeint;
        public PopupUpdateMaidStatusAppService(RoomRepository roomdalRepository,
            MaidStatusRepository maidStatusdalRepository,
            ISqoopeMessgingAppService sqoopeint
            )
        {

            _roomdalRepository = roomdalRepository;
            _maidStatusdalRepository = maidStatusdalRepository;
            _sqoopeint = sqoopeint;
        }

        #region Button Click for Maid - Start / End / Delay
        public async Task<string> StartSave(PopUpMaidStatusInput input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                // var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                Guid StaffKey = user.StaffKey;
                string username = user.UserName;
                string maidKey = _roomdalRepository.staffmaidkey(user.StaffKey).Result;
                string message = "";
                try
                {

                    string strMode = input.strMode;
                    string strRoomNo = input.strRoomNo;
                    string HMMNotes = input.Note;
                    string maidstatus = "";
                    string strCheckMessage = "";
                    // Check whether this room valid to update 
                    strCheckMessage = await CheckValidToStartTaskByRoomNo(strRoomNo, maidKey);
                    if (string.IsNullOrEmpty(strCheckMessage))
                    {
                        Guid RoomKey = new Guid(input.roomKey);
                        Room room = new Room();
                        room.Id = RoomKey;
                        room.Unit = strRoomNo;
                        room.HMMNotes = HMMNotes;
                        var check = _maidStatusdalRepository.GetAllMaidStatus();
                        foreach (DataRow data in check.Rows)
                        {
                            if (data["MaidStatus"].ToString() == CommomData.HouseKeepingMaidStatusMaidInRoom)
                            {
                                maidstatus = CommomData.HouseKeepingMaidStatusMaidInRoom;
                            }
                            else if (data["MaidStatus"].ToString() == CommomData.HouseKeepingMaidStatusMaidInRoom2)
                            {
                                maidstatus = CommomData.HouseKeepingMaidStatusMaidInRoom2;
                            }
                        }

                        Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(maidstatus);
                        Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                        int intSuccessful = _maidStatusdalRepository.UpdateMaidStatusByRoomKey(RoomKey, MaidStatusKey, HMMNotes);
                        if (intSuccessful == 1)
                        {
                            if (!strMode.Equals("mainreq") || !strMode.Equals("noreq"))
                            {
                                History history = GetRoomStatusChangeHistory(strMode, room, StaffKey, username);
                                if (AbpSession.TenantId != null)
                                {
                                    history.TenantId = (int?)AbpSession.TenantId;
                                }
                                intSuccessful = _maidStatusdalRepository.InsertHistory(history);
                                if (strMode.Equals("e") || strMode.Equals("delay"))
                                {
                                    _maidStatusdalRepository.UpdateRoomHistoryLinkKey(history.Id, RoomKey);
                                }
                            }
                        }

                        if (intSuccessful == 1)
                        {
                            message = "Record has been saved.";
                        }
                        else
                        {
                            // message = "Update Fail!";
                            throw new UserFriendlyException("Update Fail!");
                        }
                    }
                    else
                    {
                        //message = strCheckMessage;
                        throw new UserFriendlyException(strCheckMessage);
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());

                }
                return message;
            }
        }
        public async Task<List<MessageNotiViewLatest>> EndSave(PopUpMaidStatusInput input)
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                // var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                Guid StaffKey = user.StaffKey;
                string username = user.UserName;
                string maidKey = _roomdalRepository.staffmaidkey(user.StaffKey).Result;
                bool cleandirect = false;
                string allowcleandirect = _roomdalRepository.getAllowCleanDirectly(user.StaffKey).Result;


                if (allowcleandirect == "10")
                {
                    cleandirect = true;
                }
                try
                {
                    string strMode = input.strMode;
                    string strRoomNo = input.strRoomNo;
                    string HMMNotes = input.Note;

                    string strCheckMessage = "";

                    // Check whether this room valid to update 
                    strCheckMessage = await CheckValidToEndTaskByRoomNo(strRoomNo, maidKey);

                    if (string.IsNullOrEmpty(strCheckMessage))
                    {
                        AttendantCheckList stop = new AttendantCheckList();
                        Guid RoomKey = new Guid(input.roomKey);
                        Room room = new Room();
                        room.Id = RoomKey;
                        room.Unit = strRoomNo;
                        room.HMMNotes = HMMNotes;
                        if (cleandirect)
                        {
                            room.HMMNotes = "clean";
                            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusClean);
                            Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                            room.MaidStatusKey = MaidStatusKey;
                        }
                        else
                        {
                            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusInspectionRequired);
                            Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                            room.MaidStatusKey = MaidStatusKey;
                        }
                        
                        
                        DateTime date = DateTime.Now;
                        int SRqty = _maidStatusdalRepository.GetAttLinenItemByKey(room.Id, date).Result;

                        if (SRqty > 0)
                        {
                            _maidStatusdalRepository.StopUpdateLinenItem(RoomKey);
                        }

                        int intSuccessful = _maidStatusdalRepository.UpdateMaidStatusByRoomKey(RoomKey, room.MaidStatusKey.Value, HMMNotes);
                        if (intSuccessful == 1)
                        {
                            if (!strMode.Equals("mainreq") || !strMode.Equals("noreq"))
                            {
                                History history=new History();
                                if (cleandirect)
                                {
                                    history = GetStatusChangeHistory(strMode, "Nophy", room, StaffKey, username);
                                    
                                }
                                else
                                {
                                    history = GetRoomStatusChangeHistory(strMode, room, StaffKey, username);

                                }
                                if (AbpSession.TenantId != null)
                                {
                                    history.TenantId = (int?)AbpSession.TenantId;
                                }
                                intSuccessful = _maidStatusdalRepository.InsertHistory(history);
                                if (strMode.Equals("e") || strMode.Equals("delay"))
                                {
                                    _maidStatusdalRepository.UpdateRoomHistoryLinkKey(history.Id, RoomKey);
                                }
                            }
                        }
                        if (intSuccessful == 1)
                        {
                            if (cleandirect)
                            { }
                            else
                            {
                                // Send msg to sqoope users if any
                                Alllst = await _sqoopeint.SendMessageToSqoope(CommomData.MsgType_iClean_InspectionRequired, strRoomNo, user.StaffKey.ToString(), "", "");
                                foreach (var v in Alllst)
                                {
                                    MessageNotiViewLatest vc = new MessageNotiViewLatest();
                                    if (v.ToStaffList.Count > 0)
                                    {
                                        string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).ToList().Where(x => x != "").ToArray();
                                        vc.to = too;
                                    }

                                    vc.Message = v.Message;


                                    Alllstlatest.Add(vc);
                                }
                                //To Change Notification
                                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_yes", "OnEndClose();", true);

                                // message = "Record has been saved.";
                            }


                        }
                        else
                        {
                            a.Message = "Update Fail!";
                            Alllst.Add(a);
                        }

                    }
                    else
                    {
                        //message = strCheckMessage;
                        throw new UserFriendlyException(strCheckMessage);

                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());

                }
                // return message;
                return Alllstlatest;
            }
        }

        public async Task<string> AngEndSave(PopUpMaidStatusInput input)
        {
            string message = "";
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            //MessageNotiView a = new MessageNotiView();
            //List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                // var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                Guid StaffKey = user.StaffKey;
                string username = user.UserName;
                string maidKey = _roomdalRepository.staffmaidkey(user.StaffKey).Result;
                bool cleandirect = false;
                string allowcleandirect = _roomdalRepository.getAllowCleanDirectly(user.StaffKey).Result;


                if (allowcleandirect == "10")
                {
                    cleandirect = true;
                }
                try
                {
                    string strMode = input.strMode;
                    string strRoomNo = input.strRoomNo;
                    string HMMNotes = input.Note;

                    string strCheckMessage = "";

                    // Check whether this room valid to update 
                    strCheckMessage = await CheckValidToEndTaskByRoomNo(strRoomNo, maidKey);

                    if (string.IsNullOrEmpty(strCheckMessage))
                    {
                        AttendantCheckList stop = new AttendantCheckList();
                        Guid RoomKey = new Guid(input.roomKey);
                        Room room = new Room();
                        room.Id = RoomKey;
                        room.Unit = strRoomNo;
                        room.HMMNotes = HMMNotes;
                        if (cleandirect)
                        {
                            room.HMMNotes = "clean";
                            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusClean);
                            Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                            room.MaidStatusKey = MaidStatusKey;
                        }
                        else
                        {
                            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusInspectionRequired);
                            Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                            room.MaidStatusKey = MaidStatusKey;
                        }


                        DateTime date = DateTime.Now;
                        int SRqty = _maidStatusdalRepository.GetAttLinenItemByKey(room.Id, date).Result;

                        if (SRqty > 0)
                        {
                            _maidStatusdalRepository.StopUpdateLinenItem(RoomKey);
                        }

                        int intSuccessful = _maidStatusdalRepository.UpdateMaidStatusByRoomKey(RoomKey, room.MaidStatusKey.Value, HMMNotes);
                        if (intSuccessful == 1)
                        {
                            if (!strMode.Equals("mainreq") || !strMode.Equals("noreq"))
                            {
                                History history = new History();
                                if (cleandirect)
                                {
                                    history = GetStatusChangeHistory(strMode, "Nophy", room, StaffKey, username);

                                }
                                else
                                {
                                    history = GetRoomStatusChangeHistory(strMode, room, StaffKey, username);

                                }
                                if (AbpSession.TenantId != null)
                                {
                                    history.TenantId = (int?)AbpSession.TenantId;
                                }
                                intSuccessful = _maidStatusdalRepository.InsertHistory(history);
                                if (strMode.Equals("e") || strMode.Equals("delay"))
                                {
                                    _maidStatusdalRepository.UpdateRoomHistoryLinkKey(history.Id, RoomKey);
                                }
                            }
                        }
                        if (intSuccessful == 1)
                        {
                            string success = "";
                            if (cleandirect)
                            { message = "Record has been saved."; }
                            else
                            {
                                // Send msg to sqoope users if any
                                Alllst = await _sqoopeint.SendMessageToSqoope(CommomData.MsgType_iClean_InspectionRequired, strRoomNo, user.StaffKey.ToString(), "", "");
                                foreach (var v in Alllst)
                                {
                                    MessageNotiWeb vc = new MessageNotiWeb();
                                    if (v.ToStaffList.Count > 0)
                                    {
                                        //string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).ToList().Where(x => x != "").ToArray();
                                        string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                                        vc.to = too;
                                    }
                                    if (vc.to == null || vc.to.Length == 0)
                                    {
                                        success = "success";
                                    }
                                    else
                                    {
                                        vc.Message = v.Message;
                                        vc.Title = CommomData.MsgType_iClean_InspectionRequired;
                                        success = await SendNotiICleanWoAsync(vc);
                                    }
                                }
                                if (success == "success")
                                {
                                    message = "Record has been saved.";
                                }
                                else
                                {
                                    message = "Noti Sent Fail";
                                }
                                //To Change Notification
                                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_yes", "OnEndClose();", true);

                            }
                        }
                        else
                        {
                            message= "Update Fail!";
                            //a.Message = "Update Fail!";
                            //Alllst.Add(a);
                        }

                    }
                    else
                    {
                        //message = strCheckMessage;
                        throw new UserFriendlyException(strCheckMessage);

                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());

                }
                 return message;
                //return Alllstlatest;
            }
        }
        private async Task<string> SendNotiICleanWoAsync(MessageNotiWeb input)
        {
            string strReturnValue = "";
            if (input != null && !string.IsNullOrEmpty(input.Message) && input.to.Length > 0)
            {
                var credential = GoogleCredential.FromFile("iclean-362305-a5691d33f307.json")//C:/inetpub/wwwroot/iRepairAPI/
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                string url = "https://fcm.googleapis.com/v1/projects/iclean-362305/messages:send";

                #region send multiple device
                for (int i = 0; i < input.to.Length; i++)
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                        var message = new
                        {
                            message = new
                            {
                                token = input.to[i],
                                notification = new
                                {
                                    title = input.Title,
                                    body = input.Message
                                }
                            }
                        };

                        var jsonMessage = JsonSerializer.Serialize(message);
                        var httpContent = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync(url, httpContent);

                        if (response.IsSuccessStatusCode)
                        {
                            strReturnValue = "success";
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            strReturnValue = $"Error: {response.StatusCode} - {errorContent}";
                            break;
                        }
                    }

                }
                #endregion
            }

            return strReturnValue;
        }
        public async Task<string> DelaySave(PopUpMaidStatusInput input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                Guid StaffKey = user.StaffKey;
                string username = user.UserName;
                string maidKey = _roomdalRepository.staffmaidkey(user.StaffKey).Result;
                string message = "";
                try
                {

                    string strMode = input.strMode;
                    string strRoomNo = input.strRoomNo;
                    string HMMNotes = input.Note;

                    string strCheckMessage = "";

                    // Check whether this room valid to update 
                    strCheckMessage = await CheckValidToEndTaskByRoomNo(strRoomNo, maidKey);

                    if (string.IsNullOrEmpty(strCheckMessage))
                    {
                        Guid RoomKey = new Guid(input.roomKey);
                        Room room = new Room();
                        room.Id = RoomKey;
                        room.Unit = strRoomNo;
                        room.HMMNotes = HMMNotes;
                        Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusDirty);
                        Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                        room.MaidStatusKey = MaidStatusKey;
                        int intSuccessful = _maidStatusdalRepository.UpdateMaidStatusByRoomKey(RoomKey, MaidStatusKey, HMMNotes);
                        if (intSuccessful == 1)
                        {
                            if (!strMode.Equals("mainreq") || !strMode.Equals("noreq"))
                            {
                                History history = GetRoomStatusChangeHistory(strMode, room, StaffKey, username);
                                if (AbpSession.TenantId != null)
                                {
                                    history.TenantId = (int?)AbpSession.TenantId;
                                }
                                intSuccessful = _maidStatusdalRepository.InsertHistory(history);
                                if (strMode.Equals("e") || strMode.Equals("delay"))
                                {
                                    _maidStatusdalRepository.UpdateRoomHistoryLinkKey(history.Id, RoomKey);
                                }
                            }
                        }

                        if (intSuccessful == 1)
                        {
                            message = "Record has been saved.";
                        }
                        else
                        {
                            // message = "Update Fail!";
                            throw new UserFriendlyException("Update Fail!");
                        }
                    }
                    else
                    {
                        //message = strCheckMessage;
                        throw new UserFriendlyException(strCheckMessage);
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());

                }
                return message;
            }
        }

        private async Task<string> CheckValidToStartTaskByRoomNo(string roomNo, string maidKey)
        {
            string strCheckMessage = "";
            // Check whether this room valid to update 
            DateTime dtBusinessDate = DateTime.Now;
            if (!string.IsNullOrEmpty(maidKey))
            {
                // var control = await _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefaultAsync();
                Task<List<GetMaidStatusOutput>> lstbd = _maidStatusdalRepository.GetBusinessDate();
                dtBusinessDate = lstbd.Result[0].BusinessDate;
            }
            string maidStatusKey = ""; string roomStatusKey = ""; string floor = ""; string guestStatus = "";
            List<GetDashRoomByMaidKeyOutput> lst = await _maidStatusdalRepository.GetRoomByMaidKey(dtBusinessDate, maidKey, maidStatusKey, roomStatusKey, floor, guestStatus);
            //  string strCheckMessage = BLL_Room.CheckValidToStartTaskByRoomNo(strProperty, strRoomNo, BLL_Staff.GetLoginMaidKey());

            if (lst.Count > 0)
            {
                bool blnHasStartedTask = false;
                // Check whether there is any Room with 'Maid In Room' status
                Task<List<MaidHasStartedTaskOutput>> lsta = _roomdalRepository.GetRoomCountByMaidKey(dtBusinessDate, maidKey, floor);
                foreach (MaidHasStartedTaskOutput room in lsta.Result)
                {
                    if (room.MaidStatus.Equals(CommomData.HouseKeepingMaidStatusMaidInRoom))
                        blnHasStartedTask = true;
                }

                if (blnHasStartedTask)
                {
                    strCheckMessage += "You can only start one task at a time. <br/>";
                }

                // This room is assigned to this maid.
                bool blnAssign = IsRoomNoExist(lst, roomNo);
                if (!blnAssign)
                {
                    strCheckMessage += "Room# " + roomNo + " is not assigned to you.<br/>";
                }

                // Maid Status must be Dirty
                string strMessage = CheckRoomMaidStatusIsValid(lst, roomNo, CommomData.HouseKeepingMaidStatusDirty);
                if (!string.IsNullOrEmpty(strMessage))
                {
                    strCheckMessage += "No Action Required.<br/>" + strMessage;
                }

            }
            else
            {
                strCheckMessage = "No Record Found.";
            }
            return strCheckMessage;
        }
        private async Task<string> CheckValidToEndTaskByRoomNo(string roomNo, string maidKey)
        {
            string strCheckMessage = "";
            DateTime dtBusinessDate = DateTime.Now;
            //if (!string.IsNullOrEmpty(maidKey))
            //{
            //    // var control = await _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefaultAsync();
            //    Task<List<GetMaidStatusOutput>> lstbd = _maidStatusdalRepository.GetBusinessDate();
            //    dtBusinessDate = lstbd.Result[0].BusinessDate;
            //}
            string maidStatusKey = ""; string roomStatusKey = ""; string floor = ""; string guestStatus = "";
            List<GetDashRoomByMaidKeyOutput> lst = await _maidStatusdalRepository.GetRoomByMaidKey(dtBusinessDate, maidKey, maidStatusKey, roomStatusKey, floor, guestStatus);
            //  string strCheckMessage = BLL_Room.CheckValidToStartTaskByRoomNo(strProperty, strRoomNo, BLL_Staff.GetLoginMaidKey());

            if (lst.Count > 0)
            {

                // This room is assigned to this maid.
                bool blnAssign = IsRoomNoExist(lst, roomNo);
                if (!blnAssign)
                {
                    strCheckMessage += "Room# " + roomNo + " is not assigned to you.<br/>";
                }
                string maidstatus = "";
                var check = _maidStatusdalRepository.GetAllMaidStatus();
                foreach (DataRow data in check.Rows)
                {
                    if (data["MaidStatus"].ToString() == CommomData.HouseKeepingMaidStatusMaidInRoom)
                    {
                        maidstatus = CommomData.HouseKeepingMaidStatusMaidInRoom;
                    }
                    else if (data["MaidStatus"].ToString() == CommomData.HouseKeepingMaidStatusMaidInRoom2)
                    {
                        maidstatus = CommomData.HouseKeepingMaidStatusMaidInRoom2;
                    }
                }

                //Maid Status must be Dirty
                string strMessage = CheckRoomMaidStatusIsValid(lst, roomNo, maidstatus);
                if (!string.IsNullOrEmpty(strMessage))
                {
                    strCheckMessage += "No Action Required.<br/>" + strMessage;
                }

            }
            else
            {
                strCheckMessage = "No Record Found.";
            }
            return strCheckMessage;
        }
        private static bool IsRoomNoExist(List<GetDashRoomByMaidKeyOutput> lst, string strRoomNo)
        {
            bool blnAssign = false;

            foreach (GetDashRoomByMaidKeyOutput room in lst)
            {
                if (room.Unit.Equals(strRoomNo))
                {
                    blnAssign = true; break;
                }
            }
            return blnAssign;

        }
        private static string CheckRoomMaidStatusIsValid(List<GetDashRoomByMaidKeyOutput> lst, string roomNo, string status)
        {
            string strReturnValue = "";

            foreach (GetDashRoomByMaidKeyOutput room in lst)
            {
                if (room.Unit.Equals(roomNo))
                {
                    if (!room.MaidStatus.ToString().Equals(status))
                    {
                        strReturnValue = "Room# " + roomNo + " : Maid Status is " + room.MaidStatus.ToString();
                    }
                }
            }
            return strReturnValue;

        }

        #region history
        public static History GetRoomStatusChangeHistory(string mode, Room room, Guid staffKey, string username)
        {

            History history = new History();
            try
            {
                history.Id = Guid.NewGuid();
                history.StaffKey = staffKey;
                history.SourceKey = room.Id;
                history.Operation = "U";
                if (mode.Equals("enable") || mode.Equals("disable"))
                    history.TableName = "RoomDND";
                else if (mode.Equals(CommomData.RoomAttendantAssignment) || mode.Equals(CommomData.RoomAttendantReAssignment) || mode.Equals(CommomData.RoomAttendantUnAssignment))
                    history.TableName = "RoomAssign";
                else
                    history.TableName = "Room";
                history.Detail = "(iClean) " + GetRoomStatusDetail(mode, room, username);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
                // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return history;
        }
        private static string GetRoomStatusDetail(string mode, Room room, string username)
        {
            string strReturnValue = "";
            try
            {
                if (mode.Equals("s") || mode.Equals("e"))
                {
                    strReturnValue = username + ((mode.Equals("s")) ? " STARTS " : " ENDS ") + " housekeeping in Room# " + room.Unit +
                                      (!string.IsNullOrEmpty(room.HMMNotes) ? ". Notes: " + room.HMMNotes : "");
                }
                else if (mode.Equals("delay"))
                {
                    strReturnValue = username + " DELAYS " + " housekeeping in Room# " + room.Unit;
                }
                else if (mode.Equals("c") || mode.Equals("d"))
                {
                    strReturnValue = username + " has updated Room# " + room.Unit + " as " + ((mode.Equals("c")) ? " CLEAN " : " DIRTY ") +
                                      (!string.IsNullOrEmpty(room.HMMNotes) ? ". Notes: " + room.HMMNotes : "");
                }


            }
            catch
            {
                throw;
            }
            return strReturnValue;
        }
        #endregion

        #endregion

        #region Button Click for Supervisor - Clean / Dirty
        public async Task<List<MessageNotiViewLatest>> CleanSave(PopUpMaidStatusInput input)
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                Guid StaffKey = user.StaffKey;
                string username = user.UserName;
                //string maidKey = _roomdalRepository.staffmaidkey(user.Result.StaffKey).Result;

                try
                {

                    string strMode = input.strMode;
                    string strRoomNo = input.strRoomNo;
                    string HMMNotes = input.Note;
                    string strPhy = input.phy;

                    // Check whether this room valid to update 
                    if (strMode == "supclean")
                    {
                        string strCheckMessage = "";
                        strCheckMessage = await CheckValidToUpdateCleanOrDirtyByRoomNoSupervisor(strRoomNo);
                        if (string.IsNullOrEmpty(strCheckMessage))
                        {
                            Guid RoomKey = new Guid(input.roomKey);
                            Room room = new Room();
                            room.Id = RoomKey;
                            room.Unit = strRoomNo;
                            room.HMMNotes = HMMNotes;
                            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusClean);
                            Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                            room.MaidStatusKey = MaidStatusKey;
                            int intSuccessful = _maidStatusdalRepository.UpdateMaidStatusByRoomKey(RoomKey, MaidStatusKey, HMMNotes);
                            if (intSuccessful == 1)
                            {
                                if (!strMode.Equals("mainreq") || !strMode.Equals("noreq"))
                                {
                                    History history = GetSupervisorStatusChangeHistory(strMode, strPhy, room, StaffKey, username);
                                    if (AbpSession.TenantId != null)
                                    {
                                        history.TenantId = (int?)AbpSession.TenantId;
                                    }
                                    intSuccessful = _maidStatusdalRepository.InsertHistory(history);
                                    if (strMode.Equals("e") || strMode.Equals("delay"))
                                    {
                                        _maidStatusdalRepository.UpdateRoomHistoryLinkKey(history.Id, RoomKey);
                                    }
                                }
                            }

                            if (intSuccessful == 1)
                            {
                                // Send msg to sqoope users if any
                                Alllst = await _sqoopeint.SendMessageToSqoope(CommomData.MsgType_iClean_CheckClean, strRoomNo, user.StaffKey.ToString(), "", "");
                                //tochange notification
                                foreach (var v in Alllst)
                                {
                                    MessageNotiViewLatest vc = new MessageNotiViewLatest();
                                    if (v.ToStaffList.Count > 0)
                                    {
                                        string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).ToList().Where(x => x != "").ToArray();
                                        vc.to = too;
                                    }

                                    vc.Message = v.Message;


                                    Alllstlatest.Add(vc);
                                }
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_yes", "OnUpdateMaidStatusClose();", true);
                                // message = "Record has been saved.";

                            }
                            else
                            {
                                // message = "Update Fail!";
                                a.Message = "Update Fail!";
                                Alllst.Add(a);
                            }

                        }
                        else
                        {
                            //message = strCheckMessage;
                            throw new UserFriendlyException(strCheckMessage);

                        }
                    }
                    else
                    {
                        string strCheckMessage = "";
                        strCheckMessage = await CheckValidToUpdateCleanOrDirtyByRoomNo(strRoomNo);
                        if (string.IsNullOrEmpty(strCheckMessage))
                        {
                            Guid RoomKey = new Guid(input.roomKey);
                            Room room = new Room();
                            room.Id = RoomKey;
                            room.Unit = strRoomNo;
                            room.HMMNotes = HMMNotes;
                            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusClean);
                            Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                            room.MaidStatusKey = MaidStatusKey;
                            int intSuccessful = _maidStatusdalRepository.UpdateMaidStatusByRoomKey(RoomKey, MaidStatusKey, HMMNotes);
                            if (intSuccessful == 1)
                            {
                                if (!strMode.Equals("mainreq") || !strMode.Equals("noreq"))
                                {
                                    History history = GetStatusChangeHistory(strMode, strPhy, room, StaffKey, username);
                                    if (AbpSession.TenantId != null)
                                    {
                                        history.TenantId = (int?)AbpSession.TenantId;
                                    }
                                    intSuccessful = _maidStatusdalRepository.InsertHistory(history);
                                    if (strMode.Equals("e") || strMode.Equals("delay"))
                                    {
                                        _maidStatusdalRepository.UpdateRoomHistoryLinkKey(history.Id, RoomKey);
                                    }
                                }
                            }

                            if (intSuccessful == 1)
                            {
                                // Send msg to sqoope users if any
                                Alllst = await _sqoopeint.SendMessageToSqoope(CommomData.MsgType_iClean_CheckClean, strRoomNo, user.StaffKey.ToString(), "", "");
                                //SqoopeMessgingHelper.SendMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iClean_CheckClean, strRoomNo, BLL_Staff.GetLoginUserStaffKey());

                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_yes", "OnUpdateMaidStatusClose();", true);
                                foreach (var v in Alllst)
                                {
                                    MessageNotiViewLatest vc = new MessageNotiViewLatest();
                                    if (v.ToStaffList.Count > 0)
                                    {
                                        string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).ToList().Where(x => x != "").ToArray();
                                        vc.to = too;
                                    }

                                    vc.Message = v.Message;


                                    Alllstlatest.Add(vc);
                                }
                                // message = "Record has been saved.";

                            }
                            else
                            {
                                a.Message = "Update Fail!";
                                Alllst.Add(a);
                            }

                        }
                        else
                        {
                            //message = strCheckMessage;
                            throw new UserFriendlyException(strCheckMessage);

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());

                }
                return Alllstlatest;
            }
        }

        public async Task<List<MessageNotiViewLatest>> DirtySave(PopUpMaidStatusInput input)
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                Guid StaffKey = user.StaffKey;
                string username = user.UserName;
                //  string maidKey = _roomdalRepository.staffmaidkey(user.Result.StaffKey).Result;

                try
                {
                    string strMode = input.strMode;
                    string strRoomNo = input.strRoomNo;
                    string HMMNotes = input.Note;
                    string strPhy = input.phy;

                    if (strMode == "supdirty")
                    {
                        string strCheckMessage = "";
                        strCheckMessage = await CheckValidToUpdateCleanOrDirtyByRoomNoSupervisor(strRoomNo);
                        if (string.IsNullOrEmpty(strCheckMessage))
                        {
                            Guid RoomKey = new Guid(input.roomKey);
                            Room room = new Room();
                            room.Id = RoomKey;
                            room.Unit = strRoomNo;
                            room.HMMNotes = HMMNotes;
                            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusDirty);
                            Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                            room.MaidStatusKey = MaidStatusKey;
                            int intSuccessful = _maidStatusdalRepository.UpdateMaidStatusByRoomKey(RoomKey, MaidStatusKey, HMMNotes);
                            if (intSuccessful == 1)
                            {
                                // History history = GetStatusChangeHistory(strMode, strPhy, room, StaffKey,username);
                                History history = GetSupervisorStatusChangeHistory(strMode, strPhy, room, StaffKey, username);
                                if (AbpSession.TenantId != null)
                                {
                                    history.TenantId = (int?)AbpSession.TenantId;
                                }
                                intSuccessful = _maidStatusdalRepository.InsertHistory(history);
                                if (strMode.Equals("e") || strMode.Equals("delay"))
                                {
                                    _maidStatusdalRepository.UpdateRoomHistoryLinkKey(history.Id, RoomKey);
                                }
                            }

                            if (intSuccessful == 1)
                            {
                                Alllst = await _sqoopeint.SendMessageToSqoope(CommomData.MsgType_iClean_CheckDirty, strRoomNo, user.StaffKey.ToString(), "", "");
                                // SqoopeMessgingHelper.SendMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iClean_CheckDirty, strRoomNo, BLL_Staff.GetLoginUserStaffKey());
                                foreach (var v in Alllst)
                                {
                                    MessageNotiViewLatest vc = new MessageNotiViewLatest();
                                    if (v.ToStaffList.Count > 0)
                                    {
                                        string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).ToList().Where(x => x != "").ToArray();
                                        vc.to = too;
                                    }

                                    vc.Message = v.Message;


                                    Alllstlatest.Add(vc);
                                }
                                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_yes", "OnUpdateMaidStatusClose();", true);


                            }
                            else
                            {
                                a.Message = "Update Fail!";
                                Alllst.Add(a);
                            }

                        }
                        else
                        {
                            //message = strCheckMessage;
                            throw new UserFriendlyException(strCheckMessage);

                        }
                    }
                    else
                    {
                        // Check whether this room valid to update 
                        string strCheckMessage = "";
                        strCheckMessage = await CheckValidToUpdateCleanOrDirtyByRoomNo(strRoomNo);
                        if (string.IsNullOrEmpty(strCheckMessage))
                        {
                            Guid RoomKey = new Guid(input.roomKey);
                            Room room = new Room();
                            room.Id = RoomKey;
                            room.Unit = strRoomNo;
                            room.HMMNotes = HMMNotes;
                            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusDirty);
                            Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                            room.MaidStatusKey = MaidStatusKey;
                            int intSuccessful = _maidStatusdalRepository.UpdateMaidStatusByRoomKey(RoomKey, MaidStatusKey, HMMNotes);
                            if (intSuccessful == 1)
                            {
                                History history = GetStatusChangeHistory(strMode, strPhy, room, StaffKey, username);
                                if (AbpSession.TenantId != null)
                                {
                                    history.TenantId = AbpSession.TenantId;
                                }
                                intSuccessful = _maidStatusdalRepository.InsertHistory(history);
                                if (strMode.Equals("e") || strMode.Equals("delay"))
                                {
                                    _maidStatusdalRepository.UpdateRoomHistoryLinkKey(history.Id, RoomKey);
                                }
                            }


                            if (intSuccessful == 1)
                            {
                                // Send msg to sqoope users if any
                                Alllst = await _sqoopeint.SendMessageToSqoope(CommomData.MsgType_iClean_CheckDirty, strRoomNo, user.StaffKey.ToString(), "", "");
                                // SqoopeMessgingHelper.SendMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iClean_CheckDirty, strRoomNo, BLL_Staff.GetLoginUserStaffKey());
                                //  ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_yes", "OnUpdateMaidStatusClose();", true);

                                foreach (var v in Alllst)
                                {
                                    MessageNotiViewLatest vc = new MessageNotiViewLatest();
                                    if (v.ToStaffList.Count > 0)
                                    {
                                        string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).ToList().Where(x => x != "").ToArray();
                                        vc.to = too;
                                    }

                                    vc.Message = v.Message;


                                    Alllstlatest.Add(vc);
                                }
                            }
                            else
                            {
                                a.Message = "Update Fail!";
                                Alllst.Add(a);
                            }

                        }
                        else
                        {
                            //message = strCheckMessage;
                            throw new UserFriendlyException(strCheckMessage);

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());

                }
                return Alllstlatest;
            }
        }

        public async Task<string> AngDirtySave(PopUpMaidStatusInput input)
        {
            string message = "";
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            //MessageNotiView a = new MessageNotiView();
            //List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                Guid StaffKey = user.StaffKey;
                string username = user.UserName;
                //  string maidKey = _roomdalRepository.staffmaidkey(user.Result.StaffKey).Result;

                try
                {
                    string strMode = input.strMode;
                    string strRoomNo = input.strRoomNo;
                    string HMMNotes = input.Note;
                    string strPhy = input.phy;

                    if (strMode == "supdirty")
                    {
                        string strCheckMessage = "";
                        strCheckMessage = await CheckValidToUpdateCleanOrDirtyByRoomNoSupervisor(strRoomNo);
                        if (string.IsNullOrEmpty(strCheckMessage))
                        {
                            Guid RoomKey = new Guid(input.roomKey);
                            Room room = new Room();
                            room.Id = RoomKey;
                            room.Unit = strRoomNo;
                            room.HMMNotes = HMMNotes;
                            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusDirty);
                            Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                            room.MaidStatusKey = MaidStatusKey;
                            int intSuccessful = _maidStatusdalRepository.UpdateMaidStatusByRoomKey(RoomKey, MaidStatusKey, HMMNotes);
                            if (intSuccessful == 1)
                            {
                                // History history = GetStatusChangeHistory(strMode, strPhy, room, StaffKey,username);
                                History history = GetSupervisorStatusChangeHistory(strMode, strPhy, room, StaffKey, username);
                                if (AbpSession.TenantId != null)
                                {
                                    history.TenantId = (int?)AbpSession.TenantId;
                                }
                                intSuccessful = _maidStatusdalRepository.InsertHistory(history);
                                if (strMode.Equals("e") || strMode.Equals("delay"))
                                {
                                    _maidStatusdalRepository.UpdateRoomHistoryLinkKey(history.Id, RoomKey);
                                }
                            }

                            if (intSuccessful == 1)
                            {
                                string success = "";
                                Alllst = await _sqoopeint.SendMessageToSqoope(CommomData.MsgType_iClean_CheckDirty, strRoomNo, user.StaffKey.ToString(), "", "");
                                // SqoopeMessgingHelper.SendMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iClean_CheckDirty, strRoomNo, BLL_Staff.GetLoginUserStaffKey());
                                foreach (var v in Alllst)
                                {
                                    MessageNotiWeb vc = new MessageNotiWeb();
                                    if (v.ToStaffList.Count > 0)
                                    {
                                        //string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).ToList().Where(x => x != "").ToArray();
                                        string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                                        vc.to = too;
                                    }
                                    if (vc.to == null || vc.to.Length == 0)
                                    {
                                        success = "success";
                                    }
                                    else
                                    {
                                        vc.Message = v.Message;
                                        vc.Title = CommomData.MsgType_iClean_CheckDirty;
                                        success = await SendNotiICleanWoAsync(vc);
                                    }
                                }
                                if (success == "success")
                                {
                                    message = "Record has been saved.";
                                }
                                else
                                {
                                    message = "Noti Sent Fail";
                                }
                                
                                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_yes", "OnUpdateMaidStatusClose();", true);


                            }
                            else
                            {
                                message = "Update Fail!";
                                //a.Message = "Update Fail!";
                                //Alllst.Add(a);
                            }

                        }
                        else
                        {
                            //message = strCheckMessage;
                            throw new UserFriendlyException(strCheckMessage);

                        }
                    }
                    else
                    {
                        // Check whether this room valid to update 
                        string strCheckMessage = "";
                        strCheckMessage = await CheckValidToUpdateCleanOrDirtyByRoomNo(strRoomNo);
                        if (string.IsNullOrEmpty(strCheckMessage))
                        {
                            Guid RoomKey = new Guid(input.roomKey);
                            Room room = new Room();
                            room.Id = RoomKey;
                            room.Unit = strRoomNo;
                            room.HMMNotes = HMMNotes;
                            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusDirty);
                            Guid MaidStatusKey = new Guid(lstmk.Result[0].MaidStatusKey);
                            room.MaidStatusKey = MaidStatusKey;
                            int intSuccessful = _maidStatusdalRepository.UpdateMaidStatusByRoomKey(RoomKey, MaidStatusKey, HMMNotes);
                            if (intSuccessful == 1)
                            {
                                History history = GetStatusChangeHistory(strMode, strPhy, room, StaffKey, username);
                                if (AbpSession.TenantId != null)
                                {
                                    history.TenantId = AbpSession.TenantId;
                                }
                                intSuccessful = _maidStatusdalRepository.InsertHistory(history);
                                if (strMode.Equals("e") || strMode.Equals("delay"))
                                {
                                    _maidStatusdalRepository.UpdateRoomHistoryLinkKey(history.Id, RoomKey);
                                }
                            }


                            if (intSuccessful == 1)
                            {
                                string success = "";
                                // Send msg to sqoope users if any
                                Alllst = await _sqoopeint.SendMessageToSqoope(CommomData.MsgType_iClean_CheckDirty, strRoomNo, user.StaffKey.ToString(), "", "");
                                // SqoopeMessgingHelper.SendMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iClean_CheckDirty, strRoomNo, BLL_Staff.GetLoginUserStaffKey());
                                //  ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_yes", "OnUpdateMaidStatusClose();", true);
                                foreach (var v in Alllst)
                                {
                                    MessageNotiWeb vc = new MessageNotiWeb();
                                    if (v.ToStaffList.Count > 0)
                                    {
                                        //string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).ToList().Where(x => x != "").ToArray();
                                        string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                                        vc.to = too;
                                    }
                                    if (vc.to == null || vc.to.Length == 0)
                                    {
                                        success = "success";
                                    }
                                    else
                                    {
                                        vc.Message = v.Message;
                                        vc.Title = CommomData.MsgType_iClean_CheckDirty;
                                        success = await SendNotiICleanWoAsync(vc);
                                    }
                                   

                                }
                                if (success == "success")
                                {
                                    message = "Record has been saved.";
                                }
                                else
                                {
                                    message = "Noti Sent Fail";
                                }
                                
                            }
                            else
                            {
                                message = "Update Fail!";
                                //a.Message = "Update Fail!";
                                //Alllst.Add(a);
                            }

                        }
                        else
                        {
                            //message = strCheckMessage;
                            throw new UserFriendlyException(strCheckMessage);

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());

                }
                return message;
                //return Alllstlatest;
            }
        }

        private async Task<string> CheckValidToUpdateCleanOrDirtyByRoomNoSupervisor(string roomNo)
        {
            string strReturnValue = "";
            List<GetDashRoomByMaidStatusKeyOutput> dt = new List<GetDashRoomByMaidStatusKeyOutput>();


            DateTime dtBusinessDate = DateTime.Now;
            string maidKey = ""; string floorNo = ""; string maidStatusKey = ""; string roomStatusKey = "";
            Task<List<GetMaidStatusOutput>> lstbd = _maidStatusdalRepository.GetBusinessDate();
            dtBusinessDate = lstbd.Result[0].BusinessDate;


            dt = await _roomdalRepository.GetSupervisorRoomByMaidStatusKey(dtBusinessDate, maidKey, floorNo, maidStatusKey, roomStatusKey);



            if (dt.Count > 0)
            {
                // This room is assigned to this maid.
                bool blnAssign = IsRoomNoExist(dt, roomNo);
                if (!blnAssign)
                {
                    strReturnValue += "Room# " + roomNo + " status is already updated. No Action Required.<br/>";
                }

            }
            else
            {
                strReturnValue = "No Record Found.";
            }
            return strReturnValue;

        }
        public async Task<string> CheckValidToUpdateCleanOrDirtyByRoomNo(string roomNo)
        {
            string strReturnValue = "";
            List<GetDashRoomByMaidStatusKeyOutput> dt = new List<GetDashRoomByMaidStatusKeyOutput>();

            DateTime dtBusinessDate = DateTime.Now;
            Task<List<GetMaidStatusOutput>> lstmk = _maidStatusdalRepository.GetMaidStatusKeyByStatusAsync(CommomData.HouseKeepingMaidStatusInspectionRequired);

            if (lstmk.Result.Count > 0)
            {
                string maidStatusKey = lstmk.Result[0].MaidStatusKey.ToString();
                Task<List<GetMaidStatusOutput>> lstbd = _maidStatusdalRepository.GetBusinessDate();
                dtBusinessDate = lstbd.Result[0].BusinessDate;
                dt = await _roomdalRepository.GetRoomByMaidStatusKey(dtBusinessDate, maidStatusKey, "", "", "");
            }
            if (dt.Count > 0)
            {
                // This room is assigned to this maid.
                bool blnAssign = IsRoomNoExist(dt, roomNo);
                if (!blnAssign)
                {
                    strReturnValue += "Room# " + roomNo + " status is already updated. No Action Required.<br/>";
                }
                // Maid Status must be Dirty
                string strMessage = CheckRoomMaidStatusIsValid(dt, roomNo, CommomData.HouseKeepingMaidStatusInspectionRequired);
                if (!string.IsNullOrEmpty(strMessage))
                {
                    strReturnValue += "No Action Required.<br/>" + strMessage;
                }

            }
            else
            {
                strReturnValue = "No Record Found.";
            }
            return strReturnValue;

        }
        private bool IsRoomNoExist(List<GetDashRoomByMaidStatusKeyOutput> dt, string roomNo)
        {
            bool blnAssign = false;

            foreach (GetDashRoomByMaidStatusKeyOutput room in dt)
            {
                if (room.Unit.Equals(roomNo))
                {
                    blnAssign = true; break;
                }
            }
            return blnAssign;
        }
        private string CheckRoomMaidStatusIsValid(List<GetDashRoomByMaidStatusKeyOutput> dt, string roomNo, string status)
        {
            string strReturnValue = "";

            foreach (GetDashRoomByMaidStatusKeyOutput room in dt)
            {
                if (room.Unit.Equals(roomNo))
                {
                    if (!room.MaidStatus.ToString().Equals(status))
                    {
                        strReturnValue = "Room# " + roomNo + " : Maid Status is " + room.MaidStatus.ToString();
                    }
                }
            }
            return strReturnValue;
        }
        #region historysupervisor
        public static History GetSupervisorStatusChangeHistory(string mode, string phy, Room room, Guid staffKey, string username)
        {
            History history = new History();
            try
            {
                history.Id = Guid.NewGuid();
                history.StaffKey = staffKey;
                history.SourceKey = room.Id;
                history.Operation = "U";
                if (mode.Equals("enable") || mode.Equals("disable"))
                    history.TableName = "RoomDND";
                else if (mode.Equals(CommomData.RoomAttendantAssignment) || mode.Equals(CommomData.RoomAttendantReAssignment) || mode.Equals(CommomData.RoomAttendantUnAssignment))
                    history.TableName = "RoomAssign";
                else
                    history.TableName = "Room";
                history.Detail = "(iClean) " + GetSuperivsorStatusDetail(mode, phy, room, username);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return history;
        }
        private static string GetSuperivsorStatusDetail(string mode, string phy, Room room, string username)
        {
            string strReturnValue = "";
            try
            {
                if (mode.Equals("supclean") || mode.Equals("supdirty"))//not d
                {
                    strReturnValue = username + " updated Room# " + room.Unit + " as " + ((mode.Equals("supclean")) ? " CLEAN " : " DIRTY ") +
                                    (!string.IsNullOrEmpty(room.HMMNotes) ? ". Notes: " + room.HMMNotes : "");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return strReturnValue;
        }
        #endregion
        #region historynosup
        public static History GetStatusChangeHistory(string mode, string phy, Room room, Guid staffKey, string username)
        {
            History history = new History();
            try
            {
                history.Id = Guid.NewGuid();
                history.StaffKey = staffKey;
                history.SourceKey = room.Id;
                history.Operation = "U";
                if (mode.Equals("enable") || mode.Equals("disable"))
                    history.TableName = "RoomDND";
                else if (mode.Equals(CommomData.RoomAttendantAssignment) || mode.Equals(CommomData.RoomAttendantReAssignment) || mode.Equals(CommomData.RoomAttendantUnAssignment))
                    history.TableName = "RoomAssign";
                else
                    history.TableName = "Room";
                history.Detail = "(iClean) " + GetStatusDetail(mode, phy, room, username);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return history;
        }
        private static string GetStatusDetail(string mode, string phy, Room room, string username)
        {
            string strReturnValue = "";
            try
            {
                if (mode.Equals("c") || mode.Equals("d"))
                {
                    strReturnValue = username + " updated Room# " + room.Unit + " as " + ((mode.Equals("c")) ? " CLEAN " : " DIRTY ") +
                                    ((phy.Equals("phy")) ? " with PHYSICAL INSPECTION " : " without PHYSICAL INSPECTION ") +
                                    (!string.IsNullOrEmpty(room.HMMNotes) ? ". Notes: " + room.HMMNotes : "");
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return strReturnValue;
        }
        #endregion
        #endregion
    }
}
