using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class RealizationVbList
    {
        public int Id { get; set; }
        public string VBNo { get; set; }
        public string VBNoRealize { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset DateEstimate { get; set; }
        public string CreatedBy { get; set; }
        public bool isVerified { get; set; }
        public string VBRealizeCategory { get; set; }
    }
}