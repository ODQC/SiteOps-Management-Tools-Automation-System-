using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Interfaces
{
    public interface IServicioResource
    {
        RespuestaGenerica guardarResource(ResourceDTO resourceDTO);

        RespuestaGenerica obtenerResource(string idResourceDTO);

        RespuestaGenerica obtenerResources();

        RespuestaGenerica deshabilitarResource(string idResourceDTO);

        RespuestaGenerica eliminarResource(string idResourceDTO);

        RespuestaGenerica modificarResource(long pK_idResource, ResourceDTO resourceDTO);

        RespuestaGenerica obtenerPKResourceNA();

        RespuestaGenerica goalsXresource(String code);

        RespuestaGenerica verificarResource(String code);
    }
}
