using Com.Danliris.Service.Finance.Accounting.Lib.Models.Memo;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Memo;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Memo
{
    public class MemoService : IMemoService
    {
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly List<string> _alphabets;
        private const string UserAgent = "finance-service";

        public MemoService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
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

        public Task<int> CreateAsync(MemoModel model)
        {
            model.DocumentNo = GetMemoDocumentNo(model);

            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);

            foreach (var item in model.Items)
                EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);

            _dbContext.Memos.Add(model);

            return _dbContext.SaveChangesAsync();
        }

        private string GetMemoDocumentNo(MemoModel model)
        {
            var now = model.Date;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var documentNo = $"{year}{month}MEM{model.BuyerCode}";

            var countSameDocumentNo = _dbContext.Memos.Count(entity => entity.DocumentNo.Contains(documentNo));

            if (countSameDocumentNo > 0)
            {
                documentNo += _alphabets[countSameDocumentNo];
            }

            return documentNo;
        }

        public Task<int> DeleteAsync(int id)
        {
            var model = _dbContext.Memos.Include(entity => entity.Items).Where(entity => entity.Id == id).FirstOrDefault();

            if (model != null)
            {
                EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);

                foreach (var item in model.Items)
                    EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent);

                _dbContext.Memos.Update(model);
            }

            return _dbContext.SaveChangesAsync();
        }

        public ReadResponse<MemoList> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.Memos.Include(entity => entity.Items).AsQueryable();

            var searchAttributes = new List<string>()
            {
                "DocumentNo",
                "SalesInvoiceNo",
                "MemoType",
                "BuyerName"
            };

            query = QueryHelper<MemoModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<MemoModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<MemoModel>.Order(query, orderDictionary);

            var pageable = new Pageable<MemoModel>(query, page - 1, size);
            var data = pageable.Data.Select(entity => new MemoList() 
            {
                BuyerName = entity.BuyerName,
                CurrencyCodes = string.Join("\n", entity.Items.Select(element => element.CurrencyCode).Distinct().Select(element => $"- {element}")),
                Date = entity.Date,
                DocumentNo = entity.DocumentNo,
                Id = entity.Id,
                LastModifiedUtc = entity.LastModifiedUtc,
                MemoType = entity.MemoType,
                SalesInvoiceNo = entity.SalesInvoiceNo
            }).ToList();

            int totalData = pageable.TotalCount;

            return new ReadResponse<MemoList>(data, totalData, orderDictionary, new List<string>());
        }

        public Task<MemoModel> ReadByIdAsync(int id)
        {
            return _dbContext.Memos.Include(entity => entity.Items).Where(entity => entity.Id == id).FirstOrDefaultAsync();
        }

        public Task<MemoModel> ReadBySalesInvoiceAsync(string SalesInvoiceNo)
        {
            return _dbContext.Memos.Include(entity => entity.Items).Where(entity => entity.SalesInvoiceNo == SalesInvoiceNo).FirstOrDefaultAsync();
        }

        public Task<int> UpdateAsync(int id, MemoModel model)
        {
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);

            var itemIds = _dbContext.MemoItems.Where(entity => entity.MemoId == id).Select(entity => entity.Id).ToList();

            foreach (var itemId in itemIds)
            {
                var item = model.Items.FirstOrDefault(element => element.Id == itemId);
                if (item == null)
                {
                    var itemToDelete = _dbContext.MemoItems.FirstOrDefault(entity => entity.Id == itemId);
                    EntityExtension.FlagForDelete(itemToDelete, _identityService.Username, UserAgent);
                    _dbContext.MemoItems.Update(itemToDelete);
                }
                else
                {
                    EntityExtension.FlagForUpdate(item, _identityService.Username, UserAgent);
                    _dbContext.MemoItems.Update(item);
                }
            }

            foreach (var item in model.Items)
            {
                if (item.Id <= 0)
                {
                    EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
                    _dbContext.MemoItems.Add(item);
                }
            }

            _dbContext.Memos.Update(model);

            return _dbContext.SaveChangesAsync();
        }
    }
}
