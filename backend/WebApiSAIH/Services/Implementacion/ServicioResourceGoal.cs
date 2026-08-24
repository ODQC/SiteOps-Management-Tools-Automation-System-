using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAIH_Backend.Servicios.Clases_estaticas;
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
    public class ServicioResourceGoal : IServicioResourceGoal
    {
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioResourceGoal(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica deleteResourceGoal(long idResourceGoalDTO)
        {
            ResourceGoal resourceGoal = _context.ResourceGoals.Where(
                s => s.PK_idGoalResource == idResourceGoalDTO).FirstOrDefault<ResourceGoal>();

            if (resourceGoal == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Resource/goal link not found", null);
            }

            try
            {
                _context.ResourceGoals.Remove(resourceGoal);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Resource goal" + resourceGoal.PK_idGoalResource + " deleted successfully", _mapper.Map<ResourceGoalDTO>(resourceGoal));
        }

        public RespuestaGenerica createResourceGoal(ResourceGoalDTO resourceGoalDTO)
        {
            try
            {
                ResourceGoal resourceGoal2 = _mapper.Map<ResourceGoal>(resourceGoalDTO);

                _context.ResourceGoals.Add(resourceGoal2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Resource/goal link registered", _mapper.Map<ResourceGoalDTO>(resourceGoal2));
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

        public RespuestaGenerica createResourceGoalList(List<ResourceGoalDTO> listResourcegoalDTO)
        {
            try
            {
                List<ResourceGoal> listResourceGoal2 = _mapper.Map<List<ResourceGoal>>(listResourcegoalDTO);
                List<ResourceGoal> listResourceGoal1 = new List<ResourceGoal>();

                for (int i = 0; i < listResourceGoal2.Count; i++)
                {
                    listResourceGoal1.Add(_context.ResourceGoals.Add(listResourceGoal2[i]).Entity);
                }
                
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Resource/goal links registered", _mapper.Map<List<ResourceGoalDTO>>(listResourceGoal1));
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

        public RespuestaGenerica checkExistingLink(long fk_idResource2, long fk_idGoal2)
        {
            try
            {
                ResourceGoal resourceGoal = _context.ResourceGoals.Where(
                    s => s.Fk_idResource2 == fk_idResource2 && s.Fk_idGoal2 == fk_idGoal2).FirstOrDefault<ResourceGoal>();

                if (resourceGoal != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Resource/goal link already exists", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Resource/goal link does not exist", false);
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

        public RespuestaGenerica getResourceGoals()
        {
            List<ResourceGoal> resourceGoals = null;
            try
            {
                resourceGoals = _context.ResourceGoals.ToList();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Server error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Resource/goal links listed", _mapper.Map<List<ResourceGoalDTO>>(resourceGoals));
        }
    }
}
