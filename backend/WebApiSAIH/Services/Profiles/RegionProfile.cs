using AutoMapper;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Profiles
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Region, RegionDTO>()
                .ForMember(dest =>
                    dest.PK_IdRegion,
                    opt => opt.MapFrom(src => src.PK_IdRegion))
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
                .ReverseMap();
        }
    }
}
