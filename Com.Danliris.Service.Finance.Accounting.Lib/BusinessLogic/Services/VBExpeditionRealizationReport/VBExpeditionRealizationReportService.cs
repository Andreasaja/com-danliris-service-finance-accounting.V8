using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBExpeditionRealizationReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBExpeditionRealizationReport;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBExpeditionRealizationReport
{
    public class VBExpeditionRealizationReportService : IVBExpeditionRealizationReportService
    {
        private const string _UserAgent = "finance-service";
        protected DbSet<VbRequestModel> _DbSet;
        protected DbSet<RealizationVbModel> _RealizationDbSet;
        private readonly IServiceProvider _serviceProvider;
        protected IIdentityService _IdentityService;
        public FinanceDbContext _DbContext;

        public VBExpeditionRealizationReportService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _DbContext = dbContext;
            _DbSet = dbContext.Set<VbRequestModel>();
            _RealizationDbSet = dbContext.Set<RealizationVbModel>();
            _serviceProvider = serviceProvider;
            _IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        private async Task<List<VBExpeditionRealizationReportViewModel>> GetReportQuery(int vbRequestId, int vbRealizeId, string ApplicantName, int unitId, int divisionId, string isVerified, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, int offSet)
        {
            var requestQuery = _DbSet.AsQueryable().Where(s => s.IsDeleted == false);

            if (vbRequestId != 0)
            {
                requestQuery = requestQuery.Where(s => s.Id == vbRequestId);
            }

            if (!string.IsNullOrEmpty(ApplicantName))
            {
                requestQuery = requestQuery.Where(s => s.CreatedBy == ApplicantName);
            }

            if (unitId != 0)
            {
                requestQuery = requestQuery.Where(s => s.UnitId == unitId);
            }

            if (divisionId != 0)
            {
                requestQuery = requestQuery.Where(s => s.UnitDivisionId == divisionId);
            }

            var realizationQuery = _RealizationDbSet.AsQueryable().Where(s => s.IsDeleted == false);

            if (vbRealizeId != 0)
            {
                realizationQuery = realizationQuery.Where(s => s.Id == vbRealizeId);
            }

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

            var result = new List<VBExpeditionRealizationReportViewModel>();

            switch (isVerified.ToUpper())
            {
                case "CASHIER":
                    result = requestQuery
                    .Join(realizationQuery,
                    (rqst) => rqst.VBNo,
                    (real) => real.VBNo,
                    (rqst, real) => new VBExpeditionRealizationReportViewModel()
                    {
                        Id = rqst.Id,
                        RequestVBNo = rqst.VBNo,
                        RealizationVBNo = real.VBNoRealize,
                        VbType = rqst.VBRequestCategory,
                        Applicant = rqst.CreatedBy,
                        Unit = new Unit()
                        {
                            Id = rqst.Id,
                            Name = rqst.UnitName,
                        },
                        Division = new Division()
                        {
                            Id = rqst.Id,
                            Name = rqst.UnitDivisionName,
                        },
                        DateUnitSend = null,
                        Usage = rqst.Usage,
                        RequestCurrency = rqst.CurrencyCode,
                        RequestAmount = rqst.Amount,
                        RealizationCurrency = real.CurrencyCode,
                        RealizationAmount = real.Amount,
                        RealizationDate = real.Date,
                        DateVerifReceive = null,
                        Verificator = real.CreatedBy,
                        DateVerifSend = real.VerifiedDate,
                        Status = real.isVerified ? "K" : "",
                        VerificationStatus = real.isNotVeridied ? "R" : "",
                        Notes = real.Reason_NotVerified,
                        DateCashierReceive = rqst.CompleteDate,
                        LastModifiedUtc = real.LastModifiedUtc,
                        Position = real.Position,
                    })
                    //.Where(t => t.Status == "K" && t.VerificationStatus != "R")
                    .Where(t => t.Position == 5)
                    .OrderByDescending(s => s.LastModifiedUtc)
                    .ToList();
                    break;

                case "RETURN":
                    result = requestQuery
                    .Join(realizationQuery,
                    (rqst) => rqst.VBNo,
                    (real) => real.VBNo,
                    (rqst, real) => new VBExpeditionRealizationReportViewModel()
                    {
                        Id = rqst.Id,
                        RequestVBNo = rqst.VBNo,
                        RealizationVBNo = real.VBNoRealize,
                        VbType = rqst.VBRequestCategory,
                        Applicant = rqst.CreatedBy,
                        Unit = new Unit()
                        {
                            Id = rqst.Id,
                            Name = rqst.UnitName,
                        },
                        Division = new Division()
                        {
                            Id = rqst.Id,
                            Name = rqst.UnitDivisionName,
                        },
                        DateUnitSend = null,
                        Usage = rqst.Usage,
                        RequestCurrency = rqst.CurrencyCode,
                        RequestAmount = rqst.Amount,
                        RealizationCurrency = real.CurrencyCode,
                        RealizationAmount = real.Amount,
                        RealizationDate = real.Date,
                        DateVerifReceive = null,
                        Verificator = real.CreatedBy,
                        DateVerifSend = real.VerifiedDate,
                        Status = real.isVerified ? "K" : "",
                        VerificationStatus = real.isNotVeridied ? "R" : "",
                        Notes = real.Reason_NotVerified,
                        DateCashierReceive = rqst.CompleteDate,
                        LastModifiedUtc = real.LastModifiedUtc,
                        Position = real.Position,
                    })
                    //.Where(t => t.Status != "K" && t.VerificationStatus == "R")
                    .Where(t => t.Position == 6)
                    .OrderByDescending(s => s.LastModifiedUtc)
                    .ToList();
                    break;

                case "ALL":
                    result = requestQuery
                    .Join(realizationQuery,
                    (rqst) => rqst.VBNo,
                    (real) => real.VBNo,
                    (rqst, real) => new VBExpeditionRealizationReportViewModel()
                    {
                        Id = rqst.Id,
                        RequestVBNo = rqst.VBNo,
                        RealizationVBNo = real.VBNoRealize,
                        VbType = rqst.VBRequestCategory,
                        Applicant = rqst.CreatedBy,
                        Unit = new Unit()
                        {
                            Id = rqst.Id,
                            Name = rqst.UnitName,
                        },
                        Division = new Division()
                        {
                            Id = rqst.Id,
                            Name = rqst.UnitDivisionName,
                        },
                        DateUnitSend = null,
                        Usage = rqst.Usage,
                        RequestCurrency = rqst.CurrencyCode,
                        RequestAmount = rqst.Amount,
                        RealizationCurrency = real.CurrencyCode,
                        RealizationAmount = real.Amount,
                        RealizationDate = real.Date,
                        DateVerifReceive = null,
                        Verificator = real.CreatedBy,
                        DateVerifSend = real.VerifiedDate,
                        Status = real.isVerified ? "K" : "",
                        VerificationStatus = real.isNotVeridied ? "R" : "",
                        Notes = real.Reason_NotVerified,
                        DateCashierReceive = rqst.CompleteDate,
                        LastModifiedUtc = real.LastModifiedUtc,
                        Position = real.Position,
                    })
                    //.Where(t => t.Status == "K" || t.VerificationStatus == "R")
                    .Where(t => t.Position == 5 || t.Position == 6)
                    .OrderByDescending(s => s.LastModifiedUtc)
                    .ToList();
                    break;
            }

            return result.ToList();
        }

        public async Task<List<VBExpeditionRealizationReportViewModel>> GetReport(int vbRequestId, int vbRealizeId, string ApplicantName, int unitId, int divisionId, string isVerified, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, int offSet)
        {
            var data = await GetReportQuery(vbRequestId, vbRealizeId, ApplicantName, unitId, divisionId, isVerified, realizeDateFrom, realizeDateTo, offSet);

            return data;
        }

        public async Task<MemoryStream> GenerateExcel(int vbRequestId, int vbRealizeId, string ApplicantName, int unitId, int divisionId, string isVerified, DateTimeOffset? realizeDateFrom, DateTimeOffset? realizeDateTo, int offSet)
        {
            var data = await GetReportQuery(vbRequestId, vbRealizeId, ApplicantName, unitId, divisionId, isVerified, realizeDateFrom, realizeDateTo, offSet);

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn() { ColumnName = "No VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "No. Realisasi VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tipe VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nama", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Bagian/Unit", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Divisi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Unit Kirim", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Keperluan", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kurs VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nominal VB", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Kurs Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nominal Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Realisasi", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Verif Terima", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Nama Verifikator", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Verif Kirim Kasir/Retur", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Status Kasir/Retur", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Keterangan (Retur)", DataType = typeof(string) });
            dt.Columns.Add(new DataColumn() { ColumnName = "Tgl. Kasir Terima", DataType = typeof(string) });

            if (data.Count == 0)
            {
                dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            }
            else
            {
                data = data.OrderByDescending(s => s.LastModifiedUtc).ToList();
                foreach (var item in data)
                {
                    dt.Rows.Add(
                        item.RequestVBNo,
                        item.RealizationVBNo,
                        item.VbType,
                        item.Applicant,
                        item.Unit.Name,
                        item.Division.Name,
                        item.DateUnitSend?.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.Usage,
                        item.RequestCurrency,
                        item.RequestAmount,
                        item.RealizationCurrency,
                        item.RealizationAmount,
                        item.RealizationDate.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.DateVerifReceive?.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.Verificator,
                        item.DateVerifSend.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID")),
                        item.Status,
                        item.Notes,
                        item.DateCashierReceive?.ToOffset(new TimeSpan(offSet, 0, 0)).ToString("d/M/yyyy", new CultureInfo("id-ID"))
                        );
                }
            }

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(dt, "Ekspedisi Realisasi VB") }, true);
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
    }
}
