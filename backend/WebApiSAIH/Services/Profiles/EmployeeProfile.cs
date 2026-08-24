using AutoMapper;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest =>
                    dest.NationalId,
                    opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest =>
                        dest.FirstName,
                        opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest =>
                        dest.LastName,
                        opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest =>
                        dest.SecondLastName,
                        opt => opt.MapFrom(src => src.SecondLastName))
                .ForMember(dest =>
                        dest.Phone,
                        opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest =>
                        dest.Email,
                        opt => opt.MapFrom(src => src.Email))
                .ForMember(dest =>
                        dest.FK_idDepartment1,
                        opt => opt.MapFrom(src => src.FK_idDepartment1))
                .ForMember(dest =>
                        dest.FK_idSite1,
                        opt => opt.MapFrom(src => src.FK_idSite1))
                .ForMember(dest =>
                        dest.FK_idRole1,
                        opt => opt.MapFrom(src => src.FK_idRole1))
                .ForMember(dest =>
                        dest.FK_idDocument1,
                        opt => opt.MapFrom(src => src.FK_idDocument1))
                .ReverseMap();
        }
    }
}
