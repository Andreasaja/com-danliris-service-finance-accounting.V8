using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.LockTransaction;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.LockTransaction
{
    public class LockTransactionProfile : Profile
    {
        public LockTransactionProfile()
        {
            CreateMap<LockTransactionModel, LockTransactionViewModel>()
                .ReverseMap();
        }
    }
}
