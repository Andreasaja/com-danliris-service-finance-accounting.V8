using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DPPVATBankExpenditureNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.DPPVATBankExpenditureNote
{
    public class DPPVATBankExpenditureNoteService : IDPPVATBankExpenditureNoteService
    {
        private const string UserAgent = "finance-accounting-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;

        public DPPVATBankExpenditureNoteService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        public async Task<int> Create(FormDto form)
        {
            var timeOffset = new TimeSpan(_identityService.TimezoneOffset, 0, 0);
            var documentNo = await GetDocumentNo("K", form.Bank.BankCode, _identityService.Username, form.Date.GetValueOrDefault().ToOffset(timeOffset).Date);
            var model = new DPPVATBankExpenditureNoteModel(documentNo, form.Bank.Id, form.Bank.AccountNumber, form.Bank.BankName, form.Bank.BankCode, form.Currency.Id, form.Currency.Code, form.Currency.Rate, form.Supplier.Id, form.Supplier.Name, form.Supplier.IsImport, form.BGCheckNo, form.Amount, form.Date.GetValueOrDefault(), form.Bank.Currency.Code, form.Bank.Currency.Id, form.Bank.Currency.Rate);
            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            _dbContext.DPPVATBankExpenditureNotes.Add(model);
            _dbContext.SaveChanges();

            foreach (var formItem in form.Items.Where(element => element.Select))
            {
                var item = new DPPVATBankExpenditureNoteItemModel(model.Id, formItem.InternalNote.Id, formItem.InternalNote.DocumentNo, formItem.InternalNote.Date, formItem.InternalNote.DueDate, formItem.InternalNote.Supplier.Id, formItem.InternalNote.Supplier.Name, formItem.InternalNote.Supplier.IsImport, formItem.InternalNote.VATAmount, formItem.InternalNote.IncomeTaxAmount, formItem.InternalNote.DPP, formItem.InternalNote.TotalAmount, formItem.InternalNote.Currency.Id, formItem.InternalNote.Currency.Code, formItem.OutstandingAmount, formItem.InternalNote.Supplier.Code);
                EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
                _dbContext.DPPVATBankExpenditureNoteItems.Add(item);
                _dbContext.SaveChanges();

                foreach (var formDetail in formItem.InternalNote.Items.Where(element => element.SelectInvoice))
                {
                    var detailDOString = JsonConvert.SerializeObject(formDetail.Invoice.DetailDO);
                    var detail = new DPPVATBankExpenditureNoteDetailModel(model.Id, item.Id, formDetail.Invoice.Id, formDetail.Invoice.DocumentNo, formDetail.Invoice.Date, formDetail.Invoice.ProductNames, formDetail.Invoice.Category.Id, formDetail.Invoice.Category.Name, formDetail.Invoice.Amount, formDetail.Invoice.PaymentMethod, formDetail.Invoice.DeliveryOrdersNo, formDetail.Invoice.PaymentBills, formDetail.Invoice.BillsNo, detailDOString);
                    EntityExtension.FlagForCreate(detail, _identityService.Username, UserAgent);
                    _dbContext.DPPVATBankExpenditureNoteDetails.Add(detail);
                    _dbContext.SaveChanges();

                    foreach(var detailDo in formDetail.Invoice.DetailDO)
                    {
                        var detailDoDd = new DPPVATBankExpenditureNoteDetailDoModel(detailDo.DONo,detailDo.TotalAmount,detailDo.PaymentBill,detailDo.BillNo,detailDo.DOId,detailDo.CurrencyRate,model.Id,item.Id,detail.Id);
                        EntityExtension.FlagForCreate(detailDoDd, _identityService.Username, UserAgent);
                        _dbContext.DPPVATBankExpenditureNoteDetailDos.Add(detailDoDd);
                        _dbContext.SaveChanges();
                    }
                }
            }

            var internalNoteIds = form.Items.Where(element => element.Select).Select(element => element.InternalNote.Id).ToList();
            var invoiceNoteIds = form.Items.Where(element => element.Select).SelectMany(element => element.InternalNote.Items).Where(element => element.SelectInvoice).Select(element => element.Invoice.Id).ToList();

            await UpdateInternalNoteInvoiceNoteIsPaid(true, model.Id, model.DocumentNo, internalNoteIds, invoiceNoteIds);

            return model.Id;
        }

        //private async Task<string> GetDocumentNo(string type, string bankCode, string username)
        //{
        //    var jsonSerializerSettings = new JsonSerializerSettings
        //    {
        //        MissingMemberHandling = MissingMemberHandling.Ignore
        //    };

        //    var http = _serviceProvider.GetService<IHttpClientService>();
        //    var uri = APIEndpoint.Purchasing + $"bank-expenditure-notes/bank-document-no?type={type}&bankCode={bankCode}&username={username}";
        //    var response = await http.GetAsync(uri);

        //    var result = new BaseResponse<string>();

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        result = JsonConvert.DeserializeObject<BaseResponse<string>>(responseContent, jsonSerializerSettings);
        //    }

        //    return result.data;
        //}

        public async Task<string> GetDocumentNo(string type, string bankCode, string username,DateTime date)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.Purchasing + $"bank-expenditure-notes/bank-document-no-date?type={type}&bankCode={bankCode}&username={username}&date={date}";
            var response = await http.GetAsync(uri);

            var result = new BaseResponse<string>();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<BaseResponse<string>>(responseContent, jsonSerializerSettings);
            }

            return result.data;
        }

        private async Task UpdateInternalNoteInvoiceNoteIsPaid(bool dppVATIsPaid, int bankExpenditureNoteId, string bankExpenditureNoteNo, List<int> internalNoteIds, List<int> invoiceNoteIds)
        {
            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.Purchasing + $"garment-intern-notes/dpp-vat-bank-expenditures/is-paid?dppVATIsPaid={dppVATIsPaid}&bankExpenditureNoteId={bankExpenditureNoteId}&bankExpenditureNoteNo={bankExpenditureNoteNo}&internalNoteIds={JsonConvert.SerializeObject(internalNoteIds)}&invoiceNoteIds={JsonConvert.SerializeObject(invoiceNoteIds)}";
            await http.PutAsync(uri, new StringContent(JsonConvert.SerializeObject(internalNoteIds), Encoding.UTF8, General.JsonMediaType));
        }

        public async Task<int> Delete(int id)
        {
            var model = _dbContext
                .DPPVATBankExpenditureNotes
                .FirstOrDefault(entity => entity.Id == id);
            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);
            _dbContext.DPPVATBankExpenditureNotes.Update(model);

            var items = _dbContext.DPPVATBankExpenditureNoteItems
                .Where(entity => entity.DPPVATBankExpenditureNoteId == id)
                .ToList()
                .Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                })
                .ToList();
            _dbContext.DPPVATBankExpenditureNoteItems.UpdateRange(items);

            var details = _dbContext.DPPVATBankExpenditureNoteDetails
                .Where(entity => entity.DPPVATBankExpenditureNoteId == id)
                .ToList()
                .Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                })
                .ToList();
            _dbContext.DPPVATBankExpenditureNoteDetails.UpdateRange(details);

            var detailDos = _dbContext.DPPVATBankExpenditureNoteDetailDos
                .Where(entity => entity.DPPVATBankExpenditureNoteId == id)
                .ToList()
                .Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                })
                .ToList();
            _dbContext.DPPVATBankExpenditureNoteDetailDos.UpdateRange(detailDos);

            var internalNoteIds = items.Select(element => element.InternalNoteId).ToList();
            var invoiceNoteIds = details.Select(element => element.InvoiceId).ToList();

            await UpdateInternalNoteInvoiceNoteIsPaid(false, model.Id, model.DocumentNo, internalNoteIds, invoiceNoteIds);

            return _dbContext.SaveChanges();
        }

        public DPPVATBankExpenditureNoteDto Read(int id)
        {
            var model = _dbContext.DPPVATBankExpenditureNotes.FirstOrDefault(entity => entity.Id == id);
            if (model == null)
                return null;

            var items = _dbContext.DPPVATBankExpenditureNoteItems.Where(entity => entity.DPPVATBankExpenditureNoteId == id).ToList();
            var details = _dbContext.DPPVATBankExpenditureNoteDetails.Where(entity => entity.DPPVATBankExpenditureNoteId == id).ToList();

            return new DPPVATBankExpenditureNoteDto(model, items, details);
        }

        public ReadResponse<DPPVATBankExpenditureNoteIndexDto> Read(string keyword, int page = 1, int size = 25, string order = "{}")
        {
            var query = _dbContext.DPPVATBankExpenditureNotes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.DocumentNo.Contains(keyword) || entity.BankAccountNumber.Contains(keyword) || entity.BankName.Contains(keyword) || entity.SupplierName.Contains(keyword) || entity.CurrencyCode.Contains(keyword));

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<DPPVATBankExpenditureNoteModel>.Order(query, orderDictionary);

            var count = query.Count();

            var itemQuery = _dbContext.DPPVATBankExpenditureNoteItems.AsQueryable();

            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .ToList()
                .Select(entity => new DPPVATBankExpenditureNoteIndexDto(entity.Id, entity.DocumentNo, entity.Date, entity.BankName, entity.BankAccountNumber, itemQuery.Where(item => item.DPPVATBankExpenditureNoteId == entity.Id).Sum(item => item.TotalAmount), entity.CurrencyCode, string.Join("\n", itemQuery.Where(item => item.DPPVATBankExpenditureNoteId == entity.Id).Select(item => $"- {item.InternalNoteNo}").ToList()), entity.SupplierName, entity.IsPosted))
                .ToList();

            return new ReadResponse<DPPVATBankExpenditureNoteIndexDto>(data, count, orderDictionary, new List<string>());
        }

        public async Task<int> Update(int id, FormDto form)
        {
            var model = _dbContext
                .DPPVATBankExpenditureNotes
                .FirstOrDefault(entity => entity.Id == id);
            model.UpdateData(form.Amount, form.Supplier.Id, form.Supplier.IsImport, form.Supplier.Name, form.BGCheckNo, form.Date.GetValueOrDefault(), form.Currency.Rate);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.DPPVATBankExpenditureNotes.Update(model);

            var items = _dbContext.DPPVATBankExpenditureNoteItems
                .Where(entity => entity.DPPVATBankExpenditureNoteId == id)
                .ToList()
                .Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                })
                .ToList();
            _dbContext.DPPVATBankExpenditureNoteItems.UpdateRange(items);

            var details = _dbContext.DPPVATBankExpenditureNoteDetails
                .Where(entity => entity.DPPVATBankExpenditureNoteId == id)
                .ToList()
                .Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                })
                .ToList();
            _dbContext.DPPVATBankExpenditureNoteDetails.UpdateRange(details);

            var detailsDo = _dbContext.DPPVATBankExpenditureNoteDetailDos
                .Where(entity => entity.DPPVATBankExpenditureNoteId == id)
                .ToList()
                .Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                })
                .ToList();
            _dbContext.DPPVATBankExpenditureNoteDetailDos.UpdateRange(detailsDo);

            var existingInternalNoteIds = items.Select(element => element.InternalNoteId).ToList();
            var existingInvoiceNoteIds = details.Select(element => element.InvoiceId).ToList();

            await UpdateInternalNoteInvoiceNoteIsPaid(false, model.Id, model.DocumentNo, existingInternalNoteIds, existingInvoiceNoteIds);

            var formItems = form.Items.Where(item => item.Select);

            if (formItems != null)
            {
                foreach (var formItem in formItems)
                {
                    var item = new DPPVATBankExpenditureNoteItemModel(model.Id, formItem.InternalNote.Id, formItem.InternalNote.DocumentNo, formItem.InternalNote.Date, formItem.InternalNote.DueDate, formItem.InternalNote.Supplier.Id, formItem.InternalNote.Supplier.Name, formItem.InternalNote.Supplier.IsImport, formItem.InternalNote.VATAmount, formItem.InternalNote.IncomeTaxAmount, formItem.InternalNote.DPP, formItem.InternalNote.TotalAmount, formItem.InternalNote.Currency.Id, formItem.InternalNote.Currency.Code, formItem.OutstandingAmount, formItem.InternalNote.Supplier.Code);
                    EntityExtension.FlagForCreate(item, _identityService.Username, UserAgent);
                    _dbContext.DPPVATBankExpenditureNoteItems.Add(item);
                    _dbContext.SaveChanges();

                    var formDetails = formItem.InternalNote.Items.Where(invoiceItem => invoiceItem.SelectInvoice);

                    if (formDetails != null)
                    {
                        foreach (var formDetail in formItem.InternalNote.Items.Where(invoiceItem => invoiceItem.SelectInvoice))
                        {
                            var detailDoJson = JsonConvert.SerializeObject(formDetail.Invoice.DetailDO);
                            var detail = new DPPVATBankExpenditureNoteDetailModel(model.Id, item.Id, formDetail.Invoice.Id, formDetail.Invoice.DocumentNo, formDetail.Invoice.Date, formDetail.Invoice.ProductNames, formDetail.Invoice.Category.Id, formDetail.Invoice.Category.Name, formDetail.Invoice.Amount, formDetail.Invoice.PaymentMethod, formDetail.Invoice.DeliveryOrdersNo, formDetail.Invoice.PaymentBills, formDetail.Invoice.BillsNo,detailDoJson);
                            EntityExtension.FlagForCreate(detail, _identityService.Username, UserAgent);
                            _dbContext.DPPVATBankExpenditureNoteDetails.Add(detail);
                            _dbContext.SaveChanges();

                            var detailDos = formDetail.Invoice.DetailDO;

                            if (detailDos != null)
                            {
                                foreach (var detailDo in formDetail.Invoice.DetailDO)
                                {
                                    var detailDoDd = new DPPVATBankExpenditureNoteDetailDoModel(detailDo.DONo, detailDo.TotalAmount, detailDo.PaymentBill, detailDo.BillNo, detailDo.DOId, detailDo.CurrencyRate);
                                    EntityExtension.FlagForCreate(detailDoDd, _identityService.Username, UserAgent);
                                    _dbContext.DPPVATBankExpenditureNoteDetailDos.Add(detailDoDd);
                                    _dbContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }

            var internalNoteIds = form.Items.Where(element => element.Select).Select(element => element.InternalNote.Id).ToList();
            var invoiceNoteIds = form.Items.Where(element => element.Select).SelectMany(element => element.InternalNote.Items).Where(element => element.SelectInvoice).Select(element => element.Invoice.Id).ToList();

            await UpdateInternalNoteInvoiceNoteIsPaid(true, model.Id, model.DocumentNo, internalNoteIds, invoiceNoteIds);

            return model.Id;
        }

        public List<ReportDto> ExpenditureReport(int expenditureId, int internalNoteId, int invoiceId, int supplierId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var detailQuery = _dbContext.DPPVATBankExpenditureNoteDetails.AsQueryable();
            var itemQuery = _dbContext.DPPVATBankExpenditureNoteItems.AsQueryable();
            var query = _dbContext.DPPVATBankExpenditureNotes.AsQueryable();

            var reportQuery = from detail in detailQuery

                              join item in itemQuery on detail.DPPVATBankExpenditureNoteItemId equals item.Id into itemDetails
                              from itemDetail in itemDetails.DefaultIfEmpty()

                              join document in query on itemDetail.DPPVATBankExpenditureNoteId equals document.Id into documentItems
                              from documentItem in documentItems.DefaultIfEmpty()

                              select new ReportDto(detail, itemDetail, documentItem);

            reportQuery = reportQuery.Where(entity => entity.ExpenditureDate >= startDate && entity.ExpenditureDate <= endDate);

            if (expenditureId > 0)
                reportQuery = reportQuery.Where(entity => entity.ExpenditureId == expenditureId);

            if (internalNoteId > 0)
                reportQuery = reportQuery.Where(entity => entity.InternalNoteId == internalNoteId);

            if (invoiceId > 0)
                reportQuery = reportQuery.Where(entity => entity.InvoiceId == invoiceId);

            if (supplierId > 0)
                reportQuery = reportQuery.Where(entity => entity.SupplierId == supplierId);

            return reportQuery.ToList();
        }

        public List<ReportDto> ExpenditureReportDetailDO(int expenditureId, int internalNoteId, int invoiceId, int supplierId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var detailQuery = _dbContext.DPPVATBankExpenditureNoteDetails.Include(s=> s.DPPVATBankExpenditureNoteDetailDos).AsQueryable();
            var itemQuery = _dbContext.DPPVATBankExpenditureNoteItems.AsQueryable();
            var query = _dbContext.DPPVATBankExpenditureNotes.AsQueryable();
            //var detailDoQuery = _dbContext.DPPVATBankExpenditureNoteDetailDos.AsQueryable();
            var offset = new TimeSpan(_identityService.TimezoneOffset, 0, 0);

            if (expenditureId > 0)
                query = query.Where(entity => entity.Id == expenditureId);

            if (internalNoteId > 0)
                itemQuery = itemQuery.Where(entity => entity.InternalNoteId == internalNoteId);

            if (invoiceId > 0)
                detailQuery = detailQuery.Where(entity => entity.InvoiceId == invoiceId);

            if (supplierId > 0)
                itemQuery = itemQuery.Where(entity => entity.SupplierId == supplierId); ;

            //query = query.Where(entity => entity.Date.ToOffset(offset) >= startDate.ToOffset(offset) && entity.Date.ToOffset(offset) <= endDate.ToOffset(offset));
            query = query.Where(entity => entity.Date >= startDate && entity.Date <= endDate);

            //tmbh ini
            var queryViewModel = query.ToList();
            //------
            //
            //var queryViewModel = query.ToList();
            //var data = queryViewModel.OrderByDescending(s => s.LastModifiedUtc).ToList();
            //foreach (var item in data)
            //{
            //};
            //=----

            //                var reportQuery = from detail in detailQuery
            //                              join item in itemQuery on detail.DPPVATBankExpenditureNoteItemId equals item.Id into itemDetails

            var reportQuery = from detail in detailQuery.ToList() 
                              join item in itemQuery.ToList() on detail.DPPVATBankExpenditureNoteItemId equals item.Id into itemDetails
                              from itemDetail in itemDetails

                                  //join document in query on itemDetail.DPPVATBankExpenditureNoteId equals document.Id into documentItems
                                  
                                  join document in queryViewModel on itemDetail.DPPVATBankExpenditureNoteId equals document.Id into documentItems

                                  from documentItem in documentItems

                              select new ReportDto(detail, itemDetail, documentItem, detail.DPPVATBankExpenditureNoteDetailDos.Select(s=> new ReportDoDetailDto(s.DONo,s.DOId,s.PaymentBill,s.BillNo,s.TotalAmount,s.CurrencyRate)).ToList());

            return reportQuery.ToList();
        }

        public List<ReportDto> ExpenditureReportDetailDOPagination(int page, int size,int expenditureId, int internalNoteId, int invoiceId, int supplierId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var detailQuery = _dbContext.DPPVATBankExpenditureNoteDetails.Include(s => s.DPPVATBankExpenditureNoteDetailDos).AsQueryable();
            var itemQuery = _dbContext.DPPVATBankExpenditureNoteItems.AsQueryable();
            var query = _dbContext.DPPVATBankExpenditureNotes.AsQueryable();
            //var detailDoQuery = _dbContext.DPPVATBankExpenditureNoteDetailDos.AsQueryable();
            var offset = new TimeSpan(_identityService.TimezoneOffset, 0, 0);

            if (expenditureId > 0)
                query = query.Where(entity => entity.Id == expenditureId);

            if (internalNoteId > 0)
                itemQuery = itemQuery.Where(entity => entity.InternalNoteId == internalNoteId);

            if (invoiceId > 0)
                detailQuery = detailQuery.Where(entity => entity.InvoiceId == invoiceId);

            if (supplierId > 0)
                itemQuery = itemQuery.Where(entity => entity.SupplierId == supplierId); ;

            //query = query.Where(entity => entity.Date >= startDate && entity.Date <= endDate);
            query = query.Where(entity => entity.Date.ToOffset(offset) >= startDate.ToOffset(offset) && entity.Date.ToOffset(offset) <= endDate.ToOffset(offset));

            var reportQuery = from detail in detailQuery
                              join item in itemQuery on detail.DPPVATBankExpenditureNoteItemId equals item.Id into itemDetails
                              from itemDetail in itemDetails

                              join document in query on itemDetail.DPPVATBankExpenditureNoteId equals document.Id into documentItems
                              from documentItem in documentItems

                              select new ReportDto(detail, itemDetail, documentItem, detail.DPPVATBankExpenditureNoteDetailDos.Select(s => new ReportDoDetailDto(s.DONo, s.DOId, s.PaymentBill, s.BillNo, s.TotalAmount, s.CurrencyRate)).ToList());

            var paged = reportQuery.Skip((page - 1) * size).Take(size).ToList();

            return paged;
        }

        public async Task<int> Posting(List<int> ids)
        {
            var documents = _dbContext
                .DPPVATBankExpenditureNotes
                .Where(entity => ids.Contains(entity.Id))
                .ToList()
                .Select(element =>
                {
                    element.Posted();
                    EntityExtension.FlagForUpdate(element, _identityService.Username, UserAgent);

                    return element;
                })
                .ToList();
            _dbContext.DPPVATBankExpenditureNotes.UpdateRange(documents);

            foreach (var document in documents)
            {
                var internalNoteIds = _dbContext.DPPVATBankExpenditureNoteItems.Where(entity => entity.DPPVATBankExpenditureNoteId == document.Id).Select(entity => entity.InternalNoteId).ToList();
                var invoiceNoteIds = _dbContext.DPPVATBankExpenditureNoteDetails.Where(entity => entity.DPPVATBankExpenditureNoteId == document.Id).Select(entity => entity.InvoiceId).ToList();

                await UpdateInternalNoteInvoiceNoteIsPaid(true, document.Id, document.DocumentNo, internalNoteIds, invoiceNoteIds);
            }

            return _dbContext.SaveChanges();
        }


        public ReportDto ExpenditureFromInvoice( long InvoiceId)
        {
            var detailQuery = _dbContext.DPPVATBankExpenditureNoteDetails.AsQueryable();
            var itemQuery = _dbContext.DPPVATBankExpenditureNoteItems.AsQueryable();
            var query = _dbContext.DPPVATBankExpenditureNotes.AsQueryable();

            var reportQuery = from detail in detailQuery

                              join item in itemQuery on detail.DPPVATBankExpenditureNoteItemId equals item.Id into itemDetails
                              from itemDetail in itemDetails.DefaultIfEmpty()

                              join document in query on itemDetail.DPPVATBankExpenditureNoteId equals document.Id into documentItems
                              from documentItem in documentItems.DefaultIfEmpty()

                              where detail.InvoiceId == InvoiceId

                              select new ReportDto(detail, itemDetail, documentItem);

            

            return reportQuery.FirstOrDefault();
        }
    }
}
