using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class CashBalanceFormDto : IValidatableObject
    {
        public int UnitId { get; set; }
        public int DivisionId { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<CashflowUnitItemFormDto> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UnitId <= 0)
            {
                yield return new ValidationResult("Unit harus diisi", new List<string> { "UnitId" });
            }
            else
            {
                if (DivisionId <= 0)
                {
                    yield return new ValidationResult("Unit harus memiliki Divisi", new List<string> { "DivisionId" });
                }
            }

            if (Date == null)
            {
                yield return new ValidationResult("Tanggal harus diisi", new List<string> { "Date" });
            }

            if (Items == null || Items.Count == 0)
            {
                yield return new ValidationResult("Item tidak boleh kosong", new List<string> { "Item" });
            }
            else
            {
                {
                    var CountItemsError = 0;
                    var ItemsError = "[";

                    foreach (var item in Items)
                    {
                        ItemsError += "{ ";
                        if (item.CurrencyId <= 0)
                        {
                            CountItemsError++;
                            ItemsError += "'CurrencyId': 'Mata Uang harus diisi', ";
                        }

                        if (item.IsIDR)
                        {
                            if (item.Nominal <= 0)
                            {
                                CountItemsError++;
                                ItemsError += "'Nominal': 'Nominal harus lebih besar dari 0', ";
                            }
                        }
                        else
                        {
                            if (item.CurrencyNominal <= 0)
                            {
                                CountItemsError++;
                                ItemsError += "'CurrencyNominal': 'Nominal harus lebih besar dari 0', ";
                            }
                        }

                        if (item.Total <= 0)
                        {
                            CountItemsError++;
                            ItemsError += "'Total': 'Total harus lebih besar dari 0', ";
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
}