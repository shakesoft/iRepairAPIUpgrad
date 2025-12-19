using System.Threading.Tasks;
using Abp.Dependency;

namespace BEZNgCore.MultiTenancy.Accounting;

public interface IInvoiceNumberGenerator : ITransientDependency
{
    Task<string> GetNewInvoiceNumber();
}

