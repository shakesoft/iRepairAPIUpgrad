using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class ReportMenuAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<ReportBatch, Guid> _reportbatchRepository;
        private readonly IRepository<Report, Guid> _reportRepository;
        private readonly IRepository<Staff, Guid> _staffRepository;
        private readonly IRepository<ReportSecurity, Guid> _reportsecurityRepository;
        public ReportMenuAppService(
            IRepository<ReportBatch, Guid> reportbatchRepository,
            IRepository<Report, Guid> reportRepository,
            IRepository<Staff, Guid> staffRepository,
            IRepository<ReportSecurity, Guid> reportsecurityRepository)
        {
            _reportbatchRepository = reportbatchRepository;
            _reportRepository = reportRepository;
            _staffRepository = staffRepository;
            _reportsecurityRepository = reportsecurityRepository;
        }
        [HttpGet]
        public async Task<ListResultDto<ReportViewData>> GetReportViewData()
        {
            List<ReportViewData> Alllst = new List<ReportViewData>();
            ReportViewData a = new ReportViewData();

            List<BatchReport> Allbatchlst = new List<BatchReport>();
            BatchReport b = new BatchReport();
            b.ReportOutputs = _reportbatchRepository.GetAll().OrderBy(s => s.Sort)
                                       .Select(s => new ReportOutput
                                       {
                                           ReportKey = s.Id,
                                           Caption = s.BatchName,
                                           RptName = s.Report1
                                       })
                                       .ToList();
            Allbatchlst.Add(b);
            a.BatchReports = Allbatchlst;
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                // var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                int admin = _staffRepository.GetAll().Where(x => x.Active == 1 && x.Id == user.StaffKey).Select(x => x.Administrator).FirstOrDefault().Value;
                if (admin > 0)
                {
                    a.ParrentReports = _reportRepository.GetAll().Where(x => x.ParentReportKey == null).OrderBy(x => x.Sort)
                        .Select(x => new ParrentReport
                        {
                            ReportKey = x.Id,
                            Caption = x.Caption,
                            RptName = x.RptName,
                            ReportOutputs = _reportRepository.GetAll().Where(c => c.ParentReportKey == x.Id).OrderBy(c => c.Sort)
                            .Select(c => new ReportOutput
                            {
                                ReportKey = c.Id,
                                Caption = c.Caption,
                                RptName = c.RptName
                            }).ToList()
                        }).ToList();

                }
                else
                {
                    a.ParrentReports = (from rp in _reportRepository.GetAll()
                                        join rps in _reportsecurityRepository.GetAll() on rp.Id equals rps.ReportKey
                                        where rps.Sec_Report == 10 && rps.StaffKey == user.StaffKey
                                        select new ParrentReport
                                        {
                                            ReportKey = rp.Id,
                                            Caption = rp.Caption,
                                            RptName = rp.RptName,
                                            ReportOutputs = _reportRepository.GetAll().Where(c => c.ParentReportKey == rp.Id).OrderBy(c => c.Sort)
                                               .Select(c => new ReportOutput
                                               {
                                                   ReportKey = c.Id,
                                                   Caption = c.Caption,
                                                   RptName = c.RptName
                                               }).ToList()
                                        }).ToList();
                }
                Alllst.Add(a);
                return new ListResultDto<ReportViewData>(Alllst);
            }
        }
        public ListResultDto<ReportBatchOutput> LoadBatchReport()
        {
            var r = _reportbatchRepository.GetAll().OrderBy(s => s.Sort)
                                       .Select(s => new ReportBatchOutput
                                       {
                                           ReportBatchKey = s.Id,
                                           Report1 = s.Report1,
                                           BatchName = s.BatchName
                                       })
                                       .ToList();
            return new ListResultDto<ReportBatchOutput>(r);
        }
        public ListResultDto<ReportOutput> loadChildReport(string parrentKey)
        {
            Guid pkey = new Guid(parrentKey);
            var r = _reportRepository.GetAll().Where(s => s.ParentReportKey == pkey).OrderBy(s => s.Sort)
                                       .Select(s => new ReportOutput
                                       {
                                           ReportKey = s.Id,
                                           Caption = s.Caption,
                                           RptName = s.RptName
                                       })
                                       .ToList();
            return new ListResultDto<ReportOutput>(r);
        }
    }
}
