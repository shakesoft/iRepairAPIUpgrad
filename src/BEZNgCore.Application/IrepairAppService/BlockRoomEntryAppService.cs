using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.Common;
using System.Threading.Tasks;
using Abp.UI;
using BEZNgCore.IRepairIAppService;
using System.Text;
using BEZNgCore.IrepairAppService.DAL;
using System.Collections;
using Abp.Runtime.Session;

namespace BEZNgCore.IrepairAppService
{

    public class BlockRoomEntryAppService : BEZNgCoreAppServiceBase
    {
        //private readonly IRepository<MWorkOrder, int> _mworkorderRepository;
        private readonly IRepository<RoomStatus, Guid> _roomstatusRepository;
        private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IRepository<Control, Guid> _controlRepository;
        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly IMworkorderdalRepository _mwordorderdalRepository;
        private readonly IMworkordertimesheetdalRepository _mworkordertimesheetdalRepository;
        private readonly ISqoopeMessgingAppService _sqoopeint;
        StaffDAL dalstaff;
        public BlockRoomEntryAppService(
            //IRepository<MWorkOrder, int> mworkorderRepository,
            IRepository<RoomStatus, Guid> roomstatusRepository,
            IRepository<Room, Guid> roomRepository,
            IRepository<Control, Guid> controlRepository,
            IRepository<Staff, Guid> staffRepository,
            IMworkorderdalRepository mwordorderdalRepository,
            IMworkordertimesheetdalRepository mworkordertimesheetdalRepository,
            ISqoopeMessgingAppService sqoopeint
            )
        {
           // _mworkorderRepository = mworkorderRepository;
            _roomstatusRepository = roomstatusRepository;
            _roomRepository = roomRepository;
            _controlRepository = controlRepository;
            _staffRepository = staffRepository;
            dalstaff = new StaffDAL(_staffRepository);
            _mwordorderdalRepository = mwordorderdalRepository;
            _mworkordertimesheetdalRepository = mworkordertimesheetdalRepository;
            _sqoopeint = sqoopeint;
        }

