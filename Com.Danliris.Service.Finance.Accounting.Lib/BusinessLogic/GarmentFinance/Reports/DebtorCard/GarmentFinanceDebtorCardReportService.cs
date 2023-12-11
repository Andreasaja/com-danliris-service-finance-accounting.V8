using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.DebtorCard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;
using static Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.DebtorCard.GarmentShippingInvoiceViewModel;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.DebtorCard
{
    public class GarmentFinanceDebtorCardReportService : IGarmentFinanceDebtorCardReportService
    {
        private readonly IServiceProvider _serviceProvider;
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        protected DbSet<GarmentFinanceMemorialDetailModel> DbSetMemoDetail;
        protected DbSet<GarmentFinanceMemorialDetailItemModel> DbSetDetailItemMemo;
        protected DbSet<BankCashReceiptDetailItemModel> DbSetBankCashDetailItem;
        protected DbSet<BankCashReceiptDetailModel> DBSetBankCashDetail;



        public GarmentFinanceDebtorCardReportService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _dbContext = dbContext;

            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public List<GarmentShippingInvoiceViewModel> GetDataShippingInvoice(int month, int year, string buyer)
        {
            var uri = APIEndpoint.PackingInventory + $"garment-shipping/invoices/packing-list-for-debtor-card?month={month}&year={year}&buyer={buyer}";
            
            var httpService = (IHttpClientService)_serviceProvider.GetService(typeof(IHttpClientService));
            List<GarmentShippingInvoiceViewModel> garmentShipping = new List<GarmentShippingInvoiceViewModel>();
            var http = _serviceProvider.GetService<IHttpClientService>();
            var response = httpService.GetAsync(uri).Result.Content.ReadAsStringAsync();
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Result);
            var json = result.Single(p => p.Key.Equals("data")).Value;
            var listData = JsonConvert.DeserializeObject<List<GarmentShippingInvoiceViewModel>>(json.ToString());
            foreach (var item in listData)
            {
                garmentShipping.Add(item);
            }

            return garmentShipping;
        }
        public List<GarmentShippingInvoiceViewModel> GetDataShippingInvoiceNow(int month, int year, string buyer)
        {
            var uri = APIEndpoint.PackingInventory + $"garment-shipping/invoices/packing-list-for-debtor-card-now?month={month}&year={year}&buyer={buyer}";
            var httpService = (IHttpClientService)_serviceProvider.GetService(typeof(IHttpClientService));
            List<GarmentShippingInvoiceViewModel> garmentShipping = new List<GarmentShippingInvoiceViewModel>();
            var http = _serviceProvider.GetService<IHttpClientService>();
            var response = httpService.GetAsync(uri).Result.Content.ReadAsStringAsync();
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Result);
            var json = result.Single(p => p.Key.Equals("data")).Value;
            var listData = JsonConvert.DeserializeObject<List<GarmentShippingInvoiceViewModel>>(json.ToString());
            foreach (var item in listData)
            {
                garmentShipping.Add(item);
            }

            return garmentShipping;
        }

        public List<GarmentShippingInvoiceViewModel> GetDataBalance(string buyer)
        {
            var httpService = (IHttpClientService)_serviceProvider.GetService(typeof(IHttpClientService));
            List<GarmentShippingInvoiceViewModel> garmentShipping = new List<GarmentShippingInvoiceViewModel>();
            var http = _serviceProvider.GetService<IHttpClientService>();
            Dictionary<string, object> filter = new Dictionary<string, object> { { "BuyerAgentCode", buyer } };
            var filterBuyer = JsonConvert.SerializeObject(filter);
            var uri = APIEndpoint.PackingInventory + $"garment-shipping/garment-debitur-balances?filter=" + filterBuyer;
            var response = httpService.GetAsync(uri).Result.Content.ReadAsStringAsync();
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Result);
            var json = result.Single(p => p.Key.Equals("data")).Value;
            var listData = JsonConvert.DeserializeObject<List<GarmentShippingInvoiceViewModel>>(json.ToString());
            foreach (var item in listData)
            {
                garmentShipping.Add(item);
            }
            return garmentShipping;
        }


        public List<GarmentFinanceDebtorCardReportViewModel> GetReportQuery(int month, int year, string buyer, int offset)
        {
            var invoicePackingListNow = GetDataShippingInvoiceNow(month, year, buyer);
            var invoicePackingListBalance = GetDataShippingInvoice(month, year, buyer);
            var balance = GetDataBalance(buyer);

            DateTime date = new DateTime(year, month, 1);
            List<GarmentFinanceDebtorCardReportViewModel> data = new List<GarmentFinanceDebtorCardReportViewModel>();

            var beginingMemo = from a in (from aa in _dbContext.GarmentFinanceMemorialDetails where aa.MemorialDate.AddHours(7).Date < date select new { aa.Id })
                               join c in _dbContext.GarmentFinanceMemorialDetailItems on a.Id equals c.MemorialDetailId
                               where c.BuyerCode == buyer
                               select new GarmentFinanceDebtorCardReportViewModel
                               {
                                   BeginAmount = Convert.ToDecimal(-c.Amount),
                                   SellAmount = 0,
                                   PaidAmount = 0,
                                   BalanceAmount = 0,
                                   Code = "AL"

                               };
            var beginingBankCashReceipt = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetails where aa.BankCashReceiptDate.AddHours(7).Date < date select new { aa.Id })
                                          join b in _dbContext.GarmentFinanceBankCashReceiptDetailItems on a.Id equals b.BankCashReceiptDetailId
                                          where b.BuyerCode == buyer
                                          select new GarmentFinanceDebtorCardReportViewModel
                                          {
                                              BeginAmount = Convert.ToDecimal(-b.Amount),
                                              SellAmount = 0,
                                              PaidAmount = 0,
                                              BalanceAmount = 0,
                                              Code = "AL"

                                          };
            var beginingInvoice = from a in invoicePackingListBalance
                                  select new GarmentFinanceDebtorCardReportViewModel
                                  {
                                      BeginAmount = Convert.ToDecimal(a.amount),
                                      SellAmount = 0,
                                      PaidAmount = 0,
                                      BalanceAmount = 0,
                                      Code = "AL"

                                  };
            var beginingBalance = from a in balance
                                  select new GarmentFinanceDebtorCardReportViewModel
                                  {
                                      BeginAmount = Convert.ToDecimal(a.balanceAmount),
                                      SellAmount = 0,
                                      PaidAmount = 0,
                                      BalanceAmount = 0,
                                      Code = "AL"

                                  };
            var salesNow = from a in invoicePackingListNow
                           select new GarmentFinanceDebtorCardReportViewModel
                           {
                               SellAmount = Convert.ToDecimal(a.amount),
                               BeginAmount = 0,
                               PaidAmount = 0,
                               BalanceAmount = 0,
                               Code = "JL",
                               InvoiceNo = a.invoiceNo,
                               Date = a.date,
                               TruckingDate = a.truckingDate
                           };
            var memoNow = from a in (from aa in _dbContext.GarmentFinanceMemorialDetails where aa.MemorialDate.AddHours(7).Date.Month == month && aa.MemorialDate.AddHours(7).Date.Year == year select new { aa.Id, aa.MemorialNo, aa.MemorialDate })
                          join c in _dbContext.GarmentFinanceMemorialDetailItems on a.Id equals c.MemorialDetailId
                          where c.BuyerCode == buyer
                          select new GarmentFinanceDebtorCardReportViewModel
                          {
                              PaidAmount = Convert.ToDecimal(c.Amount),
                              BeginAmount = 0,
                              SellAmount = 0,
                              BalanceAmount = 0,
                              Code = "BY",
                              InvoiceNo = c.InvoiceNo,
                              Date = a.MemorialDate,
                              ReceiptNo = a.MemorialNo
                          };
            var bankCashReceiptNow = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetails where aa.BankCashReceiptDate.AddHours(7).Date.Month == month && aa.BankCashReceiptDate.AddHours(7).Date.Year == year select new { aa.Id, aa.BankCashReceiptNo, aa.BankCashReceiptDate })
                                     join b in _dbContext.GarmentFinanceBankCashReceiptDetailItems on a.Id equals b.BankCashReceiptDetailId
                                     where b.BuyerCode == buyer
                                     select new GarmentFinanceDebtorCardReportViewModel
                                     {
                                         PaidAmount = Convert.ToDecimal(b.Amount),
                                         BeginAmount = 0,
                                         SellAmount = 0,
                                         BalanceAmount = 0,
                                         Code = "BY",
                                         InvoiceNo = b.InvoiceNo,
                                         Date = a.BankCashReceiptDate,
                                         ReceiptNo = a.BankCashReceiptNo
                                     };

            var beginBalanceQuery = beginingBalance.Union(beginingMemo).Union(beginingBankCashReceipt).Union(beginingInvoice);
            var beginBalance = beginBalanceQuery.Sum(a => a.BeginAmount);

            var firstRow = new GarmentFinanceDebtorCardReportViewModel
            {
                Code = "AL",
                Date = null,
                InvoiceDate = null,
                InvoiceNo = null,
                InvoiceNoBY = null,
                InvoiceNoJL = null,
                BeginAmount = 0,
                BalanceAmount = beginBalance,
                PaidAmount = 0,
                SellAmount = 0,
                ReceiptNo = null,
                TruckingDate = null
            };
            data.Add(firstRow);

            var queryUnion = memoNow.Union(bankCashReceiptNow).Union(salesNow);

            var querySum = queryUnion.ToList()
                   .GroupBy(x => new { x.Code, x.InvoiceNo, x.Date, x.TruckingDate, x.ReceiptNo }, (key, group) => new
                   {
                       code = key.Code,
                       invoiceNo = key.InvoiceNo,
                       beginingBalance = group.Sum(s => s.BeginAmount),
                       sell = group.Sum(s => s.SellAmount),
                       buy = group.Sum(s => s.PaidAmount),
                       date = key.Date,
                       trucking = key.TruckingDate,
                       receiptNo = key.ReceiptNo
                   }).OrderBy(s => s.date);
            decimal SumBY = 0;
            decimal SumJL = 0;

            foreach (var item in querySum)
            {
                GarmentFinanceDebtorCardReportViewModel model = new GarmentFinanceDebtorCardReportViewModel
                {
                    Code = item.code,
                    InvoiceNoBY = item.code == "BY" ? item.invoiceNo : null,
                    InvoiceNoJL = item.code == "JL" ? item.invoiceNo : null,
                    TruckingDate = item.trucking,
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

            var lastRow = new GarmentFinanceDebtorCardReportViewModel
            {
                Code = "Saldo",
                Date = null,
                InvoiceDate = null,
                InvoiceNo = null,
                InvoiceNoBY = null,
                InvoiceNoJL = null,
                BeginAmount = 0,
                BalanceAmount = beginBalance,
                PaidAmount = SumBY,
                SellAmount = SumJL,
                ReceiptNo = null,
                TruckingDate = null
            };
            data.Add(lastRow);
            return data;
        }

        public Tuple<MemoryStream, string> GenerateExcel(int month, int year, string buyer, int offset)
        {
            var Query = GetReportQuery(month, year, buyer, offset);

            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn() { ColumnName = "Code", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Invoice", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No. Invoice", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah Invoice (Valas)", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Trucking", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Kwitansi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No. Kwitansi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No. Invoice (dibayar)", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah Kwitansi (Valas)", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Saldo", DataType = typeof(double) });
            if (Query.ToArray().Count() == 0)
                result.Rows.Add("", "", "", "", "", "", "", "", "", 0); // to allow column name to be generated properly for empty data as template
            else
            {
                foreach (var item in Query)
                {
                    string BYDate = item.BYDate == null ? "" : item.BYDate.GetValueOrDefault().AddHours(offset).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    string JLDate = item.JLDate == null ? "" : item.JLDate.GetValueOrDefault().AddHours(offset).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    string Trucking = item.TruckingDate==null ? "" : item.TruckingDate.GetValueOrDefault().AddHours(offset).ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    string JLAmount = item.SellAmount > 0 ? string.Format("{0:n2}", item.SellAmount) : "";
                    string BYAmount = item.PaidAmount > 0 ? string.Format("{0:n2}", item.PaidAmount) : "";
                    result.Rows.Add(item.Code, JLDate, item.InvoiceNoJL, JLAmount, Trucking, BYDate, item.ReceiptNo, item.InvoiceNoBY, BYAmount, item.BalanceAmount);
                }

            }
            ExcelPackage package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Report Kartu Debitur");
            sheet.Cells["A1"].LoadFromDataTable(result, true, OfficeOpenXml.Table.TableStyles.Light16);

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Cells[sheet.Dimension.Address].Style.WrapText = true;

            MemoryStream streamExcel = new MemoryStream();
            package.SaveAs(streamExcel);

            //Dictionary<string, string> FilterDictionary = new Dictionary<string, string>(JsonConvert.DeserializeObject<Dictionary<string, string>>(filter), StringComparer.OrdinalIgnoreCase);
            string fileName = string.Concat("Report Kartu Debitur ", DateTime.Now.Date, ".xlsx");

            return Tuple.Create(streamExcel, fileName);
        }


        public List<GarmentFinanceDebtorCardReportViewModel> GetMonitoring(int month, int year, string buyer, int offset)
        {
            var Data = GetReportQuery(month,year,buyer,offset);
            return Data;
        }
    }
}
