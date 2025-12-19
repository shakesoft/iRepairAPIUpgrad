using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Abp;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Abp.UI;
using Abp.Zero.Configuration;
using BEZNgCore.Authorization.Users;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Identity;

namespace BEZNgCore.Authorization
{
    public class AbpLoginManagerNew<TTenant, TRole, TUser> : ITransientDependency
      where TTenant : AbpTenant<TUser>
      where TRole : AbpRole<TUser>, new()
      where TUser : AbpUser<TUser>
    {
        public IClientInfoProvider ClientInfoProvider { get; set; }

        protected IMultiTenancyConfig MultiTenancyConfig { get; }
        protected IRepository<TTenant> TenantRepository { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected AbpUserManager<TRole, TUser> UserManager { get; }
        protected ISettingManager SettingManager { get; }
        protected IRepository<UserLoginAttempt, long> UserLoginAttemptRepository { get; }
        protected IUserManagementConfig UserManagementConfig { get; }
        protected IIocResolver IocResolver { get; }
        protected AbpRoleManager<TRole, TUser> RoleManager { get; }

        private readonly IPasswordHasher<TUser> _passwordHasher;

        private readonly AbpUserClaimsPrincipalFactory<TUser, TRole> _claimsPrincipalFactory;

        //private readonly IRepository<Reservation, Guid> _reservationRepository;
        //private readonly IRepository<Guest, Guid> _guestRepository;
        //private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly ISetupdalRepository _setupdalRepository;
        public AbpLoginManagerNew(
            AbpUserManager<TRole, TUser> userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<TTenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            IPasswordHasher<TUser> passwordHasher,
            AbpRoleManager<TRole, TUser> roleManager,
            AbpUserClaimsPrincipalFactory<TUser, TRole> claimsPrincipalFactory,
            //IRepository<Guest, Guid> guestRepository,IRepository<Reservation, Guid> reservationRepository, IRepository<Room, Guid> roomRepository, 
            IRepository<User, long> userRepository,
            IRepository<Staff, Guid> staffRepository,
            ISetupdalRepository setupdalRepository)
        {
            _passwordHasher = passwordHasher;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            MultiTenancyConfig = multiTenancyConfig;
            TenantRepository = tenantRepository;
            UnitOfWorkManager = unitOfWorkManager;
            SettingManager = settingManager;
            UserLoginAttemptRepository = userLoginAttemptRepository;
            UserManagementConfig = userManagementConfig;
            IocResolver = iocResolver;
            RoleManager = roleManager;
            UserManager = userManager;
            _userRepository = userRepository;
            _staffRepository = staffRepository;
            _setupdalRepository = setupdalRepository;

            ClientInfoProvider = NullClientInfoProvider.Instance;

            //_guestRepository = guestRepository;
            //_reservationRepository = reservationRepository;
            //_roomRepository = roomRepository;
        }

        [UnitOfWork]
        public virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsync(UserLoginInfo login, string tenancyName = null)
        {
            var result = await LoginAsyncInternal(login, tenancyName);
            await SaveLoginAttemptAsync(result, tenancyName, login.ProviderKey + "@" + login.LoginProvider);
            return result;
        }

        protected virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsyncInternal(UserLoginInfo login, string tenancyName)
        {
            if (login == null || login.LoginProvider.IsNullOrEmpty() || login.ProviderKey.IsNullOrEmpty())
            {
                throw new ArgumentException("login");
            }

            //Get and check tenant
            TTenant tenant = null;
            if (!MultiTenancyConfig.IsEnabled)
            {
                tenant = await GetDefaultTenantAsync();
            }
            else if (!string.IsNullOrWhiteSpace(tenancyName))
            {
                tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                if (tenant == null)
                {
                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidTenancyName);
                }

                if (!tenant.IsActive)
                {
                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.TenantIsNotActive, tenant);
                }
            }

            int? tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var user = await UserManager.FindAsync(tenantId, login);
                if (user == null)
                {
                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UnknownExternalLogin, tenant);
                }

