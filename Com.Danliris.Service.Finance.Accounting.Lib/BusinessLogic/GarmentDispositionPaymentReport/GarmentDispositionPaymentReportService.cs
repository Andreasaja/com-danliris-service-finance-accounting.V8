using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.GarmentDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.PurchasingMemoDetailTextile;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Migrations;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
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
                //ini ori
                var expeditionQuery = _dbContext.GarmentDispositionExpeditions.AsQueryable();
               

                //ini ori
                var expenditureQuery = _dbContext.GarmentInvoicePurchasingDispositions.AsQueryable();
                

                switch (position)
                {
                    case GarmentPurchasingExpeditionPosition.SendToVerification:
                        //ini ori
                       // expeditionQuery = expeditionQuery.Where(entity => entity.SendToVerificationDate.HasValue && entity.SendToVerificationDate.GetValueOrDefault() >= startDate && entity.SendToVerificationDate.GetValueOrDefault() <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.SendToVerification);

                        expeditionQuery = expeditionQuery.Where(entity =>  (entity.SendToVerificationDate==null ? DateTimeOffset.MinValue : entity.SendToVerificationDate) >= startDate && (entity.SendToVerificationDate == null ? DateTimeOffset.MinValue : entity.SendToVerificationDate) <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.SendToVerification);
                        break;
                    case GarmentPurchasingExpeditionPosition.VerificationAccepted:
                        //ini ori
                        //expeditionQuery = expeditionQuery.Where(entity => entity.VerificationAcceptedDate.HasValue && entity.VerificationAcceptedDate.GetValueOrDefault() >= startDate && entity.VerificationAcceptedDate.GetValueOrDefault() <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.VerificationAccepted);
                       
                        expeditionQuery = expeditionQuery.Where(entity => ( entity.VerificationAcceptedDate == null ? DateTimeOffset.MinValue : entity.VerificationAcceptedDate) >= startDate && (entity.VerificationAcceptedDate == null ? DateTimeOffset.MinValue : entity.VerificationAcceptedDate) <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.VerificationAccepted);


                        break;
                    case GarmentPurchasingExpeditionPosition.SendToCashier:
                        //ini ori
                        //expeditionQuery = expeditionQuery.Where(entity => entity.SendToCashierDate.HasValue && entity.SendToCashierDate.GetValueOrDefault() >= startDate && entity.SendToCashierDate.GetValueOrDefault() <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier);

                        expeditionQuery = expeditionQuery.Where(entity => ( entity.SendToCashierDate == null ? DateTimeOffset.MinValue : entity.SendToCashierDate) >= startDate && (entity.SendToCashierDate == null ? DateTimeOffset.MinValue : entity.SendToCashierDate) <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.SendToCashier);
                        
                        break;
                    case GarmentPurchasingExpeditionPosition.CashierAccepted:
                       //ini ori
                        // expeditionQuery = expeditionQuery.Where(entity => entity.CashierAcceptedDate.HasValue && entity.CashierAcceptedDate.GetValueOrDefault() >= startDate && entity.CashierAcceptedDate.GetValueOrDefault() <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.CashierAccepted);

                        expeditionQuery = expeditionQuery.Where(entity =>  (entity.CashierAcceptedDate == null ? DateTimeOffset.MinValue : entity.CashierAcceptedDate) >= startDate && (entity.CashierAcceptedDate == null ? DateTimeOffset.MinValue : entity.CashierAcceptedDate) <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.CashierAccepted);
                        break;
                    case GarmentPurchasingExpeditionPosition.SendToPurchasing:
                        //ini ori
                        //expeditionQuery = expeditionQuery.Where(entity => entity.SendToPurchasingDate.HasValue && entity.SendToPurchasingDate.GetValueOrDefault() >= startDate && entity.SendToPurchasingDate.GetValueOrDefault() <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing);


                        expeditionQuery = expeditionQuery.Where(entity =>  (entity.SendToPurchasingDate == null ? DateTimeOffset.MinValue : entity.SendToPurchasingDate) >= startDate && (entity.SendToPurchasingDate == null ? DateTimeOffset.MinValue : entity.SendToPurchasingDate) <= endDate && entity.Position == GarmentPurchasingExpeditionPosition.SendToPurchasing);
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
                    
                    //buka sc vs 2019 versi netcore 2,lalu cari return dari GetDispositions lalu buat statis array di var dispositions
                    //var dispositions = await GetDispositions(startDate, endDate, dispositionIds);
                    //dibawah ini adlh hasil statis array nya
                    //GarmentDispositionDto newdispo = new GarmentDispositionDto
                    //    {
                    //    DispositionId = 2,
                    //    DispositionNoteNo = "23-12-GJ002",
                    //    DispositionNoteDate = DateTimeOffset.Parse("2023-12-20T01:36:07.3924946+07:00"),
                    //    DispositionNoteDueDate = DateTimeOffset.Parse("2023-12-19T17:00:00+00:00"),
                    //    ProformaNo = "0",
                    //    SupplierId = 1505,
                    //    SupplierCode = "AMBAS",
                    //    SupplierName = "PT. AMBASSADOR GARMINDO",
                    //    CurrencyId = 2767,
                    //    CurrencyCode = "IDR",
                    //    CurrencyRate = 1.0,
                    //    DPPAmount = 69875865.0,
                    //    VATAmount = 7686345.15,
                    //    IncomeTaxAmount = 0.0,
                    //    OthersExpenditureAmount = 0.0,
                    //    TotalAmount = 69875865.0,
                    //    CategoryId = 0,
                    //    CategoryCode = "FABRIC",
                    //    CategoryName = "FABRIC",
                    //    ExternalPurchaseOrderId = 19,
                    //    ExternalPurchaseOrderNo = "PO231000004",
                    //    DispositionQuantity = 995.0,
                    //    DeliveryOrderId = 16,
                    //    DeliveryOrderNo = "67751",
                    //    DeliveryOrderQuantity = 995.0,
                    //    PaymentBillsNo = "BB231027001",
                    //    BillsNo = "BP231027091731000004",
                    //    CustomsNoteId = 25,
                    //    CustomsNoteNo = "67554",
                    //    CustomsNoteDate = DateTimeOffset.Parse("2023-10-27T00:00:00+00:00"),
                    //    UnitReceiptNoteId = 0,
                    //    UnitReceiptNoteNo = "",
                    //    InternalNoteId = 6,
                    //    InternalNoteNo = "NI23120002L",
                    //    InternalNoteDate = DateTimeOffset.Parse("2023-12-14T08:07:11.8983548+00:00"),
                    //    DispositionCreatedBy = "AMIN",
                    //};

                    //List<GarmentDispositionDto> dispositions = new List<GarmentDispositionDto>();

                    //dispositions.Add(newdispo);
                    

            

            
             
                        
                   
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
