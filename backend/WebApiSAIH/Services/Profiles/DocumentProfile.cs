using AutoMapper;
using BackendAPI3._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Profiles
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<Document, DocumentDTO>()

                .ForMember(dest =>
                   dest.Name,
                   opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest =>
                   dest.CreatedOn,
                   opt => opt.MapFrom(src => src.CreatedOn))
                 .ForMember(dest =>
                   dest.DataFiles,
                   opt => opt.MapFrom(src => src.DataFiles))
                 .ForMember(dest =>
                   dest.FilePath,
                   opt => opt.MapFrom(src => src.FilePath))
                 .ForMember(dest =>
                   dest.FileType,
                   opt => opt.MapFrom(src => src.FileType))
            .ReverseMap();
        }
    }
}
