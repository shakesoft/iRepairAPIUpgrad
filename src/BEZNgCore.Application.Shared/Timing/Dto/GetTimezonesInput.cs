using Abp.Configuration;

namespace BEZNgCore.Timing.Dto;

public class GetTimezonesInput
{
    public SettingScopes DefaultTimezoneScope { get; set; }
}

