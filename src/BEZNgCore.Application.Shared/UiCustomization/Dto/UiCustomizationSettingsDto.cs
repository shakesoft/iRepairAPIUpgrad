using BEZNgCore.Configuration.Dto;

namespace BEZNgCore.UiCustomization.Dto;

public class UiCustomizationSettingsDto
{
    public ThemeSettingsDto BaseSettings { get; set; }

    public bool IsLeftMenuUsed { get; set; }

    public bool IsTopMenuUsed { get; set; }

    public bool IsTabMenuUsed { get; set; }

    public bool AllowMenuScroll { get; set; } = true;
}

