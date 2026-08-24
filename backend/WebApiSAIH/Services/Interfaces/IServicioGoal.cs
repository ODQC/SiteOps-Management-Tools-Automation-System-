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
        RespuestaGenerica createGoal(GoalDTO goalDTO);

        RespuestaGenerica getGoal(string idGoalDTO);

        RespuestaGenerica getGoals();

        RespuestaGenerica toggleGoalStatus(string idGoalDTO);

        RespuestaGenerica deleteGoal(string idGoalDTO);

        RespuestaGenerica updateGoal(long pK_idGoal, GoalDTO goalDTO);

        RespuestaGenerica getNAGoalId();

        RespuestaGenerica resourcesByGoal(String code);

        RespuestaGenerica checkGoal(String code);
    }
}
