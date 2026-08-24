using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Profiles
{
    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            CreateMap<TaskItem, TaskItemDTO>()
               .ForMember(dest =>
                   dest.PK_idTaskItem,
                   opt => opt.MapFrom(src => src.PK_idTaskItem))
               .ForMember(dest =>
                   dest.Code,
                   opt => opt.MapFrom(src => src.Code))
               .ForMember(dest =>
                   dest.TaskStatus,
                   opt => opt.MapFrom(src => src.TaskStatus))
               .ForMember(dest =>
                   dest.Collaborators,
                   opt => opt.MapFrom(src => src.Collaborators))
               .ForMember(dest =>
                   dest.Status,
                   opt => opt.MapFrom(src => src.Status))
               .ForMember(dest =>
                   dest.CompletionDate,
                   opt => opt.MapFrom(src => src.CompletionDate))
               .ForMember(dest =>
                   dest.Fk_IdGoal1,
                   opt => opt.MapFrom(src => src.Fk_IdGoal1))
               .ForMember(dest =>
                   dest.Name,
                   opt => opt.MapFrom(src => src.Name))
               .ForMember(dest =>
                   dest.Notes,
                   opt => opt.MapFrom(src => src.Notes))
               .ForMember(dest =>
                   dest.FK_idProject,
                   opt => opt.MapFrom(src => src.FK_idProject))
            .ReverseMap();
        }

    }
}