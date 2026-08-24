using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class ServicioResource : IServicioResource
    {
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioResource(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica deshabilitarResource(string idResourceDTO)
        {
            Resource resource = _context.Resources.Where(
                s => s.Code == idResourceDTO).FirstOrDefault<Resource>();

            if (resource == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Resource no encontrada", null);
            }

            if (resource.Status == Status.ACTIVE)
            {
                resource.Status = Status.INACTIVE;
            }
            else if (resource.Status == Status.INACTIVE)
            {
                resource.Status = Status.ACTIVE;
            }

            try
            {
                _context.Update(resource);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El estado de la resource ha sido modificada", _mapper.Map<ResourceDTO>(resource));
        }

        public RespuestaGenerica eliminarResource(string idResourceDTO)
        {
            Resource resource = _context.Resources.Where(
                s => s.Code == idResourceDTO).FirstOrDefault<Resource>();

            if (resource == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Resource no encontrada", null);
            }

            try
            {
                _context.Resources.Remove(resource);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Resource " + resource.Code + " eliminado exitosamente", _mapper.Map<ResourceDTO>(resource));
        }

        public RespuestaGenerica guardarResource(ResourceDTO resourceDTO)
        {
            try
            {
                Resource resource = _context.Resources.Where(
                 s => s.Code == resourceDTO.Code).FirstOrDefault<Resource>();

                if (resource != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "La resource ya esta registrada en el sistema", null);
                }

                Resource resource2 = _mapper.Map<Resource>(resourceDTO);

                if (validarCampos(resource2) != null)
                {
                    return validarCampos(resource2);
                }

                _context.Resources.Add(resource2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Resource registrada", _mapper.Map<ResourceDTO>(resource2));
            }
            catch (DbUpdateException e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        public RespuestaGenerica modificarResource(long pK_idResource, ResourceDTO resourceDTO)
        {
            try
            {
                Resource resource = _context.Resources.Where(
                 s => s.PK_idResource == pK_idResource).FirstOrDefault<Resource>();

                if (resource == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Resource no encontrada", "No se actualizo ninguna resource");
                }

                Resource resourceCodigo = _context.Resources.Where(
                 s => s.Code == resourceDTO.Code).FirstOrDefault<Resource>();

                if (resourceCodigo != null && (resourceCodigo != resource))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El codigo de la resource ya está siendo utilizado", "");
                }

                resource = covertirDTOAEntidad(resource, resourceDTO);
                _context.Update(resource);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Resource actualizado", _mapper.Map<ResourceDTO>(resource));
            }
            catch (Exception e)
            {
                if (!ResourceExists(pK_idResource))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Resource no encontrada", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
                }
            }
        }

        private bool ResourceExists(long pK_idResource)
        {
            return _context.Resources.Any(e => e.PK_idResource == pK_idResource);
        }

        public RespuestaGenerica obtenerResource(string idResourceDTO)
        {
            Resource resource = _context.Resources
               .Where(s => s.Code == idResourceDTO).FirstOrDefault<Resource>();

            if (resource == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro la resource", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Resource", _mapper.Map<ResourceDTO>(resource));
        }

        public RespuestaGenerica obtenerResources()
        {
            List<Resource> resources = _context.Resources.Where(s => s.Code != "N/A").ToList();

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Resources desplegados", _mapper.Map<List<ResourceDTO>>(resources));
        }

        public RespuestaGenerica obtenerPKResourceNA()
        {
            Resource resource = _context.Resources
                .Where(s => s.Code == "N/A").FirstOrDefault<Resource>();

            if (resource == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro resource con N/A", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "PK resource con N/A", resource.PK_idResource);
        }

        private Resource covertirDTOAEntidad(Resource resource, ResourceDTO resourceDTO)
        {
            resource.Code = resourceDTO.Code;
            resource.Type = resourceDTO.Type;
            resource.Description = resource.Description;
            resource.Status = resourceDTO.Status;
            return resource;
        }

        private RespuestaGenerica validarCampos(Resource resource)
        {
            List<String> listaErrores = new List<String>();

            Resource resource2 = _context.Resources.Where(
                 s => s.Code == resource.Code).FirstOrDefault<Resource>();

            if (resource2 != null)
            {
                listaErrores.Add(Error.RESOURCE_ALREADY_REGISTERED);
            }

            if (listaErrores.Count() == 0)
            {
                return null;
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Error de validaciones", listaErrores);
        }

        public RespuestaGenerica goalsXresource(string code)
        {
            try
            {
                Resource resource = _context.Resources.Where(
                 s => s.Code == code).FirstOrDefault<Resource>();

                if (resource == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro la resource", null);
                }

                List<ResourceGoal> listResourceGoal = _context.ResourceGoals.Where(
                    s => s.Fk_idResource2 == resource.PK_idResource).ToList();

                List<Goal> goals = new List<Goal>();

                for (int i = 0; i < listResourceGoal.Count; i++)
                {
                    goals.Add(_context.Goals.Where(
                        s => s.PK_idGoal == listResourceGoal[i].Fk_idGoal2).FirstOrDefault<Goal>());
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Goals asociados a la resource " + code,
                    _mapper.Map<List<GoalDTO>>(goals));
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        public RespuestaGenerica verificarResource(string code)
        {
            try
            {
                Resource resource = _context.Resources.Where(
                s => s.Code == code).FirstOrDefault<Resource>();

                if (resource != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Resource en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Resource disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }
    }
}
