using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Interfaces
{
    public interface IServicioSite
    {
        RespuestaGenerica obtenerParquesNacionales(int idRegion);
        RespuestaGenerica obtenerPKParqueNA();
        RespuestaGenerica obtenerTodosParquesNacionales();
        RespuestaGenerica guardarSite(SiteDTO siteDTO);
        RespuestaGenerica obtenerSite(string code);
        RespuestaGenerica deshabilitarSite(string code);
        RespuestaGenerica eliminarSite(string code);
        RespuestaGenerica modificarSite(long pk_IdSite, SiteDTO siteDTO);
        RespuestaGenerica verificarParque(string code);
        RespuestaGenerica verificarAdminParque(string code);
    }
}