namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel
{
    public class CurrencyViewModel
    {
        public string _id { get; set; }
        public string code { get; set; }
        public string symbol { get; set; }
        public double rate { get; set; }
        public string description { get; set; }
    }

    //public class NewCurrencyViewModel
    //{
    //    public int? Id { get; set; }
    //    public string Code { get; set; }
    //    public string Symbol { get; set; }
    //    public double? Rate { get; set; }
    //    public string Description { get; set; }
    //}
}