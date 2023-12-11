using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionExpedition
{
    public class SendToVerificationAccountingFormDto : IValidatableObject
    {
        public SendToVerificationAccountingFormDto()
        {
            Items = new List<FormItemDto>();
        }

        public List<FormItemDto> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Items == null || Items.Count.Equals(0))
            {
                yield return new ValidationResult("Nota Intern tidak boleh kosong", new List<string> { "Item" });
            }
            else if (Items.Count > 0)
            {
                int CountItemsError = 0;
                string ItemsError = "[";

                foreach (var item in Items)
                {
                    ItemsError += "{ ";

                    if (item.DispositionNote == null || item.DispositionNote.Id <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'DispositionNote': 'Disposisi harus diisi', ";
                    }

                    ItemsError += "}, ";
                }

                ItemsError += "]";

                if (CountItemsError > 0)
                    yield return new ValidationResult(ItemsError, new List<string> { "Items" });
            }
        }
    }
}
