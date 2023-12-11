using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote;
using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition
{
    public class ReportDto
    {
        public int VBRealizationId { get; set; }
        public int VBId { get; set; }
        public string VBNo { get;  set; }
        public string VBRealizationNo { get;  set; }
        public DateTimeOffset VBRealizationDate { get;  set; }
        public string VBRequestName { get;  set; }
        public int UnitId { get;  set; }
        public string UnitName { get;  set; }
        public int DivisionId { get;  set; }
        public string DivisionName { get;  set; }
        public decimal VBAmount { get;  set; }
        public decimal VBRealizationAmount { get;  set; }
        public string CurrencyCode { get;  set; }
        public double CurrencyRate { get;  set; }
        public string SendToVerificationBy { get;  set; }
        public DateTimeOffset? SendToVerificationDate { get;  set; }
        public string VerificationReceiptBy { get;  set; }
        public DateTimeOffset? VerificationReceiptDate { get;  set; }
        public string VerifiedToCashierBy { get;  set; }
        public DateTimeOffset? VerifiedToCashierDate { get;  set; }
        public string CashierReceiptBy { get;  set; }
        public DateTimeOffset? CashierReceiptDate { get;  set; }
        public string NotVerifiedReason { get;  set; }
        public string NotVerifiedBy { get;  set; }
        public DateTimeOffset? NotVerifiedDate { get;  set; }
        public VBRealizationPosition Position { get;  set; }
        public VBType VBType { get; set; }
        public string Purpose { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public string RemarkRealization { get; set; }
    }
}