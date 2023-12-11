using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VbNonPORequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBNonPO
{
    public class RealizationVbNonPOViewModel : BaseViewModel, IValidatableObject
    {
        public string VBRealizationNo { get; set; }

        public DateTimeOffset? Date { get; set; }
        public string TypeVBNonPO { get; set; }

        public DetailRequestNonPO numberVB { get; set; }
        //-----------------------------------------
        public Unit Unit { get; set; }
        public Division Division { get; set; }
        public CurrencyVBRequest Currency { get; set; }
        public decimal AmountVB { get; set; }
        public DateTimeOffset? DateVB { get; set; }
        public DateTimeOffset? DateEstimateVB { get; set; }
        public bool Spinning1 { get; set; }
        public bool Spinning2 { get; set; }
        public bool Spinning3 { get; set; }
        public bool Weaving1 { get; set; }
        public bool Weaving2 { get; set; }
        public bool Finishing { get; set; }
        public bool Printing { get; set; }
        public bool Konfeksi1A { get; set; }
        public bool Konfeksi1B { get; set; }
        public bool Konfeksi2A { get; set; }
        public bool Konfeksi2B { get; set; }
        public bool Konfeksi2C { get; set; }
        public bool Umum { get; set; }
        public bool Others { get; set; }
        public string DetailOthers { get; set; }

        public decimal? AmountSpinning1 { get; set; }
        public decimal? AmountWeaving1 { get; set; }
        public decimal? AmountPrinting { get; set; }
        public decimal? AmountKonfeksi2A { get; set; }
        public decimal? AmountUmum { get; set; }
        public decimal? AmountSpinning2 { get; set; }
        public decimal? AmountWeaving2 { get; set; }
        public decimal? AmountKonfeksi1A { get; set; }
        public decimal? AmountKonfeksi2B { get; set; }
        public decimal? AmountOthers { get; set; }
        public decimal? AmountSpinning3 { get; set; }
        public decimal? AmountFinishing { get; set; }
        public decimal? AmountKonfeksi1B { get; set; }
        public decimal? AmountKonfeksi2C { get; set; }

        public bool Spinning1VB { get; set; }
        public bool Spinning2VB { get; set; }
        public bool Spinning3VB { get; set; }
        public bool Weaving1VB { get; set; }
        public bool Weaving2VB { get; set; }
        public bool FinishingVB { get; set; }
        public bool PrintingVB { get; set; }
        public bool Konfeksi1AVB { get; set; }
        public bool Konfeksi1BVB { get; set; }
        public bool Konfeksi2AVB { get; set; }
        public bool Konfeksi2BVB { get; set; }
        public bool Konfeksi2CVB { get; set; }
        public bool UmumVB { get; set; }
        public bool OthersVB { get; set; }
        public string DetailOthersVB { get; set; }

        public decimal? AmountSpinning1VB { get; set; }
        public decimal? AmountWeaving1VB { get; set; }
        public decimal? AmountPrintingVB { get; set; }
        public decimal? AmountKonfeksi2AVB { get; set; }
        public decimal? AmountUmumVB { get; set; }
        public decimal? AmountSpinning2VB { get; set; }
        public decimal? AmountWeaving2VB { get; set; }
        public decimal? AmountKonfeksi1AVB { get; set; }
        public decimal? AmountKonfeksi2BVB { get; set; }
        public decimal? AmountOthersVB { get; set; }
        public decimal? AmountSpinning3VB { get; set; }
        public decimal? AmountFinishingVB { get; set; }
        public decimal? AmountKonfeksi1BVB { get; set; }
        public decimal? AmountKonfeksi2CVB { get; set; }

        public ICollection<VbNonPORequestDetailViewModel> Items { get; set; }

        private decimal SumUnitCostNonVB()
        {
            var result = AmountSpinning1.GetValueOrDefault() + AmountWeaving1.GetValueOrDefault() + AmountPrinting.GetValueOrDefault() + AmountKonfeksi2A.GetValueOrDefault() + AmountUmum.GetValueOrDefault() + AmountSpinning2.GetValueOrDefault() + AmountWeaving2.GetValueOrDefault() + AmountKonfeksi1A.GetValueOrDefault() + AmountKonfeksi2B.GetValueOrDefault() + AmountOthers.GetValueOrDefault() + AmountSpinning3.GetValueOrDefault() + AmountFinishing.GetValueOrDefault() + AmountKonfeksi1B.GetValueOrDefault() + AmountKonfeksi2C.GetValueOrDefault();
            return result;
        }

        private decimal SumUnitCostVB()
        {
            var result = AmountSpinning1VB.GetValueOrDefault() + AmountWeaving1VB.GetValueOrDefault() + AmountPrintingVB.GetValueOrDefault() + AmountKonfeksi2AVB.GetValueOrDefault() + AmountUmumVB.GetValueOrDefault() + AmountSpinning2VB.GetValueOrDefault() + AmountWeaving2VB.GetValueOrDefault() + AmountKonfeksi1AVB.GetValueOrDefault() + AmountKonfeksi2BVB.GetValueOrDefault() + AmountOthersVB.GetValueOrDefault() + AmountSpinning3VB.GetValueOrDefault() + AmountFinishingVB.GetValueOrDefault() + AmountKonfeksi1BVB.GetValueOrDefault() + AmountKonfeksi2CVB.GetValueOrDefault();
            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date == null)
                yield return new ValidationResult("Tanggal harus diisi!", new List<string> { "Date" });

            if (string.IsNullOrWhiteSpace(TypeVBNonPO))
                yield return new ValidationResult("Tipe VB harus dipilih!", new List<string> { "TypeVBNonPO" });
            else if (TypeVBNonPO == "Tanpa Nomor VB")
            {

                //if (DateEstimateVB == null)
                //    yield return new ValidationResult("Estimasi Tanggal Realisasi harus diisi!", new List<string> { "DateEstimateVB" });

                //if (DateVB == null)
                //    yield return new ValidationResult("Tanggal VB harus diisi!", new List<string> { "DateVB" });

                if (Unit == null || Unit.Id <= 0)
                    yield return new ValidationResult("Kode VB harus diisi!", new List<string> { "VBCodeManual" });

                if (Currency == null /*|| Currency.Id <= 0*/)
                    yield return new ValidationResult("Mata Uang harus diisi!", new List<string> { "Currency" });

                if (AmountVB <= 0)
                    yield return new ValidationResult("Jumlah Uang harus diisi!", new List<string> { "AmountVB" });

                //if (string.IsNullOrWhiteSpace(Usage))
                //    yield return new ValidationResult("Kegunaan harus diisi!", new List<string> { "Usage" });

                if (Spinning1 == false && Spinning2 == false && Spinning3 == false && Weaving1 == false && Weaving2 == false && Finishing == false
                    && Printing == false && Konfeksi1A == false && Konfeksi1B == false && Konfeksi2A == false && Konfeksi2B == false
                    && Konfeksi2C == false && Umum == false && Others == false)
                    yield return new ValidationResult("Beban Unit harus dipilih salah satu!", new List<string> { "UnitLoadCheck" });

                else 
                {
                    if (Spinning1 == true && AmountSpinning1 <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountSpinning1" });

                    if (Spinning2 == true && AmountSpinning2 <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountSpinning2" });

                    if (Spinning3 == true && AmountSpinning3 <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountSpinning3" });

                    if (Weaving1 == true && AmountWeaving1 <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountWeaving1" });

                    if (Weaving2 == true && AmountWeaving2 <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountWeaving2" });

                    if (Finishing == true && AmountFinishing <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountFinishing" });

                    if (Printing == true && AmountPrinting <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountPrinting" });

                    if (Konfeksi1A == true && AmountKonfeksi1A <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountKonfeksi1A" });

                    if (Konfeksi1B == true && AmountKonfeksi1B <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountKonfeksi1B" });

                    if (Konfeksi2A == true && AmountKonfeksi2A <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountKonfeksi2A" });

                    if (Konfeksi2B == true && AmountKonfeksi2B <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountKonfeksi2B" });

                    if (Konfeksi2C == true && AmountKonfeksi2C <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountKonfeksi2C" });

                    if (Umum == true && AmountUmum <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountUmum" });

                    if (Others == true && AmountOthers <= 0)
                        yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountOthers" });
                }
                //
                if (Others == true && string.IsNullOrWhiteSpace(DetailOthers))
                    yield return new ValidationResult("Isian harus diisi!", new List<string> { "DetailOthers" });
            }
            else
            {
                if (numberVB == null)
                    yield return new ValidationResult("No VB harus diisi!", new List<string> { "VBCode" });

                if (Spinning1VB == true && AmountSpinning1VB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountSpinning1VB" });

                if (Spinning2VB == true && AmountSpinning2VB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountSpinning2VB" });

                if (Spinning3VB == true && AmountSpinning3VB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountSpinning3VB" });

                if (Weaving1VB == true && AmountWeaving1VB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountWeaving1VB" });

                if (Weaving2VB == true && AmountWeaving2VB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountWeaving2VB" });

                if (FinishingVB == true && AmountFinishingVB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountFinishingVB" });

                if (PrintingVB == true && AmountPrintingVB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountPrintingVB" });

                if (Konfeksi1AVB == true && AmountKonfeksi1AVB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountKonfeksi1AVB" });

                if (Konfeksi1BVB == true && AmountKonfeksi1BVB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountKonfeksi1BVB" });

                if (Konfeksi2AVB == true && AmountKonfeksi2AVB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountKonfeksi2AVB" });

                if (Konfeksi2BVB == true && AmountKonfeksi2BVB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountKonfeksi2BVB" });

                if (Konfeksi2CVB == true && AmountKonfeksi2CVB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountKonfeksi2CVB" });

                if (UmumVB == true && AmountUmumVB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountUmumVB" });

                if (OthersVB == true && AmountOthersVB <= 0)
                    yield return new ValidationResult("Nominal harus diisi!", new List<string> { "AmountOthersVB" });

            }

            if(TypeVBNonPO == "Tanpa Nomor VB")
            {
                if (Items == null || Items.Count.Equals(0))
                {
                    yield return new ValidationResult("Daftar harus diisi!", new List<string> { "Item" });
                }
                else if (Date == null)
                {
                    yield return new ValidationResult("Tanggal Realisasi harus ada!", new List<string> { "Item" });
                }
                //else if (numberVB == null)
                //{
                //    yield return new ValidationResult("No VB harus ada!", new List<string> { "Item" });
                //}
                else if (Items.Count > 0)
                {
                    int CountItemsError = 0;
                    string ItemsError = "[";

                    foreach (var item in Items)
                    {
                        ItemsError += "{ ";

                        if (item.DateDetail == null)
                        {
                            CountItemsError++;
                            ItemsError += "'DateDetail': 'Tanggal harus diisi!', ";
                        }

                        if (item.DateDetail.HasValue && item.DateDetail.Value.Date > Date.Value.Date)
                        {
                            CountItemsError++;
                            ItemsError += "'DateDetail': 'Tanggal Nota harus kurang atau sama dengan Tanggal Realisasi!', ";
                        }
                        //else if (item.DateDetail.HasValue && item.DateDetail.Value.Date > DateVB.Value.Date)
                        //{
                        //    CountItemsError++;
                        //    ItemsError += "'DateDetail': 'Tanggal Nota harus lebih atau sama dengan Tanggal VB!', ";
                        //}

                        if (string.IsNullOrWhiteSpace(item.Remark))
                        {
                            CountItemsError++;
                            ItemsError += "'Remark': 'Keterangan harus diisi!', ";
                        }

                        if (item.Amount <= 0)
                        {
                            CountItemsError++;
                            ItemsError += "'Amount': 'Jumlah harus lebih besar dari 0!', ";
                        }

                        if (item.isGetPPh == true)
                        {
                            if (item.IncomeTax == null)
                            {
                                CountItemsError++;
                                ItemsError += "'incomeTax': 'Nomor PPh Harus Diisi!', ";
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

                    if (SumUnitCostNonVB() != (decimal)Items.Sum(element => element.Total.GetValueOrDefault()))
                        yield return new ValidationResult("Nominal beban unit dan total nota harus sama!", new List<string> { "CompareNominal" });
                }
                
            }
            else
            {

                if (Items == null || Items.Count.Equals(0))
                {
                    yield return new ValidationResult("Daftar harus diisi!", new List<string> { "Item" });
                }
                else if (Date == null)
                {
                    yield return new ValidationResult("Tanggal Realisasi harus ada!", new List<string> { "Item" });
                }
                else if (numberVB == null)
                {
                    yield return new ValidationResult("No VB harus ada!", new List<string> { "Item" });
                }
                else if (Items.Count > 0)
                {
                    int CountItemsError = 0;
                    string ItemsError = "[";

                    foreach (var item in Items)
                    {
                        ItemsError += "{ ";

                        if (item.DateDetail == null)
                        {
                            CountItemsError++;
                            ItemsError += "'DateDetail': 'Tanggal harus diisi!', ";
                        }

                        if (item.DateDetail.HasValue && item.DateDetail.GetValueOrDefault().Date > Date.GetValueOrDefault().Date)
                        {
                            CountItemsError++;
                            ItemsError += "'DateDetail': 'Tanggal Nota harus kurang atau sama dengan Tanggal Realisasi!', ";
                        }
                        else if (item.DateDetail.HasValue && item.DateDetail.GetValueOrDefault().Date < numberVB.Date.GetValueOrDefault().Date)
                        {
                            CountItemsError++;
                            ItemsError += "'DateDetail': 'Tanggal Nota harus lebih kecil atau sama dengan Tanggal VB!', ";
                        }

                        if (string.IsNullOrWhiteSpace(item.Remark))
                        {
                            CountItemsError++;
                            ItemsError += "'Remark': 'Keterangan harus diisi!', ";
                        }

                        if (item.Amount.GetValueOrDefault() <= 0)
                        {
                            CountItemsError++;
                            ItemsError += "'Amount': 'Jumlah harus lebih besar dari 0!', ";
                        }

                        if (item.isGetPPh == true)
                        {
                            if (item.IncomeTax == null)
                            {
                                CountItemsError++;
                                ItemsError += "'incomeTax': 'Nomor PPh Harus Diisi!', ";
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

                    if (SumUnitCostVB() != (decimal)Items.Sum(element => element.Total.GetValueOrDefault()))
                        yield return new ValidationResult("Nominal beban unit dan total nota harus sama!", new List<string> { "CompareNominal" });
                }
            }

        }
    }
}