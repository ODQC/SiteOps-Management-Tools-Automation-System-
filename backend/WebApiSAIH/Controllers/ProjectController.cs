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
        public IActionResult getProjects()
        {
            return Ok(_projectService.getProjects());
        }

        [HttpGet("projectNA")]
        public IActionResult getNAProjectId()
        {
            RespuestaGenerica respuestaGenerica = _projectService.getNAProjectId();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("{pk_idProject}")]
        public IActionResult getProject(long pk_idProject)
        {
            RespuestaGenerica respuestaGenerica = _projectService.getProject(pk_idProject);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ROLE_SITE_MANAGER + "," + Roles.ROLE_EMPLOYEE + "," +
        Roles.ROLE_ADMIN)]
        public IActionResult createProject(ProjectDTO projectDTO)
        {
            RespuestaGenerica respuestaGenerica = _projectService.createProject(projectDTO);

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
        public IActionResult updateProject(long pk_IdProject, ProjectDTO projectDTO)
        {
            RespuestaGenerica respuestaGenerica = _projectService.updateProject(pk_IdProject, projectDTO);

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
        public IActionResult deleteProject(long pk_idProject)
        {
            RespuestaGenerica respuestaGenerica = _projectService.deleteProject(pk_idProject);

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

        [HttpGet("toggle-status/{pk_idPlan}/{pk_idEmployee}")]
        [Authorize(Roles = Roles.ROLE_SITE_MANAGER + "," + Roles.ROLE_ADMIN)]
        public IActionResult toggleProjectStatus(long pk_idPlan, long pk_idEmployee)
        {
            RespuestaGenerica respuestaGenerica = _projectService.toggleProjectStatus(pk_idPlan, pk_idEmployee);

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

        [HttpGet("tasksByProject/{pk_idProject}")]
        public IActionResult resourcesByGoal(long pk_idProject)
        {
            RespuestaGenerica respuestaGenerica = _projectService.tasksByProject(pk_idProject);

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

        [HttpGet("checkProject/{code}/{fk_idEmployee}")]
        public IActionResult checkProject(string code, long fk_idEmployee)
        {
            RespuestaGenerica respuestaGenerica = _projectService.checkProject(code, fk_idEmployee);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("projectByEmployee/{fk_idEmployee}")]
        public IActionResult projectByEmployee(long fk_idEmployee)
        {
            RespuestaGenerica respuestaGenerica = _projectService.projectByEmployee(fk_idEmployee);

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

        [HttpGet("update-progress/{pk_idProject}/{progress}")]
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