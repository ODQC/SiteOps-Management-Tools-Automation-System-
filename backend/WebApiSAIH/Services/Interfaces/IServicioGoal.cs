using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Interfaces
{
    public interface IServicioGoal
    {
        RespuestaGenerica guardarGoal(GoalDTO goalDTO);

        RespuestaGenerica obtenerGoal(string idGoalDTO);

        RespuestaGenerica obtenerGoals();

        RespuestaGenerica deshabilitarGoal(string idGoalDTO);

        RespuestaGenerica eliminarGoal(string idGoalDTO);

        RespuestaGenerica modificarGoal(long pK_idGoal, GoalDTO goalDTO);

        RespuestaGenerica obtenerPKGoalNA();

        RespuestaGenerica resourcesXgoal(String code);

        RespuestaGenerica verificarGoal(String code);
    }
}
