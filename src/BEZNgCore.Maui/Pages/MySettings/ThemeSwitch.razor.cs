using Microsoft.AspNetCore.Components;
using BEZNgCore.Maui.Core.Components;
using BEZNgCore.Maui.Core.Threading;
using BEZNgCore.Maui.Services.UI;


namespace BEZNgCore.Maui.Pages.MySettings;

public partial class ThemeSwitch : BEZNgCoreComponentBase
{
    private string _selectedTheme = ThemeService.GetUserTheme();

    private string[] _themes = ThemeService.GetAllThemes();

    public string SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            _selectedTheme = value;
            AsyncRunner.Run(ThemeService.SetUserTheme(JS, SelectedTheme));
        }
    }
}