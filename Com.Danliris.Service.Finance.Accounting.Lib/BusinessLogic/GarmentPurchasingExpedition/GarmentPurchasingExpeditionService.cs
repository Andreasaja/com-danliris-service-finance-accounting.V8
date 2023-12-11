using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingExpedition;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingExpedition
{
    public class GarmentPurchasingExpeditionService : IGarmentPurchasingExpeditionService
    {
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;

        public GarmentPurchasingExpeditionService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public ReadResponse<IndexDto> GetByPosition(string keyword, int page, int size, string order, GarmentPurchasingExpeditionPosition position, int internalNoteId, int supplierId,string currencyCode = null)
        {
            var query = _dbContext.GarmentPurchasingExpeditions.Where(entity => entity.Position == position);

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.InternalNoteNo.Contains(keyword) || entity.SupplierName.Contains(keyword) || entity.CurrencyCode.Contains(keyword));

            if (internalNoteId > 0)
                query = query.Where(entity => entity.InternalNoteId == internalNoteId);

            if (supplierId > 0)
                query = query.Where(entity => entity.SupplierId == supplierId);

            if (!string.IsNullOrEmpty(currencyCode))
                query = query.Where(entity => entity.CurrencyCode == currencyCode);

            //var query = _dbContext.GarmentPurchasingExpeditions.Where(entity => entity.Position == position && entity.InternalNoteNo.Contains(keyword) || entity.SupplierName.Contains(keyword) || entity.CurrencyCode.Contains(keyword)
            //                                                                        && entity.InternalNoteId.Equals(internalNoteId) && entity.SupplierId.Equals(supplierId) && entity.CurrencyCode.Contains(currencyCode));

            if (position == GarmentPurchasingExpeditionPosition.Purchasing)
            {
                var notPurchasingInternalNoteIds = _dbContext.GarmentPurchasingExpeditions
                   .GroupBy(entity => new { entity.InternalNoteId, entity.Position })
                   .Select(groupped => new { groupped.Key.InternalNoteId, groupped.Key.Position })
                   .Where(entity => entity.Position > GarmentPurchasingExpeditionPosition.Purchasing)
                   .Select(entity => entity.InternalNoteId)
                   .ToList();

                var firstInternalNoteIds = _dbContext.GarmentPurchasingExpeditions
                    .Where(entity => entity.Position == GarmentPurchasingExpeditionPosition.Purchasing && !string.IsNullOrEmpty(entity.SendToPurchasingRemark))
                    .GroupBy(entity => new { entity.InternalNoteId, entity.Position })
                    .Select(groupped => new { groupped.OrderByDescending(entity => entity.CreatedUtc).FirstOrDefault().Id })
                    .Select(entity => entity.Id)
                    .ToList();

                query = query.Where(entity => !notPurchasingInternalNoteIds.Contains(entity.InternalNoteId));

                query = query.Where(entity => firstInternalNoteIds.Contains(entity.Id));
            }

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentPurchasingExpeditionModel>.Order(query, orderDictionary);

            var count = query.Count();

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(entity => new IndexDto(entity))
                .ToList();

            return new ReadResponse<IndexDto>(data, count, orderDictionary, new List<string>());
        }

        public ReadResponse<IndexDto> GetByPositionRetur(string keyword, int page, int size, string order, GarmentPurchasingExpeditionPosition position, int internalNoteId, int supplierId, string currencyCode = null)
        {
            var query = _dbContext.GarmentPurchasingExpeditions.Where(entity => entity.Position == position && entity.IsDeleted == false && entity.SendToPurchasingRemark != null );

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.InternalNoteNo.Contains(keyword) || entity.SupplierName.Contains(keyword) || entity.CurrencyCode.Contains(keyword));

            if (internalNoteId > 0)
                query = query.Where(entity => entity.InternalNoteId == internalNoteId);

            if (supplierId > 0)
                query = query.Where(entity => entity.SupplierId == supplierId);

            if (!string.IsNullOrEmpty(currencyCode))
                query = query.Where(entity => entity.CurrencyCode == currencyCode);

            //if (position == GarmentPurchasingExpeditionPosition.Purchasing)
            //{
            //    //var notPurchasingInternalNoteIds = _dbContext.GarmentPurchasingExpeditions
            //    //   .GroupBy(entity => new { entity.InternalNoteId, entity.Position, entity.CreatedUtc })
            //    //   .Select(groupped => new { groupped.Key.InternalNoteId, groupped.Key.Position, groupped.OrderByDescending(entity => entity.CreatedUtc).FirstOrDefault().Id })
            //    //   .Where(entity => entity.Position > GarmentPurchasingExpeditionPosition.Purchasing)
            //    //   .Select(entity => entity.InternalNoteId)
            //    //   .ToList();

            //    query = query.Where(entity => !notPurchasingInternalNoteIds.Contains(entity.InternalNoteId));
            //}

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentPurchasingExpeditionModel>.Order(query, orderDictionary);

            var count = query.Count();

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(entity => new IndexDto(entity))
                .ToList();

            ////var data = query.Select(x => new IndexDto(x)).ToList();
            //var data = query.Select(x => new GarmentPurchasingExpeditionReturnViewModel
            //{
            //    InternalNoteDate = x.InternalNoteDate.ToString("dd MMM yyyy"),
            //    InternalNoteNo = x.InternalNoteNo,
            //    //VerificationAcceptedDate = x.VerificationAcceptedDate,
            //    //SupplierName = x.SupplierName,
            //    //Amount = x.TotalPaid,
            //    //CurrencyCode = x.CurrencyCode,
            //    //SendToPurchasingRemark = x.SendToPurchasingRemark
            //}).ToList();

            return new ReadResponse<IndexDto>(data, count, orderDictionary, new List<string>());
        }

        public ReadResponse<IndexDto> GetSendToVerificationOrAccounting(string keyword, int page, int size, string order)
        {
            var query = _dbContext.GarmentPurchasingExpeditions.Where(entity => entity.Position == GarmentPurchasingExpeditionPosition.SendToVerification || entity.Position == GarmentPurchasingExpeditionPosition.SendToAccounting);

            //gk smpai d bwh ini
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.InternalNoteNo.Contains(keyword) || entity.SupplierName.Contains(keyword) || entity.CurrencyCode.Contains(keyword));

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentPurchasingExpeditionModel>.Order(query, orderDictionary);

            var count = query.Count();

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(entity => new IndexDto(entity.Id, entity.InternalNoteNo, entity.InternalNoteDate, entity.InternalNoteDueDate, entity.SupplierName, entity.TotalPaid, entity.CurrencyCode, entity.Remark, entity.Position))
                .ToList();

            return new ReadResponse<IndexDto>(data, count, orderDictionary, new List<string>());
        }

        public async Task<int> SendToAccounting(SendToVerificationAccountingForm form)
        {
            var models = new List<GarmentPurchasingExpeditionModel>();
            foreach (var item in form.Items)
            {
                var model = new GarmentPurchasingExpeditionModel(item.InternalNote.Id, item.InternalNote.DocumentNo, item.InternalNote.Date, item.InternalNote.DueDate, item.InternalNote.SupplierId, item.InternalNote.SupplierName, item.InternalNote.VAT, item.InternalNote.IncomeTax, item.InternalNote.TotalPaid, item.InternalNote.CurrencyId, item.InternalNote.CurrencyCode, item.Remark, item.InternalNote.AmountDPP, item.InternalNote.CorrectionAmount, item.InternalNote.PaymentMethod, item.InternalNote.PaymentType, item.InternalNote.PaymentDueDays, item.InternalNote.InvoicesNo);
                model.SendToAccounting(_identityService.Username);

                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                models.Add(model);
            }
            _dbContext.GarmentPurchasingExpeditions.UpdateRange(models);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                Ids = models.Select(element => element.InternalNoteId).ToList(),
                Position = GarmentPurchasingExpeditionPosition.SendToAccounting
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));
            return _dbContext.SaveChanges();
        }

        public async Task<int> SendToPurchasing(int id)
        {
            var model = _dbContext.GarmentPurchasingExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.SendToPurchasing(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentPurchasingExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                Ids = new List<int>() { model.InternalNoteId },
                Position = GarmentPurchasingExpeditionPosition.Purchasing
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        private async Task<int> UpdateInternalNotePosition(List<int> internalNoteIds, GarmentPurchasingExpeditionPosition position)
        {
            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                Ids = internalNoteIds,
                Position = position
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> SendToVerification(SendToVerificationAccountingForm form)
        {
            var models = new List<GarmentPurchasingExpeditionModel>();
            foreach (var item in form.Items)
            {
                var model = new GarmentPurchasingExpeditionModel(item.InternalNote.Id, item.InternalNote.DocumentNo, item.InternalNote.Date, item.InternalNote.DueDate, item.InternalNote.SupplierId, item.InternalNote.SupplierName, item.InternalNote.VAT, item.InternalNote.IncomeTax, item.InternalNote.TotalPaid, item.InternalNote.CurrencyId, item.InternalNote.CurrencyCode, item.Remark, item.InternalNote.AmountDPP, item.InternalNote.CorrectionAmount, item.InternalNote.PaymentMethod, item.InternalNote.PaymentType, item.InternalNote.PaymentDueDays, item.InternalNote.InvoicesNo);
                model.SendToVerification(_identityService.Username);

                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                models.Add(model);
            }
            _dbContext.GarmentPurchasingExpeditions.UpdateRange(models);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                Ids = models.Select(element => element.InternalNoteId).ToList(),
                Position = GarmentPurchasingExpeditionPosition.SendToVerification
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));
            return _dbContext.SaveChanges();
        }

        public async Task<int> VerificationAccepted(List<int> ids)
        {
            var models = _dbContext.GarmentPurchasingExpeditions.Where(entity => ids.Contains(entity.Id)).ToList();

            models = models.Select(model =>
            {
                model.VerificationAccepted(_identityService.Username);

                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                return model;
            }).ToList();


            _dbContext.GarmentPurchasingExpeditions.UpdateRange(models);
            var result = await _dbContext.SaveChangesAsync();

            var internalNoteIds = models.Select(model => model.InternalNoteId).ToList();
            await UpdateInternalNotePosition(internalNoteIds, GarmentPurchasingExpeditionPosition.VerificationAccepted);

            return result;
        }

        public async Task<int> CashierAccepted(List<int> ids)
        {
            var models = _dbContext.GarmentPurchasingExpeditions.Where(entity => ids.Contains(entity.Id)).ToList();

            models = models.Select(model =>
            {
                model.CashierAccepted(_identityService.Username);

                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                return model;
            }).ToList();


            _dbContext.GarmentPurchasingExpeditions.UpdateRange(models);
            var result = await _dbContext.SaveChangesAsync();

            var internalNoteIds = models.Select(model => model.InternalNoteId).ToList();
            await UpdateInternalNotePosition(internalNoteIds, GarmentPurchasingExpeditionPosition.CashierAccepted);

            return result;
        }

        public async Task<int> AccountingAccepted(List<int> ids)
        {
            var models = _dbContext.GarmentPurchasingExpeditions.Where(entity => ids.Contains(entity.Id)).ToList();

            models = models.Select(model =>
            {
                model.AccountingAccepted(_identityService.Username);

                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                return model;
            }).ToList();

            _dbContext.GarmentPurchasingExpeditions.UpdateRange(models);
            var result = await _dbContext.SaveChangesAsync();

            var internalNoteIds = models.Select(model => model.InternalNoteId).ToList();
            await UpdateInternalNotePosition(internalNoteIds, GarmentPurchasingExpeditionPosition.AccountingAccepted);

            return result;
        }

        public async Task<int> PurchasingAccepted(List<int> ids)
        {
            var models = _dbContext.GarmentPurchasingExpeditions.Where(entity => ids.Contains(entity.Id)).ToList();

            models = models.Select(model =>
            {
                model.PurchasingAccepted(_identityService.Username);

                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                return model;
            }).ToList();

            _dbContext.GarmentPurchasingExpeditions.UpdateRange(models);
            var result = await _dbContext.SaveChangesAsync();

            var internalNoteIds = models.Select(model => model.InternalNoteId).ToList();
            await UpdateInternalNotePosition(internalNoteIds, GarmentPurchasingExpeditionPosition.Purchasing);

            return result;
        }

        public async Task<int> VoidVerificationAccepted(int id)
        {
            var model = _dbContext.GarmentPurchasingExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.VoidVerification(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentPurchasingExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                Ids = new List<int>() { model.InternalNoteId },
                Position = GarmentPurchasingExpeditionPosition.SendToVerification
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> VoidCashierAccepted(int id)
        {
            var model = _dbContext.GarmentPurchasingExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.VoidCashier(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentPurchasingExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                Ids = new List<int>() { model.InternalNoteId },
                Position = GarmentPurchasingExpeditionPosition.VerificationAccepted
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> VoidAccountingAccepted(int id)
        {
            var model = _dbContext.GarmentPurchasingExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.VoidAccounting(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentPurchasingExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                Ids = new List<int>() { model.InternalNoteId },
                model.Position
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public ReadResponse<IndexDto> GetVerified(string keyword, int page, int size, string order)
        {
            var query = _dbContext.GarmentPurchasingExpeditions.Where(entity => entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier || entity.Position == GarmentPurchasingExpeditionPosition.SendToAccounting || entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing);

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.InternalNoteNo.Contains(keyword) || entity.SupplierName.Contains(keyword) || entity.CurrencyCode.Contains(keyword));

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentPurchasingExpeditionModel>.Order(query, orderDictionary);

            var count = query.Count();

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(entity => new IndexDto(entity))
                .ToList();

            return new ReadResponse<IndexDto>(data, count, orderDictionary, new List<string>());
        }

        public async Task<int> SendToAccounting(int id)
        {
            var model = _dbContext.GarmentPurchasingExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.SendToAccounting(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentPurchasingExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                Ids = new List<int>() { model.InternalNoteId },
                Position = GarmentPurchasingExpeditionPosition.SendToAccounting
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> SendToCashier(int id)
        {
            var model = _dbContext.GarmentPurchasingExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.SendToCashier(_identityService.Username);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentPurchasingExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                Ids = new List<int>() { model.InternalNoteId },
                Position = GarmentPurchasingExpeditionPosition.SendToCashier
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> SendToPurchasingRejected(int id, string remark)
        {
            var model = _dbContext.GarmentPurchasingExpeditions.FirstOrDefault(entity => entity.Id == id);
            model.SendToPurchasingRejected(_identityService.Username, remark);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.GarmentPurchasingExpeditions.Update(model);

            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                Ids = new List<int>() { model.InternalNoteId },
                Position = GarmentPurchasingExpeditionPosition.SendToPurchasing
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));

            return _dbContext.SaveChanges();
        }

        public async Task<int> SendToPurchasingRejected(List<int> ids, string remark)
        {
            var modelsaved = 0;
            var modelList = _dbContext.GarmentPurchasingExpeditions.Where(entity => ids.Contains( entity.Id)).ToList();
            foreach (var model in modelList)
            {
                model.SendToPurchasingRejected(_identityService.Username, remark);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                _dbContext.GarmentPurchasingExpeditions.Update(model);
          
                modelsaved = _dbContext.SaveChanges();
            }
            var httpClient = _serviceProvider.GetService<IHttpClientService>();
            var updateInternalNotePositionData = new
            {
                //Ids = new List<int>() { model.InternalNoteId },
                Ids = modelList.Select(s=> s.InternalNoteId).ToList(),
                Position = GarmentPurchasingExpeditionPosition.SendToPurchasing
            };

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes/position", new StringContent(JsonConvert.SerializeObject(updateInternalNotePositionData), Encoding.UTF8, General.JsonMediaType));
            return modelsaved;
        }

        public IndexDto GetById(int id)
        {
            return _dbContext.GarmentPurchasingExpeditions.Select(entity => new IndexDto(entity)).FirstOrDefault(entity => entity.Id == id);
        }
    }
}
