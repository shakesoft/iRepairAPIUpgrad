using Abp.Dependency;

namespace BEZNgCore.Maui.Services.UI;

public class LanguageService : ISingletonDependency
{
    public event EventHandler OnLanguageChanged;

    public void ChangeLanguage()
    {
        OnLanguageChanged?.Invoke(this, EventArgs.Empty);
    }
}