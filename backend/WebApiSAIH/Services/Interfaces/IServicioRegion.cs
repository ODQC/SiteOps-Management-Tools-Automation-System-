using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Interfaces
{
    public interface IServicioRegion
    {
        RespuestaGenerica getRegions();
        RespuestaGenerica getNARegionId();
        RespuestaGenerica getRegion(string code);
        RespuestaGenerica createRegion(RegionDTO regionDTO);
        RespuestaGenerica updateRegion(int pkIdRegion, RegionDTO regionDTO);
        RespuestaGenerica deleteRegion(string code);
        RespuestaGenerica toggleRegionStatus(string code);
        RespuestaGenerica checkRegion(string code);
        RespuestaGenerica getRegionBySite(long pk_idSite);
    }
}
