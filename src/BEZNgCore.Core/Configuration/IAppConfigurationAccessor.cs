using Microsoft.Extensions.Configuration;

namespace BEZNgCore.Configuration;

public interface IAppConfigurationAccessor
{
    IConfigurationRoot Configuration { get; }
}

