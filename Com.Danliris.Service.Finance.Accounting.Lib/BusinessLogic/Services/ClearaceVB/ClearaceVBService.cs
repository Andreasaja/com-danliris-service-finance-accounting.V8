using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.ClearaceVB;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.ClearaceVB;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.ClearaceVB
{
    public class ClearaceVBService : IClearaceVBService
    {
        private const string _UserAgent = "finance-service";
        private readonly DbSet<VBRequestDocumentModel> _RequestDbSet;
        private readonly DbSet<VBRealizationDocumentModel> _RealizationDbSet;
        private readonly IServiceProvider _serviceProvider;
        private readonly IIdentityService _IdentityService;
        private readonly IAutoJournalService _autoJournalService;
        private readonly IAutoDailyBankTransactionService _autoDailyBankTransactionService;
        private readonly FinanceDbContext _DbContext;

        public ClearaceVBService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _DbContext = dbContext;
            _RequestDbSet = dbContext.Set<VBRequestDocumentModel>();
            _RealizationDbSet = dbContext.Set<VBRealizationDocumentModel>();
            _serviceProvider = serviceProvider;
            _IdentityService = serviceProvider.GetService<IIdentityService>();
            _autoJournalService = serviceProvider.GetService<IAutoJournalService>();
            _autoDailyBankTransactionService = serviceProvider.GetService<IAutoDailyBankTransactionService>();
        }
        public static IQueryable<ClearaceVBViewModel> Filter(IQueryable<ClearaceVBViewModel> query, Dictionary<string, object> filterDictionary)
        {
            if (filterDictionary != null && !filterDictionary.Count.Equals(0))
            {
                foreach (var f in filterDictionary)
                {
                    string key = f.Key;
                    object Value = f.Value;
                    string filterQuery = string.Concat(string.Empty, key, " == @0");

                    query = query.Where(filterQuery, Value);
                }
            }
            return query;
        }

        public static IQueryable<ClearaceVBViewModel> Order(IQueryable<ClearaceVBViewModel> query, Dictionary<string, string> orderDictionary)
        {
            /* Default Order */
            if (orderDictionary.Count.Equals(0))
            {
                orderDictionary.Add("LastModifiedUtc", "desc");

                query = query.OrderByDescending(b => b.LastModifiedUtc);
            }
            /* Custom Order */
            else
            {
                string Key = orderDictionary.Keys.First();
                string OrderType = orderDictionary[Key];

                query = query.OrderBy(string.Concat(Key.Replace(".", ""), " ", OrderType));
            }
            return query;
        }

        public static IQueryable<ClearaceVBViewModel> Search(IQueryable<ClearaceVBViewModel> query, List<string> searchAttributes, string keyword, bool ToLowerCase = false)
        {
            /* Search with Keyword */
            if (keyword != null)
            {
                string SearchQuery = String.Empty;
                foreach (string Attribute in searchAttributes)
                {
                    //if (Attribute.Contains("."))
                    //{
                    //    var Key = Attribute.Split(".");
                    //    SearchQuery = string.Concat(SearchQuery, Key[0], $".Any({Key[1]}.Contains(@0)) OR ");
                    //}
                    //else
                    //{
                    //    SearchQuery = string.Concat(SearchQuery, Attribute, ".Contains(@0) OR ");
                    //}
                    SearchQuery = string.Concat(SearchQuery, Attribute, ".Contains(@0) OR ");
                }

                SearchQuery = SearchQuery.Remove(SearchQuery.Length - 4);

                //if (ToLowerCase)
                //{
                //    SearchQuery = SearchQuery.Replace(".Contains(@0)", ".ToLower().Contains(@0)");
                //    keyword = keyword.ToLower();
                //}

                query = query.Where(SearchQuery, keyword);
            }
            return query;
        }

        public Task<int> CreateAsync(VBRequestDocumentModel model)
        {
            EntityExtension.FlagForCreate(model, _IdentityService.Username, _UserAgent);

            _DbContext.VBRequestDocuments.Add(model);

            return _DbContext.SaveChangesAsync();
        }

        public virtual void UpdateAsync(long id, VBRequestDocumentModel model)
        {
            EntityExtension.FlagForUpdate(model, _IdentityService.Username, _UserAgent);
            _RequestDbSet.Update(model);
        }

        public Task<VBRequestDocumentModel> ReadByIdAsync(long id)
        {
            return _DbContext.VBRequestDocuments.Where(entity => entity.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> ClearanceVBPost(List<ClearencePostId> listId)
        {
            var vbRequestIds = listId.Select(element => element.VBRequestId).ToList();
            var vbRealizationIds = listId.Select(element => element.VBRealizationId).ToList();

            var postedVB = new List<int>();
            foreach (var id in vbRequestIds)
            {
                if (id > 0)
                {
                    var model = await ReadByIdAsync(id);
                    model.SetIsCompleted(true, _IdentityService.Username, _UserAgent);
                    model.SetCompletedDate(DateTimeOffset.UtcNow, _IdentityService.Username, _UserAgent);
                    model.SetCompletedBy(_IdentityService.Username, _IdentityService.Username, _UserAgent);

                    UpdateAsync(id, model);

                    //if (model.Type == VBType.WithPO)
                    //{
                    //    var epoIds = _DbContext.VBRequestDocumentEPODetails.Where(entity => entity.VBRequestDocumentId == id).Select(entity => (long)entity.EPOId).ToList();
                    //    var autoJournalEPOUri = "vb-request-po-external/auto-journal-epo";

                    //    var body = new VBAutoJournalFormDto()
                    //    {
                    //        Date = DateTimeOffset.UtcNow,
                    //        DocumentNo = model.DocumentNo,
                    //        EPOIds = epoIds
                    //    };
                    //    postedVB.Add(model.Id);

                    //    var httpClient = _serviceProvider.GetService<IHttpClientService>();
                    //    var response = httpClient.PostAsync($"{APIEndpoint.Purchasing}{autoJournalEPOUri}", new StringContent(JsonConvert.SerializeObject(body).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
                    //}
                }
            }

            var vbNonPOIdsToBeAccounted = new List<int>();
            foreach (var id in vbRealizationIds)
            {
                if (id > 0)
                {
                    var model = _DbContext.VBRealizationDocuments.FirstOrDefault(entity => entity.Id == id);
                    model.SetIsCompleted(DateTimeOffset.UtcNow, _IdentityService.Username, _UserAgent, null);
                    _DbContext.VBRealizationDocuments.Update(model);

                    if (model.Type == VBType.NonPO)
                    {
                        vbNonPOIdsToBeAccounted.Add(model.Id);
                    }

                    if (model.Type == VBType.WithPO)
                    {
                        var epoIds = _DbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == model.Id).Select(entity => (long)entity.UnitPaymentOrderId).ToList();
                        var upoIds = _DbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == model.Id).Select(entity => new UPOAndAmountDto() { UPOId = entity.UnitPaymentOrderId, Amount = (double)entity.Amount }).ToList();
                        if (epoIds.Count > 0)
                        {
                            var autoJournalEPOUri = "vb-request-po-external/auto-journal-epo";

                            var body = new VBAutoJournalFormDtoOld()
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

                        var vbRequest = _DbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == model.VBRequestDocumentId);

                        //if (vbRequest.Type == VBType.WithPO)
                        //{
                        //    var epoIds = _DbContext.VBRequestDocumentEPODetails.Where(entity => entity.VBRequestDocumentId == vbRequest.Id).Select(entity => (long)entity.EPOId).ToList();
                        //    if (epoIds.Count > 0)
                        //    {
                        //        var autoJournalEPOUri = "vb-request-po-external/auto-journal-epo";

                        //        var body = new VBAutoJournalFormDto()
                        //        {
                        //            Date = DateTimeOffset.UtcNow,
                        //            DocumentNo = model.DocumentNo,
                        //            EPOIds = epoIds
                        //        };

                        //        var httpClient = _serviceProvider.GetService<IHttpClientService>();
                        //        var response = httpClient.PostAsync($"{APIEndpoint.Purchasing}{autoJournalEPOUri}", new StringContent(JsonConvert.SerializeObject(body).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
                        //    }
                        //}



                    }
                }
            }

            var result = await _DbContext.SaveChangesAsync();

            if (vbNonPOIdsToBeAccounted.Count > 0)
            {
                await _autoJournalService.AutoJournalVBNonPOClearence(vbNonPOIdsToBeAccounted);
            }

            return result;
        }

        public async Task<int> ClearanceVBUnpost(long id)
        {
            var model = await ReadByIdAsync(id);
            model.SetIsCompleted(false, _IdentityService.Username, _UserAgent);
            UpdateAsync(id, model);
            return await _DbContext.SaveChangesAsync();
        }

        public ReadResponse<ClearaceVBViewModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _RequestDbSet.AsQueryable();
            var realizationQuery = _RealizationDbSet.AsQueryable().Where(s => s.Position == VBRealizationPosition.Cashier);

            List<string> SearchAttributes = new List<string>()
            {
                "RqstNo","Appliciant","RealNo","Status","DiffStatus"
            };

            var vbRequestQuery = _RequestDbSet.AsQueryable();
            var vbRealizationQuery = _RealizationDbSet.AsQueryable().Where(s => s.Position == VBRealizationPosition.Cashier);

            var newQuery = from realization in vbRealizationQuery
                           join request in vbRequestQuery on realization.VBRequestDocumentId equals request.Id into vbRequestRealizations

                           from vbRequestRealization in vbRequestRealizations.DefaultIfEmpty()
                           select new ClearaceVBViewModel()
                           {
                               Id = realization.VBRequestDocumentId,
                               VBRealizationDocumentId = realization.Id,
                               RqstNo = realization.VBRequestDocumentNo,
                               VBCategory = realization.Type,
                               RqstDate = realization.VBRequestDocumentDate,
                               //RqstDate = rqst.Date.AddHours(7).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                               Unit = new Unit()
                               {
                                   Id = realization.SuppliantUnitId,
                                   Name = realization.SuppliantUnitName,
                               },
                               Appliciant = realization.VBRequestDocumentCreatedBy,
                               RealNo = realization.DocumentNo,
                               RealDate = realization.Date,
                               //RealDate = rqst.Realization_Status == true ? real.Date.AddHours(7).ToString("dd MMMM yyyy", new CultureInfo("id-ID")) : "",
                               VerDate = realization.VerificationDate,
                               //VerDate = real.isVerified == true ? real.VerifiedDate.AddHours(7).ToString("dd MMMM yyyy", new CultureInfo("id-ID")) : "",
                               //DiffStatus = real.StatusReqReal,
                               DiffAmount = realization.VBRequestDocumentAmount - realization.Amount,
                               ClearanceDate = vbRequestRealization != null ? vbRequestRealization.CompletedDate : realization.CompletedDate,
                               DiffStatus = realization.VBRequestDocumentAmount - realization.Amount < 0 ? "Kurang" : realization.VBRequestDocumentAmount - realization.Amount > 0 ? "Sisa" : "Sesuai",
                               //ClearanceDate = rqst.Complete_Status == true ? rqst.CompleteDate.ToString() : "",
                               IsPosted = vbRequestRealization != null ? vbRequestRealization.IsCompleted : realization.IsCompleted,
                               Status = vbRequestRealization != null ? vbRequestRealization.IsCompleted ? "Completed" : "Uncompleted" : "",
                               LastModifiedUtc = realization.LastModifiedUtc,
                               CurrencyCode = realization.CurrencyCode
                           };

            //var data = query
            //   .Join(realizationQuery,
            //   (rqst) => rqst.Id,
            //   (real) => real.VBRequestDocumentId,
            //   (rqst, real) => new ClearaceVBViewModel()
            //   {
            //       Id = rqst.Id,
            //       VBRealizationDocumentId = real.Id,
            //       RqstNo = rqst.DocumentNo,
            //       VBCategory = rqst.Type,
            //       RqstDate = rqst.Date,
            //       //RqstDate = rqst.Date.AddHours(7).ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
            //       Unit = new Unit()
            //       {
            //           Id = rqst.SuppliantUnitId,
            //           Name = rqst.SuppliantUnitName,
            //       },
            //       Appliciant = rqst.CreatedBy,
            //       RealNo = real.DocumentNo,
            //       RealDate = real.Date,
            //       //RealDate = rqst.Realization_Status == true ? real.Date.AddHours(7).ToString("dd MMMM yyyy", new CultureInfo("id-ID")) : "",
            //       VerDate = real.VerificationDate,
            //       //VerDate = real.isVerified == true ? real.VerifiedDate.AddHours(7).ToString("dd MMMM yyyy", new CultureInfo("id-ID")) : "",
            //       //DiffStatus = real.StatusReqReal,
            //       DiffAmount = rqst.Amount - real.Amount,
            //       ClearanceDate = rqst.CompletedDate,
            //       DiffStatus = rqst.Amount - real.Amount < 0 ? "Kurang" : rqst.Amount - real.Amount > 0 ? "Sisa" : "Sesuai",
            //       //ClearanceDate = rqst.Complete_Status == true ? rqst.CompleteDate.ToString() : "",
            //       IsPosted = rqst.IsCompleted,
            //       Status = rqst.IsCompleted ? "Completed" : "Uncompleted",
            //       LastModifiedUtc = real.LastModifiedUtc,
            //   })
            //   .OrderByDescending(s => s.LastModifiedUtc).AsQueryable();

            newQuery = newQuery.OrderByDescending(entity => entity.LastModifiedUtc);
            newQuery = Search(newQuery, SearchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            newQuery = Filter(newQuery, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            newQuery = Order(newQuery, orderDictionary);

            var pageable = new Pageable<ClearaceVBViewModel>(newQuery, page - 1, size);
            var data = pageable.Data.ToList();

            int totalData = pageable.TotalCount;
            return new ReadResponse<ClearaceVBViewModel>(data, totalData, orderDictionary, new List<string>());
        }

        public async Task<int> ClearanceVBPost(ClearenceFormDto form)
        {
            var vbRequestIds = form.ListIds.Select(element => element.VBRequestId).ToList();
            var vbRealizationIds = form.ListIds.Select(element => element.VBRealizationId).ToList();


            var postedVB = new List<int>();
            foreach (var id in vbRequestIds)
            {
                if (id > 0)
                {
                    var model = await ReadByIdAsync(id);
                    model.SetIsCompleted(true, _IdentityService.Username, _UserAgent);
                    model.SetCompletedDate(DateTimeOffset.UtcNow, _IdentityService.Username, _UserAgent);
                    model.SetCompletedBy(_IdentityService.Username, _IdentityService.Username, _UserAgent);


                    UpdateAsync(id, model);

                    //if (model.Type == VBType.WithPO)
                    //{
                    //    var epoIds = _DbContext.VBRequestDocumentEPODetails.Where(entity => entity.VBRequestDocumentId == id).Select(entity => (long)entity.EPOId).ToList();
                    //    var autoJournalEPOUri = "vb-request-po-external/auto-journal-epo";

                    //    var body = new VBAutoJournalFormDto()
                    //    {
                    //        Date = DateTimeOffset.UtcNow,
                    //        DocumentNo = model.DocumentNo,
                    //        EPOIds = epoIds
                    //    };
                    //    postedVB.Add(model.Id);

                    //    var httpClient = _serviceProvider.GetService<IHttpClientService>();
                    //    var response = httpClient.PostAsync($"{APIEndpoint.Purchasing}{autoJournalEPOUri}", new StringContent(JsonConvert.SerializeObject(body).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
                    //}
                }
            }

            var vbNonPOIdsToBeAccounted = new List<int>();
            foreach (var id in vbRealizationIds)
            {
                if (id > 0)
                {

                    var model = _DbContext.VBRealizationDocuments.FirstOrDefault(entity => entity.Id == id);
                    var referenceNo = await GetDocumentNo("K", form.Bank.BankCode, _IdentityService.Username, DateTime.UtcNow);

                    model.SetIsCompleted(DateTimeOffset.UtcNow, _IdentityService.Username, _UserAgent, referenceNo);
                    _DbContext.VBRealizationDocuments.Update(model);

                    if (model.Type == VBType.NonPO)
                    {
                        //vbNonPOIdsToBeAccounted.Add(model.Id);
                        await _autoJournalService.AutoJournalVBNonPOClearence(new List<int>() {  model.Id }, form.Bank, referenceNo);

                    }

                    if (model.Type == VBType.WithPO)
                    {
                        var epoIds = _DbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == model.Id).Select(entity => (long)entity.UnitPaymentOrderId).ToList();
                        var upoIds = _DbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == model.Id).Select(entity => new UPOAndAmountDto() { UPOId = entity.UnitPaymentOrderId, Amount = (double)entity.Amount }).ToList();
                        if (epoIds.Count > 0)
                        {
                            var autoJournalEPOUri = $"vb-request-po-external/auto-journal-epo";

                            var body = new VBAutoJournalFormDtoOld()
                            {
                                Date = DateTimeOffset.UtcNow,
                                DocumentNo = model.DocumentNo,
                                ReferenceNo = referenceNo,
                                EPOIds = epoIds,
                                UPOIds = upoIds
                            };

                            var httpClient = _serviceProvider.GetService<IHttpClientService>();
                            var response = httpClient.PostAsync($"{APIEndpoint.Purchasing}{autoJournalEPOUri}", new StringContent(JsonConvert.SerializeObject(body).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
                        }
                    }

                    if (model.VBRequestDocumentId > 0 && !postedVB.Contains(model.VBRequestDocumentId))
                    {

                        var vbRequest = _DbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == model.VBRequestDocumentId);

                        //if (vbRequest.Type == VBType.WithPO)
                        //{
                        //    var epoIds = _DbContext.VBRequestDocumentEPODetails.Where(entity => entity.VBRequestDocumentId == vbRequest.Id).Select(entity => (long)entity.EPOId).ToList();
                        //    if (epoIds.Count > 0)
                        //    {
                        //        var autoJournalEPOUri = "vb-request-po-external/auto-journal-epo";

                        //        var body = new VBAutoJournalFormDto()
                        //        {
                        //            Date = DateTimeOffset.UtcNow,
                        //            DocumentNo = model.DocumentNo,
                        //            EPOIds = epoIds
                        //        };

                        //        var httpClient = _serviceProvider.GetService<IHttpClientService>();
                        //        var response = httpClient.PostAsync($"{APIEndpoint.Purchasing}{autoJournalEPOUri}", new StringContent(JsonConvert.SerializeObject(body).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
                        //    }
                        //}



                    }
                    await _autoDailyBankTransactionService.AutoCreateFromClearenceVB(new List<int>() { model.Id }, form.Bank, referenceNo);

                }
            }

            var result = await _DbContext.SaveChangesAsync();

            if (vbNonPOIdsToBeAccounted.Count > 0)
            {
                
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
    }
}
