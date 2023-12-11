using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition
{
    public class VBRealizationDocumentExpeditionService : IVBRealizationDocumentExpeditionService
    {
        private const string UserAgent = "finance-accounting-service";
        private readonly DbSet<VBRequestDocumentModel> _RequestDbSet;
        private readonly DbSet<VBRealizationDocumentModel> _RealizationDbSet;
        private readonly IServiceProvider _serviceProvider;
        private readonly IIdentityService _identityService;
        private readonly IAutoJournalService _autoJournalService;
        private readonly IAutoDailyBankTransactionService _autoDailyBankTransactionService;
        private readonly FinanceDbContext _dbContext;

        public VBRealizationDocumentExpeditionService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _RequestDbSet = dbContext.Set<VBRequestDocumentModel>();
            _RealizationDbSet = dbContext.Set<VBRealizationDocumentModel>();
            _serviceProvider = serviceProvider;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _autoJournalService = serviceProvider.GetService<IAutoJournalService>();
            _autoDailyBankTransactionService = serviceProvider.GetService<IAutoDailyBankTransactionService>();
        }

        public Task<int> CashierReceipt(List<int> vbRealizationIds)
        {
            var models = _dbContext.VBRealizationDocumentExpeditions.Where(entity => vbRealizationIds.Contains(entity.VBRealizationId) && entity.Position == VBRealizationPosition.VerifiedToCashier).ToList();
            var documents = _dbContext.VBRealizationDocuments.Where(a => vbRealizationIds.Contains(a.Id)).ToList();
            models.ForEach(model =>
            {                   
                model.CashierVerification(_identityService.Username);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            });

            documents.ForEach(document =>
            {
                var journals = _dbContext.JournalTransactions.Include(a => a.Items).Where(entity => entity.ReferenceNo == document.ReferenceNo).ToList();
                journals.ForEach(journal =>
                {
                    journal.Items.ToList().ForEach(journalItem =>
                    {
                        journalItem.Remark = document.Remark;
                    });
                });
            });

            _dbContext.VBRealizationDocumentExpeditions.UpdateRange(models);
            UpdateVBRealizationPosition(vbRealizationIds, VBRealizationPosition.Cashier, "");

            return _dbContext.SaveChangesAsync();
        }

        private void UpdateVBRealizationPosition(int vbRealizationId, VBRealizationPosition position, string reason)
        {
            var model = _dbContext.VBRealizationDocuments.FirstOrDefault(entity => entity.Id == vbRealizationId);
            model.UpdatePosition(position, _identityService.Username, UserAgent);

            if (position == VBRealizationPosition.VerifiedToCashier || position == VBRealizationPosition.NotVerified || position == VBRealizationPosition.Cashier)
                model.UpdateVerified(position, reason, _identityService.Username, UserAgent);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.VBRealizationDocuments.Update(model);
        }

        private void UpdateVBRealizationPosition(List<int> vbRealizationIds, VBRealizationPosition position, string reason)
        {
            var models = _dbContext.VBRealizationDocuments.Where(entity => vbRealizationIds.Contains(entity.Id)).ToList();
            models.ForEach(model =>
            {
                model.UpdatePosition(position, _identityService.Username, UserAgent);
                if (position == VBRealizationPosition.VerifiedToCashier || position == VBRealizationPosition.NotVerified)
                    model.UpdateVerified(position, reason, _identityService.Username, UserAgent);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            });
            _dbContext.VBRealizationDocuments.UpdateRange(models);
        }

        public async Task<VBRealizationDocumentExpeditionReportDto> GetReports(int vbId, int vbRealizationId, string vbRequestName, int unitId, int divisionId, DateTimeOffset dateStart, DateTimeOffset dateEnd, string status, int page = 1, int size = 25)
        {
            var vbRealizationQuery = _dbContext.VBRealizationDocuments.AsQueryable();
            var expeditionQuery = _dbContext.VBRealizationDocumentExpeditions.AsQueryable();

            var query = from realization in vbRealizationQuery
                        join expedition in expeditionQuery on realization.Id equals expedition.VBRealizationId into realizationExpeditions

                        from realizationExpedition in realizationExpeditions.DefaultIfEmpty()

                        select new ReportDto()
                        {
                            CashierReceiptBy = realizationExpedition != null ? realizationExpedition.CashierReceiptBy : null,
                            CashierReceiptDate = realizationExpedition != null ? realizationExpedition.CashierReceiptDate : null,
                            CurrencyCode = realization.CurrencyCode,
                            CurrencyRate = realization.CurrencyRate,
                            DivisionId = realization.SuppliantDivisionId,
                            DivisionName = realization.SuppliantDivisionName,
                            NotVerifiedBy = realizationExpedition != null ? realizationExpedition.NotVerifiedBy : null,
                            NotVerifiedDate = realizationExpedition != null ? realizationExpedition.NotVerifiedDate : null,
                            NotVerifiedReason = realizationExpedition != null ? realizationExpedition.NotVerifiedReason : null,
                            Position = realizationExpedition != null ? realizationExpedition.Position : VBRealizationPosition.Purchasing,
                            SendToVerificationBy = realizationExpedition != null ? realizationExpedition.SendToVerificationBy : null,
                            SendToVerificationDate = realizationExpedition != null ? realizationExpedition.SendToVerificationDate : null,
                            UnitId = realization.SuppliantUnitId,
                            UnitName = realization.SuppliantUnitName,
                            VBAmount = realization.VBRequestDocumentAmount,
                            VBId = realization.VBRequestDocumentId,
                            VBNo = realization.VBRequestDocumentNo,
                            VBRealizationAmount = realization.Amount,
                            VBRealizationDate = realization.Date,
                            VBRealizationId = realization.Id,
                            VBRealizationNo = realization.DocumentNo,
                            VBRequestName = !string.IsNullOrWhiteSpace(realization.VBRequestDocumentCreatedBy) ? realization.VBRequestDocumentCreatedBy : realization.CreatedBy,
                            VBType = realization.Type,
                            VerificationReceiptBy = realizationExpedition != null ? realizationExpedition.VerificationReceiptBy : null,
                            VerificationReceiptDate = realizationExpedition != null ? realizationExpedition.VerificationReceiptDate : null,
                            VerifiedToCashierBy = realizationExpedition != null ? realizationExpedition.VerifiedToCashierBy : null,
                            VerifiedToCashierDate = realizationExpedition != null ? realizationExpedition.VerifiedToCashierDate : null,
                            Purpose = realization.VBRequestDocumentPurpose,
                            LastModifiedDate = realization.LastModifiedUtc,
                            RemarkRealization = realization.Remark,
                        };

            query = query.Where(entity => entity.VBRealizationDate >= dateStart && entity.VBRealizationDate <= dateEnd);

            if (vbId > 0)
                query = query.Where(entity => entity.VBId == vbId);

            if (vbRealizationId > 0)
                query = query.Where(entity => entity.VBRealizationId == vbRealizationId);

            if (!string.IsNullOrWhiteSpace(vbRequestName))
                query = query.Where(entity => entity.VBRequestName == vbRequestName);

            if (unitId > 0)
                query = query.Where(entity => entity.UnitId == unitId);

            if (!string.IsNullOrWhiteSpace(status) && status.ToUpper() == "UNIT")
                query = query.Where(entity => entity.Position <= VBRealizationPosition.PurchasingToVerification);
            else if (!string.IsNullOrWhiteSpace(status) && status.ToUpper() == "VERIFIKASI")
                query = query.Where(entity => entity.Position >= VBRealizationPosition.Verification && entity.Position <= VBRealizationPosition.VerifiedToCashier);
            else if (!string.IsNullOrWhiteSpace(status) && status.ToUpper() == "KASIR")
                query = query.Where(entity => entity.Position == VBRealizationPosition.Cashier);
            else if (!string.IsNullOrWhiteSpace(status) && status.ToUpper() == "RETUR")
                query = query.Where(entity => entity.Position == VBRealizationPosition.NotVerified);

            if (divisionId > 0)
                query = query.Where(entity => entity.DivisionId == divisionId);

            query = query.OrderByDescending(e => e.LastModifiedDate);

            var vbRealizationIds = query.Select(entity => entity.VBRealizationId).ToList();
            var unitCosts = _dbContext.VBRealizationDocumentUnitCostsItems.Where(entity => vbRealizationIds.Contains(entity.VBRealizationDocumentId) && entity.UseIncomeTax && entity.IncomeTaxBy.ToUpper() == "SUPPLIER").Select(entity => new { entity.VBRealizationDocumentId, entity.IncomeTaxRate, entity.Amount }).ToList();
            var expenditureItems = _dbContext.VBRealizationDocumentExpenditureItems.Where(entity => vbRealizationIds.Contains(entity.VBRealizationDocumentId) && entity.UseIncomeTax && entity.IncomeTaxBy.ToUpper() == "SUPPLIER").Select(entity => new { entity.VBRealizationDocumentId, entity.IncomeTaxRate }).ToList();

            var result = query.Skip((page - 1) * size).Take(size).ToList();
            result = result.Select(element =>
            {
                //var unitIncomeTax = unitCosts.Where(unit => unit.VBRealizationDocumentId == element.VBRealizationId).Sum(s => (decimal)s.IncomeTaxRate / 100 * element.VBAmount);
                //var itemIncomeTax = expenditureItems.Where(unit => unit.VBRealizationDocumentId == element.VBRealizationId).Sum(s => (decimal)s.IncomeTaxRate / 100 * element.VBAmount);
                //element.VBRealizationAmount = element.VBRealizationAmount - unitIncomeTax - itemIncomeTax;
                return element;
            }).ToList();
            var total = await query.CountAsync();
            return new VBRealizationDocumentExpeditionReportDto(result, total, size, page);
        }

        public Task<int> InitializeExpedition(int vbRealizationId)
        {
            var realizationVB = _dbContext.RealizationVbs.FirstOrDefault(entity => entity.Id == vbRealizationId);

            var model = new VBRealizationDocumentExpeditionModel(
                //realizationVB.Id,
                //realizationVB.VBId,
                //realizationVB.VBNo,
                //realizationVB.VBNoRealize,
                //realizationVB.Date,
                //realizationVB.RequestVbName,
                //realizationVB.UnitId,
                //realizationVB.UnitName,
                //realizationVB.DivisionId,
                //realizationVB.DivisionName,
                //realizationVB.Amount_VB,
                //realizationVB.Amount,
                //realizationVB.CurrencyCode,
                //(double)realizationVB.CurrencyRate,
                //realizationVB.VBRealizeCategory
                );

            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);

            _dbContext.Add(model);
            UpdateVBRealizationPosition(vbRealizationId, VBRealizationPosition.Purchasing, "");

            return _dbContext.SaveChangesAsync();
        }

        public ReadResponse<VBRealizationDocumentExpeditionModel> Read(int page, int size, string order, string keyword, VBRealizationPosition position, int vbId, int vbRealizationId, DateTimeOffset? realizationDate, string vbRealizationRequestPerson, int unitId)
        {
            var query = _dbContext.VBRealizationDocumentExpeditions.AsQueryable();

            //var vbRequestNotCompletedIds = _dbContext.VBRequestDocuments.Where(entity => !entity.IsCompleted).Select(entity => entity.Id).ToList();
            //var vbRealizationNotCompletedIds = _dbContext.VBRealizationDocuments.Where(entity => vbRequestNotCompletedIds.Contains(entity.VBRequestDocumentId) || !entity.IsCompleted).Select(entity => entity.Id).ToList();

            //query = query.Where(entity => vbRealizationNotCompletedIds.Contains(entity.VBRealizationId));

            if (position > 0)
                query = query.Where(entity => entity.Position == position);

            if (vbId > 0)
                query = query.Where(entity => entity.VBId == vbId);

            if (vbRealizationId > 0)
                query = query.Where(entity => entity.VBRealizationId == vbRealizationId);

            if (realizationDate.HasValue)
            {
                var date = realizationDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset * -1);
                query = query.Where(entity => entity.VBRealizationDate.Date == date.Date);
            }

            if (!string.IsNullOrWhiteSpace(vbRealizationRequestPerson))
                query = query.Where(entity => entity.VBRequestName == vbRealizationRequestPerson);

            if (unitId > 0)
                query = query.Where(entity => entity.UnitId == unitId);

            //query = query
            //    .Select(entity => new VBRealizationDocumentExpeditionIndexDto
            //    {
            //        Id = entity.Id,
            //        LastModifiedUtc = entity.LastModifiedUtc,
            //        VBNo = entity.VBNo,
            //        VBRealizationNo = entity.VBRealizationNo,
            //        VBRealizationDate = entity.VBRealizationDate,
            //        VBName = entity.VBRequestName,
            //        UnitId = entity.UnitId,
            //        UnitName = entity.UnitName,
            //        DivisionId = entity.DivisionId,
            //        DivisionName = entity.DivisionName,
            //        Currency

            //    });

            List<string> searchAttributes = new List<string>()
            {
                "VBRealizationNo", "VBRequestName", "UnitName"
            };

            query = QueryHelper<VBRealizationDocumentExpeditionModel>.Search(query, searchAttributes, keyword);

            //var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            //Query = QueryHelper<JournalTransactionModel>.Filter(Query, FilterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VBRealizationDocumentExpeditionModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VBRealizationDocumentExpeditionModel>(query, page - 1, size);
            var data = pageable.Data.ToList();

            var totalData = pageable.TotalCount;

            return new ReadResponse<VBRealizationDocumentExpeditionModel>(data, totalData, orderDictionary, new List<string>());
        }

        public Task<int> Reject(int vbRealizationId, string reason)
        {
            var vbRealizationExpedition = _dbContext.VBRealizationDocumentExpeditions.OrderByDescending(x => x.Id).FirstOrDefault(entity => entity.VBRealizationId == vbRealizationId && (entity.Position == VBRealizationPosition.Verification || entity.Position == VBRealizationPosition.VerifiedToCashier));

            vbRealizationExpedition.VerificationRejected(_identityService.Username, reason);
            EntityExtension.FlagForUpdate(vbRealizationExpedition, _identityService.Username, UserAgent);

            _dbContext.VBRealizationDocumentExpeditions.Update(vbRealizationExpedition);
            UpdateVBRealizationPosition(vbRealizationId, VBRealizationPosition.NotVerified, reason);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> CashierDelete(int vbRealizationId)
        {
            var vbRealizationExpedition = _dbContext.VBRealizationDocumentExpeditions.OrderByDescending(x => x.Id).FirstOrDefault(e => e.VBRealizationId == vbRealizationId && e.Position == VBRealizationPosition.Cashier);

            vbRealizationExpedition.CashierDelete();
            EntityExtension.FlagForUpdate(vbRealizationExpedition, _identityService.Username, UserAgent);

            _dbContext.VBRealizationDocumentExpeditions.Update(vbRealizationExpedition);
            UpdateVBRealizationPosition(vbRealizationId, VBRealizationPosition.VerifiedToCashier, null);

            var vbRealizationDocuments = _dbContext.VBRealizationDocuments.FirstOrDefault(x => x.Id == vbRealizationId);

            EntityExtension.FlagForUpdate(vbRealizationDocuments, _identityService.Username, UserAgent);
            vbRealizationDocuments.UpdatePosition(VBRealizationPosition.VerifiedToCashier, _identityService.Username, UserAgent);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> SubmitToVerification(List<int> vbRealizationIds)
        {
            var vbRealizationDocuments = _dbContext.VBRealizationDocuments.Where(entity => vbRealizationIds.Contains(entity.Id) && (entity.Position == VBRealizationPosition.Purchasing || entity.Position == VBRealizationPosition.NotVerified)).ToList();

            var models = vbRealizationDocuments.Select(element =>
            {
                var result = new VBRealizationDocumentExpeditionModel(
                   element.Id,
                   element.VBRequestDocumentId,
                   element.VBRequestDocumentNo,
                   element.DocumentNo,
                   element.Date,
                   !string.IsNullOrWhiteSpace(element.VBRequestDocumentCreatedBy) ? element.VBRequestDocumentCreatedBy : element.CreatedBy,
                   element.SuppliantUnitId,
                   element.SuppliantUnitName,
                   element.SuppliantDivisionId,
                   element.SuppliantDivisionName,
                   element.VBRequestDocumentAmount,
                   element.Amount,
                   element.CurrencyCode,
                   element.CurrencyRate,
                   element.VBRequestDocumentPurpose,
                   element.Type);
                result.SubmitToVerification(_identityService.Username);
                EntityExtension.FlagForCreate(result, _identityService.Username, UserAgent);

                return result;
            }).ToList();

            vbRealizationDocuments = vbRealizationDocuments.Select(element =>
            {
                element.UpdatePosition(VBRealizationPosition.PurchasingToVerification, _identityService.Username, UserAgent);

                return element;
            }).ToList();

            _dbContext.VBRealizationDocuments.UpdateRange(vbRealizationDocuments);
            _dbContext.VBRealizationDocumentExpeditions.AddRange(models);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> VerifiedToCashier(List<int> vbRealizationIds)
        {
            var models = _dbContext.VBRealizationDocumentExpeditions.Where(entity => vbRealizationIds.Contains(entity.VBRealizationId) || entity.Position == VBRealizationPosition.Verification).ToList();

            models.ForEach(model =>
            {
                model.SendToCashier(_identityService.Username);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            });

            _dbContext.VBRealizationDocumentExpeditions.UpdateRange(models);
            UpdateVBRealizationPosition(vbRealizationIds, VBRealizationPosition.VerifiedToCashier, null);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> VerificationDocumentReceipt(List<int> vbRealizationIds)
        {
            var models = _dbContext.VBRealizationDocumentExpeditions.Where(entity => vbRealizationIds.Contains(entity.VBRealizationId) && entity.Position == VBRealizationPosition.PurchasingToVerification).ToList();

            models.ForEach(model =>
            {
                model.VerificationReceipt(_identityService.Username);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            });

            _dbContext.VBRealizationDocumentExpeditions.UpdateRange(models);
            UpdateVBRealizationPosition(vbRealizationIds, VBRealizationPosition.Verification, null);

            return _dbContext.SaveChangesAsync();
        }

        public ReadResponse<VBRealizationDocumentModel> ReadRealizationToVerification(int vbId, int vbRealizationId, DateTimeOffset? realizationDate, string vbRealizationRequestPerson, int unitId)
        {
            var query = _dbContext.Set<VBRealizationDocumentModel>().AsQueryable();
            query = query.Where(entity => entity.Position == VBRealizationPosition.Purchasing || entity.Position == VBRealizationPosition.NotVerified);

            if (vbId > 0)
                query = query.Where(entity => entity.VBRequestDocumentId == vbId);

            if (vbRealizationId > 0)
                query = query.Where(entity => entity.Id == vbRealizationId);

            if (realizationDate.HasValue)
            {
                var date = realizationDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset * -1);
                query = query.Where(entity => entity.Date.Date == date.Date);
            }

            if (!string.IsNullOrWhiteSpace(vbRealizationRequestPerson))
                query = query.Where(entity => entity.VBRequestDocumentCreatedBy == vbRealizationRequestPerson);

            if (unitId > 0)
                query = query.Where(entity => entity.SuppliantUnitId == unitId);

            var result = query.ToList();
            return new ReadResponse<VBRealizationDocumentModel>(result, result.Count, new Dictionary<string, string>(), new List<string>());
        }

        public Task<int> VerifiedToCashier(int vbRealizationId)
        {
            var vbRealizationExpedition = _dbContext.VBRealizationDocumentExpeditions.OrderByDescending(x => x.Id).FirstOrDefault(entity => entity.VBRealizationId == vbRealizationId && (entity.Position == VBRealizationPosition.Verification || entity.Position == VBRealizationPosition.NotVerified));

            vbRealizationExpedition.SendToCashier(_identityService.Username);
            EntityExtension.FlagForUpdate(vbRealizationExpedition, _identityService.Username, UserAgent);

            _dbContext.VBRealizationDocumentExpeditions.Update(vbRealizationExpedition);

            UpdateVBRealizationPosition(vbRealizationId, VBRealizationPosition.VerifiedToCashier, null);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> UpdateExpeditionByRealizationId(int vbRealizationId)
        {
            var realizationVB = _dbContext.RealizationVbs.FirstOrDefault(entity => entity.Id == vbRealizationId);

            var model = _dbContext.VBRealizationDocumentExpeditions.FirstOrDefault(entity => entity.VBRealizationId == vbRealizationId);

            model.UpdateVBRealizationInfo(realizationVB);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.VBRealizationDocumentExpeditions.Update(model);
            return _dbContext.SaveChangesAsync();
        }

        public ReadResponse<VBRealizationDocumentExpeditionModel> ReadVerification(int page, int size, string order, string keyword, VBRealizationPosition position, int vbId, int vbRealizationId, DateTimeOffset? realizationDate, string vbRealizationRequestPerson, int unitId)
        {
            var query = _dbContext.Set<VBRealizationDocumentExpeditionModel>().AsQueryable();

            var idQuery = query;
            var selectData = idQuery.GroupBy(entity => entity.VBRealizationId)
                .Select(e => e.OrderByDescending(x => x.Id)
                .FirstOrDefault()).ToList();
            var ids = selectData.Select(element => element.Id).ToList();

            query = query.Where(entity => ids.Contains(entity.Id) && (entity.Position == VBRealizationPosition.VerifiedToCashier || entity.Position == VBRealizationPosition.NotVerified));

            if (vbId > 0)
                query = query.Where(entity => entity.VBId == vbId);

            if (vbRealizationId > 0)
                query = query.Where(entity => entity.VBRealizationId == vbRealizationId);

            if (realizationDate.HasValue)
            {
                var date = realizationDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset * -1);
                query = query.Where(entity => entity.VBRealizationDate.Date == date.Date);
            }

            if (!string.IsNullOrWhiteSpace(vbRealizationRequestPerson))
                query = query.Where(entity => entity.VBRequestName == vbRealizationRequestPerson);

            if (unitId > 0)
                query = query.Where(entity => entity.UnitId == unitId);

            //query = query
            //    .Select(entity => new VBRealizationDocumentExpeditionIndexDto
            //    {
            //        Id = entity.Id,
            //        LastModifiedUtc = entity.LastModifiedUtc,
            //        VBNo = entity.VBNo,
            //        VBRealizationNo = entity.VBRealizationNo,
            //        VBRealizationDate = entity.VBRealizationDate,
            //        VBName = entity.VBRequestName,
            //        UnitId = entity.UnitId,
            //        UnitName = entity.UnitName,
            //        DivisionId = entity.DivisionId,
            //        DivisionName = entity.DivisionName,
            //        Currency

            //    });

            List<string> searchAttributes = new List<string>()
            {
                "VBRealizationNo", "VBRequestName", "UnitName"
            };

            query = QueryHelper<VBRealizationDocumentExpeditionModel>.Search(query, searchAttributes, keyword);

            //var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            //Query = QueryHelper<JournalTransactionModel>.Filter(Query, FilterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VBRealizationDocumentExpeditionModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VBRealizationDocumentExpeditionModel>(query, page - 1, size);
            var data = pageable.Data.ToList();

            var totalData = pageable.TotalCount;

            return new ReadResponse<VBRealizationDocumentExpeditionModel>(data, totalData, orderDictionary, new List<string>());
        }

        public virtual void UpdateAsync(long id, VBRequestDocumentModel model)
        {
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _RequestDbSet.Update(model);
        }

        public Task<VBRequestDocumentModel> ReadByIdAsync(long id)
        {
            return _dbContext.VBRequestDocuments.Where(entity => entity.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> ClearanceVBPost(List<ClearancePostId> listId)
        {
            var vbRequestIds = listId.Select(element => element.VBRequestId).ToList();
            var vbRealizationIds = listId.Select(element => element.VBRealizationId).ToList();

            var postedVB = new List<int>();
            foreach (var id in vbRequestIds)
            {
                if (id > 0)
                {
                    var model = await ReadByIdAsync(id);
                    model.SetIsCompleted(true, _identityService.Username, UserAgent);
                    model.SetCompletedDate(DateTimeOffset.UtcNow, _identityService.Username, UserAgent);
                    model.SetCompletedBy(_identityService.Username, _identityService.Username, UserAgent);

                    UpdateAsync(id, model);
                }
            }

            var vbNonPOIdsToBeAccounted = new List<int>();
            foreach (var id in vbRealizationIds)
            {
                if (id > 0)
                {
                    var model = _dbContext.VBRealizationDocuments.FirstOrDefault(entity => entity.Id == id);
                    model.SetIsCompleted(DateTimeOffset.UtcNow, _identityService.Username, UserAgent, null);
                    _dbContext.VBRealizationDocuments.Update(model);

                    if (model.Type == VBType.NonPO)
                    {
                        vbNonPOIdsToBeAccounted.Add(model.Id);
                    }

                    if (model.Type == VBType.WithPO)
                    {
                        var epoIds = _dbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == model.Id).Select(entity => (long)entity.UnitPaymentOrderId).ToList();
                        var upoIds = _dbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == model.Id).Select(entity => new UPOandAmountDto() { UPOId = entity.UnitPaymentOrderId, Amount = (double)entity.Amount }).ToList();
                        if (epoIds.Count > 0)
                        {
                            var autoJournalEPOUri = "vb-request-po-external/auto-journal-epo";

                            var body = new VBAutoJournalFormDto()
                            {
                                Date = DateTimeOffset.UtcNow,
                                DocumentNo = model.DocumentNo,
                                EPOIds = epoIds,
                                UPOIds = upoIds
                            };

                            var httpClient = _serviceProvider.GetService<IHttpClientService>();
                            var response = httpClient.PostAsync($"{APIEndpoint.Purchasing}{autoJournalEPOUri}", new StringContent(JsonConvert.SerializeObject(body).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
                        }
                    }

                    if (model.VBRequestDocumentId > 0 && !postedVB.Contains(model.VBRequestDocumentId))
                    {

                        var vbRequest = _dbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == model.VBRequestDocumentId);
                    }
                }
            }

            var result = await _dbContext.SaveChangesAsync();

            if (vbNonPOIdsToBeAccounted.Count > 0)
            {
                await _autoJournalService.AutoJournalVBNonPOClearence(vbNonPOIdsToBeAccounted);
            }

            return result;
        }

        public async Task<string> GetDocumentNo(string type, string bankCode, string username, DateTime date)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.Purchasing + $"bank-expenditure-notes/bank-document-no-date?type={type}&bankCode={bankCode}&username={username}&date={date}";
            var response = await http.GetAsync(uri);

            var result = new BaseResponse<string>();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<BaseResponse<string>>(responseContent, jsonSerializerSettings);
            }

            return result.data;
        }

        public async Task<int> ClearanceVBPost(ClearanceFormDto form)
        {
            var vbRequestIds = form.ListIds.Select(element => element.VBRequestId).ToList();
            var vbRealizationIds = form.ListIds.Select(element => element.VBRealizationId).ToList();

            var postedVB = new List<int>();
            foreach (var id in vbRequestIds)
            {
                if (id > 0)
                {
                    var model = await ReadByIdAsync(id);
                    model.SetIsCompleted(true, _identityService.Username, UserAgent);
                    model.SetCompletedDate(DateTimeOffset.UtcNow, _identityService.Username, UserAgent);
                    model.SetCompletedBy(_identityService.Username, _identityService.Username, UserAgent);

                    UpdateAsync(id, model);
                }
            }

            var vbNonPOIdsToBeAccounted = new List<int>();
            foreach (var id in vbRealizationIds)
            {
                if (id > 0)
                {
                    var model = _dbContext.VBRealizationDocuments.FirstOrDefault(entity => entity.Id == id);
                    var referenceNo = await GetDocumentNo("K", form.Bank.BankCode, _identityService.Username, model.Date.DateTime);

                    model.SetIsCompleted(DateTimeOffset.UtcNow, _identityService.Username, UserAgent, referenceNo);
                    _dbContext.VBRealizationDocuments.Update(model);

                    if (model.Type == VBType.NonPO)
                    {
                        //vbNonPOIdsToBeAccounted.Add(model.Id);
                        await _autoJournalService.AutoJournalVBNonPOClearence(new List<int>() { model.Id }, form.Bank, referenceNo);
                    }

                    if (model.Type == VBType.WithPO)
                    {
                        var epoIds = _dbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == model.Id).Select(entity => (long)entity.UnitPaymentOrderId).ToList();
                        var upoIds = _dbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == model.Id).Select(entity => new UPOandAmountDto() { UPOId = entity.UnitPaymentOrderId, Amount = (double)entity.Amount }).ToList();
                        if (epoIds.Count > 0)
                        {
                            var autoJournalEPOUri = "vb-request-po-external/auto-journal-epo";

                            var body = new VBAutoJournalFormDto()
                            {
                                Date = DateTimeOffset.UtcNow,
                                DocumentNo = referenceNo,
                                EPOIds = epoIds,
                                UPOIds = upoIds,
                                Bank = form.Bank
                            };

                            var httpClient = _serviceProvider.GetService<IHttpClientService>();
                            var response = await httpClient.PostAsync($"{APIEndpoint.Purchasing}{autoJournalEPOUri}", new StringContent(JsonConvert.SerializeObject(body).ToString(), Encoding.UTF8, General.JsonMediaType));
                        }
                    }

                    if (model.VBRequestDocumentId > 0 && !postedVB.Contains(model.VBRequestDocumentId))
                    {

                        var vbRequest = _dbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == model.VBRequestDocumentId);
                    }

                    await _autoDailyBankTransactionService.AutoCreateFromClearenceVB(new List<int>() { model.Id }, form.Bank, referenceNo);
                }
            }

            var result = await _dbContext.SaveChangesAsync();

            //if (vbNonPOIdsToBeAccounted.Count > 0)
            //{
            //    await _autoJournalService.AutoJournalVBNonPOClearence(vbNonPOIdsToBeAccounted, form.Bank, null);
            //}

            return result;
        }
    }
}
