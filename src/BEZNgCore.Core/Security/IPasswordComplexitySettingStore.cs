using System.Threading.Tasks;

namespace BEZNgCore.Security;

public interface IPasswordComplexitySettingStore
{
    Task<PasswordComplexitySetting> GetSettingsAsync();
}

