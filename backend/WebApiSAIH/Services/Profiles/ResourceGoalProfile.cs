using AutoMapper;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Profiles
{
    public class ResourceGoalProfile : Profile
    {
        public ResourceGoalProfile()
        {
            CreateMap<ResourceGoal, ResourceGoalDTO>()
               .ForMember(dest =>
                   dest.PK_idGoalResource,
                   opt => opt.MapFrom(src => src.PK_idGoalResource))
               .ForMember(dest =>
                   dest.FK_idResource2,
                   opt => opt.MapFrom(src => src.Fk_idResource2))
               .ForMember(dest =>
                   dest.FK_idGoal2,
                   opt => opt.MapFrom(src => src.Fk_idGoal2))
            .ReverseMap();
        }
    }
}
