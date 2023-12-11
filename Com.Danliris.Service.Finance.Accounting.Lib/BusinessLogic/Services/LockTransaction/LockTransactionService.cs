using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.LockTransaction
{
    public class LockTransactionService : ILockTransactionService
    {
        private readonly FinanceDbContext _dbContext;
        private readonly DbSet<LockTransactionModel> _dbSet;
        private readonly IServiceProvider _serviceProvider;
        private readonly IIdentityService _identityService;
        private const string _userAgent = "finance-service";

        public LockTransactionService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<LockTransactionModel>();
            _serviceProvider = serviceProvider;
            _identityService = serviceProvider.GetService<IIdentityService>();
        }

        public async Task<int> CreateAsync(LockTransactionModel model)
        {
            model.IsActiveStatus = true;
            EntityExtension.FlagForCreate(model, _identityService.Username, _userAgent);
            SetOthersToBeInactive(model);
            _dbSet.Add(model);
            return await _dbContext.SaveChangesAsync();
        }

        private void SetOthersToBeInactive(LockTransactionModel model)
        {
            var dataToInactivated = _dbSet.Where(w => w.IsActiveStatus && w.Type.Equals(model.Type)).ToList();
            foreach (var data in dataToInactivated)
                data.IsActiveStatus = false;

            _dbSet.UpdateRange(dataToInactivated);
        }

        public Task<int> DeleteAsync(int id)
        {
            return Task.FromResult(1);
        }

        public Task<LockTransactionModel> GetLastActiveLockTransaction(string type)
        {
            return _dbSet.FirstOrDefaultAsync(f => f.IsActiveStatus && f.Type.Equals(type));
        }

        public ReadResponse<LockTransactionModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<LockTransactionModel> Query = _dbSet;

            Query = Query
                .Select(s => new LockTransactionModel
                {
                    Id = s.Id,
                    CreatedUtc = s.CreatedUtc,
                    CreatedBy = s.CreatedBy,
                    LockDate = s.LockDate,
                    Type = s.Type,
                    //EndLockDate = s.EndLockDate,
                    IsActiveStatus = s.IsActiveStatus,
                    Description = s.Description,
                    LastModifiedUtc = s.LastModifiedUtc
                });

            List<string> searchAttributes = new List<string>()
            {
                "Description", "CreatedBy"
            };

            Query = QueryHelper<LockTransactionModel>.Search(Query, searchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<LockTransactionModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<LockTransactionModel>.Order(Query, OrderDictionary);

            Pageable<LockTransactionModel> pageable = new Pageable<LockTransactionModel>(Query, page - 1, size);
            List<LockTransactionModel> Data = pageable.Data.ToList();

            List<LockTransactionModel> list = new List<LockTransactionModel>();
            list.AddRange(
               Data.Select(s => new LockTransactionModel
               {
                   Id = s.Id,
                   CreatedUtc = s.CreatedUtc,
                   CreatedBy = s.CreatedBy,
                   LockDate = s.LockDate,
                   Type = s.Type,
                   //EndLockDate = s.EndLockDate,
                   IsActiveStatus = s.IsActiveStatus,
                   Description = s.Description,
                   LastModifiedUtc = s.LastModifiedUtc
               }).ToList()
            );

            int TotalData = pageable.TotalCount;

            return new ReadResponse<LockTransactionModel>(list, TotalData, OrderDictionary, new List<string>());
        }

        public Task<LockTransactionModel> ReadByIdAsync(int id)
        {
            return _dbSet.FirstOrDefaultAsync(f => f.Id.Equals(id));
        }

        public Task<int> UpdateAsync(int id, LockTransactionModel model)
        {
            return Task.FromResult(1);
        }
    }
}
