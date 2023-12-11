using Com.Moonlay.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoDetailTextile
{
    public class PurchasingMemoDetailTextileDetailModel : StandardEntity
    {
        public PurchasingMemoDetailTextileDetailModel()
        {

        }

        public PurchasingMemoDetailTextileDetailModel(int purchasingMemoDetailTextileId, int purchasingMemoDetailTextileItemId)
        {
            PurchasingMemoDetailTextileId = purchasingMemoDetailTextileId;
            PurchasingMemoDetailTextileItemId = purchasingMemoDetailTextileItemId;
        }

        public PurchasingMemoDetailTextileDetailModel(int purchasingMemoDetailTextileId, int purchasingMemoDetailTextileItemId, int expenditureId, string expenditureNo, DateTimeOffset expenditureDate, int supplierId, string supplierCode, string supplierName, string remark, int unitPaymentOrderId, string unitPaymentOrderNo, DateTimeOffset unitPaymentOrderDate, double purchaseAmountCurrency, double purchaseAmount, double paymentAmountCurrency, double paymentAmount) : this(purchasingMemoDetailTextileId, purchasingMemoDetailTextileItemId)
        {
            ExpenditureId = expenditureId;
            ExpenditureNo = expenditureNo;
            ExpenditureDate = expenditureDate;
            SupplierId = supplierId;
            SupplierCode = supplierCode;
            SupplierName = supplierName;
            Remark = remark;
            UnitPaymentOrderId = unitPaymentOrderId;
            UnitPaymentOrderNo = unitPaymentOrderNo;
            UnitPaymentOrderDate = unitPaymentOrderDate;
            PurchaseAmountCurrency = purchaseAmountCurrency;
            PurchaseAmount = purchaseAmount;
            PaymentAmountCurrency = paymentAmountCurrency;
            PaymentAmount = paymentAmount;
        }

        public int PurchasingMemoDetailTextileId { get; private set; }
        public int PurchasingMemoDetailTextileItemId { get; private set; }
        public int ExpenditureId { get; private set; }
        [MaxLength(64)]
        public string ExpenditureNo { get; private set; }
        public DateTimeOffset ExpenditureDate { get; private set; }
        public int SupplierId { get; private set; }
        [MaxLength(64)]
        public string SupplierCode { get; private set; }
        [MaxLength(512)]
        public string SupplierName { get; private set; }
        public string Remark { get; private set; }
        public int UnitPaymentOrderId { get; private set; }
        [MaxLength(64)]
        public string UnitPaymentOrderNo { get; private set; }
        public DateTimeOffset UnitPaymentOrderDate { get; private set; }
        public double PurchaseAmountCurrency { get; private set; }
        public double PurchaseAmount { get; private set; }
        public double PaymentAmountCurrency { get; private set; }
        public double PaymentAmount { get; private set; }
    }
}
