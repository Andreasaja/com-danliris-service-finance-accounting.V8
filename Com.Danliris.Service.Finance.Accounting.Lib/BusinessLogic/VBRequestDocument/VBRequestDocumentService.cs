using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class VBRequestDocumentService : IVBRequestDocumentService
    {
        private const string UserAgent = "finance-service";
        public readonly FinanceDbContext _dbContext;

        private readonly IIdentityService _identityService;
        private readonly IAutoJournalService _autoJournalTransactionService;
        private readonly IJournalTransactionService _journalTransactionService;
        private readonly IAutoDailyBankTransactionService _dailyBankTransactionService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDPPVATBankExpenditureNoteService _dppVatBankExpenditureNoteService;

        public VBRequestDocumentService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _autoJournalTransactionService = serviceProvider.GetService<IAutoJournalService>();
            _journalTransactionService = serviceProvider.GetService<IJournalTransactionService>();
            _dailyBankTransactionService = serviceProvider.GetService<IAutoDailyBankTransactionService>();
            _dppVatBankExpenditureNoteService = serviceProvider.GetService<IDPPVATBankExpenditureNoteService>();
            _serviceProvider = serviceProvider;
        }

        private Tuple<string, int> GetDocumentNo(VBRequestDocumentNonPOFormDto form, VBRequestDocumentModel existingData)
        {
            var now = form.Date.GetValueOrDefault().AddHours(_identityService.TimezoneOffset);
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            //var unit = model.UnitCode.ToString().Split(" - ");

            var unitCode = "T";
            if (form.SuppliantUnit.Division.Name.ToUpper() == "GARMENT")
                unitCode = "G";

            if (form.IsInklaring) unitCode += "I";

            var documentNo = $"VB-{unitCode}-{month}{year}-";

            var index = 1;

            if (existingData != null)
            {
                index = existingData.Index + 1;
            }

            documentNo += string.Format("{0:000}", index);

            //var countSameDocumentNo = _dbContext.VBRequestDocuments.IgnoreQueryFilters().Where(a => a.Date.AddHours(_identityService.TimezoneOffset).Month == form.Date.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Month).Count();

            //if (countSameDocumentNo >= 0)
            //{
            //    countSameDocumentNo += 1;

            //    documentNo += string.Format("{0:000}", countSameDocumentNo);
            //}

            return new Tuple<string, int>(documentNo, index);
        }

        private Tuple<string, int> GetDocumentNo(VBRequestDocumentWithPOFormDto form, VBRequestDocumentModel existingData)
        {
            var now = form.Date.GetValueOrDefault().AddHours(_identityService.TimezoneOffset);
            var year = now.ToString("yy");
            var month = now.ToString("MM");


            //var unit = model.UnitCode.ToString().Split(" - ");

            var unitCode = "T";
            if (form.SuppliantUnit.Division.Name.ToUpper() == "GARMENT")
                unitCode = "G";

            var documentNo = $"VB-{unitCode}-{month}{year}-";

            var index = 1;

            if (existingData != null)
            {
                index = existingData.Index + 1;
            }

            documentNo += string.Format("{0:000}", index);

            return new Tuple<string, int>(documentNo, index);
        }

        private string GetDocumentUnitCode(string division, bool isInklaring)
        {
            var unitCode = "T";
            if (division.ToUpper() == "GARMENT")
                unitCode = "G";

            unitCode += (isInklaring) ? "I" : null;

            return $"VB-{unitCode}-";
        }

        //public int CreateNonPO(VBRequestDocumentNonPOFormDto form)
        public async Task<int> CreateNonPO(VBRequestDocumentNonPOFormDto form)
        {
            var internalTransaction = _dbContext.Database.CurrentTransaction == null;
            var transaction = !internalTransaction ? _dbContext.Database.CurrentTransaction : _dbContext.Database.BeginTransaction();

            try
            {
                var unitCode = GetDocumentUnitCode(form.SuppliantUnit.Division.Name.ToUpper(), form.IsInklaring);
                var existingData = _dbContext.VBRequestDocuments.Where(a => a.Date.AddHours(_identityService.TimezoneOffset).Month == form.Date.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Month && a.Date.AddHours(_identityService.TimezoneOffset).Year == form.Date.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Year && a.DocumentNo.StartsWith(unitCode)).OrderByDescending(s => s.Id).FirstOrDefault();
                var documentNo = GetDocumentNo(form, existingData);
                var model = new VBRequestDocumentModel(
                    documentNo.Item1,
                    form.Date.GetValueOrDefault(),
                    form.RealizationEstimationDate.GetValueOrDefault(),
                    form.SuppliantUnit.Id.GetValueOrDefault(),
                    form.SuppliantUnit.Code,
                    form.SuppliantUnit.Name,
                    form.SuppliantUnit.Division.Id.GetValueOrDefault(),
                    form.SuppliantUnit.Division.Code,
                    form.SuppliantUnit.Division.Name,
                    form.Currency.Id.GetValueOrDefault(),
                    form.Currency.Code,
                    form.Currency.Symbol,
                    form.Currency.Description,
                    form.Currency.Rate.GetValueOrDefault(),
                    form.Purpose,
                    form.Amount.GetValueOrDefault(),
                    false,
                    false,
                    VBType.NonPO,
                    documentNo.Item2,
                    form.IsInklaring,
                    form.NoBL,
                    form.NoPO,
                    null
                    );

                model.FlagForCreate(_identityService.Username, UserAgent);
                _dbContext.VBRequestDocuments.Add(model);
                await _dbContext.SaveChangesAsync();

                var items = AddNonPOItems(model.Id, form.Items);

                _dbContext.VBRequestDocumentItems.AddRange(items);
                await _dbContext.SaveChangesAsync();

                if (internalTransaction)
                    transaction.Commit();

                return model.Id;
            }
            catch (Exception ex)
            {
                if (internalTransaction)
                    transaction.Rollback();
                throw ex;
            }

        }

        private List<VBRequestDocumentItemModel> AddNonPOItems(int id, List<VBRequestDocumentNonPOItemFormDto> items)
        {
            var models = items.Select(element =>
            {
                var result = new VBRequestDocumentItemModel(
                    id,
                    element.Unit.Id.GetValueOrDefault(),
                    element.Unit.Name,
                    element.Unit.Code,
                    element.Unit.Division == null ? 0 : element.Unit.Division.Id.GetValueOrDefault(),
                    element.Unit.Division?.Name,
                    element.Unit.Division?.Code,
                    element.IsSelected,
                    element.Unit.VBDocumentLayoutOrder.GetValueOrDefault()
                    );

                result.FlagForCreate(_identityService.Username, UserAgent);
                return result;
            }).ToList();

            return models;
        }

        public int CreateWithPO(VBRequestDocumentWithPOFormDto form)
        {
            var unitCode = GetDocumentUnitCode(form.SuppliantUnit.Division.Name.ToUpper(), form.IsInklaring);
            var existingData = _dbContext.VBRequestDocuments.Where(a => a.Date.AddHours(_identityService.TimezoneOffset).Month == form.Date.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Month && a.DocumentNo.StartsWith(unitCode) && a.Date.AddHours(_identityService.TimezoneOffset).Year == form.Date.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Year).OrderByDescending(s => s.Id).FirstOrDefault();
            var documentNo = GetDocumentNo(form, existingData);

            var model = new VBRequestDocumentModel(
                documentNo.Item1,
                form.Date.GetValueOrDefault(),
                form.RealizationEstimationDate.GetValueOrDefault(),
                form.SuppliantUnit.Id.GetValueOrDefault(),
                form.SuppliantUnit.Code,
                form.SuppliantUnit.Name,
                form.SuppliantUnit.Division.Id.GetValueOrDefault(),
                form.SuppliantUnit.Division.Code,
                form.SuppliantUnit.Division.Name,
                form.Currency.Id.GetValueOrDefault(),
                form.Currency.Code,
                form.Currency.Symbol,
                form.Currency.Description,
                form.Currency.Rate.GetValueOrDefault(),
                form.Purpose,
                form.Amount.GetValueOrDefault(),
                false,
                false,
                VBType.WithPO,
                documentNo.Item2,
                form.IsInklaring,
                null, // NoBL
                null, // NoPO
                form.TypePurchasing
                );

            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            _dbContext.VBRequestDocuments.Add(model);
            _dbContext.SaveChanges();

            AddItems(model.Id, form.Items);

            return model.Id;
        }

        private void AddItems(int documentId, List<VBRequestDocumentWithPOItemFormDto> items)
        {
            foreach (var item in items)
            {
                var epoDetailModel = new VBRequestDocumentEPODetailModel(
                    documentId,
                    item.PurchaseOrderExternal.Id.GetValueOrDefault(),
                    item.PurchaseOrderExternal.No,
                    string.Empty
                    );

                EntityExtension.FlagForCreate(epoDetailModel, _identityService.Username, UserAgent);
                _dbContext.VBRequestDocumentEPODetails.Add(epoDetailModel);
                _dbContext.SaveChanges();

                AddDetails(documentId, epoDetailModel.Id, epoDetailModel.EPOId, item.PurchaseOrderExternal.Items);
            }
        }

        private void AddDetails(int documentId, int documentItemId, int epoId, List<PurchaseOrderExternalItem> details)
        {
            var models = details.Select(element =>
            {
                var result = new VBRequestDocumentItemModel(
                    documentId,
                    documentItemId,
                    element.Unit.Id.GetValueOrDefault(),
                    element.Unit.Name,
                    element.Unit.Code,
                    element.Unit.Division.Id.GetValueOrDefault(),
                    element.Unit.Division.Name,
                    element.Unit.Division.Code,
                    epoId,
                    element.UseIncomeTax,
                    element.IncomeTax.Id.GetValueOrDefault(),
                    element.IncomeTax.Name,
                    element.IncomeTax.Rate.GetValueOrDefault(),
                    element.IncomeTaxBy,
                    element.Product.Id.GetValueOrDefault(),
                    element.Product.Code,
                    element.Product.Name,
                    element.DefaultQuantity.GetValueOrDefault(),
                    element.Product.UOM.Id.GetValueOrDefault(),
                    element.Product.UOM.Unit,
                    element.DealQuantity.GetValueOrDefault(),
                    element.DealUOM.Id.GetValueOrDefault(),
                    element.DealUOM.Unit,
                    element.Conversion.GetValueOrDefault(),
                    element.Price.GetValueOrDefault(),
                    element.UseVat,
                    element.VatTax.Id,
                    element.VatTax.Rate
                    );

                EntityExtension.FlagForCreate(result, _identityService.Username, UserAgent);
                return result;
            }).ToList();

            _dbContext.VBRequestDocumentItems.AddRange(models);
            _dbContext.SaveChanges();
        }

        private void DeleteItemNonPO(int id)
        {
            var items = _dbContext.VBRequestDocumentItems.Where(s => s.VBRequestDocumentId == id).OrderBy(s => s.VBDocumentLayoutOrder).ToList();

            foreach (var item in items)
            {
                item.FlagForDelete(_identityService.Username, UserAgent);
            }
        }

        public Task<int> DeleteNonPO(int id)
        {
            var data = _dbContext.VBRequestDocuments.FirstOrDefault(s => s.Id == id);
            data.FlagForDelete(_identityService.Username, UserAgent);

            DeleteItemNonPO(id);

            return _dbContext.SaveChangesAsync();
        }

        public int DeleteWithPO(int id)
        {
            var model = _dbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == id);
            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);
            _dbContext.Update(model);

            var items = _dbContext.VBRequestDocumentItems.Where(entity => entity.VBRequestDocumentId == model.Id).ToList();
            items = items.Select(element =>
            {
                EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                return element;
            }).ToList();

            var itemIds = items.Select(element => element.Id).ToList();
            var details = _dbContext.VBRequestDocumentEPODetails.Where(entity => itemIds.Contains(entity.VBRequestDocumentItemId)).ToList();
            details = details.Select(element =>
            {
                EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                return element;
            }).ToList();

            _dbContext.SaveChanges();

            return model.Id;
        }

        public ReadResponse<VBRequestDocumentModel> Get(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.Set<VBRequestDocumentModel>().AsQueryable();

            List<string> searchAttributes = new List<string>()
            {
                "DocumentNo", "SuppliantUnitName"
            };

            query = QueryHelper<VBRequestDocumentModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<VBRequestDocumentModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VBRequestDocumentModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VBRequestDocumentModel>(query, page - 1, size);
            var data = pageable.Data.ToList();

            int TotalData = pageable.TotalCount;

            return new ReadResponse<VBRequestDocumentModel>(data, TotalData, orderDictionary, new List<string>());
        }
        
        public ReadResponse<VBRequestDocumentModel> GetByUser(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.Set<VBRequestDocumentModel>().AsQueryable();

            query = query.Where(entity => entity.CreatedBy == _identityService.Username);

            List<string> searchAttributes = new List<string>()
            {
                "DocumentNo", "SuppliantUnitName"
            };

            query = QueryHelper<VBRequestDocumentModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<VBRequestDocumentModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VBRequestDocumentModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VBRequestDocumentModel>(query, page - 1, size);
            var data = pageable.Data.ToList();

            int TotalData = pageable.TotalCount;

            return new ReadResponse<VBRequestDocumentModel>(data, TotalData, orderDictionary, new List<string>());
        }

        private VBRequestDocumentNonPODto MapToNonPODTO(VBRequestDocumentModel model, List<VBRequestDocumentItemModel> items)
        {
            return new VBRequestDocumentNonPODto()
            {
                Active = model.Active,
                Amount = model.Amount,
                DocumentNo = model.DocumentNo,
                IsApproved = model.ApprovalStatus == ApprovalStatus.Approved,
                ApprovalStatus = model.ApprovalStatus.ToString(),
                CreatedAgent = model.CreatedAgent,
                Items = items.Select(s => new VBRequestDocumentNonPOItemDto()
                {
                    IsSelected = s.IsSelected,
                    Unit = new UnitDto()
                    {
                        Code = s.UnitCode,
                        Division = new DivisionDto()
                        {
                            Code = s.DivisionCode,
                            Id = s.DivisionId,
                            Name = s.DivisionName
                        },
                        Name = s.UnitName,
                        Id = s.UnitId,
                        VBDocumentLayoutOrder = s.VBDocumentLayoutOrder
                    },
                    Id = s.Id,
                    Active = s.Active,
                    CreatedAgent = s.CreatedAgent,
                    CreatedBy = s.CreatedBy,
                    CreatedUtc = s.CreatedUtc,
                    IsDeleted = s.IsDeleted,
                    LastModifiedAgent = s.LastModifiedAgent,
                    LastModifiedBy = s.LastModifiedBy,
                    LastModifiedUtc = s.LastModifiedUtc
                }).ToList(),
                LastModifiedUtc = model.LastModifiedUtc,
                LastModifiedBy = model.LastModifiedBy,
                LastModifiedAgent = model.LastModifiedAgent,
                IsDeleted = model.IsDeleted,
                CreatedUtc = model.CreatedUtc,
                CreatedBy = model.CreatedBy,
                Currency = new CurrencyDto()
                {
                    Code = model.CurrencyCode,
                    Description = model.CurrencyDescription,
                    Id = model.CurrencyId,
                    Rate = model.CurrencyRate,
                    Symbol = model.CurrencySymbol
                },
                Id = model.Id,
                Date = model.Date,
                Purpose = model.Purpose,
                RealizationEstimationDate = model.RealizationEstimationDate,
                SuppliantUnit = new UnitDto()
                {
                    Id = model.SuppliantUnitId,
                    Code = model.SuppliantUnitCode,
                    Name = model.SuppliantUnitName,
                    Division = new DivisionDto()
                    {
                        Name = model.SuppliantDivisionName,
                        Code = model.SuppliantDivisionCode,
                        Id = model.SuppliantDivisionId
                    }
                },
                IsInklaring = model.IsInklaring,
                NoBL = model.NoBL,
                NoPO = model.NoPO
            };
        }

        public async Task<VBRequestDocumentNonPODto> GetNonPOById(int id)
        {
            var data = await _dbContext.VBRequestDocuments.FirstOrDefaultAsync(s => s.Id == id);

            if (data == null)
                return null;

            var items = _dbContext.VBRequestDocumentItems.Where(s => s.VBRequestDocumentId == id).OrderBy(s => s.VBDocumentLayoutOrder).ToList();

            var result = MapToNonPODTO(data, items);

            return result;
        }

        public VBRequestDocumentWithPODto GetWithPOById(int id)
        {
            var model = _dbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == id);
            var epoDetails = _dbContext.VBRequestDocumentEPODetails.Where(entity => entity.VBRequestDocumentId == id).ToList();

            var result = new VBRequestDocumentWithPODto()
            {
                Amount = model.Amount,
                Currency = new CurrencyDto()
                {
                    Code = model.CurrencyCode,
                    Description = model.CurrencyDescription,
                    Id = model.CurrencyId,
                    Rate = model.CurrencyRate,
                    Symbol = model.CurrencySymbol
                },
                Date = model.Date,
                RealizationEstimationDate = model.RealizationEstimationDate,
                SuppliantUnit = new UnitDto()
                {
                    Id = model.SuppliantUnitId,
                    Code = model.SuppliantUnitCode,
                    Name = model.SuppliantUnitName,
                    Division = new DivisionDto()
                    {
                        Code = model.SuppliantDivisionCode,
                        Id = model.SuppliantDivisionId,
                        Name = model.SuppliantDivisionName
                    }
                },
                DocumentNo = model.DocumentNo,
                Id = model.Id,
                IsApproved = model.ApprovalStatus == ApprovalStatus.Approved,
                Purpose = model.Purpose,
                CreatedBy = model.CreatedBy,
                IsInklaring = model.IsInklaring,
                ApprovalStatus = model.ApprovalStatus.ToString(),
                TypePurchasing = model.TypePurchasing,
                Items = epoDetails.Select(epoDetail =>
                {
                    var details = _dbContext.VBRequestDocumentItems.Where(entity => entity.VBRequestDocumentEPODetailId == epoDetail.Id).ToList();
                    var elementResult = new VBRequestDocumentWithPOItemDto()
                    {
                        Id = epoDetail.Id,
                        PurchaseOrderExternal = new PurchaseOrderExternal()
                        {
                            Id = epoDetail.EPOId,
                            No = epoDetail.EPONo,
                            Items = details.Select(entity => new PurchaseOrderExternalItem()
                            {
                                Conversion = entity.Conversion,
                                DealQuantity = entity.DealQuantity,
                                DealUOM = new UnitOfMeasurement()
                                {
                                    Unit = entity.DealUOMUnit,
                                    Id = entity.DealUOMId
                                },
                                DefaultQuantity = entity.DefaultQuantity,
                                Id = entity.Id,
                                Price = entity.Price,
                                Product = new Product()
                                {
                                    Code = entity.ProductCode,
                                    Name = entity.ProductName,
                                    UOM = new UnitOfMeasurement()
                                    {
                                        Unit = entity.DefaultUOMUnit,
                                        Id = entity.DefaultUOMId
                                    },
                                    Id = entity.ProductId
                                },
                                UseVat = entity.UseVat,
                                IncomeTax = new IncomeTaxDto()
                                {
                                    Id = entity.IncomeTaxId,
                                    Name = entity.IncomeTaxName,
                                    Rate = entity.IncomeTaxRate
                                },
                                VatTax = new VatTaxDto()
                                { 
                                   Id = entity.VatId,
                                   Rate = entity.VatRate,
                                },
                                IncomeTaxBy = entity.IncomeTaxBy,
                                Unit = new UnitDto()
                                {
                                    Id = entity.UnitId,
                                    Code = entity.UnitCode,
                                    Name = entity.UnitName,
                                    Division = new DivisionDto()
                                    {
                                        Id = entity.DivisionId,
                                        Code = entity.DivisionCode,
                                        Name = entity.DivisionName
                                    }
                                }
                            }).ToList()
                        }
                    };

                    return elementResult;
                }).ToList()
            };

            return result;
        }

        public Task<int> UpdateNonPO(int id, VBRequestDocumentNonPOFormDto form)
        {
            var data = _dbContext.VBRequestDocuments.FirstOrDefault(s => s.Id == id);

            data.SetDate(form.Date.GetValueOrDefault(), _identityService.Username, UserAgent);
            data.SetRealizationEstimationDate(form.RealizationEstimationDate.GetValueOrDefault(), _identityService.Username, UserAgent);
            data.SetCurrency(form.Currency.Id.GetValueOrDefault(), form.Currency.Code, form.Currency.Symbol, form.Currency.Rate.GetValueOrDefault(), form.Currency.Description, _identityService.Username, UserAgent);
            data.SetAmount(form.Amount.GetValueOrDefault(), _identityService.Username, UserAgent);
            data.SetPurpose(form.Purpose, _identityService.Username, UserAgent);
            if(data.IsInklaring) data.SetInklaring(form.NoBL, form.NoPO);

            EditNonPOItems(id, form.Items);
            data.FlagForUpdate(_identityService.Username, UserAgent);
            return _dbContext.SaveChangesAsync();
        }

        private void EditNonPOItems(int id, List<VBRequestDocumentNonPOItemFormDto> formItems)
        {
            var items = _dbContext.VBRequestDocumentItems.Where(s => s.VBRequestDocumentId == id).OrderBy(s => s.VBDocumentLayoutOrder).ToList();
            foreach (var item in items)
            {
                var formItem = formItems.FirstOrDefault(s => s.Id == item.Id);
                if (formItem == null)
                {
                    item.FlagForDelete(_identityService.Username, UserAgent);
                }
                else
                {
                    item.SetIsSelected(formItem.IsSelected, _identityService.Username, UserAgent);
                    item.SetUnit(formItem.Unit.Id.GetValueOrDefault(), formItem.Unit.Name, formItem.Unit.Code, _identityService.Username, UserAgent);

                    if (formItem.Unit.Division == null)
                    {
                        item.SetDivision(0, null, null, _identityService.Username, UserAgent);

                    }
                    else
                    {
                        item.SetDivision(formItem.Unit.Division.Id.GetValueOrDefault(), formItem.Unit.Division.Name, formItem.Unit.Division.Code, _identityService.Username, UserAgent);
                    }

                    //item.SetVBDocumentLayoutOrder(formItem.Unit.VBDocumentLayoutOrder.GetValueOrDefault(), _identityService.Username, UserAgent);
                }
            }

            var newItems = formItems.Where(s => s.Id == 0).Select(element =>
            {
                var result = new VBRequestDocumentItemModel(
                    id,
                    element.Unit.Id.GetValueOrDefault(),
                    element.Unit.Name,
                    element.Unit.Code,
                    element.Unit.Division == null ? 0 : element.Unit.Division.Id.GetValueOrDefault(),
                    element.Unit.Division?.Name,
                    element.Unit.Division?.Code,
                    element.IsSelected,
                    element.Unit.VBDocumentLayoutOrder.GetValueOrDefault()
                    );

                result.FlagForCreate(_identityService.Username, UserAgent);
                return result;
            }).ToList();

            _dbContext.VBRequestDocumentItems.AddRange(newItems);

            //return _dbContext.SaveChangesAsync();
        }

        public int UpdateWithPO(int id, VBRequestDocumentWithPOFormDto form)
        {
            var header = _dbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == id);
            header.UpdateFromForm(form);
            EntityExtension.FlagForUpdate(header, _identityService.Username, UserAgent);
            _dbContext.Update(header);
            _dbContext.SaveChanges();

            UpdateWithPOEPODetail(id, form.Items);
            return id;
        }

        private void UpdateWithPOEPODetail(int documentId, List<VBRequestDocumentWithPOItemFormDto> items)
        {
            var epoDetails = _dbContext.VBRequestDocumentEPODetails.Where(entity => entity.VBRequestDocumentId == documentId).ToList();

            foreach (var epoDetail in epoDetails)
            {
                var item = items.FirstOrDefault(element => element.Id.GetValueOrDefault() == epoDetail.Id);

                if (item == null)
                {
                    EntityExtension.FlagForDelete(epoDetail, _identityService.Username, UserAgent);

                    var documentItems = _dbContext.VBRequestDocumentItems.Where(entity => entity.VBRequestDocumentEPODetailId == epoDetail.Id).ToList();
                    documentItems = documentItems.Select(element =>
                    {
                        EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                        return element;
                    }).ToList();


                    _dbContext.VBRequestDocumentEPODetails.Update(epoDetail);
                    _dbContext.UpdateRange(documentItems);
                }
                else
                {
                    epoDetail.UpdateFromForm(item);
                    _dbContext.VBRequestDocumentEPODetails.Update(epoDetail);

                    var documentItems = _dbContext.VBRequestDocumentItems.Where(entity => entity.VBRequestDocumentEPODetailId == epoDetail.Id).ToList();
                    documentItems = documentItems.Select(element =>
                    {
                        EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                        return element;
                    }).ToList();
                    AddDetails(documentId, epoDetail.Id, epoDetail.EPOId, item.PurchaseOrderExternal.Items);
                }
            }

            _dbContext.SaveChanges();
            var newItems = items.Where(element => element.Id.GetValueOrDefault() <= 0).ToList();
            AddItems(documentId, newItems);
        }

        public List<VBRequestDocumentModel> GetNotApprovedData(int type, int vbId, int suppliantUnitId, DateTime? date, string order)
        {
            var enumType = (VBType)type;
            var offset = _identityService.TimezoneOffset;
            var query = _dbContext.VBRequestDocuments.Where(s => s.ApprovalStatus == ApprovalStatus.Draft && s.Type == enumType);

            if (vbId != 0)
            {
                query = query.Where(s => s.Id == vbId);
            }

            if (suppliantUnitId != 0)
            {
                query = query.Where(s => s.SuppliantUnitId == suppliantUnitId);
            }

            if (date.HasValue)
            {
                query = query.Where(s => s.Date.ToOffset(new TimeSpan(offset, 0, 0)).Date == date.Value.Date);
            }

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VBRequestDocumentModel>.Order(query, orderDictionary);

            return query.ToList();
        }

        public async Task<int> ApprovalData(ApprovalVBFormDto data)
        {
            var vbDocuments = _dbContext.VBRequestDocuments.Where(s => data.Ids.Contains(s.Id));

            var vbRequestIdJournals = new List<int>();
            var vbRequestsList = new List<ApprovalVBAutoJournalDto>();
            foreach (var item in vbDocuments)
            {
                item.SetIsApproved(_identityService.Username, UserAgent);
                item.SetBank(data.Bank, _identityService.Username, UserAgent);
                //if (data.IsApproved)
                //{
                //    item.SetApprovedBy(_identityService.Username, _identityService.Username, UserAgent);
                //    item.SetApprovedDate(DateTimeOffset.UtcNow, _identityService.Username, UserAgent);
                //}

                if (item.IsInklaring)
                {
                    vbRequestIdJournals.Add(item.Id);
                    var bankDocumentNo = await _dppVatBankExpenditureNoteService.GetDocumentNo("K",data.Bank.BankCode,_identityService.Username,DateTime.UtcNow);
                    item.SetBankDocumentNo(bankDocumentNo,_identityService.Username,UserAgent);
                    vbRequestsList.Add(new ApprovalVBAutoJournalDto { VbRequestDocument = item, Bank = data.Bank });
                }

                //if (item.Type == VBType.WithPO)
                //{

                //    //var epoIds = _dbContext.VBRequestDocumentEPODetails.Where(entity => entity.VBRequestDocumentId == item.Id).Select(entity => (long)entity.EPOId).ToList();
                //    //var autoJournalEPOUri = "vb-request-po-external/auto-journal-epo";

                //    //var body = new VBAutoJournalFormDto()
                //    //{
                //    //    Date = DateTimeOffset.UtcNow,
                //    //    DocumentNo = item.DocumentNo,
                //    //    EPOIds = epoIds
                //    //};

                //    //var httpClient = _serviceProvider.GetService<IHttpClientService>();
                //    //var response = httpClient.PostAsync($"{APIEndpoint.Purchasing}{autoJournalEPOUri}", new StringContent(JsonConvert.SerializeObject(body).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
                //}
            }

            var result = await _dbContext.SaveChangesAsync();

            await _autoJournalTransactionService.AutoJournalInklaring(vbRequestIdJournals, data.Bank);
            await _dailyBankTransactionService.AutoCreateVbApproval(vbRequestsList);
            return result;
        }

        public Task<int> CancellationDocuments(CancellationFormDto form)
        {
            var vbDocuments = _dbContext.VBRequestDocuments.Where(s => form.Ids.Contains(s.Id));

            foreach (var item in vbDocuments)
            {
                item.SetCancellation(form.Reason, _identityService.Username, UserAgent);

                //if (item.Type == VBType.WithPO)
                //{
                //    var epoIds = _dbContext.VBRequestDocumentEPODetails.Where(entity => entity.VBRequestDocumentId == item.Id).Select(entity => (long)entity.EPOId).ToList();
                //    var autoJournalEPOUri = "vb-request-po-external/auto-journal-epo";

                //    var body = new VBAutoJournalFormDto()
                //    {
                //        Date = DateTimeOffset.UtcNow,
                //        DocumentNo = item.DocumentNo,
                //        EPOIds = epoIds
                //    };

                //    var httpClient = _serviceProvider.GetService<IHttpClientService>();
                //    var response = httpClient.PostAsync($"{APIEndpoint.Purchasing}{autoJournalEPOUri}", new StringContent(JsonConvert.SerializeObject(body).ToString(), Encoding.UTF8, General.JsonMediaType)).Result;
                //}
                if (item.IsInklaring)
                    _journalTransactionService.ReverseJournalTransactionByReferenceNo(item.DocumentNo);
            }

            return _dbContext.SaveChangesAsync();
        }

        public bool GetVBForPurchasing(int id)
        {
            bool Response = true;

            var model = _dbContext.VBRequestDocumentEPODetails.Where(s => s.EPOId == id && s.IsDeleted == false).ToList();

            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    var VBmodel = _dbContext.VBRequestDocuments.Where(s => s.Id == item.VBRequestDocumentId).ToList();

                    Response = !VBmodel.Count.Equals(0);

                    if (Response == true)
                    {
                        break;
                    }
                }
            }
            else
            {
                Response = false;
            }

            return Response;
        }
    }
}
