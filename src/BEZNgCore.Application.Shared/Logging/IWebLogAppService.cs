using Abp.Application.Services;
using BEZNgCore.Dto;
using BEZNgCore.Logging.Dto;

namespace BEZNgCore.Logging;

public interface IWebLogAppService : IApplicationService
{
    GetLatestWebLogsOutput GetLatestWebLogs();

    FileDto DownloadWebLogs();
}

