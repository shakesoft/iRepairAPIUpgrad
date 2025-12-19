using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BEZNgCore.Web.Authentication.JwtBearer;

public class AsyncJwtBearerOptions : JwtBearerOptions
{
    public readonly List<IAsyncSecurityTokenValidator> AsyncSecurityTokenValidators;

    private readonly BEZNgCoreAsyncJwtSecurityTokenHandler _defaultAsyncHandler = new BEZNgCoreAsyncJwtSecurityTokenHandler();

    public AsyncJwtBearerOptions()
    {
        AsyncSecurityTokenValidators = new List<IAsyncSecurityTokenValidator>() { _defaultAsyncHandler };
    }
}


