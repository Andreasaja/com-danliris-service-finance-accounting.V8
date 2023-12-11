using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePayment;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePaymentViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentInvoicePayment
{
    public class GarmentInvoicePaymentProfile : Profile
    {
        public GarmentInvoicePaymentProfile()
        {
            CreateMap<GarmentInvoicePaymentModel, GarmentInvoicePaymentViewModel>()
                .ForPath(d => d.Buyer.Id, opt => opt.MapFrom(s => s.BuyerId))
                .ForPath(d => d.Buyer.Code, opt => opt.MapFrom(s => s.BuyerCode))
                .ForPath(d => d.Buyer.Name, opt => opt.MapFrom(s => s.BuyerName))

                .ForPath(d => d.Currency.Id, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.Currency.Code, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.Currency.Rate, opt => opt.MapFrom(s => s.CurrencyRate))

                .ReverseMap();

            CreateMap<GarmentInvoicePaymentItemModel, GarmentInvoicePaymentItemViewModel>()
                .ReverseMap();
        }
    }
}
