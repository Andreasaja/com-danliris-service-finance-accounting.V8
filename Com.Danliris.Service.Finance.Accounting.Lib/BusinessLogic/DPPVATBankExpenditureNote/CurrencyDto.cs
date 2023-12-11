namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class CurrencyDto
    {
        public CurrencyDto()
        {

        }

        public CurrencyDto(string currencyCode, int currencyId, double currencyRate)
        {
            Id = currencyId;
            Code = currencyCode;
            Rate = currencyRate;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public double Rate { get; set; }
    }
}