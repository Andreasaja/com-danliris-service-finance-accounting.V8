using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using System;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument
{
    public class UnitPaymentOrderItemDto
    {
        public decimal? Amount { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string IncomeTaxBy { get; set; }
        public IncomeTaxDto IncomeTax { get; set; }
        public string Remark { get; set; }
        public bool? UseIncomeTax { get; set; }
        public bool? UseVat { get; set; }
        public int Id { get; set; }
    }
}