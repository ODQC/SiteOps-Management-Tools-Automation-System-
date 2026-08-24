using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.DTO;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IServicioProject _projectService;

        public ProjectController(IServicioProject projectService)
        {

            _projectService = projectService;

        }

        [HttpGet]
        public IActionResult obtenerProjects()
        {
            return Ok(_projectService.obtenerProjects());
        }

        [HttpGet("projectNA")]
        public IActionResult obtenerPKPlanTrabjoNA()
        {
            RespuestaGenerica respuestaGenerica = _projectService.obtenerPKProjectNA();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("{pk_idProject}")]
        public IActionResult obtenerProject(long pk_idProject)
        {
            RespuestaGenerica respuestaGenerica = _projectService.obtenerProject(pk_idProject);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ROLE_SITE_MANAGER + "," + Roles.ROLE_EMPLOYEE + "," +
        Roles.ROLE_ADMIN)]
        public IActionResult guardarProject(ProjectDTO projectDTO)
        {
            RespuestaGenerica respuestaGenerica = _projectService.guardarProject(projectDTO);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return StatusCode(200, respuestaGenerica);
            }

            return StatusCode(201, respuestaGenerica);
        }

        [HttpPut("{pk_IdProject}")]
        [Authorize(Roles = Roles.ROLE_SITE_MANAGER + "," + Roles.ROLE_EMPLOYEE + "," +
            Roles.ROLE_ADMIN)]
        public IActionResult modificarProject(long pk_IdProject, ProjectDTO projectDTO)
        {
            RespuestaGenerica respuestaGenerica = _projectService.modificarProject(pk_IdProject, projectDTO);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_BAD_REQUEST)
            {
                return BadRequest(respuestaGenerica);
            }

            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpDelete("{pk_idProject}")]
        [Authorize(Roles =  Roles.ROLE_SITE_MANAGER + "," + Roles.ROLE_ADMIN)]
        public IActionResult eliminarProject(long pk_idProject)
        {
            RespuestaGenerica respuestaGenerica = _projectService.eliminarProject(pk_idProject);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }
            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("cambiarEstado/{pk_idPlan}/{pk_idEmployee}")]
        [Authorize(Roles = Roles.ROLE_SITE_MANAGER + "," + Roles.ROLE_ADMIN)]
        public IActionResult deshabilitarProject(long pk_idPlan, long pk_idEmployee)
        {
            RespuestaGenerica respuestaGenerica = _projectService.deshabilitarProject(pk_idPlan, pk_idEmployee);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_BAD_REQUEST)
            {
                return BadRequest(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("taskesXproject/{pk_idProject}")]
        public IActionResult resourcesXgoal(long pk_idProject)
        {
            RespuestaGenerica respuestaGenerica = _projectService.taskesXproject(pk_idProject);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("verificarProject/{codigoProject}/{fk_idEmployee}")]
        public IActionResult verificarProject(string codigoProject, long fk_idEmployee)
        {
            RespuestaGenerica respuestaGenerica = _projectService.verificarProject(codigoProject, fk_idEmployee);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("projectXemployee/{fk_idEmployee}")]
        public IActionResult projectXemployee(long fk_idEmployee)
        {
            RespuestaGenerica respuestaGenerica = _projectService.projectXemployee(fk_idEmployee);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("cambiarProgress/{pk_idProject}/{progress}")]
        public IActionResult cambiarProgesoPlan(long pk_idProject, string progress)
        {
            RespuestaGenerica respuestaGenerica = _projectService.cambiarProgesoPlan(pk_idProject, progress);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_BAD_REQUEST)
            {
                return BadRequest(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }
    }
}