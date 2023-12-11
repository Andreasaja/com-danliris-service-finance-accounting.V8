using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class VbVerificationList
    {
        public int Id { get; set; }
        public string VBNo { get; set; }
        public DateTimeOffset DateRealization { get; set; }
        public DateTimeOffset DateEstimate { get; set; }
        public string UnitLoad { get; set; }
        public decimal Amount_Realization { get; set; }
        public string UnitName { get; set; }
        public string VBNoRealize { get; set; }
        public string RequestVbName { get; set; }
        public string VBRealizeCategory { get; set; }
        public DateTimeOffset DateVB { get; set; }
        public string Currency { get; set; }
        public decimal Diff { get; set; }
        public string Usage { get; set; }
        public decimal Amount_Request { get; set; }
        public string Status_ReqReal { get; set; }
        public decimal Amount_Vat { get; set; }

        public List<ModelVbItem> DetailItems { get; set; }
    }
}