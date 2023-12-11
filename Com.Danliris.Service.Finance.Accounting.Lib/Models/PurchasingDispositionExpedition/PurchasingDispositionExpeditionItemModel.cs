using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition
{
    public class PurchasingDispositionExpeditionItemModel : StandardEntity, IValidatableObject
    {
        public string EPOId { get; set; }
        public string EPONo { get; set; }
        public double Price { get; set; }
        [MaxLength(50)]
        public string ProductId { get; set; }
        [MaxLength(255)]
        public string ProductCode { get; set; }
        [MaxLength(255)]
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        [MaxLength(50)]
        public string UnitId { get; set; }
        [MaxLength(255)]
        public string UnitCode { get; set; }
        [MaxLength(255)]
        public string UnitName { get; set; }
        public string UomId { get; set; }
        public string UomUnit { get; set; }
        public int PurchasingDispositionDetailId { get; set; }
        public virtual int PurchasingDispositionExpeditionId { get; set; }
        [ForeignKey("PurchasingDispositionExpeditionId")]
        public virtual PurchasingDispositionExpeditionModel PurchasingDispositionExpedition { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
