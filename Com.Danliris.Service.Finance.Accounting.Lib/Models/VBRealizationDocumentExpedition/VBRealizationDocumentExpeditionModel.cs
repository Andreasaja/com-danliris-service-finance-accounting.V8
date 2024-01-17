using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Moonlay.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition
{
    public class VBRealizationDocumentExpeditionModel : StandardEntity
    {
        public VBRealizationDocumentExpeditionModel()
        {

        }

        public VBRealizationDocumentExpeditionModel(
            int vbRealizationId,
            int vbId,
            string vbNo,
            string vbRealizationNo,
            DateTimeOffset vbRealizationDate,
            string vbRequestName,
            int unitId,
            string unitName,
            int divisionId,
            string divisionName,
            decimal vbAmount,
            decimal vbRealizationAmount,
            string currencyCode,
            double currencyRate,
            string purpose,
            VBType vbType
            )
        {
            VBRealizationId = vbRealizationId;
            VBId = vbId;
            VBNo = vbNo;
            VBRealizationNo = vbRealizationNo;
            VBRealizationDate = vbRealizationDate;
            VBRequestName = vbRequestName;
            UnitId = unitId;
            UnitName = unitName;
            DivisionId = divisionId;
            DivisionName = divisionName;
            VBAmount = vbAmount;
            VBRealizationAmount = vbRealizationAmount;
            CurrencyCode = currencyCode;
            CurrencyRate = currencyRate;
            VBType = vbType;
            Purpose = purpose;

            Position = VBRealizationPosition.PurchasingToVerification;
        }

        public int VBRealizationId { get; private set; }
        public int VBId { get; private set; }
        [MaxLength(64)]
        public string? VBNo { get; private set; }
        [MaxLength(64)]
        public string? VBRealizationNo { get; private set; }
        public DateTimeOffset VBRealizationDate { get; private set; }
        [MaxLength(256)]
        public string? VBRequestName { get; private set; }
        public int UnitId { get; private set; }
        [MaxLength(64)]
        public string? UnitName { get; private set; }
        public int DivisionId { get; private set; }
        [MaxLength(64)]
        public string? DivisionName { get; private set; }
        public decimal VBAmount { get; private set; }
        public decimal VBRealizationAmount { get; private set; }
        public string? CurrencyCode { get; private set; }
        public double CurrencyRate { get; private set; }
        [MaxLength(256)]
        public string? SendToVerificationBy { get; private set; }
        public DateTimeOffset? SendToVerificationDate { get; private set; }
        [MaxLength(256)]
        public string? VerificationReceiptBy { get; private set; }
        public DateTimeOffset? VerificationReceiptDate { get; private set; }
        [MaxLength(256)]
        public string? VerifiedToCashierBy { get; private set; }
        public DateTimeOffset? VerifiedToCashierDate { get; private set; }
        [MaxLength(256)]
        public string? CashierReceiptBy  { get; private set; }
        public DateTimeOffset? CashierReceiptDate { get; private set; }
        public string? NotVerifiedReason { get; private set; }
        [MaxLength(256)]
        public string? NotVerifiedBy { get; private set; }
        public DateTimeOffset? NotVerifiedDate { get; private set; }
        public VBRealizationPosition Position { get; private set; }
        [MaxLength(64)]
        public VBType VBType { get; set; }
        public string? Purpose { get; private set; }

        public void SubmitToVerification(string name)
        {
            SendToVerificationBy = name;
            SendToVerificationDate = DateTimeOffset.Now;
            Position = VBRealizationPosition.PurchasingToVerification;
        }

        public void VerificationReceipt(string name)
        {
            VerificationReceiptBy = name;
            VerificationReceiptDate = DateTimeOffset.Now;
            Position = VBRealizationPosition.Verification;
        }

        public void SendToCashier(string name)
        {
            VerifiedToCashierBy = name;
            NotVerifiedBy = null;
            NotVerifiedDate = null;
            NotVerifiedReason = null;
            VerifiedToCashierDate = DateTimeOffset.Now;
            Position = VBRealizationPosition.VerifiedToCashier;
        }

        public void CashierVerification(string name)
        {
            CashierReceiptBy = name;
            CashierReceiptDate = DateTimeOffset.Now;
            Position = VBRealizationPosition.Cashier;
        }

        public void CashierDelete()
        {
            CashierReceiptBy = null;
            CashierReceiptDate = null;
            Position = VBRealizationPosition.VerifiedToCashier;
        }

        public void VerificationRejected(string name, string reason)
        {
            NotVerifiedBy = name;
            NotVerifiedDate = DateTimeOffset.Now;
            NotVerifiedReason = reason;
            VerifiedToCashierBy = null;
            VerifiedToCashierDate = null;
            Position = VBRealizationPosition.NotVerified;
        }

        public void UpdateVBRealizationInfo(RealizationVbModel realizationVB)
        {
            VBRealizationId = realizationVB.Id;
            VBId = realizationVB.VBId;
            VBNo = realizationVB.VBNo;
            VBRealizationNo = realizationVB.VBNoRealize;
            VBRealizationDate = realizationVB.Date;
            VBRequestName = realizationVB.RequestVbName;
            UnitId = realizationVB.UnitId;
            UnitName = realizationVB.UnitName;
            DivisionId = realizationVB.DivisionId;
            DivisionName = realizationVB.DivisionName;
            VBAmount = realizationVB.Amount_VB;
            VBRealizationAmount = realizationVB.Amount;
            CurrencyCode = realizationVB.CurrencyCode;
            CurrencyRate = (double)realizationVB.CurrencyRate;
        }
    }
}
