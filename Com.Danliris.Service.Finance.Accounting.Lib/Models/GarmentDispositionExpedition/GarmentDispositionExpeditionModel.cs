using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDispositionExpedition
{
    public class GarmentDispositionExpeditionModel : StandardEntity
    {
        public GarmentDispositionExpeditionModel()
        {

        }

        public GarmentDispositionExpeditionModel(int dispositionNoteId, string dispositionNoteNo, DateTimeOffset dispositionNotedate, DateTimeOffset dispositionNoteDueDate, int supplierId, string supplierName, double vatAmount, double currencyVATAmount, double incomeTax, double currencyIncomeTaxAmount, double totalPaid, double currencyTotalPaid, int currencyId, string currencyCode, string remark, double dppAmount, double currencyDPPAmount, string supplierCode, double currencyRate, string proformaNo,string category)
        {
            DispositionNoteId = dispositionNoteId;
            DispositionNoteNo = dispositionNoteNo;
            DispositionNoteDate = dispositionNotedate;
            DispositionNoteDueDate = dispositionNoteDueDate;
            SupplierId = supplierId;
            SupplierName = supplierName;
            DPPAmount = dppAmount;
            CurrencyDPPAmount = currencyDPPAmount;
            VATAmount = vatAmount;
            CurrencyVATAmount = currencyVATAmount;
            IncomeTaxAmount = incomeTax;
            CurrencyIncomeTaxAmount = currencyIncomeTaxAmount;
            TotalPaid = totalPaid;
            CurrencyTotalPaid = currencyTotalPaid;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            Remark = remark;
            SupplierCode = supplierCode;
            CurrencyRate = currencyRate;
            ProformaNo = proformaNo;
            Category = category;
        }

        public GarmentDispositionExpeditionModel(int id, string dispositionNoteNo,
            DateTimeOffset dispositionNoteDate,
            DateTimeOffset dispositionNoteDueDate,
            int dispositionNoteId,
            double currencyTotalPaid,
            double totalPaid,
            int currencyId,
            string currencyCode,
            string suppliername,
            string remark,
            string proformaNo,
            string createdBy,
            double currencyRate,
            int supplierId,
            string supplierCode,
            double vatAmount,
            double currencyVatAmount,
            double incomeTaxAmount,
            double currencyIncomeTaxAmount,
            double dppAmount,
            double currencyDppAmount,
            DateTimeOffset verifiedDateSend,
            DateTimeOffset verifiedDateReceived,
            string sendToPurchasingRemark,
            DateTime createdDate)
        {
            Id = id;
            DispositionNoteNo = dispositionNoteNo;
            DispositionNoteDate = dispositionNoteDate;
            DispositionNoteDueDate = dispositionNoteDueDate;
            DispositionNoteId = dispositionNoteId;
            CurrencyTotalPaid = currencyTotalPaid;
            TotalPaid = totalPaid;
            CurrencyId = currencyId;
            CurrencyCode = currencyCode;
            SupplierName = suppliername;
            Remark = remark;
            SendToVerificationBy = createdBy;
            CreatedBy = createdBy;
            ProformaNo = proformaNo;
            CurrencyRate = currencyRate;
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            VATAmount = vatAmount;
            CurrencyVATAmount = currencyVatAmount;
            IncomeTaxAmount = incomeTaxAmount;
            CurrencyIncomeTaxAmount = currencyIncomeTaxAmount;
            DPPAmount = dppAmount;
            CurrencyDPPAmount = currencyDppAmount;
            VerifiedDateSend = verifiedDateSend;
            VerifiedDateReceived = verifiedDateReceived;
            SendToPurchasingRemark = sendToPurchasingRemark;
            CreatedUtc = createdDate;
        }

        public int DispositionNoteId { get; private set; }
        [MaxLength(64)]
        public string? DispositionNoteNo { get; private set; }
        public DateTimeOffset DispositionNoteDate { get; private set; }
        public DateTimeOffset DispositionNoteDueDate { get; private set; }
        public int SupplierId { get; private set; }
        [MaxLength(512)]
        public string? SupplierName { get; private set; }
        [MaxLength(128)]
        public string? SupplierCode { get; private set; }
        public double DPPAmount { get; private set; }
        public double CurrencyDPPAmount { get; private set; }
        public double VATAmount { get; private set; }
        public double CurrencyVATAmount { get; private set; }
        public double IncomeTaxAmount { get; private set; }
        public double CurrencyIncomeTaxAmount { get; private set; }
        public double TotalPaid { get; private set; }
        public double CurrencyTotalPaid { get; private set; }
        public int CurrencyId { get; private set; }
        [MaxLength(16)]
        public string? CurrencyCode { get; private set; }
        public double CurrencyRate { get; private set; }
        public string? Remark { get; private set; }
        public string? ProformaNo { get; set; }
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
        /// <summary>
        /// Tanggal pembelian Kirim
        /// </summary>
        public DateTimeOffset VerifiedDateSend { get; private set; }
        /// <summary>
        /// Tanggal Pembelian Terima
        /// </summary>
        public DateTimeOffset VerifiedDateReceived { get; private set; }
        [MaxLength(64)]
        public string? VerifiedBy { get; private set; }
        public DateTimeOffset? VerifiedDate { get; private set; }

        public bool IsPaid { get; set; }
        public string? BankExpenditureNoteNo { get; set; }
        public DateTimeOffset BankExpenditureNoteDate { get; set; }
        public string? Category { get; set; }

        public void SendToVerification(string username)
        {
            SendToVerificationBy = username;
            SendToVerificationDate = DateTimeOffset.Now;
            Position = GarmentPurchasingExpeditionPosition.SendToVerification;
            VerifiedDateSend = DateTimeOffset.Now.ToUniversalTime();
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
            VerifiedBy = username;
            VerifiedDate = DateTimeOffset.Now;
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
            VerifiedDateReceived = DateTimeOffset.Now.ToUniversalTime();
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
            VerifiedBy = null;
            VerifiedDate = null;
        }

        public void VoidCashier(string username)
        {
            SendToCashierBy = null;
            SendToCashierDate = null;
            VerifiedBy = null;
            VerifiedDate = null;
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
            VerifiedBy = username;
            VerifiedDate = DateTimeOffset.Now;
        }

        public void PurchasingAccepted(string username)
        {
            Position = GarmentPurchasingExpeditionPosition.Purchasing;
        }
    }
}
