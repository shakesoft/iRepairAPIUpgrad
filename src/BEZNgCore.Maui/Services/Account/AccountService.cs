using Abp.Dependency;
using BEZNgCore.ApiClient;
using BEZNgCore.ApiClient.Models;
using BEZNgCore.Maui.Core.Localization;
using BEZNgCore.Maui.Core.Threading;
using BEZNgCore.Maui.Services.Navigation;
using BEZNgCore.Maui.Services.Storage;
using BEZNgCore.Maui.Services.UI;
using BEZNgCore.Sessions;
using BEZNgCore.Sessions.Dto;

namespace BEZNgCore.Maui.Services.Account;

public class AccountService : IAccountService, ISingletonDependency
{
    private readonly IApplicationContext _applicationContext;
    private readonly ISessionAppService _sessionAppService;
    private readonly IAccessTokenManager _accessTokenManager;
    private readonly IDataStorageService _dataStorageService;
    private readonly INavigationService _navigationService;

    public AccountService(
        IApplicationContext applicationContext,
        ISessionAppService sessionAppService,
        IAccessTokenManager accessTokenManager,
        AbpAuthenticateModel abpAuthenticateModel,
        IDataStorageService dataStorageService,
        INavigationService navigationService
    )
    {
        _applicationContext = applicationContext;
        _sessionAppService = sessionAppService;
        _accessTokenManager = accessTokenManager;
        _dataStorageService = dataStorageService;
        AbpAuthenticateModel = abpAuthenticateModel;
        _navigationService = navigationService;
    }

    public AbpAuthenticateModel AbpAuthenticateModel { get; set; }
    public AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

    public async Task LoginUserAsync()
    {
        await WebRequestExecuter.Execute(_accessTokenManager.LoginAsync, AuthenticateSucceed, ex => Task.CompletedTask);
    }

    public Task LogoutAsync()
    {
        _accessTokenManager.Logout();
        _applicationContext.ClearLoginInfo();
        _dataStorageService.ClearSessionPersistance();
        return Task.CompletedTask;
    }

    private async Task AuthenticateSucceed(AbpAuthenticateResultModel result)
    {
        AuthenticateResultModel = result;

        if (AuthenticateResultModel.ShouldResetPassword)
        {
            await UserDialogsService.Instance.AlertError(L.Localize("LoginFailed") + " " + L.Localize("ChangePasswordToLogin"));
            return;
        }

        if (AuthenticateResultModel.RequiresTwoFactorVerification)
        {
            _navigationService.NavigateTo(NavigationUrlConsts.SendTwoFactorCode);
            return;
        }

        await _dataStorageService.StoreAuthenticateResultAsync(AuthenticateResultModel);

        AbpAuthenticateModel.Password = null;
        await SetCurrentUserInfoAsync();
        await UserConfigurationManager.GetAsync();
        _navigationService.NavigateTo(NavigationUrlConsts.Users);
    }

    private async Task SetCurrentUserInfoAsync()
    {
        await WebRequestExecuter.Execute(async () =>
            await _sessionAppService.GetCurrentLoginInformations(), GetCurrentUserInfoExecuted);
    }

    private async Task GetCurrentUserInfoExecuted(GetCurrentLoginInformationsOutput result)
    {
        _applicationContext.SetLoginInfo(result);

        await _dataStorageService.StoreLoginInformationAsync(_applicationContext.LoginInfo);
    }
}