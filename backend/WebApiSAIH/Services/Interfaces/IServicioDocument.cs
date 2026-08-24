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
        RespuestaGenerica guardarDocument(FormFile files);
        RespuestaGenerica obtenerDocuments();
        RespuestaGenerica obtenerDocumentXemployee(string cedula);
        RespuestaGenerica obtenerDocumentXemployeeBytes(string cedula);
        Task<RespuestaGenerica> descargarDocument(long id);
        RespuestaGenerica eliminarDocument(long id);
        RespuestaGenerica obtenerImagenEmployee(string nationalId);
        RespuestaGenerica obtenerNombrePkDocument(long pk_idDocument);
    }
}
