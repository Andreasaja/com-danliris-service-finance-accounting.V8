using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalSalesOutstanding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml.Style;
using System.Data;
using OfficeOpenXml;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Newtonsoft.Json;
using static Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Report.LocalSalesOutstanding.GarmentFinanceLocalSalesListModel;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Reports.LocalSalesOutstanding
{
    public class GarmentFinanceLocalSalesOutstandingReportService : IGarmentFinanceLocalSalesOutstandingReportService
    {
        private readonly IServiceProvider _serviceProvider;
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        string _buyerName = "";

        public GarmentFinanceLocalSalesOutstandingReportService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _dbContext = dbContext;

            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }
        public async Task<GarmentFinanceLocalSalesListModel> GetDataShippingInvoiceNow(int month, int year)
        {
            GarmentFinanceLocalSalesListModel garmentShipping = new GarmentFinanceLocalSalesListModel();
            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.PackingInventory + $"garment-shipping/local-sales-notes/localSalesDebtorNow?month={month}&year={year}";
            var response = await http.GetAsync(uri);


            var result = new BaseResponse<string>();

            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                    var dataString = content.GetValueOrDefault("data").ToString();
                    var listData = JsonConvert.DeserializeObject<List<GarmentFinanceLocalSalesModel>>(dataString);
                    garmentShipping.data = listData;
                }
            }
            else
            {
                var err = await response.Content.ReadAsStringAsync();

            }

            return garmentShipping;
        }

        public async Task<List<GarmentFinanceLocalSalesOutstandingReportViewModel>> GetReportQuery(int month, int year, string buyer, int offset)
        {
            GarmentFinanceLocalSalesListModel invoicePackingListNow = await GetDataShippingInvoiceNow(month, year);

            DateTime date = new DateTime(year, month, 1);

            List<GarmentFinanceLocalSalesOutstandingReportViewModel> data = new List<GarmentFinanceLocalSalesOutstandingReportViewModel>();

            var invoice = from a in invoicePackingListNow.data
                          where (buyer == null || buyer == "undefined" || (buyer != null && buyer != "undefined" && buyer != "" && a.buyer.code == buyer))
                          select new GarmentFinanceLocalSalesOutstandingReportViewModel
                          {
                              Amount = Convert.ToDecimal(a.items.Sum(s => a.useVat ? (s.price * s.quantity) +  (0.1 * (s.price * s.quantity)): s.price * s.quantity)),
                              InvoiceId = Convert.ToInt32(a.id),
                              InvoiceNo = a.noteNo,
                              TruckingDate = a.date,
                              BuyerName = a.buyer.name
                          };
            var memorial = from a in (from aa in _dbContext.GarmentFinanceMemorialDetailLocals
                                      where aa.MemorialDate.AddHours(7).Date.Month <= month && aa.MemorialDate.AddHours(7).Date.Year == year 
                                      select new { aa.Id, aa.MemorialNo, aa.MemorialDate })
                           join c in _dbContext.GarmentFinanceMemorialDetailLocalItems on a.Id equals c.MemorialDetailLocalId
                           where (buyer == null || (buyer != null && buyer != "" && c.BuyerCode == buyer))
                           select new GarmentFinanceLocalSalesOutstandingReportViewModel
                           {
                               Amount = Convert.ToDecimal(-c.Amount),
                               InvoiceNo = c.LocalSalesNoteNo,
                               InvoiceId = c.LocalSalesNoteId,
                               TruckingDate = null,
                               BuyerName = c.BuyerName

                           };
            var bankCashReceipt = from a in (from aa in _dbContext.GarmentFinanceBankCashReceiptDetailLocals
                                             where aa.BankCashReceiptDate.AddHours(7).Date.Month <= month && aa.BankCashReceiptDate.AddHours(7).Date.Year == year 
                                             select new { aa.Id, aa.BankCashReceiptNo, aa.BankCashReceiptDate })
                                  join b in _dbContext.GarmentFinanceBankCashReceiptDetailLocalItems
                                  on a.Id equals b.BankCashReceiptDetailLocalId
                                  where (buyer == null || (buyer != null && buyer != "" && b.BuyerCode == buyer))
                                  select new GarmentFinanceLocalSalesOutstandingReportViewModel
                                  {
                                      Amount = Convert.ToDecimal(-b.Amount),
                                      InvoiceNo = b.LocalSalesNoteNo,
                                      InvoiceId = b.LocalSalesNoteId,
                                      TruckingDate = null,
                                      BuyerName = b.BuyerName

                                  };
            var unionQuery = memorial.Union(bankCashReceipt).Union(invoice);
            if (buyer == null || buyer == "undefined" || buyer == "")
            {
                _buyerName = "ALL";
            }
            else
            {
                _buyerName = (from a in unionQuery.ToList()
                              select a.BuyerName).FirstOrDefault() + " >> " + buyer;
            }
            var querySum = unionQuery.ToList().GroupBy(a => new { a.InvoiceId }, (key, group) => new
            {
                invoiveId = key.InvoiceId,
                balance = group.Sum(s => s.Amount)
            });
            decimal total = 0;
            int index = 1;
            foreach (var item in invoice)
            {
                GarmentFinanceLocalSalesOutstandingReportViewModel model = new GarmentFinanceLocalSalesOutstandingReportViewModel
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
            data=data.OrderBy(a => a.BuyerName).ToList();
            index = 0;
            var lastRow = new GarmentFinanceLocalSalesOutstandingReportViewModel
            {
                InvoiceNo = "TOTAL",
                TruckingDate = null,
                Amount = total
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
            if (Data.Count() <= 1)
                result.Rows.Add("", "", "", 0); // to allow column name to be generated properly for empty data as template
            else
            {

                foreach (var item in Data)
                {
                    counter++;
                    var truckingDate = "";
                    if (item.TruckingDate != null)
                    {
                        DateTimeOffset PassDate = (DateTimeOffset)item.TruckingDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset);
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
                worksheet.Cells["A2"].Value = "OUTSTANDING PENJUALAN LOCAL ";
                worksheet.Cells["A3"].Value = "BULAN " + monthValue + " " + year;
                worksheet.Cells["A4"].Value = "DEBITUR " + _buyerName;
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
            //throw new NotImplementedException();
        }

        public List<GarmentFinanceLocalSalesOutstandingReportViewModel> GetMonitoring(int month, int year, string buyer, int offset)
        {
            var Data = GetReportQuery(month, year, buyer, offset);
            return Data.Result;
            //var data = GetDataShippingInvoiceNow(month, year);

            //return new List<GarmentFinanceLocalSalesOutstandingReportViewModel>();
            //throw new NotImplementedException();
        }
    }
}
