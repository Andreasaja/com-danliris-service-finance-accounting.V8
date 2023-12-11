using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBStatusReport
{
    public class VBStatusReportViewModel
    {
        //public long Id { get; set; }
        //public string VBNo { get; set; }
        //public DateTimeOffset Date { get; set; }
        //public DateTimeOffset DateEstimate { get; set; }
        //public Unit Unit { get; set; }
        //public string CreateBy { get; set; }
        //public string RealizationNo { get; set; }
        //public DateTimeOffset? RealizationDate { get; set; }
        //public string Usage { get; set; }
        //public int Aging { get; set; }
        //public decimal Amount { get; set; }
        //public decimal RealizationAmount { get; set; }
        //public decimal Difference { get; set; }
        //public string Status { get; set; }
        //public DateTime LastModifiedUtc { get; set; }
        //public virtual ICollection<RealizationVbDetailModel> Details { get; set; }


        public long Id { get; set; }
        public string VBNo { get; set; }
        public string Date { get; set; }
        public string DateEstimate { get; set; }
        public Unit Unit { get; set; }
        public string CreateBy { get; set; }
        public string RealizationNo { get; set; }
        public string RealizationDate { get; set; }
        public string Usage { get; set; }
        public int Aging { get; set; }
        public decimal Amount { get; set; }
        public decimal RealizationAmount { get; set; }
        public decimal Difference { get; set; }
        public string Status { get; set; }
        public string LastModifiedUtc { get; set; }
        public string ApprovalDate { get; set; }
        public string ClearenceDate { get; set; }
        public virtual ICollection<RealizationVbDetailModel> Details { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsInklaring { get; internal set; }
        public string NoBL { get; internal set; }
    }
}
