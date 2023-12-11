using Com.Danliris.Service.Finance.Accounting.Lib.Models.AccountingBook;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.AccountingBook
{
    public class AccountingBookService : IAccountingBookService
    {
        private const string UserAgent = "finance-service";

        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;

        private readonly IServiceProvider _serviceProvider;

        public AccountingBookService(FinanceDbContext context,IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();


            _serviceProvider = serviceProvider;

        }



        public async Task<int> CreateAsync(AccountingBookModel model)
        {
            try
            {
                var existResultCode = await FindbyCode(model.Code);
                if (existResultCode != null)
                    throw new Exception("Code Already Exist");

                var existResultName = await FindByName(model.AccountingBookType);
                if (existResultName != null)
                    throw new Exception("Name Already Exist");


                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                _dbContext.AccountingBooks.Add(model);

                await _dbContext.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var model = _dbContext.AccountingBooks.FirstOrDefault(entity => entity.Id == id);
            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);
            _dbContext.AccountingBooks.Update(model);

            await _dbContext.SaveChangesAsync();
            return model.Id;
        }

        public ReadResponse<AccountingBookModel> Read(int page = 1, int size = 25, string order = "{}", List<string> select = null, string keyword = "{}", string filter = "{}")
        {
            var query = _dbContext.AccountingBooks.AsQueryable();

            try
            {
                var searchAttributes = new List<string>
                {
               "Code",
               "AccountingBookType"
                };

                query = QueryHelper<AccountingBookModel>.Search(query, searchAttributes, keyword);

                var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
                query = QueryHelper<AccountingBookModel>.Filter(query, filterDictionary);

                var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                query = QueryHelper<AccountingBookModel>.Order(query, orderDictionary);

                var pageable = new Pageable<AccountingBookModel>(query, page - 1, size);

                var data = pageable.Data.Select(entity => new AccountingBookModel
                {
                    Id = entity.Id,
                    AccountingBookType = entity.AccountingBookType,
                    Code = entity.Code,
                    Remarks = entity.Remarks,
                    LastModifiedUtc = entity.LastModifiedUtc
                }).ToList();

                int totalData = pageable.TotalCount;
                return new ReadResponse<AccountingBookModel>(data, totalData, orderDictionary, new List<string>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<AccountingBookModel> ReadByIdAsync(int id)
        {
            return _dbContext.AccountingBooks.Where(entity => entity.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> UpdateAsync(int id, AccountingBookModel model)
        {
            try
            {
                var models = _dbContext.AccountingBooks.Where(entity => entity.Id.Equals(id)).FirstOrDefault();

                var existResultCode =   FindbyCodeUpdate(model.Code, id);
                if (existResultCode > 0)
                    throw new Exception("Code: Code Already Exist");

                var existResultName =  FindbyNameUpdate(model.AccountingBookType, id);
                if (existResultName > 0)
                    throw new Exception("AccountBookType: Name Already Exist");


                models.AccountingBookType = model.AccountingBookType;
                models.Code = model.Code;
                models.Remarks = model.Remarks;

                EntityExtension.FlagForUpdate(models, _identityService.Username, UserAgent);
                _dbContext.AccountingBooks.Update(models);

                return _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private async Task<AccountingBookModel> FindbyCode(string code, int id = 0)
        {
            try
            {
                var result = await _dbContext.AccountingBooks
                    .Where(x => id > 0 ? x.Code.Equals(code) && !x.Id.Equals(id) && x.IsDeleted == false : x.Code.Equals(code)).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int FindbyCodeUpdate(string code, int id = 0)
        {
            try
            {
                var result =  _dbContext.AccountingBooks
                    .Where(x => id > 0 ? x.Code.Contains(code) && !x.Id.Equals(id) && x.IsDeleted == false : x.Code.Contains(code)).Count();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int FindbyNameUpdate(string name, int id = 0)
        {
            try
            {
                var result = _dbContext.AccountingBooks
                    .Where(x => id > 0 ? x.Code.Contains(name) && !x.Id.Equals(id) && x.IsDeleted == false : x.Code.Contains(name)).Count();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<AccountingBookModel> FindByName(string name, int id = 0)
        {
            try
            {
                var result = await _dbContext.AccountingBooks
                    .Where(x => id > 0 ? x.AccountingBookType.Equals(name) && !x.Id.Equals(id) && x.IsDeleted == false : x.AccountingBookType.Equals(name)).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
