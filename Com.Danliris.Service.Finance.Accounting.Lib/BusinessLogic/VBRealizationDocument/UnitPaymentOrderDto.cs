using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using iTextSharp.text;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument
{
    public class UnitPaymentOrderDto
    {
        public int? Id { get; set; }
        public string No { get; set; }
        public DateTimeOffset? Date { get; set; }
        public decimal? Amount { get; set; }
        public bool? UseVat { get; set; }
        public bool? UseIncomeTax { get; set; }
        public IncomeTaxDto IncomeTax { get; set; }
        public VatTaxDto VatTax { get; set; }
        public string IncomeTaxBy { get; set; }
        public SupplierDto Supplier { get; set; }
        public DivisionDto Division { get; set; }

        public List<UnitCostDto> UnitCosts { get; set; }
    }
}