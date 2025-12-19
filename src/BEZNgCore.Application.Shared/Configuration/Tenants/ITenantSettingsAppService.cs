using System.Threading.Tasks;
using Abp.Application.Services;
using BEZNgCore.Configuration.Tenants.Dto;

namespace BEZNgCore.Configuration.Tenants;

public interface ITenantSettingsAppService : IApplicationService
{
    Task<TenantSettingsEditDto> GetAllSettings();

    Task UpdateAllSettings(TenantSettingsEditDto input);

    Task ClearDarkLogo();

    Task ClearDarkLogoMinimal();

    Task ClearLightLogo();

    Task ClearLightLogoMinimal();

    Task ClearCustomCss();
}

