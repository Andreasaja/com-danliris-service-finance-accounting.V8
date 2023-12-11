using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePurchasingDisposition;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePurchasingDisposition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentInvoicePurchasingDisposition
{
    public class GarmentInvoicePurchasingDispositionProfile : Profile
    {
        public GarmentInvoicePurchasingDispositionProfile()
        {
            CreateMap<GarmentInvoicePurchasingDispositionViewModel, GarmentInvoicePurchasingDispositionModel>()
                .ForPath(d => d.BankAccountName, opt => opt.MapFrom(s => s.AccountBank.AccountName))
                .ForPath(d => d.BankAccountNo, opt => opt.MapFrom(s => s.AccountBank.AccountNumber))
                .ForPath(d => d.BankCode, opt => opt.MapFrom(s => s.AccountBank.BankCode))
                .ForPath(d => d.BankCurrencyCode, opt => opt.MapFrom(s => s.AccountBank.Currency.Code))
                .ForPath(d => d.BankCurrencyId, opt => opt.MapFrom(s => s.AccountBank.Currency.Id))
                .ForPath(d => d.BankId, opt => opt.MapFrom(s => s.AccountBank.Id))
                .ForPath(d => d.BankName, opt => opt.MapFrom(s => s.AccountBank.BankName))
                .ForPath(d => d.ChequeNo, opt => opt.MapFrom(s => s.BGCheckNumber))
                .ForPath(d => d.CurrencyCode, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.CurrencyId, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.CurrencyRate, opt => opt.MapFrom(s => s.CurrencyRate))
                .ForPath(d => d.CurrencySymbol, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForPath(d => d.InvoiceDate, opt => opt.MapFrom(s => s.PaymentDate))
                .ForPath(d => d.IsImportSupplier, opt => opt.MapFrom(s => s.Supplier.Import))
                .ForPath(d => d.IsPosted, opt => opt.MapFrom(s => s.IsPosted))
                .ForPath(d => d.Items, opt => opt.MapFrom(s => s.Items))
                .ForPath(d => d.PaymentType, opt => opt.MapFrom(s => s.TransactionType))
                .ForPath(d => d.SupplierCode, opt => opt.MapFrom(s => s.Supplier.Code))
                .ForPath(d => d.SupplierId, opt => opt.MapFrom(s => s.Supplier.Id))
                .ForPath(d => d.SupplierName, opt => opt.MapFrom(s => s.Supplier.Name))
                .ForPath(d => d.SupplierName, opt => opt.MapFrom(s => s.Supplier.Name))
                .ForPath(d => d.InvoiceNo, opt => opt.MapFrom(s => s.PaymentDispositionNo))
                .ReverseMap()
                ;

            CreateMap<GarmentInvoicePurchasingDispositionItemViewModel, GarmentInvoicePurchasingDispositionItemModel>()
                .ForPath(d => d.Category, opt => opt.MapFrom(s => s.Category))
                .ForPath(d => d.CurrencyCode, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.CurrencyId, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.CurrencyRate, opt => opt.MapFrom(s => s.CurrencyRate))
                .ForPath(d => d.DipositionDueDate, opt => opt.MapFrom(s => s.DispositionNoteDueDate))
                .ForPath(d => d.DispositionId, opt => opt.MapFrom(s => s.DispositionNoteId))
                .ForPath(d => d.DispositionNo, opt => opt.MapFrom(s => s.DispositionNoteNo))
                .ForPath(d => d.DispositionDate, opt => opt.MapFrom(s => s.DispositionNoteDate))
                .ForPath(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForPath(d => d.ProformaNo, opt => opt.MapFrom(s => s.ProformaNo))
                .ForPath(d => d.PurchasingDispositionExpeditionId, opt => opt.MapFrom(s => s.purchasingDispositionExpeditionId))
                .ForPath(d => d.SupplierName, opt => opt.MapFrom(s => s.SupplierName))
                .ForPath(d => d.TotalAmount, opt => opt.MapFrom(s => s.TotalPaid))
                .ForPath(d => d.TotalPaidBefore, opt => opt.MapFrom(s => s.TotalPaidPaymentBefore))
                .ForPath(d => d.TotalPaid, opt => opt.MapFrom(s => s.TotalPaidPayment))
                .ForPath(d => d.VATAmount, opt => opt.MapFrom(s => s.VATAmount))
                //.ForPath(d=> d.TotalAmount-(d.TotalPaid+d.TotalPaidBefore), opt=> opt.MapFrom(s=> s.DiffTotalPaidPayment))
                .ReverseMap();


        }
    }
}
