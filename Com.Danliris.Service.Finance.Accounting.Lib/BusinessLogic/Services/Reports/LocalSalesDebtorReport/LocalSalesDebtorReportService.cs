using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Rreports.ExportSalesDebtorReportController;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.Reports.LocalSalesDebtorReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetailLocal;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Reports.LocalSalesDebtorReport;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports.ExportSalesDebtorReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Newtonsoft.Json;
using static Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports.ExportSalesDebtorReport.GarmentShippingPackingList;
using static Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Reports.LocalSalesDebtorReport.GarmentShippingLocalSalesNote;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Reports.LocalSalesDebtorReport
{
    public class LocalSalesDebtorReportService : ILocalSalesDebtorReportService
    {
        private readonly IServiceProvider _serviceProvider;
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        protected DbSet<GarmentFinanceMemorialDetailLocalModel> DbSetMemoDetailLocal;
        protected DbSet<GarmentFinanceMemorialDetailLocalItemModel> DbSetDetailLocalItemMemo;
        protected DbSet<GarmentFinanceBankCashReceiptDetailLocalItemModel> DbSetBankCashDetailLocalItem;
        protected DbSet<GarmentFinanceBankCashReceiptDetailLocalModel> DBSetBankCashDetailLocal;

        public LocalSalesDebtorReportService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public async Task<MemoryStream> GenerateExcel(int month, int year)
        {
            var data = await GetReportQuery(month, year);

            DataTable result = new DataTable();
            string monthValue = "";
            switch (month)
            {
                case 1:
                    monthValue = "JANUARI";
                    break;
                case 2:
                    monthValue = "FEBRUARI";
                    break;
                case 3:
                    monthValue = "MARET";
                    break;
                case 4:
                    monthValue = "APRIL";
                    break;
                case 5:
                    monthValue = "MEI";
                    break;
                case 6:
                    monthValue = "JUNI";
                    break;
                case 7:
                    monthValue = "JULI";
                    break;
                case 8:
                    monthValue = "AGUSTUS";
                    break;
                case 9:
                    monthValue = "SEPTEMBER";
                    break;
                case 10:
                    monthValue = "OKTOBER";
                    break;
                case 11:
                    monthValue = "NOVEMBER";
                    break;
                default:
                    monthValue = "DESEMBER";
                    break;
            }
            result.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Kode", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama Buyer", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Saldo Awal", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Penjualan", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Penerimaan", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Saldo Akhir", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Umur Piutang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Umur Piutang1", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Umur Piutang2", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Umur Piutang3", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Umur Piutang4", DataType = typeof(String) });

            int counter = 0;
            result.Rows.Add("",
                    "", "", "", "", "", "", "Lancar", "1 - 30 hari", " 31- 60 hari", "61 - 90 hari", "> 90 hari");
            if (data.Count() == 0)
                result.Rows.Add("", "", "", 0, 0, 0, 0, 0, 0, 0); // to allow column name to be generated properly for empty data as template
            else
            {

                foreach (var item in data)
                {
                    counter++;
                    result.Rows.Add(item.index, item.buyerCode, item.buyerName, Math.Round(item.beginingBalance, 2), Math.Round(item.sales, 2), Math.Round(item.receipt, 2), Math.Round(item.endBalance, 2), Math.Round(item.normal, 2), Math.Round(item.oneThirty, 2), Math.Round(item.thirtySixty, 2), Math.Round(item.sixtyNinety, 2), Math.Round(item.moreThanNinety, 2));
                }
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A1"].Value = "BULAN " + monthValue + " " + year;
                worksheet.Cells["A1"].Style.Font.Size = 14;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A2"].LoadFromDataTable(result, true);
                worksheet.Cells["A" + 2 + ":A" + 3 + ""].Merge = true;
                worksheet.Cells["B" + 2 + ":B" + 3 + ""].Merge = true;
                worksheet.Cells["C" + 2 + ":C" + 3 + ""].Merge = true;
                worksheet.Cells["D" + 2 + ":D" + 3 + ""].Merge = true;
                worksheet.Cells["E" + 2 + ":E" + 3 + ""].Merge = true;
                worksheet.Cells["F" + 2 + ":F" + 3 + ""].Merge = true;
                worksheet.Cells["G" + 2 + ":G" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":L" + (counter + 3) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 2 + ":L" + (counter + 3) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 2 + ":L" + (counter + 3) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 2 + ":L" + (counter + 3) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 2 + ":L" + 3 + ""].Style.Font.Bold = true;
                worksheet.Cells["H" + 2 + ":L" + 2 + ""].Merge = true;

                worksheet.Cells["A" + 2 + ":L" + 2 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                foreach (var cell in worksheet.Cells["D" + 4 + ":L" + (counter + 3) + ""])
                {
                    cell.Value = Convert.ToDecimal(cell.Value);
                    cell.Style.Numberformat.Format = "#,##0.00";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                worksheet.Cells["A" + 2 + ":L" + (counter + 3) + ""].AutoFitColumns();
                worksheet.Cells["A" + (counter + 3) + ":L" + (counter + 3) + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + (counter + 3) + ":C" + (counter + 3) + ""].Merge = true;
                worksheet.Cells["A" + (counter + 3)].Value = "TOTAL";

                var stream = new MemoryStream();

                package.SaveAs(stream);
                return stream;
            }
        }

        public async Task<List<LocalSalesDebtorReportViewModel>> GetMonitoring(int month, int year, int offset)
        {
            var data = await GetReportQuery(month, year);
            return data;
        }

        public async Task<List<LocalSalesDebtorReportViewModel>> GetReportQuery(int month, int year)
        {
            GarmentShippingLocalSalesNote salesBalanceNow = await GetDataLocalSalesNote("now", month, year);
            GarmentShippingLocalSalesNote salesBalance = await GetDataLocalSalesNote("balance", month, year);

            List<LocalSalesDebtorReportViewModel> data = new List<LocalSalesDebtorReportViewModel>();
            
            var _sales = salesBalance.data.Union(salesBalanceNow.data);
            var querytimeSpan = from aa in _sales
                                select new timeSpanInvoice
                                {
                                    buyerCode = aa.buyer.Code,
                                    amount = Convert.ToDecimal(aa.amount),
                                    type = (aa.tempo <= 0) ? "normal" :
                                            (aa.tempo > 0 && aa.tempo < 31) ? "oneThirty" :
                                            (aa.tempo > 30 && aa.tempo < 61) ? "thirtySixty" :
                                            (aa.tempo > 60 && aa.tempo < 91) ? "sixtyNinety" :
                                            "moreThanNinety",
                                    localSalesNoteId = Convert.ToInt32(aa.Id)
                                };

            var querySumInvoice = querytimeSpan.ToList()
               .GroupBy(x => new { x.buyerCode, x.type }, (key, group) => new
               {
                   buyerCode = key.buyerCode,
                   type = key.type,
                   amount = group.Sum(s => s.amount)
               });

            var receiptBankCashReceiptDetailLocal = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetailLocals where aa.BankCashReceiptDate.AddHours(7).Month == month && aa.BankCashReceiptDate.AddHours(7).Year == year select new { aa.Id })
                                                    join b in _dbContext.GarmentFinanceBankCashReceiptDetailLocalItems on a.Id equals b.BankCashReceiptDetailLocalId

                                                    select new LocalSalesDebtorReportViewModel
                                                    {
                                                        buyerCode = b.BuyerCode.TrimEnd(),
                                                        buyerName = b.BuyerName.TrimEnd(),
                                                        beginingBalance = 0,
                                                        receipt = Convert.ToDouble(b.Amount),
                                                        sales = 0,
                                                        endBalance = 0,
                                                        normal = 0,
                                                        oneThirty = 0,
                                                        thirtySixty = 0,
                                                        sixtyNinety = 0,
                                                        moreThanNinety = 0
                                                    };

            var receiptMemorialDetailLocal = from a in (from aa in _dbContext.GarmentFinanceMemorialDetailLocals where aa.MemorialDate.AddHours(7).Month == month && aa.MemorialDate.AddHours(7).Year == year select new { aa.Id })
                                             join c in _dbContext.GarmentFinanceMemorialDetailLocalItems on a.Id equals c.MemorialDetailLocalId

                                             select new LocalSalesDebtorReportViewModel
                                             {
                                                 buyerCode = c.BuyerCode.TrimEnd(),
                                                 buyerName = c.BuyerName.TrimEnd(),
                                                 beginingBalance = 0,
                                                 receipt = Convert.ToDouble(c.Amount),
                                                 sales = 0,
                                                 endBalance = 0,
                                                 normal = 0,
                                                 oneThirty = 0,
                                                 thirtySixty = 0,
                                                 sixtyNinety = 0,
                                                 moreThanNinety = 0
                                             };

            var salesBalanceLocal = from a in salesBalanceNow.data

                                    select new LocalSalesDebtorReportViewModel
                                    {
                                        buyerCode = a.buyer.Code.TrimEnd(),
                                        buyerName = a.buyer.Name.TrimEnd(),
                                        beginingBalance = 0,
                                        receipt = 0,
                                        sales = a.amount,
                                        endBalance = 0,
                                        normal = 0,
                                        oneThirty = 0,
                                        thirtySixty = 0,
                                        sixtyNinety = 0,
                                        moreThanNinety = 0
                                    };

            var localDebtorBalance = from a in _dbContext.GarmentLocalDebiturBalances

                                     select new LocalSalesDebtorReportViewModel
                                     {
                                         buyerCode = a.BuyerCode.TrimEnd(),
                                         buyerName = a.BuyerName.TrimEnd(),
                                         beginingBalance = Convert.ToDecimal(a.BalanceAmount),
                                         receipt = 0,
                                         sales = 0,
                                         endBalance = 0,
                                         normal = 0,
                                         oneThirty = 0,
                                         thirtySixty = 0,
                                         sixtyNinety = 0,
                                         moreThanNinety = 0
                                     };

            var beginningBalanceSales = from a in salesBalance.data

                                        select new LocalSalesDebtorReportViewModel
                                        {
                                            buyerCode = a.buyer.Code.TrimEnd(),
                                            buyerName = a.buyer.Name.TrimEnd(),
                                            beginingBalance = Convert.ToDecimal(a.amount),
                                            receipt = 0,
                                            sales = 0,
                                            endBalance = 0,
                                            normal = 0,
                                            oneThirty = 0,
                                            thirtySixty = 0,
                                            sixtyNinety = 0,
                                            moreThanNinety = 0
                                        };

            var beginningBalanceReceiptBankCashReceiptDetailLocal = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetailLocals where aa.BankCashReceiptDate.AddHours(7).Month < month && aa.BankCashReceiptDate.AddHours(7).Year == year select new { aa.Id })
                                                                    join b in _dbContext.GarmentFinanceBankCashReceiptDetailLocalItems on a.Id equals b.BankCashReceiptDetailLocalId

                                                                    select new LocalSalesDebtorReportViewModel
                                                                    {
                                                                        buyerCode = b.BuyerCode.TrimEnd(),
                                                                        buyerName = b.BuyerName.TrimEnd(),
                                                                        beginingBalance = -b.Amount,
                                                                        receipt = 0,
                                                                        sales = 0,
                                                                        endBalance = 0,
                                                                        normal = 0,
                                                                        oneThirty = 0,
                                                                        thirtySixty = 0,
                                                                        sixtyNinety = 0,
                                                                        moreThanNinety = 0
                                                                    };

            var beginningBalanceReceiptMemorialDetailLocal = from a in (from aa in _dbContext.GarmentFinanceMemorialDetailLocals where aa.MemorialDate.AddHours(7).Month < month && aa.MemorialDate.AddHours(7).Year == year select new { aa.Id })
                                                             join c in _dbContext.GarmentFinanceMemorialDetailLocalItems on a.Id equals c.MemorialDetailLocalId

                                                             select new LocalSalesDebtorReportViewModel
                                                             {
                                                                 buyerCode = c.BuyerCode.TrimEnd(),
                                                                 buyerName = c.BuyerName.TrimEnd(),
                                                                 beginingBalance = -c.Amount,
                                                                 receipt = 0,
                                                                 sales = 0,
                                                                 endBalance = 0,
                                                                 normal = 0,
                                                                 oneThirty = 0,
                                                                 thirtySixty = 0,
                                                                 sixtyNinety = 0,
                                                                 moreThanNinety = 0
                                                             };

            var periodeMemoLocal = from a in (from aa in _dbContext.GarmentFinanceMemorialDetailLocals where aa.MemorialDate.AddHours(7).Month <= month && aa.MemorialDate.AddHours(7).Year == year select new { aa.Id })
                                   join c in _dbContext.GarmentFinanceMemorialDetailLocalItems on a.Id equals c.MemorialDetailLocalId

                                   select new LocalSalesDebtorReportViewModel
                                   {
                                       buyerCode = c.BuyerCode.TrimEnd(),
                                       buyerName = c.BuyerName.TrimEnd(),
                                       beginingBalance = 0,
                                       receipt = 0,
                                       sales = 0,
                                       endBalance = 0,
                                       normal = (from aa in querytimeSpan where aa.localSalesNoteId == c.LocalSalesNoteId select aa.type).FirstOrDefault() == "normal" ? Convert.ToDouble(-c.Amount) : 0,
                                       oneThirty = (from aa in querytimeSpan where aa.localSalesNoteId == c.LocalSalesNoteId select aa.type).FirstOrDefault() == "oneThirty" ? Convert.ToDouble(-c.Amount) : 0,
                                       thirtySixty = (from aa in querytimeSpan where aa.localSalesNoteId == c.LocalSalesNoteId select aa.type).FirstOrDefault() == "thirtySixty" ? Convert.ToDouble(-c.Amount) : 0,
                                       sixtyNinety = (from aa in querytimeSpan where aa.localSalesNoteId == c.LocalSalesNoteId select aa.type).FirstOrDefault() == "sixtyNinety" ? Convert.ToDouble(-c.Amount) : 0,
                                       moreThanNinety = (from aa in querytimeSpan where aa.localSalesNoteId == c.LocalSalesNoteId select aa.type).FirstOrDefault() == "moreThanNinety" ? Convert.ToDouble(-c.Amount) : 0,

                                   };

            var periodeBankCashReceiptLocal = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetailLocals where aa.BankCashReceiptDate.AddHours(7).Month <= month && aa.BankCashReceiptDate.AddHours(7).Year == year select new { aa.Id })
                                              join b in _dbContext.GarmentFinanceBankCashReceiptDetailLocalItems on a.Id equals b.BankCashReceiptDetailLocalId

                                              select new LocalSalesDebtorReportViewModel
                                              {
                                                  buyerCode = b.BuyerCode.TrimEnd(),
                                                  buyerName = b.BuyerName.TrimEnd(),
                                                  beginingBalance = 0,
                                                  receipt = 0,
                                                  sales = 0,
                                                  endBalance = 0,
                                                  normal = (from aa in querytimeSpan where aa.localSalesNoteId == b.LocalSalesNoteId select aa.type).FirstOrDefault() == "normal" ? Convert.ToDouble(-b.Amount) : 0,
                                                  oneThirty = (from aa in querytimeSpan where aa.localSalesNoteId == b.LocalSalesNoteId select aa.type).FirstOrDefault() == "oneThirty" ? Convert.ToDouble(-b.Amount) : 0,
                                                  thirtySixty = (from aa in querytimeSpan where aa.localSalesNoteId == b.LocalSalesNoteId select aa.type).FirstOrDefault() == "thirtySixty" ? Convert.ToDouble(-b.Amount) : 0,
                                                  sixtyNinety = (from aa in querytimeSpan where aa.localSalesNoteId == b.LocalSalesNoteId select aa.type).FirstOrDefault() == "sixtyNinety" ? Convert.ToDouble(-b.Amount) : 0,
                                                  moreThanNinety = (from aa in querytimeSpan where aa.localSalesNoteId == b.LocalSalesNoteId select aa.type).FirstOrDefault() == "moreThanNinety" ? Convert.ToDouble(-b.Amount) : 0,

                                              };

            var queryUnion = localDebtorBalance
                .Union(salesBalanceLocal)
                .Union(receiptMemorialDetailLocal)
                .Union(receiptBankCashReceiptDetailLocal)
                .Union(beginningBalanceSales)
                .Union(beginningBalanceReceiptBankCashReceiptDetailLocal)
                .Union(beginningBalanceReceiptMemorialDetailLocal)
                .Union(periodeMemoLocal)
                .Union(periodeBankCashReceiptLocal);

            var querySum = queryUnion.ToList()
                   .GroupBy(x => new { x.buyerCode, x.buyerName }, (key, group) => new
                   {
                       buyerCode = key.buyerCode,
                       buyerName = key.buyerName,
                       beginingBalance = group.Sum(s => s.beginingBalance),
                       receipt = group.Sum(s => s.receipt),
                       sales = group.Sum(s => s.sales),
                       endBalance = group.Sum(s => s.endBalance),
                       normal = group.Sum(s => s.normal),
                       oneThirty = group.Sum(s => s.oneThirty),
                       thirtySixty = group.Sum(s => s.thirtySixty),
                       sixtyNinety = group.Sum(s => s.sixtyNinety),
                       moreThanNinety = group.Sum(s => s.moreThanNinety)
                   }).OrderByDescending(s => s.buyerName);
            int index = 1;
            foreach (var item in querySum.OrderBy(a => a.buyerName))
            {
                LocalSalesDebtorReportViewModel model = new LocalSalesDebtorReportViewModel
                {
                    index = index.ToString(),
                    buyerCode = item.buyerCode,
                    buyerName = item.buyerName,
                    beginingBalance = item.beginingBalance,
                    receipt = item.receipt,
                    sales = item.sales,
                    endBalance = Convert.ToDouble(item.beginingBalance) + item.sales - item.receipt,
                    normal = item.normal + Convert.ToDouble((from aa in querySumInvoice where aa.buyerCode == item.buyerCode && aa.type == "normal" select aa.amount).FirstOrDefault()),
                    oneThirty = item.oneThirty + Convert.ToDouble((from aa in querySumInvoice where aa.buyerCode == item.buyerCode && (aa.type == "oneThirty") select aa.amount).FirstOrDefault()),
                    thirtySixty = item.thirtySixty + Convert.ToDouble((from aa in querySumInvoice where aa.buyerCode == item.buyerCode && (aa.type == "thirtySixty") select aa.amount).FirstOrDefault()),
                    sixtyNinety = item.sixtyNinety + Convert.ToDouble((from aa in querySumInvoice where aa.buyerCode == item.buyerCode && (aa.type == "sixtyNinety") select aa.amount).FirstOrDefault()),
                    moreThanNinety = item.moreThanNinety + Convert.ToDouble((from aa in querySumInvoice where aa.buyerCode == item.buyerCode && (aa.type == "moreThanNinety") select aa.amount).FirstOrDefault()),
                    total = "TOTAL"
                };
                data.Add(model);
                index++;
            }

            var queryTOTAL = data.ToList()
                   .GroupBy(x => new { x.total }, (key, group) => new
                   {

                       beginingBalance = group.Sum(s => s.beginingBalance),
                       receipt = group.Sum(s => s.receipt),
                       sales = group.Sum(s => s.sales),
                       endBalance = group.Sum(s => s.endBalance),
                       normal = group.Sum(s => s.normal),
                       oneThirty = group.Sum(s => s.oneThirty),
                       thirtySixty = group.Sum(s => s.thirtySixty),
                       sixtyNinety = group.Sum(s => s.sixtyNinety),
                       moreThanNinety = group.Sum(s => s.moreThanNinety)
                   });
            foreach (var item in queryTOTAL)
            {
                LocalSalesDebtorReportViewModel model = new LocalSalesDebtorReportViewModel
                {
                    index = "",
                    buyerCode = "TOTAL",
                    buyerName = "",
                    beginingBalance = item.beginingBalance,
                    receipt = item.receipt,
                    sales = item.sales,
                    endBalance = item.endBalance,
                    normal = item.normal,
                    oneThirty = item.oneThirty,
                    thirtySixty = item.thirtySixty,
                    sixtyNinety = item.sixtyNinety,
                    moreThanNinety = item.moreThanNinety
                };
                data.Add(model);
            }

            return data;
        }

        public async Task<GarmentShippingLocalSalesNote> GetDataLocalSalesNote(string type, int month, int year)
        {
            GarmentShippingLocalSalesNote garmentShippingLocalSalesNote = new GarmentShippingLocalSalesNote();
            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.PackingInventory + $"garment-shipping/local-sales-notes/localSalesDebtor?type={type}&month={month}&year={year}";
            var response = await http.GetAsync(uri);


            var result = new BaseResponse<string>();

            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                    var dataString = content.GetValueOrDefault("data").ToString();
                    var listData = JsonConvert.DeserializeObject<List<ShippingLocalSalesNoteDto>>(dataString);
                    garmentShippingLocalSalesNote.data = listData;
                }
            }
            else
            {
                var err = await response.Content.ReadAsStringAsync();
            }


            return garmentShippingLocalSalesNote;
        }

        class timeSpanInvoice
        {
            internal string buyerCode { get; set; }
            internal decimal amount { get; set; }
            internal string type { get; set; }
            internal int localSalesNoteId { get; set; }
        }

        public async Task<MemoryStream> GenerateExcelSummary(int month, int year)
        {
            var data = await GetReportQuery(month, year);

            DataTable result = new DataTable();
            string monthValue = "";
            switch (month)
            {
                case 1:
                    monthValue = "JANUARI";
                    break;
                case 2:
                    monthValue = "FEBRUARI";
                    break;
                case 3:
                    monthValue = "MARET";
                    break;
                case 4:
                    monthValue = "APRIL";
                    break;
                case 5:
                    monthValue = "MEI";
                    break;
                case 6:
                    monthValue = "JUNI";
                    break;
                case 7:
                    monthValue = "JULI";
                    break;
                case 8:
                    monthValue = "AGUSTUS";
                    break;
                case 9:
                    monthValue = "SEPTEMBER";
                    break;
                case 10:
                    monthValue = "OKTOBER";
                    break;
                case 11:
                    monthValue = "NOVEMBER";
                    break;
                default:
                    monthValue = "DESEMBER";
                    break;
            }

            result.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Kode", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama Buyer", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Saldo Akhir", DataType = typeof(String) });

            int counter = 0;
            result.Rows.Add("",
                    "", "", "");
            if (data.Count() == 0)
                result.Rows.Add("", "", "", 0); // to allow column name to be generated properly for empty data as template
            else
            {

                foreach (var item in data)
                {
                    counter++;
                    result.Rows.Add(item.index, item.buyerCode, item.buyerName, Math.Round(item.endBalance, 2));
                }
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A1"].Value = "BULAN " + monthValue + " " + year;
                worksheet.Cells["A1"].Style.Font.Size = 14;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A2"].LoadFromDataTable(result, true);
                worksheet.Cells["A" + 2 + ":A" + 3 + ""].Merge = true;
                worksheet.Cells["B" + 2 + ":B" + 3 + ""].Merge = true;
                worksheet.Cells["C" + 2 + ":C" + 3 + ""].Merge = true;
                worksheet.Cells["D" + 2 + ":D" + 3 + ""].Merge = true;
                worksheet.Cells["A" + 2 + ":D" + (counter + 3) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 2 + ":D" + (counter + 3) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 2 + ":D" + (counter + 3) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 2 + ":D" + (counter + 3) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A" + 2 + ":D" + 3 + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + 2 + ":D" + 2 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                foreach (var cell in worksheet.Cells["D" + 4 + ":D" + (counter + 3) + ""])
                {
                    cell.Value = Convert.ToDecimal(cell.Value);
                    cell.Style.Numberformat.Format = "#,##0.00";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                worksheet.Cells["A" + 2 + ":D" + (counter + 3) + ""].AutoFitColumns();
                worksheet.Cells["A" + (counter + 3) + ":D" + (counter + 3) + ""].Style.Font.Bold = true;
                worksheet.Cells["A" + (counter + 3) + ":C" + (counter + 3) + ""].Merge = true;

                var stream = new MemoryStream();
                package.SaveAs(stream);

                return stream;
            }

        }
    }
}
