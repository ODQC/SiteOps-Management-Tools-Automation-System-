using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Profiles
{
    public class GoalProfile: Profile
    {
        public GoalProfile()
        {
            CreateMap<Goal, GoalDTO>()
                .ForMember(dest =>
                    dest.PK_idGoal,
                    opt => opt.MapFrom(src => src.PK_idGoal))
                .ForMember(dest =>
                        dest.Name,
                        opt => opt.MapFrom(src => src.Name))
                .ForMember(dest =>
                        dest.Code,
                        opt => opt.MapFrom(src => src.Code))
                .ForMember(dest =>
                        dest.Description,
                        opt => opt.MapFrom(src => src.Description))
                .ForMember(dest =>
                        dest.Year,
                        opt => opt.MapFrom(src => src.Year))
                .ForMember(dest =>
                        dest.Status,
                        opt => opt.MapFrom(src => src.Status))
                .ReverseMap();
        }
    }
}
