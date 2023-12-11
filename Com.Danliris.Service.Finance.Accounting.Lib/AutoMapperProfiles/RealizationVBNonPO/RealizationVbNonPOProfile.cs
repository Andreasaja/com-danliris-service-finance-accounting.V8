using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBNonPO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.RealizationVBNonPO
{
    public class RealizationVbNonPOProfile : Profile
    {
        public RealizationVbNonPOProfile()
        {
            CreateMap<RealizationVbModel, RealizationVbNonPOViewModel>()
                .ForPath(d => d.numberVB.VBNo, opt => opt.MapFrom(s => s.VBNo))
                .ForPath(d => d.numberVB.DateEstimate, opt => opt.MapFrom(s => s.DateEstimate))
                .ForPath(d => d.numberVB.CreatedBy, opt => opt.MapFrom(s => s.RequestVbName))
                .ForPath(d => d.numberVB.UnitCode, opt => opt.MapFrom(s => s.UnitCode))
                .ForPath(d => d.numberVB.UnitName, opt => opt.MapFrom(s => s.UnitName))
                .ForPath(d => d.numberVB.Date, opt => opt.MapFrom(s => s.DateVB))
                .ForPath(d => d.numberVB.UnitLoad, opt => opt.MapFrom(s => s.UnitLoad))
                .ForPath(d => d.numberVB.Amount, opt => opt.MapFrom(s => s.Amount_VB))
                .ForPath(d => d.numberVB.CurrencyCode, opt => opt.MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.numberVB.CurrencyRate, opt => opt.MapFrom(s => s.CurrencyRate))
                .ForPath(d => d.numberVB.CurrencySymbol, opt => opt.MapFrom(s => s.CurrencySymbol))
                .ForPath(d => d.numberVB.CurrencyDescription, opt => opt.MapFrom(s => s.CurrencyDescription))
                .ForPath(d => d.numberVB.VBRequestCategory, opt => opt.MapFrom(s => s.VBRealizeCategory))
                .ForPath(d => d.numberVB.Usage, opt => opt.MapFrom(s => s.UsageVBRequest))
                .ForPath(d => d.numberVB.UnitDivisionId, opt => opt.MapFrom(s => s.DivisionId))
                .ForPath(d => d.numberVB.UnitDivisionName, opt => opt.MapFrom(s => s.DivisionName))
            .ReverseMap();

            CreateMap<RealizationVbDetailModel, VbNonPORequestDetailViewModel>()
                
            .ReverseMap();
        }
    }
}
