using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.VBWithPORequest
{
    public class VBWithPORequestProfile : Profile
    {
        public VBWithPORequestProfile()
        {
            CreateMap<VbRequestModel, VbWithPORequestViewModel>()

                .ForPath(d => d.Unit.Id, opt => opt.MapFrom(s => s.UnitId))
                .ForPath(d => d.Unit.Code, opt => opt.MapFrom(s => s.UnitCode))    
                .ForPath(d => d.Unit.Name, opt => opt.MapFrom(s => s.UnitName))

                .ForPath(d => d.Currency.Id, opt => opt.MapFrom(s => s.CurrencyId))
                .ForPath(d => d.Currency.Code, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.Currency.Rate, opt => opt.MapFrom(s => s.CurrencyRate))
                .ForPath(d => d.Currency.Symbol, opt => opt.MapFrom(s => s.CurrencySymbol))
                .ForPath(d => d.Currency.Description, opt => opt.MapFrom(s => s.CurrencyDescription))

                .ForPath(d => d.Division.Id, opt => opt.MapFrom(s => s.UnitDivisionId))
                .ForPath(d => d.Division.Name, opt => opt.MapFrom(s => s.UnitDivisionName))
                .ReverseMap();
        }
    }
}
