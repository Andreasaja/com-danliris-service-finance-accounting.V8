using System;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBNonPO
{
    public class DetailRequestNonPO
    {
        public int? Id { get; set; }
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyDescription { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? DateEstimate { get; set; }
        public string UnitCode { get; set; }
        public int UnitId { get; set; }
        public string UnitLoad { get; set; }
        public string UnitName { get; set; }
        public int UnitDivisionId { get; set; }
        public string UnitDivisionName { get; set; }
        public string VBNo { get; set; }
        public string VBRequestCategory { get; set; }
        public string Usage { get; set; }
    }
}