using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.GarmentFinance.BankCashReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.BankCashReceipt;
using System.Linq;
using Com.Moonlay.Models;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentFinance.BankCashReceipt
{
    public class BankCashReceiptService : IBankCashReceiptService
    {
        private const string UserAgent = "finance-service";
        public IIdentityService _identityService;
        public FinanceDbContext _dbContext;

        public BankCashReceiptService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
        }

        public async Task<int> CreateAsync(BankCashReceiptModel model)
        {
            model.ReceiptNo = await GenerateNo(model, 7);
            decimal totalAmount = 0;
            foreach (var item in model.Items)
            {
                if(model.CurrencyCode == "IDR")
                {
                    totalAmount += item.Summary;
                } else
                {
                    totalAmount += item.Amount;
                }
            }

            model.Amount = totalAmount;
            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
            }
            _dbContext.GarmentFinanceBankCashReceipts.Add(model);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GenerateNo(BankCashReceiptModel model, int clientTimeZoneOffset)
        {
            DateTimeOffset Now = model.ReceiptDate;
            string Year = Now.ToOffset(new TimeSpan(clientTimeZoneOffset, 0, 0)).ToString("yy");

            string no = $"{Year}/{model.NumberingCode}/";
            int Padding = 5;

            var lastNo = await _dbContext.GarmentFinanceBankCashReceipts.Where(w => w.ReceiptNo.StartsWith(no) && !w.IsDeleted).OrderByDescending(o => o.CreatedUtc).FirstOrDefaultAsync();
            no = $"{no}";

            if (lastNo == null)
            {
                return no+"1".PadLeft(Padding, '0');
            }
            else
            {
                int lastNoNumber = int.Parse(lastNo.ReceiptNo.Replace(no, "")) + 1;
                return no + lastNoNumber.ToString().PadLeft(Padding, '0');
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var existingModel = _dbContext.GarmentFinanceBankCashReceipts
                               .Include(d => d.Items)
                               .Single(x => x.Id == id && !x.IsDeleted);
            BankCashReceiptModel model = await ReadByIdAsync(id);
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent, true);
            }
            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent, true);
            _dbContext.GarmentFinanceBankCashReceipts.Update(model);

            return await _dbContext.SaveChangesAsync();
        }

        public ReadResponse<BankCashReceiptModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<BankCashReceiptModel> Query = _dbContext.GarmentFinanceBankCashReceipts.Include(m => m.Items);
            List<string> searchAttributes = new List<string>()
            {
                "ReceiptNo",
            };

            Query = QueryHelper<BankCashReceiptModel>.Search(Query, searchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<BankCashReceiptModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<BankCashReceiptModel>.Order(Query, OrderDictionary);

            Pageable<BankCashReceiptModel> pageable = new Pageable<BankCashReceiptModel>(Query, page - 1, size);
            List<BankCashReceiptModel> Data = pageable.Data.ToList();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<BankCashReceiptModel>(Data, TotalData, OrderDictionary, new List<string>());
        }

        public Task<BankCashReceiptModel> ReadByIdAsync(int id)
        {
            return _dbContext.GarmentFinanceBankCashReceipts.Include(m => m.Items).FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public async Task<int> UpdateAsync(int id, BankCashReceiptModel model)
        {
            BankCashReceiptModel exist = _dbContext.GarmentFinanceBankCashReceipts
                           .Include(d => d.Items)
                           .Single(dispo => dispo.Id == id && !dispo.IsDeleted);

            exist.Remarks = model.Remarks;
            exist.BuyerCode = model.BuyerCode;
            exist.BuyerId = model.BuyerId;
            exist.BuyerName = model.BuyerName;

            foreach (var item in exist.Items)
            {
                BankCashReceiptItemModel itemModel = model.Items.FirstOrDefault(prop => prop.Id.Equals(item.Id));

                if (itemModel == null)
                {
                    EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent, true);

                }
                else
                {
                    item.Amount = itemModel.Amount;
                    item.Summary = itemModel.Summary;
                    item.NoteNumber = itemModel.NoteNumber;
                    item.Remarks = itemModel.Remarks;
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

            decimal totalAmount = 0;
            foreach (var item in exist.Items)
            {
                if (model.CurrencyCode == "IDR")
                {
                    totalAmount += item.Summary;
                }
                else
                {
                    totalAmount += item.Amount;
                }
            }

            if (totalAmount != exist.Amount)
            {
                exist.Amount = totalAmount;
            }


            EntityExtension.FlagForUpdate(exist, _identityService.Username, UserAgent);

            return await _dbContext.SaveChangesAsync();
        }
    }
}
