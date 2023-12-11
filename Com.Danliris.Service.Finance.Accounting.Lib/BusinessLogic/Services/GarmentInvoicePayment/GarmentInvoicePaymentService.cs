using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.GarmentInvoicePayment;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePayment;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Newtonsoft.Json;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentInvoicePayment
{
    public class GarmentInvoicePaymentService : IGarmentInvoicePaymentService
    {
        private const string UserAgent = "finance-service";
        protected DbSet<GarmentInvoicePaymentModel> DbSet;
        public IIdentityService IdentityService;
        public readonly IServiceProvider ServiceProvider;
        public FinanceDbContext DbContext;

        public GarmentInvoicePaymentService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            ServiceProvider = serviceProvider;
            DbContext = dbContext;
            DbSet = dbContext.Set<GarmentInvoicePaymentModel>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public async Task<int> CreateAsync(GarmentInvoicePaymentModel model)
        {
            model.InvoicePaymentNo = await GenerateNo(model, 7);
            EntityExtension.FlagForCreate(model, IdentityService.Username, UserAgent);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForCreate(item, IdentityService.Username, UserAgent);
            }
            DbSet.Add(model);
            return await DbContext.SaveChangesAsync();
        }

        async Task<string> GenerateNo(GarmentInvoicePaymentModel model, int clientTimeZoneOffset)
        {
            DateTimeOffset Now = model.PaymentDate;
            string Year = Now.ToOffset(new TimeSpan(clientTimeZoneOffset, 0, 0)).ToString("yyyy");
            string Month = Now.ToOffset(new TimeSpan(clientTimeZoneOffset, 0, 0)).ToString("MM");

            string no = $"/{Month}/E/{Year}";
            int Padding = 3;

            var lastNo = await this.DbSet.Where(w => w.InvoicePaymentNo.EndsWith(no) && !w.IsDeleted).OrderByDescending(o => o.CreatedUtc).FirstOrDefaultAsync();
            no = $"{no}";

            if (lastNo == null)
            {
                return "1".PadLeft(Padding, '0') + no;
            }
            else
            {
                int lastNoNumber = int.Parse(lastNo.InvoicePaymentNo.Replace(no, "")) + 1;
                return lastNoNumber.ToString().PadLeft(Padding, '0') + no;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var existingModel = DbSet
                        .Include(d => d.Items)
                        .Single(x => x.Id == id && !x.IsDeleted);
            GarmentInvoicePaymentModel model = await ReadByIdAsync(id);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
            }
            EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
            DbSet.Update(model);

            return await DbContext.SaveChangesAsync();
        }

        public ReadResponse<GarmentInvoicePaymentModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<GarmentInvoicePaymentModel> Query = this.DbSet.Include(m => m.Items);
            List<string> searchAttributes = new List<string>()
            {
                "InvoicePaymentNo", "BuyerName",  "BGNo"
            };

            Query = QueryHelper<GarmentInvoicePaymentModel>.Search(Query, searchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<GarmentInvoicePaymentModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<GarmentInvoicePaymentModel>.Order(Query, OrderDictionary);

            Pageable<GarmentInvoicePaymentModel> pageable = new Pageable<GarmentInvoicePaymentModel>(Query, page - 1, size);
            List<GarmentInvoicePaymentModel> Data = pageable.Data.ToList();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<GarmentInvoicePaymentModel>(Data, TotalData, OrderDictionary, new List<string>());
        }

        public Task<GarmentInvoicePaymentModel> ReadByIdAsync(int id)
        {
            return DbSet.Include(m => m.Items)
                .FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public async Task<int> UpdateAsync(int id, GarmentInvoicePaymentModel model)
        {
            GarmentInvoicePaymentModel exist = DbSet
                        .Include(d => d.Items)
                        .Single(dispo => dispo.Id == id && !dispo.IsDeleted);

            exist.PaymentDate = model.PaymentDate;
            exist.BuyerId = model.BuyerId;
            exist.BuyerCode = model.BuyerCode;
            exist.BuyerName = model.BuyerName;
            exist.BGNo = model.BGNo;
            exist.Remark = model.Remark;

            foreach (var item in exist.Items)
            {
                GarmentInvoicePaymentItemModel itemModel = model.Items.FirstOrDefault(prop => prop.Id.Equals(item.Id));

                if (itemModel == null)
                {
                    EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);

                }
                else
                {
                    EntityExtension.FlagForUpdate(item, IdentityService.Username, UserAgent);

                }
            }
            foreach(var newItem in model.Items)
            {
                if (newItem.Id == 0)
                {
                    exist.Items.Add(newItem);
                    EntityExtension.FlagForCreate(newItem, IdentityService.Username, UserAgent);
                }
            }


            EntityExtension.FlagForUpdate(exist, IdentityService.Username, UserAgent);

            return await DbContext.SaveChangesAsync();
        }
    }
}
