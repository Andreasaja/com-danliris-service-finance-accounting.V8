using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionExpedition
{
    public class GarmentDispositionExpeditionService : IGarmentDispositionExpeditionService
    {
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;

        public GarmentDispositionExpeditionService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public ReadResponse<IndexDto> GetSendToVerificationOrAccounting(string keyword, int page, int size, string order)
        {
            var query = _dbContext.GarmentDispositionExpeditions.Where(entity => entity.Position == GarmentPurchasingExpeditionPosition.SendToVerification || entity.Position == GarmentPurchasingExpeditionPosition.SendToAccounting);

            var dispositionPurchases = GetGarmentDispostionPurchase().Result;

            query = from a in query
                    join b in dispositionPurchases on a.DispositionNoteId equals b.Id 
                    select new GarmentDispositionExpeditionModel(a.Id, a.DispositionNoteNo, a.DispositionNoteDate, a.DispositionNoteDueDate, a.DispositionNoteId, a.CurrencyTotalPaid, a.TotalPaid, a.CurrencyId, a.CurrencyCode, a.SupplierName, a.Remark, a.ProformaNo, b.CreatedBy, a.CurrencyRate, a.SupplierId, a.SupplierCode, a.VATAmount, a.CurrencyVATAmount, a.IncomeTaxAmount, a.CurrencyIncomeTaxAmount, a.DPPAmount, a.CurrencyDPPAmount, a.VerifiedDateSend, a.VerifiedDateReceived, a.SendToPurchasingRemark, a.CreatedUtc);

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.DispositionNoteNo.Contains(keyword) || entity.SupplierName.Contains(keyword) || entity.CurrencyCode.Contains(keyword));

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentDispositionExpeditionModel>.Order(query, orderDictionary);

            var count = query.Count();

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(entity => new IndexDto(entity.Id, entity.DispositionNoteNo, entity.DispositionNoteDate, entity.DispositionNoteDueDate, entity.DispositionNoteId, entity.CurrencyTotalPaid, entity.TotalPaid, entity.CurrencyId, entity.CurrencyCode, entity.SupplierName, entity.Remark, entity.ProformaNo,entity.CreatedBy,entity.CurrencyRate,entity.SupplierId,entity.SupplierCode,entity.VATAmount, entity.CurrencyVATAmount,entity.IncomeTaxAmount,entity.CurrencyIncomeTaxAmount,entity.DPPAmount,entity.CurrencyDPPAmount,entity.VerifiedDateSend,entity.VerifiedDateReceived,entity.SendToPurchasingRemark, entity.CreatedUtc))
                .ToList();

            return new ReadResponse<IndexDto>(data, count, orderDictionary, new List<string>());
        }

        public async Task<int> SendToAccounting(SendToVerificationAccountingFormDto form)
        {
            var models = new List<GarmentDispositionExpeditionModel>();
            foreach (var item in form.Items)
            {
                var model = new GarmentDispositionExpeditionModel(item.DispositionNote.Id, item.DispositionNote.DocumentNo, item.DispositionNote.Date, item.DispositionNote.DueDate, item.DispositionNote.SupplierId, item.DispositionNote.SupplierName, item.DispositionNote.VATAmount, item.DispositionNote.CurrencyVATAmount, item.DispositionNote.IncomeTaxAmount, item.DispositionNote.CurrencyIncomeTaxAmount, item.DispositionNote.TotalPaid, item.DispositionNote.CurrencyTotalPaid, item.DispositionNote.CurrencyId, item.DispositionNote.CurrencyCode, item.Remark, item.DispositionNote.DPPAmount, item.DispositionNote.CurrencyDPPAmount, item.DispositionNote.SupplierCode, item.DispositionNote.CurrencyRate, item.DispositionNote.ProformaNo, item.DispositionNote.Category);
                model.SendToAccounting(_identityService.Username);

                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                models.Add(model);
            }
            _dbContext.GarmentDispositionExpeditions.UpdateRange(models);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateDispositionNotePositionData = new
            {
                Ids = models.Select(element => element.DispositionNoteId).ToList(),
                Position = GarmentPurchasingExpeditionPosition.SendToAccounting
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/disposition-notes/position", new StringContent(JsonConvert.SerializeObject(updateDispositionNotePositionData), Encoding.UTF8, General.JsonMediaType));
            return _dbContext.SaveChanges();
        }

        public async Task<int> SendToPurchasing(int id)
        {
            var model = _dbContext.GarmentDispositionExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.SendToPurchasing(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDispositionExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateDispositionNotePositionData = new
            {
                Ids = new List<int>() { model.DispositionNoteId },
                Position = GarmentPurchasingExpeditionPosition.Purchasing
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/disposition-notes/position", new StringContent(JsonConvert.SerializeObject(updateDispositionNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        private async Task<int> UpdateDispositionNotePosition(List<int> internalNoteIds, GarmentPurchasingExpeditionPosition position)
        {
            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateDispositionNotePositionData = new
            {
                Ids = internalNoteIds,
                Position = position
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/disposition-notes/position", new StringContent(JsonConvert.SerializeObject(updateDispositionNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> SendToVerification(SendToVerificationAccountingFormDto form)
        {
            var models = new List<GarmentDispositionExpeditionModel>();
            foreach (var item in form.Items)
            {
                var query = _dbContext.GarmentDispositionExpeditions.Where(entity => entity.Position == GarmentPurchasingExpeditionPosition.SendToVerification);
                var list = query.Where(entity => entity.DispositionNoteNo.Contains(item.DispositionNote.DocumentNo)).ToList();

                var model = new GarmentDispositionExpeditionModel(item.DispositionNote.Id, item.DispositionNote.DocumentNo, item.DispositionNote.Date, item.DispositionNote.DueDate, item.DispositionNote.SupplierId, item.DispositionNote.SupplierName, item.DispositionNote.VATAmount, item.DispositionNote.CurrencyVATAmount, item.DispositionNote.IncomeTaxAmount, item.DispositionNote.CurrencyIncomeTaxAmount, item.DispositionNote.TotalPaid, item.DispositionNote.CurrencyTotalPaid, item.DispositionNote.CurrencyId, item.DispositionNote.CurrencyCode, item.Remark, item.DispositionNote.DPPAmount, item.DispositionNote.CurrencyDPPAmount, item.DispositionNote.SupplierCode, item.DispositionNote.CurrencyRate, item.DispositionNote.ProformaNo, item.DispositionNote.Category);
                model.SendToVerification(_identityService.Username);

                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                models.Add(model);

                foreach (var expedition in list)
                {
                    expedition.SendToPurchasingRejected(_identityService.Username, "");

                    EntityExtension.FlagForUpdate(expedition, _identityService.Username, UserAgent);
                    models.Add(expedition);
                }
            }
            _dbContext.GarmentDispositionExpeditions.UpdateRange(models);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateDispositionNotePositionData = new
            {
                Ids = models.Select(element => element.DispositionNoteId).ToList(),
                Position = GarmentPurchasingExpeditionPosition.SendToVerification
            };

            

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/disposition-notes/position", new StringContent(JsonConvert.SerializeObject(updateDispositionNotePositionData), Encoding.UTF8, General.JsonMediaType));
            return _dbContext.SaveChanges();
        }

        public async Task<int> VerificationAccepted(List<int> ids)
        {
            var models = _dbContext.GarmentDispositionExpeditions.Where(entity => ids.Contains(entity.Id)).ToList();

            models = models.Select(model =>
            {
                model.VerificationAccepted(_identityService.Username);

                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                return model;
            }).ToList();


            _dbContext.GarmentDispositionExpeditions.UpdateRange(models);
            var result = await _dbContext.SaveChangesAsync();

            var dispositionNoteIds = models.Select(model => model.DispositionNoteId).ToList();
            await UpdateDispositionNotePosition(dispositionNoteIds, GarmentPurchasingExpeditionPosition.VerificationAccepted);

            return result;
        }

        public async Task<int> CashierAccepted(List<int> ids)
        {
            var models = _dbContext.GarmentDispositionExpeditions.Where(entity => ids.Contains(entity.Id)).ToList();

            models = models.Select(model =>
            {
                model.CashierAccepted(_identityService.Username);

                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                return model;
            }).ToList();


            _dbContext.GarmentDispositionExpeditions.UpdateRange(models);
            var result = await _dbContext.SaveChangesAsync();

            var internalNoteIds = models.Select(model => model.DispositionNoteId).ToList();
            await UpdateDispositionNotePosition(internalNoteIds, GarmentPurchasingExpeditionPosition.CashierAccepted);

            return result;
        }

        public async Task<int> AccountingAccepted(List<int> ids)
        {
            var models = _dbContext.GarmentDispositionExpeditions.Where(entity => ids.Contains(entity.Id)).ToList();

            models = models.Select(model =>
            {
                model.AccountingAccepted(_identityService.Username);

                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                return model;
            }).ToList();

            _dbContext.GarmentDispositionExpeditions.UpdateRange(models);
            var result = await _dbContext.SaveChangesAsync();

            var internalNoteIds = models.Select(model => model.DispositionNoteId).ToList();
            await UpdateDispositionNotePosition(internalNoteIds, GarmentPurchasingExpeditionPosition.AccountingAccepted);

            return result;
        }

        public async Task<int> PurchasingAccepted(List<int> ids)
        {
            var models = _dbContext.GarmentDispositionExpeditions.Where(entity => ids.Contains(entity.Id)).ToList();

            models = models.Select(model =>
            {
                model.PurchasingAccepted(_identityService.Username);

                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                return model;
            }).ToList();

            _dbContext.GarmentDispositionExpeditions.UpdateRange(models);
            var result = await _dbContext.SaveChangesAsync();

            var internalNoteIds = models.Select(model => model.DispositionNoteId).ToList();
            await UpdateDispositionNotePosition(internalNoteIds, GarmentPurchasingExpeditionPosition.Purchasing);

            return result;
        }

        public async Task<int> VoidVerificationAccepted(int id)
        {
            var model = _dbContext.GarmentDispositionExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.VoidVerification(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDispositionExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateDispositionNotePositionData = new
            {
                Ids = new List<int>() { model.DispositionNoteId },
                Position = GarmentPurchasingExpeditionPosition.SendToVerification
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/disposition-notes/position", new StringContent(JsonConvert.SerializeObject(updateDispositionNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> VoidCashierAccepted(int id)
        {
            var model = _dbContext.GarmentDispositionExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.VoidCashier(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDispositionExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateDispositionNotePositionData = new
            {
                Ids = new List<int>() { model.DispositionNoteId },
                Position = GarmentPurchasingExpeditionPosition.VerificationAccepted
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/disposition-notes/position", new StringContent(JsonConvert.SerializeObject(updateDispositionNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> VoidAccountingAccepted(int id)
        {
            var model = _dbContext.GarmentDispositionExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.VoidAccounting(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDispositionExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateDispositionNotePositionData = new
            {
                Ids = new List<int>() { model.DispositionNoteId },
                model.Position
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/disposition-notes/position", new StringContent(JsonConvert.SerializeObject(updateDispositionNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> SendToAccounting(int id)
        {
            var model = _dbContext.GarmentDispositionExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.SendToAccounting(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDispositionExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateDispositionNotePositionData = new
            {
                Ids = new List<int>() { model.DispositionNoteId },
                Position = GarmentPurchasingExpeditionPosition.SendToAccounting
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/disposition-notes/position", new StringContent(JsonConvert.SerializeObject(updateDispositionNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> SendToCashier(int id)
        {
            var model = _dbContext.GarmentDispositionExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.SendToCashier(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDispositionExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateDispositionNotePositionData = new
            {
                Ids = new List<int>() { model.DispositionNoteId },
                Position = GarmentPurchasingExpeditionPosition.SendToCashier
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/disposition-notes/position", new StringContent(JsonConvert.SerializeObject(updateDispositionNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> SendToPurchasingRejected(int id, string remark)
        {
            var model = _dbContext.GarmentDispositionExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.SendToPurchasingRejected(_identityService.Username, remark);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentDispositionExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateDispositionNotePositionData = new
            {
                Ids = new List<int>() { model.DispositionNoteId },
                Position = GarmentPurchasingExpeditionPosition.SendToPurchasing
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/disposition-notes/position", new StringContent(JsonConvert.SerializeObject(updateDispositionNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public GarmentDispositionExpeditionModel GetById(int id)
        {
            return _dbContext
                .GarmentDispositionExpeditions
                .Where(entity => entity.Id == id)
                .FirstOrDefault();
        }

        public ReadResponse<IndexDto> GetByPosition(string keyword, int page, int size, string order, GarmentPurchasingExpeditionPosition position, int dispositionNoteId, int supplierId, string currencyCode = null, string filter ="{}")
        {
            //var query = _dbContext.GarmentDispositionExpeditions.Where(entity => entity.Position == position);
            var queryNoRetur = from exp in _dbContext.GarmentDispositionExpeditions
                        where exp.Position == position // && exp.SendToPurchasingRemark == null
                        select exp;
            var queryRetur = from exp in _dbContext.GarmentDispositionExpeditions
                        where exp.Position == position && exp.SendToPurchasingRemark != null && exp.CreatedUtc.Year == DateTime.Now.Year
                        select exp;
            var query = queryNoRetur.Union(queryRetur);

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.DispositionNoteNo.Contains(keyword) || entity.SupplierName.Contains(keyword) || entity.CurrencyCode.Contains(keyword));

            if (dispositionNoteId > 0)
                query = query.Where(entity => entity.DispositionNoteId == dispositionNoteId);

            if (supplierId > 0)
                query = query.Where(entity => entity.SupplierId == supplierId);

            if (!string.IsNullOrEmpty(currencyCode))
                query = query.Where(entity => entity.CurrencyCode == currencyCode);

            if (position == GarmentPurchasingExpeditionPosition.Purchasing)
            {
                var notPurchasingDispositionNoteIds = _dbContext.GarmentDispositionExpeditions
                    .GroupBy(entity => new { entity.DispositionNoteId, entity.Position })
                    .Select(groupped => new { groupped.Key.DispositionNoteId, groupped.Key.Position })
                    .Where(entity => entity.Position > GarmentPurchasingExpeditionPosition.Purchasing)
                    .Select(entity => entity.DispositionNoteId)
                    .ToList();

                var firstDispositionNoteIds = _dbContext.GarmentDispositionExpeditions
                    .Where(entity => entity.Position == GarmentPurchasingExpeditionPosition.Purchasing && !string.IsNullOrEmpty(entity.SendToPurchasingRemark))
                    .GroupBy(entity => new { entity.DispositionNoteId, entity.Position })
                    .Select(groupped => new { groupped.OrderByDescending(entity => entity.CreatedUtc).FirstOrDefault().Id })
                    .Select(entity => entity.Id)
                    .ToList();
                query = query.Where(entity => !notPurchasingDispositionNoteIds.Contains(entity.DispositionNoteId));
                query = query.Where(entity => firstDispositionNoteIds.Contains(entity.Id));
            }

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentDispositionExpeditionModel>.Order(query, orderDictionary);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<GarmentDispositionExpeditionModel>.Filter(query, filterDictionary);

            var count = query.Count();

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(entity => new IndexDto(entity))
                .ToList();

            var dataDispositionIds = data.Select(t => t.DispositionNoteId);

            var lastAmountPayment = _dbContext.GarmentInvoicePurchasingDispositionItems.Where(s => dataDispositionIds.Contains(s.DispositionId)).ToList();

            data.ForEach(entity =>
            {
                entity.TotalPaidPaymentBefore = lastAmountPayment.Where(t => t.DispositionId == entity.DispositionNoteId).Sum(t => t.TotalPaid);
                entity.DiffTotalPaidPayment = entity.TotalPaid - entity.TotalPaidPaymentBefore;
            });

            return new ReadResponse<IndexDto>(data, count, orderDictionary, new List<string>());
        }

        public ReadResponse<IndexDto> GetVerified(string keyword, int page, int size, string order)
        {
            var query = _dbContext.GarmentDispositionExpeditions.Where(entity => entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier || entity.Position == GarmentPurchasingExpeditionPosition.SendToAccounting || entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing);

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.DispositionNoteNo.Contains(keyword) || entity.SupplierName.Contains(keyword) || entity.CurrencyCode.Contains(keyword));

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentDispositionExpeditionModel>.Order(query, orderDictionary);

            var count = query.Count();

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(entity => new IndexDto(entity))
                .ToList();

            return new ReadResponse<IndexDto>(data, count, orderDictionary, new List<string>());
        }

        private async Task<List<DispositionPurchaseIndexDto>> GetGarmentDispostionPurchase()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.Purchasing + $"garment-disposition-purchase/all-garment-disposition";
            var response = await http.GetAsync(uri);

            var result = new BaseResponse<List<DispositionPurchaseIndexDto>>();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<BaseResponse<List<DispositionPurchaseIndexDto>>>(responseContent, jsonSerializerSettings);
            }

            return result.data;
        }
    }
}
