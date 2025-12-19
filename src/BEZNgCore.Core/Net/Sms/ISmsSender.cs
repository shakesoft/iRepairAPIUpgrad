using System.Threading.Tasks;

namespace BEZNgCore.Net.Sms;

public interface ISmsSender
{
    Task SendAsync(string number, string message);
}

