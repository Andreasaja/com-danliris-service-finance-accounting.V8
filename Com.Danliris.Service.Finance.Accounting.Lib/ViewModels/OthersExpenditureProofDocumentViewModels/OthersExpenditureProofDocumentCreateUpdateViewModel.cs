using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.OthersExpenditureProofDocument;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels
{
    public class OthersExpenditureProofDocumentCreateUpdateViewModel : IValidatableObject
    {
        public int? AccountBankId { get; set; }
        public string AccountBankCode { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string Type { get; set; }
        public string CekBgNo { get; set; }
        public string Remark { get; set; }

        public ICollection<OthersExpenditureProofDocumentCreateUpdateItemViewModel> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AccountBankId.GetValueOrDefault() <= 0)
                yield return new ValidationResult("Bank harus diisi", new List<string> { "AccountBank" });

            if (Date == null || Date > DateTimeOffset.Now)
                yield return new ValidationResult("Tanggal transaksi harus lebih kecil atau sama dengan hari ini", new List<string> { "Date" });

            if (string.IsNullOrWhiteSpace(Type))
                yield return new ValidationResult("Jenis Transaksi harus diisi", new List<string> { "Type" });

            if (Items == null || Items.Count.Equals(0))
            {
                yield return new ValidationResult("Item harus diisi", new List<string> { "Item" });
            }
            else if (!Items.Count.Equals(0))
            {
                int CountItemsError = 0;
                string ItemsError = "[";

                foreach (var item in Items)
                {
                    ItemsError += "{ ";
                    if (item.COAId.GetValueOrDefault() == 0)
                    {
                        CountItemsError++;
                        ItemsError += "'COA': 'Akun COA harus diisi', ";
                    }

                    if (item.Debit.GetValueOrDefault() <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'Debit': 'Debit harus lebih besar dari 0', ";
                    }

                    ItemsError += "}, ";
                }

                ItemsError += "]";

                if (CountItemsError > 0)
                    yield return new ValidationResult(ItemsError, new List<string> { "Items" });
            }
        }

        public OthersExpenditureProofDocumentModel MapToModel()
        {
            return new OthersExpenditureProofDocumentModel()
            {
                AccountBankId = AccountBankId.GetValueOrDefault(),
                Date = Date.GetValueOrDefault(),
                Type = Type,
                CekBgNo = CekBgNo,
                Remark = Remark
            };
        }

        public List<OthersExpenditureProofDocumentItemModel> MapItemToModel()
        {
            return Items.Select(item => new OthersExpenditureProofDocumentItemModel()
            {
                Id = item.Id.GetValueOrDefault(),
                COAId = item.COAId.GetValueOrDefault(),
                Debit = item.Debit.GetValueOrDefault(),
                Remark = item.Remark
            }).ToList();
        }
    }
}
