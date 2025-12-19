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
using NPOI.HPSF;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using PayPalCheckoutSdk.Orders;
using NPOI.XWPF.UserModel;

namespace BEZNgCore.IrepairAppService
{
    public class WorkOrderEntryAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IRepository<MArea, int> _areaRepository;
        private readonly IRepository<MWorkType, int> _mworktypeRepository;
        private readonly IRepository<MWorkOrder, int> _mworkorderRepository;
        private readonly IRepository<MaidStatus, Guid> _maidstatusRepository;
        private readonly IRepository<History, Guid> _historyRepository;
        private readonly IRepository<Control, Guid> _controlRepository;
        private readonly ISqoopeMessgingAppService _sqoopeint;
        private readonly ICommondalRepository _commondalRepository;
        //private readonly IMsgNotificationAppService _msgNotificationAppService;
        //private readonly RoomRepository _roomdalRepository;
        WorkOrderEnteryDAL dal;
        RoomDAL dalroom;
        HistoryDAL dalhistory;
        MaidStatusDAL dalmaidstatus;
        StaffDAL dalstaff;
        MareaDAL dalmarea;
        MWorkTypeDAL dalmworktype;
        public WorkOrderEntryAppService(
            IRepository<Staff, Guid> staffRepository,
            IRepository<Room, Guid> roomRepository,
            IRepository<MArea, int> areaRepository,
            IRepository<MWorkType, int> mworktypeRepository,
            IRepository<MWorkOrder, int> mworkorderRepository,
            IRepository<MaidStatus, Guid> maidstatusRepository,
            IRepository<History, Guid> historyRepository,
            IRepository<Control, Guid> controlRepository,
        ICommondalRepository commondalRepository,
            //RoomRepository roomdalRepository
            ISqoopeMessgingAppService sqoopeint//,IMsgNotificationAppService msgNotificationAppService
            )

