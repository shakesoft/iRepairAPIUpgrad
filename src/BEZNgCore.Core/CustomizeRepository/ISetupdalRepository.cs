using Abp.Domain.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.CustomizeRepository
{
    public interface ISetupdalRepository : IRepository<MWorkType, int>
    {
        List<SetupTabOutput> GetAllWorkOrderStatus();
        List<SetupTabOutput> GetActiveWorkOrderStatus();
        List<SetupTabOutput> GetAllMWorkType();
        List<SetupTabOutput> GetActiveMWorkType();
        List<SetupTabOutput> GetAllMArea();
        List<SetupTabOutput> GetActiveMArea();
        List<SetupTabOutput> GetAllMWorkTimeSheetNoteTemplate();
        List<SetupTabOutput> GetActiveMWorkTimeSheetNoteTemplate();
        List<MPriorityOutput> GetAllPriority();
        List<MTechnicianOutput> GetAllMTechnician();
        List<MTechnicianOutput> GetActiveMTechnician();
        int InsertMPriority(MPriorityModel priority);
        int InsertMWorkOrderStatus(MWorkOrderStatus status);
        int InsertMWorkTimeSheetNoteTemplate(MWorkTimeSheetNoteTemplate note);
        int InsertMArea(MArea workArea);
        int InsertMWorkType(MWorkType workType);
        int GetMaxSort();
        int UpdateMWorkOrderStatus(MWorkOrderStatus woStatus);
        int UpdateMWorkType(MWorkType wotype);
        int UpdateMArea(MArea area);
        int UpdateMWorkTimeSheetNoteTemplate(MWorkTimeSheetNoteTemplate note);
        int UpdateMPriority(MPriorityModel mp);
        int UpdateMTechnician(MTechnician techn);
        List<MPriorityOutput> GetActivePriority();
        List<ExternalAccessOutput> GetExternalAccess();
    }
}
