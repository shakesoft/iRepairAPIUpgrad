using System;
using System.Collections.Generic;
using System.Linq;
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
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using BEZNgCore.Authorization.Users;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BEZNgCore.Authorization
{
    public class AbpLoginManagerIcheckIn<TTenant, TRole, TUser> : ITransientDependency
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
        //private readonly IRepository<Room, Guid> _roomRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Reservation, Guid> _reservationRepository;
        private readonly IRepository<Guest, Guid> _guestRepository;
        public AbpLoginManagerIcheckIn(
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
            //IRepository<Room, Guid> roomRepository, 
            IRepository<User, long> userRepository,
            IRepository<Reservation, Guid> reservationRepository,
            IRepository<Guest, Guid> guestRepository
            )
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
            _reservationRepository = reservationRepository;
            _guestRepository = guestRepository;

            ClientInfoProvider = NullClientInfoProvider.Instance;
            //_roomRepository = roomRepository;
        }
        #region icheckin
        
        [UnitOfWork]
        public virtual async Task<AbpLoginResult<TTenant, TUser>> LoginIcheckInAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null, bool shouldLockout = true)
        {
            var result = await LoginAsyncInternalIcheckIn(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
            await SaveLoginAttemptAsync(result, tenancyName, userNameOrEmailAddress);
            return result;
        }
        protected virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsyncInternalIcheckIn(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {

            ///Get and check tenant
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

                if (userNameOrEmailAddress.IsNullOrEmpty())
                {
                    throw new ArgumentNullException(nameof(userNameOrEmailAddress));
                }
                else if (plainPassword.IsNullOrEmpty())
                {
                    throw new ArgumentNullException(nameof(plainPassword));
                }
                else
                {
                    List<dynamic> query = (from r in _reservationRepository.GetAll()
                                           join g in _guestRepository.GetAll() on r.GuestKey equals g.Id
                                           where (r.Status == 1 || r.Status == 2) && r.DocNo == plainPassword && g.EMail == userNameOrEmailAddress
                                           select new
                                           {
                                              // DocNo = r.DocNo,
                                              // ReservationKey = r.Id,
                                               GuestKey = g.Id,
                                               FirstName =g.FirstName,
                                               LastName=g.LastName
                                              // GuestLoginTime = DateTime.Now.ToString("MM / dd / yyyy h: mm tt")
                                           }).ToList<dynamic>();
                    if (query.Count > 0)
                    {
                        
                        if (_userRepository.GetAll().Where(x => x.EmailAddress == userNameOrEmailAddress && (x.PIN == plainPassword)).Count() == 0)
                        {
                            User u = new User();
                            u.AccessFailedCount = 0;
                            u.AuthenticationSource = null;
                            u.ConcurrencyStamp = null;
                            u.CreationTime = DateTime.Now;
                            u.CreatorUserId = null;
                            u.DeleterUserId = null;
                            u.DeletionTime = null;
                            u.EmailAddress = userNameOrEmailAddress;
                            u.EmailConfirmationCode = null;
                            u.IsActive = true;
                            u.IsDeleted = false;
                            u.IsEmailConfirmed = true;
                            u.IsLockoutEnabled = true;
                            u.IsPhoneNumberConfirmed = false;
                            u.IsTwoFactorEnabled = true;
                            u.LastModificationTime = null;
                            u.LastModifierUserId = null;
                            u.LockoutEndDateUtc = null;
                            u.Name = query[0].FirstName;
                            u.NormalizedEmailAddress = userNameOrEmailAddress;
                            u.NormalizedUserName = query[0].FirstName + " " + query[0].LastName;
                            u.Password = "AQAAAAEAACcQAAAAEJXLncV5WokLtv+TqyV7qXlGlos6wAK52lZ9j+BPTO8B8TKa7MlLElz8jzI6SpYcTw==";
                            u.PasswordResetCode = null;
                            u.PhoneNumber = null;
                            u.ProfilePictureId = null;
                            u.SecurityStamp = Guid.NewGuid().ToString();
                            u.ShouldChangePasswordOnNextLogin = false;
                            u.Surname = query[0].LastName;
                            u.TenantId = tenant.Id;
                            u.UserName = query[0].FirstName + " " + query[0].LastName; ;
                            u.SignInToken = null;
                            u.SignInTokenExpireTimeUtc = null;
                            u.GoogleAuthenticatorKey = null;
                            u.PIN = plainPassword;
                            u.StaffKey = query[0].GuestKey;
                            int success = InsertUserAsync(u, tenant.Id).Result;

                        }
                        var user = (from us in _userRepository.GetAll()
                                    where us.EmailAddress == userNameOrEmailAddress && us.PIN == plainPassword
                                    select us).SingleOrDefault();
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
                    else
                    {
                        throw new UserFriendlyException("Please input the correct Guest Email and Booking Number.");
                    }
                }
               
            }
        }

        private async Task<int> InsertUserAsync(User u, int tenantId)
        {
            int succ = 0;
            try
            {
                
                using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                {
                   
                    using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                       
                        await _userRepository.InsertAsync(u);
                        await UnitOfWorkManager.Current.SaveChangesAsync();
                        await uow.CompleteAsync();
                        succ = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg= ex.Message;
            }
            return succ;
        }
        #endregion

        #region common
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

        protected virtual async Task<TTenant> GetDefaultTenantAsync()
        {
            const string DefaultTenantName = "Default";// "TEST"
            var tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == DefaultTenantName);//AbpTenant<TUser>.DefaultTenantName
            if (tenant == null)
            {
                throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
            }

            return tenant;
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
        protected virtual async Task<bool> IsEmailConfirmationRequiredForLoginAsync(int? tenantId)
        {
            if (tenantId.HasValue)
            {
                return await SettingManager.GetSettingValueForTenantAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, tenantId.Value);
            }

            return await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
        }
        protected virtual Task<bool> IsPhoneConfirmationRequiredForLoginAsync(int? tenantId)
        {
            return Task.FromResult(false);
        }
        #endregion

    }
}
