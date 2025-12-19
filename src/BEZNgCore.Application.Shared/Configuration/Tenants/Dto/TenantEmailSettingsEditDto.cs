using Abp.Auditing;
using BEZNgCore.Configuration.Dto;

namespace BEZNgCore.Configuration.Tenants.Dto;

public class TenantEmailSettingsEditDto : EmailSettingsEditDto
{
    public bool UseHostDefaultEmailSettings { get; set; }
}

