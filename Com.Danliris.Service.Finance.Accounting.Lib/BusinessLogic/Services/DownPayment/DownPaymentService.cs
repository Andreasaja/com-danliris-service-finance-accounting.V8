using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DownPayment
{
    public class DownPaymentService: IDownPaymentService
    {
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly List<string> _alphabets;
        private const string UserAgent = "finance-service";

        public DownPaymentService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();

            _alphabets = GetAlphabets();
        }

        public List<string> GetAlphabets()
        {
            //Declare string container for alphabet
            var result = new List<string>();

            //Loop through the ASCII characters 65 to 90
            for (int i = 65; i <= 90; i++)
            {
                // Convert the int to a char to get the actual character behind the ASCII code
                result.Add(((char)i).ToString());
            }

            return result;
        }

        public ReadResponse<DownPaymentList> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.DownPayments.AsQueryable();

            var searchAttributes = new List<string>()
            {
                "DocumentNo",
                "CurrencyCode",
                "BuyerName",
                "CategoryAcceptance"
            };

            query = QueryHelper<DownPaymentModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<DownPaymentModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<DownPaymentModel>.Order(query, orderDictionary);

            var pageable = new Pageable<DownPaymentModel>(query, page - 1, size);
            var data = pageable.Data.Select(entity => new DownPaymentList()
            {
                BuyerName = entity.BuyerName,
                DatePayment = entity.DatePayment,
                DocumentNo = entity.DocumentNo,
                Id = entity.Id,
                LastModifiedUtc = entity.LastModifiedUtc,
                CurrencyCode = entity.CurrencyCode,
                TotalPayment = entity.TotalPayment,
                CategoryAcceptance = entity.CategoryAcceptance

            }).ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<DownPaymentList>(data, totalData, orderDictionary, new List<string>());
        }

        public Task<int> CreateAsync(DownPaymentModel model)
        {
            model.DocumentNo = GetDownPaymentNo(model);

            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);

            _dbContext.DownPayments.Add(model);

            return _dbContext.SaveChangesAsync();
        }

        private string GetDownPaymentNo(DownPaymentModel model)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var documentNo = $"{year}{month}MEM{model.BuyerCode}";

            var countSameDocumentNo = _dbContext.DownPayments.Count(entity => entity.DocumentNo.Contains(documentNo));

            if (countSameDocumentNo > 0)
            {
                documentNo += _alphabets[countSameDocumentNo];
            }

            return documentNo;
        }

        public Task<DownPaymentModel> ReadByIdAsync(int id)
        {
            return _dbContext.DownPayments.Where(entity => entity.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> UpdateAsync(int id, DownPaymentModel model)
        {
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);

            _dbContext.DownPayments.Update(model);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(int id)
        {
            var model = _dbContext.DownPayments.Where(entity => entity.Id == id).FirstOrDefault();

            if (model != null)
            {
                EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);

                _dbContext.DownPayments.Update(model);
            }

            return _dbContext.SaveChangesAsync();
        }
    }
}