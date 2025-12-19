using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BEZNgCore.Authorization.Permissions.Dto;

namespace BEZNgCore.Authorization.Permissions;

public interface IPermissionAppService : IApplicationService
{
    ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
}

