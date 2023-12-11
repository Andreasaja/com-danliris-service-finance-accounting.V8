using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Memorial;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Memorial;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentFinance.Memorial
{
    public class GarmentFinanceMemorialProfile : Profile
    {
        public GarmentFinanceMemorialProfile()
        {
            CreateMap<GarmentFinanceMemorialModel, GarmentFinanceMemorialViewModel>()
                .ForPath(d => d.GarmentCurrency.Id, opt => opt.MapFrom(s => s.GarmentCurrencyId))
                .ForPath(d => d.GarmentCurrency.Code, opt => opt.MapFrom(s => s.GarmentCurrencyCode))
                .ForPath(d => d.GarmentCurrency.Rate, opt => opt.MapFrom(s => s.GarmentCurrencyRate))

                .ReverseMap();

            CreateMap<GarmentFinanceMemorialItemModel, GarmentFinanceMemorialItemViewModel>()
                .ForPath(d => d.COA.Id, opt => opt.MapFrom(s => s.COAId))
                .ForPath(d => d.COA.Code, opt => opt.MapFrom(s => s.COACode))
                .ForPath(d => d.COA.Name, opt => opt.MapFrom(s => s.COAName))
                .ReverseMap();
        }
    }
}