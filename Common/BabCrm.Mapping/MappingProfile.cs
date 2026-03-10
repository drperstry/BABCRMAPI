using AutoMapper;
using BabCrm.Service.Models;
using Bab.BatchData.Models;

namespace BabCrm.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FinancialAccountDetails, FinancialAccountModel>().IncludeMembers(s => s.FinancialAccount);
            CreateMap<FinancialAccount, FinancialAccountModel>(MemberList.None);
            CreateMap<TicketRequestModel, TicketRequest>();
            CreateMap<SamaLookupModel, BaseLookupModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SamaValues.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.SamaValues.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SamaValues.Name));
                
        }
    }
}