        [HttpGet]
        public async Task<ListResultDto<BlockRoomEntryViewData>> GetBlockRoomEntryViewData(string wokey = "")
        {
            List<BlockRoomEntryViewData> Alllst = new List<BlockRoomEntryViewData>();
            BlockRoomEntryViewData a = new BlockRoomEntryViewData();
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
                var staffkey = user.StaffKey;
                if (dalstaff.IsLoginUserBlockRoomSupervisor(staffkey))
                {

                    List<DDLWorkOrderOutput> ddlw = new List<DDLWorkOrderOutput>();
                    a.DDLWorkOrder = BindDDLWorkOrder();
                    List<DDLRoomOutput> ddlr = new List<DDLRoomOutput>();
                    DDLRoomOutput ro = new DDLRoomOutput();
                    ro.RoomKey = Guid.Empty;
                    ro.Unit = "--Please select--";
                    ddlr.Add(ro);
                    var ddlr2 = _roomRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Floor).ThenBy(x => x.Unit)
                      .Select(x => new DDLRoomOutput
                      {
                          RoomKey = x.Id,
                          Unit = x.Unit
                      });
                    a.DDLRoom = ddlr.Concat(ddlr2).ToList();
                    List<DDLStatusOutput> ddls = new List<DDLStatusOutput>();
                    DDLStatusOutput rs = new DDLStatusOutput();
                    rs.RoomStatusKey = Guid.Empty;
                    rs.RoomStatus = "--Please select--";
                    ddls.Add(rs);
                    var exceptionList = new List<string> { "Vacant", "Occupied", "Due Out" };
                    string Vacant = "Vacant"; string Occupied = "Occupied"; string DO = "Due Out";
                    var ddls2 = (from r in _roomstatusRepository.GetAll()
                                 where (!r.RoomStatusName.Contains(Vacant) && !r.RoomStatusName.Contains(Occupied) && !r.RoomStatusName.Contains(DO))
                                 //.Where(e=>!e.RoomStatusName.Contains(Vacant) || !e.RoomStatusName.Contains(Occupied) ||!e.RoomStatusName.Contains(DO))
                                 //.Where(e => !exceptionList.Contains(e.RoomStatusName))
                                 orderby r.RoomStatusName ascending
                                 select new DDLStatusOutput
                                 {
                                     RoomStatusKey = r.Id,
                                     RoomStatus = r.RoomStatusName
                                 });
                    a.DDLStatus = ddls.Concat(ddls2).ToList();

                    DateTime dtBusinessDate = DateTime.Now;
                    dtBusinessDate = (DateTime)_controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
                    a.FromDate = dtBusinessDate;
                    a.ToDate = dtBusinessDate;

                    a.Reason = "Maintenance";
                    a.Note = "";

                    if (!string.IsNullOrEmpty(wokey))
                    {
                        a.ddlWrokOrderValue = wokey;
                        a.ddlWrokOrderName = a.DDLWorkOrder.Where(i => i.Seqno == wokey).Select(j => j.Description).FirstOrDefault();
                        DataTable getWOKey = _mwordorderdalRepository.GetWorkOrderByID(Convert.ToInt32(wokey));
                        foreach (DataRow dr in getWOKey.Rows)
                        {

                            var room = dr["Room"].ToString();
                            var roomkey = dr["RoomKey"].ToString();
                            if (!string.IsNullOrEmpty(roomkey))
                            {
                                a.ddlroomkey = roomkey;
                            }

                            if (!string.IsNullOrEmpty(room))
                            {
                                a.ddlroomname = room;
                            }

                        }
                    }


                    //DDLWorkOrderOutput w = new DDLWorkOrderOutput();
                    //w.Seqno = "0";
                    //w.Description = "--Please select--";
                    //ddlw.Add(w);
                    //var exceptionintList = new List<int> { 2, 3, 4, 5 };
                    //var ddlw2 = (from m in _mworkorderRepository.GetAll()
                    //             where (m.MWorkOrderStatus!=2 && m.MWorkOrderStatus != 3 && m.MWorkOrderStatus!=4 && m.MWorkOrderStatus!=5)
                    //             //.Where(e=>e.MWorkOrderStatus!=2 || e.MWorkOrderStatus != 3 || e.MWorkOrderStatus != 4 || e.MWorkOrderStatus != 5)
                    //             //.Where(e => !exceptionintList.Contains((int)e.MWorkOrderStatus))
                    //             orderby m.Id descending
                    //             select new DDLWorkOrderOutput
                    //             {
                    //                 Seqno = m.Id.ToString(),
                    //                 Description = "#"+m.Id+":"+m.Description
                    //             });
                    //a.DDLWorkOrder = ddlw.Concat(ddlw2).ToList();
                }
                else
                {
                    throw new UserFriendlyException("Access Denied");
                }
                Alllst.Add(a);
            }
            return new ListResultDto<BlockRoomEntryViewData>(Alllst);
        }

        #region Add button Click
        [HttpPost]
        public async Task<List<MessageNotiViewLatest>> AddBlockRoom(BlockRoomInput input)
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            string litMessage = "";
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
                try
                {

                    List<BlockRoom> listRoom = new List<BlockRoom>();
                    BlockRoom room;
                    Guid MWorkOrderKey = Guid.Empty;
                    int Mworkorderno = Convert.ToInt32(input.litWOID.Trim());//Convert.ToInt32(CommomData.GetWorkOrderIDFromDescription(input.litWOID));
                    DataTable getWOKey = _mwordorderdalRepository.GetWorkOrderByID(Mworkorderno);
                    if (!string.IsNullOrEmpty(getWOKey.Rows[0]["MWorkOrderKey"].ToString()) && getWOKey.Rows[0]["MWorkOrderKey"].ToString() != "")
                    {
                        MWorkOrderKey = Guid.Parse(getWOKey.Rows[0]["MWorkOrderKey"].ToString());
                    }
                    else
                    {
                        MWorkOrderKey = Guid.NewGuid();
                    }
                    Guid Roomkey = Guid.Parse(input.ddlroomkey);
                    string strRoomNo = input.ddlroomname;

                    DateTime fromBlockdate = input.FromDate;
                    DateTime toBlockdate = input.ToDate;
                    TimeSpan ts = toBlockdate - fromBlockdate;
                    if (ts.Days < 0)
                    {
                        throw new UserFriendlyException("Invalid Date Range! From Date should be earlier than To Date.");
                        //return;
                    }

                    room = new BlockRoom();
                    room.Mworkorderno = Mworkorderno;
                    room.Roomkey = Roomkey;
                    room.BlockFromDate = fromBlockdate;
                    room.BlockToDate = toBlockdate;

                    if (!_mwordorderdalRepository.IsRoomBlockExist(room))
                    {
                        string strReason = input.Reason.Trim();
                        string strNotes = input.Note.Trim();
                        int intBlock = 0;
                        if (input.ddlStatusname.Trim().ToLower() == "hold" || input.ddlStatusname.Trim().ToLower() == "out of service")
                            intBlock = 2;
                        else
                            intBlock = 1;

                        string strBlockDate = "";
                        if (fromBlockdate.Date == toBlockdate.Date)
                        {
                            strBlockDate = " at " + CommomData.GetDateToDisplay(fromBlockdate.Date);
                        }
                        else
                        {
                            strBlockDate = " from " + CommomData.GetDateToDisplay(fromBlockdate.Date) + " to " + CommomData.GetDateToDisplay(toBlockdate.Date);
                        }

                        string strLog = "WO#" + Mworkorderno + "; " + user.UserName + " has added Room#" + strRoomNo + strBlockDate + " as " + (intBlock >= 1 ? "Block" : "Unblock");
                        DataTable check = new DataTable();
                        for (int i = 0; i <= ts.Days; i++)
                        {
                            room = new BlockRoom();
                            room.Mworkorderno = Mworkorderno;
                            room.MWorkOrderKey = MWorkOrderKey;
                            room.Roomkey = Roomkey;
                            room.RoomNo = strRoomNo;
                            room.Blockdate = fromBlockdate.AddDays(i);
                            room.BlockFromDate = fromBlockdate;
                            room.BlockToDate = toBlockdate;
                            room.Reason = strReason;
                            room.Comment = strNotes;
                            room.Active = intBlock;
                            room.LastUpdatedBy = user.UserName;
                            if (input.chkIsBlock)//display none chkIsBlock
                            {
                                room.Blockstaff = user.UserName;
                                room.Blocktime = DateTime.Now;

                            }
                            else
                            {
                                room.Unblockstaff = user.UserName;
                                room.Unblocktime = DateTime.Now;

                            }
                            room.DetailLog = strLog;
                            room.NewLog = GetChangeLog(room);
                            if (AbpSession.TenantId != null)
                            {
                                room.TenantId = (int?)AbpSession.TenantId;
                            }
                            listRoom.Add(room);
                            DataTable dt = _mwordorderdalRepository.GetReservationByRoomKeyDateRange(Roomkey, fromBlockdate.AddDays(i));
                            if (check.Rows.Count == 0)
                            {
                                check = dt.Clone();
                            }
                            foreach (DataRow row in dt.Rows)
                            {
                                check.Rows.Add(row.ItemArray);
                            }
                        }

                        if (check.Rows.Count == 0)
                        {
                            int intSuccessful = _mwordorderdalRepository.Insert(listRoom);
                            if (intSuccessful > 0)
                            {
                                intSuccessful = InsertBlockRoomHistory(listRoom[0], true, user.StaffKey);
                                if (intSuccessful > 0)
                                {
                                    // Send msg to sqoope users if any
                                    // SqoopeMessgingHelper.SendiRepairMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iRepair_InformBlockRoomStatus, null, listRoom, BLL_Staff.GetLoginUserStaffKey());
                                    Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_InformBlockRoomStatus, null, listRoom, user.StaffKey.ToString());
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
                                    //litMessage= DisplayHelper.GetSuccessfulMessageBox("Record has been added.");
                                    //// rebind
                                    //BindInitialData();
                                }
                                else
                                {
                                    a.Message = "Fail to add the record.";
                                    Alllst.Add(a);
                                }
                            }


                        }
                        else
                        {
                            throw new UserFriendlyException("Room has been assigned.");
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(" Room Block exists between the selected dates. Please select the other dates.");
                    }

                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return Alllstlatest;
        }

        private int InsertBlockRoomHistory(BlockRoom blockRoom, bool blnInsert, Guid staffKey)
        {
            int success = 0;
            try
            {
                List<History> listHistory = new List<History>();
                History history = new History();
                history.SourceKey = blockRoom.MWorkOrderKey;
                history.StaffKey = staffKey;
                if (blnInsert)
                {
                    history.Operation = "I";
                    history.NewValue = blockRoom.NewLog;
                }
                else
                {
                    history.Operation = "U";
                    history.NewValue = blockRoom.NewLog;
                    history.OldValue = blockRoom.OldLog;
                }
                history.TableName = "BR";
                history.Detail = "(iRepair) " + blockRoom.DetailLog;
                history.ModuleName = "iRepair";
                if (AbpSession.TenantId != null)
                {
                    history.TenantId = (int?)AbpSession.TenantId;
                }
                listHistory.Add(history);

                success = _mwordorderdalRepository.InsertHistory(history);

            }
            catch (Exception ex)
            {
                // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return success;

        }


        #endregion
        #region Bind DDL WorkOrder List
        protected List<DDLWorkOrderOutput> BindDDLWorkOrder()
        {
            try
            {
                List<DDLWorkOrderOutput> r = new List<DDLWorkOrderOutput>();
                DataTable dt = _mwordorderdalRepository.GetWIPWorkOrder();
                if (dt.Rows.Count > 0)
                {
                    DataRow drNew = dt.NewRow();
                    drNew["Seqno"] = "0";
                    drNew["Description"] = "--Please select--";
                    dt.Rows.InsertAt(drNew, 0);
                }

                foreach (DataRow row in dt.Rows)
                {
                    DDLWorkOrderOutput o = new DDLWorkOrderOutput();
                    o.Seqno = row["Seqno"].ToString();
                    o.Description = row["Description"].ToString();
                    r.Add(o);
                }

                return r;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        private string GetChangeLog(BlockRoom work)
        {
            try
            {
                string strLog = "";
                StringBuilder sb = new StringBuilder();

                sb.Append(" Reason => " + work.Reason);

                if (!string.IsNullOrEmpty(work.Comment))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Notes => " + work.Comment);
                }
                if (!string.IsNullOrEmpty(work.Roomkey.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Room => " + work.RoomNo);
                }
                sb.Append(", Status => " + (work.Active == 1 ? "Out of Order" : "Hold"));
                if (work.BlockFromDate.Value.Date == work.BlockToDate.Value.Date)
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Block Date at " + CommomData.GetDateToDisplay(work.BlockFromDate.Value.Date));
                }
                else
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Block Date from " + CommomData.GetDateToDisplay(work.BlockFromDate.Value.Date) + " to " + CommomData.GetDateToDisplay(work.BlockToDate.Value.Date));
                }

                sb.Append(", Block => " + (work.Active >= 1 ? "Block" : "Unblock"));
                sb.Append(", Block Staff => " + work.Blockstaff);

                strLog = sb.ToString();

                return strLog;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<ListResultDto<PopupBlockRoomViewData>> GetpopupBlockRoomViewData(string mode, string woid, string key)
        {
            List<PopupBlockRoomViewData> Alllst = new List<PopupBlockRoomViewData>();
            PopupBlockRoomViewData a = new PopupBlockRoomViewData();
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
                var staffkey = user.StaffKey;
                if (dalstaff.IsLoginUserBlockRoomSupervisor(staffkey))
                {
                    if (!string.IsNullOrEmpty(mode))
                    {
                        //BindDDLRoom();
                        List<DDLRoomOutput> ddlr = new List<DDLRoomOutput>();
                        DDLRoomOutput ro = new DDLRoomOutput();
                        ro.RoomKey = Guid.Empty;
                        ro.Unit = "--Please select--";
                        ddlr.Add(ro);
                        var ddlr2 = _roomRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Floor).ThenBy(x => x.Unit)
                          .Select(x => new DDLRoomOutput
                          {
                              RoomKey = x.Id,
                              Unit = x.Unit
                          });
                        a.DDLRoom = ddlr.Concat(ddlr2).ToList();



                        List<DDLStatusOutput> ddls = new List<DDLStatusOutput>();
                        DDLStatusOutput rs = new DDLStatusOutput();
                        rs.RoomStatusKey = Guid.Empty;
                        rs.RoomStatus = "--Please select--";
                        ddls.Add(rs);
                        var exceptionList = new List<string> { "Vacant", "Occupied", "Due Out" };
                        string Vacant = "Vacant"; string Occupied = "Occupied"; string DO = "Due Out";
                        var ddls2 = (from r in _roomstatusRepository.GetAll()
                                     where (!r.RoomStatusName.Contains(Vacant) && !r.RoomStatusName.Contains(Occupied) && !r.RoomStatusName.Contains(DO))
                                     //.Where(e=>!e.RoomStatusName.Contains(Vacant) || !e.RoomStatusName.Contains(Occupied) ||!e.RoomStatusName.Contains(DO))
                                     //.Where(e => !exceptionList.Contains(e.RoomStatusName))
                                     orderby r.RoomStatusName ascending
                                     select new DDLStatusOutput
                                     {
                                         RoomStatusKey = r.Id,
                                         RoomStatus = r.RoomStatusName
                                     });
                        a.DDLStatus = ddls.Concat(ddls2).ToList();

                        if (mode.Equals("i"))
                        {
                            a.litTitle = "Add Room Block ";
                            a.litWOID = woid;

                            DateTime dtBusinessDate = DateTime.Now;
                            dtBusinessDate = (DateTime)_controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
                            a.FromDate = dtBusinessDate;
                            a.ToDate = dtBusinessDate.AddDays(1);
                            a.btnUpdate = false;
                            a.btnDelete = false;

                            DataTable list = _mwordorderdalRepository.GetWorkOrderByID(Convert.ToInt32(a.litWOID));
                            foreach (DataRow dr in list.Rows)
                            {
                                var room = dr["Room"].ToString();
                                var roomkey = dr["RoomKey"].ToString();
                                if (!string.IsNullOrEmpty(roomkey))
                                {
                                    a.ddlroomkey = roomkey;
                                }

                                if (!string.IsNullOrEmpty(room))
                                {
                                    a.ddlroomname = room;
                                }


                            }
                        }
                        else if (mode.Equals("u"))
                        {
                            a.litTitle = "Update Room Block";

                            a.btnAdd = false;
                            a.btnDelete = false;
                            //BindBlockRoomInfo(key);
                            DataTable dt = _mwordorderdalRepository.GetBlockRoomByKey(key);
                            if (dt.Rows.Count > 0)
                            {
                                a.litRoomBlockKey = key;
                                a.litWOID = dt.Rows[0]["MWorkOrderNo"].ToString();
                                if (!string.IsNullOrEmpty(dt.Rows[0]["RoomKey"].ToString()))
                                    a.ddlroomkey = dt.Rows[0]["RoomKey"].ToString();
                                if (!string.IsNullOrEmpty(dt.Rows[0]["Unit"].ToString()))
                                    a.ddlroomname = dt.Rows[0]["Unit"].ToString();
                                //ddlRoom.Enabled = false;

                                if (!string.IsNullOrEmpty(dt.Rows[0]["BlockDate"].ToString()))
                                    a.FromDate = Convert.ToDateTime(dt.Rows[0]["BlockDate"].ToString());
                                //txtFromDate.Enabled = false;
                                //txtFromDate.DateInput.BackColor = System.Drawing.Color.LightGray;

                                a.Reason = dt.Rows[0]["Reason"].ToString();
                                a.Note = dt.Rows[0]["Comment"].ToString();

                                a.chkIsBlock = (dt.Rows[0]["Active"].ToString().Equals("1") ? true : dt.Rows[0]["Active"].ToString().Equals("2") ? true : false);
                                if (dt.Rows[0]["Active"].ToString().Equals("2"))
                                {

                                    string OOS = "Out Of Service";
                                    var ContainOOS = a.DDLStatus.Where(x => x.RoomStatus.Contains(OOS)).FirstOrDefault();
                                    if (ContainOOS!=null)
                                    {
                                        a.ddlStatusname = "Out Of Service";
                                    }
                                    else
                                    {
                                        a.ddlStatusname = "Hold";
                                    }

                                }

                                else
                                {
                                    a.ddlStatusname = "Out Of Order";
                                }
                                   
                            }
                        }
                        else if (mode.Equals("d"))
                        {
                            //litTitle.Text = "Delete Time Sheet ";
                            //litSeqno.Text = Request.QueryString["seqno"].ToString();
                            //dvTimeSheetDeleteNote.Visible = true;
                            //dvTimeSheet.Visible = false;
                            //btnAdd.Visible = false;
                            //btnUpdate.Visible = false;

                        }


                    }
                }
                else
                {
                    throw new UserFriendlyException("Sorry, you do not have the access right to Popup Block Room Page");
                }

                Alllst.Add(a);
            }
            return new ListResultDto<PopupBlockRoomViewData>(Alllst);
        }

        #region Update button Click
        [HttpPost]
        public async Task<List<MessageNotiViewLatest>> UpdateBlockRoom(BlockRoomInput input)
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            string litMessage = "";
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
                try
                {


                    List<BlockRoom> listRoom = new List<BlockRoom>();
                    BlockRoom room;
                    Guid MWorkOrderKey = Guid.Empty;
                    // @Hdr_Seqno, @MTechnician, @WDate, @TimeFrom, @TimeTo, @Notes, @CreatedBy, GETDATE() )
                    //int Mworkorderno = Convert.ToInt32(ddlWrokOrder.SelectedValue);
                    //room.Mworkorderno = Convert.ToInt32(BLL_MWorkOrder.GetWorkOrderIDFromDescription(ddlWrokOrder.SelectedItem.Text));

                    Guid Roomkey = Guid.Parse(input.ddlroomkey);
                    string strRoomNo = input.ddlroomname;
                    int Mworkorderno = Convert.ToInt32(input.litWOID);
                    DataTable getWOKey = _mwordorderdalRepository.GetWorkOrderByID(Mworkorderno);
                    if (!string.IsNullOrEmpty(getWOKey.Rows[0]["MWorkOrderKey"].ToString()) && getWOKey.Rows[0]["MWorkOrderKey"].ToString() != "")
                    {
                        MWorkOrderKey = Guid.Parse(getWOKey.Rows[0]["MWorkOrderKey"].ToString());
                    }
                    else
                    {
                        MWorkOrderKey = Guid.NewGuid();
                    }
                    DateTime fromBlockdate = Convert.ToDateTime(input.FromDate);
                    //DateTime toBlockdate = Convert.ToDateTime(txtToDate.SelectedDate);
                    //TimeSpan ts = toBlockdate - fromBlockdate;

                    string strReason = input.Reason.Trim();
                    string strNotes = input.Note.Trim();
                    int intBlock = 0;
                    if (input.ddlStatusname.Trim().ToLower() == "hold" || input.ddlStatusname.Trim().ToLower() == "out of service")
                        intBlock = 2;
                    else
                        intBlock = 1;

                    //string strLog = BLL_Staff.GetLoginUsername() + " updated Block Room# " + strRoomNo + " @" + fromBlockdate.Date + " status to " + (chkIsBlock.Checked? "Block" : "UnBlock");
                    //string strLog = "WO#" + Mworkorderno + "; " + BLL_Staff.GetLoginUsername() + " has updated Room#" + strRoomNo + " at " + fromBlockdate.ToString("dd/MM/yyyy") + " as " + (intBlock == 1 ? "Block" : "Unblock");
                    DataTable dt = _mwordorderdalRepository.GetBlockRoomByWorkOrderID(Mworkorderno);
                    room = new BlockRoom();
                    room.Roomblockkey = Guid.Parse(input.litRoomBlockKey);
                    room.Mworkorderno = Mworkorderno;
                    room.MWorkOrderKey = MWorkOrderKey;
                    room.Roomkey = Roomkey;
                    room.RoomNo = strRoomNo;
                    room.Blockdate = fromBlockdate;
                    room.Reason = strReason;
                    room.Comment = strNotes;
                    room.Active = intBlock;
                    room.LastUpdatedBy = user.UserName;
                    if (input.chkIsBlock)
                    {
                        room.Blockstaff = user.UserName;
                        room.Blocktime = DateTime.Now;
                    }
                    else
                    {
                        room.Unblockstaff = user.UserName;
                        room.Unblocktime = DateTime.Now;
                    }
                    room.DetailLog = user.UserName + " updated room block.";
                    room.NewLog = GetUpdateChangeLog(dt, room, user);
                    room.OldLog = GetUpdateOldLog(dt, room);
                    if (AbpSession.TenantId != null)
                    {
                        room.TenantId = (int?)AbpSession.TenantId;
                    }
                    listRoom.Add(room);


                    int intSuccessful = _mwordorderdalRepository.UpdateBlockroom(room);
                    if (intSuccessful > 0)
                    {
                        intSuccessful = InsertBlockRoomHistory(room, false, user.StaffKey);
                        if (intSuccessful > 0)
                        {
                            // Send msg to sqoope users if any
                            //SqoopeMessgingHelper.SendiRepairMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iRepair_InformBlockRoomStatus, null, listRoom, BLL_Staff.GetLoginUserStaffKey());
                            Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_InformBlockRoomStatus, null, listRoom, user.StaffKey.ToString());
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
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_", "UpdateBlockRoomClose();", true);
                            //Record(s) has been saved.
                        }

                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to update the record.");
                    }


                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return Alllstlatest;
        }

        private string GetUpdateChangeLog(DataTable dt, BlockRoom blockRoom, Authorization.Users.User user)
        {
            try
            {
                string strLog = "";
                StringBuilder sb = new StringBuilder();

                if (!Convert.ToDateTime(dt.Rows[0]["BlockDate"]).ToString("dd/MM/yyyy").Equals(Convert.ToDateTime(blockRoom.Blockdate).ToString("dd/MM/yyyy")))
                {
                    sb.Append(" WorkDate => " + Convert.ToDateTime(blockRoom.Blockdate).ToString("dd/MM/yyyy"));
                }

                if (!dt.Rows[0]["RoomKey"].ToString().Equals(blockRoom.Roomkey.ToString()))
                {
                    sb.Append(", Room No => " + blockRoom.RoomNo);
                }
                if (blockRoom.Active != null)
                {
                    if (!Convert.ToInt32(dt.Rows[0]["Active"]).Equals(blockRoom.Active))
                    {
                        if (!string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(", ");
                        sb.Append("Status@ => " + (blockRoom.Active == 1 ? "Out of Order" : "Hold"));
                    }
                }
                if (blockRoom.Unblocktime != null)
                {
                    if (!Convert.ToDateTime(dt.Rows[0]["UnblockTime"]).Equals(Convert.ToDateTime(blockRoom.Unblocktime)))
                    {
                        if (!string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(", ");
                        sb.Append("Unblock@ => " + Convert.ToDateTime(blockRoom.Unblocktime));
                    }
                }
                if (!dt.Rows[0]["Comment"].ToString().Equals(blockRoom.Comment))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("Comment => " + blockRoom.Comment);
                }
                if (!dt.Rows[0]["Reason"].ToString().Equals(blockRoom.Reason))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("Reason => " + blockRoom.Reason);
                }
                strLog = user.UserName + " updated room block. Change Log; " + sb.ToString();

                return strLog;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetUpdateOldLog(DataTable dt, BlockRoom blockRoom)
        {
            try
            {
                string strLog = "";
                StringBuilder sb = new StringBuilder();

                if (!Convert.ToDateTime(dt.Rows[0]["BlockDate"]).ToString("dd/MM/yyyy").Equals(Convert.ToDateTime(blockRoom.Blockdate).ToString("dd/MM/yyyy")))
                {
                    sb.Append(" WorkDate => " + Convert.ToDateTime(dt.Rows[0]["BlockDate"]).ToString("dd/MM/yyyy"));
                }

                if (!dt.Rows[0]["RoomKey"].ToString().Equals(blockRoom.Roomkey.ToString()))
                {
                    sb.Append(", Room No => " + GetRoomByKey(Guid.Parse(dt.Rows[0]["RoomKey"].ToString())));
                }
                if (blockRoom.Unblocktime != null)
                {
                    if (!Convert.ToDateTime(dt.Rows[0]["UnblockTime"]).Equals(Convert.ToDateTime(blockRoom.Unblocktime)))
                    {
                        if (!string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(", ");
                        sb.Append("Unblock@ => " + Convert.ToDateTime(dt.Rows[0]["UnblockTime"]));
                    }
                }
                if (!Convert.ToInt32(dt.Rows[0]["Active"]).Equals(blockRoom.Active))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("Status@ => " + (Convert.ToInt32(dt.Rows[0]["Active"]) == 1 ? "Out of Order" : "Hold"));
                }
                if (!dt.Rows[0]["Comment"].ToString().Equals(blockRoom.Comment))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("Comment => " + dt.Rows[0]["Comment"].ToString());
                }
                if (!dt.Rows[0]["Reason"].ToString().Equals(blockRoom.Reason))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("Reason => " + dt.Rows[0]["Reason"].ToString());
                }
                strLog = "Log; " + sb.ToString();

                return strLog;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetRoomByKey(Guid key)
        {
            return _roomRepository.GetAll().Where(x => x.Id == key).Select(x => x.Unit).FirstOrDefault();
        }
        #endregion

        #region UpdateBlockStatus
        [HttpPost]
        public async Task<List<MessageNotiViewLatest>> UpdateBlockStatus(PopupBlockRoomStatusInput input)
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
                try
                {
                    if (Convert.ToInt32(input.ddlValue) > -1)
                    {

                        if (input.b.Count > 0)
                        {

                            int intStatus = Convert.ToInt32(input.ddlValue);
                            string strWOStatusDesc = input.ddlText;
                            List<BlockRoom> listRoom = new List<BlockRoom>();
                            BlockRoom room;
                            DataTable dt;
                            foreach (string key in input.b)
                            {
                                dt = _mwordorderdalRepository.GetBlockRoomByKey(key);
                                room = new BlockRoom();
                                room.Mworkorderno = Convert.ToInt32(dt.Rows[0]["MWorkOrderno"].ToString());
                                DataTable getWOKey = _mwordorderdalRepository.GetWorkOrderByID(room.Mworkorderno);
                                if (!string.IsNullOrEmpty(getWOKey.Rows[0]["MWorkOrderKey"].ToString()) && getWOKey.Rows[0]["MWorkOrderKey"].ToString() != "")
                                    room.MWorkOrderKey = Guid.Parse(getWOKey.Rows[0]["MWorkOrderKey"].ToString());
                                else
                                    room.MWorkOrderKey = Guid.NewGuid();
                                room.RoomNo = dt.Rows[0]["Unit"].ToString();
                                room.Blockdate = Convert.ToDateTime(dt.Rows[0]["BlockDate"].ToString());
                                room.Roomblockkey = Guid.Parse(key);
                                room.Active = intStatus;
                                room.LastUpdatedBy = user.UserName;
                                if (intStatus == 1)
                                {
                                    room.Blockstaff = user.UserName;
                                    room.Blocktime = DateTime.Now;
                                }
                                else
                                {
                                    room.Unblockstaff = user.UserName;
                                    room.Unblocktime = DateTime.Now;
                                }
                                room.DetailLog = "WO#" + room.Mworkorderno + "; " + user.UserName + " has updated Room#" + room.RoomNo + " at " + CommomData.GetDateToDisplay(room.Blockdate) + " as " + (intStatus == 1 ? "Block" : "Unblock");
                                if (Convert.ToInt32(dt.Rows[0]["Active"].ToString()) != intStatus)
                                    room.NewLog = intStatus == 0 ? "Block status => Unblock" : "Block status => Block";
                                else
                                    room.NewLog = "-";
                                room.OldLog = dt.Rows[0]["Active"].ToString() == "0" ? "Block status => Unblock" : "Block status => Block";
                                if (AbpSession.TenantId != null)
                                {
                                    room.TenantId = (int?)AbpSession.TenantId;
                                }
                                listRoom.Add(room);

                                if (intStatus == 0)
                                {
                                    Room rooms = new Room();
                                    rooms.Id = Guid.Parse(dt.Rows[0]["RoomKey"].ToString());
                                    rooms.Unit = dt.Rows[0]["Unit"].ToString();
                                    rooms.Status = CommomData.HouseKeepingMaidStatusDirty;
                                    List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus(CommomData.HouseKeepingMaidStatusDirty);
                                    string maidStatusKey = lstmk[0].MaidStatusKey;
                                    rooms.MaidStatusKey = new Guid(maidStatusKey);
                                    //BLL_Room.UpdateRoomMaidStatus("", rooms);
                                    int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(rooms);

                                }
                            }
                            int intSuccess = _mworkordertimesheetdalRepository.UpdateStatus(listRoom);
                            if (intSuccess == 1)
                            {
                                foreach (BlockRoom blockroom in listRoom)
                                {
                                    intSuccess = InsertBlockRoomHistory(blockroom, false, user.StaffKey);
                                }
                                

                                if (intSuccess == 1)
                                {
                                    // Send msg to sqoope users if any
                                    //SqoopeMessgingHelper.SendiRepairMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iRepair_InformBlockRoomStatus, null, listRoom, BLL_Staff.GetLoginUserStaffKey());
                                    Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_InformBlockRoomStatus, null, listRoom, user.StaffKey.ToString());
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
                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_additem", "UpdateStatusClose();", true);
                                }
                                else
                                {
                                    throw new UserFriendlyException("Update Fail");
                                }

                            }
                        }
                        else
                        {
                            //ddlBlockRoomStatus.Enabled = false;
                            //btnUpdate.Enabled = false;
                            throw new UserFriendlyException("Please close this window and select Block/unBlockRoom again.");
                        }


                    }
                    else
                    {
                        throw new UserFriendlyException("Please select Block/unBlockRoom");
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return Alllstlatest;
        }
        protected List<GetMaidStatusOutput> GetMaidStatusKeyByStatus(string status)
        {
            return _mworkordertimesheetdalRepository.GetMaidStatusKeyByStatus(status);
        }

        [HttpGet]
        public ListResultDto<PopupBlockRoomStatusViewData> GetPopupBlockRoomStatusViewData(List<string> b)
        {
            if (b.ToList().Count < 0)
            {
                throw new UserFriendlyException("Please close this window and select Room Block again.");
            }

            List<PopupBlockRoomStatusViewData> Alllst = new List<PopupBlockRoomStatusViewData>();
            PopupBlockRoomStatusViewData a = new PopupBlockRoomStatusViewData();
            a.Title = "Update Room Block Status To";
            List<DDLBlockRoomStatusOutput> ddlt = new List<DDLBlockRoomStatusOutput>();
            ddlt.AddRange(new List<DDLBlockRoomStatusOutput>
            {
            new DDLBlockRoomStatusOutput("-1","--- Please select status ----"),
            new DDLBlockRoomStatusOutput("1","Block Room"),
            new DDLBlockRoomStatusOutput("0","Unblock Room")
            });



            a.DDLBlockRoomStatus = ddlt;
            a.b = b;
            Alllst.Add(a);
            return new ListResultDto<PopupBlockRoomStatusViewData>(Alllst);
        }
        #endregion
        [HttpPost]
        public async Task<string> btnUpdatePopupWorkNote(PopupWorkNoteInput input)
        {
            string message = "";
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
                string litOldWorkDetail = "";
                DataTable dt = _mwordorderdalRepository.GetWorkNoteByKey(input.litWorkNoteKey);
                if (dt.Rows.Count > 0)
                {

                    input.litWOKey = dt.Rows[0]["MWorkOrderKey"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["Details"].ToString()))
                    {

                        litOldWorkDetail = dt.Rows[0]["Details"].ToString();
                    }
                }
                MWorkNote workNote = new MWorkNote();
                workNote.MWorkNotesKey = Guid.Parse(input.litWorkNoteKey);
                if (!string.IsNullOrEmpty(input.litWOKey))
                {
                    workNote.MWorkOrderKey = Guid.Parse(input.litWOKey);
                }
                else
                {
                    workNote.MWorkOrderKey = Guid.NewGuid();
                }
                workNote.Details = input.Description;
                workNote.DetailLog = user.UserName + " has updated Work Notes";
                if (litOldWorkDetail != workNote.Details)
                    workNote.NewLog = "Details => " + workNote.Details;
                else
                    workNote.NewLog = "-";
                workNote.OldLog = "Details => " + litOldWorkDetail;
                if (AbpSession.TenantId != null)
                {
                    workNote.TenantId = (int?)AbpSession.TenantId;
                }
                int intSuccessful = _mwordorderdalRepository.UpdateWorkNote(workNote);
                if (intSuccessful > 0)
                {
                    intSuccessful = InsertWorkNoteHistory(workNote, false, user.StaffKey);
                    if (intSuccessful > 0)
                    {
                        message = "Record(s) has been saved.";
                    }
                }

                else
                {
                    throw new UserFriendlyException("Fail to add the record.");
                }
            }
            return message;
        }
        [HttpPost]
        public async Task<string> btnAddPopupWorkNote(PopupWorkNoteInput input)
        {
            string message = "";
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

                DataTable getWOKey = _mwordorderdalRepository.GetWorkOrderByID(Convert.ToInt32(input.litWOID));
                input.litWOKey = getWOKey.Rows[0]["MWorkOrderKey"].ToString();
                MWorkNote workNote = new MWorkNote();
                if (!string.IsNullOrEmpty(input.litWOKey))
                {
                    workNote.MWorkOrderKey = Guid.Parse(input.litWOKey);
                }
                else
                {
                    workNote.MWorkOrderKey = Guid.NewGuid();
                }
                workNote.Details = input.Description;
                workNote.CreatedBy = user.StaffKey;
                workNote.DetailLog = "WO#" + input.litWOID + "; " + user.UserName + " has added Work Notes.";
                workNote.NewLog = "Details => " + workNote.Details;
                if (AbpSession.TenantId != null)
                {
                    workNote.TenantId = (int?)AbpSession.TenantId;
                }
                int intSuccessful = _mwordorderdalRepository.InsertWorkNote(workNote);
                if (intSuccessful > 0)
                {
                    intSuccessful = InsertWorkNoteHistory(workNote, true, user.StaffKey);
                    if (intSuccessful > 0)
                    {
                        message = "Record(s) has been saved.";
                    }
                }
                else
                {
                    throw new UserFriendlyException("Fail to add the record.");
                }
            }
            return message;



        }

        private int InsertWorkNoteHistory(MWorkNote workNote, bool blnInsert, Guid staffKey)
        {

            int success = 0;
            try
            {
                List<History> listHistory = new List<History>();
                History history = new History();
                history.SourceKey = workNote.MWorkOrderKey;
                history.StaffKey = staffKey;
                if (blnInsert)
                {
                    history.Operation = "I";
                    history.NewValue = workNote.NewLog;
                }
                else
                {
                    history.Operation = "U";
                    history.OldValue = workNote.OldLog;
                    history.NewValue = workNote.NewLog;
                }
                history.TableName = "WO";
                history.Detail = "(iRepair) " + workNote.DetailLog;
                history.ModuleName = "iRepair";
                if (AbpSession.TenantId != null)
                {
                    history.TenantId = (int?)AbpSession.TenantId;
                }
                listHistory.Add(history);

                success = _mwordorderdalRepository.InsertHistory(history);

            }
            catch (Exception ex)
            {
                // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return success;
        }
        [HttpGet]
        public PopupWorkNoteInput PopupWorkNoteViewdata(string Key)
        {
            PopupWorkNoteInput o = new PopupWorkNoteInput();
            o.litWorkNoteKey = Key;

            DataTable dt = _mwordorderdalRepository.GetWorkNoteByKey(Key);
            if (dt.Rows.Count > 0)
            {

                o.litWOKey = dt.Rows[0]["MWorkOrderKey"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Details"].ToString()))
                {

                    o.Description = dt.Rows[0]["Details"].ToString();
                }
            }

            return o;
        }
    }

}
