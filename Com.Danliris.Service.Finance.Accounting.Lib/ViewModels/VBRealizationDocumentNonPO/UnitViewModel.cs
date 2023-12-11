using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO
{
    public class UnitViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DivisionViewModel Division { get; set; }
        public int VBDocumentLayoutOrder { get; set; }
    }
}
