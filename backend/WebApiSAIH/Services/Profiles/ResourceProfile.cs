using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Profiles
{
    public class ResourceProfile: Profile
    {
        public ResourceProfile()
        {
            CreateMap<Resource, ResourceDTO>()
                .ForMember(dest =>
                    dest.PK_idResource,
                    opt => opt.MapFrom(src => src.PK_idResource))
                .ForMember(dest =>
                        dest.Type,
                        opt => opt.MapFrom(src => src.Type))
                .ForMember(dest =>
                        dest.Description,
                        opt => opt.MapFrom(src => src.Description))
                .ForMember(dest =>
                        dest.Status,
                        opt => opt.MapFrom(src => src.Status))
                .ForMember(dest =>
                        dest.Code,
                        opt => opt.MapFrom(src => src.Code))
                .ReverseMap();
        }
    }
}
