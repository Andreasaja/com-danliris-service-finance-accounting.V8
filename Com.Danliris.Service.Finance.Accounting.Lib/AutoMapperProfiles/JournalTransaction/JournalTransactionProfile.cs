using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MasterCOA;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.JournalTransaction
{
    public class JournalTransactionProfile : Profile
    {
        public JournalTransactionProfile()
        {
            CreateMap<JournalTransactionModel, JournalTransactionViewModel>()
                .ReverseMap();

            CreateMap<JournalTransactionItemModel, JournalTransactionItemViewModel>()
                .ForPath(d => d.COA.Id, opt => opt.MapFrom(s => s.COAId))
                .ForPath(d => d.COA.Code, opt => opt.MapFrom(s => s.COA.Code))
                .ForPath(d => d.COA.Name, opt => opt.MapFrom(s => s.COA.Name))
                .ReverseMap();
        }
    }
}
