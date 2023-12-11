using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MemoDetailGarmentPurchasing
{
    public class EditDetailRincian : IValidatableObject
    {
        public int MemoId { get; set; }
        public string MemoNo { get; set; }
        public DateTimeOffset MemoDate { get; set; }
        public int AccountingBookId { get; set; }
        public string AccountingBookType { get; set; }
        public int GarmentCurrenciesId { get; set; }
        public string GarmentCurrenciesCode { get; set; }
        public int GarmentCurrenciesRate { get; set; }
        public string Remarks { get; set; }
        public bool IsPosted { get; set; }
        public ICollection<EditDetailRincianItems> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Items.Count <= 0)
            {
                yield return new ValidationResult("Data rincian harus diisi", new List<string> { "Items" });
            }
        }
    }
}
