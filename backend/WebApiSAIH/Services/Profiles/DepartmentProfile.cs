using AutoMapper;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDTO>()
               .ForMember(dest =>
                   dest.PK_idDepartment,
                   opt => opt.MapFrom(src => src.PK_idDepartment))
               .ForMember(dest =>
                   dest.CodigoDepartment,
                   opt => opt.MapFrom(src => src.CodigoDepartment))
               .ForMember(dest =>
                   dest.NombreDepartment,
                   opt => opt.MapFrom(src => src.NombreDepartment))
               .ForMember(dest =>
                   dest.DescripcionDepartment,
                   opt => opt.MapFrom(src => src.DescripcionDepartment))
               .ForMember(dest =>
                   dest.EstadoDepartment,
                   opt => opt.MapFrom(src => src.EstadoDepartment))
            .ReverseMap();
        }

    }
}