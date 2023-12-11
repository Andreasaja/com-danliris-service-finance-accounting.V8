using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoTextile;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoTextile
{
    public class PurchasingMemoTextileService : IPurchasingMemoTextileService
    {
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IJournalTransactionService _journalTransactionService;

        public PurchasingMemoTextileService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
            _journalTransactionService = serviceProvider.GetService<IJournalTransactionService>();
        }

        public int Create(FormDto form)
        {
            var model = new PurchasingMemoTextileModel(form.MemoDetail.Id, form.MemoDetail.DocumentNo, form.MemoDetail.Date, form.MemoDetail.Currency.Id, form.MemoDetail.Currency.Code, form.MemoDetail.Currency.Rate, form.AccountingBook.Id, form.AccountingBook.AccountingBookType, form.Remark, form.AccountingBook.Code);
            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            _dbContext.PurchasingMemoTextiles.Add(model);
            _dbContext.SaveChanges();

            foreach (var item in form.Items)
            {
                var itemModel = new PurchasingMemoTextileItemModel(item.ChartOfAccount.Id, item.ChartOfAccount.Code, item.ChartOfAccount.Name, item.DebitAmount, item.CreditAmount, model.Id);
                EntityExtension.FlagForCreate(itemModel, _identityService.Username, UserAgent);
                _dbContext.PurchasingMemoTextileItems.Add(itemModel);
                _dbContext.SaveChanges();
            }

            return model.Id;
        }

        public int Delete(int id)
        {
            var deletedId = 0;

            var model = _dbContext.PurchasingMemoTextiles.FirstOrDefault(entity => entity.Id == id);

            if (model != null)
            {
                EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);
                _dbContext.PurchasingMemoTextiles.Update(model);

                var items = _dbContext.PurchasingMemoTextileItems.Where(entity => entity.PurchasingMemoTextileId == id).ToList();
                items = items.Select(item =>
                {
                    EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent);
                    return item;
                }).ToList();

                _dbContext.PurchasingMemoTextileItems.UpdateRange(items);
                _dbContext.SaveChanges();
                deletedId = model.Id;
            }
            return deletedId;
        }

        public async Task<int> Posting(PostingFormDto form)
        {
            var models = _dbContext.PurchasingMemoTextiles.Where(entity => form.Ids.Contains(entity.Id)).ToList();

            models = models.Select(model =>
            {
                model.SetIsPosted(true);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                return model;
            }).ToList();
            _dbContext.SaveChanges();

            foreach (var model in models)
            {
                var items = _dbContext.PurchasingMemoTextileItems.Where(entity => entity.PurchasingMemoTextileId == model.Id).ToList();
                var memoDetail = _dbContext.PurchasingMemoDetailTextiles.FirstOrDefault(entity => entity.Id == model.MemoDetailId);

                var type = memoDetail.Type == PurchasingMemoType.Disposition ? "Disposisi" : "Non Disposisi";
                var journalTransactionModel = new JournalTransactionModel()
                {
                    Date = model.MemoDetailDate,
                    Description = $"Memo Pembelian {type}",
                    ReferenceNo = model.MemoDetailDocumentNo,
                    Status = "POSTED",
                    Items = new List<JournalTransactionItemModel>()
                };

                foreach (var item in items)
                {
                    journalTransactionModel.Items.Add(new JournalTransactionItemModel()
                    {
                        COA = new Models.MasterCOA.COAModel()
                        {
                            Id = item.ChartOfAccountId,
                            Code = item.ChartOfAccountCode,
                            Name = item.ChartOfAccountName
                        },
                        COAId = item.ChartOfAccountId,
                        Credit = (decimal)item.CreditAmount,
                        Debit = (decimal)item.DebitAmount
                    });
                }

                await _journalTransactionService.CreateAsync(journalTransactionModel);

            }

            return models.Count;
        }

        public ReadResponse<IndexDto> Read(string keyword, int page = 1, int size = 25)
        {
            var query = _dbContext.PurchasingMemoTextiles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.MemoDetailDocumentNo.Contains(keyword));

            var count = query.Count();
            var data = query.Skip((page - 1) * size).Take(size).Select(entity => new IndexDto(entity.Id, entity.MemoDetailDocumentNo, entity.MemoDetailDate, entity.AccountingBookType, entity.MemoDetailCurrencyCode, entity.Remark, entity.IsPosted, "")).ToList();
            return new ReadResponse<IndexDto>(data, count, new Dictionary<string, string>(), new List<string>());
        }

        public PurchasingTextileDto Read(int id)
        {
            var model = _dbContext.PurchasingMemoTextiles.FirstOrDefault(entity => entity.Id == id);

            if (model != null)
            {
                var items = _dbContext.PurchasingMemoTextileItems.Where(entity => entity.PurchasingMemoTextileId == id).ToList();

                var result = new PurchasingTextileDto(model.Id, new AccountingBookDto(model.AccountingBookId, model.AccountingBookCode, model.AccountingBookType), new MemoDetailDto(model.MemoDetailId, model.MemoDetailDocumentNo, model.MemoDetailDate, new CurrencyDto(model.MemoDetailCurrencyId, model.MemoDetailCurrencyCode, model.MemoDetailCurrencyRate)), model.Remark, new List<FormItemDto>(), "");

                foreach (var item in items)
                {
                    result.Items.Add(new FormItemDto(new ChartOfAccountDto(item.ChartOfAccountId, item.ChartOfAccountCode, item.ChartOfAccountName), item.DebitAmount, item.CreditAmount));
                }

                return result;
            }
            else
                return null;
        }

        public int Update(int id, FormDto form)
        {
            var updatedId = 0;
            var model = _dbContext.PurchasingMemoTextiles.FirstOrDefault(entity => entity.Id == id);

            if (model != null)
            {
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                _dbContext.PurchasingMemoTextiles.Update(model);

                var items = _dbContext.PurchasingMemoTextileItems.Where(entity => entity.PurchasingMemoTextileId == model.Id).ToList();

                items = items.Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                }).ToList();
                _dbContext.PurchasingMemoTextileItems.UpdateRange(items);

                _dbContext.SaveChanges();

                foreach (var item in form.Items)
                {
                    var itemModel = new PurchasingMemoTextileItemModel(item.ChartOfAccount.Id, item.ChartOfAccount.Code, item.ChartOfAccount.Name, item.DebitAmount, item.CreditAmount, model.Id);
                    EntityExtension.FlagForCreate(itemModel, _identityService.Username, UserAgent);
                    _dbContext.PurchasingMemoTextileItems.Add(itemModel);
                    _dbContext.SaveChanges();
                }

                updatedId = model.Id;
            }

            return updatedId;
        }
    }
}
