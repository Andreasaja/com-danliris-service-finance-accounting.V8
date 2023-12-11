using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MasterCOA;

namespace Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.Master
{
    public class COAProfile : Profile
    {
        public COAProfile()
        {
            CreateMap<COAModel, COAViewModel>().ReverseMap();
        }
    }
}
