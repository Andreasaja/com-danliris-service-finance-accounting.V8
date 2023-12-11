using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Services.OthersExpenditureProofDocument
{
    public class OthersExpenditureProofDocumentService : IOthersExpenditureProofDocumentService
    {
        private const string _userAgent = "finance-accounting-service";
        private const int _taskDone = 1;

        private readonly FinanceDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IIdentityService _identityService;
        private readonly IAutoJournalService _autoJournalService;
        private readonly IAutoDailyBankTransactionService _autoDailyBankTransactionService;
        private readonly DbSet<OthersExpenditureProofDocumentModel> _dbSet;
        private readonly DbSet<OthersExpenditureProofDocumentItemModel> _itemDbSet;

        public OthersExpenditureProofDocumentService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<OthersExpenditureProofDocumentModel>();
            _itemDbSet = dbContext.Set<OthersExpenditureProofDocumentItemModel>();

            _serviceProvider = serviceProvider;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _autoJournalService = serviceProvider.GetService<IAutoJournalService>();
            _autoDailyBankTransactionService = serviceProvider.GetService<IAutoDailyBankTransactionService>();
        }

        public async Task<int> CreateAsync(OthersExpenditureProofDocumentCreateUpdateViewModel viewModel)
        {
            var model = viewModel.MapToModel();
            var timeOffset = new TimeSpan(_identityService.TimezoneOffset, 0, 0);
            model.DocumentNo = await GetDocumentNo("K", viewModel.AccountBankCode, _identityService.Username, viewModel.Date.GetValueOrDefault().ToOffset(timeOffset).Date);

            model.CurrencyRate = 1;
            var accountBank = await GetAccountBank(viewModel.AccountBankId.GetValueOrDefault());
            if (accountBank.Currency.Code != "IDR")
            {
                var BICurrency = await GetBICurrency(accountBank.Currency.Code, viewModel.Date.GetValueOrDefault());
                model.CurrencyRate = BICurrency.Rate.GetValueOrDefault();
            }

            EntityExtension.FlagForCreate(model, _identityService.Username, _userAgent);
            _dbSet.Add(model);
            await _dbContext.SaveChangesAsync();

            var itemModels = viewModel.MapItemToModel().Select(item =>
            {
                EntityExtension.FlagForCreate(item, _identityService.Username, _userAgent);
                item.OthersExpenditureProofDocumentId = model.Id;
                return item;
            }).ToList();
            _itemDbSet.UpdateRange(itemModels);
            await _dbContext.SaveChangesAsync();

            //await _autoJournalService.AutoJournalFromOthersExpenditureProof(viewModel, model.DocumentNo);
            //await _autoDailyBankTransactionService.AutoCreateFromOthersExpenditureProofDocument(model, itemModels);

            return _taskDone;
        }

        //private async Task<string> GetDocumentNo(string type, string bankCode, string username)
        //{
        //    var jsonSerializerSettings = new JsonSerializerSettings
        //    {
        //        MissingMemberHandling = MissingMemberHandling.Ignore
        //    };

        //    var http = _serviceProvider.GetService<IHttpClientService>();
        //    var uri = APIEndpoint.Purchasing + $"bank-expenditure-notes/bank-document-no?type={type}&bankCode={bankCode}&username={username}";
        //    var response = await http.GetAsync(uri);

        //    var result = new BaseResponse<string>();

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        result = JsonConvert.DeserializeObject<BaseResponse<string>>(responseContent, jsonSerializerSettings);
        //    }
        //    return result.data;
        //}
        private async Task<string> GetDocumentNo(string type, string bankCode, string username, DateTime date)
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

        //private async Task<GarmentCurrency> GetGarmentCurrency(string codeCurrency)
        //{
        //    string date = DateTimeOffset.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
        //    string queryString = $"code={codeCurrency}&stringDate={date}";

        //    var http = _serviceProvider.GetService<IHttpClientService>();
        //    var response = await http.GetAsync(APIEndpoint.Core + $"master/garment-currencies/single-by-code-date?{queryString}");

        //    var responseString = await response.Content.ReadAsStringAsync();
        //    var jsonSerializationSetting = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };

        //    var result = JsonConvert.DeserializeObject<APIDefaultResponse<GarmentCurrency>>(responseString, jsonSerializationSetting);

        //    return result.data;
        //}

        private async Task<GarmentCurrency> GetBICurrency(string codeCurrency, DateTimeOffset date)
        {
            string stringDate = date.ToString("yyyy/MM/dd HH:mm:ss");
            string queryString = $"code={codeCurrency}&stringDate={stringDate}";

            var http = _serviceProvider.GetService<IHttpClientService>();
            var response = await http.GetAsync(APIEndpoint.Core + $"master/bi-currencies/single-by-code-date?{queryString}");

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonSerializationSetting = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };

            var result = JsonConvert.DeserializeObject<APIDefaultResponse<GarmentCurrency>>(responseString, jsonSerializationSetting);

            return result.data;
        }

        //private string DocumentNoGenerator(OthersExpenditureProofDocumentCreateUpdateViewModel viewModel)
        //{
        //    var latestDocumentNo = _dbSet.IgnoreQueryFilters().Where(document => document.DocumentNo.Contains(viewModel.AccountBankCode)).OrderByDescending(document => document.Id).Select(document => new { document.DocumentNo, document.CreatedUtc }).FirstOrDefault();

        //    var now = DateTimeOffset.Now;
        //    if (latestDocumentNo == null)
        //    {
        //        return $"{now.ToString("yy")}{now.ToString("MM")}{viewModel.AccountBankCode}K0001";
        //    }
        //    else
        //    {
        //        if (latestDocumentNo.CreatedUtc.Month != now.Month)
        //            return $"{now.ToString("yy")}{now.ToString("MM")}{viewModel.AccountBankCode}K0001";
        //        else
        //        {
        //            var numberString = latestDocumentNo.DocumentNo.Split("K").ToList()[1];
        //            var number = int.Parse(numberString) + 1;
        //            return $"{now.ToString("yy")}{now.ToString("MM")}{viewModel.AccountBankCode}K{number.ToString().PadLeft(4, '0')}";
        //        }
        //    }
        //}

        public async Task<int> DeleteAsync(int id)
        {
            var model = _dbSet.FirstOrDefault(document => document.Id == id);
            EntityExtension.FlagForDelete(model, _identityService.Username, _userAgent);
            _dbSet.Update(model);

            var itemModels = _itemDbSet.Where(item => item.OthersExpenditureProofDocumentId == id).ToList();
            itemModels = itemModels.Select(item =>
            {
                EntityExtension.FlagForDelete(item, _identityService.Username, _userAgent);
                return item;
            }).ToList();
            _itemDbSet.UpdateRange(itemModels);

            await _dbContext.SaveChangesAsync();
            //await _autoJournalService.AutoJournalReverseFromOthersExpenditureProof(model.DocumentNo);
            //await _autoDailyBankTransactionService.AutoRevertFromOthersExpenditureProofDocument(model, itemModels);

            return _taskDone;
        }

        public async Task<List<int>> GetAccountBankIds(string keyword)
        {
            var http = _serviceProvider.GetService<IHttpClientService>();

            var response = await http.GetAsync(APIEndpoint.Core + $"master/account-banks?keyword={keyword}&size={int.MaxValue}");

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonSerializationSetting = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };

            var result = JsonConvert.DeserializeObject<APIDefaultResponse<List<AccountBank>>>(responseString, jsonSerializationSetting);

            return result.data.Select(accountBank => accountBank.Id).ToList();
        }

        /// <summary>
        /// used list id accountbank with no duplication for enhance perform
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<List<AccountBank>> GetAccountBanks(List<int> Ids)
        {
            var resultAccountBanks = new List<AccountBank>();
            foreach (var id in Ids)
            {
                var http = _serviceProvider.GetService<IHttpClientService>();

                var response = await http.GetAsync(APIEndpoint.Core + $"master/account-banks/{id}");

                var responseString = await response.Content.ReadAsStringAsync();
                var jsonSerializationSetting = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };

                var result = JsonConvert.DeserializeObject<APIDefaultResponse<AccountBank>>(responseString, jsonSerializationSetting);
                resultAccountBanks.Add(result.data);
            }

            return resultAccountBanks;
        }

        public async Task<OthersExpenditureProofPagedListViewModel> GetPagedListAsync(int page, int size, string order, string keyword, string filter)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var accountBankIds = await GetAccountBankIds(keyword);
                query = query.Where(document => document.Type.Contains(keyword) || document.DocumentNo.Contains(keyword) || accountBankIds.Contains(document.AccountBankId));
            }

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<OthersExpenditureProofDocumentModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<OthersExpenditureProofDocumentModel>.Order(query, orderDictionary);


            var total = await query.CountAsync();
            var data = query.Skip((page - 1) * size).Take(size).Select(document => new OthersExpenditureProofListViewModel()
            {
                AccountBankId = document.AccountBankId,
                DocumentNo = document.DocumentNo,
                Type = document.Type,
                Id = document.Id,
                Date = document.Date,
                IsPosted = document.IsPosted
            }).ToList();

            data = data.Select(element =>
            {
                element.Total = _itemDbSet.Where(item => item.OthersExpenditureProofDocumentId == element.Id).Sum(item => item.Debit);
                return element;
            }).ToList();

            return new OthersExpenditureProofPagedListViewModel()
            {
                Data = data,
                Page = page,
                Size = size,
                Total = total,
                Count = data.Count
            };
        }


        public async Task<OthersExpenditureProofPagedListViewModel> GetLoaderAsync(string keyword, string filter)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(document => document.DocumentNo.Contains(keyword));
            }

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<OthersExpenditureProofDocumentModel>.Filter(query, filterDictionary);

            var total = await query.CountAsync();
            var data = query.Select(document => new OthersExpenditureProofListViewModel()
            {
                DocumentNo = document.DocumentNo,
                Id = document.Id
            }).ToList();

            return new OthersExpenditureProofPagedListViewModel()
            {
                Data = data,
                Page = 1,
                Size = data.Count,
                Total = total,
                Count = data.Count
            };
        }


        public async Task<OthersExpenditureProofDocumentReportListViewModel> GetReportList(DateTimeOffset? startDate, DateTimeOffset? endDate, DateTimeOffset? dateExpenditure, string bankExpenditureNo, string division, int page, int size, string order, string keyword, string filter)
        {
            var query = _dbSet.AsNoTracking().Where(entity => entity.IsPosted);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var accountBankIds = await GetAccountBankIds(keyword);
                query = query.Where(document => document.Type.Contains(keyword) || document.DocumentNo.Contains(keyword) || accountBankIds.Contains(document.AccountBankId));
            }

            if (startDate.HasValue)
                query = query.Where(document => document.Date.AddHours(_identityService.TimezoneOffset) >= startDate.GetValueOrDefault());

            if (endDate.HasValue)
                query = query.Where(document => document.Date.AddHours(_identityService.TimezoneOffset) <= endDate.GetValueOrDefault());

            if (dateExpenditure.HasValue)
                query = query.Where(document => document.Date.AddHours(_identityService.TimezoneOffset) == dateExpenditure.GetValueOrDefault());

            if (!string.IsNullOrEmpty(bankExpenditureNo))
                query = query.Where(document => document.DocumentNo == bankExpenditureNo);

            //Todo: OtherExpenditureDocument filter search for division

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<OthersExpenditureProofDocumentModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<OthersExpenditureProofDocumentModel>.Order(query, orderDictionary);


            var total = await query.CountAsync();
            if (page > 0)
                query = query.Skip((page - 1) * size).Take(size);

            var data = query.Select(document => new OthersExpenditureProofDocumentReportViewModel
            {
                AccountBankId = document.AccountBankId,
                DocumentNo = document.DocumentNo,
                Type = document.Type,
                Id = document.Id,
                Date = document.Date,
                IsPosted = document.IsPosted,
                CekBgNo = document.CekBgNo,
                Remark = document.Remark,
                Total = decimal.Zero,
            }).ToList();

            //var queryIds = query.Select(s => s.Id).ToList();
            //var queryItems = _itemDbSet.Where(item => queryIds.Contains(item.OthersExpenditureProofDocumentId)).ToList();
            var itemAccountBanks = query.Select(s => s.AccountBankId).Distinct().ToList();
            var accountBanks = await GetAccountBanks(itemAccountBanks);

            data = data.Select(element =>
            {
                element.Total = _itemDbSet.Where(item => item.OthersExpenditureProofDocumentId == element.Id).Sum(item => item.Debit);
                return element;
            }).ToList();

            var result = (from dt in data
                          join accBank in accountBanks on dt.AccountBankId equals accBank.Id into dtAccBanks
                          from dtAccBank in dtAccBanks
                          select new OthersExpenditureProofDocumentReportViewModel
                          {
                              BankCOAId = dt.AccountBankId,
                              BankCOANo = dtAccBank.AccountCOA,
                              BankName = dtAccBank.BankName,
                              CekBgNo = dt.CekBgNo,
                              CurrencyCode = dtAccBank.Currency.Code,
                              Date = dt.Date,
                              DocumentNo = dt.DocumentNo,
                              Id = dt.Id,
                              Remark = dt.Remark,
                              Total = dt.Total,
                              IsPosted = dt.IsPosted,
                              Type = dt.Type,
                              AccountName = dtAccBank.AccountName,
                              AccountBankId = dtAccBank.Id,
                              AccountNumber = dtAccBank.AccountNumber

                          }).ToList();

            return new OthersExpenditureProofDocumentReportListViewModel()
            {
                Data = result.OrderBy(element => element.Date).ToList(),
                Page = page,
                Size = size,
                Total = total,
                Count = result.Count()
            };
        }

        public async Task<int> UpdateAsync(int id, OthersExpenditureProofDocumentCreateUpdateViewModel viewModel)
        {
            var model = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            model.Update(viewModel);
            EntityExtension.FlagForUpdate(model, _identityService.Username, _userAgent);
            _dbSet.Update(model);

            var itemFormIds = viewModel.Items.Select(item => item.Id.GetValueOrDefault()).ToList();
            var itemModels = await _itemDbSet.Where(item => itemFormIds.Contains(item.Id)).ToListAsync();
            //await _autoDailyBankTransactionService.AutoRevertFromOthersExpenditureProofDocument(model, itemModels);

            List<int> itemIds = await _itemDbSet.Where(w => w.OthersExpenditureProofDocumentId.Equals(id) && !w.IsDeleted).Select(s => s.Id).ToListAsync();
            var itemModelsToUpdate = viewModel.MapItemToModel();

            foreach (var itemId in itemIds)
            {
                var item = itemModels.FirstOrDefault(f => f.Id.Equals(itemId));
                if (item == null)
                {
                    var itemToDelete = await _itemDbSet.FirstOrDefaultAsync(f => f.Id.Equals(itemId));
                    EntityExtension.FlagForDelete(itemToDelete, _identityService.Username, _userAgent);
                    _itemDbSet.Update(itemToDelete);
                }
                else
                {
                    var itemModelToUpdate = itemModelsToUpdate.FirstOrDefault(f => f.Id == itemId);
                    item.UpdateCOAId(itemModelToUpdate.COAId);
                    item.UpdateDebit(itemModelToUpdate.Debit);
                    item.UpdateRemark(itemModelToUpdate.Remark);
                    EntityExtension.FlagForUpdate(item, _identityService.Username, _userAgent);
                    _itemDbSet.Update(item);
                }
            }

            foreach (var item in itemModelsToUpdate)
            {
                if (item.Id <= 0)
                {
                    item.OthersExpenditureProofDocumentId = model.Id;
                    EntityExtension.FlagForCreate(item, _identityService.Username, _userAgent);
                    _itemDbSet.Add(item);
                }
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<OthersExpenditureProofDocumentViewModel> GetSingleByIdAsync(int id)
        {
            var model = await _dbSet.AsNoTracking().FirstOrDefaultAsync(document => document.Id == id);
            var items = _itemDbSet.AsNoTracking().Where(item => item.OthersExpenditureProofDocumentId == id).ToList();

            return new OthersExpenditureProofDocumentViewModel(model, items);
        }

        public async Task<OthersExpenditureProofDocumentPDFViewModel> GetPDFByIdAsync(int id)
        {
            var model = await _dbSet.AsNoTracking().FirstOrDefaultAsync(document => document.Id == id);
            var items = _itemDbSet.AsNoTracking().Where(item => item.OthersExpenditureProofDocumentId == id).ToList();
            var coaIds = items.Select(element => element.COAId).ToList();
            var coas = _dbContext.ChartsOfAccounts.Where(element => coaIds.Contains(element.Id)).ToList();

            var accountBank = await GetAccountBank(model.AccountBankId);

            return new OthersExpenditureProofDocumentPDFViewModel(model, items, accountBank, coas);
        }

        private async Task<AccountBankViewModel> GetAccountBank(int bankId)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.Core + $"master/account-banks/{bankId}";
            var response = await http.GetAsync(uri);

            var result = new BaseResponse<AccountBankViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<BaseResponse<AccountBankViewModel>>(responseContent, jsonSerializerSettings);
            }
            return result.data;
        }

        public async Task<string> Posting(List<int> ids)
        {
            var models = _dbContext.OthersExpenditureProofDocuments.Where(entity => ids.Contains(entity.Id)).ToList();
            var itemModels = _dbContext.OthersExpenditureProofDocumentItems.Where(entity => ids.Contains(entity.OthersExpenditureProofDocumentId)).ToList();
            var result = "";

            foreach (var model in models)
            {
                if (model.IsPosted)
                {
                    result += "Nomor " + model.DocumentNo + ", ";
                }
                else
                {
                    var items = itemModels.Where(element => element.OthersExpenditureProofDocumentId == model.Id).ToList();

                    await _autoJournalService.AutoJournalFromOthersExpenditureProof(model, items);
                    await _autoDailyBankTransactionService.AutoCreateFromOthersExpenditureProofDocument(model, items);

                    model.IsPosted = true;
                    EntityExtension.FlagForUpdate(model, _identityService.Username, _userAgent);

                    await _dbContext.SaveChangesAsync();
                }
            }

            return result;
        }
    }
}