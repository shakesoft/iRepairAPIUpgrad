using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BEZNgCore.MultiTenancy.HostDashboard.Dto;

namespace BEZNgCore.MultiTenancy.HostDashboard;

public interface IIncomeStatisticsService
{
    Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
        ChartDateInterval dateInterval);
}
