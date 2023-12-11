using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class VBRequestDocumentWithPODto
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
        public string DocumentNo { get; set; }
        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? RealizationEstimationDate { get; set; }
        public UnitDto SuppliantUnit { get; set; }
        public CurrencyDto Currency { get; set; }
        public decimal? Amount { get; set; }
        public string Purpose { get; set; }

        public List<VBRequestDocumentWithPOItemDto> Items { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public bool IsInklaring { get; set; }
        public string TypePurchasing { get; set; }
    }
}