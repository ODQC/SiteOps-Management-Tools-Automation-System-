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
        RespuestaGenerica createResource(ResourceDTO resourceDTO);

        RespuestaGenerica getResource(string idResourceDTO);

        RespuestaGenerica getResources();

        RespuestaGenerica toggleResourceStatus(string idResourceDTO);

        RespuestaGenerica deleteResource(string idResourceDTO);

        RespuestaGenerica updateResource(long pK_idResource, ResourceDTO resourceDTO);

        RespuestaGenerica getNAResourceId();

        RespuestaGenerica goalsByResource(String code);

        RespuestaGenerica checkResource(String code);
    }
}
