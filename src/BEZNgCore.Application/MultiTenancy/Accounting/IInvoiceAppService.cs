using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using BEZNgCore.MultiTenancy.Accounting.Dto;

namespace BEZNgCore.MultiTenancy.Accounting;

public interface IInvoiceAppService
{
    Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

    Task CreateInvoice(CreateInvoiceDto input);
}
