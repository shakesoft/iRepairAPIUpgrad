using System.Collections.Generic;

namespace BEZNgCore.Authorization.Users.Profile.Dto;

public class UpdateGoogleAuthenticatorKeyInput
{
    public string GoogleAuthenticatorKey { get; set; }
    public string AuthenticatorCode { get; set; }
}

