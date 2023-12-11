using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class ReportDoDetailDto
    {
        public ReportDoDetailDto(string dONo, long dOId, string paymentBill, string billNo, double totalAmount,double currencyRate)
        {
            DONo = dONo;
            DOId = dOId;
            PaymentBill = paymentBill;
            BillNo = billNo;
            TotalAmount = totalAmount;
            CurrencyRate = currencyRate;
        }

        public string DONo { get; set; }
        public long DOId { get; set; }
        public string PaymentBill { get; set; }
        public string BillNo { get; set; }
        public double TotalAmount { get; set; }
        public double CurrencyRate { get; set; }
        //public double RatePaymentMonthOngoing { get; set; }
        //public double RateNoteAsDebt { get; set; }
        //public double PaymentAmountMonthOngoing { get; set; }
        //public double PaymentAmountAsDebt { get; set; }
        //public double CurrencyRateDifference { get; set; }

    }
}
