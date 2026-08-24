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
        RespuestaGenerica createResourceGoal(ResourceGoalDTO resourcegoalDTO);

        RespuestaGenerica createResourceGoalList(List<ResourceGoalDTO> resourcegoalDTO);

        RespuestaGenerica deleteResourceGoal(long idResourceGoalDTO);

        RespuestaGenerica checkExistingLink(long fk_idResource2, long fk_idGoal2);

        RespuestaGenerica getResourceGoals();
    }
}

