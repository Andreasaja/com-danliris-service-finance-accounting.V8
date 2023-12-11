using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports.ExportSalesDebtorReport;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.DebtorCard;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.ExportSalesOutstanding;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Reports.ExportSalesDebtorReport.GarmentShippingPackingList;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.ExportSalesOutstanding
{
    public class GarmentFinanceExportSalesOutstandingReportService : IGarmentFinanceExportSalesOutstandingReportService
    {
        private readonly IServiceProvider _serviceProvider;
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        string _buyerName = "";

        public GarmentFinanceExportSalesOutstandingReportService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _dbContext = dbContext;

            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public async Task<GarmentShippingPackingList> GetDataShippingInvoiceNow(int month, int year)
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

        public async Task<List<GarmentFinanceExportSalesOutstandingReportViewModel>> GetReportQuery(int month, int year, string buyer, int offset)
        {
            GarmentShippingPackingList invoicePackingListNow = await GetDataShippingInvoiceNow(month, year);

            DateTime date = new DateTime(year, month, 1);

            List<GarmentFinanceExportSalesOutstandingReportViewModel> data = new List<GarmentFinanceExportSalesOutstandingReportViewModel>();

            var invoice = from a in invoicePackingListNow.data
                          where (buyer == null || buyer =="undefined" || (buyer != null && buyer != "undefined" && buyer != "" && a.buyerAgentCode == buyer))

                          select new GarmentFinanceExportSalesOutstandingReportViewModel
                          {
                              Amount = Convert.ToDecimal(a.amount),
                              InvoiceId = Convert.ToInt32(a.invoiceId),
                              InvoiceNo = a.invoiceNo,
                              TruckingDate = a.truckingDate,
                              BuyerName = a.buyerAgentName
                           };
            var memorial = from a in (from aa in _dbContext.GarmentFinanceMemorialDetails where aa.MemorialDate.AddHours(7).Date.Month <= month && aa.MemorialDate.AddHours(7).Date.Year == year select new { aa.Id, aa.MemorialNo, aa.MemorialDate })
                          join c in _dbContext.GarmentFinanceMemorialDetailItems on a.Id equals c.MemorialDetailId
                          
                           where (buyer == null || (buyer != null && buyer != "" && c.BuyerCode == buyer))
                           select new GarmentFinanceExportSalesOutstandingReportViewModel
                          {
                              Amount = Convert.ToDecimal(-c.Amount),
                              InvoiceNo = c.InvoiceNo,
                              InvoiceId= c.InvoiceId,
                              TruckingDate=null,
                              BuyerName= c.BuyerName
                              
                          };
            var bankCashReceipt = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetails where aa.BankCashReceiptDate.AddHours(7).Date.Month <= month && aa.BankCashReceiptDate.AddHours(7).Date.Year == year select new { aa.Id, aa.BankCashReceiptNo, aa.BankCashReceiptDate })
                                     join b in _dbContext.GarmentFinanceBankCashReceiptDetailItems on a.Id equals b.BankCashReceiptDetailId
                                  where (buyer == null || (buyer != null && buyer != "" && b.BuyerCode == buyer))
                                  select new GarmentFinanceExportSalesOutstandingReportViewModel
                                     {
                                         Amount = Convert.ToDecimal(-b.Amount),
                                         InvoiceNo = b.InvoiceNo,
                                         InvoiceId = b.InvoiceId,
                                         TruckingDate = null,
                                         BuyerName = b.BuyerName
                                         
                                  };
            var unionQuery = memorial.Union(bankCashReceipt).Union(invoice);
            if (buyer == null || buyer =="undefined" || buyer =="")
            {
                _buyerName = "ALL";
            } else
            {
                _buyerName = (from a in unionQuery.ToList()
                              select a.BuyerName).FirstOrDefault() + " >> " + buyer;
            }
            var querySum= unionQuery.ToList().GroupBy(a=> new { a.InvoiceId }, (key, group) => new
            {
                invoiveId = key.InvoiceId,
                balance = group.Sum(s => s.Amount)
            });
            decimal total = 0;
            int index = 1;
            foreach (var item in invoice)
            {
                GarmentFinanceExportSalesOutstandingReportViewModel model = new GarmentFinanceExportSalesOutstandingReportViewModel
                {
                    Index = index,
                    InvoiceNo = item.InvoiceNo,
                    TruckingDate = item.TruckingDate,
                    Amount = querySum.First(a => a.invoiveId == item.InvoiceId).balance
                };
                data.Add(model);
                total += model.Amount;
                index++;
            }
            data.OrderBy(a => a.BuyerName);
            index = 0;
            var lastRow = new GarmentFinanceExportSalesOutstandingReportViewModel
            {
                InvoiceNo = "TOTAL",
                TruckingDate = null,
                Amount=total
            };
            data.Add(lastRow);
            return data;
        }

        public async Task<MemoryStream> GenerateExcel(int month, int year, string buyer, int offset)
        {
            var Data = await GetReportQuery(month, year, buyer, offset);
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
                result.Columns.Add(new DataColumn() { ColumnName = "Tanggal", DataType = typeof(String) });
                result.Columns.Add(new DataColumn() { ColumnName = "No Invoice", DataType = typeof(String) });
                result.Columns.Add(new DataColumn() { ColumnName = "Amount", DataType = typeof(double) });
                
                int counter = 0;
            if (Data.Count() <=1)
                result.Rows.Add("", "", "", 0); // to allow column name to be generated properly for empty data as template
            else
            {

                foreach (var item in Data)
                {
                    counter++;
                    var truckingDate = "";
                    if (item.TruckingDate != null)
                    {
                        DateTimeOffset PassDate = (DateTimeOffset)item.TruckingDate;
                        var dateFormat = "yyyy-MM-dd";
                        truckingDate = PassDate.ToString(dateFormat);
                    }
                    result.Rows.Add(item.Index, truckingDate, item.InvoiceNo, Math.Round(item.Amount, 2));
                }
            }
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                    worksheet.Cells["A1"].Value = "PT. D A N L I R I S";
                    worksheet.Cells["A2"].Value = "OUTSTANDING PENJUALAN EXPORT ";
                    worksheet.Cells["A3"].Value = "BULAN " + monthValue + " " + year;
                    worksheet.Cells["A4"].Value = "DEBITUR " + _buyerName ;
                    worksheet.Cells["A" + 1 + ":A" + 4 + ""].Style.Font.Size = 14;
                    worksheet.Cells["A" + 1 + ":A" + 4 + ""].Style.Font.Bold = true;
                    worksheet.Cells["A5"].LoadFromDataTable(result, true);
                   
                    worksheet.Cells["A" + 5 + ":D" + (counter + 5) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + 5 + ":D" + (counter + 5) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + 5 + ":D" + (counter + 5) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + 5 + ":D" + (counter + 5) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A" + 5 + ":D" + 5 + ""].Style.Font.Bold = true;
                   
                    worksheet.Cells["A" + 5 + ":D" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                foreach (var cell in worksheet.Cells["D" + 6 + ":D" + (counter + 4) + ""])
                {
                    cell.Value = Convert.ToDecimal(cell.Value);
                    cell.Style.Numberformat.Format = "#,##0.00";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }
                worksheet.Cells["B" + 4 + ":D" + (counter + 4) + ""].AutoFitColumns();
                worksheet.Cells["A" + (counter + 5) + ":C" + (counter + 5) + ""].Merge = true;
                worksheet.Cells["A" + (counter + 5) + ":C" + (counter + 5) + ""].Value = "TOTAL";

                worksheet.Cells["A" + (counter + 5) + ":C" + (counter + 5) + ""].Style.Font.Bold = true;
                var stream = new MemoryStream();

                    package.SaveAs(stream);
                    return stream;
                
            
            }
        }

        public List<GarmentFinanceExportSalesOutstandingReportViewModel> GetMonitoring(int month, int year, string buyer, int offset)
        {
            var Data = GetReportQuery(month, year, buyer, offset);
            return Data.Result;
        }
    }
}
