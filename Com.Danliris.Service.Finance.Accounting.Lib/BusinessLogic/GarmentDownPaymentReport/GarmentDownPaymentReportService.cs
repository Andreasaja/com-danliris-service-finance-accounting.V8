using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDownPaymentReport
{
    public class GarmentDownPaymentReportService : IGarmentDownPaymentReportService
    {
        private readonly FinanceDbContext _dbContext;
        private readonly object _identityService;

        public GarmentDownPaymentReportService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();
        }
        public List<GarmentDownPaymentReportDto> GetReport(SupplierType supplierType, DateTimeOffset date)
        {
            if (supplierType == SupplierType.All)
            {
                var dispositions = _dbContext.GarmentInvoicePurchasingDispositionItems
                        .Where(entity => entity.DispositionDate <= date)
                        .GroupBy(entity => entity.DispositionId)
                        .Select(group => new GarmentDownPaymentReportDto(group.Key, group.FirstOrDefault().DispositionNo, group.FirstOrDefault().DipositionDueDate, group.Select(entity => entity.GarmentInvoicePurchasingDispositionId).ToList()))
                        .ToList();

                var dispositionIds = dispositions.Select(element => element.DispositionId).ToList();

                var dispositionPaymentIds = dispositions.SelectMany(element => element.DispositionPaymentIds).Distinct().ToList();

                var dispositionPayments = _dbContext.GarmentInvoicePurchasingDispositions.Where(entity => dispositionPaymentIds.Contains(entity.Id)).ToList();

                var memoDetailDocumentDispositionItems = _dbContext.MemoDetailGarmentPurchasingDispositions.Where(entity => dispositionIds.Contains(entity.DispositionId)).Select(entity => new { entity.DispositionId, entity.MemoDetailGarmentPurchasingId }).ToList();
                var memoDetailDocumentIds = memoDetailDocumentDispositionItems.Select(entity => entity.MemoDetailGarmentPurchasingId).ToList();
                var memoDetailDocumentDetails = _dbContext.MemoDetailGarmentPurchasingDetails.Where(entity => memoDetailDocumentIds.Contains(entity.MemoDetailId)).Select(entity => new { entity.Id, entity.MemoDetailId, entity.MemoDispositionId, entity.GarmentDeliveryOrderNo }).ToList();
                var memoDetailDocuments = _dbContext.MemoDetailGarmentPurchasings.Where(entity => memoDetailDocumentIds.Contains(entity.Id)).Select(entity => new { entity.Id, entity.MemoId }).ToList();
                var memoDocumentIds = memoDetailDocuments.Select(entity => entity.MemoId).ToList();
                var memoDocuments = _dbContext.MemoGarmentPurchasings.Where(entity => memoDocumentIds.Contains(entity.Id)).ToList();


                foreach (var disposition in dispositions)
                {
                    var selectedDispositionPayments = dispositionPayments.Where(entity => disposition.DispositionPaymentIds.Contains(entity.Id)).ToList();

                    //var selectedMemoDocuments = memoDetailDocumentDetails
                    disposition.DispositionPayments.AddRange(selectedDispositionPayments.Select(element =>
                    {
                        var paymentItems = _dbContext.GarmentInvoicePaymentItems.Where(entity => entity.InvoicePaymentId == element.Id).ToList();
                        return new DispositionPaymentDto(element.Id, element.InvoiceNo, element.InvoiceDate, element.CurrencyDate, element.BankAccountNo, disposition.DispositionNo, element.SupplierName, (disposition.DispositionDueDate.DateTime - DateTime.Now).Days, paymentItems.Sum(item => (double)item.Amount), paymentItems.Sum(item => (double)item.Amount), element.CurrencyRate, paymentItems.Sum(item => (double)item.Amount), paymentItems.Sum(item => (double)item.Amount), paymentItems.Sum(item => (double)item.Amount), element.CurrencyRate, paymentItems.Sum(item => (double)item.Amount), element.CurrencyCode, element.CurrencyRate, paymentItems.Sum(item => (double)item.Amount));
                    }).ToList());
                    var selectedMemoDispositionItems = memoDetailDocumentDispositionItems.Where(element => element.DispositionId == disposition.DispositionId).ToList();
                    var memoDocumentDetailIds = selectedMemoDispositionItems.Select(element => element.MemoDetailGarmentPurchasingId).ToList();
                    var memoIds = memoDetailDocuments.Where(element => memoDocumentDetailIds.Contains(element.Id)).Select(element => element.MemoId).ToList();
                    disposition.MemoDocuments.AddRange(memoDocuments.Where(element => memoIds.Contains(element.Id)).Select(element =>
                    {
                        return new MemoDocumentDto(element.Id, element.MemoNo, element.MemoDate, element.TotalAmount, element.GarmentCurrenciesRate, element.TotalAmount, DateTimeOffset.MinValue, "", DateTimeOffset.MinValue, "", "", "", "", element.GarmentCurrenciesRate, element.TotalAmount, element.GarmentCurrenciesRate);
                    }).ToList());
                }

                return dispositions;
            }
            else if (supplierType == SupplierType.Local)
            {
                var isLocalIds = _dbContext.GarmentInvoicePurchasingDispositions.Where(entity => !entity.IsImportSupplier).Select(entity => entity.Id).ToList();
                var dispositions = _dbContext.GarmentInvoicePurchasingDispositionItems
                        .Where(entity => entity.DispositionDate <= date && isLocalIds.Contains(entity.Id))
                        .GroupBy(entity => entity.DispositionId)
                        .Select(group => new GarmentDownPaymentReportDto(group.Key, group.FirstOrDefault().DispositionNo, group.FirstOrDefault().DipositionDueDate, group.Select(entity => entity.GarmentInvoicePurchasingDispositionId).ToList()))
                        .ToList();

                var dispositionIds = dispositions.Select(element => element.DispositionId).ToList();

                var dispositionPaymentIds = dispositions.SelectMany(element => element.DispositionPaymentIds).Distinct().ToList();

                var dispositionPayments = _dbContext.GarmentInvoicePurchasingDispositions.Where(entity => dispositionPaymentIds.Contains(entity.Id)).ToList();

                var memoDetailDocumentDispositionItems = _dbContext.MemoDetailGarmentPurchasingDispositions.Where(entity => dispositionIds.Contains(entity.DispositionId)).Select(entity => new { entity.DispositionId, entity.MemoDetailGarmentPurchasingId }).ToList();
                var memoDetailDocumentIds = memoDetailDocumentDispositionItems.Select(entity => entity.MemoDetailGarmentPurchasingId).ToList();
                var memoDetailDocumentDetails = _dbContext.MemoDetailGarmentPurchasingDetails.Where(entity => memoDetailDocumentIds.Contains(entity.MemoDetailId)).Select(entity => new { entity.Id, entity.MemoDetailId, entity.MemoDispositionId, entity.GarmentDeliveryOrderNo }).ToList();
                var memoDetailDocuments = _dbContext.MemoDetailGarmentPurchasings.Where(entity => memoDetailDocumentIds.Contains(entity.Id)).Select(entity => new { entity.Id, entity.MemoId }).ToList();
                var memoDocumentIds = memoDetailDocuments.Select(entity => entity.MemoId).ToList();
                var memoDocuments = _dbContext.MemoGarmentPurchasings.Where(entity => memoDocumentIds.Contains(entity.Id)).ToList();


                foreach (var disposition in dispositions)
                {
                    var selectedDispositionPayments = dispositionPayments.Where(entity => disposition.DispositionPaymentIds.Contains(entity.Id)).ToList();

                    //var selectedMemoDocuments = memoDetailDocumentDetails
                    disposition.DispositionPayments.AddRange(selectedDispositionPayments.Select(element =>
                    {
                        var paymentItems = _dbContext.GarmentInvoicePaymentItems.Where(entity => entity.InvoicePaymentId == element.Id).ToList();
                        return new DispositionPaymentDto(element.Id, element.InvoiceNo, element.InvoiceDate, element.CurrencyDate, element.BankAccountNo, disposition.DispositionNo, element.SupplierName, (disposition.DispositionDueDate.DateTime - DateTime.Now).Days, paymentItems.Sum(item => (double)item.Amount), paymentItems.Sum(item => (double)item.Amount), element.CurrencyRate, paymentItems.Sum(item => (double)item.Amount), paymentItems.Sum(item => (double)item.Amount), paymentItems.Sum(item => (double)item.Amount), element.CurrencyRate, paymentItems.Sum(item => (double)item.Amount), element.CurrencyCode, element.CurrencyRate, paymentItems.Sum(item => (double)item.Amount));
                    }).ToList());
                    var selectedMemoDispositionItems = memoDetailDocumentDispositionItems.Where(element => element.DispositionId == disposition.DispositionId).ToList();
                    var memoDocumentDetailIds = selectedMemoDispositionItems.Select(element => element.MemoDetailGarmentPurchasingId).ToList();
                    var memoIds = memoDetailDocuments.Where(element => memoDocumentDetailIds.Contains(element.Id)).Select(element => element.MemoId).ToList();
                    disposition.MemoDocuments.AddRange(memoDocuments.Where(element => memoIds.Contains(element.Id)).Select(element =>
                    {
                        return new MemoDocumentDto(element.Id, element.MemoNo, element.MemoDate, element.TotalAmount, element.GarmentCurrenciesRate, element.TotalAmount, DateTimeOffset.MinValue, "", DateTimeOffset.MinValue, "", "", "", "", element.GarmentCurrenciesRate, element.TotalAmount, element.GarmentCurrenciesRate);
                    }).ToList());
                }

                return dispositions;
            }
            else
            {
                var isImportIds = _dbContext.GarmentInvoicePurchasingDispositions.Where(entity => entity.IsImportSupplier).Select(entity => entity.Id).ToList();
                var dispositions = _dbContext.GarmentInvoicePurchasingDispositionItems
                        .Where(entity => entity.DispositionDate <= date && isImportIds.Contains(entity.Id))
                        .GroupBy(entity => entity.DispositionId)
                        .Select(group => new GarmentDownPaymentReportDto(group.Key, group.FirstOrDefault().DispositionNo, group.FirstOrDefault().DipositionDueDate, group.Select(entity => entity.GarmentInvoicePurchasingDispositionId).ToList()))
                        .ToList();


                var dispositionIds = dispositions.Select(element => element.DispositionId).ToList();

                var dispositionPaymentIds = dispositions.SelectMany(element => element.DispositionPaymentIds).Distinct().ToList();

                var dispositionPayments = _dbContext.GarmentInvoicePurchasingDispositions.Where(entity => dispositionPaymentIds.Contains(entity.Id)).ToList();

                var memoDetailDocumentDispositionItems = _dbContext.MemoDetailGarmentPurchasingDispositions.Where(entity => dispositionIds.Contains(entity.DispositionId)).Select(entity => new { entity.DispositionId, entity.MemoDetailGarmentPurchasingId }).ToList();
                var memoDetailDocumentIds = memoDetailDocumentDispositionItems.Select(entity => entity.MemoDetailGarmentPurchasingId).ToList();
                var memoDetailDocumentDetails = _dbContext.MemoDetailGarmentPurchasingDetails.Where(entity => memoDetailDocumentIds.Contains(entity.MemoDetailId)).Select(entity => new { entity.Id, entity.MemoDetailId, entity.MemoDispositionId, entity.GarmentDeliveryOrderNo }).ToList();
                var memoDetailDocuments = _dbContext.MemoDetailGarmentPurchasings.Where(entity => memoDetailDocumentIds.Contains(entity.Id)).Select(entity => new { entity.Id, entity.MemoId }).ToList();
                var memoDocumentIds = memoDetailDocuments.Select(entity => entity.MemoId).ToList();
                var memoDocuments = _dbContext.MemoGarmentPurchasings.Where(entity => memoDocumentIds.Contains(entity.Id)).ToList();


                foreach (var disposition in dispositions)
                {
                    var selectedDispositionPayments = dispositionPayments.Where(entity => disposition.DispositionPaymentIds.Contains(entity.Id)).ToList();

                    //var selectedMemoDocuments = memoDetailDocumentDetails
                    disposition.DispositionPayments.AddRange(selectedDispositionPayments.Select(element =>
                    {
                        var paymentItems = _dbContext.GarmentInvoicePaymentItems.Where(entity => entity.InvoicePaymentId == element.Id).ToList();
                        return new DispositionPaymentDto(element.Id, element.InvoiceNo, element.InvoiceDate, element.CurrencyDate, element.BankAccountNo, disposition.DispositionNo, element.SupplierName, (disposition.DispositionDueDate.DateTime - DateTime.Now).Days, paymentItems.Sum(item => (double)item.Amount), paymentItems.Sum(item => (double)item.Amount), element.CurrencyRate, paymentItems.Sum(item => (double)item.Amount), paymentItems.Sum(item => (double)item.Amount), paymentItems.Sum(item => (double)item.Amount), element.CurrencyRate, paymentItems.Sum(item => (double)item.Amount), element.CurrencyCode, element.CurrencyRate, paymentItems.Sum(item => (double)item.Amount));
                    }).ToList());
                    var selectedMemoDispositionItems = memoDetailDocumentDispositionItems.Where(element => element.DispositionId == disposition.DispositionId).ToList();
                    var memoDocumentDetailIds = selectedMemoDispositionItems.Select(element => element.MemoDetailGarmentPurchasingId).ToList();
                    var memoIds = memoDetailDocuments.Where(element => memoDocumentDetailIds.Contains(element.Id)).Select(element => element.MemoId).ToList();
                    disposition.MemoDocuments.AddRange(memoDocuments.Where(element => memoIds.Contains(element.Id)).Select(element =>
                    {
                        return new MemoDocumentDto(element.Id, element.MemoNo, element.MemoDate, element.TotalAmount, element.GarmentCurrenciesRate, element.TotalAmount, DateTimeOffset.MinValue, "", DateTimeOffset.MinValue, "", "", "", "", element.GarmentCurrenciesRate, element.TotalAmount, element.GarmentCurrenciesRate);
                    }).ToList());
                }

                return dispositions;
            }
        }
    }
}
