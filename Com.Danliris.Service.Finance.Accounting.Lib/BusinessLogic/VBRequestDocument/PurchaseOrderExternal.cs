using iTextSharp.text;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class PurchaseOrderExternal
    {
        public int? Id { get; set; }
        public string No { get; set; }
        public List<PurchaseOrderExternalItem> Items { get; set; }

    }
}