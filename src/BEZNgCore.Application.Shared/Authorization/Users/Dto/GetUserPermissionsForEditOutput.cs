using System.Collections.Generic;
using BEZNgCore.Authorization.Permissions.Dto;

namespace BEZNgCore.Authorization.Users.Dto;

public class GetUserPermissionsForEditOutput
{
    public List<FlatPermissionDto> Permissions { get; set; }

    public List<string> GrantedPermissionNames { get; set; }
}

