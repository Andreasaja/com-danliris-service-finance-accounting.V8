using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CreditorAccount;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingMemoDetailTextile;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile
{
    public class PurchasingMemoDetailTextileService : IPurchasingMemoDetailTextileService
    {
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly ICreditorAccountService _creditorAccountService;

        public PurchasingMemoDetailTextileService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
            _creditorAccountService = serviceProvider.GetService<ICreditorAccountService>();
        }

        private string GenerateDocumentNo(FormDto form)
        {
            var documentNo = $"{form.Date.AddHours(_identityService.TimezoneOffset):MM}{form.Date.AddHours(_identityService.TimezoneOffset):yy}.MT.0001";
            var month = form.Date.AddHours(_identityService.TimezoneOffset).Month;
            var year = form.Date.AddHours(_identityService.TimezoneOffset).Year;
            var lastDocumentNo = _dbContext.PurchasingMemoDetailTextiles.Where(entity => entity.Date.AddHours(_identityService.TimezoneOffset).Month == month && entity.Date.AddHours(_identityService.TimezoneOffset).Year == year).OrderByDescending(entity => entity.Date).ThenByDescending(entity => entity.CreatedUtc).Select(entity => entity.DocumentNo).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(lastDocumentNo))
            {
                var splittedDocument = lastDocumentNo.Split('.');
                var lastNumber = int.Parse(splittedDocument.LastOrDefault());
                lastNumber += 1;
                documentNo = $"{form.Date.AddHours(_identityService.TimezoneOffset):MM}{form.Date.AddHours(_identityService.TimezoneOffset):yy}.MT.{lastNumber.ToString().PadLeft(4, '0')}";
            }

            return documentNo;
        }

        public int Create(FormDto form)
        {
            var documentNo = GenerateDocumentNo(form);
            var model = new PurchasingMemoDetailTextileModel(form.Date, form.Division.Id, form.Division.Code, form.Division.Name, form.Currency.Id, form.Currency.Code, form.Currency.Rate, form.SupplierIsImport, form.Remark, form.Type, documentNo);
            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            _dbContext.PurchasingMemoDetailTextiles.Add(model);
            _dbContext.SaveChanges();

            if (form.Type == PurchasingMemoType.Disposition)
            {
                foreach (var item in form.Items)
                {
                    var itemModel = new PurchasingMemoDetailTextileItemModel(item.Disposition.Id, item.Disposition.DocumentNo, item.Disposition.Date, model.Id);
                    EntityExtension.FlagForCreate(itemModel, _identityService.Username, UserAgent);
                    _dbContext.PurchasingMemoDetailTextileItems.Add(itemModel);
                    _dbContext.SaveChanges();

                    foreach (var detail in item.Disposition.Details)
                    {
                        var detailModel = new PurchasingMemoDetailTextileDetailModel(model.Id, itemModel.Id, detail.Expenditure.Id, detail.Expenditure.DocumentNo, detail.Expenditure.Date, detail.Supplier.Id, detail.Supplier.Code, detail.Supplier.Name, detail.Remark, detail.UnitPaymentOrder.Id, detail.UnitPaymentOrder.UnitPaymentOrderNo, detail.UnitPaymentOrder.UnitPaymentOrderDate, detail.PurchaseAmountCurrency, detail.PurchaseAmount, detail.PaymentAmountCurrency, detail.PaymentAmount);
                        EntityExtension.FlagForCreate(detailModel, _identityService.Username, UserAgent);
                        _dbContext.PurchasingMemoDetailTextileDetails.Add(detailModel);
                        _dbContext.SaveChanges();

                        _creditorAccountService.CreateFromPurchasingMemoTextile(new CreditorAccountPurchasingMemoTextileFormDto(detail.UnitPaymentOrder.UnitPaymentOrderNo, model.Id, model.DocumentNo, detail.PaymentAmount, form.Date));

                        foreach (var unitReceiptNote in detail.UnitReceiptNotes)
                        {
                            var unitReceiptNoteModel = new PurchasingMemoDetailTextileUnitReceiptNoteModel(model.Id, itemModel.Id, detailModel.Id, unitReceiptNote.Id, unitReceiptNote.UnitReceiptNoteNo, unitReceiptNote.UnitReceiptNoteDate);
                            EntityExtension.FlagForCreate(unitReceiptNoteModel, _identityService.Username, UserAgent);
                            _dbContext.PurchasingMemoDetailTextileUnitReceiptNotes.Add(unitReceiptNoteModel);
                            _dbContext.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                foreach (var detail in form.Details)
                {
                    var detailModel = new PurchasingMemoDetailTextileDetailModel(model.Id, 0, detail.Expenditure.Id, detail.Expenditure.DocumentNo, detail.Expenditure.Date, detail.Supplier.Id, detail.Supplier.Code, detail.Supplier.Name, detail.Remark, detail.UnitPaymentOrder.Id, detail.UnitPaymentOrder.UnitPaymentOrderNo, detail.UnitPaymentOrder.UnitPaymentOrderDate, detail.PurchaseAmountCurrency, detail.PurchaseAmount, detail.PaymentAmountCurrency, detail.PaymentAmount);
                    EntityExtension.FlagForCreate(detailModel, _identityService.Username, UserAgent);
                    _dbContext.PurchasingMemoDetailTextileDetails.Add(detailModel);
                    _dbContext.SaveChanges();

                    _creditorAccountService.CreateFromPurchasingMemoTextile(new CreditorAccountPurchasingMemoTextileFormDto(detail.UnitPaymentOrder.UnitPaymentOrderNo, model.Id, model.DocumentNo, detail.PaymentAmount, form.Date));

                    foreach (var unitReceiptNote in detail.UnitReceiptNotes)
                    {
                        var unitReceiptNoteModel = new PurchasingMemoDetailTextileUnitReceiptNoteModel(model.Id, 0, detailModel.Id, unitReceiptNote.Id, unitReceiptNote.UnitReceiptNoteNo, unitReceiptNote.UnitReceiptNoteDate);
                        EntityExtension.FlagForCreate(unitReceiptNoteModel, _identityService.Username, UserAgent);
                        _dbContext.PurchasingMemoDetailTextileUnitReceiptNotes.Add(unitReceiptNoteModel);
                        _dbContext.SaveChanges();
                    }
                }
            }

            return model.Id;
        }

        public int Delete(int id)
        {
            var deletedId = 0;

            var model = _dbContext.PurchasingMemoDetailTextiles.FirstOrDefault(entity => entity.Id == id);

            if (model != null)
            {
                EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);
                _dbContext.PurchasingMemoDetailTextiles.Update(model);

                var items = _dbContext.PurchasingMemoDetailTextileItems.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id).ToList();
                var details = _dbContext.PurchasingMemoDetailTextileDetails.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id).ToList();
                var unitReceiptNotes = _dbContext.PurchasingMemoDetailTextileUnitReceiptNotes.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id).ToList();

                items = items.Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                }).ToList();
                _dbContext.PurchasingMemoDetailTextileItems.UpdateRange(items);

                details = details.Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                }).ToList();
                _dbContext.PurchasingMemoDetailTextileDetails.UpdateRange(details);

                unitReceiptNotes = unitReceiptNotes.Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                }).ToList();
                _dbContext.PurchasingMemoDetailTextileUnitReceiptNotes.UpdateRange(unitReceiptNotes);

                _dbContext.SaveChanges();

                foreach (var detail in details)
                {
                    _creditorAccountService.DeleteFromPurchasingMemoTextile(new CreditorAccountPurchasingMemoTextileFormDto(detail.UnitPaymentOrderNo, 0, null, 0, null));
                }
                deletedId = model.Id;
            }

            return deletedId;
        }

        public ReadResponse<IndexDto> Read(string keyword, PurchasingMemoType type, int page = 1, int size = 25)
        {
            var query = _dbContext.PurchasingMemoDetailTextiles.Where(entity => entity.Type == type);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(entity => entity.DocumentNo.Contains(keyword) || entity.DivisionName.Contains(keyword) || entity.DivisionCode.Contains(keyword) || entity.CurrencyCode.Contains(keyword));
            }

            var count = query.Select(entity => entity.Id).Count();
            var data = query.OrderByDescending(entity => entity.LastModifiedUtc).Skip((page - 1) * size).Take(size).Select(entity => new IndexDto(entity.Id, entity.LastModifiedUtc, entity.Date, entity.DivisionName, entity.CurrencyCode, entity.SupplierIsImport, entity.Remark, entity.DocumentNo)).ToList();
            return new ReadResponse<IndexDto>(data, count, new Dictionary<string, string>(), new List<string>());
        }

        public PurchasingMemoDetailTextileDto Read(int id)
        {
            var model = _dbContext.PurchasingMemoDetailTextiles.FirstOrDefault(entity => entity.Id == id);


            if (model != null)
            {
                var memoIsCreated = _dbContext.PurchasingMemoTextiles.Any(entity => entity.MemoDetailId == model.Id);
                var result = new PurchasingMemoDetailTextileDto(model.Date, new DivisionDto(model.DivisionId, model.DivisionCode, model.DivisionName), new CurrencyDto(model.CurrencyId, model.CurrencyCode, model.CurrencyRate), model.SupplierIsImport, model.Type, new List<FormItemDto>(), new List<FormDetailDto>(), model.Remark, model.Id, model.DocumentNo, memoIsCreated);

                if (model.Type == PurchasingMemoType.Disposition)
                {
                    var items = _dbContext.PurchasingMemoDetailTextileItems.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id).ToList();
                    var itemIds = items.Select(item => item.Id).ToList();
                    var details = _dbContext.PurchasingMemoDetailTextileDetails.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id && itemIds.Contains(entity.PurchasingMemoDetailTextileItemId)).ToList();
                    var detailIds = details.Select(detail => detail.Id).ToList();
                    var unitReceiptNotes = _dbContext.PurchasingMemoDetailTextileUnitReceiptNotes.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id && itemIds.Contains(entity.PurchasingMemoDetailTextileItemId) && detailIds.Contains(entity.PurchasingMemoDetailTextileDetailId)).ToList();

                    foreach (var item in items)
                    {
                        var itemDetails = details.Where(detail => detail.PurchasingMemoDetailTextileItemId == item.Id).ToList();
                        var disposition = new DispositionDto(item.DispositionId, item.DispositionNo, item.DispositionDate, new List<FormDetailDto>());

                        foreach (var itemDetail in itemDetails)
                        {
                            var detailUnitReceiptNotes = unitReceiptNotes.Where(unitReceiptNote => unitReceiptNote.PurchasingMemoDetailTextileDetailId == itemDetail.Id).Select(unitReceiptNote => new UnitReceiptNoteDto(unitReceiptNote.UnitReceiptNoteId, unitReceiptNote.UnitReceiptNoteNo, unitReceiptNote.UnitReceiptNoteDate)).ToList();
                            var expenditure = new ExpenditureDto(itemDetail.ExpenditureId, itemDetail.ExpenditureNo, itemDetail.ExpenditureDate);
                            var supplier = new SupplierDto(itemDetail.SupplierId, itemDetail.SupplierCode, itemDetail.SupplierName);
                            var unitPaymentOrder = new UnitPaymentOrderDto(itemDetail.UnitPaymentOrderId, itemDetail.UnitPaymentOrderNo, itemDetail.UnitPaymentOrderDate);
                            disposition.Details.Add(new FormDetailDto(expenditure, supplier, itemDetail.Remark, unitPaymentOrder, detailUnitReceiptNotes, itemDetail.PurchaseAmountCurrency, itemDetail.PurchaseAmount, itemDetail.PaymentAmountCurrency, itemDetail.PaymentAmount));
                        }

                        result.Items.Add(new FormItemDto(disposition));
                    }
                }
                else
                {
                    var details = _dbContext.PurchasingMemoDetailTextileDetails.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id).ToList();
                    var detailIds = details.Select(detail => detail.Id).ToList();
                    var unitReceiptNotes = _dbContext.PurchasingMemoDetailTextileUnitReceiptNotes.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id && detailIds.Contains(entity.PurchasingMemoDetailTextileDetailId)).ToList();


                    foreach (var itemDetail in details)
                    {
                        var detailUnitReceiptNotes = unitReceiptNotes.Where(unitReceiptNote => unitReceiptNote.PurchasingMemoDetailTextileDetailId == itemDetail.Id).Select(unitReceiptNote => new UnitReceiptNoteDto(unitReceiptNote.UnitReceiptNoteId, unitReceiptNote.UnitReceiptNoteNo, unitReceiptNote.UnitReceiptNoteDate)).ToList();
                        var expenditure = new ExpenditureDto(itemDetail.ExpenditureId, itemDetail.ExpenditureNo, itemDetail.ExpenditureDate);
                        var supplier = new SupplierDto(itemDetail.SupplierId, itemDetail.SupplierCode, itemDetail.SupplierName);
                        var unitPaymentOrder = new UnitPaymentOrderDto(itemDetail.UnitPaymentOrderId, itemDetail.UnitPaymentOrderNo, itemDetail.UnitPaymentOrderDate);
                        result.Details.Add(new FormDetailDto(expenditure, supplier, itemDetail.Remark, unitPaymentOrder, detailUnitReceiptNotes, itemDetail.PurchaseAmountCurrency, itemDetail.PurchaseAmount, itemDetail.PaymentAmountCurrency, itemDetail.PaymentAmount));
                    }

                }

                return result;
            }
            else
                return null;

        }

        public int Update(int id, FormDto form)
        {
            var updatedId = 0;
            var model = _dbContext.PurchasingMemoDetailTextiles.FirstOrDefault(entity => entity.Id == id);

            if (model != null)
            {
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
                _dbContext.PurchasingMemoDetailTextiles.Update(model);

                var items = _dbContext.PurchasingMemoDetailTextileItems.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id).ToList();
                var details = _dbContext.PurchasingMemoDetailTextileDetails.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id).ToList();
                var unitReceiptNotes = _dbContext.PurchasingMemoDetailTextileUnitReceiptNotes.Where(entity => entity.PurchasingMemoDetailTextileId == model.Id).ToList();

                items = items.Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                }).ToList();
                _dbContext.PurchasingMemoDetailTextileItems.UpdateRange(items);

                details = details.Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                }).ToList();
                _dbContext.PurchasingMemoDetailTextileDetails.UpdateRange(details);

                unitReceiptNotes = unitReceiptNotes.Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                }).ToList();
                _dbContext.PurchasingMemoDetailTextileUnitReceiptNotes.UpdateRange(unitReceiptNotes);

                _dbContext.SaveChanges();

                if (form.Type == PurchasingMemoType.Disposition)
                {
                    foreach (var item in form.Items)
                    {
                        var itemModel = new PurchasingMemoDetailTextileItemModel(item.Disposition.Id, item.Disposition.DocumentNo, item.Disposition.Date, model.Id);
                        EntityExtension.FlagForCreate(itemModel, _identityService.Username, UserAgent);
                        _dbContext.PurchasingMemoDetailTextileItems.Add(itemModel);
                        _dbContext.SaveChanges();

                        foreach (var detail in item.Disposition.Details)
                        {
                            var detailModel = new PurchasingMemoDetailTextileDetailModel(model.Id, itemModel.Id, detail.Expenditure.Id, detail.Expenditure.DocumentNo, detail.Expenditure.Date, detail.Supplier.Id, detail.Supplier.Code, detail.Supplier.Name, detail.Remark, detail.UnitPaymentOrder.Id, detail.UnitPaymentOrder.UnitPaymentOrderNo, detail.UnitPaymentOrder.UnitPaymentOrderDate, detail.PurchaseAmountCurrency, detail.PurchaseAmount, detail.PaymentAmountCurrency, detail.PaymentAmount);
                            EntityExtension.FlagForCreate(detailModel, _identityService.Username, UserAgent);
                            _dbContext.PurchasingMemoDetailTextileDetails.Add(detailModel);
                            _dbContext.SaveChanges();

                            foreach (var unitReceiptNote in detail.UnitReceiptNotes)
                            {
                                var unitReceiptNoteModel = new PurchasingMemoDetailTextileUnitReceiptNoteModel(model.Id, itemModel.Id, detailModel.Id, unitReceiptNote.Id, unitReceiptNote.UnitReceiptNoteNo, unitReceiptNote.UnitReceiptNoteDate);
                                EntityExtension.FlagForCreate(unitReceiptNoteModel, _identityService.Username, UserAgent);
                                _dbContext.PurchasingMemoDetailTextileUnitReceiptNotes.Add(unitReceiptNoteModel);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    foreach (var detail in form.Details)
                    {
                        var detailModel = new PurchasingMemoDetailTextileDetailModel(model.Id, 0, detail.Expenditure.Id, detail.Expenditure.DocumentNo, detail.Expenditure.Date, detail.Supplier.Id, detail.Supplier.Code, detail.Supplier.Name, detail.Remark, detail.UnitPaymentOrder.Id, detail.UnitPaymentOrder.UnitPaymentOrderNo, detail.UnitPaymentOrder.UnitPaymentOrderDate, detail.PurchaseAmountCurrency, detail.PurchaseAmount, detail.PaymentAmountCurrency, detail.PaymentAmount);
                        EntityExtension.FlagForCreate(detailModel, _identityService.Username, UserAgent);
                        _dbContext.PurchasingMemoDetailTextileDetails.Add(detailModel);
                        _dbContext.SaveChanges();

                        foreach (var unitReceiptNote in detail.UnitReceiptNotes)
                        {
                            var unitReceiptNoteModel = new PurchasingMemoDetailTextileUnitReceiptNoteModel(model.Id, 0, detailModel.Id, unitReceiptNote.Id, unitReceiptNote.UnitReceiptNoteNo, unitReceiptNote.UnitReceiptNoteDate);
                            EntityExtension.FlagForCreate(unitReceiptNoteModel, _identityService.Username, UserAgent);
                            _dbContext.PurchasingMemoDetailTextileUnitReceiptNotes.Add(unitReceiptNoteModel);
                            _dbContext.SaveChanges();
                        }
                    }
                }

                updatedId = model.Id;
            }

            return updatedId;
        }

        public List<FormItemDto> ReadDispositions(string keyword, int divisionId, bool supplierIsImport, string currencyCode)
        {
            var dispositionQuery = _dbContext.PaymentDispositionNotes.Where(entity => entity.SupplierImport == supplierIsImport);

            if (!string.IsNullOrWhiteSpace(currencyCode))
                dispositionQuery = dispositionQuery.Where(entity => entity.CurrencyCode == currencyCode);

            var paymentIds = dispositionQuery.Select(entity => entity.Id).Distinct().ToList();
            var dispositionItemQuery = _dbContext.PaymentDispositionNoteItems.Where(entity => paymentIds.Contains(entity.PaymentDispositionNoteId));

            if (divisionId > 0)
                dispositionItemQuery = dispositionItemQuery.Where(entity => entity.DivisionId == divisionId);

            var query = dispositionItemQuery.Select(entity => new { entity.DispositionId, entity.DispositionNo, entity.DispositionDate }).Distinct().AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(entity => entity.DispositionNo.Contains(keyword));
            }


            var queryResult = query.Take(10).ToList();
            var dispositionIds = queryResult.Select(element => element.DispositionId).ToList();
            var paymentDispositionItems = _dbContext.PaymentDispositionNoteItems.Where(entity => dispositionIds.Contains(entity.DispositionId)).ToList();
            var paymentDispositionItemIds = paymentDispositionItems.Select(entity => entity.Id).ToList();
            var paymentDispositionIds = paymentDispositionItems.Select(entity => entity.PaymentDispositionNoteId).Distinct().ToList();
            var paymentDispositions = _dbContext.PaymentDispositionNotes.Where(entity => paymentDispositionIds.Contains(entity.Id)).ToList();
            var paymentDispositionDetails = _dbContext.PaymentDispositionNoteDetails.Where(entity => paymentDispositionItemIds.Contains(entity.PaymentDispositionNoteItemId)).ToList();
            var purchasingDispositionExpeditionIds = paymentDispositionItems.Select(element => element.PurchasingDispositionExpeditionId).ToList();
            var purchasingDispositionExpeditions = _dbContext.PurchasingDispositionExpeditions.Where(entity => purchasingDispositionExpeditionIds.Contains(entity.Id)).ToList();
            var purchasingDispositionExpeditionItems = _dbContext.PurchasingDispositionExpeditionItems.Where(entity => purchasingDispositionExpeditionIds.Contains(entity.PurchasingDispositionExpeditionId)).ToList();

            var result = new List<FormItemDto>();
            foreach (var item in queryResult)
            {
                var disposition = new DispositionDto(item.DispositionId, item.DispositionNo, item.DispositionDate, new List<FormDetailDto>());

                var itemPaymentDispositionNoteIds = paymentDispositionItems.Where(element => element.DispositionId == item.DispositionId).Select(element => element.PaymentDispositionNoteId).ToList();
                var itemPaymentDispositionNotes = paymentDispositions.Where(element => itemPaymentDispositionNoteIds.Contains(element.Id)).ToList();

                foreach (var itemPaymentDispositionNote in itemPaymentDispositionNotes)
                {
                    var expenditure = new ExpenditureDto(itemPaymentDispositionNote.Id, itemPaymentDispositionNote.PaymentDispositionNo, itemPaymentDispositionNote.PaymentDate);
                    var supplier = new SupplierDto(itemPaymentDispositionNote.SupplierId, itemPaymentDispositionNote.SupplierCode, itemPaymentDispositionNote.SupplierName);
                    var itemPaymentDispositionNoteItems = paymentDispositionItems.Where(element => element.PaymentDispositionNoteId == itemPaymentDispositionNote.Id).ToList();


                    foreach (var itemPaymentDispositionNoteItem in itemPaymentDispositionNoteItems)
                    {
                        var division = new DivisionDto(itemPaymentDispositionNoteItem.DivisionId, itemPaymentDispositionNoteItem.DivisionCode, itemPaymentDispositionNoteItem.DivisionName);

                        disposition.Details.Add(new FormDetailDto(expenditure, supplier, "", new UnitPaymentOrderDto(0, "", DateTimeOffset.MinValue), new List<UnitReceiptNoteDto>(), 0, 0, 0, 0));
                    }
                }

                result.Add(new FormItemDto(disposition));

            }

            return result;
        }

        public List<AutoCompleteDto> Read(string keyword)
        {
            var query = _dbContext.PurchasingMemoDetailTextiles.Where(entity => entity.DocumentNo.Contains(keyword));
            
            var result = query.Take(10).Select(entity => new AutoCompleteDto(entity.Id, entity.DocumentNo, entity.Date, entity.CurrencyCode, entity.CurrencyId, entity.CurrencyRate)).ToList();
            var ids = result.Select(element => element.Id).ToList();
            var details = _dbContext.PurchasingMemoDetailTextileDetails.Where(entity => ids.Contains(entity.PurchasingMemoDetailTextileId)).ToList();

            var coa = _dbContext.ChartsOfAccounts.Where(entity => entity.Code.Contains("14")).FirstOrDefault();

            if (coa != null)
                result = result.Select(element =>
                {
                    var amount = details.Where(detail => detail.PurchasingMemoDetailTextileId == element.Id).Sum(detail => detail.PurchaseAmount);
                    element = new AutoCompleteDto(element.Id, element.DocumentNo, element.Date, element.Currency, new List<PurchasingMemoTextile.FormItemDto>() { new PurchasingMemoTextile.FormItemDto(new PurchasingMemoTextile.ChartOfAccountDto(coa.Id, coa.Code, coa.Name), amount, amount) });
                    return element;
                }).ToList();

            return result;
        }
    }
}
