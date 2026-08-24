using AutoMapper;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Profiles
{
    public class SiteProfile : Profile
    {
        public SiteProfile()
        {
            CreateMap<Site, SiteDTO>()
                .ForMember(dest =>
                    dest.PK_IdSite,
                    opt => opt.MapFrom(src => src.PK_IdSite))
                .ForMember(dest =>
                    dest.Code,
                    opt => opt.MapFrom(src => src.Code))
                .ForMember(dest =>
                    dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest =>
                    dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(dest =>
                    dest.Status,
                    opt => opt.MapFrom(src => src.Status))
                 .ForMember(dest =>
                    dest.FK_idRegion1,
                    opt => opt.MapFrom(src => src.FK_idRegion1))
                .ReverseMap();
        }
    }
}