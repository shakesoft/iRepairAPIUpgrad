using Abp.AutoMapper;
using BEZNgCore.ApiClient;

namespace BEZNgCore.Maui.Models.Common;

[AutoMapFrom(typeof(TenantInformation)),
 AutoMapTo(typeof(TenantInformation))]
public class TenantInformationPersistanceModel
{
    public string TenancyName { get; set; }

    public int TenantId { get; set; }
}