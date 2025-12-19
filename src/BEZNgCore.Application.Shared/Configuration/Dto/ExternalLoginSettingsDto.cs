using System.Collections.Generic;

namespace BEZNgCore.Configuration.Dto;

public class ExternalLoginSettingsDto
{
    public List<string> EnabledSocialLoginSettings { get; set; } = new List<string>();
}

