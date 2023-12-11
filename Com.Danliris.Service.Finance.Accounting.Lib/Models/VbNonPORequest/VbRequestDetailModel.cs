using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest
{
    public class VbRequestDetailModel : StandardEntity
    {
        [MaxLength(64)]
        public string VBNo { get; set; }
        public int POId { get; set;}
        [MaxLength(64)]
        public string PONo { get; set; }
        public int UnitId { get; set; }
        [MaxLength(64)]
        public string UnitName { get; set; }
        [MaxLength(64)]
        public string DetailOthers { get; set; }
        [MaxLength(64)]
        public string ProductId { get; set; }
        [MaxLength(64)]
        public string ProductCode { get; set; }
        [MaxLength(64)]
        public string ProductName { get; set; }
        public decimal DefaultQuantity { get; set; }
        [MaxLength(64)]
        public string DefaultUOMId { get; set; }
        [MaxLength(64)]
        public string DefaultUOMUnit { get; set; }
        public decimal DealQuantity { get; set; }
        [MaxLength(64)]
        public string DealUOMId { get; set; }
        [MaxLength(64)]
        public string DealUOMUnit { get; set; }
        public decimal Conversion { get; set; }
        public decimal Price { get; set; }
        [MaxLength(64)]
        public string ProductRemark { get; set; }
        public int VBId { get; set; }
        public bool IsUseVat { get; set; } // include ppn?
        public bool POExtUseVat { get; set; }

        [ForeignKey("VBId")]
        public virtual VbRequestModel VbRequestDetail { get; set; }
    }
}
