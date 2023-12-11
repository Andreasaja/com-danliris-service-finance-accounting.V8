using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Rreports.ExportSalesDebtorReportController;
using Newtonsoft.Json;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using static Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports.ExportSalesDebtorReport.GarmentShippingPackingList;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Memorial;
using Microsoft.EntityFrameworkCore;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetail;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports.ExportSalesDebtorReport
{
    public class ExportSalesDebtorReportService : IExportSalesDebtorReportService
    {
        private readonly IServiceProvider _serviceProvider;
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        protected DbSet<GarmentFinanceMemorialDetailModel> DbSetMemoDetail;
        protected DbSet<GarmentFinanceMemorialDetailItemModel> DbSetDetailItemMemo;
        protected DbSet<BankCashReceiptDetailItemModel> DbSetBankCashDetailItem;
        protected DbSet<BankCashReceiptDetailModel> DBSetBankCashDetail;



        public ExportSalesDebtorReportService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
            
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public async Task<GarmentShippingPackingList> GetDataShippingInvoice(int month, int year)
        {
            GarmentShippingPackingList garmentShipping = new GarmentShippingPackingList();
            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.PackingInventory + $"garment-shipping/invoices/exportSalesDebtor?month={month}&year={year}";
            var response = await http.GetAsync(uri);


            var result = new BaseResponse<string>();

            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                    var dataString = content.GetValueOrDefault("data").ToString();
                    var listData = JsonConvert.DeserializeObject<List<ShippingPackingDto>>(dataString);
                    garmentShipping.data = listData;
                }
            }


            else
            {
                var err = await response.Content.ReadAsStringAsync();

            }


            return garmentShipping;
        }
        public async Task<GarmentShippingPackingList> GetDataShippingInvoiceNow(int month, int year)
        {
            GarmentShippingPackingList garmentShipping = new GarmentShippingPackingList();
            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.PackingInventory + $"garment-shipping/invoices/exportSalesDebtorNow?month={month}&year={year}";
            var response = await http.GetAsync(uri);


            var result = new BaseResponse<string>();

            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                    var dataString = content.GetValueOrDefault("data").ToString();
                    var listData = JsonConvert.DeserializeObject<List<ShippingPackingDto>>(dataString);
                    garmentShipping.data = listData;
                }
            }


            else
            {
                var err = await response.Content.ReadAsStringAsync();

            }


            return garmentShipping;
        }

        public async Task<GarmentShippingPackingList> GetDataBalance()
        {
            GarmentShippingPackingList garmentShipping = new GarmentShippingPackingList();
            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.PackingInventory + $"garment-shipping/garment-debitur-balances";
            var response = await http.GetAsync(uri);


            var result = new BaseResponse<string>();

            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                    var dataString = content.GetValueOrDefault("data").ToString();
                    var listData = JsonConvert.DeserializeObject<List<ShippingPackingDto>>(dataString);
                    garmentShipping.data = listData;

                }
            }


            else
            {
                var err = await response.Content.ReadAsStringAsync();

            }


            return garmentShipping;
        }
        private async Task<GarmentCurrency> GetCurrency()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var httpClient = (IHttpClientService)_serviceProvider.GetService(typeof(IHttpClientService));

            var currencyUri = APIEndpoint.Core + $"master/garment-currencies/sales-debtor-currencies";
            var currencyResponse = await httpClient.GetAsync(currencyUri);

            var currencyResult = new BaseResponse<GarmentCurrency>()
            {
                data = new GarmentCurrency()
            };

            if (currencyResponse.IsSuccessStatusCode)
            {
                //currencyResult = JsonConvert.DeserializeObject<BaseResponse<GarmentCurrency>>(currencyResponse.Content.ReadAsStringAsync().Result, jsonSerializerSettings);
                var contentString = await currencyResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();
                var listData = JsonConvert.DeserializeObject<GarmentCurrency>(dataString);
                currencyResult.data = listData;
            }

            return currencyResult.data;
        }

        private async Task<GarmentCurrency> GetCurrencyPEBDate(string stringDate)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var httpClient = (IHttpClientService)_serviceProvider.GetService(typeof(IHttpClientService));

            var currencyUri = APIEndpoint.Core + $"master/garment-currencies/sales-debtor-currencies-peb?stringDate={stringDate}";
            var currencyResponse = await httpClient.GetAsync(currencyUri);

            var currencyResult = new BaseResponse<GarmentCurrency>()
            {
                data = new GarmentCurrency()
            };

            if (currencyResponse.IsSuccessStatusCode)
            {
                //currencyResult = JsonConvert.DeserializeObject<BaseResponse<GarmentCurrency>>(currencyResponse.Content.ReadAsStringAsync().Result, jsonSerializerSettings);
                var contentString = await currencyResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();
                var listData = JsonConvert.DeserializeObject<GarmentCurrency>(dataString);
                currencyResult.data = listData;
            }

            return currencyResult.data;
        }
        public async Task<List<ExportSalesDebtorReportViewModel>> GetMonitoring(int month, int year,string type, int offset)
        {

            var data = await GetReportQuery(month, year, type);
            return data;
        }
        class timeSpanInvoice
        {
             internal string buyerCode { get; set; }
             internal decimal amount { get; set; } 
             internal int day { get; set; }
            internal string type { get; set; }
            internal int invoiceId { get; set; }
        }

        public async Task<List<ExportSalesDebtorReportViewModel>> GetReportQuery(int month, int year, string type)
        {
            GarmentShippingPackingList invoicePackingListBalance = await GetDataShippingInvoice(month, year);
            GarmentShippingPackingList invoicePackingListNow = await GetDataShippingInvoiceNow(month, year);
            GarmentShippingPackingList balance = await GetDataBalance();

            List<ExportSalesDebtorReportViewModel> data = new List<ExportSalesDebtorReportViewModel>();
            GarmentCurrency garmentCurrency = await GetCurrency();
            foreach (var item in invoicePackingListBalance.data)
            {
                GarmentCurrency currency = await GetCurrencyPEBDate(item.pebDate.Date.ToShortDateString());
                item.rate = Convert.ToDouble(currency.Rate);

            }
            foreach (var item in invoicePackingListNow.data)
            {
                GarmentCurrency currency = await GetCurrencyPEBDate(item.pebDate.Date.ToShortDateString());
                item.rate = Convert.ToDouble(currency.Rate);

            }

            var _invoice = invoicePackingListBalance.data.Union(invoicePackingListNow.data);
            var querytimeSpan = from aa in _invoice
                                select new timeSpanInvoice
                                {
                                    buyerCode = aa.buyerAgentCode,
                                    amount = type == "IDR" ? Convert.ToDecimal(aa.amount * aa.rate) : Convert.ToDecimal(aa.amount),
                                    day = (aa.truckingDate.AddDays(aa.paymentdue) - aa.truckingDate).Days,
                                    type = (aa.truckingDate.AddDays(aa.paymentdue) - aa.truckingDate).Days <= 0 ? "normal" :
                                            (aa.truckingDate.AddDays(aa.paymentdue) - aa.truckingDate).Days > 0 && (aa.truckingDate.AddDays(aa.paymentdue) - aa.truckingDate).Days < 31 ? "oneThirty" :
                                            (aa.truckingDate.AddDays(aa.paymentdue) - aa.truckingDate).Days > 30 && (aa.truckingDate.AddDays(aa.paymentdue) - aa.truckingDate).Days < 61 ? "thirtySixty" :
                                            (aa.truckingDate.AddDays(aa.paymentdue) - aa.truckingDate).Days > 60 && (aa.truckingDate.AddDays(aa.paymentdue) - aa.truckingDate).Days < 91 ? "sixtyNinety" :
                                            "moreThanNinety",
                                    invoiceId= Convert.ToInt32(aa.invoiceId)
                                };
            var querySumInvoice = querytimeSpan.ToList()
                   .GroupBy(x => new { x.buyerCode, x.type }, (key, group) => new
                   {
                       buyerCode = key.buyerCode.TrimEnd(),
                       type = key.type,
                       amount = group.Sum(s => s.amount)
                   });



            var beginingMemo = from a in (from aa in _dbContext.GarmentFinanceMemorialDetails where aa.MemorialDate.AddHours(7).Month < month && aa.MemorialDate.AddHours(7).Year == year select new { aa.Id })
                               join c in _dbContext.GarmentFinanceMemorialDetailItems on a.Id equals c.MemorialDetailId
                              
                               select new ExportSalesDebtorReportViewModel
                               {
                                   buyerCode = c.BuyerCode.TrimEnd(),
                                   buyerName = c.BuyerName.TrimEnd(),
                                   beginingBalance = type == "IDR" ? Convert.ToDecimal(-c.Amount) * Convert.ToDecimal(garmentCurrency.Rate) : Convert.ToDecimal(-c.Amount),
                                   receipt = 0,
                                   sales = 0,
                                   endBalance = 0,
                                   normal = 0,
                                   oneThirty = 0,
                                   thirtySixty = 0,
                                   sixtyNinety = 0,
                                   moreThanNinety = 0

                               };
            var beginingBankCashReceipt = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetails where aa.BankCashReceiptDate.AddHours(7).Month < month && aa.BankCashReceiptDate.AddHours(7).Year == year select new { aa.Id })
                                          join b in _dbContext.GarmentFinanceBankCashReceiptDetailItems on a.Id equals b.BankCashReceiptDetailId
                                          
                                          select new ExportSalesDebtorReportViewModel
                                          {
                                              buyerCode = b.BuyerCode.TrimEnd(),
                                              buyerName = b.BuyerName.TrimEnd(),
                                              beginingBalance = type == "IDR" ? Convert.ToDecimal(-b.CurrencyRate * b.Amount) : Convert.ToDecimal(-b.Amount),
                                              receipt = 0,
                                              sales = 0,
                                              endBalance = 0,
                                              normal = 0,
                                              oneThirty = 0,
                                              thirtySixty = 0,
                                              sixtyNinety = 0,
                                              moreThanNinety = 0
                                          };

           
            var beginingInvoice = from a in invoicePackingListBalance.data
                                  
                                  select new ExportSalesDebtorReportViewModel
                                  {
                                      buyerCode = a.buyerAgentCode.TrimEnd(),
                                      buyerName = a.buyerAgentName.TrimEnd(),
                                      beginingBalance = type == "IDR" ? Convert.ToDecimal(a.amount * a.rate) : Convert.ToDecimal(a.amount),
                                      receipt = 0,
                                      sales = 0,
                                      endBalance = 0,
                                      normal = 0,
                                      oneThirty = 0,
                                      thirtySixty = 0,
                                      sixtyNinety = 0,
                                      moreThanNinety = 0
                                  };

            var beginingBalance = from a in balance.data
                                  
                                  select new ExportSalesDebtorReportViewModel
                                  {
                                      buyerCode = a.buyerAgentCode.TrimEnd(),
                                      buyerName = a.buyerAgentName.TrimEnd(),
                                      beginingBalance = type == "IDR" ? Convert.ToDecimal(a.balanceAmountIDR) : Convert.ToDecimal(a.balanceAmount),
                                      receipt = 0,
                                      sales = 0,
                                      endBalance = 0,
                                      normal = 0,
                                      oneThirty = 0,
                                      thirtySixty = 0,
                                      sixtyNinety = 0,
                                      moreThanNinety = 0
                                  };
            var salesNow = from a in invoicePackingListNow.data
                           
                           select new ExportSalesDebtorReportViewModel
                           {
                               buyerCode = a.buyerAgentCode.TrimEnd(),
                               buyerName = a.buyerAgentName.TrimEnd(),
                               beginingBalance = 0,
                               receipt = 0,
                               sales = type == "IDR" ? Convert.ToDouble(a.amount * a.rate) : Convert.ToDouble(a.amount),
                               normal = 0,
                               oneThirty = 0,
                               thirtySixty = 0,
                               sixtyNinety = 0,
                               moreThanNinety = 0
                           };
            var memoNow = from a in (from aa in _dbContext.GarmentFinanceMemorialDetails where aa.MemorialDate.AddHours(7).Month == month && aa.MemorialDate.AddHours(7).Year == year select new { aa.Id })
                          join c in _dbContext.GarmentFinanceMemorialDetailItems on a.Id equals c.MemorialDetailId
                          
                          select new ExportSalesDebtorReportViewModel
                          {
                              buyerCode = c.BuyerCode.TrimEnd(),
                              buyerName = c.BuyerName.TrimEnd(),
                              beginingBalance = 0,
                              receipt = type == "IDR" ? Convert.ToDouble(c.Amount * c.CurrencyRate) : Convert.ToDouble(c.Amount),
                              sales = 0,
                              endBalance = 0,
                              normal = 0,
                              oneThirty = 0,
                              thirtySixty = 0,
                              sixtyNinety = 0,
                              moreThanNinety = 0

                          };
            var bankCashReceiptNow = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetails where aa.BankCashReceiptDate.AddHours(7).Month == month && aa.BankCashReceiptDate.AddHours(7).Year == year select new { aa.Id })
                                     join b in _dbContext.GarmentFinanceBankCashReceiptDetailItems on a.Id equals b.BankCashReceiptDetailId
                                     
                                     select new ExportSalesDebtorReportViewModel
                                     {
                                         buyerCode = b.BuyerCode.TrimEnd(),
                                         buyerName = b.BuyerName.TrimEnd(),
                                         beginingBalance = 0,
                                         receipt = type == "IDR" ? Convert.ToDouble(b.Amount * b.CurrencyRate) : Convert.ToDouble(b.Amount),
                                         sales = 0,
                                         endBalance = 0,
                                         normal = 0,
                                         oneThirty = 0,
                                         thirtySixty = 0,
                                         sixtyNinety = 0,
                                         moreThanNinety = 0
                                     };
            var periodeMemo = from a in (from aa in _dbContext.GarmentFinanceMemorialDetails where aa.MemorialDate.AddHours(7).Month <= month && aa.MemorialDate.AddHours(7).Year == year select new { aa.Id })
                              join c in _dbContext.GarmentFinanceMemorialDetailItems on a.Id equals c.MemorialDetailId
                              
                              select new ExportSalesDebtorReportViewModel
                              {
                                  buyerCode = c.BuyerCode.TrimEnd(),
                                  buyerName = c.BuyerName.TrimEnd(),
                                  beginingBalance = 0,
                                  receipt = 0,
                                  sales = 0,
                                  endBalance = 0,
                                  normal = type == "IDR" && (from aa in querytimeSpan where aa.invoiceId == c.InvoiceId select aa.type).FirstOrDefault()== "normal"? -c.Amount * Convert.ToDouble(garmentCurrency.Rate)  : (from aa in querytimeSpan where aa.invoiceId == c.InvoiceId select aa.type).FirstOrDefault() == "normal" ? -c.Amount :0,
                                  oneThirty = type == "IDR" && (from aa in querytimeSpan where aa.invoiceId == c.InvoiceId select aa.type).FirstOrDefault() == "oneThirty" ? -c.Amount * Convert.ToDouble(garmentCurrency.Rate)  : (from aa in querytimeSpan where aa.invoiceId == c.InvoiceId select aa.type).FirstOrDefault() == "oneThirty" ? -c.Amount : 0,
                                  thirtySixty = type == "IDR" && (from aa in querytimeSpan where aa.invoiceId == c.InvoiceId select aa.type).FirstOrDefault() == "thirtySixty" ? -c.Amount * Convert.ToDouble(garmentCurrency.Rate)  : (from aa in querytimeSpan where aa.invoiceId == c.InvoiceId select aa.type).FirstOrDefault() == "thirtySixty" ? -c.Amount : 0,
                                  sixtyNinety = type == "IDR" && (from aa in querytimeSpan where aa.invoiceId == c.InvoiceId select aa.type).FirstOrDefault() == "sixtyNinety" ? -c.Amount * Convert.ToDouble(garmentCurrency.Rate)  : (from aa in querytimeSpan where aa.invoiceId == c.InvoiceId select aa.type).FirstOrDefault() == "sixtyNinety" ? -c.Amount : 0,
                                  moreThanNinety = type == "IDR" && (from aa in querytimeSpan where aa.invoiceId == c.InvoiceId select aa.type).FirstOrDefault() =="moreThanNinety" ? -c.Amount * Convert.ToDouble(garmentCurrency.Rate)  : (from aa in querytimeSpan where aa.invoiceId == c.InvoiceId select aa.type).FirstOrDefault() == "moreThanNinety" ? -c.Amount : 0,
                                  
                              };
            var periodeBankCashReceipt = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetails where aa.BankCashReceiptDate.AddHours(7).Month <= month && aa.BankCashReceiptDate.AddHours(7).Year == year select new { aa.Id })
                                         join b in _dbContext.GarmentFinanceBankCashReceiptDetailItems on a.Id equals b.BankCashReceiptDetailId
                                         
                                         select new ExportSalesDebtorReportViewModel
                                         {
                                             buyerCode = b.BuyerCode.TrimEnd(),
                                             buyerName = b.BuyerName.TrimEnd(),
                                             beginingBalance = 0,
                                             receipt = 0,
                                             sales = 0,
                                             endBalance = 0,
                                             normal = type == "IDR" && (from aa in querytimeSpan where aa.invoiceId == b.InvoiceId select aa.type).FirstOrDefault() == "normal" ? Convert.ToDouble(-b.Amount) * Convert.ToDouble(garmentCurrency.Rate): (from aa in querytimeSpan where aa.invoiceId == b.InvoiceId select aa.type).FirstOrDefault() == "normal" ? Convert.ToDouble(-b.Amount) : 0,
                                             oneThirty = type == "IDR" && (from aa in querytimeSpan where aa.invoiceId == b.InvoiceId select aa.type).FirstOrDefault() == "oneThirty" ? Convert.ToDouble(-b.Amount) * Convert.ToDouble(garmentCurrency.Rate)  : (from aa in querytimeSpan where aa.invoiceId == b.InvoiceId select aa.type).FirstOrDefault() == "oneThirty" ? Convert.ToDouble(-b.Amount) : 0,
                                             thirtySixty = type == "IDR" && (from aa in querytimeSpan where aa.invoiceId == b.InvoiceId select aa.type).FirstOrDefault() == "thirtySixty" ? Convert.ToDouble(-b.Amount) * Convert.ToDouble(garmentCurrency.Rate)  : (from aa in querytimeSpan where aa.invoiceId == b.InvoiceId select aa.type).FirstOrDefault() == "thirtySixty" ? Convert.ToDouble(-b.Amount) : 0,
                                             sixtyNinety = type == "IDR" && (from aa in querytimeSpan where aa.invoiceId == b.InvoiceId select aa.type).FirstOrDefault() == "sixtyNinety" ? Convert.ToDouble(-b.Amount) * Convert.ToDouble(garmentCurrency.Rate)  : (from aa in querytimeSpan where aa.invoiceId == b.InvoiceId select aa.type).FirstOrDefault() == "sixtyNinety" ? Convert.ToDouble(-b.Amount) : 0,
                                             moreThanNinety = type == "IDR" && (from aa in querytimeSpan where aa.invoiceId == b.InvoiceId select aa.type).FirstOrDefault() == "moreThanNinety" ? Convert.ToDouble(-b.Amount) * Convert.ToDouble(garmentCurrency.Rate)  : (from aa in querytimeSpan where aa.invoiceId == b.InvoiceId select aa.type).FirstOrDefault() == "moreThanNinety" ? Convert.ToDouble(-b.Amount) : 0,

                                         };
            var queryUnion = beginingBalance.Union(beginingMemo).Union(beginingBankCashReceipt).Union(beginingInvoice)
                        .Union(memoNow).Union(bankCashReceiptNow).Union(salesNow)
                        .Union(periodeBankCashReceipt).Union(periodeMemo);

         
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
                ExportSalesDebtorReportViewModel model = new ExportSalesDebtorReportViewModel
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
                ExportSalesDebtorReportViewModel model = new ExportSalesDebtorReportViewModel
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
        public async Task<MemoryStream> GenerateExcel(int month, int year,string type)
        {
            var data = await GetReportQuery(month, year, type);

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
            if (type != "end")
            {
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
                        "", "", "", "", "", "", "Lancar", "1 - 30 hari"," 31- 60 hari","61 - 90 hari", "> 90 hari");
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
                    
                    var stream = new MemoryStream();

                    package.SaveAs(stream);
                    return stream;
                }
            }
            else
            {

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
                        result.Rows.Add(item.index, item.buyerCode, item.buyerName,  Math.Round(item.endBalance, 2));
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
}