        {
            _staffRepository = staffRepository;
            _roomRepository = roomRepository;
            _areaRepository = areaRepository;
            _mworktypeRepository = mworktypeRepository;
            _mworkorderRepository = mworkorderRepository;
            _maidstatusRepository = maidstatusRepository;
            _historyRepository = historyRepository;
            _controlRepository = controlRepository;
            dal = new WorkOrderEnteryDAL(_mworkorderRepository);
            dalroom = new RoomDAL(_roomRepository);
            dalhistory = new HistoryDAL(_historyRepository);
            dalmaidstatus = new MaidStatusDAL(_maidstatusRepository);
            dalstaff = new StaffDAL(_staffRepository);
            dalmarea = new MareaDAL(_areaRepository);
            dalmworktype = new MWorkTypeDAL(_mworktypeRepository);
            _commondalRepository = commondalRepository;
            //_roomdalRepository = roomdalRepository;
            _sqoopeint = sqoopeint;
            //_msgNotificationAppService = msgNotificationAppService;
        }
        #region bothuse iclean and i repair
        [HttpGet]
        public ListResultDto<WorkOrderEntryViewData> GetWorkOrderEntryViewData()
        {
            List<WorkOrderEntryViewData> Alllst = new List<WorkOrderEntryViewData>();
            WorkOrderEntryViewData a = new WorkOrderEntryViewData();
            a.GetReportedBy = dalstaff.GetAllData();
            a.Room = dalroom.GetBindDDLRoom();
            a.Area = dalmarea.GetAllData();
            a.WorkType = dalmworktype.GetAllData();
            Alllst.Add(a);
            return new ListResultDto<WorkOrderEntryViewData>(Alllst);
        }
        #endregion
        #region iclean
        public async Task<string> AddWorkOrderEntory(MWorkOrderInput input)//,string mode)//"mainreq"?"noreq"?
        {

            string message = "";
            string mode = input.mode;

            input.MWorkOrderKey = Guid.NewGuid();
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
                if (!string.IsNullOrEmpty(input.SelectedStaffKey))
                {
                    input.ReportedBy = new Guid(input.SelectedStaffKey);
                    var staffname = _staffRepository.GetAll().Where(x => x.Id == input.ReportedBy).Select(x =>x.UserName).FirstOrDefault();
                    if (staffname!=null)
                    {
                        input.StaffName = staffname;
                    }
                }
                else
                {
                    input.StaffName = user.UserName;
                    input.ReportedBy = user.StaffKey;
                }

                input.ReportedOn = input.ReportedOn.Value.Date;// DateTime.Today;

                if (input.RoomKey != null)
                {
                    if (mode.Equals("mainreq"))
                    {
                        var room = dalroom.GetbyId(input.RoomKey.Value);
                        if (room.Count > 0)
                        {
                            string status = "Maintenance Required";
                            room[0].MaidStatusKey = dalmaidstatus.GetMaidStatusKey(status);

                        }

                        if (!mode.Equals("mainreq") || !mode.Equals("noreq"))
                        {
                            CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(mode, room[0], user);
                            var history = ObjectMapper.Map<History>(hi);
                            if (AbpSession.TenantId != null)
                            {
                                history.TenantId = (int?)AbpSession.TenantId;
                            }
                            history.SourceKey = room[0].Id;
                            int Successful = dalhistory.SaveAsync(history).Result;
                            if (Successful > 0)
                            {
                                if (mode.Equals("e") || mode.Equals("delay"))
                                {
                                    var h = dalhistory.RetivehistoryforUpdate(room[0].Id);
                                    h[0].LinkKey = history.Id;
                                    // dalhistory.UpdateAsync(h[0]);
                                }
                            }

                        }
                    }
                    else
                    {

                        var room = dalroom.GetbyId(input.RoomKey.Value);
                        if (!mode.Equals("mainreq") || !mode.Equals("noreq"))
                        {
                            CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(mode, room[0], user);
                            var history = ObjectMapper.Map<History>(hi);
                            if (AbpSession.TenantId != null)
                            {
                                history.TenantId = (int?)AbpSession.TenantId;
                            }
                            history.SourceKey = room[0].Id;
                            int Successful = dalhistory.SaveAsync(history).Result;
                            if (Successful > 0)
                            {
                                if (mode.Equals("e") || mode.Equals("delay"))
                                {
                                    var h = dalhistory.RetivehistoryforUpdate(room[0].Id);
                                    h[0].LinkKey = history.Id;
                                    // dalhistory.UpdateAsync(h[0]);
                                }
                            }
                        }
                    }
                    //await ddlRoomChange(input.RoomKey.Value.ToString(), mode);
                }

                input.Description = input.Description.Length > 100 ? input.Description.Substring(0, 100) : input.Description;
                input.EnteredBy = user.UserName;
                input.EnteredStaffKey = user.StaffKey;
                //work.LastUpdateBy = user.Result.UserName;//no need
                int SqoopeWOID = 1;
                DateTime yesterday = DateTime.Now.AddDays(-1);

                var lst = dal.GetAllSeqno(yesterday);

                if (lst.Count > 0)
                {
                    SqoopeWOID = lst.Max(x => x.SqoopeWorkOrderID).Value + 1;
                }
                input.SqoopeWorkOrderID = SqoopeWOID;
                input.EnteredDateTime = DateTime.Now;
                input.Id = null;
                var o = ObjectMapper.Map<MWorkOrder>(input);
                if (AbpSession.TenantId != null)
                {
                    o.TenantId = (int?)AbpSession.TenantId;
                }
                int intSqoopeWOID = dal.SaveAsync(o).Result;
                if (intSqoopeWOID > 0)
                {
                    input.Id = intSqoopeWOID;
                    List<MWorkOrderInput> listWork = new List<MWorkOrderInput>();
                    input.LastUpdateBy = input.StaffName;//user.UserName;<=New Requirement
                    listWork.Add(input);
                    CreateOrEditHistoryDto j = new CreateOrEditHistoryDto();
                    j.StaffKey = user.StaffKey;
                    j.Sort = 0;
                    j.Sync = 0;
                    j.ModuleName = "iClean";
                    j.ChangedDate = DateTime.Now;
                    j.Operation = "I";
                    j.Id = null;
                    j.TableName = "WO";
                    j.Detail = "((iClean)) " + "WO#" + intSqoopeWOID + "; " + user.UserName + " added WO.";
                    j.Detail = (j.Detail.Trim().Length > 200 ? j.Detail.Trim().Substring(0, 190) + "..." : j.Detail.Trim());

                    var history = ObjectMapper.Map<History>(j);
                    if (AbpSession.TenantId != null)
                    {
                        history.TenantId = (int?)AbpSession.TenantId;
                    }
                    history.SourceKey = input.MWorkOrderKey;
                    history.NewValue = GetChangeLog(input); //input.NewLog;//
                    int su = dalhistory.SaveAsync(history).Result;
                    if (su > 0)
                    {
                        string success = "";
                        List<MessageNotiView> Alllst = new List<MessageNotiView>();
                        //List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
                        //Alllst = await _sqoopeint.SendiCleanWOMessageToSqoope(CommomData.MsgType_iClean_NewWorkOrder, listWork, null, user.StaffKey.ToString());
                        Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_NewWorkOrder, listWork, null, user.StaffKey.ToString());

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
                                vc.Title = "iREPAIR: NEW WO";//"iRepair:NEW_WORKORDER"; //"iClean:NEW_WORKORDER";
                                success = await SendNotiIRepairWoAsync(vc);
                            }
                        }
                        if(success== "success")
                        {
                            message = GetWorkOrderDetailLink(intSqoopeWOID.ToString()) + " has been added. ";
                        }
                        else
                        {
                            message = GetWorkOrderDetailLink(intSqoopeWOID.ToString()) + " has been added. "+"Noti Sent Fail";
                        }

                    }
                }
                else
                {
                    throw new UserFriendlyException("Fail to add the record.");
                }

            }
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
        private async Task<string> SendNotiIRepairWoAsync(MessageNotiWeb input)
        {
            string strReturnValue = "";
            if (input != null && !string.IsNullOrEmpty(input.Message) && input.to.Length > 0)
            {
                var credential = GoogleCredential.FromFile("irepairhgc-38cfab6bcfe7.json")//C:/inetpub/wwwroot/iRepairAPI/
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                string url = "https://fcm.googleapis.com/v1/projects/irepairhgc/messages:send";

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
        public async Task<string> AddWorkOrderEntoryWithImg(MWorkOrderImgInput input)
        {
            string message = "";
            
                string mode = input.wo.mode;

                input.wo.MWorkOrderKey = Guid.NewGuid();
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
                    if (!string.IsNullOrEmpty(input.wo.SelectedStaffKey))
                    {
                    input.wo.ReportedBy = new Guid(input.wo.SelectedStaffKey);
                    var staffname = _staffRepository.GetAll().Where(x => x.Id == input.wo.ReportedBy).Select(x => x.UserName).FirstOrDefault();
                    if (staffname != null)
                    {
                        input.wo.StaffName = staffname;
                    }
                    
                    }
                    else
                    {
                    input.wo.StaffName = user.UserName;
                    input.wo.ReportedBy = user.StaffKey;
                    }

                    input.wo.ReportedOn = input.wo.ReportedOn.Value.Date;// DateTime.Today;

                    if (input.wo.RoomKey != null)
                    {
                        if (mode.Equals("mainreq"))
                        {
                            var room = dalroom.GetbyId(input.wo.RoomKey.Value);
                            if (room.Count > 0)
                            {
                                string status = "Maintenance Required";
                                room[0].MaidStatusKey = dalmaidstatus.GetMaidStatusKey(status);

                            }

                            if (!mode.Equals("mainreq") || !mode.Equals("noreq"))
                            {
                                CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(mode, room[0], user);
                                var history = ObjectMapper.Map<History>(hi);
                                if (AbpSession.TenantId != null)
                                {
                                    history.TenantId = (int?)AbpSession.TenantId;
                                }
                                history.SourceKey = room[0].Id;
                                int Successful = dalhistory.SaveAsync(history).Result;
                                if (Successful > 0)
                                {
                                    if (mode.Equals("e") || mode.Equals("delay"))
                                    {
                                        var h = dalhistory.RetivehistoryforUpdate(room[0].Id);
                                        h[0].LinkKey = history.Id;
                                        // dalhistory.UpdateAsync(h[0]);
                                    }
                                }

                            }
                        }
                        else
                        {

                            var room = dalroom.GetbyId(input.wo.RoomKey.Value);
                            if (!mode.Equals("mainreq") || !mode.Equals("noreq"))
                            {
                                CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(mode, room[0], user);
                                var history = ObjectMapper.Map<History>(hi);
                                if (AbpSession.TenantId != null)
                                {
                                    history.TenantId = (int?)AbpSession.TenantId;
                                }
                                history.SourceKey = room[0].Id;
                                int Successful = dalhistory.SaveAsync(history).Result;
                                if (Successful > 0)
                                {
                                    if (mode.Equals("e") || mode.Equals("delay"))
                                    {
                                        var h = dalhistory.RetivehistoryforUpdate(room[0].Id);
                                        h[0].LinkKey = history.Id;
                                        // dalhistory.UpdateAsync(h[0]);
                                    }
                                }
                            }
                        }
                        //await ddlRoomChange(input.RoomKey.Value.ToString(), mode);
                    }

                    input.wo.Description = input.wo.Description.Length > 100 ? input.wo.Description.Substring(0, 100) : input.wo.Description;
                    input.wo.EnteredBy = user.UserName;
                    input.wo.EnteredStaffKey = user.StaffKey;
                    //work.LastUpdateBy = user.Result.UserName;//no need
                    int SqoopeWOID = 1;
                    DateTime yesterday = DateTime.Now.AddDays(-1);

                    var lst = dal.GetAllSeqno(yesterday);

                    if (lst.Count > 0)
                    {
                        SqoopeWOID = lst.Max(x => x.SqoopeWorkOrderID).Value + 1;
                    }
                    input.wo.SqoopeWorkOrderID = SqoopeWOID;
                    input.wo.EnteredDateTime = DateTime.Now;
                    input.wo.Id = null;
                    var o = ObjectMapper.Map<MWorkOrder>(input.wo);
                    if (AbpSession.TenantId != null)
                    {
                        o.TenantId = (int?)AbpSession.TenantId;
                    }
                    int intSqoopeWOID = dal.SaveAsync(o).Result;
                    if (intSqoopeWOID > 0)
                    {
                    input.wo.Id = intSqoopeWOID;
                    List<MWorkOrderInput> listWork = new List<MWorkOrderInput>();
                    input.wo.LastUpdateBy = input.wo.StaffName;//user.UserName;<=New Requirement
                    listWork.Add(input.wo);
                    CreateOrEditHistoryDto j = new CreateOrEditHistoryDto();
                        j.StaffKey = user.StaffKey;
                        j.Sort = 0;
                        j.Sync = 0;
                        j.ModuleName = "iClean";
                        j.ChangedDate = DateTime.Now;
                        j.Operation = "I";
                        j.Id = null;
                        j.TableName = "WO";
                        j.Detail = "((iClean)) " + "WO#" + intSqoopeWOID + "; " + user.UserName + " added WO.";
                        j.Detail = (j.Detail.Trim().Length > 200 ? j.Detail.Trim().Substring(0, 190) + "..." : j.Detail.Trim());

                        var history = ObjectMapper.Map<History>(j);
                        if (AbpSession.TenantId != null)
                        {
                            history.TenantId = (int?)AbpSession.TenantId;
                        }
                        history.SourceKey = input.wo.MWorkOrderKey;
                        history.NewValue = GetChangeLog(input.wo); //input.NewLog;//
                        int su = dalhistory.SaveAsync(history).Result;
                        if (su > 0)
                        {
                        if (input.imglst.Count > 0)
                        {
                            int s = 0;
                            #region img add

                            foreach (FWOImage dr in input.imglst)
                            {
                                WOImage image = new WOImage();
                                image.DocumentKey = Guid.NewGuid();
                                image.Sort = dr.Id;
                                image.LastModifiedStaff = user.StaffKey;
                                image.Description = dr.FileName;
                                image.DocumentName = dr.ContentType;
                                image.MWorkOrderKey = input.wo.MWorkOrderKey;
                                image.Signature = dr.Data;
                                s = _commondalRepository.InsertWOImage(image);

                            }
                            #endregion
                            if (s > 0)
                            {
                                string success = "";
                                List<MessageNotiView> Alllst = new List<MessageNotiView>();
                                //List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
                                //Alllst = await _sqoopeint.SendiCleanWOMessageToSqoope(CommomData.MsgType_iClean_NewWorkOrder, listWork, null, user.StaffKey.ToString());
                                Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_NewWorkOrder, listWork, null, user.StaffKey.ToString());

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
                                        vc.Title = "iREPAIR: NEW WO";//"iRepair:NEW_WORKORDER"; //"iClean:NEW_WORKORDER";
                                        success = await SendNotiIRepairWoAsync(vc);
                                    }
                                }
                                if (success == "success")
                                {
                                    message = GetWorkOrderDetailLink(intSqoopeWOID.ToString()) + " has been added. ";
                                }
                                else
                                {
                                    message = GetWorkOrderDetailLink(intSqoopeWOID.ToString()) + " has been added. " + "Noti Sent Fail";
                                }

                                
                            }
                            else
                            {
                                throw new UserFriendlyException("Fail to add Image.");
                            }
                        }
                        else
                        {
                            string success = "";
                            List<MessageNotiView> Alllst = new List<MessageNotiView>();
                            //List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
                            //Alllst = await _sqoopeint.SendiCleanWOMessageToSqoope(CommomData.MsgType_iClean_NewWorkOrder, listWork, null, user.StaffKey.ToString());
                            Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_NewWorkOrder, listWork, null, user.StaffKey.ToString());

                            foreach (var v in Alllst)
                            {
                               
                                MessageNotiWeb vc = new MessageNotiWeb();
                                if (v.ToStaffList.Count > 0)
                                {
                                    //string[] too = v.ToStaffList.DistinctBy(x => x.to).Select(x => x.to).ToList().Where(x => x != "" ).ToArray();
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
                                    vc.Title = "iREPAIR: NEW WO";//"iRepair:NEW_WORKORDER"; //"iClean:NEW_WORKORDER";
                                    success = await SendNotiIRepairWoAsync(vc);
                                }
                            }
                            if (success == "success")
                            {
                                message = GetWorkOrderDetailLink(intSqoopeWOID.ToString()) + " has been added. ";
                            }
                            else
                            {
                                message = GetWorkOrderDetailLink(intSqoopeWOID.ToString()) + " has been added. " + "Noti Sent Fail";
                            }

                        }
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to add the record.");
                    }

                }
            
            return message;
        }
        private string GetChangeLog(MWorkOrderInput input)
        {
            try
            {
                string strLog = "";
                StringBuilder sb = new StringBuilder();

                sb.Append(" Desc.=> " + input.Description);

                if (!string.IsNullOrEmpty(input.Notes))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Notes=> " + input.Notes);
                }
                if (!string.IsNullOrEmpty(input.Room))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Room=> " + input.Room);
                }
                if (input.MArea != null && input.MArea != 0)
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Area=> " + input.MAreaDesc);
                }
                if (input.MWorkType != null && input.MWorkType != 0)
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" WorkType=> " + input.MWorkTypeDesc);
                }


                sb.Append(", ReportedBy=> " + input.StaffName);
                sb.Append(", ReportedDate=> " + Convert.ToDateTime(input.ReportedOn).ToString("dd/MM/yyyy"));
                sb.Append(", Status=> " + input.MWorkOrderStatusDesc);

                strLog = sb.ToString();

                return strLog;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GetWorkOrderDetailLink(string woID)
        {
            try
            {
                string strRetunValue = "";
                strRetunValue = "Work Order#" + woID;
                return strRetunValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region irepair
        public async Task<List<MessageNotiViewLatest>> AddIRWorkOrderEntory(MWorkOrderInput input)//,string mode)//"mainreq"?"noreq"?
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            string message = "";
            string mode = input.mode;

            input.MWorkOrderKey = Guid.NewGuid();
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
                if (!string.IsNullOrEmpty(input.SelectedStaffKey))
                {
                    input.ReportedBy = new Guid(input.SelectedStaffKey);
                    var staffname = _staffRepository.GetAll().Where(x => x.Id == input.ReportedBy).Select(x => x.UserName).FirstOrDefault();
                    if (staffname != null)
                    {
                        input.StaffName = staffname;
                    }
                    
                }
                else
                {
                    input.StaffName = user.UserName;
                    input.ReportedBy = user.StaffKey;
                }

                input.ReportedOn = input.ReportedOn.Value.Date;// DateTime.Today;

                if (input.RoomKey != null)
                {
                    if (mode.Equals("mainreq"))
                    {
                        var room = dalroom.GetbyId(input.RoomKey.Value);
                        if (room.Count > 0)
                        {
                            string status = "Maintenance Required";
                            room[0].MaidStatusKey = dalmaidstatus.GetMaidStatusKey(status);

                        }

                        //if (!mode.Equals("mainreq") || !mode.Equals("noreq"))
                        //{
                        //    CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(mode, room[0], user);
                        //    var history = ObjectMapper.Map<History>(hi);
                        //    if (AbpSession.TenantId != null)
                        //    {
                        //        history.TenantId = (int?)AbpSession.TenantId;
                        //    }
                        //    history.SourceKey = room[0].Id;
                        //    int Successful = dalhistory.SaveAsync(history).Result;
                        //    if (Successful > 0)
                        //    {
                        //        if (mode.Equals("e") || mode.Equals("delay"))
                        //        {
                        //            var h = dalhistory.RetivehistoryforUpdate(room[0].Id);
                        //            h[0].LinkKey = history.Id;
                        //            // dalhistory.UpdateAsync(h[0]);
                        //        }
                        //    }

                        //}
                    }
                    //else
                    //{

                    //    var room = dalroom.GetbyId(input.RoomKey.Value);
                    //    if (!mode.Equals("mainreq") || !mode.Equals("noreq"))
                    //    {
                    //        CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(mode, room[0], user);
                    //        var history = ObjectMapper.Map<History>(hi);
                    //        if (AbpSession.TenantId != null)
                    //        {
                    //            history.TenantId = (int?)AbpSession.TenantId;
                    //        }
                    //        history.SourceKey = room[0].Id;
                    //        int Successful = dalhistory.SaveAsync(history).Result;
                    //        if (Successful > 0)
                    //        {
                    //            if (mode.Equals("e") || mode.Equals("delay"))
                    //            {
                    //                var h = dalhistory.RetivehistoryforUpdate(room[0].Id);
                    //                h[0].LinkKey = history.Id;
                    //                // dalhistory.UpdateAsync(h[0]);
                    //            }
                    //        }
                    //    }
                    //}
                    //await ddlRoomChange(input.RoomKey.Value.ToString(), mode);
                }

                input.Description = input.Description.Length > 100 ? input.Description.Substring(0, 100) : input.Description;
                input.Notes = input.Notes.Length > 2000 ? input.Notes.Substring(0, 2000) : input.Notes;
                input.EnteredBy = user.UserName;
                input.EnteredStaffKey = user.StaffKey;
                //input.LastUpdateBy = user.Result.UserName;//need for update WO
                int SqoopeWOID = 1;
                DateTime yesterday = DateTime.Now.AddDays(-1);

                var lst = dal.GetAllSeqno(yesterday);

                if (lst.Count > 0)
                {
                    SqoopeWOID = lst.Max(x => x.SqoopeWorkOrderID).Value + 1;
                }
                input.SqoopeWorkOrderID = SqoopeWOID;
                input.EnteredDateTime = DateTime.Now;
                input.Id = null;
                var o = ObjectMapper.Map<MWorkOrder>(input);
                if (AbpSession.TenantId != null)
                {
                    o.TenantId = (int?)AbpSession.TenantId;
                }
                int intSqoopeWOID = dal.SaveAsync(o).Result;
                if (intSqoopeWOID > 0)
                {
                    input.Id = intSqoopeWOID;
                    List<MWorkOrderInput> listWork = new List<MWorkOrderInput>();
                    input.LastUpdateBy = input.StaffName;//user.UserName;<=New Requirement
                    listWork.Add(input);
                    //  SqoopeMessgingHelper.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_NewWorkOrder, listWork, null, user.Result.StaffKey.ToString());
                    CreateOrEditHistoryDto j = new CreateOrEditHistoryDto();
                    j.StaffKey = user.StaffKey;
                    j.Sort = 0;
                    j.Sync = 0;
                    j.ModuleName = "iRepair";
                    j.ChangedDate = DateTime.Now;
                    j.Operation = "I";
                    j.Id = null;
                    j.TableName = "WO";
                    j.Detail = "(iRepair) " + "WO#" + intSqoopeWOID + "; " + user.UserName + " added WO.";
                    j.Detail = (j.Detail.Trim().Length > 200 ? j.Detail.Trim().Substring(0, 190) + "..." : j.Detail.Trim());

                    var history = ObjectMapper.Map<History>(j);
                    if (AbpSession.TenantId != null)
                    {
                        history.TenantId = (int?)AbpSession.TenantId;
                    }
                    history.SourceKey = input.MWorkOrderKey;
                    history.NewValue = GetChangeLog(input); //input.NewLog;//
                    int su = dalhistory.SaveAsync(history).Result;

                    if (su > 0)
                    {
                        Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_NewWorkOrder, listWork, null, user.StaffKey.ToString());
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
                        //message = GetWorkOrderIRDetailLink(intSqoopeWOID.ToString()) + " has been added. ";
                    }
                    else
                    {
                        a.Message = "Fail to add the record.";
                        Alllst.Add(a);
                    }
                }
                else
                {
                    throw new UserFriendlyException("Fail to add the record.");
                }

            }
            // return message;
            return Alllstlatest;
        }
        public async Task<List<MessageNotiViewLatest>> AddIRWorkOrderEntoryWithImg(MWorkOrderImgInput input)//,string mode)//"mainreq"?"noreq"?
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            string message = "";
            string mode = input.wo.mode;

            input.wo.MWorkOrderKey = Guid.NewGuid();
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
                if (!string.IsNullOrEmpty(input.wo.SelectedStaffKey))
                {
                    input.wo.ReportedBy = new Guid(input.wo.SelectedStaffKey);
                    var staffname = _staffRepository.GetAll().Where(x => x.Id == input.wo.ReportedBy).Select(x => x.UserName).FirstOrDefault();
                    if (staffname != null)
                    {
                        input.wo.StaffName = staffname;
                    }
                    
                }
                else
                {
                    input.wo.StaffName = user.UserName;
                    input.wo.ReportedBy = user.StaffKey;
                }

                input.wo.ReportedOn = input.wo.ReportedOn.Value.Date;// DateTime.Today;

                if (input.wo.RoomKey != null)
                {
                    if (mode.Equals("mainreq"))
                    {
                        var room = dalroom.GetbyId(input.wo.RoomKey.Value);
                        if (room.Count > 0)
                        {
                            string status = "Maintenance Required";
                            room[0].MaidStatusKey = dalmaidstatus.GetMaidStatusKey(status);

                        }

                        //if (!mode.Equals("mainreq") || !mode.Equals("noreq"))
                        //{
                        //    CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(mode, room[0], user);
                        //    var history = ObjectMapper.Map<History>(hi);
                        //    if (AbpSession.TenantId != null)
                        //    {
                        //        history.TenantId = (int?)AbpSession.TenantId;
                        //    }
                        //    history.SourceKey = room[0].Id;
                        //    int Successful = dalhistory.SaveAsync(history).Result;
                        //    if (Successful > 0)
                        //    {
                        //        if (mode.Equals("e") || mode.Equals("delay"))
                        //        {
                        //            var h = dalhistory.RetivehistoryforUpdate(room[0].Id);
                        //            h[0].LinkKey = history.Id;
                        //            // dalhistory.UpdateAsync(h[0]);
                        //        }
                        //    }

                        //}
                    }
                    //else
                    //{

                    //    var room = dalroom.GetbyId(input.RoomKey.Value);
                    //    if (!mode.Equals("mainreq") || !mode.Equals("noreq"))
                    //    {
                    //        CreateOrEditHistoryDto hi = CommomData.GetRoomStatusChangeHistory(mode, room[0], user);
                    //        var history = ObjectMapper.Map<History>(hi);
                    //        if (AbpSession.TenantId != null)
                    //        {
                    //            history.TenantId = (int?)AbpSession.TenantId;
                    //        }
                    //        history.SourceKey = room[0].Id;
                    //        int Successful = dalhistory.SaveAsync(history).Result;
                    //        if (Successful > 0)
                    //        {
                    //            if (mode.Equals("e") || mode.Equals("delay"))
                    //            {
                    //                var h = dalhistory.RetivehistoryforUpdate(room[0].Id);
                    //                h[0].LinkKey = history.Id;
                    //                // dalhistory.UpdateAsync(h[0]);
                    //            }
                    //        }
                    //    }
                    //}
                    //await ddlRoomChange(input.RoomKey.Value.ToString(), mode);
                }

                input.wo.Description = input.wo.Description.Length > 100 ? input.wo.Description.Substring(0, 100) : input.wo.Description;
                input.wo.Notes = input.wo.Notes.Length > 2000 ? input.wo.Notes.Substring(0, 2000) : input.wo.Notes;
                input.wo.EnteredBy = user.UserName;
                input.wo.EnteredStaffKey = user.StaffKey;
                //input.LastUpdateBy = user.Result.UserName;//need for update WO
                int SqoopeWOID = 1;
                DateTime yesterday = DateTime.Now.AddDays(-1);

                var lst = dal.GetAllSeqno(yesterday);

                if (lst.Count > 0)
                {
                    SqoopeWOID = lst.Max(x => x.SqoopeWorkOrderID).Value + 1;
                }
                input.wo.SqoopeWorkOrderID = SqoopeWOID;
                input.wo.EnteredDateTime = DateTime.Now;
                input.wo.Id = null;
                var o = ObjectMapper.Map<MWorkOrder>(input.wo);
                if (AbpSession.TenantId != null)
                {
                    o.TenantId = (int?)AbpSession.TenantId;
                }
                int intSqoopeWOID = dal.SaveAsync(o).Result;
                if (intSqoopeWOID > 0)
                {
                    input.wo.Id = intSqoopeWOID;
                    List<MWorkOrderInput> listWork = new List<MWorkOrderInput>();
                    input.wo.LastUpdateBy = input.wo.StaffName;//user.UserName;<=New Requirement
                    listWork.Add(input.wo);
                    //  SqoopeMessgingHelper.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_NewWorkOrder, listWork, null, user.Result.StaffKey.ToString());
                    CreateOrEditHistoryDto j = new CreateOrEditHistoryDto();
                    j.StaffKey = user.StaffKey;
                    j.Sort = 0;
                    j.Sync = 0;
                    j.ModuleName = "iRepair";
                    j.ChangedDate = DateTime.Now;
                    j.Operation = "I";
                    j.Id = null;
                    j.TableName = "WO";
                    j.Detail = "(iRepair) " + "WO#" + intSqoopeWOID + "; " + user.UserName + " added WO.";
                    j.Detail = (j.Detail.Trim().Length > 200 ? j.Detail.Trim().Substring(0, 190) + "..." : j.Detail.Trim());

                    var history = ObjectMapper.Map<History>(j);
                    if (AbpSession.TenantId != null)
                    {
                        history.TenantId = (int?)AbpSession.TenantId;
                    }
                    history.SourceKey = input.wo.MWorkOrderKey;
                    history.NewValue = GetChangeLog(input.wo); //input.NewLog;//
                    int su = dalhistory.SaveAsync(history).Result;

                    if (su > 0)
                    {
                        if (input.imglst.Count > 0)
                        {
                            int s = 0;
                            #region img add

                            foreach (FWOImage dr in input.imglst)
                            {
                                WOImage image = new WOImage();
                                image.DocumentKey = Guid.NewGuid();
                                image.Sort = dr.Id;
                                image.LastModifiedStaff = user.StaffKey;
                                image.Description = dr.FileName;
                                image.DocumentName = dr.ContentType;
                                image.MWorkOrderKey = input.wo.MWorkOrderKey;
                                image.Signature = dr.Data;
                                s = _commondalRepository.InsertWOImage(image);

                            }
                            #endregion
                            if (s > 0)
                            {
                            }
                            else
                            {
                                throw new UserFriendlyException("Fail to add Image.");
                            }
                        }

                        
                            Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_NewWorkOrder, listWork, null, user.StaffKey.ToString());
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
                            //message = GetWorkOrderIRDetailLink(intSqoopeWOID.ToString()) + " has been added. ";
                        
                    }
                    else
                    {
                        a.Message = "Fail to add the record.";
                        Alllst.Add(a);
                    }
                }
                else
                {
                    throw new UserFriendlyException("Fail to add the record.");
                }

            }
            // return message;
            return Alllstlatest;
        }
        private string GetWorkOrderIRDetailLink(string woID)
        {
            try
            {
                string strRetunValue = "";
                strRetunValue = "<a href='ViewWorkOrderDetail.aspx?id=" + woID + "'> Work Order#" + woID + "</a>";
                return strRetunValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region commom
        [HttpGet]
        public ListResultDto<ViewSystemDtae> GetViewSystemDate()
        {
            List<ViewSystemDtae> Alllst = new List<ViewSystemDtae>();
            ViewSystemDtae a = new ViewSystemDtae();

            DateTime dtBusinessDate = DateTime.Now;
            dtBusinessDate = (DateTime)_controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
            a.SystemDate = dtBusinessDate;

            Alllst.Add(a);
            return new ListResultDto<ViewSystemDtae>(Alllst);
        }
        #endregion
    }
}
