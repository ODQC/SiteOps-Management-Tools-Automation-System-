using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDTO>()
               .ForMember(dest =>
                   dest.PK_idProject,
                   opt => opt.MapFrom(src => src.PK_idProject))
               .ForMember(dest =>
                   dest.Code,
                   opt => opt.MapFrom(src => src.Code))
               .ForMember(dest =>
                   dest.Status,
                   opt => opt.MapFrom(src => src.Status))
               .ForMember(dest =>
                   dest.EndDate,
                   opt => opt.MapFrom(src => src.EndDate))
               .ForMember(dest =>
                   dest.FK_idRegion2,
                   opt => opt.MapFrom(src => src.FK_idRegion2))
               .ForMember(dest =>
                   dest.Fk_IdEmployee1,
                   opt => opt.MapFrom(src => src.Fk_IdEmployee1))
               .ForMember(dest =>
                   dest.StartDate,
                   opt => opt.MapFrom(src => src.StartDate))
               .ForMember(dest =>
                   dest.Progress,
                   opt => opt.MapFrom(src => src.Progress))
            .ReverseMap();
        }

    }
}