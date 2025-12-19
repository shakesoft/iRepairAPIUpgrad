using Abp.Authorization;
using BEZNgCore.Authorization.Roles;
using BEZNgCore.Authorization.Users;

namespace BEZNgCore.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {

    }
}

