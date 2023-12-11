using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO;
using Com.Moonlay.Models;
using Microsoft.Extensions.Primitives;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Danliris.Service.Finance.Accounting.Lib
{
    public class RealizationVbDetailModel : StandardEntity
    {
        [MaxLength(64)]
        public string DivisionSPB { get; set; }
        [MaxLength(64)]
        public string NoSPB { get; set; }
        public DateTimeOffset? DateSPB { get; set; }
        [MaxLength(64)]
        public string SupplierCode { get; set; }
        [MaxLength(64)]
        public string SupplierName { get; set; }
        [MaxLength(64)]
        public string CurrencyId { get; set; }
        [MaxLength(64)]
        public string CurrencyCode { get; set; }
        [MaxLength(64)]
        public string CurrencySymbol { get; set; }
        public double CurrencyRate { get; set; }
        [MaxLength(64)]
        public string NoPOSPB { get; set; }
        public decimal PriceTotalSPB { get; set; }
        [MaxLength(64)]
        public string IdProductSPB { get; set; }
        [MaxLength(64)]
        public string CodeProductSPB { get; set; }
        [MaxLength(64)]
        public string NameProductSPB { get; set; }
        public decimal AmountNonPO { get; set; }
        public DateTimeOffset? DateNonPO { get; set; }
        [MaxLength(255)]
        public string Remark { get; set; }
        public bool isGetPPn { get; set; }
        public bool isGetPPh { get; set; }
        [MaxLength(64)]
        public int IncomeTaxId { get; set; }
        [MaxLength(64)]
        public string IncomeTaxName { get; set; }
        [MaxLength(64)]
        public double IncomeTaxRate { get; set; }
        [MaxLength(64)]
        public string IncomeTaxBy { get; set; }
        public int VBRealizationId { get; set; }

        [ForeignKey("VBRealizationId")]
        public virtual RealizationVbModel RealizationVbDetail { get; set; }
        public double Total { get; set; }
    }
}