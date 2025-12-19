using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.Common;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IrepairAppService.DAL;
using BEZNgCore.IRepairIAppService;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats;
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
    public class RoomStatusPageAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<RoomStatus, Guid> _roomstatusRepository;
        private readonly IRepository<GuestStatus, Guid> _gueststatusRepository;
        private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IRepository<Control, Guid> _controlRepository;
        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly IRepository<Maid, Guid> _maidRepository;
        private readonly IRepository<History, Guid> _historyRepository;
        private readonly ISqoopeMessgingAppService _sqoopeint;
        private readonly RoomRepository _roomdalRepository;

        RoomDAL dalroom;
        RoomStatusDAL dalroomstatus;
        HistoryDAL dalhistory;
        public RoomStatusPageAppService(
            IRepository<RoomStatus, Guid> roomstatusRepository,
            IRepository<GuestStatus, Guid> gueststatusRepository,
            IRepository<Room, Guid> roomRepository,
            IRepository<Control, Guid> controlRepository,
            IRepository<Staff, Guid> staffRepository,
            IRepository<Maid, Guid> maidRepository,
            RoomRepository roomdalRepository,
            IRepository<History, Guid> historyRepository,
            ISqoopeMessgingAppService sqoopeint
             )
        {
            _roomstatusRepository = roomstatusRepository;
            _gueststatusRepository = gueststatusRepository;
            _roomRepository = roomRepository;
            _controlRepository = controlRepository;
            _staffRepository = staffRepository;
            _roomdalRepository = roomdalRepository;
            dalroom = new RoomDAL(_roomRepository);
            dalroomstatus = new RoomStatusDAL(_roomstatusRepository);
            _maidRepository = maidRepository;
            _historyRepository = historyRepository;
            dalhistory = new HistoryDAL(_historyRepository);
            _sqoopeint = sqoopeint;
        }
        [HttpGet]
        public ListResultDto<RoomViewData> GetRoomViewData()
        {
            List<RoomViewData> Alllst = new List<RoomViewData>();
            RoomViewData a = new RoomViewData();
            a.GetHotelFloors = dalroom.BindHotelFloorList();
            a.GetRoomStatuss = dalroomstatus.GetAllRoomStatus();
            a.GetGuestStatuss = _gueststatusRepository.GetAll().OrderBy(s => s.Sort)
                       .Select(s => new GetGuestStatus
                       {
                           GuestStatusKey = s.Id,
                           StatusCode = s.StatusCode,
                           Status = s.Status
                       })
                       .ToList();
            Alllst.Add(a);
            return new ListResultDto<RoomViewData>(Alllst);
        }

        [HttpGet]
        public ListResultDto<RoomViewData> GetRoomViewDataIncludeAll()
        {
            List<RoomViewData> Alllst = new List<RoomViewData>();
            RoomViewData a = new RoomViewData();
            List<GetHotelFloor> f = new List<GetHotelFloor>();
            GetHotelFloor f1 = new GetHotelFloor();
            f1.Floor = "0";
            f1.btnFloor = "ALL";
            f.Add(f1);
            var hf = _roomRepository.GetAll().GroupBy(x => x.Floor)
                       .OrderBy(x => x.Key)
                       .Select(x => new GetHotelFloor
                       {
                           btnFloor = CommomData.GetNumber(x.Key.ToString()),
                           Floor = x.Key.ToString()
                       });
            a.GetHotelFloors = f.Concat(hf).ToList();
            List<GetRoomStatus> r = new List<GetRoomStatus>();
            GetRoomStatus r1 = new GetRoomStatus();
            r1.RoomStatusKey = Guid.Empty;
            r1.RoomStatus = "ALL";
            r.Add(r1);
            var rs = _roomstatusRepository.GetAll().OrderByDescending(s => s.Seq)
                                        .Select(s => new GetRoomStatus
                                        {
                                            RoomStatusKey = s.Id,
                                            RoomStatus = s.RoomStatusName
                                        });//dalroomstatus.GetAllRoomStatus();
            a.GetRoomStatuss = r.Concat(rs).ToList();
            List<GetGuestStatus> l = new List<GetGuestStatus>();
            GetGuestStatus t1 = new GetGuestStatus();
            t1.GuestStatusKey = Guid.Empty;
            t1.StatusCode = 90;
            t1.Status = "ALL";
            l.Add(t1);
            var b = _gueststatusRepository.GetAll().OrderBy(s => s.Sort)
                       .Select(s => new GetGuestStatus
                       {
                           GuestStatusKey = s.Id,
                           StatusCode = s.StatusCode,
                           Status = s.Status
                       });
            a.GetGuestStatuss = l.Concat(b).ToList();

            Alllst.Add(a);
            return new ListResultDto<RoomViewData>(Alllst);
        }
        [HttpGet]
        public ListResultDto<RoomPopupViewData> GetRoomPopupViewData(string roomNo)
        {
            List<RoomPopupViewData> Alllst = new List<RoomPopupViewData>();
            RoomPopupViewData a = new RoomPopupViewData();
            //a.Attendantlist = _maidRepository.GetAll().Where(x => x.Active == 1)
            //           .OrderBy(x => x.Name)
            //           .Select(x => new DDLAttendantOutput
            //           {
            //               MaidKey = x.Id,
            //               Name = x.Name
            //           })
            //                            .ToList();
            a.Attendantlist = (from m in _maidRepository.GetAll()
                               join s in _staffRepository.GetAll()
                               on m.Id equals s.MaidKey
                               where (m.Active == 1 && s.Active == 1 && s.AnywhereAccess == 1 && s.PIN != null)
                               orderby m.Name
                               select new DDLAttendantOutput
                               {
                                   MaidKey = m.Id,
                                   Name = m.Name
                               }).ToList();
            DataTable dt = _roomdalRepository.GetHotelRoomByRoomNo(roomNo);
            if (dt.Rows.Count > 0)
            {
                a.Unit = dt.Rows[0]["Unit"].ToString();
                a.PreviousAttendantName = (string.IsNullOrEmpty(dt.Rows[0]["Name"].ToString()) ? "none" : dt.Rows[0]["Name"].ToString());
                a.PreviousAttendantKey = dt.Rows[0]["MaidKey"].ToString();
                a.btnUnassignLink = (string.IsNullOrEmpty(dt.Rows[0]["Name"].ToString()) ? false : true);
            }
            else
            {
                throw new UserFriendlyException("No Record Found. Please close this window and try again.");
            }
            Alllst.Add(a);
            return new ListResultDto<RoomPopupViewData>(Alllst);
        }
        public async Task<List<MessageNotiViewLatest>> btnAssignClick(AssignInput input)
        {
            string strMode = CommomData.RoomAttendantAssignment;
            if (!input.strAssignedAttendantKey.ToLower().Equals(input.PreviousAttendantKey.ToLower()))
            {
                if (!string.IsNullOrEmpty(input.PreviousAttendantKey))
                    strMode = CommomData.RoomAttendantReAssignment;
                return await UpdateRoomAssignmentAsync(strMode, input);
            }
            else
            {
                throw new UserFriendlyException("No change on assigned Attendant.");
            }

        }

        public async Task<string> AngbtnAssignClick(AssignInput input)
        {
            string strMode = CommomData.RoomAttendantAssignment;
            if (!input.strAssignedAttendantKey.ToLower().Equals(input.PreviousAttendantKey.ToLower()))
            {
                if (!string.IsNullOrEmpty(input.PreviousAttendantKey))
                    strMode = CommomData.RoomAttendantReAssignment;
                return await AngUpdateRoomAssignmentAsync(strMode, input);
            }
            else
            {
                throw new UserFriendlyException("No change on assigned Attendant.");
            }

        }

        public async Task<List<MessageNotiViewLatest>> lbtnUnassignClick(AssignInput input)
        {
            try
            {
                return await UpdateRoomAssignmentAsync(CommomData.RoomAttendantUnAssignment, input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }

        public async Task<string> AnglbtnUnassignClick(AssignInput input)
        {
            try
            {
                return await AngUpdateRoomAssignmentAsync(CommomData.RoomAttendantUnAssignment, input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }


        private async Task<List<MessageNotiViewLatest>> UpdateRoomAssignmentAsync(string strMode, AssignInput input)
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();

            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            try
            {
                Room room = new Room();
                DataTable dtRoom = _roomdalRepository.GetHotelRoomByRoomNo(input.roomNo);
                if (dtRoom.Rows.Count > 0)
                {
                    room.Id = Guid.Parse(dtRoom.Rows[0]["RoomKey"].ToString());
                }
                room.Unit = input.roomNo;

                //var room = dalroom.GetbyUnit(input.roomNo);
                //if (room.Count > 0)
                //{
                //    if (!strMode.Equals(CommomData.RoomAttendantUnAssignment))
                //    {
                //        room[0].MaidKey = Guid.Parse(input.strAssignedAttendantKey);
                //    }
                //    else
                //    {
                //        room[0].MaidKey = null;
                //    }
                //}
                //int s = dalroom.updateAsync(room[0]).Result;
                int s = 0;
                if (!strMode.Equals(CommomData.RoomAttendantUnAssignment))
                {
                    room.MaidKey = Guid.Parse(input.strAssignedAttendantKey);
                    s = _roomdalRepository.UpdateAssignment(Guid.Parse(input.strAssignedAttendantKey), input.roomNo);

                }
                else
                {
                    s = _roomdalRepository.UpdateAssignment(Guid.Empty, input.roomNo);
                }

                if (s == 1)
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
                        string PreviousAttendantName = input.PreviousAttendantName == "none" ? "" : input.PreviousAttendantName;
                        CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(strMode, room, user, input.strAssignedAttendantName, PreviousAttendantName);
                        var history = ObjectMapper.Map<History>(hi);
                        if (AbpSession.TenantId != null)
                        {
                            history.TenantId = (int?)AbpSession.TenantId;
                        }
                        history.SourceKey = room.Id;
                        int intSuccessful = dalhistory.SaveAsync(history).Result;
                        if (intSuccessful == 1)
                        {
                            // Send msg to sqoope users if any
                            Alllst = await _sqoopeint.SendMessageToSqoope(CommomData.MsgType_iClean_AssignAttendant, room.Unit, user.StaffKey.ToString(), input.strAssignedAttendantName, input.PreviousAttendantKey.Trim());
                            // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_yes", "OnUpdateClose();", true);
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
                            throw new UserFriendlyException("Update Fail!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
            return Alllstlatest;
        }

        private async Task<string> AngUpdateRoomAssignmentAsync(string strMode, AssignInput input)
        {
            string message = "";
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();

            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            try
            {
                Room room = new Room();
                DataTable dtRoom = _roomdalRepository.GetHotelRoomByRoomNo(input.roomNo);
                if (dtRoom.Rows.Count > 0)
                {
                    room.Id = Guid.Parse(dtRoom.Rows[0]["RoomKey"].ToString());
                }
                room.Unit = input.roomNo;

                //var room = dalroom.GetbyUnit(input.roomNo);
                //if (room.Count > 0)
                //{
                //    if (!strMode.Equals(CommomData.RoomAttendantUnAssignment))
                //    {
                //        room[0].MaidKey = Guid.Parse(input.strAssignedAttendantKey);
                //    }
                //    else
                //    {
                //        room[0].MaidKey = null;
                //    }
                //}
                //int s = dalroom.updateAsync(room[0]).Result;
                int s = 0;
                if (!strMode.Equals(CommomData.RoomAttendantUnAssignment))
                {
                    room.MaidKey = Guid.Parse(input.strAssignedAttendantKey);
                    s = _roomdalRepository.UpdateAssignment(Guid.Parse(input.strAssignedAttendantKey), input.roomNo);

                }
                else
                {
                    s = _roomdalRepository.UpdateAssignment(Guid.Empty, input.roomNo);
                }

                if (s == 1)
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
                        string PreviousAttendantName = input.PreviousAttendantName == "none" ? "" : input.PreviousAttendantName;
                        CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(strMode, room, user, input.strAssignedAttendantName, PreviousAttendantName);
                        var history = ObjectMapper.Map<History>(hi);
                        if (AbpSession.TenantId != null)
                        {
                            history.TenantId = (int?)AbpSession.TenantId;
                        }
                        history.SourceKey = room.Id;
                        int intSuccessful = dalhistory.SaveAsync(history).Result;
                        if (intSuccessful == 1)
                        {
                            string success = "";
                            // Send msg to sqoope users if any
                            Alllst = await _sqoopeint.SendMessageToSqoope(CommomData.MsgType_iClean_AssignAttendant, room.Unit, user.StaffKey.ToString(), input.strAssignedAttendantName, input.PreviousAttendantKey.Trim());
                            // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_yes", "OnUpdateClose();", true);
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
                                    vc.Title = CommomData.MsgType_iClean_AssignAttendant;
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
                            throw new UserFriendlyException("Update Fail!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
            //return Alllstlatest;
            return message;
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

        [HttpGet]
        public ListResultDto<ReasonPopupViewData> GetReasonPopupViewData(string mode, string roomNo)
        {
            string strQuestion = "Reasons for Room# " + roomNo ;
            List<ReasonPopupViewData> Alllst = new List<ReasonPopupViewData>();
            ReasonPopupViewData a = new ReasonPopupViewData();
            List<DDLReason> sl = new List<DDLReason>();
            DDLReason s1 = new DDLReason();
            s1.Reason ="";
            s1.HousekeepingOptOutReasonCode = "--Please select--";
            sl.Add(s1);
            List<DDLReason> ss = GetAllReason();
            a.Reasonlist = sl.Concat(ss).ToList();
            if (mode.Equals("enableopt"))
            {
                a.strQuestion = "Confirm update 'Opt Out' status for Room# " + roomNo + "?";
            }
            else if (mode.Equals("disableopt"))
            {
                a.strQuestion = "Confirm remove 'Opt Out' status for Room# " + roomNo + "?";
            }
            else if (mode.Equals("d"))
            {
                //strCheckMessage = BLL_Room.CheckValidToUpdateCleanOrDirtyByRoomNo(strProperty, roomNo);
                a.strQuestion =  "Are you sure that you want to update Room# " + roomNo + " status as Dirty?";
               
            }
            Alllst.Add(a);
            return new ListResultDto<ReasonPopupViewData>(Alllst);
        }

        [HttpGet]
        public ListResultDto<ReasonDirtyPopupViewData> GetDirtyReasonPopupViewData(string mode, string roomNo)
        {
            string strQuestion = "Reasons for Room# " + roomNo;
            List<ReasonDirtyPopupViewData> Alllst = new List<ReasonDirtyPopupViewData>();
            ReasonDirtyPopupViewData a = new ReasonDirtyPopupViewData();
            List<DDLDirtyReason> sl = new List<DDLDirtyReason>();
            DDLDirtyReason s1 = new DDLDirtyReason();
            s1.Reason = "";
            s1.HousekeepingDirtyReasonCode = "--Please select--";
            sl.Add(s1);
            List<DDLDirtyReason> ss = GetDirtyAllReason();
            a.Reasonlist = sl.Concat(ss).ToList();
            if (mode.Equals("enableopt"))
            {
                a.strQuestion = "Confirm update 'Opt Out' status for Room# " + roomNo + "?";
            }
            else if (mode.Equals("disableopt"))
            {
                a.strQuestion = "Confirm remove 'Opt Out' status for Room# " + roomNo + "?";
            }
            else if (mode.Equals("d"))
            {
                //strCheckMessage = BLL_Room.CheckValidToUpdateCleanOrDirtyByRoomNo(strProperty, roomNo);
                a.strQuestion = "Are you sure that you want to update Room# " + roomNo + " status as Dirty?";

            }
            Alllst.Add(a);
            return new ListResultDto<ReasonDirtyPopupViewData>(Alllst);
        }
        protected List<DDLReason> GetAllReason()
        {
            return _roomdalRepository.GetAllReason();
        }
        protected List<DDLDirtyReason> GetDirtyAllReason()
        {
            return _roomdalRepository.GetDirtyAllReason();
        }
        #region Bind Room Info & Confirmation Message
        public async Task<string> GetBindRoomInfoAndConfirmationMsg(string mode, string roomNo)
        {
            string strQuestion = "";

            if (mode.Equals("enable"))
            {
                strQuestion = "Confirm update 'Do Not Disturb' status for Room# " + roomNo + "?";
            }
            else if (mode.Equals("disable"))
            {
                strQuestion = "Confirm remove 'Do Not Disturb' from Room# " + roomNo + "?";
            }
            else if (mode.Equals("enableopt"))
            {
                strQuestion = "updated Room# " + roomNo + " as " + "Opt Out";
            }
            else if (mode.Equals("disableopt")) // enable/disable DND
            {

                strQuestion = "updated Room# " + roomNo + " as " + "Opt In";
            }

            return strQuestion;
        }
        #endregion
        public async Task btnUpdateDND(string strMode, string strRoomNo)
        {
            // Check whether this room valid to update 
            string strCheckMessage = "";
            int intDND = 0;
            DataTable dtRoom = _roomdalRepository.GetHotelRoomByRoomNo(strRoomNo);
            if (dtRoom.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dtRoom.Rows[0]["DND"].ToString()))
                {
                    intDND = Convert.ToInt32(dtRoom.Rows[0]["DND"]);
                }

                if (strMode.Equals("enable"))
                {
                    if (intDND == 1)
                        strCheckMessage = "Room DND status already updated. No Action Required.";
                }
                else
                {
                    if (intDND == 0)
                        strCheckMessage = "Room DND status already updated. No Action Required.";
                }
            }
            else
            {
                strCheckMessage = "No Record Found.";
            }
            if (string.IsNullOrEmpty(strCheckMessage))
            {
                var room = dalroom.GetbyUnit(strRoomNo);
                if (room.Count > 0)
                {
                    room[0].DND = strMode.Equals("enable") ? 1 : 0;
                }
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
                    CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(strMode, room[0], user);
                    var history = ObjectMapper.Map<History>(hi);
                    if (AbpSession.TenantId != null)
                    {
                        history.TenantId = (int?)AbpSession.TenantId;
                    }
                    history.SourceKey = room[0].Id;
                    int intSuccessful = dalhistory.SaveAsync(history).Result;
                    if (intSuccessful == 1)
                    {
                    }
                    else
                    {
                        throw new UserFriendlyException("Update Fail!");
                    }
                }
            }
            else
            {
                throw new UserFriendlyException(strCheckMessage);
            }

        }

        public async Task btnUpdateOpt(OptReasonM m)
        {
            string enabledisable = "";
            string enabledisableshort = "";
            if (m.strMode.Equals("enableopt"))
            {
                enabledisable = " Updated Room# " + m.strRoomNo + " as " + "enable opt out reason "+ m.ddlOptReasonSelectedText;
                enabledisableshort = "Opt Out enabled: [" + m.ddlOptReasonSelectedText + "]";
            }
            else if (m.strMode.Equals("disableopt"))
            {
               
                enabledisable = " Updated Room# " + m.strRoomNo + " as " + "disable opt out reason "+ m.ddlOptReasonSelectedText;
                enabledisableshort = "Opt Out disabled: [" + m.ddlOptReasonSelectedText + "]";
            }
           
                var room = dalroom.GetbyUnit(m.strRoomNo);
                if (room.Count > 0)
                {
                room[0].HMMNotes = enabledisableshort;// enabledisable;
                }
                else
                {
                throw new UserFriendlyException("No Record Found.");
                }
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
                    ReservationOptOutModel r = new ReservationOptOutModel();
                    r.ReservationOptOutKey = Guid.NewGuid();
                    if (m.ddlOptReasonSelectedValue != "")
                    {
                        r.ReservationOptOutCode = m.ddlOptReasonSelectedText;
                        r.ReservationOptOutReason = enabledisable +"("+ m.optReason + ")";
                    }
                    else
                    {
                        r.ReservationOptOutReason = enabledisable + "(" + m.optReason + ")";
                }
                    r.OptOut = DateTime.Now;
                    r.Unit = m.strRoomNo;
                    if (AbpSession.TenantId != null)
                    {
                        r.TenantId = (int?)AbpSession.TenantId;
                    }
                    r.AttendantID = user.StaffKey;

                    int intSuccessful = _roomdalRepository.UpdateOptReason(r);
                    if (intSuccessful == 1)
                    {
                        CreateOrEditHistoryDto hi = CommomData.GetOptReason(m.strMode, room[0], user, enabledisable);
                        var history = ObjectMapper.Map<History>(hi);
                        if (AbpSession.TenantId != null)
                        {
                            history.TenantId = (int?)AbpSession.TenantId;
                        }
                        history.SourceKey = room[0].Id;
                        history.LinkKey = r.ReservationOptOutKey;
                        int Successful = dalhistory.SaveAsync(history).Result;
                        if (Successful == 1)
                        {
                            //strCheckMessage = "Record(s) has been saved.";
                        }
                        else
                        {
                            throw new UserFriendlyException("Update Fail!");
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("Update Fail!");
                    }
                }
        }
        public ListResultDto<GetRoomStatus> GetBindRoomStatusButton()//BindRoomStatusButton();
        {
            var roomstatus = _roomstatusRepository.GetAll().OrderByDescending(s => s.Seq)
                                        .Select(s => new GetRoomStatus
                                        {
                                            RoomStatusKey = s.Id,
                                            RoomStatus = s.RoomStatusName
                                        })
                                        .ToList();

            return new ListResultDto<GetRoomStatus>(roomstatus);
        }
        public ListResultDto<GetGuestStatus> GetBindGuestStatusButton()//BindGuestStatusButton
        {
            var gueststatus = _gueststatusRepository.GetAll().OrderBy(s => s.Sort)
                       .Select(s => new GetGuestStatus
                       {
                           GuestStatusKey = s.Id,
                           StatusCode = s.StatusCode,
                           Status = s.Status
                       })
                       .ToList();

            return new ListResultDto<GetGuestStatus>(gueststatus);
        }
        public async Task<PagedResultDto<RoomStatusHotelRoomOutput>> GetBindHotelRoomButtonList(string SelectedRoomStatus = "ALL", string GuestStatus = "", string floorNo = "0")//BindHotelRoomButtonList(floorno);
        {
            List<RoomStatusHotelRoomOutput> dt = new List<RoomStatusHotelRoomOutput>();
            string[] list = null;
            //if (Session[SessionHelper.SessionMultiGuest] != null)
            //{
            //    var listGuest = Session[SessionHelper.SessionMultiGuest].ToString();
            //    list = listGuest.Split(',');
            //}
            DateTime searchDate = DateTime.Now;
            var d = _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
            if (d != null)
            {
                searchDate = d.Value;
            }
            string strRoomStatusKey = "";
            if (SelectedRoomStatus != "ALL")
            {
                strRoomStatusKey = _roomstatusRepository.GetAll().Where(x => x.RoomStatusName == SelectedRoomStatus).Select(x => x.Id).FirstOrDefault().ToString();
            }

            int floor = Convert.ToInt32(floorNo);

            dt = _roomdalRepository.GetHotelRoomByDateAndFloor(searchDate, floor, strRoomStatusKey, GuestStatus, list);

            var Count = dt.Count;
            //return new ListResultDto<RoomStatusHotelRoomOutput>(dt);
            return new PagedResultDto<RoomStatusHotelRoomOutput>(
               Count,
               dt
           );
        }
        public async Task<PagedResultDto<RoomStatusPageOutput>> GetBindGrid(string SelectedRoomStatus = "ALL", string GuestStatus = "", string floorNo = "0")
        {
            List<RoomStatusPageOutput> dt = new List<RoomStatusPageOutput>();
            string[] list = null;
            //if (Session[SessionHelper.SessionMultiGuest] != null)
            //{
            //    var listGuest = Session[SessionHelper.SessionMultiGuest].ToString();
            //    list = listGuest.Split(',');
            //}
            DateTime searchDate = DateTime.Now;
            var d = _controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
            if (d != null)
            {
                searchDate = d.Value;
            }
            string strRoomStatusKey = "";
            if (SelectedRoomStatus != "ALL")
            {
                strRoomStatusKey = _roomstatusRepository.GetAll().Where(x => x.RoomStatusName == SelectedRoomStatus).Select(x => x.Id).FirstOrDefault().ToString();
            }

            int floor = Convert.ToInt32(floorNo);

            dt = await _roomdalRepository.RoomStatusBindGrid(searchDate, floor, strRoomStatusKey, GuestStatus, list);

            var Count = dt.Count;
            //return new ListResultDto<RoomStatusHotelRoomOutput>(dt);
            return new PagedResultDto<RoomStatusPageOutput>(
               Count,
               dt
           );
        }

        [HttpPost]
        public async Task<string> btnSaveDndPhoto(DndImg input)
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("Session has expired.");
            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                int s = 0;
                #region img add
                    DndImage image = new DndImage();
                    image.DndphotoKey = Guid.NewGuid();
                    image.RoomKey = Guid.Parse(input.RoomKey);
                    image.Sort = input.Id;
                    image.DocumentName = input.ContentType;
                    image.LastModifiedStaff = user.StaffKey;
                    image.Signature = input.Data;
                //int exitcount = _roomdalRepository.CheckDndImage(image);
                //if (exitcount == 0)
                //{
                    s = _roomdalRepository.InsertDndImage(image);
                //}
                //else
                //{
                //    s = _roomdalRepository.UpdateDndImage(image);
                //}
                
                #endregion
                if (s > 0)
                {
                    message = "Record has been saved.";
                }
                else
                {
                    throw new UserFriendlyException("Fail to add Image.");
                }

            }
            return message;
        }

    }
}
