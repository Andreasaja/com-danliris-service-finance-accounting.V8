using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBStatusReport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBStatusReport;
using Com.Moonlay.Models;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBStatusReport
{
    public class VBStatusReportService : IVBStatusReportService
    {
        private const string _UserAgent = "finance-service";
        protected DbSet<VbRequestModel> _DbSet;
        protected DbSet<RealizationVbModel> _RealizationDbSet;
        private readonly IServiceProvider _serviceProvider;
        protected IIdentityService _IdentityService;
        public FinanceDbContext _DbContext;

        public VBStatusReportService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _DbContext = dbContext;
            _DbSet = dbContext.Set<VbRequestModel>();
            _RealizationDbSet = dbContext.Set<RealizationVbModel>();
            _serviceProvider = serviceProvider;
            _IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        private List<VBStatusReportViewModel> NewGetReportQuery(int unitId, long vbRequestId, string applicantName, string clearanceStatus, DateTimeOffset? requestDateFrom, DateTimeOffset? requestDateTo, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, DateTimeOffset? clearanceDateFrom, DateTimeOffset? clearanceDateTo, int offSet)
        {
            var requestQuery = _DbContext.VBRequestDocuments.AsNoTracking().Where(s => s.ApprovalStatus > ApprovalStatus.Draft);

            var realizationQuery = _DbContext.VBRealizationDocuments.AsNoTracking();

            var realizationExpenditureQuery = _DbContext.VBRealizationDocumentExpenditureItems.AsNoTracking();

            var expeditionQuery = _DbContext.VBRealizationDocumentExpeditions.AsNoTracking().Where(entity => entity.Position == VBRealizationDocumentExpedition.VBRealizationPosition.Cashier);

            if (unitId != 0)
            {
                requestQuery = requestQuery.Where(s => s.SuppliantUnitId == unitId);
            }

            if (vbRequestId != 0)
            {
                requestQuery = requestQuery.Where(s => s.Id == vbRequestId);
            }

            if (!string.IsNullOrEmpty(applicantName))
            {
                requestQuery = requestQuery.Where(s => s.CreatedBy == applicantName);
            }

            if (requestDateFrom.HasValue && requestDateTo.HasValue)
            {
                requestQuery = requestQuery.Where(s => requestDateFrom.Value.Date <= s.Date.AddHours(offSet).Date && s.Date.AddHours(offSet).Date <= requestDateTo.Value.Date);
            }
            else if (requestDateFrom.HasValue && !requestDateTo.HasValue)
            {
                requestQuery = requestQuery.Where(s => requestDateFrom.Value.Date <= s.Date.AddHours(offSet).Date);
            }
            else if (!requestDateFrom.HasValue && requestDateTo.HasValue)
            {
                requestQuery = requestQuery.Where(s => s.Date.AddHours(offSet).Date <= requestDateTo.Value.Date);
            }

            if (clearanceDateFrom.HasValue && clearanceDateTo.HasValue)
            {
                requestQuery = requestQuery.Where(s => clearanceDateFrom.Value.Date <= s.CompletedDate.GetValueOrDefault().AddHours(offSet).Date && s.CompletedDate.GetValueOrDefault().AddHours(offSet).Date <= clearanceDateTo.Value.Date);
            }
            else if (clearanceDateFrom.HasValue && !clearanceDateTo.HasValue)
            {
                requestQuery = requestQuery.Where(s => clearanceDateFrom.Value.Date <= s.CompletedDate.GetValueOrDefault().AddHours(offSet).Date);
            }
            else if (!clearanceDateFrom.HasValue && clearanceDateTo.HasValue)
            {
                requestQuery = requestQuery.Where(s => s.CompletedDate.GetValueOrDefault().AddHours(offSet).Date <= clearanceDateTo.Value.Date);
            }

            if (clearanceStatus.ToUpper() == "CLEARANCE")
            {
                requestQuery = requestQuery.Where(s => s.IsCompleted);
            }
            else if (clearanceStatus.ToUpper() == "OUTSTANDING")
            {
                requestQuery = requestQuery.Where(s => !s.IsCompleted && s.ApprovalStatus != ApprovalStatus.Cancelled);
            }
            else if (clearanceStatus.ToUpper() == "CANCEL")
            {
                requestQuery = requestQuery.Where(s => s.ApprovalStatus == ApprovalStatus.Cancelled);
            }

            IQueryable<VBStatusReportViewModel> result;

            if (!realizeDateFrom.HasValue && !realizeDateTo.HasValue)
            {
                result = (from rqst in requestQuery
                          join real in realizationQuery
                          on rqst.Id equals real.VBRequestDocumentId into data
                          from real in data.DefaultIfEmpty()
                          join expedition in expeditionQuery
                          on real.Id equals expedition.VBRealizationId into expdata
                          from expedition in expdata.DefaultIfEmpty()
                          select new VBStatusReportViewModel()
                          {
                              Id = rqst.Id,
                              VBNo = rqst.DocumentNo,
                              Date = rqst.Date.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                              DateEstimate = rqst.RealizationEstimationDate.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                              Unit = new Unit()
                              {
                                  Id = rqst.SuppliantUnitId,
                                  Name = rqst.SuppliantUnitName
                              },
                              CreateBy = rqst.CreatedBy,
                              RealizationNo = real.DocumentNo,
                              RealizationDate = real.Date.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                              Usage = rqst.Purpose,
                              //Aging = rqst.IsCompleted ? (int)(rqst.CompletedDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date - rqst.ApprovalDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date).TotalDays
                              //      : (int)(requestDateTo.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date - rqst.ApprovalDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date).TotalDays,
                              Aging = expedition != null && expedition.CashierReceiptDate != null && rqst.IsCompleted ? (int)(expedition.CashierReceiptDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date - rqst.ApprovalDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date).TotalDays
                                    : (int)(requestDateTo.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date - rqst.ApprovalDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date).TotalDays,
                              Amount = rqst.Amount,
                              RealizationAmount = real != null ? real.Amount : 0,
                              Difference = real != null ? rqst.Amount - real.Amount : 0,
                              Status = rqst.ApprovalStatus == ApprovalStatus.Cancelled ? "Cancel" : rqst.IsCompleted ? "Clearance" : "Outstanding",
                              LastModifiedUtc = real.LastModifiedUtc.AddHours(offSet).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                              ApprovalDate = rqst.ApprovalDate.HasValue ? rqst.ApprovalDate.Value.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID"))
                                : "-",
                              ClearenceDate = rqst.CompletedDate.HasValue ? rqst.CompletedDate.Value.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID"))
                                : "-",
                              CurrencyCode = rqst.CurrencyCode,
                              IsInklaring = rqst.IsInklaring,
                              NoBL = rqst.NoBL

                          })
                              .OrderByDescending(s => s.LastModifiedUtc);
            }
            else
            {
                if (realizeDateFrom.HasValue && realizeDateTo.HasValue)
                {
                    realizationQuery = realizationQuery.Where(s => realizeDateFrom.Value.Date <= s.Date.AddHours(offSet).Date && s.Date.AddHours(offSet).Date <= realizeDateTo.Value.Date);
                }
                else if (realizeDateFrom.HasValue && !realizeDateTo.HasValue)
                {
                    realizationQuery = realizationQuery.Where(s => realizeDateFrom.Value.Date <= s.Date.AddHours(offSet).Date);
                }
                else if (!realizeDateFrom.HasValue && realizeDateTo.HasValue)
                {
                    realizationQuery = realizationQuery.Where(s => s.Date.AddHours(offSet).Date <= realizeDateTo.Value.Date);
                }

                result = (from rqst in requestQuery
                          join real in realizationQuery
                          on rqst.Id equals real.VBRequestDocumentId
                          join expedition in expeditionQuery
                          on real.Id equals expedition.VBRealizationId
                          select new VBStatusReportViewModel()
                          {
                              Id = rqst.Id,
                              VBNo = rqst.DocumentNo,
                              Date = rqst.Date.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                              DateEstimate = rqst.RealizationEstimationDate.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                              Unit = new Unit()
                              {
                                  Id = rqst.SuppliantUnitId,
                                  Name = rqst.SuppliantUnitName
                              },
                              CreateBy = rqst.CreatedBy,
                              RealizationNo = real.DocumentNo,
                              RealizationDate = real.Date.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                              Usage = rqst.Purpose,
                              //Aging = rqst.IsCompleted ? (int)(rqst.CompletedDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date - rqst.ApprovalDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date).TotalDays
                              //      : (int)(requestDateTo.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date - rqst.ApprovalDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date).TotalDays,
                              Aging = expedition != null && expedition.CashierReceiptDate != null && rqst.IsCompleted ? (int)(expedition.CashierReceiptDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date - rqst.ApprovalDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date).TotalDays
                                    : (int)(requestDateTo.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date - rqst.ApprovalDate.GetValueOrDefault().ToOffset(new TimeSpan(offSet, 0, 0)).Date).TotalDays,
                              Amount = rqst.Amount,
                              RealizationAmount = real != null ? real.Amount : 0,
                              Difference = real != null ? rqst.Amount - real.Amount : 0,
                              Status = rqst.ApprovalStatus == ApprovalStatus.Cancelled ? "Cancel" : rqst.IsCompleted ? "Clearance" : "Outstanding",
                              LastModifiedUtc = real.LastModifiedUtc.AddHours(offSet).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                              ApprovalDate = rqst.ApprovalDate.HasValue ? rqst.ApprovalDate.Value.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID"))
                                : "-",
                              ClearenceDate = rqst.CompletedDate.HasValue ? rqst.CompletedDate.Value.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("dd MMMM yyyy", new CultureInfo("id-ID"))
                                : "-",
                              IsInklaring = rqst.IsInklaring,
                              NoBL = rqst.NoBL

                          })
                          .OrderByDescending(s => s.LastModifiedUtc);
            }

           
            return result.ToList();
        }

        public List<VBStatusReportViewModel> GetReport(int unitId, long vbRequestId, string applicantName, string clearanceStatus, DateTimeOffset? requestDateFrom, DateTimeOffset? requestDateTo, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, DateTimeOffset? clearanceDateFrom, DateTimeOffset? clearanceDateTo, int offSet)
        {
            var data = NewGetReportQuery(unitId, vbRequestId, applicantName, clearanceStatus, requestDateFrom, requestDateTo, realizeDateFrom, realizeDateTo, clearanceDateFrom, clearanceDateTo, offSet);

            return data;
        }

        public MemoryStream GenerateExcel(int unitId, long vbRequestId, string applicantName, string clearanceStatus, DateTimeOffset? requestDateFrom, DateTimeOffset? requestDateTo, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, DateTimeOffset? clearanceDateFrom, DateTimeOffset? clearanceDateTo, int offSet)
        {
            var data = NewGetReportQuery(unitId, vbRequestId, applicantName, clearanceStatus, requestDateFrom, requestDateTo, realizeDateFrom, realizeDateTo, clearanceDateFrom, clearanceDateTo, offSet);

            var currencyGroup = data
                .GroupBy(element => element.CurrencyCode)
                .Select(group => new VBStatusByCurrencyReportViewModel()
                {
                    CurrencyCode = group.Key,
                    Total = group.Sum(element => element.Amount)
                }).ToList();

            var dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "No VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Estimasi Tgl Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Unit", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Pemohon VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Approval", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Keperluan VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Aging (Hari)", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Jumlah VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Sisa (Kurang/Lebih)", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tanggal Clearance", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Status", DataType = typeof(string) });

            var dtCurrency = new DataTable();
            dtCurrency.Columns.Add(new DataColumn() { ColumnName = "Mata Uang", DataType = typeof(string) });
            dtCurrency.Columns.Add(new DataColumn() { ColumnName = "Total", DataType = typeof(string) });

            var requestTotal = 0.0;
            var realizationTotal = 0.0;
            if (data.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            }
            else
            {
                data = data.OrderByDescending(s => s.LastModifiedUtc).ToList();

                foreach (var item in data)
                {
                    dt.Rows.Add(item.VBNo, item.Date, item.DateEstimate, item.Unit.Name, item.CreateBy, item.ApprovalDate, item.RealizationNo, item.RealizationDate, item.Usage, item.Aging, item.CurrencyCode,
                        item.Amount.ToString("#,##0.###0"), item.RealizationAmount.ToString("#,##0.###0"), item.Difference.ToString("#,##0.###0"), item.ClearenceDate, item.Status);
                }

                requestTotal = (double)data.Sum(element => element.Amount);
                realizationTotal = (double)data.Sum(element => element.RealizationAmount);
            }

            if (currencyGroup.Count == 0)
            {
                dtCurrency.Rows.Add("", "");
            }
            else
            {
                currencyGroup = currencyGroup.OrderBy(element => element.CurrencyCode).ToList();
                foreach (var item in currencyGroup)
                {
                    dtCurrency.Rows.Add(item.CurrencyCode, item.Total.ToString("#,##0.###0"));
                }
            }

            return Excel.CreateExcelVBStatusReport(new KeyValuePair<DataTable, string>(dt, "Status VB"), new KeyValuePair<DataTable, string>(dtCurrency, "MataUang"), requestDateFrom.GetValueOrDefault(), requestDateTo.GetValueOrDefault(), true, requestTotal, realizationTotal);
        }

        public Task<int> CreateAsync(VbRequestModel model)
        {
            EntityExtension.FlagForCreate(model, _IdentityService.Username, _UserAgent);

            _DbContext.VbRequests.Add(model);

            return _DbContext.SaveChangesAsync();
        }

        public Task<VbRequestModel> ReadByIdAsync(int id)
        {
            return _DbContext.VbRequests.Where(entity => entity.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<VbRequestModel>> GetByApplicantName(string applicantName)
        {
            var data = await _DbContext.VbRequests.Where(entity => entity.CreatedBy == applicantName).ToListAsync();
            return data;
        }

        public ReportViewModel GetReportWithCurrency(int unitId, long vbRequestId, string applicantName, string clearanceStatus, DateTimeOffset? requestDateFrom, DateTimeOffset? requestDateTo, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, DateTimeOffset? clearanceDateFrom, DateTimeOffset? clearanceDateTo, int offSet)
        {
            var data = NewGetReportQuery(unitId, vbRequestId, applicantName, clearanceStatus, requestDateFrom, requestDateTo, realizeDateFrom, realizeDateTo, clearanceDateFrom, clearanceDateTo, offSet);

            var currencyGroup = data
                .GroupBy(element => element.CurrencyCode)
                .Select(group => new VBStatusByCurrencyReportViewModel()
                {
                    CurrencyCode = group.Key,
                    Total = group.Sum(element => element.Amount)
                }).ToList();

            return new ReportViewModel()
            {
                VBStatusByCurrencyReport = currencyGroup,
                VBStatusReport = data
            };
        }
    }
}
