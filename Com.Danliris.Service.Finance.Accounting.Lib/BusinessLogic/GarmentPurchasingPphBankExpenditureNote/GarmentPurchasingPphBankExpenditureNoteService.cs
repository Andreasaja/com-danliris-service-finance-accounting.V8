using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.DailyBankTransaction;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Newtonsoft.Json;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentPurchasingPphBankExpenditureNote;
using System.Threading.Tasks;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using System.Net.Http;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using System.IO;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingPphBankExpenditureNote.ExcelGenerator;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentPurchasingPphBankExpenditureNote
{
    public class GarmentPurchasingPphBankExpenditureNoteService : IGarmentPurchasingPphBankExpenditureNoteService
    {
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDailyBankTransactionService _serviceDailyBankTransaction;
        private readonly IMapper _mapper;

        public GarmentPurchasingPphBankExpenditureNoteService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
            _serviceDailyBankTransaction = serviceProvider.GetService<IDailyBankTransactionService>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        public async Task CreateAsync(FormInsert model)
        {
            var typeDocument = "K";
            var username = _identityService.Username;
            var timeOffset = new TimeSpan(_identityService.TimezoneOffset, 0, 0);
            model.PphBankInvoiceNo = await _serviceDailyBankTransaction.GetDocumentNo("K", model.Bank.BankCode, username, model.Date.GetValueOrDefault().ToOffset(timeOffset).Date);
            //model.PphBankInvoiceNo = "test";

            var mapper = _mapper.Map<GarmentPurchasingPphBankExpenditureNoteModel>(model);

            EntityExtension.FlagForCreate(mapper, username, UserAgent);
            foreach (var item in mapper.Items)
            {
                EntityExtension.FlagForCreate(item, username, UserAgent);
                foreach (var invoice in item.GarmentPurchasingPphBankExpenditureNoteInvoices)
                {
                    EntityExtension.FlagForCreate(invoice, username, UserAgent);
                }
            }
            _dbContext.GarmentPurchasingPphBankExpenditureNotes.Add(mapper);
            await _dbContext.SaveChangesAsync();

            ///update ispaidPphStatus
            var listIsPaidStatus = model.PPHBankExpenditureNoteItems.Select( s=> new GarmentPurchasingPphBankExpenditureNoteInternNoteIsPaidDto { 
                InternNoteId = s.INId,
                InternNoteNo = s.INNo,
                IsPphPaid = true
            }).ToList();
            await UpdateIsPaidPph(listIsPaidStatus);
        }

        public async Task UpdateAsync(FormInsert model)
        {
            //var modelExisting = _dbContext.GarmentPurchasingPphBankExpenditureNotes.Include(t => t.Items).ThenInclude(t => t.GarmentPurchasingPphBankExpenditureNoteInvoices).Where(entity => entity.Id == model.Id);
            var modelExsiting = await ReadByIdAsync(model.Id);
            var dataById = _dbContext.GarmentPurchasingPphBankExpenditureNotes.AsNoTracking().Where(s => s.Id == model.Id).FirstOrDefault();
            var modelModif = modelExsiting;
            modelModif.Date = model.Date;
            var username = _identityService.Username;

            //removeModel
            var modelMustHave = model.PPHBankExpenditureNoteItems.Where(s => s.Id != 0).Select(s => s.Id).ToList();
            var NiWillbeRemoved = modelExsiting.PPHBankExpenditureNoteItems.Where(s => !modelMustHave.Contains(s.Id));
            var InvoiceWillBeRemoved = NiWillbeRemoved.Select(s => new {s.Id, s.INId, s.INNo });
            var InvoiceWillBeRemovedIdOnly = NiWillbeRemoved.Select(s => s.Id);

            var pphData = _dbContext.GarmentPurchasingPphBankExpenditureNoteItems.AsNoTracking().Where(s => InvoiceWillBeRemovedIdOnly.Contains(s.Id));
            foreach(var rmInv in pphData)
            {
                EntityExtension.FlagForDelete(rmInv, username, UserAgent, true);
                _dbContext.GarmentPurchasingPphBankExpenditureNoteItems.Update(rmInv);
            }
            _dbContext.SaveChanges();

            ///update ispaidPphStatus
            var listIsPaidStatusRmoved = InvoiceWillBeRemoved.Select(s => new GarmentPurchasingPphBankExpenditureNoteInternNoteIsPaidDto
            {
                InternNoteId = s.INId,
                InternNoteNo = s.INNo,
                IsPphPaid = false
            }).ToList();
            await UpdateIsPaidPph(listIsPaidStatusRmoved);

            //insert new Data 
            var newData = model.PPHBankExpenditureNoteItems.Where(s => s.Id == 0);

            var dataWillBeInsert = modelModif;
            dataWillBeInsert.PPHBankExpenditureNoteItems = newData.ToList();

            var mapperNewData = _mapper.Map<GarmentPurchasingPphBankExpenditureNoteModel>(dataWillBeInsert);
            mapperNewData.CreatedAgent = dataById.CreatedAgent;
            mapperNewData.CreatedBy = dataById.CreatedBy;
            mapperNewData.CreatedUtc = dataById.CreatedUtc;
            EntityExtension.FlagForUpdate(mapperNewData, username, UserAgent);
            foreach (var item in mapperNewData.Items)
            {
                EntityExtension.FlagForCreate(item, username, UserAgent);
                foreach (var invoice in item.GarmentPurchasingPphBankExpenditureNoteInvoices)
                {
                    EntityExtension.FlagForCreate(invoice, username, UserAgent);
                }
            }
            _dbContext.GarmentPurchasingPphBankExpenditureNotes.Update(mapperNewData);
            await _dbContext.SaveChangesAsync();

            ///update ispaidPphStatus
            var listIsPaidStatus = dataWillBeInsert.PPHBankExpenditureNoteItems.Select(s => new GarmentPurchasingPphBankExpenditureNoteInternNoteIsPaidDto
            {
                InternNoteId = s.INId,
                InternNoteNo = s.INNo,
                IsPphPaid = true
            }).ToList();
            await UpdateIsPaidPph(listIsPaidStatus);
        }

        public async Task DeleteAsync(int id)
        {
            var existingModel = _dbContext.GarmentPurchasingPphBankExpenditureNotes
                        .Include(d => d.Items)
                        .ThenInclude(d => d.GarmentPurchasingPphBankExpenditureNoteInvoices)
                        .Single(x => x.Id == id && !x.IsDeleted);
            var model = await ReadByIdAsync(id);
            foreach (var item in existingModel.Items)
            {
                EntityExtension.FlagForDelete(item, _identityService.Username, UserAgent, true);
            }
            EntityExtension.FlagForDelete(existingModel, _identityService.Username, UserAgent, true);
            _dbContext.GarmentPurchasingPphBankExpenditureNotes.Update(existingModel);

            await _dbContext.SaveChangesAsync();

            ///update ispaidPphStatus
            var listIsPaidStatus = model.PPHBankExpenditureNoteItems.Select(s => new GarmentPurchasingPphBankExpenditureNoteInternNoteIsPaidDto
            {
                InternNoteId = s.INId,
                InternNoteNo = s.INNo,
                IsPphPaid = false
            }).ToList();
            await UpdateIsPaidPph(listIsPaidStatus);
        }

        public async Task PostingDocument(List<int> ids)
        {
            foreach (var id in ids)
            {
                var existingModel = _dbContext.GarmentPurchasingPphBankExpenditureNotes
                            .Include(d => d.Items)
                            .Single(x => x.Id == id && !x.IsDeleted);

                existingModel.IsPosted = true;


                foreach (var item in existingModel.Items)
                {
                    EntityExtension.FlagForUpdate(item, _identityService.Username, UserAgent);
                }
                EntityExtension.FlagForUpdate(existingModel, _identityService.Username, UserAgent);
                _dbContext.GarmentPurchasingPphBankExpenditureNotes.Update(existingModel);

                var model = await ReadByIdAsync(id);
                // insert DailyBankTransaction
                var dailyBankMap = _mapper.Map<DailyBankTransactionModel>(model);

                var insertDailyBank = await _serviceDailyBankTransaction.CreateAsync(dailyBankMap);

                await _dbContext.SaveChangesAsync();
            }
        }

        //public List<GarmentPurchasingPphBankExpenditureNoteDataViewModel> PrintInvoice(int id)
        //{
        //    var existingModel = DbSet
        //                .Include(d => d.Items)
        //                .Single(x => x.Id == id && !x.IsDeleted);
        //    GarmentInvoicePaymentModel model = await ReadByIdAsync(id);
        //    foreach (var item in model.Items)
        //    {
        //        EntityExtension.FlagForDelete(item, IdentityService.Username, UserAgent, true);
        //    }
        //    EntityExtension.FlagForDelete(model, IdentityService.Username, UserAgent, true);
        //    DbSet.Update(model);

        //    return await DbContext.SaveChangesAsync();
        //}

        public ReadResponse<GarmentPurchasingPphBankExpenditureNoteDataViewModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            //var query = _dbContext.GarmentPurchasingPphBankExpenditureNotes.Include(s=> s.Items).ThenInclude(s=> s.GarmentPurchasingPphBankExpenditureNoteInvoices).AsQueryable();
            var query = _dbContext.GarmentPurchasingPphBankExpenditureNoteItems.Include(s => s.GarmentPurchasingPphBankExpenditureNote).Include(s=>s.GarmentPurchasingPphBankExpenditureNoteInvoices).AsQueryable();


            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.GarmentPurchasingPphBankExpenditureNote.InvoiceOutNumber.Contains(keyword) || entity.GarmentPurchasingPphBankExpenditureNote.BankName.Contains(keyword) || entity.IncomeTaxName.Contains(keyword) || entity.GarmentPurchasingPphBankExpenditureNote.BankCurrencyCode.Contains(keyword) || keyword.Contains(entity.InternalNotesNo));

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<GarmentPurchasingPphBankExpenditureNoteItemModel>.Order(query, orderDictionary);

            var count = query.Count();

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(entity => new GarmentPurchasingPphBankExpenditureNoteDataViewModel(entity))
                .ToList();

            return new ReadResponse<GarmentPurchasingPphBankExpenditureNoteDataViewModel>(data, count, orderDictionary, new List<string>());
        }

        public async Task<FormInsert> ReadByIdAsync(int id)
        {
            return await _dbContext.GarmentPurchasingPphBankExpenditureNotes
                .Include(element => element.Items)
                .ThenInclude(invoice => invoice.GarmentPurchasingPphBankExpenditureNoteInvoices)
                .Select(s => new FormInsert
                {
                    Id = s.Id,
                    IsPosted = s.IsPosted,
                    Bank = new Bank
                    {
                        AccountCOA = s.AccountBankCOA,
                        AccountName = s.AccountBankName,
                        AccountNumber = s.AccountBankNumber,
                        BankAddress = s.BankAddress,
                        BankCode = s.BankCode,
                        BankName = s.BankName,
                        Code = s.BankCode1,
                        Currency = new ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels.Currency
                        {
                            Code = s.BankCurrencyCode,
                            Id = s.BankCurrencyId
                        },
                        SwiftCode = s.BankSwiftCode,
                    },
                    Date = s.InvoiceOutDate,
                    DateFrom = s.DueDateStart,
                    DateTo = s.DueDateEnd,
                    PphBankInvoiceNo = s.InvoiceOutNumber,
                    IncomeTax = new ViewModels.GarmentPurchasingPphBankExpenditureNoteViewModels.IncomeTax
                    {
                        Name = s.IncomeTaxName,
                        Rate = Convert.ToDecimal(s.IncomeTaxRate),
                        Id = s.IncomeTaxId
                    },
                    PPHBankExpenditureNoteItems = s.Items.Select(item => new FormAdd
                    {
                        Id = item.Id,
                        CurrencyCode = item.CurrencyCode,
                        CurrencyId = item.CurrencyId,
                        SupplierId = item.SupplierId,
                        SupplierName = item.SupplierName,
                        INDate = item.Date,
                        INDueDate = item.DueDate,
                        INNo = item.InternalNotesNo,
                        INId = item.InternalNotesId,
                        TotalIncomeTaxNI = item.TotalPaid * (item.IncomeTaxTotal/100),
                        Items =
                        new List<Item>()
                        {
                            new Item
                            {
                                Id = item.Id,
                                TotalAmount = item.TotalPaid,
                                Details = item.GarmentPurchasingPphBankExpenditureNoteInvoices.Select(detail => new Detail {
                                            InvoiceDate = detail.InvoicesDate,
                                            InvoiceId = Convert.ToInt32(detail.InvoicesId),
                                            InvoiceNo = detail.InvoicesNo,
                                            InvoiceTotalAmount = Convert.ToDouble(detail.Total),
                                            ProductCode = detail.ProductCode,
                                            ProductId = Convert.ToInt32(detail.ProductId),
                                            ProductCategory = detail.ProductCategory,
                                            ProductName = detail.ProductName,
                                            PriceTotal = detail.Total,
                                            UnitCode = detail.UnitCode,
                                            UnitId = detail.UnitId,
                                            UnitName = detail.UnitName,
                                            Id = detail.Id
                                            }).ToList(),
                                TotalIncomeTax = item.TotalPaid *(item.IncomeTaxTotal/100)
                            }
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync(entity => entity.Id == id);
        }

        

        public ReadResponse<GarmentPurchasingPphBankExpenditureNoteReportViewDto> GetReportView(int page, int size, string order, GarmentPurchasingPphBankExpenditureNoteFilterReportDto filter)
        {
            var query = _dbContext.GarmentPurchasingPphBankExpenditureNoteInvoices.Include(s => s.GarmentPurchasingPphBankExpenditureNoteItem).ThenInclude(s=> s.GarmentPurchasingPphBankExpenditureNote).AsQueryable();

            if (!string.IsNullOrEmpty(filter.InvoiceOutNo))
            {
                query = query.Where(entity => entity.GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.InvoiceOutNumber.Contains(filter.InvoiceOutNo));
            }
            
            if(!string.IsNullOrEmpty(filter.InvoiceNo))
            {
                query = query.Where(entity => entity.InvoicesNo.Contains(filter.InvoiceNo));
            }

            if (!string.IsNullOrEmpty(filter.INNo))
            {
                query = query.Where(entity => entity.GarmentPurchasingPphBankExpenditureNoteItem.InternalNotesNo.Contains(filter.INNo));
            }

            if (!string.IsNullOrEmpty(filter.SupplierName))
            {
                query = query.Where(entity => entity.GarmentPurchasingPphBankExpenditureNoteItem.SupplierName.Contains(filter.SupplierName));
            }

            //if(filter.DateStart.HasValue)
            if (filter.DateStart.HasValue && filter.DateStart.GetValueOrDefault().Year != 1)
            {
                query = query.Where(entity => entity.GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.InvoiceOutDate >= filter.DateStart);
            }

            //if(filter.DateEnd.HasValue)
            if (filter.DateEnd.HasValue && filter.DateEnd.GetValueOrDefault().Year != 1)
            {
                query = query.Where(entity => entity.GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.InvoiceOutDate <= filter.DateEnd);
            }
            //if (filter.DateStart.HasValue && filter.DateStart.GetValueOrDefault().Year != 1)
            //{
            //    query = query.Where(entity => (entity.CreatedUtc>= entity.LastModifiedUtc? entity.CreatedUtc : entity.LastModifiedUtc) >= filter.DateStart);
            //}

            ////if(filter.DateEnd.HasValue)
            //if (filter.DateEnd.HasValue && filter.DateEnd.GetValueOrDefault().Year != 1)
            //{
            //    query = query.Where(entity => (entity.CreatedUtc >= entity.LastModifiedUtc ? entity.CreatedUtc : entity.LastModifiedUtc) <= filter.DateEnd);
            //}
            //var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //query = QueryHelper<GarmentPurchasingPphBankExpenditureNoteItemModel>.Order(query, orderDictionary);

            var count = query.Count();
            var queryList = query.ToList();

            //var data = queryList
            //    .Skip((page - 1) * size)
            //    .Take(size)
            //    .ToList()
            //                    .GroupBy(
            //    invoice => new { invoice.InvoicesId, invoice.InvoicesNo, invoice.InvoicesDate },
            //    groupInvoice => groupInvoice,
            //    (key, grp) => new GarmentPurchasingPphBankExpenditureNoteReportViewDto
            //    {
            //        BankName = grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.AccountBankName + " - "+ 
            //                   grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.BankName+ " - "+
            //                   grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.AccountBankNumber+" - "+
            //                   grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.BankCurrencyCode
            //                    ?? string.Empty,
            //        Category = grp.FirstOrDefault().ProductCategory ?? string.Empty,
            //        INNO = grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.InternalNotesNo ?? string.Empty,
            //        InvoiceNo = key.InvoicesNo ?? string.Empty,
            //        CurrencyCode = grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.CurrencyCode ?? string.Empty,
            //        InvoiceOutNo = grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.InvoiceOutNumber ?? string.Empty,
            //        PaidDate = grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.InvoiceOutDate,
            //        SupplierName = grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.SupplierName ?? string.Empty,
            //        PPH = grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.TotalPaid * (grp.FirstOrDefault().GarmentPurchasingPphBankExpenditureNoteItem.IncomeTaxTotal / 100)
            //    }).ToList()
            //    ;

            var data = queryList
                .Skip((page - 1) * size)
                .Take(size)
                .Select(grp => new GarmentPurchasingPphBankExpenditureNoteReportViewDto
                {
                    Id = grp.Id,
                    BankName = grp.GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.AccountBankName + " - " +
                               grp.GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.BankName + " - " +
                               grp.GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.AccountBankNumber + " - " +
                               grp.GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.BankCurrencyCode
                                ?? string.Empty,
                    Category = grp.ProductCategory ?? string.Empty,
                    INNO = grp.GarmentPurchasingPphBankExpenditureNoteItem.InternalNotesNo ?? string.Empty,
                    InvoiceNo = grp.InvoicesNo ?? string.Empty,
                    CurrencyCode = grp.GarmentPurchasingPphBankExpenditureNoteItem.CurrencyCode ?? string.Empty,
                    InvoiceOutNo = grp.GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.InvoiceOutNumber ?? string.Empty,
                    PaidDate = grp.GarmentPurchasingPphBankExpenditureNoteItem.GarmentPurchasingPphBankExpenditureNote.InvoiceOutDate,
                    SupplierName = grp.GarmentPurchasingPphBankExpenditureNoteItem.SupplierName ?? string.Empty,
                    PPH = grp.GarmentPurchasingPphBankExpenditureNoteItem.TotalPaid * (grp.GarmentPurchasingPphBankExpenditureNoteItem.IncomeTaxTotal / 100)
                }).ToList()
                ;

            return new ReadResponse<GarmentPurchasingPphBankExpenditureNoteReportViewDto>(data, count, new Dictionary<string, string>(), new List<string>());
        }

        public ReadResponse<GarmentPurchasingPphBankExpenditureNoteReportGroupView> GetReportGroupView(int page, int size, string order, GarmentPurchasingPphBankExpenditureNoteFilterReportDto filter)
        {
            var reportView = GetReportView(page, size, order, filter);

            var groupReport = reportView.Data.GroupBy(
                key => new { key.INNO, key.InvoiceOutNo, key.PaidDate, key.PPH, key.SupplierName, key.CurrencyCode, key.Category, key.BankName },
                value => new { value.InvoiceNo },
                (key, value) => new GarmentPurchasingPphBankExpenditureNoteReportGroupView
                {
                    BankName = key.BankName,
                    Category = key.Category,
                    CurrencyCode = key.CurrencyCode,
                    //Id = key.Id,
                    INNO = key.INNO,
                    InvoiceOutNo = key.InvoiceOutNo,
                    PaidDate = key.PaidDate,
                    PPH = key.PPH,
                    SupplierName = key.SupplierName,
                    InvoiceItems = value.Select(s => new GarmentPurchasingPphBankExpenditureReportGroupItemDto { NoInvoice = s.InvoiceNo }).ToList()
                }
                ).ToList();
            return new ReadResponse<GarmentPurchasingPphBankExpenditureNoteReportGroupView>(groupReport, reportView.Count, new Dictionary<string, string>(), new List<string>());
        }


        public List<GarmentPurchasingPphBankExpenditureNoteModel> GetReportData( GarmentPurchasingPphBankExpenditureNoteFilterReportDto filter)
        {
            var query = _dbContext.GarmentPurchasingPphBankExpenditureNotes.Include(s => s.Items).ThenInclude(s => s.GarmentPurchasingPphBankExpenditureNoteInvoices).AsQueryable();

            if (!string.IsNullOrEmpty(filter.InvoiceOutNo))
            {
                query = query.Where(entity => entity.InvoiceOutNumber.Contains(filter.InvoiceOutNo));
            }

            if (!string.IsNullOrEmpty(filter.InvoiceNo))
            {
                query = query.Where(entity => entity.Items.Any(s=> s.GarmentPurchasingPphBankExpenditureNoteInvoices.Any(t => t.InvoicesNo.Contains(filter.InvoiceNo))));
            }

            if (!string.IsNullOrEmpty(filter.INNo))
            {
                query = query.Where(entity => entity.Items.Any(s=> s.InternalNotesNo.Contains(filter.INNo)));
            }

            if (!string.IsNullOrEmpty(filter.SupplierName))
            {
                query = query.Where(entity => entity.Items.Any(s=> s.SupplierName.Contains(filter.SupplierName)));
            }

            if (filter.DateStart.HasValue && filter.DateStart.GetValueOrDefault().Year!=1)
            {
                query = query.Where(entity => entity.InvoiceOutDate >= filter.DateStart);
                //query = query.Where(entity => (entity.CreatedUtc >= entity.LastModifiedUtc ? entity.CreatedUtc : entity.LastModifiedUtc) >= filter.DateStart);

            }

            if (filter.DateEnd.HasValue && filter.DateEnd.GetValueOrDefault().Year != 1)
            {
                query = query.Where(entity => entity.InvoiceOutDate <= filter.DateEnd);
                //query = query.Where(entity => (entity.CreatedUtc >= entity.LastModifiedUtc ? entity.CreatedUtc : entity.LastModifiedUtc) <= filter.DateEnd);
            }

            //var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            //query = QueryHelper<GarmentPurchasingPphBankExpenditureNoteItemModel>.Order(query, orderDictionary);

            var count = query.Count();
            var queryList = query.ToList();

            var data = queryList
                //.Skip((page - 1) * size)
                //.Take(size)
                .ToList()
                ;
            return data;


        }

        public MemoryStream DownloadReportXls(GarmentPurchasingPphBankExpenditureNoteFilterReportDto filter)
        {
            var data = GetReportData(filter);
            var xls = GarmentPurchasingPphBankExpenditureNoteReportExcelGenerator.Create("Lap Pembayaran PPH", data, filter.DateStart.GetValueOrDefault(), filter.DateEnd.GetValueOrDefault(), _identityService.TimezoneOffset);
            return xls;
        }

        public List<GarmentPurchasingPphBankExpenditureNoteLoaderInternNote> GetLoaderInterNotePPH(string keyword)
        {
            var query = _dbContext.GarmentPurchasingPphBankExpenditureNoteItems.Where(s=> s.InternalNotesNo.Contains(keyword)).Select(s => new GarmentPurchasingPphBankExpenditureNoteLoaderInternNote
            {
                Id = s.InternalNotesId,
                Name = s.InternalNotesNo,
            })
                .Skip((1 - 1) * 10)
                .Take(10);
            return query.ToList();
        }

        public List<GarmentPurchasingPphBankExpenditureLoaderSupplierDto> GetLoaderSupplier(string keyword)
        {
            var query = _dbContext.GarmentPurchasingPphBankExpenditureNoteItems.Where(s => s.SupplierName.Contains(keyword)).Select(s => new GarmentPurchasingPphBankExpenditureLoaderSupplierDto
            {
                Id = s.SupplierId,
                Name = s.SupplierName,
            })
                .Skip((1 - 1) * 10)
                .Take(10);
            return query.ToList();
        }

        public List<GarmentPurchasingPphBankExpenditureNoteLoaderInvoiceDto> GetLoaderInvoice(string keyword)
        {
            var query = _dbContext.GarmentPurchasingPphBankExpenditureNoteInvoices.Where(s => s.InvoicesNo.Contains(keyword)).Select(s => new GarmentPurchasingPphBankExpenditureNoteLoaderInvoiceDto
            {
                Id = s.InvoicesId,
                Name = s.InvoicesNo,
            })
                .Skip((1 - 1) * 10)
                .Take(10);
            return query.ToList();
        }
        public List<GarmentPurchasingPphBankExpenditureNoteLoaderInvoiceOutDto> GetLoaderInvoiceOut(string keyword)
        {
            var query = _dbContext.GarmentPurchasingPphBankExpenditureNotes.Where(s => s.InvoiceOutNumber.Contains(keyword)).Select(s => new GarmentPurchasingPphBankExpenditureNoteLoaderInvoiceOutDto
            {
                Id = s.Id,
                Name = s.InvoiceOutNumber,
            })
                .Skip((1 - 1) * 10)
                .Take(10);
            return query.ToList();
        }

        public List<GarmentPurchasingPphBankExpenditureNoteInternNoteIsPaidDto> GetInternNoteIsPaid()
        {
            var query = _dbContext.GarmentPurchasingPphBankExpenditureNoteItems
                .GroupBy(
                internNote => new { internNote.InternalNotesId, internNote.InternalNotesNo },
                grpInternNote => grpInternNote,
                (internNote, grpInternNote) => new GarmentPurchasingPphBankExpenditureNoteInternNoteIsPaidDto
                {
                    InternNoteId = internNote.InternalNotesId,
                    InternNoteNo = internNote.InternalNotesNo
                });
            return query.ToList();
        }

        public async Task UpdateIsPaidPph(List<GarmentPurchasingPphBankExpenditureNoteInternNoteIsPaidDto> listModel )
        {
           
            var httpClient = _serviceProvider.GetService<IHttpClientService>();

            await httpClient.PutAsync($"{APIEndpoint.Purchasing}garment-purchasing-expeditions/internal-notes-update-paid-pph", new StringContent(JsonConvert.SerializeObject(listModel), Encoding.UTF8, General.JsonMediaType));
        }
    }
}
