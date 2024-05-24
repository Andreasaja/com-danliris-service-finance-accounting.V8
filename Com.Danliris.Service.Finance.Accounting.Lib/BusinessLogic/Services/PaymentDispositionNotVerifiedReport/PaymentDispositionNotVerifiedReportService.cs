using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.PaymentDispositionNotVerifiedReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNotVerifiedReportViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using System.Data;
using System.Globalization;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PaymentDispositionNotVerifiedReport
{
    public class PaymentDispositionNotVerifiedReportService : IPaymentDispositionNotVerifiedReport
    {
        private const string UserAgent = "finance-service";
        protected DbSet<PurchasingDispositionExpeditionModel> DbSet;
        public IIdentityService IdentityService;
        public readonly IServiceProvider ServiceProvider;
        public FinanceDbContext DbContext;

        public PaymentDispositionNotVerifiedReportService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            DbContext = dbContext;
            ServiceProvider = serviceProvider;
            DbSet = dbContext.Set<PurchasingDispositionExpeditionModel>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
        }
        public IQueryable<PaymentDispositionNotVerifiedReportViewModel> GetReportQuery(string no, string supplier, string division, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offset, string type)
        {
            DateTimeOffset dateFromFilter = (dateFrom == null ? new DateTime(1970, 1, 1) : dateFrom.Value.Date);
            DateTimeOffset dateToFilter = (dateTo == null ? DateTimeOffset.UtcNow.Date : dateTo.Value.Date);

            var header = DbContext.PurchasingDispositionExpeditions.AsQueryable();

            if (type == "notHistory")
            {
                header = header.GroupBy(x => x.DispositionNo).ToList()
                        .Select(g => g.OrderByDescending(x => x.LastModifiedUtc).FirstOrDefault()).AsQueryable();
            }

            var Query = (from a in header
                         where a.Position == ExpeditionPosition.SEND_TO_PURCHASING_DIVISION
                         && a.IsDeleted == false
                         && a.DispositionNo == (string.IsNullOrWhiteSpace(no) ? a.DispositionNo : no)
                         && a.SupplierCode == (string.IsNullOrWhiteSpace(supplier) ? a.SupplierCode : supplier)
                         && a.DivisionCode == (string.IsNullOrWhiteSpace(division) ? a.DivisionCode : division)
                         && a.VerifyDate >= dateFromFilter
                         && a.VerifyDate <= dateToFilter

                         select new PaymentDispositionNotVerifiedReportViewModel
                       {
                             DispositionNo = a.DispositionNo,
                             DivisionName = a.DivisionName,
                             SupplierName = a.SupplierName,
                             VerifyDate = a.VerifyDate,
                             Currency = a.CurrencyCode,
                             DispositionDate = a.DispositionDate,
                             PayToSupplier = a.PayToSupplier,
                             NotVerifiedReason = a.NotVerifiedReason,
                             LastModifiedUtc = a.LastModifiedUtc,
                             CreatedUtc = a.CreatedUtc
                         });

            return Query;
        }

        public Tuple<List<PaymentDispositionNotVerifiedReportViewModel>, int> GetReport(string no, string supplier, string division, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int page, int size, string Order, int offset, string type)
        {
            var Query = GetReportQuery(no, supplier, division, dateFrom, dateTo, offset, type);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = Query.OrderByDescending(b => b.DispositionNo).ThenByDescending(b=>b.LastModifiedUtc);


            Pageable<PaymentDispositionNotVerifiedReportViewModel> pageable = new Pageable<PaymentDispositionNotVerifiedReportViewModel>(Query, page - 1, size);
            List<PaymentDispositionNotVerifiedReportViewModel> Data = pageable.Data.ToList<PaymentDispositionNotVerifiedReportViewModel>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData);
        }

        public MemoryStream GenerateExcel(string no, string supplier, string division, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offset, string type)
        {
            string title = type == "history" ? "Histori Disposisi Not Verified" : "Laporan Disposisi Not Verified",
                _dateFrom = dateFrom == null ? "-" : dateFrom.Value.Date.ToString("dd MMMM yyy"),
                _dateTo = dateTo == null ? "-" : dateTo.Value.Date.ToString("dd MMMM yyy");


            var Query = GetReportQuery(no, supplier, division, dateFrom, dateTo, offset, type);
            Query = Query.OrderByDescending(b => b.DispositionNo).ThenByDescending(b => b.LastModifiedUtc);
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Verifikasi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "No Disposisi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Disposisi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal Kirim dari Pembelian", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Supplier", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Divisi", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Total Bayar", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Alasan", DataType = typeof(String) });

            int index = 0;
            if (Query.ToArray().Count() == 0)
            {
                result.Rows.Add("", "", "", "", "", "", 0.ToString("#,##0.#0"), "", ""); // to allow column name to be generated properly for empty data as template
                index++;
            }
            else
            {
                foreach (var item in Query)
                {
                    index++;
                    DateTimeOffset vDate = item.VerifyDate ?? new DateTime(1970, 1, 1);
                    string verifyDate = vDate == new DateTime(1970, 1, 1) ? "-" : vDate.ToOffset(new TimeSpan(offset, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                    result.Rows.Add(verifyDate, item.DispositionNo, item.DispositionDate.ToOffset(new TimeSpan(offset, 0, 0)).ToString("dd MMM yyyy", new CultureInfo("id-ID")),
                        item.CreatedUtc.AddHours(offset).ToString("dd MMM yyyy", new CultureInfo("id-ID")),
                        item.SupplierName, item.DivisionName, item.PayToSupplier.ToString("#,##0.#0"), item.Currency, item.NotVerifiedReason);
                }
            }

            return Excel.CreateExcelWithTitle(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") },
                new List<KeyValuePair<string, int>>() { new KeyValuePair<string, int>("Territory", index) }, title, _dateFrom, _dateTo, true);
        }
    }
}
