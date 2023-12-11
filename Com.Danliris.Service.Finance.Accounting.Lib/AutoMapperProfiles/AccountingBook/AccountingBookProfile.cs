using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.AccountingBook;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.AccountingBook;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.AccountingBook
{
    public class AccountingBookProfile : Profile
    {
        public AccountingBookProfile()
        {
            CreateMap<AccountingBookModel, AccountingBookViewModel>()
                .ForPath(d => d.AccountingBookType, opt => opt.MapFrom(s => s.AccountingBookType))
                .ForPath(d => d.Code, opt => opt.MapFrom(s => s.Code))
                .ForPath(d => d.Remarks, opt => opt.MapFrom(s => s.Remarks))
                .ReverseMap();                


        }
    }
}
