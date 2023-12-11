using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.Memo;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Memo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.Memo
{
    public class MemoProfile : Profile
    {
        public MemoProfile()
        {
            CreateMap<MemoModel, MemoViewModel>()
                .ForPath(d => d.SalesInvoice.Id, opt => opt.MapFrom(s => s.SalesInvoiceId))
                .ForPath(d => d.SalesInvoice.SalesInvoiceNo, opt => opt.MapFrom(s => s.SalesInvoiceNo))

                .ForPath(d => d.SalesInvoice.Buyer.Id, opt => opt.MapFrom(s => s.BuyerId))
                .ForPath(d => d.SalesInvoice.Buyer.Code, opt => opt.MapFrom(s => s.BuyerCode))
                .ForPath(d => d.SalesInvoice.Buyer.Name, opt => opt.MapFrom(s => s.BuyerName))

                .ForPath(d => d.Unit.Id, opt => opt.MapFrom(s => s.UnitId))
                .ForPath(d => d.Unit.Code, opt => opt.MapFrom(s => s.UnitCode))
                .ForPath(d => d.Unit.Name, opt => opt.MapFrom(s => s.UnitName))

                .ReverseMap();

            CreateMap<MemoItemModel, MemoItemViewModel>()
                .ForPath(d => d.Currency.Id, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.Currency.Code, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.Currency.Rate, opt => opt.MapFrom(s => s.CurrencyRate))

                .ReverseMap();
        }
    }
}
