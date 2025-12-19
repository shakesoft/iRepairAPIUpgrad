using Abp.Domain.Repositories;
using Abp.UI;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.Common;
using BEZNgCore.Configuration;
using BEZNgCore.IrepairAppService.DAL;
using BEZNgCore.IRepairIAppService;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class SqoopeMessgingAppService : BEZNgCoreAppServiceBase, ISqoopeMessgingAppService
    {

        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly IRepository<MTechnician, int> _mtechnicanRepository;
        private readonly IRepository<GeneralProfile, Guid> _generalprofileRepository;
        private readonly IRepository<SqoopeMsgType, Guid> _sqoopemsgtypeRepository;
        //private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IRepository<SqoopeMsgLog, Guid> _sqoopemsglogRepository;
        private readonly RoomRepository _roomdalRepository;
        StaffDAL dalstaff;
        GeneralProfileDAL dalgeneralprofile;
        SqoopeMsgTypeDAL dalsqoopemsgtype;

        private readonly IAppConfigurationAccessor _configurationAccessor;
        public SqoopeMessgingAppService(IRepository<Staff, Guid> staffRepository,
             IRepository<MTechnician, int> mtechnicanRepository,
             IRepository<GeneralProfile, Guid> generalprofileRepository,
             IRepository<SqoopeMsgType, Guid> sqoopemsgtypeRepository,
             //IRepository<Room, Guid> roomRepository,
             IRepository<SqoopeMsgLog, Guid> sqoopemsglogRepository,
             RoomRepository roomdalRepository,
             IAppConfigurationAccessor configurationAccessor
             )
        {
            _staffRepository = staffRepository;
            _mtechnicanRepository = mtechnicanRepository;
            dalstaff = new StaffDAL(_staffRepository);
            _generalprofileRepository = generalprofileRepository;
            dalgeneralprofile = new GeneralProfileDAL(_generalprofileRepository);
            _sqoopemsgtypeRepository = sqoopemsgtypeRepository;
            dalsqoopemsgtype = new SqoopeMsgTypeDAL(_sqoopemsgtypeRepository);
            //_roomRepository = roomRepository;
            _sqoopemsglogRepository = sqoopemsglogRepository;
            _roomdalRepository = roomdalRepository;
            _configurationAccessor = configurationAccessor;
        }

        #region iClean - Messaging Helper functions
        #region Helper
        public string GetSqoopeAuthentication()
        {
            string strSqoopeAuth = "";
            try
            {

                strSqoopeAuth = "client_id=" + _configurationAccessor.Configuration["client_id"] + "&auth=" + _configurationAccessor.Configuration["auth"];

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
            }
            return strSqoopeAuth;
        }

        public static SqoopeResponse GetSqoopeResponse(string sqoopeResponse)
        {
            SqoopeResponse response = new SqoopeResponse();
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                response = js.Deserialize<SqoopeResponse>(sqoopeResponse);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
            }
            return response;
        }


        #endregion
        #region Main Sqoope Request Service
        private static string SendAndGetResponseSqoopeService(string sqoopeURL, string postData)
        {
            string strResponseMessage = "";
            try
            {
                //var request = (HttpWebRequest)WebRequest.Create("http://192.168.10.10/apicontact/http/get");
                //var request = (HttpWebRequest)WebRequest.Create("http://192.168.10.10/apimessage/http/post");
                //var request = (HttpWebRequest)WebRequest.Create("http://192.168.10.10/apimessage/sqmessage/http");
                var request = (HttpWebRequest)WebRequest.Create(sqoopeURL);

                //var postData = "auth=8878r1d80T0BDin7EcKkmW7KVrSzqV&contact_id=190&mid=1";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                strResponseMessage = responseString.ToString();
            }
            catch (Exception ex)
            {
                return strResponseMessage;
                // throw new UserFriendlyException("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
            }
            return strResponseMessage;
        }
        #endregion

        #endregion

        #region Message Helper
        private string GetSqoopeMessageByMsgCode(string msgCode, string roomNo, string attendant, string previousAttendant)
        {
            string strReturnValue = "";

            try
            {
                strReturnValue = dalsqoopemsgtype.GetMessageTemplateByMsgCode(msgCode);
                strReturnValue = strReturnValue.Replace("#ROOM_NO#", roomNo);
                if (msgCode.Equals(CommomData.MsgType_iClean_AssignAttendant))
                    strReturnValue = strReturnValue.Replace("#ROOM_ASSIGNMENT_INFO#", GetRoomAssignmentMessage(attendant, previousAttendant));
                return strReturnValue;
            }
            catch (Exception ex)
            {
                return strReturnValue;
                //throw new UserFriendlyException(ex.Message);
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                //throw ex;
            }
        }

        /*  Room Assignment 
         *  1. Attendant != "" && PreviousAttant == "" : assign
         *  2. Attendant != "" && PreviousAttant != "" : reassign
         *  3. Attendant == "" && PreviousAttant != "" : unassign
         */

        private static string GetRoomAssignmentMessage(string attendant, string previousAttendant)
        {
            string strReturnValue = "";

            try
            {
                if (!string.IsNullOrEmpty(attendant) && string.IsNullOrEmpty(previousAttendant))
                {
                    strReturnValue = "is assigned to " + attendant;
                }
                else if (!string.IsNullOrEmpty(attendant) && !string.IsNullOrEmpty(previousAttendant))
                {
                    strReturnValue = "is reassigned from " + previousAttendant + " to " + attendant;
                }
                else if (string.IsNullOrEmpty(attendant) && !string.IsNullOrEmpty(previousAttendant))
                {
                    strReturnValue = "is unassigned from " + previousAttendant;
                }

                return strReturnValue;
            }
            catch (Exception ex)
            {
                return strReturnValue;
                // throw new UserFriendlyException(ex.Message);
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                //throw ex;
            }
        }


        private SqoopeMsgLog GetSqoopeMessageForRoomDirty(Staff loginStaff, string msgCode, string roomNo, string strSqoopeMessage)
        {
            SqoopeMsgLog msg = new SqoopeMsgLog();
            try
            {
                Staff staffMaid = GetStaffInfoByRoomNo(roomNo);
                //StaffInfoBySqoope staffMaid = GetStaffInfoByRoomNo(roomNo);
                string strMessageKey = dalsqoopemsgtype.GetMessageKeyByMsgCode(msgCode);

                if (!string.IsNullOrEmpty(strMessageKey) && staffMaid.Id != Guid.Empty)//staffMaid.Contact_Id != null && staffMaid.Contact_Id != "0" && 
                {
                    msg = new SqoopeMsgLog();
                    msg.Id = Guid.NewGuid();
                    msg.SqoopeMessageKey = Guid.Parse(strMessageKey);
                    if (loginStaff.Contact_Id != null)
                        msg.FromContactId = Convert.ToInt32(loginStaff.Contact_Id);
                    if (loginStaff.Id != null)
                        msg.CreatedBy = loginStaff.Id;
                    if (!string.IsNullOrEmpty(staffMaid.Contact_Id))
                        msg.ToContactId = Convert.ToInt32(staffMaid.Contact_Id);
                    msg.ToStaffKey = staffMaid.Id;
                    if (!string.IsNullOrEmpty(staffMaid.FirebaseToken_Id))
                        msg.FirebaseToken_Id = staffMaid.FirebaseToken_Id;
                    msg.Msg = strSqoopeMessage;
                }
                return msg;
            }
            catch (Exception ex)
            {
                return msg;
                //  throw new UserFriendlyException(ex.Message);
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                //throw ex;
            }
        }

        private SqoopeMsgLog GetSqoopeMessageForRoomAssignment(Staff loginStaff, string msgCode, string roomNo, string strSqoopeMessage)
        {
            SqoopeMsgLog msg = new SqoopeMsgLog();
            try
            {
                //StaffInfoBySqoope staffMaid = GetStaffInfoByRoomNo(roomNo);
                Staff staffMaid = GetStaffInfoByRoomNo(roomNo);
                string strMessageKey = dalsqoopemsgtype.GetMessageKeyByMsgCode(msgCode);
                string strMessageTemplate = dalsqoopemsgtype.GetMessageTemplateByMsgCode(msgCode);

                if (!string.IsNullOrEmpty(strMessageKey) && staffMaid.Id != Guid.Empty)//staffMaid.Contact_Id != null && staffMaid.Contact_Id != "0" &&
                {
                    strMessageTemplate = strMessageTemplate.Replace("#ROOM_NO#", roomNo);
                    strMessageTemplate = strMessageTemplate.Replace("#ROOM_ASSIGNMENT_INFO#", "is assigned to you");

                    msg = new SqoopeMsgLog();
                    msg.Id = Guid.NewGuid();
                    msg.SqoopeMessageKey = Guid.Parse(strMessageKey);
                    if (loginStaff.Contact_Id != null)
                        msg.FromContactId = Convert.ToInt32(loginStaff.Contact_Id);
                    if (loginStaff.Id != null)
                        msg.CreatedBy = loginStaff.Id;
                    if (!string.IsNullOrEmpty(staffMaid.Contact_Id))
                        msg.ToContactId = Convert.ToInt32(staffMaid.Contact_Id);
                    msg.ToStaffKey = staffMaid.Id;
                    if (!string.IsNullOrEmpty(staffMaid.FirebaseToken_Id))
                        msg.FirebaseToken_Id = staffMaid.FirebaseToken_Id;
                    msg.Msg = strMessageTemplate;
                }
                return msg;
            }
            catch (Exception ex)
            {
                return msg;
                //throw new UserFriendlyException(ex.Message);
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                //throw ex;
            }
        }

        private Staff GetStaffInfoByRoomNo(string roomNo)
        {

            Staff staff = new Staff();
            DataTable dt = null;
            try
            {
                dt = _roomdalRepository.GetStaffInfoByRoomNo(roomNo);
                foreach (DataRow dr in dt.Rows)
                {
                    staff.Id = new Guid(dr["StaffKey"].ToString());
                    staff.UserName = Convert.IsDBNull(dr["Username"]) ? null : Convert.ToString(dr["Username"]);
                    staff.Active = string.IsNullOrEmpty(dr["Active"].ToString()) ? 0 : Convert.ToInt32(dr["Active"]);
                    staff.Sec_Supervisor = string.IsNullOrEmpty(dr["Sec_Supervisor"].ToString()) ? 0 : Convert.ToInt32(dr["Sec_Supervisor"]);
                    staff.Contact_Id = string.IsNullOrEmpty(dr["Contact_Id"].ToString()) ? "0" : Convert.ToString(dr["Contact_Id"]);
                    if (!string.IsNullOrEmpty(dr["MaidKey"].ToString()))
                        staff.MaidKey = new Guid(dr["MaidKey"].ToString());
                    if (!string.IsNullOrEmpty(dr["FirebaseToken_Id"].ToString()))
                        staff.FirebaseToken_Id = string.IsNullOrEmpty(dr["FirebaseToken_Id"].ToString()) ? "" : Convert.ToString(dr["FirebaseToken_Id"]);
                }
                //st = (from s in _staffRepository.GetAll()
                //                   join r in _roomRepository.GetAll() on s.MaidKey equals r.Id into t
                //                   from rt in t.DefaultIfEmpty()
                //                   where rt.Unit == roomNo
                //                   select new StaffInfoBySqoope 
                //                   {
                //                       StaffKey=s.Id,
                //                       UserName=s.UserName,
                //                       Active=s.Active,
                //                       Sec_Supervisor=s.Sec_Supervisor,
                //                       Contact_ID=s.Contact_Id,
                //                       MaidKey=s.MaidKey
                //                   }).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return staff;

        }

        private SqoopeMsgLog GetSqoopeMessageForRoomUnAssignment(Staff loginStaff, string msgCode, string roomNo, Staff previousAttendant)
        {
            SqoopeMsgLog msg = new SqoopeMsgLog();
            try
            {
                //Staff staffMaid = BLL_Staff.GetStaffInfoByRoomNo(propertyCode, roomNo);
                string strMessageKey = dalsqoopemsgtype.GetMessageKeyByMsgCode(msgCode);
                string strMessageTemplate = dalsqoopemsgtype.GetMessageTemplateByMsgCode(msgCode);

                if (!string.IsNullOrEmpty(strMessageKey) && previousAttendant.Id != Guid.Empty)//previousAttendant.Contact_Id != null && previousAttendant.Contact_Id != "0" && 
                {
                    strMessageTemplate = strMessageTemplate.Replace("#ROOM_NO#", roomNo);
                    strMessageTemplate = strMessageTemplate.Replace("#ROOM_ASSIGNMENT_INFO#", "is unassigned from you");

                    msg = new SqoopeMsgLog();
                    msg.Id = Guid.NewGuid();
                    msg.SqoopeMessageKey = Guid.Parse(strMessageKey);
                    if (loginStaff.Contact_Id != null)
                        msg.FromContactId = Convert.ToInt32(loginStaff.Contact_Id);
                    if (loginStaff.Id != null)
                        msg.CreatedBy = loginStaff.Id;
                    if (!string.IsNullOrEmpty(previousAttendant.Contact_Id))
                        msg.ToContactId = Convert.ToInt32(previousAttendant.Contact_Id);
                    msg.ToStaffKey = previousAttendant.Id;
                    if (!string.IsNullOrEmpty(previousAttendant.FirebaseToken_Id))
                        msg.FirebaseToken_Id = previousAttendant.FirebaseToken_Id;
                    msg.Msg = strMessageTemplate;
                }
                return msg;
            }
            catch (Exception ex)
            {
                return msg;
                // throw new UserFriendlyException(ex.Message);
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                // throw ex;
            }
        }

        public async Task<List<MessageNotiView>> SendMessageToSqoope(string msgCode, string roomNo, string loginStaffKey, string attendant, string previousAttendantKey)
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            // MessageNotiView a = new MessageNotiView();
            List<ToStaffList> lst = new List<ToStaffList>();
            try
            {
                if (dalgeneralprofile.IsUseSqoopeMessagingService())
                {
                    #region Create the Sqoope msg list to send
                    Guid staffkey = new Guid(loginStaffKey);
                    Staff loginStaff = dalstaff.GetStaffInfoByStaffKey(staffkey);

                    Staff previousAttendantStaff = new Staff();
                    if (!string.IsNullOrEmpty(previousAttendantKey))
                        previousAttendantStaff = dalstaff.GetStaffInfoByAttendantKey(previousAttendantKey);

                    List<SqoopeMsgLog> listSqoopeMsgLog = new List<SqoopeMsgLog>();
                    SqoopeMsgLog msg;
                    string strSqoopeMessage = GetSqoopeMessageByMsgCode(msgCode, roomNo, attendant, previousAttendantStaff.UserName);
                    DataTable dtContact = _roomdalRepository.GetContactListByMsgCode(msgCode);
                    foreach (DataRow dr in dtContact.Rows)
                    {

                        msg = new SqoopeMsgLog();
                        msg.Id = Guid.NewGuid();
                        msg.SqoopeMessageKey = Guid.Parse(dr["MessageKey"].ToString());
                        if (loginStaff.Contact_Id != null)
                            msg.FromContactId = Convert.ToInt32(loginStaff.Contact_Id);
                        if (loginStaff.Id != null)
                            msg.CreatedBy = loginStaff.Id;
                        msg.ToContactId = (!DBNull.Value.Equals(dr["Contact_ID"])) ? (!string.IsNullOrEmpty(dr["Contact_ID"].ToString()) ? Convert.ToInt32(dr["Contact_ID"].ToString()) : 0) : 0;
                        // msg.ToContactId = !DBNull.Value.Equals(dr["Contact_ID"]) ? Convert.ToInt32(dr["Contact_ID"].ToString()) : 0; //Convert.ToInt32(dr["Contact_ID"].ToString());
                        msg.Msg = strSqoopeMessage;
                        msg.ToStaffKey = Guid.Parse(dr["StaffKey"].ToString());
                        msg.FirebaseToken_Id = (!DBNull.Value.Equals(dr["FirebaseToken_Id"])) ? (!string.IsNullOrEmpty(dr["FirebaseToken_Id"].ToString()) ? (dr["FirebaseToken_Id"].ToString()) : "") : "";
                        listSqoopeMsgLog.Add(msg);
                    }
                    #endregion

                    #region Create the Sqoope Message for the room status is DIRTY, send to maid ContactID

                    if (msgCode.Equals(CommomData.MsgType_iClean_CheckDirty))
                    {
                        SqoopeMsgLog msgForMaid = GetSqoopeMessageForRoomDirty(loginStaff, msgCode, roomNo, strSqoopeMessage);
                        if (msgForMaid != null && msgForMaid.Msg != "" && msgForMaid.Id != Guid.Empty)//&& msgForMaid.ToContactId != 0
                        {
                            listSqoopeMsgLog.Add(msgForMaid);
                        }
                    }

                    #endregion

                    #region Create the Sqoope Message for the room is assigned to you(attendant)

                    if (msgCode.Equals(CommomData.MsgType_iClean_AssignAttendant))
                    {
                        // Message for assigned Attendant
                        SqoopeMsgLog msgForAttendant = GetSqoopeMessageForRoomAssignment(loginStaff, msgCode, roomNo, strSqoopeMessage);
                        if (msgForAttendant != null && msgForAttendant.Msg != "" && msgForAttendant.Id != Guid.Empty)//&& msgForAttendant.ToContactId != 0
                        {
                            listSqoopeMsgLog.Add(msgForAttendant);
                        }

                        // Message(of unassignment) for previous assigned Attendant
                        //if (previousAttendantStaff.Contact_Id != null && previousAttendantStaff.Contact_Id != "0")
                        if (previousAttendantStaff.Id != null && previousAttendantStaff.Id != Guid.Empty)
                        {
                            msgForAttendant = GetSqoopeMessageForRoomUnAssignment(loginStaff, msgCode, roomNo, previousAttendantStaff);
                            if (msgForAttendant != null && msgForAttendant.Msg != "" && msgForAttendant.Id != Guid.Empty)//&& msgForAttendant.ToContactId != 0 
                            {
                                listSqoopeMsgLog.Add(msgForAttendant);
                            }
                        }
                    }

                    #endregion


                    #region Send msg to Sqoope contact
                    if (listSqoopeMsgLog.Count > 0)
                    {
                        try
                        {
                            foreach (SqoopeMsgLog ms in listSqoopeMsgLog)
                            {

                                try
                                {
                                    ms.CreatedOn = DateTime.Now;
                                    //ms.TenantId = 1;
                                    ms.TenantId = (int?)AbpSession.TenantId;
                                    ms.Read = false;
                                    ms.Send = false;

                                    _sqoopemsglogRepository.Insert(ms);


                                }
                                catch (Exception ex)
                                {
                                    // throw new UserFriendlyException("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                                    //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                                    break;
                                }
                            }
                            Alllst = listSqoopeMsgLog.GroupBy(u => u.Msg)
                            .Select(g => new MessageNotiView
                            {
                                Message = g.Key,
                                ToStaffList = g.Select(tb => new ToStaffList { to = tb.FirebaseToken_Id }).ToList()
                            }).ToList();
                        }
                        catch (Exception ex)
                        {
                            //throw new UserFriendlyException("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                            // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                        }

                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                //throw new UserFriendlyException("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
            }

            return Alllst;
        }
        #endregion
        #region iRepair - Messaging Helper functions
        public async Task<List<MessageNotiView>> SendiRepairMessageToSqoope(string msgCode, List<MWorkOrderInput> listWork, List<BlockRoom> listRoom, string loginStaffKey = "")
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<ToStaffList> lst = new List<ToStaffList>();

            try
            {
                //if (BLL_Profile.IsUseSqoopeMessagingService(propertyCode))
                if (dalgeneralprofile.IsUseSqoopeMessagingService())
                {
                    #region Create the Sqoope msg list to send
                    Guid staffkey = new Guid(loginStaffKey);
                    //Staff loginStaff = dalstaff.GetStaffInfoByStaffKey(staffkey);
                    var loginStaff = (from s in _staffRepository.GetAll()
                                      where s.Id == staffkey
                                      join t in _mtechnicanRepository.GetAll()
                                      on s.TechnicianKey equals t.TechnicianKey
                                      into stafftechnicalGroup
                                      from st in stafftechnicalGroup.DefaultIfEmpty()
                                      select new StaffDto
                                      {
                                          StaffKey = s.Id,
                                          UserName = s.UserName,
                                          Active = s.Active,
                                          Sec_Supervisor = s.Sec_Supervisor,
                                          Sec_BlockRoom = s.Sec_BlockRoom,
                                          Sec_TechSupervisor = s.Sec_TechSupervisor,
                                          Contact_ID = s.Contact_Id == null ? 0 : Convert.ToInt32(s.Contact_Id),
                                          MaidKey = s.MaidKey == null ? Guid.Empty : s.MaidKey,
                                          TechnicianKey = s.TechnicianKey == null ? Guid.Empty : s.TechnicianKey,
                                          TechnicianID = st.Id == null ? 0 : st.Id
                                      }).FirstOrDefault();

                    List<SqoopeMsgLog> listSqoopeMsgLog = new List<SqoopeMsgLog>();
                    SqoopeMsgLog sqoopeMsgLog;
                    List<string> listSqoopeMessage = new List<string>();


                    if (msgCode.Equals(CommomData.MsgType_iRepair_InformBlockRoomStatus))
                    {
                        listSqoopeMessage = GetiRepairSqoopeMessageForBlockRoom(msgCode, listRoom);
                    }
                    else
                    {
                        listSqoopeMessage = GetiRepairSqoopeMessageByMsgCode(msgCode, listWork);
                    }

                    DataTable dtContact = _roomdalRepository.GetIRContactListByMsgCode(msgCode);// BLL_Sqoope.GetContactListByMsgCode(propertyCode, msgCode);
                    if (listSqoopeMessage.Count > 0)
                    {
                        foreach (DataRow dr in dtContact.Rows)
                        {
                            foreach (string sqoopeMessage in listSqoopeMessage)
                            {
                                sqoopeMsgLog = new SqoopeMsgLog();
                                sqoopeMsgLog.Id = Guid.NewGuid();
                                sqoopeMsgLog.SqoopeMessageKey = Guid.Parse(dr["MessageKey"].ToString());
                                if (loginStaff.Contact_ID != null)
                                    sqoopeMsgLog.FromContactId = Convert.ToInt32(loginStaff.Contact_ID);
                                if (loginStaff.StaffKey != null)
                                    sqoopeMsgLog.CreatedBy = loginStaff.StaffKey;
                                sqoopeMsgLog.ToContactId = (!DBNull.Value.Equals(dr["Contact_ID"])) ? (!string.IsNullOrEmpty(dr["Contact_ID"].ToString()) ? Convert.ToInt32(dr["Contact_ID"].ToString()) : 0) : 0;/*Convert.ToInt32(dr["Contact_ID"].ToString());*/
                                sqoopeMsgLog.Msg = sqoopeMessage;
                                sqoopeMsgLog.ToStaffKey = Guid.Parse(dr["StaffKey"].ToString());
                                sqoopeMsgLog.FirebaseToken_Id = (!DBNull.Value.Equals(dr["FirebaseToken_IdiRepair"])) ? (!string.IsNullOrEmpty(dr["FirebaseToken_IdiRepair"].ToString()) ? (dr["FirebaseToken_IdiRepair"].ToString()) : "") : "";
                                listSqoopeMsgLog.Add(sqoopeMsgLog);
                            }
                        }
                    }

                    #endregion

                    #region Create the Sqoope Message for task assignment to Technician

                    if (msgCode.Equals(CommomData.MsgType_iRepair_AssignWorkOrder))
                    {
                        List<SqoopeMsgLog> listMsgForTechnician = GetSqoopeMessageForTaskAssignment(loginStaff, msgCode, listWork);
                        if (listMsgForTechnician != null && listMsgForTechnician.Count > 0)
                        {
                            listSqoopeMsgLog.AddRange(listMsgForTechnician);
                        }
                    }

                    #endregion


                    #region Send msg to Sqoope contact
                    if (listSqoopeMsgLog.Count > 0)
                    {
                        //BezSqoopeService.SendMessageToSqoope(listSqoopeMsgLog);
                        try
                        {
                            foreach (SqoopeMsgLog ms in listSqoopeMsgLog)
                            {

                                try
                                {
                                    ms.CreatedOn = DateTime.Now;
                                    //ms.TenantId = 1;
                                    ms.TenantId = (int?)AbpSession.TenantId;
                                    ms.Read = false;
                                    ms.Send = false;

                                    _sqoopemsglogRepository.Insert(ms);


                                }
                                catch (Exception ex)
                                {
                                    // throw new UserFriendlyException("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                                    //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                                    break;
                                }
                            }
                            Alllst = listSqoopeMsgLog.GroupBy(u => u.Msg)
                            .Select(g => new MessageNotiView
                            {
                                Message = g.Key,
                                ToStaffList = g.Select(tb => new ToStaffList { to = tb.FirebaseToken_Id }).ToList()
                            }).ToList();
                        }
                        catch (Exception ex)
                        {
                            //throw new UserFriendlyException("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                            // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
            }

            return Alllst;
        }
        private List<string> GetiRepairSqoopeMessageForBlockRoom(string msgCode, List<BlockRoom> listBlockRoom)
        {
            List<string> listMessage = new List<string>();

            try
            {
                string strMessageTemplate = dalsqoopemsgtype.GetMessageTemplateByMsgCode(msgCode);

                if (listBlockRoom[0].Roomblockkey == null)
                {
                    string strBlockDate = "";
                    if (listBlockRoom[0].BlockFromDate == listBlockRoom[0].BlockToDate)
                    {
                        strBlockDate = " at " + CommomData.GetDateToDisplay(listBlockRoom[0].BlockFromDate);
                    }
                    else
                    {
                        strBlockDate = " from " + CommomData.GetDateToDisplay(listBlockRoom[0].BlockFromDate) + " to " + CommomData.GetDateToDisplay(listBlockRoom[0].BlockToDate);
                    }

                    // New entry
                    string strMessage = strMessageTemplate;
                    strMessage = strMessage.Replace("#USERNAME#", listBlockRoom[0].LastUpdatedBy);
                    strMessage = strMessage.Replace("#ROOM_NO#", listBlockRoom[0].RoomNo);
                    strMessage = strMessage.Replace("#WO_NO#", listBlockRoom[0].Mworkorderno.ToString());
                    strMessage = strMessage.Replace("#BLOCK_DATE#", strBlockDate);
                    strMessage = strMessage.Replace("#BLOCK_STATUS#", listBlockRoom[0].Active == 1 ? "Block" : "Unblock");
                    strMessage = strMessage.Replace("/updated", "");
                    listMessage.Add(strMessage);
                }
                else
                {
                    // Update status
                    foreach (BlockRoom room in listBlockRoom)
                    {
                        string strMessage = strMessageTemplate;
                        strMessage = strMessage.Replace("#USERNAME#", room.LastUpdatedBy);
                        strMessage = strMessage.Replace("#ROOM_NO#", listBlockRoom[0].RoomNo);
                        strMessage = strMessage.Replace("#WO_NO#", room.Mworkorderno.ToString());
                        strMessage = strMessage.Replace("#BLOCK_DATE#", " at " + CommomData.GetDateToDisplay(room.Blockdate));
                        strMessage = strMessage.Replace("#BLOCK_STATUS#", room.Active == 1 ? "Block" : "Unblock");
                        strMessage = strMessage.Replace("added/", "");
                        listMessage.Add(strMessage);
                    }
                }



                return listMessage;
            }
            catch (Exception ex)
            {
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                throw ex;
            }
        }

        private List<string> GetiRepairSqoopeMessageByMsgCode(string msgCode, List<MWorkOrderInput> listWorkOrder)
        {
            List<string> listMessage = new List<string>();

            try
            {
                string strMessageTemplate = dalsqoopemsgtype.GetMessageTemplateByMsgCode(msgCode);

                foreach (MWorkOrderInput work in listWorkOrder)
                {
                    string strMessage = strMessageTemplate;
                    strMessage = strMessage.Replace("#USERNAME#", work.LastUpdateBy);
                    strMessage = strMessage.Replace("#WO_NO#", work.Id.ToString());
                    strMessage = strMessage.Replace("#WO_DESC#", work.Description);
                    strMessage = strMessage.Replace("#TECHNICIAN#", work.MTechnicianName);
                    strMessage = strMessage.Replace("#WO_STATUS#", work.MWorkOrderStatusDesc);

                    listMessage.Add(strMessage);
                }

                return listMessage;
            }
            catch (Exception ex)
            {
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                throw ex;
            }
        }
        private List<SqoopeMsgLog> GetSqoopeMessageForTaskAssignment(StaffDto loginStaff, string msgCode, List<MWorkOrderInput> listWorkOrder)
        {
            List<SqoopeMsgLog> listSqoopeMsgLog = new List<SqoopeMsgLog>();
            SqoopeMsgLog msg = new SqoopeMsgLog();
            try
            {
                // Staff staffTechnician = BLL_Staff.GetStaffInfoByTechnicianID(Convert.ToInt32(listWorkOrder[0].MTechnician));
                int TechnicianId = Convert.ToInt32(listWorkOrder[0].MTechnician);
                var staffTechnician = (from s in _staffRepository.GetAll()
                                       join t in _mtechnicanRepository.GetAll()
                                       on s.TechnicianKey equals t.TechnicianKey
                                       into stafftechnicalGroup
                                       from st in stafftechnicalGroup.DefaultIfEmpty()
                                       where st.Id == TechnicianId
                                       select new StaffDto
                                       {
                                           StaffKey = s.Id,
                                           UserName = s.UserName,
                                           Active = s.Active,
                                           Sec_Supervisor = s.Sec_Supervisor,
                                           FirebaseToken_Id = s.FirebaseToken_IdiRepair,
                                           //Sec_BlockRoom = s.Sec_BlockRoom,
                                           //Sec_TechSupervisor = s.Sec_TechSupervisor,
                                           Contact_ID = s.Contact_Id == null ? 0 : Convert.ToInt32(s.Contact_Id),
                                           MaidKey = s.MaidKey == null ? Guid.Empty : s.MaidKey,
                                           //TechnicianKey = s.TechnicianKey == null ? Guid.Empty : s.TechnicianKey,
                                           //TechnicianID = st.Id == null ? 0 : st.Id
                                       }).FirstOrDefault();
                string strMessageKey = dalsqoopemsgtype.GetMessageKeyByMsgCode(msgCode);
                string strMessageTemplate = dalsqoopemsgtype.GetMessageTemplateByMsgCode(msgCode);

                // if (staffTechnician.Contact_ID != null && staffTechnician.Contact_ID != 0 && !string.IsNullOrEmpty(strMessageKey))
                if (staffTechnician != null && !string.IsNullOrEmpty(strMessageKey) && staffTechnician.StaffKey != Guid.Empty)
                {
                    foreach (MWorkOrderInput work in listWorkOrder)
                    {
                        string strMessage = strMessageTemplate;
                        strMessage = strMessage.Replace("#USERNAME#", work.LastUpdateBy);
                        strMessage = strMessage.Replace("#WO_NO#", work.Id.ToString());
                        strMessage = strMessage.Replace("#WO_DESC#", work.Description);
                        strMessage = strMessage.Replace("#TECHNICIAN#", "you");

                        msg = new SqoopeMsgLog();
                        msg.Id = Guid.NewGuid();
                        msg.SqoopeMessageKey = Guid.Parse(strMessageKey);
                        if (loginStaff.Contact_ID != null)
                            msg.FromContactId = Convert.ToInt32(loginStaff.Contact_ID);
                        if (loginStaff.StaffKey != null)
                            msg.CreatedBy = loginStaff.StaffKey;
                        msg.ToContactId = Convert.ToInt32(staffTechnician.Contact_ID);
                        msg.ToStaffKey = staffTechnician.StaffKey;// previousAttendant.Id;
                        //if (!string.IsNullOrEmpty(previousAttendant.FirebaseToken_Id))
                        if (!string.IsNullOrEmpty(staffTechnician.FirebaseToken_Id))
                            msg.FirebaseToken_Id = staffTechnician.FirebaseToken_Id;
                        // msg.FirebaseToken_Id = previousAttendant.FirebaseToken_Id;
                        msg.Msg = strMessage;
                        listSqoopeMsgLog.Add(msg);
                    }
                }
                return listSqoopeMsgLog;
            }
            catch (Exception ex)
            {
                // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                throw ex;
            }
        }
        #endregion
        #region WONotiIClean
        public async Task<List<MessageNotiView>> SendiCleanWOMessageToSqoope(string msgCode, List<MWorkOrderInput> listWork, List<BlockRoom> listRoom, string loginStaffKey = "")
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<ToStaffList> lst = new List<ToStaffList>();

            try
            {
                if (dalgeneralprofile.IsUseSqoopeMessagingService())
                {
                    #region Create the Sqoope msg list to send
                    Guid staffkey = new Guid(loginStaffKey);
                    
                    List<SqoopeMsgLog> listSqoopeMsgLog = new List<SqoopeMsgLog>();
                    SqoopeMsgLog sqoopeMsgLog;
                    List<string> listSqoopeMessage = new List<string>();
                    listSqoopeMessage = GetiCleanWoSqoopeMessageByMsgCode(msgCode, listWork);
                    DataTable dtContact = _roomdalRepository.GetContactListByMsgCode(msgCode);// BLL_Sqoope.GetContactListByMsgCode(propertyCode, msgCode);
                    if (listSqoopeMessage.Count > 0)
                    {
                        foreach (DataRow dr in dtContact.Rows)
                        {
                            foreach (string sqoopeMessage in listSqoopeMessage)
                            {
                                sqoopeMsgLog = new SqoopeMsgLog();
                                sqoopeMsgLog.Id = Guid.NewGuid();
                                sqoopeMsgLog.SqoopeMessageKey = Guid.Parse(dr["MessageKey"].ToString());
                                sqoopeMsgLog.FromContactId = 0;
                                sqoopeMsgLog.CreatedBy = staffkey; //loginStaff.StaffKey;
                                sqoopeMsgLog.ToContactId = (!DBNull.Value.Equals(dr["Contact_ID"])) ? (!string.IsNullOrEmpty(dr["Contact_ID"].ToString()) ? Convert.ToInt32(dr["Contact_ID"].ToString()) : 0) : 0;/*Convert.ToInt32(dr["Contact_ID"].ToString());*/
                                sqoopeMsgLog.Msg = sqoopeMessage;
                                sqoopeMsgLog.ToStaffKey = Guid.Parse(dr["StaffKey"].ToString());
                                sqoopeMsgLog.FirebaseToken_Id = (!DBNull.Value.Equals(dr["FirebaseToken_Id"])) ? (!string.IsNullOrEmpty(dr["FirebaseToken_Id"].ToString()) ? (dr["FirebaseToken_Id"].ToString()) : "") : "";
                                listSqoopeMsgLog.Add(sqoopeMsgLog);
                            }
                        }
                    }

                    #endregion
                    #region Send msg to Sqoope contact
                    if (listSqoopeMsgLog.Count > 0)
                    {
                        //BezSqoopeService.SendMessageToSqoope(listSqoopeMsgLog);
                        try
                        {
                            foreach (SqoopeMsgLog ms in listSqoopeMsgLog)
                            {

                                try
                                {
                                    ms.CreatedOn = DateTime.Now;
                                    //ms.TenantId = 1;
                                    ms.TenantId = (int?)AbpSession.TenantId;
                                    ms.Read = false;
                                    ms.Send = false;

                                    _sqoopemsglogRepository.Insert(ms);


                                }
                                catch (Exception ex)
                                {
                                    // throw new UserFriendlyException("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                                    //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                                    break;
                                }
                            }
                            Alllst = listSqoopeMsgLog.GroupBy(u => u.Msg)
                            .Select(g => new MessageNotiView
                            {
                                Message = g.Key,
                                ToStaffList = g.Select(tb => new ToStaffList { to = tb.FirebaseToken_Id }).ToList()
                            }).ToList();
                        }
                        catch (Exception ex)
                        {
                            //throw new UserFriendlyException("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                            // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
            }

            return Alllst;
        }
        private List<string> GetiCleanWoSqoopeMessageByMsgCode(string msgCode, List<MWorkOrderInput> listWorkOrder)
        {
            List<string> listMessage = new List<string>();

            try
            {
                string strMessageTemplate = dalsqoopemsgtype.GetMessageTemplateByMsgCode(msgCode);

                foreach (MWorkOrderInput work in listWorkOrder)
                {
                    string strMessage = strMessageTemplate;
                    strMessage = strMessage.Replace("#USERNAME#", work.LastUpdateBy);
                    strMessage = strMessage.Replace("#WO_NO#", work.Id.ToString());
                    strMessage = strMessage.Replace("#WO_DESC#", work.Description);
                    listMessage.Add(strMessage);
                }

                return listMessage;
            }
            catch (Exception ex)
            {
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "SqoopeError/SqoopeException");
                throw ex;
            }
        }

        #endregion
    }
}
