using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Interfaces
{
    public interface IServicioResourceGoal
    {
        RespuestaGenerica guardarResourceGoal(ResourceGoalDTO resourcegoalDTO);

        RespuestaGenerica guardarResourceGoalList(List<ResourceGoalDTO> resourcegoalDTO);

        RespuestaGenerica eliminarResourceGoal(long idResourceGoalDTO);

        RespuestaGenerica verificarRelacionExistente(long fk_idResource2, long fk_idGoal2);

        RespuestaGenerica obtenerResourcesGoal();
    }
}

