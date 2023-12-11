using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class VBRequestDocumentNonPODto : BaseViewModel
    {
        public VBRequestDocumentNonPODto()
        {
            Items = new List<VBRequestDocumentNonPOItemDto>();
        }

        public string DocumentNo { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? RealizationEstimationDate { get; set; }
        public UnitDto SuppliantUnit { get; set; }
        public CurrencyDto Currency { get; set; }
        public decimal? Amount { get; set; }
        public string Purpose { get; set; }

        public bool IsApproved { get; set; }

        public List<VBRequestDocumentNonPOItemDto> Items { get; set; }
        public string ApprovalStatus { get; set; }
        public bool IsInklaring { get; set; }
        public string NoBL { get; set; }
        public string NoPO { get; set; }
    }
}