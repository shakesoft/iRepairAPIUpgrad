using Abp.Application.Services;
using BEZNgCore.Tenants.Dashboard.Dto;

namespace BEZNgCore.Tenants.Dashboard;

public interface ITenantDashboardAppService : IApplicationService
{
    GetMemberActivityOutput GetMemberActivity();

    GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input);

    GetDailySalesOutput GetDailySales();

    GetProfitShareOutput GetProfitShare();

    GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input);

    GetTopStatsOutput GetTopStats();

    GetRegionalStatsOutput GetRegionalStats();

    GetGeneralStatsOutput GetGeneralStats();
}

