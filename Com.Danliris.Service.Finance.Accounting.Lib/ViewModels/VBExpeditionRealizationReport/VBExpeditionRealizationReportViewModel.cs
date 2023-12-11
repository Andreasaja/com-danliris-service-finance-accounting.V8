using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBExpeditionRealizationReport
{
    public class VBExpeditionRealizationReportViewModel
    {
        public int Id { get; set; }
        public string RequestVBNo { get; set; }
        public string RealizationVBNo { get; set; }
        public string VbType { get; set; }
        public string Applicant { get; set; }
        public Unit Unit { get; set; }
        public Division Division { get; set; }
        public DateTimeOffset? DateUnitSend { get; set; }
        public string Usage { get; set; }
        public string RequestCurrency { get; set; }
        public string RealizationCurrency { get; set; }
        public decimal RequestAmount { get; set; }
        public decimal RealizationAmount { get; set; }
        public DateTimeOffset RealizationDate { get; set; }
        public DateTimeOffset? DateVerifReceive { get; set; }
        public string Verificator { get; set; }
        public DateTimeOffset DateVerifSend { get; set; }
        public string Status { get; set; }
        public string VerificationStatus { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset? DateCashierReceive { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public int Position { get; set; }
    }
}
