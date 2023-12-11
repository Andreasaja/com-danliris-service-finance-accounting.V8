using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingExpedition
{
    public class GarmentPurchasingExpeditionReportViewModel : BaseViewModel
    {
        public int? InternalNoteId { get; set; }
        public string? InternalNoteNo { get; set; }
        public DateTimeOffset InternalNoteDate { get; set; }
        public DateTimeOffset InternalNoteDueDate { get; set; }
        public int?  SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public double? DPP { get; set; }
        public double? VAT { get; set; }
        public double? CorrectionAmount { get; set; }
        public double? IncomeTax { get; set; }
        public double? TotalPaid { get; set; }
        public int? CurrencyId { get; set; }
        public string? CurrencyCode { get; set; }
        public string? PaymentType { get; set; }
        public string? InvoicesNo { get; set; }
        public string? PaymentMethod { get; set; }
        public int? PaymentDueDays { get; set; }
        public string? Remark { get; set; }
        public GarmentPurchasingExpeditionPosition Position { get; set; }

        public DateTimeOffset? SendToVerificationDate { get; set; }
        public string? SendToVerificationBy { get; set; }
        public DateTimeOffset? VerificationAcceptedDate { get; set; }
        public string? VerificationAcceptedBy { get; set; }
        public DateTimeOffset? SendToCashierDate { get; set; }
        public string? SendToCashierBy { get; set; }
        public DateTimeOffset? CashierAcceptedDate { get; set; }
        public string? CashierAcceptedBy { get; set; }
        public DateTimeOffset? SendToPurchasingDate { get; set; }
        public string? SendToPurchasingBy { get; set; }
        public string? SendToPurchasingRemark { get; set; }
        public DateTimeOffset? SendToAccountingDate { get; set; }
        public string? SendToAccountingBy { get; set; }
        public DateTimeOffset? AccountingAcceptedDate { get; set; }
        public string? AccountingAcceptedBy { get; set; }
        public DateTimeOffset? VerificationSendDate { get; set; }
        public string? BankExpenditureNoteNo { get; set; }
        public DateTimeOffset? BankExpenditureNoteDate { get; set; }
        //public GarmentPurchasingExpeditionReportViewModel(GarmentPurchasingExpeditionModel model)
        //{
        //    InternalNoteId = model.InternalNoteId;
        //    InternalNoteNo = model.InternalNoteNo;
        //    InternalNoteDate = model.InternalNoteDate;
        //    InternalNoteDueDate = model.InternalNoteDueDate;
        //    SupplierId = model.SupplierId;
        //    SupplierName = model.SupplierName;
        //    DPP = model.DPP;
        //    VAT = model.VAT;
        //    CorrectionAmount = model.CorrectionAmount;
        //    IncomeTax = model.IncomeTax;
        //    TotalPaid = model.TotalPaid;
        //    CurrencyId = model.CurrencyId;
        //    CurrencyCode = model.CurrencyCode;
        //    PaymentType = model.PaymentType;
        //    InvoicesNo = model.InvoicesNo;
        //    PaymentMethod = model.PaymentMethod;
        //    PaymentDueDays = model.PaymentDueDays;
        //    Remark = model.Remark;
        //    Position = model.Position;
        //    SendToVerificationDate = model.SendToVerificationDate;
        //    SendToVerificationBy = model.SendToVerificationBy;
        //    VerificationAcceptedDate = model.VerificationAcceptedDate;
        //    VerificationAcceptedBy = model.VerificationAcceptedBy;
        //    SendToCashierDate = model.SendToCashierDate;
        //    SendToCashierBy = model.SendToCashierBy;
        //    CashierAcceptedDate = model.CashierAcceptedDate;
        //    CashierAcceptedBy = model.CashierAcceptedBy;
        //    SendToPurchasingDate = model.SendToPurchasingDate;
        //    SendToPurchasingBy = model.SendToPurchasingBy;
        //    SendToPurchasingRemark = model.SendToPurchasingRemark;
        //    SendToAccountingDate = model.SendToAccountingDate;
        //    SendToAccountingBy = model.SendToAccountingBy;
        //    AccountingAcceptedDate = model.AccountingAcceptedDate;
        //    AccountingAcceptedBy = model.AccountingAcceptedBy;

        //    if (Position == GarmentPurchasingExpeditionPosition.SendToCashier || Position == GarmentPurchasingExpeditionPosition.CashierAccepted || Position == GarmentPurchasingExpeditionPosition.DispositionPayment)
        //        VerificationSendDate = model.SendToCashierDate;
        //    else if (Position == GarmentPurchasingExpeditionPosition.SendToAccounting || Position == GarmentPurchasingExpeditionPosition.AccountingAccepted)
        //        VerificationSendDate = model.SendToAccountingDate;
        //    else if (Position == GarmentPurchasingExpeditionPosition.Purchasing || Position == GarmentPurchasingExpeditionPosition.SendToPurchasing)
        //        VerificationSendDate = model.SendToPurchasingDate;
        //}
    }
}
