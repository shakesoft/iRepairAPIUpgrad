using System.Threading.Tasks;
using BEZNgCore.Security.Recaptcha;

namespace BEZNgCore.Test.Base.Web;

public class FakeRecaptchaValidator : IRecaptchaValidator
{
    public Task ValidateAsync(string captchaResponse)
    {
        return Task.CompletedTask;
    }
}
