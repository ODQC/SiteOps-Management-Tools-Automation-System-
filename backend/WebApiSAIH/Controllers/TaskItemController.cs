using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize(Roles = Roles.ROLE_SUPERVISOR + "," + Roles.ROLE_SITE_MANAGER + "," + Roles.ROLE_EMPLOYEE + "," +
        Roles.ROLE_ADMIN)]
    public class TaskItemController : ControllerBase
    {
        private readonly IServicioTaskItem _taskService;

        public TaskItemController(IServicioTaskItem taskService)
        {

            _taskService = taskService;

        }

        [HttpGet]
        public IActionResult obtenerTaskes()
        {
            return Ok(_taskService.obtenerTaskes());
        }

        [HttpGet("taskNA")]
        public IActionResult obtenerPKTaskNA()
        {
            RespuestaGenerica respuestaGenerica = _taskService.obtenerPKTaskNA();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("{pk_idTaskItem}")]
        public IActionResult obtenerTask(long pk_idTaskItem)
        {
            RespuestaGenerica respuestaGenerica = _taskService.obtenerTask(pk_idTaskItem);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        public IActionResult guardarTask(TaskItemDTO taskDTO)
        {
            RespuestaGenerica respuestaGenerica = _taskService.guardarTask(taskDTO);

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

        [HttpPut("{pK_idTaskItem}")]
        public IActionResult modificarTask(long pK_idTaskItem, TaskItemDTO taskDTO)
        {
            RespuestaGenerica respuestaGenerica = _taskService.modificarTask(pK_idTaskItem, taskDTO);

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

        [HttpDelete("{pK_idTaskItem}")]
        public IActionResult eliminarTask(long pK_idTaskItem)
        {
            RespuestaGenerica respuestaGenerica = _taskService.eliminarTask(pK_idTaskItem);

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

        [HttpGet("cambiarEstado/{pK_idTaskItem}")]
        public IActionResult deshabilitarTask(long pK_idTaskItem)
        {
            RespuestaGenerica respuestaGenerica = _taskService.deshabilitarTask(pK_idTaskItem);

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

        [HttpGet("planesTrabajoXtask/{code}")]
        public IActionResult planesTrabajoXtask(string code)
        {
            RespuestaGenerica respuestaGenerica = _taskService.planesTrabajoXtask(code);

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

        [HttpGet("verificarTask/{code}/{fk_idProject}")]
        public IActionResult verificarTask(string code, long fk_idProject)
        {
            RespuestaGenerica respuestaGenerica = _taskService.verificarTask(code, fk_idProject);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("taskesXproject/{fk_idProject}")]
        public IActionResult taskesXproject(long fk_idProject)
        {
            RespuestaGenerica respuestaGenerica = _taskService.taskesXproject(fk_idProject);

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

        [HttpGet("cambiarProgress/{pk_idTaskItem}/{progress}")]
        public IActionResult cambiarProgress(long pk_idTaskItem, string progress)
        {
            RespuestaGenerica respuestaGenerica = _taskService.cambiarTaskStatus(pk_idTaskItem, progress);

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