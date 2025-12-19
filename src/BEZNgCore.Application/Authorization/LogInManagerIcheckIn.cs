using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using BEZNgCore.Authorization.Roles;
using BEZNgCore.Authorization.Users;
using BEZNgCore.MultiTenancy;
using System;
using BEZNgCore.IrepairModel;

namespace BEZNgCore.Authorization
{
    public class LogInManagerIcheckIn : AbpLoginManagerIcheckIn<Tenant, Role, User>
    {
        public LogInManagerIcheckIn(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher,
            UserClaimsPrincipalFactory claimsPrincipalFactory,
            //IRepository<Room, Guid> roomRepository ,
            IRepository<User, long> userRepository,
            IRepository<Reservation, Guid> reservationRepository,
            IRepository<Guest, Guid> guestRepository

            )
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  passwordHasher,
                  roleManager,
                  claimsPrincipalFactory,
                  userRepository,
                  reservationRepository,
                  guestRepository
                  )
        {

        }

    }
}
