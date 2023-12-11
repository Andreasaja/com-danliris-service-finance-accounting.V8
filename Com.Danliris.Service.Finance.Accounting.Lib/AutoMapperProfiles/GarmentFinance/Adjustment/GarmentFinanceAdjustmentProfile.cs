using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentFinance.Adjustment;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentFinance.Adjustment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.GarmentFinance.Adjustment
{
    public class GarmentFinanceAdjustmentProfile : Profile
    {
        public GarmentFinanceAdjustmentProfile()
        {
            CreateMap<GarmentFinanceAdjustmentModel, GarmentFinanceAdjustmentViewModel>()
                .ForPath(d => d.GarmentCurrency.Id, opt => opt.MapFrom(s => s.GarmentCurrencyId))
                .ForPath(d => d.GarmentCurrency.Code, opt => opt.MapFrom(s => s.GarmentCurrencyCode))
                .ForPath(d => d.GarmentCurrency.Rate, opt => opt.MapFrom(s => s.GarmentCurrencyRate))

                .ReverseMap();

            CreateMap<GarmentFinanceAdjustmentItemModel, GarmentFinanceAdjustmentItemViewModel>()
                .ForPath(d => d.COA.Id, opt => opt.MapFrom(s => s.COAId))
                .ForPath(d => d.COA.Code, opt => opt.MapFrom(s => s.COACode))
                .ForPath(d => d.COA.Name, opt => opt.MapFrom(s => s.COAName))
                .ReverseMap();
        }
    }
}