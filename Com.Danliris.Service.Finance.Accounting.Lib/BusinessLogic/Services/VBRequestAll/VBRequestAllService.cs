using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBRequestAll;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRequestAll;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using Com.Moonlay.NetCore.Lib;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Com.Moonlay.Models;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBRequestAll
{
    public class VBRequestAllService : IVBRequestAllService
    {
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private const string _userAgent = "finance-service";
        protected DbSet<VbRequestModel> _DbSet;
        protected DbSet<VbRequestDetailModel> _DetailDbSet;
        private readonly IServiceProvider _serviceProvider;

        public VBRequestAllService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _DbSet = _dbContext.Set<VbRequestModel>();
            _DetailDbSet = _dbContext.Set<VbRequestDetailModel>();
            _serviceProvider = serviceProvider;
        }

        public ReadResponse<VBRequestAllViewModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.VbRequests.AsQueryable();

            var searchAttributes = new List<string>()
            {
                "VBNo",
                "UnitLoad",
                "CreatedBy"
            };

            query = QueryHelper<VbRequestModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<VbRequestModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VbRequestModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VbRequestModel>(query, page - 1, size);
            var data = query.Include(s => s.VbRequestDetail).Select(entity => new VBRequestAllViewModel
            {
                Id = entity.Id,
                VBNo = entity.VBNo,
                Date = entity.Date,
                DateEstimate = entity.DateEstimate,
                ApproveDate = entity.ApproveDate,
                UnitLoad = entity.UnitLoad,
                UnitId = entity.UnitId,
                UnitCode = entity.UnitCode,
                UnitName = entity.UnitName,
                CurrencyId = entity.CurrencyId,
                CurrencyCode = entity.CurrencyCode,
                CurrencyRate = entity.CurrencyRate,
                CurrencySymbol = entity.CurrencySymbol,
                CreateBy = entity.CreatedBy,
                Amount = entity.Amount,
                Approve_Status = entity.Apporve_Status,
                Complete_Status = entity.Complete_Status,
                VBRequestCategory = entity.VBRequestCategory,
            }).ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<VBRequestAllViewModel>(data, totalData, orderDictionary, new List<string>());
        }

        public Task<int> CreateAsync(VbRequestModel model)
        {
            EntityExtension.FlagForCreate(model, _identityService.Username, _userAgent);

            _dbContext.VbRequests.Add(model);

            return _dbContext.SaveChangesAsync();
        }

        public Task<VbRequestModel> ReadByIdAsync(int id)
        {
            return _dbContext.VbRequests.Where(entity => entity.Id == id).FirstOrDefaultAsync();
        }
    }
}
