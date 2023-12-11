using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.GarmentFinance.BankCashReceiptDetail;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceiptDetail;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceipt;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceiptDetail
{
    public class BankCashReceiptDetailService : IBankCashReceiptDetailService
    {
        private const string UserAgent = "finance-service";
        public IIdentityService _identityService;
        public FinanceDbContext _dbContext;

        public BankCashReceiptDetailService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
        }

        public async Task<int> CreateAsync(BankCashReceiptDetailModel model)
        {
            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
            }
            foreach (var otherItem in model.OtherItems)
            {
                EntityExtension.FlagForCreate(otherItem, _identityService.Username, UserAgent);
            }
            var receipts = await _dbContext.GarmentFinanceBankCashReceipts.FirstOrDefaultAsync(a => a.Id == model.BankCashReceiptId);
            if(receipts != null)
            {
                receipts.IsUsed = true;
            }
            _dbContext.GarmentFinanceBankCashReceiptDetails.Add(model);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var existingModel = _dbContext.GarmentFinanceBankCashReceiptDetails
                               .Include(d => d.Items)
                               .Include(d => d.OtherItems)
                               .Single(x => x.Id == id && !x.IsDeleted);
            BankCashReceiptDetailModel model = await ReadByIdAsync(id);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent, true);
            }
            foreach (var otherItem in model.OtherItems)
            {
                EntityExtension.FlagForDelete(otherItem, _identityService.Username, UserAgent, true);
            }

            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent, true);
            var receipts = await _dbContext.GarmentFinanceBankCashReceipts.FirstOrDefaultAsync(a => a.Id == model.BankCashReceiptId);
            if (receipts != null)
            {
                receipts.IsUsed = false;
            }
            _dbContext.GarmentFinanceBankCashReceiptDetails.Update(model);

            return await _dbContext.SaveChangesAsync();
        }

        public ReadResponse<BankCashReceiptDetailModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<BankCashReceiptDetailModel> Query = _dbContext.GarmentFinanceBankCashReceiptDetails.Include(m => m.Items).Include(m => m.OtherItems);
            List<string> searchAttributes = new List<string>()
            {
                "BankCashReceiptNo",
            };

            Query = QueryHelper<BankCashReceiptDetailModel>.Search(Query, searchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<BankCashReceiptDetailModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<BankCashReceiptDetailModel>.Order(Query, OrderDictionary);

            Pageable<BankCashReceiptDetailModel> pageable = new Pageable<BankCashReceiptDetailModel>(Query, page - 1, size);
            List<BankCashReceiptDetailModel> Data = pageable.Data.ToList();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<BankCashReceiptDetailModel>(Data, TotalData, OrderDictionary, new List<string>());
        }

        public async Task<BankCashReceiptDetailModel> ReadByIdAsync(int id)
        {
            return await _dbContext.GarmentFinanceBankCashReceiptDetails.Include(m => m.Items).Include(m => m.OtherItems).FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public async Task<int> UpdateAsync(int id, BankCashReceiptDetailModel model)
        {
            BankCashReceiptDetailModel exist = _dbContext.GarmentFinanceBankCashReceiptDetails
                           .Include(d => d.Items)
                           .Include(d => d.OtherItems)
                           .Single(dispo => dispo.Id == id && !dispo.IsDeleted);

            foreach (var item in exist.Items)
            {
                BankCashReceiptDetailItemModel itemModel = model.Items.FirstOrDefault(prop => prop.Id.Equals(item.Id));

                if (itemModel == null)
                {
                    EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent, true);

                }
                else
                {
                    item.Amount = itemModel.Amount;
                    EntityExtension.FlagForUpdate(item, _identityService.Username, UserAgent);

                }
            }

            foreach (var newItem in model.Items)
            {
                if (newItem.Id == 0)
                {
                    exist.Items.Add(newItem);
                    EntityExtension.FlagForCreate(newItem, _identityService.Username, UserAgent);
                }
            }

            foreach (var otherItem in exist.OtherItems)
            {
                BankCashReceiptDetailOtherItemModel itemModel = model.OtherItems.FirstOrDefault(prop => prop.Id.Equals(otherItem.Id));

                if (itemModel == null)
                {
                    EntityExtension.FlagForDelete(otherItem, _identityService.Username, UserAgent, true);

                }
                else
                {
                    otherItem.Amount = itemModel.Amount;
                    otherItem.TypeAmount = itemModel.TypeAmount;
                    EntityExtension.FlagForUpdate(otherItem, _identityService.Username, UserAgent);

                }
            }

            foreach (var newOtherItem in model.OtherItems)
            {
                if (newOtherItem.Id == 0)
                {
                    exist.OtherItems.Add(newOtherItem);
                    EntityExtension.FlagForCreate(newOtherItem, _identityService.Username, UserAgent);
                }
            }

            EntityExtension.FlagForUpdate(exist, _identityService.Username, UserAgent);

            return await _dbContext.SaveChangesAsync();
        }

        public double GetAmountByInvoiceId(int invoiceId)
        {
            double bankCashReceiptDetailItem = (double)_dbContext.GarmentFinanceBankCashReceiptDetailItems.Where(a => a.InvoiceId == invoiceId).Sum(a=>a.Amount);
            double memorialDetailItem = _dbContext.GarmentFinanceMemorialDetailItems.Where(a => a.InvoiceId == invoiceId).Sum(a => a.Amount);

            return bankCashReceiptDetailItem + memorialDetailItem;
        }
    }
}
