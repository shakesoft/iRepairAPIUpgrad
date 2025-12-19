using System.Threading.Tasks;
using Abp.Application.Services;
using BEZNgCore.Configuration.Host.Dto;

namespace BEZNgCore.Configuration.Host;

public interface IHostSettingsAppService : IApplicationService
{
    Task<HostSettingsEditDto> GetAllSettings();

    Task UpdateAllSettings(HostSettingsEditDto input);

    Task SendTestEmail(SendTestEmailInput input);
}

