using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionPaymentReport
{
    public class GarmentDispositionPaymentReportService : IGarmentDispositionPaymentReportService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly FinanceDbContext _dbContext;

        public GarmentDispositionPaymentReportService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
        }

        public List<PositionOption> GetPositionOptions()
        {
            var result = new List<PositionOption>();

            var positions = Enum.GetValues(typeof(GarmentPurchasingExpeditionPosition)).Cast<GarmentPurchasingExpeditionPosition>();

            foreach (var position in positions)
            {
                if (position == GarmentPurchasingExpeditionPosition.AccountingAccepted || position == GarmentPurchasingExpeditionPosition.SendToAccounting)
                    continue;
                result.Add(new PositionOption(position));
            }

            return result;
        }

        public async Task<List<GarmentDispositionPaymentReportDto>> GetReport(int dispositionId, int epoId, int supplierId, GarmentPurchasingExpeditionPosition position, string purchasingStaff, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var result = new List<GarmentDispositionPaymentReportDto>();
            if (position <= GarmentPurchasingExpeditionPosition.Purchasing)
            {
                var dispositions = await GetDispositions(startDate, endDate, new List<int>());

                if (dispositionId > 0)
                    dispositions = dispositions.Where(element => element.DispositionId == dispositionId).ToList();

                if (epoId > 0)
                    dispositions = dispositions.Where(entity => entity.ExternalPurchaseOrderId == epoId).ToList();

                if (supplierId > 0)
                    dispositions = dispositions.Where(entity => entity.SupplierId == supplierId).ToList();

                if (!string.IsNullOrWhiteSpace(purchasingStaff))
                    dispositions = dispositions.Where(entity => entity.DispositionCreatedBy == purchasingStaff).ToList();

                var dispositionIds = dispositions.Select(element => element.DispositionId).ToList();
                var expeditions = _dbContext.GarmentDispositionExpeditions.Where(entity => dispositionIds.Contains(entity.DispositionNoteId)).ToList();
                var dispositionPaymentItems = _dbContext.GarmentInvoicePurchasingDispositionItems.Where(entity => dispositionIds.Contains(entity.DispositionId)).ToList();
                var dispositionPaymentIds = dispositionPaymentItems.Select(element => element.GarmentInvoicePurchasingDispositionId).ToList();
                var dispositionPayments = _dbContext.GarmentInvoicePurchasingDispositions.Where(entity => dispositionPaymentIds.Contains(entity.Id)).ToList();

                foreach (var disposition in dispositions)
                {
                    var selectedExpeditions = expeditions.Where(element => element.DispositionNoteId == disposition.DispositionId).ToList();
                    if (selectedExpeditions.Count > 0)
                    {
                        foreach (var expedition in selectedExpeditions)
                        {
                            var paymentItems = dispositionPaymentItems.Where(element => element.DispositionId == disposition.DispositionId).ToList();
                            if (paymentItems != null && paymentItems.Count > 0)
                            {
                                //payment.InvoiceDate, payment.InvoiceNo, paymentItem.TotalPaid
                                var invoicesDate = "";
                                var paymentInvoicesNo = "";
                                var paymentTotalPaid = "";
                                foreach (var paymentItem in paymentItems)
                                {
                                    var payment = dispositionPayments.FirstOrDefault(element => element.Id == paymentItem.GarmentInvoicePurchasingDispositionId);
                                    invoicesDate += $"- {payment.InvoiceDate:dd/MM/yyyy}\n";
                                    paymentInvoicesNo += $"- {payment.InvoiceNo}\n";
                                    paymentTotalPaid += $"- {paymentItem.TotalPaid:N2}\n";
                                }

                                result.Add(new GarmentDispositionPaymentReportDto(dispositionId, disposition.DispositionNoteNo, disposition.DispositionNoteDate, disposition.DispositionNoteDueDate, disposition.ProformaNo, disposition.SupplierId, disposition.SupplierCode, disposition.SupplierName, disposition.CurrencyId, disposition.CurrencyCode, disposition.CurrencyRate, disposition.DPPAmount, 0, disposition.VATAmount, 0, disposition.IncomeTaxAmount, 0, disposition.OthersExpenditureAmount, disposition.TotalAmount, 0, disposition.CategoryCode, disposition.CategoryName, GarmentPurchasingExpeditionPosition.DispositionPayment, expedition.SendToPurchasingRemark, expedition.SendToVerificationDate, expedition.VerificationAcceptedDate, expedition.VerifiedBy, expedition.CashierAcceptedDate, invoicesDate, paymentInvoicesNo, paymentTotalPaid, disposition.ExternalPurchaseOrderId, disposition.ExternalPurchaseOrderNo, disposition.DispositionQuantity, disposition.DeliveryOrderId, disposition.DeliveryOrderNo, disposition.DeliveryOrderQuantity, disposition.PaymentBillsNo, disposition.BillsNo, disposition.CustomsNoteId, disposition.CustomsNoteNo, disposition.CustomsNoteDate, disposition.UnitReceiptNoteId, disposition.UnitReceiptNoteNo, disposition.InternalNoteId, disposition.InternalNoteNo, disposition.InternalNoteDate, expedition.SendToVerificationBy, expedition.VerifiedDate, expedition.Remark, disposition.DispositionCreatedBy));
                            }
                            else
                            {
                                result.Add(new GarmentDispositionPaymentReportDto(dispositionId, disposition.DispositionNoteNo, disposition.DispositionNoteDate, disposition.DispositionNoteDueDate, disposition.ProformaNo, disposition.SupplierId, disposition.SupplierCode, disposition.SupplierName, disposition.CurrencyId, disposition.CurrencyCode, disposition.CurrencyRate, disposition.DPPAmount, 0, disposition.VATAmount, 0, disposition.IncomeTaxAmount, 0, disposition.OthersExpenditureAmount, disposition.TotalAmount, disposition.CategoryId, disposition.CategoryCode, disposition.CategoryName, expedition.Position, expedition.SendToPurchasingRemark, expedition.SendToVerificationDate, expedition.VerificationAcceptedDate, expedition.VerifiedBy, expedition.CashierAcceptedDate, null, null, null, disposition.ExternalPurchaseOrderId, disposition.ExternalPurchaseOrderNo, disposition.DispositionQuantity, disposition.DeliveryOrderId, disposition.DeliveryOrderNo, disposition.DeliveryOrderQuantity, disposition.PaymentBillsNo, disposition.BillsNo, disposition.CustomsNoteId, disposition.CustomsNoteNo, disposition.CustomsNoteDate, disposition.UnitReceiptNoteId, disposition.UnitReceiptNoteNo, disposition.InternalNoteId, disposition.InternalNoteNo, disposition.InternalNoteDate, expedition.SendToVerificationBy, expedition.VerifiedDate, expedition.Remark, disposition.DispositionCreatedBy));
                            }
                        }
                    }
                    else
                    {
                        result.Add(new GarmentDispositionPaymentReportDto(dispositionId, disposition.DispositionNoteNo, disposition.DispositionNoteDate, disposition.DispositionNoteDueDate, disposition.ProformaNo, disposition.SupplierId, disposition.SupplierCode, disposition.SupplierName, disposition.CurrencyId, disposition.CurrencyCode, disposition.CurrencyRate, disposition.DPPAmount, 0, disposition.VATAmount, 0, disposition.IncomeTaxAmount, 0, disposition.OthersExpenditureAmount, disposition.TotalAmount, disposition.CategoryId, disposition.CategoryCode, disposition.CategoryName, GarmentPurchasingExpeditionPosition.Purchasing, null, null, null, null, null, null, null, null, disposition.ExternalPurchaseOrderId, disposition.ExternalPurchaseOrderNo, disposition.DispositionQuantity, disposition.DeliveryOrderId, disposition.DeliveryOrderNo, disposition.DeliveryOrderQuantity, disposition.PaymentBillsNo, disposition.BillsNo, disposition.CustomsNoteId, disposition.CustomsNoteNo, disposition.CustomsNoteDate, disposition.UnitReceiptNoteId, disposition.UnitReceiptNoteNo, disposition.InternalNoteId, disposition.InternalNoteNo, disposition.InternalNoteDate, disposition.DispositionCreatedBy, null, null, disposition.DispositionCreatedBy));
                    }
                }
            }
            else
            {
                var expeditionQuery = _dbContext.GarmentDispositionExpeditions.AsQueryable();
                var expenditureQuery = _dbContext.GarmentInvoicePurchasingDispositions.AsQueryable();

                switch (position)
                {
                    case GarmentPurchasingExpeditionPosition.SendToVerification:
                        expeditionQuery = expeditionQuery.Where(entity => entity.SendToVerificationDate.HasValue && entity.SendToVerificationDate.GetValueOrDefault() >= startDate && entity.SendToVerificationDate.GetValueOrDefault() <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.SendToVerification);
                        break;
                    case GarmentPurchasingExpeditionPosition.VerificationAccepted:
                        expeditionQuery = expeditionQuery.Where(entity => entity.VerificationAcceptedDate.HasValue && entity.VerificationAcceptedDate.GetValueOrDefault() >= startDate && entity.VerificationAcceptedDate.GetValueOrDefault() <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.VerificationAccepted);
                        break;
                    case GarmentPurchasingExpeditionPosition.SendToCashier:
                        expeditionQuery = expeditionQuery.Where(entity => entity.SendToCashierDate.HasValue && entity.SendToCashierDate.GetValueOrDefault() >= startDate && entity.SendToCashierDate.GetValueOrDefault() <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier);
                        break;
                    case GarmentPurchasingExpeditionPosition.CashierAccepted:
                        expeditionQuery = expeditionQuery.Where(entity => entity.CashierAcceptedDate.HasValue && entity.CashierAcceptedDate.GetValueOrDefault() >= startDate && entity.CashierAcceptedDate.GetValueOrDefault() <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.CashierAccepted);
                        break;
                    case GarmentPurchasingExpeditionPosition.SendToPurchasing:
                        expeditionQuery = expeditionQuery.Where(entity => entity.SendToPurchasingDate.HasValue && entity.SendToPurchasingDate.GetValueOrDefault() >= startDate && entity.SendToPurchasingDate.GetValueOrDefault() <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing);
                        break;
                    case GarmentPurchasingExpeditionPosition.DispositionPayment:
                        expenditureQuery = expenditureQuery.Where(entity => entity.InvoiceDate >= startDate && entity.InvoiceDate <= endDate);
                        break;
                }

                if (position == GarmentPurchasingExpeditionPosition.DispositionPayment)
                {
                    var dispositionPayments = expenditureQuery.ToList();
                    var dispositionPaymentIds = dispositionPayments.Select(element => element.Id).ToList();
                    var dispositionPaymentItems = _dbContext.GarmentInvoicePurchasingDispositionItems.Where(entity => dispositionPaymentIds.Contains(entity.GarmentInvoicePurchasingDispositionId)).ToList();
                    var dispositionIds = dispositionPaymentItems.Select(element => element.DispositionId).ToList();
                    var dispositions = await GetDispositions(startDate, endDate, dispositionIds);

                    if (dispositionId > 0)
                        dispositions = dispositions.Where(element => element.DispositionId == dispositionId).ToList();

                    if (epoId > 0)
                        dispositions = dispositions.Where(entity => entity.ExternalPurchaseOrderId == epoId).ToList();

                    if (supplierId > 0)
                        dispositions = dispositions.Where(entity => entity.SupplierId == supplierId).ToList();

                    if (!string.IsNullOrWhiteSpace(purchasingStaff))
                        dispositions = dispositions.Where(entity => entity.DispositionCreatedBy == purchasingStaff).ToList();

                    var expeditions = _dbContext.GarmentDispositionExpeditions.Where(entity => dispositionIds.Contains(entity.DispositionNoteId) && entity.Position >= GarmentPurchasingExpeditionPosition.CashierAccepted).ToList();

                    foreach (var disposition in dispositions)
                    {
                        var selectedExpeditions = expeditions.Where(element => element.DispositionNoteId == disposition.DispositionId).ToList();
                        foreach (var expedition in selectedExpeditions)
                        {
                            var paymentItems = dispositionPaymentItems.Where(element => element.DispositionId == disposition.DispositionId).ToList();
                            if (paymentItems != null && paymentItems.Count > 0)
                            {
                                var invoicesDate = "";
                                var paymentInvoicesNo = "";
                                var paymentTotalPaid = "";
                                foreach (var paymentItem in paymentItems)
                                {
                                    var payment = dispositionPayments.FirstOrDefault(element => element.Id == paymentItem.GarmentInvoicePurchasingDispositionId);
                                    invoicesDate += $"- {payment.InvoiceDate:dd/MM/yyyy}\n";
                                    paymentInvoicesNo += $"- {payment.InvoiceNo}\n";
                                    paymentTotalPaid += $"- {paymentItem.TotalPaid:N2}\n";
                                }

                                result.Add(new GarmentDispositionPaymentReportDto(dispositionId, disposition.DispositionNoteNo, disposition.DispositionNoteDate, disposition.DispositionNoteDueDate, disposition.ProformaNo, disposition.SupplierId, disposition.SupplierCode, disposition.SupplierName, disposition.CurrencyId, disposition.CurrencyCode, disposition.CurrencyRate, disposition.DPPAmount, 0, disposition.VATAmount, 0, disposition.IncomeTaxAmount, 0, disposition.OthersExpenditureAmount, disposition.TotalAmount, 0, disposition.CategoryCode, disposition.CategoryName, GarmentPurchasingExpeditionPosition.DispositionPayment, expedition.SendToPurchasingRemark, expedition.SendToVerificationDate, expedition.VerificationAcceptedDate, expedition.VerifiedBy, expedition.CashierAcceptedDate, invoicesDate, paymentInvoicesNo, paymentTotalPaid, disposition.ExternalPurchaseOrderId, disposition.ExternalPurchaseOrderNo, disposition.DispositionQuantity, disposition.DeliveryOrderId, disposition.DeliveryOrderNo, disposition.DeliveryOrderQuantity, disposition.PaymentBillsNo, disposition.BillsNo, disposition.CustomsNoteId, disposition.CustomsNoteNo, disposition.CustomsNoteDate, disposition.UnitReceiptNoteId, disposition.UnitReceiptNoteNo, disposition.InternalNoteId, disposition.InternalNoteNo, disposition.InternalNoteDate, expedition.SendToVerificationBy, expedition.VerifiedDate, expedition.Remark, disposition.DispositionCreatedBy));
                            }
                            else
                            {
                                result.Add(new GarmentDispositionPaymentReportDto(dispositionId, disposition.DispositionNoteNo, disposition.DispositionNoteDate, disposition.DispositionNoteDueDate, disposition.ProformaNo, disposition.SupplierId, disposition.SupplierCode, disposition.SupplierName, disposition.CurrencyId, disposition.CurrencyCode, disposition.CurrencyRate, disposition.DPPAmount, 0, disposition.VATAmount, 0, disposition.IncomeTaxAmount, 0, disposition.OthersExpenditureAmount, disposition.TotalAmount, disposition.CategoryId, disposition.CategoryCode, disposition.CategoryName, expedition.Position, expedition.SendToPurchasingRemark, expedition.SendToVerificationDate, expedition.VerificationAcceptedDate, expedition.VerifiedBy, expedition.CashierAcceptedDate, null, null, null, disposition.ExternalPurchaseOrderId, disposition.ExternalPurchaseOrderNo, disposition.DispositionQuantity, disposition.DeliveryOrderId, disposition.DeliveryOrderNo, disposition.DeliveryOrderQuantity, disposition.PaymentBillsNo, disposition.BillsNo, disposition.CustomsNoteId, disposition.CustomsNoteNo, disposition.CustomsNoteDate, disposition.UnitReceiptNoteId, disposition.UnitReceiptNoteNo, disposition.InternalNoteId, disposition.InternalNoteNo, disposition.InternalNoteDate, expedition.SendToVerificationBy, expedition.VerifiedDate, expedition.Remark, disposition.DispositionCreatedBy));
                            }
                        }
                    }
                }
                else
                {
                    var expeditions = expeditionQuery.ToList();
                    var dispositionIds = expeditions.Select(element => element.DispositionNoteId).ToList();
                    var dispositions = await GetDispositions(startDate, endDate, dispositionIds);

                    if (dispositionId > 0)
                        dispositions = dispositions.Where(element => element.DispositionId == dispositionId).ToList();

                    if (epoId > 0)
                        dispositions = dispositions.Where(entity => entity.ExternalPurchaseOrderId == epoId).ToList();

                    if (supplierId > 0)
                        dispositions = dispositions.Where(entity => entity.SupplierId == supplierId).ToList();

                    if (!string.IsNullOrWhiteSpace(purchasingStaff))
                        dispositions = dispositions.Where(entity => entity.DispositionCreatedBy == purchasingStaff).ToList();

                    var dispositionPaymentItems = _dbContext.GarmentInvoicePurchasingDispositionItems.Where(entity => dispositionIds.Contains(entity.DispositionId)).ToList();
                    var dispositionPaymentIds = dispositionPaymentItems.Select(element => element.GarmentInvoicePurchasingDispositionId).ToList();
                    var dispositionPayments = _dbContext.GarmentInvoicePurchasingDispositions.Where(entity => dispositionPaymentIds.Contains(entity.Id)).ToList();

                    foreach (var disposition in dispositions)
                    {
                        var selectedExpeditions = expeditions.Where(element => element.DispositionNoteId == disposition.DispositionId).ToList();
                        foreach (var expedition in selectedExpeditions)
                        {
                            var paymentItems = dispositionPaymentItems.Where(element => element.DispositionId == disposition.DispositionId).ToList();
                            if (paymentItems != null && paymentItems.Count > 0)
                            {
                                var invoicesDate = "";
                                var paymentInvoicesNo = "";
                                var paymentTotalPaid = "";
                                foreach (var paymentItem in paymentItems)
                                {
                                    var payment = dispositionPayments.FirstOrDefault(element => element.Id == paymentItem.GarmentInvoicePurchasingDispositionId);
                                    invoicesDate += $"- {payment.InvoiceDate:dd/MM/yyyy}\n";
                                    paymentInvoicesNo += $"- {payment.InvoiceNo}\n";
                                    paymentTotalPaid += $"- {paymentItem.TotalPaid:N2}\n";
                                }

                                result.Add(new GarmentDispositionPaymentReportDto(dispositionId, disposition.DispositionNoteNo, disposition.DispositionNoteDate, disposition.DispositionNoteDueDate, disposition.ProformaNo, disposition.SupplierId, disposition.SupplierCode, disposition.SupplierName, disposition.CurrencyId, disposition.CurrencyCode, disposition.CurrencyRate, disposition.DPPAmount, 0, disposition.VATAmount, 0, disposition.IncomeTaxAmount, 0, disposition.OthersExpenditureAmount, disposition.TotalAmount, 0, disposition.CategoryCode, disposition.CategoryName, GarmentPurchasingExpeditionPosition.DispositionPayment, expedition.SendToPurchasingRemark, expedition.SendToVerificationDate, expedition.VerificationAcceptedDate, expedition.VerifiedBy, expedition.CashierAcceptedDate, invoicesDate, paymentInvoicesNo, paymentTotalPaid, disposition.ExternalPurchaseOrderId, disposition.ExternalPurchaseOrderNo, disposition.DispositionQuantity, disposition.DeliveryOrderId, disposition.DeliveryOrderNo, disposition.DeliveryOrderQuantity, disposition.PaymentBillsNo, disposition.BillsNo, disposition.CustomsNoteId, disposition.CustomsNoteNo, disposition.CustomsNoteDate, disposition.UnitReceiptNoteId, disposition.UnitReceiptNoteNo, disposition.InternalNoteId, disposition.InternalNoteNo, disposition.InternalNoteDate, expedition.SendToVerificationBy, expedition.VerifiedDate, expedition.Remark, disposition.DispositionCreatedBy));
                            }
                            else
                            {
                                result.Add(new GarmentDispositionPaymentReportDto(dispositionId, disposition.DispositionNoteNo, disposition.DispositionNoteDate, disposition.DispositionNoteDueDate, disposition.ProformaNo, disposition.SupplierId, disposition.SupplierCode, disposition.SupplierName, disposition.CurrencyId, disposition.CurrencyCode, disposition.CurrencyRate, disposition.DPPAmount, 0, disposition.VATAmount, 0, disposition.IncomeTaxAmount, 0, disposition.OthersExpenditureAmount, disposition.TotalAmount, disposition.CategoryId, disposition.CategoryCode, disposition.CategoryName, expedition.Position, expedition.SendToPurchasingRemark, expedition.SendToVerificationDate, expedition.VerificationAcceptedDate, expedition.VerifiedBy, expedition.CashierAcceptedDate, null, null, null, disposition.ExternalPurchaseOrderId, disposition.ExternalPurchaseOrderNo, disposition.DispositionQuantity, disposition.DeliveryOrderId, disposition.DeliveryOrderNo, disposition.DeliveryOrderQuantity, disposition.PaymentBillsNo, disposition.BillsNo, disposition.CustomsNoteId, disposition.CustomsNoteNo, disposition.CustomsNoteDate, disposition.UnitReceiptNoteId, disposition.UnitReceiptNoteNo, disposition.InternalNoteId, disposition.InternalNoteNo, disposition.InternalNoteDate, expedition.SendToVerificationBy, expedition.VerifiedDate, expedition.Remark, disposition.DispositionCreatedBy));
                            }
                        }
                    }
                }
            }

            //override filter position
            if (position != GarmentPurchasingExpeditionPosition.Invalid)
                result = result.Where(s => s.Position == position).ToList();

            return result;
        }

        private async Task<List<GarmentDispositionDto>> GetDispositions(DateTimeOffset startDate, DateTimeOffset endDate, List<int> dispositionIds)
        {
            if (dispositionIds == null)
                dispositionIds = new List<int>();


            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.Purchasing + $"garment-purchasing-expeditions/report/disposition-payment?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&dispositionIds={JsonConvert.SerializeObject(dispositionIds)}";
            var response = await http.GetAsync(uri);

            var result = new BaseResponse<List<GarmentDispositionDto>>();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<BaseResponse<List<GarmentDispositionDto>>>(responseContent, jsonSerializerSettings);
            }

            return result.data;
        }
    }
}
