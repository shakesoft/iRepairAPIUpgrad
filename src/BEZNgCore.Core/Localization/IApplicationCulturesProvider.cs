using System.Globalization;

namespace BEZNgCore.Localization;

public interface IApplicationCulturesProvider
{
    CultureInfo[] GetAllCultures();
}

