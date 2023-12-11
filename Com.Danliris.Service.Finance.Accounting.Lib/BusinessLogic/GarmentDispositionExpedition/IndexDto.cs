using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDispositionExpedition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionExpedition
{
    public class IndexDto
    {
        public IndexDto(int id, string dispositionNoteNo, DateTimeOffset dispositionNoteDate, int dispositionNoteId, string supplierName, GarmentPurchasingExpeditionPosition position, double totalPaid, string currencyCode, string remark,DateTimeOffset createdDate)
        {
            Id = id;
            DispositionNoteNo = dispositionNoteNo;
            DispositionNoteDate = dispositionNoteDate;
            DispositionNoteId = dispositionNoteId;
            SupplierName = supplierName;
            TotalPaid = totalPaid;
            CurrencyCode = currencyCode;
            Remark = remark;
            Status = position.ToDescriptionString();
            CreatedDate = createdDate;
        }
        public IndexDto(int id, string dispositionNoteNo, DateTimeOffset dispositionNoteDate, int dispositionNoteId, string supplierName, GarmentPurchasingExpeditionPosition position, double totalPaid, string currencyCode, string remark, DateTimeOffset verifiedDateSend, DateTimeOffset verifiedDateReceived,string sendToPurchasingRemark, DateTimeOffset createdDate)
        {
            Id = id;
            DispositionNoteNo = dispositionNoteNo;
            DispositionNoteDate = dispositionNoteDate;
            DispositionNoteId = dispositionNoteId;
            SupplierName = supplierName;
            TotalPaid = totalPaid;
            CurrencyCode = currencyCode;
            Remark = remark;
            Status = position.ToDescriptionString();
            VerifiedDateReceived = verifiedDateReceived;
            VerifiedDateSend = verifiedDateSend;
            SendToPurchasingRemark = sendToPurchasingRemark;
            CreatedDate = createdDate;
        }

        public IndexDto(int id, string dispositionNoteNo,
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
            DateTimeOffset createdDate
            )
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
            CreatedDate = createdDate;
        }

        public IndexDto(GarmentDispositionExpeditionModel entity)
        {
            Id = entity.Id;
            DispositionNoteNo = entity.DispositionNoteNo;
            DispositionNoteDate = entity.DispositionNoteDate;
            DispositionNoteDueDate = entity.DispositionNoteDueDate;
            DispositionNoteId = entity.DispositionNoteId;
            SupplierName = entity.SupplierName;
            TotalPaid = entity.TotalPaid;
            CurrencyCode = entity.CurrencyCode;
            VerificationAcceptedDate = entity.VerificationAcceptedDate;
            SendToPurchasingRemark = entity.SendToPurchasingRemark;
            Remark = entity.Remark;
            DPPAmount = entity.DPPAmount;
            VATAmount = entity.VATAmount;
            IncomeTaxAmount = entity.IncomeTaxAmount;
            Status = entity.Position.ToDescriptionString();
            Date = entity.Position == GarmentPurchasingExpeditionPosition.SendToAccounting ? entity.SendToAccountingDate : entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier ? entity.SendToCashierDate : entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing ? entity.SendToPurchasingDate : entity.VerificationAcceptedDate;
            VerifiedBy = entity.Position == GarmentPurchasingExpeditionPosition.SendToAccounting ? entity.SendToAccountingBy : entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier ? entity.SendToCashierBy : entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing ? entity.SendToPurchasingBy : entity.VerificationAcceptedBy;
            SentDate = entity.Position == GarmentPurchasingExpeditionPosition.SendToAccounting || entity.Position == GarmentPurchasingExpeditionPosition.AccountingAccepted ? entity.SendToAccountingDate : entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier || entity.Position == GarmentPurchasingExpeditionPosition.CashierAccepted ? entity.SendToCashierDate : entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing ? entity.SendToPurchasingDate : entity.SendToVerificationDate;
            AcceptedDate = entity.Position == GarmentPurchasingExpeditionPosition.AccountingAccepted ? entity.AccountingAcceptedDate : entity.Position == GarmentPurchasingExpeditionPosition.CashierAccepted ? entity.CashierAcceptedDate : entity.Position == GarmentPurchasingExpeditionPosition.VerificationAccepted ? entity.VerificationAcceptedDate : null;
            CreatedDate = entity.CreatedUtc;
            ProformaNo = entity.ProformaNo;
            Amount = entity.DPPAmount + VATAmount - IncomeTaxAmount;
            Category = entity.Category;
            VerifiedDate = entity.VerifiedDate;
            
        }

        public int Id { get; private set; }
        public string DispositionNoteNo { get; private set; }
        public DateTimeOffset DispositionNoteDate { get; private set; }
        public DateTimeOffset DispositionNoteDueDate { get; private set; }
        public int DispositionNoteId { get; private set; }
        public double CurrencyTotalPaid { get; private set; }
        public double TotalPaid { get; private set; }
        public int CurrencyId { get; private set; }
        public string CurrencyCode { get; private set; }
        public DateTimeOffset? VerificationAcceptedDate { get; private set; }
        public double CurrencyRate { get; private set; }
        public string SupplierName { get; private set; }
        public int SupplierId { get; private set; }
        public string SupplierCode { get; private set; }
        public double VATAmount { get; private set; }
        public double CurrencyVATAmount { get; private set; }
        public double IncomeTaxAmount { get; private set; }
        public double CurrencyIncomeTaxAmount { get; private set; }
        public double DPPAmount { get; private set; }
        public double CurrencyDPPAmount { get; set; }
        public string Remark { get; private set; }
        public string Status { get; private set; }
        public DateTimeOffset? Date { get; private set; }
        public string VerifiedBy { get; private set; }
        public DateTimeOffset? SentDate { get; private set; }
        public DateTimeOffset? AcceptedDate { get; private set; }
        public string ProformaNo { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTimeOffset VerifiedDateSend { get; set; }
        public DateTimeOffset VerifiedDateReceived { get; set; }
        public string SendToPurchasingRemark { get; private set; }
        public DateTimeOffset CreatedDate { get; private set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public double TotalPaidPaymentBefore { get; set; }
        public double DiffTotalPaidPayment { get; set; }
        public DateTimeOffset? VerifiedDate { get; set; }
    }
}
