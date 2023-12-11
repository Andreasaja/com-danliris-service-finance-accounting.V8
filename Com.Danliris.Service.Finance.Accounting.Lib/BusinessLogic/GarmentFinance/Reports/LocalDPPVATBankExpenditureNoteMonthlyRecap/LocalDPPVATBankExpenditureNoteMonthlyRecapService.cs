using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalDPPVATBankExpenditureNoteMonthlyRecap;
using System.Linq;
using System.IO;
using System.Data;
using OfficeOpenXml;
using System.Globalization;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalDPPVATBankExpenditureNoteMonthlyRecap
{
    public class LocalDPPVATBankExpenditureNoteMonthlyRecapService : ILocalDPPVATBankExpenditureNoteMonthlyRecapService
    {
        private readonly IServiceProvider _serviceProvider;
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;

        public LocalDPPVATBankExpenditureNoteMonthlyRecapService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _dbContext = dbContext;

            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public List<LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel> GetReportQuery(DateTime? dateFrom, DateTime? dateTo, int offset)
        {

            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;

            List<LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel> data = new List<LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel>();
            List<LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel> data1 = new List<LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel>();

            var headerDebit = _dbContext.DPPVATBankExpenditureNotes
                              .Where(a => a.Date.AddHours(7).Date >= DateFrom.Date && a.Date.AddHours(7).Date <= DateTo.Date && a.CurrencyCode=="IDR")

                              .Select(a => new LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
                              {
                                AccountNo = a.BankAccountingCode.Substring(0, 2),
                                AccountName = a.BankName,
                                Debit = 0,
                                Credit = Convert.ToDecimal(a.Amount)
                              });

            var first = headerDebit.ToList()
                .GroupBy(x => new { x.AccountName, x.AccountNo }, (key, group) => new LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
                {
                    AccountName = key.AccountName,
                    AccountNo = key.AccountNo,
                    Credit = group.Sum(a => a.Credit),
                    Debit = group.Sum(a => a.Debit)
                }).OrderByDescending(a => a.Debit).ThenBy(s => s.AccountNo);
            
            foreach (var i in first)
            {
                LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel row = new LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
                {
                    AccountNo = i.AccountNo.Substring(0,2)=="HN" ? "101808" : i.AccountNo.Substring(0, 2) == "MD" ? "101804" : i.AccountNo.Substring(0, 2) == "CN" ? "101801" : "101805",
                    AccountName = i.AccountNo.Substring(0, 2) == "HN" ? "       KAS DI BANK HANA BANK" : i.AccountNo.Substring(0, 2) == "MD" ? "       KAS DI BANK MANDIRI EXIM (RP)" : i.AccountNo.Substring(0, 2) == "CN" ? "       KAS DI BANK CIMB NIAGA (RP)" : "       KAS DI BANK PANIN SOLO (RP)",
                    Credit = i.Credit,
                    Debit = i.Debit
                };
                data1.Add(row);
            }

            var debit = new LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
            {
                AccountNo = "300300",
                AccountName = "HUTANG USAHA LOKAL",                
                Credit = 0,
                Debit = first.Sum(a => a.Credit),               
            };
            data.Add(debit);

            var sumQuery = data1.ToList()
                .GroupBy(x => new { x.AccountName, x.AccountNo }, (key, group) => new
                {
                    name = key.AccountName,
                    code = key.AccountNo,
                    credit = group.Sum(a => a.Credit),
                    debit = group.Sum(a => a.Debit)
                }).OrderBy(s => s.code).ThenByDescending(a => a.debit);

            foreach (var item in sumQuery)
            {
                LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel recap = new LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
                {
                    AccountName = item.name,
                    AccountNo = item.code,
                    Credit = item.credit,
                    Debit = item.debit
                };
                data.Add(recap);
            }

                LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel total = new LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
                {
                AccountName = "",
                AccountNo = "TOTAL",
                Credit = sumQuery.Sum(a => a.credit),
                Debit = sumQuery.Sum(a => a.credit)
                };
            data.Add(total);

            return data;
        }

         public MemoryStream GenerateExcel(DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;

            var Query = GetReportQuery(dateFrom, dateTo, offset);

            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn() { ColumnName = "NAMA PERKIRAAN", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "NO AKUN", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "DEBET", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "KREDIT", DataType = typeof(double) });


            ExcelPackage package = new ExcelPackage();
            if (Query.ToArray().Count() == 0)
            {
                result.Rows.Add("", "", 0, 0);
                bool styling = true;

                foreach (KeyValuePair<DataTable, String> item in new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") })
                {
                    var sheet = package.Workbook.Worksheets.Add(item.Value);

                    sheet.Column(1).Width = 100;
                    sheet.Column(2).Width = 20;
                    sheet.Column(3).Width = 25;
                    sheet.Column(4).Width = 25;

                    #region KopTable
                    sheet.Cells[$"A1:D1"].Value = "PT. DAN LIRIS";
                    sheet.Cells[$"A1:D1"].Merge = true;
                    sheet.Cells[$"A1:D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A1:D1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A1:D1"].Style.Font.Bold = true;

                    sheet.Cells[$"A2:D2"].Value = "ACCOUNTING DEPT.";
                    sheet.Cells[$"A2:D2"].Merge = true;
                    sheet.Cells[$"A2:D2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A2:D2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A2:D2"].Style.Font.Bold = true;

                    sheet.Cells[$"A4:D4"].Value = "IKHTISAR JURNAL";
                    sheet.Cells[$"A4:D4"].Merge = true;
                    sheet.Cells[$"A4:D4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"A4:D4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A4:D4"].Style.Font.Bold = true;

                    sheet.Cells[$"C5"].Value = "BUKU HARIAN";
                    sheet.Cells[$"C5"].Style.Font.Bold = true;
                    sheet.Cells[$"D5"].Value = ": PENGELUARAN KAS BANK - IDR";
                    sheet.Cells[$"D5"].Style.Font.Bold = true;

                    sheet.Cells[$"C6"].Value = "PERIODE";
                    sheet.Cells[$"C6"].Style.Font.Bold = true;
                    sheet.Cells[$"D6"].Value = ": " + DateFrom.ToString("dd-MM-yyyy") + " S/D " + DateTo.ToString("dd-MM-yyyy");
                    sheet.Cells[$"D6"].Style.Font.Bold = true;

                    #endregion
                    sheet.Cells["A8"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);

                    //sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                }
            }
            else
            {
                int index = 0;
                foreach (var d in Query)
                {
                    index++;

                    result.Rows.Add(d.AccountName, d.AccountNo, d.Debit, d.Credit);
                }

                bool styling = true;

                foreach (KeyValuePair<DataTable, String> item in new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") })
                {
                    var sheet = package.Workbook.Worksheets.Add(item.Value);

                    #region KopTable
                    sheet.Cells[$"A1:D1"].Value = "PT. DAN LIRIS";
                    sheet.Cells[$"A1:D1"].Merge = true;
                    sheet.Cells[$"A1:D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A1:D1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A1:D1"].Style.Font.Bold = true;

                    sheet.Cells[$"A2:D2"].Value = "ACCOUNTING DEPT.";
                    sheet.Cells[$"A2:D2"].Merge = true;
                    sheet.Cells[$"A2:D2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A2:D2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A2:D2"].Style.Font.Bold = true;

                    sheet.Cells[$"A4:D4"].Value = "IKHTISAR JURNAL";
                    sheet.Cells[$"A4:D4"].Merge = true;
                    sheet.Cells[$"A4:D4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"A4:D4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A4:D4"].Style.Font.Bold = true;

                    sheet.Cells[$"C5"].Value = "BUKU HARIAN";
                    sheet.Cells[$"C5"].Style.Font.Bold = true;
                    sheet.Cells[$"D5"].Value = ": PENGELUARAN KAS BANK - IDR";
                    sheet.Cells[$"D5"].Style.Font.Bold = true;

                    sheet.Cells[$"C6"].Value = "PERIODE";
                    sheet.Cells[$"C6"].Style.Font.Bold = true;
                    sheet.Cells[$"D6"].Value = ": " + DateFrom.ToString("dd-MM-yyyy") + " S/D " + DateTo.ToString("dd-MM-yyyy");
                    sheet.Cells[$"D6"].Style.Font.Bold = true;

                    #endregion
                    sheet.Cells["A8"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);

                    //sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                }
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);

            return stream;
          
        }

        public List<LocalDPPVATBankExpenditureNoteMonthlyRecapViewModel> GetMonitoring(DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            var Query = GetReportQuery(dateFrom, dateTo, offset);
            return Query.ToList();
        }
        //

        public List<DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel> GetReportDetailQuery(DateTime? dateFrom, DateTime? dateTo, int offset)
        {

            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;

            List<DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel> data = new List<DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel>();
            List<DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel> data1 = new List<DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel>();

            var headerDebit = _dbContext.DPPVATBankExpenditureNotes
                              .Where(a => a.Date.AddHours(7).Date >= DateFrom.Date && a.Date.AddHours(7).Date <= DateTo.Date && a.CurrencyCode == "IDR")

                              .Select(a => new DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
                              {
                                  DocumentNo = a.DocumentNo,
                                  Date = a.Date,
                                  CurrencyCode = a.CurrencyCode,
                                  SupplierName = a.SupplierName,

                                  INNo = "-",
                                  INDate = DateTimeOffset.MinValue,
                         
                                  InvoiceNo = "-",
                                  InvoiceDate = DateTimeOffset.MinValue,
                                  ProductName = "-",
                                  BillNo = "-",
                                  PaymentBill = "-",

                                  AccountNo = a.BankAccountingCode.Substring(0, 2),
                                  AccountName = a.BankName,
                                  Debit = 0,
                                  Credit = Convert.ToDecimal(a.Amount)
                              }).OrderBy(x=> x.DocumentNo);


            var DtlData = (from a in _dbContext.DPPVATBankExpenditureNotes join
                           b in _dbContext.DPPVATBankExpenditureNoteItems on a.Id equals b.DPPVATBankExpenditureNoteId join
                           c in _dbContext.DPPVATBankExpenditureNoteDetails on b.Id equals c.DPPVATBankExpenditureNoteItemId
                           where a.Date.AddHours(7).Date >= DateFrom.Date && a.Date.AddHours(7).Date <= DateTo.Date && a.CurrencyCode == "IDR"

                          select new DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
                          {
                              DocumentNo = a.DocumentNo,
                              Date = a.Date,
                              CurrencyCode = a.CurrencyCode,
                              SupplierName = a.SupplierName,
                              INNo = b.InternalNoteNo,
                              INDate = b.InternalNoteDate,
                              InvoiceNo = c.InvoiceNo,
                              InvoiceDate = c.InvoiceDate,
                              ProductName = c.ProductNames,
                              BillNo = c.BillsNo,
                              PaymentBill = c.PaymentBills,
                              AccountNo = "300300",
                              AccountName = "HUTANG USAHA LOKAL",
                              Debit = Convert.ToDecimal(c.Amount),
                              Credit = 0,
                          }).OrderBy(x => x.DocumentNo).ThenBy(x => x.INNo).ThenBy(x => x.InvoiceNo);

            string DocNo = "";

            foreach (var i in headerDebit)
            {
               DocNo = i.DocumentNo;

                foreach (DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel x in DtlData.Where(x => x.DocumentNo == DocNo))
                {
                    var debit1 = new DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
                    {
                        DocumentNo = x.DocumentNo,
                        Date = x.Date,
                        CurrencyCode = x.CurrencyCode,
                        SupplierName = x.SupplierName,
                        INNo = x.INNo,
                        INDate = x.INDate,
                        InvoiceNo = x.InvoiceNo,
                        InvoiceDate = x.InvoiceDate,
                        ProductName = x.ProductName,
                        BillNo = x.BillNo,
                        PaymentBill = x.PaymentBill,
                        AccountNo = "300300",
                        AccountName = "HUTANG USAHA LOKAL",
                        Debit = Convert.ToDecimal(x.Debit),
                        Credit = 0,
                    };

                    data.Add(debit1);
                }


                var HdrData = new DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
                {
                    AccountNo = i.AccountNo.Substring(0, 2) == "HN" ? "101808" : i.AccountNo.Substring(0, 2) == "MD" ? "101804" : i.AccountNo.Substring(0, 2) == "CN" ? "101801" : "101805",
                    AccountName = i.AccountNo.Substring(0, 2) == "HN" ? "       KAS DI BANK HANA BANK" : i.AccountNo.Substring(0, 2) == "MD" ? "       KAS DI BANK MANDIRI EXIM (RP)" : i.AccountNo.Substring(0, 2) == "CN" ? "       KAS DI BANK CIMB NIAGA (RP)" : "       KAS DI BANK PANIN SOLO (RP)",
                    Credit = i.Credit,
                    Debit = i.Debit,
                    DocumentNo = i.DocumentNo,
                    Date = i.Date,
                    CurrencyCode = i.CurrencyCode,
                    SupplierName = i.SupplierName,
                };
                data.Add(HdrData);

            }

            DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel total = new DetailLocalDPPVATBankExpenditureNoteMonthlyRecapViewModel
            {
                DocumentNo = "",
                Date = DateTimeOffset.MinValue,
                CurrencyCode = "",
                SupplierName = "",

                INNo = "-",
                INDate = DateTimeOffset.MinValue,
    
                InvoiceNo = "-",
                InvoiceDate = DateTimeOffset.MinValue,
                ProductName = "-",
                BillNo = "-",
                PaymentBill = "-",

                AccountName = "",
                AccountNo = "TOTAL",
                Credit = headerDebit.Sum(a => a.Credit),
                Debit = headerDebit.Sum(a => a.Credit)
            };
            data.Add(total);

            return data;
        }

        public MemoryStream GenerateDetailExcel(DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;

            var Query = GetReportDetailQuery(dateFrom, dateTo, offset);

            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn() { ColumnName = "NO KASBON", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "TGL KASBON", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "NO AKUN ", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "NAMA PERKIRAAN", DataType = typeof(string) });

            result.Columns.Add(new DataColumn() { ColumnName = "SUPPLIER", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "KETERANGAN", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "NO NOTA INTERN", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "TGL NOTA INTERN", DataType = typeof(string) });

            result.Columns.Add(new DataColumn() { ColumnName = "NO INVOICE", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "TGL INVOICE", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "NO BILL", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "NO PAYMENTBILL", DataType = typeof(string) });

            result.Columns.Add(new DataColumn() { ColumnName = "MATA UANG", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "DEBET", DataType = typeof(decimal) });
            result.Columns.Add(new DataColumn() { ColumnName = "KREDIT", DataType = typeof(decimal) });


            ExcelPackage package = new ExcelPackage();
            if (Query.ToArray().Count() == 0)
            {
                result.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", 0, 0);
                bool styling = true;

                foreach (KeyValuePair<DataTable, String> item in new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") })
                {
                    var sheet = package.Workbook.Worksheets.Add(item.Value);

                    sheet.Column(1).Width = 25;
                    sheet.Column(2).Width = 15;
                    sheet.Column(3).Width = 15;
                    sheet.Column(4).Width = 30;

                    sheet.Column(5).Width = 30;
                    sheet.Column(6).Width = 30;
                    sheet.Column(7).Width = 30;
                    sheet.Column(8).Width = 15;

                    sheet.Column(9).Width = 30;
                    sheet.Column(10).Width = 15;
                    sheet.Column(11).Width = 30;
                    sheet.Column(12).Width = 30;

                    sheet.Column(13).Width = 10;
                    sheet.Column(14).Width = 20;
                    sheet.Column(15).Width = 20;
           
                    #region KopTable
                    sheet.Cells[$"A1:O1"].Value = "PT. DAN LIRIS";
                    sheet.Cells[$"A1:O1"].Merge = true;
                    sheet.Cells[$"A1:O1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A1:O1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A1:O1"].Style.Font.Bold = true;

                    sheet.Cells[$"A2:O2"].Value = "ACCOUNTING DEPT.";
                    sheet.Cells[$"A2:O2"].Merge = true;
                    sheet.Cells[$"A2:O2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A2:O2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A2:O2"].Style.Font.Bold = true;

                    sheet.Cells[$"A4:O4"].Value = "IKHTISAR JURNAL";
                    sheet.Cells[$"A4:O4"].Merge = true;
                    sheet.Cells[$"A4:O4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"A4:O4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A4:O4"].Style.Font.Bold = true;

                    sheet.Cells[$"H5"].Value = "BUKU HARIAN";
                    sheet.Cells[$"H5"].Style.Font.Bold = true;
                    sheet.Cells[$"H5"].Value = ": DETAIL PENGELUARAN KAS BANK - IDR";
                    sheet.Cells[$"H5"].Style.Font.Bold = true;

                    sheet.Cells[$"H6"].Value = "PERIODE";
                    sheet.Cells[$"H6"].Style.Font.Bold = true;
                    sheet.Cells[$"H6"].Value = ": " + DateFrom.ToString("dd-MM-yyyy") + " S/D " + DateTo.ToString("dd-MM-yyyy");
                    sheet.Cells[$"H6"].Style.Font.Bold = true;

                    #endregion
                    sheet.Cells["A8"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);

                    //sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                }
            }
            else
            {
                int index = 0;
                foreach (var d in Query)
                {
                    index++;
                    string DocDate = d.Date == new DateTime(1970, 1, 1) ? "-" : d.Date.ToOffset(new TimeSpan(offset, 0, 0)).ToString("MM/dd/yyyy", new CultureInfo("us-US"));
                    string InDate = d.INDate == new DateTime(1970, 1, 1) ? "-" : d.INDate.ToOffset(new TimeSpan(offset, 0, 0)).ToString("MM/dd/yyyy", new CultureInfo("us-US"));
                    string InvDate = d.InvoiceDate == new DateTime(1970, 1, 1) ? "-" : d.InvoiceDate.ToOffset(new TimeSpan(offset, 0, 0)).ToString("MM/dd/yyyy", new CultureInfo("us-US"));

                    result.Rows.Add(d.DocumentNo, DocDate, d.AccountNo, d.AccountName, d.SupplierName, d.ProductName, d.INNo, InDate, d.InvoiceNo, InvDate, d.BillNo, d.PaymentBill, d.CurrencyCode, d.Debit, d.Credit);
                }

                bool styling = true;

                foreach (KeyValuePair<DataTable, String> item in new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") })
                {
                    var sheet = package.Workbook.Worksheets.Add(item.Value);

                    sheet.Column(1).Width = 25;
                    sheet.Column(2).Width = 15;
                    sheet.Column(3).Width = 15;
                    sheet.Column(4).Width = 30;

                    sheet.Column(5).Width = 30;
                    sheet.Column(6).Width = 30;
                    sheet.Column(7).Width = 30;
                    sheet.Column(8).Width = 15;

                    sheet.Column(9).Width = 30;
                    sheet.Column(10).Width = 15;
                    sheet.Column(11).Width = 30;
                    sheet.Column(12).Width = 30;

                    sheet.Column(13).Width = 10;
                    sheet.Column(14).Width = 20;
                    sheet.Column(15).Width = 20;

                    #region KopTable
                    sheet.Cells[$"A1:O1"].Value = "PT. DAN LIRIS";
                    sheet.Cells[$"A1:O1"].Merge = true;
                    sheet.Cells[$"A1:O1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A1:O1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A1:O1"].Style.Font.Bold = true;

                    sheet.Cells[$"A2:O2"].Value = "ACCOUNTING DEPT.";
                    sheet.Cells[$"A2:O2"].Merge = true;
                    sheet.Cells[$"A2:O2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A2:O2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A2:O2"].Style.Font.Bold = true;

                    sheet.Cells[$"A4:O4"].Value = "IKHTISAR JURNAL";
                    sheet.Cells[$"A4:O4"].Merge = true;
                    sheet.Cells[$"A4:O4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"A4:O4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A4:O4"].Style.Font.Bold = true;

                    sheet.Cells[$"H5"].Value = "BUKU HARIAN";
                    sheet.Cells[$"H5"].Style.Font.Bold = true;
                    sheet.Cells[$"H5"].Value = ": DETAIL PENGELUARAN KAS BANK - IDR";
                    sheet.Cells[$"H5"].Style.Font.Bold = true;

                    sheet.Cells[$"H6"].Value = "PERIODE";
                    sheet.Cells[$"H6"].Style.Font.Bold = true;
                    sheet.Cells[$"H6"].Value = ": " + DateFrom.ToString("dd-MM-yyyy") + " S/D " + DateTo.ToString("dd-MM-yyyy");
                    sheet.Cells[$"H6"].Style.Font.Bold = true;

                    #endregion
                    sheet.Cells["A8"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);

                    //sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                }
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);

            return stream;

        }
    }
}
