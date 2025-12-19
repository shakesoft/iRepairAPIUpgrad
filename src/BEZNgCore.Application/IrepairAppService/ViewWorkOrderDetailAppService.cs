using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.Authorization.Users;
using BEZNgCore.Common;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IRepairIAppService;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.OpenXmlFormats;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class ViewWorkOrderDetailAppService : BEZNgCoreAppServiceBase
    {
        //private readonly IRepository<MPriority, int> _mpriorityRepository;
        //private readonly IRepository<MArea, int> _mareaRepository;
        //private readonly IRepository<Room, Guid> _roomRepository;
        //private readonly IRepository<MTechnician, int> _technicianRepository;
        //private readonly IRepository<MWorkTimeSheet, int> _mworktimesheetRepository;
        //private readonly IRepository<Staff, Guid> _staffRepository;
        //private readonly IRepository<MWorkOrderStatus, int> _mworkorderstatusRepository;
        //private readonly IRepository<MWorkType, int> _worktypeRepository;
        //private readonly IRepository<MWorkTimeSheetNoteTemplate, int> _mWorktimesheetnotetemplateRepository;
        private readonly IMworkordertimesheetdalRepository _mworkordertimesheetdalRepository;
        private readonly ISqoopeMessgingAppService _sqoopeint;
        public ViewWorkOrderDetailAppService(
               //IRepository<MPriority, int> mpriorityRepository,
               //IRepository<MArea, int> mareaRepository,
               //    IRepository<Room, Guid> roomRepository,
               //    IRepository<MTechnician, int> technicianRepository,
               //IRepository<MWorkOrder, int> mworkorderRepository,
               //IRepository<RoomStatus, Guid> roomstatusRepository,

               //IRepository<MWorkTimeSheet, int> mworktimesheetRepository,
               //IRepository<Staff, Guid> staffRepository,
               //IRepository<MWorkOrderStatus, int> mworkorderstatusRepository,
               //IRepository<MWorkType, int> worktypeRepository,
               //IRepository<MWorkTimeSheetNoteTemplate, int> mWorktimesheetnotetemplateRepository,
               IMworkordertimesheetdalRepository mworkordertimesheetdalRepository,
                ISqoopeMessgingAppService sqoopeint
                )
        {
            //_mpriorityRepository = mpriorityRepository;
            //_mareaRepository = mareaRepository;
            //_roomRepository = roomRepository;
            //_technicianRepository = technicianRepository;
            //_mworkorderRepository = mworkorderRepository;
            //_roomstatusRepository = roomstatusRepository;

            //_mworktimesheetRepository = mworktimesheetRepository;
            //_staffRepository = staffRepository;
            //_mworkorderstatusRepository = mworkorderstatusRepository;
            //_worktypeRepository = worktypeRepository;
            //_mWorktimesheetnotetemplateRepository = mWorktimesheetnotetemplateRepository;
            _mworkordertimesheetdalRepository = mworkordertimesheetdalRepository;
            _sqoopeint = sqoopeint;
        }
        //[HttpGet]
        //public ListResultDto<ViewWorkOrderDetailViewData> GetViewWorkOrderDetailViewData(int Seqno)
        //{
        //    List<ViewWorkOrderDetailViewData> Alllst = new List<ViewWorkOrderDetailViewData>();
        //    ViewWorkOrderDetailViewData a = new ViewWorkOrderDetailViewData();

        //    #region WorkOrderInfoDropdown
        //    WorkOrderInfoDropdown woinf = new WorkOrderInfoDropdown();
        //    //List<DDLRoomOutput> ddlr = new List<DDLRoomOutput>();
        //    //DDLRoomOutput ro = new DDLRoomOutput();
        //    //ro.RoomKey = Guid.Empty;
        //    //ro.Unit = "";
        //    //ddlr.Add(ro);
        //    //var ddlr2 = _roomRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Floor).ThenBy(x => x.Unit)
        //    //  .Select(x => new DDLRoomOutput
        //    //  {
        //    //      RoomKey = x.Id,
        //    //      Unit = x.Unit
        //    //  });
        //    //woinf.Room = ddlr.Concat(ddlr2).ToList();
        //    List<DDLRoomOutput> rl = new List<DDLRoomOutput>();
        //    DDLRoomOutput r1 = new DDLRoomOutput();
        //    r1.RoomKey = Guid.Empty;
        //    r1.Unit = "";
        //    rl.Add(r1);
        //    List<DDLRoomOutput> rr = GetAllRoom();
        //    woinf.Room = rl.Concat(rr).ToList();

        //    List<DDLAreaOutput> ddlarea = new List<DDLAreaOutput>();
        //    DDLAreaOutput area = new DDLAreaOutput();
        //    area.Seqno = 0;
        //    area.Description = "";
        //    ddlarea.Add(area);
        //    List<DDLAreaOutput> ddlarea2 = GetAllArea();
        //    //var ddlarea2 = _mareaRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Description)
        //    //    .Select(x => new DDLAreaOutput
        //    //    {
        //    //        Seqno = x.Id,
        //    //        Description = x.Description
        //    //    });
        //    woinf.Area = ddlarea.Concat(ddlarea2).ToList();

        //    List<DDLWorkTypeOutput> ddlwt = new List<DDLWorkTypeOutput>();
        //    DDLWorkTypeOutput wt = new DDLWorkTypeOutput();
        //    wt.Seqno = 0;
        //    wt.Description = "";
        //    ddlwt.Add(wt);
        //    List<DDLWorkTypeOutput> ddlwt2 = GetAllWorkType();
        //    //var ddlwt2 = _worktypeRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Description)
        //    //  .Select(x => new DDLWorkTypeOutput
        //    //  {
        //    //      Seqno = x.Id,
        //    //      Description = x.Description
        //    //  });
        //    woinf.WorkType = ddlwt.Concat(ddlwt2).ToList();

        //    //woinf.ReportedBy = _staffRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.UserName)
        //    //    .Select(x => new MaidOutput
        //    //    {
        //    //        StaffKey = x.Id,
        //    //        UserName = x.UserName
        //    //    })
        //    //    .ToList();
        //    woinf.ReportedBy = GetAllReportedBy();

        //    //woinf.CurrentStatus = _mworkorderstatusRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Description)
        //    //  .Select(x => new DDLWorkStatusOutput
        //    //  {
        //    //      Seqno = x.Id.ToString(),
        //    //      Description = x.Description
        //    //  }).ToList();
        //    woinf.CurrentStatus = GetAllCurrentStatus();

        //    List <DDPriorityOutput> ddp = new List<DDPriorityOutput>();
        //    DDPriorityOutput p = new DDPriorityOutput();
        //    p.Sort = 99;
        //    p.Priority = "-- Please select --";
        //    ddp.Add(p);
        //    //var ddp2 = _mpriorityRepository.GetAll()
        //    //  .Select(x => new DDPriorityOutput
        //    //  {
        //    //      Sort = x.Sort.Value,
        //    //      Priority = x.Priority
        //    //  });
        //    List<DDPriorityOutput> ddp2 = GetAllPriority();

        //    woinf.Priority = ddp.Concat(ddp2).ToList();


        //    List<DDLTechnicianOutput> ddlt = new List<DDLTechnicianOutput>();
        //    DDLTechnicianOutput t = new DDLTechnicianOutput();
        //    t.Seqno = "0";
        //    t.Name = "--- Please assign technician ----";
        //    ddlt.Add(t);
        //    //var ddlt2 = _technicianRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Name)
        //    //  .Select(x => new DDLTechnicianOutput
        //    //  {
        //    //      Seqno = x.Id.ToString(),
        //    //      Name = x.Name
        //    //  });
        //    List<DDLTechnicianOutput> ddlt2 = GetAllTechnician();

        //    woinf.Technician = ddlt.Concat(ddlt2).ToList();
        //    a.WorkOrderInfoDropdown = woinf;
        //    #endregion

        //    #region Woentryviewdetailinfo
        //    WorkOrderInfoOutput info = new WorkOrderInfoOutput();
        //    DataTable dt = GetWorkOrderByID(Seqno);
        //    if (dt.Rows.Count > 0)
        //    {
        //        info.MWorkOrderKey = dt.Rows[0]["MWorkOrderKey"].ToString();
        //        info.WorkDescription = dt.Rows[0]["Description"].ToString();
        //        info.Notes = dt.Rows[0]["Notes"].ToString();
        //        if (!string.IsNullOrEmpty(dt.Rows[0]["RoomKey"].ToString()))
        //        {
        //            info.RoomKey = dt.Rows[0]["RoomKey"].ToString();
        //            info.Unit = dt.Rows[0]["Room"].ToString();
        //        }


        //        if (!string.IsNullOrEmpty(dt.Rows[0]["MArea"].ToString()))
        //        {
        //            info.MArea = dt.Rows[0]["MArea"].ToString();
        //            info.MAreaDes= GetAreaByKey(Convert.ToInt32(dt.Rows[0]["MArea"].ToString() == "" ? "0" : dt.Rows[0]["MArea"].ToString()));
        //        }

        //        if (!string.IsNullOrEmpty(dt.Rows[0]["Priority"].ToString()))
        //        {
        //            info.Priority = dt.Rows[0]["Priority"].ToString();
        //            info.PriorityDesc = GetPriorityName(Convert.ToInt32(dt.Rows[0]["Priority"].ToString()));
        //        }

        //        if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkType"].ToString()))
        //        {
        //            info.MWorkType = dt.Rows[0]["MWorkType"].ToString();
        //            info.MWorkTypeDes= GetWorkTypeByKey(Convert.ToInt32(dt.Rows[0]["MWorkType"].ToString() == "" ? "-1" : dt.Rows[0]["MWorkType"].ToString()));
        //        }

        //        if (!string.IsNullOrEmpty(dt.Rows[0]["ReportedBy"].ToString()))
        //        {
        //            info.ReportedByKey = dt.Rows[0]["ReportedBy"].ToString();
        //            info.ReportedByName = GetReportedName(dt.Rows[0]["ReportedBy"].ToString());
        //        }


        //        if (!string.IsNullOrEmpty(dt.Rows[0]["ReportedOn"].ToString()))
        //            info.ReportedDate = Convert.ToDateTime(dt.Rows[0]["ReportedOn"]).Date;

        //        if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkOrderStatus"].ToString()))
        //        {
        //            info.WorkOrderStatus = dt.Rows[0]["MWorkOrderStatus"].ToString();
        //            info.WorkOrderStatusDesc = GetWorkStatusByKey(Convert.ToInt32(dt.Rows[0]["MWorkOrderStatus"].ToString()));
        //        }

        //        if (!string.IsNullOrEmpty(dt.Rows[0]["MTechnician"].ToString()))
        //        {
        //            info.Technician = dt.Rows[0]["MTechnician"].ToString();
        //            info.TechnicianName= GetMTechnicianBySeqNo(Convert.ToInt32(dt.Rows[0]["MTechnician"].ToString()));
        //        }


        //        if (!string.IsNullOrEmpty(dt.Rows[0]["ScheduledFrom"].ToString()))
        //            info.ScheduledStart = Convert.ToDateTime(dt.Rows[0]["ScheduledFrom"].ToString());

        //        if (!string.IsNullOrEmpty(dt.Rows[0]["ScheduledTo"].ToString()))
        //            info.ScheduledEnd = Convert.ToDateTime(dt.Rows[0]["ScheduledTo"].ToString());


        //        info.EnteredBy = dt.Rows[0]["EnteredBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["ReportedOn"]);

        //        if (!string.IsNullOrEmpty(dt.Rows[0]["CompletedBy"].ToString()))
        //            info.CompletedBy = dt.Rows[0]["CompletedBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["CompletedDateTime"]);

        //        if (!string.IsNullOrEmpty(dt.Rows[0]["SignedOffBy"].ToString()))
        //            info.SignedOffBy = dt.Rows[0]["SignedOffBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["SignedOffDateTime"]);

        //        if (!string.IsNullOrEmpty(dt.Rows[0]["CancelledBy"].ToString()))
        //            info.CancelledBy = dt.Rows[0]["CancelledBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["CancelledDateTime"]);

        //        if (!string.IsNullOrEmpty(dt.Rows[0]["LastUpdateBy"].ToString()))
        //            info.LastUpdateBy = dt.Rows[0]["LastUpdateBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["LastUpdateDateTime"]);

        //    }
        //    bool blnHasStartedTask = MaidHasStartedTask(Seqno);
        //    if (blnHasStartedTask)
        //        info.btnStartEnabled = false;
        //    else
        //        info.btnEndEnabled = false;
        //    a.WorkOrderInfoOutput = info;
        //    #endregion
        //    #region Wotimesheet
        //    List<WoTimeSheetListOutput> wotlist = new List<WoTimeSheetListOutput>();
        //    wotlist = GetWorkTimeSheetByWOID(Seqno);
        //    a.WoTimeSheetList = wotlist;
        //    #endregion
        //    #region BlockUnBlockRoom
        //    List<BlockUnBlockRoomListOutput> burlst = new List<BlockUnBlockRoomListOutput>();
        //    burlst = GetBlockRoomByWorkOrderID(Seqno);
        //    a.BlockUnBlockRoomList = burlst;
        //    #endregion

        //    #region WoWorkNote
        //    List<WoWorkNote> wonlst = new List<WoWorkNote>();
        //    wonlst = GetWorkNotes(info.MWorkOrderKey);
        //    a.WoWorkNoteList = wonlst;
        //    #endregion
        //    #region SecurityAudit
        //    List<WoSecurityAuditlist> wosalst = new List<WoSecurityAuditlist>();
        //    wosalst = GetWorkOrderHistory(info.MWorkOrderKey);
        //    a.WoSecurityAuditlist = wosalst;
        //    #endregion

        //    Alllst.Add(a);
        //    return new ListResultDto<ViewWorkOrderDetailViewData>(Alllst);
        //}

        [HttpGet]
        public async Task<PagedResultDto<GetViewWorkOrderOutput>> GetViewWorkOrder(string status, string task)
        {
            try
            {
                List<GetViewWorkOrderOutput> dt = new List<GetViewWorkOrderOutput>();
                GetViewWorkOrderOutput a = new GetViewWorkOrderOutput();
                List<VWorkOrderOutput> dtt = new List<VWorkOrderOutput>();
                if (task == "unassign")
                {
                    dtt = _mworkordertimesheetdalRepository.GetUnassignedTechWorkOrder();
                    a.WorkOrderStatus = "Unassigned Technician Tasks";

                }
                else if (status != "")
                {
                    dtt = _mworkordertimesheetdalRepository.GetWorkOrderByStatus(Convert.ToInt32(status));
                    a.WorkOrderStatus = CommomData.GetWorkOrderStatusDescriptionByStatus(status);
                }
                else
                {
                    dtt = _mworkordertimesheetdalRepository.GetWorkOrderByStatus(0);
                    a.WorkOrderStatus = CommomData.GetWorkOrderStatusDescriptionByStatus(status);
                }

                a.vWorkOrderOutputList = dtt;
                dt.Add(a);
                var Count = a.vWorkOrderOutputList.Count;

                return new PagedResultDto<GetViewWorkOrderOutput>(
                Count,
                dt
            );
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public ListResultDto<ViewWODetailWOInfo> GetViewWODetailWOInfo(int Seqno)
        {
            List<ViewWODetailWOInfo> Alllst = new List<ViewWODetailWOInfo>();
            ViewWODetailWOInfo a = new ViewWODetailWOInfo();

            #region WorkOrderInfoDropdown
            WorkOrderInfoDropdown woinf = new WorkOrderInfoDropdown();

            List<DDLRoomOutput> rl = new List<DDLRoomOutput>();
            DDLRoomOutput r1 = new DDLRoomOutput();
            r1.RoomKey = Guid.Empty;
            r1.Unit = "";
            rl.Add(r1);
            List<DDLRoomOutput> rr = GetAllRoom();
            woinf.Room = rl.Concat(rr).ToList();

            List<DDLAreaOutput> ddlarea = new List<DDLAreaOutput>();
            DDLAreaOutput area = new DDLAreaOutput();
            area.Seqno = -1;
            area.Description = "";
            ddlarea.Add(area);
            List<DDLAreaOutput> ddlarea2 = GetAllArea();

            woinf.Area = ddlarea.Concat(ddlarea2).ToList();

            List<DDLWorkTypeOutput> ddlwt = new List<DDLWorkTypeOutput>();
            DDLWorkTypeOutput wt = new DDLWorkTypeOutput();
            wt.Seqno = -1;
            wt.Description = "";
            ddlwt.Add(wt);
            List<DDLWorkTypeOutput> ddlwt2 = GetAllWorkType();

            woinf.WorkType = ddlwt.Concat(ddlwt2).ToList();

            woinf.ReportedBy = GetAllReportedBy();


            woinf.CurrentStatus = GetAllCurrentStatus();

            List<DDPriorityOutput> ddp = new List<DDPriorityOutput>();
            DDPriorityOutput p = new DDPriorityOutput();
            p.Sort = 99;
            p.Priority = "-- Please select --";
            ddp.Add(p);

            List<DDPriorityOutput> ddp2 = GetAllPriority();

            woinf.Priority = ddp.Concat(ddp2).ToList();


            List<DDLTechnicianOutput> ddlt = new List<DDLTechnicianOutput>();
            DDLTechnicianOutput t = new DDLTechnicianOutput();
            t.Seqno = "-1";
            t.Name = "--- Please assign technician ----";
            ddlt.Add(t);

            List<DDLTechnicianOutput> ddlt2 = GetAllTechnician();

            woinf.Technician = ddlt.Concat(ddlt2).ToList();
            a.WorkOrderInfoDropdown = woinf;
            #endregion

            #region Woentryviewdetailinfo
            WorkOrderInfoOutput info = new WorkOrderInfoOutput();
            DataTable dt = GetWorkOrderByID(Seqno);
            if (dt.Rows.Count > 0)
            {
                info.MWorkOrderKey = dt.Rows[0]["MWorkOrderKey"].ToString();
                if (string.IsNullOrEmpty(info.MWorkOrderKey))
                    info.MWorkOrderKey = Guid.NewGuid().ToString();
                info.WorkDescription = dt.Rows[0]["Description"].ToString();
                info.Notes = dt.Rows[0]["Notes"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["RoomKey"].ToString()))
                {
                    info.RoomKey = dt.Rows[0]["RoomKey"].ToString();
                    info.Unit = dt.Rows[0]["Room"].ToString();
                }


                if (!string.IsNullOrEmpty(dt.Rows[0]["MArea"].ToString()))
                {
                    info.MArea = dt.Rows[0]["MArea"].ToString();
                    info.MAreaDes = GetAreaByKey(Convert.ToInt32(dt.Rows[0]["MArea"].ToString() == "" ? "-1" : dt.Rows[0]["MArea"].ToString()));
                }

                if (!string.IsNullOrEmpty(dt.Rows[0]["Priority"].ToString()))
                {
                    info.Priority = dt.Rows[0]["Priority"].ToString();
                    info.PriorityDesc = GetPriorityName(Convert.ToInt32(dt.Rows[0]["Priority"].ToString()));
                }

                if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkType"].ToString()))
                {
                    info.MWorkType = dt.Rows[0]["MWorkType"].ToString();
                    info.MWorkTypeDes = GetWorkTypeByKey(Convert.ToInt32(dt.Rows[0]["MWorkType"].ToString() == "" ? "-1" : dt.Rows[0]["MWorkType"].ToString()));
                }

                if (!string.IsNullOrEmpty(dt.Rows[0]["ReportedBy"].ToString()))
                {
                    info.ReportedByKey = dt.Rows[0]["ReportedBy"].ToString();
                    info.ReportedByName = GetReportedName(dt.Rows[0]["ReportedBy"].ToString());
                }


                if (!string.IsNullOrEmpty(dt.Rows[0]["ReportedOn"].ToString()))
                    info.ReportedDate = Convert.ToDateTime(dt.Rows[0]["ReportedOn"]).Date;

                if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkOrderStatus"].ToString()))
                {
                    info.WorkOrderStatus = dt.Rows[0]["MWorkOrderStatus"].ToString();
                    info.WorkOrderStatusDesc = GetWorkStatusByKey(Convert.ToInt32(dt.Rows[0]["MWorkOrderStatus"].ToString()));
                }

                if (!string.IsNullOrEmpty(dt.Rows[0]["MTechnician"].ToString()))
                {
                    info.Technician = dt.Rows[0]["MTechnician"].ToString();
                    info.TechnicianName = GetMTechnicianBySeqNo(Convert.ToInt32(dt.Rows[0]["MTechnician"].ToString()));
                }


                if (!string.IsNullOrEmpty(dt.Rows[0]["ScheduledFrom"].ToString()))
                    info.ScheduledStart = Convert.ToDateTime(dt.Rows[0]["ScheduledFrom"].ToString());

                if (!string.IsNullOrEmpty(dt.Rows[0]["ScheduledTo"].ToString()))
                    info.ScheduledEnd = Convert.ToDateTime(dt.Rows[0]["ScheduledTo"].ToString());


                info.EnteredBy = dt.Rows[0]["EnteredBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["ReportedOn"]);

                if (!string.IsNullOrEmpty(dt.Rows[0]["CompletedBy"].ToString()))
                    info.CompletedBy = dt.Rows[0]["CompletedBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["CompletedDateTime"]);

                if (!string.IsNullOrEmpty(dt.Rows[0]["SignedOffBy"].ToString()))
                    info.SignedOffBy = dt.Rows[0]["SignedOffBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["SignedOffDateTime"]);

                if (!string.IsNullOrEmpty(dt.Rows[0]["CancelledBy"].ToString()))
                    info.CancelledBy = dt.Rows[0]["CancelledBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["CancelledDateTime"]);

                if (!string.IsNullOrEmpty(dt.Rows[0]["LastUpdateBy"].ToString()))
                    info.LastUpdateBy = dt.Rows[0]["LastUpdateBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["LastUpdateDateTime"]);

            }

            #endregion

            a.WorkOrderInfoOutput = info;
            Alllst.Add(a);
            return new ListResultDto<ViewWODetailWOInfo>(Alllst);
        }

        public ListResultDto<ViewWODetailWOImgInfo> GetViewWODetailWOInfoImg(int Seqno)
        {
            List<ViewWODetailWOImgInfo> Alllst = new List<ViewWODetailWOImgInfo>();
            ViewWODetailWOImgInfo a = new ViewWODetailWOImgInfo();

            #region WorkOrderInfoDropdown
            WorkOrderInfoDropdown woinf = new WorkOrderInfoDropdown();

            List<DDLRoomOutput> rl = new List<DDLRoomOutput>();
            DDLRoomOutput r1 = new DDLRoomOutput();
            r1.RoomKey = Guid.Empty;
            r1.Unit = "";
            rl.Add(r1);
            List<DDLRoomOutput> rr = GetAllRoom();
            woinf.Room = rl.Concat(rr).ToList();

            List<DDLAreaOutput> ddlarea = new List<DDLAreaOutput>();
            DDLAreaOutput area = new DDLAreaOutput();
            area.Seqno = -1;
            area.Description = "";
            ddlarea.Add(area);
            List<DDLAreaOutput> ddlarea2 = GetAllArea();

            woinf.Area = ddlarea.Concat(ddlarea2).ToList();

            List<DDLWorkTypeOutput> ddlwt = new List<DDLWorkTypeOutput>();
            DDLWorkTypeOutput wt = new DDLWorkTypeOutput();
            wt.Seqno = -1;
            wt.Description = "";
            ddlwt.Add(wt);
            List<DDLWorkTypeOutput> ddlwt2 = GetAllWorkType();

            woinf.WorkType = ddlwt.Concat(ddlwt2).ToList();

            woinf.ReportedBy = GetAllReportedBy();


            woinf.CurrentStatus = GetAllCurrentStatus();

            List<DDPriorityOutput> ddp = new List<DDPriorityOutput>();
            DDPriorityOutput p = new DDPriorityOutput();
            p.Sort = 99;
            p.Priority = "-- Please select --";
            ddp.Add(p);

            List<DDPriorityOutput> ddp2 = GetAllPriority();

            woinf.Priority = ddp.Concat(ddp2).ToList();


            List<DDLTechnicianOutput> ddlt = new List<DDLTechnicianOutput>();
            DDLTechnicianOutput t = new DDLTechnicianOutput();
            t.Seqno = "-1";
            t.Name = "--- Please assign technician ----";
            ddlt.Add(t);

            List<DDLTechnicianOutput> ddlt2 = GetAllTechnician();

            woinf.Technician = ddlt.Concat(ddlt2).ToList();
            a.WorkOrderInfoDropdown = woinf;
            #endregion
            List<FWOImageD> lst = new List<FWOImageD>();
            #region Woentryviewdetailinfo
            WorkOrderInfoOutput info = new WorkOrderInfoOutput();
            DataTable dt = GetWorkOrderByID(Seqno);
            if (dt.Rows.Count > 0)
            {
                info.MWorkOrderKey = dt.Rows[0]["MWorkOrderKey"].ToString();
                if (string.IsNullOrEmpty(info.MWorkOrderKey))
                    info.MWorkOrderKey = Guid.NewGuid().ToString();
                info.WorkDescription = dt.Rows[0]["Description"].ToString();
                info.Notes = dt.Rows[0]["Notes"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["RoomKey"].ToString()))
                {
                    info.RoomKey = dt.Rows[0]["RoomKey"].ToString();
                    info.Unit = dt.Rows[0]["Room"].ToString();
                }


                if (!string.IsNullOrEmpty(dt.Rows[0]["MArea"].ToString()))
                {
                    info.MArea = dt.Rows[0]["MArea"].ToString();
                    info.MAreaDes = GetAreaByKey(Convert.ToInt32(dt.Rows[0]["MArea"].ToString() == "" ? "-1" : dt.Rows[0]["MArea"].ToString()));
                }

                if (!string.IsNullOrEmpty(dt.Rows[0]["Priority"].ToString()))
                {
                    info.Priority = dt.Rows[0]["Priority"].ToString();
                    info.PriorityDesc = GetPriorityName(Convert.ToInt32(dt.Rows[0]["Priority"].ToString()));
                }

                if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkType"].ToString()))
                {
                    info.MWorkType = dt.Rows[0]["MWorkType"].ToString();
                    info.MWorkTypeDes = GetWorkTypeByKey(Convert.ToInt32(dt.Rows[0]["MWorkType"].ToString() == "" ? "-1" : dt.Rows[0]["MWorkType"].ToString()));
                }

                if (!string.IsNullOrEmpty(dt.Rows[0]["ReportedBy"].ToString()))
                {
                    info.ReportedByKey = dt.Rows[0]["ReportedBy"].ToString();
                    info.ReportedByName = GetReportedName(dt.Rows[0]["ReportedBy"].ToString());
                }


                if (!string.IsNullOrEmpty(dt.Rows[0]["ReportedOn"].ToString()))
                    info.ReportedDate = Convert.ToDateTime(dt.Rows[0]["ReportedOn"]).Date;

                if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkOrderStatus"].ToString()))
                {
                    info.WorkOrderStatus = dt.Rows[0]["MWorkOrderStatus"].ToString();
                    info.WorkOrderStatusDesc = GetWorkStatusByKey(Convert.ToInt32(dt.Rows[0]["MWorkOrderStatus"].ToString()));
                }

                if (!string.IsNullOrEmpty(dt.Rows[0]["MTechnician"].ToString()))
                {
                    info.Technician = dt.Rows[0]["MTechnician"].ToString();
                    info.TechnicianName = GetMTechnicianBySeqNo(Convert.ToInt32(dt.Rows[0]["MTechnician"].ToString()));
                }


                if (!string.IsNullOrEmpty(dt.Rows[0]["ScheduledFrom"].ToString()))
                    info.ScheduledStart = Convert.ToDateTime(dt.Rows[0]["ScheduledFrom"].ToString());

                if (!string.IsNullOrEmpty(dt.Rows[0]["ScheduledTo"].ToString()))
                    info.ScheduledEnd = Convert.ToDateTime(dt.Rows[0]["ScheduledTo"].ToString());


                //info.EnteredBy = dt.Rows[0]["EnteredBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["ReportedOn"]);
                info.EnteredBy = dt.Rows[0]["EnteredBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["EnteredDateTime"]);

                if (!string.IsNullOrEmpty(dt.Rows[0]["CompletedBy"].ToString()))
                    info.CompletedBy = dt.Rows[0]["CompletedBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["CompletedDateTime"]);

                if (!string.IsNullOrEmpty(dt.Rows[0]["SignedOffBy"].ToString()))
                    info.SignedOffBy = dt.Rows[0]["SignedOffBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["SignedOffDateTime"]);

                if (!string.IsNullOrEmpty(dt.Rows[0]["CancelledBy"].ToString()))
                    info.CancelledBy = dt.Rows[0]["CancelledBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["CancelledDateTime"]);

                if (!string.IsNullOrEmpty(dt.Rows[0]["LastUpdateBy"].ToString()))
                    info.LastUpdateBy = dt.Rows[0]["LastUpdateBy"].ToString() + " @" + CommomData.GetDateTimeToDisplay(dt.Rows[0]["LastUpdateDateTime"]);


                #region image

                DataTable dtimg = _mworkordertimesheetdalRepository.GetDocumentByWoKey(Guid.Parse(info.MWorkOrderKey));
                foreach (DataRow dr in dtimg.Rows)
                {
                    FWOImageD o = new FWOImageD();
                    //o.Id = Convert.ToInt32(dr["sort"]);
                    //o.FileName = dr["Description"].ToString();
                    //o.ContentType = dr["Document"].ToString();
                    //o.Data = dr["Signature"] as byte[];
                    o.Id = !DBNull.Value.Equals(dr["sort"]) ? Convert.ToInt32(dr["sort"]) : 0;
                    o.FileName = !DBNull.Value.Equals(dr["Description"]) ? dr["Description"].ToString() : "";
                    o.ContentType = !DBNull.Value.Equals(dr["Document"]) ? dr["Document"].ToString() : "";
                    o.Data = !DBNull.Value.Equals(dr["Signature"]) ? dr["Signature"] as byte[] : Array.Empty<byte>();
                    var base64Image = Convert.ToBase64String(o.Data);
                    o.imageSrc = $"data:{o.ContentType};base64,{base64Image}";
                    lst.Add(o);
                }
                #endregion
            }

            #endregion

            a.WorkOrderInfoOutput = info;
            a.imglst = lst;
          
            Alllst.Add(a);
            return new ListResultDto<ViewWODetailWOImgInfo>(Alllst);
        }
        public ListResultDto<ViewWODetailTimeSheet> GetViewWODetailTimeSheet(int Seqno)
        {
            List<ViewWODetailTimeSheet> Alllst = new List<ViewWODetailTimeSheet>();
            ViewWODetailTimeSheet a = new ViewWODetailTimeSheet();
            #region Wotimesheet
            List<WoTimeSheetListOutput> wotlist = new List<WoTimeSheetListOutput>();
            wotlist = GetWorkTimeSheetByWOID(Seqno);
            a.WoTimeSheetList = wotlist;
            #endregion
            bool blnHasStartedTask = MaidHasStartedTask(Seqno);
            if (blnHasStartedTask)
                a.btnStartEnabled = false;
            else
                a.btnEndEnabled = false;
            Alllst.Add(a);
            return new ListResultDto<ViewWODetailTimeSheet>(Alllst);
        }
        public ListResultDto<ViewWODetailBlockUnBlockRoom> GetViewWODetailBlockUnBlockRoom(int Seqno)
        {
            List<ViewWODetailBlockUnBlockRoom> Alllst = new List<ViewWODetailBlockUnBlockRoom>();
            ViewWODetailBlockUnBlockRoom a = new ViewWODetailBlockUnBlockRoom();

            #region BlockUnBlockRoom
            List<BlockUnBlockRoomListOutput> burlst = new List<BlockUnBlockRoomListOutput>();
            burlst = GetBlockRoomByWorkOrderID(Seqno);
            a.BlockUnBlockRoomList = burlst;
            #endregion


            Alllst.Add(a);
            return new ListResultDto<ViewWODetailBlockUnBlockRoom>(Alllst);
        }
        public ListResultDto<ViewWODetailWorkNote> GetViewWODetailWorkNote(int Seqno)
        {
            List<ViewWODetailWorkNote> Alllst = new List<ViewWODetailWorkNote>();
            ViewWODetailWorkNote a = new ViewWODetailWorkNote();
            WorkOrderInfoOutput info = new WorkOrderInfoOutput();
            List<WoWorkNote> wonlst = new List<WoWorkNote>();
            DataTable dt = GetWorkOrderByID(Seqno);

            if (dt.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(dt.Rows[0]["MWorkOrderKey"].ToString()))
                {
                    info.MWorkOrderKey = dt.Rows[0]["MWorkOrderKey"].ToString();
                    #region WoWorkNote
                    wonlst = GetWorkNotes(info.MWorkOrderKey);
                    a.WoWorkNoteList = wonlst;
                    #endregion
                }

            }


            Alllst.Add(a);
            return new ListResultDto<ViewWODetailWorkNote>(Alllst);
        }
        public ListResultDto<ViewWODetailSecurityAudit> GetViewWODetailSecurityAudit(int Seqno)
        {
            List<ViewWODetailSecurityAudit> Alllst = new List<ViewWODetailSecurityAudit>();
            ViewWODetailSecurityAudit a = new ViewWODetailSecurityAudit();
            WorkOrderInfoOutput info = new WorkOrderInfoOutput();
            List<WoSecurityAuditlist> wosalst = new List<WoSecurityAuditlist>();
            DataTable dt = GetWorkOrderByID(Seqno);
            if (dt.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(dt.Rows[0]["MWorkOrderKey"].ToString()))
                {
                    info.MWorkOrderKey = dt.Rows[0]["MWorkOrderKey"].ToString();
                    #region SecurityAudit

                    wosalst = GetWorkOrderHistory(info.MWorkOrderKey);
                    a.WoSecurityAuditlist = wosalst;
                    #endregion
                }

            }
           

            Alllst.Add(a);
            return new ListResultDto<ViewWODetailSecurityAudit>(Alllst);
        }
        [HttpGet]
        public ListResultDto<PopupTechnicianViewData> GetPopupTechnicianViewData(List<string> listWO)
        {
            if (listWO.ToList().Count < 0)
            {
                throw new UserFriendlyException("Please Select Work Order(s) to proceed");
            }
            List<PopupTechnicianViewData> Alllst = new List<PopupTechnicianViewData>();
            PopupTechnicianViewData a = new PopupTechnicianViewData();

            List<DDLTechnicianOutput> ddlt = new List<DDLTechnicianOutput>();
            DDLTechnicianOutput t = new DDLTechnicianOutput();
            t.Seqno = "-1";
            t.Name = "--- Please assign technician ----";
            ddlt.Add(t);
            //var ddlt2 = _technicianRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Name)
            //  .Select(x => new DDLTechnicianOutput
            //  {
            //      Seqno = x.Id.ToString(),
            //      Name = x.Name
            //  });
            List<DDLTechnicianOutput> ddlt2 = GetAllTechnician();

            a.DDLTechnician = ddlt.Concat(ddlt2).ToList();
            a.listWO = listWO;
            Alllst.Add(a);
            return new ListResultDto<PopupTechnicianViewData>(Alllst);
        }
        #region Btn Assign Technician Click
        [HttpPost]
        public async Task<List<MessageNotiViewLatest>> btnTechnicianAssignClick(TAssignInput input)
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

                    if (Convert.ToInt32(input.ddlTechnicianKey) > -1)
                    {

                        if (input.listWO.Count > 0)
                        {
                            //List<string> listWOID = (List<string>)Session[SessionHelper.SessionTempSelectedWO];

                            int intTechnicianID = Convert.ToInt32(input.ddlTechnicianKey);
                            string strTechnicianName = input.ddlTechnicianName.Trim();
                            List<MWorkOrderInput> listWork = new List<MWorkOrderInput>();
                            MWorkOrderInput work;
                            DataTable dt;
                            foreach (string woID in input.listWO)
                            {
                                dt = GetWorkOrderByID(Convert.ToInt32(woID));
                                work = new MWorkOrderInput();
                                work.Id = Convert.ToInt32(woID);
                                if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkOrderKey"].ToString()) && dt.Rows[0]["MWorkOrderKey"].ToString() != "")
                                {
                                    work.MWorkOrderKey = Guid.Parse(dt.Rows[0]["MWorkOrderKey"].ToString());
                                }
                                else
                                {
                                    work.MWorkOrderKey = Guid.Empty;
                                }
                                work.Description = dt.Rows[0]["Description"].ToString();
                                work.MTechnician = intTechnicianID;
                                work.MTechnicianName = strTechnicianName;
                                work.LastUpdateBy = user.UserName;
                                work.LastUpdateStaffKey = user.StaffKey;
                                if (!string.IsNullOrEmpty(dt.Rows[0]["MTechnician"].ToString()) && dt.Rows[0]["MTechnician"].ToString() != "")
                                    work.OldLog = "Technician => " + GetMTechnicianBySeqNo(Convert.ToInt32(dt.Rows[0]["MTechnician"].ToString()));
                                else
                                    work.OldLog = "Technician => -";
                                if (!string.IsNullOrEmpty(dt.Rows[0]["MTechnician"].ToString()) && dt.Rows[0]["MTechnician"].ToString() != "")
                                {
                                    if (intTechnicianID != Convert.ToInt32(dt.Rows[0]["MTechnician"].ToString()))
                                        work.NewLog = "Technician => " + strTechnicianName;
                                }
                                else
                                    work.NewLog = "Technician => " + strTechnicianName;
                                if (AbpSession.TenantId != null)
                                {
                                    work.TenantId = (int?)AbpSession.TenantId;
                                }
                                listWork.Add(work);
                            }
                            int intSuccess = _mworkordertimesheetdalRepository.UpdatAssignTechnicianToWorkOrder(listWork);
                            if (intSuccess == 1)
                            {
                                List<History> listHistory = new List<History>();
                                History history;
                                foreach (MWorkOrderInput w in listWork)
                                {
                                    history = new History();
                                    history.Id = Guid.NewGuid();
                                    history.SourceKey = w.MWorkOrderKey;
                                    history.StaffKey = user.StaffKey;
                                    history.Operation = "U";
                                    history.OldValue = w.OldLog;
                                    history.NewValue = w.NewLog;
                                    history.TableName = "WOAssign";
                                    history.Detail = "(iRepair) " + "WO#" + w.Id + "; " + user.UserName + " assigned to " + w.MTechnicianName;
                                    history.Detail = (history.Detail.Trim().Length > 200 ? history.Detail.Trim().Substring(0, 190) + "..." : history.Detail.Trim());
                                    history.ModuleName = "iRepair";
                                    if (AbpSession.TenantId != null)
                                    {
                                        history.TenantId = (int?)AbpSession.TenantId;
                                    }
                                    listHistory.Add(history);
                                }
                                intSuccess = _mworkordertimesheetdalRepository.InsertHistoryList(listHistory);
                                if (intSuccess == 1)
                                {
                                    // Send msg to sqoope users if any
                                    // SqoopeMessgingHelper.SendiRepairMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iRepair_AssignWorkOrder, listWork, null, BLL_Staff.GetLoginUserStaffKey());

                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_additem", "AssignTechnicianClose();", true);
                                    Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_AssignWorkOrder, listWork, null, user.StaffKey.ToString());
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

                            }
                            else
                            {
                                throw new UserFriendlyException("Update Fail");
                            }

                        }
                        else
                        {

                            throw new UserFriendlyException("Please close this window and select work order again.");
                        }


                    }
                    else
                    {
                        throw new UserFriendlyException("Please select Technician");
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message);
                }
            }
            return Alllstlatest;

        }



        #endregion

        [HttpGet]
        public ListResultDto<PopupWorkOrderStatusViewData> GetpopupWorkOrderStatusViewData(List<string> listWO, string mode = "multiple", string woid = "")
        {
            List<PopupWorkOrderStatusViewData> Alllst = new List<PopupWorkOrderStatusViewData>();
            PopupWorkOrderStatusViewData a = new PopupWorkOrderStatusViewData();
            if (mode != "")
            {
                // litTitle.Text = "Update Work Order Status To";

                List<DDLWorkStatusOutput> ddwos = new List<DDLWorkStatusOutput>();
                DDLWorkStatusOutput wos = new DDLWorkStatusOutput();
                wos.Seqno = "-1";
                wos.Description = "--- Please select work order status ----";
                ddwos.Add(wos);
                //  var ddwos2 = _mworkorderstatusRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Description)
                //.Select(x => new DDLWorkStatusOutput
                //{
                //    Seqno = x.Id.ToString(),
                //    Description = x.Description
                //});
                List<DDLWorkStatusOutput> ddwos2 = GetAllCurrentStatus();
                a.WorkOrderStatus = ddwos.Concat(ddwos2).ToList();
                if (mode == "multiple")
                {
                    if (listWO.ToList().Count < 0)
                    {
                        throw new UserFriendlyException("Please close this window and select work order again.");
                    }
                    a.listWO = listWO;
                }
                else if (mode == "single")
                {
                    if (woid != "")
                    {
                        DataTable dt = GetWorkOrderByID(Convert.ToInt32(woid));
                        if (dt.Rows.Count > 0)
                        {
                            a.ddlWorkOrderStatusSelectedValue = dt.Rows[0]["MWorkOrderStatus"].ToString();

                        }
                        List<string> listWOID = new List<string>();
                        listWOID.Add(woid);
                        a.listWO = listWOID;

                    }
                }



            }
            Alllst.Add(a);
            return new ListResultDto<PopupWorkOrderStatusViewData>(Alllst);
        }
        #region Btn Update WO Status Click
        [HttpPost]
        public async Task<List<MessageNotiViewLatest>> btnWOStatusUpdateClick(WOStatusUpdateInput input)
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
                try
                {
                    if (Convert.ToInt32(input.ddlWostatusKey) > -1)
                    {

                        if (input.listWO.Count > 0)
                        {


                            int intWOStatus = Convert.ToInt32(input.ddlWostatusKey);
                            string strWOStatusDesc = input.ddlWostatusName.Trim();
                           
                            List<MWorkOrderInput> listWork = new List<MWorkOrderInput>();
                            List<Room> rlst = new List<Room>();
                            MWorkOrderInput work;
                            DataTable dt;
                            foreach (string woID in input.listWO)
                            {

                                dt = GetWorkOrderByID(Convert.ToInt32(woID));
                                work = new MWorkOrderInput();
                                work.Id = Convert.ToInt32(woID);
                                if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkOrderKey"].ToString()) && dt.Rows[0]["MWorkOrderKey"].ToString() != "")
                                {
                                    work.MWorkOrderKey = Guid.Parse(dt.Rows[0]["MWorkOrderKey"].ToString());
                                }
                                else
                                {
                                    work.MWorkOrderKey = Guid.NewGuid();
                                }
                                work.Description = dt.Rows[0]["Description"].ToString();
                                work.MWorkOrderStatus = intWOStatus;
                                work.MWorkOrderStatusDesc = strWOStatusDesc;
                                work.LastUpdateBy = user.UserName;
                                work.LastUpdateStaffKey = user.StaffKey;
                                work.LastUpdateDateTime = DateTime.Now;
                                if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkOrderStatus"].ToString()) && dt.Rows[0]["MWorkOrderStatus"].ToString() != "")
                                    work.OldLog = "Work Order Status => " + GetWorkStatusByKey(Convert.ToInt32(dt.Rows[0]["MWorkOrderStatus"].ToString()));
                                else
                                    work.OldLog = "Work Order Status => -";
                                if (intWOStatus != Convert.ToInt32(dt.Rows[0]["MWorkOrderStatus"].ToString()))
                                    work.NewLog = "Work Order Status => " + strWOStatusDesc;
                                if (AbpSession.TenantId != null)
                                {
                                    work.TenantId = (int?)AbpSession.TenantId;
                                }
                                listWork.Add(work);
                                #region wo completed=> maintenance required  to Inspection required
                                if (strWOStatusDesc == "Completed")
                                {
                                    Room room = new Room();
                                    if (!String.IsNullOrEmpty(dt.Rows[0]["RoomKey"].ToString()))
                                    {
                                        room.Id = Guid.Parse(dt.Rows[0]["RoomKey"].ToString());
                                        string status = "Maintenance Required";
                                        List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus(status);
                                        string maidStatusKey = lstmk.Count>0?lstmk[0].MaidStatusKey:Guid.Empty.ToString();
                                        room.MaidStatusKey = new Guid(maidStatusKey);
                                        int exitcount = _mworkordertimesheetdalRepository.MRCheckExit(room.Id,room.MaidStatusKey);
                                        if (exitcount > 0)
                                        {
                                            List<GetMaidStatusOutput> lstmk1 = GetMaidStatusKeyByStatus(input.cleanStatus == "Dirty" ? CommomData.HouseKeepingMaidStatusDirty : input.cleanStatus == "Clean" ? CommomData.HouseKeepingMaidStatusClean : CommomData.HouseKeepingMaidStatusInspectionRequired);
                                            string maidStatusKey1 = lstmk1[0].MaidStatusKey;
                                            if (!String.IsNullOrEmpty(dt.Rows[0]["Room"].ToString()))
                                                room.Unit = dt.Rows[0]["Room"].ToString();
                                            room.MaidStatusKey = new Guid(maidStatusKey1);
                                            rlst.Add(room);
                                        }
                                        #region 13.10.2025 update (else scope)
                                        else {
                                            List<GetMaidStatusOutput> lstmk1 = GetMaidStatusKeyByStatus(input.cleanStatus == "Dirty" ? CommomData.HouseKeepingMaidStatusDirty : input.cleanStatus == "Clean" ? CommomData.HouseKeepingMaidStatusClean : CommomData.HouseKeepingMaidStatusInspectionRequired);
                                            string maidStatusKey1 = lstmk1[0].MaidStatusKey;
                                            if (!String.IsNullOrEmpty(dt.Rows[0]["Room"].ToString()))
                                                room.Unit = dt.Rows[0]["Room"].ToString();
                                            room.MaidStatusKey = new Guid(maidStatusKey1);
                                            rlst.Add(room);
                                        }
                                        #endregion
                                    }

                                }
                                #endregion
                            }
                            int intSuccess = _mworkordertimesheetdalRepository.UpdatWOStatusToWorkOrder(listWork);
                            if (intSuccess == 1)
                            {
                                if(rlst.Count > 0)
                                {
                                    foreach (Room r in rlst)
                                    {
                                        int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(r);
                                    }
                                }
                                
                                List<History> listHistory = new List<History>();
                                History history;
                                foreach (MWorkOrderInput w in listWork)
                                {
                                    history = new History();
                                    history.Id = Guid.NewGuid();
                                    history.SourceKey = w.MWorkOrderKey;
                                    history.StaffKey = user.StaffKey;
                                    history.Operation = "U";
                                    history.OldValue = w.OldLog;
                                    history.NewValue = w.NewLog;
                                    history.TableName = "WO";
                                    //history.Detail = "(iRepair) " + "WO#" + w.Id + "; " + user.UserName + " updated status to " + w.MWorkOrderStatusDesc;
                                    history.Detail = "(iRepair) " + user.UserName + " updated Room#" + rlst[0].Unit+" as "+input.cleanStatus;
                                    history.Detail = (history.Detail.Trim().Length > 200 ? history.Detail.Trim().Substring(0, 190) + "..." : history.Detail.Trim());
                                    history.ModuleName = "iRepair";
                                    if (AbpSession.TenantId != null)
                                    {
                                        history.TenantId = (int?)AbpSession.TenantId;
                                    }
                                    listHistory.Add(history);
                                }
                                intSuccess = _mworkordertimesheetdalRepository.InsertHistoryList(listHistory);
                                if (intSuccess == 1)
                                {

                                    // Send msg to sqoope users if any
                                    //SqoopeMessgingHelper.SendiRepairMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iRepair_UpdateWorkOrderStatus, listWork, null, BLL_Staff.GetLoginUserStaffKey());

                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_additem", "UpdateWOStatusClose();", true);

                                    Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_UpdateWorkOrderStatus, listWork, null, user.StaffKey.ToString());
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
                            }
                            else
                            {
                                throw new UserFriendlyException("Update Fail");
                            }

                        }
                        else
                        {
                            throw new UserFriendlyException("Please close this window and select work order again.");
                        }


                    }
                    else
                    {
                        throw new UserFriendlyException("Please select Workorderstatus");
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message);
                }
            }
            return Alllstlatest;

        }


        #endregion

        #region Start Or End
        [HttpPost]
        public async Task<string> StartTimeSheet(string woID)
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
                Guid TechnicianKey = GetTechnicianKey(user.StaffKey);
                int TechnicalID = GetTechnicalID(TechnicianKey);
                // Guid TechnicianKey = _staffRepository.GetAll().Where(x => x.Id == user.Result.StaffKey).Select(x => x.TechnicianKey.Value).FirstOrDefault();
                // int TechnicalID = _technicianRepository.GetAll().Where(x => x.TechnicianKey == TechnicianKey).OrderBy(x => x.Name).Select(x => x.Id).FirstOrDefault();
                string message = "";
                try
                {

                    DataTable getWOKey = GetWorkOrderByID(Convert.ToInt32(woID));
                    string woKey = getWOKey.Rows[0]["MWorkOrderKey"].ToString();
                    List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus("Maintenance in the Room");
                    string maidStatusKey = lstmk[0].MaidStatusKey;
                    MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();
                    Room room = new Room();
                    if (!String.IsNullOrEmpty(getWOKey.Rows[0]["RoomKey"].ToString()))
                        room.Id = Guid.Parse(getWOKey.Rows[0]["RoomKey"].ToString());
                    if (!String.IsNullOrEmpty(getWOKey.Rows[0]["Room"].ToString()))
                        room.Unit = getWOKey.Rows[0]["Room"].ToString();


                    room.MaidStatusKey = new Guid(maidStatusKey);//5E1BE1E8-B25E-4429-BEFA-85E9A0C63A00

                    timeSheet.Hdr_Seqno = Convert.ToInt32(woID);

                    timeSheet.MTechnicianName = _mworkordertimesheetdalRepository.GetMTechnicianBySeqNo(TechnicalID);
                    timeSheet.MTechnician = TechnicalID;
                    if(!String.IsNullOrEmpty(woKey))
                    {
                        timeSheet.MWorkOrderKey = Guid.Parse(woKey);
                    }
                   
                    timeSheet.WDate = DateTime.Now.Date;

                    timeSheet.TimeFrom = DateTime.Now;

                    timeSheet.CreatedBy = StaffKey;

                    timeSheet.DetailLog = user.UserName + " added time sheet.";
                    timeSheet.NewLog = GetChangeLog(timeSheet);
                    if (AbpSession.TenantId != null)
                    {
                        timeSheet.TenantId = (int?)AbpSession.TenantId;
                    }
                    int intSuccessful = _mworkordertimesheetdalRepository.Inserttimesheet(timeSheet);

                    int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);
                    if (Successful > 0)
                    {
                        Successful = GetRoomStatusChangeHistory("mroom", room, timeSheet, user);
                    }

                    if (intSuccessful > 0)
                    {
                        intSuccessful = InsertWorkTimesheetHistory(timeSheet, user, "i");
                        message = "Success Records are saved";
                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to add the record.");
                    }

                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());

                }
                return message;
            }
        }
        [HttpPost]
        public async Task<string> EndTimeSheet(string woID)
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
                Guid TechnicianKey = GetTechnicianKey(user.StaffKey);
                int TechnicalID = GetTechnicalID(TechnicianKey);
                // Guid TechnicianKey = _staffRepository.GetAll().Where(x => x.Id == user.Result.StaffKey).Select(x => x.TechnicianKey.Value).FirstOrDefault();
                //int TechnicalID = _technicianRepository.GetAll().Where(x => x.TechnicianKey == TechnicianKey).OrderBy(x => x.Name).Select(x => x.Id).FirstOrDefault();
                string message = "";
                try
                {

                    DataTable getWOKey = GetWorkOrderByID(Convert.ToInt32(woID));
                    string woKey = getWOKey.Rows[0]["MWorkOrderKey"].ToString();
                    List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus(CommomData.HouseKeepingMaidStatusInspectionRequired);
                    string maidStatusKey = lstmk[0].MaidStatusKey;
                    MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();
                    Room room = new Room();
                    if (!String.IsNullOrEmpty(getWOKey.Rows[0]["RoomKey"].ToString()))
                        room.Id = Guid.Parse(getWOKey.Rows[0]["RoomKey"].ToString());
                    if (!String.IsNullOrEmpty(getWOKey.Rows[0]["Room"].ToString()))
                        room.Unit = getWOKey.Rows[0]["Room"].ToString();
                   
                    room.MaidStatusKey = new Guid(maidStatusKey);

                    timeSheet.Hdr_Seqno = Convert.ToInt32(woID);
                    DataTable dt = _mworkordertimesheetdalRepository.GetWorkTimeSheetByHdr_Seqno(timeSheet.Hdr_Seqno);
                    timeSheet.Seqno = Convert.ToInt32(dt.Rows[0]["Seqno"].ToString());
                    timeSheet.MTechnicianName = _mworkordertimesheetdalRepository.GetMTechnicianBySeqNo(TechnicalID);
                    timeSheet.MTechnician = TechnicalID;
                    if (!String.IsNullOrEmpty(woKey))
                    {
                        timeSheet.MWorkOrderKey = Guid.Parse(woKey);
                    }
                   
                    timeSheet.WDate = DateTime.Now.Date;

                    timeSheet.TimeTo = DateTime.Now;

                    timeSheet.ModifiedBy = user.StaffKey;//Guid.Parse(BLL_Staff.GetLoginUserStaffKey());

                    timeSheet.Notes = dt.Rows[0]["Notes"].ToString();
                    timeSheet.DetailLog = user.UserName + " updated time sheet.";//BLL_Staff.GetLoginUsername()
                    timeSheet.NewLog = GetUpdateChangeLog(dt, timeSheet);
                    timeSheet.OldLog = GetUpdateOldLog(dt, timeSheet);

                    int intSuccessful = _mworkordertimesheetdalRepository.UpdateByHdr_Seqno(timeSheet);
                    //BLL_Room.UpdateRoomMaidStatus("inspect", room, timeSheet);
                    int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);
                    if (Successful > 0)
                    {
                        Successful = GetRoomStatusChangeHistory("inspect", room, timeSheet, user);
                    }

                    if (intSuccessful > 0)
                    {
                        intSuccessful = InsertWorkTimesheetHistory(timeSheet, user, "u");
                        message = "Success Records are saved";

                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_", "ReleaseRoom('" + woID + "', '" + strProperty + "');", true);
                        //message = "Records are saved";
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
                return message;
            }
        }
        [HttpPost]
        public async Task<string> RelaceFromMr(string woID,string cleanStatus)
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

                Guid StaffKey = user.StaffKey;
                Guid TechnicianKey = GetTechnicianKey(user.StaffKey);
                int TechnicalID = GetTechnicalID(TechnicianKey);
                string message = "";
                try
                {

                    DataTable getWOKey = GetWorkOrderByID(Convert.ToInt32(woID));
                    string woKey = getWOKey.Rows[0]["MWorkOrderKey"].ToString();
                    List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus(cleanStatus == "Dirty" ? CommomData.HouseKeepingMaidStatusDirty : cleanStatus == "Clean" ? CommomData.HouseKeepingMaidStatusClean : CommomData.HouseKeepingMaidStatusInspectionRequired);
                    string maidStatusKey = lstmk[0].MaidStatusKey;
                    MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();
                    timeSheet.Hdr_Seqno = Convert.ToInt32(woID);
                    timeSheet.MTechnicianName = _mworkordertimesheetdalRepository.GetMTechnicianBySeqNo(TechnicalID);
                    Room room = new Room();
                    if (!String.IsNullOrEmpty(getWOKey.Rows[0]["RoomKey"].ToString()))
                        room.Id = Guid.Parse(getWOKey.Rows[0]["RoomKey"].ToString());
                    if (!String.IsNullOrEmpty(getWOKey.Rows[0]["Room"].ToString()))
                        room.Unit = getWOKey.Rows[0]["Room"].ToString();
                    room.MaidStatusKey = new Guid(maidStatusKey);
                    int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);
                    if (Successful > 0)
                    {
                        //Successful = GetRoomStatusChangeHistory("inspect", room, timeSheet, user);
                        Successful = GetRoomStatusChangeHistoryDependCleanStatus(room, user, cleanStatus);
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
                return message;
            }
        }
        [HttpPost]
        public async Task<string> EndTimeSheetPop(string woID,string cleanStatus)
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
                Guid TechnicianKey = GetTechnicianKey(user.StaffKey);
                int TechnicalID = GetTechnicalID(TechnicianKey);
                // Guid TechnicianKey = _staffRepository.GetAll().Where(x => x.Id == user.Result.StaffKey).Select(x => x.TechnicianKey.Value).FirstOrDefault();
                //int TechnicalID = _technicianRepository.GetAll().Where(x => x.TechnicianKey == TechnicianKey).OrderBy(x => x.Name).Select(x => x.Id).FirstOrDefault();
                string message = "";
                try
                {

                    DataTable getWOKey = GetWorkOrderByID(Convert.ToInt32(woID));
                    string woKey = getWOKey.Rows[0]["MWorkOrderKey"].ToString();
                    List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus(cleanStatus == "Dirty" ? CommomData.HouseKeepingMaidStatusDirty : cleanStatus == "Clean" ? CommomData.HouseKeepingMaidStatusClean : CommomData.HouseKeepingMaidStatusInspectionRequired);                    
                    string maidStatusKey = lstmk[0].MaidStatusKey;
                    MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();
                    Room room = new Room();
                    if (!String.IsNullOrEmpty(getWOKey.Rows[0]["RoomKey"].ToString()))
                        room.Id = Guid.Parse(getWOKey.Rows[0]["RoomKey"].ToString());
                    if (!String.IsNullOrEmpty(getWOKey.Rows[0]["Room"].ToString()))
                        room.Unit = getWOKey.Rows[0]["Room"].ToString();

                    room.MaidStatusKey = new Guid(maidStatusKey);

                    timeSheet.Hdr_Seqno = Convert.ToInt32(woID);
                    DataTable dt = _mworkordertimesheetdalRepository.GetWorkTimeSheetByHdr_Seqno(timeSheet.Hdr_Seqno);
                    timeSheet.Seqno = Convert.ToInt32(dt.Rows[0]["Seqno"].ToString());
                    timeSheet.MTechnicianName = _mworkordertimesheetdalRepository.GetMTechnicianBySeqNo(TechnicalID);
                    timeSheet.MTechnician = TechnicalID;
                    if (!String.IsNullOrEmpty(woKey))
                    {
                        timeSheet.MWorkOrderKey = Guid.Parse(woKey);
                    }

                    timeSheet.WDate = DateTime.Now.Date;

                    timeSheet.TimeTo = DateTime.Now;

                    timeSheet.ModifiedBy = user.StaffKey;//Guid.Parse(BLL_Staff.GetLoginUserStaffKey());

                    timeSheet.Notes = dt.Rows[0]["Notes"].ToString();
                    timeSheet.DetailLog = user.UserName + " updated time sheet.";//BLL_Staff.GetLoginUsername()
                    timeSheet.NewLog = GetUpdateChangeLog(dt, timeSheet);
                    timeSheet.OldLog = GetUpdateOldLog(dt, timeSheet);

                    int intSuccessful = _mworkordertimesheetdalRepository.UpdateByHdr_Seqno(timeSheet);
                    //BLL_Room.UpdateRoomMaidStatus("inspect", room, timeSheet);
                    int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);
                    if (Successful > 0)
                    {
                        //Successful = GetRoomStatusChangeHistory("inspect", room, timeSheet, user);
                        Successful = GetRoomStatusChangeHistoryDependCleanStatus(room, user, cleanStatus);
                    }

                    if (intSuccessful > 0)
                    {
                        intSuccessful = InsertWorkTimesheetHistory(timeSheet, user, "u");
                        if (intSuccessful>0)
                        {
                            DataTable blockroom = GetBlockRoomByWorkOrderIDDatatable(Convert.ToInt32(woID));
                            if (blockroom.Rows.Count > 0)
                            {
                                
                                    message = "Resease";
                                
                            }
                            else
                            {
                                message = "Success Records are saved";
                            }
                        }
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "close_", "ReleaseRoom('" + woID + "', '" + strProperty + "');", true);
                        //message = "Records are saved";
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
                return message;
            }
        }

        [HttpPost]
        public async Task<string> Release(string woID)
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
                Guid TechnicianKey = GetTechnicianKey(user.StaffKey);
                int TechnicalID = GetTechnicalID(TechnicianKey);
                //Guid TechnicianKey = _staffRepository.GetAll().Where(x => x.Id == user.Result.StaffKey).Select(x => x.TechnicianKey.Value).FirstOrDefault();
                //int TechnicalID = _technicianRepository.GetAll().Where(x => x.TechnicianKey == TechnicianKey).OrderBy(x => x.Name).Select(x => x.Id).FirstOrDefault();
                string message = "";
                try
                {
                    List<BlockRoom> listRoom = new List<BlockRoom>();
                    BlockRoom room;
                    int intStatus = 0;
                    DataTable blockroom = GetBlockRoomByWorkOrderIDDatatable(Convert.ToInt32(woID));
                    if (blockroom.Rows.Count > 0)
                    {
                        foreach (DataRow data in blockroom.Rows)
                        {
                            room = new BlockRoom();
                            room.Mworkorderno = Convert.ToInt32(woID);
                            DataTable getWOKey = GetWorkOrderByID(room.Mworkorderno);
                            if (!string.IsNullOrEmpty(getWOKey.Rows[0]["MWorkOrderKey"].ToString()) && getWOKey.Rows[0]["MWorkOrderKey"].ToString() != "")
                                room.MWorkOrderKey = Guid.Parse(getWOKey.Rows[0]["MWorkOrderKey"].ToString());
                            else
                                room.MWorkOrderKey = Guid.NewGuid();
                            room.RoomNo = data["Unit"].ToString();
                            room.Blockdate = Convert.ToDateTime(data["BlockDate"].ToString());
                            room.Roomblockkey = Guid.Parse(data["RoomBlockKey"].ToString());
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
                            if (Convert.ToInt32(data["Active"].ToString()) != intStatus)
                                room.NewLog = intStatus == 0 ? "Block status => Unblock" : "Block status => Block";
                            else
                                room.NewLog = "-";
                            room.OldLog = data["Active"].ToString() == "0" ? "Block status => Unblock" : "Block status => Block";
                            if (AbpSession.TenantId != null)
                            {
                                room.TenantId = (int?)AbpSession.TenantId;
                            }
                            listRoom.Add(room);
                        }
                        int intSuccess = _mworkordertimesheetdalRepository.UpdateStatus(listRoom);
                        if (intSuccess == 1)
                        {
                            foreach (BlockRoom b in listRoom)
                            {
                                intSuccess = InsertBlockRoomHistory(b, false, user.StaffKey);
                            }
                            //intSuccess = InsertBlockRoomHistory(listRoom[0], false, user.StaffKey);
                            message = "Success Records are saved";
                        }
                        else
                        {
                            message = "There's error during updating Room Block status. Please check it.";
                        }
                    }
                    else
                    {
                        message = "There's no block room";
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());

                }
                return message;
            }
        }

        [HttpPost]
        public async Task<string> StartTimeSheetWod(string woID)
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
                Guid TechnicianKey = GetTechnicianKey(user.StaffKey);
                int TechnicalID = GetTechnicalID(TechnicianKey);
                // Guid TechnicianKey = _staffRepository.GetAll().Where(x => x.Id == user.Result.StaffKey).Select(x => x.TechnicianKey.Value).FirstOrDefault();
                // int TechnicalID = _technicianRepository.GetAll().Where(x => x.TechnicianKey == TechnicianKey).OrderBy(x => x.Name).Select(x => x.Id).FirstOrDefault();
                string message = "";
                try
                {

                    DataTable getWOKey = GetWorkOrderByID(Convert.ToInt32(woID));
                    string woKey = getWOKey.Rows[0]["MWorkOrderKey"].ToString();
                    List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus("Dirty");
                    string maidStatusKey = lstmk[0].MaidStatusKey;
                    MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();
                    Room room = new Room();
                    if (!String.IsNullOrEmpty(getWOKey.Rows[0]["RoomKey"].ToString()))
                        room.Id = Guid.Parse(getWOKey.Rows[0]["RoomKey"].ToString());
                    if (!String.IsNullOrEmpty(getWOKey.Rows[0]["Room"].ToString()))
                        room.Unit = getWOKey.Rows[0]["Room"].ToString();
                    room.MaidStatusKey = new Guid(maidStatusKey);//5E1BE1E8-B25E-4429-BEFA-85E9A0C63A00

                    timeSheet.Hdr_Seqno = Convert.ToInt32(woID);

                    timeSheet.MTechnicianName = _mworkordertimesheetdalRepository.GetMTechnicianBySeqNo(TechnicalID);
                    timeSheet.MTechnician = TechnicalID;
                    if (!String.IsNullOrEmpty(woKey))
                    {
                        timeSheet.MWorkOrderKey = Guid.Parse(woKey);
                    }
                    timeSheet.WDate = DateTime.Now.Date;

                    timeSheet.TimeFrom = DateTime.Now;

                    timeSheet.CreatedBy = StaffKey;

                    timeSheet.DetailLog = user.UserName + " added time sheet.";
                    timeSheet.NewLog = GetChangeLog(timeSheet);
                    if (AbpSession.TenantId != null)
                    {
                        timeSheet.TenantId = (int?)AbpSession.TenantId;
                    }
                    int intSuccessful = _mworkordertimesheetdalRepository.Inserttimesheet(timeSheet);

                    int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);
                    if (Successful > 0)
                    {
                        Successful = GetRoomStatusChangeHistory("mroom", room, timeSheet, user);
                    }

                    if (intSuccessful > 0)
                    {
                        intSuccessful = InsertWorkTimesheetHistory(timeSheet, user, "i");
                        message = "Success Records are saved";
                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to add the record.");
                    }

                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());

                }
                return message;
            }
        }
        #endregion

        [HttpGet]
        public async Task<ListResultDto<PopupTimeSheetViewData>> GetPopupTimeSheetViewData(string mode, string seqno, string woid, string technician)
        {
            List<PopupTimeSheetViewData> Alllst = new List<PopupTimeSheetViewData>();
            if (!string.IsNullOrEmpty(mode))
            {
                PopupTimeSheetViewData a = new PopupTimeSheetViewData();
                List<DDLTechnicianOutput> ddlt = new List<DDLTechnicianOutput>();
                DDLTechnicianOutput t = new DDLTechnicianOutput();
                t.Seqno = "-1";
                t.Name = "-- Select technician --";
                ddlt.Add(t);
                //var ddlt2 = _technicianRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Name)
                //  .Select(x => new DDLTechnicianOutput
                //  {
                //      Seqno = x.Id.ToString(),
                //      Name = x.Name
                //  });
                List<DDLTechnicianOutput> ddlt2 = GetAllTechnician();

                a.DDLTechnician = ddlt.Concat(ddlt2).ToList();

                List<DDLNoteTemplateOutput> ddlnt = new List<DDLNoteTemplateOutput>();
                DDLNoteTemplateOutput nt = new DDLNoteTemplateOutput();
                nt.Seqno = "0";
                nt.Description = "--Select to use template note--";
                ddlnt.Add(nt);
                //var ddlnt2 = _mWorktimesheetnotetemplateRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Description)
                //  .Select(x => new DDLNoteTemplateOutput
                //  {
                //      Seqno = x.Id.ToString(),
                //      Description = x.Description
                //  });
                List<DDLNoteTemplateOutput> ddlnt2 = GetAllNoteTemplate();

                a.DDLNoteTemplate = ddlnt.Concat(ddlnt2).ToList();


                if (mode.Equals("i"))
                {
                    a.Title = "Add Time Sheet ";
                    a.WOID = woid;
                    a.btnUpdateVisible = false;
                    a.btnDeleteVisible = false;

                    if (technician != null && technician == "select")
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
                            Guid TechnicianKey = GetTechnicianKey(user.StaffKey);
                            int TechnicalID = GetTechnicalID(TechnicianKey);
                            //Guid TechnicianKey = _staffRepository.GetAll().Where(x => x.Id == user.Result.StaffKey).Select(x => x.TechnicianKey.Value).FirstOrDefault();
                            //int TechnicalID = _technicianRepository.GetAll().Where(x => x.TechnicianKey == TechnicianKey).OrderBy(x => x.Name).Select(x => x.Id).FirstOrDefault();
                            a.ddlTechnicianSelectedValue = TechnicalID.ToString();
                        }
                    }
                }
                else if (mode.Equals("u"))
                {
                    a.Title = "Update Time Sheet ";
                    a.btnAddVisible = false;
                    a.btnDeleteVisible = false;

                    DataTable dt = GetWorkTimeSheetByID(seqno);
                    if (dt.Rows.Count > 0)
                    {
                        a.Seqno = seqno;
                        a.WOID = dt.Rows[0]["Hdr_Seqno"].ToString();
                        if (!string.IsNullOrEmpty(dt.Rows[0]["MTechnician"].ToString()))
                            a.ddlTechnicianSelectedValue = dt.Rows[0]["MTechnician"].ToString();

                        if (!string.IsNullOrEmpty(dt.Rows[0]["WDate"].ToString()))
                            a.WrokDate = Convert.ToDateTime(dt.Rows[0]["WDate"].ToString());

                        if (!string.IsNullOrEmpty(dt.Rows[0]["TimeFrom"].ToString()))
                            a.radStartTime = Convert.ToDateTime(dt.Rows[0]["TimeFrom"].ToString()).TimeOfDay;

                        if (!string.IsNullOrEmpty(dt.Rows[0]["TimeTo"].ToString()))
                            a.radEndTime = Convert.ToDateTime(dt.Rows[0]["TimeTo"].ToString()).TimeOfDay;

                        a.Note = dt.Rows[0]["Notes"].ToString();
                    }
                }
                else if (mode.Equals("d"))
                {
                    a.WOID = woid;
                    a.Title = "Delete Time Sheet ";
                    a.Seqno = seqno;
                    a.btnAddVisible = false;
                    a.btnUpdateVisible = false;

                }

                Alllst.Add(a);
            }

            return new ListResultDto<PopupTimeSheetViewData>(Alllst);
        }

        [HttpPost]
        public async Task<string> btnTimeSheetUpdate(PopupTimeSheetInput input)
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
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                try
                {

                    MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();


                    timeSheet.Seqno = Convert.ToInt32(input.Seqno);
                    DataTable dt = GetWorkTimeSheetByID(input.Seqno);
                    timeSheet.Hdr_Seqno = Convert.ToInt32(input.WOID);
                    DataTable getWOKey = GetWorkOrderByID(timeSheet.Hdr_Seqno);
                    if (Convert.ToInt32(input.ddlTechnicianValue) >-1)//>= 0)
                    {
                        timeSheet.MTechnicianName = input.ddlTechnicianText;
                        timeSheet.MTechnician = Convert.ToInt32(input.ddlTechnicianValue);
                    }

                    if (!string.IsNullOrEmpty(getWOKey.Rows[0]["MWorkOrderKey"].ToString()) && getWOKey.Rows[0]["MWorkOrderKey"].ToString() != "")
                    {
                        timeSheet.MWorkOrderKey = Guid.Parse(getWOKey.Rows[0]["MWorkOrderKey"].ToString());
                    }
                    else
                    {
                        timeSheet.MWorkOrderKey = Guid.NewGuid();
                    }
                    timeSheet.WDate = input.WrokDate;

                    if (input.radStartTime != null)
                        timeSheet.TimeFrom = Convert.ToDateTime(Convert.ToDateTime(timeSheet.WDate).ToString("dd-MMM-yyyy") + " " + input.radStartTime.ToString());

                    if (input.radEndTime != null)
                        timeSheet.TimeTo = Convert.ToDateTime(Convert.ToDateTime(timeSheet.WDate).ToString("dd-MMM-yyyy") + " " + input.radEndTime.ToString());

                    timeSheet.Notes = input.Note;//.Trim();
                    timeSheet.ModifiedBy = user.StaffKey;

                    timeSheet.DetailLog = user.UserName + " updated time sheet.";
                    timeSheet.NewLog = GetUpdateChangeLog(dt, timeSheet);
                    timeSheet.OldLog = GetUpdateOldLog(dt, timeSheet);


                    int intSuccessful = _mworkordertimesheetdalRepository.UpdateTimesheet(timeSheet);

                    if (intSuccessful > 0)
                    {
                        intSuccessful = InsertWorkTimesheetHistory(timeSheet, user, "u");

                        message = "Record(s) has been saved";
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
            return message;
        }
        private DataTable GetWorkTimeSheetByID(string seqno)
        {
            return _mworkordertimesheetdalRepository.GetWorkTimeSheetByID(Convert.ToInt32(seqno));
        }

        private DataTable GetBlockRoomByWorkOrderIDDatatable(int v)
        {
            return _mworkordertimesheetdalRepository.GetBlockRoomByWorkOrderIDDatatable(v);
        }

        private int InsertBlockRoomHistory(BlockRoom blockRoom, bool blnInsert, Guid staffKey)
        {
            int success = 0;
            try
            {
                List<History> listHistory = new List<History>();
                History history = new History();
                history.Id = Guid.NewGuid();
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

                success = _mworkordertimesheetdalRepository.InsertHistoryList(listHistory);

            }
            catch (Exception ex)
            {
                // LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return success;

        }
        protected List<GetMaidStatusOutput> GetMaidStatusKeyByStatus(string status)
        {
            return _mworkordertimesheetdalRepository.GetMaidStatusKeyByStatus(status);
        }
        private string GetUpdateOldLog(DataTable dt, MWorkTimeSheetInput timesheet)
        {
            try
            {
                string strLog = "";
                StringBuilder sb = new StringBuilder();

                if (!Convert.ToDateTime(dt.Rows[0]["WDate"]).ToString("dd/MM/yyyy").Equals(Convert.ToDateTime(timesheet.WDate).ToString("dd/MM/yyyy")))
                {
                    sb.Append(" WorkDate => " + Convert.ToDateTime(dt.Rows[0]["WDate"]).ToString("dd/MM/yyyy"));
                }

                if (!dt.Rows[0]["MTechnician"].ToString().Equals(timesheet.MTechnician.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");

                    if (string.IsNullOrEmpty(dt.Rows[0]["MTechnician"].ToString()))
                        sb.Append("Technician => -");
                    else
                        sb.Append(", Technician => " + _mworkordertimesheetdalRepository.GetMTechnicianBySeqNo(Convert.ToInt32(dt.Rows[0]["MTechnician"].ToString())));
                }
                if (timesheet.TimeFrom != null)
                {
                    if (!CommomData.GetTimeToDisplay(dt.Rows[0]["TimeFrom"]).Equals(CommomData.GetTimeToDisplay(timesheet.TimeFrom)))
                    {
                        if (!string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(", ");
                        sb.Append("Start@ => " + CommomData.GetTimeToDisplay(dt.Rows[0]["TimeFrom"]));
                    }
                }
                if (!CommomData.GetTimeToDisplay(dt.Rows[0]["TimeTo"]).Equals(CommomData.GetTimeToDisplay(timesheet.TimeTo)))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("End@ => " + CommomData.GetTimeToDisplay(dt.Rows[0]["TimeTo"]));
                }
                if (!dt.Rows[0]["Notes"].ToString().Equals(timesheet.Notes))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("Notes => " + dt.Rows[0]["Notes"].ToString());
                }
                strLog = " Log; " + sb.ToString();

                return strLog;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static string GetUpdateChangeLog(DataTable dt, MWorkTimeSheetInput timesheet)
        {
            try
            {
                string strLog = "";
                StringBuilder sb = new StringBuilder();

                if (!Convert.ToDateTime(dt.Rows[0]["WDate"]).ToString("dd/MM/yyyy").Equals(Convert.ToDateTime(timesheet.WDate).ToString("dd/MM/yyyy")))
                {
                    sb.Append(" WorkDate to=> " + Convert.ToDateTime(timesheet.WDate).ToString("dd/MM/yyyy"));
                }


                if (!dt.Rows[0]["MTechnician"].ToString().Equals(timesheet.MTechnician.ToString()))
                {
                    sb.Append(", Technician to=> " + timesheet.MTechnicianName);
                }
                if (timesheet.TimeFrom != null)
                {
                    if (!CommomData.GetTimeToDisplay(dt.Rows[0]["TimeFrom"]).Equals(CommomData.GetTimeToDisplay(timesheet.TimeFrom)))
                    {
                        if (!string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(", ");
                        sb.Append("Start@ to=> " + CommomData.GetTimeToDisplay(timesheet.TimeFrom));
                    }
                }
                if (!CommomData.GetTimeToDisplay(dt.Rows[0]["TimeTo"]).Equals(CommomData.GetTimeToDisplay(timesheet.TimeTo)))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("End@=> " + CommomData.GetTimeToDisplay(timesheet.TimeTo));
                }
                if (!dt.Rows[0]["Notes"].ToString().Equals(timesheet.Notes))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("Notes=> " + timesheet.Notes);
                }
                strLog = " Change Log; " + sb.ToString();

                return strLog;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static string GetChangeLog(MWorkTimeSheetInput timesheet, bool blnNew = true)
        {
            try
            {
                string strLog = "";
                StringBuilder sb = new StringBuilder();

                sb.Append(" WorkDate=> " + Convert.ToDateTime(timesheet.WDate).ToString("dd/MM/yyyy"));

                if (timesheet.MTechnician != null && timesheet.MTechnician != -1)
                {
                    sb.Append(", Technician=> " + timesheet.MTechnicianName);
                }

                if (timesheet.TimeFrom != null)
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("Start@=> " + CommomData.GetTimeToDisplay(timesheet.TimeFrom));
                }
                if (timesheet.TimeTo != null)
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("End@=> " + CommomData.GetTimeToDisplay(timesheet.TimeTo));
                }
                if (!string.IsNullOrEmpty(timesheet.Notes))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append("Notes=> " + timesheet.Notes);
                }
                strLog = "Log; " + sb.ToString();

                return strLog;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int InsertWorkTimesheetHistory(MWorkTimeSheetInput timeSheet, Authorization.Users.User user, string action)
        {
            int succ = 0;
            try
            {
                List<History> listHistory = new List<History>();
                History history;
                history = new History();
                history.Id = Guid.NewGuid();
                history.SourceKey = timeSheet.MWorkOrderKey;
                history.StaffKey = user.StaffKey;
                if (action == "i")
                {
                    history.Operation = "I";
                    history.NewValue = timeSheet.NewLog;
                }
                else if (action == "u")
                {
                    history.Operation = "U";
                    history.NewValue = timeSheet.NewLog;
                    history.OldValue = timeSheet.OldLog;
                }
                else if (action == "d")
                {
                    history.Operation = "D";
                    history.NewValue = timeSheet.NewLog;
                    history.OldValue = timeSheet.OldLog;
                }
                history.TableName = "WTS";
                history.ModuleName = "iRepair";
                history.Detail = "(iRepair) WO#" + timeSheet.Hdr_Seqno + "; " + timeSheet.DetailLog;
                if (AbpSession.TenantId != null)
                {
                    history.TenantId = (int?)AbpSession.TenantId;
                }
                listHistory.Add(history);

                succ = _mworkordertimesheetdalRepository.InsertHistoryList(listHistory);
            }
            catch (Exception ex)
            {
                //  LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return succ;
        }
        private int GetRoomStatusChangeHistory(string mode, Room room, MWorkTimeSheetInput timeSheet, Authorization.Users.User user)
        {
            int succ = 0;
            List<History> listHistory = new List<History>();
            History history = new History();
            try
            {
                history.Id = Guid.NewGuid();
                history.StaffKey = user.StaffKey;
                history.SourceKey = room.Id;
                history.Operation = "U";
                history.TableName = "WO";
                history.ModuleName = "iRepair";
                history.Detail = GetRoomStatusDetail(mode, room, timeSheet);
                if (AbpSession.TenantId != null)
                {
                    history.TenantId = (int?)AbpSession.TenantId;
                }
                listHistory.Add(history);

                succ = _mworkordertimesheetdalRepository.InsertHistoryList(listHistory);
            }
            catch (Exception ex)
            {
                //  LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return succ;
        }
        private int GetRoomStatusChangeHistoryDependCleanStatus(Room room, Authorization.Users.User user,string cleanstatus)
        {
            int succ = 0;
            List<History> listHistory = new List<History>();
            History history = new History();
            try
            {
                history.Id = Guid.NewGuid();
                history.StaffKey = user.StaffKey;
                history.SourceKey = room.Id;
                history.Operation = "U";
                history.TableName = "WO";
                history.ModuleName = "iRepair";
                history.Detail = "(iRepair) " + user.UserName + " updated Room#" + room.Unit + " as " + cleanstatus;
                if (AbpSession.TenantId != null)
                {
                    history.TenantId = (int?)AbpSession.TenantId;
                }
                listHistory.Add(history);

                succ = _mworkordertimesheetdalRepository.InsertHistoryList(listHistory);
            }
            catch (Exception ex)
            {
                //  LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return succ;
        }
        private static string GetRoomStatusDetail(string mode, Room room, MWorkTimeSheetInput timeSheet)
        {
            string strReturnValue = "";
            try
            {
                if (mode.Equals("mroom"))
                {
                    strReturnValue = "(iRepair) " + timeSheet.MTechnicianName + " has updated Work Order #" + timeSheet.Hdr_Seqno + " as Maintenance in the Room";
                }
                else if (mode.Equals("mainreq") || mode.Equals("noreq"))
                {
                    strReturnValue = "(iRepair) " + timeSheet.MTechnicianName + " has updated Work Order #" + timeSheet.Hdr_Seqno + " as " + (mode.Equals("mainreq") ? " Maintenance Required " : room.Status);
                }
                else if (mode.Equals("inspect")) // enable/disable DND
                {
                    strReturnValue = "(iRepair) " + timeSheet.MTechnicianName + " has updated Work Order #" + timeSheet.Hdr_Seqno + " as Inspection Required";
                }
            }
            catch
            {
                throw;
            }
            return strReturnValue;
        }

        [HttpPost]
        public async Task<List<MessageNotiViewLatest>> btnWoInfoSave(WorkOrderInfoInput input)
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            string message = "";
            string mode = input.mode;
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
                    bool blnAssignTechnician = false;
                    bool blnUpdateStatus = false;

                    int intWOID = Convert.ToInt32(input.seqno);
                    //Staff staff = BLL_Staff.GetStaffInfoByStaffKey(ddlStaff.SelectedValue);

                    DataTable dt = GetWorkOrderByID(intWOID);

                    MWorkOrderInput work = new MWorkOrderInput();
                    work.Id = intWOID;
                    if (!string.IsNullOrEmpty(input.MWorkOrderKey))
                        work.MWorkOrderKey = Guid.Parse(input.MWorkOrderKey);
                    else
                        work.MWorkOrderKey = Guid.NewGuid();
                    work.StaffName = input.ReportedByName;//staff.UserName;
                    work.ReportedBy = Guid.Parse(input.ReportedByKey);
                    work.ReportedOn = input.ReportedDate;
                    if (input.RoomKey != Guid.Empty.ToString())
                    {
                        work.Room = input.Unit;
                        work.RoomKey = Guid.Parse(input.RoomKey);
                    }
                    if (Convert.ToInt32(input.MArea) > -1)
                    {
                        work.MAreaDesc = input.MAreaDes;
                        work.MArea = Convert.ToInt32(input.MArea);
                    }
                    if (Convert.ToInt32(input.MWorkType) > -1)
                    {
                        work.MWorkTypeDesc = input.MWorkTypeDes;
                        work.MWorkType = Convert.ToInt32(input.MWorkType);
                    }
                    if (Convert.ToInt32(input.Priority) != 99)
                    {
                        work.PriorityDesc = input.PriorityDesc;
                        work.Priority = Convert.ToInt32(input.Priority);
                    }
                    if (input.RoomKey != Guid.Empty.ToString())
                    {
                        if (input.mode != null)
                        {
                            Room room = new Room();
                            if (mode.Equals("mainreq"))
                            {

                                room.Id = Guid.Parse(input.RoomKey);
                                List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus("Maintenance Required");
                                string maidStatusKey = lstmk[0].MaidStatusKey;
                                room.MaidStatusKey = new Guid(maidStatusKey);
                                MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();
                                timeSheet.Hdr_Seqno = intWOID;
                                if (Convert.ToInt32(input.Technician) > -1)
                                {
                                    timeSheet.MTechnicianName = input.TechnicianName;
                                    timeSheet.MTechnician = Convert.ToInt32(input.Technician);
                                }
                                // BLL_Room.UpdateRoomMaidStatus("mainreq", room, timeSheet);
                                int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);
                                if (Successful > 0)
                                {
                                    Successful = GetRoomStatusChangeHistory("mainreq", room, timeSheet, user);
                                }
                            }
                            else
                            {
                                room.Id = Guid.Parse(input.RoomKey);
                                room.Unit = input.Unit;
                                string status = GetMaidStatusByRoomKey(room.Id);
                                List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus(status);
                                string maidStatusKey = lstmk[0].MaidStatusKey;
                                room.Status = status;
                                room.MaidStatusKey = new Guid(maidStatusKey);

                                MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();
                                timeSheet.Hdr_Seqno = intWOID;
                                if (Convert.ToInt32(input.Technician) > -1)
                                {
                                    timeSheet.MTechnicianName = input.TechnicianName;
                                    timeSheet.MTechnician = Convert.ToInt32(input.Technician);
                                }
                                //BLL_Room.UpdateRoomMaidStatus("noreq", room, timeSheet);
                                int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);
                                if (Successful > 0)
                                {
                                    Successful = GetRoomStatusChangeHistory("noreq", room, timeSheet, user);
                                }

                            }
                        }
                    }
                    work.Description = input.WorkDescription;
                    work.Notes = input.Notes;

                    work.MWorkOrderStatus = Convert.ToInt32(input.WorkOrderStatus);
                    work.MWorkOrderStatusDesc = input.WorkOrderStatusDesc;
                    work.LastUpdateBy = user.UserName;
                    work.LastUpdateStaffKey = user.StaffKey;

                    if (work.MWorkOrderStatus == 3)
                    {
                        work.CompletedBy = user.UserName;
                        work.CompletedStaffKey = user.StaffKey;
                        work.CompletedDateTime = DateTime.Now;
                    }
                    else if (work.MWorkOrderStatus == 4)
                    {
                        work.SignedOffBy = user.UserName;
                        work.SignedOffStaffKey = user.StaffKey;
                        work.SignedOffDateTime = DateTime.Now;
                    }
                    else if (work.MWorkOrderStatus == 5)
                    {
                        work.CancelledBy = user.UserName;
                        work.CancelledStaffKey = user.StaffKey;
                        work.CancelledDateTime = DateTime.Now;
                    }

                    if (Convert.ToInt32(input.Technician) > -1)
                    {
                        work.MTechnicianName = input.TechnicianName;
                        work.MTechnician = Convert.ToInt32(input.Technician);
                    }

                    if (input.ScheduledStart != null)
                        work.ScheduledFrom = input.ScheduledStart;
                    if (input.ScheduledEnd != null)
                        work.ScheduledTo = input.ScheduledEnd;

                    work.DetailLog = user.UserName + " updated WO.";
                    work.NewLog = GetNewChangeLog(dt, work);
                    work.OldLog = GetChangeLogWoInfo(dt, work);
                    if (AbpSession.TenantId != null)
                    {
                        work.TenantId = (int?)AbpSession.TenantId;
                    }
                    blnAssignTechnician = IsAssignToTechnician(dt, work);
                    blnUpdateStatus = IsUpdateWorkOrderStatus(dt, work);

                    int intSuccess = UpdatWorkOrder(work);
                    if (intSuccess > 0)
                    {
                        intSuccess = InsertWorkOrderLog(work, false, user);
                        if (intSuccess > 0)
                        {
                            // Send msg to sqoope users if any
                            List<MWorkOrderInput> listWork = new List<MWorkOrderInput>();
                            listWork.Add(work);
                            if (blnAssignTechnician)
                            {
                                // SqoopeMessgingHelper.SendiRepairMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iRepair_AssignWorkOrder, listWork, null, BLL_Staff.GetLoginUserStaffKey());
                                Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_AssignWorkOrder, listWork, null, user.StaffKey.ToString());
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
                            if (blnUpdateStatus)
                            {
                                // SqoopeMessgingHelper.SendiRepairMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iRepair_UpdateWorkOrderStatus, listWork, null, BLL_Staff.GetLoginUserStaffKey());
                                #region wo completed=> maintenance required  to Inspection required
                                if (work.MWorkOrderStatusDesc == "Completed")
                                {
                                    DataTable dt1 = GetWorkOrderByID(intWOID);
                                    Room room = new Room();
                                    if (!String.IsNullOrEmpty(dt1.Rows[0]["RoomKey"].ToString()))
                                    {
                                        room.Id = Guid.Parse(dt1.Rows[0]["RoomKey"].ToString());
                                        string status = "Maintenance Required";
                                        List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus(status);
                                        string maidStatusKey = lstmk[0].MaidStatusKey;
                                        room.MaidStatusKey = new Guid(maidStatusKey);
                                        int exitcount = _mworkordertimesheetdalRepository.MRCheckExit(room.Id, room.MaidStatusKey);
                                        if (exitcount > 0)
                                        {
                                            List<GetMaidStatusOutput> lstmk1 = GetMaidStatusKeyByStatus(CommomData.HouseKeepingMaidStatusInspectionRequired);
                                            string maidStatusKey1 = lstmk1[0].MaidStatusKey;
                                            if (!String.IsNullOrEmpty(dt1.Rows[0]["Room"].ToString()))
                                                room.Unit = dt1.Rows[0]["Room"].ToString();
                                            room.MaidStatusKey = new Guid(maidStatusKey1);
                                            int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);

                                        }

                                    }

                                }
                                #endregion
                                Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_UpdateWorkOrderStatus, listWork, null, user.StaffKey.ToString());
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



                            message = "Record has been saved.";
                        }


                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to save the record.");
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return Alllstlatest;
        }
        [HttpPost]
        public async Task<List<MessageNotiViewLatest>> btnWoInfoSaveImg(WorkOrderInfoImgInput input)
        {
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            MessageNotiView a = new MessageNotiView();
            List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
            string message = "";
            string mode = input.mode;
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
                    bool blnAssignTechnician = false;
                    bool blnUpdateStatus = false;

                    int intWOID = Convert.ToInt32(input.seqno);
                    //Staff staff = BLL_Staff.GetStaffInfoByStaffKey(ddlStaff.SelectedValue);

                    DataTable dt = GetWorkOrderByID(intWOID);

                    MWorkOrderInput work = new MWorkOrderInput();
                    work.Id = intWOID;
                    if (!string.IsNullOrEmpty(input.MWorkOrderKey))
                        work.MWorkOrderKey = Guid.Parse(input.MWorkOrderKey);
                    else
                        work.MWorkOrderKey = Guid.NewGuid();
                    work.StaffName = input.ReportedByName;//staff.UserName;
                    work.ReportedBy = Guid.Parse(input.ReportedByKey);
                    work.ReportedOn = input.ReportedDate;
                    if (input.RoomKey != Guid.Empty.ToString())
                    {
                        work.Room = input.Unit;
                        work.RoomKey = Guid.Parse(input.RoomKey);
                    }
                    if (Convert.ToInt32(input.MArea) > -1)
                    {
                        work.MAreaDesc = input.MAreaDes;
                        work.MArea = Convert.ToInt32(input.MArea);
                    }
                    if (Convert.ToInt32(input.MWorkType) > -1)
                    {
                        work.MWorkTypeDesc = input.MWorkTypeDes;
                        work.MWorkType = Convert.ToInt32(input.MWorkType);
                    }
                    if (Convert.ToInt32(input.Priority) != 99)
                    {
                        work.PriorityDesc = input.PriorityDesc;
                        work.Priority = Convert.ToInt32(input.Priority);
                    }
                    if (input.RoomKey != Guid.Empty.ToString())
                    {
                        if (input.mode != null)
                        {
                            Room room = new Room();
                            if (mode.Equals("mainreq"))
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkOrderStatus"].ToString()))
                                {
                                    if (dt.Rows[0]["MWorkOrderStatus"].ToString() == "3")
                                    {
                                        room.Id = Guid.Parse(input.RoomKey);
                                        room.Unit = input.Unit;
                                        string status = GetMaidStatusByRoomKey(room.Id);
                                        List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus(status);
                                        string maidStatusKey = lstmk[0].MaidStatusKey;
                                        room.Status = status;
                                        room.MaidStatusKey = new Guid(maidStatusKey);

                                        MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();
                                        timeSheet.Hdr_Seqno = intWOID;
                                        if (Convert.ToInt32(input.Technician) > -1)
                                        {
                                            timeSheet.MTechnicianName = input.TechnicianName;
                                            timeSheet.MTechnician = Convert.ToInt32(input.Technician);
                                        }
                                        //BLL_Room.UpdateRoomMaidStatus("noreq", room, timeSheet);
                                        int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);
                                        if (Successful > 0)
                                        {
                                            Successful = GetRoomStatusChangeHistory("mainreq", room, timeSheet, user);
                                        }
                                    }
                                }
                                else
                                {
                                    room.Id = Guid.Parse(input.RoomKey);
                                    List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus("Maintenance Required");
                                    string maidStatusKey = lstmk[0].MaidStatusKey;
                                    room.MaidStatusKey = new Guid(maidStatusKey);
                                    MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();
                                    timeSheet.Hdr_Seqno = intWOID;
                                    if (Convert.ToInt32(input.Technician) > -1)
                                    {
                                        timeSheet.MTechnicianName = input.TechnicianName;
                                        timeSheet.MTechnician = Convert.ToInt32(input.Technician);
                                    }
                                    // BLL_Room.UpdateRoomMaidStatus("mainreq", room, timeSheet);
                                    int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);
                                    if (Successful > 0)
                                    {
                                        Successful = GetRoomStatusChangeHistory("mainreq", room, timeSheet, user);
                                    }
                                }                                    
                            }
                            else
                            {
                                room.Id = Guid.Parse(input.RoomKey);
                                room.Unit = input.Unit;
                                string status = GetMaidStatusByRoomKey(room.Id);
                                List<GetMaidStatusOutput> lstmk = GetMaidStatusKeyByStatus(status);
                                string maidStatusKey = lstmk[0].MaidStatusKey;
                                room.Status = status;
                                room.MaidStatusKey = new Guid(maidStatusKey);

                                MWorkTimeSheetInput timeSheet = new MWorkTimeSheetInput();
                                timeSheet.Hdr_Seqno = intWOID;
                                if (Convert.ToInt32(input.Technician) > -1)
                                {
                                    timeSheet.MTechnicianName = input.TechnicianName;
                                    timeSheet.MTechnician = Convert.ToInt32(input.Technician);
                                }
                                //BLL_Room.UpdateRoomMaidStatus("noreq", room, timeSheet);
                                int Successful = _mworkordertimesheetdalRepository.UpdateRoomMaidStatus(room);
                                if (Successful > 0)
                                {
                                    Successful = GetRoomStatusChangeHistory("noreq", room, timeSheet, user);
                                }
                            }
                        }
                       
                    }
                    work.Description = input.WorkDescription;
                    work.Notes = input.Notes;

                    work.MWorkOrderStatus = Convert.ToInt32(input.WorkOrderStatus);
                    work.MWorkOrderStatusDesc = input.WorkOrderStatusDesc;
                    work.LastUpdateBy = user.UserName;
                    work.LastUpdateStaffKey = user.StaffKey;

                    if (work.MWorkOrderStatus == 3)
                    {
                        work.CompletedBy = user.UserName;
                        work.CompletedStaffKey = user.StaffKey;
                        work.CompletedDateTime = DateTime.Now;
                    }
                    else if (work.MWorkOrderStatus == 4)
                    {
                        work.SignedOffBy = user.UserName;
                        work.SignedOffStaffKey = user.StaffKey;
                        work.SignedOffDateTime = DateTime.Now;
                    }
                    else if (work.MWorkOrderStatus == 5)
                    {
                        work.CancelledBy = user.UserName;
                        work.CancelledStaffKey = user.StaffKey;
                        work.CancelledDateTime = DateTime.Now;
                    }

                    if (Convert.ToInt32(input.Technician) > -1)
                    {
                        work.MTechnicianName = input.TechnicianName;
                        work.MTechnician = Convert.ToInt32(input.Technician);
                    }

                    if (input.ScheduledStart != null)
                        work.ScheduledFrom = input.ScheduledStart;
                    if (input.ScheduledEnd != null)
                        work.ScheduledTo = input.ScheduledEnd;

                    work.DetailLog = user.UserName + " updated WO.";
                    work.NewLog = GetNewChangeLog(dt, work);
                    work.OldLog = GetChangeLogWoInfo(dt, work);
                    if (AbpSession.TenantId != null)
                    {
                        work.TenantId = (int?)AbpSession.TenantId;
                    }
                    blnAssignTechnician = IsAssignToTechnician(dt, work);
                    blnUpdateStatus = IsUpdateWorkOrderStatus(dt, work);

                    int intSuccess = UpdatWorkOrder(work);
                    if (intSuccess > 0)
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
                                image.MWorkOrderKey = work.MWorkOrderKey;
                                image.Signature = dr.Data;
                                int exitcount = _mworkordertimesheetdalRepository.CheckWoImage(image);
                                if (exitcount == 0)
                                {
                                    s = _mworkordertimesheetdalRepository.InsertWoImage(image);
                                }
                                else
                                {
                                    s = _mworkordertimesheetdalRepository.UpdateWoImage(image);
                                }


                            }
                            #endregion
                            if (s > 0)
                            {
                                message = "Record has been updated.";
                            }
                            else
                            {
                                throw new UserFriendlyException("Fail to add Image.");
                            }
                        }
                        intSuccess = InsertWorkOrderLog(work, false, user);
                        if (intSuccess > 0)
                        {
                            // Send msg to sqoope users if any
                            List<MWorkOrderInput> listWork = new List<MWorkOrderInput>();
                            listWork.Add(work);
                            if (blnAssignTechnician)
                            {
                                // SqoopeMessgingHelper.SendiRepairMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iRepair_AssignWorkOrder, listWork, null, BLL_Staff.GetLoginUserStaffKey());
                                Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_AssignWorkOrder, listWork, null, user.StaffKey.ToString());
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
                            if (blnUpdateStatus)
                            {
                                // SqoopeMessgingHelper.SendiRepairMessageToSqoope(strProperty, SqoopeMessgingHelper.MsgType_iRepair_UpdateWorkOrderStatus, listWork, null, BLL_Staff.GetLoginUserStaffKey());

                                Alllst = await _sqoopeint.SendiRepairMessageToSqoope(CommomData.MsgType_iRepair_UpdateWorkOrderStatus, listWork, null, user.StaffKey.ToString());
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



                            message = "Record has been saved.";
                        }



                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to save the record.");
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return Alllstlatest;
        }
        private string GetMaidStatusByRoomKey(Guid key)
        {
            return _mworkordertimesheetdalRepository.GetMaidStatusByRoomKey(key);
        }

        private int InsertWorkOrderLog(MWorkOrderInput work, bool blnInsert, Authorization.Users.User user)
        {
            int success = 0;
            List<History> listHistory = new List<History>();
            History history = new History();
            try
            {

                history.Id = Guid.NewGuid();
                history.StaffKey = user.StaffKey;
                history.SourceKey = work.MWorkOrderKey;
                if (blnInsert)
                {
                    history.Operation = "I";
                    history.NewValue = work.NewLog;
                }
                else
                {
                    history.Operation = "U";
                    history.NewValue = work.NewLog;
                    history.OldValue = work.OldLog;
                }
                history.ModuleName = "iRepair";
                history.TableName = "WO";
                history.Detail = "(iRepair) " + "WO#" + work.Id + "; " + work.DetailLog; // BLL_Staff.GetLoginUsername() + " added WO#" + work.Seqno + ": " + work.Description;
                if (AbpSession.TenantId != null)
                {
                    history.TenantId = (int?)AbpSession.TenantId;
                }
                listHistory.Add(history);

                success = _mworkordertimesheetdalRepository.InsertHistoryList(listHistory);
            }
            catch (Exception ex)
            {
                //LogHelper.writeLog("Error : \r\n" + ex.ToString(), "Error/DBException");
            }
            return success;
        }

        private string GetNewChangeLog(DataTable dt, MWorkOrderInput work)
        {
            try
            {
                string strLog = "";
                StringBuilder sb = new StringBuilder();
                if (!dt.Rows[0]["Description"].ToString().Equals(work.Description))
                {
                    sb.Append(" Desc. to=> " + work.Description);
                }
                if (!dt.Rows[0]["Notes"].ToString().Equals(work.Notes))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Notes to=> " + work.Notes);
                }
                if (!dt.Rows[0]["RoomKey"].ToString().Equals(work.RoomKey.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Room to=> " + work.Room);
                }
                if (!dt.Rows[0]["MArea"].ToString().Equals(work.MArea.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Area to=> " + work.MAreaDesc);
                }
                if (!dt.Rows[0]["MWorkType"].ToString().Equals(work.MWorkType.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" WorkType to=> " + work.MWorkTypeDesc);
                }
                if (!dt.Rows[0]["Priority"].ToString().Equals(work.Priority.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Priority => " + work.PriorityDesc);
                }
                if (!dt.Rows[0]["StaffName"].ToString().Equals(work.StaffName))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" ReportedBy to=> " + work.StaffName);
                }
                if (dt.Rows[0]["ReportedOn"].ToString() != null && dt.Rows[0]["ReportedOn"].ToString() != "")
                {
                    if (!Convert.ToDateTime(dt.Rows[0]["ReportedOn"]).ToString("dd/MM/yyyy").Equals(Convert.ToDateTime(work.ReportedOn).ToString("dd/MM/yyyy")))
                    {
                        if (!string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(", ");
                        sb.Append(" ReportedDate to=> " + Convert.ToDateTime(work.ReportedOn).ToString("dd/MM/yyyy"));
                    }
                }
                else
                {
                    if (work.ReportedOn.HasValue)
                    {
                        if (!string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(", ");
                        sb.Append(" ReportedDate to=> " + Convert.ToDateTime(work.ReportedOn).ToString("dd/MM/yyyy"));
                    }
                }
                if (!dt.Rows[0]["MWorkOrderStatus"].ToString().Equals(work.MWorkOrderStatus.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Status to=> " + work.MWorkOrderStatusDesc);
                }
                if (!dt.Rows[0]["MTechnician"].ToString().Equals(work.MTechnician.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Technician to=> " + work.MTechnicianName);
                }


                strLog = "Change Log- " + (string.IsNullOrEmpty(sb.ToString()) ? "None" : sb.ToString());

                return strLog;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetChangeLogWoInfo(DataTable dt, MWorkOrderInput work)
        {
            try
            {
                string strLog = "";
                StringBuilder sb = new StringBuilder();
                if (!dt.Rows[0]["Description"].ToString().Equals(work.Description))
                {
                    sb.Append(" Desc. => " + dt.Rows[0]["Description"].ToString());
                }
                if (!dt.Rows[0]["Notes"].ToString().Equals(work.Notes))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" Notes => " + dt.Rows[0]["Notes"].ToString());
                }
                if (!dt.Rows[0]["RoomKey"].ToString().Equals(work.RoomKey.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    if (!string.IsNullOrEmpty(dt.Rows[0]["RoomKey"].ToString()) && dt.Rows[0]["RoomKey"].ToString() != "")
                    {
                        var room = GetRoomByKey(Guid.Parse(dt.Rows[0]["RoomKey"].ToString() == "" ? Guid.Empty.ToString() : dt.Rows[0]["RoomKey"].ToString()));
                        if (!string.IsNullOrEmpty(room) && room != "")
                            sb.Append(" Room => " + room);       //change something
                    }
                    else
                        sb.Append(" Room => -");
                }
                if (!dt.Rows[0]["MArea"].ToString().Equals(work.MArea.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    var area = GetAreaByKey(Convert.ToInt32(dt.Rows[0]["MArea"].ToString() == "" ? "-1" : dt.Rows[0]["MArea"].ToString()));
                    if (!string.IsNullOrEmpty(area) && area != "")
                        sb.Append(" Area => " + area);      //change something
                    else
                        sb.Append(" Area => -");
                }
                if (!dt.Rows[0]["MWorkType"].ToString().Equals(work.MWorkType.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    if (dt.Rows[0]["MWorkType"].ToString() != "" && !string.IsNullOrEmpty(dt.Rows[0]["MWorkType"].ToString()))
                    {
                        var worktype = GetWorkTypeByKey(Convert.ToInt32(dt.Rows[0]["MWorkType"].ToString() == "" ? "-1" : dt.Rows[0]["MWorkType"].ToString()));
                        if (!string.IsNullOrEmpty(worktype) && worktype != "")
                            sb.Append(" WorkType => " + worktype);      //change something
                    }
                    else
                        sb.Append(" WorkType => -");
                }
                if (!dt.Rows[0]["Priority"].ToString().Equals(work.Priority.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    if (dt.Rows[0]["Priority"].ToString() != "" && !string.IsNullOrEmpty(dt.Rows[0]["Priority"].ToString()))
                    {
                        var mpriority = GetPriorityName(Convert.ToInt32(dt.Rows[0]["Priority"].ToString() == "" ? "-1" : dt.Rows[0]["Priority"].ToString()));
                        if (!string.IsNullOrEmpty(mpriority) && mpriority != "")
                            sb.Append(" Priority => " + mpriority);      //change something
                        //if (dt.Rows[0]["Priority"].ToString() == "1")
                        //    sb.Append(" Priority => Low");
                        //else if (dt.Rows[0]["Priority"].ToString() == "2")
                        //    sb.Append(" Priority => Medium");
                        //else
                        //    sb.Append(" Priority => High");
                    }
                    else
                        sb.Append(" Priority => -");
                }
                if (!dt.Rows[0]["StaffName"].ToString().Equals(work.StaffName))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" ReportedBy => " + dt.Rows[0]["StaffName"].ToString());
                }
                if (dt.Rows[0]["ReportedOn"].ToString() != null && dt.Rows[0]["ReportedOn"].ToString() != "")
                {
                    if (!Convert.ToDateTime(dt.Rows[0]["ReportedOn"]).ToString("dd/MM/yyyy").Equals(Convert.ToDateTime(work.ReportedOn).ToString("dd/MM/yyyy")))
                    {
                        if (!string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(", ");
                        sb.Append(" ReportedDate => " + Convert.ToDateTime(dt.Rows[0]["ReportedOn"]).ToString("dd/MM/yyyy"));
                    }
                }
                else
                {
                    //if (work.ReportedOn.HasValue)
                    //{
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    sb.Append(" ReportedDate => -");
                    //}
                }
                if (!dt.Rows[0]["MWorkOrderStatus"].ToString().Equals(work.MWorkOrderStatus.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    if (!string.IsNullOrEmpty(dt.Rows[0]["MWorkOrderStatus"].ToString()) && dt.Rows[0]["MWorkOrderStatus"].ToString() != "")
                    {
                        var workstatus = GetWorkStatusByKey(Convert.ToInt32(dt.Rows[0]["MWorkOrderStatus"].ToString()));
                        if (!string.IsNullOrEmpty(workstatus) && workstatus != "")
                            sb.Append(" Status => " + workstatus);     //change something
                    }
                    else
                        sb.Append(" Status => -");
                }
                if (!dt.Rows[0]["MTechnician"].ToString().Equals(work.MTechnician.ToString()))
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        sb.Append(", ");
                    if (!string.IsNullOrEmpty(dt.Rows[0]["MTechnician"].ToString()) && dt.Rows[0]["MTechnician"].ToString() != "")
                    {
                        var name = GetMTechnicianBySeqNo(Convert.ToInt32(dt.Rows[0]["MTechnician"].ToString()));
                        if (!string.IsNullOrEmpty(name) && name != "")
                            sb.Append(" Technician => " + name);      //change something
                    }
                    else
                        sb.Append(" Technician => -");
                }

                strLog = "Log- " + (string.IsNullOrEmpty(sb.ToString()) ? "None" : sb.ToString());

                return strLog;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //private string GetAreaByKey(int v)
        //{
        //    return _mareaRepository.GetAll().Where(x => x.Id == v).Select(x => x.Description).FirstOrDefault();
        //}

        //private string GetWorkTypeByKey(int v)
        //{
        //    return _worktypeRepository.GetAll().Where(x => x.Id == v).Select(x => x.Description).FirstOrDefault();
        //}

        //private string GetRoomByKey(Guid key)
        //{
        //    return _roomRepository.GetAll().Where(x => x.Id == key).Select(x => x.Unit).FirstOrDefault();
        //}

        private int UpdatWorkOrder(MWorkOrderInput work)
        {
            return _mworkordertimesheetdalRepository.UpdatWorkOrder(work);
        }

        private bool IsUpdateWorkOrderStatus(DataTable dt, MWorkOrderInput work)
        {
            try
            {
                bool blnUpdate = false;
                if (!dt.Rows[0]["MWorkOrderStatus"].ToString().Equals(work.MWorkOrderStatus.ToString()))
                {
                    blnUpdate = true;
                }

                return blnUpdate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsAssignToTechnician(DataTable dt, MWorkOrderInput work)
        {
            try
            {
                bool blnAssignToTechnician = false;
                if (!dt.Rows[0]["MTechnician"].ToString().Equals(work.MTechnician.ToString()))
                {
                    blnAssignToTechnician = true;
                }

                return blnAssignToTechnician;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region common
        protected List<DDLRoomOutput> GetAllRoom()
        {
            return _mworkordertimesheetdalRepository.GetAllRoom();
        }
        protected List<DDLAreaOutput> GetAllArea()
        {
            return _mworkordertimesheetdalRepository.GetAllArea();
        }
        private List<DDLWorkTypeOutput> GetAllWorkType()
        {
            return _mworkordertimesheetdalRepository.GetAllWorkType();
        }
        protected List<MaidOutput> GetAllReportedBy()
        {
            return _mworkordertimesheetdalRepository.GetAllReportedBy();
        }
        protected List<DDLWorkStatusOutput> GetAllCurrentStatus()
        {
            return _mworkordertimesheetdalRepository.GetAllCurrentStatus();
        }
        protected List<DDPriorityOutput> GetAllPriority()
        {
            return _mworkordertimesheetdalRepository.GetAllPriority();
        }
        protected List<DDLTechnicianOutput> GetAllTechnician()
        {
            return _mworkordertimesheetdalRepository.GetAllTechnician();
        }
        protected List<DDLNoteTemplateOutput> GetAllNoteTemplate()
        {
            return _mworkordertimesheetdalRepository.GetAllNoteTemplate();
        }
        protected List<WoSecurityAuditlist> GetWorkOrderHistory(string mWorkOrderKey)
        {
            return _mworkordertimesheetdalRepository.GetWorkOrderHistory(mWorkOrderKey);
        }

        protected List<WoWorkNote> GetWorkNotes(string mWorkOrderKey)
        {
            return _mworkordertimesheetdalRepository.GetWorkNotes(mWorkOrderKey);
        }

        protected DataTable GetWorkOrderByID(int seqno)
        {
            return _mworkordertimesheetdalRepository.GetWorkOrderByID(seqno);
        }

        protected List<BlockUnBlockRoomListOutput> GetBlockRoomByWorkOrderID(int seqno)
        {
            return _mworkordertimesheetdalRepository.GetBlockRoomByWorkOrderID(seqno);
        }

        protected bool MaidHasStartedTask(int seqno)
        {
            return _mworkordertimesheetdalRepository.MaidHasStartedTask(seqno);
        }

        protected List<WoTimeSheetListOutput> GetWorkTimeSheetByWOID(int seqno)
        {
            return _mworkordertimesheetdalRepository.GetWorkTimeSheetByWOID(seqno);
        }

        private string GetAreaByKey(int key)
        {
            return _mworkordertimesheetdalRepository.GetAreaByKey(key);
        }

        private string GetWorkTypeByKey(int key)
        {
            return _mworkordertimesheetdalRepository.GetWorkTypeByKey(key);
        }

        private string GetRoomByKey(Guid key)
        {
            return _mworkordertimesheetdalRepository.GetRoomByKey(key);
        }
        protected string GetMTechnicianBySeqNo(int Seqno)
        {
            return _mworkordertimesheetdalRepository.GetMTechnicianBySeqNo(Seqno);

        }
        protected string GetWorkStatusByKey(int Seqno)
        {
            return _mworkordertimesheetdalRepository.GetWorkStatusByKey(Seqno);
        }
        protected string GetReportedName(string staffkey)
        {
            Guid id = new Guid(staffkey);
            return _mworkordertimesheetdalRepository.GetReportedName(id);
        }
        protected string GetPriorityName(int sort)
        {
            return _mworkordertimesheetdalRepository.GetPriorityName(sort);
        }
        protected Guid GetTechnicianKey(Guid id)
        {
            return _mworkordertimesheetdalRepository.GetTechnicianKey(id);
        }
        protected int GetTechnicalID(Guid TechnicianKey)
        {
            return _mworkordertimesheetdalRepository.GetTechnicalID(TechnicianKey);
        }

        #endregion
    }
}
