
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.MemorialDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Memorial;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.MemorialDetail
{
    public class GarmentFinanceMemorialDetailService : IGarmentFinanceMemorialDetailService
    {
        private const string UserAgent = "finance-service";
        protected DbSet<GarmentFinanceMemorialDetailModel> DbSet;
        public IIdentityService IdentityService;
        public readonly IServiceProvider ServiceProvider;
        protected DbSet<GarmentFinanceMemorialModel> GarmentFinanceMemorialDbSet;
        public FinanceDbContext DbContext;

        public GarmentFinanceMemorialDetailService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            DbContext = dbContext;
            ServiceProvider = serviceProvider;
            DbSet = dbContext.Set<GarmentFinanceMemorialDetailModel>();
            GarmentFinanceMemorialDbSet= dbContext.Set<GarmentFinanceMemorialModel>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public ReadResponse<GarmentFinanceMemorialDetailModel> Read(int page, int size, string order, List<string> select, string keyword, string filter = "{}")
        {
            IQueryable<GarmentFinanceMemorialDetailModel> Query = this.DbSet.Include(m => m.Items).Include(m => m.OtherItems).Include(m => m.RupiahItems);
            List<string> searchAttributes = new List<string>()
            {
                "MemorialNo"
            };

            Query = QueryHelper<GarmentFinanceMemorialDetailModel>.Search(Query, searchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<GarmentFinanceMemorialDetailModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<GarmentFinanceMemorialDetailModel>.Order(Query, OrderDictionary);


            var Data = Query.Skip((page - 1) * size).Take(size).ToList();
            int TotalData = Query.Count();

            return new ReadResponse<GarmentFinanceMemorialDetailModel>(Data, TotalData, OrderDictionary, new List<string>());
        }

        public async Task<int> CreateAsync(GarmentFinanceMemorialDetailModel model)
        {
            EntityExtension.FlagForCreate(model, IdentityService.Username, UserAgent);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForCreate(item, IdentityService.Username, UserAgent);
            }
            foreach (var otherItem in model.OtherItems)
            {
                EntityExtension.FlagForCreate(otherItem, IdentityService.Username, UserAgent);
            }
            foreach (var rupiahItem in model.RupiahItems)
            {
                EntityExtension.FlagForCreate(rupiahItem, IdentityService.Username, UserAgent);
            }
            GarmentFinanceMemorialModel memorial = GarmentFinanceMemorialDbSet.FirstOrDefault(a => a.Id == model.MemorialId);
            memorial.IsUsed = true;
            DbSet.Add(model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<GarmentFinanceMemorialDetailModel> ReadByIdAsync(int id)
        {
            return await DbSet.Include(m => m.Items).Include(m => m.OtherItems).Include(m => m.RupiahItems)
                .FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public async Task<int> DeleteAsync(int id)
        {
            var existingModel = DbSet
                        .Include(d => d.Items)
                        .Include(d => d.OtherItems)
                        .Include(m => m.RupiahItems)
                        .Single(o => o.Id == id && !o.IsDeleted);

            GarmentFinanceMemorialDetailModel model = await ReadByIdAsync(id);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
            }
            foreach (var otherItem in model.OtherItems)
            {
                EntityExtension.FlagForDelete(otherItem, IdentityService.Username, UserAgent, true);
            }
            foreach (var rupiahItem in model.RupiahItems)
            {
                EntityExtension.FlagForDelete(rupiahItem, IdentityService.Username, UserAgent, true);
            }
            EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
            var memorial = GarmentFinanceMemorialDbSet.Single(a => a.Id == model.MemorialId);
            memorial.IsUsed = false;
            DbSet.Update(model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(int id, GarmentFinanceMemorialDetailModel model)
        {
            var exist = DbSet
                        .Include(d => d.Items)
                        .Include(d => d.OtherItems)
                        .Include(d => d.RupiahItems)
                        .Single(o => o.Id == id && !o.IsDeleted);

            foreach (var item in exist.Items)
            {
                GarmentFinanceMemorialDetailItemModel itemModel = model.Items.FirstOrDefault(prop => prop.Id.Equals(item.Id));

                if (itemModel == null)
                {
                    EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
                }
                else
                {
                    EntityExtension.FlagForUpdate(item, IdentityService.Username, UserAgent);
                }
            }
            foreach (var newItem in model.Items)
            {
                if (newItem.Id <= 0)
                {
                    EntityExtension.FlagForCreate(newItem, IdentityService.Username, UserAgent);
                    exist.Items.Add(newItem);
                }
            }

            foreach (var otherItem in exist.OtherItems)
            {
                GarmentFinanceMemorialDetailOtherItemModel otherItemModel = model.OtherItems.FirstOrDefault(prop => prop.Id.Equals(otherItem.Id));

                if (otherItemModel == null)
                {
                    EntityExtension.FlagForDelete(otherItem, IdentityService.Username, UserAgent, true);
                }
                else
                {
                    otherItem.Amount = otherItemModel.Amount;
                    otherItem.CurrencyRate = otherItemModel.CurrencyRate;
                    otherItem.TypeAmount = otherItemModel.TypeAmount;
                    EntityExtension.FlagForUpdate(otherItem, IdentityService.Username, UserAgent);
                }
            }
            foreach (var newOtherItem in model.OtherItems)
            {
                if (newOtherItem.Id <= 0)
                {
                    EntityExtension.FlagForCreate(newOtherItem, IdentityService.Username, UserAgent);
                    exist.OtherItems.Add(newOtherItem);
                }
            }

            foreach (var rupiahItem in exist.RupiahItems)
            {
                GarmentFinanceMemorialDetailRupiahItemModel rupiahItemModel = model.RupiahItems.FirstOrDefault(prop => prop.Id.Equals(rupiahItem.Id));

                if (rupiahItemModel == null)
                {
                    EntityExtension.FlagForDelete(rupiahItem, IdentityService.Username, UserAgent, true);
                }
                else
                {
                    rupiahItem.Credit = rupiahItemModel.Credit;
                    rupiahItem.Debit = rupiahItemModel.Debit;
                    EntityExtension.FlagForUpdate(rupiahItem, IdentityService.Username, UserAgent);
                }
            }
            foreach (var newRupiahItem in model.RupiahItems)
            {
                if (newRupiahItem.Id <= 0)
                {
                    EntityExtension.FlagForCreate(newRupiahItem, IdentityService.Username, UserAgent);
                    exist.RupiahItems.Add(newRupiahItem);
                }
            }

            EntityExtension.FlagForUpdate(exist, IdentityService.Username, UserAgent);
            return await DbContext.SaveChangesAsync();
        }
    }
}
