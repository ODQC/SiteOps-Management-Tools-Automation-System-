using Microsoft.AspNetCore.Http;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.Interfaces
{
    public interface IServicioDocument
    {
        RespuestaGenerica createDocument(FormFile files);
        RespuestaGenerica getDocuments();
        RespuestaGenerica getDocumentByEmployee(string nationalId);
        RespuestaGenerica getDocumentBytesByEmployee(string nationalId);
        Task<RespuestaGenerica> descargarDocument(long id);
        RespuestaGenerica deleteDocument(long id);
        RespuestaGenerica getEmployeeImage(string nationalId);
        RespuestaGenerica getDocumentNameByPk(long pk_idDocument);
    }
}
