using BEZNgCore.Auditing.Dto;
using BEZNgCore.Dto;
using BEZNgCore.EntityChanges.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BEZNgCore.Auditing.Exporting;

public interface IAuditLogListExcelExporter
{
    Task<FileDto> ExportToFile(List<AuditLogListDto> auditLogListDtos);

    Task<FileDto> ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
}
