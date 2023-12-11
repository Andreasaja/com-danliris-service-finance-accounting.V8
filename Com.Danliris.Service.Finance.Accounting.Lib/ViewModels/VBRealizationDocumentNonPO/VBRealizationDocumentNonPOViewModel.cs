using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO
{
    public class VBRealizationDocumentNonPOViewModel : BaseViewModel, IValidatableObject
    {
        public VBRealizationDocumentNonPOViewModel()
        {
            Items = new HashSet<VBRealizationDocumentNonPOExpenditureItemViewModel>();
            UnitCosts = new HashSet<VBRealizationDocumentNonPOUnitCostViewModel>();
        }

        public VBType Type { get; set; }
        public int Index { get; set; }
        public string DocumentNo { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string VBNonPOType { get; set; }
        public VBRequestDocumentNonPODto VBDocument { get; set; }
        public UnitViewModel Unit { get; set; }
        public CurrencyViewModel Currency { get; set; }

        public RealizationDocumentType DocumentType { get; set; }

        public VBRealizationPosition Position { get; set; }

        public decimal Amount { get; set; }

        public string BLAWBNumber { get; set; }

        public string ContractPONumber { get; set; }

        public bool IsInklaring { get; set; }

        public IEnumerable<VBRealizationDocumentNonPOExpenditureItemViewModel> Items { get; set; }
        public IEnumerable<VBRealizationDocumentNonPOUnitCostViewModel> UnitCosts { get; set; }
        public string Remark { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date == null)
                yield return new ValidationResult("Tanggal harus diisi!", new List<string> { "Date" });

            if (string.IsNullOrWhiteSpace(VBNonPOType))
            {
                yield return new ValidationResult("Tipe VB harus dipilih!", new List<string> { "VBNonPOType" });
            }
            else
            {
                if (VBNonPOType == "Dengan Nomor VB")
                {
                    if (VBDocument == null || VBDocument.Id == 0)
                    {
                        yield return new ValidationResult("No VB harus diisi!", new List<string> { "VBDocument" });
                    }
                }
                else
                {
                    if (Unit == null || Unit.Id == 0)
                        yield return new ValidationResult("Unit harus diisi!", new List<string> { "Unit" });

                    if (Currency == null || Currency.Id == 0)
                        yield return new ValidationResult("Mata Uang harus diisi!", new List<string> { "Currency" });
                }
            }

            if (Items.Count() == 0)
            {
                yield return new ValidationResult("Daftar harus diisi", new List<string> { "Item" });
            }
            else
            {
                int CountItemsError = 0;
                string ItemsError = "[";

                foreach (var item in Items)
                {
                    ItemsError += "{ ";

                    if (!item.DateDetail.HasValue)
                    {
                        CountItemsError++;
                        ItemsError += "'DateDetail': 'Tanggal harus diisi!', ";
                    }
                    else
                    {
                        if (item.DateDetail.Value > Date)
                        {
                            CountItemsError++;
                            ItemsError += "'DateDetail': 'Tanggal Nota harus kurang atau sama dengan Tanggal Realisasi!', ";
                        }
                        else if (VBNonPOType == "Dengan Nomor VB" && item.DateDetail.Value.Date < VBDocument.Date.GetValueOrDefault().Date)
                        {
                            CountItemsError++;
                            ItemsError += "'DateDetail': 'Tanggal Nota harus lebih atau sama dengan Tanggal Permohonan VB!', ";
                        }


                    }

                    if (string.IsNullOrWhiteSpace(item.Remark))
                    {
                        CountItemsError++;
                        ItemsError += "'Remark': 'Keterangan harus diisi!', ";
                    }

                    if(IsInklaring)
                    {
                        if (string.IsNullOrWhiteSpace(item.BLAWBNumber))
                        {
                            CountItemsError++;
                            ItemsError += "'BLAWBNumber': 'No. BL/AWB harus diisi!', ";
                        }

                        if (item.IsGetPPn)
                        {
                            if (item.PPnAmount <= 0)
                            {
                                CountItemsError++;
                                ItemsError += "'PPnAmount': 'Jumlah harus lebih besar dari 0!', ";
                            }
                        }


                    }

                    if (item.IsGetPPn)
                    {
                        if (!IsInklaring)
                        {
                            if (item.VatTax == null || string.IsNullOrWhiteSpace(item.VatTax.Id))
                            {
                                CountItemsError++;
                                ItemsError += "'VatTax': 'Rate PPn Harus DiIsi!', ";
                            }
                        }
                    }

                    if (item.Amount <= 0)
                    {
                        CountItemsError++;
                        ItemsError += "'Amount': 'Jumlah harus lebih besar dari 0!', ";
                    }

                    if (item.IsGetPPh)
                    {
                        if (!IsInklaring)
                        {
                            if (item.IncomeTax == null || item.IncomeTax.Id == 0)
                            {
                                CountItemsError++;
                                ItemsError += "'IncomeTax': 'Nomor PPh Harus Diisi!', ";
                            }
                        } else
                        {
                            if (item.PPhAmount <= 0)
                            {
                                CountItemsError++;
                                ItemsError += "'PPhAmount': 'Jumlah harus lebih besar dari 0!', ";
                            }
                        }

                        if (string.IsNullOrWhiteSpace(item.IncomeTaxBy))
                        {
                            CountItemsError++;
                            ItemsError += "'IncomeTaxBy': 'Ditanggung Oleh harus Dipilih!', ";
                        }
                    }

                    ItemsError += "}, ";
                }

                ItemsError += "]";

                if (CountItemsError > 0)
                    yield return new ValidationResult(ItemsError, new List<string> { "Items" });
            }

            if (UnitCosts.Count() == 0)
            {
                yield return new ValidationResult("Beban unit harus diisi", new List<string> { "UnitCost" });
            }
            else
            {
                if (UnitCosts.Count(s => s.IsSelected) == 0)
                {
                    yield return new ValidationResult("Beban unit harus dipilih minimal 1", new List<string> { "UnitCost" });
                }
                else
                {
                    if (!UnitCosts.Where(d => d.IsSelected).All(s => s.Unit != null && s.Unit.Id != 0))
                    {
                        yield return new ValidationResult("Beban Unit harus memiliki Nama Unit", new List<string> { "UnitCost" });
                    }
                    else if (!UnitCosts.Where(d => d.IsSelected).All(s => s.Amount > 0))
                    {
                        yield return new ValidationResult("Beban unit terpilih harus memiliki Amount", new List<string> { "UnitCost" });
                    }
                }

            }
            if (UnitCosts.Sum(s => s.Amount) != Items.Sum(s => s.Total))
                yield return new ValidationResult("Nominal beban unit dan total nota harus sama!", new List<string> { "CompareNominal" });


        }
    }
}
