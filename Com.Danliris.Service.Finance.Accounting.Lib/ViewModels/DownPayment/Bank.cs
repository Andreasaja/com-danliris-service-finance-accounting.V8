using Org.BouncyCastle.Asn1.Mozilla;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DownPayment
{
    public class Bank
    {
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public BankCurrency Currency { get; set; }
    }
}