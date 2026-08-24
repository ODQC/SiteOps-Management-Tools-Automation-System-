using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Servicios.Clases_estaticas;
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
    public class ProjectTaskController : ControllerBase
    {
        private readonly IServicioProjectTask _servicioProjectTask;

        public ProjectTaskController(IServicioProjectTask servicioProjectTask)
        {

            _servicioProjectTask = servicioProjectTask;

        }

        [HttpPost]
        public IActionResult guardarProjectTask(ProjectTaskDTO projectTaskDTO)
        {
            RespuestaGenerica respuestaGenerica = _servicioProjectTask.guardarProjectTask(projectTaskDTO);

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

        [HttpDelete("{pK_idProjectTask}")]
        public IActionResult eliminarProjectTask(long pK_idProjectTask)
        {
            RespuestaGenerica respuestaGenerica = _servicioProjectTask.eliminarProjectTask(pK_idProjectTask);

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

        [HttpPost("guardarLista")]
        public IActionResult guardarProjectTaskList(List<ProjectTaskDTO> listProjectTaskDTO)
        {
            RespuestaGenerica respuestaGenerica = _servicioProjectTask.guardarProjectTaskList(listProjectTaskDTO);

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

        [HttpGet("verificarRelacionExistente/{fk_idProject}/{fk_idTask}")]
        public IActionResult verificarRelacionExistente(long fk_idProject, long fk_idTask)
        {
            RespuestaGenerica respuestaGenerica = _servicioProjectTask.verificarRelacionExistente(fk_idProject, fk_idTask);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return StatusCode(200, respuestaGenerica);
        }

        [HttpGet]
        public IActionResult obtenerProjectTask()
        {
            RespuestaGenerica respuestaGenerica = _servicioProjectTask.obtenerProjectTask();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return StatusCode(200, respuestaGenerica);
        }
    }
}
