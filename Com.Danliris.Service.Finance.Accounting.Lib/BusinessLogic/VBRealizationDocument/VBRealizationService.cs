using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument
{
    public class VBRealizationService : IVBRealizationService
    {
        private const string UserAgent = "finance-service";
        public readonly FinanceDbContext _dbContext;

        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;

        public VBRealizationService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public ReadResponse<VBRealizationDocumentModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.Set<VBRealizationDocumentModel>().AsQueryable();

            List<string> searchAttributes = new List<string>()
            {
                "DocumentNo", "SuppliantUnitName","VBRequestDocumentNo","VBRequestDocumentCreatedBy"
            };

            query = QueryHelper<VBRealizationDocumentModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<VBRealizationDocumentModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VBRealizationDocumentModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VBRealizationDocumentModel>(query, page - 1, size);
            var data = pageable.Data.ToList();

            int TotalData = pageable.TotalCount;

            return new ReadResponse<VBRealizationDocumentModel>(data, TotalData, orderDictionary, new List<string>());
        }

        public async Task<Tuple<VBRealizationDocumentModel, List<VBRealizationDocumentExpenditureItemModel>, List<VBRealizationDocumentUnitCostsItemModel>>> ReadByIdAsync(int id)
        {
            var data = await _dbContext.VBRealizationDocuments.FirstOrDefaultAsync(s => s.Id == id);

            if (data == null)
                return new Tuple<VBRealizationDocumentModel, List<VBRealizationDocumentExpenditureItemModel>, List<VBRealizationDocumentUnitCostsItemModel>>(null, new List<VBRealizationDocumentExpenditureItemModel>(), new List<VBRealizationDocumentUnitCostsItemModel>());

            var items = _dbContext.VBRealizationDocumentExpenditureItems.Where(s => s.VBRealizationDocumentId == id).ToList();
            var expenditureIds = items.Select(s => s.Id);
            var unitCostItems = new List<VBRealizationDocumentUnitCostsItemModel>();
            if(data.Type == VBType.WithPO)
            {

                unitCostItems = _dbContext.VBRealizationDocumentUnitCostsItems.Where(s => expenditureIds.Contains(s.VBRealizationDocumentExpenditureItemId)).ToList();
            }
            else
            {
                unitCostItems = _dbContext.VBRealizationDocumentUnitCostsItems.Where(s => s.VBRealizationDocumentId == id).ToList();

            }

            return new Tuple<VBRealizationDocumentModel, List<VBRealizationDocumentExpenditureItemModel>, List<VBRealizationDocumentUnitCostsItemModel>>(data, items, unitCostItems);
        }

        public PostingJournalDto ReadByReferenceNo(string referenceNo)
        {
            var result = _dbContext.VBRealizationDocuments.Where(entity => entity.ReferenceNo == referenceNo).Select(entity => new PostingJournalDto(entity.VBRequestDocumentNo, entity.DocumentNo)).FirstOrDefault();
            return result;
        }
    }
}
