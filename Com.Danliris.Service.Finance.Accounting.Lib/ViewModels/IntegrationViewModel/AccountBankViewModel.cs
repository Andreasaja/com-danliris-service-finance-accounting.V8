namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel
{
    public class AccountBankViewModel
    {
        public string _id { get; set; }
        public string code { get; set; }
        public string accountName { get; set; }
        public string accountNumber { get; set; }
        public string bankName { get; set; }
        public string bankCode { get; set; }
        public string accountCurrencyId { get; set; }
        public CurrencyViewModel currency { get; set; }
    }

    //public class NewAccountBankViewModel
    //{
    //    public int? Id { get; set; }
    //    public string Code { get; set; }
    //    public string AccountName { get; set; }
    //    public string AccountNumber { get; set; }
    //    public string BankName { get; set; }
    //    public string BankCode { get; set; }
    //    public NewCurrencyViewModel Currency { get; set; }
    //}
}
