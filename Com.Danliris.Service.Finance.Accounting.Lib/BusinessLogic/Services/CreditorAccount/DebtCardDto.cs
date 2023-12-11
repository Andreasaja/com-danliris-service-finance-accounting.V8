using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.CreditorAccount
{
    public class DebtCardDto
    {
        public DebtCardDto(DateTimeOffset date, string unitReceiptNoteNo, string bankExpenditureNoteNo, string unitPaymentOrderNo, string invoiceNo, string unitPaymentCorrectionNoteNo, int paymentDuration, decimal dppAmount, decimal dppAmountCurrency, decimal vatAmount, decimal mutation, decimal purchaseAmount, decimal paymentAmount, string products)
        {
            Date = date;
            Products = products;
            UnitReceiptNoteNo = unitReceiptNoteNo;
            BankExpenditureNoteNo = bankExpenditureNoteNo;
            UnitPaymentOrderNo = unitPaymentOrderNo;
            InvoiceNo = invoiceNo;
            UnitPaymentCorrectionNoteNo = unitPaymentCorrectionNoteNo;
            PaymentDuration = paymentDuration;
            DPPAmount = dppAmount;
            DPPAmountCurrency = dppAmountCurrency;
            VATAmount = vatAmount;
            Mutation = mutation;
            PurchaseAmount = purchaseAmount;
            PaymentAmount = paymentAmount;
        }

        public DateTimeOffset Date { get; private set; }
        public string Products { get; private set; }
        public string UnitReceiptNoteNo { get; private set; }
        public string BankExpenditureNoteNo { get; private set; }
        public string UnitPaymentOrderNo { get; private set; }
        public string InvoiceNo { get; private set; }
        public string UnitPaymentCorrectionNoteNo { get; private set; }
        public int PaymentDuration { get; private set; }
        public decimal DPPAmount { get; private set; }
        public decimal DPPAmountCurrency { get; private set; }
        public decimal VATAmount { get; private set; }
        public decimal Mutation { get; private set; }
        public decimal PurchaseAmount { get; private set; }
        public decimal PaymentAmount { get; private set; }
        public decimal FinalBalance { get; private set; }
        public string Remark { get; private set; }

        public DebtCardDto(string remark, decimal finalBalance)
        {
            Remark = remark;
            FinalBalance = finalBalance;
        }

        public void SetFinalBalance(decimal finalBalance)
        {
            FinalBalance = finalBalance;
        }

    }
}
