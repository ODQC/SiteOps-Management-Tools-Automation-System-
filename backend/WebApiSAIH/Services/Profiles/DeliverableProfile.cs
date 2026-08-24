using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Profiles
{
    public class DeliverableProfile : Profile
    {
        public DeliverableProfile()
        {
            CreateMap<Deliverable, DeliverableDTO>()
               .ForMember(dest =>
                   dest.PK_idDeliverable,
                   opt => opt.MapFrom(src => src.PK_idDeliverable))
               .ForMember(dest =>
                   dest.Code,
                   opt => opt.MapFrom(src => src.Code))
               .ForMember(dest =>
                   dest.FK_idDocument,
                   opt => opt.MapFrom(src => src.FK_idDocument))
               .ForMember(dest =>
                   dest.Status,
                   opt => opt.MapFrom(src => src.Status))
               .ForMember(dest =>
                   dest.Name,
                   opt => opt.MapFrom(src => src.Name))
               .ForMember(dest =>
                   dest.Description,
                   opt => opt.MapFrom(src => src.Description))
               .ForMember(dest =>
                   dest.FK_idTask1,
                   opt => opt.MapFrom(src => src.FK_idTask1))
               .ForMember(dest =>
                   dest.DocumentName,
                   opt => opt.MapFrom(src => src.DocumentName))
            .ReverseMap();
        }
    }
}
