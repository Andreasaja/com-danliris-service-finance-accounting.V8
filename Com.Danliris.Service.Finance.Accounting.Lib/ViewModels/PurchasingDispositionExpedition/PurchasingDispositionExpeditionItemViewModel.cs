using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionExpedition
{
    public class PurchasingDispositionExpeditionItemViewModel : BaseViewModel, IValidatableObject
    {
        public string epoId { get; set; }
        public double price { get; set; }
        public ProductViewModel product { get; set; }
        public double quantity { get; set; }
        public UnitViewModel unit { get; set; }
        public UomViewModel uom { get; set; }
        public int purchasingDispositionDetailId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
