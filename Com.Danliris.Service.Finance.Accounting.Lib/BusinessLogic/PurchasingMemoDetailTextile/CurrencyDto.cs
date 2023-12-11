namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile
{
    public class CurrencyDto
    {
        public CurrencyDto(int id, string code, double rate)
        {
            Id = id;
            Code = code;
            Rate = rate;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public double Rate { get; set; }
    }
}