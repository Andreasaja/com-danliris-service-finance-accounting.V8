using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentFinance.Memorial
{
    public class GarmentFinanceMemorialService : IGarmentFinanceMemorialService
    {
        private const string UserAgent = "finance-service";
        protected DbSet<GarmentFinanceMemorialModel> DbSet;
        public IIdentityService IdentityService;
        public readonly IServiceProvider ServiceProvider;
        public FinanceDbContext DbContext;

        public GarmentFinanceMemorialService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            DbContext = dbContext;
            ServiceProvider = serviceProvider;
            DbSet = dbContext.Set<GarmentFinanceMemorialModel>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
        }
        public async Task<int> CreateAsync(GarmentFinanceMemorialModel model)
        {
            model.MemorialNo = GenerateNo(model);

            EntityExtension.FlagForCreate(model, IdentityService.Username, UserAgent);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForCreate(item, IdentityService.Username, UserAgent);
            }
            DbSet.Add(model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task DeleteModel(int id)
        {
            GarmentFinanceMemorialModel model = await ReadByIdAsync(id);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
            }
            EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
            DbSet.Update(model);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var existingModel = DbSet
                        .Include(d => d.Items)
                        .Single(o => o.Id == id && !o.IsDeleted);

            await DeleteModel(id);
            return await DbContext.SaveChangesAsync();
        }

        public ReadResponse<GarmentFinanceMemorialModel> Read(int page, int size, string order, List<string> select, string keyword, string filter = "{}")
        {
            IQueryable<GarmentFinanceMemorialModel> Query = this.DbSet.Include(m => m.Items);
            List<string> searchAttributes = new List<string>()
            {
                "MemorialNo",  "AccountingBookCode", "AccountingBookType"
            };

            Query = QueryHelper<GarmentFinanceMemorialModel>.Search(Query, searchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<GarmentFinanceMemorialModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<GarmentFinanceMemorialModel>.Order(Query, OrderDictionary);


            var Data = Query.Skip((page - 1) * size).Take(size).ToList();
            int TotalData = Query.Count();

            return new ReadResponse<GarmentFinanceMemorialModel>(Data, TotalData, OrderDictionary, new List<string>());
        }

        public async Task<GarmentFinanceMemorialModel> ReadByIdAsync(int id)
        {
            return await DbSet.Include(m => m.Items)
                .FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public async Task<int> UpdateAsync(int id, GarmentFinanceMemorialModel model)
        {
            var exist = DbSet
                        .Include(d => d.Items)
                        .Single(o => o.Id == id && !o.IsDeleted);

            if(exist.Date != model.Date)
            {
                exist.Date = model.Date;
            }
            if (exist.Remark != model.Remark)
            {
                exist.Remark = model.Remark;
            }
            if (exist.GarmentCurrencyRate != model.GarmentCurrencyRate)
            {
                exist.GarmentCurrencyRate = model.GarmentCurrencyRate;
            }

            foreach (var item in exist.Items)
            {
                GarmentFinanceMemorialItemModel itemModel = model.Items.FirstOrDefault(prop => prop.Id.Equals(item.Id));

                if (itemModel == null)
                {
                    EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
                }
                else
                {
                    if(item.Debit!= itemModel.Debit)
                    {
                        item.Debit = itemModel.Debit;
                    }
                    if (item.Credit != itemModel.Credit)
                    {
                        item.Credit = itemModel.Credit;
                    }
                    EntityExtension.FlagForUpdate(item, IdentityService.Username, UserAgent);
                }
            }
            foreach(var newItem in model.Items)
            {
                if (newItem.Id <= 0)
                {
                    EntityExtension.FlagForCreate(newItem, IdentityService.Username, UserAgent);
                    exist.Items.Add(newItem);
                }
            }

            EntityExtension.FlagForUpdate(exist, IdentityService.Username, UserAgent);
            return await DbContext.SaveChangesAsync();
        }

        private string GenerateNo(GarmentFinanceMemorialModel model)
        {
            var yy = model.Date.ToString("yy");
            var mm = model.Date.ToString("MM");
            string prefix = yy + "/" + mm + "/E/";

            var lastNo = DbSet.Where(w => w.MemorialNo.StartsWith(prefix))
                .OrderByDescending(o => o.MemorialNo)
                .Select(s => int.Parse(s.MemorialNo.Replace(prefix, "")))
                .FirstOrDefault();

            var curNo = $"{prefix}{(lastNo + 1).ToString("D3")}";

            return curNo;
        }
    }
}
