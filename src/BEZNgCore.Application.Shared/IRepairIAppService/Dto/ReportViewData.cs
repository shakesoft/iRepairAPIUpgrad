using System;
using System.Collections.Generic;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class ReportViewData
    {
        public ReportViewData()
        {

            BatchReports = new HashSet<BatchReport>();
            ParrentReports = new HashSet<ParrentReport>();
        }

        public ICollection<BatchReport> BatchReports { get; set; }
        public ICollection<ParrentReport> ParrentReports { get; set; }
    }
    public class BatchReport
    {
        public BatchReport()
        {

            ReportOutputs = new HashSet<ReportOutput>();
        }

        public ICollection<ReportOutput> ReportOutputs { get; set; }
    }
    public class ParrentReport
    {
        public ParrentReport()
        {

            ReportOutputs = new HashSet<ReportOutput>();
        }
        public Guid ReportKey { get; set; }
        public string Caption { get; set; }
        public string RptName { get; set; }
        public ICollection<ReportOutput> ReportOutputs { get; set; }
    }
    public class ReportOutput
    {
        public Guid ReportKey { get; set; }
        public string Caption { get; set; }
        public string RptName { get; set; }

    }
    public class ReportBatchOutput
    {
        public Guid ReportBatchKey { get; set; }
        public string BatchName { get; set; }
        public string Report1 { get; set; }
    }
}
