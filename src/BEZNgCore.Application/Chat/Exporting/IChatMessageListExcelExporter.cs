using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using BEZNgCore.Chat.Dto;
using BEZNgCore.Dto;

namespace BEZNgCore.Chat.Exporting;

public interface IChatMessageListExcelExporter
{
    Task<FileDto> ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
}