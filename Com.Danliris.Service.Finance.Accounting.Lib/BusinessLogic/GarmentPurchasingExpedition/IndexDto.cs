using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingExpedition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingExpedition
{
    public class IndexDto
    {
        public IndexDto(int id, string internalNoteNo, DateTimeOffset internalNoteDate, DateTimeOffset internalNoteDueDate, string supplierName, double amount, string currencyCode, string remark, GarmentPurchasingExpeditionPosition position)
        {
            Id = id;
            InternalNoteNo = internalNoteNo;
            InternalNoteDate = internalNoteDate;
            InternalNoteDueDate = internalNoteDueDate;
            SupplierName = supplierName;
            Amount = amount;
            CurrencyCode = currencyCode;
            Remark = remark;
            Status = position.ToDescriptionString();
        }

        public IndexDto(int id, DateTimeOffset? verificationAcceptedDate, string internalNoteNo, DateTimeOffset internalNoteDate, DateTimeOffset internalNoteDueDate, string supplierName, double amount, string currencyCode)
        {
            Id = id;
            InternalNoteNo = internalNoteNo;
            InternalNoteDate = internalNoteDate;
            InternalNoteDueDate = internalNoteDueDate;
            SupplierName = supplierName;
            Amount = amount;
            CurrencyCode = currencyCode;
            VerificationAcceptedDate = verificationAcceptedDate;
        }

        public IndexDto(GarmentPurchasingExpeditionModel entity)
        {
            Id = entity.Id;
            InternalNoteNo = entity.InternalNoteNo;
            InternalNoteDate = entity.InternalNoteDate;
            InternalNoteDueDate = entity.InternalNoteDueDate;
            InternalNoteId = entity.InternalNoteId;
            SupplierName = entity.SupplierName;
            Amount = entity.TotalPaid;
            CurrencyCode = entity.CurrencyCode;
            VerificationAcceptedDate = entity.VerificationAcceptedDate;
            SendToPurchasingRemark = entity.SendToPurchasingRemark;
            Remark = entity.Remark;
            AmountDPP = entity.DPP;
            VAT = entity.VAT;
            CorrectionAmount = entity.CorrectionAmount;
            IncomeTax = entity.IncomeTax;
            Status = entity.Position.ToDescriptionString();
            Date = entity.Position == GarmentPurchasingExpeditionPosition.SendToAccounting ? entity.SendToAccountingDate : entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier ? entity.SendToCashierDate : entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing ? entity.SendToPurchasingDate : entity.VerificationAcceptedDate;
            VerifiedBy = entity.Position == GarmentPurchasingExpeditionPosition.SendToAccounting ? entity.SendToAccountingBy : entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier ? entity.SendToCashierBy : entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing ? entity.SendToPurchasingBy : entity.VerificationAcceptedBy;
            SentDate = entity.Position == GarmentPurchasingExpeditionPosition.SendToAccounting || entity.Position == GarmentPurchasingExpeditionPosition.AccountingAccepted ? entity.SendToAccountingDate : entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier || entity.Position == GarmentPurchasingExpeditionPosition.CashierAccepted ? entity.SendToCashierDate : entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing ? entity.SendToPurchasingDate : entity.SendToVerificationDate;
            AcceptedDate = entity.Position == GarmentPurchasingExpeditionPosition.AccountingAccepted ? entity.AccountingAcceptedDate : entity.Position == GarmentPurchasingExpeditionPosition.CashierAccepted ? entity.CashierAcceptedDate : entity.Position == GarmentPurchasingExpeditionPosition.VerificationAccepted ? entity.VerificationAcceptedDate : null;
            PaymentType = entity.PaymentType;
        }

        public int Id { get; private set; }
        public string InternalNoteNo { get; private set; }
        public DateTimeOffset InternalNoteDate { get; private set; }
        public DateTimeOffset InternalNoteDueDate { get; private set; }
        public int InternalNoteId { get; private set; }
        public string SupplierName { get; private set; }
        public double Amount { get; private set; }
        public string CurrencyCode { get; private set; }
        public DateTimeOffset? VerificationAcceptedDate { get; private set; }
        public string Remark { get; private set; }
        public double AmountDPP { get; private set; }
        public double VAT { get; private set; }
        public double CorrectionAmount { get; private set; }
        public double IncomeTax { get; private set; }
        public string Status { get; private set; }
        public string SendToPurchasingRemark { get; private set; }
        public DateTimeOffset? Date { get; private set; }
        public string VerifiedBy { get; private set; }
        public DateTimeOffset? SentDate { get; private set; }
        public DateTimeOffset? AcceptedDate { get; private set; }
        public string PaymentType { get; private set; }
    }
}
