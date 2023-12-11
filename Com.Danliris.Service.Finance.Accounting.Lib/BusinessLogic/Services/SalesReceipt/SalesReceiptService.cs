using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Newtonsoft.Json;
using System.Net.Http;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using System.Text;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.SalesReceipt;
using System.IO;
using System.Data;
using System.Globalization;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.SalesReceipt
{
    public class SalesReceiptService : ISalesReceiptService
    {
        private const string _UserAgent = "finance-service";
        protected DbSet<SalesReceiptModel> _DbSet;
        protected DbSet<SalesReceiptDetailModel> _DetailDbSet;
        private readonly IServiceProvider _serviceProvider;
        protected IIdentityService _IdentityService;
        public FinanceDbContext _DbContext;

        public SalesReceiptService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _DbContext = dbContext;
            _DbSet = dbContext.Set<SalesReceiptModel>();
            _DetailDbSet = dbContext.Set<SalesReceiptDetailModel>();
            _serviceProvider = serviceProvider;
            _IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public async Task<int> CreateAsync(SalesReceiptModel model)
        {
            int result = 0;
            using (var transaction = _DbContext.Database.BeginTransaction())
            {
                try
                {
                    int index = 0;

                    do
                    {
                        model.Code = CodeGenerator.Generate();
                    }
                    while (_DbSet.Any(d => d.Code.Equals(model.Code)));
                    model.SalesReceiptDetails = model.SalesReceiptDetails.Where(s => s.Nominal > 0).ToList();

                    SalesReceiptNumberGenerator(model, index);

                    foreach (var detail in model.SalesReceiptDetails)
                    {
                        EntityExtension.FlagForCreate(detail, _IdentityService.Username, _UserAgent);
                    }

                    EntityExtension.FlagForCreate(model, _IdentityService.Username, _UserAgent);
                    _DbSet.Add(model);

                    index++;

                    result = await _DbContext.SaveChangesAsync();

                    foreach (var item in model.SalesReceiptDetails)
                    {
                        var updateModel = new SalesReceiptUpdateModel()
                        {
                            TotalPaid = item.Paid,
                            IsPaidOff = item.IsPaidOff,
                        };

                        UpdateToSalesInvoice(item.SalesInvoiceId, updateModel);
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }

            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var transaction = _DbContext.Database.BeginTransaction())
            {
                try
                {
                    SalesReceiptModel model = await _DbSet.Where(p => p.SalesReceiptDetails.Select(d => d.SalesReceiptModel.Id)
                    .Contains(p.Id)).Include(p => p.SalesReceiptDetails).FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
                    model.SalesReceiptDetails = model.SalesReceiptDetails.OrderBy(s => s.Id).ToArray();

                    if (model != null)
                    {
                        SalesReceiptModel salesReceiptModel = new SalesReceiptModel();

                        salesReceiptModel = model;

                        model = await ReadByIdAsync(id);

                        foreach (var detail in model.SalesReceiptDetails)
                        {
                            EntityExtension.FlagForDelete(detail, _IdentityService.Username, _UserAgent, true);
                        }

                        EntityExtension.FlagForDelete(model, _IdentityService.Username, _UserAgent, true);
                        _DbSet.Update(model);

                        foreach (var item in model.SalesReceiptDetails)
                        {
                            var updateModel = new SalesReceiptUpdateModel()
                            {
                                TotalPaid = model.TotalPaid - item.Nominal,
                                IsPaidOff = false,
                            };

                            UpdateToSalesInvoice(item.SalesInvoiceId, updateModel);
                        }
                    }
                }
                catch (Exception e)
                {

                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            return await _DbContext.SaveChangesAsync();
        }

        public ReadResponse<SalesReceiptModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<SalesReceiptModel> Query = _DbSet.Include(x => x.SalesReceiptDetails);

            List<string> SearchAttributes = new List<string>()
            {
                "SalesReceiptNo","BuyerName","CurrencyCode","OriginAccountNumber",
            };

            Query = QueryHelper<SalesReceiptModel>.Search(Query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<SalesReceiptModel>.Filter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>()
            {
                "Id","Code","SalesReceiptNo","SalesReceiptDate","Buyer","Currency","Unit","OriginAccountNumber","AdministrationFee","TotalPaid","SalesReceiptDetails"
            };

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<SalesReceiptModel>.Order(Query, OrderDictionary);

            Pageable<SalesReceiptModel> pageable = new Pageable<SalesReceiptModel>(Query, page - 1, size);
            List<SalesReceiptModel> data = pageable.Data.ToList<SalesReceiptModel>();
            int totalData = pageable.TotalCount;

            return new ReadResponse<SalesReceiptModel>(data, totalData, OrderDictionary, SelectedFields);

        }

        public async Task<SalesReceiptModel> ReadByIdAsync(int id)
        {
            var SalesReceipt = await _DbSet.Where(d => d.Id.Equals(id) && !d.IsDeleted).FirstOrDefaultAsync();
            SalesReceipt.SalesReceiptDetails = await _DetailDbSet.Where(f => f.SalesReceiptId.Equals(id) && !f.IsDeleted).ToListAsync();
            return SalesReceipt;
        }

        public async Task<int> UpdateAsync(int id, SalesReceiptModel model)
        {
            using (var transaction = _DbContext.Database.BeginTransaction())
            {
                try
                {
                    if (model.SalesReceiptDetails != null)
                    {
                        List<int> detailIds = await _DetailDbSet.Where(w => w.SalesReceiptId.Equals(id) && !w.IsDeleted).Select(s => s.Id).ToListAsync();

                        foreach (var itemId in detailIds)
                        {
                            SalesReceiptDetailModel data = model.SalesReceiptDetails.FirstOrDefault(prop => prop.Id.Equals(itemId));
                            if (data == null)
                            {
                                var detailModel = await ReadByIdAsync(itemId);
                                EntityExtension.FlagForDelete(detailModel, _IdentityService.Username, _UserAgent, true);
                            }
                            else
                            {
                                EntityExtension.FlagForUpdate(data, _IdentityService.Username, _UserAgent);
                            }
                        }

                        foreach (SalesReceiptDetailModel item in model.SalesReceiptDetails)
                        {
                            if (item.Id == 0)
                            {
                                EntityExtension.FlagForCreate(item, _IdentityService.Username, _UserAgent);
                            }
                        }
                    }

                    EntityExtension.FlagForUpdate(model, _IdentityService.Username, _UserAgent);
                    _DbSet.Update(model);

                    foreach (var item in model.SalesReceiptDetails)
                    {
                        var updateModel = new SalesReceiptUpdateModel()
                        {
                            TotalPaid = item.Paid,
                            IsPaidOff = item.IsPaidOff,
                        };

                        UpdateToSalesInvoice(item.SalesInvoiceId, updateModel);
                    }

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }

            return await _DbContext.SaveChangesAsync();
        }

        private void SalesReceiptNumberGenerator(SalesReceiptModel model, int index)
        {
            int MonthNow = DateTime.Now.Month;
            int YearNow = DateTime.Now.Year;
            var MonthNowString = DateTime.Now.ToString("MM");
            var YearNowString = DateTime.Now.ToString("yy");
            SalesReceiptModel lastData = _DbSet.IgnoreQueryFilters().OrderByDescending(o => o.AutoIncreament).FirstOrDefault();

            if (lastData == null)
            {
                model.AutoIncreament = 1 + index;
                model.SalesReceiptNo = $"{YearNowString}{MonthNowString}{model.BankCode}M{model.AutoIncreament.ToString().PadLeft(6, '0')}";
            }
            else
            {
                if (YearNow > lastData.CreatedUtc.Year || MonthNow > lastData.CreatedUtc.Month)
                {
                    model.AutoIncreament = 1 + index;
                    model.SalesReceiptNo = $"{YearNowString}{MonthNowString}{model.BankCode}M{model.AutoIncreament.ToString().PadLeft(6, '0')}";
                }
                else
                {
                    model.AutoIncreament = lastData.AutoIncreament + (1 + index);
                    model.SalesReceiptNo = $"{YearNowString}{MonthNowString}{model.BankCode}M{model.AutoIncreament.ToString().PadLeft(6, '0')}";
                }
            }
        }

        private void UpdateToSalesInvoice(int id, SalesReceiptUpdateModel model)
        {
            string salesInvoiceUri = "sales/sales-invoices/update-from-sales-receipt/";

            string Uri = $"{APIEndpoint.Sales}{salesInvoiceUri}{id}";
            var data = new
            {
                model.TotalPaid,
                model.IsPaidOff,
            };

            IHttpClientService httpClient = (IHttpClientService)this._serviceProvider.GetService(typeof(IHttpClientService));
            var response = httpClient.PutAsync(Uri, new StringContent(JsonConvert.SerializeObject(data).ToString(), Encoding.UTF8, General.JsonMediaType)).Result; if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("{0}, {1}, {2}", response.StatusCode, response.Content, APIEndpoint.Purchasing));
            }
        }

        public List<SalesInvoiceReportSalesReceiptViewModel> GetSalesInvoice(SalesInvoicePostForm dataForm)
        {
            var query = _DetailDbSet.Include(s => s.SalesReceiptModel).Where(s => dataForm.SalesInvoiceIds.Contains(s.SalesInvoiceId))
                .Select(s => new SalesInvoiceReportSalesReceiptViewModel()
                {
                    CurrencySymbol = s.CurrencySymbol,
                    SalesInvoiceId = s.SalesInvoiceId,
                    Nominal = s.Nominal,
                    SalesReceiptDate = s.SalesReceiptModel.SalesReceiptDate,
                    SalesReceiptNo = s.SalesReceiptModel.SalesReceiptNo,
                    TotalPaid = s.TotalPaid,
                    UnPaid = s.Unpaid
                }).ToList();

            return query;
        }

        private IQueryable<SalesReceiptReportViewModel> GetReportQuery(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet)
        {
            var query = _DbSet.AsQueryable();

            if (dateFrom.HasValue && dateTo.HasValue)
            {
                query = query.Where(s => dateFrom.Value.Date <= s.SalesReceiptDate.AddHours(offSet).Date && s.SalesReceiptDate.AddHours(offSet).Date <= dateTo.Value.Date);
            }
            else if (dateFrom.HasValue && !dateTo.HasValue)
            {
                query = query.Where(s => dateFrom.Value.Date <= s.SalesReceiptDate.AddHours(offSet).Date);
            }
            else if (!dateFrom.HasValue && dateTo.HasValue)
            {
                query = query.Where(s => s.SalesReceiptDate.AddHours(offSet).Date <= dateTo.Value.Date);
            }

            var result = query.OrderBy(s => s.SalesReceiptDate)
                .Select(s => new SalesReceiptReportViewModel()
                {
                    Buyer = s.BuyerName,
                    SalesReceiptDate = s.SalesReceiptDate,
                    CurrencyCode = s.CurrencyCode,
                    TotalPaid = s.TotalPaid,
                    SalesReceiptNo = s.SalesReceiptNo
                });

            return result;
        }

        public List<SalesReceiptReportViewModel> GetReport(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet)
        {
            var data = GetReportQuery(dateFrom, dateTo, offSet);

            return data.ToList();
        }

        public MemoryStream GenerateExcel(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet)
        {
            var data = GetReportQuery(dateFrom, dateTo, offSet);
            string title = "Laporan Kwitansi",
                dateStart = dateFrom == null ? "-" : dateFrom.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                dateEnd = dateTo == null ? "-" : dateTo.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID"));

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "No Kwitansi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Pembayaran", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Jumlah Pembayaran", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(string) });

            int index = 0;
            if (data.Count() == 0)
            {
                dt.Rows.Add("", "", 0.ToString("#,##0.#0"), "", "");
                index++;
            }
            else
            {
                foreach (var item in data)
                {
                    dt.Rows.Add(item.SalesReceiptNo, item.SalesReceiptDate.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.TotalPaid.ToString("#,##0.#0"), item.CurrencyCode, item.Buyer);
                    index++;
                }
            }

            return Excel.CreateExcelWithTitle(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Bukti Pembayaran Faktur") },
                new List<KeyValuePair<string, int>>() { new KeyValuePair<string, int>("Bukti Pembayaran Faktur", index) }, title, dateStart, dateEnd, true);
        }
    }
}
