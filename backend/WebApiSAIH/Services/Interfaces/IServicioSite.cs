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
        RespuestaGenerica getSitesByRegion(int idRegion);
        RespuestaGenerica getNASiteId();
        RespuestaGenerica getAllSites();
        RespuestaGenerica createSite(SiteDTO siteDTO);
        RespuestaGenerica getSite(string code);
        RespuestaGenerica toggleSiteStatus(string code);
        RespuestaGenerica deleteSite(string code);
        RespuestaGenerica updateSite(long pk_IdSite, SiteDTO siteDTO);
        RespuestaGenerica checkSite(string code);
        RespuestaGenerica checkSiteManager(string code);
    }
}