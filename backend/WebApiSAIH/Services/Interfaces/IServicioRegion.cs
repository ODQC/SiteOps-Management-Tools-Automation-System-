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
        RespuestaGenerica obtenerAreasConservacion();
        RespuestaGenerica obtenerPKRegionNA();
        RespuestaGenerica obtenerRegion(string code);
        RespuestaGenerica guardarRegion(RegionDTO regionDTO);
        RespuestaGenerica actualizarRegion(int pkIdRegion, RegionDTO regionDTO);
        RespuestaGenerica eliminarRegion(string code);
        RespuestaGenerica deshabilitarRegion(string code);
        RespuestaGenerica verificarArea(string code);
        RespuestaGenerica obtenerAreaXparque(long pk_idParque);
    }
}
