using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingExpedition
{
    public class GarmentPurchasingExpeditionModel : StandardEntity
    {
        public GarmentPurchasingExpeditionModel()
        {

        }

        public GarmentPurchasingExpeditionModel(int internalNoteId, string internalNoteNo, DateTimeOffset internalNoteDate, DateTimeOffset internalNoteDueDate, int supplierId, string supplierName, double vat, double incomeTax, double totalPaid, int currencyId, string currencyCode, string remark, double dpp, double correctionAmount, string paymentMethod, string paymentType, int paymentDueDays, string invoicesNo)
        {
            InternalNoteId = internalNoteId;
            InternalNoteNo = internalNoteNo;
            InternalNoteDate = internalNoteDate;
            InternalNoteDueDate = internalNoteDueDate;
            SupplierId = supplierId;
            SupplierName = supplierName;
            DPP = dpp;
            VAT = vat;
            CorrectionAmount = correctionAmount;
            IncomeTax = incomeTax;
            TotalPaid = totalPaid;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            Remark = remark;
            PaymentMethod = paymentMethod;
            PaymentDueDays = paymentDueDays;
            PaymentType = paymentType;
            InvoicesNo = invoicesNo;
        }

        public int InternalNoteId { get; private set; }
        [MaxLength(64)]
        public string? InternalNoteNo { get; private set; }
        public DateTimeOffset InternalNoteDate { get; private set; }
        public DateTimeOffset InternalNoteDueDate { get; private set; }
        public int SupplierId { get; private set; }
        [MaxLength(512)]
        public string? SupplierName { get; private set; }
        public double DPP { get; private set; }
        public double VAT { get; private set; }
        public double CorrectionAmount { get; private set; }
        public double IncomeTax { get; private set; }
        public double TotalPaid { get; private set; }
        public int CurrencyId { get; private set; }
        [MaxLength(16)]
        public string? CurrencyCode { get; private set; }
        [MaxLength(512)]
        public string? PaymentType { get; private set; }
        public string? InvoicesNo { get; private set; }
        [MaxLength(512)]
        public string? PaymentMethod { get; private set; }
        public int PaymentDueDays { get; private set; }
        public string? Remark { get; private set; }
        public GarmentPurchasingExpeditionPosition Position { get; private set; }

        public DateTimeOffset? SendToVerificationDate { get; private set; }
        [MaxLength(64)]
        public string? SendToVerificationBy { get; private set; }

        public DateTimeOffset? VerificationAcceptedDate { get; private set; }
        [MaxLength(64)]
        public string? VerificationAcceptedBy { get; private set; }

        public DateTimeOffset? SendToCashierDate { get; private set; }
        [MaxLength(64)]
        public string? SendToCashierBy { get; private set; }

        public DateTimeOffset? CashierAcceptedDate { get; private set; }
        [MaxLength(64)]
        public string? CashierAcceptedBy { get; private set; }

        public DateTimeOffset? SendToPurchasingDate { get; private set; }
        [MaxLength(64)]
        public string? SendToPurchasingBy { get; private set; }
        public string? SendToPurchasingRemark { get; private set; }

        public DateTimeOffset? SendToAccountingDate { get; private set; }
        [MaxLength(64)]
        public string? SendToAccountingBy { get; private set; }

        public DateTimeOffset? AccountingAcceptedDate { get; private set; }
        [MaxLength(64)]
        public string? AccountingAcceptedBy { get; private set; }

        public void SendToVerification(string username)
        {
            SendToVerificationBy = username;
            SendToVerificationDate = DateTimeOffset.Now;
            Position = GarmentPurchasingExpeditionPosition.SendToVerification;
        }

        public void SendToAccounting(string username)
        {
            SendToAccountingBy = username;
            SendToAccountingDate = DateTimeOffset.Now;
            Position = GarmentPurchasingExpeditionPosition.SendToAccounting;
        }

        public void SendToCashier(string username)
        {
            Position = GarmentPurchasingExpeditionPosition.SendToCashier;
            SendToCashierBy = username;
            SendToCashierDate = DateTimeOffset.Now;
        }

        public void SendToPurchasing(string username)
        {
            Position = GarmentPurchasingExpeditionPosition.Purchasing;
            SendToAccountingBy = null;
            SendToAccountingDate = null;
            SendToVerificationBy = null;
            SendToVerificationDate = null;
        }

        public void VerificationAccepted(string username)
        {
            VerificationAcceptedBy = username;
            VerificationAcceptedDate = DateTimeOffset.Now;
            Position = GarmentPurchasingExpeditionPosition.VerificationAccepted;
        }

        public void CashierAccepted(string username)
        {
            CashierAcceptedBy = username;
            CashierAcceptedDate = DateTimeOffset.Now;
            Position = GarmentPurchasingExpeditionPosition.CashierAccepted;
        }

        public void AccountingAccepted(string username)
        {
            AccountingAcceptedBy = username;
            AccountingAcceptedDate = DateTimeOffset.Now;
            Position = GarmentPurchasingExpeditionPosition.AccountingAccepted;
        }

        public void VoidVerification(string username)
        {
            VerificationAcceptedBy = null;
            VerificationAcceptedDate = null;
            Position = GarmentPurchasingExpeditionPosition.SendToVerification;
        }

        public void VoidCashier(string username)
        {
            SendToCashierBy = null;
            SendToCashierDate = null;
            CashierAcceptedBy = null;
            CashierAcceptedDate = null;
            Position = GarmentPurchasingExpeditionPosition.VerificationAccepted;
        }

        public void VoidAccounting(string username)
        {
            var now = DateTimeOffset.Now;
            if (SendToVerificationBy == null || SendToVerificationDate == null)
            {
                SendToVerificationBy = username;
                SendToVerificationDate = now;
                Position = GarmentPurchasingExpeditionPosition.SendToVerification;
                SendToAccountingBy = null;
                SendToVerificationDate = null;
                AccountingAcceptedBy = null;
                AccountingAcceptedDate = null;
            }
            else
            {
                Position = GarmentPurchasingExpeditionPosition.VerificationAccepted;
                SendToCashierBy = null;
                SendToCashierDate = null;
                CashierAcceptedBy = null;
                CashierAcceptedDate = null;
                SendToAccountingBy = null;
                SendToAccountingDate = null;
                AccountingAcceptedBy = null;
                AccountingAcceptedDate = null;
            }
        }

        public void SendToPurchasingRejected(string username, string remark)
        {
            Position = GarmentPurchasingExpeditionPosition.SendToPurchasing;
            SendToPurchasingBy = username;
            SendToPurchasingDate = DateTimeOffset.Now;
            SendToPurchasingRemark = remark;
        }

        public void PurchasingAccepted(string username)
        {
            Position = GarmentPurchasingExpeditionPosition.Purchasing;
        }
    }
}
