using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BEZNgCore.Authorization.IrepairDal;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class ViewWorkOrderStatusAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IRepository<MTechnician, int> _technicianRepository;
        //private readonly IRepository<Control, Guid> _controlRepository;
        private readonly IRepository<MArea, int> _mareaRepository;
        private readonly IRepository<MWorkOrderStatus, int> _mworkorderstatusRepository;
        private readonly IRepository<MWorkType, int> _worktypeRepository;
        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly IMworkorderdalRepository _mwordorderdalRepository;
        public ViewWorkOrderStatusAppService(
           IRepository<Room, Guid> roomRepository,
           IRepository<MTechnician, int> technicianRepository,
           //IRepository<Control, Guid> controlRepository,
           IRepository<MArea, int> mareaRepository,
           IRepository<MWorkOrderStatus, int> mworkorderstatusRepository,
           IRepository<MWorkType, int> worktypeRepository,
           IRepository<Staff, Guid> staffRepository,
           IMworkorderdalRepository mwordorderdalRepository

            )
        {
            _roomRepository = roomRepository;
            _technicianRepository = technicianRepository;
            //_controlRepository = controlRepository;
            _mareaRepository = mareaRepository;
            _mworkorderstatusRepository = mworkorderstatusRepository;
            _worktypeRepository = worktypeRepository;
            _staffRepository = staffRepository;
            _mwordorderdalRepository = mwordorderdalRepository;
        }
        [HttpGet]
        public ListResultDto<ViewWorkOrderStatusViewData> GetViewWorkOrderStatusViewData()
        {
            List<ViewWorkOrderStatusViewData> Alllst = new List<ViewWorkOrderStatusViewData>();
            ViewWorkOrderStatusViewData a = new ViewWorkOrderStatusViewData();

            List<DDLTechnicianOutput> ddlt = new List<DDLTechnicianOutput>();
            DDLTechnicianOutput t = new DDLTechnicianOutput();
            t.Seqno = "-1";
            t.Name = "ALL";
            ddlt.Add(t);
            var ddlt2 = _technicianRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Name)
              .Select(x => new DDLTechnicianOutput
              {
                  Seqno = x.Id.ToString(),
                  Name = x.Name
              });


            a.DDLTechnician = ddlt.Concat(ddlt2).ToList();

            List<DDLAreaOutput> ddla = new List<DDLAreaOutput>();
            DDLAreaOutput ar = new DDLAreaOutput();
            ar.Seqno = 99;
            ar.Description = "ALL";
            ddla.Add(ar);
            var ddla2 = _mareaRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Description)
              .Select(x => new DDLAreaOutput
              {
                  Seqno = x.Id,
                  Description = x.Description
              });


            a.DDLArea = ddla.Concat(ddla2).ToList();



            List<DDLRoomOutput> ddlr = new List<DDLRoomOutput>();
            DDLRoomOutput ro = new DDLRoomOutput();
            ro.RoomKey = Guid.Empty;
            ro.Unit = "ALL";
            ddlr.Add(ro);
            var ddlr2 = _roomRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Floor).ThenBy(x => x.Unit)
              .Select(x => new DDLRoomOutput
              {
                  RoomKey = x.Id,
                  Unit = x.Unit
              });
            a.DDLRoom = ddlr.Concat(ddlr2).ToList();

            List<DDLWorkStatusOutput> ddlws = new List<DDLWorkStatusOutput>();
            DDLWorkStatusOutput ws = new DDLWorkStatusOutput();
            ws.Seqno = "999";
            ws.Description = "ALL";
            ddlws.Add(ws);
            var ddlws2 = _mworkorderstatusRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Description)
              .Select(x => new DDLWorkStatusOutput
              {
                  Seqno = x.Id.ToString(),
                  Description = x.Description
              });
            a.DDLWorkStatus = ddlws.Concat(ddlws2).ToList();

            List<DDLWorkTypeOutput> ddlwt = new List<DDLWorkTypeOutput>();
            DDLWorkTypeOutput wt = new DDLWorkTypeOutput();
            wt.Seqno = 999;
            wt.Description = "ALL";
            ddlwt.Add(wt);
            var ddlwt2 = _worktypeRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Description)
              .Select(x => new DDLWorkTypeOutput
              {
                  Seqno = x.Id,
                  Description = x.Description
              });
            a.DDLWorkType = ddlwt.Concat(ddlwt2).ToList();

            List<DDLCreatedByOutput> ddlcb = new List<DDLCreatedByOutput>();
            DDLCreatedByOutput cb = new DDLCreatedByOutput();
            cb.StaffKey = Guid.Empty;
            cb.UserName = "ALL";
            ddlcb.Add(cb);
            var ddlcb2 = _staffRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.UserName)
              .Select(x => new DDLCreatedByOutput
              {
                  StaffKey = x.Id,
                  UserName = x.UserName
              });
            a.DDLCreatedBy = ddlcb.Concat(ddlcb2).ToList();

            List<DDLMPriorityOutput> sl = new List<DDLMPriorityOutput>();
            DDLMPriorityOutput s1 = new DDLMPriorityOutput();
            s1.strPriorityStatus = "-1";
            s1.strPriorityDesc = "ALL";
            sl.Add(s1);
            List<DDLMPriorityOutput> mm = BindPriority();
            a.DDLMPriority = sl.Concat(mm).ToList();



            Alllst.Add(a);
            return new ListResultDto<ViewWorkOrderStatusViewData>(Alllst);
        }
        private List<DDLMPriorityOutput> BindPriority()
        {
            
            return _mwordorderdalRepository.GetDDLPriority();

        }
        public async Task<PagedResultDto<GetAllWoOutput>> GetBindGrid(string ddlTechnician = "-1", string ddlWorkType = "999", string ddlWorkOrderStatus = "999",
            string ddlArea = "99", string ddlRoom = "ALL", string ddlStaff = "00000000-0000-0000-0000-000000000000", string ddlMpriority = "-1")
        {
            try
            {
                List<GetAllWoOutput> dt = new List<GetAllWoOutput>();
                var tech = Convert.ToInt32(ddlTechnician);
                var worktype = Convert.ToInt32(ddlWorkType);
                var workstatus = Convert.ToInt32(ddlWorkOrderStatus);
                var area = Convert.ToInt32(ddlArea);
                var unit = ddlRoom.Trim();
                var staff = Guid.Parse(ddlStaff);
                var mpriority = Convert.ToInt32(ddlMpriority);
                dt = _mwordorderdalRepository.GetIncompletedWorkOrderStatus(tech, worktype, workstatus, area, unit, staff, mpriority);

                var Count = dt.Count;

                return new PagedResultDto<GetAllWoOutput>(
                Count,
                dt
            );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
