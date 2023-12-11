using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.PaymentDispositionNote
{
    public class PaymentDispositionNoteProfile : Profile
    {
        public PaymentDispositionNoteProfile()
        {
            CreateMap<PaymentDispositionNoteModel, PaymentDispositionNoteViewModel>()
                .ForPath(d => d.AccountBank.Id, opt => opt.MapFrom(s => s.BankId))
                .ForPath(d => d.AccountBank.Code, opt => opt.MapFrom(s => s.BankCode))
                .ForPath(d => d.AccountBank.AccountName, opt => opt.MapFrom(s => s.BankAccountName))
                .ForPath(d => d.AccountBank.AccountNumber, opt => opt.MapFrom(s => s.BankAccountNumber))
                .ForPath(d => d.AccountBank.BankName, opt => opt.MapFrom(s => s.BankName))
                .ForPath(d => d.AccountBank.Currency.Id, opt => opt.MapFrom(s => s.BankCurrencyId))
                .ForPath(d => d.AccountBank.Currency.Code, opt => opt.MapFrom(s => s.BankCurrencyCode))
                .ForPath(d => d.AccountBank.Currency.Rate, opt => opt.MapFrom(s => s.BankCurrencyRate))

                .ForPath(d => d.Supplier.Id, opt => opt.MapFrom(s => s.SupplierId))
                .ForPath(d => d.Supplier.Code, opt => opt.MapFrom(s => s.SupplierCode))
                .ForPath(d => d.Supplier.Name, opt => opt.MapFrom(s => s.SupplierName))
                .ForPath(d => d.Supplier.Import, opt => opt.MapFrom(s => s.SupplierImport))

                .ReverseMap();

            CreateMap<PaymentDispositionNoteItemModel, PaymentDispositionNoteItemViewModel>()

                .ForPath(d => d.category._id, opt => opt.MapFrom(s => s.CategoryId))
                .ForPath(d => d.category.code, opt => opt.MapFrom(s => s.CategoryCode))
                .ForPath(d => d.category.name, opt => opt.MapFrom(s => s.CategoryName))

                .ForPath(d => d.division._id, opt => opt.MapFrom(s => s.DivisionId))
                .ForPath(d => d.division.code, opt => opt.MapFrom(s => s.DivisionCode))
                .ForPath(d => d.division.name, opt => opt.MapFrom(s => s.DivisionName))

                .ReverseMap();

            CreateMap<PaymentDispositionNoteDetailModel, PaymentDispositionNoteDetailViewModel>()
                .ForPath(d => d.product._id, opt => opt.MapFrom(s => s.ProductId))
                .ForPath(d => d.product.code, opt => opt.MapFrom(s => s.ProductCode))
                .ForPath(d => d.product.name, opt => opt.MapFrom(s => s.ProductName))

                .ForPath(d => d.unit._id, opt => opt.MapFrom(s => s.UnitId))
                .ForPath(d => d.unit.code, opt => opt.MapFrom(s => s.UnitCode))
                .ForPath(d => d.unit.name, opt => opt.MapFrom(s => s.UnitName))

                .ForPath(d => d.uom._id, opt => opt.MapFrom(s => s.UomId))
                .ForPath(d => d.uom.unit, opt => opt.MapFrom(s => s.UomUnit))

                .ReverseMap();
        }
    }
}
