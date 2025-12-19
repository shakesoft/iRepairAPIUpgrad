using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
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
    public class ViewBlockRoomAppService : BEZNgCoreAppServiceBase
    {

        private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IRepository<MTechnician, int> _technicianRepository;
        private readonly IRepository<Control, Guid> _controlRepository;
        private readonly IMworkorderdalRepository _mwordorderdalRepository;
        public ViewBlockRoomAppService(
            IRepository<Room, Guid> roomRepository,
            IRepository<MTechnician, int> technicianRepository,
           IRepository<Control, Guid> controlRepository,
           IMworkorderdalRepository mwordorderdalRepository
            )
        {
            _roomRepository = roomRepository;
            _technicianRepository = technicianRepository;
            _controlRepository = controlRepository;
            _mwordorderdalRepository = mwordorderdalRepository;
        }
        [HttpGet]
        public ListResultDto<ViewBlockRoomViewData> GetViewBlockRoomViewData()
        {
            List<ViewBlockRoomViewData> Alllst = new List<ViewBlockRoomViewData>();
            ViewBlockRoomViewData a = new ViewBlockRoomViewData();

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

            List<DDLRoomOutput> ddlr = new List<DDLRoomOutput>();
            DDLRoomOutput ro = new DDLRoomOutput();
            ro.RoomKey = Guid.Empty;
            ro.Unit = "ALL Rooms";
            ddlr.Add(ro);
            var ddlr2 = _roomRepository.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Floor).ThenBy(x => x.Unit)
              .Select(x => new DDLRoomOutput
              {
                  RoomKey = x.Id,
                  Unit = x.Unit
              });
            a.DDLRoom = ddlr.Concat(ddlr2).ToList();

            DateTime dtBusinessDate = DateTime.Now;
            dtBusinessDate = (DateTime)_controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
            a.FromDate = dtBusinessDate;
            a.ToDate = dtBusinessDate.AddDays(7);


            Alllst.Add(a);
            return new ListResultDto<ViewBlockRoomViewData>(Alllst);
        }

        public async Task<PagedResultDto<GetViewBlockRoomOutput>> GetBindGrid(string tech = "-1", string Room = "00000000-0000-0000-0000-000000000000", string FromDate = "", string ToDate = "")
        {
            try
            {
                DateTime dtFromDate; DateTime dtToDate;
                DateTime dtBusinessDate = DateTime.Now;
                dtBusinessDate = (DateTime)_controlRepository.GetAll().Select(x => x.SystemDate).FirstOrDefault();
                dtFromDate = (string.IsNullOrEmpty(FromDate)) ? dtBusinessDate : Convert.ToDateTime(FromDate);
                dtToDate = (string.IsNullOrEmpty(ToDate)) ? dtBusinessDate.AddDays(7) : Convert.ToDateTime(ToDate);
                List<GetViewBlockRoomOutput> dt = new List<GetViewBlockRoomOutput>();
                dt = _mwordorderdalRepository.GetBlockRoomWorkOrderBy(Convert.ToInt32(tech), Room, dtFromDate, dtToDate);
                var Count = dt.Count;

                return new PagedResultDto<GetViewBlockRoomOutput>(
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
