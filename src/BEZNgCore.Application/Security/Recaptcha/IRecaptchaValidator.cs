using System.Threading.Tasks;

namespace BEZNgCore.Security.Recaptcha;

public interface IRecaptchaValidator
{
    Task ValidateAsync(string captchaResponse);
}
