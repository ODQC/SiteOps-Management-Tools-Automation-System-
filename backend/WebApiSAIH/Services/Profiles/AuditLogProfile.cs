using AutoMapper;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Profiles
{
    public class AuditLogProfile : Profile
    {
        public AuditLogProfile() {

            CreateMap<AuditLog, AuditLogDTO>()
              
                .ForMember(dest =>
                   dest.Description,
                   opt => opt.MapFrom(src => src.Description))
                 .ForMember(dest =>
                   dest.Date,
                   opt => opt.MapFrom(src => src.Date))
                 .ForMember(dest =>
                   dest.Fk_IdEmployee2,
                   opt => opt.MapFrom(src => src.Fk_IdEmployee2))
                 .ForMember(dest =>
                   dest.PK_idAuditLog,
                   opt => opt.MapFrom(src => src.PK_idAuditLog))

            .ReverseMap();
        }
       
    }
}