                return await CreateLoginResultAsync(user, tenant);
            }
        }


        #region irepair
        
        [UnitOfWork]
        public virtual async Task<AbpLoginResult<TTenant, TUser>> LoginIRAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null, bool shouldLockout = true)
        {
            var result = await LoginAsyncInternalIR(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
            await SaveLoginAttemptAsync(result, tenancyName, userNameOrEmailAddress);
            return result;
        }
        protected virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsyncInternalIR(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {



            //Get and check tenant
            TTenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await UserManager.InitializeOptionsAsync(tenantId);


                var loggedInFromExternalSource = true;
                if (!userNameOrEmailAddress.IsNullOrEmpty())
                {
                    bool pin = false;
                    pin = IsDigitsOnly(userNameOrEmailAddress);
                    if (pin)
                    {
                        string enpin = Base64Encryption(userNameOrEmailAddress);

                        var user = (from u in _userRepository.GetAll()
                                    where u.PIN == enpin
                                    select u).SingleOrDefault();
                        var TUsr = user as TUser;

                        if (user == null)
                        {
                           // return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                           throw new UserFriendlyException("Incorrect PIN number.");
                        }
                        else
                        {
                            var ss = (from s in _staffRepository.GetAll()
                                         where s.Id == user.StaffKey
                                         select s).SingleOrDefault();
                            if(ss!=null)
                            {
                                if (!string.IsNullOrEmpty(ss.UserName) && ss.TechnicianKey != null)
                                {
                                    string ClientIpAddress = ClientInfoProvider.ClientIpAddress == "::1" ? "127.0.0.1" : ClientInfoProvider.ClientIpAddress;
                                    List<ExternalAccessOutput> iplst = new List<ExternalAccessOutput>();
                                    iplst = _setupdalRepository.GetExternalAccess();
                                    bool isInRange = false;

                                    if (ss.Active == 1)
                                    {
                                       

                                    }
                                    else
                                    {

                                        //return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserIsNotActive, tenant, TUsr);
                                        throw new UserFriendlyException("Your account is inactive.");
                                    }
                                    if (ss.AnywhereAccess.HasValue)
                                    {
                                        if (ss.AnywhereAccess.Value == 1)
                                        {

                                        }
                                        else
                                        {
                                            if (iplst.Count == 0)
                                            {
                                                throw new UserFriendlyException("ClientIpAddress range not found in the database.");
                                            }
                                            else
                                            {
                                                foreach (ExternalAccessOutput dr in iplst)
                                                {
                                                    bool checkInRange = IsIpInRange(ClientIpAddress, dr.Start, dr.END);
                                                    isInRange = (checkInRange || isInRange);
                                                }
                                            }
                                            if (!isInRange)
                                            {
                                                throw new UserFriendlyException("You are not authorised to sign in from this location");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (iplst.Count == 0)
                                        {
                                            throw new UserFriendlyException("ClientIpAddress range not found in the database.");
                                        }
                                        else
                                        {
                                            foreach (ExternalAccessOutput dr in iplst)
                                            {
                                                bool checkInRange = IsIpInRange(ClientIpAddress, dr.Start, dr.END);
                                                isInRange = (checkInRange || isInRange);
                                            }
                                        }
                                        if (!isInRange)
                                        {
                                            throw new UserFriendlyException("You are not authorised to sign in from this location");
                                        }
                                    }

                                }
                                else
                                {
                                    // return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserIsNotActive, tenant, TUsr);
                                    throw new UserFriendlyException("Invalid.");
                                }
                            }
                            else
                            {
                                throw new UserFriendlyException("Incorrect PIN number.");
                            }

                        }

                        if (await UserManager.IsLockedOutAsync(TUsr))
                        {
                            return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                        }
                        if (!loggedInFromExternalSource)
                        {
                            if (!await UserManager.CheckPasswordAsync(TUsr, plainPassword))
                            {
                                if (shouldLockout)
                                {
                                    if (await TryLockOutAsync(tenantId, user.Id))
                                    {
                                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                                    }
                                }

                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidPassword, tenant, TUsr);
                            }
                        }

                        return await CreateLoginResultAsync(TUsr, tenant);
                    }
                    else
                    {
                        if (plainPassword.IsNullOrEmpty())
                        {
                            throw new ArgumentNullException(nameof(plainPassword));
                        }
                        else
                        {
                            var user = (from u in _userRepository.GetAll()
                                        where u.EmailAddress == userNameOrEmailAddress || u.UserName == userNameOrEmailAddress || u.PhoneNumber == userNameOrEmailAddress
                                        select u).SingleOrDefault();
                            var TUsr = user as TUser;

                            if (user == null)
                            {
                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                            }

                            if (await UserManager.IsLockedOutAsync(TUsr))
                            {
                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                            }
                            if (!loggedInFromExternalSource)
                            {
                                if (!await UserManager.CheckPasswordAsync(TUsr, plainPassword))
                                {
                                    if (shouldLockout)
                                    {
                                        if (await TryLockOutAsync(tenantId, user.Id))
                                        {
                                            return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                                        }
                                    }

                                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidPassword, tenant, TUsr);
                                }
                            }

                            return await CreateLoginResultAsync(TUsr, tenant);
                        }
                    }
                }

                else
                {
                    throw new ArgumentNullException(nameof(userNameOrEmailAddress));
                }
            }
        }


        protected virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsyncInternalIR_BK(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {



            //Get and check tenant
            TTenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await UserManager.InitializeOptionsAsync(tenantId);


                var loggedInFromExternalSource = true;
                if (!userNameOrEmailAddress.IsNullOrEmpty())
                {
                    bool pin = false;
                    pin = IsDigitsOnly(userNameOrEmailAddress);
                    if (pin)
                    {
                        string enpin = Base64Encryption(userNameOrEmailAddress);

                        var user = (from u in _userRepository.GetAll()
                                    where u.PIN == enpin
                                    select u).SingleOrDefault();
                        var TUsr = user as TUser;

                        if (user == null)
                        {
                            // return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                            throw new UserFriendlyException("Incorrect PIN number.");
                        }
                        else
                        {
                            var ss = (from s in _staffRepository.GetAll()
                                      where s.Id == user.StaffKey
                                      select s).SingleOrDefault();
                            if (ss != null)
                            {
                                if (!string.IsNullOrEmpty(ss.UserName) && ss.TechnicianKey != null)
                                {
                                    string ClientIpAddress = ClientInfoProvider.ClientIpAddress == "::1" ? "127.0.0.1" : ClientInfoProvider.ClientIpAddress;
                                    List<ExternalAccessOutput> iplst = new List<ExternalAccessOutput>();
                                    iplst = _setupdalRepository.GetExternalAccess();
                                    bool isInRange = false;

                                    if (ss.Active == 1)
                                    {


                                    }
                                    else
                                    {

                                        //return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserIsNotActive, tenant, TUsr);
                                        throw new UserFriendlyException("Your account is inactive.");
                                    }
                                    #region ExternalAccessWithClientIpAddress

                                    if (iplst.Count == 0)
                                    {
                                        throw new UserFriendlyException("ClientIpAddress range not found in the database.");
                                    }
                                    else
                                    {
                                        foreach (ExternalAccessOutput dr in iplst)
                                        {
                                            bool checkInRange = IsIpInRange(ClientIpAddress, dr.Start, dr.END);
                                            isInRange = (checkInRange || isInRange);
                                        }
                                    }
                                    //if (!isInRange)
                                    //{
                                    //    throw new UserFriendlyException("You are not authorised to sign in from this location");
                                    //}
                                    #endregion

                                }
                                else
                                {
                                    // return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserIsNotActive, tenant, TUsr);
                                    throw new UserFriendlyException("Invalid.");
                                }
                            }
                            else
                            {
                                throw new UserFriendlyException("Incorrect PIN number.");
                            }

                        }

                        if (await UserManager.IsLockedOutAsync(TUsr))
                        {
                            return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                        }
                        if (!loggedInFromExternalSource)
                        {
                            if (!await UserManager.CheckPasswordAsync(TUsr, plainPassword))
                            {
                                if (shouldLockout)
                                {
                                    if (await TryLockOutAsync(tenantId, user.Id))
                                    {
                                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                                    }
                                }

                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidPassword, tenant, TUsr);
                            }
                        }

                        return await CreateLoginResultAsync(TUsr, tenant);
                    }
                    else
                    {
                        if (plainPassword.IsNullOrEmpty())
                        {
                            throw new ArgumentNullException(nameof(plainPassword));
                        }
                        else
                        {
                            var user = (from u in _userRepository.GetAll()
                                        where u.EmailAddress == userNameOrEmailAddress || u.UserName == userNameOrEmailAddress || u.PhoneNumber == userNameOrEmailAddress
                                        select u).SingleOrDefault();
                            var TUsr = user as TUser;

                            if (user == null)
                            {
                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                            }

                            if (await UserManager.IsLockedOutAsync(TUsr))
                            {
                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                            }
                            if (!loggedInFromExternalSource)
                            {
                                if (!await UserManager.CheckPasswordAsync(TUsr, plainPassword))
                                {
                                    if (shouldLockout)
                                    {
                                        if (await TryLockOutAsync(tenantId, user.Id))
                                        {
                                            return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                                        }
                                    }

                                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidPassword, tenant, TUsr);
                                }
                            }

                            return await CreateLoginResultAsync(TUsr, tenant);
                        }
                    }
                }

                else
                {
                    throw new ArgumentNullException(nameof(userNameOrEmailAddress));
                }
            }
        }

        #endregion
        #region iclean

        [UnitOfWork]
        public virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null, bool shouldLockout = true)
        {
            var result = await LoginAsyncInternal(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
            await SaveLoginAttemptAsync(result, tenancyName, userNameOrEmailAddress);
            return result;
        }

        protected virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {



            //Get and check tenant
            TTenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await UserManager.InitializeOptionsAsync(tenantId);


                var loggedInFromExternalSource = true;
                if (!userNameOrEmailAddress.IsNullOrEmpty())
                {
                    bool pin = false;
                    pin = IsDigitsOnly(userNameOrEmailAddress);
                    if (pin)
                    {
                        string enpin = Base64Encryption(userNameOrEmailAddress);

                        var user = (from u in _userRepository.GetAll()
                                    where u.PIN == enpin
                                    select u).SingleOrDefault();
                        var TUsr = user as TUser;

                        if (user == null)
                        {
                            //return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                            throw new UserFriendlyException("Incorrect PIN number.");
                        }
                        else
                        {
                            var staff=(from st in _staffRepository.GetAll() 
                                       where st.Id == user.StaffKey select st).SingleOrDefault();
                            if(staff!=null)
                            {
                               if(!string.IsNullOrEmpty(staff.UserName) && (staff.MaidKey != null || staff.Sec_Supervisor == 10))
                                {
                                    if (staff.Active == 1)
                                    {
                                        
                                    }
                                    else
                                    {
                                        // return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserIsNotActive, tenant, TUsr);
                                        throw new UserFriendlyException("Your account is inactive.");
                                    }  
                                    string ClientIpAddress = ClientInfoProvider.ClientIpAddress == "::1" ? "127.0.0.1" : ClientInfoProvider.ClientIpAddress;

                                    List<ExternalAccessOutput> iplst = new List<ExternalAccessOutput>();
                                    iplst = _setupdalRepository.GetExternalAccess();
                                    bool isInRange = false;

                                    if (staff.AnywhereAccess.HasValue)
                                    {
                                        if (staff.AnywhereAccess.Value == 1)
                                        {
                                           
                                        }
                                        else
                                        {
                                            if (iplst.Count == 0)
                                            {
                                                throw new UserFriendlyException("ClientIpAddress range not found in the database.");
                                            }
                                            else
                                            {
                                                foreach (ExternalAccessOutput dr in iplst)
                                                {
                                                    bool checkInRange = IsIpInRange(ClientIpAddress, dr.Start, dr.END);
                                                    isInRange = (checkInRange || isInRange);
                                                }
                                            }
                                            if (!isInRange)
                                            {
                                                throw new UserFriendlyException("You are not authorised to sign in from this location");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (iplst.Count == 0)
                                        {
                                            throw new UserFriendlyException("ClientIpAddress range not found in the database.");
                                        }
                                        else
                                        {
                                            foreach (ExternalAccessOutput dr in iplst)
                                            {
                                                bool checkInRange = IsIpInRange(ClientIpAddress, dr.Start, dr.END);
                                                isInRange = (checkInRange || isInRange);
                                            }
                                        }
                                        if (!isInRange)
                                        {
                                            throw new UserFriendlyException("You are not authorised to sign in from this location");
                                        }
                                    }
                                  
                                 }
                                else
                                {
                                    throw new UserFriendlyException("Incorrect PIN number.");
                                }

                   
                            }
                            else
                            {
                                throw new UserFriendlyException("Incorrect PIN number.");
                            }
                        }

                        if (await UserManager.IsLockedOutAsync(TUsr))
                        {
                            return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                        }
                        if (!loggedInFromExternalSource)
                        {
                            if (!await UserManager.CheckPasswordAsync(TUsr, plainPassword))
                            {
                                if (shouldLockout)
                                {
                                    if (await TryLockOutAsync(tenantId, user.Id))
                                    {
                                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                                    }
                                }

                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidPassword, tenant, TUsr);
                            }
                        }

                        return await CreateLoginResultAsync(TUsr, tenant);
                    }
                    else
                    {
                        if (plainPassword.IsNullOrEmpty())
                        {
                            throw new ArgumentNullException(nameof(plainPassword));
                        }
                        else
                        {
                            var user = (from u in _userRepository.GetAll()
                                        where u.EmailAddress == userNameOrEmailAddress || u.UserName == userNameOrEmailAddress || u.PhoneNumber == userNameOrEmailAddress
                                        select u).SingleOrDefault();
                            var TUsr = user as TUser;

                            if (user == null)
                            {
                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                            }

                            if (await UserManager.IsLockedOutAsync(TUsr))
                            {
                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                            }
                            if (!loggedInFromExternalSource)
                            {
                                if (!await UserManager.CheckPasswordAsync(TUsr, plainPassword))
                                {
                                    if (shouldLockout)
                                    {
                                        if (await TryLockOutAsync(tenantId, user.Id))
                                        {
                                            return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                                        }
                                    }

                                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidPassword, tenant, TUsr);
                                }
                            }

                            return await CreateLoginResultAsync(TUsr, tenant);
                        }
                    }
                }

                else
                {
                    throw new ArgumentNullException(nameof(userNameOrEmailAddress));
                }
            }
        }

        protected virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsyncInternal_BK(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {



            //Get and check tenant
            TTenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await UserManager.InitializeOptionsAsync(tenantId);


                var loggedInFromExternalSource = true;
                if (!userNameOrEmailAddress.IsNullOrEmpty())
                {
                    bool pin = false;
                    pin = IsDigitsOnly(userNameOrEmailAddress);
                    if (pin)
                    {
                        string enpin = Base64Encryption(userNameOrEmailAddress);

                        var user = (from u in _userRepository.GetAll()
                                    where u.PIN == enpin
                                    select u).SingleOrDefault();
                        var TUsr = user as TUser;

                        if (user == null)
                        {
                            //return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                            throw new UserFriendlyException("Incorrect PIN number.");
                        }
                        else
                        {
                            var staff = (from st in _staffRepository.GetAll()
                                         where st.Id == user.StaffKey
                                         select st).SingleOrDefault();
                            if (staff != null)
                            {
                                if (!string.IsNullOrEmpty(staff.UserName) && (staff.MaidKey != null || staff.Sec_Supervisor == 10))
                                {
                                    if (staff.Active == 1)
                                    {

                                    }
                                    else
                                    {
                                        // return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserIsNotActive, tenant, TUsr);
                                        throw new UserFriendlyException("Your account is inactive.");
                                    }

                                    if (staff.AnywhereAccess.HasValue)
                                    {
                                        if (staff.AnywhereAccess.Value == 1)
                                        {

                                        }
                                        else
                                        {
                                            throw new UserFriendlyException("Invalid Anywhere Access.");
                                        }
                                    }
                                    else
                                    {
                                        throw new UserFriendlyException("Invalid Anywhere Access.");
                                    }
                                    #region ExternalAccessWithClientIpAddress
                                    string ClientIpAddress = ClientInfoProvider.ClientIpAddress == "::1" ? "127.0.0.1" : ClientInfoProvider.ClientIpAddress;

                                    List<ExternalAccessOutput> iplst = new List<ExternalAccessOutput>();
                                    iplst = _setupdalRepository.GetExternalAccess();
                                    bool isInRange = false;
                                    if (iplst.Count == 0)
                                    {
                                        throw new UserFriendlyException("ClientIpAddress range not found in the database.");
                                    }
                                    else
                                    {
                                        foreach (ExternalAccessOutput dr in iplst)
                                        {
                                            bool checkInRange = IsIpInRange(ClientIpAddress, dr.Start, dr.END);
                                            isInRange = (checkInRange || isInRange);
                                        }
                                    }
                                    //if (!isInRange)
                                    //{
                                    //    throw new UserFriendlyException("You are not authorised to sign in from this location");
                                    //}
                                    #endregion
                                }
                                else
                                {
                                    throw new UserFriendlyException("Incorrect PIN number.");
                                }


                            }
                            else
                            {
                                throw new UserFriendlyException("Incorrect PIN number.");
                            }
                        }

                        if (await UserManager.IsLockedOutAsync(TUsr))
                        {
                            return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                        }
                        if (!loggedInFromExternalSource)
                        {
                            if (!await UserManager.CheckPasswordAsync(TUsr, plainPassword))
                            {
                                if (shouldLockout)
                                {
                                    if (await TryLockOutAsync(tenantId, user.Id))
                                    {
                                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                                    }
                                }

                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidPassword, tenant, TUsr);
                            }
                        }

                        return await CreateLoginResultAsync(TUsr, tenant);
                    }
                    else
                    {
                        if (plainPassword.IsNullOrEmpty())
                        {
                            throw new ArgumentNullException(nameof(plainPassword));
                        }
                        else
                        {
                            var user = (from u in _userRepository.GetAll()
                                        where u.EmailAddress == userNameOrEmailAddress || u.UserName == userNameOrEmailAddress || u.PhoneNumber == userNameOrEmailAddress
                                        select u).SingleOrDefault();
                            var TUsr = user as TUser;

                            if (user == null)
                            {
                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                            }

                            if (await UserManager.IsLockedOutAsync(TUsr))
                            {
                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                            }
                            if (!loggedInFromExternalSource)
                            {
                                if (!await UserManager.CheckPasswordAsync(TUsr, plainPassword))
                                {
                                    if (shouldLockout)
                                    {
                                        if (await TryLockOutAsync(tenantId, user.Id))
                                        {
                                            return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, TUsr);
                                        }
                                    }

                                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidPassword, tenant, TUsr);
                                }
                            }

                            return await CreateLoginResultAsync(TUsr, tenant);
                        }
                    }
                }

                else
                {
                    throw new ArgumentNullException(nameof(userNameOrEmailAddress));
                }
            }
        }

        private bool IsIpInRange(string clientIpAddress, string start, string eND)
        {
            uint clientIpNumeric = IpToUInt32(clientIpAddress);
            uint startIpNumeric = IpToUInt32(start);
            uint endIpNumeric = IpToUInt32(eND);

            return clientIpNumeric >= startIpNumeric && clientIpNumeric <= endIpNumeric;
        }

        private uint IpToUInt32(string clientIpAddress)
        {
            return (uint)(IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(IPAddress.Parse(clientIpAddress).GetAddressBytes(), 0)));
        }
        #endregion
        private string Base64Encryption(string userNameOrEmailAddress)
        {
            if (userNameOrEmailAddress == null)
            {
                return null;
            }
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(userNameOrEmailAddress));
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        protected virtual async Task<AbpLoginResult<TTenant, TUser>> CreateLoginResultAsync(TUser user, TTenant tenant = null)
        {
            if (!user.IsActive)
            {
                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserIsNotActive);
            }

            if (await IsEmailConfirmationRequiredForLoginAsync(user.TenantId) && !user.IsEmailConfirmed)
            {
                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserEmailIsNotConfirmed);
            }

            if (await IsPhoneConfirmationRequiredForLoginAsync(user.TenantId) && !user.IsPhoneNumberConfirmed)
            {
                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserPhoneNumberIsNotConfirmed);
            }

            var principal = await _claimsPrincipalFactory.CreateAsync(user);

            return new AbpLoginResult<TTenant, TUser>(
                tenant,
                user,
                principal.Identity as ClaimsIdentity
            );
        }

        protected virtual async Task SaveLoginAttemptAsync(AbpLoginResult<TTenant, TUser> loginResult, string tenancyName, string userNameOrEmailAddress)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                var tenantId = loginResult.Tenant != null ? loginResult.Tenant.Id : (int?)null;
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var loginAttempt = new UserLoginAttempt
                    {
                        TenantId = tenantId,
                        TenancyName = tenancyName,

                        UserId = loginResult.User != null ? loginResult.User.Id : (long?)null,
                        UserNameOrEmailAddress = userNameOrEmailAddress,

                        Result = loginResult.Result,

                        BrowserInfo = ClientInfoProvider.BrowserInfo,
                        ClientIpAddress = ClientInfoProvider.ClientIpAddress,
                        ClientName = ClientInfoProvider.ComputerName,
                    };

                    await UserLoginAttemptRepository.InsertAsync(loginAttempt);
                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();
                }
            }
        }

        protected virtual void SaveLoginAttempt(AbpLoginResult<TTenant, TUser> loginResult, string tenancyName, string userNameOrEmailAddress)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                var tenantId = loginResult.Tenant != null ? loginResult.Tenant.Id : (int?)null;
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var loginAttempt = new UserLoginAttempt
                    {
                        TenantId = tenantId,
                        TenancyName = tenancyName,

                        UserId = loginResult.User != null ? loginResult.User.Id : (long?)null,
                        UserNameOrEmailAddress = userNameOrEmailAddress,

                        Result = loginResult.Result,

                        BrowserInfo = ClientInfoProvider.BrowserInfo,
                        ClientIpAddress = ClientInfoProvider.ClientIpAddress,
                        ClientName = ClientInfoProvider.ComputerName,
                    };

                    UserLoginAttemptRepository.Insert(loginAttempt);
                    UnitOfWorkManager.Current.SaveChanges();

                    uow.Complete();
                }
            }
        }

        protected virtual async Task<bool> TryLockOutAsync(int? tenantId, long userId)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var user = await UserManager.FindByIdAsync(userId.ToString());

                    (await UserManager.AccessFailedAsync(user)).CheckErrors();

                    var isLockOut = await UserManager.IsLockedOutAsync(user);

                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();

                    return isLockOut;
                }
            }
        }

        protected virtual async Task<bool> TryLoginFromExternalAuthenticationSourcesAsync(string userNameOrEmailAddress, string plainPassword, TTenant tenant)
        {
            if (!UserManagementConfig.ExternalAuthenticationSources.Any())
            {
                return false;
            }

            foreach (var sourceType in UserManagementConfig.ExternalAuthenticationSources)
            {
                using (var source = IocResolver.ResolveAsDisposable<IExternalAuthenticationSource<TTenant, TUser>>(sourceType))
                {
                    if (await source.Object.TryAuthenticateAsync(userNameOrEmailAddress, plainPassword, tenant))
                    {
                        var tenantId = tenant == null ? (int?)null : tenant.Id;
                        using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                        {
                            var user = await UserManager.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                            if (user == null)
                            {
                                user = await source.Object.CreateUserAsync(userNameOrEmailAddress, tenant);

                                user.TenantId = tenantId;
                                user.AuthenticationSource = source.Object.Name;
                                user.Password = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString("N").Left(16)); //Setting a random password since it will not be used
                                user.SetNormalizedNames();

                                if (user.Roles == null)
                                {
                                    user.Roles = new List<UserRole>();
                                    foreach (var defaultRole in RoleManager.Roles.Where(r => r.TenantId == tenantId && r.IsDefault).ToList())
                                    {
                                        user.Roles.Add(new UserRole(tenantId, user.Id, defaultRole.Id));
                                    }
                                }

                                await UserManager.CreateAsync(user);
                            }
                            else
                            {
                                await source.Object.UpdateUserAsync(user, tenant);

                                user.AuthenticationSource = source.Object.Name;

                                await UserManager.UpdateAsync(user);
                            }

                            await UnitOfWorkManager.Current.SaveChangesAsync();

                            return true;
                        }
                    }
                }
            }

            return false;
        }


        protected virtual async Task<TTenant> GetDefaultTenantAsync()
        {
            const string DefaultTenantName = "TEST";//"TCOC";//"HGCB";// "TCOH";//"HGCM";//"HGCAW";//"JCHGC";// "HCO";//"HGCH";// "HGCL";
            var tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == DefaultTenantName);//AbpTenant<TUser>.DefaultTenantName
            if (tenant == null)
            {
                throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
            }

            return tenant;
        }

        protected virtual TTenant GetDefaultTenant()
        {
            const string DefaultTenantName = "TEST";// "TEST";
            var tenant = TenantRepository.FirstOrDefault(t => t.TenancyName == DefaultTenantName);//AbpTenant<TUser>.DefaultTenantName
            if (tenant == null)
            {
                throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
            }

            return tenant;
        }

        protected virtual async Task<bool> IsEmailConfirmationRequiredForLoginAsync(int? tenantId)
        {
            if (tenantId.HasValue)
            {
                return await SettingManager.GetSettingValueForTenantAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, tenantId.Value);
            }

            return await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
        }

        protected virtual bool IsEmailConfirmationRequiredForLogin(int? tenantId)
        {
            if (tenantId.HasValue)
            {
                return SettingManager.GetSettingValueForTenant<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, tenantId.Value);
            }

            return SettingManager.GetSettingValueForApplication<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
        }

        protected virtual Task<bool> IsPhoneConfirmationRequiredForLoginAsync(int? tenantId)
        {
            return Task.FromResult(false);
        }

        protected virtual bool IsPhoneConfirmationRequiredForLogin(int? tenantId)
        {
            return false;
        }
    }
}
