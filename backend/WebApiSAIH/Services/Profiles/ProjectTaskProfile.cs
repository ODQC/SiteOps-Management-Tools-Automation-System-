using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Profiles
{
    public class ProjectTaskProfile : Profile
    {
        public ProjectTaskProfile()
        {
            CreateMap<ProjectTask, ProjectTaskDTO>()
               .ForMember(dest =>
                   dest.PK_idProjectTask,
                   opt => opt.MapFrom(src => src.PK_idProjectTask))
               .ForMember(dest =>
                   dest.FK_idTask,
                   opt => opt.MapFrom(src => src.FK_idTask))
               .ForMember(dest =>
                   dest.FK_idProject,
                   opt => opt.MapFrom(src => src.FK_idProject))
            .ReverseMap();
        }
    }
}
