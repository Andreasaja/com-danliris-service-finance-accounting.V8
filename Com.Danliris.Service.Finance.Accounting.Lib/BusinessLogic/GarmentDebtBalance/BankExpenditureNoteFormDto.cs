namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDebtBalance
{
    public class BankExpenditureNoteFormDto
    {
        public int BankExpenditureNoteId { get;  set; }
        public string BankExpenditureNoteNo { get;  set; }
        public double BankExpenditureNoteInvoiceAmount { get;  set; }
        public double CurrencyBankExpenditureNoteInvoiceAmount { get; set; }
    }
}