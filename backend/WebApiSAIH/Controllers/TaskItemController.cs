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
        public IActionResult getTasks()
        {
            return Ok(_taskService.getTasks());
        }

        [HttpGet("taskNA")]
        public IActionResult getNATaskId()
        {
            RespuestaGenerica respuestaGenerica = _taskService.getNATaskId();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("{pk_idTaskItem}")]
        public IActionResult getTask(long pk_idTaskItem)
        {
            RespuestaGenerica respuestaGenerica = _taskService.getTask(pk_idTaskItem);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        public IActionResult createTask(TaskItemDTO taskDTO)
        {
            RespuestaGenerica respuestaGenerica = _taskService.createTask(taskDTO);

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
        public IActionResult updateTask(long pK_idTaskItem, TaskItemDTO taskDTO)
        {
            RespuestaGenerica respuestaGenerica = _taskService.updateTask(pK_idTaskItem, taskDTO);

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
        public IActionResult deleteTask(long pK_idTaskItem)
        {
            RespuestaGenerica respuestaGenerica = _taskService.deleteTask(pK_idTaskItem);

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

        [HttpGet("toggle-status/{pK_idTaskItem}")]
        public IActionResult toggleTaskStatus(long pK_idTaskItem)
        {
            RespuestaGenerica respuestaGenerica = _taskService.toggleTaskStatus(pK_idTaskItem);

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

        [HttpGet("projectsByTask/{code}")]
        public IActionResult projectsByTask(string code)
        {
            RespuestaGenerica respuestaGenerica = _taskService.projectsByTask(code);

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

        [HttpGet("checkTask/{code}/{fk_idProject}")]
        public IActionResult checkTask(string code, long fk_idProject)
        {
            RespuestaGenerica respuestaGenerica = _taskService.checkTask(code, fk_idProject);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("tasksByProject/{fk_idProject}")]
        public IActionResult tasksByProject(long fk_idProject)
        {
            RespuestaGenerica respuestaGenerica = _taskService.tasksByProject(fk_idProject);

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

        [HttpGet("update-progress/{pk_idTaskItem}/{progress}")]
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