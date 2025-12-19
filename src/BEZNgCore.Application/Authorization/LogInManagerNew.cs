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
using BEZNgCore.CustomizeRepository;

namespace BEZNgCore.Authorization
{
    public class LogInManagerNew : AbpLoginManagerNew<Tenant, Role, User>
    {
        public LogInManagerNew(
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

        //IRepository<Guest, Guid> guestRepository, IRepository<Reservation, Guid> reservationRepository, IRepository<Room, Guid> roomRepository ,
        IRepository<User, long> userRepository,
            IRepository<Staff, Guid> staffRepository,
            ISetupdalRepository setupdalRepository

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
                  staffRepository,
                  setupdalRepository
                  )
        {

        }

    }
}
