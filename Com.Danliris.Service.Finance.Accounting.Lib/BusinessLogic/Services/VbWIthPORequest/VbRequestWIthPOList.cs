using iTextSharp.text;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VbWIthPORequest
{
    public class VbRequestWIthPOList
    {
        public int Id { get; set; }
        public string VBNo { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset DateEstimate { get; set; }
        public DateTimeOffset? ApproveDate { get; set; }
        public string UnitLoad { get; set; }
        public int UnitId { get; set; }
        public string UnitCode { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal Amount { get; set; }
        public string UnitName { get; set; }
        public string CreatedBy { get; set; }
        public bool Apporve_Status { get; set; }
        public bool Complete_Status { get; set; }
        public string VBRequestCategory { get; set; }
        public List<ModelVbPONumber> PONo { get; set; }
    }
}