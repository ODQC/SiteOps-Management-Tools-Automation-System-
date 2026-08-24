using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.Errores;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Models;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.DTO;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Services.Implementacion
{
    public class ServicioGoal : IServicioGoal
    {
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioGoal(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica toggleGoalStatus(string idGoalDTO)
        {
            Goal goal = _context.Goals.Where(
                s => s.Code == idGoalDTO).FirstOrDefault<Goal>();

            if (goal == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Goal not found", null);
            }

            if (goal.Status == Status.ACTIVE)
            {
                goal.Status = Status.INACTIVE;
            }
            else if (goal.Status == Status.INACTIVE)
            {
                goal.Status = Status.ACTIVE;
            }

            try
            {
                _context.Update(goal);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The goal's status has been changed", _mapper.Map<GoalDTO>(goal));
        }

        public RespuestaGenerica deleteGoal(string idGoalDTO)
        {
            Goal goal = _context.Goals.Where(
                s => s.Code == idGoalDTO).FirstOrDefault<Goal>();

            if (goal == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Goal not found", null);
            }

            try
            {
                _context.Goals.Remove(goal);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Goal " + goal.Code + " deleted successfully", _mapper.Map<GoalDTO>(goal));
        }

        public RespuestaGenerica createGoal(GoalDTO goalDTO)
        {
            try
            {
                Goal goal = _context.Goals.Where(
                 s => s.Code == goalDTO.Code).FirstOrDefault<Goal>();

                if (goal != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This goal is already registered in the system", null);
                }

                Goal goal2 = _mapper.Map<Goal>(goalDTO);

                if (validarCampos(goal2) != null)
                {
                    return validarCampos(goal2);
                }

                _context.Goals.Add(goal2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Goal registered", _mapper.Map<GoalDTO>(goal2));
            }
            catch (DbUpdateException e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        public RespuestaGenerica updateGoal(long pK_idGoal, GoalDTO goalDTO)
        {
            try
            {
                Goal goal = _context.Goals.Where(
                 s => s.PK_idGoal == pK_idGoal).FirstOrDefault<Goal>();

                if (goal == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Goal not found", "No goal was updated");
                }

                Goal goalCodigo = _context.Goals.Where(
                 s => s.Code == goalDTO.Code).FirstOrDefault<Goal>();

                if (goalCodigo != null && (goalCodigo != goal))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This goal code is already in use", "");
                }

                goal = covertirDTOAEntidad(goal, goalDTO);
                _context.Update(goal);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Goal updated", _mapper.Map<GoalDTO>(goal));
            }
            catch (Exception e)
            {
                if (!GoalExists(pK_idGoal))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Goal not found", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
                }
            }
        }

        private bool GoalExists(long pK_idGoal)
        {
            return _context.Goals.Any(e => e.PK_idGoal == pK_idGoal);
        }

        public RespuestaGenerica getGoal(string idGoalDTO)
        {
            Goal goal = _context.Goals
               .Where(s => s.Code == idGoalDTO).FirstOrDefault<Goal>();

            if (goal == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Goal not found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Goal", _mapper.Map<GoalDTO>(goal));
        }

        public RespuestaGenerica getGoals()
        {
            List<Goal> goals = _context.Goals.Where(s => s.Code != "N/A").ToList();

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Goals listed", _mapper.Map<List<GoalDTO>>(goals));
        }

        public RespuestaGenerica getNAGoalId()
        {
            Goal goal = _context.Goals
                .Where(s => s.Code == "N/A").FirstOrDefault<Goal>();

            if (goal == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No N/A goal found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Goal PK for N/A", goal.PK_idGoal);
        }

        private Goal covertirDTOAEntidad(Goal goal, GoalDTO goalDTO)
        {
            goal.Code = goalDTO.Code;
            goal.Name = goalDTO.Name;
            goal.Description = goalDTO.Description;
            goal.Status = goalDTO.Status;
            return goal;
        }

        private RespuestaGenerica validarCampos(Goal goal)
        {
            List<String> listaErrores = new List<String>();

            Goal goal2 = _context.Goals.Where(
                 s => s.Code == goal.Code).FirstOrDefault<Goal>();

            if (goal2 != null)
            {
                listaErrores.Add(Error.GOAL_ALREADY_REGISTERED);
            }

            if (listaErrores.Count() == 0)
            {
                return null;
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Validation error", listaErrores);
        }

        public RespuestaGenerica resourcesByGoal(string code)
        {
            try
            {
                Goal goal = _context.Goals.Where(
                 s => s.Code == code).FirstOrDefault<Goal>();

                if (goal == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Goal not found", null);
                }

                List<ResourceGoal> listResourceGoal = _context.ResourceGoals.Where(
                    s => s.Fk_idGoal2 == goal.PK_idGoal).ToList();

                List<Resource> resources = new List<Resource>();

                for (int i = 0; i < listResourceGoal.Count; i++)
                {
                    resources.Add(_context.Resources.Where(
                        s => s.PK_idResource == listResourceGoal[i].Fk_idResource2).FirstOrDefault<Resource>());
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Resources associated with the goal " + code,
                    _mapper.Map<List<ResourceDTO>>(resources));
            }
            catch(Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        public RespuestaGenerica checkGoal(string code)
        {
            try
            {
                Goal goal = _context.Goals.Where(
                s => s.Code == code).FirstOrDefault<Goal>();

                if (goal != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Goal en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Goal disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }
    }
}
