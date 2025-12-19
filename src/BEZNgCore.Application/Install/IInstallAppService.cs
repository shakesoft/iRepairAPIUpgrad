using System.Threading.Tasks;
using Abp.Application.Services;
using BEZNgCore.Install.Dto;

namespace BEZNgCore.Install;

public interface IInstallAppService : IApplicationService
{
    Task Setup(InstallDto input);

    AppSettingsJsonDto GetAppSettingsJson();

    CheckDatabaseOutput CheckDatabase();
}
