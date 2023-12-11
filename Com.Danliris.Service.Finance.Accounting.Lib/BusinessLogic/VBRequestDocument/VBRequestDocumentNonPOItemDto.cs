using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class VBRequestDocumentNonPOItemDto : BaseViewModel
    {
        public UnitDto Unit { get; set; }
        public bool IsSelected { get; set; }
    }
}
