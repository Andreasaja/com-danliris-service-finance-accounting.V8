using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService
{
    public class APIDefaultResponse<T>
    {
        public T data { get; set; }
    }

    /// <summary>
    /// You can add any property. Just don't remove!
    /// </summary>
    public class AccountBank
    {
        public int Id { get; set; }
        public string AccountCOA { get; set; }
        public string AccountName { get; set; }
        public Currency Currency { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string DivisionName { get; set; }
    }

    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
    }

    public class GarmentCurrency
    {
        public string UId { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public double? Rate { get; set; }
    }
}