using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Newtonsoft.Json;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.NetCore.Lib;
using Com.Moonlay.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VbNonPORequest;
using OfficeOpenXml.FormulaParsing.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBVerification
{
    public class VbVerificationService : IVbVerificationService
    {
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IVBRealizationDocumentExpeditionService _expeditionService;
        private const string UserAgent = "finance-service";
        private readonly DbSet<RealizationVbModel> dbSet;
        private readonly DbSet<VbRequestModel> _RequestDbSet;

        public VbVerificationService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _expeditionService = serviceProvider.GetService<IVBRealizationDocumentExpeditionService>();

            dbSet = _dbContext.Set<RealizationVbModel>();
            _RequestDbSet = _dbContext.Set<VbRequestModel>();
        }

        public ReadResponse<VbVerificationList> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.RealizationVbs.AsQueryable();
            var query2 = _RequestDbSet.AsQueryable();

            var searchAttributes = new List<string>()
            {
                "VBNo",
                "UnitLoad",
                "VBNoRealize",
                "RequestVbName",
                "CurrencyCode",
                "UnitName",
                "VBRealizeCategory"
            };

            query = QueryHelper<RealizationVbModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<RealizationVbModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<RealizationVbModel>.Order(query, orderDictionary);

            var pageable = new Pageable<RealizationVbModel>(query, page - 1, size);

            var data = query.Include(s => s.RealizationVbDetail).Join(query2,
               (real) => real.VBNo,
               (rqst) => rqst.VBNo,
               (real, rqst) => new VbVerificationList()
               {
                   Id = real.Id,
                   VBNo = real.VBNo,
                   DateRealization = real.Date,
                   DateEstimate = real.DateEstimate,
                   UnitLoad = real.UnitLoad,
                   Amount_Realization = real.Amount,
                   VBNoRealize = real.VBNoRealize,
                   RequestVbName = real.RequestVbName,
                   DateVB = real.DateVB,
                   Currency = real.CurrencyCode,
                   UnitName = real.UnitName,
                   VBRealizeCategory = real.VBRealizeCategory.Contains("NONPO") ? "Non PO" : "PO",
                   Diff = real.DifferenceReqReal,
                   Status_ReqReal = real.StatusReqReal,

                   Usage = rqst.Usage,
                   Amount_Request = real.Amount_VB,
                   Amount_Vat = real.VatAmount,

                   DetailItems = real.RealizationVbDetail.Select(s => new ModelVbItem
                   {
                       DateSPB = s.DateSPB,
                       NoSPB = s.NoSPB,
                       SupplierName = s.SupplierName,
                       PriceTotalSPB = s.PriceTotalSPB,
                       Date = s.DateNonPO,
                       Remark = s.Remark,
                       Amount = s.AmountNonPO,
                       isGetPPn = s.isGetPPn

                   }).ToList()

               }).ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<VbVerificationList>(data, totalData, orderDictionary, new List<string>());
        }

        public ReadResponse<VbVerificationResultList> ReadVerification(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.RealizationVbs.Where(entity => entity.Position == (int)VBRealizationPosition.VerifiedToCashier || entity.Position == (int)VBRealizationPosition.NotVerified);
            //.Union(_dbContext.RealizationVbs.Where(entity => entity.isNotVeridied == true).AsQueryable()).AsQueryable();
            //var query2 = _dbContext.RealizationVbs.Where(entity => entity.isNotVeridied == true).AsQueryable();

            var searchAttributes = new List<string>()
            {
                "VBNoRealize",
                "RequestVbName",
                "UnitName",
                "VBNo",
                "VbNo",
                "VBRealizeCategory",
                "CurrencyCode"
            };

            query = QueryHelper<RealizationVbModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<RealizationVbModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<RealizationVbModel>.Order(query, orderDictionary);

            var pageable = new Pageable<RealizationVbModel>(query, page - 1, size);

            var data = pageable.Data.Select(entity => new VbVerificationResultList()
            {
                Id = entity.Id,
                DateVerified = entity.VerifiedDate,
                RealizeNo = entity.VBNoRealize,
                DateRealize = entity.Date,
                RequestName = entity.RequestVbName,
                UnitRequest = entity.UnitName,
                SendTo = entity.UnitName,
                VbNo = entity.VBNo,
                VBCategory = entity.VBRealizeCategory.Contains("NONPO") ? "Non PO" : "PO",
                Currency = entity.CurrencyCode,
                isVerified = entity.isVerified,
                Amount = entity.Amount,
                Usage = entity.UsageVBRequest,
                Reason_NotVerified = entity.Reason_NotVerified,
                IsVerified = entity.isVerified,
                isNotVeridied = entity.isNotVeridied

            }).ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<VbVerificationResultList>(data, totalData, orderDictionary, new List<string>());
        }

        public async Task<int> CreateAsync(VbVerificationViewModel viewmodel)
        {
            //using (var transaction = _dbContext.Database.BeginTransaction())
            //{

            var m = dbSet.SingleOrDefault(e => e.Id == viewmodel.numberVB.Id);
            EntityExtension.FlagForUpdate(m, _identityService.Username, UserAgent);
            m.isVerified = viewmodel.isVerified;
            m.isNotVeridied = viewmodel.isNotVeridied;
            m.VerifiedName = _identityService.Username;

            m.VerifiedDate = (DateTimeOffset)viewmodel.VerifyDate;

            if (string.IsNullOrEmpty(viewmodel.Reason))
            {
                m.Reason_NotVerified = "";
            }
            else
            {
                m.Reason_NotVerified = viewmodel.Reason;
            }

            //transaction.Commit();
            //}

            await _dbContext.SaveChangesAsync();

            if (m.isVerified)
                await _expeditionService.VerifiedToCashier(m.Id);
            else
                await _expeditionService.Reject(m.Id, viewmodel.Reason);

            return m.Id;
        }

        public async Task<VbVerificationViewModel> ReadById(int id)
        {

            var model = await _dbContext.RealizationVbs.Include(entity => entity.RealizationVbDetail).Where(entity => entity.Id == id).FirstOrDefaultAsync();

            var result = new VbVerificationViewModel()
            {

                numberVB = new NumberVBData()
                {
                    DetailItems = model.RealizationVbDetail.Select(s => new VbVerificationDetailViewModel()
                    {
                        DateSPB = s.DateSPB,
                        NoSPB = s.NoSPB,
                        SupplierName = s.SupplierName,
                        PriceTotalSPB = s.PriceTotalSPB,
                        Date = s.DateNonPO,
                        Remark = s.Remark,
                        Amount = s.AmountNonPO,
                        isGetPPn = s.isGetPPn

                    }).ToList(),

                    Id = model.Id,
                    Amount_Realization = model.Amount,
                    Amount_Request = model.Amount_VB,
                    Currency = model.CurrencyCode,
                    DateEstimate = model.DateEstimate,
                    DateRealization = model.Date,
                    DateVB = model.DateVB,
                    Diff = model.DifferenceReqReal,
                    RequestVbName = model.RequestVbName,
                    UnitName = model.UnitName,
                    Usage = model.UsageVBRequest,
                    VBNo = model.VBNo,
                    VBNoRealize = model.VBNoRealize,
                    VBRealizeCategory = model.VBRealizeCategory.Contains("NONPO") ? "Non PO" : "PO",
                    Amount_Vat = model.VatAmount,
                    Status_ReqReal = model.StatusReqReal

                },
                Reason = model.Reason_NotVerified,
                Remark = model.Reason_NotVerified,
                VerifyDate = model.VerifiedDate,
                isVerified = model.isVerified,
                isNotVeridied = model.isNotVeridied
            };

            return result;

        }

        public ReadResponse<VbVerificationList> ReadToVerified(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.RealizationVbs.Where(a => a.Position == (int)VBRealizationPosition.Verification).AsQueryable();
            var query2 = _RequestDbSet.AsQueryable();

            var searchAttributes = new List<string>()
            {
                "VBNo",
                "UnitLoad",
                "VBNoRealize",
                "RequestVbName",
                "CurrencyCode",
                "UnitName",
                "VBRealizeCategory"
            };

            query = QueryHelper<RealizationVbModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<RealizationVbModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<RealizationVbModel>.Order(query, orderDictionary);

            var pageable = new Pageable<RealizationVbModel>(query, page - 1, size);

            var data = query.Include(s => s.RealizationVbDetail).Join(query2,
               (real) => real.VBNo,
               (rqst) => rqst.VBNo,
               (real, rqst) => new VbVerificationList()
               {
                   Id = real.Id,
                   VBNo = real.VBNo,
                   DateRealization = real.Date,
                   DateEstimate = real.DateEstimate,
                   UnitLoad = real.UnitLoad,
                   Amount_Realization = real.Amount,
                   VBNoRealize = real.VBNoRealize,
                   RequestVbName = real.RequestVbName,
                   DateVB = real.DateVB,
                   Currency = real.CurrencyCode,
                   UnitName = real.UnitName,
                   VBRealizeCategory = real.VBRealizeCategory.Contains("NONPO") ? "Non PO" : "PO",
                   Diff = real.DifferenceReqReal,
                   Status_ReqReal = real.StatusReqReal,

                   Usage = rqst.Usage,
                   Amount_Request = real.Amount_VB,
                   Amount_Vat = real.VatAmount,

                   DetailItems = real.RealizationVbDetail.Select(s => new ModelVbItem
                   {
                       DateSPB = s.DateSPB,
                       NoSPB = s.NoSPB,
                       SupplierName = s.SupplierName,
                       PriceTotalSPB = s.PriceTotalSPB,
                       Date = s.DateNonPO,
                       Remark = s.Remark,
                       Amount = s.AmountNonPO,
                       isGetPPn = s.isGetPPn

                   }).ToList()

               }).ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<VbVerificationList>(data, totalData, orderDictionary, new List<string>());
        }
    }
}
