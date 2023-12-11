using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.RealizationVBWithPO
{
    public class RealizationVBWithPOProfile : Profile
    {
        public RealizationVBWithPOProfile()
        {
            CreateMap<RealizationVbModel, RealizationVbWithPOViewModel>()
                .ForPath(d => d.numberVB.VBNo, opt => opt.MapFrom(s => s.VBNo))
                .ForPath(d => d.numberVB.DateEstimate, opt => opt.MapFrom(s => s.DateEstimate))
                .ForPath(d => d.numberVB.CreateBy, opt => opt.MapFrom(s => s.RequestVbName))
                .ForPath(d => d.numberVB.UnitCode, opt => opt.MapFrom(s => s.UnitCode))
                .ForPath(d => d.numberVB.UnitName, opt => opt.MapFrom(s => s.UnitName))
                .ForPath(d => d.numberVB.Amount, opt => opt.MapFrom(s => s.Amount_VB))
            .ReverseMap();
        }
    }
}
