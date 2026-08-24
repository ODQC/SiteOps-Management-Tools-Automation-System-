using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.VO;
using WebApiSAIH.Services.DTO;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliverableController : ControllerBase
    {
        IServicioDeliverable _servicioDeliverable;
        public DeliverableController(IServicioDeliverable servicioDeliverable)
        {
            _servicioDeliverable = servicioDeliverable;
        }

        [HttpGet]
        public IActionResult getDeliverables()
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.getDeliverables();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }
            return Ok(respuestaGenerica);
        }

        [HttpGet("{pk_IdDeliverable}")]
        public IActionResult getDeliverable(long pk_IdDeliverable)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.getDeliverable(pk_IdDeliverable);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        public IActionResult createDeliverable(DeliverableDTO deliverableDTO)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.createDeliverable(deliverableDTO);

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

        [HttpPut("{pk_IdDeliverable}")]
        public IActionResult updateDeliverable(long pk_IdDeliverable, DeliverableDTO deliverableDTO)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.updateDeliverable(pk_IdDeliverable, deliverableDTO);

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

        [HttpDelete("{pk_IdDeliverable}")]
        public IActionResult deleteDeliverable(long pk_IdDeliverable)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.deleteDeliverable(pk_IdDeliverable);

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

        [HttpGet("toggle-status/{pk_IdDeliverable}")]
        public IActionResult toggleDepartmentStatus(long pk_IdDeliverable)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.toggleDeliverableStatus(pk_IdDeliverable);

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

        [HttpGet("deliverablesByTask/{fk_idTask}")]
        public IActionResult deliverablesByTask(long fk_idTask)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.deliverablesByTask(fk_idTask);

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

        [HttpGet("checkDeliverable/{code}/{fk_idTask}")]
        public IActionResult checkDeliverable(string code, long fk_idTask)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.checkDeliverable(code, fk_idTask);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }
    }
}
