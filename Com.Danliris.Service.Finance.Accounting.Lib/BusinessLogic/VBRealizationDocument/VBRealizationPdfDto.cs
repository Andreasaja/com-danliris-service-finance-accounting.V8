using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument
{
    public class VBRealizationPdfDto
    {
        public VBRealizationPdfDto()
        {
            Items = new HashSet<VBRealizationDocumentExpenditureItemModel>();
            UnitCosts = new HashSet<VBRealizationDocumentUnitCostsItemModel>();
        }

        public VBRealizationDocumentModel Header { get; set; }

        public IEnumerable<VBRealizationDocumentExpenditureItemModel> Items { get; set; }

        public IEnumerable<VBRealizationDocumentUnitCostsItemModel> UnitCosts { get; set; }
    }
}
