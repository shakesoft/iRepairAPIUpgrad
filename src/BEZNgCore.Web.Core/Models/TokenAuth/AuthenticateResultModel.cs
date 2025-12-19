using System;
using System.Collections.Generic;

namespace BEZNgCore.Web.Models.TokenAuth;

public class AuthenticateResultModel
{
    public string AccessToken { get; set; }

    public string EncryptedAccessToken { get; set; }

    public int ExpireInSeconds { get; set; }

    public bool ShouldResetPassword { get; set; }

    public string PasswordResetCode { get; set; }

    public long UserId { get; set; }

    public bool RequiresTwoFactorVerification { get; set; }

    public IList<string> TwoFactorAuthProviders { get; set; }

    public string TwoFactorRememberClientToken { get; set; }

    public string ReturnUrl { get; set; }

    public string RefreshToken { get; set; }

    public int RefreshTokenExpireInSeconds { get; set; }
    public string c { get; set; }

    public Guid StaffKey { get; set; }
    public string UserName { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsTechSupervisor { get; set; }
    public bool IsBlockRoom { get; set; }
    public int refreshInterval { get; set; }

    public bool SecIPSetUp { get; set; }
    public bool SecIPViewLog { get; set; }
    public bool SecIPAssignTasks { get; set; }
    public bool SecIPBlockRoom { get; set; }
    public string apilatestupdate { get; set; } = "08/07/2025";
    public bool SecSupervisorB { get; set; }
    public bool SecSupervisorMode { get; set; }
    public bool SecRooms { get; set; }
    public bool SecMiniBar { get; set; }
    public bool SecMiniBarCo { get; set; }
    public bool SecLaundry { get; set; }
    public bool SecLostFound { get; set; }
    public bool SecWOEntry { get; set; }
    public bool SecViewLogs { get; set; }
    public bool SecRoomstoInspect { get; set; }
    public bool SecGuestRequest { get; set; }
    public bool ChangeCleanStatus { get; set; }
}

