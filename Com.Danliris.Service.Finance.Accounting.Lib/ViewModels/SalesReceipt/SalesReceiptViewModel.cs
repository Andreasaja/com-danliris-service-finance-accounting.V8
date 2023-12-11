using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.SalesReceipt
{
    public class SalesReceiptViewModel : BaseViewModel, IValidatableObject
    {
        [MaxLength(255)]
        public string Code { get; set; }
        public long? AutoIncreament { get; set; }
        [MaxLength(255)]
        public string SalesReceiptNo { get; set; }
        public DateTimeOffset? SalesReceiptDate { get; set; }
        public NewUnitViewModel Unit { get; set; }
        public NewBuyerViewModel Buyer { get; set; }
        [MaxLength(255)]
        public string OriginAccountNumber { get; set; }
        public CurrencyViewModel Currency { get; set; }
        public AccountBankViewModel Bank { get; set; }
        public double? AdministrationFee { get; set; }
        public double TotalPaid { get; set; }

        public ICollection<SalesReceiptDetailViewModel> SalesReceiptDetails { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SalesReceiptDate.HasValue || SalesReceiptDate.Value > DateTimeOffset.Now)
                yield return new ValidationResult("Tgl Kuitansi harus diisi & lebih kecil atau sama dengan hari ini", new List<string> { "SalesReceiptDate" });

            if (Unit == null || string.IsNullOrWhiteSpace(Unit.Name))
                yield return new ValidationResult("Unit harus diisi", new List<string> { "UnitName" });

            if (Bank == null || string.IsNullOrWhiteSpace(Bank.AccountName))
                yield return new ValidationResult("Nama Bank Tujuan harus diisi", new List<string> { "AccountName" });

            if (Buyer == null || string.IsNullOrWhiteSpace(Buyer.Name))
                yield return new ValidationResult("Nama Buyer harus di isi", new List<string> { "BuyerName" });

            //if (string.IsNullOrWhiteSpace(OriginAccountNumber))
            //    yield return new ValidationResult("No Rek. Bank Asal harus di isi", new List<string> { "OriginAccountNumber" });

            if (Currency == null || string.IsNullOrWhiteSpace(Currency.Code))
                yield return new ValidationResult("Jenis Mata Uang harus diisi", new List<string> { "CurrencyCode" });

            if (AdministrationFee < 0)
                yield return new ValidationResult("Total Paid kosong", new List<string> { "AdministrationFee" });

            if (TotalPaid <= 0)
                yield return new ValidationResult("Total Paid kosong", new List<string> { "TotalPaid" });

            if (SalesReceiptDetails != null && SalesReceiptDetails.Count > 0 && SalesReceiptDetails.All(s => s.Nominal <= 0))
                yield return new ValidationResult("Nominal tidak boleh kosong", new List<string> { "NominalNotNull" });

            int Count = 0;
            string DetailErrors = "[";

            if (SalesReceiptDetails != null && SalesReceiptDetails.Count > 0)
            {
                foreach (SalesReceiptDetailViewModel detail in SalesReceiptDetails)
                {
                    DetailErrors += "{";

                    var rowErrorCount = 0;

                    if (string.IsNullOrWhiteSpace(detail.SalesInvoice.SalesInvoiceNo))
                    {
                        Count++;
                        rowErrorCount++;
                        DetailErrors += "SalesInvoiceNo : 'Gagal memperoleh No. Faktur Jual',";
                    }
                    if (detail.Currency == null || string.IsNullOrWhiteSpace(detail.Currency.Code))
                    {
                        Count++;
                        rowErrorCount++;
                        DetailErrors += "CurrencyCode : 'Kurs harus diisi',";
                    }
                    if (detail.TotalPayment <= 0)
                    {
                        Count++;
                        rowErrorCount++;
                        DetailErrors += "TotalPayment : 'Kode Faktur harus diisi untuk memperoleh jumlah pembayaran',";
                    }
                    if (detail.TotalPaid < 0)
                    {
                        Count++;
                        rowErrorCount++;
                        DetailErrors += "TotalPaid : 'Kode Faktur harus diisi untuk memperoleh jumlah pembayaran sebelumnya',";
                    }
                    if (detail.Paid < 0)
                    {
                        Count++;
                        rowErrorCount++;
                        DetailErrors += "Paid : 'Kode Faktur harus diisi untuk memperoleh jumlah yang sudah dibayar',";
                    }
                    //if (detail.Nominal < 0)
                    //{
                    //    Count++;
                    //    rowErrorCount++;
                    //    DetailErrors += "Nominal : 'Nominal tidak boleh kosong & atau lebih kecil dari 0',";
                    //}
                    if (detail.Unpaid < 0)
                    {
                        Count++;
                        rowErrorCount++;
                        DetailErrors += "Unpaid : 'Kode Faktur & Nominal harus diisi untuk memperoleh sisa pembayaran',";
                    }

                    DetailErrors += "}, ";
                }
            }
            else
            {
                yield return new ValidationResult("Detail harus diisi", new List<string> { "SalesReceiptDetail" });
            }

            DetailErrors += "]";

            if (Count > 0)
                yield return new ValidationResult(DetailErrors, new List<string> { "SalesReceiptDetails" });

        }
    }
}
