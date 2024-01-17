using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote
{
    public class DPPVATBankExpenditureNoteDetailDoModel: StandardEntity
    {
        public DPPVATBankExpenditureNoteDetailDoModel(string dONo, double totalAmount, string paymentBill, string billNo, long dOId,double currencyRate)
        {
            DONo = dONo;
            TotalAmount = totalAmount;
            PaymentBill = paymentBill;
            BillNo = billNo;
            DOId = dOId;
            CurrencyRate = currencyRate;
        }

        public DPPVATBankExpenditureNoteDetailDoModel(string dONo, double totalAmount, string paymentBill, string billNo, long dOId,double currencyRate, int dPPVATBankExpenditureNoteId, int dPPVATBankExpenditureNoteItemId, int dPPVATBankExpenditureNoteDetailId)
        {
            DONo = dONo;
            TotalAmount = totalAmount;
            PaymentBill = paymentBill;
            BillNo = billNo;
            DOId = dOId;
            CurrencyRate = currencyRate;
            DPPVATBankExpenditureNoteId = dPPVATBankExpenditureNoteId;
            DPPVATBankExpenditureNoteItemId = dPPVATBankExpenditureNoteItemId;
            DPPVATBankExpenditureNoteDetailId = dPPVATBankExpenditureNoteDetailId;
        }
        public DPPVATBankExpenditureNoteDetailDoModel()
        {

        }

        public string? DONo { get; set; }
        public double TotalAmount { get; set; }
        public string? PaymentBill { get; set; }
        public string? BillNo { get; set; }
        public long DOId { get; set; }
        public double CurrencyRate { get; set; }
        public int DPPVATBankExpenditureNoteId { get; set; }
        public int DPPVATBankExpenditureNoteItemId { get; set; }
        public int DPPVATBankExpenditureNoteDetailId { get; set; }

        [ForeignKey("DPPVATBankExpenditureNoteDetailId")]
        public virtual DPPVATBankExpenditureNoteDetailModel DPPVATBankExpenditureNoteDetail { get; set; }
    }
}
