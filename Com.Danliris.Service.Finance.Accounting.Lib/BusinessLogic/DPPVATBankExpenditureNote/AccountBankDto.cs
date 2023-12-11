namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class AccountBankDto
    {
        public AccountBankDto()
        {

        }

        public AccountBankDto(int bankAccountId, string bankAccountingCode, string bankAccountNumber, string bankName, string currencyCode, int currencyId, double currencyRate)
        {
            BankName = bankName;
            Id = bankAccountId;
            BankCode = bankAccountingCode;
            AccountNumber = bankAccountNumber;
            Currency = new CurrencyDto(currencyCode, currencyId, currencyRate);
        }

        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public CurrencyDto Currency { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
    }
}