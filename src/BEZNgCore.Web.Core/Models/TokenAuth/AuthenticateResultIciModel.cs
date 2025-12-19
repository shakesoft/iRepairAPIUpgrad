using System;
using System.Collections.Generic;

namespace BEZNgCore.Web.Models.TokenAuth
{
    public class AuthenticateResultIciModel
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
        public Guid GuestKey { get; set; }
        public string UserName { get; set; }
        public string logintime { get; set; }

    }
}