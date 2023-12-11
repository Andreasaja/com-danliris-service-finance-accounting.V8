using Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class VbRequestList
    {
        public int Id { get; set; }
        public string VBNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset DateEstimate { get; set; }
        public string UnitLoad { get; set; }
        public int UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public int UnitDivisionId { get; set; }
        public string UnitDivisionName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
        public bool Status_Post { get; set; }
        public bool Apporve_Status { get; set; }
        public bool Complete_Status { get; set; }
        public string VBRequestCategory { get; set; }
        public string Usage { get; set; }
        public bool RealizationStatus { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyDescription { get; set; }
        public List<VbRequestDetailModel> PONo { get; set; }
    }
}