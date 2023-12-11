using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalDebtorCard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalDebtorCard
{
    public class GarmentFinanceLocalDebtorCardReportService : IGarmentFinanceLocalDebtorCardReportService
    {
        private readonly IServiceProvider _serviceProvider;
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;

        public GarmentFinanceLocalDebtorCardReportService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _dbContext = dbContext;

            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }
        public List<GarmentLocalSalesNoteViewModel> GetDataLocalSalesNote(string type, int month, int year, string buyer)
        {
            var uri = APIEndpoint.PackingInventory + $"garment-shipping/local-sales-notes/finance-reports?type={type}&month={month}&year={year}&buyer={buyer}";

            var httpService = (IHttpClientService)_serviceProvider.GetService(typeof(IHttpClientService));
            List<GarmentLocalSalesNoteViewModel> salesNote = new List<GarmentLocalSalesNoteViewModel>();
            var http = _serviceProvider.GetService<IHttpClientService>();
            var response = httpService.GetAsync(uri).Result.Content.ReadAsStringAsync();
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Result);
            var json = result.Single(p => p.Key.Equals("data")).Value;
            var listData = JsonConvert.DeserializeObject<List<GarmentLocalSalesNoteViewModel>>(json.ToString());
            foreach (var item in listData)
            {
                salesNote.Add(item);
            }

            return salesNote;
        }
        public List<GarmentFinanceLocalDebtorCardReportViewModel> GetReportQuery(int month, int year, string buyer, int offset)
        {
            var SalesNoteNow = GetDataLocalSalesNote("now",month, year, buyer);
            var SalesNoteBalance = GetDataLocalSalesNote("balance", month, year, buyer);

            DateTime date = new DateTime(year, month, 1);
            List<GarmentFinanceLocalDebtorCardReportViewModel> data = new List<GarmentFinanceLocalDebtorCardReportViewModel>();

            var beginingMemo = from a in (from aa in _dbContext.GarmentFinanceMemorialDetailLocals where aa.MemorialDate.AddHours(7).Date < date select new { aa.Id })
                               join c in _dbContext.GarmentFinanceMemorialDetailLocalItems on a.Id equals c.MemorialDetailLocalId
                               where c.BuyerCode == buyer
                               select new GarmentFinanceLocalDebtorCardReportViewModel
                               {
                                   BeginAmount = Convert.ToDecimal(-c.Amount),
                                   SellAmount = 0,
                                   PaidAmount = 0,
                                   BalanceAmount = 0,
                                   Code = "AL"

                               };
            var beginingBankCashReceipt = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetailLocals where aa.BankCashReceiptDate.AddHours(7).Date < date select new { aa.Id })
                                          join b in _dbContext.GarmentFinanceBankCashReceiptDetailLocalItems on a.Id equals b.BankCashReceiptDetailLocalId
                                          where b.BuyerCode == buyer
                                          select new GarmentFinanceLocalDebtorCardReportViewModel
                                          {
                                              BeginAmount = Convert.ToDecimal(-b.Amount),
                                              SellAmount = 0,
                                              PaidAmount = 0,
                                              BalanceAmount = 0,
                                              Code = "AL"

                                          };
            var beginingInvoice = from a in SalesNoteBalance
                                  select new GarmentFinanceLocalDebtorCardReportViewModel
                                  {
                                      BeginAmount = Convert.ToDecimal(a.Amount),
                                      SellAmount = 0,
                                      PaidAmount = 0,
                                      BalanceAmount = 0,
                                      Code = "AL"

                                  };
            var beginingBalance = from a in _dbContext.GarmentLocalDebiturBalances
                                  where a.BuyerCode==buyer
                                  select new GarmentFinanceLocalDebtorCardReportViewModel
                                  {
                                      BeginAmount = Convert.ToDecimal(a.BalanceAmount),
                                      SellAmount = 0,
                                      PaidAmount = 0,
                                      BalanceAmount = 0,
                                      Code = "AL"

                                  };
            var salesNow = from a in SalesNoteNow
                           select new GarmentFinanceLocalDebtorCardReportViewModel
                           {
                               SellAmount = Convert.ToDecimal(a.Amount),
                               BeginAmount = 0,
                               PaidAmount = 0,
                               BalanceAmount = 0,
                               Code = "JL",
                               SalesNoteNo = a.SalesNoteNo,
                               Date = a.Date
                           };
            var memoNow = from a in (from aa in _dbContext.GarmentFinanceMemorialDetailLocals where aa.MemorialDate.AddHours(7).Date.Month == month && aa.MemorialDate.AddHours(7).Date.Year == year select new { aa.Id, aa.MemorialNo, aa.MemorialDate })
                          join c in _dbContext.GarmentFinanceMemorialDetailLocalItems on a.Id equals c.MemorialDetailLocalId
                          where c.BuyerCode == buyer
                          select new GarmentFinanceLocalDebtorCardReportViewModel
                          {
                              PaidAmount = Convert.ToDecimal(c.Amount),
                              BeginAmount = 0,
                              SellAmount = 0,
                              BalanceAmount = 0,
                              Code = "BY",
                              SalesNoteNo = c.LocalSalesNoteNo,
                              Date = a.MemorialDate,
                              ReceiptNo = a.MemorialNo
                          };
            var bankCashReceiptNow = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetailLocals where aa.BankCashReceiptDate.AddHours(7).Date.Month == month && aa.BankCashReceiptDate.AddHours(7).Date.Year == year select new { aa.Id, aa.BankCashReceiptNo, aa.BankCashReceiptDate })
                                     join b in _dbContext.GarmentFinanceBankCashReceiptDetailLocalItems on a.Id equals b.BankCashReceiptDetailLocalId
                                     where b.BuyerCode == buyer
                                     select new GarmentFinanceLocalDebtorCardReportViewModel
                                     {
                                         PaidAmount = Convert.ToDecimal(b.Amount),
                                         BeginAmount = 0,
                                         SellAmount = 0,
                                         BalanceAmount = 0,
                                         Code = "BY",
                                         SalesNoteNo = b.LocalSalesNoteNo,
                                         Date = a.BankCashReceiptDate,
                                         ReceiptNo = a.BankCashReceiptNo
                                     };

            var beginBalanceQuery = beginingBalance.Union(beginingMemo).Union(beginingBankCashReceipt).Union(beginingInvoice);
            var beginBalance = beginBalanceQuery.Sum(a => a.BeginAmount);

            var firstRow = new GarmentFinanceLocalDebtorCardReportViewModel
            {
                Code = "AL",
                Date = null,
                SalesNoteDate = null,
                SalesNoteNo = null,
                SalesNoteNoJL = null,
                SalesNoteBY = null,
                BeginAmount = 0,
                BalanceAmount = beginBalance,
                PaidAmount = 0,
                SellAmount = 0,
                ReceiptNo = null,
            };
            data.Add(firstRow);

            var queryUnion = memoNow.Union(bankCashReceiptNow).Union(salesNow);

            var querySum = queryUnion.ToList()
                   .GroupBy(x => new { x.Code, x.SalesNoteNo, x.Date, x.ReceiptNo }, (key, group) => new
                   {
                       code = key.Code,
                       invoiceNo = key.SalesNoteNo,
                       beginingBalance = group.Sum(s => s.BeginAmount),
                       sell = group.Sum(s => s.SellAmount),
                       buy = group.Sum(s => s.PaidAmount),
                       date = key.Date,
                       receiptNo = key.ReceiptNo
                   }).OrderBy(s => s.date).ThenByDescending(s=>s.sell);
            decimal SumBY = 0;
            decimal SumJL = 0;

            foreach (var item in querySum)
            {
                GarmentFinanceLocalDebtorCardReportViewModel model = new GarmentFinanceLocalDebtorCardReportViewModel
                {
                    Code = item.code,
                    SalesNoteBY = item.code == "BY" ? item.invoiceNo : null,
                    SalesNoteNoJL = item.code == "JL" ? item.invoiceNo : null,
                    JLDate = item.code == "JL" ? item.date : null,
                    BYDate = item.code == "BY" ? item.date : null,
                    SellAmount = item.sell,
                    PaidAmount = item.buy,
                    BalanceAmount = beginBalance + item.sell - item.buy,
                    ReceiptNo = item.receiptNo,
                };
                data.Add(model);
                beginBalance = beginBalance + item.sell - item.buy;
                SumBY += item.buy;
                SumJL += item.sell;
            }

            var lastRow = new GarmentFinanceLocalDebtorCardReportViewModel
            {
                Code = "Saldo",
                Date = null,
                SalesNoteDate = null,
                SalesNoteNo = null,
                SalesNoteNoJL = null,
                SalesNoteBY = null,
                BeginAmount = 0,
                BalanceAmount = beginBalance,
                PaidAmount = SumBY,
                SellAmount = SumJL,
                ReceiptNo = null,
            };
            data.Add(lastRow);
            return data;
        }
        public Tuple<MemoryStream, string> GenerateExcel(int month, int year, string buyer, int offset)
        {
            var Query = GetReportQuery(month, year, buyer, offset);

            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn() { ColumnName = "Code", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No. Nota Penjualan", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Kwitansi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No. Kwitansi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No. Nota Penjualan (dibayar)", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah Kwitansi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Saldo", DataType = typeof(double) });
            if (Query.ToArray().Count() == 0)
                result.Rows.Add("", "", "", "", "", "", "", "", 0); // to allow column name to be generated properly for empty data as template
            else
            {
                foreach (var item in Query)
                {
                    string BYDate = item.BYDate == null ? "" : item.BYDate.GetValueOrDefault().AddHours(offset).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    string JLDate = item.JLDate == null ? "" : item.JLDate.GetValueOrDefault().AddHours(offset).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    string JLAmount = item.SellAmount > 0 ? string.Format("{0:n2}", item.SellAmount) : "";
                    string BYAmount = item.PaidAmount > 0 ? string.Format("{0:n2}", item.PaidAmount) : "";
                    result.Rows.Add(item.Code, JLDate, item.SalesNoteNoJL, JLAmount, BYDate, item.ReceiptNo, item.SalesNoteBY, BYAmount, item.BalanceAmount);
                }

            }
            ExcelPackage package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Report Kartu Debitur Lokal");
            sheet.Cells["A1"].LoadFromDataTable(result, true, OfficeOpenXml.Table.TableStyles.Light16);

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Cells[sheet.Dimension.Address].Style.WrapText = true;

            MemoryStream streamExcel = new MemoryStream();
            package.SaveAs(streamExcel);

            //Dictionary<string, string> FilterDictionary = new Dictionary<string, string>(JsonConvert.DeserializeObject<Dictionary<string, string>>(filter), StringComparer.OrdinalIgnoreCase);
            string fileName = string.Concat("Report Kartu Debitur Lokal", DateTime.Now.Date, ".xlsx");

            return Tuple.Create(streamExcel, fileName);
        }

        public List<GarmentFinanceLocalDebtorCardReportViewModel> GetMonitoring(int month, int year, string buyer, int offset)
        {
            var Data = GetReportQuery(month, year, buyer, offset);
            return Data;
        }
    }
}
